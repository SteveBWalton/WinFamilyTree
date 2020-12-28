using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;


namespace FamilyTree.Viewer
{
    public partial class frmViewTree : System.Windows.Forms.Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmViewTree));
            System.Windows.Forms.MenuStrip oMainMenu;
            System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
            System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
            System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
            System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
            System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
            System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem menuView;
            System.Windows.Forms.ToolStrip oToolBar;
            System.Windows.Forms.StatusStrip oStatusBar;
            System.Windows.Forms.ToolStripButton tsbSave;
            System.Windows.Forms.ToolStripButton tsbCopy;
            System.Windows.Forms.ToolStripButton tsbPrintPreview;
            System.Windows.Forms.ToolStripButton tsbMagnifyPlus;
            System.Windows.Forms.ToolStripButton tsbMagnifyMinus;
            System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem printPreviewToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem menuCopy;
            System.Windows.Forms.ToolStripMenuItem menuZoomIn;
            System.Windows.Forms.ToolStripMenuItem menuZoomOut;
            System.Windows.Forms.ToolStripMenuItem menuZoomReset;
            System.Windows.Forms.ToolStripButton tsbMagnifyReset;
            this.m_tsLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.m_oPrintDocument = new System.Drawing.Printing.PrintDocument();
            this.m_hScrollBar = new System.Windows.Forms.HScrollBar();
            this.m_vScrollBar = new System.Windows.Forms.VScrollBar();
            this.m_SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.m_PictureBox = new System.Windows.Forms.PictureBox();
            oImageList16x16 = new System.Windows.Forms.ImageList(this.components);
            oMainMenu = new System.Windows.Forms.MenuStrip();
            fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            menuView = new System.Windows.Forms.ToolStripMenuItem();
            oToolBar = new System.Windows.Forms.ToolStrip();
            oStatusBar = new System.Windows.Forms.StatusStrip();
            tsbSave = new System.Windows.Forms.ToolStripButton();
            tsbCopy = new System.Windows.Forms.ToolStripButton();
            tsbPrintPreview = new System.Windows.Forms.ToolStripButton();
            tsbMagnifyPlus = new System.Windows.Forms.ToolStripButton();
            tsbMagnifyMinus = new System.Windows.Forms.ToolStripButton();
            saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            printPreviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            menuCopy = new System.Windows.Forms.ToolStripMenuItem();
            menuZoomIn = new System.Windows.Forms.ToolStripMenuItem();
            menuZoomOut = new System.Windows.Forms.ToolStripMenuItem();
            menuZoomReset = new System.Windows.Forms.ToolStripMenuItem();
            tsbMagnifyReset = new System.Windows.Forms.ToolStripButton();
            oMainMenu.SuspendLayout();
            oToolBar.SuspendLayout();
            oStatusBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_PictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // oImageList16x16
            // 
            oImageList16x16.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("oImageList16x16.ImageStream")));
            oImageList16x16.TransparentColor = System.Drawing.Color.Silver;
            oImageList16x16.Images.SetKeyName(0, "");
            oImageList16x16.Images.SetKeyName(1, "");
            // 
            // oMainMenu
            // 
            oMainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            fileToolStripMenuItem,
            editToolStripMenuItem,
            menuView});
            oMainMenu.Location = new System.Drawing.Point(0, 0);
            oMainMenu.Name = "oMainMenu";
            oMainMenu.Size = new System.Drawing.Size(535, 24);
            oMainMenu.TabIndex = 5;
            oMainMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            saveToolStripMenuItem,
            toolStripMenuItem1,
            printPreviewToolStripMenuItem,
            printToolStripMenuItem,
            toolStripMenuItem2,
            closeToolStripMenuItem});
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new System.Drawing.Size(157, 6);
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new System.Drawing.Size(157, 6);
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            menuCopy,
            toolStripMenuItem3,
            optionsToolStripMenuItem});
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            editToolStripMenuItem.Text = "Edit";
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new System.Drawing.Size(131, 6);
            // 
            // optionsToolStripMenuItem
            // 
            optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            optionsToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            optionsToolStripMenuItem.Text = "Options...";
            optionsToolStripMenuItem.Click += new System.EventHandler(this.menuOptions_Click);
            // 
            // menuView
            // 
            menuView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            menuZoomIn,
            menuZoomOut,
            menuZoomReset});
            menuView.Name = "menuView";
            menuView.Size = new System.Drawing.Size(41, 20);
            menuView.Text = "View";
            // 
            // oToolBar
            // 
            oToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            tsbSave,
            tsbCopy,
            tsbPrintPreview,
            tsbMagnifyPlus,
            tsbMagnifyMinus,
            tsbMagnifyReset});
            oToolBar.Location = new System.Drawing.Point(0, 24);
            oToolBar.Name = "oToolBar";
            oToolBar.Size = new System.Drawing.Size(535, 25);
            oToolBar.TabIndex = 6;
            oToolBar.Text = "toolStrip1";
            // 
            // oStatusBar
            // 
            oStatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_tsLabel});
            oStatusBar.Location = new System.Drawing.Point(0, 331);
            oStatusBar.Name = "oStatusBar";
            oStatusBar.Size = new System.Drawing.Size(535, 22);
            oStatusBar.TabIndex = 7;
            oStatusBar.Text = "statusStrip1";
            // 
            // m_tsLabel
            // 
            this.m_tsLabel.Name = "m_tsLabel";
            this.m_tsLabel.Size = new System.Drawing.Size(520, 17);
            this.m_tsLabel.Spring = true;
            this.m_tsLabel.Text = "toolStripStatusLabel1";
            this.m_tsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_oPrintDocument
            // 
            this.m_oPrintDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.PrintDocument_PrintPage);
            // 
            // m_hScrollBar
            // 
            this.m_hScrollBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.m_hScrollBar.Location = new System.Drawing.Point(0, 313);
            this.m_hScrollBar.Name = "m_hScrollBar";
            this.m_hScrollBar.Size = new System.Drawing.Size(535, 18);
            this.m_hScrollBar.TabIndex = 3;
            this.m_hScrollBar.ValueChanged += new System.EventHandler(this.hScrollBar_ValueChanged);
            // 
            // m_vScrollBar
            // 
            this.m_vScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.m_vScrollBar.Location = new System.Drawing.Point(517, 49);
            this.m_vScrollBar.Name = "m_vScrollBar";
            this.m_vScrollBar.Size = new System.Drawing.Size(18, 264);
            this.m_vScrollBar.TabIndex = 4;
            this.m_vScrollBar.ValueChanged += new System.EventHandler(this.vScrollBar_ValueChanged);
            // 
            // m_PictureBox
            // 
            this.m_PictureBox.BackColor = System.Drawing.Color.White;
            this.m_PictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_PictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_PictureBox.Location = new System.Drawing.Point(0, 49);
            this.m_PictureBox.Name = "m_PictureBox";
            this.m_PictureBox.Size = new System.Drawing.Size(517, 264);
            this.m_PictureBox.TabIndex = 0;
            this.m_PictureBox.TabStop = false;
            this.m_PictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureBox_MouseMove);
            this.m_PictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureBox_MouseDown);
            this.m_PictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.PictureBox_Paint);
            this.m_PictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PictureBox_MouseUp);
            // 
            // tsbSave
            // 
            tsbSave.Image = global::FamilyTree.Viewer.Properties.Resources.Save;
            tsbSave.ImageTransparentColor = System.Drawing.Color.Silver;
            tsbSave.Name = "tsbSave";
            tsbSave.Size = new System.Drawing.Size(51, 22);
            tsbSave.Text = "Save";
            tsbSave.Click += new System.EventHandler(this.menuSave_Click);
            // 
            // tsbCopy
            // 
            tsbCopy.Image = global::FamilyTree.Viewer.Properties.Resources.Copy;
            tsbCopy.ImageTransparentColor = System.Drawing.Color.Silver;
            tsbCopy.Name = "tsbCopy";
            tsbCopy.Size = new System.Drawing.Size(52, 22);
            tsbCopy.Text = "Copy";
            tsbCopy.Click += new System.EventHandler(this.menuCopy_Click);
            // 
            // tsbPrintPreview
            // 
            tsbPrintPreview.Image = global::FamilyTree.Viewer.Properties.Resources.Preview;
            tsbPrintPreview.ImageTransparentColor = System.Drawing.Color.Silver;
            tsbPrintPreview.Name = "tsbPrintPreview";
            tsbPrintPreview.Size = new System.Drawing.Size(65, 22);
            tsbPrintPreview.Text = "Preview";
            tsbPrintPreview.Click += new System.EventHandler(this.menuPrintPreview_Click);
            // 
            // tsbMagnifyPlus
            // 
            tsbMagnifyPlus.Image = global::FamilyTree.Viewer.Properties.Resources.MagnifyPlus;
            tsbMagnifyPlus.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbMagnifyPlus.Name = "tsbMagnifyPlus";
            tsbMagnifyPlus.Size = new System.Drawing.Size(58, 22);
            tsbMagnifyPlus.Text = "Larger";
            tsbMagnifyPlus.Click += new System.EventHandler(this.menuZoomIn_Click);
            // 
            // tsbMagnifyMinus
            // 
            tsbMagnifyMinus.Image = global::FamilyTree.Viewer.Properties.Resources.MagnifyMinus;
            tsbMagnifyMinus.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbMagnifyMinus.Name = "tsbMagnifyMinus";
            tsbMagnifyMinus.Size = new System.Drawing.Size(61, 22);
            tsbMagnifyMinus.Text = "Smaller";
            tsbMagnifyMinus.Click += new System.EventHandler(this.menuZoomOut_Click);
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Image = global::FamilyTree.Viewer.Properties.Resources.Save;
            saveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Silver;
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            saveToolStripMenuItem.Text = "Save...";
            saveToolStripMenuItem.Click += new System.EventHandler(this.menuSave_Click);
            // 
            // printPreviewToolStripMenuItem
            // 
            printPreviewToolStripMenuItem.Image = global::FamilyTree.Viewer.Properties.Resources.Preview;
            printPreviewToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Silver;
            printPreviewToolStripMenuItem.Name = "printPreviewToolStripMenuItem";
            printPreviewToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            printPreviewToolStripMenuItem.Text = "Print Preview...";
            printPreviewToolStripMenuItem.Click += new System.EventHandler(this.menuPrintPreview_Click);
            // 
            // printToolStripMenuItem
            // 
            printToolStripMenuItem.Image = global::FamilyTree.Viewer.Properties.Resources.Print;
            printToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Silver;
            printToolStripMenuItem.Name = "printToolStripMenuItem";
            printToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            printToolStripMenuItem.Text = "Print";
            // 
            // closeToolStripMenuItem
            // 
            closeToolStripMenuItem.Image = global::FamilyTree.Viewer.Properties.Resources.Exit;
            closeToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Silver;
            closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            closeToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            closeToolStripMenuItem.Text = "Close";
            closeToolStripMenuItem.Click += new System.EventHandler(this.menuClose_Click);
            // 
            // menuCopy
            // 
            menuCopy.Image = global::FamilyTree.Viewer.Properties.Resources.Copy;
            menuCopy.ImageTransparentColor = System.Drawing.Color.Silver;
            menuCopy.Name = "menuCopy";
            menuCopy.Size = new System.Drawing.Size(134, 22);
            menuCopy.Text = "Copy";
            menuCopy.Click += new System.EventHandler(this.menuCopy_Click);
            // 
            // menuZoomIn
            // 
            menuZoomIn.Image = global::FamilyTree.Viewer.Properties.Resources.MagnifyPlus;
            menuZoomIn.Name = "menuZoomIn";
            menuZoomIn.Size = new System.Drawing.Size(152, 22);
            menuZoomIn.Text = "Zoom In";
            menuZoomIn.Click += new System.EventHandler(this.menuZoomIn_Click);
            // 
            // menuZoomOut
            // 
            menuZoomOut.Image = global::FamilyTree.Viewer.Properties.Resources.MagnifyMinus;
            menuZoomOut.Name = "menuZoomOut";
            menuZoomOut.Size = new System.Drawing.Size(152, 22);
            menuZoomOut.Text = "Zoom Out";
            menuZoomOut.Click += new System.EventHandler(this.menuZoomOut_Click);
            // 
            // menuZoomReset
            // 
            menuZoomReset.Image = global::FamilyTree.Viewer.Properties.Resources.OneHundredPercent;
            menuZoomReset.Name = "menuZoomReset";
            menuZoomReset.Size = new System.Drawing.Size(152, 22);
            menuZoomReset.Text = "Zoom Reset";
            menuZoomReset.Click += new System.EventHandler(this.menuZoomReset_Click);
            // 
            // tsbMagnifyReset
            // 
            tsbMagnifyReset.Image = global::FamilyTree.Viewer.Properties.Resources.OneHundredPercent;
            tsbMagnifyReset.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbMagnifyReset.Name = "tsbMagnifyReset";
            tsbMagnifyReset.Size = new System.Drawing.Size(55, 22);
            tsbMagnifyReset.Text = "Reset";
            tsbMagnifyReset.Click += new System.EventHandler(this.menuZoomReset_Click);
            // 
            // frmViewTree
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(535, 353);
            this.Controls.Add(this.m_PictureBox);
            this.Controls.Add(this.m_vScrollBar);
            this.Controls.Add(this.m_hScrollBar);
            this.Controls.Add(oToolBar);
            this.Controls.Add(oMainMenu);
            this.Controls.Add(oStatusBar);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = oMainMenu;
            this.Name = "frmViewTree";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmViewTree";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Shown += new System.EventHandler(this.frmViewTree_Shown);
            this.Resize += new System.EventHandler(this.frmViewTree_Resize);
            oMainMenu.ResumeLayout(false);
            oMainMenu.PerformLayout();
            oToolBar.ResumeLayout(false);
            oToolBar.PerformLayout();
            oStatusBar.ResumeLayout(false);
            oStatusBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_PictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.PictureBox m_PictureBox;
        private System.Windows.Forms.HScrollBar m_hScrollBar;
        private System.Windows.Forms.VScrollBar m_vScrollBar;
        private SaveFileDialog m_SaveFileDialog;
        private ToolStripStatusLabel m_tsLabel;
        private System.Drawing.Printing.PrintDocument m_oPrintDocument;


    }
}
