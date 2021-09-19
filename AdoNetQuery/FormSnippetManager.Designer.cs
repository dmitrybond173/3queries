namespace AdoNetQuery
{
    partial class FormSnippetManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSnippetManager));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.miStructure = new System.Windows.Forms.ToolStripMenuItem();
            this.miNewFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.miNewSnippet = new System.Windows.Forms.ToolStripMenuItem();
            this.miRenameSelectedItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miRemoveSelectedItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.miApplySnippet = new System.Windows.Forms.ToolStripMenuItem();
            this.miApplyWholeSnippet = new System.Windows.Forms.ToolStripMenuItem();
            this.miApplyOnlyQuery = new System.Windows.Forms.ToolStripMenuItem();
            this.miApplyOnlyParameters = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.miSaveChanges = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tbApply = new System.Windows.Forms.ToolStripSplitButton();
            this.tmiApplyWholeSnippet = new System.Windows.Forms.ToolStripMenuItem();
            this.tmiApplyOnlyQuery = new System.Windows.Forms.ToolStripMenuItem();
            this.tmiApplyOnlyParameters = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tbNewItem = new System.Windows.Forms.ToolStripSplitButton();
            this.tmiNewFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.tmiNewSnippet = new System.Windows.Forms.ToolStripMenuItem();
            this.tbRemoveItem = new System.Windows.Forms.ToolStripButton();
            this.tbRename = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tbSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tbPasteCurrent = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tvSnippets = new System.Windows.Forms.TreeView();
            this.cmnuTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiExpandAll = new System.Windows.Forms.ToolStripMenuItem();
            this.imglTree = new System.Windows.Forms.ImageList(this.components);
            this.labTree = new System.Windows.Forms.Label();
            this.panSnippet = new System.Windows.Forms.Panel();
            this.labSnippetCaption = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.cmnuTree.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miStructure});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(703, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // miStructure
            // 
            this.miStructure.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miNewFolder,
            this.miNewSnippet,
            this.miRenameSelectedItem,
            this.miRemoveSelectedItem,
            this.toolStripMenuItem1,
            this.miApplySnippet,
            this.toolStripMenuItem2,
            this.miSaveChanges});
            this.miStructure.Name = "miStructure";
            this.miStructure.Size = new System.Drawing.Size(67, 20);
            this.miStructure.Text = "&Structure";
            // 
            // miNewFolder
            // 
            this.miNewFolder.Name = "miNewFolder";
            this.miNewFolder.Size = new System.Drawing.Size(191, 22);
            this.miNewFolder.Text = "New &Folder...";
            this.miNewFolder.Click += new System.EventHandler(this.tmiNewFolder_Click);
            // 
            // miNewSnippet
            // 
            this.miNewSnippet.Name = "miNewSnippet";
            this.miNewSnippet.Size = new System.Drawing.Size(191, 22);
            this.miNewSnippet.Text = "New &Snippet...";
            this.miNewSnippet.Click += new System.EventHandler(this.tmiNewSnippet_Click);
            // 
            // miRenameSelectedItem
            // 
            this.miRenameSelectedItem.Name = "miRenameSelectedItem";
            this.miRenameSelectedItem.Size = new System.Drawing.Size(191, 22);
            this.miRenameSelectedItem.Text = "Re&name Selected Item";
            this.miRenameSelectedItem.Click += new System.EventHandler(this.tbRename_Click);
            // 
            // miRemoveSelectedItem
            // 
            this.miRemoveSelectedItem.Name = "miRemoveSelectedItem";
            this.miRemoveSelectedItem.Size = new System.Drawing.Size(191, 22);
            this.miRemoveSelectedItem.Text = "&Remove Selected Item";
            this.miRemoveSelectedItem.Click += new System.EventHandler(this.tbRemoveItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(188, 6);
            // 
            // miApplySnippet
            // 
            this.miApplySnippet.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miApplyWholeSnippet,
            this.miApplyOnlyQuery,
            this.miApplyOnlyParameters});
            this.miApplySnippet.Name = "miApplySnippet";
            this.miApplySnippet.Size = new System.Drawing.Size(191, 22);
            this.miApplySnippet.Text = "&Apply Snippet";
            // 
            // miApplyWholeSnippet
            // 
            this.miApplyWholeSnippet.Name = "miApplyWholeSnippet";
            this.miApplyWholeSnippet.Size = new System.Drawing.Size(195, 22);
            this.miApplyWholeSnippet.Text = "Apply &Whole Snippet";
            this.miApplyWholeSnippet.Click += new System.EventHandler(this.tmiApplyWholeSnippet_Click);
            // 
            // miApplyOnlyQuery
            // 
            this.miApplyOnlyQuery.Name = "miApplyOnlyQuery";
            this.miApplyOnlyQuery.Size = new System.Drawing.Size(195, 22);
            this.miApplyOnlyQuery.Text = "Apply Only &Query";
            this.miApplyOnlyQuery.Click += new System.EventHandler(this.tmiApplyOnlyQuery_Click);
            // 
            // miApplyOnlyParameters
            // 
            this.miApplyOnlyParameters.Name = "miApplyOnlyParameters";
            this.miApplyOnlyParameters.Size = new System.Drawing.Size(195, 22);
            this.miApplyOnlyParameters.Text = "Apply Only &Parameters";
            this.miApplyOnlyParameters.Click += new System.EventHandler(this.tmiApplyOnlyParameters_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(188, 6);
            // 
            // miSaveChanges
            // 
            this.miSaveChanges.Name = "miSaveChanges";
            this.miSaveChanges.Size = new System.Drawing.Size(191, 22);
            this.miSaveChanges.Text = "Save Changes";
            this.miSaveChanges.Click += new System.EventHandler(this.tbSave_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbApply,
            this.toolStripSeparator1,
            this.tbNewItem,
            this.tbRemoveItem,
            this.tbRename,
            this.toolStripSeparator2,
            this.tbSave,
            this.toolStripSeparator3,
            this.tbPasteCurrent});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(703, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tbApply
            // 
            this.tbApply.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbApply.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tmiApplyWholeSnippet,
            this.tmiApplyOnlyQuery,
            this.tmiApplyOnlyParameters});
            this.tbApply.Image = ((System.Drawing.Image)(resources.GetObject("tbApply.Image")));
            this.tbApply.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbApply.Name = "tbApply";
            this.tbApply.Size = new System.Drawing.Size(32, 22);
            this.tbApply.Text = "Apply Snippet";
            this.tbApply.ToolTipText = "Apply Snippet (Insert Selected Snippet into Query UI)";
            this.tbApply.ButtonClick += new System.EventHandler(this.tbApply_ButtonClick);
            // 
            // tmiApplyWholeSnippet
            // 
            this.tmiApplyWholeSnippet.Name = "tmiApplyWholeSnippet";
            this.tmiApplyWholeSnippet.Size = new System.Drawing.Size(195, 22);
            this.tmiApplyWholeSnippet.Text = "Apply Whole Snippet";
            this.tmiApplyWholeSnippet.Click += new System.EventHandler(this.tmiApplyWholeSnippet_Click);
            // 
            // tmiApplyOnlyQuery
            // 
            this.tmiApplyOnlyQuery.Name = "tmiApplyOnlyQuery";
            this.tmiApplyOnlyQuery.Size = new System.Drawing.Size(195, 22);
            this.tmiApplyOnlyQuery.Text = "Apply Only Query";
            this.tmiApplyOnlyQuery.Click += new System.EventHandler(this.tmiApplyOnlyQuery_Click);
            // 
            // tmiApplyOnlyParameters
            // 
            this.tmiApplyOnlyParameters.Name = "tmiApplyOnlyParameters";
            this.tmiApplyOnlyParameters.Size = new System.Drawing.Size(195, 22);
            this.tmiApplyOnlyParameters.Text = "Apply Only Parameters";
            this.tmiApplyOnlyParameters.Click += new System.EventHandler(this.tmiApplyOnlyParameters_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tbNewItem
            // 
            this.tbNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbNewItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tmiNewFolder,
            this.tmiNewSnippet});
            this.tbNewItem.Image = ((System.Drawing.Image)(resources.GetObject("tbNewItem.Image")));
            this.tbNewItem.ImageTransparentColor = System.Drawing.Color.White;
            this.tbNewItem.Name = "tbNewItem";
            this.tbNewItem.Size = new System.Drawing.Size(32, 22);
            this.tbNewItem.Text = "New Item...";
            this.tbNewItem.ButtonClick += new System.EventHandler(this.tbNewItem_ButtonClick);
            // 
            // tmiNewFolder
            // 
            this.tmiNewFolder.Image = ((System.Drawing.Image)(resources.GetObject("tmiNewFolder.Image")));
            this.tmiNewFolder.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.tmiNewFolder.Name = "tmiNewFolder";
            this.tmiNewFolder.Size = new System.Drawing.Size(143, 22);
            this.tmiNewFolder.Text = "New &Folder...";
            this.tmiNewFolder.Click += new System.EventHandler(this.tmiNewFolder_Click);
            // 
            // tmiNewSnippet
            // 
            this.tmiNewSnippet.Image = ((System.Drawing.Image)(resources.GetObject("tmiNewSnippet.Image")));
            this.tmiNewSnippet.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.tmiNewSnippet.Name = "tmiNewSnippet";
            this.tmiNewSnippet.Size = new System.Drawing.Size(143, 22);
            this.tmiNewSnippet.Text = "New &Snippet";
            this.tmiNewSnippet.Click += new System.EventHandler(this.tmiNewSnippet_Click);
            // 
            // tbRemoveItem
            // 
            this.tbRemoveItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbRemoveItem.Image = ((System.Drawing.Image)(resources.GetObject("tbRemoveItem.Image")));
            this.tbRemoveItem.ImageTransparentColor = System.Drawing.Color.White;
            this.tbRemoveItem.Name = "tbRemoveItem";
            this.tbRemoveItem.Size = new System.Drawing.Size(23, 22);
            this.tbRemoveItem.Text = "Remove Item";
            this.tbRemoveItem.Click += new System.EventHandler(this.tbRemoveItem_Click);
            // 
            // tbRename
            // 
            this.tbRename.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbRename.Image = ((System.Drawing.Image)(resources.GetObject("tbRename.Image")));
            this.tbRename.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbRename.Name = "tbRename";
            this.tbRename.Size = new System.Drawing.Size(23, 22);
            this.tbRename.Text = "Rename Item";
            this.tbRename.Click += new System.EventHandler(this.tbRename_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tbSave
            // 
            this.tbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbSave.Image = ((System.Drawing.Image)(resources.GetObject("tbSave.Image")));
            this.tbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbSave.Name = "tbSave";
            this.tbSave.Size = new System.Drawing.Size(23, 22);
            this.tbSave.Text = "Save";
            this.tbSave.ToolTipText = "Save changes";
            this.tbSave.Click += new System.EventHandler(this.tbSave_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tbPasteCurrent
            // 
            this.tbPasteCurrent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbPasteCurrent.Image = ((System.Drawing.Image)(resources.GetObject("tbPasteCurrent.Image")));
            this.tbPasteCurrent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbPasteCurrent.Name = "tbPasteCurrent";
            this.tbPasteCurrent.Size = new System.Drawing.Size(23, 22);
            this.tbPasteCurrent.Text = "Paste Current Query and Params info Selected Snippet";
            this.tbPasteCurrent.Click += new System.EventHandler(this.tbPasteCurrent_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 480);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(703, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 49);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tvSnippets);
            this.splitContainer1.Panel1.Controls.Add(this.labTree);
            this.splitContainer1.Panel1MinSize = 140;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panSnippet);
            this.splitContainer1.Panel2.Controls.Add(this.labSnippetCaption);
            this.splitContainer1.Panel2MinSize = 0;
            this.splitContainer1.Size = new System.Drawing.Size(703, 431);
            this.splitContainer1.SplitterDistance = 220;
            this.splitContainer1.TabIndex = 3;
            // 
            // tvSnippets
            // 
            this.tvSnippets.ContextMenuStrip = this.cmnuTree;
            this.tvSnippets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvSnippets.ImageIndex = 0;
            this.tvSnippets.ImageList = this.imglTree;
            this.tvSnippets.Location = new System.Drawing.Point(0, 13);
            this.tvSnippets.MinimumSize = new System.Drawing.Size(160, 240);
            this.tvSnippets.Name = "tvSnippets";
            this.tvSnippets.SelectedImageIndex = 0;
            this.tvSnippets.Size = new System.Drawing.Size(220, 418);
            this.tvSnippets.TabIndex = 0;
            this.tvSnippets.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvSnippets_AfterSelect);
            // 
            // cmnuTree
            // 
            this.cmnuTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiExpandAll});
            this.cmnuTree.Name = "cmnuTree";
            this.cmnuTree.Size = new System.Drawing.Size(130, 26);
            // 
            // cmiExpandAll
            // 
            this.cmiExpandAll.Name = "cmiExpandAll";
            this.cmiExpandAll.Size = new System.Drawing.Size(129, 22);
            this.cmiExpandAll.Text = "&Expand All";
            this.cmiExpandAll.Click += new System.EventHandler(this.cmiExpandAll_Click);
            // 
            // imglTree
            // 
            this.imglTree.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imglTree.ImageStream")));
            this.imglTree.TransparentColor = System.Drawing.Color.Fuchsia;
            this.imglTree.Images.SetKeyName(0, "folder1.bmp");
            this.imglTree.Images.SetKeyName(1, "reg-type-text.bmp");
            this.imglTree.Images.SetKeyName(2, "tree-bimb-cyan.bmp");
            // 
            // labTree
            // 
            this.labTree.BackColor = System.Drawing.SystemColors.ControlDark;
            this.labTree.Dock = System.Windows.Forms.DockStyle.Top;
            this.labTree.Location = new System.Drawing.Point(0, 0);
            this.labTree.Name = "labTree";
            this.labTree.Size = new System.Drawing.Size(220, 13);
            this.labTree.TabIndex = 1;
            this.labTree.Text = "Snippets";
            // 
            // panSnippet
            // 
            this.panSnippet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panSnippet.Location = new System.Drawing.Point(0, 13);
            this.panSnippet.MinimumSize = new System.Drawing.Size(320, 240);
            this.panSnippet.Name = "panSnippet";
            this.panSnippet.Size = new System.Drawing.Size(479, 418);
            this.panSnippet.TabIndex = 1;
            // 
            // labSnippetCaption
            // 
            this.labSnippetCaption.BackColor = System.Drawing.SystemColors.ControlDark;
            this.labSnippetCaption.Dock = System.Windows.Forms.DockStyle.Top;
            this.labSnippetCaption.Location = new System.Drawing.Point(0, 0);
            this.labSnippetCaption.Name = "labSnippetCaption";
            this.labSnippetCaption.Size = new System.Drawing.Size(479, 13);
            this.labSnippetCaption.TabIndex = 0;
            this.labSnippetCaption.Text = "...";
            // 
            // FormSnippetManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(703, 502);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormSnippetManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Snippets Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSnippetManager_FormClosing);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FormSnippetManager_KeyPress);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.cmnuTree.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label labTree;
        private System.Windows.Forms.TreeView tvSnippets;
        private System.Windows.Forms.ImageList imglTree;
        private System.Windows.Forms.Label labSnippetCaption;
        private System.Windows.Forms.Panel panSnippet;
        private System.Windows.Forms.ToolStripButton tbSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSplitButton tbNewItem;
        private System.Windows.Forms.ToolStripMenuItem tmiNewFolder;
        private System.Windows.Forms.ToolStripMenuItem tmiNewSnippet;
        private System.Windows.Forms.ToolStripButton tbRemoveItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSplitButton tbApply;
        private System.Windows.Forms.ToolStripMenuItem tmiApplyWholeSnippet;
        private System.Windows.Forms.ToolStripMenuItem tmiApplyOnlyQuery;
        private System.Windows.Forms.ToolStripMenuItem tmiApplyOnlyParameters;
        private System.Windows.Forms.ToolStripMenuItem miStructure;
        private System.Windows.Forms.ToolStripMenuItem miNewFolder;
        private System.Windows.Forms.ToolStripMenuItem miNewSnippet;
        private System.Windows.Forms.ToolStripMenuItem miRemoveSelectedItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem miApplySnippet;
        private System.Windows.Forms.ToolStripMenuItem miApplyWholeSnippet;
        private System.Windows.Forms.ToolStripMenuItem miApplyOnlyQuery;
        private System.Windows.Forms.ToolStripMenuItem miApplyOnlyParameters;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem miSaveChanges;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton tbPasteCurrent;
        private System.Windows.Forms.ToolStripButton tbRename;
        private System.Windows.Forms.ToolStripMenuItem miRenameSelectedItem;
        private System.Windows.Forms.ContextMenuStrip cmnuTree;
        private System.Windows.Forms.ToolStripMenuItem cmiExpandAll;
    }
}