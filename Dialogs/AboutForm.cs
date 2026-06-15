using System.Reflection;

namespace RadMapCopySharp.Dialogs;

public sealed class AboutForm : Form
{
    public AboutForm()
    {
        Text = "About RadMapCopySharp";
        Width = 460;
        Height = 240;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;

        var label = new Label
        {
            Left = 15,
            Top = 20,
            Width = 420,
            Height = 120,
            Text =
                "RadMapCopySharp\r\n" +
                $"Version: 1.0\r\n\r\n" +
                "C# replication and extension of the classic RadMap Copy workflow.\r\n" +
                "Supports MUL map/statics and UOP land-map patching."
        };

        var btnClose = new Button
        {
            Left = 340,
            Top = 150,
            Width = 90,
            Text = "Close",
            DialogResult = DialogResult.OK
        };

        Controls.Add(label);
        Controls.Add(btnClose);
        AcceptButton = btnClose;
        CancelButton = btnClose;
    }
}

