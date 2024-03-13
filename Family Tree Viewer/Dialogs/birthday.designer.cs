namespace family_tree.viewer
{
    partial class BirthdayDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BirthdayDialog));
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label6;
            System.Windows.Forms.Button cmdCopy;
            this.nudAge1_ = new System.Windows.Forms.NumericUpDown();
            this.dtpDate1_ = new System.Windows.Forms.DateTimePicker();
            this.dtpDate2_ = new System.Windows.Forms.DateTimePicker();
            this.nudAge2_ = new System.Windows.Forms.NumericUpDown();
            this.labReport_ = new System.Windows.Forms.Label();
            this.cmdOK = new System.Windows.Forms.Button();
            this.picTick1_ = new System.Windows.Forms.PictureBox();
            this.picTick2_ = new System.Windows.Forms.PictureBox();
            this.picTick3_ = new System.Windows.Forms.PictureBox();
            this.dtpDate3_ = new System.Windows.Forms.DateTimePicker();
            this.nudAge3_ = new System.Windows.Forms.NumericUpDown();
            oImageList16 = new System.Windows.Forms.ImageList(this.components);
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            cmdCopy = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudAge1_)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAge2_)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTick1_)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTick2_)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTick3_)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAge3_)).BeginInit();
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
            cmdCopy.Image = global::family_tree.viewer.Properties.Resources.Copy;
            cmdCopy.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            cmdCopy.Location = new System.Drawing.Point(169, 231);
            cmdCopy.Name = "cmdCopy";
            cmdCopy.Size = new System.Drawing.Size(100, 30);
            cmdCopy.TabIndex = 26;
            cmdCopy.Text = "Copy";
            cmdCopy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            cmdCopy.UseVisualStyleBackColor = true;
            cmdCopy.Click += new System.EventHandler(this.cmdCopyClick);
            // 
            // m_nudAge1
            // 
            this.nudAge1_.Location = new System.Drawing.Point(96, 34);
            this.nudAge1_.Name = "m_nudAge1";
            this.nudAge1_.Size = new System.Drawing.Size(48, 21);
            this.nudAge1_.TabIndex = 10;
            this.nudAge1_.ValueChanged += new System.EventHandler(this.calculateBirthday);
            // 
            // m_dtpDate1
            // 
            this.dtpDate1_.Location = new System.Drawing.Point(175, 32);
            this.dtpDate1_.Name = "m_dtpDate1";
            this.dtpDate1_.Size = new System.Drawing.Size(200, 21);
            this.dtpDate1_.TabIndex = 13;
            this.dtpDate1_.ValueChanged += new System.EventHandler(this.calculateBirthday);
            // 
            // m_dtpDate2
            // 
            this.dtpDate2_.Location = new System.Drawing.Point(175, 69);
            this.dtpDate2_.Name = "m_dtpDate2";
            this.dtpDate2_.Size = new System.Drawing.Size(200, 21);
            this.dtpDate2_.TabIndex = 17;
            this.dtpDate2_.ValueChanged += new System.EventHandler(this.calculateBirthday);
            // 
            // m_nudAge2
            // 
            this.nudAge2_.Location = new System.Drawing.Point(96, 71);
            this.nudAge2_.Name = "m_nudAge2";
            this.nudAge2_.Size = new System.Drawing.Size(48, 21);
            this.nudAge2_.TabIndex = 14;
            this.nudAge2_.ValueChanged += new System.EventHandler(this.calculateBirthday);
            // 
            // m_labReport
            // 
            this.labReport_.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labReport_.Location = new System.Drawing.Point(26, 148);
            this.labReport_.Name = "m_labReport";
            this.labReport_.Size = new System.Drawing.Size(349, 60);
            this.labReport_.TabIndex = 18;
            this.labReport_.Text = "label5";
            // 
            // cmdOK
            // 
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Image = global::family_tree.viewer.Properties.Resources.OK;
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
            this.picTick1_.Image = global::family_tree.viewer.Properties.Resources.Tick;
            this.picTick1_.Location = new System.Drawing.Point(42, 34);
            this.picTick1_.Name = "m_picTick1";
            this.picTick1_.Size = new System.Drawing.Size(16, 16);
            this.picTick1_.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picTick1_.TabIndex = 19;
            this.picTick1_.TabStop = false;
            // 
            // m_picTick2
            // 
            this.picTick2_.Image = global::family_tree.viewer.Properties.Resources.Tick;
            this.picTick2_.Location = new System.Drawing.Point(42, 73);
            this.picTick2_.Name = "m_picTick2";
            this.picTick2_.Size = new System.Drawing.Size(16, 16);
            this.picTick2_.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picTick2_.TabIndex = 20;
            this.picTick2_.TabStop = false;
            // 
            // m_picTick3
            // 
            this.picTick3_.Image = global::family_tree.viewer.Properties.Resources.Tick;
            this.picTick3_.Location = new System.Drawing.Point(42, 112);
            this.picTick3_.Name = "m_picTick3";
            this.picTick3_.Size = new System.Drawing.Size(16, 16);
            this.picTick3_.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picTick3_.TabIndex = 25;
            this.picTick3_.TabStop = false;
            // 
            // m_dtpDate3
            // 
            this.dtpDate3_.Location = new System.Drawing.Point(175, 108);
            this.dtpDate3_.Name = "m_dtpDate3";
            this.dtpDate3_.Size = new System.Drawing.Size(200, 21);
            this.dtpDate3_.TabIndex = 24;
            this.dtpDate3_.ValueChanged += new System.EventHandler(this.calculateBirthday);
            // 
            // m_nudAge3
            // 
            this.nudAge3_.Location = new System.Drawing.Point(96, 110);
            this.nudAge3_.Name = "m_nudAge3";
            this.nudAge3_.Size = new System.Drawing.Size(48, 21);
            this.nudAge3_.TabIndex = 21;
            this.nudAge3_.ValueChanged += new System.EventHandler(this.calculateBirthday);
            // 
            // frmBirthday
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(386, 273);
            this.Controls.Add(cmdCopy);
            this.Controls.Add(this.picTick3_);
            this.Controls.Add(this.dtpDate3_);
            this.Controls.Add(label5);
            this.Controls.Add(label6);
            this.Controls.Add(this.nudAge3_);
            this.Controls.Add(this.picTick2_);
            this.Controls.Add(this.picTick1_);
            this.Controls.Add(this.labReport_);
            this.Controls.Add(this.dtpDate2_);
            this.Controls.Add(label3);
            this.Controls.Add(label4);
            this.Controls.Add(this.nudAge2_);
            this.Controls.Add(this.dtpDate1_);
            this.Controls.Add(label2);
            this.Controls.Add(label1);
            this.Controls.Add(this.nudAge1_);
            this.Controls.Add(this.cmdOK);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmBirthday";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Birthday Range";
            ((System.ComponentModel.ISupportInitialize)(this.nudAge1_)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAge2_)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTick1_)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTick2_)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTick3_)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAge3_)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.NumericUpDown nudAge1_;
        private System.Windows.Forms.DateTimePicker dtpDate1_;
        private System.Windows.Forms.DateTimePicker dtpDate2_;
        private System.Windows.Forms.NumericUpDown nudAge2_;
        private System.Windows.Forms.Label labReport_;
        private System.Windows.Forms.PictureBox picTick1_;
        private System.Windows.Forms.PictureBox picTick2_;
        private System.Windows.Forms.PictureBox picTick3_;
        private System.Windows.Forms.DateTimePicker dtpDate3_;
        private System.Windows.Forms.NumericUpDown nudAge3_;
    }
}