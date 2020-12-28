using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using FamilyTree.Objects;

namespace FamilyTree.Viewer
{
	/// <summary>
	/// Form to allow a person to be edited.
	/// </summary>
    public partial class frmEditPerson : System.Windows.Forms.Form
    {
		#region Member Variables

		/// <summary>Person object to edit.</summary>
		private clsPerson m_oPerson;

		/// <summary>Active sources.</summary>
		private clsSources m_oSources;

		/// <summary>Active relationship.</summary>
		private clsRelationship m_oActiveRelationship;

		#endregion

		#region Constructors

        /// Initialises the edit person dialog with the specified person in the specified database.
        /// <summary>
        /// Initialises the edit person dialog with the specified person in the specified database.
        /// </summary>
		/// <param name="nPersonID">ID of the person to edit.</param>
		/// <param name="oDB">Database to save the person into.</param>
        public frmEditPerson(int nPersonID, clsDatabase oDB)
        {
            // Required for Windows Form Designer support
            InitializeComponent();

            // Initialise the form
            clsFactType[] oFactTypes = oDB.GetFactTypes();
            for(int nI = 0; nI < oFactTypes.Length; nI++)
            {
                m_cboFactType.Items.Add(oFactTypes[nI]);
            }
            clsIDName[] oSources = oDB.GetSources(enumSortOrder.Date);
            for(int nI = 0; nI < oSources.Length; nI++)
            {
                m_cboSources.Items.Add(oSources[nI]);
            }

            // Create the person object to edit
            if(nPersonID == 0)
            {
                m_oPerson = new clsPerson(oDB);
            }
            else
            {
                m_oPerson = oDB.GetPerson(nPersonID);
            }

            // Update the values in the dialog
            if(m_oPerson.Male)
            {
                m_cboSex.SelectedIndex = 0;
            }
            else
            {
                m_cboSex.SelectedIndex = 1;
            }
            m_txtSurname.Text = m_oPerson.Surname;
            m_txtForename.Text = m_oPerson.Forenames;
            m_txtMaidenName.Text = m_oPerson.Maidenname;
            m_dateDoB.Value = m_oPerson.DoB;
            m_dateDoD.Value = m_oPerson.DoD;
            m_chkChildrenKnown.Checked = m_oPerson.AllChildrenKnown;
            m_txtComments.Text = m_oPerson.Comments;
            // m_labEditor.Text = "Last Edit by "+m_oPerson.LastEditBy+" on "+m_oPerson.LastEditDate.ToString("d-MMM-yyyy HH:mm:ss");

            // Initialise the sources grid
            RefreshSources(m_oPerson.SourceName, "Name");

            // Initialise the facts grid
            PopulateFactsGrid();

            // Update the description
            m_labDescription.Text = m_oPerson.Description(false, false, false, false, false);

            // Initialise the relationships tab
            clsRelationship[] oRelationships = m_oPerson.GetRelationships();
            for(int nI = 0; nI < oRelationships.Length; nI++)
            {
                m_lstRelationships.Items.Add(oRelationships[nI]);
            }

            // Add the possible people to the relationship combo box
            clsIDName[] oPossiblePartners = m_oPerson.PossiblePartners();
            for(int nI = 0; nI < oPossiblePartners.Length; nI++)
            {
                m_cboAddPartner.Items.Add(oPossiblePartners[nI]);
            }
        }

        /// Initialise the edit person dialog with a new person.
        /// <summary>
        /// Initialise the edit person dialog with a new person.
        /// </summary>
		/// <param name="oDB"></param>
		public frmEditPerson			(			clsDatabase oDB			):this((int)0,oDB)
		{
		}

        /// Clean up any resources being used.
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#endregion

        #region Supporting Functions

        /// <summary>
        /// Returns the ID of the person on the form.
        /// </summary>
        /// <returns>The ID of the person on the form.</returns>
        public int GetPersonID()
        {
            return m_oPerson.ID;
        }

