namespace RadMapCopySharp.Dialogs;

public sealed class HelpForm : Form
{
    public HelpForm()
    {
        Text = "RadMapCopySharp Help";
        Width = 760;
        Height = 560;
        StartPosition = FormStartPosition.CenterParent;

        var helpText = new TextBox
        {
            Multiline = true,
            ReadOnly = true,
            ScrollBars = ScrollBars.Vertical,
            Dock = DockStyle.Fill,
            Font = new Font("Consolas", 10F),
            Text =
                "RadMapCopySharp Help\r\n\r\n" +
                "1) Map Copy\r\n" +
                "- Select source and destination map files (.mul or .uop).\r\n" +
                "- Set source rectangle and destination anchor.\r\n" +
                "- Choose altitude mode (Keep Z, Add Fixed, Random Z, Add Random).\r\n" +
                "- Add Fixed adds the offset value to each source Z (land, statics, spawner CentreZ).\r\n" +
                "- Optional skip preset applies to map tile IDs only.\r\n\r\n" +
                "2) Statics Copy\r\n" +
                "- Enable Copy statics and provide source/destination staidx/statics files.\r\n" +
                "- Coordinates must be aligned to 8x8 blocks.\r\n" +
                "- Destination blocks are replaced by appended static blocks.\r\n\r\n" +
                "3) Actions\r\n" +
                "- Preview: open side-by-side source and destination full maps.\r\n" +
                "- Copy: run map/statics copy using current settings.\r\n" +
                "- Create Empty Map: choose preset, default tile and Z, optional empty statics files.\r\n" +
                "- Extend to ML: copy source map into a blank 7168x4096 map.\r\n\r\n" +
                "4) Map Preview\r\n" +
                "- Opened from the Preview action button.\r\n" +
                "- Uses radarcol.mul to color terrain tiles (required for meaningful colors).\r\n" +
                "- Mouse wheel zooms toward cursor, left-drag pans each pane independently.\r\n" +
                "- Use Fit map to reset each pane to full-map fit view.\r\n" +
                "- Optional Regions.xml overlay can be toggled per pane (when that facet has regions).\r\n" +
                "- Hover over a region (when Display regions is on) to see name and coordinates.\r\n" +
                "- Large facets use significant RAM while rendered (Felucca can exceed 100 MB per pane).\r\n" +
                "- If coordinates are invalid, preview still opens with origin view and no overlay.\r\n\r\n" +
                "5) UOP\r\n" +
                "- UOP destination writes map cells in-place.\r\n" +
                "- Always back up UOP files before running copy.\r\n" +
                "- Statics remain MUL files and are copied separately.\r\n"
        };

        Controls.Add(helpText);
    }
}

