using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

// Family tree objects.
using family_tree.objects;

namespace family_tree.viewer
{
    /// <summary>Dialog to allow the user to edit the complete list of sources.</summary>
    public partial class EditSourcesDialog : System.Windows.Forms.Form
    {
        #region Member Variables

        /// <summary>Database that to edit the sources in.</summary>
        private Database database_;

        /// <summary>The source that we are currently editing.</summary>
        private Source activeSource_;

        /// <summary>True when we are allowing events.</summary>
        private bool isAllowEvents_;

        #endregion

        #region Constructors etc ...



        /// <summary>Class constructor.  Specify the specific source to start editting.</summary>
        /// <param name="database">Specify the database to show the sources from.</param>
        /// <param name="sourceIndex">Specify the ID of the source to edit initially.</param>
        public EditSourcesDialog(Database database, int sourceIndex)
        {
            // Required for Windows Form Designer support
            InitializeComponent();

            // Initialise member variables.
            database_ = database;

            // Show the additional information types.
            IndexName[] sources = database_.getSourceAdditionalTypes();
            foreach (IndexName additional in sources)
            {
                cboAdditionalInfo_.Items.Add(additional);
            }

            // Add the repositories.
            sources = database_.getRepositories();
            foreach (IndexName repository in sources)
            {
                cboRepository_.Items.Add(repository);
            }

            // Add the sources to the dialog box.
            Source selected = null;
            sources = database.getSources(family_tree.objects.SortOrder.DATE);
            for (int i = 0; i < sources.Length; i++)
            {
                Source source = new Source(database_, sources[i].index);
                lstSources_.Items.Add(source);
                if (sources[i].index == sourceIndex)
                {
                    selected = source;
                }
            }

            // Select the specified source.
            if (selected != null)
            {
                lstSources_.SelectedItem = selected;
            }

            // Allow events.
            isAllowEvents_ = true;
        }



