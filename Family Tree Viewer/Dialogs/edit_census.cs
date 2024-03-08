using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using family_tree.objects;

namespace family_tree.viewer
{
    /// <summary>Class to represent the edit census dialog.</summary>
    public class EditCensusDialog : System.Windows.Forms.Form
    {
        /// <summary>Connection to the database.</summary>
        private Database database_;

        private System.Windows.Forms.ComboBox cboYear_;
        private System.Windows.Forms.ComboBox cboAddress_;
        private System.Windows.Forms.Button cmdCreate_;
        private System.Windows.Forms.DataGrid peopleGrid_;
        private System.Windows.Forms.ComboBox cboPerson_;
        private Button cmdRemovePerson_;
        private Button cmdAddPerson_;
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

            // Move to the initial record if specified.
            int theYear = 0;
            if (initialRecord != 0)
            {
                // Find the initial object.
                Census census = new Census(initialRecord, database_);

                // Move to the specified year.
                theYear = census.censusDate.Year;
                string yearName = theYear.ToString();
                foreach (string item in cboYear_.Items)
                {
                    if (item == yearName)
                    {
                        cboYear_.SelectedItem = item;
                    }
                }

                // Show the options in the address combo box.
                foreach (IdxName address in cboAddress_.Items)
                {
                    if (address.idx == initialRecord)
                    {
                        cboAddress_.SelectedItem = address;
                    }
                }
            }

            // Intialise the grid to display CensusPerson objects.
            // Create a DataGridTabkeStyle object for the facts.
            DataGridTableStyle membersTable = new DataGridTableStyle();
            membersTable.MappingName = "CensusPerson[]";

            // Sets the AlternatingBackColor so you can see the difference.
            membersTable.AlternatingBackColor = System.Drawing.Color.LightBlue;

            // Creates a column for the person.
            DataGridTextBoxColumn columnPerson = new DataGridTextBoxColumn();
            columnPerson.MappingName = "personName";
            columnPerson.HeaderText = "DB Name";
            columnPerson.ReadOnly = true;
            columnPerson.Width = 150;

            DataGridTextBoxColumn columnName = new DataGridTextBoxColumn();
            columnName.MappingName = "censusName";
            columnName.HeaderText = "Entered Name";
            columnName.Width = 150;

            // Create a column for the relation.
            DataGridTextBoxColumn columnRelation = new DataGridTextBoxColumn();
            columnRelation.MappingName = "relationToHead";
            columnRelation.HeaderText = "Relation";
            columnRelation.Width = theYear == 1939 ? 10 : 50;

            // Create a column for the age.
            DataGridTextBoxColumn columnAge = new DataGridTextBoxColumn();
            columnAge.MappingName = "age";
            columnAge.HeaderText = "Age";
            columnAge.Width = theYear == 1939 ? 10 : 50;

            // Create a column for the date of birth.
            DataGridTextBoxColumn columnDoB = new DataGridTextBoxColumn();
            columnDoB.MappingName = "dateOfBirth";
            columnDoB.HeaderText = "DoB";
            columnDoB.Width = theYear == 1939 ? 70 : 10;

            // Create a column for the sex.
            DataGridTextBoxColumn columnSex = new DataGridTextBoxColumn();
            columnSex.MappingName = "sex";
            columnSex.HeaderText = "Sex";
            columnSex.Width = theYear == 1939 ? 50 : 10;

            // Create a column for the occupation.
            DataGridTextBoxColumn columnOccupation = new DataGridTextBoxColumn();
            columnOccupation.MappingName = "occupation";
            columnOccupation.HeaderText = "Occupation";
            columnOccupation.Width = 100;

            // Create a column for the born location.
            DataGridTextBoxColumn columnBorn = new DataGridTextBoxColumn();
            columnBorn.MappingName = "BornLocation";
            columnBorn.HeaderText = "Born Location";
            columnBorn.Width = theYear == 1939 ? 10 : 150;

            // Create a column for the martial status.
            DataGridTextBoxColumn columnMaritalStatus = new DataGridTextBoxColumn();
            columnMaritalStatus.MappingName = "maritalStatus";
            columnMaritalStatus.HeaderText = "Marital Status";
            columnMaritalStatus.Width = theYear == 1939 ? 50 : 10;

