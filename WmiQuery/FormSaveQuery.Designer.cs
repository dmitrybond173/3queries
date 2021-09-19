namespace XService.UI.CommonForms
{
    partial class FormSaveQuery
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
            this.labQueryName = new System.Windows.Forms.Label();
            this.labQueryDescr = new System.Windows.Forms.Label();
            this.txtQueryDescr = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtQueryName = new System.Windows.Forms.ComboBox();
            this.labErrMsg = new System.Windows.Forms.Label();
            this.chkSaveParams = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // labQueryName
            // 
            this.labQueryName.AutoSize = true;
            this.labQueryName.Location = new System.Drawing.Point(13, 13);
            this.labQueryName.Name = "labQueryName";
            this.labQueryName.Size = new System.Drawing.Size(66, 13);
            this.labQueryName.TabIndex = 0;
            this.labQueryName.Text = "Query Name";
            // 
            // labQueryDescr
            // 
            this.labQueryDescr.AutoSize = true;
            this.labQueryDescr.Location = new System.Drawing.Point(13, 58);
            this.labQueryDescr.Name = "labQueryDescr";
            this.labQueryDescr.Size = new System.Drawing.Size(89, 13);
            this.labQueryDescr.TabIndex = 2;
            this.labQueryDescr.Text = "Query Descrption";
            // 
            // txtQueryDescr
            // 
            this.txtQueryDescr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtQueryDescr.Location = new System.Drawing.Point(13, 75);
            this.txtQueryDescr.Name = "txtQueryDescr";
            this.txtQueryDescr.Size = new System.Drawing.Size(366, 20);
            this.txtQueryDescr.TabIndex = 3;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(222, 107);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(304, 107);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // txtQueryName
            // 
            this.txtQueryName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtQueryName.FormattingEnabled = true;
            this.txtQueryName.Location = new System.Drawing.Point(12, 29);
            this.txtQueryName.Name = "txtQueryName";
            this.txtQueryName.Size = new System.Drawing.Size(367, 21);
            this.txtQueryName.TabIndex = 1;
            // 
            // labErrMsg
            // 
            this.labErrMsg.ForeColor = System.Drawing.Color.Maroon;
            this.labErrMsg.Location = new System.Drawing.Point(13, 107);
            this.labErrMsg.Name = "labErrMsg";
            this.labErrMsg.Size = new System.Drawing.Size(203, 23);
            this.labErrMsg.TabIndex = 0;
            // 
            // chkSaveParams
            // 
            this.chkSaveParams.AutoSize = true;
            this.chkSaveParams.Location = new System.Drawing.Point(16, 107);
            this.chkSaveParams.Name = "chkSaveParams";
            this.chkSaveParams.Size = new System.Drawing.Size(128, 17);
            this.chkSaveParams.TabIndex = 6;
            this.chkSaveParams.Text = "Save also &parameters";
            this.chkSaveParams.UseVisualStyleBackColor = true;
            this.chkSaveParams.CheckedChanged += new System.EventHandler(this.chkSaveParams_CheckedChanged);
            // 
            // FormSaveQuery
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(393, 139);
            this.Controls.Add(this.chkSaveParams);
            this.Controls.Add(this.txtQueryName);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtQueryDescr);
            this.Controls.Add(this.labErrMsg);
            this.Controls.Add(this.labQueryDescr);
            this.Controls.Add(this.labQueryName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSaveQuery";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Save Query";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labQueryName;
        private System.Windows.Forms.Label labQueryDescr;
        private System.Windows.Forms.TextBox txtQueryDescr;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox txtQueryName;
        private System.Windows.Forms.Label labErrMsg;
        private System.Windows.Forms.CheckBox chkSaveParams;
    }
}