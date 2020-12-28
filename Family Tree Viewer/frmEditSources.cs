using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

// Family tree objects
using FamilyTree.Objects;

namespace FamilyTree.Viewer
{
	/// <summary>
	/// Dialog to allow the user to edit the complete list of sources.
	/// </summary>
    public partial class frmEditSources : System.Windows.Forms.Form
    {
		#region Member Variables

		/// <summary>Database that to edit the sources in.</summary>
		private clsDatabase m_oDb;

		/// <summary>The source that we are currently editing.</summary>
		private clsSource m_ActiveSource;

		/// <summary>True when we are allowing events.</summary>
		private bool m_bAllowEvents;

		#endregion

		#region Constructors etc ...

        // Class constructor.
        /// <summary>
        /// Class constructor.
        /// Specify the specific source to start editting.
		/// </summary>
        /// <param name="oDb">Specify the database to show the sources from.</param>
        /// <param name="nSourceID">Specify the ID of the source to edit initially.</param>
        public frmEditSources(clsDatabase oDb, int nSourceID)
        {
            // Required for Windows Form Designer support
            InitializeComponent();

            // Initialise member variables
            m_oDb = oDb;

            // Show the additional information types
            clsIDName[] oSources = m_oDb.GetSourceAdditionalTypes();
            foreach(clsIDName oAdditional in oSources)
            {
                m_cboAdditionalInfo.Items.Add(oAdditional);
            }

            // Add the repositories
            oSources = m_oDb.GetRepositories();
            foreach(clsIDName oRepository in oSources)
            {
                m_cboRepository.Items.Add(oRepository);
            }

            // Add the sources to the dialog box
            clsSource oSelected = null;
            oSources = oDb.GetSources(enumSortOrder.Date);
            for(int nI = 0; nI < oSources.Length; nI++)
            {
                clsSource oSource = new clsSource(m_oDb, oSources[nI].ID);
                m_lstSources.Items.Add(oSource);
                if(oSources[nI].ID == nSourceID)
                {
                    oSelected = oSource;
                }
            }

            // Select the specified source
            if(oSelected != null)
            {
                m_lstSources.SelectedItem = oSelected;
            }

            // Allow events
            m_bAllowEvents = true;
        }

        /// <summary>
        /// Class constructor
        /// Open the edit sources dialog box without specifing a particular source to edit.
        /// </summary>
        /// <param name="oDb">Specify the database to show the sources from.</param>
        public frmEditSources
        (
            clsDatabase oDb
        ) : this(oDb,0)
        {
        }


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

        #region Additional Information / Form Specific Information