            // Adds the column styles to the grid table style.
            membersTable.GridColumnStyles.Add(columnPerson);
            membersTable.GridColumnStyles.Add(columnName);
            membersTable.GridColumnStyles.Add(columnRelation);
            membersTable.GridColumnStyles.Add(columnAge);
            membersTable.GridColumnStyles.Add(columnDoB);
            membersTable.GridColumnStyles.Add(columnSex);
            membersTable.GridColumnStyles.Add(columnOccupation);
            membersTable.GridColumnStyles.Add(columnBorn);
            membersTable.GridColumnStyles.Add(columnMaritalStatus);

            // Add the table style to the collection, but clear the collection first.
            peopleGrid_.TableStyles.Clear();
            peopleGrid_.TableStyles.Add(membersTable);
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
            System.Windows.Forms.Label label3;
            System.Windows.Forms.ImageList imageList16x16;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditCensusDialog));
            System.Windows.Forms.ComboBox cboSource;
            System.Windows.Forms.GroupBox grpPerson;
            this.cmdRemovePerson_ = new System.Windows.Forms.Button();
            this.cmdAddPerson_ = new System.Windows.Forms.Button();
            this.cboPerson_ = new System.Windows.Forms.ComboBox();
            this.peopleGrid_ = new System.Windows.Forms.DataGrid();
            this.cboYear_ = new System.Windows.Forms.ComboBox();
            this.cboAddress_ = new System.Windows.Forms.ComboBox();
            this.cmdCreate_ = new System.Windows.Forms.Button();
            cmdOK = new System.Windows.Forms.Button();
            cmdCancel = new System.Windows.Forms.Button();
            cmdSave = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            imageList16x16 = new System.Windows.Forms.ImageList(this.components);
            cboSource = new System.Windows.Forms.ComboBox();
            grpPerson = new System.Windows.Forms.GroupBox();
            grpPerson.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.peopleGrid_)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            cmdOK.Image = global::family_tree.viewer.Properties.Resources.OK;
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
            cmdCancel.Image = global::family_tree.viewer.Properties.Resources.Cancel;
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
            cmdSave.Image = global::family_tree.viewer.Properties.Resources.Save;
            cmdSave.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            cmdSave.Location = new System.Drawing.Point(613, 234);
            cmdSave.Name = "cmdSave";
            cmdSave.Size = new System.Drawing.Size(100, 30);
            cmdSave.TabIndex = 10;
            cmdSave.Text = "Save";
            cmdSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
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
            // imageList16x16
            // 
            imageList16x16.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList16x16.ImageStream")));
            imageList16x16.TransparentColor = System.Drawing.Color.Silver;
            imageList16x16.Images.SetKeyName(0, "");
            imageList16x16.Images.SetKeyName(1, "");
            imageList16x16.Images.SetKeyName(2, "");
            imageList16x16.Images.SetKeyName(3, "");
            imageList16x16.Images.SetKeyName(4, "");
            imageList16x16.Images.SetKeyName(5, "");
            imageList16x16.Images.SetKeyName(6, "");
            imageList16x16.Images.SetKeyName(7, "");
            // 
            // cboSource
            // 
            cboSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            cboSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboSource.Location = new System.Drawing.Point(70, 359);
            cboSource.Name = "cboSource";
            cboSource.Size = new System.Drawing.Size(248, 21);
            cboSource.TabIndex = 8;
            // 
            // grpPerson
            // 
            grpPerson.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            grpPerson.Controls.Add(cmdSave);
            grpPerson.Controls.Add(this.cmdRemovePerson_);
            grpPerson.Controls.Add(this.cmdAddPerson_);
            grpPerson.Controls.Add(this.cboPerson_);
            grpPerson.Controls.Add(this.peopleGrid_);
            grpPerson.Location = new System.Drawing.Point(8, 64);
            grpPerson.Name = "grpPerson";
            grpPerson.Size = new System.Drawing.Size(721, 268);
            grpPerson.TabIndex = 10;
            grpPerson.TabStop = false;
            grpPerson.Text = "Household Members";
            // 
            // cmdRemovePerson_
            // 
            this.cmdRemovePerson_.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdRemovePerson_.Enabled = false;
            this.cmdRemovePerson_.Image = global::family_tree.viewer.Properties.Resources.delete;
            this.cmdRemovePerson_.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cmdRemovePerson_.Location = new System.Drawing.Point(402, 234);
            this.cmdRemovePerson_.Name = "cmdRemovePerson_";
            this.cmdRemovePerson_.Size = new System.Drawing.Size(100, 30);
            this.cmdRemovePerson_.TabIndex = 9;
            this.cmdRemovePerson_.Text = "Delete";
            this.cmdRemovePerson_.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdRemovePerson_.Click += new System.EventHandler(this.cmdRemovePerson_Click);
            // 
            // cmdAddPerson_
            // 
            this.cmdAddPerson_.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdAddPerson_.Enabled = false;
            this.cmdAddPerson_.Image = global::family_tree.viewer.Properties.Resources.add;
            this.cmdAddPerson_.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cmdAddPerson_.Location = new System.Drawing.Point(296, 234);
            this.cmdAddPerson_.Name = "cmdAddPerson_";
            this.cmdAddPerson_.Size = new System.Drawing.Size(100, 30);
            this.cmdAddPerson_.TabIndex = 8;
            this.cmdAddPerson_.Text = "Add";
            this.cmdAddPerson_.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdAddPerson_.Click += new System.EventHandler(this.cmdAddPerson_Click);
            // 
            // cboPerson_
            // 
            this.cboPerson_.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboPerson_.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPerson_.Location = new System.Drawing.Point(8, 236);
            this.cboPerson_.Name = "cboPerson_";
            this.cboPerson_.Size = new System.Drawing.Size(280, 21);
            this.cboPerson_.TabIndex = 0;
            // 
            // peopleGrid_
            // 
            this.peopleGrid_.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.peopleGrid_.DataMember = "";
            this.peopleGrid_.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.peopleGrid_.Location = new System.Drawing.Point(8, 26);
            this.peopleGrid_.Name = "peopleGrid_";
            this.peopleGrid_.Size = new System.Drawing.Size(705, 202);
            this.peopleGrid_.TabIndex = 6;
            // 
            // cboYear_
            // 
            this.cboYear_.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboYear_.Items.AddRange(new object[] {
            "1939",
            "1921",
            "1911",
            "1901",
            "1891",
            "1881",
            "1871",
            "1861",
            "1851",
            "1841"});
            this.cboYear_.Location = new System.Drawing.Point(48, 32);
            this.cboYear_.Name = "cboYear_";
            this.cboYear_.Size = new System.Drawing.Size(80, 21);
            this.cboYear_.TabIndex = 0;
            this.cboYear_.SelectedIndexChanged += new System.EventHandler(this.cboYear_SelectedIndexChanged);
            // 
            // cboAddress_
            // 
            this.cboAddress_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboAddress_.Location = new System.Drawing.Point(200, 32);
            this.cboAddress_.Name = "cboAddress_";
            this.cboAddress_.Size = new System.Drawing.Size(423, 21);
            this.cboAddress_.TabIndex = 3;
            this.cboAddress_.Text = "comboBox1";
            this.cboAddress_.TextChanged += new System.EventHandler(this.cboAddress_TextChanged);
            // 
            // cmdCreate_
            // 
            this.cmdCreate_.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCreate_.Enabled = false;
            this.cmdCreate_.Image = global::family_tree.viewer.Properties.Resources.add;
            this.cmdCreate_.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cmdCreate_.Location = new System.Drawing.Point(629, 28);
            this.cmdCreate_.Name = "cmdCreate_";
            this.cmdCreate_.Size = new System.Drawing.Size(100, 30);
            this.cmdCreate_.TabIndex = 7;
            this.cmdCreate_.Text = "Create";
            this.cmdCreate_.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // EditCensusDialog
            // 
            this.AcceptButton = cmdOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.CancelButton = cmdCancel;
            this.ClientSize = new System.Drawing.Size(737, 392);
            this.Controls.Add(grpPerson);
            this.Controls.Add(label3);
            this.Controls.Add(cboSource);
            this.Controls.Add(this.cmdCreate_);
            this.Controls.Add(cmdCancel);
            this.Controls.Add(cmdOK);
            this.Controls.Add(this.cboAddress_);
            this.Controls.Add(label2);
            this.Controls.Add(label1);
            this.Controls.Add(this.cboYear_);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "EditCensusDialog";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Census";
            grpPerson.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.peopleGrid_)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion



        /// <summary>Message handler for the year combo value changing.</summary>
        private void cboYear_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // Find the selected year.
            int theYear = int.Parse(cboYear_.Text);

            IdxName[] houseHolds = database_.cenusGetHouseholds(theYear);

            // Add the available options to the address combo.
            cboAddress_.Items.Clear();
            foreach (IdxName houseHold in houseHolds)
            {
                cboAddress_.Items.Add(houseHold);
            }

            // Populate the list of people combo.
            IdxName[] people = database_.getPeople(ChooseSex.EITHER, family_tree.objects.SortOrder.ALPHABETICAL, theYear);
            cboPerson_.Items.Clear();
            foreach (IdxName person in people)
            {
                cboPerson_.Items.Add(person);
            }
        }



        /// <summary>Message handler for the text in the household address combo changing.  If the text if the from the combo box then display the contents of the existing census household.  Otherwise, enable the creation of a new census household.</summary>
        private void cboAddress_TextChanged(object sender, System.EventArgs e)
        {
            if (cboAddress_.SelectedIndex >= 0)
            {
                // Disable the creation of new households.  This one already exists.
                cmdCreate_.Enabled = false;

                cboPerson_.Enabled = true;
                cmdAddPerson_.Enabled = true;
                cmdRemovePerson_.Enabled = true;

                // Find the ID of the household.
                IdxName address = (IdxName)cboAddress_.SelectedItem;
                int householdIndex = address.idx;

                // Display the members of this household.
                peopleGrid_.SetDataBinding(database_.censusHouseholdMembers(householdIndex), "");
            }
            else
            {
                // Disable the editing of the people.  This household does not exist yet.
                cmdCreate_.Enabled = true;

                cboPerson_.Enabled = false;
                cmdAddPerson_.Enabled = false;
                cmdRemovePerson_.Enabled = false;

            }
        }



        /// <summary>Message handler for the save button.</summary>
        private void cmdSave_Click(object sender, System.EventArgs e)
        {
            save();
        }



        /// <summary>Message handler for the add person to the census button.</summary>
        private void cmdAddPerson_Click(object sender, System.EventArgs e)
        {
            // Find the ID of the person selected
            if (cboPerson_.SelectedItem == null)
            {
                // No one is selected.
                return;
            }
            IdxName selectedPerson = (IdxName)cboPerson_.SelectedItem;

            // Find the ID of the current household.
            if (cboAddress_.SelectedIndex < 0)
            {
                // No Household is selected
                return;
            }
            IdxName household = (IdxName)cboAddress_.SelectedItem;

            // Load the person to get his name.
            Person person = new Person(selectedPerson.idx, database_);

            // Create a new object to add to the list box.
            CensusPerson censusPerson = new CensusPerson();
            censusPerson.idx = 0;
            censusPerson.houseHoldIdx = household.idx;
            censusPerson.personIdx = person.idx;
            censusPerson.personName = person.getName(true, true);
            censusPerson.censusName = person.getName(false, true);

            censusPerson.save(database_);

            // Display the members of this household.
            peopleGrid_.SetDataBinding(database_.censusHouseholdMembers(household.idx), "");
        }


        /// <summary>Signal handler for the remove person button click.</summary>
        private void cmdRemovePerson_Click(object sender, System.EventArgs e)
        {
            // Check that a person is selected in the grid
            if (peopleGrid_.CurrentCell.RowNumber < 0)
            {
                return;
            }

            // Find the fact
            CensusPerson censusPerson = ((CensusPerson[])peopleGrid_.DataSource)[peopleGrid_.CurrentCell.RowNumber];
            int householdIndex = censusPerson.houseHoldIdx;
            censusPerson.delete();
            save();

            // Display the members of this household.
            peopleGrid_.SetDataBinding(database_.censusHouseholdMembers(householdIndex), "");
        }
    }
}