        /// <summary>
        /// Populate the facts grid with the facts from the m_oPerson object
        /// </summary>
        /// <returns>True for success, false otherwise.</returns>
        private bool PopulateFactsGrid()
        {
            // Populate the grid with data
            m_gridFacts.SetDataBinding(m_oPerson.GetFacts(),"");

            // Creates two DataGridTableStyle objects, one for the Machine
            // array, and one for the Parts ArrayList.

            // Create a DataGridTabkeStyle object for the facts
            DataGridTableStyle oFactsTableStyle = new DataGridTableStyle();
            oFactsTableStyle.MappingName = "clsFact[]";

            // Sets the AlternatingBackColor so you can see the difference.
            oFactsTableStyle.AlternatingBackColor = System.Drawing.Color.LightBlue;

            // Creates a column for the rank
            DataGridTextBoxColumn columnRank = new DataGridTextBoxColumn();
            columnRank.MappingName = "Rank";
            columnRank.HeaderText = "Rank";
            columnRank.ReadOnly = false;
            columnRank.Width = 40;

            // Create a column for the type
            DataGridTextBoxColumn columnName = new DataGridTextBoxColumn();
            columnName.MappingName = "TypeName";
            columnName.HeaderText = "TypeName";
            columnName.ReadOnly = true;
            columnName.Width = 120;

            // Create a column for the information
            DataGridTextBoxColumn columnInfo = new DataGridTextBoxColumn();
            columnInfo.MappingName = "Information";
            columnInfo.HeaderText = "Information";
            columnInfo.Width = 350;

            /*
            DataGridTextBoxColumn columnUse = new DataGridTextBoxColumn();
            columnUse.MappingName= "UseInDescription";
            columnUse.HeaderText= "UseInDescription";
            columnUse.Width = 50;
            */

            // Adds the column styles to the grid table style.
            oFactsTableStyle.GridColumnStyles.Add(columnRank);
            oFactsTableStyle.GridColumnStyles.Add(columnName);
            oFactsTableStyle.GridColumnStyles.Add(columnInfo);

            // Add the table style to the collection, but clear the collection first.
            m_gridFacts.TableStyles.Clear();
            m_gridFacts.TableStyles.Add(oFactsTableStyle);

            // Return success
            return true;
        }

        #endregion

        #region Sources

        // Repopulate the sources grid with the specified sources.
        /// <summary>
        /// Repopulate the sources grid with the specified sources.
        /// The grid is labelled as belonging to the specified string.
        /// </summary>
        /// <param name="oSources">Specifiy the collection of sources to populate the grid with.</param>
        /// <param name="sOwner">Name of the owner to label the grid with.</param>
        private void RefreshSources(clsSources oSources, string sOwner)
        {
            RefreshSources(oSources);
            m_gridSources.CaptionText = "Sources for " + sOwner;
            m_grpSources.Text = "Sources for " + sOwner;
        }

        // Refresh the sources grid without changing the label.
        /// <summary>
        /// Refresh the sources grid without changing the label.
        /// Use as RefreshSources(m_oSources) to repaint the control
        /// </summary>
        /// <param name="oSources"></param>
        private void RefreshSources(clsSources oSources)
        {
            clsSource[] oListSources;
            m_oSources = oSources;
            if(oSources == null)
            {
                oListSources = new clsSource[0];
                m_gridSources.SetDataBinding(oListSources, "");
            }
            else
            {
                oListSources = oSources.GetAsSources();
                m_gridSources.SetDataBinding(oListSources, "");
                CreateSourcesGridStyle();
            }
        }

        // Clear the sources grid.
        /// <summary>
        /// Clear the sources grid.
        /// Use when the active control has no sources.
        /// </summary>
        private void RefreshSources()
        {
            RefreshSources((clsSources)null);
            m_gridSources.CaptionText = "Sources";
            m_grpSources.Text = "Sources";
        }

        /// <summary>
        /// Creates the style for the ToDo grid
        /// </summary>
        private void CreateToDoGridStyle()
        {
            // Define a style to apply to the ToDo grid
            DataGridTableStyle oToDoTableStyle = new DataGridTableStyle();
            
            // Sets the MappingName to the class name plus brackets.    
            oToDoTableStyle.MappingName = "clsToDo[]";

            // Sets the AlternatingBackColor so you can see the difference.
            oToDoTableStyle.AlternatingBackColor = System.Drawing.Color.LightBlue;

            // Create a column to hold the source
            DataGridTextBoxColumn columnPriority = new DataGridTextBoxColumn();
            columnPriority.MappingName = "Priority";
            columnPriority.HeaderText = "Ranking";
            columnPriority.ReadOnly = false;
            columnPriority.Width = 50;

            DataGridTextBoxColumn columnDescription = new DataGridTextBoxColumn();
            columnDescription.MappingName = "Description";
            columnDescription.HeaderText = "Description";
            columnDescription.ReadOnly = false;
            columnDescription.Width = 500;

            // Adds the column styles to the grid table style.
            oToDoTableStyle.GridColumnStyles.Add(columnPriority);
            oToDoTableStyle.GridColumnStyles.Add(columnDescription);

            // Add the table style to the collection, but clear the collection first.
            m_gridToDo.TableStyles.Clear();
            m_gridToDo.TableStyles.Add(oToDoTableStyle);
            m_gridToDo.ReadOnly = false;
        }
        
