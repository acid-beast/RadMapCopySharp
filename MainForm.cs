using System.Reflection;
using System.Text.RegularExpressions;
using RadMapCopySharp.Core;
using RadMapCopySharp.Core.Rendering;
using RadMapCopySharp.Core.Operations;
using RadMapCopySharp.Core.Validation;
using RadMapCopySharp.Dialogs;

namespace RadMapCopySharp;

public partial class MainForm : Form
{
    private readonly SettingsStore _settingsStore;
    private readonly MapCopyOperation _mapCopyOperation;
    private readonly StaticsCopyOperation _staticsCopyOperation;
    private readonly CreateEmptyMapOperation _createEmptyMapOperation;
    private readonly ExtendToMLOperation _extendToMLOperation;
    private readonly string _definitionsPath;

    private MapProfile? _sourceProfile;
    private MapProfile? _destinationProfile;
    private string _sourceProfileReason = string.Empty;
    private string _destinationProfileReason = string.Empty;
    private string _radarColPath = string.Empty;
    private string _regionsXmlPath = string.Empty;
    private Font _statusNormalFont;
    private Font _statusErrorFont;

    public MainForm()
    {
        InitializeComponent();

        _mapCopyOperation = new MapCopyOperation();
        _staticsCopyOperation = new StaticsCopyOperation();
        _createEmptyMapOperation = new CreateEmptyMapOperation();
        _extendToMLOperation = new ExtendToMLOperation();
        _settingsStore = new SettingsStore(Path.Combine(AppContext.BaseDirectory, "radmapcopy.ini"));
        _definitionsPath = Path.Combine(AppContext.BaseDirectory, "definitions.ini");
        _statusNormalFont = lblStatus.Font;
        _statusErrorFont = new Font(lblStatus.Font, FontStyle.Bold);
        
        Text = $"RadMapCopySharp 1.0";

        InitializeCopyUi();
        LoadInitialSettings();
        HookValidationEvents();
        ValidateAllInputs();

        FormClosing += MainForm_FormClosing;
    }

    private void InitializeCopyUi()
    {
        numZ1.Minimum = sbyte.MinValue;
        numZ1.Maximum = sbyte.MaxValue;
        numZ2.Minimum = sbyte.MinValue;
        numZ2.Maximum = sbyte.MaxValue;

        // Coordinate inputs are 4-digit fields by design.
        foreach (var box in new[] { txtSX1, txtSY1, txtSX2, txtSY2, txtDX, txtDY })
        {
            box.MaxLength = 4;
        }

        rbKeepZ.CheckedChanged += (_, _) =>
        {
            UpdateAltitudeInputState();
            ValidateAllInputs();
        };

        rbRandomZ.CheckedChanged += (_, _) =>
        {
            UpdateAltitudeInputState();
            ValidateAllInputs();
        };

        rbAddRandomZ.CheckedChanged += (_, _) =>
        {
            UpdateAltitudeInputState();
            ValidateAllInputs();
        };

        chkCopyMap.CheckedChanged += (_, _) =>
        {
            UpdateCopyModeVisibility();
            ValidateAllInputs();
        };

        chkCopyStatics.CheckedChanged += (_, _) =>
        {
            UpdateCopyModeVisibility();
            ValidateAllInputs();
        };

        var presets = SettingsStore.LoadSkipTilePresets(_definitionsPath).ToList();
        if (presets.Count == 0)
        {
            presets.Add(new SkipTilePreset("None", new HashSet<ushort>()));
        }

        cmbSkipPreset.Items.Clear();
        cmbSkipPreset.Items.AddRange(presets.Cast<object>().ToArray());
        cmbSkipPreset.SelectedIndex = 0;

        UpdateAltitudeInputState();
        UpdateCopyModeVisibility();
    }

