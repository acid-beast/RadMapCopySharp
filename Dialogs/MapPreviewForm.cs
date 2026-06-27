using RadMapCopySharp.Core.IO;
using RadMapCopySharp.Core.Regions;
using RadMapCopySharp.Core.Rendering;
using RadMapCopySharp.Core.Spawns;

namespace RadMapCopySharp.Dialogs;

public sealed class MapPreviewForm : Form
{
    private readonly Func<MapPreviewState> _stateProvider;
    private readonly Action<string> _onRadarPathChanged;
    private readonly Action<string> _onRegionsPathChanged;

    private readonly Label _lblSourceHeader;
    private readonly Label _lblDestinationHeader;
    private readonly Label _lblSourceInfo;
    private readonly Label _lblDestinationInfo;
    private readonly Label _lblStatus;
    private readonly TextBox _txtRadarCol;
    private readonly TextBox _txtRegionsXml;
    private readonly Button _btnRefresh;
    private readonly Button _btnBrowseRadar;
    private readonly Button _btnBrowseRegions;
    private readonly Button _btnFitSource;
    private readonly Button _btnFitDestination;
    private readonly CheckBox _chkShowSourceRegions;
    private readonly CheckBox _chkShowDestRegions;
    private readonly CheckBox _chkShowSourceSpawners;
    private readonly CheckBox _chkShowDestSpawners;
    private readonly CheckBox _chkShowSourceAllSpawners;
    private readonly CheckBox _chkShowDestAllSpawners;
    private readonly MapPreviewPanel _sourcePanel;
    private readonly MapPreviewPanel _destinationPanel;
    private readonly ToolTip _regionToolTip;

    private readonly MapRadarRenderer _renderer = new();
    private MapPreviewState _state;
    private RegionsDocument? _regionsDocument;
    private string _loadedRegionsPath = string.Empty;
    private SpawnsDocument? _sourceSpawnsDocument;
    private SpawnsDocument? _destinationSpawnsDocument;
    private string _loadedSourceSpawnsPath = string.Empty;
    private string _loadedDestinationSpawnsPath = string.Empty;
    private string _sourceBaseInfo = string.Empty;
    private string _destinationBaseInfo = string.Empty;