        /// <summary>
        /// Create the style for the sources grid
        /// </summary>
        private void CreateSourcesGridStyle()
        {
            // Creates two DataGridTableStyle objects, one for the Machine
            // array, and one for the Parts ArrayList.

            DataGridTableStyle oSourceTableStyle = new DataGridTableStyle();

            // Sets the MappingName to the class name plus brackets.    
            oSourceTableStyle.MappingName = "clsSource[]";

            // Sets the AlternatingBackColor so you can see the difference.
            oSourceTableStyle.AlternatingBackColor = System.Drawing.Color.LightBlue;

            // Create a column to hold the source
            DataGridTextBoxColumn columnSource = new DataGridTextBoxColumn();
            columnSource.MappingName = "Label";
            columnSource.HeaderText = "Source";
            columnSource.ReadOnly = true;
            columnSource.Width = 500;

            DataGridTextBoxColumn columnRank = new DataGridTextBoxColumn();
            columnRank.MappingName = "Ranking";
            columnRank.HeaderText = "Ranking";
            columnRank.ReadOnly = false;
            columnRank.Width = 50;

            // Adds the column styles to the grid table style.
            oSourceTableStyle.GridColumnStyles.Add(columnSource);
            oSourceTableStyle.GridColumnStyles.Add(columnRank);

            // Add the table style to the collection, but clear the collection first.
            m_gridSources.TableStyles.Clear();
            m_gridSources.TableStyles.Add(oSourceTableStyle);
            m_gridSources.ReadOnly = false;
        }

        #endregion

		#region Message Handlers

		#region Form Events

		/// <summary>
		/// Message handler for the Form load event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void frmEditPerson_Load(object sender, System.EventArgs e)
		{
			// Label for the dialog
			Text = m_oPerson.GetName(true,false);
		}

        /// <summary>
        /// Message handler for the form shown event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmEditPerson_Shown(object sender,EventArgs e)
        {
            // Initialise the editor combo with the possible editors
            string[] sEditors = m_oPerson.Database.GetEditors();
            foreach(string sEditor in sEditors)
            {
                m_cboEditor.Items.Add(sEditor);
                if(sEditor == "Steve Walton")
                {
                    m_cboEditor.SelectedIndex = m_cboEditor.Items.Count - 1;
                }
            }            
        }

		/// <summary>
		/// This is called when the user clicks the OK button.
		/// It saves the data on the form into a person record.
		/// If does not handle closing the form that is handled by .NET
		/// </summary>
		private void cmdOK_Click(object sender, System.EventArgs e)
		{
			// Save the record to the database
            m_oPerson.LastEditBy = m_cboEditor.SelectedItem.ToString();
			m_oPerson.Save();
		}

		/// <summary>
		/// This is called when the user clicks the add source button
		/// This adds a source to the active soruce collection.
		/// The active source is defined when each control adds it source to the dialog.  The last source is active
		/// </summary>
		private void cmdAddSource_Click(object sender, System.EventArgs e)
		{
			// Check that a source is selected
			if(m_cboSources.SelectedIndex<0)
			{
				return;
			}

			// Check that a sources object is available
			if(m_oSources==null)
			{
				return;
			}

			// Find the SourceID
			clsIDName oSource = (clsIDName)m_cboSources.Items[m_cboSources.SelectedIndex];

			// Add the source to this fact
			m_oSources.Add(oSource.ID);

			// Update the display
			RefreshSources(m_oSources);
			m_cboSources.SelectedIndex = -1;
		}

		private void cmdDeleteSource_Click(object sender, System.EventArgs e)
		{
			// Validate the active cell
			if(m_gridSources.CurrentCell.RowNumber<0)
			{
				return;
			}

			// Mark the source as deleted
			m_oSources.Delete(m_gridSources.CurrentCell.RowNumber);

			// Update the display
			RefreshSources(m_oSources);
		}