    private void LoadInitialSettings()
    {
        var settings = _settingsStore.Load();
        txtSrcMap.Text = settings.SourceMapPath;
        txtDstMap.Text = settings.DestinationMapPath;
        txtSrcStaidx.Text = settings.SourceStaidxPath;
        txtSrcStatics.Text = settings.SourceStaticsPath;
        txtDstStaidx.Text = settings.DestinationStaidxPath;
        txtDstStatics.Text = settings.DestinationStaticsPath;
        _radarColPath = ResolveRadarColPath(settings.RadarColPath, settings.SourceMapPath, settings.DestinationMapPath);
        _regionsXmlPath = ResolveRegionsXmlPath(settings.RegionsXmlPath, settings.SourceMapPath, settings.DestinationMapPath, _radarColPath);
    }

    private void HookValidationEvents()
    {
        txtSX1.TextChanged += (_, _) => ValidateAllInputs();
        txtSY1.TextChanged += (_, _) => ValidateAllInputs();
        txtSX2.TextChanged += (_, _) => ValidateAllInputs();
        txtSY2.TextChanged += (_, _) => ValidateAllInputs();
        txtDX.TextChanged += (_, _) => ValidateAllInputs();
        txtDY.TextChanged += (_, _) => ValidateAllInputs();

        txtSrcStaidx.TextChanged += (_, _) => ValidateAllInputs();
        txtSrcStatics.TextChanged += (_, _) => ValidateAllInputs();
        txtDstStaidx.TextChanged += (_, _) => ValidateAllInputs();
        txtDstStatics.TextChanged += (_, _) => ValidateAllInputs();

        numZ1.ValueChanged += (_, _) => ValidateAllInputs();
        numZ2.ValueChanged += (_, _) => ValidateAllInputs();
    }

    private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
    {
        _settingsStore.Save(new AppSettings
        {
            SourceMapPath = txtSrcMap.Text,
            DestinationMapPath = txtDstMap.Text,
            RadarColPath = _radarColPath,
            RegionsXmlPath = _regionsXmlPath,
            SourceStaidxPath = txtSrcStaidx.Text,
            SourceStaticsPath = txtSrcStatics.Text,
            DestinationStaidxPath = txtDstStaidx.Text,
            DestinationStaticsPath = txtDstStatics.Text
        });
    }

    private void ValidateAllInputs()
    {
        ValidatePathAndProfile(txtSrcMap, lblSourceProfile, isSource: true);
        ValidatePathAndProfile(txtDstMap, lblDestinationProfile, isSource: false);

        ValidateAuxPath(txtSrcStaidx, mustExist: true);
        ValidateAuxPath(txtSrcStatics, mustExist: true);
        ValidateAuxPath(txtDstStaidx, mustExist: false);
        ValidateAuxPath(txtDstStatics, mustExist: false);

        var coordsAligned = ValidateCoordinateFields();

        if (!TryBuildMapCopyRequest(out _, out var error))
        {
            SetStatus(error, isError: true);
        }
        else
        {
            SetStatus("Ready", isError: false);
        }

        UpdatePreviewButtonState();
        UpdateCopyButtonState();
    }

    private void SetActionsBusy(bool busy)
    {
        if (busy)
        {
            btnPreview.Enabled = false;
            btnCopy.Enabled = false;
            btnCreateEmptyMap.Enabled = false;
            btnExtendToMl.Enabled = false;
            return;
        }

        btnCreateEmptyMap.Enabled = true;
        btnExtendToMl.Enabled = true;
        UpdatePreviewButtonState();
        UpdateCopyButtonState();
    }

    private void UpdateCopyButtonState()
    {
        btnCopy.Enabled = CanStartCopy(out var reason);
        if (!btnCopy.Enabled && !string.IsNullOrWhiteSpace(reason))
        {
            SetStatus(reason, isError: true);
        }
    }

    private void SetStatus(string message, bool isError)
    {
        lblStatus.Text = message;
        lblStatus.ForeColor = isError ? Color.DarkRed : SystemColors.ControlText;
        lblStatus.Font = isError ? _statusErrorFont : _statusNormalFont;
    }