    public MapPreviewForm(
        MapPreviewState state,
        Func<MapPreviewState> stateProvider,
        Action<string> onRadarPathChanged,
        Action<string> onRegionsPathChanged)
    {
        _state = state;
        _stateProvider = stateProvider;
        _onRadarPathChanged = onRadarPathChanged;
        _onRegionsPathChanged = onRegionsPathChanged;

        Text = "Map Preview";
        Width = (int)(1220 * 1.1);
        Height = (int)(790 * 1.1);
        MinimumSize = new Size((int)(980 * 1.1), (int)(640 * 1.1));
        StartPosition = FormStartPosition.CenterParent;

        _lblSourceHeader = new Label { AutoSize = true, TextAlign = ContentAlignment.MiddleLeft, Margin = new Padding(0, 6, 6, 0) };
        _lblDestinationHeader = new Label { AutoSize = true, TextAlign = ContentAlignment.MiddleLeft, Margin = new Padding(0, 6, 6, 0) };
        _lblSourceInfo = new Label { Dock = DockStyle.Fill, AutoSize = false, TextAlign = ContentAlignment.MiddleLeft };
        _lblDestinationInfo = new Label { Dock = DockStyle.Fill, AutoSize = false, TextAlign = ContentAlignment.MiddleLeft };
        _lblStatus = new Label { AutoSize = true, Text = "Ready" };

        _txtRadarCol = new TextBox { Width = 560 };
        _txtRadarCol.TextChanged += (_, _) => _onRadarPathChanged(_txtRadarCol.Text);

        _txtRegionsXml = new TextBox { Width = 560 };
        _txtRegionsXml.TextChanged += (_, _) =>
        {
            _onRegionsPathChanged(_txtRegionsXml.Text);
            OnRegionsPathChanged(showSuccessStatus: false);
        };

        _btnBrowseRadar = new Button { Text = "Browse...", Width = 84 };
        _btnBrowseRadar.Click += (_, _) => BrowseRadarColPath();

        _btnBrowseRegions = new Button { Text = "Browse...", Width = 84 };
        _btnBrowseRegions.Click += (_, _) => BrowseRegionsPath();

        _btnRefresh = new Button { Text = "Refresh", Width = 84 };
        _btnRefresh.Click += async (_, _) => await RefreshFromProviderAsync();

        _sourcePanel = new MapPreviewPanel { Dock = DockStyle.Fill };
        _destinationPanel = new MapPreviewPanel { Dock = DockStyle.Fill };
        _sourcePanel.ViewChanged += (_, _) => UpdateSourceInfo();
        _destinationPanel.ViewChanged += (_, _) => UpdateDestinationInfo();
        _sourcePanel.RegionHoverChanged += (_, _) => UpdateHoverTooltip(_sourcePanel);
        _destinationPanel.RegionHoverChanged += (_, _) => UpdateHoverTooltip(_destinationPanel);
        _sourcePanel.SpawnerHoverChanged += (_, _) => UpdateHoverTooltip(_sourcePanel);
        _destinationPanel.SpawnerHoverChanged += (_, _) => UpdateHoverTooltip(_destinationPanel);

        _regionToolTip = new ToolTip
        {
            ShowAlways = true,
            AutomaticDelay = 0,
            AutoPopDelay = 10000,
            InitialDelay = 0,
            ReshowDelay = 0
        };

        _btnFitSource = new Button { Text = "Fit map", Width = 72 };
        _btnFitSource.Click += (_, _) => _sourcePanel.FitToWindow();

        _btnFitDestination = new Button { Text = "Fit map", Width = 72 };
        _btnFitDestination.Click += (_, _) => _destinationPanel.FitToWindow();

        _chkShowSourceRegions = new CheckBox { AutoSize = true, Text = "Display regions", Margin = new Padding(10, 8, 0, 0) };
        _chkShowSourceRegions.CheckedChanged += (_, _) => ApplyRegionOverlays();

        _chkShowDestRegions = new CheckBox { AutoSize = true, Text = "Display regions", Margin = new Padding(10, 8, 0, 0) };
        _chkShowDestRegions.CheckedChanged += (_, _) => ApplyRegionOverlays();

        _chkShowSourceSpawners = new CheckBox { AutoSize = true, Text = "In copy area", Margin = new Padding(10, 8, 0, 0) };
        _chkShowSourceSpawners.CheckedChanged += (_, _) => ApplySpawnerOverlays();

        _chkShowDestSpawners = new CheckBox { AutoSize = true, Text = "In copy area", Margin = new Padding(10, 8, 0, 0) };
        _chkShowDestSpawners.CheckedChanged += (_, _) => ApplySpawnerOverlays();

        _chkShowSourceAllSpawners = new CheckBox { AutoSize = true, Text = "All spawners", Margin = new Padding(10, 8, 0, 0) };
        _chkShowSourceAllSpawners.CheckedChanged += (_, _) => ApplySpawnerOverlays();

        _chkShowDestAllSpawners = new CheckBox { AutoSize = true, Text = "All spawners", Margin = new Padding(10, 8, 0, 0) };
        _chkShowDestAllSpawners.CheckedChanged += (_, _) => ApplySpawnerOverlays();

        var btnClose = new Button { Text = "Close", Width = 84, DialogResult = DialogResult.OK };

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 4
        };
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 54f));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 54f));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 52f));

        var mapsLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 3,
            Padding = new Padding(10)
        };
        mapsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
        mapsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
        mapsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48f));
        mapsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
        mapsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));

        var sourceHeaderHost = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = true, FlowDirection = FlowDirection.LeftToRight, Margin = new Padding(0) };
        sourceHeaderHost.Controls.Add(_lblSourceHeader);
        sourceHeaderHost.Controls.Add(_btnFitSource);
        sourceHeaderHost.Controls.Add(_chkShowSourceRegions);
        sourceHeaderHost.Controls.Add(_chkShowSourceSpawners);
        sourceHeaderHost.Controls.Add(_chkShowSourceAllSpawners);

        var destinationHeaderHost = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = true, FlowDirection = FlowDirection.LeftToRight, Margin = new Padding(0) };
        destinationHeaderHost.Controls.Add(_lblDestinationHeader);
        destinationHeaderHost.Controls.Add(_btnFitDestination);
        destinationHeaderHost.Controls.Add(_chkShowDestRegions);
        destinationHeaderHost.Controls.Add(_chkShowDestSpawners);
        destinationHeaderHost.Controls.Add(_chkShowDestAllSpawners);

        mapsLayout.Controls.Add(sourceHeaderHost, 0, 0);
        mapsLayout.Controls.Add(destinationHeaderHost, 1, 0);
        mapsLayout.Controls.Add(_sourcePanel, 0, 1);
        mapsLayout.Controls.Add(_destinationPanel, 1, 1);
        mapsLayout.Controls.Add(_lblSourceInfo, 0, 2);
        mapsLayout.Controls.Add(_lblDestinationInfo, 1, 2);

        var radarRow = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            Padding = new Padding(10, 12, 10, 8),
            WrapContents = false
        };
        radarRow.Controls.Add(new Label { Text = "radarcol.mul", Width = 90, TextAlign = ContentAlignment.MiddleLeft });
        radarRow.Controls.Add(_txtRadarCol);
        radarRow.Controls.Add(_btnBrowseRadar);

        var regionsRow = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            Padding = new Padding(10, 12, 10, 8),
            WrapContents = false
        };
        regionsRow.Controls.Add(new Label { Text = "Regions.xml", Width = 90, TextAlign = ContentAlignment.MiddleLeft });
        regionsRow.Controls.Add(_txtRegionsXml);
        regionsRow.Controls.Add(_btnBrowseRegions);
        regionsRow.Controls.Add(_btnRefresh);
        regionsRow.Controls.Add(btnClose);

        var statusRow = new Panel { Dock = DockStyle.Fill, Padding = new Padding(12, 4, 12, 0) };
        statusRow.Controls.Add(_lblStatus);

        root.Controls.Add(mapsLayout, 0, 0);
        root.Controls.Add(radarRow, 0, 1);
        root.Controls.Add(regionsRow, 0, 2);
        root.Controls.Add(statusRow, 0, 3);

        Controls.Add(root);
        AcceptButton = _btnRefresh;
        CancelButton = btnClose;

        Shown += async (_, _) =>
        {
            await RenderAsync(_state);
            OnRegionsPathChanged(showSuccessStatus: false);
            LoadSpawnsFromState(_state);
        };
    }

    private async Task RefreshFromProviderAsync()
    {
        try
        {
            _state = _stateProvider();
        }
        catch (Exception ex)
        {
            _lblStatus.Text = ex.Message;
            _lblStatus.ForeColor = Color.DarkRed;
            return;
        }

        _state = new MapPreviewState
        {
            SourceMapPath = _state.SourceMapPath,
            DestinationMapPath = _state.DestinationMapPath,
            SourceProfile = _state.SourceProfile,
            DestinationProfile = _state.DestinationProfile,
            SourceRect = _state.SourceRect,
            DestinationAnchor = _state.DestinationAnchor,
            OverlayMessage = _state.OverlayMessage,
            RadarColPath = _txtRadarCol.Text,
            RegionsXmlPath = _txtRegionsXml.Text,
            SourceSpawnsXmlPath = _state.SourceSpawnsXmlPath,
            DestinationSpawnsXmlPath = _state.DestinationSpawnsXmlPath
        };

        _onRadarPathChanged(_state.RadarColPath);
        _onRegionsPathChanged(_state.RegionsXmlPath);
        await RenderAsync(_state);
        OnRegionsPathChanged(showSuccessStatus: false);
        LoadSpawnsFromState(_state);
    }

    private async Task RenderAsync(MapPreviewState state)
    {
        _btnRefresh.Enabled = false;
        _btnBrowseRadar.Enabled = false;
        _btnBrowseRegions.Enabled = false;
        _btnFitSource.Enabled = false;
        _btnFitDestination.Enabled = false;
        _lblStatus.ForeColor = SystemColors.ControlText;
        _lblStatus.Text = "Rendering preview...";

        _lblSourceHeader.Text = $"Source: {state.SourceProfile.Name}";
        _lblDestinationHeader.Text = $"Destination: {state.DestinationProfile.Name}";
        if (!string.Equals(_txtRadarCol.Text, state.RadarColPath, StringComparison.OrdinalIgnoreCase))
        {
            _txtRadarCol.Text = state.RadarColPath;
        }

        if (!string.Equals(_txtRegionsXml.Text, state.RegionsXmlPath, StringComparison.OrdinalIgnoreCase))
        {
            _txtRegionsXml.Text = state.RegionsXmlPath;
        }

        try
        {
            LoadSpawnsFromState(state);
            var sourceProgress = new Progress<int>(percent =>
            {
                _lblStatus.Text = $"Rendering source map... {percent}%";
            });

            var destinationProgress = new Progress<int>(percent =>
            {
                _lblStatus.Text = $"Rendering destination map... {percent}%";
            });

            var rendered = await Task.Run(() => BuildRenderedPreview(state, sourceProgress, destinationProgress));
            _sourcePanel.SetContent(rendered.SourceBitmap, rendered.SourceMapSize, rendered.SourceOverlay, null, Color.Gold, fitToWindow: true);
            _destinationPanel.SetContent(rendered.DestinationBitmap, rendered.DestinationMapSize, rendered.DestinationOverlay, rendered.DestinationAnchor, Color.LimeGreen, fitToWindow: true);

            _sourceBaseInfo = rendered.SourceInfo;
            _destinationBaseInfo = rendered.DestinationInfo;
            UpdateSourceInfo();
            UpdateDestinationInfo();

            _lblStatus.Text = rendered.Status;
            _lblStatus.ForeColor = rendered.IsError ? Color.DarkRed : SystemColors.ControlText;
        }
        catch (Exception ex)
        {
            _sourcePanel.SetContent(null, new Size(1, 1), null, null, Color.Gold, fitToWindow: true);
            _destinationPanel.SetContent(null, new Size(1, 1), null, null, Color.LimeGreen, fitToWindow: true);
            _sourceBaseInfo = "Source preview unavailable.";
            _destinationBaseInfo = "Destination preview unavailable.";
            _lblSourceInfo.Text = _sourceBaseInfo;
            _lblDestinationInfo.Text = _destinationBaseInfo;
            _lblStatus.Text = ex.Message;
            _lblStatus.ForeColor = Color.DarkRed;
        }
        finally
        {
            _btnRefresh.Enabled = true;
            _btnBrowseRadar.Enabled = true;
            _btnBrowseRegions.Enabled = true;
            _btnFitSource.Enabled = true;
            _btnFitDestination.Enabled = true;
        }
    }

    private RenderResult BuildRenderedPreview(MapPreviewState state, IProgress<int>? sourceProgress, IProgress<int>? destinationProgress)
    {
        var table = RadarColorTable.Load(state.RadarColPath);

        var sourceOverlay = state.SourceRect;
        var destinationOverlay = state.GetDestinationRect();

        using var sourceMap = LandMapFileFactory.Open(state.SourceMapPath, FileAccess.Read);
        using var destinationMap = LandMapFileFactory.Open(state.DestinationMapPath, FileAccess.Read);

        var sourceBitmap = _renderer.RenderFullMap(sourceMap, table, sourceProgress);
        var destinationBitmap = _renderer.RenderFullMap(destinationMap, table, destinationProgress);

        var sourceInfo = sourceOverlay == null
            ? $"Map: {state.SourceProfile.Width}x{state.SourceProfile.Height}"
            : $"Rect: {sourceOverlay.X1},{sourceOverlay.Y1} - {sourceOverlay.X2},{sourceOverlay.Y2} ({sourceOverlay.Width}x{sourceOverlay.Height})";

        var destinationInfo = destinationOverlay == null
            ? $"Map: {state.DestinationProfile.Width}x{state.DestinationProfile.Height}"
            : $"Anchor: {state.DestinationAnchor!.Value.X},{state.DestinationAnchor!.Value.Y}";

        var status = string.IsNullOrWhiteSpace(state.OverlayMessage)
            ? "Preview updated. Mouse wheel zooms and drag pans each pane independently."
            : state.OverlayMessage;

        return new RenderResult(
            sourceBitmap,
            destinationBitmap,
            new Size(state.SourceProfile.Width, state.SourceProfile.Height),
            new Size(state.DestinationProfile.Width, state.DestinationProfile.Height),
            sourceOverlay,
            destinationOverlay,
            state.DestinationAnchor,
            sourceInfo,
            destinationInfo,
            status,
            false);
    }

    private void UpdateSourceInfo()
    {
        _lblSourceInfo.Text = ComposePaneInfo(_sourceBaseInfo, _sourcePanel);
    }

    private void UpdateDestinationInfo()
    {
        _lblDestinationInfo.Text = ComposePaneInfo(_destinationBaseInfo, _destinationPanel);
    }

    private static string ComposePaneInfo(string baseText, MapPreviewPanel panel)
    {
        var pointer = panel.PointToClient(Control.MousePosition);
        var zoomText = $"Zoom: {panel.Zoom * 100f:0}%";
        var parts = new List<string> { baseText, zoomText };

        if (panel.TryScreenToMap(pointer, out var tile))
        {
            parts.Add($"Tile: {tile.X},{tile.Y}");
        }

        if (panel.HoveredSpawner is { } spawner)
        {
            parts.Add($"Spawner: {spawner.Name}");
        }
        else if (panel.HoveredRegion is { } region)
        {
            parts.Add($"Region: {region.Name}");
        }

        return string.Join(" | ", parts);
    }

    private void UpdateHoverTooltip(MapPreviewPanel panel)
    {
        if (panel.HoveredSpawner is { } spawner)
        {
            _regionToolTip.SetToolTip(panel, BuildSpawnerTooltip(spawner));
        }
        else if (panel.HoveredRegion is { } region)
        {
            var bounds = region.Bounds;
            var typeSuffix = string.IsNullOrWhiteSpace(region.Type) ? string.Empty : $" ({region.Type})";
            var text = $"{region.Name}{typeSuffix}\n{bounds.X1},{bounds.Y1} - {bounds.X2},{bounds.Y2}";
            _regionToolTip.SetToolTip(panel, text);
        }
        else
        {
            _regionToolTip.SetToolTip(panel, string.Empty);
        }

        if (panel == _sourcePanel)
        {
            UpdateSourceInfo();
        }
        else
        {
            UpdateDestinationInfo();
        }
    }

    private static string BuildSpawnerTooltip(SpawnerOverlay spawner)
    {
        var bounds = spawner.Bounds;
        var creatureText = FormatCreaturesForTooltip(spawner.Creatures);
        var spawnsLine = creatureText.Contains('\n', StringComparison.Ordinal)
            ? $"Spawns:\n{creatureText}"
            : $"Spawns: {creatureText}";
        var delayUnit = spawner.DelayInSec ? "s" : "m";
        return $"{spawner.Name}\n" +
               $"{spawnsLine}\n" +
               $"Max {spawner.MaxCount} | Range {spawner.Range} | Delay {spawner.MinDelay}-{spawner.MaxDelay}{delayUnit}\n" +
               $"Centre {spawner.CentreX},{spawner.CentreY} | Box {bounds.X1},{bounds.Y1} - {bounds.X2},{bounds.Y2}";
    }

    private static string FormatCreaturesForTooltip(IReadOnlyList<string> creatures)
    {
        if (creatures.Count == 0)
        {
            return "(none)";
        }

        var joined = string.Join(", ", creatures);
        if (joined.Length <= 120)
        {
            return joined;
        }

        var lines = new List<string>();
        for (var i = 0; i < creatures.Count; i += 5)
        {
            var chunk = creatures.Skip(i).Take(5);
            lines.Add(string.Join(", ", chunk));
        }

        return string.Join("\n", lines);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _regionToolTip.Dispose();
        }

        base.Dispose(disposing);
    }

    private void BrowseRadarColPath()
    {
        using var dialog = new OpenFileDialog
        {
            Filter = "radarcol.mul|radarcol.mul|MUL files (*.mul)|*.mul|All files (*.*)|*.*",
            FileName = "radarcol.mul"
        };

        if (dialog.ShowDialog(this) == DialogResult.OK)
        {
            _txtRadarCol.Text = dialog.FileName;
            _onRadarPathChanged(dialog.FileName);
        }
    }

    private void BrowseRegionsPath()
    {
        using var dialog = new OpenFileDialog
        {
            Filter = "Regions.xml|Regions.xml|XML files (*.xml)|*.xml|All files (*.*)|*.*",
            FileName = "Regions.xml"
        };

        if (dialog.ShowDialog(this) == DialogResult.OK)
        {
            _txtRegionsXml.Text = dialog.FileName;
            _onRegionsPathChanged(dialog.FileName);
            OnRegionsPathChanged(showSuccessStatus: true);
        }
    }

    private void OnRegionsPathChanged(bool showSuccessStatus)
    {
        var path = _txtRegionsXml.Text;
        if (!string.Equals(path, _loadedRegionsPath, StringComparison.OrdinalIgnoreCase))
        {
            try
            {
                _regionsDocument = string.IsNullOrWhiteSpace(path) ? null : RegionsXmlParser.Load(path);
                _loadedRegionsPath = path;

                if (showSuccessStatus && _regionsDocument != null)
                {
                    _lblStatus.Text = "Regions.xml loaded.";
                    _lblStatus.ForeColor = SystemColors.ControlText;
                }
            }
            catch (Exception ex)
            {
                _regionsDocument = null;
                _loadedRegionsPath = string.Empty;
                _lblStatus.Text = $"Regions overlay disabled: {ex.Message}";
                _lblStatus.ForeColor = Color.DarkRed;
            }
        }

        UpdateRegionCheckboxState();
        ApplyRegionOverlays();
    }

    private void UpdateRegionCheckboxState()
    {
        var sourceCount = _regionsDocument?.GetCountForProfile(_state.SourceProfile) ?? 0;
        var destinationCount = _regionsDocument?.GetCountForProfile(_state.DestinationProfile) ?? 0;

        var sourceEnabled = _regionsDocument != null && sourceCount > 0;
        var destinationEnabled = _regionsDocument != null && destinationCount > 0;

        _chkShowSourceRegions.Enabled = sourceEnabled;
        _chkShowDestRegions.Enabled = destinationEnabled;

        _chkShowSourceRegions.Text = sourceEnabled ? $"Display regions ({sourceCount})" : "Display regions";
        _chkShowDestRegions.Text = destinationEnabled ? $"Display regions ({destinationCount})" : "Display regions";

        if (!sourceEnabled)
        {
            _chkShowSourceRegions.Checked = false;
        }

        if (!destinationEnabled)
        {
            _chkShowDestRegions.Checked = false;
        }
    }

    private void ApplyRegionOverlays()
    {
        _sourcePanel.SetRegions(_regionsDocument?.GetForProfile(_state.SourceProfile), _chkShowSourceRegions.Checked);
        _destinationPanel.SetRegions(_regionsDocument?.GetForProfile(_state.DestinationProfile), _chkShowDestRegions.Checked);
    }

    private void LoadSpawnsFromState(MapPreviewState state)
    {
        LoadSpawnsDocument(
            state.SourceSpawnsXmlPath,
            ref _loadedSourceSpawnsPath,
            ref _sourceSpawnsDocument,
            "source");
        LoadSpawnsDocument(
            state.DestinationSpawnsXmlPath,
            ref _loadedDestinationSpawnsPath,
            ref _destinationSpawnsDocument,
            "destination");
        UpdateSpawnerCheckboxState();
        ApplySpawnerOverlays();
    }

    private void LoadSpawnsDocument(
        string path,
        ref string loadedPath,
        ref SpawnsDocument? document,
        string role)
    {
        if (string.Equals(path, loadedPath, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        try
        {
            document = string.IsNullOrWhiteSpace(path) ? null : SpawnsXmlParser.Load(path);
            loadedPath = path;
        }
        catch (Exception ex)
        {
            document = null;
            loadedPath = string.Empty;
            _lblStatus.Text = $"Spawner overlay disabled for {role} map: {ex.Message}";
            _lblStatus.ForeColor = Color.DarkRed;
        }
    }

    private void UpdateSpawnerCheckboxState()
    {
        var sourceSpawners = _sourceSpawnsDocument?.GetForProfile(_state.SourceProfile);
        var destinationSpawners = _destinationSpawnsDocument?.GetForProfile(_state.DestinationProfile);
        var sourceCount = sourceSpawners?.Count ?? 0;
        var destinationCount = destinationSpawners?.Count ?? 0;
        var sourceInRegion = CountSpawnersInRegion(sourceSpawners, _state.SourceRect);
        var destinationInRegion = CountSpawnersInRegion(destinationSpawners, _state.GetDestinationRect());
        var sourceHasRegion = _state.SourceRect != null;
        var destinationHasRegion = _state.GetDestinationRect() != null;

        var sourceEnabled = sourceSpawners != null && sourceCount > 0;
        var destinationEnabled = destinationSpawners != null && destinationCount > 0;

        _chkShowSourceSpawners.Enabled = sourceEnabled && sourceHasRegion;
        _chkShowDestSpawners.Enabled = destinationEnabled && destinationHasRegion;
        _chkShowSourceAllSpawners.Enabled = sourceEnabled;
        _chkShowDestAllSpawners.Enabled = destinationEnabled;

        _chkShowSourceSpawners.Text = sourceEnabled && sourceHasRegion
            ? $"In copy area ({sourceInRegion})"
            : "In copy area";
        _chkShowDestSpawners.Text = destinationEnabled && destinationHasRegion
            ? $"In copy area ({destinationInRegion})"
            : "In copy area";
        _chkShowSourceAllSpawners.Text = sourceEnabled
            ? $"All spawners ({sourceCount})"
            : "All spawners";
        _chkShowDestAllSpawners.Text = destinationEnabled
            ? $"All spawners ({destinationCount})"
            : "All spawners";

        if (!sourceEnabled)
        {
            _chkShowSourceSpawners.Checked = false;
            _chkShowSourceAllSpawners.Checked = false;
        }
        else if (!sourceHasRegion)
        {
            _chkShowSourceSpawners.Checked = false;
        }

        if (!destinationEnabled)
        {
            _chkShowDestSpawners.Checked = false;
            _chkShowDestAllSpawners.Checked = false;
        }
        else if (!destinationHasRegion)
        {
            _chkShowDestSpawners.Checked = false;
        }
    }

    private void ApplySpawnerOverlays()
    {
        var sourceAll = _sourceSpawnsDocument?.GetForProfile(_state.SourceProfile);
        var destinationAll = _destinationSpawnsDocument?.GetForProfile(_state.DestinationProfile);
        var sourceInRegion = FilterSpawnersByCentre(sourceAll, _state.SourceRect);
        var destinationInRegion = FilterSpawnersByCentre(destinationAll, _state.GetDestinationRect());

        IReadOnlyList<SpawnerOverlay>? sourceToShow = null;
        if (_chkShowSourceAllSpawners.Checked)
        {
            sourceToShow = sourceAll;
        }
        else if (_chkShowSourceSpawners.Checked)
        {
            sourceToShow = sourceInRegion;
        }

        IReadOnlyList<SpawnerOverlay>? destinationToShow = null;
        if (_chkShowDestAllSpawners.Checked)
        {
            destinationToShow = destinationAll;
        }
        else if (_chkShowDestSpawners.Checked)
        {
            destinationToShow = destinationInRegion;
        }

        _sourcePanel.SetSpawners(
            sourceToShow,
            _chkShowSourceSpawners.Checked || _chkShowSourceAllSpawners.Checked);
        _destinationPanel.SetSpawners(
            destinationToShow,
            _chkShowDestSpawners.Checked || _chkShowDestAllSpawners.Checked);
    }

    private static int CountSpawnersInRegion(IReadOnlyList<SpawnerOverlay>? spawners, CopyRectangle? region)
    {
        if (spawners == null || region == null)
        {
            return 0;
        }

        var count = 0;
        foreach (var spawner in spawners)
        {
            if (region.ContainsPoint(spawner.CentreX, spawner.CentreY))
            {
                count++;
            }
        }

        return count;
    }

    private static IReadOnlyList<SpawnerOverlay>? FilterSpawnersByCentre(
        IReadOnlyList<SpawnerOverlay>? spawners,
        CopyRectangle? region)
    {
        if (spawners == null)
        {
            return null;
        }

        if (region == null)
        {
            return spawners;
        }

        return spawners
            .Where(spawner => region.ContainsPoint(spawner.CentreX, spawner.CentreY))
            .ToList();
    }

    private sealed record RenderResult(
        Bitmap SourceBitmap,
        Bitmap DestinationBitmap,
        Size SourceMapSize,
        Size DestinationMapSize,
        CopyRectangle? SourceOverlay,
        CopyRectangle? DestinationOverlay,
        Point? DestinationAnchor,
        string SourceInfo,
        string DestinationInfo,
        string Status,
        bool IsError);
}