        /// <summary>
        /// Message handler for a drag-drop object passing over the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmEditPerson_DragEnter(object sender,System.Windows.Forms.DragEventArgs e)
        {
            // If the data is a file, display the copy cursor.
            if(e.Data.GetDataPresent(DataFormats.FileDrop))
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

        // Message handler for a control with the non specific source receiving the focus.
        /// <summary>
        /// Message handler for a control with the non specific source receiving the focus.
        /// Display the non specific sources for the person.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void evtNonSpecificSource(object sender,System.EventArgs e)
        {
            RefreshSources(m_oPerson.SourceNonSpecific,"Non Specific");
        }

        // Message handler for the sources grid losing the focus.
        /// <summary>
        /// Message handler for the sources grid losing the focus.
        /// Write any changes to the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridSources_Leave(object sender,System.EventArgs e)
        {
            if(m_oSources != null)
            {
                // Loop through the sources on the grid	
                clsSource[] oSources = (clsSource[])m_gridSources.DataSource;
                for(int nIndex = 0;nIndex < oSources.Length;nIndex++)
                {
                    if(oSources[nIndex] != null)
                    {
                        int nNewRanking = oSources[nIndex].Ranking;

                        if(m_oSources.GetRanking(nIndex) != nNewRanking)
                        {
                            // Show the message on the screen.
                            // MessageBox.Show(this,nIndex.ToString()+" = "+nNewRanking.ToString());		

                            // Change the ranking on this source		
                            m_oSources.ChangeRanking(nIndex,nNewRanking);
                        }
                    }
                }
            }
        }

        // Message handler for the tab control changing the active tab.
        /// <summary>
        /// Message handler for the tab control changing the active tab.
        /// Populate the Advanced tab if it is displayed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            switch(m_TabControl.SelectedIndex)
            {
            case 1:
                // Force the focus on the first spouse on the relationship tab
                if(m_lstRelationships.Items.Count > 0)
                {
                    if(m_lstRelationships.SelectedIndex < 0)
                    {
                        m_lstRelationships.SelectedIndex = 0;
                    }
                }
                break;

            case 3:
                // Populate the advance tab
                if(m_cboFather.Items.Count == 0)
                {
                    clsIDName[] oPossibleFathers = m_oPerson.PossibleFathers();
                    for(int nI = 0; nI < oPossibleFathers.Length; nI++)
                    {
                        m_cboFather.Items.Add(oPossibleFathers[nI]);
                        if(oPossibleFathers[nI].ID == m_oPerson.FatherID)
                        {
                            m_cboFather.SelectedItem = oPossibleFathers[nI];
                        }
                    }
                }
                if(m_cboMother.Items.Count == 0)
                {
                    clsIDName[] oPossibleMothers = m_oPerson.PossibleMothers();
                    for(int nI = 0; nI < oPossibleMothers.Length; nI++)
                    {
                        m_cboMother.Items.Add(oPossibleMothers[nI]);
                        if(oPossibleMothers[nI].ID == m_oPerson.MotherID)
                        {
                            m_cboMother.SelectedItem = oPossibleMothers[nI];
                        }
                    }
                }

                if(m_cboMainImage.Items.Count == 0)
                {
                    clsMedia[] oMedias = m_oPerson.GetMedia(false);
                    foreach(clsMedia oMedia in oMedias)
                    {
                        m_cboMainImage.Items.Add(oMedia);
                        if(oMedia.ID == m_oPerson.MediaID)
                        {
                            m_cboMainImage.SelectedIndex = m_cboMainImage.Items.Count - 1;
                        }
                    }
                }
                m_chkGedcom.Checked = m_oPerson.IncludeInGedcom;
                break;

            case 4:
                //Populate the ToDo List                
                if(m_gridToDo.DataSource == null)
                {
                    m_gridToDo.SetDataBinding(m_oPerson.GetToDo(), "");
                    CreateToDoGridStyle();
                }
                break;
            }
        }

        #endregion

        #region Controls on the Basic Tab

        // Update the sources when the any of the name fields are active.
        /// <summary>
        /// Update the sources when the any of the name fields are active.
        /// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Name_Enter(object sender, System.EventArgs e)
		{
			RefreshSources(m_oPerson.SourceName,"Name");
		}

        // Update the sources when the sex field is active.
        /// <summary>
        /// Update the sources when the sex field is active.
        /// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cboSex_Enter(object sender, System.EventArgs e)
		{
            RefreshSources();
		}

        // Update the sources when the DoB field is active.
        /// <summary>
        /// Update the sources when the DoB field is active.
        /// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void dateDoB_Enter(object sender, System.EventArgs e)
		{
			RefreshSources(m_oPerson.SourceDoB,"Date of Birth");
		}