    private bool CanStartCopy(out string reason)
    {
        reason = string.Empty;

        if (!chkCopyMap.Checked && !chkCopyStatics.Checked)
        {
            reason = "Select at least one copy target: map and/or statics.";
            return false;
        }

        if (!TryBuildMapCopyRequest(out _, out reason))
        {
            return false;
        }

        return true;
    }

    private static void ValidateAuxPath(TextBox textBox, bool mustExist)
    {
        if (string.IsNullOrWhiteSpace(textBox.Text))
        {
            textBox.ForeColor = Color.DarkRed;
            return;
        }

        var exists = File.Exists(textBox.Text);
        textBox.ForeColor = (!mustExist || exists) ? Color.DarkGreen : Color.DarkRed;
    }

    private void ValidatePathAndProfile(TextBox textBox, Label profileLabel, bool isSource)
    {
        var path = textBox.Text;
        var validPath = CoordinateValidator.IsPathValid(path);
        textBox.ForeColor = validPath ? Color.DarkGreen : Color.DarkRed;

        if (!validPath)
        {
            profileLabel.Text = "Profile: invalid path";
            if (isSource)
            {
                _sourceProfile = null;
                _sourceProfileReason = "invalid path";
            }
            else
            {
                _destinationProfile = null;
                _destinationProfileReason = "invalid path";
            }

            return;
        }

        if (MapProfileDetector.TryDetect(path, out var profile, out var reason) && profile != null)
        {
            profileLabel.Text = $"Profile: {profile.Name} ({profile.Width}x{profile.Height})";
            if (isSource)
            {
                _sourceProfile = profile;
                _sourceProfileReason = string.Empty;
            }
            else
            {
                _destinationProfile = profile;
                _destinationProfileReason = string.Empty;
            }
        }
        else
        {
            var detail = string.IsNullOrWhiteSpace(reason) ? "unknown" : $"unknown ({reason})";
            profileLabel.Text = $"Profile: {detail}";

            if (isSource)
            {
                _sourceProfile = null;
                _sourceProfileReason = reason;
            }
            else
            {
                _destinationProfile = null;
                _destinationProfileReason = reason;
            }
        }
    }

    private bool ValidateCoordinateFields()
    {
        var boxes = new[]
        {
            txtSX1, txtSY1, txtSX2, txtSY2, txtDX, txtDY
        };

        var parsed = new Dictionary<TextBox, int>();
        foreach (var box in boxes)
        {
            if (TryParseCoordinate(box.Text, out var value))
            {
                parsed[box] = value;
                box.ForeColor = Color.DarkGreen;
            }
            else
            {
                box.ForeColor = Color.DarkRed;
            }
        }

        if (chkCopyStatics.Checked && parsed.Count == boxes.Length)
        {
            var sourceAligned = CoordinateValidator.AreBlockAligned(
                parsed[txtSX1],
                parsed[txtSY1],
                parsed[txtSX2],
                parsed[txtSY2]);

            if (!sourceAligned)
            {
                var sourceBoxes = new[] { txtSX1, txtSY1, txtSX2, txtSY2 };
                foreach (var box in sourceBoxes)
                {
                    if ((parsed[box] % 8) != 0)
                    {
                        box.ForeColor = Color.DarkRed;
                    }
                }
            }

            return sourceAligned;
        }

        return parsed.Count == boxes.Length;
    }

    private static bool TryParseCoordinate(string text, out int value)
    {
        value = 0;

        var trimmed = text.Trim();
        if (trimmed.Length == 0 || trimmed.Length > 4)
        {
            return false;
        }

        return int.TryParse(trimmed, out value);
    }

    private bool TryBuildMapCopyRequest(out MapCopyRequest request)
    {
        return TryBuildMapCopyRequest(out request, out _);
    }

