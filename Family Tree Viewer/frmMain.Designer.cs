using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Text;

// Family tree objects
using FamilyTree.Objects;

namespace FamilyTree.Viewer
{
    public partial class frmMain : System.Windows.Forms.Form
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
            System.Windows.Forms.ToolStripMenuItem menuExportGedcom;
            System.Windows.Forms.ToolStripSeparator toolStripMenuItem9;
            System.Windows.Forms.ToolStripSeparator toolStripMenuItem10;
            System.Windows.Forms.ToolStripSeparator toolStripMenuItem11;
            System.Windows.Forms.ToolStripMenuItem editCensusRecordsToolStripMenuItem;
            System.Windows.Forms.ToolStripSeparator toolStripMenuItem12;
            System.Windows.Forms.ToolStripMenuItem addFatherToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem addMotherToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem addSiblingToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem addChildToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem addPartnerToolStripMenuItem;
            System.Windows.Forms.ToolStripSeparator toolStripMenuItem13;
            System.Windows.Forms.ToolStripMenuItem menuUnlinkedPlaces;
            System.Windows.Forms.ToolStripMenuItem menuRecentChanges;
            System.Windows.Forms.ToolStripSeparator toolStripMenuItem14;
            System.Windows.Forms.ImageList oImageList16x16;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            System.Windows.Forms.ToolStripMenuItem menuAddMedia;
            System.Windows.Forms.ToolStripSeparator toolStripMenuItem15;
            System.Windows.Forms.ToolStripSeparator toolStripMenuItem16;
            System.Windows.Forms.ToolStripMenuItem menuHtmlSource;
            System.Windows.Forms.ToolStripMenuItem menuToDo;
            System.Windows.Forms.ToolStrip oToolStrip;
            System.Windows.Forms.ToolStripButton tsbHome;
            System.Windows.Forms.ToolStripButton tsbGoto;
            System.Windows.Forms.ToolStripButton tsbEdit;
            System.Windows.Forms.ToolStripButton tsbSources;
            System.Windows.Forms.ToolStripButton tsbTree;
            System.Windows.Forms.ToolStripButton tsbReport;
            System.Windows.Forms.ToolStripButton tsbAge;
            System.Windows.Forms.ToolStripButton tsbReduceWidth;
            System.Windows.Forms.MenuStrip oMainMenu;
            System.Windows.Forms.ToolStripMenuItem menuFile;
            System.Windows.Forms.ToolStripMenuItem menuOpen;
            System.Windows.Forms.ToolStripMenuItem menuExportSQLScript;
            System.Windows.Forms.ToolStripMenuItem menuOpenTree;
            System.Windows.Forms.ToolStripMenuItem menuHome;
            System.Windows.Forms.ToolStripMenuItem menuExit;
            System.Windows.Forms.ToolStripMenuItem menuEdit;
            System.Windows.Forms.ToolStripMenuItem menuEditCurrent;
            System.Windows.Forms.ToolStripMenuItem editSourcesToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem menuUserOptions;
            System.Windows.Forms.ToolStripMenuItem menuView;
            System.Windows.Forms.ToolStripMenuItem menuGoto;
            System.Windows.Forms.ToolStripMenuItem menuAge;
            System.Windows.Forms.ToolStripMenuItem menuBirthday;
            System.Windows.Forms.ToolStripMenuItem menuReduceWidth;
            System.Windows.Forms.ToolStripMenuItem menuStandardWidth;
            System.Windows.Forms.ToolStripMenuItem menuReports;
            System.Windows.Forms.ToolStripMenuItem menuToTree;
            System.Windows.Forms.ToolStripMenuItem menuReportToHtml;
            System.Windows.Forms.StatusStrip oStatusStrip;
            System.Windows.Forms.ToolStripStatusLabel tslabUserName;
            this.m_tsddbBack = new System.Windows.Forms.ToolStripDropDownButton();
            this.m_tsbBack = new System.Windows.Forms.ToolStripButton();
            this.m_tsddbForward = new System.Windows.Forms.ToolStripDropDownButton();
            this.m_tsbForward = new System.Windows.Forms.ToolStripButton();
            this.m_tsbImage = new System.Windows.Forms.ToolStripButton();
            this.m_tsbLocation = new System.Windows.Forms.ToolStripButton();
            this.m_menuRecentFile1 = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuRecentFile2 = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuRecentFile3 = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuRecentFile4 = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuImage = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuLocation = new System.Windows.Forms.ToolStripMenuItem();
            this.m_tslabStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.m_tsProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.m_labPerson = new System.Windows.Forms.Label();
            this.m_labPersonDates = new System.Windows.Forms.Label();
            this.m_OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.m_SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.m_oPanelTree = new System.Windows.Forms.Panel();
            this.m_marMotherParents = new FamilyTree.Viewer.ucRelationship();
            this.m_marFatherParents = new FamilyTree.Viewer.ucRelationship();
            this.m_marParents = new FamilyTree.Viewer.ucRelationship();
            this.m_psnFatherFather = new FamilyTree.Viewer.ucPerson();
            this.m_psnMother = new FamilyTree.Viewer.ucPerson();
            this.m_psnMotherMother = new FamilyTree.Viewer.ucPerson();
            this.m_psnFather = new FamilyTree.Viewer.ucPerson();
            this.m_psnFatherMother = new FamilyTree.Viewer.ucPerson();
            this.m_psnMotherFather = new FamilyTree.Viewer.ucPerson();
            this.m_oWebBrowser = new System.Windows.Forms.WebBrowser();
            menuExportGedcom = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem9 = new System.Windows.Forms.ToolStripSeparator();
            toolStripMenuItem10 = new System.Windows.Forms.ToolStripSeparator();
            toolStripMenuItem11 = new System.Windows.Forms.ToolStripSeparator();
            editCensusRecordsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem12 = new System.Windows.Forms.ToolStripSeparator();
            addFatherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            addMotherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            addSiblingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            addChildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            addPartnerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem13 = new System.Windows.Forms.ToolStripSeparator();
            menuUnlinkedPlaces = new System.Windows.Forms.ToolStripMenuItem();
            menuRecentChanges = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem14 = new System.Windows.Forms.ToolStripSeparator();
            oImageList16x16 = new System.Windows.Forms.ImageList(this.components);
            menuAddMedia = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem15 = new System.Windows.Forms.ToolStripSeparator();
            toolStripMenuItem16 = new System.Windows.Forms.ToolStripSeparator();
            menuHtmlSource = new System.Windows.Forms.ToolStripMenuItem();
            menuToDo = new System.Windows.Forms.ToolStripMenuItem();
            oToolStrip = new System.Windows.Forms.ToolStrip();
            tsbHome = new System.Windows.Forms.ToolStripButton();
            tsbGoto = new System.Windows.Forms.ToolStripButton();
            tsbEdit = new System.Windows.Forms.ToolStripButton();
            tsbSources = new System.Windows.Forms.ToolStripButton();
            tsbTree = new System.Windows.Forms.ToolStripButton();
            tsbReport = new System.Windows.Forms.ToolStripButton();
            tsbAge = new System.Windows.Forms.ToolStripButton();
            tsbReduceWidth = new System.Windows.Forms.ToolStripButton();
            oMainMenu = new System.Windows.Forms.MenuStrip();
            menuFile = new System.Windows.Forms.ToolStripMenuItem();
            menuOpen = new System.Windows.Forms.ToolStripMenuItem();
            menuExportSQLScript = new System.Windows.Forms.ToolStripMenuItem();
            menuOpenTree = new System.Windows.Forms.ToolStripMenuItem();
            menuHome = new System.Windows.Forms.ToolStripMenuItem();
            menuExit = new System.Windows.Forms.ToolStripMenuItem();
            menuEdit = new System.Windows.Forms.ToolStripMenuItem();
            menuEditCurrent = new System.Windows.Forms.ToolStripMenuItem();
            editSourcesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            menuUserOptions = new System.Windows.Forms.ToolStripMenuItem();
            menuView = new System.Windows.Forms.ToolStripMenuItem();
            menuGoto = new System.Windows.Forms.ToolStripMenuItem();
            menuAge = new System.Windows.Forms.ToolStripMenuItem();
            menuBirthday = new System.Windows.Forms.ToolStripMenuItem();
            menuReduceWidth = new System.Windows.Forms.ToolStripMenuItem();
            menuStandardWidth = new System.Windows.Forms.ToolStripMenuItem();
            menuReports = new System.Windows.Forms.ToolStripMenuItem();
            menuToTree = new System.Windows.Forms.ToolStripMenuItem();
            menuReportToHtml = new System.Windows.Forms.ToolStripMenuItem();
            oStatusStrip = new System.Windows.Forms.StatusStrip();
            tslabUserName = new System.Windows.Forms.ToolStripStatusLabel();
            oToolStrip.SuspendLayout();
            oMainMenu.SuspendLayout();
            oStatusStrip.SuspendLayout();
            this.m_oPanelTree.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuExportGedcom
            // 
            menuExportGedcom.Name = "menuExportGedcom";
            menuExportGedcom.Size = new System.Drawing.Size(180, 22);
            menuExportGedcom.Text = "Export Gedcom...";
            menuExportGedcom.Click += new System.EventHandler(this.menuExportGedcom_Click);
            // 
            // toolStripMenuItem9
            // 
            toolStripMenuItem9.Name = "toolStripMenuItem9";
            toolStripMenuItem9.Size = new System.Drawing.Size(177, 6);
            // 
            // toolStripMenuItem10
            // 
            toolStripMenuItem10.Name = "toolStripMenuItem10";
            toolStripMenuItem10.Size = new System.Drawing.Size(177, 6);
            // 
            // toolStripMenuItem11
            // 
            toolStripMenuItem11.Name = "toolStripMenuItem11";
            toolStripMenuItem11.Size = new System.Drawing.Size(177, 6);
            // 
            // editCensusRecordsToolStripMenuItem
            // 
            editCensusRecordsToolStripMenuItem.Name = "editCensusRecordsToolStripMenuItem";
            editCensusRecordsToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            editCensusRecordsToolStripMenuItem.Text = "Edit Census Records...";
            editCensusRecordsToolStripMenuItem.Click += new System.EventHandler(this.menuCensus_Click);
            // 
            // toolStripMenuItem12
            // 
            toolStripMenuItem12.Name = "toolStripMenuItem12";
            toolStripMenuItem12.Size = new System.Drawing.Size(197, 6);
            // 
            // addFatherToolStripMenuItem
            // 
            addFatherToolStripMenuItem.Name = "addFatherToolStripMenuItem";
            addFatherToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            addFatherToolStripMenuItem.Text = "Add Father...";
            addFatherToolStripMenuItem.Click += new System.EventHandler(this.menuAddFather_Click);
            // 
            // addMotherToolStripMenuItem
            // 
            addMotherToolStripMenuItem.Name = "addMotherToolStripMenuItem";
            addMotherToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            addMotherToolStripMenuItem.Text = "Add Mother...";
            addMotherToolStripMenuItem.Click += new System.EventHandler(this.menuAddMother_Click);
            // 
            // addSiblingToolStripMenuItem
            // 
            addSiblingToolStripMenuItem.Name = "addSiblingToolStripMenuItem";
            addSiblingToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            addSiblingToolStripMenuItem.Text = "Add Sibling...";
            addSiblingToolStripMenuItem.Click += new System.EventHandler(this.menuAddSibling_Click);
            // 
            // addChildToolStripMenuItem
            // 
            addChildToolStripMenuItem.Name = "addChildToolStripMenuItem";
            addChildToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            addChildToolStripMenuItem.Text = "Add Child...";
            addChildToolStripMenuItem.Click += new System.EventHandler(this.menuAddChild_Click);
            // 
            // addPartnerToolStripMenuItem
            // 
            addPartnerToolStripMenuItem.Name = "addPartnerToolStripMenuItem";
            addPartnerToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            addPartnerToolStripMenuItem.Text = "Add Partner...";
            addPartnerToolStripMenuItem.Click += new System.EventHandler(this.menuAddPartner_Click);
            // 
            // toolStripMenuItem13
            // 
            toolStripMenuItem13.Name = "toolStripMenuItem13";
            toolStripMenuItem13.Size = new System.Drawing.Size(197, 6);
            // 
            // menuUnlinkedPlaces
            // 
            menuUnlinkedPlaces.Name = "menuUnlinkedPlaces";
            menuUnlinkedPlaces.Size = new System.Drawing.Size(200, 22);
            menuUnlinkedPlaces.Text = "Remove Unlinked Places";
            menuUnlinkedPlaces.Click += new System.EventHandler(this.menuUnlinkedPlaces_Click);
            // 
            // menuRecentChanges
            // 
            menuRecentChanges.Name = "menuRecentChanges";
            menuRecentChanges.Size = new System.Drawing.Size(218, 22);
            menuRecentChanges.Text = "Recent Changes";
            menuRecentChanges.Click += new System.EventHandler(this.menuRecentChanges_Click);
            // 
            // toolStripMenuItem14
            // 
            toolStripMenuItem14.Name = "toolStripMenuItem14";
            toolStripMenuItem14.Size = new System.Drawing.Size(215, 6);
            // 
            // oImageList16x16
            // 
            oImageList16x16.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("oImageList16x16.ImageStream")));
            oImageList16x16.TransparentColor = System.Drawing.Color.Silver;
            oImageList16x16.Images.SetKeyName(0, "");
            oImageList16x16.Images.SetKeyName(1, "");
            oImageList16x16.Images.SetKeyName(2, "");
            oImageList16x16.Images.SetKeyName(3, "");
            oImageList16x16.Images.SetKeyName(4, "");
            oImageList16x16.Images.SetKeyName(5, "");
            oImageList16x16.Images.SetKeyName(6, "");
            oImageList16x16.Images.SetKeyName(7, "");
            oImageList16x16.Images.SetKeyName(8, "");
            oImageList16x16.Images.SetKeyName(9, "");
            // 
            // menuAddMedia
            // 
            menuAddMedia.Name = "menuAddMedia";
            menuAddMedia.Size = new System.Drawing.Size(200, 22);
            menuAddMedia.Text = "Add Media...";
            menuAddMedia.Click += new System.EventHandler(this.menuAddMedia_Click);
            // 
            // toolStripMenuItem15
            // 
            toolStripMenuItem15.Name = "toolStripMenuItem15";
            toolStripMenuItem15.Size = new System.Drawing.Size(197, 6);
            // 
            // toolStripMenuItem16
            // 
            toolStripMenuItem16.Name = "toolStripMenuItem16";
            toolStripMenuItem16.Size = new System.Drawing.Size(215, 6);
            // 
            // menuHtmlSource
            // 
            menuHtmlSource.Name = "menuHtmlSource";
            menuHtmlSource.Size = new System.Drawing.Size(218, 22);
            menuHtmlSource.Text = "html Source";
            menuHtmlSource.Click += new System.EventHandler(this.menuHtmlSource_Click);
            // 
            // menuToDo
            // 
            menuToDo.Name = "menuToDo";
            menuToDo.Size = new System.Drawing.Size(218, 22);
            menuToDo.Text = "To Do List";
            menuToDo.Click += new System.EventHandler(this.menuToDo_Click);
            // 
            // oToolStrip
            // 
            oToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            tsbHome,
            this.m_tsddbBack,
            this.m_tsbBack,
            this.m_tsddbForward,
            this.m_tsbForward,
            tsbGoto,
            tsbEdit,
            tsbSources,
            tsbTree,
            tsbReport,
            tsbAge,
            this.m_tsbImage,
            this.m_tsbLocation,
            tsbReduceWidth});
            oToolStrip.Location = new System.Drawing.Point(0, 24);
            oToolStrip.Name = "oToolStrip";
            oToolStrip.Size = new System.Drawing.Size(804, 25);
            oToolStrip.TabIndex = 16;
            oToolStrip.Text = "toolStrip1";
            // 
            // tsbHome
            // 
            tsbHome.Image = global::FamilyTree.Viewer.Properties.Resources.Home;
            tsbHome.ImageTransparentColor = System.Drawing.Color.Silver;
            tsbHome.Name = "tsbHome";
            tsbHome.Size = new System.Drawing.Size(54, 22);
            tsbHome.Text = "Home";
            tsbHome.Click += new System.EventHandler(this.menuHome_Click);
            // 
            // m_tsddbBack
            // 
            this.m_tsddbBack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.None;
            this.m_tsddbBack.Image = global::FamilyTree.Viewer.Properties.Resources.Back;
            this.m_tsddbBack.ImageTransparentColor = System.Drawing.Color.Silver;
            this.m_tsddbBack.Name = "m_tsddbBack";
            this.m_tsddbBack.Size = new System.Drawing.Size(13, 22);
            this.m_tsddbBack.Text = "Back";
            // 
            // m_tsbBack
            // 
            this.m_tsbBack.Image = global::FamilyTree.Viewer.Properties.Resources.Back;
            this.m_tsbBack.ImageTransparentColor = System.Drawing.Color.Silver;
            this.m_tsbBack.Name = "m_tsbBack";
            this.m_tsbBack.Size = new System.Drawing.Size(49, 22);
            this.m_tsbBack.Text = "Back";
            this.m_tsbBack.Click += new System.EventHandler(this.menuBack_Click);
            // 
            // m_tsddbForward
            // 
            this.m_tsddbForward.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.None;
            this.m_tsddbForward.Image = ((System.Drawing.Image)(resources.GetObject("m_tsddbForward.Image")));
            this.m_tsddbForward.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_tsddbForward.Name = "m_tsddbForward";
            this.m_tsddbForward.Size = new System.Drawing.Size(13, 22);
            this.m_tsddbForward.Text = "Forward";
            // 
            // m_tsbForward
            // 
            this.m_tsbForward.Image = global::FamilyTree.Viewer.Properties.Resources.Forward;
            this.m_tsbForward.ImageTransparentColor = System.Drawing.Color.Silver;
            this.m_tsbForward.Name = "m_tsbForward";
            this.m_tsbForward.Size = new System.Drawing.Size(67, 22);
            this.m_tsbForward.Text = "Forward";
            this.m_tsbForward.Click += new System.EventHandler(this.menuForward_Click);
            // 
            // tsbGoto
            // 
            tsbGoto.Image = global::FamilyTree.Viewer.Properties.Resources.Person;
            tsbGoto.ImageTransparentColor = System.Drawing.Color.Silver;
            tsbGoto.Name = "tsbGoto";
            tsbGoto.Size = new System.Drawing.Size(50, 22);
            tsbGoto.Text = "Goto";
            tsbGoto.Click += new System.EventHandler(this.menuGoto_Click);
            // 
            // tsbEdit
            // 
            tsbEdit.Image = global::FamilyTree.Viewer.Properties.Resources.Textbox;
            tsbEdit.ImageTransparentColor = System.Drawing.Color.Silver;
            tsbEdit.Name = "tsbEdit";
            tsbEdit.Size = new System.Drawing.Size(45, 22);
            tsbEdit.Text = "Edit";
            tsbEdit.Click += new System.EventHandler(this.menuEdit_Click);
            // 
            // tsbSources
            // 
            tsbSources.Image = global::FamilyTree.Viewer.Properties.Resources.CardFile;
            tsbSources.ImageTransparentColor = System.Drawing.Color.Silver;
            tsbSources.Name = "tsbSources";
            tsbSources.Size = new System.Drawing.Size(65, 22);
            tsbSources.Text = "Sources";
            tsbSources.Click += new System.EventHandler(this.menuEditSources_Click);
            // 
            // tsbTree
            // 
            tsbTree.Image = global::FamilyTree.Viewer.Properties.Resources.Tree;
            tsbTree.ImageTransparentColor = System.Drawing.Color.Silver;
            tsbTree.Name = "tsbTree";
            tsbTree.Size = new System.Drawing.Size(49, 22);
            tsbTree.Text = "Tree";
            tsbTree.Click += new System.EventHandler(this.menuToTree_Click);
            // 
            // tsbReport
            // 
            tsbReport.Image = global::FamilyTree.Viewer.Properties.Resources.web;
            tsbReport.ImageTransparentColor = System.Drawing.Color.Silver;
            tsbReport.Name = "tsbReport";
            tsbReport.Size = new System.Drawing.Size(60, 22);
            tsbReport.Text = "Report";
            tsbReport.Click += new System.EventHandler(this.menuReportToHtml_Click);
            // 
            // tsbAge
            // 
            tsbAge.Image = global::FamilyTree.Viewer.Properties.Resources.Cake;
            tsbAge.ImageTransparentColor = System.Drawing.Color.Silver;
            tsbAge.Name = "tsbAge";
            tsbAge.Size = new System.Drawing.Size(46, 22);
            tsbAge.Text = "Age";
            tsbAge.Click += new System.EventHandler(this.menuCalcAge_Click);
            // 
            // m_tsbImage
            // 
            this.m_tsbImage.Checked = true;
            this.m_tsbImage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_tsbImage.Image = global::FamilyTree.Viewer.Properties.Resources.picture;
            this.m_tsbImage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_tsbImage.Name = "m_tsbImage";
            this.m_tsbImage.Size = new System.Drawing.Size(62, 22);
            this.m_tsbImage.Text = "Images";
            this.m_tsbImage.Click += new System.EventHandler(this.menuImage_Click);
            // 
            // m_tsbLocation
            // 
            this.m_tsbLocation.Image = global::FamilyTree.Viewer.Properties.Resources.Earth;
            this.m_tsbLocation.ImageTransparentColor = System.Drawing.Color.Silver;
            this.m_tsbLocation.Name = "m_tsbLocation";
            this.m_tsbLocation.Size = new System.Drawing.Size(67, 22);
            this.m_tsbLocation.Text = "Location";
            this.m_tsbLocation.Click += new System.EventHandler(this.menuLocation_Click);
            // 
            // tsbReduceWidth
            // 
            tsbReduceWidth.Image = global::FamilyTree.Viewer.Properties.Resources.Width_Reduce;
            tsbReduceWidth.ImageTransparentColor = System.Drawing.Color.Silver;
            tsbReduceWidth.Name = "tsbReduceWidth";
            tsbReduceWidth.Size = new System.Drawing.Size(63, 22);
            tsbReduceWidth.Text = "Reduce";
            tsbReduceWidth.Click += new System.EventHandler(this.menuReduceWidth_Click);
            // 
            // oMainMenu
            // 
            oMainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            menuFile,
            menuEdit,
            menuView,
            menuReports});
            oMainMenu.Location = new System.Drawing.Point(0, 0);
            oMainMenu.Name = "oMainMenu";
            oMainMenu.Size = new System.Drawing.Size(804, 24);
            oMainMenu.TabIndex = 19;
            oMainMenu.Text = "menuStrip1";
            // 
            // menuFile
            // 
            menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            menuOpen,
            menuExportGedcom,
            menuExportSQLScript,
            menuOpenTree,
            toolStripMenuItem9,
            menuHome,
            toolStripMenuItem10,
            this.m_menuRecentFile1,
            this.m_menuRecentFile2,
            this.m_menuRecentFile3,
            this.m_menuRecentFile4,
            toolStripMenuItem11,
            menuExit});
            menuFile.Name = "menuFile";
            menuFile.Size = new System.Drawing.Size(35, 20);
            menuFile.Text = "File";
            // 
            // menuOpen
            // 
            menuOpen.Image = global::FamilyTree.Viewer.Properties.Resources.Open;
            menuOpen.ImageTransparentColor = System.Drawing.Color.Silver;
            menuOpen.Name = "menuOpen";
            menuOpen.Size = new System.Drawing.Size(180, 22);
            menuOpen.Text = "Open...";
            menuOpen.Click += new System.EventHandler(this.menuOpen_Click);
            // 
            // menuExportSQLScript
            // 
            menuExportSQLScript.Name = "menuExportSQLScript";
            menuExportSQLScript.Size = new System.Drawing.Size(180, 22);
            menuExportSQLScript.Text = "Export SQL script...";
            menuExportSQLScript.Click += new System.EventHandler(this.menuExportSQLScript_Click);
            // 
            // menuOpenTree
            // 
            menuOpenTree.Image = global::FamilyTree.Viewer.Properties.Resources.Tree;
            menuOpenTree.ImageTransparentColor = System.Drawing.Color.Silver;
            menuOpenTree.Name = "menuOpenTree";
            menuOpenTree.Size = new System.Drawing.Size(180, 22);
            menuOpenTree.Text = "Open Tree...";
            menuOpenTree.Click += new System.EventHandler(this.menuOpenTree_Click);
            // 
            // menuHome
            // 
            menuHome.Image = global::FamilyTree.Viewer.Properties.Resources.Home;
            menuHome.ImageTransparentColor = System.Drawing.Color.Silver;
            menuHome.Name = "menuHome";
            menuHome.Size = new System.Drawing.Size(180, 22);
            menuHome.Text = "Home";
            menuHome.Click += new System.EventHandler(this.menuHome_Click);
            // 
            // m_menuRecentFile1
            // 
            this.m_menuRecentFile1.Name = "m_menuRecentFile1";
            this.m_menuRecentFile1.Size = new System.Drawing.Size(180, 22);
            this.m_menuRecentFile1.Text = "1";
            this.m_menuRecentFile1.Click += new System.EventHandler(this.menuRecentFile_Click);
            // 
            // m_menuRecentFile2
            // 
            this.m_menuRecentFile2.Name = "m_menuRecentFile2";
            this.m_menuRecentFile2.Size = new System.Drawing.Size(180, 22);
            this.m_menuRecentFile2.Text = "2";
            this.m_menuRecentFile2.Click += new System.EventHandler(this.menuRecentFile_Click);
            // 
            // m_menuRecentFile3
            // 
            this.m_menuRecentFile3.Name = "m_menuRecentFile3";
            this.m_menuRecentFile3.Size = new System.Drawing.Size(180, 22);
            this.m_menuRecentFile3.Text = "3";
            this.m_menuRecentFile3.Click += new System.EventHandler(this.menuRecentFile_Click);
            // 
            // m_menuRecentFile4
            // 
            this.m_menuRecentFile4.Name = "m_menuRecentFile4";
            this.m_menuRecentFile4.Size = new System.Drawing.Size(180, 22);
            this.m_menuRecentFile4.Text = "4";
            this.m_menuRecentFile4.Click += new System.EventHandler(this.menuRecentFile_Click);
            // 
            // menuExit
            // 
            menuExit.Image = global::FamilyTree.Viewer.Properties.Resources.Exit;
            menuExit.ImageTransparentColor = System.Drawing.Color.Silver;
            menuExit.Name = "menuExit";
            menuExit.Size = new System.Drawing.Size(180, 22);
            menuExit.Text = "Exit";
            menuExit.Click += new System.EventHandler(this.menuExit_Click);
            // 
            // menuEdit
            // 
            menuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            menuEditCurrent,
            editSourcesToolStripMenuItem,
            editCensusRecordsToolStripMenuItem,
            menuUnlinkedPlaces,
            toolStripMenuItem12,
            addFatherToolStripMenuItem,
            addMotherToolStripMenuItem,
            addSiblingToolStripMenuItem,
            addChildToolStripMenuItem,
            addPartnerToolStripMenuItem,
            toolStripMenuItem13,
            menuAddMedia,
            toolStripMenuItem15,
            menuUserOptions});
            menuEdit.Name = "menuEdit";
            menuEdit.Size = new System.Drawing.Size(37, 20);
            menuEdit.Text = "Edit";
            // 
            // menuEditCurrent
            // 
            menuEditCurrent.Image = global::FamilyTree.Viewer.Properties.Resources.Textbox;
            menuEditCurrent.ImageTransparentColor = System.Drawing.Color.Silver;
            menuEditCurrent.Name = "menuEditCurrent";
            menuEditCurrent.Size = new System.Drawing.Size(200, 22);
            menuEditCurrent.Text = "Edit...";
            menuEditCurrent.Click += new System.EventHandler(this.menuEdit_Click);
            // 
            // editSourcesToolStripMenuItem
            // 
            editSourcesToolStripMenuItem.Image = global::FamilyTree.Viewer.Properties.Resources.CardFile;
            editSourcesToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Silver;
            editSourcesToolStripMenuItem.Name = "editSourcesToolStripMenuItem";
            editSourcesToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            editSourcesToolStripMenuItem.Text = "Edit Sources...";
            editSourcesToolStripMenuItem.Click += new System.EventHandler(this.menuEditSources_Click);
            // 
            // menuUserOptions
            // 
            menuUserOptions.Image = global::FamilyTree.Viewer.Properties.Resources.Wrench;
            menuUserOptions.Name = "menuUserOptions";
            menuUserOptions.Size = new System.Drawing.Size(200, 22);
            menuUserOptions.Text = "User Options...";
            menuUserOptions.Click += new System.EventHandler(this.menuOptions_Click);
            // 
            // menuView
            // 
            menuView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            menuGoto,
            this.m_menuImage,
            this.m_menuLocation,
            menuRecentChanges,
            menuToDo,
            menuAge,
            menuBirthday,
            toolStripMenuItem14,
            menuReduceWidth,
            menuStandardWidth,
            toolStripMenuItem16,
            menuHtmlSource});
            menuView.Name = "menuView";
            menuView.Size = new System.Drawing.Size(41, 20);
            menuView.Text = "View";
            // 
            // menuGoto
            // 
            menuGoto.Image = global::FamilyTree.Viewer.Properties.Resources.Person;
            menuGoto.Name = "menuGoto";
            menuGoto.Size = new System.Drawing.Size(218, 22);
            menuGoto.Text = "Goto Person...";
            menuGoto.Click += new System.EventHandler(this.menuGoto_Click);
            // 
            // m_menuImage
            // 
            this.m_menuImage.Checked = true;
            this.m_menuImage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_menuImage.Image = global::FamilyTree.Viewer.Properties.Resources.picture;
            this.m_menuImage.Name = "m_menuImage";
            this.m_menuImage.Size = new System.Drawing.Size(218, 22);
            this.m_menuImage.Text = "Image";
            this.m_menuImage.Click += new System.EventHandler(this.menuImage_Click);
            // 
            // m_menuLocation
            // 
            this.m_menuLocation.Image = global::FamilyTree.Viewer.Properties.Resources.Earth;
            this.m_menuLocation.Name = "m_menuLocation";
            this.m_menuLocation.Size = new System.Drawing.Size(218, 22);
            this.m_menuLocation.Text = "Location";
            this.m_menuLocation.Click += new System.EventHandler(this.menuLocation_Click);
            // 
            // menuAge
            // 
            menuAge.Image = global::FamilyTree.Viewer.Properties.Resources.Cake;
            menuAge.ImageTransparentColor = System.Drawing.Color.Silver;
            menuAge.Name = "menuAge";
            menuAge.Size = new System.Drawing.Size(218, 22);
            menuAge.Text = "Calculate Age...";
            menuAge.Click += new System.EventHandler(this.menuCalcAge_Click);
            // 
            // menuBirthday
            // 
            menuBirthday.Image = global::FamilyTree.Viewer.Properties.Resources.Cake;
            menuBirthday.ImageTransparentColor = System.Drawing.Color.Silver;
            menuBirthday.Name = "menuBirthday";
            menuBirthday.Size = new System.Drawing.Size(218, 22);
            menuBirthday.Text = "Calculate Birthday Range...";
            menuBirthday.Click += new System.EventHandler(this.menuBirthday_Click);
            // 
            // menuReduceWidth
            // 
            menuReduceWidth.Image = global::FamilyTree.Viewer.Properties.Resources.Width_Reduce;
            menuReduceWidth.ImageTransparentColor = System.Drawing.Color.Silver;
            menuReduceWidth.Name = "menuReduceWidth";
            menuReduceWidth.Size = new System.Drawing.Size(218, 22);
            menuReduceWidth.Text = "Reduce Width";
            menuReduceWidth.Click += new System.EventHandler(this.menuReduceWidth_Click);
            // 
            // menuStandardWidth
            // 
            menuStandardWidth.Image = global::FamilyTree.Viewer.Properties.Resources.Width_Increase;
            menuStandardWidth.ImageTransparentColor = System.Drawing.Color.Silver;
            menuStandardWidth.Name = "menuStandardWidth";
            menuStandardWidth.Size = new System.Drawing.Size(218, 22);
            menuStandardWidth.Text = "Standard Width";
            menuStandardWidth.Click += new System.EventHandler(this.menuStandardWidth_Click);
            // 
            // menuReports
            // 
            menuReports.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            menuToTree,
            menuReportToHtml});
            menuReports.Name = "menuReports";
            menuReports.Size = new System.Drawing.Size(57, 20);
            menuReports.Text = "Reports";
            // 
            // menuToTree
            // 
            menuToTree.Image = global::FamilyTree.Viewer.Properties.Resources.Tree;
            menuToTree.ImageTransparentColor = System.Drawing.Color.Silver;
            menuToTree.Name = "menuToTree";
            menuToTree.Size = new System.Drawing.Size(122, 22);
            menuToTree.Text = "To Tree";
            menuToTree.Click += new System.EventHandler(this.menuToTree_Click);
            // 
            // menuReportToHtml
            // 
            menuReportToHtml.Image = global::FamilyTree.Viewer.Properties.Resources.web;
            menuReportToHtml.Name = "menuReportToHtml";
            menuReportToHtml.Size = new System.Drawing.Size(122, 22);
            menuReportToHtml.Text = "To Html";
            menuReportToHtml.Click += new System.EventHandler(this.menuReportToHtml_Click);
            // 
            // oStatusStrip
            // 
            oStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_tslabStatus,
            this.m_tsProgressBar,
            tslabUserName});
            oStatusStrip.Location = new System.Drawing.Point(0, 465);
            oStatusStrip.Name = "oStatusStrip";
            oStatusStrip.Size = new System.Drawing.Size(804, 25);
            oStatusStrip.TabIndex = 20;
            oStatusStrip.Text = "statusStrip1";
            // 
            // m_tslabStatus
            // 
            this.m_tslabStatus.Image = global::FamilyTree.Viewer.Properties.Resources.OK;
            this.m_tslabStatus.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.m_tslabStatus.Name = "m_tslabStatus";
            this.m_tslabStatus.Size = new System.Drawing.Size(697, 20);
            this.m_tslabStatus.Spring = true;
            this.m_tslabStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_tsProgressBar
            // 
            this.m_tsProgressBar.Name = "m_tsProgressBar";
            this.m_tsProgressBar.Size = new System.Drawing.Size(200, 19);
            this.m_tsProgressBar.Step = 1;
            this.m_tsProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.m_tsProgressBar.Visible = false;
            // 
            // tslabUserName
            // 
            tslabUserName.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            tslabUserName.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            tslabUserName.Image = global::FamilyTree.Viewer.Properties.Resources.Person;
            tslabUserName.Name = "tslabUserName";
            tslabUserName.Size = new System.Drawing.Size(92, 20);
            tslabUserName.Text = "Steve Walton";
            // 
            // m_labPerson
            // 
            this.m_labPerson.BackColor = System.Drawing.Color.LightPink;
            this.m_labPerson.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_labPerson.Location = new System.Drawing.Point(3, 151);
            this.m_labPerson.Name = "m_labPerson";
            this.m_labPerson.Size = new System.Drawing.Size(312, 23);
            this.m_labPerson.TabIndex = 3;
            this.m_labPerson.Text = "label1";
            this.m_labPerson.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.m_labPerson.Paint += new System.Windows.Forms.PaintEventHandler(this.labPerson_Paint);
            // 
            // m_labPersonDates
            // 
            this.m_labPersonDates.BackColor = System.Drawing.Color.LightPink;
            this.m_labPersonDates.Location = new System.Drawing.Point(3, 174);
            this.m_labPersonDates.Name = "m_labPersonDates";
            this.m_labPersonDates.Size = new System.Drawing.Size(312, 41);
            this.m_labPersonDates.TabIndex = 5;
            this.m_labPersonDates.Text = "label1";
            this.m_labPersonDates.Paint += new System.Windows.Forms.PaintEventHandler(this.labPersonDates_Paint);
            // 
            // m_oPanelTree
            // 
            this.m_oPanelTree.Controls.Add(this.m_labPersonDates);
            this.m_oPanelTree.Controls.Add(this.m_marMotherParents);
            this.m_oPanelTree.Controls.Add(this.m_labPerson);
            this.m_oPanelTree.Controls.Add(this.m_marFatherParents);
            this.m_oPanelTree.Controls.Add(this.m_marParents);
            this.m_oPanelTree.Controls.Add(this.m_psnFatherFather);
            this.m_oPanelTree.Controls.Add(this.m_psnMother);
            this.m_oPanelTree.Controls.Add(this.m_psnMotherMother);
            this.m_oPanelTree.Controls.Add(this.m_psnFather);
            this.m_oPanelTree.Controls.Add(this.m_psnFatherMother);
            this.m_oPanelTree.Controls.Add(this.m_psnMotherFather);
            this.m_oPanelTree.Dock = System.Windows.Forms.DockStyle.Top;
            this.m_oPanelTree.Location = new System.Drawing.Point(0, 49);
            this.m_oPanelTree.Name = "m_oPanelTree";
            this.m_oPanelTree.Size = new System.Drawing.Size(804, 338);
            this.m_oPanelTree.TabIndex = 16;
            this.m_oPanelTree.Paint += new System.Windows.Forms.PaintEventHandler(this.PanelTree_Paint);
            // 
            // m_marMotherParents
            // 
            this.m_marMotherParents.Location = new System.Drawing.Point(403, 109);
            this.m_marMotherParents.Name = "m_marMotherParents";
            this.m_marMotherParents.Size = new System.Drawing.Size(16, 16);
            this.m_marMotherParents.TabIndex = 15;
            // 
            // m_marFatherParents
            // 
            this.m_marFatherParents.Location = new System.Drawing.Point(440, 109);
            this.m_marFatherParents.Name = "m_marFatherParents";
            this.m_marFatherParents.Size = new System.Drawing.Size(16, 16);
            this.m_marFatherParents.TabIndex = 14;
            // 
            // m_marParents
            // 
            this.m_marParents.Location = new System.Drawing.Point(316, 79);
            this.m_marParents.Name = "m_marParents";
            this.m_marParents.Size = new System.Drawing.Size(16, 16);
            this.m_marParents.TabIndex = 6;
            // 
            // m_psnFatherFather
            // 
            this.m_psnFatherFather.BackColor = System.Drawing.Color.LightSkyBlue;
            this.m_psnFatherFather.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_psnFatherFather.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_psnFatherFather.Location = new System.Drawing.Point(6, 3);
            this.m_psnFatherFather.Name = "m_psnFatherFather";
            this.m_psnFatherFather.Size = new System.Drawing.Size(150, 59);
            this.m_psnFatherFather.TabIndex = 10;
            // 
            // m_psnMother
            // 
            this.m_psnMother.BackColor = System.Drawing.Color.LightPink;
            this.m_psnMother.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_psnMother.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_psnMother.Location = new System.Drawing.Point(160, 79);
            this.m_psnMother.Name = "m_psnMother";
            this.m_psnMother.Size = new System.Drawing.Size(150, 59);
            this.m_psnMother.TabIndex = 2;
            // 
            // m_psnMotherMother
            // 
            this.m_psnMotherMother.BackColor = System.Drawing.Color.LightPink;
            this.m_psnMotherMother.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_psnMotherMother.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_psnMotherMother.Location = new System.Drawing.Point(482, 3);
            this.m_psnMotherMother.Name = "m_psnMotherMother";
            this.m_psnMotherMother.Size = new System.Drawing.Size(150, 59);
            this.m_psnMotherMother.TabIndex = 13;
            // 
            // m_psnFather
            // 
            this.m_psnFather.BackColor = System.Drawing.Color.LightSkyBlue;
            this.m_psnFather.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_psnFather.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_psnFather.Location = new System.Drawing.Point(4, 79);
            this.m_psnFather.Name = "m_psnFather";
            this.m_psnFather.Size = new System.Drawing.Size(150, 59);
            this.m_psnFather.TabIndex = 1;
            // 
            // m_psnFatherMother
            // 
            this.m_psnFatherMother.BackColor = System.Drawing.Color.LightPink;
            this.m_psnFatherMother.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_psnFatherMother.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_psnFatherMother.Location = new System.Drawing.Point(160, 3);
            this.m_psnFatherMother.Name = "m_psnFatherMother";
            this.m_psnFatherMother.Size = new System.Drawing.Size(150, 59);
            this.m_psnFatherMother.TabIndex = 11;
            // 
            // m_psnMotherFather
            // 
            this.m_psnMotherFather.BackColor = System.Drawing.Color.LightSkyBlue;
            this.m_psnMotherFather.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_psnMotherFather.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_psnMotherFather.Location = new System.Drawing.Point(326, 3);
            this.m_psnMotherFather.Name = "m_psnMotherFather";
            this.m_psnMotherFather.Size = new System.Drawing.Size(150, 59);
            this.m_psnMotherFather.TabIndex = 12;
            // 
            // m_oWebBrowser
            // 
            this.m_oWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_oWebBrowser.Location = new System.Drawing.Point(0, 387);
            this.m_oWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.m_oWebBrowser.Name = "m_oWebBrowser";
            this.m_oWebBrowser.Size = new System.Drawing.Size(804, 78);
            this.m_oWebBrowser.TabIndex = 18;
            this.m_oWebBrowser.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.WebBrowser_Navigating);
            // 
            // frmMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(804, 490);
            this.Controls.Add(this.m_oWebBrowser);
            this.Controls.Add(this.m_oPanelTree);
            this.Controls.Add(oToolStrip);
            this.Controls.Add(oMainMenu);
            this.Controls.Add(oStatusStrip);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = oMainMenu;
            this.Name = "frmMain";
            this.Text = "Family Tree Viewer";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Shown += new System.EventHandler(this.frmMain_Shown);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmMain_Closing);
            oToolStrip.ResumeLayout(false);
            oToolStrip.PerformLayout();
            oMainMenu.ResumeLayout(false);
            oMainMenu.PerformLayout();
            oStatusStrip.ResumeLayout(false);
            oStatusStrip.PerformLayout();
            this.m_oPanelTree.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.OpenFileDialog m_OpenFileDialog;
        private System.Windows.Forms.Label m_labPerson;
        private System.Windows.Forms.Label m_labPersonDates;
        private FamilyTree.Viewer.ucPerson m_psnFather;
        private FamilyTree.Viewer.ucPerson m_psnMother;
        private FamilyTree.Viewer.ucPerson m_psnFatherFather;
        private FamilyTree.Viewer.ucPerson m_psnFatherMother;
        private FamilyTree.Viewer.ucPerson m_psnMotherFather;
        private FamilyTree.Viewer.ucPerson m_psnMotherMother;
        private FamilyTree.Viewer.ucRelationship m_marParents;
        private FamilyTree.Viewer.ucRelationship m_marFatherParents;
        private FamilyTree.Viewer.ucRelationship m_marMotherParents;
        private System.Windows.Forms.SaveFileDialog m_SaveFileDialog;
        private Panel m_oPanelTree;
        private WebBrowser m_oWebBrowser;
        private ToolStripMenuItem m_menuRecentFile1;
        private ToolStripMenuItem m_menuRecentFile2;
        private ToolStripMenuItem m_menuRecentFile3;
        private ToolStripMenuItem m_menuRecentFile4;
        private ToolStripMenuItem m_menuImage;
        private ToolStripDropDownButton m_tsddbBack;
        private ToolStripButton m_tsbBack;
        private ToolStripButton m_tsbForward;
        private ToolStripDropDownButton m_tsddbForward;
        private ToolStripStatusLabel m_tslabStatus;
        private ToolStripProgressBar m_tsProgressBar;
        private ToolStripMenuItem m_menuLocation;
        private ToolStripButton m_tsbLocation;
        private ToolStripButton m_tsbImage;
    }
}
