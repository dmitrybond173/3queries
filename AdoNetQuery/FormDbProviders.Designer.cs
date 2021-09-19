namespace AdoNetQuery
{
    partial class FormDbProviders
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
            this.dgvProviders = new System.Windows.Forms.DataGridView();
            this.bsData = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProviders)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsData)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvProviders
            // 
            this.dgvProviders.AutoGenerateColumns = false;
            this.dgvProviders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProviders.DataSource = this.bsData;
            this.dgvProviders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProviders.Location = new System.Drawing.Point(0, 0);
            this.dgvProviders.Name = "dgvProviders";
            this.dgvProviders.Size = new System.Drawing.Size(784, 301);
            this.dgvProviders.TabIndex = 0;
            this.dgvProviders.DoubleClick += new System.EventHandler(this.dgvProviders_DoubleClick);
            // 
            // FormDbProviders
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 301);
            this.Controls.Add(this.dgvProviders);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(480, 320);
            this.Name = "FormDbProviders";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ADO.NET Data Providers";
            this.Shown += new System.EventHandler(this.FormDbProviders_Shown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FormDbProviders_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProviders)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvProviders;
        private System.Windows.Forms.BindingSource bsData;
    }
}