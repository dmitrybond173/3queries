namespace WmiQuery
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tbExecute = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tbLockSearcher = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tbSnippets = new System.Windows.Forms.ToolStripButton();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mmiSaveResultsetToXML = new System.Windows.Forms.ToolStripMenuItem();
            this.mmiSaveResultsetToCSV = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.mmiExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuQuery = new System.Windows.Forms.ToolStripMenuItem();
            this.mmiExecute = new System.Windows.Forms.ToolStripMenuItem();
            this.mmiLockWMISearcher = new System.Windows.Forms.ToolStripMenuItem();
            this.miExpandEnvironmentVariables = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.mmiInsertDefaultParameters = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.snippetsManagerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mmiHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuUsefulLinks = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.mmiAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.stbarMain = new System.Windows.Forms.StatusStrip();
            this.stLab1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.stLab2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.stLab3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.dgvResultset = new System.Windows.Forms.DataGridView();
            this.pmnuDataGrigView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pmiCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.pmiCopyRow = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.pmiClear = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.pmiResizeColumns = new System.Windows.Forms.ToolStripMenuItem();
            this.pmiResizeByHeader = new System.Windows.Forms.ToolStripMenuItem();
            this.pmiResizeByValue = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.pmiSaveAsXml = new System.Windows.Forms.ToolStripMenuItem();
            this.bsData = new System.Windows.Forms.BindingSource(this.components);
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panQuery = new System.Windows.Forms.Panel();
            this.pgcQuery = new System.Windows.Forms.TabControl();
            this.tabQuery = new System.Windows.Forms.TabPage();
            this.txtQuery = new System.Windows.Forms.TextBox();
            this.tabParams = new System.Windows.Forms.TabPage();
            this.txtParams = new System.Windows.Forms.TextBox();
            this.panLogger = new System.Windows.Forms.Panel();
            this.lvLogger = new System.Windows.Forms.ListView();
            this.chTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chEvent = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pmnuLogger = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pmiCopyAll = new System.Windows.Forms.ToolStripMenuItem();
            this.pmiCopyLine = new System.Windows.Forms.ToolStripMenuItem();
            this.dlgSave = new System.Windows.Forms.SaveFileDialog();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.dlgSaveCsv = new System.Windows.Forms.SaveFileDialog();
            this.toolStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.stbarMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResultset)).BeginInit();
            this.pmnuDataGrigView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsData)).BeginInit();
            this.panQuery.SuspendLayout();
            this.pgcQuery.SuspendLayout();
            this.tabQuery.SuspendLayout();
            this.tabParams.SuspendLayout();
            this.panLogger.SuspendLayout();
            this.pmnuLogger.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbExecute,
            this.toolStripSeparator1,
            this.tbLockSearcher,
            this.toolStripSeparator2,
            this.tbSnippets});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(784, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tbExecute
            // 
            this.tbExecute.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbExecute.Image = ((System.Drawing.Image)(resources.GetObject("tbExecute.Image")));
            this.tbExecute.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbExecute.Name = "tbExecute";
            this.tbExecute.Size = new System.Drawing.Size(23, 22);
            this.tbExecute.Text = "toolStripButton1";
            this.tbExecute.ToolTipText = "Execute WMI query";
            this.tbExecute.Click += new System.EventHandler(this.tbExecute_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tbLockSearcher
            // 
            this.tbLockSearcher.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbLockSearcher.Image = ((System.Drawing.Image)(resources.GetObject("tbLockSearcher.Image")));
            this.tbLockSearcher.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbLockSearcher.Name = "tbLockSearcher";
            this.tbLockSearcher.Size = new System.Drawing.Size(23, 22);
            this.tbLockSearcher.Text = "Lock WMI Searcher";
            this.tbLockSearcher.ToolTipText = "Lock WMI Searcher";
            this.tbLockSearcher.Click += new System.EventHandler(this.tbLockSearcher_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tbSnippets
            // 
            this.tbSnippets.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbSnippets.Image = ((System.Drawing.Image)(resources.GetObject("tbSnippets.Image")));
            this.tbSnippets.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbSnippets.Name = "tbSnippets";
            this.tbSnippets.Size = new System.Drawing.Size(23, 22);
            this.tbSnippets.Text = "toolStripButton1";
            this.tbSnippets.Click += new System.EventHandler(this.tbSnippets_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuQuery,
            this.mnuHelp});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(784, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mmiSaveResultsetToXML,
            this.mmiSaveResultsetToCSV,
            this.toolStripMenuItem7,
            this.mmiExit});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(37, 20);
            this.mnuFile.Text = "&File";
            // 
            // mmiSaveResultsetToXML
            // 
            this.mmiSaveResultsetToXML.Name = "mmiSaveResultsetToXML";
            this.mmiSaveResultsetToXML.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.mmiSaveResultsetToXML.Size = new System.Drawing.Size(270, 22);
            this.mmiSaveResultsetToXML.Text = "Save Resultset to XML...";
            this.mmiSaveResultsetToXML.Click += new System.EventHandler(this.mmiSaveResultsetToXML_Click);
            // 
            // mmiSaveResultsetToCSV
            // 
            this.mmiSaveResultsetToCSV.Name = "mmiSaveResultsetToCSV";
            this.mmiSaveResultsetToCSV.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.mmiSaveResultsetToCSV.Size = new System.Drawing.Size(270, 22);
            this.mmiSaveResultsetToCSV.Text = "Save Resultset to CSV...";
            this.mmiSaveResultsetToCSV.Click += new System.EventHandler(this.mmiSaveResultsetToCSV_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(267, 6);
            // 
            // mmiExit
            // 
            this.mmiExit.Name = "mmiExit";
            this.mmiExit.Size = new System.Drawing.Size(270, 22);
            this.mmiExit.Text = "E&xit";
            this.mmiExit.Click += new System.EventHandler(this.mmiExit_Click);
            // 
            // mnuQuery
            // 
            this.mnuQuery.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mmiExecute,
            this.mmiLockWMISearcher,
            this.miExpandEnvironmentVariables,
            this.toolStripMenuItem4,
            this.mmiInsertDefaultParameters,
            this.toolStripMenuItem5,
            this.snippetsManagerToolStripMenuItem});
            this.mnuQuery.Name = "mnuQuery";
            this.mnuQuery.Size = new System.Drawing.Size(51, 20);
            this.mnuQuery.Text = "&Query";
            // 
            // mmiExecute
            // 
            this.mmiExecute.Name = "mmiExecute";
            this.mmiExecute.ShortcutKeys = System.Windows.Forms.Keys.F9;
            this.mmiExecute.Size = new System.Drawing.Size(233, 22);
            this.mmiExecute.Text = "&Execute";
            this.mmiExecute.Click += new System.EventHandler(this.mmiExecute_Click);
            // 
            // mmiLockWMISearcher
            // 
            this.mmiLockWMISearcher.Name = "mmiLockWMISearcher";
            this.mmiLockWMISearcher.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.mmiLockWMISearcher.Size = new System.Drawing.Size(233, 22);
            this.mmiLockWMISearcher.Text = "&Lock WMI Searcher";
            this.mmiLockWMISearcher.Click += new System.EventHandler(this.mmiLockWMISearcher_Click);
            // 
            // miExpandEnvironmentVariables
            // 
            this.miExpandEnvironmentVariables.CheckOnClick = true;
            this.miExpandEnvironmentVariables.Name = "miExpandEnvironmentVariables";
            this.miExpandEnvironmentVariables.Size = new System.Drawing.Size(233, 22);
            this.miExpandEnvironmentVariables.Text = "Expand Environment Variables";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(230, 6);
            // 
            // mmiInsertDefaultParameters
            // 
            this.mmiInsertDefaultParameters.Name = "mmiInsertDefaultParameters";
            this.mmiInsertDefaultParameters.Size = new System.Drawing.Size(233, 22);
            this.mmiInsertDefaultParameters.Text = "Insert Default Parameters";
            this.mmiInsertDefaultParameters.Click += new System.EventHandler(this.mmiInsertDefaultParameters_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(230, 6);
            // 
            // snippetsManagerToolStripMenuItem
            // 
            this.snippetsManagerToolStripMenuItem.Name = "snippetsManagerToolStripMenuItem";
            this.snippetsManagerToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F8)));
            this.snippetsManagerToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.snippetsManagerToolStripMenuItem.Text = "Snippets &Manager...";
            this.snippetsManagerToolStripMenuItem.Click += new System.EventHandler(this.tbSnippets_Click);
            // 
            // mnuHelp
            // 
            this.mnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mmiHelp,
            this.mnuUsefulLinks,
            this.toolStripMenuItem6,
            this.mmiAbout});
            this.mnuHelp.Name = "mnuHelp";
            this.mnuHelp.Size = new System.Drawing.Size(44, 20);
            this.mnuHelp.Text = "&Help";
            // 
            // mmiHelp
            // 
            this.mmiHelp.Name = "mmiHelp";
            this.mmiHelp.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.mmiHelp.Size = new System.Drawing.Size(137, 22);
            this.mmiHelp.Text = "&Help";
            this.mmiHelp.Click += new System.EventHandler(this.mmiHelp_Click);
            // 
            // mnuUsefulLinks
            // 
            this.mnuUsefulLinks.Name = "mnuUsefulLinks";
            this.mnuUsefulLinks.Size = new System.Drawing.Size(137, 22);
            this.mnuUsefulLinks.Text = "Useful Links";
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(134, 6);
            // 
            // mmiAbout
            // 
            this.mmiAbout.Name = "mmiAbout";
            this.mmiAbout.Size = new System.Drawing.Size(137, 22);
            this.mmiAbout.Text = "&About...";
            this.mmiAbout.Click += new System.EventHandler(this.mmiAbout_Click);
            // 
            // stbarMain
            // 
            this.stbarMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stLab1,
            this.stLab2,
            this.stLab3});
            this.stbarMain.Location = new System.Drawing.Point(0, 530);
            this.stbarMain.Name = "stbarMain";
            this.stbarMain.Size = new System.Drawing.Size(784, 22);
            this.stbarMain.TabIndex = 2;
            this.stbarMain.Text = "statusStrip1";
            // 
            // stLab1
            // 
            this.stLab1.AutoSize = false;
            this.stLab1.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.stLab1.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.stLab1.Name = "stLab1";
            this.stLab1.Size = new System.Drawing.Size(118, 17);
            this.stLab1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // stLab2
            // 
            this.stLab2.AutoSize = false;
            this.stLab2.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.stLab2.Name = "stLab2";
            this.stLab2.Size = new System.Drawing.Size(110, 17);
            // 
            // stLab3
            // 
            this.stLab3.AutoSize = false;
            this.stLab3.Name = "stLab3";
            this.stLab3.Size = new System.Drawing.Size(480, 17);
            this.stLab3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvResultset
            // 
            this.dgvResultset.AllowUserToAddRows = false;
            this.dgvResultset.AllowUserToDeleteRows = false;
            this.dgvResultset.AllowUserToOrderColumns = true;
            this.dgvResultset.AutoGenerateColumns = false;
            this.dgvResultset.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvResultset.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgvResultset.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResultset.ContextMenuStrip = this.pmnuDataGrigView;
            this.dgvResultset.DataSource = this.bsData;
            this.dgvResultset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvResultset.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvResultset.Location = new System.Drawing.Point(0, 198);
            this.dgvResultset.MinimumSize = new System.Drawing.Size(320, 100);
            this.dgvResultset.Name = "dgvResultset";
            this.dgvResultset.ReadOnly = true;
            this.dgvResultset.Size = new System.Drawing.Size(784, 229);
            this.dgvResultset.TabIndex = 3;
            this.dgvResultset.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvResultset_DataError);
            this.dgvResultset.SelectionChanged += new System.EventHandler(this.dgvResultset_SelectionChanged);
            // 
            // pmnuDataGrigView
            // 
            this.pmnuDataGrigView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pmiCopy,
            this.pmiCopyRow,
            this.toolStripMenuItem1,
            this.pmiClear,
            this.toolStripMenuItem2,
            this.pmiResizeColumns,
            this.toolStripMenuItem3,
            this.pmiSaveAsXml});
            this.pmnuDataGrigView.Name = "pmnuDataGrigView";
            this.pmnuDataGrigView.Size = new System.Drawing.Size(181, 132);
            // 
            // pmiCopy
            // 
            this.pmiCopy.Name = "pmiCopy";
            this.pmiCopy.Size = new System.Drawing.Size(180, 22);
            this.pmiCopy.Text = "&Copy";
            this.pmiCopy.Click += new System.EventHandler(this.pmiCopy_Click);
            // 
            // pmiCopyRow
            // 
            this.pmiCopyRow.Name = "pmiCopyRow";
            this.pmiCopyRow.Size = new System.Drawing.Size(180, 22);
            this.pmiCopyRow.Text = "Copy Selected &Rows";
            this.pmiCopyRow.Click += new System.EventHandler(this.pmiCopyRow_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(177, 6);
            // 
            // pmiClear
            // 
            this.pmiClear.Name = "pmiClear";
            this.pmiClear.Size = new System.Drawing.Size(180, 22);
            this.pmiClear.Text = "&Clear";
            this.pmiClear.Click += new System.EventHandler(this.pmiClear_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(177, 6);
            // 
            // pmiResizeColumns
            // 
            this.pmiResizeColumns.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pmiResizeByHeader,
            this.pmiResizeByValue});
            this.pmiResizeColumns.Name = "pmiResizeColumns";
            this.pmiResizeColumns.Size = new System.Drawing.Size(180, 22);
            this.pmiResizeColumns.Text = "Resi&ze Columns";
            // 
            // pmiResizeByHeader
            // 
            this.pmiResizeByHeader.Name = "pmiResizeByHeader";
            this.pmiResizeByHeader.Size = new System.Drawing.Size(128, 22);
            this.pmiResizeByHeader.Text = "By &Header";
            this.pmiResizeByHeader.Click += new System.EventHandler(this.pmiResizeByHeader_Click);
            // 
            // pmiResizeByValue
            // 
            this.pmiResizeByValue.Name = "pmiResizeByValue";
            this.pmiResizeByValue.Size = new System.Drawing.Size(128, 22);
            this.pmiResizeByValue.Text = "By &Value";
            this.pmiResizeByValue.Click += new System.EventHandler(this.pmiResizeByValue_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(177, 6);
            // 
            // pmiSaveAsXml
            // 
            this.pmiSaveAsXml.Name = "pmiSaveAsXml";
            this.pmiSaveAsXml.Size = new System.Drawing.Size(180, 22);
            this.pmiSaveAsXml.Text = "Save as XML...";
            this.pmiSaveAsXml.Click += new System.EventHandler(this.pmiSaveAsXml_Click);
            // 
            // bsData
            // 
            this.bsData.AllowNew = true;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(0, 427);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(784, 3);
            this.splitter1.TabIndex = 4;
            this.splitter1.TabStop = false;
            // 
            // panQuery
            // 
            this.panQuery.Controls.Add(this.pgcQuery);
            this.panQuery.Dock = System.Windows.Forms.DockStyle.Top;
            this.panQuery.Location = new System.Drawing.Point(0, 49);
            this.panQuery.MinimumSize = new System.Drawing.Size(320, 100);
            this.panQuery.Name = "panQuery";
            this.panQuery.Size = new System.Drawing.Size(784, 149);
            this.panQuery.TabIndex = 5;
            // 
            // pgcQuery
            // 
            this.pgcQuery.Controls.Add(this.tabQuery);
            this.pgcQuery.Controls.Add(this.tabParams);
            this.pgcQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgcQuery.Location = new System.Drawing.Point(0, 0);
            this.pgcQuery.Name = "pgcQuery";
            this.pgcQuery.SelectedIndex = 0;
            this.pgcQuery.Size = new System.Drawing.Size(784, 149);
            this.pgcQuery.TabIndex = 0;
            // 
            // tabQuery
            // 
            this.tabQuery.Controls.Add(this.txtQuery);
            this.tabQuery.Location = new System.Drawing.Point(4, 22);
            this.tabQuery.Name = "tabQuery";
            this.tabQuery.Padding = new System.Windows.Forms.Padding(3);
            this.tabQuery.Size = new System.Drawing.Size(776, 123);
            this.tabQuery.TabIndex = 0;
            this.tabQuery.Text = "Query";
            this.tabQuery.UseVisualStyleBackColor = true;
            // 
            // txtQuery
            // 
            this.txtQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtQuery.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtQuery.Location = new System.Drawing.Point(3, 3);
            this.txtQuery.Multiline = true;
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtQuery.Size = new System.Drawing.Size(770, 117);
            this.txtQuery.TabIndex = 0;
            this.txtQuery.WordWrap = false;
            // 
            // tabParams
            // 
            this.tabParams.Controls.Add(this.txtParams);
            this.tabParams.Location = new System.Drawing.Point(4, 22);
            this.tabParams.Name = "tabParams";
            this.tabParams.Padding = new System.Windows.Forms.Padding(3);
            this.tabParams.Size = new System.Drawing.Size(776, 123);
            this.tabParams.TabIndex = 1;
            this.tabParams.Text = "Parameters";
            this.tabParams.UseVisualStyleBackColor = true;
            // 
            // txtParams
            // 
            this.txtParams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtParams.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtParams.Location = new System.Drawing.Point(3, 3);
            this.txtParams.Multiline = true;
            this.txtParams.Name = "txtParams";
            this.txtParams.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtParams.Size = new System.Drawing.Size(770, 117);
            this.txtParams.TabIndex = 1;
            this.txtParams.WordWrap = false;
            this.txtParams.TextChanged += new System.EventHandler(this.txtParams_TextChanged);
            // 
            // panLogger
            // 
            this.panLogger.Controls.Add(this.lvLogger);
            this.panLogger.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panLogger.Location = new System.Drawing.Point(0, 430);
            this.panLogger.Name = "panLogger";
            this.panLogger.Size = new System.Drawing.Size(784, 100);
            this.panLogger.TabIndex = 6;
            // 
            // lvLogger
            // 
            this.lvLogger.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvLogger.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chTime,
            this.chEvent});
            this.lvLogger.ContextMenuStrip = this.pmnuLogger;
            this.lvLogger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvLogger.FullRowSelect = true;
            this.lvLogger.Location = new System.Drawing.Point(0, 0);
            this.lvLogger.MinimumSize = new System.Drawing.Size(320, 100);
            this.lvLogger.MultiSelect = false;
            this.lvLogger.Name = "lvLogger";
            this.lvLogger.Size = new System.Drawing.Size(784, 100);
            this.lvLogger.TabIndex = 0;
            this.lvLogger.UseCompatibleStateImageBehavior = false;
            this.lvLogger.View = System.Windows.Forms.View.Details;
            // 
            // chTime
            // 
            this.chTime.Text = "Time";
            this.chTime.Width = 82;
            // 
            // chEvent
            // 
            this.chEvent.Text = "Event";
            this.chEvent.Width = 600;
            // 
            // pmnuLogger
            // 
            this.pmnuLogger.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pmiCopyAll,
            this.pmiCopyLine});
            this.pmnuLogger.Name = "pmnuLogger";
            this.pmnuLogger.Size = new System.Drawing.Size(128, 48);
            // 
            // pmiCopyAll
            // 
            this.pmiCopyAll.Name = "pmiCopyAll";
            this.pmiCopyAll.Size = new System.Drawing.Size(127, 22);
            this.pmiCopyAll.Text = "Copy &All";
            this.pmiCopyAll.Click += new System.EventHandler(this.pmiCopyAll_Click);
            // 
            // pmiCopyLine
            // 
            this.pmiCopyLine.Name = "pmiCopyLine";
            this.pmiCopyLine.Size = new System.Drawing.Size(127, 22);
            this.pmiCopyLine.Text = "Copy &Line";
            this.pmiCopyLine.Click += new System.EventHandler(this.pmiCopyLine_Click);
            // 
            // dlgSave
            // 
            this.dlgSave.DefaultExt = "xml";
            this.dlgSave.Filter = "XML Files|*.xml|All Files|*.*";
            this.dlgSave.InitialDirectory = ".";
            this.dlgSave.Title = "Save Data XML";
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter2.Location = new System.Drawing.Point(0, 198);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(784, 3);
            this.splitter2.TabIndex = 8;
            this.splitter2.TabStop = false;
            // 
            // dlgSaveCsv
            // 
            this.dlgSaveCsv.DefaultExt = "csv";
            this.dlgSaveCsv.Filter = "CSV Files|*.csv|Text files|*.txt|All Files|*.*";
            this.dlgSaveCsv.InitialDirectory = ".";
            this.dlgSaveCsv.Title = "Save Data CSV";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 552);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.dgvResultset);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panLogger);
            this.Controls.Add(this.panQuery);
            this.Controls.Add(this.stbarMain);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.Text = "WMI Query";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.Shown += new System.EventHandler(this.FormMain_Shown);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.stbarMain.ResumeLayout(false);
            this.stbarMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResultset)).EndInit();
            this.pmnuDataGrigView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsData)).EndInit();
            this.panQuery.ResumeLayout(false);
            this.pgcQuery.ResumeLayout(false);
            this.tabQuery.ResumeLayout(false);
            this.tabQuery.PerformLayout();
            this.tabParams.ResumeLayout(false);
            this.tabParams.PerformLayout();
            this.panLogger.ResumeLayout(false);
            this.pmnuLogger.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem mmiExit;
        private System.Windows.Forms.StatusStrip stbarMain;
        private System.Windows.Forms.ToolStripStatusLabel stLab1;
        private System.Windows.Forms.DataGridView dgvResultset;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panQuery;
        private System.Windows.Forms.TabControl pgcQuery;
        private System.Windows.Forms.TabPage tabQuery;
        private System.Windows.Forms.TabPage tabParams;
        private System.Windows.Forms.TextBox txtQuery;
        private System.Windows.Forms.TextBox txtParams;
        private System.Windows.Forms.ToolStripButton tbExecute;
        private System.Windows.Forms.ToolStripMenuItem mnuQuery;
        private System.Windows.Forms.ToolStripMenuItem mmiExecute;
        private System.Windows.Forms.Panel panLogger;
        private System.Windows.Forms.ListView lvLogger;
        private System.Windows.Forms.ColumnHeader chTime;
        private System.Windows.Forms.ColumnHeader chEvent;
        private System.Windows.Forms.ToolStripMenuItem mnuHelp;
        private System.Windows.Forms.ToolStripMenuItem mmiAbout;
        private System.Windows.Forms.ContextMenuStrip pmnuDataGrigView;
        private System.Windows.Forms.ToolStripMenuItem pmiCopy;
        private System.Windows.Forms.ToolStripMenuItem pmiCopyRow;
        private System.Windows.Forms.SaveFileDialog dlgSave;
        private System.Windows.Forms.ToolStripMenuItem pmiSaveAsXml;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.BindingSource bsData;
        private System.Windows.Forms.ToolStripMenuItem pmiClear;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem pmiResizeColumns;
        private System.Windows.Forms.ToolStripMenuItem pmiResizeByValue;
        private System.Windows.Forms.ToolStripMenuItem pmiResizeByHeader;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ContextMenuStrip pmnuLogger;
        private System.Windows.Forms.ToolStripMenuItem pmiCopyAll;
        private System.Windows.Forms.ToolStripMenuItem pmiCopyLine;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem mmiInsertDefaultParameters;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.ToolStripStatusLabel stLab3;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem mnuUsefulLinks;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem mmiHelp;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tbLockSearcher;
        private System.Windows.Forms.ToolStripMenuItem mmiLockWMISearcher;
        private System.Windows.Forms.ToolStripStatusLabel stLab2;
        private System.Windows.Forms.ToolStripMenuItem mmiSaveResultsetToXML;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem miExpandEnvironmentVariables;
        private System.Windows.Forms.ToolStripMenuItem mmiSaveResultsetToCSV;
        private System.Windows.Forms.SaveFileDialog dlgSaveCsv;
        private System.Windows.Forms.ToolStripMenuItem snippetsManagerToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tbSnippets;
    }
}

