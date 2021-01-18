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
            FamilyTree.Objects.CompoundDate compoundDate1 = new FamilyTree.Objects.CompoundDate();
            FamilyTree.Objects.CompoundDate compoundDate2 = new FamilyTree.Objects.CompoundDate();
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
            FamilyTree.Objects.CompoundDate compoundDate3 = new FamilyTree.Objects.CompoundDate();
            FamilyTree.Objects.CompoundDate compoundDate4 = new FamilyTree.Objects.CompoundDate();
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
            this.dateDoB_ = new FamilyTree.Viewer.CompoundDateEditBox();
            this.dateDoD_ = new FamilyTree.Viewer.CompoundDateEditBox();
            this.txtComments_ = new System.Windows.Forms.TextBox();
            this.chkChildrenKnown_ = new System.Windows.Forms.CheckBox();
            this.cboSex_ = new System.Windows.Forms.ComboBox();
            this.labMaidenName_ = new System.Windows.Forms.Label();
            this.txtMaidenName_ = new System.Windows.Forms.TextBox();
            this.txtForename_ = new System.Windows.Forms.TextBox();
            this.txtSurname_ = new System.Windows.Forms.TextBox();
            this.m_cboRelationshipType = new System.Windows.Forms.ComboBox();
            this.m_cboTerminated = new System.Windows.Forms.ComboBox();
            this.m_txtRelationComments = new System.Windows.Forms.TextBox();
            this.txtRelationLocation_ = new System.Windows.Forms.TextBox();
            this.cboAddPartner_ = new System.Windows.Forms.ComboBox();
            this.dateRelationEnd_ = new FamilyTree.Viewer.CompoundDateEditBox();
            this.dateRelationStart_ = new FamilyTree.Viewer.CompoundDateEditBox();
            this.lstRelationships_ = new System.Windows.Forms.ListBox();
            this.cboFactType_ = new System.Windows.Forms.ComboBox();
            this.gridFacts_ = new System.Windows.Forms.DataGrid();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.chkGedcom_ = new System.Windows.Forms.CheckBox();
            this.cboMainImage_ = new System.Windows.Forms.ComboBox();
            this.cboMother_ = new System.Windows.Forms.ComboBox();
            this.cboFather_ = new System.Windows.Forms.ComboBox();
            this.m_Image = new System.Windows.Forms.PictureBox();
            this.labDescription_ = new System.Windows.Forms.Label();
            this.gridToDo_ = new System.Windows.Forms.DataGrid();
            this.grpSources_ = new System.Windows.Forms.GroupBox();
            this.cboSources_ = new System.Windows.Forms.ComboBox();
            this.gridSources_ = new System.Windows.Forms.DataGrid();
            this.tabControl_ = new System.Windows.Forms.TabControl();
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
            ((System.ComponentModel.ISupportInitialize)(this.gridFacts_)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            tabAdvanced.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_Image)).BeginInit();
            groupBox2.SuspendLayout();
            tabToDo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridToDo_)).BeginInit();
            this.grpSources_.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridSources_)).BeginInit();
            this.tabControl_.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabBasic
            // 
            tabBasic.Controls.Add(this.dateDoB_);
            tabBasic.Controls.Add(this.dateDoD_);
            tabBasic.Controls.Add(this.txtComments_);
            tabBasic.Controls.Add(this.chkChildrenKnown_);
            tabBasic.Controls.Add(label7);
            tabBasic.Controls.Add(label6);
            tabBasic.Controls.Add(label5);
            tabBasic.Controls.Add(label4);
            tabBasic.Controls.Add(this.cboSex_);
            tabBasic.Controls.Add(this.labMaidenName_);
            tabBasic.Controls.Add(this.txtMaidenName_);
            tabBasic.Controls.Add(this.txtForename_);
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
            this.dateDoB_.Location = new System.Drawing.Point(360, 8);
            this.dateDoB_.Name = "m_dateDoB";
            this.dateDoB_.Size = new System.Drawing.Size(144, 24);
            this.dateDoB_.TabIndex = 36;
            compoundDate1.date = new System.DateTime(2007, 10, 11, 0, 0, 0, 0);
            compoundDate1.status = 0;
            this.dateDoB_.theDate = compoundDate1;
            this.dateDoB_.eventValueChanged += new FamilyTree.Viewer.FuncValueChanged(this.dateDoB_evtValueChanged);
            this.dateDoB_.Enter += new System.EventHandler(this.dateDoB_Enter);
            // 
            // m_dateDoD
            // 
            this.dateDoD_.Location = new System.Drawing.Point(360, 32);
            this.dateDoD_.Name = "m_dateDoD";
            this.dateDoD_.Size = new System.Drawing.Size(144, 24);
            this.dateDoD_.TabIndex = 35;
            compoundDate2.date = new System.DateTime(2007, 10, 11, 0, 0, 0, 0);
            compoundDate2.status = 0;
            this.dateDoD_.theDate = compoundDate2;
            this.dateDoD_.eventValueChanged += new FamilyTree.Viewer.FuncValueChanged(this.dateDoD_evtValueChanged);
            this.dateDoD_.Enter += new System.EventHandler(this.dateDoD_Enter);
            // 
            // m_txtComments
            // 
            this.txtComments_.Location = new System.Drawing.Point(8, 136);
            this.txtComments_.Multiline = true;
            this.txtComments_.Name = "m_txtComments";
            this.txtComments_.Size = new System.Drawing.Size(656, 112);
            this.txtComments_.TabIndex = 33;
            this.txtComments_.Text = "textBox1";
            this.txtComments_.TextChanged += new System.EventHandler(this.txtComments_TextChanged);
            this.txtComments_.Enter += new System.EventHandler(this.evtNonSpecificSource);
            // 
            // m_chkChildrenKnown
            // 
            this.chkChildrenKnown_.Location = new System.Drawing.Point(360, 56);
            this.chkChildrenKnown_.Name = "m_chkChildrenKnown";
            this.chkChildrenKnown_.Size = new System.Drawing.Size(136, 24);
            this.chkChildrenKnown_.TabIndex = 32;
            this.chkChildrenKnown_.Text = "All Children Known";
            this.chkChildrenKnown_.CheckedChanged += new System.EventHandler(this.chkChildrenKnown_CheckedChanged);
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
            this.labMaidenName_.Location = new System.Drawing.Point(8, 56);
            this.labMaidenName_.Name = "m_labMaidenName";
            this.labMaidenName_.Size = new System.Drawing.Size(100, 23);
            this.labMaidenName_.TabIndex = 24;
            this.labMaidenName_.Text = "Maiden Name:";
            this.labMaidenName_.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_txtMaidenName
            // 
            this.txtMaidenName_.Location = new System.Drawing.Point(112, 56);
            this.txtMaidenName_.Name = "m_txtMaidenName";
            this.txtMaidenName_.Size = new System.Drawing.Size(128, 21);
            this.txtMaidenName_.TabIndex = 23;
            this.txtMaidenName_.Text = "txtSurname";
            this.txtMaidenName_.TextChanged += new System.EventHandler(this.txtMaidenName_TextChanged);
            this.txtMaidenName_.Enter += new System.EventHandler(this.txtName_Enter);
            // 
            // m_txtForename
            // 
            this.txtForename_.Location = new System.Drawing.Point(112, 32);
            this.txtForename_.Name = "m_txtForename";
            this.txtForename_.Size = new System.Drawing.Size(128, 21);
            this.txtForename_.TabIndex = 22;
            this.txtForename_.Text = "txtSurname";
            this.txtForename_.TextChanged += new System.EventHandler(this.txtForename_TextChanged);
            this.txtForename_.Enter += new System.EventHandler(this.txtName_Enter);
            // 
            // m_txtSurname
            // 
            this.txtSurname_.Location = new System.Drawing.Point(112, 8);
            this.txtSurname_.Name = "m_txtSurname";
            this.txtSurname_.Size = new System.Drawing.Size(128, 21);
            this.txtSurname_.TabIndex = 20;
            this.txtSurname_.Text = "txtSurname";
            this.txtSurname_.TextChanged += new System.EventHandler(this.txtSurname_TextChanged);
            this.txtSurname_.Enter += new System.EventHandler(this.txtName_Enter);
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
            tabRelationships.Controls.Add(this.txtRelationLocation_);
            tabRelationships.Controls.Add(label9);
            tabRelationships.Controls.Add(label8);
            tabRelationships.Controls.Add(this.cboAddPartner_);
            tabRelationships.Controls.Add(cmdDeleteRelationship);
            tabRelationships.Controls.Add(AddRelationship);
            tabRelationships.Controls.Add(this.dateRelationEnd_);
            tabRelationships.Controls.Add(this.dateRelationStart_);
            tabRelationships.Controls.Add(this.lstRelationships_);
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
            this.txtRelationLocation_.Location = new System.Drawing.Point(328, 40);
            this.txtRelationLocation_.Name = "m_txtRelationLocation";
            this.txtRelationLocation_.Size = new System.Drawing.Size(306, 21);
            this.txtRelationLocation_.TabIndex = 7;
            this.txtRelationLocation_.Text = "textBox1";
            this.txtRelationLocation_.TextChanged += new System.EventHandler(this.txtRelationLocation_TextChanged);
            this.txtRelationLocation_.Enter += new System.EventHandler(this.txtRelationLocation_Enter);
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
            this.cboAddPartner_.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAddPartner_.Location = new System.Drawing.Point(8, 136);
            this.cboAddPartner_.Name = "m_cboAddPartner";
            this.cboAddPartner_.Size = new System.Drawing.Size(206, 21);
            this.cboAddPartner_.TabIndex = 1;
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
            this.dateRelationEnd_.Location = new System.Drawing.Point(512, 64);
            this.dateRelationEnd_.Name = "m_dateRelationEnd";
            this.dateRelationEnd_.Size = new System.Drawing.Size(144, 24);
            this.dateRelationEnd_.TabIndex = 15;
            compoundDate3.date = new System.DateTime(2007, 10, 11, 0, 0, 0, 0);
            compoundDate3.status = 0;
            this.dateRelationEnd_.theDate = compoundDate3;
            this.dateRelationEnd_.eventValueChanged += new FamilyTree.Viewer.FuncValueChanged(this.dateRelationEnd_evtValueChanged);
            // 
            // m_dateRelationStart
            // 
            this.dateRelationStart_.Location = new System.Drawing.Point(328, 16);
            this.dateRelationStart_.Name = "m_dateRelationStart";
            this.dateRelationStart_.Size = new System.Drawing.Size(144, 24);
            this.dateRelationStart_.TabIndex = 14;
            compoundDate4.date = new System.DateTime(2007, 10, 11, 0, 0, 0, 0);
            compoundDate4.status = 0;
            this.dateRelationStart_.theDate = compoundDate4;
            this.dateRelationStart_.eventValueChanged += new FamilyTree.Viewer.FuncValueChanged(this.dateRelationStart_evtValueChanged);
            this.dateRelationStart_.Enter += new System.EventHandler(this.dateRelationStart_Enter);
            // 
            // m_lstRelationships
            // 
            this.lstRelationships_.Location = new System.Drawing.Point(8, 8);
            this.lstRelationships_.Name = "m_lstRelationships";
            this.lstRelationships_.Size = new System.Drawing.Size(206, 121);
            this.lstRelationships_.TabIndex = 0;
            this.lstRelationships_.SelectedIndexChanged += new System.EventHandler(this.lstRelationships_SelectedIndexChanged);
            this.lstRelationships_.Enter += new System.EventHandler(this.lstRelationships_Enter);
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
            tabFacts.Controls.Add(this.cboFactType_);
            tabFacts.Controls.Add(this.gridFacts_);
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
            this.cboFactType_.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFactType_.Location = new System.Drawing.Point(8, 228);
            this.cboFactType_.Name = "m_cboFactType";
            this.cboFactType_.Size = new System.Drawing.Size(272, 21);
            this.cboFactType_.TabIndex = 2;
            // 
            // m_gridFacts
            // 
            this.gridFacts_.CaptionVisible = false;
            this.gridFacts_.ContextMenuStrip = this.contextMenuStrip1;
            this.gridFacts_.DataMember = "";
            this.gridFacts_.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.gridFacts_.Location = new System.Drawing.Point(8, 8);
            this.gridFacts_.Name = "m_gridFacts";
            this.gridFacts_.Size = new System.Drawing.Size(656, 208);
            this.gridFacts_.TabIndex = 0;
            this.gridFacts_.Enter += new System.EventHandler(this.gridFacts_CurrentCellChanged);
            this.gridFacts_.MouseUp += new System.Windows.Forms.MouseEventHandler(this.m_gridFacts_MouseUp);
            this.gridFacts_.CurrentCellChanged += new System.EventHandler(this.gridFacts_CurrentCellChanged);
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
            tabAdvanced.Controls.Add(this.chkGedcom_);
            tabAdvanced.Controls.Add(label3);
            tabAdvanced.Controls.Add(this.cboMainImage_);
            tabAdvanced.Controls.Add(this.cboMother_);
            tabAdvanced.Controls.Add(this.cboFather_);
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
            this.chkGedcom_.AutoSize = true;
            this.chkGedcom_.Location = new System.Drawing.Point(112, 137);
            this.chkGedcom_.Name = "m_chkGedcom";
            this.chkGedcom_.Size = new System.Drawing.Size(130, 17);
            this.chkGedcom_.TabIndex = 13;
            this.chkGedcom_.Text = "Include in Gedcom file";
            this.chkGedcom_.UseVisualStyleBackColor = true;
            this.chkGedcom_.CheckedChanged += new System.EventHandler(this.chkGedcom_CheckedChanged);
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
            this.cboMainImage_.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMainImage_.Location = new System.Drawing.Point(112, 88);
            this.cboMainImage_.Name = "m_cboMainImage";
            this.cboMainImage_.Size = new System.Drawing.Size(272, 21);
            this.cboMainImage_.TabIndex = 11;
            this.cboMainImage_.SelectedIndexChanged += new System.EventHandler(this.cboMainImage_SelectedIndexChanged);
            // 
            // m_cboMother
            // 
            this.cboMother_.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMother_.Location = new System.Drawing.Point(112, 32);
            this.cboMother_.Name = "m_cboMother";
            this.cboMother_.Size = new System.Drawing.Size(272, 21);
            this.cboMother_.TabIndex = 3;
            this.cboMother_.SelectedIndexChanged += new System.EventHandler(this.cboMother_SelectedIndexChanged);
            // 
            // m_cboFather
            // 
            this.cboFather_.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFather_.Location = new System.Drawing.Point(112, 8);
            this.cboFather_.Name = "m_cboFather";
            this.cboFather_.Size = new System.Drawing.Size(272, 21);
            this.cboFather_.TabIndex = 2;
            this.cboFather_.SelectedIndexChanged += new System.EventHandler(this.cboFather_SelectedIndexChanged);
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
            groupBox2.Controls.Add(this.labDescription_);
            groupBox2.Location = new System.Drawing.Point(8, 296);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(680, 104);
            groupBox2.TabIndex = 20;
            groupBox2.TabStop = false;
            groupBox2.Text = "Description";
            // 
            // m_labDescription
            // 
            this.labDescription_.Location = new System.Drawing.Point(8, 24);
            this.labDescription_.Name = "m_labDescription";
            this.labDescription_.Size = new System.Drawing.Size(664, 72);
            this.labDescription_.TabIndex = 0;
            this.labDescription_.Text = "label7";
            // 
            // tabToDo
            // 
            tabToDo.Controls.Add(cmdDeleteToDo);
            tabToDo.Controls.Add(cmdAddToDo);
            tabToDo.Controls.Add(this.gridToDo_);
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
            this.gridToDo_.CaptionVisible = false;
            this.gridToDo_.DataMember = "";
            this.gridToDo_.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.gridToDo_.Location = new System.Drawing.Point(8, 8);
            this.gridToDo_.Name = "m_gridToDo";
            this.gridToDo_.Size = new System.Drawing.Size(656, 208);
            this.gridToDo_.TabIndex = 1;
            this.gridToDo_.Enter += new System.EventHandler(this.evtNonSpecificSource);
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
            this.grpSources_.Controls.Add(cmdDeleteSource);
            this.grpSources_.Controls.Add(this.cboSources_);
            this.grpSources_.Controls.Add(cmdAddSource);
            this.grpSources_.Controls.Add(this.gridSources_);
            this.grpSources_.Location = new System.Drawing.Point(8, 400);
            this.grpSources_.Name = "m_grpSources";
            this.grpSources_.Size = new System.Drawing.Size(680, 221);
            this.grpSources_.TabIndex = 19;
            this.grpSources_.TabStop = false;
            this.grpSources_.Text = "Sources";
            // 
            // m_cboSources
            // 
            this.cboSources_.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSources_.Location = new System.Drawing.Point(8, 184);
            this.cboSources_.Name = "m_cboSources";
            this.cboSources_.Size = new System.Drawing.Size(368, 21);
            this.cboSources_.TabIndex = 13;
            // 
            // m_gridSources
            // 
            this.gridSources_.CaptionText = "Sources for me";
            this.gridSources_.CaptionVisible = false;
            this.gridSources_.DataMember = "";
            this.gridSources_.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.gridSources_.Location = new System.Drawing.Point(8, 24);
            this.gridSources_.Name = "m_gridSources";
            this.gridSources_.Size = new System.Drawing.Size(664, 152);
            this.gridSources_.TabIndex = 11;
            this.gridSources_.Leave += new System.EventHandler(this.gridSources_Leave);
            // 
            // m_TabControl
            // 
            this.tabControl_.Controls.Add(tabBasic);
            this.tabControl_.Controls.Add(tabRelationships);
            this.tabControl_.Controls.Add(tabFacts);
            this.tabControl_.Controls.Add(tabAdvanced);
            this.tabControl_.Controls.Add(tabToDo);
            this.tabControl_.ImageList = oImageList16x16;
            this.tabControl_.Location = new System.Drawing.Point(8, 8);
            this.tabControl_.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl_.Name = "m_TabControl";
            this.tabControl_.SelectedIndex = 0;
            this.tabControl_.Size = new System.Drawing.Size(680, 284);
            this.tabControl_.TabIndex = 23;
            this.tabControl_.Enter += new System.EventHandler(this.evtNonSpecificSource);
            this.tabControl_.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
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
            this.Controls.Add(this.tabControl_);
            this.Controls.Add(groupBox2);
            this.Controls.Add(this.grpSources_);
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
            ((System.ComponentModel.ISupportInitialize)(this.gridFacts_)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            tabAdvanced.ResumeLayout(false);
            tabAdvanced.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_Image)).EndInit();
            groupBox2.ResumeLayout(false);
            tabToDo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridToDo_)).EndInit();
            this.grpSources_.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridSources_)).EndInit();
            this.tabControl_.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cboSex_;
        private System.Windows.Forms.Label labMaidenName_;
        private System.Windows.Forms.TextBox txtMaidenName_;
        private System.Windows.Forms.TextBox txtForename_;
        private System.Windows.Forms.TextBox txtSurname_;
        private System.Windows.Forms.TabControl tabControl_;
        private System.Windows.Forms.ComboBox cboFactType_;
        private System.Windows.Forms.CheckBox chkChildrenKnown_;
        private System.Windows.Forms.ListBox lstRelationships_;
        private System.Windows.Forms.ComboBox cboAddPartner_;
        private System.Windows.Forms.TextBox m_txtRelationComments;
        private System.Windows.Forms.TextBox txtComments_;
        private System.Windows.Forms.ComboBox m_cboTerminated;
        private System.Windows.Forms.ComboBox cboFather_;
        private System.Windows.Forms.ComboBox cboMother_;
        private System.Windows.Forms.PictureBox m_Image;
        private System.Windows.Forms.DataGrid gridFacts_;
        private System.Windows.Forms.DataGrid gridSources_;
        private System.Windows.Forms.ComboBox cboSources_;
        private FamilyTree.Viewer.CompoundDateEditBox dateDoD_;
        private FamilyTree.Viewer.CompoundDateEditBox dateDoB_;
        private FamilyTree.Viewer.CompoundDateEditBox dateRelationStart_;
        private FamilyTree.Viewer.CompoundDateEditBox dateRelationEnd_;
        private System.Windows.Forms.ComboBox m_cboRelationshipType;
        private Label labDescription_;
        private GroupBox grpSources_;
        private TextBox txtRelationLocation_;
        private ComboBox cboMainImage_;
        private DataGrid gridToDo_;
        private ComboBox m_cboEditor;
        private CheckBox chkGedcom_;
        private ContextMenuStrip contextMenuStrip1;

    }
}
