using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;

namespace family_tree.viewer
{
    public partial class UserOptionsDialog : System.Windows.Forms.Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ImageList oImageList16x16;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserOptionsDialog));
            System.Windows.Forms.TabControl tabControl;
            System.Windows.Forms.TabPage tabMain;
            System.Windows.Forms.TabPage tabTree;
            System.Windows.Forms.Button cmdTreeSubFont;
            System.Windows.Forms.Button cmdTreeMainFont;
            System.Windows.Forms.Panel oBottomPanel;
            System.Windows.Forms.Button cmdCancel;
            System.Windows.Forms.Button cmdOK;
            System.Windows.Forms.Label labelGoogleMaps;
            this.m_cmdChangeHtmlFont = new System.Windows.Forms.Button();
            this.labHtmlStyleFont_ = new System.Windows.Forms.Label();
            this.cboFont_ = new System.Windows.Forms.ComboBox();
            this.webBrowser_ = new System.Windows.Forms.WebBrowser();
            this.chkTreePersonBox_ = new System.Windows.Forms.CheckBox();
            this.labTreeSubFont_ = new System.Windows.Forms.Label();
            this.labTreeMainFont_ = new System.Windows.Forms.Label();
            this.fontDialog_ = new System.Windows.Forms.FontDialog();
            this.tabUser = new System.Windows.Forms.TabPage();
            this.textBoxGoogleMapsKey_ = new System.Windows.Forms.TextBox();
            oImageList16x16 = new System.Windows.Forms.ImageList(this.components);
            tabControl = new System.Windows.Forms.TabControl();
            tabMain = new System.Windows.Forms.TabPage();
            tabTree = new System.Windows.Forms.TabPage();
            cmdTreeSubFont = new System.Windows.Forms.Button();
            cmdTreeMainFont = new System.Windows.Forms.Button();
            oBottomPanel = new System.Windows.Forms.Panel();
            cmdCancel = new System.Windows.Forms.Button();
            cmdOK = new System.Windows.Forms.Button();
            labelGoogleMaps = new System.Windows.Forms.Label();
            tabControl.SuspendLayout();
            tabMain.SuspendLayout();
            tabTree.SuspendLayout();
            oBottomPanel.SuspendLayout();
            this.tabUser.SuspendLayout();
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
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabMain);
            tabControl.Controls.Add(tabTree);
            tabControl.Controls.Add(this.tabUser);
            tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControl.ImageList = oImageList16x16;
            tabControl.Location = new System.Drawing.Point(0, 0);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new System.Drawing.Size(600, 400);
            tabControl.TabIndex = 2;
            // 
            // tabMain
            // 
            tabMain.Controls.Add(this.m_cmdChangeHtmlFont);
            tabMain.Controls.Add(this.labHtmlStyleFont_);
            tabMain.Controls.Add(this.cboFont_);
            tabMain.Controls.Add(this.webBrowser_);
            tabMain.ImageIndex = 2;
            tabMain.Location = new System.Drawing.Point(4, 23);
            tabMain.Name = "tabMain";
            tabMain.Size = new System.Drawing.Size(592, 373);
            tabMain.TabIndex = 1;
            tabMain.Text = "Main";
            tabMain.UseVisualStyleBackColor = true;
            // 
            // m_cmdChangeHtmlFont
            // 
            this.m_cmdChangeHtmlFont.Image = global::family_tree.viewer.Properties.Resources.Font;
            this.m_cmdChangeHtmlFont.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.m_cmdChangeHtmlFont.Location = new System.Drawing.Point(8, 244);
            this.m_cmdChangeHtmlFont.Name = "m_cmdChangeHtmlFont";
            this.m_cmdChangeHtmlFont.Size = new System.Drawing.Size(147, 34);
            this.m_cmdChangeHtmlFont.TabIndex = 3;
            this.m_cmdChangeHtmlFont.Text = "Change...";
            this.m_cmdChangeHtmlFont.UseVisualStyleBackColor = true;
            this.m_cmdChangeHtmlFont.Click += new System.EventHandler(this.cmdChangeHtmlFontClick);
            // 
            // labHtmlStyleFont_
            // 
            this.labHtmlStyleFont_.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labHtmlStyleFont_.Location = new System.Drawing.Point(161, 218);
            this.labHtmlStyleFont_.Name = "labHtmlStyleFont_";
            this.labHtmlStyleFont_.Size = new System.Drawing.Size(423, 60);
            this.labHtmlStyleFont_.TabIndex = 2;
            this.labHtmlStyleFont_.Text = "Main Font: Tahoma 11";
            this.labHtmlStyleFont_.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboFont_
            // 
            this.cboFont_.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFont_.FormattingEnabled = true;
            this.cboFont_.Location = new System.Drawing.Point(8, 218);
            this.cboFont_.Name = "cboFont_";
            this.cboFont_.Size = new System.Drawing.Size(147, 21);
            this.cboFont_.TabIndex = 1;
            this.cboFont_.SelectedIndexChanged += new System.EventHandler(this.cboFontSelectedIndexChanged);
            // 
            // webBrowser_
            // 
            this.webBrowser_.Location = new System.Drawing.Point(8, 3);
            this.webBrowser_.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser_.Name = "webBrowser_";
            this.webBrowser_.Size = new System.Drawing.Size(576, 209);
            this.webBrowser_.TabIndex = 0;
            this.webBrowser_.TabStop = false;
            // 
            // tabTree
            // 
            tabTree.Controls.Add(this.chkTreePersonBox_);
            tabTree.Controls.Add(cmdTreeSubFont);
            tabTree.Controls.Add(cmdTreeMainFont);
            tabTree.Controls.Add(this.labTreeSubFont_);
            tabTree.Controls.Add(this.labTreeMainFont_);
            tabTree.ImageIndex = 3;
            tabTree.Location = new System.Drawing.Point(4, 23);
            tabTree.Name = "tabTree";
            tabTree.Size = new System.Drawing.Size(592, 373);
            tabTree.TabIndex = 0;
            tabTree.Text = "Tree";
            tabTree.UseVisualStyleBackColor = true;
            // 
            // chkTreePersonBox_
            // 
            this.chkTreePersonBox_.Location = new System.Drawing.Point(8, 112);
            this.chkTreePersonBox_.Name = "chkTreePersonBox_";
            this.chkTreePersonBox_.Size = new System.Drawing.Size(224, 24);
            this.chkTreePersonBox_.TabIndex = 4;
            this.chkTreePersonBox_.Text = "Box around people";
            // 
            // cmdTreeSubFont
            // 
            cmdTreeSubFont.Image = global::family_tree.viewer.Properties.Resources.Font;
            cmdTreeSubFont.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            cmdTreeSubFont.Location = new System.Drawing.Point(8, 56);
            cmdTreeSubFont.Name = "cmdTreeSubFont";
            cmdTreeSubFont.Size = new System.Drawing.Size(75, 27);
            cmdTreeSubFont.TabIndex = 3;
            cmdTreeSubFont.Text = "Sub Font";
            cmdTreeSubFont.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            cmdTreeSubFont.Click += new System.EventHandler(this.cmdTreeSubFontClick);
            // 
            // cmdTreeMainFont
            // 
            cmdTreeMainFont.Image = global::family_tree.viewer.Properties.Resources.Font;
            cmdTreeMainFont.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            cmdTreeMainFont.Location = new System.Drawing.Point(8, 16);
            cmdTreeMainFont.Name = "cmdTreeMainFont";
            cmdTreeMainFont.Size = new System.Drawing.Size(75, 27);
            cmdTreeMainFont.TabIndex = 2;
            cmdTreeMainFont.Text = "Main Font";
            cmdTreeMainFont.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            cmdTreeMainFont.Click += new System.EventHandler(this.cmdTreeMainFontClick);
            // 
            // labTreeSubFont_
            // 
            this.labTreeSubFont_.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labTreeSubFont_.Location = new System.Drawing.Point(96, 56);
            this.labTreeSubFont_.Name = "labTreeSubFont_";
            this.labTreeSubFont_.Size = new System.Drawing.Size(336, 32);
            this.labTreeSubFont_.TabIndex = 1;
            this.labTreeSubFont_.Text = "Sub Font: Tahoma 8";
            this.labTreeSubFont_.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labTreeMainFont_
            // 
            this.labTreeMainFont_.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labTreeMainFont_.Location = new System.Drawing.Point(96, 16);
            this.labTreeMainFont_.Name = "labTreeMainFont_";
            this.labTreeMainFont_.Size = new System.Drawing.Size(336, 32);
            this.labTreeMainFont_.TabIndex = 0;
            this.labTreeMainFont_.Text = "Main Font: Tahoma 11";
            this.labTreeMainFont_.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // oBottomPanel
            // 
            oBottomPanel.Controls.Add(cmdCancel);
            oBottomPanel.Controls.Add(cmdOK);
            oBottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            oBottomPanel.Location = new System.Drawing.Point(0, 400);
            oBottomPanel.Name = "oBottomPanel";
            oBottomPanel.Size = new System.Drawing.Size(600, 40);
            oBottomPanel.TabIndex = 3;
            // 
            // cmdCancel
            // 
            cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cmdCancel.Image = global::family_tree.viewer.Properties.Resources.Cancel;
            cmdCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            cmdCancel.Location = new System.Drawing.Point(440, 8);
            cmdCancel.Name = "cmdCancel";
            cmdCancel.Size = new System.Drawing.Size(75, 27);
            cmdCancel.TabIndex = 1;
            cmdCancel.Text = "Cancel";
            cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmdOK
            // 
            cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            cmdOK.Image = global::family_tree.viewer.Properties.Resources.OK;
            cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            cmdOK.Location = new System.Drawing.Point(521, 8);
            cmdOK.Name = "cmdOK";
            cmdOK.Size = new System.Drawing.Size(75, 27);
            cmdOK.TabIndex = 0;
            cmdOK.Text = "OK";
            cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabUser
            // 
            this.tabUser.Controls.Add(this.textBoxGoogleMapsKey_);
            this.tabUser.Controls.Add(labelGoogleMaps);
            this.tabUser.ImageIndex = 2;
            this.tabUser.Location = new System.Drawing.Point(4, 23);
            this.tabUser.Name = "tabUser";
            this.tabUser.Size = new System.Drawing.Size(592, 373);
            this.tabUser.TabIndex = 2;
            this.tabUser.Text = "User";
            this.tabUser.UseVisualStyleBackColor = true;
            // 
            // labelGoogleMaps
            // 
            labelGoogleMaps.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            labelGoogleMaps.Location = new System.Drawing.Point(10, 12);
            labelGoogleMaps.Name = "labelGoogleMaps";
            labelGoogleMaps.Size = new System.Drawing.Size(100, 21);
            labelGoogleMaps.TabIndex = 0;
            labelGoogleMaps.Text = "Google Maps Key";
            labelGoogleMaps.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxGoogleMapsKey
            // 
            this.textBoxGoogleMapsKey_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxGoogleMapsKey_.Location = new System.Drawing.Point(112, 12);
            this.textBoxGoogleMapsKey_.Name = "textBoxGoogleMapsKey";
            this.textBoxGoogleMapsKey_.Size = new System.Drawing.Size(472, 21);
            this.textBoxGoogleMapsKey_.TabIndex = 1;
            // 
            // UserOptionsDialog
            // 
            this.AcceptButton = cmdOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.CancelButton = cmdCancel;
            this.ClientSize = new System.Drawing.Size(600, 440);
            this.Controls.Add(tabControl);
            this.Controls.Add(oBottomPanel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserOptionsDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "User Options";
            this.Load += new System.EventHandler(this.frmUserOptionsLoad);
            tabControl.ResumeLayout(false);
            tabMain.ResumeLayout(false);
            tabTree.ResumeLayout(false);
            oBottomPanel.ResumeLayout(false);
            this.tabUser.ResumeLayout(false);
            this.tabUser.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.FontDialog fontDialog_;
        private System.Windows.Forms.Label labTreeMainFont_;
        private System.Windows.Forms.Label labTreeSubFont_;
        private System.Windows.Forms.CheckBox chkTreePersonBox_;
        private Button m_cmdChangeHtmlFont;
        private Label labHtmlStyleFont_;
        private WebBrowser webBrowser_;
        private ComboBox cboFont_;
        private TabPage tabUser;
        private TextBox textBoxGoogleMapsKey_;
    }
}
