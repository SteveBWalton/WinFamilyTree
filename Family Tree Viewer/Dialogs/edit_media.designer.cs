namespace family_tree.viewer
{
    partial class EditMediaDialog
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Panel LeftPanel;
            System.Windows.Forms.Button cmdRemovePerson;
            System.Windows.Forms.ImageList oImageList16x16;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditMediaDialog));
            System.Windows.Forms.Button cmdAddPerson;
            System.Windows.Forms.Button cmdOpen;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Button cmdCancel;
            System.Windows.Forms.Button cmdOK;
            System.Windows.Forms.Splitter oSplitter;
            this.lstPeople_ = new System.Windows.Forms.ListBox();
            this.cboPeople_ = new System.Windows.Forms.ComboBox();
            this.txtFilename_ = new System.Windows.Forms.TextBox();
            this.txtHeight_ = new System.Windows.Forms.TextBox();
            this.txtWidth_ = new System.Windows.Forms.TextBox();
            this.m_chkThumbnail = new System.Windows.Forms.CheckBox();
            this.m_chkPrimary = new System.Windows.Forms.CheckBox();
            this.m_txtTitle = new System.Windows.Forms.TextBox();
            this.pictureBox_ = new System.Windows.Forms.PictureBox();
            this.openFileDialog_ = new System.Windows.Forms.OpenFileDialog();
            LeftPanel = new System.Windows.Forms.Panel();
            cmdRemovePerson = new System.Windows.Forms.Button();
            oImageList16x16 = new System.Windows.Forms.ImageList(this.components);
            cmdAddPerson = new System.Windows.Forms.Button();
            cmdOpen = new System.Windows.Forms.Button();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            cmdCancel = new System.Windows.Forms.Button();
            cmdOK = new System.Windows.Forms.Button();
            oSplitter = new System.Windows.Forms.Splitter();
            LeftPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_)).BeginInit();
            this.SuspendLayout();
            // 
            // LeftPanel
            // 
            LeftPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            LeftPanel.Controls.Add(cmdRemovePerson);
            LeftPanel.Controls.Add(this.lstPeople_);
            LeftPanel.Controls.Add(cmdAddPerson);
            LeftPanel.Controls.Add(this.cboPeople_);
            LeftPanel.Controls.Add(cmdOpen);
            LeftPanel.Controls.Add(this.txtFilename_);
            LeftPanel.Controls.Add(label4);
            LeftPanel.Controls.Add(label3);
            LeftPanel.Controls.Add(this.txtHeight_);
            LeftPanel.Controls.Add(this.txtWidth_);
            LeftPanel.Controls.Add(label2);
            LeftPanel.Controls.Add(this.m_chkThumbnail);
            LeftPanel.Controls.Add(this.m_chkPrimary);
            LeftPanel.Controls.Add(this.m_txtTitle);
            LeftPanel.Controls.Add(label1);
            LeftPanel.Controls.Add(cmdCancel);
            LeftPanel.Controls.Add(cmdOK);
            LeftPanel.Dock = System.Windows.Forms.DockStyle.Right;
            LeftPanel.Location = new System.Drawing.Point(359, 0);
            LeftPanel.Name = "LeftPanel";
            LeftPanel.Size = new System.Drawing.Size(176, 508);
            LeftPanel.TabIndex = 0;
            // 
            // cmdRemovePerson
            // 
            cmdRemovePerson.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            cmdRemovePerson.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            cmdRemovePerson.ImageIndex = 4;
            cmdRemovePerson.ImageList = oImageList16x16;
            cmdRemovePerson.Location = new System.Drawing.Point(93, 298);
            cmdRemovePerson.Name = "cmdRemovePerson";
            cmdRemovePerson.Size = new System.Drawing.Size(75, 27);
            cmdRemovePerson.TabIndex = 16;
            cmdRemovePerson.Text = "Remove";
            cmdRemovePerson.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            cmdRemovePerson.UseVisualStyleBackColor = true;
            cmdRemovePerson.Click += new System.EventHandler(this.cmdRemovePerson_Click);
            // 
            // oImageList16x16
            // 
            oImageList16x16.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("oImageList16x16.ImageStream")));
            oImageList16x16.TransparentColor = System.Drawing.Color.Silver;
            oImageList16x16.Images.SetKeyName(0, "Cross Red.bmp");
            oImageList16x16.Images.SetKeyName(1, "Tick.bmp");
            oImageList16x16.Images.SetKeyName(2, "Open.bmp");
            oImageList16x16.Images.SetKeyName(3, "Arrow Red Down.bmp");
            oImageList16x16.Images.SetKeyName(4, "Arrow Red Up.bmp");
            // 
            // m_lstPeople
            // 
            this.lstPeople_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstPeople_.FormattingEnabled = true;
            this.lstPeople_.Location = new System.Drawing.Point(8, 331);
            this.lstPeople_.Name = "m_lstPeople";
            this.lstPeople_.Size = new System.Drawing.Size(160, 95);
            this.lstPeople_.TabIndex = 15;
            // 
            // cmdAddPerson
            // 
            cmdAddPerson.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            cmdAddPerson.ImageIndex = 3;
            cmdAddPerson.ImageList = oImageList16x16;
            cmdAddPerson.Location = new System.Drawing.Point(8, 298);
            cmdAddPerson.Name = "cmdAddPerson";
            cmdAddPerson.Size = new System.Drawing.Size(75, 27);
            cmdAddPerson.TabIndex = 14;
            cmdAddPerson.Text = "Add";
            cmdAddPerson.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            cmdAddPerson.UseVisualStyleBackColor = true;
            cmdAddPerson.Click += new System.EventHandler(this.cmdAddPerson_Click);
            // 
            // m_cboPeople
            // 
            this.cboPeople_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboPeople_.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPeople_.FormattingEnabled = true;
            this.cboPeople_.Location = new System.Drawing.Point(8, 271);
            this.cboPeople_.Name = "m_cboPeople";
            this.cboPeople_.Size = new System.Drawing.Size(160, 21);
            this.cboPeople_.TabIndex = 13;
            // 
            // cmdOpen
            // 
            cmdOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            cmdOpen.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            cmdOpen.ImageIndex = 2;
            cmdOpen.ImageList = oImageList16x16;
            cmdOpen.Location = new System.Drawing.Point(93, 51);
            cmdOpen.Name = "cmdOpen";
            cmdOpen.Size = new System.Drawing.Size(75, 27);
            cmdOpen.TabIndex = 12;
            cmdOpen.Text = "Open";
            cmdOpen.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            cmdOpen.UseVisualStyleBackColor = true;
            cmdOpen.Click += new System.EventHandler(this.cmdOpen_Click);
            // 
            // m_txtFilename
            // 
            this.txtFilename_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilename_.Location = new System.Drawing.Point(8, 24);
            this.txtFilename_.Name = "m_txtFilename";
            this.txtFilename_.Size = new System.Drawing.Size(160, 21);
            this.txtFilename_.TabIndex = 11;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(5, 9);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(49, 13);
            label4.TabIndex = 10;
            label4.Text = "Filename";
            // 
            // label3
            // 
            label3.Location = new System.Drawing.Point(5, 244);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(65, 17);
            label3.TabIndex = 9;
            label3.Text = "Height";
            label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_txtHeight
            // 
            this.txtHeight_.Location = new System.Drawing.Point(72, 244);
            this.txtHeight_.Name = "m_txtHeight";
            this.txtHeight_.ReadOnly = true;
            this.txtHeight_.Size = new System.Drawing.Size(100, 21);
            this.txtHeight_.TabIndex = 8;
            // 
            // m_txtWidth
            // 
            this.txtWidth_.Location = new System.Drawing.Point(72, 216);
            this.txtWidth_.Name = "m_txtWidth";
            this.txtWidth_.ReadOnly = true;
            this.txtWidth_.Size = new System.Drawing.Size(100, 21);
            this.txtWidth_.TabIndex = 7;
            // 
            // label2
            // 
            label2.Location = new System.Drawing.Point(5, 216);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(65, 17);
            label2.TabIndex = 6;
            label2.Text = "Width";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_chkThumbnail
            // 
            this.m_chkThumbnail.AutoSize = true;
            this.m_chkThumbnail.Location = new System.Drawing.Point(8, 193);
            this.m_chkThumbnail.Name = "m_chkThumbnail";
            this.m_chkThumbnail.Size = new System.Drawing.Size(74, 17);
            this.m_chkThumbnail.TabIndex = 5;
            this.m_chkThumbnail.Text = "Thumbnail";
            this.m_chkThumbnail.UseVisualStyleBackColor = true;
            // 
            // m_chkPrimary
            // 
            this.m_chkPrimary.AutoSize = true;
            this.m_chkPrimary.Location = new System.Drawing.Point(8, 170);
            this.m_chkPrimary.Name = "m_chkPrimary";
            this.m_chkPrimary.Size = new System.Drawing.Size(62, 17);
            this.m_chkPrimary.TabIndex = 4;
            this.m_chkPrimary.Text = "Primary";
            this.m_chkPrimary.UseVisualStyleBackColor = true;
            // 
            // m_txtTitle
            // 
            this.m_txtTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_txtTitle.Location = new System.Drawing.Point(8, 94);
            this.m_txtTitle.Multiline = true;
            this.m_txtTitle.Name = "m_txtTitle";
            this.m_txtTitle.Size = new System.Drawing.Size(160, 70);
            this.m_txtTitle.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(5, 78);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(27, 13);
            label1.TabIndex = 2;
            label1.Text = "Title";
            // 
            // cmdCancel
            // 
            cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cmdCancel.Image = global::family_tree.viewer.Properties.Resources.Cancel;
            cmdCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            cmdCancel.Location = new System.Drawing.Point(85, 469);
            cmdCancel.Name = "cmdCancel";
            cmdCancel.Size = new System.Drawing.Size(75, 27);
            cmdCancel.TabIndex = 1;
            cmdCancel.Text = "Cancel";
            cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdOK
            // 
            cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            cmdOK.Image = global::family_tree.viewer.Properties.Resources.OK;
            cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            cmdOK.Location = new System.Drawing.Point(4, 469);
            cmdOK.Name = "cmdOK";
            cmdOK.Size = new System.Drawing.Size(75, 27);
            cmdOK.TabIndex = 0;
            cmdOK.Text = "OK";
            cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            cmdOK.UseVisualStyleBackColor = true;
            cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // oSplitter
            // 
            oSplitter.Dock = System.Windows.Forms.DockStyle.Right;
            oSplitter.Location = new System.Drawing.Point(356, 0);
            oSplitter.Name = "oSplitter";
            oSplitter.Size = new System.Drawing.Size(3, 508);
            oSplitter.TabIndex = 1;
            oSplitter.TabStop = false;
            // 
            // m_PictureBox
            // 
            this.pictureBox_.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox_.Location = new System.Drawing.Point(0, 0);
            this.pictureBox_.Name = "m_PictureBox";
            this.pictureBox_.Size = new System.Drawing.Size(356, 508);
            this.pictureBox_.TabIndex = 2;
            this.pictureBox_.TabStop = false;
            // 
            // m_OpenFileDialog
            // 
            this.openFileDialog_.FileName = "openFileDialog1";
            // 
            // frmEditMedia
            // 
            this.AcceptButton = cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = cmdCancel;
            this.ClientSize = new System.Drawing.Size(535, 508);
            this.Controls.Add(this.pictureBox_);
            this.Controls.Add(oSplitter);
            this.Controls.Add(LeftPanel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmEditMedia";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Media";
            this.Load += new System.EventHandler(this.frmEditMedia_Load);
            LeftPanel.ResumeLayout(false);
            LeftPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox_;
        private System.Windows.Forms.CheckBox m_chkPrimary;
        private System.Windows.Forms.TextBox m_txtTitle;
        private System.Windows.Forms.TextBox txtHeight_;
        private System.Windows.Forms.TextBox txtWidth_;
        private System.Windows.Forms.CheckBox m_chkThumbnail;
        private System.Windows.Forms.TextBox txtFilename_;
        private System.Windows.Forms.OpenFileDialog openFileDialog_;
        private System.Windows.Forms.ListBox lstPeople_;
        private System.Windows.Forms.ComboBox cboPeople_;

    }
}