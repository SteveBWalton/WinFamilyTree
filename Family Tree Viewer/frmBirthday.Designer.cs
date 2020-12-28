namespace FamilyTree.Viewer
{
    partial class frmBirthday
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
            System.Windows.Forms.ImageList oImageList16;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBirthday));
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label6;
            System.Windows.Forms.Button cmdCopy;
            this.m_nudAge1 = new System.Windows.Forms.NumericUpDown();
            this.m_dtpDate1 = new System.Windows.Forms.DateTimePicker();
            this.m_dtpDate2 = new System.Windows.Forms.DateTimePicker();
            this.m_nudAge2 = new System.Windows.Forms.NumericUpDown();
            this.m_labReport = new System.Windows.Forms.Label();
            this.cmdOK = new System.Windows.Forms.Button();
            this.m_picTick1 = new System.Windows.Forms.PictureBox();
            this.m_picTick2 = new System.Windows.Forms.PictureBox();
            this.m_picTick3 = new System.Windows.Forms.PictureBox();
            this.m_dtpDate3 = new System.Windows.Forms.DateTimePicker();
            this.m_nudAge3 = new System.Windows.Forms.NumericUpDown();
            oImageList16 = new System.Windows.Forms.ImageList(this.components);
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            cmdCopy = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudAge1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudAge2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_picTick1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_picTick2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_picTick3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudAge3)).BeginInit();
            this.SuspendLayout();
            // 
            // oImageList16
            // 
            oImageList16.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("oImageList16.ImageStream")));
            oImageList16.TransparentColor = System.Drawing.Color.Silver;
            oImageList16.Images.SetKeyName(0, "");
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(64, 36);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(26, 13);
            label1.TabIndex = 11;
            label1.Text = "Age";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(150, 36);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(19, 13);
            label2.TabIndex = 12;
            label2.Text = "on";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(150, 73);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(19, 13);
            label3.TabIndex = 16;
            label3.Text = "on";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(64, 73);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(26, 13);
            label4.TabIndex = 15;
            label4.Text = "Age";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(150, 112);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(19, 13);
            label5.TabIndex = 23;
            label5.Text = "on";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(64, 112);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(26, 13);
            label6.TabIndex = 22;
            label6.Text = "Age";
            // 
            // cmdCopy
            // 
            cmdCopy.Image = global::FamilyTree.Viewer.Properties.Resources.Copy;
            cmdCopy.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            cmdCopy.Location = new System.Drawing.Point(169, 231);
            cmdCopy.Name = "cmdCopy";
            cmdCopy.Size = new System.Drawing.Size(100, 30);
            cmdCopy.TabIndex = 26;
            cmdCopy.Text = "Copy";
            cmdCopy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            cmdCopy.UseVisualStyleBackColor = true;
            cmdCopy.Click += new System.EventHandler(this.cmdCopy_Click);
            // 
            // m_nudAge1
            // 
            this.m_nudAge1.Location = new System.Drawing.Point(96, 34);
            this.m_nudAge1.Name = "m_nudAge1";
            this.m_nudAge1.Size = new System.Drawing.Size(48, 21);
            this.m_nudAge1.TabIndex = 10;
            this.m_nudAge1.ValueChanged += new System.EventHandler(this.CalculateBirthday);
            // 
            // m_dtpDate1
            // 
            this.m_dtpDate1.Location = new System.Drawing.Point(175, 32);
            this.m_dtpDate1.Name = "m_dtpDate1";
            this.m_dtpDate1.Size = new System.Drawing.Size(200, 21);
            this.m_dtpDate1.TabIndex = 13;
            this.m_dtpDate1.ValueChanged += new System.EventHandler(this.CalculateBirthday);
            // 
            // m_dtpDate2
            // 
            this.m_dtpDate2.Location = new System.Drawing.Point(175, 69);
            this.m_dtpDate2.Name = "m_dtpDate2";
            this.m_dtpDate2.Size = new System.Drawing.Size(200, 21);
            this.m_dtpDate2.TabIndex = 17;
            this.m_dtpDate2.ValueChanged += new System.EventHandler(this.CalculateBirthday);
            // 
            // m_nudAge2
            // 
            this.m_nudAge2.Location = new System.Drawing.Point(96, 71);
            this.m_nudAge2.Name = "m_nudAge2";
            this.m_nudAge2.Size = new System.Drawing.Size(48, 21);
            this.m_nudAge2.TabIndex = 14;
            this.m_nudAge2.ValueChanged += new System.EventHandler(this.CalculateBirthday);
            // 
            // m_labReport
            // 
            this.m_labReport.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_labReport.Location = new System.Drawing.Point(26, 148);
            this.m_labReport.Name = "m_labReport";
            this.m_labReport.Size = new System.Drawing.Size(349, 60);
            this.m_labReport.TabIndex = 18;
            this.m_labReport.Text = "label5";
            // 
            // cmdOK
            // 
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Image = global::FamilyTree.Viewer.Properties.Resources.OK;
            this.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cmdOK.Location = new System.Drawing.Point(275, 231);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(100, 30);
            this.cmdOK.TabIndex = 9;
            this.cmdOK.Text = "OK";
            this.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_picTick1
            // 
            this.m_picTick1.Image = global::FamilyTree.Viewer.Properties.Resources.Tick;
            this.m_picTick1.Location = new System.Drawing.Point(42, 34);
            this.m_picTick1.Name = "m_picTick1";
            this.m_picTick1.Size = new System.Drawing.Size(16, 16);
            this.m_picTick1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.m_picTick1.TabIndex = 19;
            this.m_picTick1.TabStop = false;
            // 
            // m_picTick2
            // 
            this.m_picTick2.Image = global::FamilyTree.Viewer.Properties.Resources.Tick;
            this.m_picTick2.Location = new System.Drawing.Point(42, 73);
            this.m_picTick2.Name = "m_picTick2";
            this.m_picTick2.Size = new System.Drawing.Size(16, 16);
            this.m_picTick2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.m_picTick2.TabIndex = 20;
            this.m_picTick2.TabStop = false;
            // 
            // m_picTick3
            // 
            this.m_picTick3.Image = global::FamilyTree.Viewer.Properties.Resources.Tick;
            this.m_picTick3.Location = new System.Drawing.Point(42, 112);
            this.m_picTick3.Name = "m_picTick3";
            this.m_picTick3.Size = new System.Drawing.Size(16, 16);
            this.m_picTick3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.m_picTick3.TabIndex = 25;
            this.m_picTick3.TabStop = false;
            // 
            // m_dtpDate3
            // 
            this.m_dtpDate3.Location = new System.Drawing.Point(175, 108);
            this.m_dtpDate3.Name = "m_dtpDate3";
            this.m_dtpDate3.Size = new System.Drawing.Size(200, 21);
            this.m_dtpDate3.TabIndex = 24;
            this.m_dtpDate3.ValueChanged += new System.EventHandler(this.CalculateBirthday);
            // 
            // m_nudAge3
            // 
            this.m_nudAge3.Location = new System.Drawing.Point(96, 110);
            this.m_nudAge3.Name = "m_nudAge3";
            this.m_nudAge3.Size = new System.Drawing.Size(48, 21);
            this.m_nudAge3.TabIndex = 21;
            this.m_nudAge3.ValueChanged += new System.EventHandler(this.CalculateBirthday);
            // 
            // frmBirthday
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(386, 273);
            this.Controls.Add(cmdCopy);
            this.Controls.Add(this.m_picTick3);
            this.Controls.Add(this.m_dtpDate3);
            this.Controls.Add(label5);
            this.Controls.Add(label6);
            this.Controls.Add(this.m_nudAge3);
            this.Controls.Add(this.m_picTick2);
            this.Controls.Add(this.m_picTick1);
            this.Controls.Add(this.m_labReport);
            this.Controls.Add(this.m_dtpDate2);
            this.Controls.Add(label3);
            this.Controls.Add(label4);
            this.Controls.Add(this.m_nudAge2);
            this.Controls.Add(this.m_dtpDate1);
            this.Controls.Add(label2);
            this.Controls.Add(label1);
            this.Controls.Add(this.m_nudAge1);
            this.Controls.Add(this.cmdOK);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmBirthday";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Birthday Range";
            ((System.ComponentModel.ISupportInitialize)(this.m_nudAge1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudAge2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_picTick1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_picTick2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_picTick3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudAge3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.NumericUpDown m_nudAge1;
        private System.Windows.Forms.DateTimePicker m_dtpDate1;
        private System.Windows.Forms.DateTimePicker m_dtpDate2;
        private System.Windows.Forms.NumericUpDown m_nudAge2;
        private System.Windows.Forms.Label m_labReport;
        private System.Windows.Forms.PictureBox m_picTick1;
        private System.Windows.Forms.PictureBox m_picTick2;
        private System.Windows.Forms.PictureBox m_picTick3;
        private System.Windows.Forms.DateTimePicker m_dtpDate3;
        private System.Windows.Forms.NumericUpDown m_nudAge3;
    }
}