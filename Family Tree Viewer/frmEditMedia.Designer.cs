namespace FamilyTree.Viewer
{
    partial class frmEditMedia
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEditMedia));
            System.Windows.Forms.Button cmdAddPerson;
            System.Windows.Forms.Button cmdOpen;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Button cmdCancel;
            System.Windows.Forms.Button cmdOK;
            System.Windows.Forms.Splitter oSplitter;
            this.m_lstPeople = new System.Windows.Forms.ListBox();
            this.m_cboPeople = new System.Windows.Forms.ComboBox();
            this.m_txtFilename = new System.Windows.Forms.TextBox();
            this.m_txtHeight = new System.Windows.Forms.TextBox();
            this.m_txtWidth = new System.Windows.Forms.TextBox();
            this.m_chkThumbnail = new System.Windows.Forms.CheckBox();
            this.m_chkPrimary = new System.Windows.Forms.CheckBox();
            this.m_txtTitle = new System.Windows.Forms.TextBox();
            this.m_PictureBox = new System.Windows.Forms.PictureBox();
            this.m_OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
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
            ((System.ComponentModel.ISupportInitialize)(this.m_PictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // LeftPanel
            // 
            LeftPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            LeftPanel.Controls.Add(cmdRemovePerson);
            LeftPanel.Controls.Add(this.m_lstPeople);
            LeftPanel.Controls.Add(cmdAddPerson);
            LeftPanel.Controls.Add(this.m_cboPeople);
            LeftPanel.Controls.Add(cmdOpen);
            LeftPanel.Controls.Add(this.m_txtFilename);
            LeftPanel.Controls.Add(label4);
            LeftPanel.Controls.Add(label3);
            LeftPanel.Controls.Add(this.m_txtHeight);
            LeftPanel.Controls.Add(this.m_txtWidth);
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
            this.m_lstPeople.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_lstPeople.FormattingEnabled = true;
            this.m_lstPeople.Location = new System.Drawing.Point(8, 331);
            this.m_lstPeople.Name = "m_lstPeople";
            this.m_lstPeople.Size = new System.Drawing.Size(160, 95);
            this.m_lstPeople.TabIndex = 15;
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
            this.m_cboPeople.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_cboPeople.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cboPeople.FormattingEnabled = true;
            this.m_cboPeople.Location = new System.Drawing.Point(8, 271);
            this.m_cboPeople.Name = "m_cboPeople";
            this.m_cboPeople.Size = new System.Drawing.Size(160, 21);
            this.m_cboPeople.TabIndex = 13;
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
            this.m_txtFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_txtFilename.Location = new System.Drawing.Point(8, 24);
            this.m_txtFilename.Name = "m_txtFilename";
            this.m_txtFilename.Size = new System.Drawing.Size(160, 21);
            this.m_txtFilename.TabIndex = 11;
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
            this.m_txtHeight.Location = new System.Drawing.Point(72, 244);
            this.m_txtHeight.Name = "m_txtHeight";
            this.m_txtHeight.ReadOnly = true;
            this.m_txtHeight.Size = new System.Drawing.Size(100, 21);
            this.m_txtHeight.TabIndex = 8;
            // 
            // m_txtWidth
            // 
            this.m_txtWidth.Location = new System.Drawing.Point(72, 216);
            this.m_txtWidth.Name = "m_txtWidth";
            this.m_txtWidth.ReadOnly = true;
            this.m_txtWidth.Size = new System.Drawing.Size(100, 21);
            this.m_txtWidth.TabIndex = 7;
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
            cmdCancel.Image = global::FamilyTree.Viewer.Properties.Resources.Cancel;
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
            cmdOK.Image = global::FamilyTree.Viewer.Properties.Resources.OK;
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
            this.m_PictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_PictureBox.Location = new System.Drawing.Point(0, 0);
            this.m_PictureBox.Name = "m_PictureBox";
            this.m_PictureBox.Size = new System.Drawing.Size(356, 508);
            this.m_PictureBox.TabIndex = 2;
            this.m_PictureBox.TabStop = false;
            // 
            // m_OpenFileDialog
            // 
            this.m_OpenFileDialog.FileName = "openFileDialog1";
            // 
            // frmEditMedia
            // 
            this.AcceptButton = cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = cmdCancel;
            this.ClientSize = new System.Drawing.Size(535, 508);
            this.Controls.Add(this.m_PictureBox);
            this.Controls.Add(oSplitter);
            this.Controls.Add(LeftPanel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmEditMedia";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Media";
            this.Load += new System.EventHandler(this.frmEditMedia_Load);
            LeftPanel.ResumeLayout(false);
            LeftPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_PictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox m_PictureBox;
        private System.Windows.Forms.CheckBox m_chkPrimary;
        private System.Windows.Forms.TextBox m_txtTitle;
        private System.Windows.Forms.TextBox m_txtHeight;
        private System.Windows.Forms.TextBox m_txtWidth;
        private System.Windows.Forms.CheckBox m_chkThumbnail;
        private System.Windows.Forms.TextBox m_txtFilename;
        private System.Windows.Forms.OpenFileDialog m_OpenFileDialog;
        private System.Windows.Forms.ListBox m_lstPeople;
        private System.Windows.Forms.ComboBox m_cboPeople;

    }
}