        /// <summary>Class constructor.  Open the edit sources dialog box without specifing a particular source to edit.</summary>
        /// <param name="database">Specify the database to show the sources from.</param>
        public EditSourcesDialog(Database database) : this(database, 0)
        {
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



        #endregion

        #region Additional Information / Form Specific Information



        /// <summary>Display the optional additional information for this source.</summary>
        private void showAdditionalInfo()
        {
            // Disable events.
            bool isEvents = isAllowEvents_;
            isAllowEvents_ = false;

            // Activate the additional information.
            switch (activeSource_.additionalInfoTypeIndex)
            {
            case 0: // None
            default: // Unknown.
                hideAllAditionalInfo();
                break;
            case 1:
                grpBirth_.Visible = true;
                grpCensus_.Visible = false;
                grpDeath_.Visible = false;
                grpMarriage_.Visible = false;
                grpFreeTable_.Visible = false;

                if (activeSource_.additionalBirth == null)
                {
                    txtBirthDistrict_.Text = "";
                    dtpBirthWhen_.Value = DateTime.Now;
                    txtBirthWhenWhere_.Text = "";
                    txtBirthName_.Text = "";
                    txtBirthSex_.Text = "";
                    txtBirthFather_.Text = "";
                    txtBirthFatherOccupation_.Text = "";
                    txtBirthMother_.Text = "";
                    txtBirthMotherDetails_.Text = "";
                    txtBirthInformant_.Text = "";
                    txtBirthInformantAddress_.Text = "";
                    txtBirthWhenReg_.Text = "";
                    txtBirthReference_.Text = "";
                }
                else
                {
                    txtBirthDistrict_.Text = activeSource_.additionalBirth.registrationDistrict;
                    try
                    {
                        dtpBirthWhen_.Value = activeSource_.additionalBirth.when;
                    }
                    catch { }
                    txtBirthWhenWhere_.Text = activeSource_.additionalBirth.whenAndWhere;
                    txtBirthName_.Text = activeSource_.additionalBirth.name;
                    txtBirthSex_.Text = activeSource_.additionalBirth.sex;
                    txtBirthFather_.Text = activeSource_.additionalBirth.father;
                    txtBirthFatherOccupation_.Text = activeSource_.additionalBirth.fatherOccupation;
                    txtBirthMother_.Text = activeSource_.additionalBirth.mother;
                    txtBirthMotherDetails_.Text = activeSource_.additionalBirth.motherDetails;
                    txtBirthInformant_.Text = activeSource_.additionalBirth.informant;
                    txtBirthInformantAddress_.Text = activeSource_.additionalBirth.informantAddress;
                    txtBirthWhenReg_.Text = activeSource_.additionalBirth.whenRegistered;
                    txtBirthReference_.Text = activeSource_.additionalBirth.groReference;
                }
                break;

            case 2:
                grpBirth_.Visible = false;
                grpCensus_.Visible = false;
                grpDeath_.Visible = false;
                grpMarriage_.Visible = true;
                grpFreeTable_.Visible = false;

                if (activeSource_.additionalMarriage == null)
                {
                    dtpMarrWhen_.Value = DateTime.Now;
                    txtMarrLocation_.Text = "";
                    txtMarrGroom_.Text = "";
                    txtMarrGroomAge_.Text = "";
                    txtMarrGroomOccu_.Text = "";
                    txtMarrGroomLoca_.Text = "";
                    txtMarrGroomFather_.Text = "";
                    txtMarrGroomFatherOcc_.Text = "";
                    txtMarrBride_.Text = "";
                    txtMarrBrideAge_.Text = "";
                    txtMarrBrideOccu_.Text = "";
                    txtMarrBrideLoca_.Text = "";
                    txtMarrBrideFather_.Text = "";
                    txtMarrBrideFatherOcc_.Text = "";
                    txtMarrWitness_.Text = "";
                    txtMarrGro_.Text = "";
                }
                else
                {
                    try
                    {
                        dtpMarrWhen_.Value = activeSource_.additionalMarriage.when;
                    }
                    catch { }
                    txtMarrLocation_.Text = activeSource_.additionalMarriage.location;
                    txtMarrGroom_.Text = activeSource_.additionalMarriage.groomName;
                    txtMarrGroomAge_.Text = activeSource_.additionalMarriage.groomAge;
                    txtMarrGroomOccu_.Text = activeSource_.additionalMarriage.groomOccupation;
                    txtMarrGroomLoca_.Text = activeSource_.additionalMarriage.groomLiving;
                    txtMarrGroomFather_.Text = activeSource_.additionalMarriage.groomFather;
                    txtMarrGroomFatherOcc_.Text = activeSource_.additionalMarriage.groomFatherOccupation;
                    txtMarrBride_.Text = activeSource_.additionalMarriage.brideName;
                    txtMarrBrideAge_.Text = activeSource_.additionalMarriage.brideAge;
                    txtMarrBrideOccu_.Text = activeSource_.additionalMarriage.brideOccupation;
                    txtMarrBrideLoca_.Text = activeSource_.additionalMarriage.brideLiving;
                    txtMarrBrideFather_.Text = activeSource_.additionalMarriage.brideFather;
                    txtMarrBrideFatherOcc_.Text = activeSource_.additionalMarriage.brideFatherOccupation;
                    txtMarrWitness_.Text = activeSource_.additionalMarriage.witness;
                    txtMarrGro_.Text = activeSource_.additionalMarriage.groReference;
                }
                break;

            case 3:
                grpBirth_.Visible = false;
                grpCensus_.Visible = false;
                grpDeath_.Visible = true;
                grpMarriage_.Visible = false;
                grpFreeTable_.Visible = false;

                if (activeSource_.additionalDeath == null)
                {
                    txtDeathDistrict_.Text = "";
                    txtDeathWhen_.Text = "";
                    txtDeathWhere_.Text = "";
                    txtDeathName_.Text = "";
                    txtDeathSex_.Text = "";
                    txtDeathDatePlace_.Text = "";
                    txtDeathOccupation_.Text = "";
                    txtDeathUsualAddress_.Text = "";
                    txtDeathCause_.Text = "";
                    txtDeathInformant_.Text = "";
                    txtDeathInformantDescription_.Text = "";
                    txtDeathInformantAddress_.Text = "";
                    txtDeathWhenReg_.Text = "";
                    txtDeathReference_.Text = "";
                }
                else
                {
                    txtDeathDistrict_.Text = activeSource_.additionalDeath.registrationDistrict;
                    txtDeathWhen_.Text = activeSource_.additionalDeath.when;
                    txtDeathWhere_.Text = activeSource_.additionalDeath.place;
                    txtDeathName_.Text = activeSource_.additionalDeath.name;
                    txtDeathSex_.Text = activeSource_.additionalDeath.sex;
                    txtDeathDatePlace_.Text = activeSource_.additionalDeath.datePlaceOfBirth;
                    txtDeathOccupation_.Text = activeSource_.additionalDeath.occupation;
                    txtDeathUsualAddress_.Text = activeSource_.additionalDeath.usualAddress;
                    txtDeathCause_.Text = activeSource_.additionalDeath.causeOfDeath;
                    txtDeathInformant_.Text = activeSource_.additionalDeath.informant;
                    txtDeathInformantDescription_.Text = activeSource_.additionalDeath.informantDescription;
                    txtDeathInformantAddress_.Text = activeSource_.additionalDeath.informantAddress;
                    txtDeathWhenReg_.Text = activeSource_.additionalDeath.whenRegistered;
                    txtDeathReference_.Text = activeSource_.additionalDeath.groReference;
                }
                break;

            case 4:
                grpBirth_.Visible = false;
                grpCensus_.Visible = true;
                grpDeath_.Visible = false;
                grpMarriage_.Visible = false;
                grpFreeTable_.Visible = false;

                if (activeSource_.additionalCensus == null)
                {
                    txtCensusAddress_.Text = "Null";
                    txtCensusSeries_.Text = "Null";
                    txtCensusPiece_.Text = "Null";
                    txtCensusFolio_.Text = "Null";
                    txtCensusPage_.Text = "Null";
                }
                else
                {
                    txtCensusAddress_.Text = activeSource_.additionalCensus.address;
                    txtCensusSeries_.Text = activeSource_.additionalCensus.series;
                    txtCensusPiece_.Text = activeSource_.additionalCensus.piece;
                    txtCensusFolio_.Text = activeSource_.additionalCensus.folio;
                    txtCensusPage_.Text = activeSource_.additionalCensus.page;
                }
                break;

            case 5:
                grpBirth_.Visible = false;
                grpCensus_.Visible = false;
                grpDeath_.Visible = false;
                grpMarriage_.Visible = false;
                grpFreeTable_.Visible = true;

                populateSourceFreeTable();
                break;
            }

            // Enable events.
            isAllowEvents_ = isEvents;
        }



        /// <summary>Hide all the optional additonal information sections.</summary>
        private void hideAllAditionalInfo()
        {
            grpBirth_.Visible = false;
            grpCensus_.Visible = false;
            grpDeath_.Visible = false;
            grpMarriage_.Visible = false;
            grpFreeTable_.Visible = false;
        }



        #endregion

        #region Message Handlers

        #region Form Events



        /// <summary>Message handler for the form shown event.</summary>
        private void frmEditSources_Shown(object sender, EventArgs e)
        {
            // Select the first source if nothing is already selected
            if (lstSources_.SelectedIndex < 0)
            {
                lstSources_.SelectedIndex = 0;
            }
        }


        #endregion



        private void radioDate_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radioDate.Checked)
            {
                lstSources_.Sorted = false;
            }
        }



