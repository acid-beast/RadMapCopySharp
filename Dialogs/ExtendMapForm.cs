using System.ComponentModel;
using RadMapCopySharp.Core;

namespace RadMapCopySharp.Dialogs;

public sealed class ExtendMapForm : Form
{
    private readonly TextBox _txtSource;
    private readonly TextBox _txtDestination;
    private readonly Label _lblSourceProfile;

    public string SourcePath => _txtSource.Text;
    public string DestinationPath => _txtDestination.Text;

    public ExtendMapForm()
    {
        Text = "Extend Map To ML";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        MinimumSize = new Size(580, 0);
        ClientSize = new Size(580, 0);

        _txtSource = new TextBox { Anchor = AnchorStyles.Left | AnchorStyles.Right, Width = 100 };
        var btnBrowseSource = new Button { Text = "...", Anchor = AnchorStyles.Left | AnchorStyles.Right, Width = 40 };
        btnBrowseSource.Click += (_, _) => BrowseOpen(_txtSource);

        _txtDestination = new TextBox { Anchor = AnchorStyles.Left | AnchorStyles.Right, Width = 100 };
        var btnBrowseDestination = new Button { Text = "...", Anchor = AnchorStyles.Left | AnchorStyles.Right, Width = 40 };
        btnBrowseDestination.Click += (_, _) => BrowseSave(_txtDestination);

        _lblSourceProfile = new Label
        {
            AutoSize = true,
            Text = "Source profile: unknown",
            Margin = new Padding(0, 0, 0, 4)
        };
        _txtSource.TextChanged += (_, _) => UpdateSourceProfile();

        var btnOk = new Button { Width = 75, Text = "OK", DialogResult = DialogResult.OK };
        btnOk.Click += (_, e) =>
        {
            if (!ValidateInput())
            {
                ((CancelEventArgs)e).Cancel = true;
            }
        };

        var btnCancel = new Button { Width = 75, Text = "Cancel", DialogResult = DialogResult.Cancel };

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
            RowCount = 6,
            Dock = DockStyle.Fill,
            Padding = new Padding(12)
        };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        for (var i = 0; i < 6; i++)
        {
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        }

        root.Controls.Add(CreateSectionLabel("Source map"), 0, 0);
        root.Controls.Add(CreatePathRow(_txtSource, btnBrowseSource), 0, 1);
        root.Controls.Add(_lblSourceProfile, 0, 2);
        root.Controls.Add(CreateSectionLabel("Destination ML map"), 0, 3);
        root.Controls.Add(CreatePathRow(_txtDestination, btnBrowseDestination), 0, 4);
        root.Controls.Add(buttonRow, 0, 5);

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

    private void UpdateSourceProfile()
    {
        if (MapProfileDetector.TryDetect(_txtSource.Text, out var profile) && profile != null)
        {
            _lblSourceProfile.Text = $"Source profile: {profile.Name}";
        }
        else
        {
            _lblSourceProfile.Text = "Source profile: unknown";
        }
    }

    private bool ValidateInput()
    {
        if (string.IsNullOrWhiteSpace(_txtSource.Text) || !File.Exists(_txtSource.Text))
        {
            MessageBox.Show("Select a valid source map file.");
            return false;
        }

        if (!MapProfileDetector.TryDetect(_txtSource.Text, out var profile) || profile == null)
        {
            MessageBox.Show("Could not detect source profile.");
            return false;
        }

        if (profile.Width == 7168 && profile.Height == 4096)
        {
            MessageBox.Show("Source map is already ML-sized.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(_txtDestination.Text))
        {
            MessageBox.Show("Select destination map file.");
            return false;
        }

        return true;
    }

    private static void BrowseOpen(TextBox target)
    {
        using var dialog = new OpenFileDialog { Filter = "Map files (*.mul)|*.mul|All files (*.*)|*.*" };
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            target.Text = dialog.FileName;
        }
    }

    private static void BrowseSave(TextBox target)
    {
        using var dialog = new SaveFileDialog { Filter = "Map files (*.mul)|*.mul|All files (*.*)|*.*" };
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            target.Text = dialog.FileName;
        }
    }
}