        /// <summary>
        /// Display the optional additional information for this source.
        /// </summary>
        private void ShowAdditionalInfo()
        {
            // Disable events
            bool bEvents = m_bAllowEvents;
            m_bAllowEvents = false;

            // Activate the additional information
            switch(m_ActiveSource.AdditionalInfoTypeID)
            {
            case 0: // None
                HideAllAditionalInfo();
                break;
            case 1:
                m_grpBirth.Visible = true;
                m_grpCensus.Visible = false;
                m_grpDeath.Visible = false;
                m_grpMarriage.Visible = false;
                if(m_ActiveSource.AdditionalBirth == null)
                {
                    m_txtBirthDistrict.Text = "";
                    m_dtpBirthWhen.Value = DateTime.Now;
                    m_txtBirthWhenWhere.Text = "";
                    m_txtBirthName.Text = "";
                    m_txtBirthSex.Text = "";
                    m_txtBirthFather.Text = "";
                    m_txtBirthFatherOccupation.Text = "";
                    m_txtBirthMother.Text = "";
                    m_txtBirthMotherDetails.Text = "";
                    m_txtBirthInformant.Text = "";
                    m_txtBirthInformantAddress.Text = "";
                    m_txtBirthWhenReg.Text = "";
                    m_txtBirthReference.Text = "";
                }
                else
                {
                    m_txtBirthDistrict.Text = m_ActiveSource.AdditionalBirth.RegistrationDistrict;
                    try
                    {
                        m_dtpBirthWhen.Value = m_ActiveSource.AdditionalBirth.When;
                    }
                    catch { }
                    m_txtBirthWhenWhere.Text = m_ActiveSource.AdditionalBirth.WhenAndWhere;
                    m_txtBirthName.Text = m_ActiveSource.AdditionalBirth.Name;
                    m_txtBirthSex.Text = m_ActiveSource.AdditionalBirth.Sex;
                    m_txtBirthFather.Text = m_ActiveSource.AdditionalBirth.Father;
                    m_txtBirthFatherOccupation.Text = m_ActiveSource.AdditionalBirth.FatherOccupation;
                    m_txtBirthMother.Text = m_ActiveSource.AdditionalBirth.Mother;
                    m_txtBirthMotherDetails.Text = m_ActiveSource.AdditionalBirth.MotherDetails;
                    m_txtBirthInformant.Text = m_ActiveSource.AdditionalBirth.Informant;
                    m_txtBirthInformantAddress.Text = m_ActiveSource.AdditionalBirth.InformantAddress;
                    m_txtBirthWhenReg.Text = m_ActiveSource.AdditionalBirth.WhenRegistered;
                    m_txtBirthReference.Text = m_ActiveSource.AdditionalBirth.GroReference;
                }
                break;

            case 2:
                m_grpBirth.Visible = false;
                m_grpCensus.Visible = false;
                m_grpDeath.Visible = false;
                m_grpMarriage.Visible = true;

                if(m_ActiveSource.AdditionalMarriage == null)
                {
                    m_dtpMarrWhen.Value = DateTime.Now;
                    m_txtMarrLocation.Text = "";
                    m_txtMarrGroom.Text = "";
                    m_txtMarrGroomAge.Text = "";
                    m_txtMarrGroomOccu.Text = "";
                    m_txtMarrGroomLoca.Text = "";
                    m_txtMarrGroomFather.Text = "";
                    m_txtMarrGroomFatherOcc.Text = "";
                    m_txtMarrBride.Text = "";
                    m_txtMarrBrideAge.Text = "";
                    m_txtMarrBrideOccu.Text = "";
                    m_txtMarrBrideLoca.Text = "";
                    m_txtMarrBrideFather.Text = "";
                    m_txtMarrBrideFatherOcc.Text = "";
                    m_txtMarrWitness.Text = "";
                    m_txtMarrGro.Text = "";
                }
                else
                {
                    try
                    {
                        m_dtpMarrWhen.Value = m_ActiveSource.AdditionalMarriage.When;
                    }
                    catch { }
                    m_txtMarrLocation.Text = m_ActiveSource.AdditionalMarriage.Location;
                    m_txtMarrGroom.Text = m_ActiveSource.AdditionalMarriage.GroomName;
                    m_txtMarrGroomAge.Text = m_ActiveSource.AdditionalMarriage.GroomAge;
                    m_txtMarrGroomOccu.Text = m_ActiveSource.AdditionalMarriage.GroomOccupation;
                    m_txtMarrGroomLoca.Text = m_ActiveSource.AdditionalMarriage.GroomLiving;
                    m_txtMarrGroomFather.Text = m_ActiveSource.AdditionalMarriage.GroomFather;
                    m_txtMarrGroomFatherOcc.Text = m_ActiveSource.AdditionalMarriage.GroomFatherOccupation;
                    m_txtMarrBride.Text = m_ActiveSource.AdditionalMarriage.BrideName;
                    m_txtMarrBrideAge.Text = m_ActiveSource.AdditionalMarriage.BrideAge;
                    m_txtMarrBrideOccu.Text = m_ActiveSource.AdditionalMarriage.BrideOccupation;
                    m_txtMarrBrideLoca.Text = m_ActiveSource.AdditionalMarriage.BrideLiving;
                    m_txtMarrBrideFather.Text = m_ActiveSource.AdditionalMarriage.BrideFather;
                    m_txtMarrBrideFatherOcc.Text = m_ActiveSource.AdditionalMarriage.BrideFatherOccupation;
                    m_txtMarrWitness.Text = m_ActiveSource.AdditionalMarriage.Witness;
                    m_txtMarrGro.Text = m_ActiveSource.AdditionalMarriage.GroReference;
                }
                break;

            case 3:
                m_grpBirth.Visible = false;
                m_grpCensus.Visible = false;
                m_grpDeath.Visible = true;
                m_grpMarriage.Visible = false;
                if(m_ActiveSource.AdditionalDeath == null)
                {
                    m_txtDeathDistrict.Text = "";
                    m_txtDeathWhen.Text = "";
                    m_txtDeathWhere.Text = "";
                    m_txtDeathName.Text = "";
                    m_txtDeathSex.Text = "";
                    m_txtDeathDatePlace.Text = "";
                    m_txtDeathOccupation.Text = "";
                    m_txtDeathUsualAddress.Text = "";
                    m_txtDeathCause.Text = "";
                    m_txtDeathInformant.Text = "";
                    m_txtDeathInformantDescription.Text = "";
                    m_txtDeathInformantAddress.Text = "";
                    m_txtDeathWhenReg.Text = "";
                    m_txtDeathReference.Text = "";
                }
                else
                {
                    m_txtDeathDistrict.Text = m_ActiveSource.AdditionalDeath.RegistrationDistrict;
                    m_txtDeathWhen.Text = m_ActiveSource.AdditionalDeath.When;
                    m_txtDeathWhere.Text = m_ActiveSource.AdditionalDeath.Place;
                    m_txtDeathName.Text = m_ActiveSource.AdditionalDeath.Name;
                    m_txtDeathSex.Text = m_ActiveSource.AdditionalDeath.Sex;
                    m_txtDeathDatePlace.Text = m_ActiveSource.AdditionalDeath.DatePlaceOfBirth;
                    m_txtDeathOccupation.Text = m_ActiveSource.AdditionalDeath.Occupation;
                    m_txtDeathUsualAddress.Text = m_ActiveSource.AdditionalDeath.UsualAddress;
                    m_txtDeathCause.Text = m_ActiveSource.AdditionalDeath.CauseOfDeath;
                    m_txtDeathInformant.Text = m_ActiveSource.AdditionalDeath.Informant;
                    m_txtDeathInformantDescription.Text = m_ActiveSource.AdditionalDeath.InformantDescription;
                    m_txtDeathInformantAddress.Text = m_ActiveSource.AdditionalDeath.InformantAddress;
                    m_txtDeathWhenReg.Text = m_ActiveSource.AdditionalDeath.WhenRegistered;
                    m_txtDeathReference.Text = m_ActiveSource.AdditionalDeath.GroReference;
                }
                break;

            case 4:
                m_grpBirth.Visible = false;
                m_grpCensus.Visible = true;
                m_grpDeath.Visible = false;
                m_grpMarriage.Visible = false;

                if(m_ActiveSource.AdditionalCensus == null)
                {
                    m_txtCensusAddress.Text = "Null";
                    m_txtCensusSeries.Text = "Null";
                    m_txtCensusPiece.Text = "Null";
                    m_txtCensusFolio.Text = "Null";
                    m_txtCensusPage.Text = "Null";
                }
                else
                {
                    m_txtCensusAddress.Text = m_ActiveSource.AdditionalCensus.Address;
                    m_txtCensusSeries.Text = m_ActiveSource.AdditionalCensus.Series;
                    m_txtCensusPiece.Text = m_ActiveSource.AdditionalCensus.Piece;
                    m_txtCensusFolio.Text = m_ActiveSource.AdditionalCensus.Folio;
                    m_txtCensusPage.Text = m_ActiveSource.AdditionalCensus.Page;
                }
                break;
            }

            // Enable events
            m_bAllowEvents = bEvents;
        }

