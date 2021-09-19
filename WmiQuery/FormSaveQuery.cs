/* 
 * DBO-Tools collection.
 * WMI Query tool.
 * Simple application to execute WQL/WMI queries.
 * 
 * SaveQuery UI.
 * 
 * Written by Dmitry Bond. (dima_ben@ukr.net)
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using XService.Utils;

namespace XService.UI.CommonForms
{
    public partial class FormSaveQuery : Form
    {
        public delegate bool ValidateInputMethod(SavedQuery pQuery);

        public static bool ExecuteLoadQuery(Form pOwner, SavedQuery pQuery, List<SavedQuery> pSavedQueries)
        {
            using (FormSaveQuery frm = new FormSaveQuery())
            {
                //frm.Parent = pOwner;
                frm.Text = "Load Saved Query";
                frm.saving = false;
                frm.savedQueries = pSavedQueries;
                frm.query = pQuery;
                frm.chkSaveParams.Enabled = false;
                frm.chkSaveParams.Visible = false;
                
                frm.txtQueryName.Text = "";
                frm.txtQueryDescr.Text = "";

                frm.txtQueryName.DropDownStyle = ComboBoxStyle.DropDownList;
                foreach (SavedQuery item in pSavedQueries)
                {
                    frm.txtQueryName.Items.Add(item.Name);
                }
                frm.txtQueryName.SelectedIndexChanged += frm.txtQueryName_SelectedIndexChanged;
                if (frm.txtQueryName.Items.Count > 0)
                    frm.txtQueryName.SelectedIndex = 0;

                frm.txtQueryDescr.ReadOnly = true;
                frm.txtQueryDescr.BackColor = SystemColors.ButtonFace;
               
                DialogResult dr = frm.ShowDialog(pOwner);
                bool isCommit = (dr == DialogResult.OK);
                if (isCommit)
                {
                    pQuery.Name = frm.txtQueryName.Text;
                    pQuery.Description = frm.txtQueryDescr.Text;
                }
                return isCommit;
            }
        }

        public static bool ExecuteSaveQuery(Form pOwner, SavedQuery pQuery, ValidateInputMethod pValidator)
        {
            using (FormSaveQuery frm = new FormSaveQuery())
            {
                //frm.Parent = pOwner;
                frm.Text = "Save Query";
                frm.saving = true;
                frm.query = pQuery;

                frm.txtQueryName.DropDownStyle = ComboBoxStyle.Simple;

                frm.txtQueryName.Text = pQuery.Name;
                if (string.IsNullOrEmpty(pQuery.Description))
                {
                    if (pQuery.Query.Trim(StrUtils.CH_SPACES).StartsWith("--"))
                    {
                        pQuery.Description = StrUtils.GetToPattern(pQuery.Query, Environment.NewLine).Trim("-".ToCharArray());
                    }
                }
                frm.txtQueryDescr.Text = pQuery.Description;
                
                DialogResult dr = frm.ShowDialog();
                bool isCommit = (dr == DialogResult.OK);
                if (isCommit)
                {
                    pQuery.Name = frm.txtQueryName.Text;
                    pQuery.Description = frm.txtQueryDescr.Text;
                    pQuery.SaveParams = frm.chkSaveParams.Checked;
                }
                return isCommit;
            }
        }

        public FormSaveQuery()
        {
            InitializeComponent();
        }

        private int indexOfQueryName(string pName)
        {
            for (int i = 0; i < this.savedQueries.Count; i++)
            {
                SavedQuery q = this.savedQueries[i];
                if (string.Compare(q.Name, pName) == 0)
                    return i;
            }
            return -1;
        }

        private void txtQueryName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.saving) return;

            string s;
            this.query.Name = txtQueryName.Text;
            int idx = indexOfQueryName(this.query.Name);
            if (idx >= 0)
            {
                SavedQuery q = this.savedQueries[idx];
                this.query.Name = q.Name;
                this.query.Description = q.Description;
                this.query.Query = q.Query;
                this.query.SaveParams = q.SaveParams;
                this.query.Parameters = q.Parameters;
                s = q.Query;
                string[] lines = s.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');
                if (lines.Length > 0)
                    txtQueryDescr.Text = lines[0];
                else
                    txtQueryDescr.Text = "???";
                this.query.Description = txtQueryDescr.Text;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.saving)
            {
                this.query.Name = txtQueryName.Text.Trim();
                this.query.Description = txtQueryDescr.Text.Trim();
                bool isOk = !string.IsNullOrEmpty(this.query.Name);
                if (isOk && this.validator != null)
                    isOk = this.validator(this.query);
                if (isOk)
                {
                    this.DialogResult = DialogResult.OK;
                    return;
                }

                labErrMsg.Text = "Invalid input";
            }
            else
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private void chkSaveParams_CheckedChanged(object sender, EventArgs e)
        {
            this.query.SaveParams = chkSaveParams.Checked;
        }

        private bool saving = true;
        private SavedQuery query;
        private ValidateInputMethod validator = null;
        private List<SavedQuery> savedQueries;
    }

    public class SavedQuery
    {
        public const string STR_PARAMS_DELIMITER = "\r\n\x01\r\n";

        public string Name = "";
        public string Description = "";
        public string Query = "";
        public string Parameters = "";
        public bool SaveParams = false;

        // used only in UI
        public bool ToSave = false;

        public void Assign(SavedQuery pSource)
        { 
            Name = pSource.Name;
            Description = pSource.Description;
            Query = pSource.Query;
            Parameters = pSource.Parameters;
            SaveParams = pSource.SaveParams;
            ToSave = pSource.ToSave;
        }
    }

}
