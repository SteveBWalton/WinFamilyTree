using FamilyTree.Objects;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace FamilyTree.Viewer
{
    /// <summary>Form to allow a person to be edited.</summary>
    public partial class EditPersonDialog : System.Windows.Forms.Form
    {
        #region Member Variables

        /// <summary>Person object to edit.</summary>
        private Person person_;

        /// <summary>Active sources.</summary>
        private Sources sources_;

        /// <summary>Active relationship.</summary>
        private Relationship activeRelationship_;

        #endregion

        #region Constructors



        /// <summary>Initialises the edit person dialog with the specified person in the specified database.</summary>
        /// <param name="personIndex">Specifies the ID of the person to edit.</param>
        /// <param name="database">Specifies the database to save the person into.</param>
        public EditPersonDialog(int personIndex, Database database)
        {
            // Required for Windows Form Designer support.
            InitializeComponent();

            // Initialise the form.
            FactType[] factTypes = database.getFactTypes();
            for (int i = 0; i < factTypes.Length; i++)
            {
                cboFactType_.Items.Add(factTypes[i]);
            }
            IndexName[] sources = database.getSources(Objects.SortOrder.DATE);
            for (int i = 0; i < sources.Length; i++)
            {
                cboSources_.Items.Add(sources[i]);
            }

            // Create the person object to edit.
            if (personIndex == 0)
            {
                person_ = new Person(database);
            }
            else
            {
                person_ = database.getPerson(personIndex);
            }

            // Update the values in the dialog.
            if (person_.isMale)
            {
                cboSex_.SelectedIndex = 0;
            }
            else
            {
                cboSex_.SelectedIndex = 1;
            }
            txtSurname_.Text = person_.surname;
            txtForename_.Text = person_.forenames;
            txtMaidenName_.Text = person_.maidenname;
            dateDoB_.theDate = person_.dob;
            dateDoD_.theDate = person_.dod;
            chkChildrenKnown_.Checked = person_.isAllChildrenKnown;
            txtComments_.Text = person_.comments;
            // m_labEditor.Text = "Last Edit by "+m_oPerson.LastEditBy+" on "+m_oPerson.LastEditDate.ToString("d-MMM-yyyy HH:mm:ss");

            // Initialise the sources grid.
            refreshSources(person_.sourceName, "Name");

            // Initialise the facts grid.
            populateFactsGrid();

            // Update the description.
            labDescription_.Text = person_.getDescription(false, false, false, false, false);

            // Initialise the relationships tab.
            Relationship[] relationships = person_.getRelationships();
            for (int i = 0; i < relationships.Length; i++)
            {
                lstRelationships_.Items.Add(relationships[i]);
            }

            // Add the possible people to the relationship combo box.
            IndexName[] possiblePartners = person_.possiblePartners();
            for (int i = 0; i < possiblePartners.Length; i++)
            {
                cboAddPartner_.Items.Add(possiblePartners[i]);
            }
        }



        /// <summary>Initialise the edit person dialog with a new person.</summary>
        /// <param name="database">Specifies the database to write this person to.</param>
        public EditPersonDialog(Database database) : this((int)0, database)
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

        #region Supporting Functions



        /// <summary>Returns the ID of the person on the form.</summary>
        /// <returns>The ID of the person on the form.</returns>
        public int getPersonIndex()
        {
            return person_.index;
        }



        /// <summary>Populate the facts grid with the facts from the m_oPerson object.</summary>
        /// <returns>True for success, false otherwise.</returns>
        private bool populateFactsGrid()
        {
            // Populate the grid with data.
            gridFacts_.SetDataBinding(person_.getFacts(), "");

            // Creates two DataGridTableStyle objects, one for the Machine
            // array, and one for the Parts ArrayList.

            // Create a DataGridTabkeStyle object for the facts.
            DataGridTableStyle factsTableStyle = new DataGridTableStyle
            {
                MappingName = "Fact[]",

                // Sets the AlternatingBackColor so you can see the difference.
                AlternatingBackColor = System.Drawing.Color.LightBlue
            };

            // Creates a column for the rank.
            DataGridTextBoxColumn columnRank = new DataGridTextBoxColumn
            {
                MappingName = "rank",
                HeaderText = "Rank",
                ReadOnly = false,
                Width = 40
            };

            // Create a column for the type.
            DataGridTextBoxColumn columnName = new DataGridTextBoxColumn()
            {
                MappingName = "typeName",
                HeaderText = "Type",
                ReadOnly = true,
                Width = 120
            };

            // Create a column for the information.
            DataGridTextBoxColumn columnInfo = new DataGridTextBoxColumn
            {
                MappingName = "information",
                HeaderText = "Information",
                Width = 350
            };

            /*
            DataGridTextBoxColumn columnUse = new DataGridTextBoxColumn();
            columnUse.MappingName= "UseInDescription";
            columnUse.HeaderText= "UseInDescription";
            columnUse.Width = 50;
            */

            // Adds the column styles to the grid table style.
            factsTableStyle.GridColumnStyles.Add(columnRank);
            factsTableStyle.GridColumnStyles.Add(columnName);
            factsTableStyle.GridColumnStyles.Add(columnInfo);

            // Add the table style to the collection, but clear the collection first.
            gridFacts_.TableStyles.Clear();
            gridFacts_.TableStyles.Add(factsTableStyle);

            // Return success.
            return true;
        }



        #endregion

        #region Sources



        /// <summary>Repopulate the sources grid with the specified sources.  The grid is labelled as belonging to the specified string.</summary>
        /// <param name="sources">Specifiy the collection of sources to populate the grid with.</param>
        /// <param name="ownerLabel">Name of the owner to label the grid with.</param>
        private void refreshSources(Sources sources, string ownerLabel)
        {
            refreshSources(sources);
            gridSources_.CaptionText = "Sources for " + ownerLabel;
            grpSources_.Text = "Sources for " + ownerLabel;
        }



        /// <summary>Refresh the sources grid without changing the label.  Use as RefreshSources(m_oSources) to repaint the control.</summary>
        /// <param name="sources">Specifies the sources to refresh the grid with.</param>
        private void refreshSources(Sources sources)
        {
            Source[] listSources;
            sources_ = sources;
            if (sources == null)
            {
                listSources = new Source[0];
                gridSources_.SetDataBinding(listSources, "");
            }
            else
            {
                listSources = sources.getAsSources();
                gridSources_.SetDataBinding(listSources, "");
                createSourcesGridStyle();
            }
        }



        /// <summary>Clear the sources grid.  Use when the active control has no sources.</summary>
        private void refreshSources()
        {
            refreshSources((Sources)null);
            gridSources_.CaptionText = "Sources";
            grpSources_.Text = "Sources";
        }



        /// <summary>Creates the style for the ToDo grid.</summary>
        private void createToDoGridStyle()
        {
            // Define a style to apply to the ToDo grid
            DataGridTableStyle toDoTableStyle = new DataGridTableStyle
            {
                // Sets the MappingName to the class name plus brackets.
                MappingName = "ToDo[]",

                // Sets the AlternatingBackColor so you can see the difference.
                AlternatingBackColor = System.Drawing.Color.LightBlue
            };

            // Create a column to hold the source.
            DataGridTextBoxColumn columnPriority = new DataGridTextBoxColumn
            {
                MappingName = "priority",
                HeaderText = "Ranking",
                ReadOnly = false,
                Width = 50
            };

            DataGridTextBoxColumn columnDescription = new DataGridTextBoxColumn
            {
                MappingName = "description",
                HeaderText = "Description",
                ReadOnly = false,
                Width = 500
            };

            // Adds the column styles to the grid table style.
            toDoTableStyle.GridColumnStyles.Add(columnPriority);
            toDoTableStyle.GridColumnStyles.Add(columnDescription);

            // Add the table style to the collection, but clear the collection first.
            gridToDo_.TableStyles.Clear();
            gridToDo_.TableStyles.Add(toDoTableStyle);
            gridToDo_.ReadOnly = false;
        }



        /// <summary>Create the style for the sources grid.</summary>
        private void createSourcesGridStyle()
        {
            // Creates two DataGridTableStyle objects, one for the Machine
            // array, and one for the Parts ArrayList.

            DataGridTableStyle sourceTableStyle = new DataGridTableStyle
            {
                // Sets the MappingName to the class name plus brackets.
                MappingName = "Source[]",

                // Sets the AlternatingBackColor so you can see the difference.
                AlternatingBackColor = System.Drawing.Color.LightBlue
            };

            // Create a column to hold the source.
            DataGridTextBoxColumn columnSource = new DataGridTextBoxColumn
            {
                MappingName = "label",
                HeaderText = "Source",
                ReadOnly = true,
                Width = 500
            };

            DataGridTextBoxColumn columnRank = new DataGridTextBoxColumn
            {
                MappingName = "ranking",
                HeaderText = "Ranking",
                ReadOnly = false,
                Width = 50
            };

            // Adds the column styles to the grid table style.
            sourceTableStyle.GridColumnStyles.Add(columnSource);
            sourceTableStyle.GridColumnStyles.Add(columnRank);

            // Add the table style to the collection, but clear the collection first.
            gridSources_.TableStyles.Clear();
            gridSources_.TableStyles.Add(sourceTableStyle);
            gridSources_.ReadOnly = false;
        }



        #endregion

        #region Message Handlers

        #region Form Events



        /// <summary>Message handler for the Form load event.</summary>
        private void frmEditPerson_Load(object sender, System.EventArgs e)
        {
            // Label for the dialog.
            Text = person_.getName(true, false);
        }



        /// <summary>Message handler for the form shown event.</summary>
        private void frmEditPerson_Shown(object sender, EventArgs e)
        {
            // Initialise the editor combo with the possible editors.
            string[] editors = person_.database.getEditors();
            foreach (string editor in editors)
            {
                cboEditor_.Items.Add(editor);
                if (editor == "Steve Walton")
                {
                    cboEditor_.SelectedIndex = cboEditor_.Items.Count - 1;
                }
            }
        }



        /// <summary>This is called when the user clicks the OK button.  It saves the data on the form into a person record.  If does not handle closing the form that is handled by .NET.</summary>
        private void cmdOK_Click(object sender, System.EventArgs e)
        {
            // Save the record to the database.
            person_.lastEditBy = cboEditor_.SelectedItem.ToString();
            person_.save();
        }



        /// <summary>This is called when the user clicks the add source button.  This adds a source to the active source collection.  The active source is defined when each control adds it source to the dialog.  The last source is active.</summary>
        private void cmdAddSource_Click(object sender, System.EventArgs e)
        {
            // Check that a source is selected.
            if (cboSources_.SelectedIndex < 0)
            {
                return;
            }

            // Check that a sources object is available.
            if (sources_ == null)
            {
                return;
            }

            // Find the Source index.
            IndexName source = (IndexName)cboSources_.Items[cboSources_.SelectedIndex];

            // Add the source to this fact.
            sources_.add(source.index);

            // Update the display.
            refreshSources(sources_);
            cboSources_.SelectedIndex = -1;
        }



        private void cmdDeleteSource_Click(object sender, System.EventArgs e)
        {
            // Validate the active cell.
            if (gridSources_.CurrentCell.RowNumber < 0)
            {
                return;
            }

            // Mark the source as deleted.
            sources_.delete(gridSources_.CurrentCell.RowNumber);

            // Update the display.
            refreshSources(sources_);
        }



        /// <summary>Message handler for a drag-drop object passing over the window.</summary>
        private void frmEditPerson_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            // If the data is a file, display the copy cursor.
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }



        #endregion

        #region Controls that apply to all tabs



        /// <summary>Message handler for a control with the non specific source receiving the focus.  Display the non specific sources for the person.</summary>
        private void evtNonSpecificSource(object sender, System.EventArgs e)
        {
            refreshSources(person_.sourceNonSpecific, "Non Specific");
        }



        /// <summary>Message handler for the sources grid losing the focus.  Write any changes to the database.</summary>
        private void gridSources_Leave(object sender, System.EventArgs e)
        {
            if (sources_ != null)
            {
                // Loop through the sources on the grid	
                Source[] sources = (Source[])gridSources_.DataSource;
                for (int i = 0; i < sources.Length; i++)
                {
                    if (sources[i] != null)
                    {
                        int newRanking = sources[i].ranking;

                        if (sources_.getRanking(i) != newRanking)
                        {
                            // Show the message on the screen.
                            // MessageBox.Show(this,nIndex.ToString()+" = "+nNewRanking.ToString());

                            // Change the ranking on this source.
                            sources_.changeRanking(i, newRanking);
                        }
                    }
                }
            }
        }



        /// <summary>Message handler for the tab control changing the active tab.  Populate the Advanced tab if it is displayed.</summary>
        private void tabControl1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            switch (tabControl_.SelectedIndex)
            {
            case 1:
                // Force the focus on the first spouse on the relationship tab.
                if (lstRelationships_.Items.Count > 0)
                {
                    if (lstRelationships_.SelectedIndex < 0)
                    {
                        lstRelationships_.SelectedIndex = 0;
                    }
                }
                break;

            case 3:
                // Populate the advance tab.
                if (cboFather_.Items.Count == 0)
                {
                    IndexName[] possibleFathers = person_.possibleFathers();
                    for (int i = 0; i < possibleFathers.Length; i++)
                    {
                        cboFather_.Items.Add(possibleFathers[i]);
                        if (possibleFathers[i].index == person_.fatherIndex)
                        {
                            cboFather_.SelectedItem = possibleFathers[i];
                        }
                    }
                }
                if (cboMother_.Items.Count == 0)
                {
                    IndexName[] possibleMothers = person_.possibleMothers();
                    for (int i = 0; i < possibleMothers.Length; i++)
                    {
                        cboMother_.Items.Add(possibleMothers[i]);
                        if (possibleMothers[i].index == person_.motherIndex)
                        {
                            cboMother_.SelectedItem = possibleMothers[i];
                        }
                    }
                }

                if (cboMainImage_.Items.Count == 0)
                {
                    Media[] medias = person_.getMedia(false);
                    foreach (Media media in medias)
                    {
                        cboMainImage_.Items.Add(media);
                        if (media.index_ == person_.mediaIndex)
                        {
                            cboMainImage_.SelectedIndex = cboMainImage_.Items.Count - 1;
                        }
                    }
                }
                chkGedcom_.Checked = person_.isIncludeInGedcom;
                break;

            case 4:
                // Populate the ToDo List.
                if (gridToDo_.DataSource == null)
                {
                    gridToDo_.SetDataBinding(person_.getToDo(), "");
                    createToDoGridStyle();
                }
                break;
            }
        }



        #endregion

        #region Controls on the Basic Tab



        /// <summary>Update the sources when the any of the name fields are active.</summary>
        private void txtName_Enter(object sender, System.EventArgs e)
        {
            refreshSources(person_.sourceName, "Name");
        }



        /// <summary>Update the sources when the sex field is active.</summary>
        private void cboSex_Enter(object sender, System.EventArgs e)
        {
            refreshSources();
        }



        /// <summary>Update the sources when the DoB field is active.</summary>
        private void dateDoB_Enter(object sender, System.EventArgs e)
        {
            refreshSources(person_.sourceDoB, "Date of Birth");
        }



        /// <summary>Update the sources when the DoD field is active.</summary>
        private void dateDoD_Enter(object sender, System.EventArgs e)
        {
            refreshSources(person_.sourceDoD, "Date of Death");
        }



        /// <summary>Message handler for the surname changing.  Update the person object and display the person description.</summary>
        private void txtSurname_TextChanged(object sender, System.EventArgs e)
        {
            person_.surname = this.txtSurname_.Text;

            // Update the description.
            labDescription_.Text = person_.getDescription(false, false, false, false, false);
        }



        /// <summary>Message handler for the forename changing.  Update the person object and display the person description.</summary>
        private void txtForename_TextChanged(object sender, System.EventArgs e)
        {
            person_.forenames = this.txtForename_.Text;

            // Update the description.
            labDescription_.Text = person_.getDescription(false, false, false, false, false);
        }



        /// <summary>Message handler for the maiden name changing.  Update the person object and display the person description.</summary>
        private void txtMaidenName_TextChanged(object sender, System.EventArgs e)
        {
            person_.maidenname = this.txtMaidenName_.Text;

            // Update the description.
            labDescription_.Text = person_.getDescription(false, false, false, false, false);
        }



        /// <summary>Message handler for the sex of the person changing.  Update the person object and display the person description.</summary>
        private void cboSex_SelectedValueChanged(object sender, System.EventArgs e)
        {
            if (cboSex_.SelectedIndex == 0)
            {
                person_.isMale = true;
                labMaidenName_.Visible = false;
                txtMaidenName_.Visible = false;
            }
            else
            {
                person_.isFemale = true;
                labMaidenName_.Visible = true;
                txtMaidenName_.Visible = true;
            }

            // Update the description.
            labDescription_.Text = person_.getDescription(false, false, false, false, false);
        }



        /// <summary>Message handler for the all children known check box changing value.  Update the person object and display the person description.</summary>
        private void chkChildrenKnown_CheckedChanged(object sender, System.EventArgs e)
        {
            person_.isAllChildrenKnown = this.chkChildrenKnown_.Checked;

            // Update the description.
            labDescription_.Text = person_.getDescription(false, false, false, false, false);
        }



        /// <summary>Message handler for the date of birth control changing value.  Update the person object and display the person description.</summary>
        private void dateDoB_evtValueChanged(object oSender)
        {
            person_.dob.date = dateDoB_.getDate();
            person_.dob.status = dateDoB_.getStatus();

            // Update the description.
            labDescription_.Text = person_.getDescription(false, false, false, false, false);
        }



        /// <summary>Message handler for the date of death control changing value.  Update the person object and display the person description.</summary>
        private void dateDoD_evtValueChanged(object oSender)
        {
            person_.dod.date = dateDoD_.getDate();
            person_.dod.status = dateDoD_.getStatus();

            // Update the description.
            labDescription_.Text = person_.getDescription(false, false, false, false, false);
        }



        /// <summary>Message handler for the comments text box contents changing.  Update the comments property of the person object.</summary>
        private void txtComments_TextChanged(object sender, System.EventArgs e)
        {
            person_.comments = this.txtComments_.Text;
        }



        #endregion

        #region Controls on the Facts Tab



        /// <summary>This is called when the current cell changes within the facts grid and when the grid first gets the focus.</summary>
        private void gridFacts_CurrentCellChanged(object sender, System.EventArgs e)
        {
            Fact[] facts = (Fact[])gridFacts_.DataSource;
            refreshSources(facts[gridFacts_.CurrentCell.RowNumber].sources, facts[gridFacts_.CurrentCell.RowNumber].information);

            // Update the description.
            labDescription_.Text = person_.getDescription(false, false, false, false, false);
        }



        /// <summary>This is called when the user clicks the Add Fact button.  This adds a fact to the form control (not the person).</summary>
        private void cmdAddFact_Click(object sender, System.EventArgs e)
        {
            // Check that a fact type is selected.
            if (cboFactType_.SelectedIndex < 0)
            {
                return;
            }

            // Find the fact type.
            FactType factType = (FactType)cboFactType_.Items[cboFactType_.SelectedIndex];

            // Count the number of existing facts to get an initial rank for the new fact.
            Fact[] facts = (Fact[])gridFacts_.DataSource;
            int newRank = facts.Length + 1;

            // Create the new fact.
            Fact fact = new Fact(0, person_, factType.index, newRank, "Empty");

            // Add the fact	to the person
            person_.addFact(fact);

            // Update the display.
            populateFactsGrid();
        }



        private void cmdDeleteFact_Click(object sender, System.EventArgs e)
        {
            // Check that a fact is selected in the grid.
            if (gridFacts_.CurrentCell.RowNumber < 0)
            {
                return;
            }

            // Find the fact.
            Fact fact = ((Fact[])gridFacts_.DataSource)[gridFacts_.CurrentCell.RowNumber];
            fact.delete();

            // Update the display.
            populateFactsGrid();
        }



        #endregion

        #region Controls on the Relationships tab



        /// <summary>Message handler for the marriage terminated combo box getting the focus.  Display the sources for the marriage termination status.</summary>
        private void cboTerminated_Enter(object sender, System.EventArgs e)
        {
            if (activeRelationship_ == null)
            {
                refreshSources();
            }
            else
            {
                refreshSources(activeRelationship_.sourceTerminated, "Relationship End Type");
            }
        }



        /// <summary>Message handler for the marriage terminated combo box chaning value.</summary>
        private void cboTerminated_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // Update the active relationship.
            if (activeRelationship_ != null)
            {
                activeRelationship_.terminatedIndex = cboTerminated_.SelectedIndex + 1;
                activeRelationship_.lastEditBy = cboEditor_.SelectedItem.ToString();
            }
        }



        /// <summary>Message handler for the relationship type value change event.  Update the value in the active relationship.</summary>
        private void cboRelationshipType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // Update the active relationship.
            if (activeRelationship_ != null)
            {
                activeRelationship_.typeIndex = cboRelationshipType_.SelectedIndex + 1;
                activeRelationship_.lastEditBy = cboEditor_.SelectedItem.ToString();
            }
        }



        /// <summary>Message handler for the relationship type getting the focus.  Do not record the source for this information (currently) so just clear the sources area.</summary>
        private void cboRelationshipType_Enter(object sender, System.EventArgs e)
        {
            refreshSources();
        }



        private void lstRelationships_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // Validate the selected index.
            if (lstRelationships_.SelectedIndex < 0)
            {
                activeRelationship_ = null;
                return;
            }

            // Get the selected relationship.
            activeRelationship_ = (Relationship)this.lstRelationships_.SelectedItem;

            // Update the form.
            dateRelationStart_.theDate = activeRelationship_.start;
            //			this.chkTerminated.Checked = m_oActiveRelationship.Terminated;
            cboTerminated_.SelectedIndex = activeRelationship_.terminatedIndex - 1;
            txtRelationLocation_.Text = activeRelationship_.location;
            dateRelationEnd_.theDate = activeRelationship_.end;
            txtRelationComments_.Text = activeRelationship_.comments;
            cboRelationshipType_.SelectedIndex = activeRelationship_.typeIndex - 1;

            refreshSources(activeRelationship_.sourcePartner, "Relationship Partner");
        }



        private void dateRelationStart_Enter(object sender, System.EventArgs e)
        {
            if (activeRelationship_ == null)
            {
                refreshSources();
            }
            else
            {
                refreshSources(activeRelationship_.sourceStart, "Relationship Start Date");
            }
        }



        private void chkTerminated_Enter(object sender, System.EventArgs e)
        {
            if (activeRelationship_ == null)
            {
                refreshSources();
            }
            else
            {
                refreshSources(activeRelationship_.sourceTerminated, "Relationship Termination Status");
            }
        }



        private void txtRelationLocation_Enter(object sender, System.EventArgs e)
        {
            if (activeRelationship_ == null)
            {
                refreshSources();
            }
            else
            {
                refreshSources(activeRelationship_.sourceLocation, "Relationship Location");
            }
        }



        private void dateRelationEnd_Enter(object sender, System.EventArgs e)
        {
            if (activeRelationship_ == null)
            {
                refreshSources();
            }
            else
            {
                refreshSources(activeRelationship_.sourceEnd, "Relationship End Date");
            }
        }


        private void lstRelationships_Enter(object sender, System.EventArgs e)
        {
            if (activeRelationship_ == null)
            {
                refreshSources();
            }
            else
            {
                refreshSources(activeRelationship_.sourcePartner, "Relationship Partner");
            }
        }


        private void txtRelationLocation_TextChanged(object sender, System.EventArgs e)
        {
            // Update the active relationship.
            if (activeRelationship_ != null)
            {
                activeRelationship_.location = this.txtRelationLocation_.Text;
                activeRelationship_.lastEditBy = cboEditor_.SelectedItem.ToString();

                // Update the description.
                labDescription_.Text = person_.getDescription(false, false, false, false, false);
            }
        }



        private void dateRelationStart_evtValueChanged(object oSender)
        {
            // Update the active relationship.
            if (activeRelationship_ != null)
            {
                activeRelationship_.start.date = dateRelationStart_.getDate();
                activeRelationship_.start.status = dateRelationStart_.getStatus();
                activeRelationship_.lastEditBy = cboEditor_.SelectedItem.ToString();

                // Update the description.
                labDescription_.Text = person_.getDescription(false, false, false, false, false);
            }
        }



        private void dateRelationEnd_evtValueChanged(object oSender)
        {
            // Update the active relationship.
            if (activeRelationship_ != null)
            {
                activeRelationship_.end.date = dateRelationEnd_.getDate();
                activeRelationship_.end.status = dateRelationEnd_.getStatus();
                activeRelationship_.lastEditBy = cboEditor_.SelectedItem.ToString();

                // Update the description.
                labDescription_.Text = person_.getDescription(false, false, false, false, false);
            }
        }



        private void txtRelationComments_TextChanged(object sender, System.EventArgs e)
        {
            // Update the active relationship.
            if (activeRelationship_ != null)
            {
                activeRelationship_.comments = this.txtRelationComments_.Text;
                activeRelationship_.lastEditBy = cboEditor_.SelectedItem.ToString();
            }
        }


        private void addRelationship_Click(object sender, System.EventArgs e)
        {
            // Check that a person is selected.
            if (cboAddPartner_.SelectedIndex == -1)
            {
                return;
            }

            // Find the person that is selected.
            IndexName partner = (IndexName)cboAddPartner_.SelectedItem;

            // Create a relationship object.
            Relationship relationship = new Relationship(person_, partner.index);

            // Add the relationship to the persons collection.
            person_.addRelationship(relationship);

            // Add the partner to the list box.
            lstRelationships_.Items.Add(relationship);

            // Update the description.
            labDescription_.Text = person_.getDescription(false, false, false, false, false);
        }



        private void cmdDeleteRelationship_Click(object sender, System.EventArgs e)
        {
            // Get the selected relationship.
            if (activeRelationship_ == null)
            {
                return;
            }

            // Mark the active relationship for deletion.
            activeRelationship_.delete();

            // Remove the relationship from the listbox.
            lstRelationships_.Items.Remove(activeRelationship_);

            // No selection any more.
            activeRelationship_ = null;

            // Update the description.
            labDescription_.Text = person_.getDescription(false, false, false, false, false);
        }



        #endregion

        #region Controls on the Advance tab



        /// <summary>Message handler for the father combo box changing value.</summary>
        private void cboFather_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // Find the index of the selected father.
            IndexName father = (IndexName)cboFather_.SelectedItem;

            // Save the ID in the person object.
            person_.fatherIndex = father.index;
        }



        /// <summary>Message handler for the mother combo box changing value.</summary>
        private void cboMother_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // Find the ID the selected mother
            IndexName mother = (IndexName)cboMother_.SelectedItem;

            // Save the index in the person object.
            person_.motherIndex = mother.index;
        }



        /// <summary>Update the image on the advance tab.</summary>
        private void showImage()
        {
            if (cboMainImage_.SelectedItem == null)
            {
                // Nothing to display.
                pictureboximage_.Image = null;
            }
            else
            {
                Media media = (Media)cboMainImage_.SelectedItem;

                // Open the specified image.
                Bitmap bitmap = null;
                try
                {
                    bitmap = new Bitmap(media.fullFileName);
                }
                catch
                {
                    bitmap = null;
                }
                if (bitmap == null)
                {
                    // Can't display this image.
                    pictureboximage_.Image = null;
                    return;
                }

                // Display this image.
                pictureboximage_.Image = bitmap;
            }
        }



        /// <summary>Message handler for the main image combo box changing value.</summary>
        private void cboMainImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Find the index the selected media.
            Media media = (Media)cboMainImage_.SelectedItem;

            // Save the index in the person object.
            person_.mediaIndex = media.index_;

            // Display the image on the form.
            showImage();
        }



        /// <summary>Message handler for the include in Gedcom check box value changing.</summary>
        private void chkGedcom_CheckedChanged(object sender, EventArgs e)
        {
            person_.isIncludeInGedcom = chkGedcom_.Checked;
        }



        #endregion



        private void cmdAddToDo_Click(object sender, EventArgs e)
        {
            ToDo toDo = new ToDo { personIndex_ = person_.index, priority = 50, description = "New ToDo item." };
            person_.addToDo(toDo);
            gridToDo_.SetDataBinding(person_.getToDo(), "");
        }



        private void cmdDeleteToDo_Click(object sender, System.EventArgs e)
        {
            // Check that a fact is selected in the grid.
            if (gridToDo_.CurrentCell.RowNumber < 0)
            {
                return;
            }

            // Find the to do.
            ToDo toDo = ((ToDo[])gridToDo_.DataSource)[gridToDo_.CurrentCell.RowNumber];
            toDo.delete();

            // Update the display.
            gridToDo_.SetDataBinding(person_.getToDo(), "");
        }



        /// <summary>Location of the last right click.</summary>
        DataGrid.HitTestInfo hitTestInfo_;



        private void menuEditLocation_Click(object sender, EventArgs e)
        {
            if (hitTestInfo_ == null)
            {
                return;
            }

            if (hitTestInfo_.Row < 0)
            {
                return;
            }

            // Find the fact.
            Fact fact = ((Fact[])gridFacts_.DataSource)[hitTestInfo_.Row];

            SelectLocationDialog selectLocationDialog = new SelectLocationDialog(person_.database, fact.information);
            if (selectLocationDialog.ShowDialog(this) == DialogResult.OK)
            {
                fact.information = selectLocationDialog.locationName;
                gridFacts_.Refresh();
            }
        }



        /// <summary>Message handler for the mouse up event of the facts grid.  Store the location of any right clicks for the context menu to use.</summary>
        private void gridFacts_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                hitTestInfo_ = gridFacts_.HitTest(e.X, e.Y);
            }
        }



        /// <summary>Message handler for the relationship location lookup button click.</summary>
        private void cmdRelationshipAddress_Click(object sender, EventArgs e)
        {
            SelectLocationDialog selectLocationDialog = new SelectLocationDialog(person_.database, txtRelationLocation_.Text);
            if (selectLocationDialog.ShowDialog(this) == DialogResult.OK)
            {
                txtRelationLocation_.Text = selectLocationDialog.locationName;
            }
        }



        #endregion

    }
}
