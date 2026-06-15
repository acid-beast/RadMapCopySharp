using System.ComponentModel;
using RadMapCopySharp.Core;

namespace RadMapCopySharp.Dialogs;

public sealed class CreateMapForm : Form
{
    private readonly ComboBox _cmbPreset;
    private readonly TextBox _txtMapPath;
    private readonly NumericUpDown _numTileId;
    private readonly NumericUpDown _numZ;
    private readonly CheckBox _chkCreateStatics;
    private readonly TextBox _txtStaidxPath;
    private readonly TextBox _txtStaticsPath;

    public MapProfile? SelectedProfile => _cmbPreset.SelectedItem as MapProfile;
    public string MapPath => _txtMapPath.Text;
    public ushort TileId => (ushort)_numTileId.Value;
    public sbyte Z => (sbyte)_numZ.Value;
    public bool CreateStatics => _chkCreateStatics.Checked;
    public string StaidxPath => _txtStaidxPath.Text;
    public string StaticsPath => _txtStaticsPath.Text;

    public CreateMapForm()
    {
        Text = "Create Empty Map";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        MinimumSize = new Size(580, 0);
        ClientSize = new Size(580, 0);

        _cmbPreset = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            DisplayMember = "Name",
            Anchor = AnchorStyles.Left | AnchorStyles.Right,
            Width = 100
        };
        _cmbPreset.Items.AddRange(MapProfileDetector.AllProfiles.Cast<object>().ToArray());
        _cmbPreset.SelectedIndex = 0;

        _txtMapPath = new TextBox { Anchor = AnchorStyles.Left | AnchorStyles.Right, Width = 100 };
        var btnBrowseMap = new Button { Text = "...", Anchor = AnchorStyles.Left | AnchorStyles.Right, Width = 40 };
        btnBrowseMap.Click += (_, _) => BrowseFile(_txtMapPath, "Map files (*.mul)|*.mul|All files (*.*)|*.*");

        _numTileId = new NumericUpDown { Minimum = 0, Maximum = ushort.MaxValue, Anchor = AnchorStyles.Left | AnchorStyles.Right, Width = 100 };
        _numZ = new NumericUpDown { Minimum = sbyte.MinValue, Maximum = sbyte.MaxValue, Anchor = AnchorStyles.Left | AnchorStyles.Right, Width = 100 };

        _chkCreateStatics = new CheckBox { AutoSize = true, Text = "Create empty staidx/statics", Margin = new Padding(0, 4, 0, 4) };
        _chkCreateStatics.CheckedChanged += (_, _) => ToggleStaticsFields();

        _txtStaidxPath = new TextBox { Anchor = AnchorStyles.Left | AnchorStyles.Right, Width = 100, Enabled = false };
        var btnBrowseStaidx = new Button { Text = "...", Anchor = AnchorStyles.Left | AnchorStyles.Right, Width = 40, Enabled = false };
        btnBrowseStaidx.Click += (_, _) => BrowseFile(_txtStaidxPath, "MUL files (*.mul)|*.mul|All files (*.*)|*.*");

        _txtStaticsPath = new TextBox { Anchor = AnchorStyles.Left | AnchorStyles.Right, Width = 100, Enabled = false };
        var btnBrowseStatics = new Button { Text = "...", Anchor = AnchorStyles.Left | AnchorStyles.Right, Width = 40, Enabled = false };
        btnBrowseStatics.Click += (_, _) => BrowseFile(_txtStaticsPath, "MUL files (*.mul)|*.mul|All files (*.*)|*.*");

        _chkCreateStatics.Tag = new Control[] { _txtStaidxPath, _txtStaticsPath, btnBrowseStaidx, btnBrowseStatics };

        var btnOk = new Button { Width = 75, Text = "OK", DialogResult = DialogResult.OK };
        btnOk.Click += (_, e) =>
        {
            if (!ValidateInput())
            {
                ((CancelEventArgs)e).Cancel = true;
            }
        };

        var btnCancel = new Button { Width = 75, Text = "Cancel", DialogResult = DialogResult.Cancel };

        var defaultsRow = CreateDefaultsRow(_numTileId, _numZ);

