namespace family_tree.viewer
{
    partial class EditPlaceDialog
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
            this.labName_ = new System.Windows.Forms.Label();
            this.cboType_ = new System.Windows.Forms.ComboBox();
            this.nudLatitude_ = new System.Windows.Forms.NumericUpDown();
            this.nudLongitude_ = new System.Windows.Forms.NumericUpDown();
            this.nudZoom_ = new System.Windows.Forms.NumericUpDown();
            this.txtPrivateComments_ = new System.Windows.Forms.TextBox();
            this.webBrowser_ = new System.Windows.Forms.WebBrowser();
            this.chkUseParentLocation_ = new System.Windows.Forms.CheckBox();
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
            ((System.ComponentModel.ISupportInitialize)(this.nudLatitude_)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLongitude_)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudZoom_)).BeginInit();
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
            this.labName_.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labName_.Location = new System.Drawing.Point(12, 9);
            this.labName_.Name = "m_labName";
            this.labName_.Size = new System.Drawing.Size(268, 23);
            this.labName_.TabIndex = 0;
            this.labName_.Text = "m_labName";
            this.labName_.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_cboType
            // 
            this.cboType_.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboType_.FormattingEnabled = true;
            this.cboType_.Items.AddRange(new object[] {
            "Place",
            "Address"});
            this.cboType_.Location = new System.Drawing.Point(92, 43);
            this.cboType_.Name = "m_cboType";
            this.cboType_.Size = new System.Drawing.Size(121, 21);
            this.cboType_.TabIndex = 1;
            // 
            // m_nudLatitude
            // 
            this.nudLatitude_.DecimalPlaces = 4;
            this.nudLatitude_.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudLatitude_.Location = new System.Drawing.Point(93, 70);
            this.nudLatitude_.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.nudLatitude_.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.nudLatitude_.Name = "m_nudLatitude";
            this.nudLatitude_.Size = new System.Drawing.Size(120, 21);
            this.nudLatitude_.TabIndex = 6;
            // 
            // m_nudLongitude
            // 
            this.nudLongitude_.DecimalPlaces = 4;
            this.nudLongitude_.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudLongitude_.Location = new System.Drawing.Point(93, 97);
            this.nudLongitude_.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.nudLongitude_.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.nudLongitude_.Name = "m_nudLongitude";
            this.nudLongitude_.Size = new System.Drawing.Size(120, 21);
            this.nudLongitude_.TabIndex = 9;
            // 
            // m_nudZoom
            // 
            this.nudZoom_.Location = new System.Drawing.Point(92, 124);
            this.nudZoom_.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudZoom_.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudZoom_.Name = "m_nudZoom";
            this.nudZoom_.Size = new System.Drawing.Size(120, 21);
            this.nudZoom_.TabIndex = 11;
            this.nudZoom_.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudZoom_.ValueChanged += new System.EventHandler(this.nudZoomValueChanged);
            // 
            // m_txtPrivateComments
            // 
            this.txtPrivateComments_.Location = new System.Drawing.Point(97, 303);
            this.txtPrivateComments_.Multiline = true;
            this.txtPrivateComments_.Name = "m_txtPrivateComments";
            this.txtPrivateComments_.Size = new System.Drawing.Size(556, 96);
            this.txtPrivateComments_.TabIndex = 12;
            // 
            // m_webBrowser
            // 
            this.webBrowser_.Location = new System.Drawing.Point(230, 49);
            this.webBrowser_.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser_.Name = "m_webBrowser";
            this.webBrowser_.ScrollBarsEnabled = false;
            this.webBrowser_.Size = new System.Drawing.Size(418, 227);
            this.webBrowser_.TabIndex = 13;
            // 
            // m_chkUseParentLocation
            // 
            this.chkUseParentLocation_.AutoSize = true;
            this.chkUseParentLocation_.Location = new System.Drawing.Point(92, 151);
            this.chkUseParentLocation_.Name = "m_chkUseParentLocation";
            this.chkUseParentLocation_.Size = new System.Drawing.Size(127, 17);
            this.chkUseParentLocation_.TabIndex = 15;
            this.chkUseParentLocation_.Text = "Use Parents Location";
            this.chkUseParentLocation_.UseVisualStyleBackColor = true;
            // 
            // m_cmdOK
            // 
            this.m_cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.m_cmdOK.Image = global::family_tree.viewer.Properties.Resources.OK;
            this.m_cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.m_cmdOK.Location = new System.Drawing.Point(548, 405);
            this.m_cmdOK.Name = "m_cmdOK";
            this.m_cmdOK.Size = new System.Drawing.Size(100, 30);
            this.m_cmdOK.TabIndex = 3;
            this.m_cmdOK.Text = "OK";
            this.m_cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.m_cmdOK.UseVisualStyleBackColor = true;
            this.m_cmdOK.Click += new System.EventHandler(this.cmdOkClick);
            // 
            // m_cmdCancel
            // 
            this.m_cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_cmdCancel.Image = global::family_tree.viewer.Properties.Resources.Cancel;
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
            cmdRight.Image = global::family_tree.viewer.Properties.Resources.Arrow_Blue_Right;
            cmdRight.Location = new System.Drawing.Point(183, 189);
            cmdRight.Name = "cmdRight";
            cmdRight.Size = new System.Drawing.Size(30, 30);
            cmdRight.TabIndex = 19;
            cmdRight.UseVisualStyleBackColor = true;
            cmdRight.Click += new System.EventHandler(this.cmdRightClick);
            // 
            // cmdDown
            // 
            cmdDown.Image = global::family_tree.viewer.Properties.Resources.Arrow_Blue_Down;
            cmdDown.Location = new System.Drawing.Point(147, 210);
            cmdDown.Name = "cmdDown";
            cmdDown.Size = new System.Drawing.Size(30, 30);
            cmdDown.TabIndex = 18;
            cmdDown.UseVisualStyleBackColor = true;
            cmdDown.Click += new System.EventHandler(this.cmdDownClick);
            // 
            // cmdUp
            // 
            cmdUp.Image = global::family_tree.viewer.Properties.Resources.Arrow_Blue_Up;
            cmdUp.Location = new System.Drawing.Point(147, 174);
            cmdUp.Name = "cmdUp";
            cmdUp.Size = new System.Drawing.Size(30, 30);
            cmdUp.TabIndex = 17;
            cmdUp.UseVisualStyleBackColor = true;
            cmdUp.Click += new System.EventHandler(this.cmdUpClick);
            // 
            // cmdLeft
            // 
            cmdLeft.Image = global::family_tree.viewer.Properties.Resources.Arrow_Blue_Left;
            cmdLeft.Location = new System.Drawing.Point(111, 189);
            cmdLeft.Name = "cmdLeft";
            cmdLeft.Size = new System.Drawing.Size(30, 30);
            cmdLeft.TabIndex = 16;
            cmdLeft.UseVisualStyleBackColor = true;
            cmdLeft.Click += new System.EventHandler(this.cmdLeftClick);
            // 
            // cmdRefresh
            // 
            cmdRefresh.Image = global::family_tree.viewer.Properties.Resources.refresh;
            cmdRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            cmdRefresh.Location = new System.Drawing.Point(113, 246);
            cmdRefresh.Name = "cmdRefresh";
            cmdRefresh.Size = new System.Drawing.Size(100, 30);
            cmdRefresh.TabIndex = 14;
            cmdRefresh.Text = "Refresh";
            cmdRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            cmdRefresh.UseVisualStyleBackColor = true;
            cmdRefresh.Click += new System.EventHandler(this.cmdRefreshClick);
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
            this.Controls.Add(this.chkUseParentLocation_);
            this.Controls.Add(cmdRefresh);
            this.Controls.Add(this.webBrowser_);
            this.Controls.Add(this.txtPrivateComments_);
            this.Controls.Add(this.nudZoom_);
            this.Controls.Add(label5);
            this.Controls.Add(this.nudLongitude_);
            this.Controls.Add(label4);
            this.Controls.Add(label3);
            this.Controls.Add(this.nudLatitude_);
            this.Controls.Add(label2);
            this.Controls.Add(this.m_cmdCancel);
            this.Controls.Add(this.m_cmdOK);
            this.Controls.Add(label1);
            this.Controls.Add(this.cboType_);
            this.Controls.Add(this.labName_);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmEditPlace";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Place";
            ((System.ComponentModel.ISupportInitialize)(this.nudLatitude_)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLongitude_)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudZoom_)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labName_;
        private System.Windows.Forms.ComboBox cboType_;
        private System.Windows.Forms.Button m_cmdOK;
        private System.Windows.Forms.Button m_cmdCancel;
        private System.Windows.Forms.NumericUpDown nudLatitude_;
        private System.Windows.Forms.NumericUpDown nudLongitude_;
        private System.Windows.Forms.NumericUpDown nudZoom_;
        private System.Windows.Forms.TextBox txtPrivateComments_;
        private System.Windows.Forms.WebBrowser webBrowser_;
        private System.Windows.Forms.CheckBox chkUseParentLocation_;
    }
}