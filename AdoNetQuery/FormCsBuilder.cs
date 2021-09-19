/* 
 * DBO-Tools collection.
 * ADO.NET Query tool.
 * Simple application to execute SQL queries via ADO.NET interface.
 * 
 * UI for Connection String Builder.
 * 
 * Written by Dmitry Bond. (dima_ben@ukr.net)
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using XService.Utils;
using XService.UI.CommonForms;

namespace AdoNetQuery
{
    public partial class FormCsBuilder : Form
    {
        public static bool Execute(Form pOwner, string pProviderName, ref string pConnectionStr, DbConnectionStringBuilder pBuilder)
        {
            using (FormCsBuilder frm = new FormCsBuilder())
            {
                //frm.Parent = pOwner;
                frm.Display(pProviderName, pConnectionStr, pBuilder);

                DialogResult dr = frm.ShowDialog(pOwner);
                bool isCommit = (dr == DialogResult.OK);
                if (isCommit)
                {
                    frm.Commit(ref pConnectionStr);
                }
                return isCommit;
            }
        }

        public FormCsBuilder()
        {
            InitializeComponent();
        }

        private void Display(string pProviderName, string pConnectionStr, DbConnectionStringBuilder pBuilder)
        {
            this.builder = pBuilder;
            this.providerName = pProviderName;
            this.connectionStr = pConnectionStr;

            this.Text += string.Format(" for [{0}]", this.providerName);

            CollectionUtils.ParseParametersStrEx(this.connectionParams, this.connectionStr, true, ';', "=");
            displayCsParameters();
        }

        private void displayCsParameters()
        {
            lvParams.BeginUpdate();
            try
            {
                lvParams.Items.Clear();
                int i = 0;
                string[,] matrix = new string[this.builder.Keys.Count, 2];
                foreach (string k in this.builder.Keys)
                {
                    matrix[i++, 0] = k;
                }
                i = 0;
                foreach (object v in this.builder.Values)
                {
                    matrix[i++, 1] = v.ToString();
                }
                for (i=0; i<this.builder.Keys.Count; i++)
                {
                    string key = matrix[i, 0];
                    string value = matrix[i, 1];
                    ListViewItem li = new ListViewItem(new string[] { key, value });
                    li.Checked = this.connectionParams.TryGetValue(key.ToLower(), out value);
                    if (li.Checked)
                        li.SubItems[1].Text = value;
                    lvParams.Items.Add(li);
                }
            }
            finally 
            { 
                lvParams.EndUpdate(); 
            }
        }

        private void Commit(ref string pConnectionStr)
        {
            string cs = "";
            for (int i = 0; i < lvParams.Items.Count; i++)
            { 
                ListViewItem li = lvParams.Items[i];
                if (li.Checked)
                {
                    cs += string.Format("{0}={1}; ", li.SubItems[0].Text, li.SubItems[1].Text);
                }
            }
            pConnectionStr = cs;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void lvParams_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnEditValue.Enabled = (lvParams.SelectedItems.Count > 0);
        }

        private void lvParams_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (e.Item.Checked)
            {
                if (string.IsNullOrEmpty(e.Item.SubItems[1].Text))
                {
                    e.Item.Selected = true;
                    btnEditValue_Click(btnEditValue, e);
                }
            }
        }

        private void btnEditValue_Click(object sender, EventArgs e)
        {
            if (lvParams.SelectedItems.Count <= 0) return;

            ListViewItem li = lvParams.SelectedItems[0];
            string value = li.SubItems[1].Text;
            if (FormEditValue.Execute(this, "Edit Value of: " + li.SubItems[0].Text, li.SubItems[0].Text, ref value))
            {
                li.SubItems[1].Text = value;
            }
        }

        private ListViewItemComparer lvComparer = null;

        private void lvParams_ColumnClick(object sender, ColumnClickEventArgs e)
        {            
            if (this.lvComparer == null)
                this.lvComparer = new ListViewItemComparer(lvParams, e.Column);

            ColumnHeader columnHdr = lvParams.Columns[e.Column];
            bool isSameColumn = (lvComparer.ColumnNo == e.Column);

            if (!isSameColumn)
            {
                ColumnHeader prevColumnHdr = lvParams.Columns[lvComparer.ColumnNo];
                prevColumnHdr.ImageIndex = -1;
                prevColumnHdr.TextAlign = chName.TextAlign;
                lvParams.Sorting = SortOrder.None;
            }

            this.lvComparer.ColumnNo = e.Column;
            // Note: have to re-set ListViewItemSorter property to keep it working correctly! :-\
            lvParams.ListViewItemSorter = this.lvComparer;
            this.lvComparer.RevertSorting = false;

            if (lvParams.Sorting == SortOrder.None)
            {
                //chName.ImageKey = "sort-asc";
                columnHdr.ImageIndex = 0;
                lvParams.Sorting = SortOrder.Ascending;
                lvParams.Sort();
            }
            else if (lvParams.Sorting == SortOrder.Ascending)
            {
                //chName.ImageKey = "sort-desc";
                columnHdr.ImageIndex = 1;
                lvParams.Sorting = SortOrder.Descending;
                this.lvComparer.RevertSorting = true;
                lvParams.Sort();
            }
            else 
            {
                // Note: there is a bug in .NET - http://social.msdn.microsoft.com/Forums/en/winformsdesigner/thread/4cf0b454-2dd8-46bb-87a9-5290a2b7a7e3
                // So, the only way to reset column image is to use "chName.TextAlign = chName.TextAlign" after ImageIndex set to -1.
                //chName.ImageKey = "empty";
                columnHdr.ImageIndex = -1;
                columnHdr.TextAlign = chName.TextAlign;
                lvParams.Sorting = SortOrder.None;
                displayCsParameters();
            }
        }

        private string providerName;
        private string connectionStr;
        private DbConnectionStringBuilder builder;
        private Dictionary<string, string> connectionParams = new Dictionary<string, string>();

        private void FormCsBuilder_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\x1B')
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

    }
}