        // Update the sources when the DoD field is active.
        /// <summary>
        /// Update the sources when the DoD field is active.
        /// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void dateDoD_Enter(object sender, System.EventArgs e)
		{
			RefreshSources(m_oPerson.SourceDoD,"Date of Death");
		}

        // Message handler for the surname changing.
        /// <summary>
        /// Message handler for the surname changing.
        /// Update the person object and display the person description.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void txtSurname_TextChanged(object sender, System.EventArgs e)
		{
			m_oPerson.Surname = this.m_txtSurname.Text;

			// Update the description
            m_labDescription.Text = m_oPerson.Description(false, false, false, false, false);
		}

        // Message handler for the forename changing.
        /// <summary>
        /// Message handler for the forename changing.
        /// Update the person object and display the person description.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void txtForename_TextChanged(object sender, System.EventArgs e)
		{
			m_oPerson.Forenames = this.m_txtForename.Text;

			// Update the description
            m_labDescription.Text = m_oPerson.Description(false, false, false, false, false);
		}

        // Message handler for the maiden name changing.
        /// <summary>
        /// Message handler for the maiden name changing.
        /// Update the person object and display the person description.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>		
		private void txtMaidenName_TextChanged(object sender, System.EventArgs e)
		{
			m_oPerson.Maidenname = this.m_txtMaidenName.Text;

			// Update the description
            m_labDescription.Text = m_oPerson.Description(false, false, false, false, false);
		}

        // Message handler for the sex of the person changing.
        /// <summary>
        /// Message handler for the sex of the person changing.
        /// Update the person object and display the person description.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cboSex_SelectedValueChanged(object sender, System.EventArgs e)
		{
			if(m_cboSex.SelectedIndex==0)
			{
				m_oPerson.Male = true;
                m_labMaidenName.Visible = false;
                m_txtMaidenName.Visible = false;
            }
			else
			{
				m_oPerson.Female = true;
                m_labMaidenName.Visible = true;
                m_txtMaidenName.Visible = true;
			}

			// Update the description
            m_labDescription.Text = m_oPerson.Description(false, false, false, false, false);
		}

        // Message handler for the all children known check box changing value.
        /// <summary>
        /// Message handler for the all children known check box changing value.
        /// Update the person object and display the person description.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void chkChildrenKnown_CheckedChanged(object sender, System.EventArgs e)
		{
			m_oPerson.AllChildrenKnown = this.m_chkChildrenKnown.Checked;

			// Update the description
            m_labDescription.Text = m_oPerson.Description(false, false, false, false, false);
		}

        // Message handler for the date of birth control changing value.
        /// <summary>
        /// Message handler for the date of birth control changing value.
        /// Update the person object and display the person description.
		/// </summary>
		/// <param name="oSender"></param>
		private void dateDoB_evtValueChanged(object oSender)
		{
			m_oPerson.DoB.Date = m_dateDoB.GetDate();
			m_oPerson.DoB.Status = m_dateDoB.GetStatus();

			// Update the description
            m_labDescription.Text = m_oPerson.Description(false, false, false, false, false);
		}

        // Message handler for the date of death control changing value.
        /// <summary>
        /// Message handler for the date of death control changing value.
        /// Update the person object and display the person description.
		/// </summary>
		/// <param name="oSender"></param>
		private void dateDoD_evtValueChanged(object oSender)
		{
			m_oPerson.DoD.Date = m_dateDoD.GetDate();
			m_oPerson.DoD.Status = m_dateDoD.GetStatus();

			// Update the description
            m_labDescription.Text = m_oPerson.Description(false, false, false, false, false);
		}

        // Message handler for the comments text box contents changing.
        /// <summary>
        /// Message handler for the comments text box contents changing.
        /// Update the comments property of the person object.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void txtComments_TextChanged(object sender, System.EventArgs e)
		{
			m_oPerson.Comments = this.m_txtComments.Text;
		}

		#endregion

		#region Controls on the Facts Tab

		/// <summary>
		/// This is called when the current cell changes within the facts grid and when the grid first gets the focus
		/// </summary>
		private void gridFacts_CurrentCellChanged(object sender, System.EventArgs e)
		{			
			clsFact[] oFacts = (clsFact[])m_gridFacts.DataSource; 
			RefreshSources(oFacts[m_gridFacts.CurrentCell.RowNumber].Sources,oFacts[m_gridFacts.CurrentCell.RowNumber].Information);

			// Update the description
            m_labDescription.Text = m_oPerson.Description(false, false, false, false, false);
		}

