using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using FamilyTree.Objects;

namespace FamilyTree.Viewer
{
    /// <summary>Class to represent the edit census dialog.</summary>
    public class EditCensusDialog : System.Windows.Forms.Form
    {
        /// <summary>Connection to the database.</summary>
        private Database database_;

        private System.Windows.Forms.ComboBox cboYear_;
        private System.Windows.Forms.ComboBox cboAddress_;
        private System.Windows.Forms.ImageList imageList16x16;
        private System.Windows.Forms.Button m_cmdCreate;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.DataGrid peopleGrid_;
        private System.Windows.Forms.GroupBox m_grpPerson;
        private System.Windows.Forms.ComboBox cboPerson_;
        private Button m_cmdRemovePerson;
        private Button m_cmdAddPerson;
        private System.ComponentModel.IContainer components;



        /// <summary>Class constructor.  Estabishes the connection to the specified database.</summary>
        /// <param name="database">Specifies the database to read the census data from.</param>
        /// <param name="initialRecord">Specifies the ID of the initial record to display.  Or 0 for nothing.</param>	
        public EditCensusDialog(Database database, int initialRecord)
        {
            // Required for Windows Form Designer support.
            InitializeComponent();

            // Initialise member variables.
            database_ = database;

            // Intialise the grid to display CensusPerson objects.
            // Create a DataGridTabkeStyle object for the facts.
            DataGridTableStyle membersTable = new DataGridTableStyle();
            membersTable.MappingName = "CensusPerson[]";

            // Sets the AlternatingBackColor so you can see the difference.
            membersTable.AlternatingBackColor = System.Drawing.Color.LightBlue;

            // Creates a column for the person.
            DataGridTextBoxColumn columnPerson = new DataGridTextBoxColumn();
            columnPerson.MappingName = "PersonName";
            columnPerson.HeaderText = "DB Name";
            columnPerson.ReadOnly = true;
            columnPerson.Width = 150;

            DataGridTextBoxColumn columnName = new DataGridTextBoxColumn();
            columnName.MappingName = "CensusName";
            columnName.HeaderText = "Entered Name";
            columnName.Width = 150;

            // Create a column for the Relation.
            DataGridTextBoxColumn columnRelation = new DataGridTextBoxColumn();
            columnRelation.MappingName = "RelationToHead";
            columnRelation.HeaderText = "Relation";
            columnRelation.Width = 50;

            // Create a column for the age.
            DataGridTextBoxColumn columnAge = new DataGridTextBoxColumn();
            columnAge.MappingName = "Age";
            columnAge.HeaderText = "Age";
            columnAge.Width = 50;

            // Create a column for the Occupation.
            DataGridTextBoxColumn columnOccupation = new DataGridTextBoxColumn();
            columnOccupation.MappingName = "Occupation";
            columnOccupation.HeaderText = "Occupation";
            columnOccupation.Width = 100;

            // Create a column for the BornLocation.
            DataGridTextBoxColumn columnBorn = new DataGridTextBoxColumn();
            columnBorn.MappingName = "BornLocation";
            columnBorn.HeaderText = "Born";
            columnBorn.Width = 150;

            // Adds the column styles to the grid table style.
            membersTable.GridColumnStyles.Add(columnPerson);
            membersTable.GridColumnStyles.Add(columnName);
            membersTable.GridColumnStyles.Add(columnRelation);
            membersTable.GridColumnStyles.Add(columnAge);
            membersTable.GridColumnStyles.Add(columnOccupation);
            membersTable.GridColumnStyles.Add(columnBorn);

            // Add the table style to the collection, but clear the collection first.
            peopleGrid_.TableStyles.Clear();
            peopleGrid_.TableStyles.Add(membersTable);

            // Move to the initial record if specified.
            if (initialRecord != 0)
            {
                // Find the initial object.
                Census census = new Census(initialRecord, database_);

                // Move to the specified year.
                string theYear = census.censusDate.Year.ToString();
                foreach (string sItem in cboYear_.Items)
                {
                    if (sItem == theYear)
                    {
                        this.cboYear_.SelectedItem = sItem;
                    }
                }

                // Show the options in the address combo box.
                foreach (IndexName address in cboAddress_.Items)
                {
                    if (address.index == initialRecord)
                    {
                        cboAddress_.SelectedItem = address;
                    }
                }
            }
        }



        /// <summary>Clean up any resources being used.</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }



