namespace RadMapCopySharp
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelpHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.panelMain = new System.Windows.Forms.Panel();
            this.tableRoot = new System.Windows.Forms.TableLayoutPanel();
            this.tablePaths = new System.Windows.Forms.TableLayoutPanel();
            this.grpSourceFiles = new System.Windows.Forms.GroupBox();
            this.tableSourceFiles = new System.Windows.Forms.TableLayoutPanel();
            this.lblSrcMap = new System.Windows.Forms.Label();
            this.txtSrcMap = new System.Windows.Forms.TextBox();
            this.btnBrowseSrcMap = new System.Windows.Forms.Button();
            this.lblSrcStaidx = new System.Windows.Forms.Label();
            this.txtSrcStaidx = new System.Windows.Forms.TextBox();
            this.btnBrowseSrcStaidx = new System.Windows.Forms.Button();
            this.lblSrcStatics = new System.Windows.Forms.Label();
            this.txtSrcStatics = new System.Windows.Forms.TextBox();
            this.btnBrowseSrcStatics = new System.Windows.Forms.Button();
            this.grpDestinationFiles = new System.Windows.Forms.GroupBox();
            this.tableDestinationFiles = new System.Windows.Forms.TableLayoutPanel();
            this.lblDstMap = new System.Windows.Forms.Label();
            this.txtDstMap = new System.Windows.Forms.TextBox();
            this.btnBrowseDstMap = new System.Windows.Forms.Button();
            this.lblDstStaidx = new System.Windows.Forms.Label();
            this.txtDstStaidx = new System.Windows.Forms.TextBox();
            this.btnBrowseDstStaidx = new System.Windows.Forms.Button();
            this.lblDstStatics = new System.Windows.Forms.Label();
            this.txtDstStatics = new System.Windows.Forms.TextBox();
            this.btnBrowseDstStatics = new System.Windows.Forms.Button();
            this.tableInfo = new System.Windows.Forms.TableLayoutPanel();
            this.grpSourceInfo = new System.Windows.Forms.GroupBox();
            this.lblSourceProfile = new System.Windows.Forms.Label();
            this.grpDestinationInfo = new System.Windows.Forms.GroupBox();
            this.lblDestinationProfile = new System.Windows.Forms.Label();
            this.tableCoords = new System.Windows.Forms.TableLayoutPanel();
            this.grpSourceRect = new System.Windows.Forms.GroupBox();
            this.tableSourceRect = new System.Windows.Forms.TableLayoutPanel();
            this.lblX1 = new System.Windows.Forms.Label();
            this.txtSX1 = new System.Windows.Forms.TextBox();
            this.lblY1 = new System.Windows.Forms.Label();
            this.txtSY1 = new System.Windows.Forms.TextBox();
            this.lblX2 = new System.Windows.Forms.Label();
            this.txtSX2 = new System.Windows.Forms.TextBox();
            this.lblY2 = new System.Windows.Forms.Label();
            this.txtSY2 = new System.Windows.Forms.TextBox();
            this.grpDestinationAnchor = new System.Windows.Forms.GroupBox();
            this.tableDestinationAnchor = new System.Windows.Forms.TableLayoutPanel();
            this.lblDestX = new System.Windows.Forms.Label();
            this.txtDX = new System.Windows.Forms.TextBox();
            this.lblDestY = new System.Windows.Forms.Label();
            this.txtDY = new System.Windows.Forms.TextBox();
            this.grpCopyMode = new System.Windows.Forms.GroupBox();
            this.flowCopyMode = new System.Windows.Forms.FlowLayoutPanel();
            this.rbKeepZ = new System.Windows.Forms.RadioButton();
            this.rbRandomZ = new System.Windows.Forms.RadioButton();
            this.rbAddRandomZ = new System.Windows.Forms.RadioButton();
            this.lblZ1 = new System.Windows.Forms.Label();
            this.numZ1 = new System.Windows.Forms.NumericUpDown();
            this.lblZ2 = new System.Windows.Forms.Label();
            this.numZ2 = new System.Windows.Forms.NumericUpDown();
            this.flowOptions = new System.Windows.Forms.FlowLayoutPanel();
            this.chkCopyMap = new System.Windows.Forms.CheckBox();
            this.chkCopyStatics = new System.Windows.Forms.CheckBox();
            this.lblSkipPreset = new System.Windows.Forms.Label();
            this.cmbSkipPreset = new System.Windows.Forms.ComboBox();
            this.tableCopyOptions = new System.Windows.Forms.TableLayoutPanel();
            this.grpActions = new System.Windows.Forms.GroupBox();
            this.tableActions = new System.Windows.Forms.TableLayoutPanel();
            this.btnPreview = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnCreateEmptyMap = new System.Windows.Forms.Button();
            this.btnExtendToMl = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.menuStripMain.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.tableRoot.SuspendLayout();
            this.tablePaths.SuspendLayout();
            this.grpSourceFiles.SuspendLayout();
            this.tableSourceFiles.SuspendLayout();
            this.grpDestinationFiles.SuspendLayout();
            this.tableDestinationFiles.SuspendLayout();
            this.tableInfo.SuspendLayout();
            this.grpSourceInfo.SuspendLayout();
            this.grpDestinationInfo.SuspendLayout();
            this.tableCoords.SuspendLayout();
            this.grpSourceRect.SuspendLayout();
            this.tableSourceRect.SuspendLayout();
            this.grpDestinationAnchor.SuspendLayout();
            this.tableDestinationAnchor.SuspendLayout();
            this.grpCopyMode.SuspendLayout();
            this.flowCopyMode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numZ1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numZ2)).BeginInit();
            this.flowOptions.SuspendLayout();
            this.tableCopyOptions.SuspendLayout();
            this.grpActions.SuspendLayout();
            this.tableActions.SuspendLayout();
            this.SuspendLayout();
            //
            // menuStripMain
            //
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuHelp});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new System.Drawing.Size(1200, 24);
            this.menuStripMain.TabIndex = 0;
            this.menuStripMain.Text = "menuStripMain";
            //
            // menuFile
            //
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFileExit});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(37, 20);
            this.menuFile.Text = "File";
            //
            // menuFileExit
            //
            this.menuFileExit.Name = "menuFileExit";
            this.menuFileExit.Size = new System.Drawing.Size(93, 22);
            this.menuFileExit.Text = "Exit";
            this.menuFileExit.Click += new System.EventHandler(this.menuFileExit_Click);
            //
            // menuHelp
            //
            this.menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuHelpHelp,
            this.menuHelpAbout});
            this.menuHelp.Name = "menuHelp";
            this.menuHelp.Size = new System.Drawing.Size(44, 20);
            this.menuHelp.Text = "Help";
            //
            // menuHelpHelp
            //
            this.menuHelpHelp.Name = "menuHelpHelp";
            this.menuHelpHelp.Size = new System.Drawing.Size(107, 22);
            this.menuHelpHelp.Text = "Help";
            this.menuHelpHelp.Click += new System.EventHandler(this.menuHelpHelp_Click);
            //
            // menuHelpAbout
            //
            this.menuHelpAbout.Name = "menuHelpAbout";
            this.menuHelpAbout.Size = new System.Drawing.Size(107, 22);
            this.menuHelpAbout.Text = "About";
            this.menuHelpAbout.Click += new System.EventHandler(this.menuHelpAbout_Click);
            //
            // panelFooter
            //
            this.panelFooter.Controls.Add(this.lblStatus);
            this.panelFooter.Controls.Add(this.progressBar);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(0, 548);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.panelFooter.Size = new System.Drawing.Size(1200, 52);
            this.panelFooter.TabIndex = 2;
            //
            // lblStatus
            //
            this.lblStatus.AutoSize = true;
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatus.Location = new System.Drawing.Point(12, 31);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(38, 15);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "Ready";
            //
            // progressBar
            //
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.progressBar.Location = new System.Drawing.Point(12, 8);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(1176, 18);
            this.progressBar.TabIndex = 0;
            //
            // panelMain
            //
            this.panelMain.Controls.Add(this.tableRoot);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 24);
            this.panelMain.Name = "panelMain";
            this.panelMain.Padding = new System.Windows.Forms.Padding(12);
            this.panelMain.Size = new System.Drawing.Size(1200, 524);
            this.panelMain.TabIndex = 1;
            //
            // tableRoot
            //
            this.tableRoot.ColumnCount = 1;
            this.tableRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableRoot.Controls.Add(this.tablePaths, 0, 0);
            this.tableRoot.Controls.Add(this.tableInfo, 0, 1);
            this.tableRoot.Controls.Add(this.tableCoords, 0, 2);
            this.tableRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableRoot.Location = new System.Drawing.Point(12, 12);
            this.tableRoot.Name = "tableRoot";
            this.tableRoot.RowCount = 4;
            this.tableRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tableRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.tableRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 188F));
            this.tableRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableRoot.Size = new System.Drawing.Size(1176, 500);
            this.tableRoot.TabIndex = 0;
            //
            // tablePaths
            //
            this.tablePaths.ColumnCount = 2;
            this.tablePaths.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tablePaths.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tablePaths.Controls.Add(this.grpSourceFiles, 0, 0);
            this.tablePaths.Controls.Add(this.grpDestinationFiles, 1, 0);
            this.tablePaths.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePaths.Location = new System.Drawing.Point(0, 0);
            this.tablePaths.Margin = new System.Windows.Forms.Padding(0);
            this.tablePaths.Name = "tablePaths";
            this.tablePaths.RowCount = 1;
            this.tablePaths.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tablePaths.Size = new System.Drawing.Size(1176, 160);
            this.tablePaths.TabIndex = 0;
            //
            // grpSourceFiles
            //
            this.grpSourceFiles.Controls.Add(this.tableSourceFiles);
            this.grpSourceFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSourceFiles.Location = new System.Drawing.Point(3, 3);
            this.grpSourceFiles.Name = "grpSourceFiles";
            this.grpSourceFiles.Padding = new System.Windows.Forms.Padding(8);
            this.grpSourceFiles.Size = new System.Drawing.Size(362, 154);
            this.grpSourceFiles.TabIndex = 0;
            this.grpSourceFiles.TabStop = false;
            this.grpSourceFiles.Text = "Source Files";
            //
            // tableSourceFiles
            //
            this.tableSourceFiles.ColumnCount = 3;
            this.tableSourceFiles.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.tableSourceFiles.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableSourceFiles.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableSourceFiles.Controls.Add(this.lblSrcMap, 0, 0);
            this.tableSourceFiles.Controls.Add(this.txtSrcMap, 1, 0);
            this.tableSourceFiles.Controls.Add(this.btnBrowseSrcMap, 2, 0);
            this.tableSourceFiles.Controls.Add(this.lblSrcStaidx, 0, 1);
            this.tableSourceFiles.Controls.Add(this.txtSrcStaidx, 1, 1);
            this.tableSourceFiles.Controls.Add(this.btnBrowseSrcStaidx, 2, 1);
            this.tableSourceFiles.Controls.Add(this.lblSrcStatics, 0, 2);
            this.tableSourceFiles.Controls.Add(this.txtSrcStatics, 1, 2);
            this.tableSourceFiles.Controls.Add(this.btnBrowseSrcStatics, 2, 2);
            this.tableSourceFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableSourceFiles.Location = new System.Drawing.Point(8, 24);
            this.tableSourceFiles.Name = "tableSourceFiles";
            this.tableSourceFiles.RowCount = 3;
            this.tableSourceFiles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableSourceFiles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableSourceFiles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableSourceFiles.Size = new System.Drawing.Size(346, 122);
            this.tableSourceFiles.TabIndex = 0;
            //
            // lblSrcMap
            //
            this.lblSrcMap.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblSrcMap.AutoSize = true;
            this.lblSrcMap.Location = new System.Drawing.Point(3, 12);
            this.lblSrcMap.Name = "lblSrcMap";
            this.lblSrcMap.Size = new System.Drawing.Size(32, 15);
            this.lblSrcMap.TabIndex = 0;
            this.lblSrcMap.Text = "Map";
            //
            // txtSrcMap
            //
            this.txtSrcMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSrcMap.Location = new System.Drawing.Point(59, 8);
            this.txtSrcMap.Name = "txtSrcMap";
            this.txtSrcMap.Size = new System.Drawing.Size(252, 23);
            this.txtSrcMap.TabIndex = 1;
            this.txtSrcMap.TextChanged += new System.EventHandler(this.txtSrcMap_TextChanged);
            //
            // btnBrowseSrcMap
            //
            this.btnBrowseSrcMap.Location = new System.Drawing.Point(317, 3);
            this.btnBrowseSrcMap.Name = "btnBrowseSrcMap";
            this.btnBrowseSrcMap.Size = new System.Drawing.Size(26, 34);
            this.btnBrowseSrcMap.TabIndex = 2;
            this.btnBrowseSrcMap.Text = "...";
            this.btnBrowseSrcMap.UseVisualStyleBackColor = true;
            this.btnBrowseSrcMap.Click += new System.EventHandler(this.btnBrowseSrcMap_Click);
            //
            // lblSrcStaidx
            //
            this.lblSrcStaidx.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblSrcStaidx.AutoSize = true;
            this.lblSrcStaidx.Location = new System.Drawing.Point(3, 52);
            this.lblSrcStaidx.Name = "lblSrcStaidx";
            this.lblSrcStaidx.Size = new System.Drawing.Size(42, 15);
            this.lblSrcStaidx.TabIndex = 3;
            this.lblSrcStaidx.Text = "StaIdx";
            //
            // txtSrcStaidx
            //
            this.txtSrcStaidx.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSrcStaidx.Location = new System.Drawing.Point(59, 48);
            this.txtSrcStaidx.Name = "txtSrcStaidx";
            this.txtSrcStaidx.Size = new System.Drawing.Size(252, 23);
            this.txtSrcStaidx.TabIndex = 4;
            //
            // btnBrowseSrcStaidx
            //
            this.btnBrowseSrcStaidx.Location = new System.Drawing.Point(317, 43);
            this.btnBrowseSrcStaidx.Name = "btnBrowseSrcStaidx";
            this.btnBrowseSrcStaidx.Size = new System.Drawing.Size(26, 34);
            this.btnBrowseSrcStaidx.TabIndex = 5;
            this.btnBrowseSrcStaidx.Text = "...";
            this.btnBrowseSrcStaidx.UseVisualStyleBackColor = true;
            this.btnBrowseSrcStaidx.Click += new System.EventHandler(this.btnBrowseSrcStaidx_Click);
            //
            // lblSrcStatics
            //
            this.lblSrcStatics.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblSrcStatics.AutoSize = true;
            this.lblSrcStatics.Location = new System.Drawing.Point(3, 93);
            this.lblSrcStatics.Name = "lblSrcStatics";
            this.lblSrcStatics.Size = new System.Drawing.Size(43, 15);
            this.lblSrcStatics.TabIndex = 6;
            this.lblSrcStatics.Text = "Statics";
            //
            // txtSrcStatics
            //
            this.txtSrcStatics.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSrcStatics.Location = new System.Drawing.Point(59, 89);
            this.txtSrcStatics.Name = "txtSrcStatics";
            this.txtSrcStatics.Size = new System.Drawing.Size(252, 23);
            this.txtSrcStatics.TabIndex = 7;
            //
            // btnBrowseSrcStatics
            //
            this.btnBrowseSrcStatics.Location = new System.Drawing.Point(317, 83);
            this.btnBrowseSrcStatics.Name = "btnBrowseSrcStatics";
            this.btnBrowseSrcStatics.Size = new System.Drawing.Size(26, 36);
            this.btnBrowseSrcStatics.TabIndex = 8;
            this.btnBrowseSrcStatics.Text = "...";
            this.btnBrowseSrcStatics.UseVisualStyleBackColor = true;
            this.btnBrowseSrcStatics.Click += new System.EventHandler(this.btnBrowseSrcStatics_Click);
            //
            // grpDestinationFiles
            //
            this.grpDestinationFiles.Controls.Add(this.tableDestinationFiles);
            this.grpDestinationFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpDestinationFiles.Location = new System.Drawing.Point(371, 3);
            this.grpDestinationFiles.Name = "grpDestinationFiles";
            this.grpDestinationFiles.Padding = new System.Windows.Forms.Padding(8);
            this.grpDestinationFiles.Size = new System.Drawing.Size(362, 154);
            this.grpDestinationFiles.TabIndex = 1;
            this.grpDestinationFiles.TabStop = false;
            this.grpDestinationFiles.Text = "Destination Files";
            //
            // tableDestinationFiles
            //
            this.tableDestinationFiles.ColumnCount = 3;
            this.tableDestinationFiles.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.tableDestinationFiles.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableDestinationFiles.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableDestinationFiles.Controls.Add(this.lblDstMap, 0, 0);
            this.tableDestinationFiles.Controls.Add(this.txtDstMap, 1, 0);
            this.tableDestinationFiles.Controls.Add(this.btnBrowseDstMap, 2, 0);
            this.tableDestinationFiles.Controls.Add(this.lblDstStaidx, 0, 1);
            this.tableDestinationFiles.Controls.Add(this.txtDstStaidx, 1, 1);
            this.tableDestinationFiles.Controls.Add(this.btnBrowseDstStaidx, 2, 1);
            this.tableDestinationFiles.Controls.Add(this.lblDstStatics, 0, 2);
            this.tableDestinationFiles.Controls.Add(this.txtDstStatics, 1, 2);
            this.tableDestinationFiles.Controls.Add(this.btnBrowseDstStatics, 2, 2);
            this.tableDestinationFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableDestinationFiles.Location = new System.Drawing.Point(8, 24);
            this.tableDestinationFiles.Name = "tableDestinationFiles";
            this.tableDestinationFiles.RowCount = 3;
            this.tableDestinationFiles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableDestinationFiles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableDestinationFiles.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableDestinationFiles.Size = new System.Drawing.Size(346, 122);
            this.tableDestinationFiles.TabIndex = 0;
            //
            // lblDstMap
            //
            this.lblDstMap.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDstMap.AutoSize = true;
            this.lblDstMap.Location = new System.Drawing.Point(3, 12);
            this.lblDstMap.Name = "lblDstMap";
            this.lblDstMap.Size = new System.Drawing.Size(32, 15);
            this.lblDstMap.TabIndex = 0;
            this.lblDstMap.Text = "Map";
            //
            // txtDstMap
            //
            this.txtDstMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDstMap.Location = new System.Drawing.Point(59, 8);
            this.txtDstMap.Name = "txtDstMap";
            this.txtDstMap.Size = new System.Drawing.Size(252, 23);
            this.txtDstMap.TabIndex = 1;
            this.txtDstMap.TextChanged += new System.EventHandler(this.txtDstMap_TextChanged);
            //
            // btnBrowseDstMap
            //
            this.btnBrowseDstMap.Location = new System.Drawing.Point(317, 3);
            this.btnBrowseDstMap.Name = "btnBrowseDstMap";
            this.btnBrowseDstMap.Size = new System.Drawing.Size(26, 34);
            this.btnBrowseDstMap.TabIndex = 2;
            this.btnBrowseDstMap.Text = "...";
            this.btnBrowseDstMap.UseVisualStyleBackColor = true;
            this.btnBrowseDstMap.Click += new System.EventHandler(this.btnBrowseDstMap_Click);
            //
            // lblDstStaidx
            //
            this.lblDstStaidx.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDstStaidx.AutoSize = true;
            this.lblDstStaidx.Location = new System.Drawing.Point(3, 52);
            this.lblDstStaidx.Name = "lblDstStaidx";
            this.lblDstStaidx.Size = new System.Drawing.Size(42, 15);
            this.lblDstStaidx.TabIndex = 3;
            this.lblDstStaidx.Text = "StaIdx";
            //
            // txtDstStaidx
            //
            this.txtDstStaidx.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDstStaidx.Location = new System.Drawing.Point(59, 48);
            this.txtDstStaidx.Name = "txtDstStaidx";
            this.txtDstStaidx.Size = new System.Drawing.Size(252, 23);
            this.txtDstStaidx.TabIndex = 4;
            //
            // btnBrowseDstStaidx
            //
            this.btnBrowseDstStaidx.Location = new System.Drawing.Point(317, 43);
            this.btnBrowseDstStaidx.Name = "btnBrowseDstStaidx";
            this.btnBrowseDstStaidx.Size = new System.Drawing.Size(26, 34);
            this.btnBrowseDstStaidx.TabIndex = 5;
            this.btnBrowseDstStaidx.Text = "...";
            this.btnBrowseDstStaidx.UseVisualStyleBackColor = true;
            this.btnBrowseDstStaidx.Click += new System.EventHandler(this.btnBrowseDstStaidx_Click);
            //
            // lblDstStatics
            //
            this.lblDstStatics.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDstStatics.AutoSize = true;
            this.lblDstStatics.Location = new System.Drawing.Point(3, 93);
            this.lblDstStatics.Name = "lblDstStatics";
            this.lblDstStatics.Size = new System.Drawing.Size(43, 15);
            this.lblDstStatics.TabIndex = 6;
            this.lblDstStatics.Text = "Statics";
            //
            // txtDstStatics
            //
            this.txtDstStatics.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDstStatics.Location = new System.Drawing.Point(59, 89);
            this.txtDstStatics.Name = "txtDstStatics";
            this.txtDstStatics.Size = new System.Drawing.Size(252, 23);
            this.txtDstStatics.TabIndex = 7;
            //
            // btnBrowseDstStatics
            //
            this.btnBrowseDstStatics.Location = new System.Drawing.Point(317, 83);
            this.btnBrowseDstStatics.Name = "btnBrowseDstStatics";
            this.btnBrowseDstStatics.Size = new System.Drawing.Size(26, 36);
            this.btnBrowseDstStatics.TabIndex = 8;
            this.btnBrowseDstStatics.Text = "...";
            this.btnBrowseDstStatics.UseVisualStyleBackColor = true;
            this.btnBrowseDstStatics.Click += new System.EventHandler(this.btnBrowseDstStatics_Click);
            //
            // tableInfo
            //
            this.tableInfo.ColumnCount = 2;
            this.tableInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableInfo.Controls.Add(this.grpSourceInfo, 0, 0);
            this.tableInfo.Controls.Add(this.grpDestinationInfo, 1, 0);
            this.tableInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableInfo.Location = new System.Drawing.Point(0, 160);
            this.tableInfo.Margin = new System.Windows.Forms.Padding(0);
            this.tableInfo.Name = "tableInfo";
            this.tableInfo.RowCount = 1;
            this.tableInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableInfo.Size = new System.Drawing.Size(1176, 56);
            this.tableInfo.TabIndex = 1;
            //
            // grpSourceInfo
            //
            this.grpSourceInfo.Controls.Add(this.lblSourceProfile);
            this.grpSourceInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSourceInfo.Location = new System.Drawing.Point(3, 3);
            this.grpSourceInfo.Name = "grpSourceInfo";
            this.grpSourceInfo.Size = new System.Drawing.Size(362, 50);
            this.grpSourceInfo.TabIndex = 0;
            this.grpSourceInfo.TabStop = false;
            this.grpSourceInfo.Text = "Source Profile";
            //
            // lblSourceProfile
            //
            this.lblSourceProfile.AutoSize = true;
            this.lblSourceProfile.Location = new System.Drawing.Point(9, 22);
            this.lblSourceProfile.Name = "lblSourceProfile";
            this.lblSourceProfile.Size = new System.Drawing.Size(97, 15);
            this.lblSourceProfile.TabIndex = 0;
            this.lblSourceProfile.Text = "Profile: unknown";
            //
            // grpDestinationInfo
            //
            this.grpDestinationInfo.Controls.Add(this.lblDestinationProfile);
            this.grpDestinationInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpDestinationInfo.Location = new System.Drawing.Point(371, 3);
            this.grpDestinationInfo.Name = "grpDestinationInfo";
            this.grpDestinationInfo.Size = new System.Drawing.Size(362, 50);
            this.grpDestinationInfo.TabIndex = 1;
            this.grpDestinationInfo.TabStop = false;
            this.grpDestinationInfo.Text = "Destination Profile";
            //
            // lblDestinationProfile
            //
            this.lblDestinationProfile.AutoSize = true;
            this.lblDestinationProfile.Location = new System.Drawing.Point(9, 22);
            this.lblDestinationProfile.Name = "lblDestinationProfile";
            this.lblDestinationProfile.Size = new System.Drawing.Size(97, 15);
            this.lblDestinationProfile.TabIndex = 0;
            this.lblDestinationProfile.Text = "Profile: unknown";
            //
            // tableCoords
            //
            this.tableCoords.ColumnCount = 2;
            this.tableCoords.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableCoords.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableCoords.Controls.Add(this.grpSourceRect, 0, 0);
            this.tableCoords.Controls.Add(this.grpDestinationAnchor, 1, 0);
            this.tableCoords.Controls.Add(this.grpCopyMode, 0, 1);
            this.tableCoords.Controls.Add(this.grpActions, 1, 1);
            this.tableCoords.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableCoords.Location = new System.Drawing.Point(0, 216);
            this.tableCoords.Margin = new System.Windows.Forms.Padding(0);
            this.tableCoords.Name = "tableCoords";
            this.tableCoords.RowCount = 2;
            this.tableCoords.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 88F));
            this.tableCoords.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableCoords.Size = new System.Drawing.Size(1176, 188);
            this.tableCoords.TabIndex = 2;
            //
            // grpSourceRect
            //
            this.grpSourceRect.Controls.Add(this.tableSourceRect);
            this.grpSourceRect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSourceRect.Location = new System.Drawing.Point(3, 3);
            this.grpSourceRect.Name = "grpSourceRect";
            this.grpSourceRect.Padding = new System.Windows.Forms.Padding(8);
            this.grpSourceRect.Size = new System.Drawing.Size(362, 82);
            this.grpSourceRect.TabIndex = 0;
            this.grpSourceRect.TabStop = false;
            this.grpSourceRect.Text = "Source Rectangle";
            //
            // tableSourceRect
            //
            this.tableSourceRect.ColumnCount = 8;
            this.tableSourceRect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableSourceRect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableSourceRect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableSourceRect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableSourceRect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableSourceRect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableSourceRect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableSourceRect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableSourceRect.Controls.Add(this.lblX1, 0, 0);
            this.tableSourceRect.Controls.Add(this.txtSX1, 1, 0);
            this.tableSourceRect.Controls.Add(this.lblY1, 2, 0);
            this.tableSourceRect.Controls.Add(this.txtSY1, 3, 0);
            this.tableSourceRect.Controls.Add(this.lblX2, 4, 0);
            this.tableSourceRect.Controls.Add(this.txtSX2, 5, 0);
            this.tableSourceRect.Controls.Add(this.lblY2, 6, 0);
            this.tableSourceRect.Controls.Add(this.txtSY2, 7, 0);
            this.tableSourceRect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableSourceRect.Location = new System.Drawing.Point(8, 24);
            this.tableSourceRect.Name = "tableSourceRect";
            this.tableSourceRect.RowCount = 1;
            this.tableSourceRect.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableSourceRect.Size = new System.Drawing.Size(346, 50);
            this.tableSourceRect.TabIndex = 0;
            //
            // lblX1
            //
            this.lblX1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblX1.AutoSize = true;
            this.lblX1.Location = new System.Drawing.Point(3, 17);
            this.lblX1.Name = "lblX1";
            this.lblX1.Size = new System.Drawing.Size(20, 15);
            this.lblX1.TabIndex = 0;
            this.lblX1.Text = "X1";
            //
            // txtSX1
            //
            this.txtSX1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtSX1.Location = new System.Drawing.Point(29, 13);
            this.txtSX1.Name = "txtSX1";
            this.txtSX1.Size = new System.Drawing.Size(50, 23);
            this.txtSX1.TabIndex = 1;
            //
            // lblY1
            //
            this.lblY1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblY1.AutoSize = true;
            this.lblY1.Location = new System.Drawing.Point(87, 17);
            this.lblY1.Name = "lblY1";
            this.lblY1.Size = new System.Drawing.Size(20, 15);
            this.lblY1.TabIndex = 2;
            this.lblY1.Text = "Y1";
            //
            // txtSY1
            //
            this.txtSY1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtSY1.Location = new System.Drawing.Point(113, 13);
            this.txtSY1.Name = "txtSY1";
            this.txtSY1.Size = new System.Drawing.Size(50, 23);
            this.txtSY1.TabIndex = 3;
            //
            // lblX2
            //
            this.lblX2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblX2.AutoSize = true;
            this.lblX2.Location = new System.Drawing.Point(171, 17);
            this.lblX2.Name = "lblX2";
            this.lblX2.Size = new System.Drawing.Size(20, 15);
            this.lblX2.TabIndex = 4;
            this.lblX2.Text = "X2";
            //
            // txtSX2
            //
            this.txtSX2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtSX2.Location = new System.Drawing.Point(197, 13);
            this.txtSX2.Name = "txtSX2";
            this.txtSX2.Size = new System.Drawing.Size(50, 23);
            this.txtSX2.TabIndex = 5;
            //
            // lblY2
            //
            this.lblY2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblY2.AutoSize = true;
            this.lblY2.Location = new System.Drawing.Point(255, 17);
            this.lblY2.Name = "lblY2";
            this.lblY2.Size = new System.Drawing.Size(20, 15);
            this.lblY2.TabIndex = 6;
            this.lblY2.Text = "Y2";
            //
            // txtSY2
            //
            this.txtSY2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtSY2.Location = new System.Drawing.Point(281, 13);
            this.txtSY2.Name = "txtSY2";
            this.txtSY2.Size = new System.Drawing.Size(50, 23);
            this.txtSY2.TabIndex = 7;
            //
            // grpDestinationAnchor
            //
            this.grpDestinationAnchor.Controls.Add(this.tableDestinationAnchor);
            this.grpDestinationAnchor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpDestinationAnchor.Location = new System.Drawing.Point(371, 3);
            this.grpDestinationAnchor.Name = "grpDestinationAnchor";
            this.grpDestinationAnchor.Padding = new System.Windows.Forms.Padding(8);
            this.grpDestinationAnchor.Size = new System.Drawing.Size(362, 82);
            this.grpDestinationAnchor.TabIndex = 1;
            this.grpDestinationAnchor.TabStop = false;
            this.grpDestinationAnchor.Text = "Destination Anchor";
            //
            // tableDestinationAnchor
            //
            this.tableDestinationAnchor.ColumnCount = 4;
            this.tableDestinationAnchor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableDestinationAnchor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableDestinationAnchor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableDestinationAnchor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableDestinationAnchor.Controls.Add(this.lblDestX, 0, 0);
            this.tableDestinationAnchor.Controls.Add(this.txtDX, 1, 0);
            this.tableDestinationAnchor.Controls.Add(this.lblDestY, 2, 0);
            this.tableDestinationAnchor.Controls.Add(this.txtDY, 3, 0);
            this.tableDestinationAnchor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableDestinationAnchor.Location = new System.Drawing.Point(8, 24);
            this.tableDestinationAnchor.Name = "tableDestinationAnchor";
            this.tableDestinationAnchor.RowCount = 1;
            this.tableDestinationAnchor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableDestinationAnchor.Size = new System.Drawing.Size(346, 50);
            this.tableDestinationAnchor.TabIndex = 0;
            //
            // lblDestX
            //
            this.lblDestX.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDestX.AutoSize = true;
            this.lblDestX.Location = new System.Drawing.Point(3, 17);
            this.lblDestX.Name = "lblDestX";
            this.lblDestX.Size = new System.Drawing.Size(14, 15);
            this.lblDestX.TabIndex = 0;
            this.lblDestX.Text = "X";
            //
            // txtDX
            //
            this.txtDX.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtDX.Location = new System.Drawing.Point(29, 13);
            this.txtDX.Name = "txtDX";
            this.txtDX.Size = new System.Drawing.Size(50, 23);
            this.txtDX.TabIndex = 1;
            //
            // lblDestY
            //
            this.lblDestY.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDestY.AutoSize = true;
            this.lblDestY.Location = new System.Drawing.Point(99, 17);
            this.lblDestY.Name = "lblDestY";
            this.lblDestY.Size = new System.Drawing.Size(14, 15);
            this.lblDestY.TabIndex = 2;
            this.lblDestY.Text = "Y";
            //
            // txtDY
            //
            this.txtDY.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtDY.Location = new System.Drawing.Point(125, 13);
            this.txtDY.Name = "txtDY";
            this.txtDY.Size = new System.Drawing.Size(50, 23);
            this.txtDY.TabIndex = 3;
            //
            // grpCopyMode
            //
            this.grpCopyMode.Controls.Add(this.tableCopyOptions);
            this.grpCopyMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpCopyMode.Location = new System.Drawing.Point(3, 91);
            this.grpCopyMode.Name = "grpCopyMode";
            this.grpCopyMode.Padding = new System.Windows.Forms.Padding(8);
            this.grpCopyMode.Size = new System.Drawing.Size(582, 94);
            this.grpCopyMode.TabIndex = 3;
            this.grpCopyMode.TabStop = false;
            this.grpCopyMode.Text = "Copy Mode";
            //
            // flowCopyMode
            //
            this.flowCopyMode.Controls.Add(this.rbKeepZ);
            this.flowCopyMode.Controls.Add(this.rbRandomZ);
            this.flowCopyMode.Controls.Add(this.rbAddRandomZ);
            this.flowCopyMode.Controls.Add(this.lblZ1);
            this.flowCopyMode.Controls.Add(this.numZ1);
            this.flowCopyMode.Controls.Add(this.lblZ2);
            this.flowCopyMode.Controls.Add(this.numZ2);
            this.flowCopyMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowCopyMode.Location = new System.Drawing.Point(0, 0);
            this.flowCopyMode.Margin = new System.Windows.Forms.Padding(0);
            this.flowCopyMode.Name = "flowCopyMode";
            this.flowCopyMode.Size = new System.Drawing.Size(566, 39);
            this.flowCopyMode.TabIndex = 0;
            this.flowCopyMode.WrapContents = false;
            //
            // rbKeepZ
            //
            this.rbKeepZ.AutoSize = true;
            this.rbKeepZ.Checked = true;
            this.rbKeepZ.Location = new System.Drawing.Point(3, 3);
            this.rbKeepZ.Name = "rbKeepZ";
            this.rbKeepZ.Size = new System.Drawing.Size(62, 19);
            this.rbKeepZ.TabIndex = 0;
            this.rbKeepZ.TabStop = true;
            this.rbKeepZ.Text = "Keep Z";
            this.rbKeepZ.UseVisualStyleBackColor = true;
            //
            // rbRandomZ
            //
            this.rbRandomZ.AutoSize = true;
            this.rbRandomZ.Location = new System.Drawing.Point(71, 3);
            this.rbRandomZ.Name = "rbRandomZ";
            this.rbRandomZ.Size = new System.Drawing.Size(80, 19);
            this.rbRandomZ.TabIndex = 1;
            this.rbRandomZ.Text = "Random Z";
            this.rbRandomZ.UseVisualStyleBackColor = true;
            //
            // rbAddRandomZ
            //
            this.rbAddRandomZ.AutoSize = true;
            this.rbAddRandomZ.Location = new System.Drawing.Point(157, 3);
            this.rbAddRandomZ.Name = "rbAddRandomZ";
            this.rbAddRandomZ.Size = new System.Drawing.Size(95, 19);
            this.rbAddRandomZ.TabIndex = 2;
            this.rbAddRandomZ.Text = "Add Random";
            this.rbAddRandomZ.UseVisualStyleBackColor = true;
            //
            // lblZ1
            //
            this.lblZ1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblZ1.AutoSize = true;
            this.lblZ1.Location = new System.Drawing.Point(258, 5);
            this.lblZ1.Name = "lblZ1";
            this.lblZ1.Size = new System.Drawing.Size(20, 15);
            this.lblZ1.TabIndex = 3;
            this.lblZ1.Text = "Z1";
            //
            // numZ1
            //
            this.numZ1.Location = new System.Drawing.Point(284, 3);
            this.numZ1.Name = "numZ1";
            this.numZ1.Size = new System.Drawing.Size(64, 23);
            this.numZ1.TabIndex = 4;
            //
            // lblZ2
            //
            this.lblZ2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblZ2.AutoSize = true;
            this.lblZ2.Location = new System.Drawing.Point(354, 5);
            this.lblZ2.Name = "lblZ2";
            this.lblZ2.Size = new System.Drawing.Size(20, 15);
            this.lblZ2.TabIndex = 5;
            this.lblZ2.Text = "Z2";
            //
            // numZ2
            //
            this.numZ2.Location = new System.Drawing.Point(380, 3);
            this.numZ2.Name = "numZ2";
            this.numZ2.Size = new System.Drawing.Size(64, 23);
            this.numZ2.TabIndex = 6;
            //
            // flowOptions
            //
            this.flowOptions.Controls.Add(this.chkCopyMap);
            this.flowOptions.Controls.Add(this.chkCopyStatics);
            this.flowOptions.Controls.Add(this.lblSkipPreset);
            this.flowOptions.Controls.Add(this.cmbSkipPreset);
            this.flowOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowOptions.Location = new System.Drawing.Point(0, 39);
            this.flowOptions.Margin = new System.Windows.Forms.Padding(0);
            this.flowOptions.Name = "flowOptions";
            this.flowOptions.Size = new System.Drawing.Size(566, 39);
            this.flowOptions.TabIndex = 1;
            this.flowOptions.WrapContents = false;
            //
            // chkCopyMap
            //
            this.chkCopyMap.AutoSize = true;
            this.chkCopyMap.Checked = true;
            this.chkCopyMap.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCopyMap.Location = new System.Drawing.Point(3, 3);
            this.chkCopyMap.Name = "chkCopyMap";
            this.chkCopyMap.Size = new System.Drawing.Size(77, 19);
            this.chkCopyMap.TabIndex = 0;
            this.chkCopyMap.Text = "Copy map";
            this.chkCopyMap.UseVisualStyleBackColor = true;
            //
            // chkCopyStatics
            //
            this.chkCopyStatics.AutoSize = true;
            this.chkCopyStatics.Location = new System.Drawing.Point(86, 3);
            this.chkCopyStatics.Name = "chkCopyStatics";
            this.chkCopyStatics.Size = new System.Drawing.Size(86, 19);
            this.chkCopyStatics.TabIndex = 1;
            this.chkCopyStatics.Text = "Copy statics";
            this.chkCopyStatics.UseVisualStyleBackColor = true;
            //
            // lblSkipPreset
            //
            this.lblSkipPreset.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblSkipPreset.AutoSize = true;
            this.lblSkipPreset.Location = new System.Drawing.Point(178, 5);
            this.lblSkipPreset.Name = "lblSkipPreset";
            this.lblSkipPreset.Size = new System.Drawing.Size(53, 15);
            this.lblSkipPreset.TabIndex = 2;
            this.lblSkipPreset.Text = "Skip IDs:";
            //
            // cmbSkipPreset
            //
            this.cmbSkipPreset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSkipPreset.FormattingEnabled = true;
            this.cmbSkipPreset.Location = new System.Drawing.Point(237, 3);
            this.cmbSkipPreset.Name = "cmbSkipPreset";
            this.cmbSkipPreset.Size = new System.Drawing.Size(220, 23);
            this.cmbSkipPreset.TabIndex = 3;
            //
            // tableCopyOptions
            //
            this.tableCopyOptions.ColumnCount = 1;
            this.tableCopyOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableCopyOptions.Controls.Add(this.flowCopyMode, 0, 0);
            this.tableCopyOptions.Controls.Add(this.flowOptions, 0, 1);
            this.tableCopyOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableCopyOptions.Location = new System.Drawing.Point(8, 24);
            this.tableCopyOptions.Name = "tableCopyOptions";
            this.tableCopyOptions.RowCount = 2;
            this.tableCopyOptions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableCopyOptions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableCopyOptions.Size = new System.Drawing.Size(566, 62);
            this.tableCopyOptions.TabIndex = 0;
            //
            // grpActions
            //
            this.grpActions.Controls.Add(this.tableActions);
            this.grpActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpActions.Location = new System.Drawing.Point(591, 91);
            this.grpActions.Name = "grpActions";
            this.grpActions.Padding = new System.Windows.Forms.Padding(8);
            this.grpActions.Size = new System.Drawing.Size(582, 94);
            this.grpActions.TabIndex = 4;
            this.grpActions.TabStop = false;
            this.grpActions.Text = "Actions";
            //
            // tableActions
            //
            this.tableActions.ColumnCount = 2;
            this.tableActions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableActions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableActions.Controls.Add(this.btnPreview, 0, 0);
            this.tableActions.Controls.Add(this.btnCopy, 1, 0);
            this.tableActions.Controls.Add(this.btnCreateEmptyMap, 0, 1);
            this.tableActions.Controls.Add(this.btnExtendToMl, 1, 1);
            this.tableActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableActions.Location = new System.Drawing.Point(8, 24);
            this.tableActions.Name = "tableActions";
            this.tableActions.RowCount = 2;
            this.tableActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableActions.Size = new System.Drawing.Size(566, 62);
            this.tableActions.TabIndex = 0;
            //
            // btnPreview
            //
            this.btnPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPreview.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnPreview.Location = new System.Drawing.Point(3, 3);
            this.btnPreview.Margin = new System.Windows.Forms.Padding(3);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(277, 25);
            this.btnPreview.TabIndex = 0;
            this.btnPreview.Text = "Preview";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            //
            // btnCopy
            //
            this.btnCopy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCopy.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnCopy.Location = new System.Drawing.Point(286, 3);
            this.btnCopy.Margin = new System.Windows.Forms.Padding(3);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(277, 25);
            this.btnCopy.TabIndex = 1;
            this.btnCopy.Text = "Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            //
            // btnCreateEmptyMap
            //
            this.btnCreateEmptyMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCreateEmptyMap.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnCreateEmptyMap.Location = new System.Drawing.Point(3, 34);
            this.btnCreateEmptyMap.Margin = new System.Windows.Forms.Padding(3);
            this.btnCreateEmptyMap.Name = "btnCreateEmptyMap";
            this.btnCreateEmptyMap.Size = new System.Drawing.Size(277, 25);
            this.btnCreateEmptyMap.TabIndex = 2;
            this.btnCreateEmptyMap.Text = "Create Empty Map";
            this.btnCreateEmptyMap.UseVisualStyleBackColor = true;
            this.btnCreateEmptyMap.Click += new System.EventHandler(this.btnCreateEmptyMap_Click);
            //
            // btnExtendToMl
            //
            this.btnExtendToMl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExtendToMl.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnExtendToMl.Location = new System.Drawing.Point(286, 34);
            this.btnExtendToMl.Margin = new System.Windows.Forms.Padding(3);
            this.btnExtendToMl.Name = "btnExtendToMl";
            this.btnExtendToMl.Size = new System.Drawing.Size(277, 25);
            this.btnExtendToMl.TabIndex = 3;
            this.btnExtendToMl.Text = "Extend to ML";
            this.btnExtendToMl.UseVisualStyleBackColor = true;
            this.btnExtendToMl.Click += new System.EventHandler(this.btnExtendToMl_Click);
            //
            // MainForm
            //
            this.AcceptButton = this.btnCopy;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 550);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelFooter);
            this.Controls.Add(this.menuStripMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStripMain;
            this.MaximumSize = new System.Drawing.Size(1200, 550);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1200, 550);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RadMapCopySharp";
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.panelFooter.ResumeLayout(false);
            this.panelFooter.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.tableRoot.ResumeLayout(false);
            this.tablePaths.ResumeLayout(false);
            this.grpSourceFiles.ResumeLayout(false);
            this.tableSourceFiles.ResumeLayout(false);
            this.tableSourceFiles.PerformLayout();
            this.grpDestinationFiles.ResumeLayout(false);
            this.tableDestinationFiles.ResumeLayout(false);
            this.tableDestinationFiles.PerformLayout();
            this.tableInfo.ResumeLayout(false);
            this.grpSourceInfo.ResumeLayout(false);
            this.grpSourceInfo.PerformLayout();
            this.grpDestinationInfo.ResumeLayout(false);
            this.grpDestinationInfo.PerformLayout();
            this.tableCoords.ResumeLayout(false);
            this.grpSourceRect.ResumeLayout(false);
            this.tableSourceRect.ResumeLayout(false);
            this.tableSourceRect.PerformLayout();
            this.grpDestinationAnchor.ResumeLayout(false);
            this.tableDestinationAnchor.ResumeLayout(false);
            this.tableDestinationAnchor.PerformLayout();
            this.grpCopyMode.ResumeLayout(false);
            this.flowCopyMode.ResumeLayout(false);
            this.flowCopyMode.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numZ1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numZ2)).EndInit();
            this.flowOptions.ResumeLayout(false);
            this.flowOptions.PerformLayout();
            this.tableCopyOptions.ResumeLayout(false);
            this.grpActions.ResumeLayout(false);
            this.tableActions.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuFileExit;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripMenuItem menuHelpHelp;
        private System.Windows.Forms.ToolStripMenuItem menuHelpAbout;
        private System.Windows.Forms.Panel panelFooter;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.TableLayoutPanel tableRoot;
        private System.Windows.Forms.TableLayoutPanel tablePaths;
        private System.Windows.Forms.GroupBox grpSourceFiles;
        private System.Windows.Forms.TableLayoutPanel tableSourceFiles;
        private System.Windows.Forms.Label lblSrcMap;
        private System.Windows.Forms.TextBox txtSrcMap;
        private System.Windows.Forms.Button btnBrowseSrcMap;
        private System.Windows.Forms.Label lblSrcStaidx;
        private System.Windows.Forms.TextBox txtSrcStaidx;
        private System.Windows.Forms.Button btnBrowseSrcStaidx;
        private System.Windows.Forms.Label lblSrcStatics;
        private System.Windows.Forms.TextBox txtSrcStatics;
        private System.Windows.Forms.Button btnBrowseSrcStatics;
        private System.Windows.Forms.GroupBox grpDestinationFiles;
        private System.Windows.Forms.TableLayoutPanel tableDestinationFiles;
        private System.Windows.Forms.Label lblDstMap;
        private System.Windows.Forms.TextBox txtDstMap;
        private System.Windows.Forms.Button btnBrowseDstMap;
        private System.Windows.Forms.Label lblDstStaidx;
        private System.Windows.Forms.TextBox txtDstStaidx;
        private System.Windows.Forms.Button btnBrowseDstStaidx;
        private System.Windows.Forms.Label lblDstStatics;
        private System.Windows.Forms.TextBox txtDstStatics;
        private System.Windows.Forms.Button btnBrowseDstStatics;
        private System.Windows.Forms.TableLayoutPanel tableInfo;
        private System.Windows.Forms.GroupBox grpSourceInfo;
        private System.Windows.Forms.Label lblSourceProfile;
        private System.Windows.Forms.GroupBox grpDestinationInfo;
        private System.Windows.Forms.Label lblDestinationProfile;
        private System.Windows.Forms.TableLayoutPanel tableCoords;
        private System.Windows.Forms.GroupBox grpSourceRect;
        private System.Windows.Forms.TableLayoutPanel tableSourceRect;
        private System.Windows.Forms.Label lblX1;
        private System.Windows.Forms.TextBox txtSX1;
        private System.Windows.Forms.Label lblY1;
        private System.Windows.Forms.TextBox txtSY1;
        private System.Windows.Forms.Label lblX2;
        private System.Windows.Forms.TextBox txtSX2;
        private System.Windows.Forms.Label lblY2;
        private System.Windows.Forms.TextBox txtSY2;
        private System.Windows.Forms.GroupBox grpDestinationAnchor;
        private System.Windows.Forms.TableLayoutPanel tableDestinationAnchor;
        private System.Windows.Forms.Label lblDestX;
        private System.Windows.Forms.TextBox txtDX;
        private System.Windows.Forms.Label lblDestY;
        private System.Windows.Forms.TextBox txtDY;
        private System.Windows.Forms.GroupBox grpCopyMode;
        private System.Windows.Forms.FlowLayoutPanel flowCopyMode;
        private System.Windows.Forms.RadioButton rbKeepZ;
        private System.Windows.Forms.RadioButton rbRandomZ;
        private System.Windows.Forms.RadioButton rbAddRandomZ;
        private System.Windows.Forms.Label lblZ1;
        private System.Windows.Forms.NumericUpDown numZ1;
        private System.Windows.Forms.Label lblZ2;
        private System.Windows.Forms.NumericUpDown numZ2;
        private System.Windows.Forms.FlowLayoutPanel flowOptions;
        private System.Windows.Forms.CheckBox chkCopyMap;
        private System.Windows.Forms.CheckBox chkCopyStatics;
        private System.Windows.Forms.Label lblSkipPreset;
        private System.Windows.Forms.ComboBox cmbSkipPreset;
        private System.Windows.Forms.TableLayoutPanel tableCopyOptions;
        private System.Windows.Forms.GroupBox grpActions;
        private System.Windows.Forms.TableLayoutPanel tableActions;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnCreateEmptyMap;
        private System.Windows.Forms.Button btnExtendToMl;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}