		/// <summary>
		/// This is called when the user clicks the Add Fact button.
		/// This adds a fact to the form control (not the person)
		/// </summary>
		private void cmdAddFact_Click(object sender, System.EventArgs e)
		{
			// Check that a fact type is selected
			if(m_cboFactType.SelectedIndex<0)
			{
				return;
			}

			// Find the fact type
			clsFactType oFactType = (clsFactType)m_cboFactType.Items[m_cboFactType.SelectedIndex];
			
			// Count the number of existing facts to get an initial rank for the new fact
			clsFact[] oFacts = (clsFact[])m_gridFacts.DataSource;
			int nNewRank = oFacts.Length+1;
			
			// Create the new fact			
			clsFact oFact = new clsFact(0,m_oPerson,oFactType.ID,nNewRank,"Empty");

			// Add the fact	to the person
			m_oPerson.AddFact(oFact);

			// Update the display
			PopulateFactsGrid();
		}

        private void cmdDeleteFact_Click(object sender,System.EventArgs e)
        {
            // Check that a fact is selected in the grid
            if(m_gridFacts.CurrentCell.RowNumber < 0)
            {
                return;
            }

            // Find the fact
            clsFact oFact = ((clsFact[])m_gridFacts.DataSource)[m_gridFacts.CurrentCell.RowNumber];
            oFact.Delete();

            // Update the display
            PopulateFactsGrid();
        }

		#endregion

		#region Controls on the Relationships tab

        /// <summary>
        /// Message handler for the marriage terminated combo box getting the focus.
        /// Display the sources for the marriage termination status.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboTerminated_Enter(object sender,System.EventArgs e)
        {
            if(m_oActiveRelationship == null)
            {
                RefreshSources();
            }
            else
            {
                RefreshSources(m_oActiveRelationship.SourceTerminated,"Relationship End Type");
            }
        }

        /// <summary>
        /// Message handler for the marriage terminated combo box chaning value,
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboTerminated_SelectedIndexChanged(object sender,System.EventArgs e)
        {
            // Update the active relationship
            if(m_oActiveRelationship != null)
            {
                m_oActiveRelationship.TerminatedID = this.m_cboTerminated.SelectedIndex + 1;
                m_oActiveRelationship.LastEditBy = m_cboEditor.SelectedItem.ToString();
            }
        }

        /// <summary>
        /// Message handler for the relationship type value change event.
        /// Update the value in the active relationship.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboRelationshipType_SelectedIndexChanged(object sender,System.EventArgs e)
        {
            // Update the active relationship
            if(m_oActiveRelationship != null)
            {
                m_oActiveRelationship.TypeID = m_cboRelationshipType.SelectedIndex + 1;
                m_oActiveRelationship.LastEditBy = m_cboEditor.SelectedItem.ToString();
            }
        }

        /// <summary>
        /// Message handler for the relationship type getting the focus.
        /// Do not record the source for this information (currently) so just clear the sources area.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_cboRelationshipType_Enter(object sender,System.EventArgs e)
        {
            RefreshSources();
        }

		private void lstRelationships_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			// Validate the selected index
			if(this.m_lstRelationships.SelectedIndex<0)
			{				
				m_oActiveRelationship = null;				
				return;
			}

			// Get the selected relationship
			m_oActiveRelationship = (clsRelationship)this.m_lstRelationships.SelectedItem;

			// Update the form
			m_dateRelationStart.Value = m_oActiveRelationship.Start;
//			this.chkTerminated.Checked = m_oActiveRelationship.Terminated;
			m_cboTerminated.SelectedIndex = m_oActiveRelationship.TerminatedID-1;
			m_txtRelationLocation.Text = m_oActiveRelationship.Location;
			m_dateRelationEnd.Value = m_oActiveRelationship.End;
			m_txtRelationComments.Text = m_oActiveRelationship.Comments;
			m_cboRelationshipType.SelectedIndex = m_oActiveRelationship.TypeID-1;

			RefreshSources(m_oActiveRelationship.SourcePartner,"Relationship Partner");		
		}

		private void dateRelationStart_Enter(object sender, System.EventArgs e)
		{
			if(m_oActiveRelationship==null)
			{
				RefreshSources();		
			}
			else
			{
				RefreshSources(m_oActiveRelationship.SourceStart,"Relationship Start Date");		
			}
		}