        /// <summary>
        /// Hide all the optional information sections.
        /// </summary>
        private void HideAllAditionalInfo()
        {
            m_grpBirth.Visible = false;
            m_grpCensus.Visible = false;
            m_grpDeath.Visible = false;
            m_grpMarriage.Visible = false;
        }

        #endregion

		#region Message Handlers

        #region Form Events

        // Message handler for the form shown event.
        /// <summary>
        /// Message handler for the form shown event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmEditSources_Shown(object sender, EventArgs e)
        {
            // Select the first source if nothing is already selected
            if(m_lstSources.SelectedIndex < 0)
            {
                m_lstSources.SelectedIndex = 0;
            }
        }

        #endregion

        private void radioDate_CheckedChanged(object sender, System.EventArgs e)
		{
			if(this.radioDate.Checked)
			{
				this.m_lstSources.Sorted = false;
				/*
				clsIDName[] oSources = m_oDb.GetSources(enumSortOrder.Date);
				this.lstSources.Items.Clear();
				for(int nI=0;nI<oSources.Length;nI++)
				{
					this.lstSources.Items.Add(oSources[nI]);
				}
				*/
			}
		}

		private void radioAlpha_CheckedChanged(object sender, System.EventArgs e)
		{
			if(this.radioAlpha.Checked)
			{
				this.m_lstSources.Sorted = true;
				/*
				clsIDName[] oSources = m_oDb.GetSources(enumSortOrder.Alphabetical);
				this.lstSources.Items.Clear();
				for(int nI=0;nI<oSources.Length;nI++)
				{
					this.lstSources.Items.Add(oSources[nI]);
				}
				*/
			}
		}

