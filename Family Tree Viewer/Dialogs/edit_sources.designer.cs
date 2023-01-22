using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace family_tree.viewer
{
    public partial class EditSourcesDialog : System.Windows.Forms.Form
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
            System.Windows.Forms.Button buttonAddFreeTableRow;
            System.Windows.Forms.Button buttonRemoveFreeTableRow;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditSourcesDialog));
            family_tree.objects.CompoundDate compoundDate1 = new family_tree.objects.CompoundDate();
            this.lstSources_ = new System.Windows.Forms.ListBox();
            this.imageList16x16 = new System.Windows.Forms.ImageList(this.components);
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.gridReferences_ = new System.Windows.Forms.DataGrid();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grpFreeTable_ = new System.Windows.Forms.GroupBox();
            this.dataGridViewSourceFreeTable_ = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cboRepository_ = new System.Windows.Forms.ComboBox();
            this.cboAdditionalInfo_ = new System.Windows.Forms.ComboBox();
            this.dateSourceDate_ = new family_tree.viewer.CompoundDateEditBox();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.txtComments_ = new System.Windows.Forms.TextBox();
            this.cboPrefix_ = new System.Windows.Forms.ComboBox();
            this.txtDescription_ = new System.Windows.Forms.TextBox();
            this.grpBirth_ = new System.Windows.Forms.GroupBox();
            this.txtBirthReference_ = new System.Windows.Forms.TextBox();
            this.txtBirthMotherDetails_ = new System.Windows.Forms.TextBox();
            this.txtBirthInformantAddress_ = new System.Windows.Forms.TextBox();
            this.dtpBirthWhen_ = new System.Windows.Forms.DateTimePicker();
            this.txtBirthWhenReg_ = new System.Windows.Forms.TextBox();
            this.txtBirthInformant_ = new System.Windows.Forms.TextBox();
            this.txtBirthMother_ = new System.Windows.Forms.TextBox();
            this.txtBirthFatherOccupation_ = new System.Windows.Forms.TextBox();
            this.txtBirthFather_ = new System.Windows.Forms.TextBox();
            this.txtBirthSex_ = new System.Windows.Forms.TextBox();
            this.txtBirthName_ = new System.Windows.Forms.TextBox();
            this.txtBirthWhenWhere_ = new System.Windows.Forms.TextBox();
            this.txtBirthDistrict_ = new System.Windows.Forms.TextBox();
            this.grpMarriage_ = new System.Windows.Forms.GroupBox();
            this.txtMarrGro_ = new System.Windows.Forms.TextBox();
            this.dtpMarrWhen_ = new System.Windows.Forms.DateTimePicker();
            this.txtMarrWitness_ = new System.Windows.Forms.TextBox();
            this.txtMarrLocation_ = new System.Windows.Forms.TextBox();
            this.txtMarrBrideFatherOcc_ = new System.Windows.Forms.TextBox();
            this.txtMarrBrideFather_ = new System.Windows.Forms.TextBox();
            this.txtMarrBrideLoca_ = new System.Windows.Forms.TextBox();
            this.txtMarrBrideOccu_ = new System.Windows.Forms.TextBox();
            this.txtMarrBrideAge_ = new System.Windows.Forms.TextBox();
            this.txtMarrBride_ = new System.Windows.Forms.TextBox();
            this.txtMarrGroomFatherOcc_ = new System.Windows.Forms.TextBox();
            this.txtMarrGroomFather_ = new System.Windows.Forms.TextBox();
            this.txtMarrGroomLoca_ = new System.Windows.Forms.TextBox();
            this.txtMarrGroomOccu_ = new System.Windows.Forms.TextBox();
            this.txtMarrGroomAge_ = new System.Windows.Forms.TextBox();
            this.txtMarrGroom_ = new System.Windows.Forms.TextBox();
            this.grpCensus_ = new System.Windows.Forms.GroupBox();
            this.txtCensusPage_ = new System.Windows.Forms.TextBox();
            this.txtCensusFolio_ = new System.Windows.Forms.TextBox();
            this.txtCensusPiece_ = new System.Windows.Forms.TextBox();
            this.txtCensusSeries_ = new System.Windows.Forms.TextBox();
            this.txtCensusAddress_ = new System.Windows.Forms.TextBox();
            this.grpDeath_ = new System.Windows.Forms.GroupBox();
            this.txtDeathReference_ = new System.Windows.Forms.TextBox();
            this.txtDeathWhere_ = new System.Windows.Forms.TextBox();
            this.txtDeathWhenReg_ = new System.Windows.Forms.TextBox();
            this.txtDeathInformantDescription_ = new System.Windows.Forms.TextBox();
            this.txtDeathInformantAddress_ = new System.Windows.Forms.TextBox();
            this.txtDeathInformant_ = new System.Windows.Forms.TextBox();
            this.txtDeathUsualAddress_ = new System.Windows.Forms.TextBox();
            this.txtDeathCause_ = new System.Windows.Forms.TextBox();
            this.txtDeathOccupation_ = new System.Windows.Forms.TextBox();
            this.txtDeathDatePlace_ = new System.Windows.Forms.TextBox();
            this.txtDeathSex_ = new System.Windows.Forms.TextBox();
            this.txtDeathName_ = new System.Windows.Forms.TextBox();
            this.txtDeathWhen_ = new System.Windows.Forms.TextBox();
            this.txtDeathDistrict_ = new System.Windows.Forms.TextBox();
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
            buttonAddFreeTableRow = new System.Windows.Forms.Button();
            buttonRemoveFreeTableRow = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridReferences_)).BeginInit();
            this.panel1.SuspendLayout();
            this.grpFreeTable_.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSourceFreeTable_)).BeginInit();
            this.panel2.SuspendLayout();
            this.grpBirth_.SuspendLayout();
            this.grpMarriage_.SuspendLayout();
            this.grpCensus_.SuspendLayout();
            this.grpDeath_.SuspendLayout();
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
            cmdCensusAddress.Image = global::family_tree.viewer.Properties.Resources.Earth;
            cmdCensusAddress.Location = new System.Drawing.Point(529, 91);
            cmdCensusAddress.Name = "cmdCensusAddress";
            cmdCensusAddress.Size = new System.Drawing.Size(21, 21);
            cmdCensusAddress.TabIndex = 12;
            cmdCensusAddress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            cmdCensusAddress.Click += new System.EventHandler(this.cmdCensusAddress_Click);
            // 
            // cmdCensusOpen
            // 
            cmdCensusOpen.Image = global::family_tree.viewer.Properties.Resources.Open;
            cmdCensusOpen.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            cmdCensusOpen.Location = new System.Drawing.Point(8, 191);
            cmdCensusOpen.Name = "cmdCensusOpen";
            cmdCensusOpen.Size = new System.Drawing.Size(100, 30);
            cmdCensusOpen.TabIndex = 2;
            cmdCensusOpen.Text = "Open";
            cmdCensusOpen.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            cmdCensusOpen.Click += new System.EventHandler(this.cmdCensusOpen_Click);
            // 
            // buttonAddFreeTableRow
            // 
            buttonAddFreeTableRow.Image = global::family_tree.viewer.Properties.Resources.add;
            buttonAddFreeTableRow.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            buttonAddFreeTableRow.Location = new System.Drawing.Point(3, 3);
            buttonAddFreeTableRow.Name = "buttonAddFreeTableRow";
            buttonAddFreeTableRow.Size = new System.Drawing.Size(100, 30);
            buttonAddFreeTableRow.TabIndex = 17;
            buttonAddFreeTableRow.Text = "Add";
            buttonAddFreeTableRow.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            buttonAddFreeTableRow.Click += new System.EventHandler(this.buttonAddFreeTableRow_Click);
            // 
            // buttonRemoveFreeTableRow
            // 
            buttonRemoveFreeTableRow.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            buttonRemoveFreeTableRow.Image = global::family_tree.viewer.Properties.Resources.delete;
            buttonRemoveFreeTableRow.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            buttonRemoveFreeTableRow.Location = new System.Drawing.Point(109, 3);
            buttonRemoveFreeTableRow.Name = "buttonRemoveFreeTableRow";
            buttonRemoveFreeTableRow.Size = new System.Drawing.Size(100, 30);
            buttonRemoveFreeTableRow.TabIndex = 18;
            buttonRemoveFreeTableRow.Text = "Delete";
            buttonRemoveFreeTableRow.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            buttonRemoveFreeTableRow.Click += new System.EventHandler(this.buttonRemoveFreeTableRow_Click);
            // 
            // lstSources_
            // 
            this.lstSources_.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstSources_.Location = new System.Drawing.Point(0, 25);
            this.lstSources_.Name = "lstSources_";
            this.lstSources_.Size = new System.Drawing.Size(400, 433);
            this.lstSources_.TabIndex = 0;
            this.lstSources_.SelectedIndexChanged += new System.EventHandler(this.lstSources_SelectedIndexChanged);
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
            // gridReferences_
            // 
            this.gridReferences_.CaptionVisible = false;
            this.gridReferences_.DataMember = "";
            this.gridReferences_.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gridReferences_.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.gridReferences_.Location = new System.Drawing.Point(0, 513);
            this.gridReferences_.Name = "gridReferences_";
            this.gridReferences_.Size = new System.Drawing.Size(984, 104);
            this.gridReferences_.TabIndex = 17;
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
            this.panel1.Controls.Add(this.grpFreeTable_);
            this.panel1.Controls.Add(label31);
            this.panel1.Controls.Add(label30);
            this.panel1.Controls.Add(label29);
            this.panel1.Controls.Add(label28);
            this.panel1.Controls.Add(this.cboRepository_);
            this.panel1.Controls.Add(this.cboAdditionalInfo_);
            this.panel1.Controls.Add(this.dateSourceDate_);
            this.panel1.Controls.Add(this.cmdCancel);
            this.panel1.Controls.Add(this.cmdOK);
            this.panel1.Controls.Add(this.txtComments_);
            this.panel1.Controls.Add(this.cboPrefix_);
            this.panel1.Controls.Add(this.txtDescription_);
            this.panel1.Controls.Add(this.grpBirth_);
            this.panel1.Controls.Add(this.grpMarriage_);
            this.panel1.Controls.Add(this.grpCensus_);
            this.panel1.Controls.Add(this.grpDeath_);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(408, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(576, 505);
            this.panel1.TabIndex = 19;
            // 
            // grpFreeTable_
            // 
            this.grpFreeTable_.Controls.Add(this.dataGridViewSourceFreeTable_);
            this.grpFreeTable_.Controls.Add(this.panel2);
            this.grpFreeTable_.Location = new System.Drawing.Point(8, 139);
            this.grpFreeTable_.Name = "grpFreeTable_";
            this.grpFreeTable_.Size = new System.Drawing.Size(560, 321);
            this.grpFreeTable_.TabIndex = 35;
            this.grpFreeTable_.TabStop = false;
            this.grpFreeTable_.Text = "Free Table";
            // 
            // dataGridViewSourceFreeTable_
            // 
            this.dataGridViewSourceFreeTable_.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSourceFreeTable_.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewSourceFreeTable_.Location = new System.Drawing.Point(3, 17);
            this.dataGridViewSourceFreeTable_.Name = "dataGridViewSourceFreeTable_";
            this.dataGridViewSourceFreeTable_.Size = new System.Drawing.Size(554, 265);
            this.dataGridViewSourceFreeTable_.TabIndex = 19;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(buttonRemoveFreeTableRow);
            this.panel2.Controls.Add(buttonAddFreeTableRow);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(3, 282);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(554, 36);
            this.panel2.TabIndex = 20;
            // 
            // cboRepository_
            // 
            this.cboRepository_.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboRepository_.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRepository_.Location = new System.Drawing.Point(135, 470);
            this.cboRepository_.Name = "cboRepository_";
            this.cboRepository_.Size = new System.Drawing.Size(152, 21);
            this.cboRepository_.TabIndex = 30;
            this.cboRepository_.SelectedIndexChanged += new System.EventHandler(this.cboRepository_SelectedIndexChanged);
            // 
            // cboAdditionalInfo_
            // 
            this.cboAdditionalInfo_.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboAdditionalInfo_.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAdditionalInfo_.Location = new System.Drawing.Point(8, 470);
            this.cboAdditionalInfo_.Name = "cboAdditionalInfo_";
            this.cboAdditionalInfo_.Size = new System.Drawing.Size(121, 21);
            this.cboAdditionalInfo_.TabIndex = 25;
            this.cboAdditionalInfo_.SelectedIndexChanged += new System.EventHandler(this.cboAdditionalInfo_SelectedIndexChanged);
            // 
            // dateSourceDate_
            // 
            this.dateSourceDate_.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dateSourceDate_.Location = new System.Drawing.Point(424, 32);
            this.dateSourceDate_.Name = "dateSourceDate_";
            this.dateSourceDate_.Size = new System.Drawing.Size(144, 24);
            this.dateSourceDate_.TabIndex = 24;
            compoundDate1.date = new System.DateTime(2007, 10, 11, 0, 0, 0, 0);
            compoundDate1.status = 0;
            this.dateSourceDate_.theDate = compoundDate1;
            this.dateSourceDate_.eventValueChanged += new family_tree.viewer.FuncValueChanged(this.dateTheDate_evtValueChanged);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Image = global::family_tree.viewer.Properties.Resources.Cancel;
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
            this.cmdOK.Image = global::family_tree.viewer.Properties.Resources.OK;
            this.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cmdOK.Location = new System.Drawing.Point(468, 464);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(100, 30);
            this.cmdOK.TabIndex = 20;
            this.cmdOK.Text = "OK";
            this.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // txtComments_
            // 
            this.txtComments_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtComments_.Location = new System.Drawing.Point(80, 56);
            this.txtComments_.Multiline = true;
            this.txtComments_.Name = "txtComments_";
            this.txtComments_.Size = new System.Drawing.Size(488, 64);
            this.txtComments_.TabIndex = 19;
            this.txtComments_.Text = "textBox2";
            this.txtComments_.TextChanged += new System.EventHandler(this.txtComments_TextChanged);
            // 
            // cboPrefix_
            // 
            this.cboPrefix_.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPrefix_.Items.AddRange(new object[] {
            "Birth Certificate:",
            "Marriage Certificate:",
            "Death Certificate:",
            "Census 1861:",
            "Census 1901:",
            "Census:",
            "1939 Register:",
            "Interview:",
            "Letter:"});
            this.cboPrefix_.Location = new System.Drawing.Point(80, 32);
            this.cboPrefix_.Name = "cboPrefix_";
            this.cboPrefix_.Size = new System.Drawing.Size(208, 21);
            this.cboPrefix_.TabIndex = 18;
            this.cboPrefix_.SelectedIndexChanged += new System.EventHandler(this.cboPrefix_SelectedIndexChanged);
            // 
            // txtDescription_
            // 
            this.txtDescription_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescription_.Location = new System.Drawing.Point(80, 8);
            this.txtDescription_.Name = "txtDescription_";
            this.txtDescription_.Size = new System.Drawing.Size(488, 21);
            this.txtDescription_.TabIndex = 17;
            this.txtDescription_.Text = "txtDescription";
            this.txtDescription_.TextChanged += new System.EventHandler(this.txtDescription_TextChanged);
            // 
            // grpBirth_
            // 
            this.grpBirth_.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBirth_.Controls.Add(this.txtBirthReference_);
            this.grpBirth_.Controls.Add(label39);
            this.grpBirth_.Controls.Add(this.txtBirthMotherDetails_);
            this.grpBirth_.Controls.Add(this.txtBirthInformantAddress_);
            this.grpBirth_.Controls.Add(this.dtpBirthWhen_);
            this.grpBirth_.Controls.Add(this.txtBirthWhenReg_);
            this.grpBirth_.Controls.Add(this.txtBirthInformant_);
            this.grpBirth_.Controls.Add(this.txtBirthMother_);
            this.grpBirth_.Controls.Add(this.txtBirthFatherOccupation_);
            this.grpBirth_.Controls.Add(this.txtBirthFather_);
            this.grpBirth_.Controls.Add(label15);
            this.grpBirth_.Controls.Add(label14);
            this.grpBirth_.Controls.Add(label13);
            this.grpBirth_.Controls.Add(label12);
            this.grpBirth_.Controls.Add(label11);
            this.grpBirth_.Controls.Add(this.txtBirthSex_);
            this.grpBirth_.Controls.Add(label10);
            this.grpBirth_.Controls.Add(this.txtBirthName_);
            this.grpBirth_.Controls.Add(label9);
            this.grpBirth_.Controls.Add(this.txtBirthWhenWhere_);
            this.grpBirth_.Controls.Add(label8);
            this.grpBirth_.Controls.Add(label7);
            this.grpBirth_.Controls.Add(this.txtBirthDistrict_);
            this.grpBirth_.Location = new System.Drawing.Point(8, 139);
            this.grpBirth_.Name = "grpBirth_";
            this.grpBirth_.Size = new System.Drawing.Size(560, 321);
            this.grpBirth_.TabIndex = 28;
            this.grpBirth_.TabStop = false;
            this.grpBirth_.Text = "Birth Certificate";
            // 
            // txtBirthReference_
            // 
            this.txtBirthReference_.Location = new System.Drawing.Point(128, 168);
            this.txtBirthReference_.Name = "txtBirthReference_";
            this.txtBirthReference_.Size = new System.Drawing.Size(288, 21);
            this.txtBirthReference_.TabIndex = 27;
            this.txtBirthReference_.Text = "textBox1";
            this.txtBirthReference_.TextChanged += new System.EventHandler(this.evtAdditionalBirth_Changed);
            // 
            // txtBirthMotherDetails_
            // 
            this.txtBirthMotherDetails_.Location = new System.Drawing.Point(270, 120);
            this.txtBirthMotherDetails_.Name = "txtBirthMotherDetails_";
            this.txtBirthMotherDetails_.Size = new System.Drawing.Size(282, 21);
            this.txtBirthMotherDetails_.TabIndex = 16;
            this.txtBirthMotherDetails_.Text = "textBox1";
            this.toolTip1.SetToolTip(this.txtBirthMotherDetails_, "Mother details");
            this.txtBirthMotherDetails_.TextChanged += new System.EventHandler(this.evtAdditionalBirth_Changed);
            // 
            // txtBirthInformantAddress_
            // 
            this.txtBirthInformantAddress_.Location = new System.Drawing.Point(270, 144);
            this.txtBirthInformantAddress_.Name = "txtBirthInformantAddress_";
            this.txtBirthInformantAddress_.Size = new System.Drawing.Size(282, 21);
            this.txtBirthInformantAddress_.TabIndex = 19;
            this.txtBirthInformantAddress_.Text = "textBox1";
            this.toolTip1.SetToolTip(this.txtBirthInformantAddress_, "Informant address");
            this.txtBirthInformantAddress_.TextChanged += new System.EventHandler(this.evtAdditionalBirth_Changed);
            // 
            // dtpBirthWhen_
            // 
            this.dtpBirthWhen_.CustomFormat = "d MMM yyyy";
            this.dtpBirthWhen_.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpBirthWhen_.Location = new System.Drawing.Point(128, 47);
            this.dtpBirthWhen_.Name = "dtpBirthWhen_";
            this.dtpBirthWhen_.Size = new System.Drawing.Size(136, 21);
            this.dtpBirthWhen_.TabIndex = 1;
            this.dtpBirthWhen_.ValueChanged += new System.EventHandler(this.evtAdditionalBirth_Changed);
            // 
            // txtBirthWhenReg_
            // 
            this.txtBirthWhenReg_.Location = new System.Drawing.Point(392, 197);
            this.txtBirthWhenReg_.Name = "txtBirthWhenReg_";
            this.txtBirthWhenReg_.Size = new System.Drawing.Size(160, 21);
            this.txtBirthWhenReg_.TabIndex = 20;
            this.txtBirthWhenReg_.Text = "textBox1";
            this.txtBirthWhenReg_.TextChanged += new System.EventHandler(this.evtAdditionalBirth_Changed);
            // 
            // txtBirthInformant_
            // 
            this.txtBirthInformant_.Location = new System.Drawing.Point(128, 144);
            this.txtBirthInformant_.Name = "txtBirthInformant_";
            this.txtBirthInformant_.Size = new System.Drawing.Size(136, 21);
            this.txtBirthInformant_.TabIndex = 17;
            this.txtBirthInformant_.Text = "textBox1";
            this.txtBirthInformant_.TextChanged += new System.EventHandler(this.evtAdditionalBirth_Changed);
            // 
            // txtBirthMother_
            // 
            this.txtBirthMother_.Location = new System.Drawing.Point(128, 120);
            this.txtBirthMother_.Name = "txtBirthMother_";
            this.txtBirthMother_.Size = new System.Drawing.Size(136, 21);
            this.txtBirthMother_.TabIndex = 15;
            this.txtBirthMother_.Text = "textBox1";
            this.txtBirthMother_.TextChanged += new System.EventHandler(this.evtAdditionalBirth_Changed);
            // 
            // txtBirthFatherOccupation_
            // 
            this.txtBirthFatherOccupation_.Location = new System.Drawing.Point(392, 96);
            this.txtBirthFatherOccupation_.Name = "txtBirthFatherOccupation_";
            this.txtBirthFatherOccupation_.Size = new System.Drawing.Size(160, 21);
            this.txtBirthFatherOccupation_.TabIndex = 14;
            this.txtBirthFatherOccupation_.Text = "textBox1";
            this.txtBirthFatherOccupation_.TextChanged += new System.EventHandler(this.evtAdditionalBirth_Changed);
            // 
            // txtBirthFather_
            // 
            this.txtBirthFather_.Location = new System.Drawing.Point(128, 96);
            this.txtBirthFather_.Name = "txtBirthFather_";
            this.txtBirthFather_.Size = new System.Drawing.Size(136, 21);
            this.txtBirthFather_.TabIndex = 13;
            this.txtBirthFather_.Text = "textBox1";
            this.txtBirthFather_.TextChanged += new System.EventHandler(this.evtAdditionalBirth_Changed);
            // 
            // txtBirthSex_
            // 
            this.txtBirthSex_.Location = new System.Drawing.Point(392, 72);
            this.txtBirthSex_.Name = "txtBirthSex_";
            this.txtBirthSex_.Size = new System.Drawing.Size(160, 21);
            this.txtBirthSex_.TabIndex = 7;
            this.txtBirthSex_.Text = "textBox1";
            this.txtBirthSex_.TextChanged += new System.EventHandler(this.evtAdditionalBirth_Changed);
            // 
            // txtBirthName_
            // 
            this.txtBirthName_.Location = new System.Drawing.Point(128, 72);
            this.txtBirthName_.Name = "txtBirthName_";
            this.txtBirthName_.Size = new System.Drawing.Size(136, 21);
            this.txtBirthName_.TabIndex = 5;
            this.txtBirthName_.Text = "textBox1";
            this.txtBirthName_.TextChanged += new System.EventHandler(this.evtAdditionalBirth_Changed);
            // 
            // txtBirthWhenWhere_
            // 
            this.txtBirthWhenWhere_.Location = new System.Drawing.Point(275, 48);
            this.txtBirthWhenWhere_.Name = "txtBirthWhenWhere_";
            this.txtBirthWhenWhere_.Size = new System.Drawing.Size(277, 21);
            this.txtBirthWhenWhere_.TabIndex = 2;
            this.txtBirthWhenWhere_.Text = "textBox1";
            this.txtBirthWhenWhere_.TextChanged += new System.EventHandler(this.evtAdditionalBirth_Changed);
            // 
            // txtBirthDistrict_
            // 
            this.txtBirthDistrict_.Location = new System.Drawing.Point(128, 24);
            this.txtBirthDistrict_.Name = "txtBirthDistrict_";
            this.txtBirthDistrict_.Size = new System.Drawing.Size(424, 21);
            this.txtBirthDistrict_.TabIndex = 0;
            this.txtBirthDistrict_.Text = "textBox1";
            this.txtBirthDistrict_.TextChanged += new System.EventHandler(this.evtAdditionalBirth_Changed);
            // 
            // grpMarriage_
            // 
            this.grpMarriage_.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpMarriage_.Controls.Add(this.txtMarrGro_);
            this.grpMarriage_.Controls.Add(label38);
            this.grpMarriage_.Controls.Add(label37);
            this.grpMarriage_.Controls.Add(this.dtpMarrWhen_);
            this.grpMarriage_.Controls.Add(this.txtMarrWitness_);
            this.grpMarriage_.Controls.Add(this.txtMarrLocation_);
            this.grpMarriage_.Controls.Add(label6);
            this.grpMarriage_.Controls.Add(label5);
            this.grpMarriage_.Controls.Add(label4);
            this.grpMarriage_.Controls.Add(label3);
            this.grpMarriage_.Controls.Add(label2);
            this.grpMarriage_.Controls.Add(label1);
            this.grpMarriage_.Controls.Add(this.txtMarrBrideFatherOcc_);
            this.grpMarriage_.Controls.Add(this.txtMarrBrideFather_);
            this.grpMarriage_.Controls.Add(this.txtMarrBrideLoca_);
            this.grpMarriage_.Controls.Add(this.txtMarrBrideOccu_);
            this.grpMarriage_.Controls.Add(this.txtMarrBrideAge_);
            this.grpMarriage_.Controls.Add(this.txtMarrBride_);
            this.grpMarriage_.Controls.Add(this.txtMarrGroomFatherOcc_);
            this.grpMarriage_.Controls.Add(this.txtMarrGroomFather_);
            this.grpMarriage_.Controls.Add(this.txtMarrGroomLoca_);
            this.grpMarriage_.Controls.Add(this.txtMarrGroomOccu_);
            this.grpMarriage_.Controls.Add(this.txtMarrGroomAge_);
            this.grpMarriage_.Controls.Add(this.txtMarrGroom_);
            this.grpMarriage_.Location = new System.Drawing.Point(8, 140);
            this.grpMarriage_.Name = "grpMarriage_";
            this.grpMarriage_.Size = new System.Drawing.Size(560, 320);
            this.grpMarriage_.TabIndex = 27;
            this.grpMarriage_.TabStop = false;
            this.grpMarriage_.Text = "Marriage Certificate";
            // 
            // txtMarrGro_
            // 
            this.txtMarrGro_.Location = new System.Drawing.Point(86, 190);
            this.txtMarrGro_.Name = "txtMarrGro_";
            this.txtMarrGro_.Size = new System.Drawing.Size(288, 21);
            this.txtMarrGro_.TabIndex = 25;
            this.txtMarrGro_.Text = "textBox1";
            this.txtMarrGro_.TextChanged += new System.EventHandler(this.evtAdditionalMarriage_Changed);
            // 
            // dtpMarrWhen_
            // 
            this.dtpMarrWhen_.CustomFormat = "d MMM yyyy";
            this.dtpMarrWhen_.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpMarrWhen_.Location = new System.Drawing.Point(86, 20);
            this.dtpMarrWhen_.Name = "dtpMarrWhen_";
            this.dtpMarrWhen_.Size = new System.Drawing.Size(136, 21);
            this.dtpMarrWhen_.TabIndex = 22;
            this.dtpMarrWhen_.ValueChanged += new System.EventHandler(this.evtAdditionalMarriage_Changed);
            // 
            // txtMarrWitness_
            // 
            this.txtMarrWitness_.Location = new System.Drawing.Point(86, 165);
            this.txtMarrWitness_.Name = "txtMarrWitness_";
            this.txtMarrWitness_.Size = new System.Drawing.Size(288, 21);
            this.txtMarrWitness_.TabIndex = 18;
            this.txtMarrWitness_.Text = "textBox1";
            this.txtMarrWitness_.TextChanged += new System.EventHandler(this.evtAdditionalMarriage_Changed);
            // 
            // txtMarrLocation_
            // 
            this.txtMarrLocation_.Location = new System.Drawing.Point(86, 45);
            this.txtMarrLocation_.Name = "txtMarrLocation_";
            this.txtMarrLocation_.Size = new System.Drawing.Size(464, 21);
            this.txtMarrLocation_.TabIndex = 0;
            this.txtMarrLocation_.Text = "m_txtMarrLocation";
            this.txtMarrLocation_.TextChanged += new System.EventHandler(this.evtAdditionalMarriage_Changed);
            // 
            // txtMarrBrideFatherOcc_
            // 
            this.txtMarrBrideFatherOcc_.Location = new System.Drawing.Point(254, 141);
            this.txtMarrBrideFatherOcc_.Name = "txtMarrBrideFatherOcc_";
            this.txtMarrBrideFatherOcc_.Size = new System.Drawing.Size(120, 21);
            this.txtMarrBrideFatherOcc_.TabIndex = 12;
            this.txtMarrBrideFatherOcc_.Text = "textBox1";
            this.txtMarrBrideFatherOcc_.TextChanged += new System.EventHandler(this.evtAdditionalMarriage_Changed);
            // 
            // txtMarrBrideFather_
            // 
            this.txtMarrBrideFather_.Location = new System.Drawing.Point(86, 141);
            this.txtMarrBrideFather_.Name = "txtMarrBrideFather_";
            this.txtMarrBrideFather_.Size = new System.Drawing.Size(120, 21);
            this.txtMarrBrideFather_.TabIndex = 11;
            this.txtMarrBrideFather_.Text = "textBox1";
            this.txtMarrBrideFather_.TextChanged += new System.EventHandler(this.evtAdditionalMarriage_Changed);
            // 
            // txtMarrBrideLoca_
            // 
            this.txtMarrBrideLoca_.Location = new System.Drawing.Point(382, 93);
            this.txtMarrBrideLoca_.Name = "txtMarrBrideLoca_";
            this.txtMarrBrideLoca_.Size = new System.Drawing.Size(168, 21);
            this.txtMarrBrideLoca_.TabIndex = 8;
            this.txtMarrBrideLoca_.Text = "textBox1";
            this.txtMarrBrideLoca_.TextChanged += new System.EventHandler(this.evtAdditionalMarriage_Changed);
            // 
            // txtMarrBrideOccu_
            // 
            this.txtMarrBrideOccu_.Location = new System.Drawing.Point(254, 93);
            this.txtMarrBrideOccu_.Name = "txtMarrBrideOccu_";
            this.txtMarrBrideOccu_.Size = new System.Drawing.Size(120, 21);
            this.txtMarrBrideOccu_.TabIndex = 7;
            this.txtMarrBrideOccu_.Text = "textBox1";
            this.txtMarrBrideOccu_.TextChanged += new System.EventHandler(this.evtAdditionalMarriage_Changed);
            // 
            // txtMarrBrideAge_
            // 
            this.txtMarrBrideAge_.Location = new System.Drawing.Point(214, 93);
            this.txtMarrBrideAge_.Name = "txtMarrBrideAge_";
            this.txtMarrBrideAge_.Size = new System.Drawing.Size(32, 21);
            this.txtMarrBrideAge_.TabIndex = 6;
            this.txtMarrBrideAge_.Text = "textBox1";
            this.txtMarrBrideAge_.TextChanged += new System.EventHandler(this.evtAdditionalMarriage_Changed);
            // 
            // txtMarrBride_
            // 
            this.txtMarrBride_.Location = new System.Drawing.Point(86, 93);
            this.txtMarrBride_.Name = "txtMarrBride_";
            this.txtMarrBride_.Size = new System.Drawing.Size(120, 21);
            this.txtMarrBride_.TabIndex = 5;
            this.txtMarrBride_.Text = "textBox1";
            this.txtMarrBride_.TextChanged += new System.EventHandler(this.evtAdditionalMarriage_Changed);
            // 
            // txtMarrGroomFatherOcc_
            // 
            this.txtMarrGroomFatherOcc_.Location = new System.Drawing.Point(254, 117);
            this.txtMarrGroomFatherOcc_.Name = "txtMarrGroomFatherOcc_";
            this.txtMarrGroomFatherOcc_.Size = new System.Drawing.Size(120, 21);
            this.txtMarrGroomFatherOcc_.TabIndex = 10;
            this.txtMarrGroomFatherOcc_.Text = "textBox1";
            this.txtMarrGroomFatherOcc_.TextChanged += new System.EventHandler(this.evtAdditionalMarriage_Changed);
            // 
            // txtMarrGroomFather_
            // 
            this.txtMarrGroomFather_.Location = new System.Drawing.Point(86, 117);
            this.txtMarrGroomFather_.Name = "txtMarrGroomFather_";
            this.txtMarrGroomFather_.Size = new System.Drawing.Size(120, 21);
            this.txtMarrGroomFather_.TabIndex = 9;
            this.txtMarrGroomFather_.Text = "textBox1";
            this.txtMarrGroomFather_.TextChanged += new System.EventHandler(this.evtAdditionalMarriage_Changed);
            // 
            // txtMarrGroomLoca_
            // 
            this.txtMarrGroomLoca_.Location = new System.Drawing.Point(382, 69);
            this.txtMarrGroomLoca_.Name = "txtMarrGroomLoca_";
            this.txtMarrGroomLoca_.Size = new System.Drawing.Size(168, 21);
            this.txtMarrGroomLoca_.TabIndex = 4;
            this.txtMarrGroomLoca_.Text = "textBox1";
            this.txtMarrGroomLoca_.TextChanged += new System.EventHandler(this.evtAdditionalMarriage_Changed);
            // 
            // txtMarrGroomOccu_
            // 
            this.txtMarrGroomOccu_.Location = new System.Drawing.Point(254, 69);
            this.txtMarrGroomOccu_.Name = "txtMarrGroomOccu_";
            this.txtMarrGroomOccu_.Size = new System.Drawing.Size(120, 21);
            this.txtMarrGroomOccu_.TabIndex = 3;
            this.txtMarrGroomOccu_.Text = "textBox1";
            this.txtMarrGroomOccu_.TextChanged += new System.EventHandler(this.evtAdditionalMarriage_Changed);
            // 
            // txtMarrGroomAge_
            // 
            this.txtMarrGroomAge_.Location = new System.Drawing.Point(214, 69);
            this.txtMarrGroomAge_.Name = "txtMarrGroomAge_";
            this.txtMarrGroomAge_.Size = new System.Drawing.Size(32, 21);
            this.txtMarrGroomAge_.TabIndex = 2;
            this.txtMarrGroomAge_.Text = "textBox1";
            this.txtMarrGroomAge_.TextChanged += new System.EventHandler(this.evtAdditionalMarriage_Changed);
            // 
            // txtMarrGroom_
            // 
            this.txtMarrGroom_.Location = new System.Drawing.Point(86, 69);
            this.txtMarrGroom_.Name = "txtMarrGroom_";
            this.txtMarrGroom_.Size = new System.Drawing.Size(120, 21);
            this.txtMarrGroom_.TabIndex = 1;
            this.txtMarrGroom_.Text = "textBox1";
            this.txtMarrGroom_.TextChanged += new System.EventHandler(this.evtAdditionalMarriage_Changed);
            // 
            // grpCensus_
            // 
            this.grpCensus_.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpCensus_.Controls.Add(cmdCensusAddress);
            this.grpCensus_.Controls.Add(label36);
            this.grpCensus_.Controls.Add(label35);
            this.grpCensus_.Controls.Add(label34);
            this.grpCensus_.Controls.Add(label33);
            this.grpCensus_.Controls.Add(label32);
            this.grpCensus_.Controls.Add(this.txtCensusPage_);
            this.grpCensus_.Controls.Add(this.txtCensusFolio_);
            this.grpCensus_.Controls.Add(this.txtCensusPiece_);
            this.grpCensus_.Controls.Add(this.txtCensusSeries_);
            this.grpCensus_.Controls.Add(cmdCensusOpen);
            this.grpCensus_.Controls.Add(this.txtCensusAddress_);
            this.grpCensus_.Location = new System.Drawing.Point(8, 139);
            this.grpCensus_.Name = "grpCensus_";
            this.grpCensus_.Size = new System.Drawing.Size(560, 321);
            this.grpCensus_.TabIndex = 26;
            this.grpCensus_.TabStop = false;
            this.grpCensus_.Text = "Census";
            // 
            // txtCensusPage_
            // 
            this.txtCensusPage_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCensusPage_.Location = new System.Drawing.Point(248, 40);
            this.txtCensusPage_.Name = "txtCensusPage_";
            this.txtCensusPage_.Size = new System.Drawing.Size(75, 21);
            this.txtCensusPage_.TabIndex = 6;
            this.txtCensusPage_.Text = "textBox1";
            this.txtCensusPage_.TextChanged += new System.EventHandler(this.evtAdditionalCensus_Changed);
            // 
            // txtCensusFolio_
            // 
            this.txtCensusFolio_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCensusFolio_.Location = new System.Drawing.Point(167, 40);
            this.txtCensusFolio_.Name = "txtCensusFolio_";
            this.txtCensusFolio_.Size = new System.Drawing.Size(75, 21);
            this.txtCensusFolio_.TabIndex = 5;
            this.txtCensusFolio_.Text = "textBox1";
            this.txtCensusFolio_.TextChanged += new System.EventHandler(this.evtAdditionalCensus_Changed);
            // 
            // txtCensusPiece_
            // 
            this.txtCensusPiece_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCensusPiece_.Location = new System.Drawing.Point(86, 40);
            this.txtCensusPiece_.Name = "txtCensusPiece_";
            this.txtCensusPiece_.Size = new System.Drawing.Size(75, 21);
            this.txtCensusPiece_.TabIndex = 4;
            this.txtCensusPiece_.Text = "textBox1";
            this.txtCensusPiece_.TextChanged += new System.EventHandler(this.evtAdditionalCensus_Changed);
            // 
            // txtCensusSeries_
            // 
            this.txtCensusSeries_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCensusSeries_.Location = new System.Drawing.Point(8, 40);
            this.txtCensusSeries_.Name = "txtCensusSeries_";
            this.txtCensusSeries_.Size = new System.Drawing.Size(75, 21);
            this.txtCensusSeries_.TabIndex = 3;
            this.txtCensusSeries_.Text = "textBox1";
            this.txtCensusSeries_.TextChanged += new System.EventHandler(this.evtAdditionalCensus_Changed);
            // 
            // txtCensusAddress_
            // 
            this.txtCensusAddress_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCensusAddress_.Location = new System.Drawing.Point(8, 91);
            this.txtCensusAddress_.Name = "txtCensusAddress_";
            this.txtCensusAddress_.Size = new System.Drawing.Size(520, 21);
            this.txtCensusAddress_.TabIndex = 1;
            this.txtCensusAddress_.Text = "textBox1";
            this.txtCensusAddress_.TextChanged += new System.EventHandler(this.evtAdditionalCensus_Changed);
            // 
            // grpDeath_
            // 
            this.grpDeath_.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpDeath_.Controls.Add(this.txtDeathReference_);
            this.grpDeath_.Controls.Add(label40);
            this.grpDeath_.Controls.Add(this.txtDeathWhere_);
            this.grpDeath_.Controls.Add(label27);
            this.grpDeath_.Controls.Add(this.txtDeathWhenReg_);
            this.grpDeath_.Controls.Add(label26);
            this.grpDeath_.Controls.Add(this.txtDeathInformantDescription_);
            this.grpDeath_.Controls.Add(label25);
            this.grpDeath_.Controls.Add(label24);
            this.grpDeath_.Controls.Add(this.txtDeathInformantAddress_);
            this.grpDeath_.Controls.Add(this.txtDeathInformant_);
            this.grpDeath_.Controls.Add(label23);
            this.grpDeath_.Controls.Add(label22);
            this.grpDeath_.Controls.Add(label21);
            this.grpDeath_.Controls.Add(label20);
            this.grpDeath_.Controls.Add(this.txtDeathUsualAddress_);
            this.grpDeath_.Controls.Add(this.txtDeathCause_);
            this.grpDeath_.Controls.Add(this.txtDeathOccupation_);
            this.grpDeath_.Controls.Add(this.txtDeathDatePlace_);
            this.grpDeath_.Controls.Add(this.txtDeathSex_);
            this.grpDeath_.Controls.Add(label16);
            this.grpDeath_.Controls.Add(this.txtDeathName_);
            this.grpDeath_.Controls.Add(label17);
            this.grpDeath_.Controls.Add(this.txtDeathWhen_);
            this.grpDeath_.Controls.Add(label18);
            this.grpDeath_.Controls.Add(label19);
            this.grpDeath_.Controls.Add(this.txtDeathDistrict_);
            this.grpDeath_.Location = new System.Drawing.Point(8, 139);
            this.grpDeath_.Name = "grpDeath_";
            this.grpDeath_.Size = new System.Drawing.Size(560, 321);
            this.grpDeath_.TabIndex = 29;
            this.grpDeath_.TabStop = false;
            this.grpDeath_.Text = "Death Certificate";
            // 
            // txtDeathReference_
            // 
            this.txtDeathReference_.Location = new System.Drawing.Point(128, 274);
            this.txtDeathReference_.Name = "txtDeathReference_";
            this.txtDeathReference_.Size = new System.Drawing.Size(288, 21);
            this.txtDeathReference_.TabIndex = 33;
            this.txtDeathReference_.Text = "textBox1";
            this.txtDeathReference_.TextChanged += new System.EventHandler(this.evtAdditionalDeath_Changed);
            // 
            // txtDeathWhere_
            // 
            this.txtDeathWhere_.Location = new System.Drawing.Point(508, 48);
            this.txtDeathWhere_.Name = "txtDeathWhere_";
            this.txtDeathWhere_.Size = new System.Drawing.Size(44, 21);
            this.txtDeathWhere_.TabIndex = 12;
            this.txtDeathWhere_.Text = "Where";
            this.txtDeathWhere_.TextChanged += new System.EventHandler(this.evtAdditionalDeath_Changed);
            // 
            // txtDeathWhenReg_
            // 
            this.txtDeathWhenReg_.Location = new System.Drawing.Point(344, 248);
            this.txtDeathWhenReg_.Name = "txtDeathWhenReg_";
            this.txtDeathWhenReg_.Size = new System.Drawing.Size(208, 21);
            this.txtDeathWhenReg_.TabIndex = 30;
            this.txtDeathWhenReg_.Text = "textBox2";
            this.txtDeathWhenReg_.TextChanged += new System.EventHandler(this.evtAdditionalDeath_Changed);
            // 
            // txtDeathInformantDescription_
            // 
            this.txtDeathInformantDescription_.Location = new System.Drawing.Point(344, 224);
            this.txtDeathInformantDescription_.Name = "txtDeathInformantDescription_";
            this.txtDeathInformantDescription_.Size = new System.Drawing.Size(208, 21);
            this.txtDeathInformantDescription_.TabIndex = 28;
            this.txtDeathInformantDescription_.Text = "textBox2";
            this.txtDeathInformantDescription_.TextChanged += new System.EventHandler(this.evtAdditionalDeath_Changed);
            // 
            // txtDeathInformantAddress_
            // 
            this.txtDeathInformantAddress_.Location = new System.Drawing.Point(128, 248);
            this.txtDeathInformantAddress_.Name = "txtDeathInformantAddress_";
            this.txtDeathInformantAddress_.Size = new System.Drawing.Size(136, 21);
            this.txtDeathInformantAddress_.TabIndex = 25;
            this.txtDeathInformantAddress_.Text = "textBox1";
            this.txtDeathInformantAddress_.TextChanged += new System.EventHandler(this.evtAdditionalDeath_Changed);
            // 
            // txtDeathInformant_
            // 
            this.txtDeathInformant_.Location = new System.Drawing.Point(128, 224);
            this.txtDeathInformant_.Name = "txtDeathInformant_";
            this.txtDeathInformant_.Size = new System.Drawing.Size(136, 21);
            this.txtDeathInformant_.TabIndex = 24;
            this.txtDeathInformant_.Text = "textBox1";
            this.txtDeathInformant_.TextChanged += new System.EventHandler(this.evtAdditionalDeath_Changed);
            // 
            // txtDeathUsualAddress_
            // 
            this.txtDeathUsualAddress_.Location = new System.Drawing.Point(344, 120);
            this.txtDeathUsualAddress_.Name = "txtDeathUsualAddress_";
            this.txtDeathUsualAddress_.Size = new System.Drawing.Size(208, 21);
            this.txtDeathUsualAddress_.TabIndex = 19;
            this.txtDeathUsualAddress_.Text = "textBox2";
            this.txtDeathUsualAddress_.TextChanged += new System.EventHandler(this.evtAdditionalDeath_Changed);
            // 
            // txtDeathCause_
            // 
            this.txtDeathCause_.AcceptsReturn = true;
            this.txtDeathCause_.Location = new System.Drawing.Point(128, 144);
            this.txtDeathCause_.Multiline = true;
            this.txtDeathCause_.Name = "txtDeathCause_";
            this.txtDeathCause_.Size = new System.Drawing.Size(424, 72);
            this.txtDeathCause_.TabIndex = 18;
            this.txtDeathCause_.Text = "textBox1";
            this.txtDeathCause_.TextChanged += new System.EventHandler(this.evtAdditionalDeath_Changed);
            // 
            // txtDeathOccupation_
            // 
            this.txtDeathOccupation_.Location = new System.Drawing.Point(128, 120);
            this.txtDeathOccupation_.Name = "txtDeathOccupation_";
            this.txtDeathOccupation_.Size = new System.Drawing.Size(136, 21);
            this.txtDeathOccupation_.TabIndex = 17;
            this.txtDeathOccupation_.Text = "textBox2";
            this.txtDeathOccupation_.TextChanged += new System.EventHandler(this.evtAdditionalDeath_Changed);
            // 
            // txtDeathDatePlace_
            // 
            this.txtDeathDatePlace_.Location = new System.Drawing.Point(128, 96);
            this.txtDeathDatePlace_.Name = "txtDeathDatePlace_";
            this.txtDeathDatePlace_.Size = new System.Drawing.Size(424, 21);
            this.txtDeathDatePlace_.TabIndex = 16;
            this.txtDeathDatePlace_.Text = "m_txtDeathDatePlace";
            this.txtDeathDatePlace_.TextChanged += new System.EventHandler(this.evtAdditionalDeath_Changed);
            // 
            // txtDeathSex_
            // 
            this.txtDeathSex_.Location = new System.Drawing.Point(392, 72);
            this.txtDeathSex_.Name = "txtDeathSex_";
            this.txtDeathSex_.Size = new System.Drawing.Size(160, 21);
            this.txtDeathSex_.TabIndex = 15;
            this.txtDeathSex_.Text = "m_txtDeathSex";
            this.txtDeathSex_.TextChanged += new System.EventHandler(this.evtAdditionalDeath_Changed);
            // 
            // txtDeathName_
            // 
            this.txtDeathName_.Location = new System.Drawing.Point(128, 72);
            this.txtDeathName_.Name = "txtDeathName_";
            this.txtDeathName_.Size = new System.Drawing.Size(136, 21);
            this.txtDeathName_.TabIndex = 13;
            this.txtDeathName_.Text = "m_txtDeathName";
            this.txtDeathName_.TextChanged += new System.EventHandler(this.evtAdditionalDeath_Changed);
            // 
            // txtDeathWhen_
            // 
            this.txtDeathWhen_.Location = new System.Drawing.Point(128, 48);
            this.txtDeathWhen_.Name = "txtDeathWhen_";
            this.txtDeathWhen_.Size = new System.Drawing.Size(374, 21);
            this.txtDeathWhen_.TabIndex = 11;
            this.txtDeathWhen_.Text = "When";
            this.txtDeathWhen_.TextChanged += new System.EventHandler(this.evtAdditionalDeath_Changed);
            // 
            // txtDeathDistrict_
            // 
            this.txtDeathDistrict_.Location = new System.Drawing.Point(128, 24);
            this.txtDeathDistrict_.Name = "txtDeathDistrict_";
            this.txtDeathDistrict_.Size = new System.Drawing.Size(424, 21);
            this.txtDeathDistrict_.TabIndex = 8;
            this.txtDeathDistrict_.Text = "m_txtDeathDistrict";
            this.txtDeathDistrict_.TextChanged += new System.EventHandler(this.evtAdditionalDeath_Changed);
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
            this.m_panList.Controls.Add(this.lstSources_);
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
            this.cmdAddSource.Image = global::family_tree.viewer.Properties.Resources.add;
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
            this.cmdDeleteSource.Image = global::family_tree.viewer.Properties.Resources.delete;
            this.cmdDeleteSource.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cmdDeleteSource.Location = new System.Drawing.Point(110, 464);
            this.cmdDeleteSource.Name = "cmdDeleteSource";
            this.cmdDeleteSource.Size = new System.Drawing.Size(100, 30);
            this.cmdDeleteSource.TabIndex = 15;
            this.cmdDeleteSource.Text = "Delete";
            this.cmdDeleteSource.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdDeleteSource.Click += new System.EventHandler(this.cmdDeleteSource_Click);
            // 
            // EditSourcesDialog
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(984, 617);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.m_panList);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.gridReferences_);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "EditSourcesDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Sources";
            this.Shown += new System.EventHandler(this.frmEditSources_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.gridReferences_)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.grpFreeTable_.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSourceFreeTable_)).EndInit();
            this.panel2.ResumeLayout(false);
            this.grpBirth_.ResumeLayout(false);
            this.grpBirth_.PerformLayout();
            this.grpMarriage_.ResumeLayout(false);
            this.grpMarriage_.PerformLayout();
            this.grpCensus_.ResumeLayout(false);
            this.grpCensus_.PerformLayout();
            this.grpDeath_.ResumeLayout(false);
            this.grpDeath_.PerformLayout();
            this.m_panList.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.ImageList imageList16x16;
        private System.Windows.Forms.ListBox lstSources_;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.DataGrid gridReferences_;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button cmdDeleteSource;
        private System.Windows.Forms.Button cmdAddSource;
        private System.Windows.Forms.TextBox txtDescription_;
        private System.Windows.Forms.ComboBox cboPrefix_;
        private System.Windows.Forms.TextBox txtComments_;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.RadioButton radioAlpha;
        private System.Windows.Forms.RadioButton radioDate;
        private family_tree.viewer.CompoundDateEditBox dateSourceDate_;
        private System.Windows.Forms.Panel m_panList;
        private System.Windows.Forms.ComboBox cboAdditionalInfo_;
        private System.Windows.Forms.GroupBox grpCensus_;
        private System.Windows.Forms.GroupBox grpMarriage_;
        private System.Windows.Forms.GroupBox grpBirth_;
        private System.Windows.Forms.GroupBox grpDeath_;
        private System.Windows.Forms.TextBox txtCensusAddress_;
        private System.Windows.Forms.TextBox txtMarrGroom_;
        private System.Windows.Forms.TextBox txtMarrGroomAge_;
        private System.Windows.Forms.TextBox txtMarrGroomOccu_;
        private System.Windows.Forms.TextBox txtMarrGroomLoca_;
        private System.Windows.Forms.TextBox txtMarrGroomFather_;
        private System.Windows.Forms.TextBox txtMarrGroomFatherOcc_;
        private System.Windows.Forms.TextBox txtMarrBrideLoca_;
        private System.Windows.Forms.TextBox txtMarrBrideOccu_;
        private System.Windows.Forms.TextBox txtMarrBrideAge_;
        private System.Windows.Forms.TextBox txtMarrBride_;
        private System.Windows.Forms.TextBox txtMarrBrideFatherOcc_;
        private System.Windows.Forms.TextBox txtMarrBrideFather_;
        private System.Windows.Forms.TextBox txtMarrLocation_;
        private System.Windows.Forms.TextBox txtMarrWitness_;
        private System.Windows.Forms.TextBox txtBirthDistrict_;
        private System.Windows.Forms.TextBox txtBirthName_;
        private System.Windows.Forms.TextBox txtBirthSex_;
        private System.Windows.Forms.TextBox txtBirthFather_;
        private System.Windows.Forms.TextBox txtBirthMother_;
        private System.Windows.Forms.TextBox txtBirthInformant_;
        private System.Windows.Forms.TextBox txtBirthWhenReg_;
        private System.Windows.Forms.TextBox txtBirthWhenWhere_;
        private System.Windows.Forms.TextBox txtBirthFatherOccupation_;
        private System.Windows.Forms.TextBox txtDeathSex_;
        private System.Windows.Forms.TextBox txtDeathName_;
        private System.Windows.Forms.TextBox txtDeathWhen_;
        private System.Windows.Forms.TextBox txtDeathDistrict_;
        private System.Windows.Forms.TextBox txtDeathDatePlace_;
        private System.Windows.Forms.TextBox txtDeathOccupation_;
        private System.Windows.Forms.TextBox txtDeathCause_;
        private System.Windows.Forms.TextBox txtDeathInformant_;
        private System.Windows.Forms.TextBox txtDeathInformantAddress_;
        private System.Windows.Forms.TextBox txtDeathWhenReg_;
        private System.Windows.Forms.TextBox txtDeathUsualAddress_;
        private System.Windows.Forms.TextBox txtDeathInformantDescription_;
        private System.Windows.Forms.ComboBox cboRepository_;
        private TextBox txtDeathWhere_;
        private TextBox txtCensusPiece_;
        private TextBox txtCensusSeries_;
        private TextBox txtCensusPage_;
        private TextBox txtCensusFolio_;
        private DateTimePicker dtpBirthWhen_;
        private TextBox txtBirthInformantAddress_;
        private TextBox txtBirthMotherDetails_;
        private DateTimePicker dtpMarrWhen_;
        private TextBox txtMarrGro_;
        private TextBox txtBirthReference_;
        private TextBox txtDeathReference_;
        private ToolTip toolTip1;
        private GroupBox grpFreeTable_;
        private DataGridView dataGridViewSourceFreeTable_;
        private Panel panel2;
    }
}
