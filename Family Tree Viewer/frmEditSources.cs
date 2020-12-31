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
		private Database m_oDb;

		/// <summary>The source that we are currently editing.</summary>
		private Source m_ActiveSource;

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
        public frmEditSources(Database oDb, int nSourceID)
        {
            // Required for Windows Form Designer support
            InitializeComponent();

            // Initialise member variables
            m_oDb = oDb;

            // Show the additional information types
            IndexName[] oSources = m_oDb.GetSourceAdditionalTypes();
            foreach(IndexName oAdditional in oSources)
            {
                m_cboAdditionalInfo.Items.Add(oAdditional);
            }

            // Add the repositories
            oSources = m_oDb.GetRepositories();
            foreach(IndexName oRepository in oSources)
            {
                m_cboRepository.Items.Add(oRepository);
            }

            // Add the sources to the dialog box
            Source oSelected = null;
            oSources = oDb.GetSources(Objects.SortOrder.DATE);
            for(int nI = 0; nI < oSources.Length; nI++)
            {
                Source oSource = new Source(m_oDb, oSources[nI].index);
                m_lstSources.Items.Add(oSource);
                if(oSources[nI].index == nSourceID)
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
            Database oDb
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
            switch(m_ActiveSource.additionalInfoTypeIndex)
            {
            case 0: // None
                HideAllAditionalInfo();
                break;
            case 1:
                m_grpBirth.Visible = true;
                m_grpCensus.Visible = false;
                m_grpDeath.Visible = false;
                m_grpMarriage.Visible = false;
                if(m_ActiveSource.additionalBirth == null)
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
                    m_txtBirthDistrict.Text = m_ActiveSource.additionalBirth.registrationDistrict;
                    try
                    {
                        m_dtpBirthWhen.Value = m_ActiveSource.additionalBirth.when;
                    }
                    catch { }
                    m_txtBirthWhenWhere.Text = m_ActiveSource.additionalBirth.whenAndWhere;
                    m_txtBirthName.Text = m_ActiveSource.additionalBirth.name;
                    m_txtBirthSex.Text = m_ActiveSource.additionalBirth.sex;
                    m_txtBirthFather.Text = m_ActiveSource.additionalBirth.father;
                    m_txtBirthFatherOccupation.Text = m_ActiveSource.additionalBirth.fatherOccupation;
                    m_txtBirthMother.Text = m_ActiveSource.additionalBirth.mother;
                    m_txtBirthMotherDetails.Text = m_ActiveSource.additionalBirth.motherDetails;
                    m_txtBirthInformant.Text = m_ActiveSource.additionalBirth.informant;
                    m_txtBirthInformantAddress.Text = m_ActiveSource.additionalBirth.informantAddress;
                    m_txtBirthWhenReg.Text = m_ActiveSource.additionalBirth.whenRegistered;
                    m_txtBirthReference.Text = m_ActiveSource.additionalBirth.groReference;
                }
                break;

            case 2:
                m_grpBirth.Visible = false;
                m_grpCensus.Visible = false;
                m_grpDeath.Visible = false;
                m_grpMarriage.Visible = true;

                if(m_ActiveSource.additionalMarriage == null)
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
                        m_dtpMarrWhen.Value = m_ActiveSource.additionalMarriage.when;
                    }
                    catch { }
                    m_txtMarrLocation.Text = m_ActiveSource.additionalMarriage.location;
                    m_txtMarrGroom.Text = m_ActiveSource.additionalMarriage.groomName;
                    m_txtMarrGroomAge.Text = m_ActiveSource.additionalMarriage.groomAge;
                    m_txtMarrGroomOccu.Text = m_ActiveSource.additionalMarriage.groomOccupation;
                    m_txtMarrGroomLoca.Text = m_ActiveSource.additionalMarriage.groomLiving;
                    m_txtMarrGroomFather.Text = m_ActiveSource.additionalMarriage.groomFather;
                    m_txtMarrGroomFatherOcc.Text = m_ActiveSource.additionalMarriage.groomFatherOccupation;
                    m_txtMarrBride.Text = m_ActiveSource.additionalMarriage.brideName;
                    m_txtMarrBrideAge.Text = m_ActiveSource.additionalMarriage.brideAge;
                    m_txtMarrBrideOccu.Text = m_ActiveSource.additionalMarriage.brideOccupation;
                    m_txtMarrBrideLoca.Text = m_ActiveSource.additionalMarriage.brideLiving;
                    m_txtMarrBrideFather.Text = m_ActiveSource.additionalMarriage.brideFather;
                    m_txtMarrBrideFatherOcc.Text = m_ActiveSource.additionalMarriage.brideFatherOccupation;
                    m_txtMarrWitness.Text = m_ActiveSource.additionalMarriage.witness;
                    m_txtMarrGro.Text = m_ActiveSource.additionalMarriage.groReference;
                }
                break;

            case 3:
                m_grpBirth.Visible = false;
                m_grpCensus.Visible = false;
                m_grpDeath.Visible = true;
                m_grpMarriage.Visible = false;
                if(m_ActiveSource.additionalDeath == null)
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
                    m_txtDeathDistrict.Text = m_ActiveSource.additionalDeath.RegistrationDistrict;
                    m_txtDeathWhen.Text = m_ActiveSource.additionalDeath.When;
                    m_txtDeathWhere.Text = m_ActiveSource.additionalDeath.Place;
                    m_txtDeathName.Text = m_ActiveSource.additionalDeath.Name;
                    m_txtDeathSex.Text = m_ActiveSource.additionalDeath.Sex;
                    m_txtDeathDatePlace.Text = m_ActiveSource.additionalDeath.DatePlaceOfBirth;
                    m_txtDeathOccupation.Text = m_ActiveSource.additionalDeath.Occupation;
                    m_txtDeathUsualAddress.Text = m_ActiveSource.additionalDeath.UsualAddress;
                    m_txtDeathCause.Text = m_ActiveSource.additionalDeath.CauseOfDeath;
                    m_txtDeathInformant.Text = m_ActiveSource.additionalDeath.Informant;
                    m_txtDeathInformantDescription.Text = m_ActiveSource.additionalDeath.InformantDescription;
                    m_txtDeathInformantAddress.Text = m_ActiveSource.additionalDeath.InformantAddress;
                    m_txtDeathWhenReg.Text = m_ActiveSource.additionalDeath.WhenRegistered;
                    m_txtDeathReference.Text = m_ActiveSource.additionalDeath.GroReference;
                }
                break;

            case 4:
                m_grpBirth.Visible = false;
                m_grpCensus.Visible = true;
                m_grpDeath.Visible = false;
                m_grpMarriage.Visible = false;

                if(m_ActiveSource.additionalCensus == null)
                {
                    m_txtCensusAddress.Text = "Null";
                    m_txtCensusSeries.Text = "Null";
                    m_txtCensusPiece.Text = "Null";
                    m_txtCensusFolio.Text = "Null";
                    m_txtCensusPage.Text = "Null";
                }
                else
                {
                    m_txtCensusAddress.Text = m_ActiveSource.additionalCensus.Address;
                    m_txtCensusSeries.Text = m_ActiveSource.additionalCensus.Series;
                    m_txtCensusPiece.Text = m_ActiveSource.additionalCensus.Piece;
                    m_txtCensusFolio.Text = m_ActiveSource.additionalCensus.Folio;
                    m_txtCensusPage.Text = m_ActiveSource.additionalCensus.Page;
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
			m_ActiveSource = (Source)this.m_lstSources.SelectedItem;
			if(m_ActiveSource==null)
			{
				// Clear the controls
			}
			else if(m_ActiveSource.isValid())
			{
				// Populate the controls
				this.txtDescription.Text = m_ActiveSource.description;
				this.dateTheDate.Value = m_ActiveSource.theDate;
				this.txtComments.Text = m_ActiveSource.comments;
				m_cboAdditionalInfo.SelectedIndex = m_ActiveSource.additionalInfoTypeIndex;
				m_cboRepository.SelectedIndex = m_ActiveSource.repository; // This is not really correct

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
				clsReferences[] oReferences = m_ActiveSource.getReferences();
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
			m_ActiveSource.description = this.txtDescription.Text;
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
			m_ActiveSource.theDate.date = this.dateTheDate.GetDate();		
			m_ActiveSource.theDate.status = this.dateTheDate.GetStatus();
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
			m_ActiveSource.comments = this.txtComments.Text;		
		}

		private void cmdOK_Click(object sender, System.EventArgs e)
		{
			int				nI;			// Loop variable

			// Save all the sources
			for(nI=0;nI<this.m_lstSources.Items.Count;nI++)
			{
				Source oSource = (Source)this.m_lstSources.Items[nI];
				oSource.save();
			}
		}

		private void cmdDeleteSource_Click(object sender, System.EventArgs e)
		{
			if(m_ActiveSource==null)
			{
				return;
			}
			m_ActiveSource.delete();
		}

		private void cmdAddSource_Click(object sender, System.EventArgs e)
		{
			Source oNewSource = new Source(m_oDb);
			oNewSource.description = "New Source";
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

			IndexName oType = (IndexName)m_cboAdditionalInfo.SelectedItem;
			m_ActiveSource.additionalInfoTypeIndex = oType.index;		

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
			if(m_ActiveSource.additionalCensus==null)
			{
				return;
			}

			// Update the census object.
            m_ActiveSource.additionalCensus.Address = m_txtCensusAddress.Text;
            m_ActiveSource.additionalCensus.Series = m_txtCensusSeries.Text;
            m_ActiveSource.additionalCensus.Piece = m_txtCensusPiece.Text;
            m_ActiveSource.additionalCensus.Folio = m_txtCensusFolio.Text;
            m_ActiveSource.additionalCensus.Page = m_txtCensusPage.Text;
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
			frmEditCensus oCensus = new frmEditCensus(m_oDb,m_ActiveSource.index);

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
            m_ActiveSource.additionalMarriage.when = m_dtpMarrWhen.Value;
            m_ActiveSource.additionalMarriage.location = m_txtMarrLocation.Text;
			m_ActiveSource.additionalMarriage.groomName = m_txtMarrGroom.Text;
			m_ActiveSource.additionalMarriage.groomAge = m_txtMarrGroomAge.Text;
			m_ActiveSource.additionalMarriage.groomOccupation = m_txtMarrGroomOccu.Text;
			m_ActiveSource.additionalMarriage.groomLiving = m_txtMarrGroomLoca.Text;
			m_ActiveSource.additionalMarriage.groomFather = m_txtMarrGroomFather.Text;
			m_ActiveSource.additionalMarriage.groomFatherOccupation = m_txtMarrGroomFatherOcc.Text;
			m_ActiveSource.additionalMarriage.brideName = m_txtMarrBride.Text;
			m_ActiveSource.additionalMarriage.brideAge = m_txtMarrBrideAge.Text;
			m_ActiveSource.additionalMarriage.brideOccupation = m_txtMarrBrideOccu.Text;
			m_ActiveSource.additionalMarriage.brideLiving = m_txtMarrBrideLoca.Text;
			m_ActiveSource.additionalMarriage.brideFather = m_txtMarrBrideFather.Text;
			m_ActiveSource.additionalMarriage.brideFatherOccupation = m_txtMarrBrideFatherOcc.Text;
			m_ActiveSource.additionalMarriage.witness = m_txtMarrWitness.Text;
            m_ActiveSource.additionalMarriage.groReference = m_txtMarrGro.Text;
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
			m_ActiveSource.additionalBirth.registrationDistrict = m_txtBirthDistrict.Text;
            m_ActiveSource.additionalBirth.when = m_dtpBirthWhen.Value;
            m_ActiveSource.additionalBirth.whenAndWhere = m_txtBirthWhenWhere.Text;
			m_ActiveSource.additionalBirth.name = m_txtBirthName.Text;
			m_ActiveSource.additionalBirth.sex = m_txtBirthSex.Text;
			m_ActiveSource.additionalBirth.father = m_txtBirthFather.Text;
			m_ActiveSource.additionalBirth.fatherOccupation = m_txtBirthFatherOccupation.Text;
            m_ActiveSource.additionalBirth.mother = m_txtBirthMother.Text;
            m_ActiveSource.additionalBirth.motherDetails = m_txtBirthMotherDetails.Text;
            m_ActiveSource.additionalBirth.informant = m_txtBirthInformant.Text;
            m_ActiveSource.additionalBirth.informantAddress = m_txtBirthInformantAddress.Text;
            m_ActiveSource.additionalBirth.whenRegistered = m_txtBirthWhenReg.Text;
            m_ActiveSource.additionalBirth.groReference = m_txtBirthReference.Text;
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
			m_ActiveSource.additionalDeath.RegistrationDistrict = m_txtDeathDistrict.Text;
			m_ActiveSource.additionalDeath.When = m_txtDeathWhen.Text;
            m_ActiveSource.additionalDeath.Place = m_txtDeathWhere.Text;
			m_ActiveSource.additionalDeath.Name = m_txtDeathName.Text;
			m_ActiveSource.additionalDeath.Sex = m_txtDeathSex.Text;
			m_ActiveSource.additionalDeath.DatePlaceOfBirth = m_txtDeathDatePlace.Text;
			m_ActiveSource.additionalDeath.Occupation = m_txtDeathOccupation.Text;
			m_ActiveSource.additionalDeath.UsualAddress = m_txtDeathUsualAddress.Text;
			m_ActiveSource.additionalDeath.CauseOfDeath = m_txtDeathCause.Text;
			m_ActiveSource.additionalDeath.Informant = m_txtDeathInformant.Text;
			m_ActiveSource.additionalDeath.InformantDescription = m_txtDeathInformantDescription.Text;
			m_ActiveSource.additionalDeath.InformantAddress = m_txtDeathInformantAddress.Text;
			m_ActiveSource.additionalDeath.WhenRegistered = m_txtDeathWhenReg.Text;
            m_ActiveSource.additionalDeath.GroReference = m_txtDeathReference.Text;
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

			IndexName oRepository = (IndexName)this.m_cboRepository.SelectedItem;
			m_ActiveSource.repository = oRepository.index;
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
