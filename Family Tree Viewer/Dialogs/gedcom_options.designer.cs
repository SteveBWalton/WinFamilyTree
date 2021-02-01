namespace family_tree.viewer
{
    partial class GedcomOptionsDialog
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
            if(disposing && (components != null))
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
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Button cmdOpen;
            System.Windows.Forms.Button cmdCancel;
            System.Windows.Forms.Button cmdOK;
            System.Windows.Forms.Label label2;
            this.txtFilename_ = new System.Windows.Forms.TextBox();
            this.saveFileDialog_ = new System.Windows.Forms.SaveFileDialog();
            this.cboScheme_ = new System.Windows.Forms.ComboBox();
            this.chkPgvu_ = new System.Windows.Forms.CheckBox();
            this.chkRemoveAddresses_ = new System.Windows.Forms.CheckBox();
            this.chkUseCtry_ = new System.Windows.Forms.CheckBox();
            this.chkLongitude_ = new System.Windows.Forms.CheckBox();
            this.chkUseAddr_ = new System.Windows.Forms.CheckBox();
            label1 = new System.Windows.Forms.Label();
            cmdOpen = new System.Windows.Forms.Button();
            cmdCancel = new System.Windows.Forms.Button();
            cmdOK = new System.Windows.Forms.Button();
            label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.Location = new System.Drawing.Point(12, 16);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(100, 23);
            label1.TabIndex = 0;
            label1.Text = "Filename:";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmdOpen
            // 
            cmdOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            cmdOpen.Image = global::family_tree.viewer.Properties.Resources.Open;
            cmdOpen.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            cmdOpen.Location = new System.Drawing.Point(592, 12);
            cmdOpen.Name = "cmdOpen";
            cmdOpen.Size = new System.Drawing.Size(100, 30);
            cmdOpen.TabIndex = 2;
            cmdOpen.Text = "Open";
            cmdOpen.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            cmdOpen.UseVisualStyleBackColor = true;
            cmdOpen.Click += new System.EventHandler(this.cmdOpen_Click);
            // 
            // cmdCancel
            // 
            cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cmdCancel.Image = global::family_tree.viewer.Properties.Resources.Cancel;
            cmdCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            cmdCancel.Location = new System.Drawing.Point(486, 330);
            cmdCancel.Name = "cmdCancel";
            cmdCancel.Size = new System.Drawing.Size(100, 30);
            cmdCancel.TabIndex = 4;
            cmdCancel.Text = "Cancel";
            cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmdOK
            // 
            cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            cmdOK.Image = global::family_tree.viewer.Properties.Resources.OK;
            cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            cmdOK.Location = new System.Drawing.Point(592, 330);
            cmdOK.Name = "cmdOK";
            cmdOK.Size = new System.Drawing.Size(100, 30);
            cmdOK.TabIndex = 3;
            cmdOK.Text = "Save";
            cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // label2
            // 
            label2.Location = new System.Drawing.Point(12, 42);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(100, 23);
            label2.TabIndex = 5;
            label2.Text = "Scheme:";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_txtFilename
            // 
            this.txtFilename_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilename_.Location = new System.Drawing.Point(109, 18);
            this.txtFilename_.Name = "m_txtFilename";
            this.txtFilename_.Size = new System.Drawing.Size(477, 21);
            this.txtFilename_.TabIndex = 1;
            // 
            // m_cboScheme
            // 
            this.cboScheme_.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboScheme_.FormattingEnabled = true;
            this.cboScheme_.Items.AddRange(new object[] {
            "Custom",
            "Neutral",
            "Php Gedview",
            "Gramps"});
            this.cboScheme_.Location = new System.Drawing.Point(109, 44);
            this.cboScheme_.Name = "m_cboScheme";
            this.cboScheme_.Size = new System.Drawing.Size(185, 21);
            this.cboScheme_.TabIndex = 6;
            this.cboScheme_.SelectedIndexChanged += new System.EventHandler(this.cboScheme_SelectedIndexChanged);
            // 
            // m_chkPGVU
            // 
            this.chkPgvu_.AutoSize = true;
            this.chkPgvu_.Location = new System.Drawing.Point(35, 158);
            this.chkPgvu_.Name = "m_chkPGVU";
            this.chkPgvu_.Size = new System.Drawing.Size(163, 17);
            this.chkPgvu_.TabIndex = 7;
            this.chkPgvu_.Text = "Include _PGVU (Last Edit By)";
            this.chkPgvu_.UseVisualStyleBackColor = true;
            // 
            // m_chkRemoveAddresses
            // 
            this.chkRemoveAddresses_.AutoSize = true;
            this.chkRemoveAddresses_.Location = new System.Drawing.Point(35, 181);
            this.chkRemoveAddresses_.Name = "m_chkRemoveAddresses";
            this.chkRemoveAddresses_.Size = new System.Drawing.Size(256, 17);
            this.chkRemoveAddresses_.TabIndex = 8;
            this.chkRemoveAddresses_.Text = "Split address from PLAC tags and add ADDR tag";
            this.chkRemoveAddresses_.UseVisualStyleBackColor = true;
            // 
            // m_chkUseCTRY
            // 
            this.chkUseCtry_.AutoSize = true;
            this.chkUseCtry_.Location = new System.Drawing.Point(35, 227);
            this.chkUseCtry_.Name = "m_chkUseCTRY";
            this.chkUseCtry_.Size = new System.Drawing.Size(208, 17);
            this.chkUseCtry_.TabIndex = 9;
            this.chkUseCtry_.Text = "Add CTRY (Country) tag to PLAC tags";
            this.chkUseCtry_.UseVisualStyleBackColor = true;
            // 
            // m_chkLongitude
            // 
            this.chkLongitude_.AutoSize = true;
            this.chkLongitude_.Location = new System.Drawing.Point(35, 250);
            this.chkLongitude_.Name = "m_chkLongitude";
            this.chkLongitude_.Size = new System.Drawing.Size(174, 17);
            this.chkLongitude_.TabIndex = 10;
            this.chkLongitude_.Text = "Include Longitude and Latitude";
            this.chkLongitude_.UseVisualStyleBackColor = true;
            // 
            // m_chkUseADDR
            // 
            this.chkUseAddr_.AutoSize = true;
            this.chkUseAddr_.Location = new System.Drawing.Point(35, 204);
            this.chkUseAddr_.Name = "m_chkUseADDR";
            this.chkUseAddr_.Size = new System.Drawing.Size(209, 17);
            this.chkUseAddr_.TabIndex = 11;
            this.chkUseAddr_.Text = "Add ADDR (address) tag to PLAC tags";
            this.chkUseAddr_.UseVisualStyleBackColor = true;
            // 
            // frmGedcomOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 372);
            this.Controls.Add(this.chkUseAddr_);
            this.Controls.Add(this.chkLongitude_);
            this.Controls.Add(this.chkUseCtry_);
            this.Controls.Add(this.chkRemoveAddresses_);
            this.Controls.Add(this.chkPgvu_);
            this.Controls.Add(this.cboScheme_);
            this.Controls.Add(label2);
            this.Controls.Add(cmdCancel);
            this.Controls.Add(cmdOK);
            this.Controls.Add(cmdOpen);
            this.Controls.Add(this.txtFilename_);
            this.Controls.Add(label1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmGedcomOptions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export Gedcom Options";
            this.Load += new System.EventHandler(this.frmGedcomOptions_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtFilename_;
        private System.Windows.Forms.SaveFileDialog saveFileDialog_;
        private System.Windows.Forms.ComboBox cboScheme_;
        private System.Windows.Forms.CheckBox chkPgvu_;
        private System.Windows.Forms.CheckBox chkRemoveAddresses_;
        private System.Windows.Forms.CheckBox chkUseCtry_;
        private System.Windows.Forms.CheckBox chkLongitude_;
        private System.Windows.Forms.CheckBox chkUseAddr_;
    }
}