		private void chkTerminated_Enter(object sender, System.EventArgs e)
		{
			if(m_oActiveRelationship==null)
			{
				RefreshSources();		
			}
			else
			{
				RefreshSources(m_oActiveRelationship.SourceTerminated,"Relationship Termination Status");		
			}				
		}

		private void txtRelationLocation_Enter(object sender, System.EventArgs e)
		{
			if(m_oActiveRelationship==null)
			{
				RefreshSources();		
			}
			else
			{
				RefreshSources(m_oActiveRelationship.SourceLocation,"Relationship Location");		
			}		
		}

		private void dateRelationEnd_Enter(object sender, System.EventArgs e)
		{
			if(m_oActiveRelationship==null)
			{
				RefreshSources();		
			}
			else
			{
				RefreshSources(m_oActiveRelationship.SourceEnd,"Relationship End Date");		
			}		
		}

		private void lstRelationships_Enter(object sender, System.EventArgs e)
		{
			if(m_oActiveRelationship==null)
			{
				RefreshSources();		
			}
			else
			{
				RefreshSources(m_oActiveRelationship.SourcePartner,"Relationship Partner");		
			}				
		}

		private void txtRelationLocation_TextChanged(object sender, System.EventArgs e)
		{
			// Update the active relationship
			if(m_oActiveRelationship!=null)
			{
				m_oActiveRelationship.Location = this.m_txtRelationLocation.Text;
                m_oActiveRelationship.LastEditBy = m_cboEditor.SelectedItem.ToString();

				// Update the description
                m_labDescription.Text = m_oPerson.Description(false, false, false, false, false);
			}		
		}

        private void dateRelationStart_evtValueChanged(object oSender)
        {
            // Update the active relationship
            if(m_oActiveRelationship != null)
            {
                m_oActiveRelationship.Start.Date = m_dateRelationStart.GetDate();
                m_oActiveRelationship.Start.Status = m_dateRelationStart.GetStatus();
                m_oActiveRelationship.LastEditBy = m_cboEditor.SelectedItem.ToString();

                // Update the description
                m_labDescription.Text = m_oPerson.Description(false, false, false, false, false);
            }
        }

		private void dateRelationEnd_evtValueChanged(object oSender)
		{
			// Update the active relationship
			if(m_oActiveRelationship!=null)
			{
				m_oActiveRelationship.End.Date = m_dateRelationEnd.GetDate();
				m_oActiveRelationship.End.Status = m_dateRelationEnd.GetStatus();
                m_oActiveRelationship.LastEditBy = m_cboEditor.SelectedItem.ToString();

				// Update the description
                m_labDescription.Text = m_oPerson.Description(false, false, false, false, false);
			}						
		}

		private void txtRelationComments_TextChanged(object sender, System.EventArgs e)
		{
			// Update the active relationship
			if(m_oActiveRelationship!=null)
			{
				m_oActiveRelationship.Comments = this.m_txtRelationComments.Text;
                m_oActiveRelationship.LastEditBy = m_cboEditor.SelectedItem.ToString();
			}								
		}
		
		private void AddRelationship_Click(object sender, System.EventArgs e)
		{
			// Check that a person is selected
			if(m_cboAddPartner.SelectedIndex==-1)
			{
				return;
			}

			// Find the person that is selected
			clsIDName oPartner = (clsIDName)m_cboAddPartner.SelectedItem;

			// Create a relationship object
			clsRelationship oRelationship = new clsRelationship(m_oPerson,oPartner.ID);

			// Add the relationship to the persons collection
			m_oPerson.AddRelationship(oRelationship);

			// Add the partner to the list box
			m_lstRelationships.Items.Add(oRelationship);			

			// Update the description
            m_labDescription.Text = m_oPerson.Description(false, false, false, false, false);
		}

		private void cmdDeleteRelationship_Click(object sender, System.EventArgs e)
		{
			// Get the selected relationship
			if(m_oActiveRelationship==null)
			{
				return;
			}

			// Mark the active relationship for deletion
			m_oActiveRelationship.Delete();			

			// Remove the relationship from the listbox
			this.m_lstRelationships.Items.Remove(m_oActiveRelationship);

			// No selection any more
			m_oActiveRelationship = null;

			// Update the description
            m_labDescription.Text = m_oPerson.Description(false, false, false, false, false);
		}

		#endregion

		#region Controls on the Advance tab