        private void radioAlpha_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radioAlpha.Checked)
            {
                lstSources_.Sorted = true;
            }
        }



        bool populateSourceFreeTable()
        {
            // Remove any existing data binding so that we can edit the columns.
            dataGridViewSourceFreeTable_.DataSource = null;
            dataGridViewSourceFreeTable_.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.LightBlue;

            // Define the columns that are required and stop the data binding adding it's own.
            dataGridViewSourceFreeTable_.AutoGenerateColumns = false;
            dataGridViewSourceFreeTable_.ColumnCount = 2;
            dataGridViewSourceFreeTable_.Columns[0].Name = "Label";
            dataGridViewSourceFreeTable_.Columns[0].DataPropertyName = "labelText";
            dataGridViewSourceFreeTable_.Columns[0].ReadOnly = false;
            dataGridViewSourceFreeTable_.Columns[0].Width = 120;
            dataGridViewSourceFreeTable_.Columns[1].Name = "Value";
            dataGridViewSourceFreeTable_.Columns[1].DataPropertyName = "freeText";
            dataGridViewSourceFreeTable_.Columns[1].ReadOnly = false;
            dataGridViewSourceFreeTable_.Columns[1].Width = 370;

            // Bind the data to this grid.
            SourceFreeTableRow[] rows = activeSource_.freeTable.getRows();
            dataGridViewSourceFreeTable_.DataSource = rows;

            // Return success.
            return true;
        }



        /// <summary>Message handler for the selection on the list of sources changing.  Load and display the selected source object.</summary>
        private void lstSources_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            activeSource_ = (Source)this.lstSources_.SelectedItem;
            if (activeSource_ == null)
            {
                // Clear the controls.
            }
            else if (activeSource_.isValid())
            {
                // Populate the controls.
                txtDescription_.Text = activeSource_.description;
                dateSourceDate_.theDate = activeSource_.theDate;
                txtComments_.Text = activeSource_.comments;
                cboAdditionalInfo_.SelectedIndex = activeSource_.additionalInfoTypeIndex;
                cboRepository_.SelectedIndex = activeSource_.repository; // This is not really correct.

                DataGridTableStyle tableStyle = new DataGridTableStyle
                {
                    // Sets the MappingName to the class name plus brackets.
                    MappingName = "References[]",

                    // Sets the AlternatingBackColor so you can see the difference.
                    AlternatingBackColor = System.Drawing.Color.LightBlue
                };

                // Creates 2 column styles.
                DataGridTextBoxColumn columnPerson = new DataGridTextBoxColumn
                {
                    MappingName = "personName",
                    HeaderText = "Person",
                    ReadOnly = true,
                    Width = 250
                };

                DataGridTextBoxColumn columnReferences = new DataGridTextBoxColumn
                {
                    MappingName = "references",
                    HeaderText = "References",
                    ReadOnly = true,
                    Width = 500
                };

                // Adds the column styles to the grid table style.
                tableStyle.GridColumnStyles.Add(columnPerson);
                tableStyle.GridColumnStyles.Add(columnReferences);

                // Add the table style to the collection, but clear the collection first.
                this.gridReferences_.TableStyles.Clear();
                this.gridReferences_.TableStyles.Add(tableStyle);
                this.gridReferences_.ReadOnly = true;

                // Update the references to this source.
                References[] references = activeSource_.getReferences();
                this.gridReferences_.SetDataBinding(references, "");

                // Show the additional information for this source (birth certificate etc...)
                showAdditionalInfo();
            }
            else
            {
                txtDescription_.Text = "[Deleted]";
                txtComments_.Text = "[Deleted]";

                hideAllAditionalInfo();
            }
        }



        /// <summary>Message handler for the description text box changing value.  Update the active source.</summary>
        private void txtDescription_TextChanged(object sender, System.EventArgs e)
        {
            if (activeSource_ == null)
            {
                return;
            }
            activeSource_.description = txtDescription_.Text;
        }



        /// <summary>Message handler for the date of the source control changing value.  Update the active source.</summary>
        private void dateTheDate_evtValueChanged(object oSender)
        {
            if (activeSource_ == null)
            {
                return;
            }
            activeSource_.theDate.date = dateSourceDate_.getDate();
            activeSource_.theDate.status = dateSourceDate_.getStatus();
        }



        /// <summary>Message handler for the comments textbox changing value.  Update the active source.</summary>
        private void txtComments_TextChanged(object sender, System.EventArgs e)
        {
            if (activeSource_ == null)
            {
                return;
            }
            activeSource_.comments = txtComments_.Text;
        }



        private void cmdOK_Click(object sender, System.EventArgs e)
        {
            // Save all the sources
            for (int i = 0; i < lstSources_.Items.Count; i++)
            {
                Source source = (Source)lstSources_.Items[i];
                source.save();
            }
        }



        private void cmdDeleteSource_Click(object sender, System.EventArgs e)
        {
            if (activeSource_ == null)
            {
                return;
            }
            activeSource_.delete();
        }



        private void cmdAddSource_Click(object sender, System.EventArgs e)
        {
            Source newSource = new Source(database_) { description = "New Source" };
            int newIndex = lstSources_.Items.Add(newSource);
            lstSources_.SelectedIndex = newIndex;
        }



        private void cboPrefix_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (cboPrefix_.SelectedIndex >= 0)
            {
                txtDescription_.Text = cboPrefix_.SelectedItem.ToString() + " " + txtDescription_.Text;
                cboPrefix_.SelectedIndex = -1;
            }
        }



        /// <summary>Message handler for the addition information type combo box changing.</summary>
        private void cboAdditionalInfo_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // Check that a source is selected.
            if (activeSource_ == null)
            {
                return;
            }

            IndexName additionalType = (IndexName)cboAdditionalInfo_.SelectedItem;
            activeSource_.additionalInfoTypeIndex = additionalType.index;

            showAdditionalInfo();
        }



        /// <summary>Message handler for the census address changing.  Update the census object inside the current source.</summary>
        private void evtAdditionalCensus_Changed(object sender, System.EventArgs e)
        {
            // Allow events.
            if (!isAllowEvents_)
            {
                return;
            }

            // Check that a source is selected.
            if (activeSource_ == null)
            {
                return;
            }

            // Check that a Census object is available.
            if (activeSource_.additionalCensus == null)
            {
                return;
            }

            // Update the census object.
            activeSource_.additionalCensus.address = txtCensusAddress_.Text;
            activeSource_.additionalCensus.series = txtCensusSeries_.Text;
            activeSource_.additionalCensus.piece = txtCensusPiece_.Text;
            activeSource_.additionalCensus.folio = txtCensusFolio_.Text;
            activeSource_.additionalCensus.page = txtCensusPage_.Text;
        }



        /// <summary>Message handler for the launch census editor button click.  Open the frmEditCensus dialog display the census data for this source.</summary>
        private void cmdCensusOpen_Click(object sender, System.EventArgs e)
        {
            // Check that a source is selected.
            if (activeSource_ == null)
            {
                return;
            }

            // Create a dialog to show the full census record.
            EditCensusDialog censusDialog = new EditCensusDialog(database_, activeSource_.index);

            // Show the dialog and wait for the dialog to close.
            censusDialog.ShowDialog(this);
            censusDialog.Dispose();
        }



        /// <summary>Message handler for any of the additional information Marriage fields changing.</summary>
        private void evtAdditionalMarriage_Changed(object sender, System.EventArgs e)
        {
            // Allow events.
            if (!isAllowEvents_)
            {
                return;
            }

            // Check that a source is selected.
            if (activeSource_ == null)
            {
                return;
            }

            // Update the additional marriage information.
            activeSource_.additionalMarriage.when = dtpMarrWhen_.Value;
            activeSource_.additionalMarriage.location = txtMarrLocation_.Text;
            activeSource_.additionalMarriage.groomName = txtMarrGroom_.Text;
            activeSource_.additionalMarriage.groomAge = txtMarrGroomAge_.Text;
            activeSource_.additionalMarriage.groomOccupation = txtMarrGroomOccu_.Text;
            activeSource_.additionalMarriage.groomLiving = txtMarrGroomLoca_.Text;
            activeSource_.additionalMarriage.groomFather = txtMarrGroomFather_.Text;
            activeSource_.additionalMarriage.groomFatherOccupation = txtMarrGroomFatherOcc_.Text;
            activeSource_.additionalMarriage.brideName = txtMarrBride_.Text;
            activeSource_.additionalMarriage.brideAge = txtMarrBrideAge_.Text;
            activeSource_.additionalMarriage.brideOccupation = txtMarrBrideOccu_.Text;
            activeSource_.additionalMarriage.brideLiving = txtMarrBrideLoca_.Text;
            activeSource_.additionalMarriage.brideFather = txtMarrBrideFather_.Text;
            activeSource_.additionalMarriage.brideFatherOccupation = txtMarrBrideFatherOcc_.Text;
            activeSource_.additionalMarriage.witness = txtMarrWitness_.Text;
            activeSource_.additionalMarriage.groReference = txtMarrGro_.Text;
        }



        /// <summary>Message handler for any of the additional information birth fields changing.</summary>
        private void evtAdditionalBirth_Changed(object sender, System.EventArgs e)
        {
            // Allow events.
            if (!isAllowEvents_)
            {
                return;
            }

            // Check that a source is selected.
            if (activeSource_ == null)
            {
                return;
            }

            // Update the additional birth information.
            activeSource_.additionalBirth.registrationDistrict = txtBirthDistrict_.Text;
            activeSource_.additionalBirth.when = dtpBirthWhen_.Value;
            activeSource_.additionalBirth.whenAndWhere = txtBirthWhenWhere_.Text;
            activeSource_.additionalBirth.name = txtBirthName_.Text;
            activeSource_.additionalBirth.sex = txtBirthSex_.Text;
            activeSource_.additionalBirth.father = txtBirthFather_.Text;
            activeSource_.additionalBirth.fatherOccupation = txtBirthFatherOccupation_.Text;
            activeSource_.additionalBirth.mother = txtBirthMother_.Text;
            activeSource_.additionalBirth.motherDetails = txtBirthMotherDetails_.Text;
            activeSource_.additionalBirth.informant = txtBirthInformant_.Text;
            activeSource_.additionalBirth.informantAddress = txtBirthInformantAddress_.Text;
            activeSource_.additionalBirth.whenRegistered = txtBirthWhenReg_.Text;
            activeSource_.additionalBirth.groReference = txtBirthReference_.Text;
        }



        /// <summary>Message handler for any of the additional information death fields changing.</summary>
        private void evtAdditionalDeath_Changed(object sender, System.EventArgs e)
        {
            // Allow events.
            if (!isAllowEvents_)
            {
                return;
            }

            // Check that a source is selected.
            if (activeSource_ == null)
            {
                return;
            }

            // Update the additional death information.
            activeSource_.additionalDeath.registrationDistrict = txtDeathDistrict_.Text;
            activeSource_.additionalDeath.when = txtDeathWhen_.Text;
            activeSource_.additionalDeath.place = txtDeathWhere_.Text;
            activeSource_.additionalDeath.name = txtDeathName_.Text;
            activeSource_.additionalDeath.sex = txtDeathSex_.Text;
            activeSource_.additionalDeath.datePlaceOfBirth = txtDeathDatePlace_.Text;
            activeSource_.additionalDeath.occupation = txtDeathOccupation_.Text;
            activeSource_.additionalDeath.usualAddress = txtDeathUsualAddress_.Text;
            activeSource_.additionalDeath.causeOfDeath = txtDeathCause_.Text;
            activeSource_.additionalDeath.informant = txtDeathInformant_.Text;
            activeSource_.additionalDeath.informantDescription = txtDeathInformantDescription_.Text;
            activeSource_.additionalDeath.informantAddress = txtDeathInformantAddress_.Text;
            activeSource_.additionalDeath.whenRegistered = txtDeathWhenReg_.Text;
            activeSource_.additionalDeath.groReference = txtDeathReference_.Text;
        }



        /// <summary>Message handler for the repository combo box value changed event.</summary>
        private void cboRepository_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // Check that a source is selected.
            if (activeSource_ == null)
            {
                return;
            }

            IndexName repository = (IndexName)this.cboRepository_.SelectedItem;
            activeSource_.repository = repository.index;
        }



        /// <summary>Message handler for the "Census Address" button click.</summary>
        private void cmdCensusAddress_Click(object sender, EventArgs e)
        {
            SelectLocationDialog selectLocationDialog = new SelectLocationDialog(database_, txtCensusAddress_.Text);
            if (selectLocationDialog.ShowDialog(this) == DialogResult.OK)
            {
                txtCensusAddress_.Text = selectLocationDialog.locationName;
            }
        }



        #endregion

        private void buttonAddFreeTableRow_Click(object sender, EventArgs e)
        {
            // Add a new row to the free table.
            activeSource_.freeTable.addRow("New", "New");

            // Bind the data to the grid.
            SourceFreeTableRow[] rows = activeSource_.freeTable.getRows();
            dataGridViewSourceFreeTable_.DataSource = rows;

        }

        private void buttonRemoveFreeTableRow_Click(object sender, EventArgs e)
        {
            // Find the active row in the free table.
            int row = dataGridViewSourceFreeTable_.CurrentCell.RowIndex;

            // Remove this row.
            activeSource_.freeTable.deleteRow(row);

            // Bind the data to the grid.
            SourceFreeTableRow[] rows = activeSource_.freeTable.getRows();
            dataGridViewSourceFreeTable_.DataSource = rows;
        }
    }
}
