using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using FamilyTree.Objects;

namespace FamilyTree.Viewer
{
    public partial class frmEditPerson : System.Windows.Forms.Form
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
            System.Windows.Forms.TabPage tabBasic;
            FamilyTree.Objects.clsDate clsDate1 = new FamilyTree.Objects.clsDate();
            FamilyTree.Objects.clsDate clsDate2 = new FamilyTree.Objects.clsDate();
            System.Windows.Forms.Label label7;
            System.Windows.Forms.Label label6;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.TabPage tabRelationships;
            System.Windows.Forms.Label label11;
            System.Windows.Forms.Label label10;
            System.Windows.Forms.Label label9;
            System.Windows.Forms.Label label8;
            System.Windows.Forms.Button cmdDeleteRelationship;
            System.Windows.Forms.Button AddRelationship;
            FamilyTree.Objects.clsDate clsDate3 = new FamilyTree.Objects.clsDate();
            FamilyTree.Objects.clsDate clsDate4 = new FamilyTree.Objects.clsDate();
            System.Windows.Forms.ImageList oImageList16x16;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEditPerson));
            System.Windows.Forms.TabPage tabFacts;
            System.Windows.Forms.Button cmdDeleteFact;
            System.Windows.Forms.ToolStripMenuItem menuEditLocation;
            System.Windows.Forms.Button cmdAddFact;
            System.Windows.Forms.TabPage tabAdvanced;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label13;
            System.Windows.Forms.Label label12;
            System.Windows.Forms.GroupBox groupBox2;
            System.Windows.Forms.TabPage tabToDo;
            System.Windows.Forms.Button cmdDeleteToDo;
            System.Windows.Forms.Button cmdAddToDo;
            System.Windows.Forms.Button cmdDeleteSource;
            System.Windows.Forms.Button cmdAddSource;
            System.Windows.Forms.Button cmdCancel;
            System.Windows.Forms.Button cmdOK;
            System.Windows.Forms.Button cmdRelationshipAddress;
            this.m_dateDoB = new FamilyTree.Viewer.ucDate();
            this.m_dateDoD = new FamilyTree.Viewer.ucDate();
            this.m_txtComments = new System.Windows.Forms.TextBox();
            this.m_chkChildrenKnown = new System.Windows.Forms.CheckBox();
            this.cboSex_ = new System.Windows.Forms.ComboBox();
            this.m_labMaidenName = new System.Windows.Forms.Label();
            this.m_txtMaidenName = new System.Windows.Forms.TextBox();
            this.m_txtForename = new System.Windows.Forms.TextBox();
            this.txtSurname_ = new System.Windows.Forms.TextBox();
            this.m_cboRelationshipType = new System.Windows.Forms.ComboBox();
            this.m_cboTerminated = new System.Windows.Forms.ComboBox();
            this.m_txtRelationComments = new System.Windows.Forms.TextBox();
            this.m_txtRelationLocation = new System.Windows.Forms.TextBox();
            this.m_cboAddPartner = new System.Windows.Forms.ComboBox();
            this.m_dateRelationEnd = new FamilyTree.Viewer.ucDate();
            this.m_dateRelationStart = new FamilyTree.Viewer.ucDate();
            this.m_lstRelationships = new System.Windows.Forms.ListBox();
            this.m_cboFactType = new System.Windows.Forms.ComboBox();
            this.m_gridFacts = new System.Windows.Forms.DataGrid();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.m_chkGedcom = new System.Windows.Forms.CheckBox();
            this.m_cboMainImage = new System.Windows.Forms.ComboBox();
            this.m_cboMother = new System.Windows.Forms.ComboBox();
            this.m_cboFather = new System.Windows.Forms.ComboBox();
            this.m_Image = new System.Windows.Forms.PictureBox();
            this.m_labDescription = new System.Windows.Forms.Label();
            this.m_gridToDo = new System.Windows.Forms.DataGrid();
            this.m_grpSources = new System.Windows.Forms.GroupBox();
            this.m_cboSources = new System.Windows.Forms.ComboBox();
            this.m_gridSources = new System.Windows.Forms.DataGrid();
            this.m_TabControl = new System.Windows.Forms.TabControl();
            this.m_cboEditor = new System.Windows.Forms.ComboBox();
            tabBasic = new System.Windows.Forms.TabPage();
            label7 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            tabRelationships = new System.Windows.Forms.TabPage();
            label11 = new System.Windows.Forms.Label();
            label10 = new System.Windows.Forms.Label();
            label9 = new System.Windows.Forms.Label();
            label8 = new System.Windows.Forms.Label();
            cmdDeleteRelationship = new System.Windows.Forms.Button();
            AddRelationship = new System.Windows.Forms.Button();
            oImageList16x16 = new System.Windows.Forms.ImageList(this.components);
            tabFacts = new System.Windows.Forms.TabPage();
            cmdDeleteFact = new System.Windows.Forms.Button();
            menuEditLocation = new System.Windows.Forms.ToolStripMenuItem();
            cmdAddFact = new System.Windows.Forms.Button();
            tabAdvanced = new System.Windows.Forms.TabPage();
            label3 = new System.Windows.Forms.Label();
            label13 = new System.Windows.Forms.Label();
            label12 = new System.Windows.Forms.Label();
            groupBox2 = new System.Windows.Forms.GroupBox();
            tabToDo = new System.Windows.Forms.TabPage();
            cmdDeleteToDo = new System.Windows.Forms.Button();
            cmdAddToDo = new System.Windows.Forms.Button();
            cmdDeleteSource = new System.Windows.Forms.Button();
            cmdAddSource = new System.Windows.Forms.Button();
            cmdCancel = new System.Windows.Forms.Button();
            cmdOK = new System.Windows.Forms.Button();
            cmdRelationshipAddress = new System.Windows.Forms.Button();
            tabBasic.SuspendLayout();
            tabRelationships.SuspendLayout();
            tabFacts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_gridFacts)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            tabAdvanced.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_Image)).BeginInit();
            groupBox2.SuspendLayout();
            tabToDo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_gridToDo)).BeginInit();
            this.m_grpSources.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_gridSources)).BeginInit();
            this.m_TabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabBasic
            // 
            tabBasic.Controls.Add(this.m_dateDoB);
            tabBasic.Controls.Add(this.m_dateDoD);
            tabBasic.Controls.Add(this.m_txtComments);
            tabBasic.Controls.Add(this.m_chkChildrenKnown);
            tabBasic.Controls.Add(label7);
            tabBasic.Controls.Add(label6);
            tabBasic.Controls.Add(label5);
            tabBasic.Controls.Add(label4);
            tabBasic.Controls.Add(this.cboSex_);
            tabBasic.Controls.Add(this.m_labMaidenName);
            tabBasic.Controls.Add(this.m_txtMaidenName);
            tabBasic.Controls.Add(this.m_txtForename);
            tabBasic.Controls.Add(this.txtSurname_);
            tabBasic.Controls.Add(label2);
            tabBasic.Controls.Add(label1);
            tabBasic.ImageIndex = 6;
            tabBasic.Location = new System.Drawing.Point(4, 23);
            tabBasic.Margin = new System.Windows.Forms.Padding(0);
            tabBasic.Name = "tabBasic";
            tabBasic.Size = new System.Drawing.Size(672, 257);
            tabBasic.TabIndex = 0;
            tabBasic.Text = "Basic";
            tabBasic.UseVisualStyleBackColor = true;
            // 
            // m_dateDoB
            // 
            this.m_dateDoB.Location = new System.Drawing.Point(360, 8);
            this.m_dateDoB.Name = "m_dateDoB";
            this.m_dateDoB.Size = new System.Drawing.Size(144, 24);
            this.m_dateDoB.TabIndex = 36;
            clsDate1.Date = new System.DateTime(2007, 10, 11, 0, 0, 0, 0);
            clsDate1.Status = 0;
            this.m_dateDoB.Value = clsDate1;
            this.m_dateDoB.evtValueChanged += new FamilyTree.Viewer.dgtValueChanged(this.dateDoB_evtValueChanged);
            this.m_dateDoB.Enter += new System.EventHandler(this.dateDoB_Enter);
            // 
            // m_dateDoD
            // 
            this.m_dateDoD.Location = new System.Drawing.Point(360, 32);
            this.m_dateDoD.Name = "m_dateDoD";
            this.m_dateDoD.Size = new System.Drawing.Size(144, 24);
            this.m_dateDoD.TabIndex = 35;
            clsDate2.Date = new System.DateTime(2007, 10, 11, 0, 0, 0, 0);
            clsDate2.Status = 0;
            this.m_dateDoD.Value = clsDate2;
            this.m_dateDoD.evtValueChanged += new FamilyTree.Viewer.dgtValueChanged(this.dateDoD_evtValueChanged);
            this.m_dateDoD.Enter += new System.EventHandler(this.dateDoD_Enter);
            // 
            // m_txtComments
            // 
            this.m_txtComments.Location = new System.Drawing.Point(8, 136);
            this.m_txtComments.Multiline = true;
            this.m_txtComments.Name = "m_txtComments";
            this.m_txtComments.Size = new System.Drawing.Size(656, 112);
            this.m_txtComments.TabIndex = 33;
            this.m_txtComments.Text = "textBox1";
            this.m_txtComments.TextChanged += new System.EventHandler(this.txtComments_TextChanged);
            this.m_txtComments.Enter += new System.EventHandler(this.evtNonSpecificSource);
            // 
            // m_chkChildrenKnown
            // 
            this.m_chkChildrenKnown.Location = new System.Drawing.Point(360, 56);
            this.m_chkChildrenKnown.Name = "m_chkChildrenKnown";
            this.m_chkChildrenKnown.Size = new System.Drawing.Size(136, 24);
            this.m_chkChildrenKnown.TabIndex = 32;
            this.m_chkChildrenKnown.Text = "All Children Known";
            this.m_chkChildrenKnown.CheckedChanged += new System.EventHandler(this.chkChildrenKnown_CheckedChanged);
            // 
            // label7
            // 
            label7.Location = new System.Drawing.Point(8, 112);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(104, 23);
            label7.TabIndex = 31;
            label7.Text = "Private Comments:";
            label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            label6.Location = new System.Drawing.Point(248, 32);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(104, 23);
            label6.TabIndex = 28;
            label6.Text = "Date of Death:";
            label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            label5.Location = new System.Drawing.Point(248, 8);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(104, 23);
            label5.TabIndex = 27;
            label5.Text = "Date of Birth:";
            label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            label4.Location = new System.Drawing.Point(8, 80);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(100, 23);
            label4.TabIndex = 26;
            label4.Text = "Sex:";
            label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_cboSex
            // 
            this.cboSex_.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSex_.Items.AddRange(new object[] {
            "Male",
            "Female"});
            this.cboSex_.Location = new System.Drawing.Point(112, 80);
            this.cboSex_.Name = "m_cboSex";
            this.cboSex_.Size = new System.Drawing.Size(128, 21);
            this.cboSex_.TabIndex = 25;
            this.cboSex_.Enter += new System.EventHandler(this.cboSex_Enter);
            this.cboSex_.SelectedValueChanged += new System.EventHandler(this.cboSex_SelectedValueChanged);
            // 
            // m_labMaidenName
            // 
            this.m_labMaidenName.Location = new System.Drawing.Point(8, 56);
            this.m_labMaidenName.Name = "m_labMaidenName";
            this.m_labMaidenName.Size = new System.Drawing.Size(100, 23);
            this.m_labMaidenName.TabIndex = 24;
            this.m_labMaidenName.Text = "Maiden Name:";
            this.m_labMaidenName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_txtMaidenName
            // 
            this.m_txtMaidenName.Location = new System.Drawing.Point(112, 56);
            this.m_txtMaidenName.Name = "m_txtMaidenName";
            this.m_txtMaidenName.Size = new System.Drawing.Size(128, 21);
            this.m_txtMaidenName.TabIndex = 23;
            this.m_txtMaidenName.Text = "txtSurname";
            this.m_txtMaidenName.TextChanged += new System.EventHandler(this.txtMaidenName_TextChanged);
            this.m_txtMaidenName.Enter += new System.EventHandler(this.Name_Enter);
            // 
            // m_txtForename
            // 
            this.m_txtForename.Location = new System.Drawing.Point(112, 32);
            this.m_txtForename.Name = "m_txtForename";
            this.m_txtForename.Size = new System.Drawing.Size(128, 21);
            this.m_txtForename.TabIndex = 22;
            this.m_txtForename.Text = "txtSurname";
            this.m_txtForename.TextChanged += new System.EventHandler(this.txtForename_TextChanged);
            this.m_txtForename.Enter += new System.EventHandler(this.Name_Enter);
            // 
            // m_txtSurname
            // 
            this.txtSurname_.Location = new System.Drawing.Point(112, 8);
            this.txtSurname_.Name = "m_txtSurname";
            this.txtSurname_.Size = new System.Drawing.Size(128, 21);
            this.txtSurname_.TabIndex = 20;
            this.txtSurname_.Text = "txtSurname";
            this.txtSurname_.TextChanged += new System.EventHandler(this.txtSurname_TextChanged);
            this.txtSurname_.Enter += new System.EventHandler(this.Name_Enter);
            // 
            // label2
            // 
            label2.Location = new System.Drawing.Point(8, 32);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(100, 23);
            label2.TabIndex = 21;
            label2.Text = "Forenames:";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            label1.Location = new System.Drawing.Point(8, 8);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(100, 23);
            label1.TabIndex = 19;
            label1.Text = "Surname:";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabRelationships
            // 
            tabRelationships.Controls.Add(cmdRelationshipAddress);
            tabRelationships.Controls.Add(this.m_cboRelationshipType);
            tabRelationships.Controls.Add(label11);
            tabRelationships.Controls.Add(this.m_cboTerminated);
            tabRelationships.Controls.Add(this.m_txtRelationComments);
            tabRelationships.Controls.Add(label10);
            tabRelationships.Controls.Add(this.m_txtRelationLocation);
            tabRelationships.Controls.Add(label9);
            tabRelationships.Controls.Add(label8);
            tabRelationships.Controls.Add(this.m_cboAddPartner);
            tabRelationships.Controls.Add(cmdDeleteRelationship);
            tabRelationships.Controls.Add(AddRelationship);
            tabRelationships.Controls.Add(this.m_dateRelationEnd);
            tabRelationships.Controls.Add(this.m_dateRelationStart);
            tabRelationships.Controls.Add(this.m_lstRelationships);
            tabRelationships.ImageIndex = 5;
            tabRelationships.Location = new System.Drawing.Point(4, 23);
            tabRelationships.Name = "tabRelationships";
            tabRelationships.Size = new System.Drawing.Size(672, 257);
            tabRelationships.TabIndex = 2;
            tabRelationships.Text = "Relationships";
            tabRelationships.UseVisualStyleBackColor = true;
            // 
            // m_cboRelationshipType
            // 
            this.m_cboRelationshipType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cboRelationshipType.Items.AddRange(new object[] {
            "Regilious Marriage",
            "Civil Marriage",
            "Partners"});
            this.m_cboRelationshipType.Location = new System.Drawing.Point(472, 16);
            this.m_cboRelationshipType.Name = "m_cboRelationshipType";
            this.m_cboRelationshipType.Size = new System.Drawing.Size(184, 21);
            this.m_cboRelationshipType.TabIndex = 16;
            this.m_cboRelationshipType.SelectedIndexChanged += new System.EventHandler(this.cboRelationshipType_SelectedIndexChanged);
            this.m_cboRelationshipType.Enter += new System.EventHandler(this.m_cboRelationshipType_Enter);
            // 
            // label11
            // 
            label11.Location = new System.Drawing.Point(224, 64);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(104, 23);
            label11.TabIndex = 13;
            label11.Text = "Terminated:";
            label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_cboTerminated
            // 
            this.m_cboTerminated.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cboTerminated.Items.AddRange(new object[] {
            "No",
            "Divorce",
            "He died",
            "She died"});
            this.m_cboTerminated.Location = new System.Drawing.Point(328, 64);
            this.m_cboTerminated.Name = "m_cboTerminated";
            this.m_cboTerminated.Size = new System.Drawing.Size(184, 21);
            this.m_cboTerminated.TabIndex = 12;
            this.m_cboTerminated.SelectedIndexChanged += new System.EventHandler(this.cboTerminated_SelectedIndexChanged);
            this.m_cboTerminated.Enter += new System.EventHandler(this.cboTerminated_Enter);
            // 
            // m_txtRelationComments
            // 
            this.m_txtRelationComments.Location = new System.Drawing.Point(328, 88);
            this.m_txtRelationComments.Multiline = true;
            this.m_txtRelationComments.Name = "m_txtRelationComments";
            this.m_txtRelationComments.Size = new System.Drawing.Size(328, 96);
            this.m_txtRelationComments.TabIndex = 10;
            this.m_txtRelationComments.Text = "textBox1";
            this.m_txtRelationComments.TextChanged += new System.EventHandler(this.txtRelationComments_TextChanged);
            // 
            // label10
            // 
            label10.Location = new System.Drawing.Point(232, 88);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(96, 24);
            label10.TabIndex = 8;
            label10.Text = "Private Comments";
            label10.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // m_txtRelationLocation
            // 
            this.m_txtRelationLocation.Location = new System.Drawing.Point(328, 40);
            this.m_txtRelationLocation.Name = "m_txtRelationLocation";
            this.m_txtRelationLocation.Size = new System.Drawing.Size(306, 21);
            this.m_txtRelationLocation.TabIndex = 7;
            this.m_txtRelationLocation.Text = "textBox1";
            this.m_txtRelationLocation.TextChanged += new System.EventHandler(this.txtRelationLocation_TextChanged);
            this.m_txtRelationLocation.Enter += new System.EventHandler(this.txtRelationLocation_Enter);
            // 
            // label9
            // 
            label9.Location = new System.Drawing.Point(224, 40);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(104, 23);
            label9.TabIndex = 6;
            label9.Text = "Location:";
            label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            label8.Location = new System.Drawing.Point(224, 16);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(104, 23);
            label8.TabIndex = 3;
            label8.Text = "Start Date:";
            label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_cboAddPartner
            // 
            this.m_cboAddPartner.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cboAddPartner.Location = new System.Drawing.Point(8, 136);
            this.m_cboAddPartner.Name = "m_cboAddPartner";
            this.m_cboAddPartner.Size = new System.Drawing.Size(206, 21);
            this.m_cboAddPartner.TabIndex = 1;
            // 
            // cmdDeleteRelationship
            // 
            cmdDeleteRelationship.Image = global::FamilyTree.Viewer.Properties.Resources.delete;
            cmdDeleteRelationship.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            cmdDeleteRelationship.Location = new System.Drawing.Point(114, 160);
            cmdDeleteRelationship.Name = "cmdDeleteRelationship";
            cmdDeleteRelationship.Size = new System.Drawing.Size(100, 30);
            cmdDeleteRelationship.TabIndex = 11;
            cmdDeleteRelationship.Text = "Delete";
            cmdDeleteRelationship.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            cmdDeleteRelationship.Click += new System.EventHandler(this.cmdDeleteRelationship_Click);
            // 
            // AddRelationship
            // 
            AddRelationship.Image = global::FamilyTree.Viewer.Properties.Resources.add;
            AddRelationship.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            AddRelationship.Location = new System.Drawing.Point(8, 160);
            AddRelationship.Name = "AddRelationship";
            AddRelationship.Size = new System.Drawing.Size(100, 30);
            AddRelationship.TabIndex = 2;
            AddRelationship.Text = "Add";
            AddRelationship.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            AddRelationship.Click += new System.EventHandler(this.AddRelationship_Click);
            // 
            // m_dateRelationEnd
            // 
            this.m_dateRelationEnd.Location = new System.Drawing.Point(512, 64);
            this.m_dateRelationEnd.Name = "m_dateRelationEnd";
            this.m_dateRelationEnd.Size = new System.Drawing.Size(144, 24);
            this.m_dateRelationEnd.TabIndex = 15;
            clsDate3.Date = new System.DateTime(2007, 10, 11, 0, 0, 0, 0);
            clsDate3.Status = 0;
            this.m_dateRelationEnd.Value = clsDate3;
            this.m_dateRelationEnd.evtValueChanged += new FamilyTree.Viewer.dgtValueChanged(this.dateRelationEnd_evtValueChanged);
            // 
            // m_dateRelationStart
            // 
            this.m_dateRelationStart.Location = new System.Drawing.Point(328, 16);
            this.m_dateRelationStart.Name = "m_dateRelationStart";
            this.m_dateRelationStart.Size = new System.Drawing.Size(144, 24);
            this.m_dateRelationStart.TabIndex = 14;
            clsDate4.Date = new System.DateTime(2007, 10, 11, 0, 0, 0, 0);
            clsDate4.Status = 0;
            this.m_dateRelationStart.Value = clsDate4;
            this.m_dateRelationStart.evtValueChanged += new FamilyTree.Viewer.dgtValueChanged(this.dateRelationStart_evtValueChanged);
            this.m_dateRelationStart.Enter += new System.EventHandler(this.dateRelationStart_Enter);
            // 
            // m_lstRelationships
            // 
            this.m_lstRelationships.Location = new System.Drawing.Point(8, 8);
            this.m_lstRelationships.Name = "m_lstRelationships";
            this.m_lstRelationships.Size = new System.Drawing.Size(206, 121);
            this.m_lstRelationships.TabIndex = 0;
            this.m_lstRelationships.SelectedIndexChanged += new System.EventHandler(this.lstRelationships_SelectedIndexChanged);
            this.m_lstRelationships.Enter += new System.EventHandler(this.lstRelationships_Enter);
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
            oImageList16x16.Images.SetKeyName(9, "Picture.bmp");
            // 
            // tabFacts
            // 
            tabFacts.Controls.Add(cmdDeleteFact);
            tabFacts.Controls.Add(this.m_cboFactType);
            tabFacts.Controls.Add(this.m_gridFacts);
            tabFacts.Controls.Add(cmdAddFact);
            tabFacts.ImageIndex = 7;
            tabFacts.Location = new System.Drawing.Point(4, 23);
            tabFacts.Name = "tabFacts";
            tabFacts.Size = new System.Drawing.Size(672, 257);
            tabFacts.TabIndex = 1;
            tabFacts.Text = "Facts";
            tabFacts.UseVisualStyleBackColor = true;
            // 
            // cmdDeleteFact
            // 
            cmdDeleteFact.Image = global::FamilyTree.Viewer.Properties.Resources.delete;
            cmdDeleteFact.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            cmdDeleteFact.Location = new System.Drawing.Point(394, 222);
            cmdDeleteFact.Name = "cmdDeleteFact";
            cmdDeleteFact.Size = new System.Drawing.Size(100, 30);
            cmdDeleteFact.TabIndex = 3;
            cmdDeleteFact.Text = "Delete";
            cmdDeleteFact.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            cmdDeleteFact.Click += new System.EventHandler(this.cmdDeleteFact_Click);
            // 
            // m_cboFactType
            // 
            this.m_cboFactType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cboFactType.Location = new System.Drawing.Point(8, 228);
            this.m_cboFactType.Name = "m_cboFactType";
            this.m_cboFactType.Size = new System.Drawing.Size(272, 21);
            this.m_cboFactType.TabIndex = 2;
            // 
            // m_gridFacts
            // 
            this.m_gridFacts.CaptionVisible = false;
            this.m_gridFacts.ContextMenuStrip = this.contextMenuStrip1;
            this.m_gridFacts.DataMember = "";
            this.m_gridFacts.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.m_gridFacts.Location = new System.Drawing.Point(8, 8);
            this.m_gridFacts.Name = "m_gridFacts";
            this.m_gridFacts.Size = new System.Drawing.Size(656, 208);
            this.m_gridFacts.TabIndex = 0;
            this.m_gridFacts.Enter += new System.EventHandler(this.gridFacts_CurrentCellChanged);
            this.m_gridFacts.MouseUp += new System.Windows.Forms.MouseEventHandler(this.m_gridFacts_MouseUp);
            this.m_gridFacts.CurrentCellChanged += new System.EventHandler(this.gridFacts_CurrentCellChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            menuEditLocation});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(145, 26);
            // 
            // menuEditLocation
            // 
            menuEditLocation.Name = "menuEditLocation";
            menuEditLocation.Size = new System.Drawing.Size(144, 22);
            menuEditLocation.Text = "Location...";
            menuEditLocation.Click += new System.EventHandler(this.menuEditLocation_Click);
            // 
            // cmdAddFact
            // 
            cmdAddFact.Image = global::FamilyTree.Viewer.Properties.Resources.add;
            cmdAddFact.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            cmdAddFact.Location = new System.Drawing.Point(288, 222);
            cmdAddFact.Name = "cmdAddFact";
            cmdAddFact.Size = new System.Drawing.Size(100, 30);
            cmdAddFact.TabIndex = 1;
            cmdAddFact.Text = "Add";
            cmdAddFact.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            cmdAddFact.Click += new System.EventHandler(this.cmdAddFact_Click);
            // 
            // tabAdvanced
            // 
            tabAdvanced.BackColor = System.Drawing.Color.Transparent;
            tabAdvanced.Controls.Add(this.m_chkGedcom);
            tabAdvanced.Controls.Add(label3);
            tabAdvanced.Controls.Add(this.m_cboMainImage);
            tabAdvanced.Controls.Add(this.m_cboMother);
            tabAdvanced.Controls.Add(this.m_cboFather);
            tabAdvanced.Controls.Add(label13);
            tabAdvanced.Controls.Add(label12);
            tabAdvanced.Controls.Add(this.m_Image);
            tabAdvanced.ImageIndex = 9;
            tabAdvanced.Location = new System.Drawing.Point(4, 23);
            tabAdvanced.Name = "tabAdvanced";
            tabAdvanced.Size = new System.Drawing.Size(672, 257);
            tabAdvanced.TabIndex = 3;
            tabAdvanced.Text = "Advanced";
            tabAdvanced.UseVisualStyleBackColor = true;
            // 
            // m_chkGedcom
            // 
            this.m_chkGedcom.AutoSize = true;
            this.m_chkGedcom.Location = new System.Drawing.Point(112, 137);
            this.m_chkGedcom.Name = "m_chkGedcom";
            this.m_chkGedcom.Size = new System.Drawing.Size(130, 17);
            this.m_chkGedcom.TabIndex = 13;
            this.m_chkGedcom.Text = "Include in Gedcom file";
            this.m_chkGedcom.UseVisualStyleBackColor = true;
            this.m_chkGedcom.CheckedChanged += new System.EventHandler(this.chkGedcom_CheckedChanged);
            // 
            // label3
            // 
            label3.Location = new System.Drawing.Point(8, 86);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(100, 23);
            label3.TabIndex = 12;
            label3.Text = "Main Image";
            label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_cboMainImage
            // 
            this.m_cboMainImage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cboMainImage.Location = new System.Drawing.Point(112, 88);
            this.m_cboMainImage.Name = "m_cboMainImage";
            this.m_cboMainImage.Size = new System.Drawing.Size(272, 21);
            this.m_cboMainImage.TabIndex = 11;
            this.m_cboMainImage.SelectedIndexChanged += new System.EventHandler(this.cboMainImage_SelectedIndexChanged);
            // 
            // m_cboMother
            // 
            this.m_cboMother.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cboMother.Location = new System.Drawing.Point(112, 32);
            this.m_cboMother.Name = "m_cboMother";
            this.m_cboMother.Size = new System.Drawing.Size(272, 21);
            this.m_cboMother.TabIndex = 3;
            this.m_cboMother.SelectedIndexChanged += new System.EventHandler(this.cboMother_SelectedIndexChanged);
            // 
            // m_cboFather
            // 
            this.m_cboFather.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cboFather.Location = new System.Drawing.Point(112, 8);
            this.m_cboFather.Name = "m_cboFather";
            this.m_cboFather.Size = new System.Drawing.Size(272, 21);
            this.m_cboFather.TabIndex = 2;
            this.m_cboFather.SelectedIndexChanged += new System.EventHandler(this.cboFather_SelectedIndexChanged);
            // 
            // label13
            // 
            label13.Location = new System.Drawing.Point(8, 32);
            label13.Name = "label13";
            label13.Size = new System.Drawing.Size(100, 23);
            label13.TabIndex = 1;
            label13.Text = "Mother";
            label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            label12.Location = new System.Drawing.Point(8, 8);
            label12.Name = "label12";
            label12.Size = new System.Drawing.Size(100, 23);
            label12.TabIndex = 0;
            label12.Text = "Father";
            label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_Image
            // 
            this.m_Image.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_Image.Location = new System.Drawing.Point(426, 8);
            this.m_Image.Name = "m_Image";
            this.m_Image.Size = new System.Drawing.Size(240, 240);
            this.m_Image.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.m_Image.TabIndex = 10;
            this.m_Image.TabStop = false;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(this.m_labDescription);
            groupBox2.Location = new System.Drawing.Point(8, 296);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(680, 104);
            groupBox2.TabIndex = 20;
            groupBox2.TabStop = false;
            groupBox2.Text = "Description";
            // 
            // m_labDescription
            // 
            this.m_labDescription.Location = new System.Drawing.Point(8, 24);
            this.m_labDescription.Name = "m_labDescription";
            this.m_labDescription.Size = new System.Drawing.Size(664, 72);
            this.m_labDescription.TabIndex = 0;
            this.m_labDescription.Text = "label7";
            // 
            // tabToDo
            // 
            tabToDo.Controls.Add(cmdDeleteToDo);
            tabToDo.Controls.Add(cmdAddToDo);
            tabToDo.Controls.Add(this.m_gridToDo);
            tabToDo.ImageIndex = 0;
            tabToDo.Location = new System.Drawing.Point(4, 23);
            tabToDo.Name = "tabToDo";
            tabToDo.Size = new System.Drawing.Size(672, 257);
            tabToDo.TabIndex = 4;
            tabToDo.Text = "To Do";
            tabToDo.UseVisualStyleBackColor = true;
            // 
            // cmdDeleteToDo
            // 
            cmdDeleteToDo.Image = global::FamilyTree.Viewer.Properties.Resources.delete;
            cmdDeleteToDo.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            cmdDeleteToDo.Location = new System.Drawing.Point(564, 222);
            cmdDeleteToDo.Name = "cmdDeleteToDo";
            cmdDeleteToDo.Size = new System.Drawing.Size(100, 30);
            cmdDeleteToDo.TabIndex = 5;
            cmdDeleteToDo.Text = "Delete";
            cmdDeleteToDo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            cmdDeleteToDo.Click += new System.EventHandler(this.cmdDeleteToDo_Click);
            // 
            // cmdAddToDo
            // 
            cmdAddToDo.Image = global::FamilyTree.Viewer.Properties.Resources.add;
            cmdAddToDo.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            cmdAddToDo.Location = new System.Drawing.Point(458, 222);
            cmdAddToDo.Name = "cmdAddToDo";
            cmdAddToDo.Size = new System.Drawing.Size(100, 30);
            cmdAddToDo.TabIndex = 4;
            cmdAddToDo.Text = "Add";
            cmdAddToDo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            cmdAddToDo.Click += new System.EventHandler(this.cmdAddToDo_Click);
            // 
            // m_gridToDo
            // 
            this.m_gridToDo.CaptionVisible = false;
            this.m_gridToDo.DataMember = "";
            this.m_gridToDo.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.m_gridToDo.Location = new System.Drawing.Point(8, 8);
            this.m_gridToDo.Name = "m_gridToDo";
            this.m_gridToDo.Size = new System.Drawing.Size(656, 208);
            this.m_gridToDo.TabIndex = 1;
            this.m_gridToDo.Enter += new System.EventHandler(this.evtNonSpecificSource);
            // 
            // cmdDeleteSource
            // 
            cmdDeleteSource.Image = global::FamilyTree.Viewer.Properties.Resources.delete;
            cmdDeleteSource.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            cmdDeleteSource.Location = new System.Drawing.Point(490, 180);
            cmdDeleteSource.Name = "cmdDeleteSource";
            cmdDeleteSource.Size = new System.Drawing.Size(100, 30);
            cmdDeleteSource.TabIndex = 14;
            cmdDeleteSource.Text = "Delete";
            cmdDeleteSource.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            cmdDeleteSource.Click += new System.EventHandler(this.cmdDeleteSource_Click);
            // 
            // cmdAddSource
            // 
            cmdAddSource.Image = global::FamilyTree.Viewer.Properties.Resources.add;
            cmdAddSource.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            cmdAddSource.Location = new System.Drawing.Point(384, 180);
            cmdAddSource.Name = "cmdAddSource";
            cmdAddSource.Size = new System.Drawing.Size(100, 30);
            cmdAddSource.TabIndex = 12;
            cmdAddSource.Text = "Add";
            cmdAddSource.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            cmdAddSource.Click += new System.EventHandler(this.cmdAddSource_Click);
            // 
            // cmdCancel
            // 
            cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cmdCancel.Image = global::FamilyTree.Viewer.Properties.Resources.Cancel;
            cmdCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            cmdCancel.Location = new System.Drawing.Point(482, 627);
            cmdCancel.Name = "cmdCancel";
            cmdCancel.Size = new System.Drawing.Size(100, 30);
            cmdCancel.TabIndex = 1;
            cmdCancel.Text = "Cancel";
            cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmdOK
            // 
            cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            cmdOK.Image = global::FamilyTree.Viewer.Properties.Resources.OK;
            cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            cmdOK.Location = new System.Drawing.Point(588, 627);
            cmdOK.Name = "cmdOK";
            cmdOK.Size = new System.Drawing.Size(100, 30);
            cmdOK.TabIndex = 0;
            cmdOK.Text = "OK";
            cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // m_grpSources
            // 
            this.m_grpSources.Controls.Add(cmdDeleteSource);
            this.m_grpSources.Controls.Add(this.m_cboSources);
            this.m_grpSources.Controls.Add(cmdAddSource);
            this.m_grpSources.Controls.Add(this.m_gridSources);
            this.m_grpSources.Location = new System.Drawing.Point(8, 400);
            this.m_grpSources.Name = "m_grpSources";
            this.m_grpSources.Size = new System.Drawing.Size(680, 221);
            this.m_grpSources.TabIndex = 19;
            this.m_grpSources.TabStop = false;
            this.m_grpSources.Text = "Sources";
            // 
            // m_cboSources
            // 
            this.m_cboSources.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cboSources.Location = new System.Drawing.Point(8, 184);
            this.m_cboSources.Name = "m_cboSources";
            this.m_cboSources.Size = new System.Drawing.Size(368, 21);
            this.m_cboSources.TabIndex = 13;
            // 
            // m_gridSources
            // 
            this.m_gridSources.CaptionText = "Sources for me";
            this.m_gridSources.CaptionVisible = false;
            this.m_gridSources.DataMember = "";
            this.m_gridSources.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.m_gridSources.Location = new System.Drawing.Point(8, 24);
            this.m_gridSources.Name = "m_gridSources";
            this.m_gridSources.Size = new System.Drawing.Size(664, 152);
            this.m_gridSources.TabIndex = 11;
            this.m_gridSources.Leave += new System.EventHandler(this.gridSources_Leave);
            // 
            // m_TabControl
            // 
            this.m_TabControl.Controls.Add(tabBasic);
            this.m_TabControl.Controls.Add(tabRelationships);
            this.m_TabControl.Controls.Add(tabFacts);
            this.m_TabControl.Controls.Add(tabAdvanced);
            this.m_TabControl.Controls.Add(tabToDo);
            this.m_TabControl.ImageList = oImageList16x16;
            this.m_TabControl.Location = new System.Drawing.Point(8, 8);
            this.m_TabControl.Margin = new System.Windows.Forms.Padding(0);
            this.m_TabControl.Name = "m_TabControl";
            this.m_TabControl.SelectedIndex = 0;
            this.m_TabControl.Size = new System.Drawing.Size(680, 284);
            this.m_TabControl.TabIndex = 23;
            this.m_TabControl.Enter += new System.EventHandler(this.evtNonSpecificSource);
            this.m_TabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // m_cboEditor
            // 
            this.m_cboEditor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cboEditor.FormattingEnabled = true;
            this.m_cboEditor.Location = new System.Drawing.Point(12, 627);
            this.m_cboEditor.Name = "m_cboEditor";
            this.m_cboEditor.Size = new System.Drawing.Size(179, 21);
            this.m_cboEditor.TabIndex = 24;
            // 
            // cmdRelationshipAddress
            // 
            cmdRelationshipAddress.Image = global::FamilyTree.Viewer.Properties.Resources.Earth;
            cmdRelationshipAddress.Location = new System.Drawing.Point(635, 40);
            cmdRelationshipAddress.Name = "cmdRelationshipAddress";
            cmdRelationshipAddress.Size = new System.Drawing.Size(21, 21);
            cmdRelationshipAddress.TabIndex = 17;
            cmdRelationshipAddress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            cmdRelationshipAddress.Click += new System.EventHandler(this.cmdRelationshipAddress_Click);
            // 
            // frmEditPerson
            // 
            this.AllowDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(690, 664);
            this.Controls.Add(this.m_cboEditor);
            this.Controls.Add(this.m_TabControl);
            this.Controls.Add(groupBox2);
            this.Controls.Add(this.m_grpSources);
            this.Controls.Add(cmdCancel);
            this.Controls.Add(cmdOK);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEditPerson";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmEditPerson";
            this.Load += new System.EventHandler(this.frmEditPerson_Load);
            this.Shown += new System.EventHandler(this.frmEditPerson_Shown);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.frmEditPerson_DragEnter);
            tabBasic.ResumeLayout(false);
            tabBasic.PerformLayout();
            tabRelationships.ResumeLayout(false);
            tabRelationships.PerformLayout();
            tabFacts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_gridFacts)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            tabAdvanced.ResumeLayout(false);
            tabAdvanced.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_Image)).EndInit();
            groupBox2.ResumeLayout(false);
            tabToDo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_gridToDo)).EndInit();
            this.m_grpSources.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_gridSources)).EndInit();
            this.m_TabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cboSex_;
        private System.Windows.Forms.Label m_labMaidenName;
        private System.Windows.Forms.TextBox m_txtMaidenName;
        private System.Windows.Forms.TextBox m_txtForename;
        private System.Windows.Forms.TextBox txtSurname_;
        private System.Windows.Forms.TabControl m_TabControl;
        private System.Windows.Forms.ComboBox m_cboFactType;
        private System.Windows.Forms.CheckBox m_chkChildrenKnown;
        private System.Windows.Forms.ListBox m_lstRelationships;
        private System.Windows.Forms.ComboBox m_cboAddPartner;
        private System.Windows.Forms.TextBox m_txtRelationComments;
        private System.Windows.Forms.TextBox m_txtComments;
        private System.Windows.Forms.ComboBox m_cboTerminated;
        private System.Windows.Forms.ComboBox m_cboFather;
        private System.Windows.Forms.ComboBox m_cboMother;
        private System.Windows.Forms.PictureBox m_Image;
        private System.Windows.Forms.DataGrid m_gridFacts;
        private System.Windows.Forms.DataGrid m_gridSources;
        private System.Windows.Forms.ComboBox m_cboSources;
        private FamilyTree.Viewer.ucDate m_dateDoD;
        private FamilyTree.Viewer.ucDate m_dateDoB;
        private FamilyTree.Viewer.ucDate m_dateRelationStart;
        private FamilyTree.Viewer.ucDate m_dateRelationEnd;
        private System.Windows.Forms.ComboBox m_cboRelationshipType;
        private Label m_labDescription;
        private GroupBox m_grpSources;
        private TextBox m_txtRelationLocation;
        private ComboBox m_cboMainImage;
        private DataGrid m_gridToDo;
        private ComboBox m_cboEditor;
        private CheckBox m_chkGedcom;
        private ContextMenuStrip contextMenuStrip1;

    }
}