		/// <summary>
		/// Message handler for the father combo box changing value.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void cboFather_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			// Find the ID of the selected father
			clsIDName oFather = (clsIDName)m_cboFather.SelectedItem;

			// Save the ID in the person object
			m_oPerson.FatherID = oFather.ID;
		}

		/// <summary>
		/// Message handler for the mother combo box changing value.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void cboMother_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            // Find the ID the selected mother
            clsIDName oMother = (clsIDName)m_cboMother.SelectedItem;

            // Save the ID in the person object
            m_oPerson.MotherID = oMother.ID;
        }

		/// <summary>
		/// Update the image on the advance tab.
		/// </summary>
        private void ShowImage()
		{
            if(m_cboMainImage.SelectedItem == null)
            {
                // Nothing to display
                m_Image.Image = null;
            }
            else
            {
                clsMedia oMedia = (clsMedia)m_cboMainImage.SelectedItem;

                // Open the specified image
                Bitmap oImage = null;
                try
                {
                    oImage = new Bitmap(oMedia.FullFilename);
                }
                catch
                {
                    oImage = null;
                }
                if(oImage == null)
                {
                    // Can't display this image
                    m_Image.Image = null;
                    return;
                }

                // Display this image
                m_Image.Image = oImage;
            }
		}

        /// <summary>
        /// Message handler for the main image combo box changing value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboMainImage_SelectedIndexChanged(object sender,EventArgs e)
        {
            // Find the ID the selected media
            clsMedia oMedia = (clsMedia)m_cboMainImage.SelectedItem;

            // Save the ID in the person object
            m_oPerson.MediaID = oMedia.ID;

            // Display the image on the form
            ShowImage();
        }

        // Message handler for the include in Gedcom check box value changing.
        /// <summary>
        /// Message handler for the include in Gedcom check box value changing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkGedcom_CheckedChanged(object sender, EventArgs e)
        {
            m_oPerson.IncludeInGedcom = m_chkGedcom.Checked;
        }

        #endregion

        private void cmdAddToDo_Click(object sender,EventArgs e)
        {
            clsToDo oNew = new clsToDo();
            oNew.PersonID = m_oPerson.ID;
            oNew.Priority = 50;
            oNew.Description = "New ToDo item.";

            m_oPerson.AddToDo(oNew);

            m_gridToDo.SetDataBinding(m_oPerson.GetToDo(),"");                
        }

        private void cmdDeleteToDo_Click(object sender,System.EventArgs e)
        {
            // Check that a fact is selected in the grid
            if(m_gridToDo.CurrentCell.RowNumber < 0)
            {
                return;
            }

            // Find the fact
            clsToDo oToDo = ((clsToDo[])m_gridToDo.DataSource)[m_gridToDo.CurrentCell.RowNumber];
            oToDo.Delete();            

            // Update the display
            m_gridToDo.SetDataBinding(m_oPerson.GetToDo(),"");                
        }


        #endregion

        // Location of the last right click.
        /// <summary>
        /// Location of the last right click.
        /// </summary>
        DataGrid.HitTestInfo oHitTestInfo;

        private void menuEditLocation_Click(object sender, EventArgs e)
        {
            if(oHitTestInfo == null)
            {
                return;
            }

            if(oHitTestInfo.Row < 0)
            {
                return;
            }

            // Find the fact
            clsFact oFact = ((clsFact[])m_gridFacts.DataSource)[oHitTestInfo.Row];

            frmSelectLocation oDialog = new frmSelectLocation(m_oPerson.Database, oFact.Information);
            if(oDialog.ShowDialog(this) == DialogResult.OK)
            {
                oFact.Information = oDialog.LocationName;
                m_gridFacts.Refresh();
            }
        }

        // Message handler for the mouse up event of the facts grid.
        /// <summary>
        /// Message handler for the mouse up event of the facts grid.
        /// Store the location of any right clicks for the context menu to use.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_gridFacts_MouseUp(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                oHitTestInfo = m_gridFacts.HitTest(e.X, e.Y);
            }
        }

        // Message handler for the relationship location lookup button click.
        /// <summary>
        /// Message handler for the relationship location lookup button click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdRelationshipAddress_Click(object sender, EventArgs e)
        {
            frmSelectLocation oDialog = new frmSelectLocation(m_oPerson.Database, m_txtRelationLocation.Text);
            if(oDialog.ShowDialog(this) == DialogResult.OK)
            {
                m_txtRelationLocation.Text = oDialog.LocationName;
            }
        }

    }
}
