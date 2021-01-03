namespace FamilyTree.Viewer
{
    partial class frmSelectLocation
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
            System.Windows.Forms.Button cmdCancel;
            System.Windows.Forms.Button cmdOK;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSelectLocation));
            this.treeView_ = new System.Windows.Forms.TreeView();
            this.txtLocation_ = new System.Windows.Forms.TextBox();
            this.imageList_ = new System.Windows.Forms.ImageList(this.components);
            cmdCancel = new System.Windows.Forms.Button();
            cmdOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cmdCancel.Image = global::FamilyTree.Viewer.Properties.Resources.Cancel;
            cmdCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            cmdCancel.Location = new System.Drawing.Point(74, 383);
            cmdCancel.Name = "cmdCancel";
            cmdCancel.Size = new System.Drawing.Size(100, 30);
            cmdCancel.TabIndex = 6;
            cmdCancel.Text = "Cancel";
            cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmdOK
            // 
            cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            cmdOK.Image = global::FamilyTree.Viewer.Properties.Resources.OK;
            cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            cmdOK.Location = new System.Drawing.Point(180, 383);
            cmdOK.Name = "cmdOK";
            cmdOK.Size = new System.Drawing.Size(100, 30);
            cmdOK.TabIndex = 5;
            cmdOK.Text = "OK";
            cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // m_TreeView
            // 
            this.treeView_.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView_.FullRowSelect = true;
            this.treeView_.HideSelection = false;
            this.treeView_.ImageIndex = 0;
            this.treeView_.ImageList = this.imageList_;
            this.treeView_.Location = new System.Drawing.Point(12, 39);
            this.treeView_.Name = "m_TreeView";
            this.treeView_.SelectedImageIndex = 0;
            this.treeView_.Size = new System.Drawing.Size(268, 338);
            this.treeView_.TabIndex = 7;
            this.treeView_.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            // 
            // m_txtLocation
            // 
            this.txtLocation_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLocation_.Location = new System.Drawing.Point(12, 12);
            this.txtLocation_.Name = "m_txtLocation";
            this.txtLocation_.Size = new System.Drawing.Size(268, 21);
            this.txtLocation_.TabIndex = 8;
            // 
            // imageList1
            // 
            this.imageList_.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList_.TransparentColor = System.Drawing.Color.Silver;
            this.imageList_.Images.SetKeyName(0, "Earth.png");
            this.imageList_.Images.SetKeyName(1, "Home.png");
            // 
            // frmSelectLocation
            // 
            this.AcceptButton = cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = cmdCancel;
            this.ClientSize = new System.Drawing.Size(292, 425);
            this.Controls.Add(this.txtLocation_);
            this.Controls.Add(this.treeView_);
            this.Controls.Add(cmdCancel);
            this.Controls.Add(cmdOK);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmSelectLocation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Location";
            this.Shown += new System.EventHandler(this.frmSelectLocation_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView_;
        private System.Windows.Forms.TextBox txtLocation_;
        private System.Windows.Forms.ImageList imageList_;
    }
}