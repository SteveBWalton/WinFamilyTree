namespace FamilyTree.Viewer
{
    partial class frmTreeOptions
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
            System.Windows.Forms.ImageList oImageList16x16;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTreeOptions));
            this.panel1 = new System.Windows.Forms.Panel();
            this.m_cmdCancel = new System.Windows.Forms.Button();
            this.m_cmdOK = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.chkTreePersonBox = new System.Windows.Forms.CheckBox();
            this.cmdTreeSubFont = new System.Windows.Forms.Button();
            this.cmdTreeMainFont = new System.Windows.Forms.Button();
            this.labTreeSubFont = new System.Windows.Forms.Label();
            this.labTreeMainFont = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.m_cmdAdd = new System.Windows.Forms.Button();
            this.m_txtRuleParameter = new System.Windows.Forms.TextBox();
            this.m_cboRulePeople = new System.Windows.Forms.ComboBox();
            this.m_cboRules = new System.Windows.Forms.ComboBox();
            this.m_oRules = new System.Windows.Forms.WebBrowser();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            oImageList16x16 = new System.Windows.Forms.ImageList(this.components);
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // oImageList16x16
            // 
            oImageList16x16.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("oImageList16x16.ImageStream")));
            oImageList16x16.TransparentColor = System.Drawing.Color.Silver;
            oImageList16x16.Images.SetKeyName(0, "");
            oImageList16x16.Images.SetKeyName(1, "");
            oImageList16x16.Images.SetKeyName(2, "");
            oImageList16x16.Images.SetKeyName(3, "");
            oImageList16x16.Images.SetKeyName(4, "Arrow Red Up.bmp");
            oImageList16x16.Images.SetKeyName(5, "Font.bmp");
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.m_cmdCancel);
            this.panel1.Controls.Add(this.m_cmdOK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 440);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(618, 37);
            this.panel1.TabIndex = 0;
            // 
            // m_cmdCancel
            // 
            this.m_cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_cmdCancel.Image = global::FamilyTree.Viewer.Properties.Resources.Cancel;
            this.m_cmdCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.m_cmdCancel.Location = new System.Drawing.Point(436, 3);
            this.m_cmdCancel.Name = "m_cmdCancel";
            this.m_cmdCancel.Size = new System.Drawing.Size(85, 27);
            this.m_cmdCancel.TabIndex = 1;
            this.m_cmdCancel.Text = "Cancel";
            this.m_cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.m_cmdCancel.UseVisualStyleBackColor = true;
            // 
            // m_cmdOK
            // 
            this.m_cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.m_cmdOK.Image = global::FamilyTree.Viewer.Properties.Resources.OK;
            this.m_cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.m_cmdOK.Location = new System.Drawing.Point(529, 3);
            this.m_cmdOK.Name = "m_cmdOK";
            this.m_cmdOK.Size = new System.Drawing.Size(85, 27);
            this.m_cmdOK.TabIndex = 0;
            this.m_cmdOK.Text = "OK";
            this.m_cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.m_cmdOK.UseVisualStyleBackColor = true;
            this.m_cmdOK.Click += new System.EventHandler(this.m_cmdOK_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.ImageList = oImageList16x16;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(618, 440);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.chkTreePersonBox);
            this.tabPage1.Controls.Add(this.cmdTreeSubFont);
            this.tabPage1.Controls.Add(this.cmdTreeMainFont);
            this.tabPage1.Controls.Add(this.labTreeSubFont);
            this.tabPage1.Controls.Add(this.labTreeMainFont);
            this.tabPage1.ImageIndex = 3;
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(610, 413);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Appearance";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // chkTreePersonBox
            // 
            this.chkTreePersonBox.Location = new System.Drawing.Point(93, 243);
            this.chkTreePersonBox.Name = "chkTreePersonBox";
            this.chkTreePersonBox.Size = new System.Drawing.Size(224, 24);
            this.chkTreePersonBox.TabIndex = 9;
            this.chkTreePersonBox.Text = "Box around people";
            // 
            // cmdTreeSubFont
            // 
            this.cmdTreeSubFont.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cmdTreeSubFont.ImageIndex = 5;
            this.cmdTreeSubFont.ImageList = oImageList16x16;
            this.cmdTreeSubFont.Location = new System.Drawing.Point(93, 187);
            this.cmdTreeSubFont.Name = "cmdTreeSubFont";
            this.cmdTreeSubFont.Size = new System.Drawing.Size(85, 27);
            this.cmdTreeSubFont.TabIndex = 8;
            this.cmdTreeSubFont.Text = "Sub Font";
            this.cmdTreeSubFont.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdTreeSubFont.Click += new System.EventHandler(this.cmdTreeSubFont_Click);
            // 
            // cmdTreeMainFont
            // 
            this.cmdTreeMainFont.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cmdTreeMainFont.ImageIndex = 5;
            this.cmdTreeMainFont.ImageList = oImageList16x16;
            this.cmdTreeMainFont.Location = new System.Drawing.Point(93, 147);
            this.cmdTreeMainFont.Name = "cmdTreeMainFont";
            this.cmdTreeMainFont.Size = new System.Drawing.Size(85, 27);
            this.cmdTreeMainFont.TabIndex = 7;
            this.cmdTreeMainFont.Text = "Main Font";
            this.cmdTreeMainFont.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdTreeMainFont.Click += new System.EventHandler(this.cmdTreeMainFont_Click);
            // 
            // labTreeSubFont
            // 
            this.labTreeSubFont.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labTreeSubFont.Location = new System.Drawing.Point(181, 187);
            this.labTreeSubFont.Name = "labTreeSubFont";
            this.labTreeSubFont.Size = new System.Drawing.Size(336, 32);
            this.labTreeSubFont.TabIndex = 6;
            this.labTreeSubFont.Text = "Sub Font: Tahoma 8";
            this.labTreeSubFont.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labTreeMainFont
            // 
            this.labTreeMainFont.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labTreeMainFont.Location = new System.Drawing.Point(181, 147);
            this.labTreeMainFont.Name = "labTreeMainFont";
            this.labTreeMainFont.Size = new System.Drawing.Size(336, 32);
            this.labTreeMainFont.TabIndex = 5;
            this.labTreeMainFont.Text = "Main Font: Tahoma 11";
            this.labTreeMainFont.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.m_cmdAdd);
            this.tabPage2.Controls.Add(this.m_txtRuleParameter);
            this.tabPage2.Controls.Add(this.m_cboRulePeople);
            this.tabPage2.Controls.Add(this.m_cboRules);
            this.tabPage2.Controls.Add(this.m_oRules);
            this.tabPage2.ImageIndex = 2;
            this.tabPage2.Location = new System.Drawing.Point(4, 23);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(610, 413);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Rules";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // m_cmdAdd
            // 
            this.m_cmdAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.m_cmdAdd.ImageIndex = 4;
            this.m_cmdAdd.ImageList = oImageList16x16;
            this.m_cmdAdd.Location = new System.Drawing.Point(8, 351);
            this.m_cmdAdd.Name = "m_cmdAdd";
            this.m_cmdAdd.Size = new System.Drawing.Size(85, 27);
            this.m_cmdAdd.TabIndex = 4;
            this.m_cmdAdd.Text = "Add";
            this.m_cmdAdd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.m_cmdAdd.UseVisualStyleBackColor = true;
            this.m_cmdAdd.Click += new System.EventHandler(this.cmdAdd_Click);
            // 
            // m_txtRuleParameter
            // 
            this.m_txtRuleParameter.Location = new System.Drawing.Point(502, 384);
            this.m_txtRuleParameter.Name = "m_txtRuleParameter";
            this.m_txtRuleParameter.Size = new System.Drawing.Size(100, 21);
            this.m_txtRuleParameter.TabIndex = 3;
            // 
            // m_cboRulePeople
            // 
            this.m_cboRulePeople.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cboRulePeople.FormattingEnabled = true;
            this.m_cboRulePeople.Location = new System.Drawing.Point(270, 384);
            this.m_cboRulePeople.Name = "m_cboRulePeople";
            this.m_cboRulePeople.Size = new System.Drawing.Size(226, 21);
            this.m_cboRulePeople.TabIndex = 2;
            // 
            // m_cboRules
            // 
            this.m_cboRules.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cboRules.FormattingEnabled = true;
            this.m_cboRules.Location = new System.Drawing.Point(8, 384);
            this.m_cboRules.Name = "m_cboRules";
            this.m_cboRules.Size = new System.Drawing.Size(256, 21);
            this.m_cboRules.TabIndex = 1;
            // 
            // m_oRules
            // 
            this.m_oRules.Location = new System.Drawing.Point(8, 6);
            this.m_oRules.MinimumSize = new System.Drawing.Size(20, 20);
            this.m_oRules.Name = "m_oRules";
            this.m_oRules.Size = new System.Drawing.Size(594, 339);
            this.m_oRules.TabIndex = 0;
            // 
            // frmTreeOptions
            // 
            this.AcceptButton = this.m_cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.m_cmdCancel;
            this.ClientSize = new System.Drawing.Size(618, 477);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmTreeOptions";
            this.Text = "Tree Options";
            this.Load += new System.EventHandler(this.frmTreeOptions_Load);
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button m_cmdCancel;
        private System.Windows.Forms.Button m_cmdOK;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox chkTreePersonBox;
        private System.Windows.Forms.Button cmdTreeSubFont;
        private System.Windows.Forms.Button cmdTreeMainFont;
        private System.Windows.Forms.Label labTreeSubFont;
        private System.Windows.Forms.Label labTreeMainFont;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.WebBrowser m_oRules;
        private System.Windows.Forms.TextBox m_txtRuleParameter;
        private System.Windows.Forms.ComboBox m_cboRulePeople;
        private System.Windows.Forms.ComboBox m_cboRules;
        private System.Windows.Forms.Button m_cmdAdd;
    }
}