/* 
 * DBO-Tools collection.
 * ADO.NET Query tool.
 * Simple application to execute SQL queries via ADO.NET interface.
 * 
 * UI for List of ADO.NET Providers.
 * 
 * Written by Dmitry Bond. (dima_ben@ukr.net)
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AdoNetQuery
{
    public partial class FormDbProviders : Form
    {
        public static bool Execute(Form pOwner, ref string pProviderName)
        {
            using (FormDbProviders frm = new FormDbProviders())
            {
                frm.Display();
                DialogResult dr = frm.ShowDialog(pOwner);
                bool isCommit = (dr == DialogResult.OK);
                if (isCommit)
                {
                    pProviderName = frm.providerName;
                }
                return isCommit;
            }
        }

        public FormDbProviders()
        {
            InitializeComponent();
        }

        private void Display()
        {
            this.dgvProviders.AutoGenerateColumns = true;
            this.providersTable = DbProviderFactories.GetFactoryClasses();
            this.bsData.DataSource = this.providersTable;
            this.bsData.ResetBindings(true);
        }

        private void FormDbProviders_Shown(object sender, EventArgs e)
        {
            this.dgvProviders.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
        }

        private void dgvProviders_DoubleClick(object sender, EventArgs e)
        {
            if (this.dgvProviders.SelectedCells.Count > 0)
            {
                int rowIdx = this.dgvProviders.SelectedCells[0].RowIndex;
                DataRow row = this.providersTable.Rows[rowIdx];
                this.providerName = row["InvariantName"].ToString();
                this.DialogResult = DialogResult.OK;
            }
        }

        private DataTable providersTable;
        private string providerName;

        private void FormDbProviders_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\x1B')
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
    }
}