		/// <summary>
		/// Message handler for the selection on the list of sources changing.
		/// Load and display the selected source object.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void lstSources_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			m_ActiveSource = (clsSource)this.m_lstSources.SelectedItem;
			if(m_ActiveSource==null)
			{
				// Clear the controls
			}
			else if(m_ActiveSource.IsValid())
			{
				// Populate the controls
				this.txtDescription.Text = m_ActiveSource.Description;
				this.dateTheDate.Value = m_ActiveSource.TheDate;
				this.txtComments.Text = m_ActiveSource.Comments;
				m_cboAdditionalInfo.SelectedIndex = m_ActiveSource.AdditionalInfoTypeID;
				m_cboRepository.SelectedIndex = m_ActiveSource.Repository; // This is not really correct

				DataGridTableStyle oTableStyle = new DataGridTableStyle();
			
				// Sets the MappingName to the class name plus brackets.    
				oTableStyle.MappingName= "clsReferences[]";

				// Sets the AlternatingBackColor so you can see the difference.
				oTableStyle.AlternatingBackColor= System.Drawing.Color.LightBlue;

				// Creates 2 column styles.
				DataGridTextBoxColumn columnPerson = new DataGridTextBoxColumn();
				columnPerson.MappingName= "PersonName";
				columnPerson.HeaderText= "Person";
				columnPerson.ReadOnly = true;
				columnPerson.Width = 250;

				DataGridTextBoxColumn columnReferences = new DataGridTextBoxColumn();
				columnReferences.MappingName= "References";
				columnReferences.HeaderText= "References";
				columnReferences.ReadOnly = true;
				columnReferences.Width = 500;

				// Adds the column styles to the grid table style.
				oTableStyle.GridColumnStyles.Add(columnPerson);
				oTableStyle.GridColumnStyles.Add(columnReferences);

				// Add the table style to the collection, but clear the collection first.
				this.gridReferences.TableStyles.Clear();
				this.gridReferences.TableStyles.Add(oTableStyle);
				this.gridReferences.ReadOnly = true;

				// Update the references to this source
				clsReferences[] oReferences = m_ActiveSource.GetReferences();
				this.gridReferences.SetDataBinding(oReferences,"");

				// Show the additional information for this source (birth certificate etc...)
				ShowAdditionalInfo();
			}
			else
			{
				this.txtDescription.Text = "[Deleted]";				
				this.txtComments.Text = "[Deleted]";

				HideAllAditionalInfo();
			}
		}

		/// <summary>
		/// Message handler for the description text box changing value.
		/// Update the active source.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void txtDescription_TextChanged(object sender, System.EventArgs e)
		{
			if(m_ActiveSource==null)
			{
				return;
			}
			m_ActiveSource.Description = this.txtDescription.Text;
		}

		/// <summary>
		/// Message handler for the date of the source control changing value.
		/// Update the active source.
		/// </summary>
		/// <param name="oSender"></param>
		private void dateTheDate_evtValueChanged(object oSender)
		{
			if(m_ActiveSource==null)
			{
				return;
			}
			m_ActiveSource.TheDate.Date = this.dateTheDate.GetDate();		
			m_ActiveSource.TheDate.Status = this.dateTheDate.GetStatus();
		}

		/// <summary>
		/// Message handler for the comments textbox changing value.
		/// Update the active source.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void txtComments_TextChanged(object sender, System.EventArgs e)
		{
			if(m_ActiveSource==null)
			{
				return;
			}
			m_ActiveSource.Comments = this.txtComments.Text;		
		}

		private void cmdOK_Click(object sender, System.EventArgs e)
		{
			int				nI;			// Loop variable

			// Save all the sources
			for(nI=0;nI<this.m_lstSources.Items.Count;nI++)
			{
				clsSource oSource = (clsSource)this.m_lstSources.Items[nI];
				oSource.Save();
			}
		}

		private void cmdDeleteSource_Click(object sender, System.EventArgs e)
		{
			if(m_ActiveSource==null)
			{
				return;
			}
			m_ActiveSource.Delete();
		}

