using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;

namespace FamilyTree.Viewer
{
    public partial class frmUserOptions : System.Windows.Forms.Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUserOptions));
            System.Windows.Forms.TabControl tabControl;
            System.Windows.Forms.TabPage tabMain;
            System.Windows.Forms.TabPage tabTree;
            System.Windows.Forms.Button cmdTreeSubFont;
            System.Windows.Forms.Button cmdTreeMainFont;
            System.Windows.Forms.Panel oBottomPanel;
            System.Windows.Forms.Button cmdCancel;
            System.Windows.Forms.Button cmdOK;
            this.m_cmdChangeHtmlFont = new System.Windows.Forms.Button();
            this.m_labHtmlStyleFont = new System.Windows.Forms.Label();
            this.m_cboFont = new System.Windows.Forms.ComboBox();
            this.m_WebBrowser = new System.Windows.Forms.WebBrowser();
            this.m_chkTreePersonBox = new System.Windows.Forms.CheckBox();
            this.m_labTreeSubFont = new System.Windows.Forms.Label();
            this.m_labTreeMainFont = new System.Windows.Forms.Label();
            this.m_FontDialog = new System.Windows.Forms.FontDialog();
            oImageList16x16 = new System.Windows.Forms.ImageList(this.components);
            tabControl = new System.Windows.Forms.TabControl();
            tabMain = new System.Windows.Forms.TabPage();
            tabTree = new System.Windows.Forms.TabPage();
            cmdTreeSubFont = new System.Windows.Forms.Button();
            cmdTreeMainFont = new System.Windows.Forms.Button();
            oBottomPanel = new System.Windows.Forms.Panel();
            cmdCancel = new System.Windows.Forms.Button();
            cmdOK = new System.Windows.Forms.Button();
            tabControl.SuspendLayout();
            tabMain.SuspendLayout();
            tabTree.SuspendLayout();
            oBottomPanel.SuspendLayout();
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
            tabMain.Controls.Add(this.m_labHtmlStyleFont);
            tabMain.Controls.Add(this.m_cboFont);
            tabMain.Controls.Add(this.m_WebBrowser);
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
            this.m_cmdChangeHtmlFont.Image = global::FamilyTree.Viewer.Properties.Resources.Font;
            this.m_cmdChangeHtmlFont.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.m_cmdChangeHtmlFont.Location = new System.Drawing.Point(8, 244);
            this.m_cmdChangeHtmlFont.Name = "m_cmdChangeHtmlFont";
            this.m_cmdChangeHtmlFont.Size = new System.Drawing.Size(147, 34);
            this.m_cmdChangeHtmlFont.TabIndex = 3;
            this.m_cmdChangeHtmlFont.Text = "Change...";
            this.m_cmdChangeHtmlFont.UseVisualStyleBackColor = true;
            this.m_cmdChangeHtmlFont.Click += new System.EventHandler(this.m_cmdChangeHtmlFont_Click);
            // 
            // m_labHtmlStyleFont
            // 
            this.m_labHtmlStyleFont.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_labHtmlStyleFont.Location = new System.Drawing.Point(161, 218);
            this.m_labHtmlStyleFont.Name = "m_labHtmlStyleFont";
            this.m_labHtmlStyleFont.Size = new System.Drawing.Size(423, 60);
            this.m_labHtmlStyleFont.TabIndex = 2;
            this.m_labHtmlStyleFont.Text = "Main Font: Tahoma 11";
            this.m_labHtmlStyleFont.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // m_cboFont
            // 
            this.m_cboFont.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cboFont.FormattingEnabled = true;
            this.m_cboFont.Location = new System.Drawing.Point(8, 218);
            this.m_cboFont.Name = "m_cboFont";
            this.m_cboFont.Size = new System.Drawing.Size(147, 21);
            this.m_cboFont.TabIndex = 1;
            this.m_cboFont.SelectedIndexChanged += new System.EventHandler(this.cboFont_SelectedIndexChanged);
            // 
            // m_WebBrowser
            // 
            this.m_WebBrowser.Location = new System.Drawing.Point(8, 3);
            this.m_WebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.m_WebBrowser.Name = "m_WebBrowser";
            this.m_WebBrowser.Size = new System.Drawing.Size(576, 209);
            this.m_WebBrowser.TabIndex = 0;
            this.m_WebBrowser.TabStop = false;
            // 
            // tabTree
            // 
            tabTree.Controls.Add(this.m_chkTreePersonBox);
            tabTree.Controls.Add(cmdTreeSubFont);
            tabTree.Controls.Add(cmdTreeMainFont);
            tabTree.Controls.Add(this.m_labTreeSubFont);
            tabTree.Controls.Add(this.m_labTreeMainFont);
            tabTree.ImageIndex = 3;
            tabTree.Location = new System.Drawing.Point(4, 23);
            tabTree.Name = "tabTree";
            tabTree.Size = new System.Drawing.Size(592, 373);
            tabTree.TabIndex = 0;
            tabTree.Text = "Tree";
            tabTree.UseVisualStyleBackColor = true;
            // 
            // m_chkTreePersonBox
            // 
            this.m_chkTreePersonBox.Location = new System.Drawing.Point(8, 112);
            this.m_chkTreePersonBox.Name = "m_chkTreePersonBox";
            this.m_chkTreePersonBox.Size = new System.Drawing.Size(224, 24);
            this.m_chkTreePersonBox.TabIndex = 4;
            this.m_chkTreePersonBox.Text = "Box around people";
            // 
            // cmdTreeSubFont
            // 
            cmdTreeSubFont.Image = global::FamilyTree.Viewer.Properties.Resources.Font;
            cmdTreeSubFont.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            cmdTreeSubFont.Location = new System.Drawing.Point(8, 56);
            cmdTreeSubFont.Name = "cmdTreeSubFont";
            cmdTreeSubFont.Size = new System.Drawing.Size(75, 27);
            cmdTreeSubFont.TabIndex = 3;
            cmdTreeSubFont.Text = "Sub Font";
            cmdTreeSubFont.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            cmdTreeSubFont.Click += new System.EventHandler(this.cmdTreeSubFont_Click);
            // 
            // cmdTreeMainFont
            // 
            cmdTreeMainFont.Image = global::FamilyTree.Viewer.Properties.Resources.Font;
            cmdTreeMainFont.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            cmdTreeMainFont.Location = new System.Drawing.Point(8, 16);
            cmdTreeMainFont.Name = "cmdTreeMainFont";
            cmdTreeMainFont.Size = new System.Drawing.Size(75, 27);
            cmdTreeMainFont.TabIndex = 2;
            cmdTreeMainFont.Text = "Main Font";
            cmdTreeMainFont.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            cmdTreeMainFont.Click += new System.EventHandler(this.cmdTreeMainFont_Click);
            // 
            // m_labTreeSubFont
            // 
            this.m_labTreeSubFont.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_labTreeSubFont.Location = new System.Drawing.Point(96, 56);
            this.m_labTreeSubFont.Name = "m_labTreeSubFont";
            this.m_labTreeSubFont.Size = new System.Drawing.Size(336, 32);
            this.m_labTreeSubFont.TabIndex = 1;
            this.m_labTreeSubFont.Text = "Sub Font: Tahoma 8";
            this.m_labTreeSubFont.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // m_labTreeMainFont
            // 
            this.m_labTreeMainFont.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_labTreeMainFont.Location = new System.Drawing.Point(96, 16);
            this.m_labTreeMainFont.Name = "m_labTreeMainFont";
            this.m_labTreeMainFont.Size = new System.Drawing.Size(336, 32);
            this.m_labTreeMainFont.TabIndex = 0;
            this.m_labTreeMainFont.Text = "Main Font: Tahoma 11";
            this.m_labTreeMainFont.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            cmdCancel.Image = global::FamilyTree.Viewer.Properties.Resources.Cancel;
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
            cmdOK.Image = global::FamilyTree.Viewer.Properties.Resources.OK;
            cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            cmdOK.Location = new System.Drawing.Point(521, 8);
            cmdOK.Name = "cmdOK";
            cmdOK.Size = new System.Drawing.Size(75, 27);
            cmdOK.TabIndex = 0;
            cmdOK.Text = "OK";
            cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frmUserOptions
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
            this.Name = "frmUserOptions";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "User Options";
            this.Load += new System.EventHandler(this.frmUserOptions_Load);
            tabControl.ResumeLayout(false);
            tabMain.ResumeLayout(false);
            tabTree.ResumeLayout(false);
            oBottomPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.FontDialog m_FontDialog;
        private System.Windows.Forms.Label m_labTreeMainFont;
        private System.Windows.Forms.Label m_labTreeSubFont;
        private System.Windows.Forms.CheckBox m_chkTreePersonBox;
        private Button m_cmdChangeHtmlFont;
        private Label m_labHtmlStyleFont;
        private WebBrowser m_WebBrowser;
        private ComboBox m_cboFont;

    }
}
