/* 
 * DBO-Tools collection.
 * WMI Query tool.
 * Simple application to execute WQL/WMI queries.
 * 
 * UI for Snippets Manager.
 * 
 * Written by Dmitry Bond. (dima_ben@ukr.net)
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using PAL;
using XService.Utils;
using XService.Snippets;
using XService.UI.CommonForms;

namespace WmiQuery
{
    /// <summary>
    /// Snippets Manager - UI to maintain collection of code snippets
    /// </summary>
    public partial class FormSnippetManager : Form
    {
        public static bool Execute(Form pOwner, SnippetsStorage pStorage, ref Dictionary<string, string> pSnippetValues)
        {
            using (FormSnippetManager frm = new FormSnippetManager())
            {
                CollectionUtils.Merge(frm.snippetValues, pSnippetValues, false);
                frm.storage = pStorage;
                frm.display();
                bool isOk = (frm.ShowDialog() == DialogResult.OK);
                if (isOk)
                    pSnippetValues = frm.snippetValues;
                return isOk;
            }
        }

        private SnippetsStorage storage;
        private TreeNode tnRoot;
        private SnippetRef currentSnippet;
        private Dictionary<string, string> snippetValues = new Dictionary<string, string>();

        public FormSnippetManager()
        {
            InitializeComponent();
        }

        private void display()
        {
            storage.LoadSnippets();
            displaySnippets();
            updateControls();

            this.Text += string.Format(" [{0}]", this.storage.Filename);
        }

        private void displayChildSnippets(TreeNode pTN, SnippetRef pRef)
        { 
            if (pRef.ChildSnippets == null) return ;

            foreach (SnippetRef snp in pRef.ChildSnippets) 
            {
                TreeNode tn = pTN.Nodes.Add(snp.Caption);
                tn.ImageIndex = (snp.IsFolder ? 0 : 1);
                tn.SelectedImageIndex = tn.ImageIndex;
                tn.Tag = snp;
                if (snp.IsFolder)
                    displayChildSnippets(tn, snp);
            }
        }

        private void displaySnippets()
        {
            tvSnippets.BeginUpdate();
            try
            {
                tvSnippets.Nodes.Clear();
                this.tnRoot = tvSnippets.Nodes.Add("Snippets");
                this.tnRoot.ImageIndex = 0;
                this.tnRoot.SelectedImageIndex = this.tnRoot.ImageIndex;
                this.tnRoot.Tag = this.storage.Root;

                displayChildSnippets(this.tnRoot, this.storage.Root);
                this.currentSnippet = this.storage.Root;
            }
            finally
            {
                this.tnRoot.Expand();
                tvSnippets.EndUpdate();
            }
        }

        private Control addControlField(Control.ControlCollection pControls, string pLabel, string pText, float pHeightPrc)
        {
            Panel pan = new Panel();
            pan.Dock = (pHeightPrc == 100f ? DockStyle.Fill : DockStyle.Top);
            if (pan.Dock != DockStyle.Fill)
                pan.Height = (int)(pControls.Owner.Height * pHeightPrc / 100f);
            pControls.Add(pan);
            pan.MinimumSize = new Size(320, 100);
            pan.BringToFront();

            if (pan.Dock == DockStyle.Top)
            {
                Splitter splt = new Splitter();
                splt.Dock = DockStyle.Top;
                pControls.Add(splt);
                splt.BringToFront();
            }

            Label lab = new Label();
            lab.Dock = DockStyle.Top;
            lab.Text = pLabel;
            lab.Height = 13;
            pan.Controls.Add(lab);

            TextBox txt = new TextBox();
            txt.Font = new System.Drawing.Font("Courier New", 8.25f);
            txt.Multiline = true;
            txt.Dock = DockStyle.Fill;
            pan.Controls.Add(txt);
            txt.WordWrap = false;
            txt.ScrollBars = ScrollBars.Both;
            txt.TextChanged += new EventHandler(txt_TextChanged);
            txt.Text = pText;
            txt.BringToFront();

            return txt;
        }

        private void addControl(Control.ControlCollection pControls, SnippetRef pSnpRef)
        {
            labSnippetCaption.Text = pSnpRef.Caption;
            
            Control ctrl;
            
            ctrl = addControlField(pControls, "Query", pSnpRef["Query"], 50f);
            ctrl.Tag = "Query";

            ctrl = addControlField(pControls, "Parameters", pSnpRef["Parameters"], 100f);
            ctrl.Tag = "Parameters";
        }

        private void displaySnippetProps(TreeNode pTN)
        {
            this.currentSnippet = (SnippetRef)pTN.Tag;
            panSnippet.Controls.Clear();
            if (this.currentSnippet.ChildSnippets != null)
                return;

            addControl(panSnippet.Controls, this.currentSnippet);
        }

        private void updateControls()
        {
            bool isItemSelected = (tvSnippets.SelectedNode != null);
            bool isFolder = (tvSnippets.SelectedNode != null && tvSnippets.SelectedNode.Nodes.Count > 0);
            bool isValidSnippet = (tvSnippets.SelectedNode != null && ((SnippetRef)tvSnippets.SelectedNode.Tag).Snippet != null);

            tbRemoveItem.Enabled = (isItemSelected && tvSnippets.SelectedNode.Level > 0);
            miRemoveSelectedItem.Enabled = tbRemoveItem.Enabled;
            tbRename.Enabled = tbRemoveItem.Enabled;

            tbPasteCurrent.Enabled = isValidSnippet && this.snippetValues.ContainsKey("QUERY") && this.snippetValues.ContainsKey("PARAMETERS");            
            
            tbApply.Enabled = isValidSnippet;
            miApplySnippet.Enabled = isValidSnippet;
            //miApplyWholeSnippet.Enabled = isValidSnippet;
            //miApplyOnlyQuery.Enabled = isValidSnippet;
            //miApplyOnlyParameters.Enabled = isValidSnippet;

            tbSave.Enabled = this.storage.Registry.IsModified;
        }

        #region Form Event Handlers

        private void FormSnippetManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.storage.Registry.IsModified)
            {
                DialogResult dr = MessageBox.Show("There are some unsaved changes. Do you want to save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    this.storage.SaveChanges();
                }
            }
        }

        private void cmiExpandAll_Click(object sender, EventArgs e)
        {
            tvSnippets.ExpandAll();
        }

        private void tmiNewFolder_Click(object sender, EventArgs e)
        {
            string name = "Untitled";
            if (!FormEditValue.Execute(this, "Add New Folder", "Enter Folder Name", ref name))
                return;

            TreeNode tn = tvSnippets.SelectedNode;
            if (tn == null) tn = this.tnRoot;

            SnippetRef snp = (SnippetRef)tn.Tag;
            if (snp.ChildSnippets == null) snp.ChildSnippets = new List<object>();
            
            SnippetRef snpNew = new SnippetRef(this.storage) { Caption = name, ChildSnippets = new List<object>() };
            snp.ChildSnippets.Add(snpNew);
            
            TreeNode tnFolder = tn.Nodes.Add(snpNew.Caption);
            tnFolder.ImageIndex = 0;
            tnFolder.SelectedImageIndex = tnFolder.ImageIndex;
            tnFolder.Tag = snpNew;

            snpNew.RegKey = snp.RegKey.Owner.CreateKey(snp.RegKey.Path + "/" + snpNew.Caption);

            updateControls();
        }

        private void tmiNewSnippet_Click(object sender, EventArgs e)
        {
            string name = "Untitled";
            if (!FormEditValue.Execute(this, "Add New Snippet", "Enter Snippet Name", ref name))
                return;

            TreeNode tn = tvSnippets.SelectedNode;
            if (tn == null) tn = this.tnRoot;

            SnippetRef snp = (SnippetRef)tn.Tag;
            if (!snp.IsFolder) 
            {
                tn = tn.Parent;
                snp = (SnippetRef)tn.Tag;
            }
            if (snp.ChildSnippets == null) snp.ChildSnippets = new List<object>();

            SnippetRef snpNew = new SnippetRef(this.storage) { Caption = name };
            snpNew.CreateSnippetInstance();
            snp.ChildSnippets.Add(snpNew);

            TreeNode tnFolder = tn.Nodes.Add(snpNew.Caption);
            tnFolder.ImageIndex = 1;
            tnFolder.SelectedImageIndex = tnFolder.ImageIndex;
            tnFolder.Tag = snpNew;

            snpNew.RegKey = snp.RegKey.Owner.CreateKey(snp.RegKey.Path + "/" + snpNew.Caption);
            snpNew.RegKey.WriteString("Query", "");
            snpNew.RegKey.WriteString("Parameters", "");

            updateControls();
        }

        private void tbApply_ButtonClick(object sender, EventArgs e)
        {
            tmiApplyWholeSnippet_Click(sender, e);
        }

        private void tbNewItem_ButtonClick(object sender, EventArgs e)
        {
            tmiNewSnippet_Click(sender, e);
        }

        private void tbSave_Click(object sender, EventArgs e)
        {
            this.storage.SaveChanges();
            updateControls();
        }

        private void tbPasteCurrent_Click(object sender, EventArgs e)
        {
            bool isValid = (this.currentSnippet != null && tvSnippets.SelectedNode != null
                && this.snippetValues.ContainsKey("QUERY") && this.snippetValues.ContainsKey("PARAMETERS"));
            if (!isValid) return;

            this.currentSnippet["QUERY"] = this.snippetValues["QUERY"];
            this.currentSnippet["PARAMETERS"] = this.snippetValues["PARAMETERS"];
            displaySnippetProps(tvSnippets.SelectedNode);

            updateControls();
        }

        private void tbRemoveItem_Click(object sender, EventArgs e)
        {
            TreeNode tn = tvSnippets.SelectedNode;
            if (tn == null || tn.Level == 0) return;

            SnippetRef snp = (SnippetRef)tn.Tag;
            DialogResult dr = MessageBox.Show(string.Format("Remove ({0}) {1}?", snp.Caption, (snp.IsFolder ? "folder" : "snippet")),
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr != System.Windows.Forms.DialogResult.Yes) return;

            snp.RegKey.Owner.RemoveKey(snp.RegKey.Path);

            SnippetRef parentSnp = (SnippetRef)tn.Parent.Tag;
            parentSnp.ChildSnippets.Remove(snp);

            tvSnippets.SelectedNode = tn.Parent;
            tn.Remove();

            updateControls();
        }

        private void tbRename_Click(object sender, EventArgs e)
        {
            bool isValid = (tvSnippets.SelectedNode != null);
            if (!isValid) return;

            TreeNode tn = tvSnippets.SelectedNode;
            SnippetRef snp = (SnippetRef)tn.Tag;

            string name = snp.Caption;
            if (!FormEditValue.Execute(this, "Change Name", "Enter New Name", ref name))
                return;

            tn.Text = name;
            snp.Caption = name;
            snp["NAME"] = name;

            updateControls();
        }

        private void tmiApplyWholeSnippet_Click(object sender, EventArgs e)
        {
            if (this.currentSnippet == null) return ;

            snippetValues["NAME"] = this.currentSnippet.Caption;
            snippetValues["QUERY"] = this.currentSnippet["Query"];
            snippetValues["PARAMETERS"] = this.currentSnippet["Parameters"];
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void tmiApplyOnlyQuery_Click(object sender, EventArgs e)
        {
            if (this.currentSnippet == null) return;

            snippetValues["NAME"] = this.currentSnippet.Caption;
            snippetValues["QUERY"] = this.currentSnippet["Query"];
            //snippetValues["PARAMETERS"] = this.currentSnippet["Parameters"];
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void tmiApplyOnlyParameters_Click(object sender, EventArgs e)
        {
            if (this.currentSnippet == null) return;

            snippetValues["NAME"] = this.currentSnippet.Caption;
            //snippetValues["QUERY"] = this.currentSnippet["Query"];
            snippetValues["PARAMETERS"] = this.currentSnippet["Parameters"];
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void tvSnippets_AfterSelect(object sender, TreeViewEventArgs e)
        {
            displaySnippetProps(e.Node);
            updateControls();
        }

        private void txt_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (txt.Tag != null)
            {
                this.currentSnippet[txt.Tag.ToString()] = txt.Text;
                updateControls();
            }
        }

        #endregion // Form Event Handlers

    }

    
}