        var buttonRow = new FlowLayoutPanel
        {
            AutoSize = true,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = false,
            Dock = DockStyle.Fill,
            Margin = new Padding(0, 8, 0, 0)
        };
        buttonRow.Controls.Add(btnCancel);
        buttonRow.Controls.Add(btnOk);

        var root = new TableLayoutPanel
        {
            AutoSize = true,
            ColumnCount = 1,
            RowCount = 11,
            Dock = DockStyle.Fill,
            Padding = new Padding(12)
        };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        for (var i = 0; i < 11; i++)
        {
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        }

        root.Controls.Add(CreateSectionLabel("Preset"), 0, 0);
        root.Controls.Add(_cmbPreset, 0, 1);
        root.Controls.Add(CreateSectionLabel("Map output file"), 0, 2);
        root.Controls.Add(CreatePathRow(_txtMapPath, btnBrowseMap), 0, 3);
        root.Controls.Add(defaultsRow, 0, 4);
        root.Controls.Add(_chkCreateStatics, 0, 5);
        root.Controls.Add(CreateSectionLabel("Staidx output"), 0, 6);
        root.Controls.Add(CreatePathRow(_txtStaidxPath, btnBrowseStaidx), 0, 7);
        root.Controls.Add(CreateSectionLabel("Statics output"), 0, 8);
        root.Controls.Add(CreatePathRow(_txtStaticsPath, btnBrowseStatics), 0, 9);
        root.Controls.Add(buttonRow, 0, 10);

        Controls.Add(root);

        AcceptButton = btnOk;
        CancelButton = btnCancel;
    }

    private static Label CreateSectionLabel(string text)
    {
        return new Label
        {
            AutoSize = true,
            Text = text,
            Margin = new Padding(0, 6, 0, 2)
        };
    }

    private static TableLayoutPanel CreateDefaultsRow(NumericUpDown numTileId, NumericUpDown numZ)
    {
        var row = new TableLayoutPanel
        {
            ColumnCount = 2,
            AutoSize = true,
            Dock = DockStyle.Top,
            Margin = new Padding(0, 0, 0, 4)
        };
        row.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
        row.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
        row.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        row.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        row.Controls.Add(new Label { AutoSize = true, Text = "Default Tile ID", Margin = new Padding(0, 0, 8, 2) }, 0, 0);
        row.Controls.Add(new Label { AutoSize = true, Text = "Default Z", Margin = new Padding(8, 0, 0, 2) }, 1, 0);
        row.Controls.Add(numTileId, 0, 1);
        row.Controls.Add(numZ, 1, 1);

        return row;
    }

    private static TableLayoutPanel CreatePathRow(TextBox textBox, Button browseButton)
    {
        var row = new TableLayoutPanel
        {
            ColumnCount = 2,
            AutoSize = true,
            Dock = DockStyle.Top,
            Margin = new Padding(0, 0, 0, 4)
        };
        row.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        row.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 44f));
        row.RowStyles.Add(new RowStyle(SizeType.Absolute, 28f));
        row.Controls.Add(textBox, 0, 0);
        row.Controls.Add(browseButton, 1, 0);
        return row;
    }

    private void ToggleStaticsFields()
    {
        if (_chkCreateStatics.Tag is not Control[] controls)
        {
            return;
        }

        foreach (var control in controls)
        {
            control.Enabled = _chkCreateStatics.Checked;
        }
    }

    private static void BrowseFile(TextBox target, string filter)
    {
        using var dialog = new SaveFileDialog { Filter = filter };
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            target.Text = dialog.FileName;
        }
    }

    private bool ValidateInput()
    {
        if (SelectedProfile == null)
        {
            MessageBox.Show("Select a map preset.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(_txtMapPath.Text))
        {
            MessageBox.Show("Select output map file.");
            return false;
        }

        if (_chkCreateStatics.Checked && (string.IsNullOrWhiteSpace(_txtStaidxPath.Text) || string.IsNullOrWhiteSpace(_txtStaticsPath.Text)))
        {
            MessageBox.Show("Staidx and statics outputs are required.");
            return false;
        }

        return true;
    }
}