        private bool save()
        {
            CensusPerson[] censusPeople = (CensusPerson[])peopleGrid_.DataSource;
            foreach (CensusPerson censusPerson in censusPeople)
            {
                censusPerson.save(database_);
            }

            // Return success.
            return true;
        }



        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Button cmdOK;
            System.Windows.Forms.Button cmdCancel;
            System.Windows.Forms.Button cmdSave;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditCensusDialog));
            System.Windows.Forms.Label label3;
            this.cboYear_ = new System.Windows.Forms.ComboBox();
            this.cboAddress_ = new System.Windows.Forms.ComboBox();
            this.imageList16x16 = new System.Windows.Forms.ImageList(this.components);
            this.peopleGrid_ = new System.Windows.Forms.DataGrid();
            this.m_cmdCreate = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.m_grpPerson = new System.Windows.Forms.GroupBox();
            this.m_cmdRemovePerson = new System.Windows.Forms.Button();
            this.m_cmdAddPerson = new System.Windows.Forms.Button();
            this.cboPerson_ = new System.Windows.Forms.ComboBox();
            cmdOK = new System.Windows.Forms.Button();
            cmdCancel = new System.Windows.Forms.Button();
            cmdSave = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.peopleGrid_)).BeginInit();
            this.m_grpPerson.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            cmdOK.Image = global::FamilyTree.Viewer.Properties.Resources.OK;
            cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            cmdOK.Location = new System.Drawing.Point(629, 353);
            cmdOK.Name = "cmdOK";
            cmdOK.Size = new System.Drawing.Size(100, 30);
            cmdOK.TabIndex = 4;
            cmdOK.Text = "OK";
            cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmdCancel
            // 
            cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cmdCancel.Image = global::FamilyTree.Viewer.Properties.Resources.Cancel;
            cmdCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            cmdCancel.Location = new System.Drawing.Point(523, 353);
            cmdCancel.Name = "cmdCancel";
            cmdCancel.Size = new System.Drawing.Size(100, 30);
            cmdCancel.TabIndex = 5;
            cmdCancel.Text = "Cancel";
            cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmdSave
            // 
            cmdSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            cmdSave.Image = global::FamilyTree.Viewer.Properties.Resources.Save;
            cmdSave.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            cmdSave.Location = new System.Drawing.Point(613, 234);
            cmdSave.Name = "cmdSave";
            cmdSave.Size = new System.Drawing.Size(100, 30);
            cmdSave.TabIndex = 10;
            cmdSave.Text = "Save";
            cmdSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // m_cboYear
            // 
            this.cboYear_.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboYear_.Items.AddRange(new object[] {
            "1911",
            "1901",
            "1891",
            "1881",
            "1871",
            "1861",
            "1851",
            "1841"});
            this.cboYear_.Location = new System.Drawing.Point(48, 32);
            this.cboYear_.Name = "m_cboYear";
            this.cboYear_.Size = new System.Drawing.Size(80, 21);
            this.cboYear_.TabIndex = 0;
            this.cboYear_.SelectedIndexChanged += new System.EventHandler(this.cboYear_SelectedIndexChanged);
            // 
            // label1
            // 
            label1.Location = new System.Drawing.Point(8, 32);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(40, 23);
            label1.TabIndex = 1;
            label1.Text = "Year:";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            label2.Location = new System.Drawing.Point(144, 32);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(56, 23);
            label2.TabIndex = 2;
            label2.Text = "Address:";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_cboAddress
            // 
            this.cboAddress_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboAddress_.Location = new System.Drawing.Point(200, 32);
            this.cboAddress_.Name = "m_cboAddress";
            this.cboAddress_.Size = new System.Drawing.Size(423, 21);
            this.cboAddress_.TabIndex = 3;
            this.cboAddress_.Text = "comboBox1";
            this.cboAddress_.TextChanged += new System.EventHandler(this.cboAddress_TextChanged);
            // 
            // imageList16x16
            // 
            this.imageList16x16.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList16x16.ImageStream")));
            this.imageList16x16.TransparentColor = System.Drawing.Color.Silver;
            this.imageList16x16.Images.SetKeyName(0, "");
            this.imageList16x16.Images.SetKeyName(1, "");
            this.imageList16x16.Images.SetKeyName(2, "");
            this.imageList16x16.Images.SetKeyName(3, "");
            this.imageList16x16.Images.SetKeyName(4, "");
            this.imageList16x16.Images.SetKeyName(5, "");
            this.imageList16x16.Images.SetKeyName(6, "");
            this.imageList16x16.Images.SetKeyName(7, "");
            // 
            // m_PeopleGrid
            // 
            this.peopleGrid_.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.peopleGrid_.DataMember = "";
            this.peopleGrid_.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.peopleGrid_.Location = new System.Drawing.Point(8, 26);
            this.peopleGrid_.Name = "m_PeopleGrid";
            this.peopleGrid_.Size = new System.Drawing.Size(705, 202);
            this.peopleGrid_.TabIndex = 6;
            // 
            // m_cmdCreate
            // 
            this.m_cmdCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_cmdCreate.Enabled = false;
            this.m_cmdCreate.Image = global::FamilyTree.Viewer.Properties.Resources.add;
            this.m_cmdCreate.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.m_cmdCreate.Location = new System.Drawing.Point(629, 28);
            this.m_cmdCreate.Name = "m_cmdCreate";
            this.m_cmdCreate.Size = new System.Drawing.Size(100, 30);
            this.m_cmdCreate.TabIndex = 7;
            this.m_cmdCreate.Text = "Create";
            this.m_cmdCreate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Location = new System.Drawing.Point(70, 359);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(248, 21);
            this.comboBox1.TabIndex = 8;
            // 
            // label3
            // 
            label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            label3.Location = new System.Drawing.Point(13, 357);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(56, 23);
            label3.TabIndex = 9;
            label3.Text = "Source:";
            label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_grpPerson
            // 
            this.m_grpPerson.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_grpPerson.Controls.Add(cmdSave);
            this.m_grpPerson.Controls.Add(this.m_cmdRemovePerson);
            this.m_grpPerson.Controls.Add(this.m_cmdAddPerson);
            this.m_grpPerson.Controls.Add(this.cboPerson_);
            this.m_grpPerson.Controls.Add(this.peopleGrid_);
            this.m_grpPerson.Location = new System.Drawing.Point(8, 64);
            this.m_grpPerson.Name = "m_grpPerson";
            this.m_grpPerson.Size = new System.Drawing.Size(721, 268);
            this.m_grpPerson.TabIndex = 10;
            this.m_grpPerson.TabStop = false;
            this.m_grpPerson.Text = "Household Members";
            // 
            // m_cmdRemovePerson
            // 
            this.m_cmdRemovePerson.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_cmdRemovePerson.Enabled = false;
            this.m_cmdRemovePerson.Image = global::FamilyTree.Viewer.Properties.Resources.delete;
            this.m_cmdRemovePerson.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.m_cmdRemovePerson.Location = new System.Drawing.Point(402, 234);
            this.m_cmdRemovePerson.Name = "m_cmdRemovePerson";
            this.m_cmdRemovePerson.Size = new System.Drawing.Size(100, 30);
            this.m_cmdRemovePerson.TabIndex = 9;
            this.m_cmdRemovePerson.Text = "Delete";
            this.m_cmdRemovePerson.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.m_cmdRemovePerson.Click += new System.EventHandler(this.cmdRemovePerson_Click);
            // 
            // m_cmdAddPerson
            // 
            this.m_cmdAddPerson.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_cmdAddPerson.Enabled = false;
            this.m_cmdAddPerson.Image = global::FamilyTree.Viewer.Properties.Resources.add;
            this.m_cmdAddPerson.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.m_cmdAddPerson.Location = new System.Drawing.Point(296, 234);
            this.m_cmdAddPerson.Name = "m_cmdAddPerson";
            this.m_cmdAddPerson.Size = new System.Drawing.Size(100, 30);
            this.m_cmdAddPerson.TabIndex = 8;
            this.m_cmdAddPerson.Text = "Add";
            this.m_cmdAddPerson.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.m_cmdAddPerson.Click += new System.EventHandler(this.cmdAddPerson_Click);
            // 
            // m_cboPerson
            // 
            this.cboPerson_.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboPerson_.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPerson_.Location = new System.Drawing.Point(8, 236);
            this.cboPerson_.Name = "m_cboPerson";
            this.cboPerson_.Size = new System.Drawing.Size(280, 21);
            this.cboPerson_.TabIndex = 0;
            // 
            // frmEditCensus
            // 
            this.AcceptButton = cmdOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.CancelButton = cmdCancel;
            this.ClientSize = new System.Drawing.Size(737, 392);
            this.Controls.Add(this.m_grpPerson);
            this.Controls.Add(label3);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.m_cmdCreate);
            this.Controls.Add(cmdCancel);
            this.Controls.Add(cmdOK);
            this.Controls.Add(this.cboAddress_);
            this.Controls.Add(label2);
            this.Controls.Add(label1);
            this.Controls.Add(this.cboYear_);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmEditCensus";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Census";
            ((System.ComponentModel.ISupportInitialize)(this.peopleGrid_)).EndInit();
            this.m_grpPerson.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion



        /// <summary>Message handler for the year combo value changing.</summary>
        private void cboYear_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // Find the selected year.
            int theYear = int.Parse(cboYear_.Text);

            IndexName[] houseHolds = database_.cenusGetHouseholds(theYear);

            // Add the available options to the address combo.
            cboAddress_.Items.Clear();
            foreach (IndexName houseHold in houseHolds)
            {
                cboAddress_.Items.Add(houseHold);
            }

            // Populate the list of people combo.
            IndexName[] people = database_.getPeople(ChooseSex.EITHER, Objects.SortOrder.ALPHABETICAL, theYear);
            cboPerson_.Items.Clear();
            foreach (IndexName person in people)
            {
                cboPerson_.Items.Add(person);
            }
        }



        /// <summary>
        /// Message handler for the text in the household address combo changing.
        /// If the text if the from the combo box then display the contents of the existing census household.
        /// Otherwise, enable the creation of a new census household.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboAddress_TextChanged(object sender, System.EventArgs e)
        {
            if (cboAddress_.SelectedIndex >= 0)
            {
                // Disable the creation of new households.  This one already exists.
                m_cmdCreate.Enabled = false;

                cboPerson_.Enabled = true;
                m_cmdAddPerson.Enabled = true;
                m_cmdRemovePerson.Enabled = true;

                // Find the ID of the household
                IndexName oAddress = (IndexName)cboAddress_.SelectedItem;
                int nHouseholdID = oAddress.index;

                // Display the members of this household
                peopleGrid_.SetDataBinding(database_.censusHouseholdMembers(nHouseholdID), "");
            }
            else
            {
                // Disable the editing of the people.  This household does not exist yet.
                m_cmdCreate.Enabled = true;

                cboPerson_.Enabled = false;
                m_cmdAddPerson.Enabled = false;
                m_cmdRemovePerson.Enabled = false;

            }
        }

        /// <summary>
        /// Message handler for the save button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdSave_Click(object sender, System.EventArgs e)
        {
            save();
        }

        /// <summary>
        /// Message handler for the add person to the census button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdAddPerson_Click(object sender, System.EventArgs e)
        {
            // Find the ID of the person selected
            if (cboPerson_.SelectedItem == null)
            {
                // No one is selected
                return;
            }
            IndexName oLookup = (IndexName)cboPerson_.SelectedItem;

            // Find the ID of the current household
            if (cboAddress_.SelectedIndex < 0)
            {
                // No Household is selected
                return;
            }
            IndexName oHousehold = (IndexName)cboAddress_.SelectedItem;

            // Load the person to get his name
            Person oPerson = new Person(oLookup.index, database_);

            // Create a new object to add to the list box
            CensusPerson oMember = new CensusPerson();
            oMember.index = 0;
            oMember.houseHoldIndex = oHousehold.index;
            oMember.personIndex = oPerson.index;
            oMember.personName = oPerson.getName(true, true);
            oMember.censusName = oPerson.getName(false, true);

            oMember.save(database_);

            // Display the members of this household
            peopleGrid_.SetDataBinding(database_.censusHouseholdMembers(oHousehold.index), "");
        }

        private void cmdRemovePerson_Click(object sender, System.EventArgs e)
        {
            // Check that a person is selected in the grid
            if (peopleGrid_.CurrentCell.RowNumber < 0)
            {
                return;
            }

            // Find the fact
            CensusPerson oMember = ((CensusPerson[])peopleGrid_.DataSource)[peopleGrid_.CurrentCell.RowNumber];
            int nHouseholdID = oMember.houseHoldIndex;
            oMember.delete();
            save();

            // Display the members of this household
            peopleGrid_.SetDataBinding(database_.censusHouseholdMembers(nHouseholdID), "");
        }
    }
}
