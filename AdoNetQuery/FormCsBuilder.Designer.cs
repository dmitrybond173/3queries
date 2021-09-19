namespace AdoNetQuery
{
    partial class FormCsBuilder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCsBuilder));
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lvParams = new System.Windows.Forms.ListView();
            this.chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imglView = new System.Windows.Forms.ImageList(this.components);
            this.btnEditValue = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(340, 326);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(421, 326);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lvParams
            // 
            this.lvParams.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvParams.CheckBoxes = true;
            this.lvParams.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chName,
            this.chValue});
            this.lvParams.FullRowSelect = true;
            this.lvParams.GridLines = true;
            this.lvParams.Location = new System.Drawing.Point(12, 12);
            this.lvParams.MultiSelect = false;
            this.lvParams.Name = "lvParams";
            this.lvParams.Size = new System.Drawing.Size(481, 308);
            this.lvParams.SmallImageList = this.imglView;
            this.lvParams.TabIndex = 0;
            this.lvParams.UseCompatibleStateImageBehavior = false;
            this.lvParams.View = System.Windows.Forms.View.Details;
            this.lvParams.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvParams_ColumnClick);
            this.lvParams.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvParams_ItemChecked);
            this.lvParams.SelectedIndexChanged += new System.EventHandler(this.lvParams_SelectedIndexChanged);
            // 
            // chName
            // 
            this.chName.Text = "Name";
            this.chName.Width = 200;
            // 
            // chValue
            // 
            this.chValue.Text = "Value";
            this.chValue.Width = 210;
            // 
            // imglView
            // 
            this.imglView.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imglView.ImageStream")));
            this.imglView.TransparentColor = System.Drawing.Color.Fuchsia;
            this.imglView.Images.SetKeyName(0, "sort-asc");
            this.imglView.Images.SetKeyName(1, "sort-desc");
            this.imglView.Images.SetKeyName(2, "empty");
            // 
            // btnEditValue
            // 
            this.btnEditValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEditValue.Location = new System.Drawing.Point(12, 326);
            this.btnEditValue.Name = "btnEditValue";
            this.btnEditValue.Size = new System.Drawing.Size(95, 23);
            this.btnEditValue.TabIndex = 1;
            this.btnEditValue.Text = "Edit Value...";
            this.btnEditValue.UseVisualStyleBackColor = true;
            this.btnEditValue.Click += new System.EventHandler(this.btnEditValue_Click);
            // 
            // FormCsBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(505, 356);
            this.Controls.Add(this.btnEditValue);
            this.Controls.Add(this.lvParams);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(480, 320);
            this.Name = "FormCsBuilder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Connection String Builder";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FormCsBuilder_KeyPress);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ListView lvParams;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.ColumnHeader chValue;
        private System.Windows.Forms.Button btnEditValue;
        private System.Windows.Forms.ImageList imglView;
    }
}