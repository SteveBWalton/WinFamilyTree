namespace FamilyTree.Viewer
{
    partial class frmGedcomOptions
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
            this.m_txtFilename = new System.Windows.Forms.TextBox();
            this.m_SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.m_cboScheme = new System.Windows.Forms.ComboBox();
            this.m_chkPGVU = new System.Windows.Forms.CheckBox();
            this.m_chkRemoveAddresses = new System.Windows.Forms.CheckBox();
            this.m_chkUseCTRY = new System.Windows.Forms.CheckBox();
            this.m_chkLongitude = new System.Windows.Forms.CheckBox();
            this.m_chkUseADDR = new System.Windows.Forms.CheckBox();
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
            cmdOpen.Image = global::FamilyTree.Viewer.Properties.Resources.Open;
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
            cmdCancel.Image = global::FamilyTree.Viewer.Properties.Resources.Cancel;
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
            cmdOK.Image = global::FamilyTree.Viewer.Properties.Resources.OK;
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
            this.m_txtFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_txtFilename.Location = new System.Drawing.Point(109, 18);
            this.m_txtFilename.Name = "m_txtFilename";
            this.m_txtFilename.Size = new System.Drawing.Size(477, 21);
            this.m_txtFilename.TabIndex = 1;
            // 
            // m_cboScheme
            // 
            this.m_cboScheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cboScheme.FormattingEnabled = true;
            this.m_cboScheme.Items.AddRange(new object[] {
            "Custom",
            "Neutral",
            "Php Gedview",
            "Gramps"});
            this.m_cboScheme.Location = new System.Drawing.Point(109, 44);
            this.m_cboScheme.Name = "m_cboScheme";
            this.m_cboScheme.Size = new System.Drawing.Size(185, 21);
            this.m_cboScheme.TabIndex = 6;
            this.m_cboScheme.SelectedIndexChanged += new System.EventHandler(this.m_cboScheme_SelectedIndexChanged);
            // 
            // m_chkPGVU
            // 
            this.m_chkPGVU.AutoSize = true;
            this.m_chkPGVU.Location = new System.Drawing.Point(35, 158);
            this.m_chkPGVU.Name = "m_chkPGVU";
            this.m_chkPGVU.Size = new System.Drawing.Size(163, 17);
            this.m_chkPGVU.TabIndex = 7;
            this.m_chkPGVU.Text = "Include _PGVU (Last Edit By)";
            this.m_chkPGVU.UseVisualStyleBackColor = true;
            // 
            // m_chkRemoveAddresses
            // 
            this.m_chkRemoveAddresses.AutoSize = true;
            this.m_chkRemoveAddresses.Location = new System.Drawing.Point(35, 181);
            this.m_chkRemoveAddresses.Name = "m_chkRemoveAddresses";
            this.m_chkRemoveAddresses.Size = new System.Drawing.Size(256, 17);
            this.m_chkRemoveAddresses.TabIndex = 8;
            this.m_chkRemoveAddresses.Text = "Split address from PLAC tags and add ADDR tag";
            this.m_chkRemoveAddresses.UseVisualStyleBackColor = true;
            // 
            // m_chkUseCTRY
            // 
            this.m_chkUseCTRY.AutoSize = true;
            this.m_chkUseCTRY.Location = new System.Drawing.Point(35, 227);
            this.m_chkUseCTRY.Name = "m_chkUseCTRY";
            this.m_chkUseCTRY.Size = new System.Drawing.Size(208, 17);
            this.m_chkUseCTRY.TabIndex = 9;
            this.m_chkUseCTRY.Text = "Add CTRY (Country) tag to PLAC tags";
            this.m_chkUseCTRY.UseVisualStyleBackColor = true;
            // 
            // m_chkLongitude
            // 
            this.m_chkLongitude.AutoSize = true;
            this.m_chkLongitude.Location = new System.Drawing.Point(35, 250);
            this.m_chkLongitude.Name = "m_chkLongitude";
            this.m_chkLongitude.Size = new System.Drawing.Size(174, 17);
            this.m_chkLongitude.TabIndex = 10;
            this.m_chkLongitude.Text = "Include Longitude and Latitude";
            this.m_chkLongitude.UseVisualStyleBackColor = true;
            // 
            // m_chkUseADDR
            // 
            this.m_chkUseADDR.AutoSize = true;
            this.m_chkUseADDR.Location = new System.Drawing.Point(35, 204);
            this.m_chkUseADDR.Name = "m_chkUseADDR";
            this.m_chkUseADDR.Size = new System.Drawing.Size(209, 17);
            this.m_chkUseADDR.TabIndex = 11;
            this.m_chkUseADDR.Text = "Add ADDR (address) tag to PLAC tags";
            this.m_chkUseADDR.UseVisualStyleBackColor = true;
            // 
            // frmGedcomOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 372);
            this.Controls.Add(this.m_chkUseADDR);
            this.Controls.Add(this.m_chkLongitude);
            this.Controls.Add(this.m_chkUseCTRY);
            this.Controls.Add(this.m_chkRemoveAddresses);
            this.Controls.Add(this.m_chkPGVU);
            this.Controls.Add(this.m_cboScheme);
            this.Controls.Add(label2);
            this.Controls.Add(cmdCancel);
            this.Controls.Add(cmdOK);
            this.Controls.Add(cmdOpen);
            this.Controls.Add(this.m_txtFilename);
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

        private System.Windows.Forms.TextBox m_txtFilename;
        private System.Windows.Forms.SaveFileDialog m_SaveFileDialog;
        private System.Windows.Forms.ComboBox m_cboScheme;
        private System.Windows.Forms.CheckBox m_chkPGVU;
        private System.Windows.Forms.CheckBox m_chkRemoveAddresses;
        private System.Windows.Forms.CheckBox m_chkUseCTRY;
        private System.Windows.Forms.CheckBox m_chkLongitude;
        private System.Windows.Forms.CheckBox m_chkUseADDR;
    }
}