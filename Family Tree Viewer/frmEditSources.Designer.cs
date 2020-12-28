using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace FamilyTree.Viewer
{
    public partial class frmEditSources : System.Windows.Forms.Form
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
            System.Windows.Forms.Label label32;
            System.Windows.Forms.Label label33;
            System.Windows.Forms.Label label34;
            System.Windows.Forms.Label label35;
            System.Windows.Forms.Label label36;
            System.Windows.Forms.Label label31;
            System.Windows.Forms.Label label30;
            System.Windows.Forms.Label label29;
            System.Windows.Forms.Label label28;
            System.Windows.Forms.Label label6;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label37;
            System.Windows.Forms.Label label38;
            System.Windows.Forms.Label label15;
            System.Windows.Forms.Label label14;
            System.Windows.Forms.Label label13;
            System.Windows.Forms.Label label12;
            System.Windows.Forms.Label label11;
            System.Windows.Forms.Label label10;
            System.Windows.Forms.Label label9;
            System.Windows.Forms.Label label8;
            System.Windows.Forms.Label label7;
            System.Windows.Forms.Label label39;
            System.Windows.Forms.Label label27;
            System.Windows.Forms.Label label26;
            System.Windows.Forms.Label label25;
            System.Windows.Forms.Label label24;
            System.Windows.Forms.Label label23;
            System.Windows.Forms.Label label22;
            System.Windows.Forms.Label label21;
            System.Windows.Forms.Label label20;
            System.Windows.Forms.Label label16;
            System.Windows.Forms.Label label17;
            System.Windows.Forms.Label label18;
            System.Windows.Forms.Label label19;
            System.Windows.Forms.Label label40;
            System.Windows.Forms.Button cmdCensusAddress;
            System.Windows.Forms.Button cmdCensusOpen;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEditSources));
            FamilyTree.Objects.clsDate clsDate1 = new FamilyTree.Objects.clsDate();
            this.m_lstSources = new System.Windows.Forms.ListBox();
            this.imageList16x16 = new System.Windows.Forms.ImageList(this.components);
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.gridReferences = new System.Windows.Forms.DataGrid();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.m_cboRepository = new System.Windows.Forms.ComboBox();
            this.m_cboAdditionalInfo = new System.Windows.Forms.ComboBox();
            this.dateTheDate = new FamilyTree.Viewer.ucDate();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.txtComments = new System.Windows.Forms.TextBox();
            this.cboPrefix = new System.Windows.Forms.ComboBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.m_grpCensus = new System.Windows.Forms.GroupBox();
            this.m_txtCensusPage = new System.Windows.Forms.TextBox();
            this.m_txtCensusFolio = new System.Windows.Forms.TextBox();
            this.m_txtCensusPiece = new System.Windows.Forms.TextBox();
            this.m_txtCensusSeries = new System.Windows.Forms.TextBox();
            this.m_txtCensusAddress = new System.Windows.Forms.TextBox();
            this.m_grpDeath = new System.Windows.Forms.GroupBox();
            this.m_txtDeathReference = new System.Windows.Forms.TextBox();
            this.m_txtDeathWhere = new System.Windows.Forms.TextBox();
            this.m_txtDeathWhenReg = new System.Windows.Forms.TextBox();
            this.m_txtDeathInformantDescription = new System.Windows.Forms.TextBox();
            this.m_txtDeathInformantAddress = new System.Windows.Forms.TextBox();
            this.m_txtDeathInformant = new System.Windows.Forms.TextBox();
            this.m_txtDeathUsualAddress = new System.Windows.Forms.TextBox();
            this.m_txtDeathCause = new System.Windows.Forms.TextBox();
            this.m_txtDeathOccupation = new System.Windows.Forms.TextBox();
            this.m_txtDeathDatePlace = new System.Windows.Forms.TextBox();
            this.m_txtDeathSex = new System.Windows.Forms.TextBox();
            this.m_txtDeathName = new System.Windows.Forms.TextBox();
            this.m_txtDeathWhen = new System.Windows.Forms.TextBox();
            this.m_txtDeathDistrict = new System.Windows.Forms.TextBox();
            this.m_grpBirth = new System.Windows.Forms.GroupBox();
            this.m_txtBirthReference = new System.Windows.Forms.TextBox();
            this.m_txtBirthMotherDetails = new System.Windows.Forms.TextBox();
            this.m_txtBirthInformantAddress = new System.Windows.Forms.TextBox();
            this.m_dtpBirthWhen = new System.Windows.Forms.DateTimePicker();
            this.m_txtBirthWhenReg = new System.Windows.Forms.TextBox();
            this.m_txtBirthInformant = new System.Windows.Forms.TextBox();
            this.m_txtBirthMother = new System.Windows.Forms.TextBox();
            this.m_txtBirthFatherOccupation = new System.Windows.Forms.TextBox();
            this.m_txtBirthFather = new System.Windows.Forms.TextBox();
            this.m_txtBirthSex = new System.Windows.Forms.TextBox();
            this.m_txtBirthName = new System.Windows.Forms.TextBox();
            this.m_txtBirthWhenWhere = new System.Windows.Forms.TextBox();
            this.m_txtBirthDistrict = new System.Windows.Forms.TextBox();
            this.m_grpMarriage = new System.Windows.Forms.GroupBox();
            this.m_txtMarrGro = new System.Windows.Forms.TextBox();
            this.m_dtpMarrWhen = new System.Windows.Forms.DateTimePicker();
            this.m_txtMarrWitness = new System.Windows.Forms.TextBox();
            this.m_txtMarrLocation = new System.Windows.Forms.TextBox();
            this.m_txtMarrBrideFatherOcc = new System.Windows.Forms.TextBox();
            this.m_txtMarrBrideFather = new System.Windows.Forms.TextBox();
            this.m_txtMarrBrideLoca = new System.Windows.Forms.TextBox();
            this.m_txtMarrBrideOccu = new System.Windows.Forms.TextBox();
            this.m_txtMarrBrideAge = new System.Windows.Forms.TextBox();
            this.m_txtMarrBride = new System.Windows.Forms.TextBox();
            this.m_txtMarrGroomFatherOcc = new System.Windows.Forms.TextBox();
            this.m_txtMarrGroomFather = new System.Windows.Forms.TextBox();
            this.m_txtMarrGroomLoca = new System.Windows.Forms.TextBox();
            this.m_txtMarrGroomOccu = new System.Windows.Forms.TextBox();
            this.m_txtMarrGroomAge = new System.Windows.Forms.TextBox();
            this.m_txtMarrGroom = new System.Windows.Forms.TextBox();
            this.radioDate = new System.Windows.Forms.RadioButton();
            this.radioAlpha = new System.Windows.Forms.RadioButton();
            this.m_panList = new System.Windows.Forms.Panel();
            this.cmdAddSource = new System.Windows.Forms.Button();
            this.cmdDeleteSource = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            label32 = new System.Windows.Forms.Label();
            label33 = new System.Windows.Forms.Label();
            label34 = new System.Windows.Forms.Label();
            label35 = new System.Windows.Forms.Label();
            label36 = new System.Windows.Forms.Label();
            label31 = new System.Windows.Forms.Label();
            label30 = new System.Windows.Forms.Label();
            label29 = new System.Windows.Forms.Label();
            label28 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label37 = new System.Windows.Forms.Label();
            label38 = new System.Windows.Forms.Label();
            label15 = new System.Windows.Forms.Label();
            label14 = new System.Windows.Forms.Label();
            label13 = new System.Windows.Forms.Label();
            label12 = new System.Windows.Forms.Label();
            label11 = new System.Windows.Forms.Label();
            label10 = new System.Windows.Forms.Label();
            label9 = new System.Windows.Forms.Label();
            label8 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            label39 = new System.Windows.Forms.Label();
            label27 = new System.Windows.Forms.Label();
            label26 = new System.Windows.Forms.Label();
            label25 = new System.Windows.Forms.Label();
            label24 = new System.Windows.Forms.Label();
            label23 = new System.Windows.Forms.Label();
            label22 = new System.Windows.Forms.Label();
            label21 = new System.Windows.Forms.Label();
            label20 = new System.Windows.Forms.Label();
            label16 = new System.Windows.Forms.Label();
            label17 = new System.Windows.Forms.Label();
            label18 = new System.Windows.Forms.Label();
            label19 = new System.Windows.Forms.Label();
            label40 = new System.Windows.Forms.Label();
            cmdCensusAddress = new System.Windows.Forms.Button();
            cmdCensusOpen = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridReferences)).BeginInit();
            this.panel1.SuspendLayout();
            this.m_grpCensus.SuspendLayout();
            this.m_grpDeath.SuspendLayout();
            this.m_grpBirth.SuspendLayout();
            this.m_grpMarriage.SuspendLayout();
            this.m_panList.SuspendLayout();
            this.SuspendLayout();
            // 
            // label32
            // 
            label32.AutoSize = true;
            label32.Location = new System.Drawing.Point(5, 19);
            label32.Name = "label32";
            label32.Size = new System.Drawing.Size(36, 13);
            label32.TabIndex = 7;
            label32.Text = "Series";
            // 
            // label33
            // 
            label33.AutoSize = true;
            label33.Location = new System.Drawing.Point(88, 19);
            label33.Name = "label33";
            label33.Size = new System.Drawing.Size(32, 13);
            label33.TabIndex = 8;
            label33.Text = "Piece";
            // 
            // label34
            // 
            label34.AutoSize = true;
            label34.Location = new System.Drawing.Point(164, 19);
            label34.Name = "label34";
            label34.Size = new System.Drawing.Size(29, 13);
            label34.TabIndex = 9;
            label34.Text = "Folio";
            // 
            // label35
            // 
            label35.AutoSize = true;
            label35.Location = new System.Drawing.Point(245, 19);
            label35.Name = "label35";
            label35.Size = new System.Drawing.Size(31, 13);
            label35.TabIndex = 10;
            label35.Text = "Page";
            // 
            // label36
            // 
            label36.AutoSize = true;
            label36.Location = new System.Drawing.Point(6, 72);
            label36.Name = "label36";
            label36.Size = new System.Drawing.Size(46, 13);
            label36.TabIndex = 11;
            label36.Text = "Address";
            // 
            // label31
            // 
            label31.Location = new System.Drawing.Point(8, 8);
            label31.Name = "label31";
            label31.Size = new System.Drawing.Size(64, 23);
            label31.TabIndex = 34;
            label31.Text = "Title";
            label31.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label30
            // 
            label30.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            label30.Location = new System.Drawing.Point(352, 32);
            label30.Name = "label30";
            label30.Size = new System.Drawing.Size(64, 23);
            label30.TabIndex = 33;
            label30.Text = "Date";
            label30.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label29
            // 
            label29.Location = new System.Drawing.Point(8, 32);
            label29.Name = "label29";
            label29.Size = new System.Drawing.Size(64, 23);
            label29.TabIndex = 32;
            label29.Text = "Prefix";
            label29.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label28
            // 
            label28.Location = new System.Drawing.Point(8, 64);
            label28.Name = "label28";
            label28.Size = new System.Drawing.Size(64, 40);
            label28.TabIndex = 31;
            label28.Text = "Public Comments";
            label28.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label6
            // 
            label6.Location = new System.Drawing.Point(6, 165);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(72, 23);
            label6.TabIndex = 17;
            label6.Text = "Witness:";
            label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            label5.Location = new System.Drawing.Point(6, 45);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(72, 23);
            label5.TabIndex = 16;
            label5.Text = "Location:";
            label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            label4.Location = new System.Drawing.Point(6, 141);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(72, 23);
            label4.TabIndex = 15;
            label4.Text = "Bride\'s Dad";
            label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            label3.Location = new System.Drawing.Point(6, 117);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(72, 23);
            label3.TabIndex = 14;
            label3.Text = "Groom\'s Dad";
            label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            label2.Location = new System.Drawing.Point(6, 93);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(72, 23);
            label2.TabIndex = 13;
            label2.Text = "Bride:";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            label1.Location = new System.Drawing.Point(6, 69);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(72, 23);
            label1.TabIndex = 12;
            label1.Text = "Groom:";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label37
            // 
            label37.Location = new System.Drawing.Point(6, 19);
            label37.Name = "label37";
            label37.Size = new System.Drawing.Size(72, 23);
            label37.TabIndex = 23;
            label37.Text = "Date:";
            label37.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label38
            // 
            label38.Location = new System.Drawing.Point(6, 188);
            label38.Name = "label38";
            label38.Size = new System.Drawing.Size(72, 23);
            label38.TabIndex = 24;
            label38.Text = "GRO Ref:";
            label38.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label15
            // 
            label15.Location = new System.Drawing.Point(274, 197);
            label15.Name = "label15";
            label15.Size = new System.Drawing.Size(112, 23);
            label15.TabIndex = 12;
            label15.Text = "When:";
            label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label14
            // 
            label14.Location = new System.Drawing.Point(10, 143);
            label14.Name = "label14";
            label14.Size = new System.Drawing.Size(112, 23);
            label14.TabIndex = 11;
            label14.Text = "Informant:";
            label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label13
            // 
            label13.Location = new System.Drawing.Point(272, 96);
            label13.Name = "label13";
            label13.Size = new System.Drawing.Size(112, 23);
            label13.TabIndex = 10;
            label13.Text = "Occupation:";
            label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            label12.Location = new System.Drawing.Point(10, 120);
            label12.Name = "label12";
            label12.Size = new System.Drawing.Size(112, 23);
            label12.TabIndex = 9;
            label12.Text = "Mother:";
            label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            label11.Location = new System.Drawing.Point(10, 94);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(112, 23);
            label11.TabIndex = 8;
            label11.Text = "Father:";
            label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            label10.Location = new System.Drawing.Point(272, 72);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(112, 23);
            label10.TabIndex = 6;
            label10.Text = "Sex:";
            label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            label9.Location = new System.Drawing.Point(10, 70);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(112, 23);
            label9.TabIndex = 4;
            label9.Text = "Name:";
            label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            label8.Location = new System.Drawing.Point(10, 46);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(112, 23);
            label8.TabIndex = 2;
            label8.Text = "When and Where:";
            label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            label7.Location = new System.Drawing.Point(10, 22);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(112, 23);
            label7.TabIndex = 1;
            label7.Text = "Registration District:";
            label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label39
            // 
            label39.Location = new System.Drawing.Point(10, 166);
            label39.Name = "label39";
            label39.Size = new System.Drawing.Size(112, 23);
            label39.TabIndex = 26;
            label39.Text = "GRO Reference:";
            label39.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label27
            // 
            label27.Location = new System.Drawing.Point(272, 248);
            label27.Name = "label27";
            label27.Size = new System.Drawing.Size(64, 23);
            label27.TabIndex = 31;
            label27.Text = "Registered:";
            label27.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label26
            // 
            label26.Location = new System.Drawing.Point(8, 248);
            label26.Name = "label26";
            label26.Size = new System.Drawing.Size(112, 23);
            label26.TabIndex = 29;
            label26.Text = "Address:";
            label26.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label25
            // 
            label25.Location = new System.Drawing.Point(272, 224);
            label25.Name = "label25";
            label25.Size = new System.Drawing.Size(64, 23);
            label25.TabIndex = 27;
            label25.Text = "Description:";
            label25.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label24
            // 
            label24.Location = new System.Drawing.Point(8, 224);
            label24.Name = "label24";
            label24.Size = new System.Drawing.Size(112, 23);
            label24.TabIndex = 26;
            label24.Text = "Informant:";
            label24.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label23
            // 
            label23.Location = new System.Drawing.Point(8, 144);
            label23.Name = "label23";
            label23.Size = new System.Drawing.Size(112, 23);
            label23.TabIndex = 23;
            label23.Text = "Cause Of Death:";
            label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label22
            // 
            label22.Location = new System.Drawing.Point(8, 120);
            label22.Name = "label22";
            label22.Size = new System.Drawing.Size(112, 23);
            label22.TabIndex = 22;
            label22.Text = "Occupation:";
            label22.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label21
            // 
            label21.Location = new System.Drawing.Point(8, 96);
            label21.Name = "label21";
            label21.Size = new System.Drawing.Size(112, 23);
            label21.TabIndex = 21;
            label21.Text = "Date / Place of Birth:";
            label21.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label20
            // 
            label20.Location = new System.Drawing.Point(272, 120);
            label20.Name = "label20";
            label20.Size = new System.Drawing.Size(64, 23);
            label20.TabIndex = 20;
            label20.Text = "Address:";
            label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label16
            // 
            label16.Location = new System.Drawing.Point(272, 72);
            label16.Name = "label16";
            label16.Size = new System.Drawing.Size(112, 23);
            label16.TabIndex = 14;
            label16.Text = "Sex:";
            label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label17
            // 
            label17.Location = new System.Drawing.Point(8, 72);
            label17.Name = "label17";
            label17.Size = new System.Drawing.Size(112, 23);
            label17.TabIndex = 12;
            label17.Text = "Name:";
            label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label18
            // 
            label18.Location = new System.Drawing.Point(8, 48);
            label18.Name = "label18";
            label18.Size = new System.Drawing.Size(112, 23);
            label18.TabIndex = 10;
            label18.Text = "When and Where:";
            label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label19
            // 
            label19.Location = new System.Drawing.Point(8, 24);
            label19.Name = "label19";
            label19.Size = new System.Drawing.Size(112, 23);
            label19.TabIndex = 9;
            label19.Text = "Registration District:";
            label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label40
            // 
            label40.Location = new System.Drawing.Point(10, 272);
            label40.Name = "label40";
            label40.Size = new System.Drawing.Size(112, 23);
            label40.TabIndex = 32;
            label40.Text = "GRO Reference:";
            label40.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmdCensusAddress
            // 
            cmdCensusAddress.Image = global::FamilyTree.Viewer.Properties.Resources.Earth;
            cmdCensusAddress.Location = new System.Drawing.Point(529, 91);
            cmdCensusAddress.Name = "cmdCensusAddress";
            cmdCensusAddress.Size = new System.Drawing.Size(21, 21);
            cmdCensusAddress.TabIndex = 12;
            cmdCensusAddress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            cmdCensusAddress.Click += new System.EventHandler(this.cmdCensusAddress_Click);
            // 
            // cmdCensusOpen
            // 
            cmdCensusOpen.Image = global::FamilyTree.Viewer.Properties.Resources.Open;
            cmdCensusOpen.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            cmdCensusOpen.Location = new System.Drawing.Point(8, 191);
            cmdCensusOpen.Name = "cmdCensusOpen";
            cmdCensusOpen.Size = new System.Drawing.Size(100, 30);
            cmdCensusOpen.TabIndex = 2;
            cmdCensusOpen.Text = "Open";
            cmdCensusOpen.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            cmdCensusOpen.Click += new System.EventHandler(this.cmdCensusOpen_Click);
            // 
            // m_lstSources
            // 
            this.m_lstSources.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_lstSources.Location = new System.Drawing.Point(0, 25);
            this.m_lstSources.Name = "m_lstSources";
            this.m_lstSources.Size = new System.Drawing.Size(400, 433);
            this.m_lstSources.TabIndex = 0;
            this.m_lstSources.SelectedIndexChanged += new System.EventHandler(this.lstSources_SelectedIndexChanged);
            // 
            // imageList16x16
            // 
            this.imageList16x16.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList16x16.ImageStream")));
            this.imageList16x16.TransparentColor = System.Drawing.Color.Silver;
            this.imageList16x16.Images.SetKeyName(0, "");
            this.imageList16x16.Images.SetKeyName(1, "");
            this.imageList16x16.Images.SetKeyName(2, "");
            this.imageList16x16.Images.SetKeyName(3, "");
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(0, 505);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(984, 8);
            this.splitter1.TabIndex = 16;
            this.splitter1.TabStop = false;
            // 
            // gridReferences
            // 
            this.gridReferences.CaptionVisible = false;
            this.gridReferences.DataMember = "";
            this.gridReferences.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gridReferences.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.gridReferences.Location = new System.Drawing.Point(0, 513);
            this.gridReferences.Name = "gridReferences";
            this.gridReferences.Size = new System.Drawing.Size(984, 104);
            this.gridReferences.TabIndex = 17;
            // 
            // splitter2
            // 
            this.splitter2.Location = new System.Drawing.Point(400, 0);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(8, 505);
            this.splitter2.TabIndex = 18;
            this.splitter2.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(label31);
            this.panel1.Controls.Add(label30);
            this.panel1.Controls.Add(label29);
            this.panel1.Controls.Add(label28);
            this.panel1.Controls.Add(this.m_cboRepository);
            this.panel1.Controls.Add(this.m_cboAdditionalInfo);
            this.panel1.Controls.Add(this.dateTheDate);
            this.panel1.Controls.Add(this.cmdCancel);
            this.panel1.Controls.Add(this.cmdOK);
            this.panel1.Controls.Add(this.txtComments);
            this.panel1.Controls.Add(this.cboPrefix);
            this.panel1.Controls.Add(this.txtDescription);
            this.panel1.Controls.Add(this.m_grpBirth);
            this.panel1.Controls.Add(this.m_grpMarriage);
            this.panel1.Controls.Add(this.m_grpCensus);
            this.panel1.Controls.Add(this.m_grpDeath);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(408, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(576, 505);
            this.panel1.TabIndex = 19;
            // 
            // m_cboRepository
            // 
            this.m_cboRepository.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_cboRepository.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cboRepository.Location = new System.Drawing.Point(135, 470);
            this.m_cboRepository.Name = "m_cboRepository";
            this.m_cboRepository.Size = new System.Drawing.Size(152, 21);
            this.m_cboRepository.TabIndex = 30;
            this.m_cboRepository.SelectedIndexChanged += new System.EventHandler(this.cboRepository_SelectedIndexChanged);
            // 
            // m_cboAdditionalInfo
            // 
            this.m_cboAdditionalInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_cboAdditionalInfo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cboAdditionalInfo.Location = new System.Drawing.Point(8, 470);
            this.m_cboAdditionalInfo.Name = "m_cboAdditionalInfo";
            this.m_cboAdditionalInfo.Size = new System.Drawing.Size(121, 21);
            this.m_cboAdditionalInfo.TabIndex = 25;
            this.m_cboAdditionalInfo.SelectedIndexChanged += new System.EventHandler(this.cboAdditionalInfo_SelectedIndexChanged);
            // 
            // dateTheDate
            // 
            this.dateTheDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTheDate.Location = new System.Drawing.Point(424, 32);
            this.dateTheDate.Name = "dateTheDate";
            this.dateTheDate.Size = new System.Drawing.Size(144, 24);
            this.dateTheDate.TabIndex = 24;
            clsDate1.Date = new System.DateTime(2007, 10, 11, 0, 0, 0, 0);
            clsDate1.Status = 0;
            this.dateTheDate.Value = clsDate1;
            this.dateTheDate.evtValueChanged += new FamilyTree.Viewer.dgtValueChanged(this.dateTheDate_evtValueChanged);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Image = global::FamilyTree.Viewer.Properties.Resources.Cancel;
            this.cmdCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cmdCancel.Location = new System.Drawing.Point(362, 464);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(100, 30);
            this.cmdCancel.TabIndex = 21;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Image = global::FamilyTree.Viewer.Properties.Resources.OK;
            this.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cmdOK.Location = new System.Drawing.Point(468, 464);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(100, 30);
            this.cmdOK.TabIndex = 20;
            this.cmdOK.Text = "OK";
            this.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // txtComments
            // 
            this.txtComments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtComments.Location = new System.Drawing.Point(80, 56);
            this.txtComments.Multiline = true;
            this.txtComments.Name = "txtComments";
            this.txtComments.Size = new System.Drawing.Size(488, 64);
            this.txtComments.TabIndex = 19;
            this.txtComments.Text = "textBox2";
            this.txtComments.TextChanged += new System.EventHandler(this.txtComments_TextChanged);
            // 
            // cboPrefix
            // 
            this.cboPrefix.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPrefix.Items.AddRange(new object[] {
            "Birth Certificate:",
            "Census 1861:",
            "Census 1901:",
            "Death Certificate:",
            "Interview:",
            "Letter:",
            "Marriage Certificate:"});
            this.cboPrefix.Location = new System.Drawing.Point(80, 32);
            this.cboPrefix.Name = "cboPrefix";
            this.cboPrefix.Size = new System.Drawing.Size(208, 21);
            this.cboPrefix.TabIndex = 18;
            this.cboPrefix.SelectedIndexChanged += new System.EventHandler(this.cboPrefix_SelectedIndexChanged);
            // 
            // txtDescription
            // 
            this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescription.Location = new System.Drawing.Point(80, 8);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(488, 21);
            this.txtDescription.TabIndex = 17;
            this.txtDescription.Text = "txtDescription";
            this.txtDescription.TextChanged += new System.EventHandler(this.txtDescription_TextChanged);
            // 
            // m_grpCensus
            // 
            this.m_grpCensus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_grpCensus.Controls.Add(cmdCensusAddress);
            this.m_grpCensus.Controls.Add(label36);
            this.m_grpCensus.Controls.Add(label35);
            this.m_grpCensus.Controls.Add(label34);
            this.m_grpCensus.Controls.Add(label33);
            this.m_grpCensus.Controls.Add(label32);
            this.m_grpCensus.Controls.Add(this.m_txtCensusPage);
            this.m_grpCensus.Controls.Add(this.m_txtCensusFolio);
            this.m_grpCensus.Controls.Add(this.m_txtCensusPiece);
            this.m_grpCensus.Controls.Add(this.m_txtCensusSeries);
            this.m_grpCensus.Controls.Add(cmdCensusOpen);
            this.m_grpCensus.Controls.Add(this.m_txtCensusAddress);
            this.m_grpCensus.Location = new System.Drawing.Point(8, 139);
            this.m_grpCensus.Name = "m_grpCensus";
            this.m_grpCensus.Size = new System.Drawing.Size(560, 321);
            this.m_grpCensus.TabIndex = 26;
            this.m_grpCensus.TabStop = false;
            this.m_grpCensus.Text = "Census";
            // 
            // m_txtCensusPage
            // 
            this.m_txtCensusPage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_txtCensusPage.Location = new System.Drawing.Point(248, 40);
            this.m_txtCensusPage.Name = "m_txtCensusPage";
            this.m_txtCensusPage.Size = new System.Drawing.Size(75, 21);
            this.m_txtCensusPage.TabIndex = 6;
            this.m_txtCensusPage.Text = "textBox1";
            this.m_txtCensusPage.TextChanged += new System.EventHandler(this.evtAdditionalCensus_Changed);
            // 
            // m_txtCensusFolio
            // 
            this.m_txtCensusFolio.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_txtCensusFolio.Location = new System.Drawing.Point(167, 40);
            this.m_txtCensusFolio.Name = "m_txtCensusFolio";
            this.m_txtCensusFolio.Size = new System.Drawing.Size(75, 21);
            this.m_txtCensusFolio.TabIndex = 5;
            this.m_txtCensusFolio.Text = "textBox1";
            this.m_txtCensusFolio.TextChanged += new System.EventHandler(this.evtAdditionalCensus_Changed);
            // 
            // m_txtCensusPiece
            // 
            this.m_txtCensusPiece.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_txtCensusPiece.Location = new System.Drawing.Point(86, 40);
            this.m_txtCensusPiece.Name = "m_txtCensusPiece";
            this.m_txtCensusPiece.Size = new System.Drawing.Size(75, 21);
            this.m_txtCensusPiece.TabIndex = 4;
            this.m_txtCensusPiece.Text = "textBox1";
            this.m_txtCensusPiece.TextChanged += new System.EventHandler(this.evtAdditionalCensus_Changed);
            // 
            // m_txtCensusSeries
            // 
            this.m_txtCensusSeries.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_txtCensusSeries.Location = new System.Drawing.Point(8, 40);
            this.m_txtCensusSeries.Name = "m_txtCensusSeries";
            this.m_txtCensusSeries.Size = new System.Drawing.Size(75, 21);
            this.m_txtCensusSeries.TabIndex = 3;
            this.m_txtCensusSeries.Text = "textBox1";
            this.m_txtCensusSeries.TextChanged += new System.EventHandler(this.evtAdditionalCensus_Changed);
            // 
            // m_txtCensusAddress
            // 
            this.m_txtCensusAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_txtCensusAddress.Location = new System.Drawing.Point(8, 91);
            this.m_txtCensusAddress.Name = "m_txtCensusAddress";
            this.m_txtCensusAddress.Size = new System.Drawing.Size(520, 21);
            this.m_txtCensusAddress.TabIndex = 1;
            this.m_txtCensusAddress.Text = "textBox1";
            this.m_txtCensusAddress.TextChanged += new System.EventHandler(this.evtAdditionalCensus_Changed);
            // 
            // m_grpDeath
            // 
            this.m_grpDeath.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_grpDeath.Controls.Add(this.m_txtDeathReference);
            this.m_grpDeath.Controls.Add(label40);
            this.m_grpDeath.Controls.Add(this.m_txtDeathWhere);
            this.m_grpDeath.Controls.Add(label27);
            this.m_grpDeath.Controls.Add(this.m_txtDeathWhenReg);
            this.m_grpDeath.Controls.Add(label26);
            this.m_grpDeath.Controls.Add(this.m_txtDeathInformantDescription);
            this.m_grpDeath.Controls.Add(label25);
            this.m_grpDeath.Controls.Add(label24);
            this.m_grpDeath.Controls.Add(this.m_txtDeathInformantAddress);
            this.m_grpDeath.Controls.Add(this.m_txtDeathInformant);
            this.m_grpDeath.Controls.Add(label23);
            this.m_grpDeath.Controls.Add(label22);
            this.m_grpDeath.Controls.Add(label21);
            this.m_grpDeath.Controls.Add(label20);
            this.m_grpDeath.Controls.Add(this.m_txtDeathUsualAddress);
            this.m_grpDeath.Controls.Add(this.m_txtDeathCause);
            this.m_grpDeath.Controls.Add(this.m_txtDeathOccupation);
            this.m_grpDeath.Controls.Add(this.m_txtDeathDatePlace);
            this.m_grpDeath.Controls.Add(this.m_txtDeathSex);
            this.m_grpDeath.Controls.Add(label16);
            this.m_grpDeath.Controls.Add(this.m_txtDeathName);
            this.m_grpDeath.Controls.Add(label17);
            this.m_grpDeath.Controls.Add(this.m_txtDeathWhen);
            this.m_grpDeath.Controls.Add(label18);
            this.m_grpDeath.Controls.Add(label19);
            this.m_grpDeath.Controls.Add(this.m_txtDeathDistrict);
            this.m_grpDeath.Location = new System.Drawing.Point(8, 139);
            this.m_grpDeath.Name = "m_grpDeath";
            this.m_grpDeath.Size = new System.Drawing.Size(560, 321);
            this.m_grpDeath.TabIndex = 29;
            this.m_grpDeath.TabStop = false;
            this.m_grpDeath.Text = "Death Certificate";
            // 
            // m_txtDeathReference
            // 
            this.m_txtDeathReference.Location = new System.Drawing.Point(128, 274);
            this.m_txtDeathReference.Name = "m_txtDeathReference";
            this.m_txtDeathReference.Size = new System.Drawing.Size(288, 21);
            this.m_txtDeathReference.TabIndex = 33;
            this.m_txtDeathReference.Text = "textBox1";
            this.m_txtDeathReference.TextChanged += new System.EventHandler(this.evtAdditionalDeath_Changed);
            // 
            // m_txtDeathWhere
            // 
            this.m_txtDeathWhere.Location = new System.Drawing.Point(508, 48);
            this.m_txtDeathWhere.Name = "m_txtDeathWhere";
            this.m_txtDeathWhere.Size = new System.Drawing.Size(44, 21);
            this.m_txtDeathWhere.TabIndex = 12;
            this.m_txtDeathWhere.Text = "Where";
            this.m_txtDeathWhere.TextChanged += new System.EventHandler(this.evtAdditionalDeath_Changed);
            // 
            // m_txtDeathWhenReg
            // 
            this.m_txtDeathWhenReg.Location = new System.Drawing.Point(344, 248);
            this.m_txtDeathWhenReg.Name = "m_txtDeathWhenReg";
            this.m_txtDeathWhenReg.Size = new System.Drawing.Size(208, 21);
            this.m_txtDeathWhenReg.TabIndex = 30;
            this.m_txtDeathWhenReg.Text = "textBox2";
            this.m_txtDeathWhenReg.TextChanged += new System.EventHandler(this.evtAdditionalDeath_Changed);
            // 
            // m_txtDeathInformantDescription
            // 
            this.m_txtDeathInformantDescription.Location = new System.Drawing.Point(344, 224);
            this.m_txtDeathInformantDescription.Name = "m_txtDeathInformantDescription";
            this.m_txtDeathInformantDescription.Size = new System.Drawing.Size(208, 21);
            this.m_txtDeathInformantDescription.TabIndex = 28;
            this.m_txtDeathInformantDescription.Text = "textBox2";
            this.m_txtDeathInformantDescription.TextChanged += new System.EventHandler(this.evtAdditionalDeath_Changed);
            // 
            // m_txtDeathInformantAddress
            // 
            this.m_txtDeathInformantAddress.Location = new System.Drawing.Point(128, 248);
            this.m_txtDeathInformantAddress.Name = "m_txtDeathInformantAddress";
            this.m_txtDeathInformantAddress.Size = new System.Drawing.Size(136, 21);
            this.m_txtDeathInformantAddress.TabIndex = 25;
            this.m_txtDeathInformantAddress.Text = "textBox1";
            this.m_txtDeathInformantAddress.TextChanged += new System.EventHandler(this.evtAdditionalDeath_Changed);
            // 
            // m_txtDeathInformant
            // 
            this.m_txtDeathInformant.Location = new System.Drawing.Point(128, 224);
            this.m_txtDeathInformant.Name = "m_txtDeathInformant";
            this.m_txtDeathInformant.Size = new System.Drawing.Size(136, 21);
            this.m_txtDeathInformant.TabIndex = 24;
            this.m_txtDeathInformant.Text = "textBox1";
            this.m_txtDeathInformant.TextChanged += new System.EventHandler(this.evtAdditionalDeath_Changed);
            // 
            // m_txtDeathUsualAddress
            // 
            this.m_txtDeathUsualAddress.Location = new System.Drawing.Point(344, 120);
            this.m_txtDeathUsualAddress.Name = "m_txtDeathUsualAddress";
            this.m_txtDeathUsualAddress.Size = new System.Drawing.Size(208, 21);
            this.m_txtDeathUsualAddress.TabIndex = 19;
            this.m_txtDeathUsualAddress.Text = "textBox2";
            this.m_txtDeathUsualAddress.TextChanged += new System.EventHandler(this.evtAdditionalDeath_Changed);
            // 
            // m_txtDeathCause
            // 
            this.m_txtDeathCause.AcceptsReturn = true;
            this.m_txtDeathCause.Location = new System.Drawing.Point(128, 144);
            this.m_txtDeathCause.Multiline = true;
            this.m_txtDeathCause.Name = "m_txtDeathCause";
            this.m_txtDeathCause.Size = new System.Drawing.Size(424, 72);
            this.m_txtDeathCause.TabIndex = 18;
            this.m_txtDeathCause.Text = "textBox1";
            this.m_txtDeathCause.TextChanged += new System.EventHandler(this.evtAdditionalDeath_Changed);
            // 
            // m_txtDeathOccupation
            // 
            this.m_txtDeathOccupation.Location = new System.Drawing.Point(128, 120);
            this.m_txtDeathOccupation.Name = "m_txtDeathOccupation";
            this.m_txtDeathOccupation.Size = new System.Drawing.Size(136, 21);
            this.m_txtDeathOccupation.TabIndex = 17;
            this.m_txtDeathOccupation.Text = "textBox2";
            this.m_txtDeathOccupation.TextChanged += new System.EventHandler(this.evtAdditionalDeath_Changed);
            // 
            // m_txtDeathDatePlace
            // 
            this.m_txtDeathDatePlace.Location = new System.Drawing.Point(128, 96);
            this.m_txtDeathDatePlace.Name = "m_txtDeathDatePlace";
            this.m_txtDeathDatePlace.Size = new System.Drawing.Size(424, 21);
            this.m_txtDeathDatePlace.TabIndex = 16;
            this.m_txtDeathDatePlace.Text = "m_txtDeathDatePlace";
            this.m_txtDeathDatePlace.TextChanged += new System.EventHandler(this.evtAdditionalDeath_Changed);
            // 
            // m_txtDeathSex
            // 
            this.m_txtDeathSex.Location = new System.Drawing.Point(392, 72);
            this.m_txtDeathSex.Name = "m_txtDeathSex";
            this.m_txtDeathSex.Size = new System.Drawing.Size(160, 21);
            this.m_txtDeathSex.TabIndex = 15;
            this.m_txtDeathSex.Text = "m_txtDeathSex";
            this.m_txtDeathSex.TextChanged += new System.EventHandler(this.evtAdditionalDeath_Changed);
            // 
            // m_txtDeathName
            // 
            this.m_txtDeathName.Location = new System.Drawing.Point(128, 72);
            this.m_txtDeathName.Name = "m_txtDeathName";
            this.m_txtDeathName.Size = new System.Drawing.Size(136, 21);
            this.m_txtDeathName.TabIndex = 13;
            this.m_txtDeathName.Text = "m_txtDeathName";
            this.m_txtDeathName.TextChanged += new System.EventHandler(this.evtAdditionalDeath_Changed);
            // 
            // m_txtDeathWhen
            // 
            this.m_txtDeathWhen.Location = new System.Drawing.Point(128, 48);
            this.m_txtDeathWhen.Name = "m_txtDeathWhen";
            this.m_txtDeathWhen.Size = new System.Drawing.Size(374, 21);
            this.m_txtDeathWhen.TabIndex = 11;
            this.m_txtDeathWhen.Text = "When";
            this.m_txtDeathWhen.TextChanged += new System.EventHandler(this.evtAdditionalDeath_Changed);
            // 
            // m_txtDeathDistrict
            // 
            this.m_txtDeathDistrict.Location = new System.Drawing.Point(128, 24);
            this.m_txtDeathDistrict.Name = "m_txtDeathDistrict";
            this.m_txtDeathDistrict.Size = new System.Drawing.Size(424, 21);
            this.m_txtDeathDistrict.TabIndex = 8;
            this.m_txtDeathDistrict.Text = "m_txtDeathDistrict";
            this.m_txtDeathDistrict.TextChanged += new System.EventHandler(this.evtAdditionalDeath_Changed);
            // 
            // m_grpBirth
            // 
            this.m_grpBirth.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_grpBirth.Controls.Add(this.m_txtBirthReference);
            this.m_grpBirth.Controls.Add(label39);
            this.m_grpBirth.Controls.Add(this.m_txtBirthMotherDetails);
            this.m_grpBirth.Controls.Add(this.m_txtBirthInformantAddress);
            this.m_grpBirth.Controls.Add(this.m_dtpBirthWhen);
            this.m_grpBirth.Controls.Add(this.m_txtBirthWhenReg);
            this.m_grpBirth.Controls.Add(this.m_txtBirthInformant);
            this.m_grpBirth.Controls.Add(this.m_txtBirthMother);
            this.m_grpBirth.Controls.Add(this.m_txtBirthFatherOccupation);
            this.m_grpBirth.Controls.Add(this.m_txtBirthFather);
            this.m_grpBirth.Controls.Add(label15);
            this.m_grpBirth.Controls.Add(label14);
            this.m_grpBirth.Controls.Add(label13);
            this.m_grpBirth.Controls.Add(label12);
            this.m_grpBirth.Controls.Add(label11);
            this.m_grpBirth.Controls.Add(this.m_txtBirthSex);
            this.m_grpBirth.Controls.Add(label10);
            this.m_grpBirth.Controls.Add(this.m_txtBirthName);
            this.m_grpBirth.Controls.Add(label9);
            this.m_grpBirth.Controls.Add(this.m_txtBirthWhenWhere);
            this.m_grpBirth.Controls.Add(label8);
            this.m_grpBirth.Controls.Add(label7);
            this.m_grpBirth.Controls.Add(this.m_txtBirthDistrict);
            this.m_grpBirth.Location = new System.Drawing.Point(8, 139);
            this.m_grpBirth.Name = "m_grpBirth";
            this.m_grpBirth.Size = new System.Drawing.Size(560, 321);
            this.m_grpBirth.TabIndex = 28;
            this.m_grpBirth.TabStop = false;
            this.m_grpBirth.Text = "Birth Certificate";
            // 
            // m_txtBirthReference
            // 
            this.m_txtBirthReference.Location = new System.Drawing.Point(128, 168);
            this.m_txtBirthReference.Name = "m_txtBirthReference";
            this.m_txtBirthReference.Size = new System.Drawing.Size(288, 21);
            this.m_txtBirthReference.TabIndex = 27;
            this.m_txtBirthReference.Text = "textBox1";
            this.m_txtBirthReference.TextChanged += new System.EventHandler(this.evtAdditionalBirth_Changed);
            // 
            // m_txtBirthMotherDetails
            // 
            this.m_txtBirthMotherDetails.Location = new System.Drawing.Point(270, 120);
            this.m_txtBirthMotherDetails.Name = "m_txtBirthMotherDetails";
            this.m_txtBirthMotherDetails.Size = new System.Drawing.Size(282, 21);
            this.m_txtBirthMotherDetails.TabIndex = 16;
            this.m_txtBirthMotherDetails.Text = "textBox1";
            this.toolTip1.SetToolTip(this.m_txtBirthMotherDetails, "Mother details");
            this.m_txtBirthMotherDetails.TextChanged += new System.EventHandler(this.evtAdditionalBirth_Changed);
            // 
            // m_txtBirthInformantAddress
            // 
            this.m_txtBirthInformantAddress.Location = new System.Drawing.Point(270, 144);
            this.m_txtBirthInformantAddress.Name = "m_txtBirthInformantAddress";
            this.m_txtBirthInformantAddress.Size = new System.Drawing.Size(282, 21);
            this.m_txtBirthInformantAddress.TabIndex = 19;
            this.m_txtBirthInformantAddress.Text = "textBox1";
            this.toolTip1.SetToolTip(this.m_txtBirthInformantAddress, "Informant address");
            this.m_txtBirthInformantAddress.TextChanged += new System.EventHandler(this.evtAdditionalBirth_Changed);
            // 
            // m_dtpBirthWhen
            // 
            this.m_dtpBirthWhen.CustomFormat = "d MMM yyyy";
            this.m_dtpBirthWhen.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.m_dtpBirthWhen.Location = new System.Drawing.Point(128, 47);
            this.m_dtpBirthWhen.Name = "m_dtpBirthWhen";
            this.m_dtpBirthWhen.Size = new System.Drawing.Size(136, 21);
            this.m_dtpBirthWhen.TabIndex = 1;
            this.m_dtpBirthWhen.ValueChanged += new System.EventHandler(this.evtAdditionalBirth_Changed);
            // 
            // m_txtBirthWhenReg
            // 
            this.m_txtBirthWhenReg.Location = new System.Drawing.Point(392, 197);
            this.m_txtBirthWhenReg.Name = "m_txtBirthWhenReg";
            this.m_txtBirthWhenReg.Size = new System.Drawing.Size(160, 21);
            this.m_txtBirthWhenReg.TabIndex = 20;
            this.m_txtBirthWhenReg.Text = "textBox1";
            this.m_txtBirthWhenReg.TextChanged += new System.EventHandler(this.evtAdditionalBirth_Changed);
            // 
            // m_txtBirthInformant
            // 
            this.m_txtBirthInformant.Location = new System.Drawing.Point(128, 144);
            this.m_txtBirthInformant.Name = "m_txtBirthInformant";
            this.m_txtBirthInformant.Size = new System.Drawing.Size(136, 21);
            this.m_txtBirthInformant.TabIndex = 17;
            this.m_txtBirthInformant.Text = "textBox1";
            this.m_txtBirthInformant.TextChanged += new System.EventHandler(this.evtAdditionalBirth_Changed);
            // 
            // m_txtBirthMother
            // 
            this.m_txtBirthMother.Location = new System.Drawing.Point(128, 120);
            this.m_txtBirthMother.Name = "m_txtBirthMother";
            this.m_txtBirthMother.Size = new System.Drawing.Size(136, 21);
            this.m_txtBirthMother.TabIndex = 15;
            this.m_txtBirthMother.Text = "textBox1";
            this.m_txtBirthMother.TextChanged += new System.EventHandler(this.evtAdditionalBirth_Changed);
            // 
            // m_txtBirthFatherOccupation
            // 
            this.m_txtBirthFatherOccupation.Location = new System.Drawing.Point(392, 96);
            this.m_txtBirthFatherOccupation.Name = "m_txtBirthFatherOccupation";
            this.m_txtBirthFatherOccupation.Size = new System.Drawing.Size(160, 21);
            this.m_txtBirthFatherOccupation.TabIndex = 14;
            this.m_txtBirthFatherOccupation.Text = "textBox1";
            this.m_txtBirthFatherOccupation.TextChanged += new System.EventHandler(this.evtAdditionalBirth_Changed);
            // 
            // m_txtBirthFather
            // 
            this.m_txtBirthFather.Location = new System.Drawing.Point(128, 96);
            this.m_txtBirthFather.Name = "m_txtBirthFather";
            this.m_txtBirthFather.Size = new System.Drawing.Size(136, 21);
            this.m_txtBirthFather.TabIndex = 13;
            this.m_txtBirthFather.Text = "textBox1";
            this.m_txtBirthFather.TextChanged += new System.EventHandler(this.evtAdditionalBirth_Changed);
            // 
            // m_txtBirthSex
            // 
            this.m_txtBirthSex.Location = new System.Drawing.Point(392, 72);
            this.m_txtBirthSex.Name = "m_txtBirthSex";
            this.m_txtBirthSex.Size = new System.Drawing.Size(160, 21);
            this.m_txtBirthSex.TabIndex = 7;
            this.m_txtBirthSex.Text = "textBox1";
            this.m_txtBirthSex.TextChanged += new System.EventHandler(this.evtAdditionalBirth_Changed);
            // 
            // m_txtBirthName
            // 
            this.m_txtBirthName.Location = new System.Drawing.Point(128, 72);
            this.m_txtBirthName.Name = "m_txtBirthName";
            this.m_txtBirthName.Size = new System.Drawing.Size(136, 21);
            this.m_txtBirthName.TabIndex = 5;
            this.m_txtBirthName.Text = "textBox1";
            this.m_txtBirthName.TextChanged += new System.EventHandler(this.evtAdditionalBirth_Changed);
            // 
            // m_txtBirthWhenWhere
            // 
            this.m_txtBirthWhenWhere.Location = new System.Drawing.Point(275, 48);
            this.m_txtBirthWhenWhere.Name = "m_txtBirthWhenWhere";
            this.m_txtBirthWhenWhere.Size = new System.Drawing.Size(277, 21);
            this.m_txtBirthWhenWhere.TabIndex = 2;
            this.m_txtBirthWhenWhere.Text = "textBox1";
            this.m_txtBirthWhenWhere.TextChanged += new System.EventHandler(this.evtAdditionalBirth_Changed);
            // 
            // m_txtBirthDistrict
            // 
            this.m_txtBirthDistrict.Location = new System.Drawing.Point(128, 24);
            this.m_txtBirthDistrict.Name = "m_txtBirthDistrict";
            this.m_txtBirthDistrict.Size = new System.Drawing.Size(424, 21);
            this.m_txtBirthDistrict.TabIndex = 0;
            this.m_txtBirthDistrict.Text = "textBox1";
            this.m_txtBirthDistrict.TextChanged += new System.EventHandler(this.evtAdditionalBirth_Changed);
            // 
            // m_grpMarriage
            // 
            this.m_grpMarriage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_grpMarriage.Controls.Add(this.m_txtMarrGro);
            this.m_grpMarriage.Controls.Add(label38);
            this.m_grpMarriage.Controls.Add(label37);
            this.m_grpMarriage.Controls.Add(this.m_dtpMarrWhen);
            this.m_grpMarriage.Controls.Add(this.m_txtMarrWitness);
            this.m_grpMarriage.Controls.Add(this.m_txtMarrLocation);
            this.m_grpMarriage.Controls.Add(label6);
            this.m_grpMarriage.Controls.Add(label5);
            this.m_grpMarriage.Controls.Add(label4);
            this.m_grpMarriage.Controls.Add(label3);
            this.m_grpMarriage.Controls.Add(label2);
            this.m_grpMarriage.Controls.Add(label1);
            this.m_grpMarriage.Controls.Add(this.m_txtMarrBrideFatherOcc);
            this.m_grpMarriage.Controls.Add(this.m_txtMarrBrideFather);
            this.m_grpMarriage.Controls.Add(this.m_txtMarrBrideLoca);
            this.m_grpMarriage.Controls.Add(this.m_txtMarrBrideOccu);
            this.m_grpMarriage.Controls.Add(this.m_txtMarrBrideAge);
            this.m_grpMarriage.Controls.Add(this.m_txtMarrBride);
            this.m_grpMarriage.Controls.Add(this.m_txtMarrGroomFatherOcc);
            this.m_grpMarriage.Controls.Add(this.m_txtMarrGroomFather);
            this.m_grpMarriage.Controls.Add(this.m_txtMarrGroomLoca);
            this.m_grpMarriage.Controls.Add(this.m_txtMarrGroomOccu);
            this.m_grpMarriage.Controls.Add(this.m_txtMarrGroomAge);
            this.m_grpMarriage.Controls.Add(this.m_txtMarrGroom);
            this.m_grpMarriage.Location = new System.Drawing.Point(8, 140);
            this.m_grpMarriage.Name = "m_grpMarriage";
            this.m_grpMarriage.Size = new System.Drawing.Size(560, 320);
            this.m_grpMarriage.TabIndex = 27;
            this.m_grpMarriage.TabStop = false;
            this.m_grpMarriage.Text = "Marriage Certificate";
            // 
            // m_txtMarrGro
            // 
            this.m_txtMarrGro.Location = new System.Drawing.Point(86, 190);
            this.m_txtMarrGro.Name = "m_txtMarrGro";
            this.m_txtMarrGro.Size = new System.Drawing.Size(288, 21);
            this.m_txtMarrGro.TabIndex = 25;
            this.m_txtMarrGro.Text = "textBox1";
            this.m_txtMarrGro.TextChanged += new System.EventHandler(this.evtAdditionalMarriage_Changed);
            // 
            // m_dtpMarrWhen
            // 
            this.m_dtpMarrWhen.CustomFormat = "d MMM yyyy";
            this.m_dtpMarrWhen.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.m_dtpMarrWhen.Location = new System.Drawing.Point(86, 20);
            this.m_dtpMarrWhen.Name = "m_dtpMarrWhen";
            this.m_dtpMarrWhen.Size = new System.Drawing.Size(136, 21);
            this.m_dtpMarrWhen.TabIndex = 22;
            this.m_dtpMarrWhen.ValueChanged += new System.EventHandler(this.evtAdditionalMarriage_Changed);
            // 
            // m_txtMarrWitness
            // 
            this.m_txtMarrWitness.Location = new System.Drawing.Point(86, 165);
            this.m_txtMarrWitness.Name = "m_txtMarrWitness";
            this.m_txtMarrWitness.Size = new System.Drawing.Size(288, 21);
            this.m_txtMarrWitness.TabIndex = 18;
            this.m_txtMarrWitness.Text = "textBox1";
            this.m_txtMarrWitness.TextChanged += new System.EventHandler(this.evtAdditionalMarriage_Changed);
            // 
            // m_txtMarrLocation
            // 
            this.m_txtMarrLocation.Location = new System.Drawing.Point(86, 45);
            this.m_txtMarrLocation.Name = "m_txtMarrLocation";
            this.m_txtMarrLocation.Size = new System.Drawing.Size(464, 21);
            this.m_txtMarrLocation.TabIndex = 0;
            this.m_txtMarrLocation.Text = "m_txtMarrLocation";
            this.m_txtMarrLocation.TextChanged += new System.EventHandler(this.evtAdditionalMarriage_Changed);
            // 
            // m_txtMarrBrideFatherOcc
            // 
            this.m_txtMarrBrideFatherOcc.Location = new System.Drawing.Point(254, 141);
            this.m_txtMarrBrideFatherOcc.Name = "m_txtMarrBrideFatherOcc";
            this.m_txtMarrBrideFatherOcc.Size = new System.Drawing.Size(120, 21);
            this.m_txtMarrBrideFatherOcc.TabIndex = 12;
            this.m_txtMarrBrideFatherOcc.Text = "textBox1";
            this.m_txtMarrBrideFatherOcc.TextChanged += new System.EventHandler(this.evtAdditionalMarriage_Changed);
            // 
            // m_txtMarrBrideFather
            // 
            this.m_txtMarrBrideFather.Location = new System.Drawing.Point(86, 141);
            this.m_txtMarrBrideFather.Name = "m_txtMarrBrideFather";
            this.m_txtMarrBrideFather.Size = new System.Drawing.Size(120, 21);
            this.m_txtMarrBrideFather.TabIndex = 11;
            this.m_txtMarrBrideFather.Text = "textBox1";
            this.m_txtMarrBrideFather.TextChanged += new System.EventHandler(this.evtAdditionalMarriage_Changed);
            // 
            // m_txtMarrBrideLoca
            // 
            this.m_txtMarrBrideLoca.Location = new System.Drawing.Point(382, 93);
            this.m_txtMarrBrideLoca.Name = "m_txtMarrBrideLoca";
            this.m_txtMarrBrideLoca.Size = new System.Drawing.Size(168, 21);
            this.m_txtMarrBrideLoca.TabIndex = 8;
            this.m_txtMarrBrideLoca.Text = "textBox1";
            this.m_txtMarrBrideLoca.TextChanged += new System.EventHandler(this.evtAdditionalMarriage_Changed);
            // 
            // m_txtMarrBrideOccu
            // 
            this.m_txtMarrBrideOccu.Location = new System.Drawing.Point(254, 93);
            this.m_txtMarrBrideOccu.Name = "m_txtMarrBrideOccu";
            this.m_txtMarrBrideOccu.Size = new System.Drawing.Size(120, 21);
            this.m_txtMarrBrideOccu.TabIndex = 7;
            this.m_txtMarrBrideOccu.Text = "textBox1";
            this.m_txtMarrBrideOccu.TextChanged += new System.EventHandler(this.evtAdditionalMarriage_Changed);
            // 
            // m_txtMarrBrideAge
            // 
            this.m_txtMarrBrideAge.Location = new System.Drawing.Point(214, 93);
            this.m_txtMarrBrideAge.Name = "m_txtMarrBrideAge";
            this.m_txtMarrBrideAge.Size = new System.Drawing.Size(32, 21);
            this.m_txtMarrBrideAge.TabIndex = 6;
            this.m_txtMarrBrideAge.Text = "textBox1";
            this.m_txtMarrBrideAge.TextChanged += new System.EventHandler(this.evtAdditionalMarriage_Changed);
            // 
            // m_txtMarrBride
            // 
            this.m_txtMarrBride.Location = new System.Drawing.Point(86, 93);
            this.m_txtMarrBride.Name = "m_txtMarrBride";
            this.m_txtMarrBride.Size = new System.Drawing.Size(120, 21);
            this.m_txtMarrBride.TabIndex = 5;
            this.m_txtMarrBride.Text = "textBox1";
            this.m_txtMarrBride.TextChanged += new System.EventHandler(this.evtAdditionalMarriage_Changed);
            // 
            // m_txtMarrGroomFatherOcc
            // 
            this.m_txtMarrGroomFatherOcc.Location = new System.Drawing.Point(254, 117);
            this.m_txtMarrGroomFatherOcc.Name = "m_txtMarrGroomFatherOcc";
            this.m_txtMarrGroomFatherOcc.Size = new System.Drawing.Size(120, 21);
            this.m_txtMarrGroomFatherOcc.TabIndex = 10;
            this.m_txtMarrGroomFatherOcc.Text = "textBox1";
            this.m_txtMarrGroomFatherOcc.TextChanged += new System.EventHandler(this.evtAdditionalMarriage_Changed);
            // 
            // m_txtMarrGroomFather
            // 
            this.m_txtMarrGroomFather.Location = new System.Drawing.Point(86, 117);
            this.m_txtMarrGroomFather.Name = "m_txtMarrGroomFather";
            this.m_txtMarrGroomFather.Size = new System.Drawing.Size(120, 21);
            this.m_txtMarrGroomFather.TabIndex = 9;
            this.m_txtMarrGroomFather.Text = "textBox1";
            this.m_txtMarrGroomFather.TextChanged += new System.EventHandler(this.evtAdditionalMarriage_Changed);
            // 
            // m_txtMarrGroomLoca
            // 
            this.m_txtMarrGroomLoca.Location = new System.Drawing.Point(382, 69);
            this.m_txtMarrGroomLoca.Name = "m_txtMarrGroomLoca";
            this.m_txtMarrGroomLoca.Size = new System.Drawing.Size(168, 21);
            this.m_txtMarrGroomLoca.TabIndex = 4;
            this.m_txtMarrGroomLoca.Text = "textBox1";
            this.m_txtMarrGroomLoca.TextChanged += new System.EventHandler(this.evtAdditionalMarriage_Changed);
            // 
            // m_txtMarrGroomOccu
            // 
            this.m_txtMarrGroomOccu.Location = new System.Drawing.Point(254, 69);
            this.m_txtMarrGroomOccu.Name = "m_txtMarrGroomOccu";
            this.m_txtMarrGroomOccu.Size = new System.Drawing.Size(120, 21);
            this.m_txtMarrGroomOccu.TabIndex = 3;
            this.m_txtMarrGroomOccu.Text = "textBox1";
            this.m_txtMarrGroomOccu.TextChanged += new System.EventHandler(this.evtAdditionalMarriage_Changed);
            // 
            // m_txtMarrGroomAge
            // 
            this.m_txtMarrGroomAge.Location = new System.Drawing.Point(214, 69);
            this.m_txtMarrGroomAge.Name = "m_txtMarrGroomAge";
            this.m_txtMarrGroomAge.Size = new System.Drawing.Size(32, 21);
            this.m_txtMarrGroomAge.TabIndex = 2;
            this.m_txtMarrGroomAge.Text = "textBox1";
            this.m_txtMarrGroomAge.TextChanged += new System.EventHandler(this.evtAdditionalMarriage_Changed);
            // 
            // m_txtMarrGroom
            // 
            this.m_txtMarrGroom.Location = new System.Drawing.Point(86, 69);
            this.m_txtMarrGroom.Name = "m_txtMarrGroom";
            this.m_txtMarrGroom.Size = new System.Drawing.Size(120, 21);
            this.m_txtMarrGroom.TabIndex = 1;
            this.m_txtMarrGroom.Text = "textBox1";
            this.m_txtMarrGroom.TextChanged += new System.EventHandler(this.evtAdditionalMarriage_Changed);
            // 
            // radioDate
            // 
            this.radioDate.Checked = true;
            this.radioDate.Location = new System.Drawing.Point(8, 1);
            this.radioDate.Name = "radioDate";
            this.radioDate.Size = new System.Drawing.Size(104, 24);
            this.radioDate.TabIndex = 23;
            this.radioDate.TabStop = true;
            this.radioDate.Text = "Date Order";
            this.radioDate.CheckedChanged += new System.EventHandler(this.radioDate_CheckedChanged);
            // 
            // radioAlpha
            // 
            this.radioAlpha.Location = new System.Drawing.Point(120, 1);
            this.radioAlpha.Name = "radioAlpha";
            this.radioAlpha.Size = new System.Drawing.Size(128, 24);
            this.radioAlpha.TabIndex = 22;
            this.radioAlpha.Text = "Alphabetical Order";
            this.radioAlpha.CheckedChanged += new System.EventHandler(this.radioAlpha_CheckedChanged);
            // 
            // m_panList
            // 
            this.m_panList.Controls.Add(this.m_lstSources);
            this.m_panList.Controls.Add(this.cmdAddSource);
            this.m_panList.Controls.Add(this.cmdDeleteSource);
            this.m_panList.Controls.Add(this.radioDate);
            this.m_panList.Controls.Add(this.radioAlpha);
            this.m_panList.Dock = System.Windows.Forms.DockStyle.Left;
            this.m_panList.Location = new System.Drawing.Point(0, 0);
            this.m_panList.Name = "m_panList";
            this.m_panList.Size = new System.Drawing.Size(400, 505);
            this.m_panList.TabIndex = 20;
            // 
            // cmdAddSource
            // 
            this.cmdAddSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdAddSource.Image = global::FamilyTree.Viewer.Properties.Resources.add;
            this.cmdAddSource.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cmdAddSource.Location = new System.Drawing.Point(4, 464);
            this.cmdAddSource.Name = "cmdAddSource";
            this.cmdAddSource.Size = new System.Drawing.Size(100, 30);
            this.cmdAddSource.TabIndex = 16;
            this.cmdAddSource.Text = "Add";
            this.cmdAddSource.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdAddSource.Click += new System.EventHandler(this.cmdAddSource_Click);
            // 
            // cmdDeleteSource
            // 
            this.cmdDeleteSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdDeleteSource.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdDeleteSource.Image = global::FamilyTree.Viewer.Properties.Resources.delete;
            this.cmdDeleteSource.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cmdDeleteSource.Location = new System.Drawing.Point(110, 464);
            this.cmdDeleteSource.Name = "cmdDeleteSource";
            this.cmdDeleteSource.Size = new System.Drawing.Size(100, 30);
            this.cmdDeleteSource.TabIndex = 15;
            this.cmdDeleteSource.Text = "Delete";
            this.cmdDeleteSource.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdDeleteSource.Click += new System.EventHandler(this.cmdDeleteSource_Click);
            // 
            // frmEditSources
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(984, 617);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.m_panList);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.gridReferences);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "frmEditSources";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Sources";
            this.Shown += new System.EventHandler(this.frmEditSources_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.gridReferences)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.m_grpCensus.ResumeLayout(false);
            this.m_grpCensus.PerformLayout();
            this.m_grpDeath.ResumeLayout(false);
            this.m_grpDeath.PerformLayout();
            this.m_grpBirth.ResumeLayout(false);
            this.m_grpBirth.PerformLayout();
            this.m_grpMarriage.ResumeLayout(false);
            this.m_grpMarriage.PerformLayout();
            this.m_panList.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.ImageList imageList16x16;
        private System.Windows.Forms.ListBox m_lstSources;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.DataGrid gridReferences;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button cmdDeleteSource;
        private System.Windows.Forms.Button cmdAddSource;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.ComboBox cboPrefix;
        private System.Windows.Forms.TextBox txtComments;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.RadioButton radioAlpha;
        private System.Windows.Forms.RadioButton radioDate;
        private FamilyTree.Viewer.ucDate dateTheDate;
        private System.Windows.Forms.Panel m_panList;
        private System.Windows.Forms.ComboBox m_cboAdditionalInfo;
        private System.Windows.Forms.GroupBox m_grpCensus;
        private System.Windows.Forms.GroupBox m_grpMarriage;
        private System.Windows.Forms.GroupBox m_grpBirth;
        private System.Windows.Forms.GroupBox m_grpDeath;
        private System.Windows.Forms.TextBox m_txtCensusAddress;
        private System.Windows.Forms.TextBox m_txtMarrGroom;
        private System.Windows.Forms.TextBox m_txtMarrGroomAge;
        private System.Windows.Forms.TextBox m_txtMarrGroomOccu;
        private System.Windows.Forms.TextBox m_txtMarrGroomLoca;
        private System.Windows.Forms.TextBox m_txtMarrGroomFather;
        private System.Windows.Forms.TextBox m_txtMarrGroomFatherOcc;
        private System.Windows.Forms.TextBox m_txtMarrBrideLoca;
        private System.Windows.Forms.TextBox m_txtMarrBrideOccu;
        private System.Windows.Forms.TextBox m_txtMarrBrideAge;
        private System.Windows.Forms.TextBox m_txtMarrBride;
        private System.Windows.Forms.TextBox m_txtMarrBrideFatherOcc;
        private System.Windows.Forms.TextBox m_txtMarrBrideFather;
        private System.Windows.Forms.TextBox m_txtMarrLocation;
        private System.Windows.Forms.TextBox m_txtMarrWitness;
        private System.Windows.Forms.TextBox m_txtBirthDistrict;
        private System.Windows.Forms.TextBox m_txtBirthName;
        private System.Windows.Forms.TextBox m_txtBirthSex;
        private System.Windows.Forms.TextBox m_txtBirthFather;
        private System.Windows.Forms.TextBox m_txtBirthMother;
        private System.Windows.Forms.TextBox m_txtBirthInformant;
        private System.Windows.Forms.TextBox m_txtBirthWhenReg;
        private System.Windows.Forms.TextBox m_txtBirthWhenWhere;
        private System.Windows.Forms.TextBox m_txtBirthFatherOccupation;
        private System.Windows.Forms.TextBox m_txtDeathSex;
        private System.Windows.Forms.TextBox m_txtDeathName;
        private System.Windows.Forms.TextBox m_txtDeathWhen;
        private System.Windows.Forms.TextBox m_txtDeathDistrict;
        private System.Windows.Forms.TextBox m_txtDeathDatePlace;
        private System.Windows.Forms.TextBox m_txtDeathOccupation;
        private System.Windows.Forms.TextBox m_txtDeathCause;
        private System.Windows.Forms.TextBox m_txtDeathInformant;
        private System.Windows.Forms.TextBox m_txtDeathInformantAddress;
        private System.Windows.Forms.TextBox m_txtDeathWhenReg;
        private System.Windows.Forms.TextBox m_txtDeathUsualAddress;
        private System.Windows.Forms.TextBox m_txtDeathInformantDescription;
        private System.Windows.Forms.ComboBox m_cboRepository;
        private TextBox m_txtDeathWhere;
        private TextBox m_txtCensusPiece;
        private TextBox m_txtCensusSeries;
        private TextBox m_txtCensusPage;
        private TextBox m_txtCensusFolio;
        private DateTimePicker m_dtpBirthWhen;
        private TextBox m_txtBirthInformantAddress;
        private TextBox m_txtBirthMotherDetails;
        private DateTimePicker m_dtpMarrWhen;
        private TextBox m_txtMarrGro;
        private TextBox m_txtBirthReference;
        private TextBox m_txtDeathReference;
        private ToolTip toolTip1;
    }
}