		private void cmdAddSource_Click(object sender, System.EventArgs e)
		{
			clsSource oNewSource = new clsSource(m_oDb);
			oNewSource.Description = "New Source";
			int nNew = this.m_lstSources.Items.Add(oNewSource);			 
			this.m_lstSources.SelectedIndex = nNew;
		}

		private void cboPrefix_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(this.cboPrefix.SelectedIndex>=0)
			{
				this.txtDescription.Text = this.cboPrefix.SelectedItem.ToString() + " " + this.txtDescription.Text;
				this.cboPrefix.SelectedIndex = -1;
			}
		}

		/// <summary>
		/// Message handler for the addition information type combo box changing.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cboAdditionalInfo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			// Check that a source is selected
			if(m_ActiveSource==null)
			{
				return;
			}

			clsIDName oType = (clsIDName)m_cboAdditionalInfo.SelectedItem;
			m_ActiveSource.AdditionalInfoTypeID = oType.ID;		

			ShowAdditionalInfo();
        }

		/// <summary>
		/// Message handler for the census address changing.
		/// Update the census object inside the current source.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void evtAdditionalCensus_Changed(object sender, System.EventArgs e)
		{
            // Allow events
            if(!m_bAllowEvents)
            {
                return;
            }

			// Check that a source is selected
			if(m_ActiveSource==null)
			{
				return;
			}
			
			// Check that a Census object is available
			if(m_ActiveSource.AdditionalCensus==null)
			{
				return;
			}

			// Update the census object.
            m_ActiveSource.AdditionalCensus.Address = m_txtCensusAddress.Text;
            m_ActiveSource.AdditionalCensus.Series = m_txtCensusSeries.Text;
            m_ActiveSource.AdditionalCensus.Piece = m_txtCensusPiece.Text;
            m_ActiveSource.AdditionalCensus.Folio = m_txtCensusFolio.Text;
            m_ActiveSource.AdditionalCensus.Page = m_txtCensusPage.Text;
        }

		/// <summary>
		/// Message handler for the launch census editor button click.
		/// Open the frmEditCensus dialog display the census data for this source.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cmdCensusOpen_Click(object sender, System.EventArgs e)
		{
			// Check that a source is selected
			if(m_ActiveSource==null)
			{
				return;
			}

			// Create a dialog to show the full census record
			frmEditCensus oCensus = new frmEditCensus(m_oDb,m_ActiveSource.ID);

			// Show the dialog and wait for the dialog to close
			oCensus.ShowDialog(this);
			oCensus.Dispose();
		}

		/// <summary>
		/// Message handler for any of the additional information Marriage fields changing.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void evtAdditionalMarriage_Changed(object sender, System.EventArgs e)
		{
			// Allow events
			if(!m_bAllowEvents)
			{
				return;
			}

			// Check that a source is selected
			if(m_ActiveSource==null)
			{
				return;
			}

			// Update the additional marriage information
            m_ActiveSource.AdditionalMarriage.When = m_dtpMarrWhen.Value;
            m_ActiveSource.AdditionalMarriage.Location = m_txtMarrLocation.Text;
			m_ActiveSource.AdditionalMarriage.GroomName = m_txtMarrGroom.Text;
			m_ActiveSource.AdditionalMarriage.GroomAge = m_txtMarrGroomAge.Text;
			m_ActiveSource.AdditionalMarriage.GroomOccupation = m_txtMarrGroomOccu.Text;
			m_ActiveSource.AdditionalMarriage.GroomLiving = m_txtMarrGroomLoca.Text;
			m_ActiveSource.AdditionalMarriage.GroomFather = m_txtMarrGroomFather.Text;
			m_ActiveSource.AdditionalMarriage.GroomFatherOccupation = m_txtMarrGroomFatherOcc.Text;
			m_ActiveSource.AdditionalMarriage.BrideName = m_txtMarrBride.Text;
			m_ActiveSource.AdditionalMarriage.BrideAge = m_txtMarrBrideAge.Text;
			m_ActiveSource.AdditionalMarriage.BrideOccupation = m_txtMarrBrideOccu.Text;
			m_ActiveSource.AdditionalMarriage.BrideLiving = m_txtMarrBrideLoca.Text;
			m_ActiveSource.AdditionalMarriage.BrideFather = m_txtMarrBrideFather.Text;
			m_ActiveSource.AdditionalMarriage.BrideFatherOccupation = m_txtMarrBrideFatherOcc.Text;
			m_ActiveSource.AdditionalMarriage.Witness = m_txtMarrWitness.Text;
            m_ActiveSource.AdditionalMarriage.GroReference = m_txtMarrGro.Text;
		}

		/// <summary>
		/// Message handler for any of the additional information birth fields changing.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void evtAdditionalBirth_Changed(object sender, System.EventArgs e)
		{
			// Allow events
			if(!m_bAllowEvents)
			{
				return;
			}

			// Check that a source is selected
			if(m_ActiveSource==null)
			{
				return;
			}

			// Update the additional birth information
			m_ActiveSource.AdditionalBirth.RegistrationDistrict = m_txtBirthDistrict.Text;
            m_ActiveSource.AdditionalBirth.When = m_dtpBirthWhen.Value;
            m_ActiveSource.AdditionalBirth.WhenAndWhere = m_txtBirthWhenWhere.Text;
			m_ActiveSource.AdditionalBirth.Name = m_txtBirthName.Text;
			m_ActiveSource.AdditionalBirth.Sex = m_txtBirthSex.Text;
			m_ActiveSource.AdditionalBirth.Father = m_txtBirthFather.Text;
			m_ActiveSource.AdditionalBirth.FatherOccupation = m_txtBirthFatherOccupation.Text;
            m_ActiveSource.AdditionalBirth.Mother = m_txtBirthMother.Text;
            m_ActiveSource.AdditionalBirth.MotherDetails = m_txtBirthMotherDetails.Text;
            m_ActiveSource.AdditionalBirth.Informant = m_txtBirthInformant.Text;
            m_ActiveSource.AdditionalBirth.InformantAddress = m_txtBirthInformantAddress.Text;
            m_ActiveSource.AdditionalBirth.WhenRegistered = m_txtBirthWhenReg.Text;
            m_ActiveSource.AdditionalBirth.GroReference = m_txtBirthReference.Text;
		}

		/// <summary>
		/// Message handler for any of the additional information death fields changing.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void evtAdditionalDeath_Changed(object sender, System.EventArgs e)
		{
            // Allow events
            if(!m_bAllowEvents)
            {
                return;
            }

			// Check that a source is selected
			if(m_ActiveSource==null)
			{
				return;
			}

			// Update the additional death information
			m_ActiveSource.AdditionalDeath.RegistrationDistrict = m_txtDeathDistrict.Text;
			m_ActiveSource.AdditionalDeath.When = m_txtDeathWhen.Text;
            m_ActiveSource.AdditionalDeath.Place = m_txtDeathWhere.Text;
			m_ActiveSource.AdditionalDeath.Name = m_txtDeathName.Text;
			m_ActiveSource.AdditionalDeath.Sex = m_txtDeathSex.Text;
			m_ActiveSource.AdditionalDeath.DatePlaceOfBirth = m_txtDeathDatePlace.Text;
			m_ActiveSource.AdditionalDeath.Occupation = m_txtDeathOccupation.Text;
			m_ActiveSource.AdditionalDeath.UsualAddress = m_txtDeathUsualAddress.Text;
			m_ActiveSource.AdditionalDeath.CauseOfDeath = m_txtDeathCause.Text;
			m_ActiveSource.AdditionalDeath.Informant = m_txtDeathInformant.Text;
			m_ActiveSource.AdditionalDeath.InformantDescription = m_txtDeathInformantDescription.Text;
			m_ActiveSource.AdditionalDeath.InformantAddress = m_txtDeathInformantAddress.Text;
			m_ActiveSource.AdditionalDeath.WhenRegistered = m_txtDeathWhenReg.Text;
            m_ActiveSource.AdditionalDeath.GroReference = m_txtDeathReference.Text;
		}

		/// <summary>
        /// Message handler for the repository combo box value changed event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cboRepository_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			// Check that a source is selected
			if(m_ActiveSource==null)
			{
				return;
			}

			clsIDName oRepository = (clsIDName)this.m_cboRepository.SelectedItem;
			m_ActiveSource.Repository = oRepository.ID;
        }

        #endregion

        // Message handler for the "Census Address" button click.
        /// <summary>
        /// Message handler for the "Census Address" button click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdCensusAddress_Click(object sender, EventArgs e)
        {
            frmSelectLocation oDialog = new frmSelectLocation(m_oDb, m_txtCensusAddress.Text);
            if(oDialog.ShowDialog(this) == DialogResult.OK)
            {
                m_txtCensusAddress.Text = oDialog.LocationName;
            }
        }


    }
}
