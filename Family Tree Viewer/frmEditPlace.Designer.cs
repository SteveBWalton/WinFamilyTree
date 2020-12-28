namespace FamilyTree.Viewer
{
    partial class frmEditPlace
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
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Button cmdRight;
            System.Windows.Forms.Button cmdDown;
            System.Windows.Forms.Button cmdUp;
            System.Windows.Forms.Button cmdLeft;
            System.Windows.Forms.Button cmdRefresh;
            this.m_labName = new System.Windows.Forms.Label();
            this.m_cboType = new System.Windows.Forms.ComboBox();
            this.m_nudLatitude = new System.Windows.Forms.NumericUpDown();
            this.m_nudLongitude = new System.Windows.Forms.NumericUpDown();
            this.m_nudZoom = new System.Windows.Forms.NumericUpDown();
            this.m_txtPrivateComments = new System.Windows.Forms.TextBox();
            this.m_webBrowser = new System.Windows.Forms.WebBrowser();
            this.m_chkUseParentLocation = new System.Windows.Forms.CheckBox();
            this.m_cmdOK = new System.Windows.Forms.Button();
            this.m_cmdCancel = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            cmdRight = new System.Windows.Forms.Button();
            cmdDown = new System.Windows.Forms.Button();
            cmdUp = new System.Windows.Forms.Button();
            cmdLeft = new System.Windows.Forms.Button();
            cmdRefresh = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudLatitude)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudLongitude)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudZoom)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.Location = new System.Drawing.Point(12, 43);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(74, 21);
            label1.TabIndex = 2;
            label1.Text = "Type";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            label2.Location = new System.Drawing.Point(13, 70);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(74, 21);
            label2.TabIndex = 5;
            label2.Text = "Latitude";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            label3.Location = new System.Drawing.Point(16, 303);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(71, 37);
            label3.TabIndex = 7;
            label3.Text = "Private Comments";
            label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            label4.Location = new System.Drawing.Point(13, 97);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(74, 21);
            label4.TabIndex = 8;
            label4.Text = "Longitude";
            label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            label5.Location = new System.Drawing.Point(12, 124);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(74, 21);
            label5.TabIndex = 10;
            label5.Text = "Zoom";
            label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_labName
            // 
            this.m_labName.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_labName.Location = new System.Drawing.Point(12, 9);
            this.m_labName.Name = "m_labName";
            this.m_labName.Size = new System.Drawing.Size(268, 23);
            this.m_labName.TabIndex = 0;
            this.m_labName.Text = "m_labName";
            this.m_labName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_cboType
            // 
            this.m_cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cboType.FormattingEnabled = true;
            this.m_cboType.Items.AddRange(new object[] {
            "Place",
            "Address"});
            this.m_cboType.Location = new System.Drawing.Point(92, 43);
            this.m_cboType.Name = "m_cboType";
            this.m_cboType.Size = new System.Drawing.Size(121, 21);
            this.m_cboType.TabIndex = 1;
            // 
            // m_nudLatitude
            // 
            this.m_nudLatitude.DecimalPlaces = 4;
            this.m_nudLatitude.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.m_nudLatitude.Location = new System.Drawing.Point(93, 70);
            this.m_nudLatitude.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.m_nudLatitude.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.m_nudLatitude.Name = "m_nudLatitude";
            this.m_nudLatitude.Size = new System.Drawing.Size(120, 21);
            this.m_nudLatitude.TabIndex = 6;
            // 
            // m_nudLongitude
            // 
            this.m_nudLongitude.DecimalPlaces = 4;
            this.m_nudLongitude.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.m_nudLongitude.Location = new System.Drawing.Point(93, 97);
            this.m_nudLongitude.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.m_nudLongitude.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.m_nudLongitude.Name = "m_nudLongitude";
            this.m_nudLongitude.Size = new System.Drawing.Size(120, 21);
            this.m_nudLongitude.TabIndex = 9;
            // 
            // m_nudZoom
            // 
            this.m_nudZoom.Location = new System.Drawing.Point(92, 124);
            this.m_nudZoom.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.m_nudZoom.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_nudZoom.Name = "m_nudZoom";
            this.m_nudZoom.Size = new System.Drawing.Size(120, 21);
            this.m_nudZoom.TabIndex = 11;
            this.m_nudZoom.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_nudZoom.ValueChanged += new System.EventHandler(this.nudZoom_ValueChanged);
            // 
            // m_txtPrivateComments
            // 
            this.m_txtPrivateComments.Location = new System.Drawing.Point(97, 303);
            this.m_txtPrivateComments.Multiline = true;
            this.m_txtPrivateComments.Name = "m_txtPrivateComments";
            this.m_txtPrivateComments.Size = new System.Drawing.Size(556, 96);
            this.m_txtPrivateComments.TabIndex = 12;
            // 
            // m_webBrowser
            // 
            this.m_webBrowser.Location = new System.Drawing.Point(230, 49);
            this.m_webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.m_webBrowser.Name = "m_webBrowser";
            this.m_webBrowser.ScrollBarsEnabled = false;
            this.m_webBrowser.Size = new System.Drawing.Size(418, 227);
            this.m_webBrowser.TabIndex = 13;
            // 
            // m_chkUseParentLocation
            // 
            this.m_chkUseParentLocation.AutoSize = true;
            this.m_chkUseParentLocation.Location = new System.Drawing.Point(92, 151);
            this.m_chkUseParentLocation.Name = "m_chkUseParentLocation";
            this.m_chkUseParentLocation.Size = new System.Drawing.Size(127, 17);
            this.m_chkUseParentLocation.TabIndex = 15;
            this.m_chkUseParentLocation.Text = "Use Parents Location";
            this.m_chkUseParentLocation.UseVisualStyleBackColor = true;
            // 
            // m_cmdOK
            // 
            this.m_cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.m_cmdOK.Image = global::FamilyTree.Viewer.Properties.Resources.OK;
            this.m_cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.m_cmdOK.Location = new System.Drawing.Point(548, 405);
            this.m_cmdOK.Name = "m_cmdOK";
            this.m_cmdOK.Size = new System.Drawing.Size(100, 30);
            this.m_cmdOK.TabIndex = 3;
            this.m_cmdOK.Text = "OK";
            this.m_cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.m_cmdOK.UseVisualStyleBackColor = true;
            this.m_cmdOK.Click += new System.EventHandler(this.m_cmdOK_Click);
            // 
            // m_cmdCancel
            // 
            this.m_cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_cmdCancel.Image = global::FamilyTree.Viewer.Properties.Resources.Cancel;
            this.m_cmdCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.m_cmdCancel.Location = new System.Drawing.Point(442, 405);
            this.m_cmdCancel.Name = "m_cmdCancel";
            this.m_cmdCancel.Size = new System.Drawing.Size(100, 30);
            this.m_cmdCancel.TabIndex = 4;
            this.m_cmdCancel.Text = "Cancel";
            this.m_cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.m_cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdRight
            // 
            cmdRight.Image = global::FamilyTree.Viewer.Properties.Resources.Arrow_Blue_Right;
            cmdRight.Location = new System.Drawing.Point(183, 189);
            cmdRight.Name = "cmdRight";
            cmdRight.Size = new System.Drawing.Size(30, 30);
            cmdRight.TabIndex = 19;
            cmdRight.UseVisualStyleBackColor = true;
            cmdRight.Click += new System.EventHandler(this.cmdRight_Click);
            // 
            // cmdDown
            // 
            cmdDown.Image = global::FamilyTree.Viewer.Properties.Resources.Arrow_Blue_Down;
            cmdDown.Location = new System.Drawing.Point(147, 210);
            cmdDown.Name = "cmdDown";
            cmdDown.Size = new System.Drawing.Size(30, 30);
            cmdDown.TabIndex = 18;
            cmdDown.UseVisualStyleBackColor = true;
            cmdDown.Click += new System.EventHandler(this.cmdDown_Click);
            // 
            // cmdUp
            // 
            cmdUp.Image = global::FamilyTree.Viewer.Properties.Resources.Arrow_Blue_Up;
            cmdUp.Location = new System.Drawing.Point(147, 174);
            cmdUp.Name = "cmdUp";
            cmdUp.Size = new System.Drawing.Size(30, 30);
            cmdUp.TabIndex = 17;
            cmdUp.UseVisualStyleBackColor = true;
            cmdUp.Click += new System.EventHandler(this.cmdUp_Click);
            // 
            // cmdLeft
            // 
            cmdLeft.Image = global::FamilyTree.Viewer.Properties.Resources.Arrow_Blue_Left;
            cmdLeft.Location = new System.Drawing.Point(111, 189);
            cmdLeft.Name = "cmdLeft";
            cmdLeft.Size = new System.Drawing.Size(30, 30);
            cmdLeft.TabIndex = 16;
            cmdLeft.UseVisualStyleBackColor = true;
            cmdLeft.Click += new System.EventHandler(this.cmdLeft_Click);
            // 
            // cmdRefresh
            // 
            cmdRefresh.Image = global::FamilyTree.Viewer.Properties.Resources.refresh;
            cmdRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            cmdRefresh.Location = new System.Drawing.Point(113, 246);
            cmdRefresh.Name = "cmdRefresh";
            cmdRefresh.Size = new System.Drawing.Size(100, 30);
            cmdRefresh.TabIndex = 14;
            cmdRefresh.Text = "Refresh";
            cmdRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            cmdRefresh.UseVisualStyleBackColor = true;
            cmdRefresh.Click += new System.EventHandler(this.cmdRefresh_Click);
            // 
            // frmEditPlace
            // 
            this.AcceptButton = this.m_cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.m_cmdCancel;
            this.ClientSize = new System.Drawing.Size(665, 447);
            this.Controls.Add(cmdRight);
            this.Controls.Add(cmdDown);
            this.Controls.Add(cmdUp);
            this.Controls.Add(cmdLeft);
            this.Controls.Add(this.m_chkUseParentLocation);
            this.Controls.Add(cmdRefresh);
            this.Controls.Add(this.m_webBrowser);
            this.Controls.Add(this.m_txtPrivateComments);
            this.Controls.Add(this.m_nudZoom);
            this.Controls.Add(label5);
            this.Controls.Add(this.m_nudLongitude);
            this.Controls.Add(label4);
            this.Controls.Add(label3);
            this.Controls.Add(this.m_nudLatitude);
            this.Controls.Add(label2);
            this.Controls.Add(this.m_cmdCancel);
            this.Controls.Add(this.m_cmdOK);
            this.Controls.Add(label1);
            this.Controls.Add(this.m_cboType);
            this.Controls.Add(this.m_labName);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmEditPlace";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Place";
            ((System.ComponentModel.ISupportInitialize)(this.m_nudLatitude)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudLongitude)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudZoom)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label m_labName;
        private System.Windows.Forms.ComboBox m_cboType;
        private System.Windows.Forms.Button m_cmdOK;
        private System.Windows.Forms.Button m_cmdCancel;
        private System.Windows.Forms.NumericUpDown m_nudLatitude;
        private System.Windows.Forms.NumericUpDown m_nudLongitude;
        private System.Windows.Forms.NumericUpDown m_nudZoom;
        private System.Windows.Forms.TextBox m_txtPrivateComments;
        private System.Windows.Forms.WebBrowser m_webBrowser;
        private System.Windows.Forms.CheckBox m_chkUseParentLocation;
    }
}