    private bool TryBuildMapCopyRequest(out MapCopyRequest request, out string error)
    {
        request = null!;
        error = "Invalid input.";

        if (_sourceProfile == null || _destinationProfile == null)
        {
            var sourceDetail = _sourceProfile == null
                ? (string.IsNullOrWhiteSpace(_sourceProfileReason) ? "unknown" : _sourceProfileReason)
                : "ok";

            var destinationDetail = _destinationProfile == null
                ? (string.IsNullOrWhiteSpace(_destinationProfileReason) ? "unknown" : _destinationProfileReason)
                : "ok";

            error = $"Could not detect source/destination profiles. Source: {sourceDetail}. Destination: {destinationDetail}.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(txtSrcMap.Text) || !File.Exists(txtSrcMap.Text))
        {
            error = "Source map path is required and must exist.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(txtDstMap.Text) || !File.Exists(txtDstMap.Text))
        {
            error = "Destination map path is required and must exist.";
            return false;
        }

        if (!TryParseCoordinate(txtSX1.Text, out var sx1) ||
            !TryParseCoordinate(txtSY1.Text, out var sy1) ||
            !TryParseCoordinate(txtSX2.Text, out var sx2) ||
            !TryParseCoordinate(txtSY2.Text, out var sy2) ||
            !TryParseCoordinate(txtDX.Text, out var dx) ||
            !TryParseCoordinate(txtDY.Text, out var dy))
        {
            error = "One or more coordinate fields are invalid (numeric, max 4 digits).";
            return false;
        }

        var z1 = (int)numZ1.Value;
        var z2 = (int)numZ2.Value;

        if (!CoordinateValidator.ValidateRect(sx1, sy1, sx2, sy2, _sourceProfile.Width, _sourceProfile.Height, out var sourceError))
        {
            error = sourceError ?? "Source coordinate validation failed.";
            return false;
        }

        var width = sx2 - sx1 + 1;
        var height = sy2 - sy1 + 1;

        if (!CoordinateValidator.ValidateDestination(dx, dy, width, height, _destinationProfile.Width, _destinationProfile.Height, out var destinationError))
        {
            error = destinationError ?? "Destination coordinate validation failed.";
            return false;
        }

        if (chkCopyStatics.Checked)
        {
            if (string.IsNullOrWhiteSpace(txtSrcStaidx.Text) || !File.Exists(txtSrcStaidx.Text) ||
                string.IsNullOrWhiteSpace(txtSrcStatics.Text) || !File.Exists(txtSrcStatics.Text))
            {
                error = "Source staidx/statics paths are required and must exist for statics copy.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDstStaidx.Text) || string.IsNullOrWhiteSpace(txtDstStatics.Text))
            {
                error = "Destination staidx/statics paths are required for statics copy.";
                return false;
            }

            if (!CoordinateValidator.ValidateBlockAligned(sx1, sy1, sx2, sy2, out var alignedError))
            {
                error = alignedError ?? "Statics copy coordinates must be block aligned.";
                return false;
            }
        }

        var mode = rbRandomZ.Checked
            ? AltitudeMode.FixedRandom
            : rbAddRandomZ.Checked
                ? AltitudeMode.AddRandomOffset
                : AltitudeMode.Unchanged;

        if (mode != AltitudeMode.Unchanged && numZ1.Value > numZ2.Value)
        {
            error = "For random altitude modes, Z1 must be less than or equal to Z2.";
            return false;
        }

        var selectedPreset = cmbSkipPreset.SelectedItem as SkipTilePreset;
        var skipIds = selectedPreset?.TileIds;

        request = new MapCopyRequest
        {
            SourcePath = txtSrcMap.Text,
            DestinationPath = txtDstMap.Text,
            SourceX1 = sx1,
            SourceY1 = sy1,
            SourceX2 = sx2,
            SourceY2 = sy2,
            DestinationX = dx,
            DestinationY = dy,
            AltitudeMode = mode,
            Z1 = z1,
            Z2 = z2,
            SkipTileIds = skipIds
        };

        return true;
    }

    private async void btnCopy_Click(object sender, EventArgs e)
    {
        try
        {
            if (!TryBuildMapCopyRequest(out var request))
            {
                return;
            }

            if (!chkCopyMap.Checked && !chkCopyStatics.Checked)
            {
                MessageBox.Show("Select at least one copy target: map and/or statics.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var destinationIsUop = Path.GetExtension(request.DestinationPath).Equals(".uop", StringComparison.OrdinalIgnoreCase);
            if (destinationIsUop)
            {
                var warning = "Destination is a UOP map. Back up the file before continuing.";
                if (chkCopyStatics.Checked)
                {
                    warning += "\nStatics remain MUL files and are copied separately.";
                }

                var confirm = MessageBox.Show(warning, "UOP Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (confirm != DialogResult.OK)
                {
                    return;
                }
            }

            SetStatus("Starting copy...", isError: false);
            progressBar.Value = 0;
            SetActionsBusy(true);

            if (chkCopyMap.Checked)
            {
                await Task.Run(() => _mapCopyOperation.Execute(request, OnProgress));
            }

            if (chkCopyStatics.Checked)
            {
                if (!File.Exists(txtSrcStaidx.Text) || !File.Exists(txtSrcStatics.Text))
                {
                    throw new InvalidOperationException("Source staidx/statics files are required for statics copy.");
                }

                var staticsRequest = new StaticsCopyRequest
                {
                    SourceStaidxPath = txtSrcStaidx.Text,
                    SourceStaticsPath = txtSrcStatics.Text,
                    DestinationStaidxPath = txtDstStaidx.Text,
                    DestinationStaticsPath = txtDstStatics.Text,
                    SourceProfile = _sourceProfile!,
                    DestinationProfile = _destinationProfile!,
                    SourceX1 = request.SourceX1,
                    SourceY1 = request.SourceY1,
                    SourceX2 = request.SourceX2,
                    SourceY2 = request.SourceY2,
                    DestinationX = request.DestinationX,
                    DestinationY = request.DestinationY,
                    AltitudeMode = request.AltitudeMode,
                    Z1 = request.Z1,
                    Z2 = request.Z2
                };

                await Task.Run(() => _staticsCopyOperation.Execute(staticsRequest, OnProgress));
            }

            MessageBox.Show("Copy completed!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            SetStatus(ex.Message, isError: true);
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetActionsBusy(false);
        }
    }

    private void OnProgress(int percent, string message)
    {
        Invoke(() =>
        {
            progressBar.Value = Math.Clamp(percent, 0, 100);
            SetStatus(message, isError: false);
        });
    }

    private void UpdateAltitudeInputState()
    {
        var enabled = !rbKeepZ.Checked;
        numZ1.Enabled = enabled;
        numZ2.Enabled = enabled;
    }

    private void UpdateCopyModeVisibility()
    {
        var bothMapAndStatics = chkCopyMap.Checked && chkCopyStatics.Checked;
        rbAddRandomZ.Visible = !bothMapAndStatics;

        if (bothMapAndStatics && rbAddRandomZ.Checked)
        {
            rbRandomZ.Checked = true;
        }
    }

    private void txtSrcMap_TextChanged(object sender, EventArgs e)
    {
        ValidateAllInputs();
        AutofillCompanionPaths(txtSrcMap.Text, txtSrcStaidx, txtSrcStatics);
    }

    private void txtDstMap_TextChanged(object sender, EventArgs e)
    {
        ValidateAllInputs();
        AutofillCompanionPaths(txtDstMap.Text, txtDstStaidx, txtDstStatics);
    }

    private static void AutofillCompanionPaths(string mapPath, TextBox staidxTarget, TextBox staticsTarget)
    {
        if (string.IsNullOrWhiteSpace(mapPath))
        {
            return;
        }

        var directory = Path.GetDirectoryName(mapPath);
        if (string.IsNullOrWhiteSpace(directory))
        {
            return;
        }

        var fileName = Path.GetFileNameWithoutExtension(mapPath);
        var match = Regex.Match(fileName, @"map(?<index>\d+)", RegexOptions.IgnoreCase);
        if (!match.Success)
        {
            return;
        }

        var index = match.Groups["index"].Value;

        if (string.IsNullOrWhiteSpace(staidxTarget.Text))
        {
            staidxTarget.Text = Path.Combine(directory, $"staidx{index}.mul");
        }

        if (string.IsNullOrWhiteSpace(staticsTarget.Text))
        {
            staticsTarget.Text = Path.Combine(directory, $"statics{index}.mul");
        }
    }

    private void btnBrowseSrcMap_Click(object sender, EventArgs e)
    {
        openFileDialog.Filter = "Map files (*.mul;*.uop)|*.mul;*.uop|All files (*.*)|*.*";
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            txtSrcMap.Text = openFileDialog.FileName;
        }
    }

    private void btnBrowseDstMap_Click(object sender, EventArgs e)
    {
        openFileDialog.Filter = "Map files (*.mul;*.uop)|*.mul;*.uop|All files (*.*)|*.*";
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            txtDstMap.Text = openFileDialog.FileName;
        }
    }

    private void btnBrowseSrcStaidx_Click(object sender, EventArgs e)
    {
        BrowseMulPath(txtSrcStaidx, mustExist: true);
    }

    private void btnBrowseSrcStatics_Click(object sender, EventArgs e)
    {
        BrowseMulPath(txtSrcStatics, mustExist: true);
    }

    private void btnBrowseDstStaidx_Click(object sender, EventArgs e)
    {
        BrowseMulPath(txtDstStaidx, mustExist: false);
    }

    private void btnBrowseDstStatics_Click(object sender, EventArgs e)
    {
        BrowseMulPath(txtDstStatics, mustExist: false);
    }

    private void BrowseMulPath(TextBox target, bool mustExist)
    {
        if (mustExist)
        {
            openFileDialog.Filter = "MUL files (*.mul)|*.mul|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                target.Text = openFileDialog.FileName;
            }
        }
        else
        {
            saveFileDialog.Filter = "MUL files (*.mul)|*.mul|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                target.Text = saveFileDialog.FileName;
            }
        }
    }

    private void menuFileExit_Click(object sender, EventArgs e)
    {
        Close();
    }

    private async void btnCreateEmptyMap_Click(object sender, EventArgs e)
    {
        using var form = new CreateMapForm();
        if (form.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        try
        {
            SetActionsBusy(true);

            var request = new CreateEmptyMapRequest
            {
                MapPath = form.MapPath,
                Profile = form.SelectedProfile!,
                TileId = form.TileId,
                Z = form.Z,
                CreateEmptyStatics = form.CreateStatics,
                StaidxPath = form.CreateStatics ? form.StaidxPath : null,
                StaticsPath = form.CreateStatics ? form.StaticsPath : null
            };

            var result = await Task.Run(() => _createEmptyMapOperation.Execute(request, OnProgress));
            var message = $"New map created: {result.ResolvedMapPath}";
            if (!string.Equals(result.RequestedMapPath, result.ResolvedMapPath, StringComparison.OrdinalIgnoreCase))
            {
                message += $"\n(original target existed: {result.RequestedMapPath})";
            }

            if (result.ResolvedStaidxPath != null && result.ResolvedStaticsPath != null)
            {
                message += $"\nStaidx: {result.ResolvedStaidxPath}";
                message += $"\nStatics: {result.ResolvedStaticsPath}";
            }

            MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetActionsBusy(false);
        }
    }

    private async void btnExtendToMl_Click(object sender, EventArgs e)
    {
        using var form = new ExtendMapForm();
        if (form.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        try
        {
            SetActionsBusy(true);

            var request = new ExtendToMLRequest
            {
                SourceMapPath = form.SourcePath,
                DestinationMapPath = form.DestinationPath,
                FillTileId = 0,
                FillZ = 0
            };

            var result = await Task.Run(() => _extendToMLOperation.Execute(request, OnProgress));
            var message = $"Map extended to ML size: {result.ResolvedDestinationPath}";
            if (!string.Equals(result.RequestedDestinationPath, result.ResolvedDestinationPath, StringComparison.OrdinalIgnoreCase))
            {
                message += $"\n(original target existed: {result.RequestedDestinationPath})";
            }

            MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetActionsBusy(false);
        }
    }

    private void menuHelpHelp_Click(object sender, EventArgs e)
    {
        using var form = new HelpForm();
        form.ShowDialog(this);
    }

    private void menuHelpAbout_Click(object sender, EventArgs e)
    {
        using var form = new AboutForm();
        form.ShowDialog(this);
    }

    private void btnPreview_Click(object sender, EventArgs e)
    {
        if (!TryBuildPreviewState(out var state, out var error))
        {
            MessageBox.Show(error, "Map Preview", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var form = new MapPreviewForm(
            state,
            GetCurrentPreviewState,
            path => _radarColPath = path,
            path => _regionsXmlPath = path);
        form.ShowDialog(this);
    }

    private MapPreviewState GetCurrentPreviewState()
    {
        if (TryBuildPreviewState(out var state, out _))
        {
            return state;
        }

        if (_sourceProfile != null && _destinationProfile != null)
        {
            return BuildPreviewWithoutOverlay("Current inputs are invalid; keeping last valid map paths.");
        }

        throw new InvalidOperationException("Preview requires valid source and destination profiles.");
    }

    private bool TryBuildPreviewState(out MapPreviewState state, out string error)
    {
        state = null!;
        error = string.Empty;

        if (_sourceProfile == null || _destinationProfile == null)
        {
            error = "Preview requires valid source and destination map paths with detectable profiles.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(txtSrcMap.Text) || !File.Exists(txtSrcMap.Text) ||
            string.IsNullOrWhiteSpace(txtDstMap.Text) || !File.Exists(txtDstMap.Text))
        {
            error = "Preview requires existing source and destination map files.";
            return false;
        }

        if (!TryParseCoordinate(txtSX1.Text, out var sx1) ||
            !TryParseCoordinate(txtSY1.Text, out var sy1) ||
            !TryParseCoordinate(txtSX2.Text, out var sx2) ||
            !TryParseCoordinate(txtSY2.Text, out var sy2) ||
            !TryParseCoordinate(txtDX.Text, out var dx) ||
            !TryParseCoordinate(txtDY.Text, out var dy))
        {
            state = BuildPreviewWithoutOverlay("Coordinates are incomplete or invalid; showing map origin viewport.");
            return true;
        }

        if (!CoordinateValidator.ValidateRect(sx1, sy1, sx2, sy2, _sourceProfile.Width, _sourceProfile.Height, out var sourceError))
        {
            state = BuildPreviewWithoutOverlay(sourceError ?? "Source rectangle is invalid; showing map origin viewport.");
            return true;
        }

        var width = sx2 - sx1 + 1;
        var height = sy2 - sy1 + 1;
        if (!CoordinateValidator.ValidateDestination(dx, dy, width, height, _destinationProfile.Width, _destinationProfile.Height, out var destinationError))
        {
            state = BuildPreviewWithoutOverlay(destinationError ?? "Destination anchor is invalid; showing map origin viewport.");
            return true;
        }

        state = new MapPreviewState
        {
            SourceMapPath = txtSrcMap.Text,
            DestinationMapPath = txtDstMap.Text,
            SourceProfile = _sourceProfile,
            DestinationProfile = _destinationProfile,
            RadarColPath = ResolveRadarColPath(_radarColPath, txtSrcMap.Text, txtDstMap.Text),
            RegionsXmlPath = ResolveRegionsXmlPath(_regionsXmlPath, txtSrcMap.Text, txtDstMap.Text, _radarColPath),
            SourceRect = new CopyRectangle(sx1, sy1, sx2, sy2),
            DestinationAnchor = new Point(dx, dy)
        };

        _radarColPath = state.RadarColPath;
        _regionsXmlPath = state.RegionsXmlPath;
        return true;
    }

    private MapPreviewState BuildPreviewWithoutOverlay(string message = "")
    {
        var sourcePath = txtSrcMap.Text;
        var destinationPath = txtDstMap.Text;

        return new MapPreviewState
        {
            SourceMapPath = sourcePath,
            DestinationMapPath = destinationPath,
            SourceProfile = _sourceProfile!,
            DestinationProfile = _destinationProfile!,
            RadarColPath = ResolveRadarColPath(_radarColPath, sourcePath, destinationPath),
            RegionsXmlPath = ResolveRegionsXmlPath(_regionsXmlPath, sourcePath, destinationPath, _radarColPath),
            OverlayMessage = message
        };
    }

    private static string ResolveRadarColPath(string configuredPath, string sourceMapPath, string destinationMapPath)
    {
        if (!string.IsNullOrWhiteSpace(configuredPath) && File.Exists(configuredPath))
        {
            return configuredPath;
        }

        var sourceCandidate = BuildRadarCandidate(sourceMapPath);
        if (sourceCandidate != null)
        {
            return sourceCandidate;
        }

        var destinationCandidate = BuildRadarCandidate(destinationMapPath);
        if (destinationCandidate != null)
        {
            return destinationCandidate;
        }

        return configuredPath;
    }

    private static string? BuildRadarCandidate(string mapPath)
    {
        if (string.IsNullOrWhiteSpace(mapPath))
        {
            return null;
        }

        var directory = Path.GetDirectoryName(mapPath);
        if (string.IsNullOrWhiteSpace(directory))
        {
            return null;
        }

        var candidate = Path.Combine(directory, "radarcol.mul");
        return File.Exists(candidate) ? candidate : null;
    }

    private static string ResolveRegionsXmlPath(string configuredPath, string sourceMapPath, string destinationMapPath, string radarColPath)
    {
        if (!string.IsNullOrWhiteSpace(configuredPath) && File.Exists(configuredPath))
        {
            return configuredPath;
        }

        var sourceCandidate = BuildRegionsCandidate(sourceMapPath);
        if (sourceCandidate != null)
        {
            return sourceCandidate;
        }

        var destinationCandidate = BuildRegionsCandidate(destinationMapPath);
        if (destinationCandidate != null)
        {
            return destinationCandidate;
        }

        if (!string.IsNullOrWhiteSpace(radarColPath))
        {
            var directory = Path.GetDirectoryName(radarColPath);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                var besideRadar = Path.Combine(directory, "Regions.xml");
                if (File.Exists(besideRadar))
                {
                    return besideRadar;
                }
            }
        }

        return configuredPath;
    }

    private static string? BuildRegionsCandidate(string mapPath)
    {
        if (string.IsNullOrWhiteSpace(mapPath))
        {
            return null;
        }

        var directory = Path.GetDirectoryName(mapPath);
        if (string.IsNullOrWhiteSpace(directory))
        {
            return null;
        }

        var canonical = Path.Combine(directory, "Regions.xml");
        if (File.Exists(canonical))
        {
            return canonical;
        }

        var lower = Path.Combine(directory, "regions.xml");
        return File.Exists(lower) ? lower : null;
    }

    private void UpdatePreviewButtonState()
    {
        btnPreview.Enabled = _sourceProfile != null
                             && _destinationProfile != null
                             && !string.IsNullOrWhiteSpace(txtSrcMap.Text)
                             && !string.IsNullOrWhiteSpace(txtDstMap.Text)
                             && File.Exists(txtSrcMap.Text)
                             && File.Exists(txtDstMap.Text);
    }
}

