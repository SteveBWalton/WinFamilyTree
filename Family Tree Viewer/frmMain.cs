using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Text;

// Disk Input Output
using System.IO;

// ParameterizedThreadStart
using System.Threading;

// Family tree objects
using FamilyTree.Objects;

namespace FamilyTree.Viewer
{
    #region Delegate Functions

    // Delegate for functions that specify a single bool parameter
    /// <summary>
    /// Delegate for functions that specify a single bool parameter
    /// </summary>
    /// <param name="bPara">Specify the boolean value.</param>
    public delegate void dgtSetBool(bool bPara);

    // Delegate for functions that specifiy a single string and bool parameter.
    /// <summary>
    /// Delegate for functions that specifiy a single string and bool parameter.
    /// </summary>
    /// <param name="sText">Specify the string value.</param>
    /// <param name="bTrueFalse">Specify the boolean value.</param>
    public delegate void dgtSetTextBool(string sText,bool bTrueFalse);

    // Delefate for functions that specify a single integer parameter.
    /// <summary>
    /// Delefate for functions that specify a single integer parameter.
    /// </summary>
    /// <param name="nValue">Specify the value of the integer parameter.</param>
    public delegate void dgtSetInt(int nValue);
    
    #endregion

    // This is the main window of the application.
    /// <summary>
    /// This is the main window of the application.
    /// This displays the current person and allows the user to move from person to person and select actions.
	/// </summary>
    public partial class frmMain : System.Windows.Forms.Form
	{
		#region Member Variables and Types

        /// <summary>Enumeration to indicate the relationship of a new person to an existing person.</summary>
        private enum enumRelatedPerson
        {
            /// <summary>The new person is the father of the current person.</summary>
            Father,
            /// <summary>The new person is the mother of the current person.</summary>
            Mother,
            /// <summary>The new person has the same mother and father of the current person.</summary>
            Sibling,
            /// <summary>The new person is a partner of the current person.</summary>
            Partner,
            /// <summary>The new person is a child of the current person.</summary>
            Child
        }

        /// <summary>
        /// Enumeration of indicate the type of the current main page.
        /// </summary>
        private enum enumPage
        {
            /// <summary>The main page is displaying a person.</summary>
            Person,

            /// <summary>The main page is displaying a source document.</summary>
            Source,

            /// <summary>The main page is displayed a place.</summary>
            Place,

            /// <summary>The main page is displaying a media object.</summary>
            Media,

            /// <summary>The main page is displaying something else.</summary>
            Other
        }

        /// <summary>Type to represent a page on the main window.</summary>
        private struct typPage
        {
            /// <summary>The type content on the page.</summary>
            public enumPage Content;

            /// <summary>The key to the type of content.</summary>
            public int ID;

            /// <summary>A human readable label for the page.</summary>
            public string Label;
        }
		        
        /// <summary>Type to represent a point in integer space.</summary>
		private struct typSize
		{
			/// <summary>Horizontal position.</summary>
			public int x;
			/// <summary>Vertical position.</summary>
			public int y;
		}

		/// <summary>ID of the current person.</summary>
		// private int m_nID;
        private typPage [] m_History;

        /// <summary>Position in the history array</summary>
        private int m_nHistoryIndex;

        /// <summary>Last valid position in the history array.</summary>
        private int m_nHistoryLast;

		/// <summary>Configuration file.</summary>
		private Innoval.clsXmlDocument m_oConfig;
		
		/// <summary>Current family tree database.</summary>
		private Database m_oDb;

		/// <summary>User options.</summary>
		private clsUserOptions m_oOptions;

		/// <summary>Array of person graphical controls to display siblings of the main person.</summary>
		private FamilyTree.Viewer.ucPerson[] m_psnSiblings;
		
		/// <summary>Array of person graphical controls to display children of the main person.</summary>
		private FamilyTree.Viewer.ucPerson[] m_psnChildren;
		
		/// <summary>Array of person graphical controls to display partners of the main person.</summary>
		private FamilyTree.Viewer.ucPerson[] m_psnPartners;
		
		/// <summary>Array of relationship graphical controls to display connections to partners of the main person.</summary>
		private FamilyTree.Viewer.ucRelationship[] m_marPartners;
		
		/// <summary>Background colour for a boy.</summary>
		private System.Drawing.Color m_rgbBackgroundBoy;
		
		/// <summary>Background colour for a female.</summary>
		private	System.Drawing.Color m_rgbBackgroundGirl;
		
		/// <summary>Size of person control.</summary>
        private typSize m_PersonSize;

		/// <summary>Padding arround person controls.</summary>
		private typSize m_Padding;
		
		/// <summary>Space between people who are married (or have a relationship).</summary>
        private int m_nMarriedWidth;

        /// <summary>Size of the font used on the main person area.</summary>
        private float m_dFontSize;

		/// <summary>The list of recent files.</summary>
		private Innoval.clsFileList m_oRecentFiles;

        /// <summary>The filename of a tree document to open at load.</summary>
        private string m_sTreeToOpen;

        // Count of the number of times that the progress bar has been progressed.
        /// <summary>
        /// Count of the number of times that the progress bar has been progressed.
        /// </summary>
        private int m_nNumSteps;

		#endregion

		#region Constructors etc

        /// <summary>Constructor for the main form.</summary>
        public frmMain(string[] sArgs)
        {
            // Required for Windows Form Designer support
            InitializeComponent();

            // Add any constructor code after InitializeComponent call
            m_psnFather.evtClick += new dgtClick(ucPerson_evtClick);
            m_psnMother.evtClick += new dgtClick(ucPerson_evtClick);
            m_psnFatherFather.evtClick += new dgtClick(ucPerson_evtClick);
            m_psnFatherMother.evtClick += new dgtClick(ucPerson_evtClick);
            m_psnMotherFather.evtClick += new dgtClick(ucPerson_evtClick);
            m_psnMotherMother.evtClick += new dgtClick(ucPerson_evtClick);

            // Open the configuration file			
            string sConfigFile = Innoval.clsDataPaths.GetUserDirectory("Walton", "Family Tree Viewer", "1.0");
            sConfigFile += "\\config.xml";
            m_oConfig = new Innoval.clsXmlDocument(sConfigFile);

            // Open the recent files list
            m_oRecentFiles = new Innoval.clsFileList(4, m_oConfig);
            UpdateRecentFiles();

            // Initialise objects / variables
            m_oDb = null;
            m_oOptions = new clsUserOptions(m_oConfig);
            m_History = new typPage[5];
            m_nHistoryIndex = -1;

            m_psnSiblings = null;
            m_psnChildren = null;
            m_psnPartners = null;
            m_marPartners = null;
            m_rgbBackgroundBoy = System.Drawing.Color.LightSkyBlue;
            m_rgbBackgroundGirl = System.Drawing.Color.LightPink;
            m_PersonSize.x = 130;
            m_PersonSize.y = 70;
            m_Padding.x = 8;
            m_Padding.y = 16;
            m_nMarriedWidth = 8;
            m_dFontSize = m_oOptions.FontBase.Size;

            string sDocument = "";
            m_sTreeToOpen = "";

            // Open the most recent database
            if(m_oRecentFiles.GetRecentFilename(0) != "")
            {
                sDocument = m_oRecentFiles.GetRecentFilename(0);
            }

            // Loop through the program arguments
            foreach(string sArgument in sArgs)
            {
                if(Path.GetExtension(sArgument).ToLower() == ".tree")
                {
                    m_sTreeToOpen = sArgument;
                }
                /*
                else
                {
                    MessageBox.Show(this,sArgument,"Argument");
                }
                 */
            }

            // Open the database
            if(sDocument != "")
            {
                OpenDatabase(sDocument);
            }
        }

        /// <summary>Clean up any resources being used.</summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(m_oDb!=null)
				{
					m_oDb.Dispose();
					m_oDb = null;
				}

				CleanupDynamicControls();

				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

        /// <summary>The main entry point for the application.</summary>
        [STAThread]
        static void Main(string[] sArgs)
        {
            // The following 2 lines enable XP styles in the applicaiton
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new frmMain(sArgs));
        }

		#endregion

        #region Thread Safe UI Functions

        // Thread safe, set the cursor back to the default cursor.
        /// <summary>
        /// Thread safe, set the cursor back to the default cursor.
        /// </summary>
        private void CursorDefault()
        {
            // Check that we are on the UI thread.
            if(InvokeRequired)
            {
                Invoke(new funcVoid(CursorDefault));
                return;
            }

            // Now we are on the UI thread.
            // Important that the application wait cursor is reset before the form cursor and Tee chart cursor            
            Application.UseWaitCursor = false;            
            // Cursor = Cursors.Default;
            Cursor.Current = Cursors.Default;
            
        }

        // Thread safe, set the cursor to the wait cursor.
        /// <summary>
        /// Thread safe, set the cursor to the wait cursor.
        /// </summary>
        private void CursorWait()
        {
            // Check that we are on the UI thread.
            if(InvokeRequired)
            {
                Invoke(new funcVoid(CursorWait));
                return;
            }

            // Now we are on the UI thread.    
            Application.UseWaitCursor = true;            
            Cursor.Current = Cursors.WaitCursor;
            // Cursor = Cursors.WaitCursor;                        
        }

        // Thread safe writes a message to the status bar.
        /// <summary>
        /// Thread safe writes a message to the status bar.
        /// </summary>
        /// <param name="sText"></param>
        /// <param name="bError"></param>
        public void SetStatusBarText(string sText, bool bError)
        {
            if(InvokeRequired)
            {
                Invoke(new dgtSetTextBool(SetStatusBarText), new object[2] { sText, bError });
            }

            // Now we are on the UI thread
            m_tslabStatus.Text = sText;
        }

        // Thread safe advance the progress bar one step.
        /// <summary>
        /// Thread safe advance the progress bar one step.
        /// </summary>
        private void ProgressBarPerformStep()
        {
            // Check that we are no the UI thread
            if(InvokeRequired)
            {
                Invoke(new funcVoid(ProgressBarPerformStep));
                return;
            }

            // Now we are on the UI thread.       
            m_nNumSteps++;
            m_tsProgressBar.PerformStep();                        
        }

        /// Thread safe, show / hide the progress bar on the status bar.
        /// <summary>
        /// Thread safe, show / hide the progress bar on the status bar.
        /// </summary>
        /// <param name="bVisible"></param>
        private void ProgressBarVisible(bool bVisible)
        {
            // Check that we are on the UI thread
            if(InvokeRequired)
            {
                Invoke(new dgtSetBool(ProgressBarVisible), new object[1] { bVisible });
                return;
            }

            // Now we are on the UI thread
            m_tsProgressBar.Visible = bVisible;            
        }

        // Thread safe initialise the the progress bar.
        /// <summary>
        /// Thread safe initialise the the progress bar.
        /// This does not make the progress bar visible.
        /// </summary>
        /// <param name="nMaximum">Specify the maximum value for the progress bar.  This is the number of steps.</param>
        private void ProgressBarInitialise(int nMaximum)
        {
            // Check that we are on the UI thread
            if(InvokeRequired)
            {
                Invoke(new dgtSetInt(ProgressBarInitialise), new object[1] { nMaximum });
                return;
            }

            // Now we are on the UI thread
            m_nNumSteps = 0;
            m_tsProgressBar.Value = 0;
            m_tsProgressBar.Maximum = nMaximum;
        }

        #endregion

        #region Drawing the form

        /// <summary>
		/// Releases the resources for the dynamic controls on the form.
		/// Ready for the controls to be reallocated to another person.
		/// </summary>
		/// <returns>True for success.  False for an error.</returns>
		private bool CleanupDynamicControls()
		{
			// Release the controls for siblings
			if(m_psnSiblings!=null)
			{
				for(int nI=0;nI<m_psnSiblings.Length;nI++)
				{
					if(m_psnSiblings[nI]!=null)
					{
                        m_oPanelTree.Controls.Remove(m_psnSiblings[nI]);
						m_psnSiblings[nI].Dispose();
						m_psnSiblings[nI] = null;
					}
				}
				m_psnSiblings = null;
			}

			// Release the controls for children
			if(m_psnChildren!=null)
			{
				for(int nI=0;nI<m_psnChildren.Length;nI++)
				{
					if(m_psnChildren[nI]!=null)
					{
                        m_oPanelTree.Controls.Remove(m_psnChildren[nI]);
						m_psnChildren[nI].Dispose();
						m_psnChildren[nI] = null;
					}
				}
				m_psnChildren = null;
			}

			// Release the controls for the partners (people controls)
			if(m_psnPartners!=null)
			{
				for(int nI=0;nI<m_psnPartners.Length;nI++)
				{
					if(m_psnPartners[nI]!=null)
					{
                        m_oPanelTree.Controls.Remove(m_psnPartners[nI]);
						m_psnPartners[nI].Dispose();
						m_psnPartners[nI] = null;
					}
				}
				m_psnPartners = null;
			}

			// Release the controls for the partners (relationship controls)
			if(m_marPartners!=null)
			{
				for(int nI=0;nI<m_marPartners.Length;nI++)
				{
					if(m_marPartners[nI]!=null)
					{
                        m_oPanelTree.Controls.Remove(m_marPartners[nI]);
						m_marPartners[nI].Dispose();
						m_marPartners[nI] = null;
					}
				}
				m_marPartners = null;
			}

			// return success
			return true;
		}

        // Draws the specified main person and their relationships.
        /// <summary>
        /// Draws the specified main person and their relationships.
        /// </summary>
		/// <param name="oPerson">Specifies the person to be drawn</param>
		/// <param name="Marriages">Specifies the relationships for the person</param>
		/// <param name="nPos">Specifies the position to draw the person (number of older siblings)</param>
        /// <param name="oFont">Specifies the font to use for the standard text.</param>
        /// <returns>True for success, false otherwise</returns>
        private bool DrawMainPerson(ref clsPerson oPerson, ref clsRelationship[] Marriages, ref int nPos, Font oFont)
        {
            // Draw the marriage partners if female
            if(Marriages.Length > 0)
            {
                if(oPerson.Female)
                {
                    for(int nI = Marriages.Length - 1; nI >= 0; nI--)
                    {
                        clsPerson oRelation = m_oDb.getPerson(Marriages[nI].PartnerID);

                        // Create a person control to show the partner
                        m_psnPartners[nI] = new FamilyTree.Viewer.ucPerson();
                        m_psnPartners[nI].Location = new System.Drawing.Point(nPos, m_labPerson.Top);
                        m_psnPartners[nI].Size = new System.Drawing.Size(m_PersonSize.x, m_PersonSize.y);
                        m_psnPartners[nI].SetPerson(oRelation);
                        m_psnPartners[nI].evtClick += new dgtClick(ucPerson_evtClick);
                        m_psnPartners[nI].BackColor = m_rgbBackgroundBoy;
                        m_psnPartners[nI].Font = oFont;
                        m_psnPartners[nI].SetPerson(oRelation);
                        m_oPanelTree.Controls.Add(m_psnPartners[nI]);
                        nPos += m_PersonSize.x;

                        // Create a relationship control to show the relationship to the partner
                        m_marPartners[nI] = new FamilyTree.Viewer.ucRelationship();
                        m_marPartners[nI].Location = new System.Drawing.Point(nPos, m_labPerson.Top + 8);
                        m_marPartners[nI].Size = new System.Drawing.Size(m_nMarriedWidth, 16);
                        m_marPartners[nI].SetRelationship(Marriages[nI]);
                        m_oPanelTree.Controls.Add(m_marPartners[nI]);
                        nPos += m_nMarriedWidth;
                    }
                }
            }

            // Draw the actual person
            m_labPerson.Left = nPos;
            m_labPerson.Width = 2 * m_PersonSize.x + m_nMarriedWidth;
            m_labPersonDates.Left = nPos;
            m_labPersonDates.Width = m_labPerson.Width;
            m_labPersonDates.Top = m_labPerson.Top + m_labPerson.Height;
            m_labPersonDates.Font = oFont;
            nPos += m_labPerson.Width;

            // Draw the marriages if male
            if(Marriages.Length > 0)
            {
                if(oPerson.isMale)
                {
                    for(int nI = 0; nI < Marriages.Length; nI++)
                    {
                        clsPerson oRelation = m_oDb.getPerson(Marriages[nI].PartnerID);

                        // Create a relationship control to show the relationship to the partner
                        m_marPartners[nI] = new FamilyTree.Viewer.ucRelationship();
                        m_marPartners[nI].Location = new System.Drawing.Point(nPos, m_labPerson.Top + 8);
                        m_marPartners[nI].Size = new System.Drawing.Size(m_nMarriedWidth, 16);
                        m_marPartners[nI].SetRelationship(Marriages[nI]);
                        m_oPanelTree.Controls.Add(m_marPartners[nI]);
                        nPos += m_nMarriedWidth;

                        // Create a person control to show the partner
                        m_psnPartners[nI] = new FamilyTree.Viewer.ucPerson();
                        m_psnPartners[nI].Location = new System.Drawing.Point(nPos, m_labPerson.Top);
                        m_psnPartners[nI].Size = new System.Drawing.Size(m_PersonSize.x, m_PersonSize.y);
                        m_psnPartners[nI].SetPerson(oRelation);
                        m_psnPartners[nI].evtClick += new dgtClick(ucPerson_evtClick);
                        m_psnPartners[nI].BackColor = m_rgbBackgroundGirl;
                        m_psnPartners[nI].Font = oFont;
                        m_psnPartners[nI].SetPerson(oRelation);
                        m_oPanelTree.Controls.Add(m_psnPartners[nI]);
                        nPos += m_PersonSize.x;
                    }
                }
            }

            nPos += m_Padding.x;

            // Return success
            return true;
        }

        /// <summary>
        /// Show the specified media object on the main window.
        /// </summary>
        /// <param name="nMediaID">Specifies the ID of the media object.</param>
        /// <param name="bAddHistory">Specifies true to add the page to history.</param>
        /// <returns>True for success, false otherwise.</returns>
        private bool ShowMedia            (            int nMediaID,            bool bAddHistory            )
        {
            // Hide the person tree panel
            m_oPanelTree.Visible = false;

            // Find the place
            clsMedia oMedia = new clsMedia(m_oDb,nMediaID);
            Text = oMedia.Filename + " - Family Tree Viewer";
            if(bAddHistory)
            {
                HistoryAdd(enumPage.Media,nMediaID,"Media: " + oMedia.Filename);
            }

            // Display the Html description of the source.
            m_oWebBrowser.DocumentText = m_oOptions.RenderHtml(oMedia.ToHtml());

            // Return success
            return true;
        }

        // Show the specifed place on the main window.
        /// <summary>
        /// Show the specifed place on the main window.
        /// </summary>
        /// <param name="nPlaceID">Specifies the ID of the place to show.</param>
        /// <param name="bAddHistory">Specifies true to add the page to history.</param>
        /// <returns>True for success, false otherwise.</returns>
        private bool ShowPlace(int nPlaceID, bool bAddHistory)
        {
            // Hide the person tree panel
            m_oPanelTree.Visible = false;

            // Find the place
            clsPlace oPlace = new clsPlace(nPlaceID, m_oDb);
            Text = oPlace.Name + " - Family Tree Viewer";
            if(bAddHistory)
            {
                HistoryAdd(enumPage.Place, nPlaceID, "Place: " + oPlace.Name);
            }

            // Display the Html description of the source.
            string sPlace = oPlace.ToHtml(m_menuLocation.Checked);
            m_oWebBrowser.DocumentText = m_oOptions.RenderHtml(sPlace);

            // Return success
            return true;
        }

        /// <summary>
        /// Show the specified source on the main window.
        /// </summary>
        /// <param name="nSourceID">Specifies the ID of the source to show.</param>
        /// <param name="bAddHistory">Specifies true to add the page to history.</param>
        /// <returns>True for success, false otherwise.</returns>
        private bool ShowSource(int nSourceID, bool bAddHistory)
        {
            // Hide the person tree panel.
            m_oPanelTree.Visible = false;

            // Find the source ID.
            clsSource oSource = new clsSource(m_oDb, nSourceID);
            Text = oSource.Description + " - Family Tree Viewer";
            if(bAddHistory)
            {
                HistoryAdd(enumPage.Source, nSourceID, "Source: " + oSource.Description);
            }

            // Display the Html description of the source.
            m_oWebBrowser.DocumentText = m_oOptions.RenderHtml(oSource.ToHtml());

            // Return success.
            return true;
        }

        // Show the specified person.
        /// <summary>
        /// Show the specified person.
        /// </summary>
		/// <param name="nID">Specifies the ID of the person to show.</param>
        /// <param name="bAddHistory">Specifies to add this page to the Back / Forward list.</param>
        /// <returns>True for sucess, false for failure.</returns>
        private bool ShowPerson(int nID, bool bAddHistory)
        {
            const int TOP = 2;
            clsPerson oRelation;
            int nI;				// Loop variable
            int nJ;				// Loop variable
            int nPos;			// Horizontal position for the next control
            bool bShownPerson;	// True when the actual person has been shown
            int nTag;			// Workspace for the tag value for the person controls

            // Show the person tree panel
            m_oPanelTree.Visible = true;

            // Show the specified person
            clsPerson oPerson = m_oDb.getPerson(nID);
            Text = oPerson.GetName(true, false) + " - Family Tree Viewer";

            // Build the rich text file description of the person
            m_oWebBrowser.DocumentText = m_oOptions.RenderHtml(oPerson.Description(true, true, m_menuImage.Checked, m_menuLocation.Checked, true));

            // Update the back button            
            if(bAddHistory)
            {
                HistoryAdd(enumPage.Person, nID, oPerson.GetName(true, false));
            }
            // Font oFont = new System.Drawing.Font("Tahoma",m_dFontSize,System.Drawing.FontStyle.Regular,System.Drawing.GraphicsUnit.Point,((System.Byte)(0)));
            Font oFont = m_oOptions.FontBase.GetFont();

            // Show the father
            if(oPerson.FatherID == 0)
            {
                m_psnFather.Visible = false;
                m_psnFatherFather.Visible = false;
                m_psnFatherMother.Visible = false;
                m_marFatherParents.Visible = false;
            }
            else
            {
                clsPerson oFather = m_oDb.getPerson(oPerson.FatherID);
                m_psnFather.Width = m_PersonSize.x;
                m_psnFather.Height = m_PersonSize.y;
                m_psnFather.SetPerson(oFather);
                m_psnFather.Top = m_PersonSize.y + m_Padding.y + TOP;
                m_psnFather.Font = oFont;
                m_psnFather.Visible = true;

                // Show the father's father
                if(oFather.FatherID == 0)
                {
                    m_psnFatherFather.Visible = false;
                }
                else
                {
                    clsPerson oGrandFather = m_oDb.getPerson(oFather.FatherID);
                    m_psnFatherFather.Width = m_PersonSize.x;
                    m_psnFatherFather.Height = m_PersonSize.y;
                    m_psnFatherFather.SetPerson(oGrandFather);
                    m_psnFatherFather.Top = TOP;
                    m_psnFatherFather.Font = oFont;
                    m_psnFatherFather.Visible = true;
                }

                // Show the father's mother
                if(oFather.MotherID == 0)
                {
                    m_psnFatherMother.Visible = false;
                }
                else
                {
                    clsPerson oGrandMother = m_oDb.getPerson(oFather.MotherID);
                    m_psnFatherMother.Width = m_PersonSize.x;
                    m_psnFatherMother.Height = m_PersonSize.y;
                    m_psnFatherMother.SetPerson(oGrandMother);
                    m_psnFatherMother.Top = TOP;
                    m_psnFatherMother.Font = oFont;
                    m_psnFatherMother.Visible = true;
                }

                // Show the relationship between these 2
                if(oFather.FatherID == 0 || oFather.MotherID == 0)
                {
                    m_marFatherParents.Visible = false;
                }
                else
                {
                    m_marFatherParents.Visible = true;
                    m_marFatherParents.Width = m_nMarriedWidth;
                    m_marFatherParents.Top = TOP;
                }
            }

            // Show the mother
            if(oPerson.MotherID == 0)
            {
                m_psnMother.Visible = false;
                m_psnMotherFather.Visible = false;
                m_psnMotherMother.Visible = false;
                m_marMotherParents.Visible = false;
            }
            else
            {
                clsPerson oMother = m_oDb.getPerson(oPerson.MotherID);
                m_psnMother.Width = m_PersonSize.x;
                m_psnMother.Height = m_PersonSize.y;
                m_psnMother.SetPerson(oMother);
                m_psnMother.Top = m_PersonSize.y + m_Padding.y + TOP;
                m_psnMother.Font = oFont;
                m_psnMother.Visible = true;

                // Show the mother's father
                if(oMother.FatherID == 0)
                {
                    m_psnMotherFather.Visible = false;
                }
                else
                {
                    clsPerson oGrandFather = m_oDb.getPerson(oMother.FatherID);
                    m_psnMotherFather.Width = m_PersonSize.x;
                    m_psnMotherFather.Height = m_PersonSize.y;
                    m_psnMotherFather.SetPerson(oGrandFather);
                    m_psnMotherFather.Top = TOP;
                    m_psnMotherFather.Font = oFont;
                    m_psnMotherFather.Visible = true;
                }

                // Show the mother's mother
                if(oMother.MotherID == 0)
                {
                    m_psnMotherMother.Visible = false;
                }
                else
                {
                    clsPerson oGrandMother = m_oDb.getPerson(oMother.MotherID);
                    m_psnMotherMother.Width = m_PersonSize.x;
                    m_psnMotherMother.Height = m_PersonSize.y;
                    m_psnMotherMother.SetPerson(oGrandMother);
                    m_psnMotherMother.Top = TOP;
                    m_psnMotherMother.Font = oFont;
                    m_psnMotherMother.Visible = true;
                }

                // Show the relationship between these 2
                if(oMother.FatherID == 0 || oMother.MotherID == 0)
                {
                    m_marMotherParents.Visible = false;
                }
                else
                {
                    m_marMotherParents.Visible = true;
                    m_marMotherParents.Width = m_nMarriedWidth;
                    m_marMotherParents.Top = TOP;
                }
            }

            // Show the actual person
            m_labPerson.Font = m_oOptions.FontBaseTitle.GetFont();
            m_labPerson.Top = 2 * (m_PersonSize.y + m_Padding.y) + TOP;
            m_labPerson.Text = oPerson.GetName(false, false);
            m_labPerson.Width = 2 * m_PersonSize.x + m_nMarriedWidth;
            m_labPersonDates.Top = m_labPerson.Top + 23;
            m_labPersonDates.Text = oPerson.ShortDescription(true);
            m_labPersonDates.Width = m_labPerson.Width;
            m_labPersonDates.Height = m_PersonSize.y - 23;
            if(oPerson.isMale)
            {
                m_labPerson.BackColor = m_rgbBackgroundBoy;
            }
            else
            {
                m_labPerson.BackColor = m_rgbBackgroundGirl;
            }
            m_labPersonDates.BackColor = m_labPerson.BackColor;

            CleanupDynamicControls();

            clsRelationship[] Marriages = oPerson.GetRelationships();
            if(Marriages.Length > 0)
            {
                m_psnPartners = new FamilyTree.Viewer.ucPerson[Marriages.Length];
                m_marPartners = new FamilyTree.Viewer.ucRelationship[Marriages.Length];
            }

            // Show the siblings            
            int[] Siblings = oPerson.GetSiblings();
            nPos = 3;
            bShownPerson = false;
            if(Siblings.Length > 0)
            {
                m_psnSiblings = new FamilyTree.Viewer.ucPerson[Siblings.Length];

                for(nI = 0; nI < Siblings.Length; nI++)
                {
                    oRelation = m_oDb.getPerson(Siblings[nI]);

                    // Show the person if he is older than the current sibling (and not already shown)
                    if(oPerson.dob.Date < oRelation.dob.Date && !bShownPerson)
                    {
                        DrawMainPerson(ref oPerson, ref Marriages, ref nPos, oFont);
                        bShownPerson = true;
                    }

                    // Show the sibling
                    m_psnSiblings[nI] = new FamilyTree.Viewer.ucPerson();
                    if(oRelation.isMale)
                    {
                        m_psnSiblings[nI].BackColor = m_rgbBackgroundBoy;
                    }
                    else
                    {
                        m_psnSiblings[nI].BackColor = m_rgbBackgroundGirl;
                    }
                    m_psnSiblings[nI].Font = oFont;
                    m_psnSiblings[nI].Location = new System.Drawing.Point(nPos, m_labPerson.Top);
                    m_psnSiblings[nI].Size = new System.Drawing.Size(m_PersonSize.x, m_PersonSize.y);
                    m_psnSiblings[nI].SetPerson(oRelation);
                    m_psnSiblings[nI].evtClick += new dgtClick(ucPerson_evtClick);

                    // Build a tag value that represents which parents this sibling shares
                    nTag = 0;
                    if(oRelation.FatherID == oPerson.FatherID)
                    {
                        nTag |= 1;
                    }
                    if(oRelation.MotherID == oPerson.MotherID)
                    {
                        nTag |= 2;
                    }
                    m_psnSiblings[nI].Tag = nTag;

                    // this.Controls.Add(m_psnSiblings[nI]);
                    m_oPanelTree.Controls.Add(m_psnSiblings[nI]);
                    nPos += m_PersonSize.x + m_Padding.x;
                }
            }
            if(!bShownPerson)
            {
                DrawMainPerson(ref oPerson, ref Marriages, ref nPos, oFont);
            }

            // Reposition the parents (X direction)
            m_psnFather.Left = m_labPerson.Left;
            m_marParents.Left = m_psnFather.Left + m_PersonSize.x;
            m_marParents.Top = m_psnFather.Top;
            m_marParents.Width = m_nMarriedWidth;
            m_psnMother.Left = m_labPerson.Left + m_PersonSize.x + m_nMarriedWidth;

            // Reposition the grandparents (X direction)
            m_psnFatherMother.Left = m_labPerson.Left;
            m_psnFatherFather.Left = m_psnFatherMother.Left - m_nMarriedWidth - m_PersonSize.x;
            m_psnMotherFather.Left = m_labPerson.Left + m_PersonSize.x + m_nMarriedWidth;
            m_psnMotherMother.Left = m_psnMotherFather.Left + m_psnMotherFather.Width + m_marParents.Width;
            if(m_psnFatherFather.Left < 3)
            {
                int nOffset = 3 - m_psnFatherFather.Left;
                m_psnFatherFather.Left += nOffset;
                m_psnFatherMother.Left += nOffset;
                m_psnMotherFather.Left += nOffset;
                m_psnMotherMother.Left += nOffset;
            }
            m_marFatherParents.Left = m_psnFatherFather.Left + m_PersonSize.x;
            m_marFatherParents.Top = m_psnFatherFather.Top + 8;
            m_marMotherParents.Left = m_psnMotherFather.Left + m_PersonSize.x;
            m_marMotherParents.Top = m_psnMotherFather.Top + 8;

            // Show the children
            int[] Children = oPerson.GetChildren();
            int nHeight = m_labPerson.Top + m_PersonSize.y + m_Padding.y;
            if(oPerson.isMale)
            {
                nPos = m_labPerson.Left;
            }
            else
            {
                if(m_marPartners != null)
                {
                    nPos = m_psnPartners[m_marPartners.Length - 1].Left;
                }
                else
                {
                    nPos = m_labPerson.Left;
                }
            }

            if(Children.Length > 0)
            {
                m_psnChildren = new FamilyTree.Viewer.ucPerson[Children.Length];

                for(nI = 0; nI < Children.Length; nI++)
                {
                    oRelation = m_oDb.getPerson(Children[nI]);

                    m_psnChildren[nI] = new FamilyTree.Viewer.ucPerson();
                    if(oRelation.isMale)
                    {
                        m_psnChildren[nI].BackColor = m_rgbBackgroundBoy;
                    }
                    else
                    {
                        m_psnChildren[nI].BackColor = m_rgbBackgroundGirl;
                    }
                    m_psnChildren[nI].Font = oFont;
                    m_psnChildren[nI].Location = new System.Drawing.Point(nPos, nHeight);
                    m_psnChildren[nI].Size = new System.Drawing.Size(m_PersonSize.x, m_PersonSize.y);
                    m_psnChildren[nI].SetPerson(oRelation);
                    m_psnChildren[nI].evtClick += new dgtClick(ucPerson_evtClick);

                    // Decide which relationship this child belongs to
                    nTag = -1;
                    if(m_marPartners != null)
                    {
                        for(nJ = 0; nJ < m_marPartners.Length; nJ++)
                        {
                            if(m_marPartners[nJ].MotherID == oRelation.MotherID && m_marPartners[nJ].FatherID == oRelation.FatherID)
                            {
                                nTag = nJ;
                            }
                        }
                    }
                    m_psnChildren[nI].Tag = nTag;

                    // this.Controls.Add(m_psnChildren[nI]);
                    m_oPanelTree.Controls.Add(m_psnChildren[nI]);
                    nPos += m_PersonSize.x + m_Padding.x;
                }
            }

            // Reposition the children if off the right edge
            if(Children.Length > 0)
            {
                if(m_psnChildren[Children.Length - 1].Left + m_PersonSize.x > Width)
                {
                    int nOffset = m_psnChildren[Children.Length - 1].Left + m_PersonSize.x - Width;
                    for(nI = 0; nI < Children.Length; nI++)
                    {
                        m_psnChildren[nI].Left -= nOffset;
                    }
                }
            }

            // This causes the parent / child lines to be redrawn
            Refresh();

            // Return success
            return true;
        }

		#endregion

		#region Actions

        #region Back / Forward / History

        /// <summary>
        /// Returns the current page, according to the history.
        /// </summary>
        private typPage Current
        {
            get
            {
                if(m_nHistoryIndex >= 0)
                {
                    return m_History[m_nHistoryIndex];
                }
                return m_History[0];
            }
        }
        
        /// <summary>
        /// Add the current page to the history.
        /// </summary>
        /// <param name="nType">Specifies the type of page.</param>
        /// <param name="nID">Specifies the ID of the page.</param>
        /// <param name="sLabel">Specifies a human readable label for the page.</param>
        private void HistoryAdd            (            enumPage nType,            int nID,            string sLabel            )
        {
            // Check that some thing has changed.
            if(Current.Content == nType && Current.ID == nID)
            {
                return;
            }

            // Add to the history
            m_nHistoryIndex++;
            if(m_nHistoryIndex >= m_History.Length)
            {
                // Move all the page down one.
                for(int nIndex = 0;nIndex < m_History.Length - 1;nIndex++)
                {
                    m_History[nIndex] = m_History[nIndex + 1];
                }

                // Move back to the end of the history
                m_nHistoryIndex--;
            }

            // Set the current page
            m_History[m_nHistoryIndex].ID = nID;
            m_History[m_nHistoryIndex].Content = nType;
            m_History[m_nHistoryIndex].Label = sLabel;
            m_nHistoryLast = m_nHistoryIndex;

            // Update the back button
            UpdateBackButton();
        }

        /// <summary>
        /// Move to the page that is back in the history.
        /// </summary>
        private void HistoryBack()
        {
            // Check that there is some history to move back into
            if(m_nHistoryIndex<1)
            {
                return;
            }

            // Move back in the history            
            m_nHistoryIndex--;
            ShowCurrentPage();
        }

        /// <summary>
        /// Move to the page that is forward in the history (if available).
        /// If no page is available then nothing happens.
        /// </summary>
        private void HistoryForward()
        {
            // Check that there is some history to move into.
            if(m_nHistoryIndex == m_nHistoryLast)
            {
                return;
            }

            // Move forward in the history
            m_nHistoryIndex++;
            ShowCurrentPage();
        }

        /// <summary>
        /// Shows the current page.
        /// Does not add the page to the history.
        /// Does update the back history drop down list.
        /// </summary>
        private void ShowCurrentPage()
        {
            // Show the current page
            switch(m_History[m_nHistoryIndex].Content)
            {
            case enumPage.Person:
                ShowPerson(m_History[m_nHistoryIndex].ID,false);
                break;
            case enumPage.Source:
                ShowSource(m_History[m_nHistoryIndex].ID,false);
                break;
            case enumPage.Place:
                ShowPlace(m_History[m_nHistoryIndex].ID,false);
                break;
            }

            // Update the back button
            UpdateBackButton();
        }

        // Updates the back (and forward) buttons and combo boxes based on the current history.
        /// <summary>
        /// Updates the back (and forward) buttons and combo boxes based on the current history.
        /// </summary>
        private void UpdateBackButton()
        {
            // Update the back button
            if(m_nHistoryIndex > 0)
            {
                m_tsbBack.Enabled = true;
                m_tsddbBack.Enabled = true;

                m_tsddbBack.DropDownItems.Clear();
                for(int nI = m_nHistoryIndex - 1;nI >= 0;nI--)
                {
                    m_tsddbBack.DropDownItems.Add(m_History[nI].Label,null,IndividualBackItem_Click);
                }
            }
            else
            {
                m_tsbBack.Enabled = false;
                m_tsddbBack.Enabled = false;
            }

            // Update the forward button
            if(m_nHistoryIndex == m_nHistoryLast)
            {
                m_tsbForward.Enabled = false;
                m_tsddbForward.Enabled = false;
            }
            else
            {
                m_tsbForward.Enabled = true;
                m_tsddbForward.Enabled = true;
                m_tsddbForward.DropDownItems.Clear();
                for(int nI = m_nHistoryIndex + 1;nI <= m_nHistoryLast;nI++)
                {
                    m_tsddbForward.DropDownItems.Add(m_History[nI].Label,null,IndividualBackItem_Click);
                }
            }
        }

        #endregion

        /// <summary>Displays the home / default person.
        /// </summary>
        /// <returns>True, if successful.  False, otherwise.</returns>
        private bool GoHome()
		{
			// Find the home person and display them
            Innoval.clsXmlNode oHome = m_oConfig.GetNode("useroptions/home");
			int nHomeID = oHome.GetAttributeValue("id",1,true);
			ShowPerson(nHomeID,true);

			// Return success
			return true;
		}

		/// <summary>
		/// Display a dialog box and allow the user to select the person to show
		/// </summary>
		/// <returns></returns>
		private bool GotoPerson()
		{
			// Allow the user to select a person
			frmSelectPerson oDialog = new frmSelectPerson();
			int nPersonID = oDialog.SelectPerson(this,m_oDb);

			// If the user did select a person then show that person
			if(nPersonID>=0)
			{
				ShowPerson(nPersonID,true);
			}

			// Return success
			return true;
		}

        /// <summary>Exits the program.</summary>
		/// <returns>True, if successful.  False, otherwise.</returns>
		private bool AbsoluteEnd()
		{
			// End the program by closing the main window
			Close();

			// return success
			return true;
		}

        /// <summary>Displays a dialog that allows the user to edit the currently shown person / object.</summary>
		/// <returns>True for success, false otherwise.</returns>
		private bool Edit()
		{
            switch(Current.Content)
            {
            case enumPage.Person:
                // Create a dialog to edit this person
                frmEditPerson dlgEdit = new frmEditPerson(Current.ID,m_oDb);

                // Show the dialog and wait for the dialog to close
                if(dlgEdit.ShowDialog(this)==DialogResult.OK)
                {
                    // Refresh the display of the current person
                    ShowPerson(Current.ID,false);
                }
                dlgEdit.Dispose();
                break;

            case enumPage.Place:
                // Create a dialog to edit this place
                frmEditPlace oEditPlace = new frmEditPlace(Current.ID,m_oDb);

                // Show the dialog and wait for the dialog to close
                if(oEditPlace.ShowDialog(this)==DialogResult.OK)
                {
                    // Refresh the display of this place
                    ShowPlace(Current.ID,false);
                }
                oEditPlace.Dispose();
                break;

            case enumPage.Source:
                // Create a dialog to edit this source
                frmEditSources oEditSource = new frmEditSources(m_oDb,Current.ID);
                if(oEditSource.ShowDialog(this) == DialogResult.OK)
                {
                    // Refresh the display of this source
                    ShowSource(Current.ID,false);
                }
                oEditSource.Dispose();
                break;

            case enumPage.Media:
                // Create a dialog to edit this media
                frmEditMedia oEditMedia = new frmEditMedia(m_oDb,Current.ID);
                if(oEditMedia.ShowDialog(this) == DialogResult.OK)
                {
                    // Refresh the display of this source
                    ShowMedia(Current.ID,false);
                }
                oEditMedia.Dispose();
                break;
            }

			// Return success
			return true;
		}

		/// <summary>
		/// Adds a new person to database.
		/// </summary>
		/// <param name="Relation">Specifies the relationship of the new person to the current person.</param>
		/// <returns>True, if a new person was created.  False, otherwise.</returns>
		private bool AddPerson			(			enumRelatedPerson Relation			)
		{
			// Create a dialog to edit the new person
			frmEditPerson dlgEdit = new frmEditPerson(m_oDb);

			// Show the dialog and wait for the dialig to close
			if(dlgEdit.ShowDialog(this)==DialogResult.OK)
			{
				// A new person was created link to the current person in the required way
				int nNewPersonID = dlgEdit.GetPersonID();

				clsPerson oPerson;
				switch(Relation)
				{
				case enumRelatedPerson.Father:
                    oPerson = new clsPerson(Current.ID,m_oDb);
					oPerson.FatherID = nNewPersonID;
					oPerson.Save();
					break;

				case enumRelatedPerson.Mother:
                    oPerson = new clsPerson(Current.ID,m_oDb);
					oPerson.MotherID = nNewPersonID;
					oPerson.Save();
					break;

				case enumRelatedPerson.Sibling:
					clsPerson oNewPerson = new clsPerson(nNewPersonID,m_oDb);
                    oPerson = new clsPerson(Current.ID,m_oDb);
					oNewPerson.FatherID = oPerson.FatherID;
					oNewPerson.MotherID = oPerson.MotherID;
					oNewPerson.Save();
					break;

				case enumRelatedPerson.Partner:
                    oPerson = new clsPerson(Current.ID,m_oDb);
					clsRelationship oRelationship = new clsRelationship(oPerson,nNewPersonID);
					oRelationship.Save();					
					oPerson.AddRelationship(oRelationship);
					break;

				case enumRelatedPerson.Child:
					oNewPerson = new clsPerson(nNewPersonID,m_oDb);
                    oPerson = new clsPerson(Current.ID,m_oDb);
					if(oPerson.isMale)
					{
                        oNewPerson.FatherID = Current.ID;
					}
					else
					{
                        oNewPerson.MotherID = Current.ID;
					}
					oNewPerson.Save();
					break;
					
				}

				// Refresh the display of the current person
                ShowPerson(Current.ID,false);
			}
			dlgEdit.Dispose();

			// Return success
			return true;
		}

		/// <summary>
		/// Edit all the sources.
		/// This displays a dialog that allows the user to edit the existing sources and add new sources.
		/// </summary>
		/// <returns>True for success, false otherwise.</returns>
		private bool EditSources()
		{
			// Create a dialog to edit the sources
			frmEditSources oEdit = new frmEditSources(m_oDb);

			// Show the dialog and wait for the dialog to close
			oEdit.ShowDialog(this);
			oEdit.Dispose();

			// Return success
			return true;
		}

		/// <summary>
		/// Display a dialog that allows the user to change the user options.
		/// </summary>
		/// <returns>True for success, false otherwise.</returns>
		private bool UserOptions()
		{
			// Create a dialog to edit the user options
			frmUserOptions oOptions = new frmUserOptions();

			// Show the dialog and wait for the dialog to close
			if(oOptions.UpdateOptions(this,ref m_oOptions))
			{
				// Update the display for the new user options
                ShowCurrentPage();
			}
			oOptions.Dispose();

			// Return success
			return true;
		}

		/// <summary>
		/// Create a tree document for the current person and displays it in a tree view window.
		/// </summary>
		/// <returns>True for success, false otherwise.</returns>
		private bool CreateTree()
		{
			// Create the tree document
            clsTreeDocument oTree = new clsTreeDocument(m_oDb,m_oOptions,Current.ID);

            // Create a tree preview window
            frmViewTree oTreeWindow = new frmViewTree(oTree);
            oTreeWindow.Show(this);

			// Return success
			return true;
		}

        /// <summary>
        /// Display a dialog to allow the user to select a tree file.
        /// If the user selects one then display the tree in a tree view window.
        /// </summary>
        /// <returns>True for success, false otherwise.</returns>
        private bool OpenTree()
        {
            // Set the file open settings
            m_OpenFileDialog.Title = "Select Tree file";
            m_OpenFileDialog.CheckFileExists = true;
            m_OpenFileDialog.CheckPathExists = true;
            m_OpenFileDialog.DefaultExt = "tree";
            m_OpenFileDialog.Filter = "Tree Files (*.tree)|*.tree|All Files (*.*)|*.*";
            m_OpenFileDialog.FilterIndex = 0;
            m_OpenFileDialog.Multiselect = false;
            
            // Display the open file dialog
            if(m_OpenFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                // Open the selected file
                OpenTree(m_OpenFileDialog.FileName);
            }

            // Return success
            return true;
        }

        /// <summary>Opens the specified tree file in a new window.
        /// </summary>
        /// <param name="sTreeFile">Specifies the full filename of the tree file.</param>
        /// <returns>True for success, false otherwise.</returns>
        private bool OpenTree(string sTreeFile)
        {
            // Open the .tree file
            Innoval.clsXmlDocument oTreeFile = new Innoval.clsXmlDocument(sTreeFile);

            // Create the tree document
            clsTreeDocument oTree = new clsTreeDocument(m_oDb,oTreeFile);

            // Create a tree preview window
            frmViewTree oTreeWindow = new frmViewTree(oTree);
            oTreeWindow.Show();

            // Return success
            return true;
        }

        // Create a html report about the current person.
        /// <summary>
        /// Create a html report about the current person.
        /// </summary>
        /// <returns>True for success, false otherwise.</returns>
        private bool ReportToHtml()
        {
            // Create a report object
            clsReport oReport = new clsReport(Current.ID, m_oDb, m_oOptions);

            // Hide the person tree panel
            m_oPanelTree.Visible = false;

            // Display the Html description of the source.
            m_oWebBrowser.DocumentText = m_oOptions.RenderHtml(oReport.GetReport());

            // Return success
            return true;            
        }

        ///// <summary>
        ///// Create a report in Microsoft Word about the current person.
        ///// </summary>
        ///// <returns>True for success, false otherwise.</returns>
        //private bool ToWord()
        //{
        //    /*
        //    clsMSWord oReportToWord = new clsMSWord(Current.ID, m_oDb, m_oOptions);
        //    oReportToWord.Show();
        //    */
        //    // Return success
        //    return true;
        //}

		/// <summary>
		/// Displays a dialog that allows the age of a person to be calculated on specified dates.
		/// </summary>
		/// <returns>True of success, false otherwise.</returns>
		private bool ShowAge()
		{
			// Create the age window
            frmAge oAge = new frmAge(m_oDb,Current.ID);
			oAge.ShowDialog(this);

			// Return success;
			return true;
		}

		/// <summary>
		/// Display the edit census dialog.
		/// This allows the user to edit the census data.
		/// </summary>
		/// <returns>True for success, false otherwise.</returns>
		private bool EditCensus()
		{
			// Create a dialog to edit the sources
			frmEditCensus oCensus = new frmEditCensus(m_oDb,0);

			// Show the dialog and wait for the dialog to close
			oCensus.ShowDialog(this);
			oCensus.Dispose();

			// Return success
			return true;
		}

        // Display the recent changes on the main window.
        /// <summary>
        /// Display the recent changes on the main window.
        /// </summary>
        /// <remarks>
        /// This was previously in a separate window.
        /// </remarks>
		/// <returns>True for success, false otherwise.</returns>
		private bool ShowRecentChanges()
		{
            // Hide the person tree panel
            m_oPanelTree.Visible = false;

            // Display the Html description of the source.
            m_oWebBrowser.DocumentText = m_oOptions.RenderHtml(m_oDb.GetRecentChangesAsHtml());

            // Return success
            return true;
        }

        /// <summary>
        /// Display the To Do list on the main window.
        /// </summary>
        /// <returns>True for success, false otherwise.</returns>
        private bool ShowToDo()
        {
            // Hide the person tree panel
            m_oPanelTree.Visible = false;

            // Display the Html description of the source.
            m_oWebBrowser.DocumentText = m_oOptions.RenderHtml(m_oDb.GetToDoAsHtml());

            // Return success
            return true;
        }

        // Show a select file dialog and allow the user to select a new database file.
        /// <summary>
        /// Show a select file dialog and allow the user to select a new database file.
        /// Open the selected database file if valid.
		/// Returns true if a new database has been selected, false otherwise.
		/// </summary>
		/// <returns>True, if a new database was selected.  False, otherwise.</returns>		
		private bool OpenDatabase()
		{
			// Show the open file dialog
			m_OpenFileDialog.Title = "Select Family Tree File";
			m_OpenFileDialog.Filter = "Database Files (*.mdb)|*.mdb";
			m_OpenFileDialog.FilterIndex = 1;
			if(m_OpenFileDialog.ShowDialog(this)==DialogResult.Cancel)
			{
				// User selected cancel
				return false;
			}

			// Open the selected file
			return OpenDatabase(m_OpenFileDialog.FileName);
		}

        /// Opens the specified database file (if valid).
        /// <summary>
        /// Opens the specified database file (if valid).
        /// Returns true if a new database has been selected, false otherwise.
		/// </summary>
		/// <param name="sFilename">Specifies the filename of the database to open.</param>
		/// <returns>True, if a new database was selected.  False, otherwise.</returns>		
        private bool OpenDatabase(string sFilename)
        {
            // Validate the selected file
            if(!File.Exists(sFilename))
            {
                return false;
            }

            // Close the old database
            if(m_oDb != null)
            {
                m_oDb.Dispose();
            }

            // Open the new database
            m_oDb = new Database(sFilename);

            // Update the recent files
            m_oRecentFiles.OpenFile(sFilename);
            UpdateRecentFiles();

            // Show the default person
            GoHome();

            // Update the status bar
            SetStatusBarText("Opened " + sFilename, false);

            // Return new database
            return true;
        }

        // Updates the recent file menu.
        /// <summary>
        /// Updates the recent file menu.
        /// </summary>
		private void UpdateRecentFiles()
		{
			if(m_oRecentFiles.GetRecentFilename(0)!="")
			{
				m_menuRecentFile1.Text = "1 "+m_oRecentFiles.GetDisplayName(0);
				m_menuRecentFile1.Visible = true;
			}
			else
			{
				m_menuRecentFile1.Visible = false;
			}
			if(m_oRecentFiles.GetRecentFilename(1)!="")
			{
				m_menuRecentFile2.Text = "2 "+m_oRecentFiles.GetDisplayName(1);
				m_menuRecentFile2.Visible = true;
			}
			else
			{
				m_menuRecentFile2.Visible = false;
			}
			if(m_oRecentFiles.GetRecentFilename(2)!="")
			{
				m_menuRecentFile3.Text = "3 "+m_oRecentFiles.GetDisplayName(2);
				m_menuRecentFile3.Visible = true;
			}
			else
			{
				m_menuRecentFile3.Visible = false;
			}
			if(m_oRecentFiles.GetRecentFilename(3)!="")
			{
				m_menuRecentFile4.Text = "4 "+m_oRecentFiles.GetDisplayName(3);
				m_menuRecentFile4.Visible = true;
			}
			else
			{
				m_menuRecentFile4.Visible = false;
			}
		}

        // Allows the user to select an output file to write gedcom into.
        /// <summary>
        /// Allows the user to select an output file to write gedcom into.
        /// Currently this is a the gedcom of the whole database.
		/// </summary>
        private void ExportGedcom()
        {
            frmGedcomOptions oDialog = new frmGedcomOptions(m_oOptions.GedcomOptions);
            if(oDialog.ShowDialog(this) == DialogResult.OK)
            {
                // Save the user options
                m_oOptions.Save();

                // Unreachable code
                #pragma warning disable 162
                if(true)
                {
                    // Create a new thread to do the work.
                    ParameterizedThreadStart oThreadMethod = new ParameterizedThreadStart(WriteGedcom);
                    Thread oThread = new Thread(oThreadMethod);
                    oThread.Start(m_oOptions.GedcomOptions);
                }
                else
                {
                    // Save the gedcom into the specified file				
                    WriteGedcom(m_oOptions.GedcomOptions);
                }
                #pragma warning restore 162

            }
        }

        /// <summary>Allows the user to export a SQL script.</summary>
        private void ExportSQL()
        {
            // Initialise the select save file dialog
            m_SaveFileDialog.Title = "Select output file";
            m_SaveFileDialog.Filter = "SQL Files (*.sql)|*.sql";
            m_SaveFileDialog.FilterIndex = 1;
            m_SaveFileDialog.FileName = "walton.sql";

            if(m_SaveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                m_oDb.WriteSQL(m_SaveFileDialog.FileName);
            }
        }

        /// <summary>Writes gedcom data into the specified file.  Why is this in the main form would expect this to be in FTObjects?</summary>
		/// <param name="oParameter">Specifies the filename of the gedcom file to create.</param>		
        private void WriteGedcom(object oParameter)
        {
            // Show the wait cursor
            CursorWait();

            // Estimate the number of steps required
            Innoval.clsXmlNode xmlGedcom = m_oConfig.GetNode("gedcom");
            int nNumSteps = xmlGedcom.GetAttributeValue("steps", 1000, true);
            ProgressBarInitialise(nNumSteps);
            ProgressBarVisible(true);

            // Decode the parameter
            clsGedcomOptions oOptions = (clsGedcomOptions)oParameter;
            string sFilename = oOptions.sFilename;

            // Open the output file
            StreamWriter oFile = new StreamWriter(sFilename, false);

            // Write the Gedcom header
            oFile.WriteLine("0 HEAD");
            oFile.WriteLine("1 SOUR FamilyTree");
            oFile.WriteLine("2 NAME FamilyTree");
            oFile.WriteLine("2 VERS 1.0.0");
            oFile.WriteLine("1 DEST DISKETTE");
            oFile.WriteLine("1 DATE " + DateTime.Now.ToString("d MMM yyyy"));
            oFile.WriteLine("2 TIME "+DateTime.Now.ToString("HH:mm:ss"));
            oFile.WriteLine("1 CHAR UTF-8");
            oFile.WriteLine("1 FILE " + Path.GetFileName(oOptions.sFilename));

            // Create a list of Gedcom families objects @F1@ etc ...
            clsFamilies oFamilies = new clsFamilies();

            // Write the individuals
            IndexName[] oAllPeople = m_oDb.getPeople();
            for(int nI = 0; nI < oAllPeople.Length; nI++)
            {
                // Create an object for this person.
                clsPerson oPerson = m_oDb.getPerson(oAllPeople[nI].index);

                // Check that this person is included in the gedcom file.
                // The person will also need to be excluded from any families that try to reference him.
                if(oPerson.IncludeInGedcom)
                {
                    // Create Gedcom record for this person
                    oFile.WriteLine("0 @I" + oPerson.ID.ToString("0000") + "@ INDI");

                    // Create an intial list of sources for this person
                    ArrayList oPersonSources = new ArrayList();
                    oPerson.SourceNonSpecific.GedcomAdd(oPersonSources);

                    oFile.WriteLine("1 NAME " + oPerson.forenames + " /" + oPerson.BirthSurname + "/");
                    oFile.WriteLine("2 GIVN " + oPerson.forenames);
                    oFile.WriteLine("2 SURN " + oPerson.BirthSurname);
                    // oPerson.SourceName.WriteGedcom(2,oFile,null);
                    oPerson.SourceName.GedcomAdd(oPersonSources);

                    // Get the occupation information
                    clsFact[] oFacts = oPerson.GetFacts(20);
                    if(oFacts.Length > 0)
                    {
                        oFile.Write("2 OCCU ");
                        bool bFirst = true;
                        foreach(clsFact oFact in oFacts)
                        {
                            if(bFirst)
                            {
                                bFirst = false;
                            }
                            else
                            {
                                oFile.Write(", ");
                            }
                            oFile.Write(oFact.Information);
                            // oFact.Sources.WriteGedcom(3,oFile,null);
                        }
                        oFile.WriteLine();
                    }

                    oFile.Write("1 SEX ");
                    if(oPerson.isMale)
                    {
                        oFile.WriteLine("M");
                    }
                    else
                    {
                        oFile.WriteLine("F");
                    }
                    oFile.WriteLine("1 BIRT");
                    oFile.WriteLine("2 DATE " + oPerson.dob.Format(DateFormat.Gedcom));
                    m_oDb.WriteGedcomPlace(oFile, 2, oPerson.GetSimpleFact(10), oOptions);

                    // oPerson.SourceDoB.WriteGedcom(2,oFile,null);
                    oPerson.SourceDoB.GedcomAdd(oPersonSources);

                    if(!oPerson.DoD.IsEmpty())
                    {
                        oFile.WriteLine("1 DEAT Y");
                        if(!oPerson.DoD.IsEmpty())
                        {
                            oFile.WriteLine("2 DATE " + oPerson.DoD.Format(DateFormat.Gedcom));
                        }
                        m_oDb.WriteGedcomPlace(oFile, 2, oPerson.GetSimpleFact(90), oOptions);
                        string sCauseOfDeath = oPerson.GetSimpleFact(92);
                        if(sCauseOfDeath != "")
                        {
                            oFile.WriteLine("2 CAUS " + sCauseOfDeath);
                        }
                        // oPerson.SourceDoD.WriteGedcom(2,oFile,null);
                        oPerson.SourceDoD.GedcomAdd(oPersonSources);
                    }

                    // Create a list of the partners
                    ArrayList oPartners = new ArrayList();

                    // Get the relationship information				
                    clsRelationship[] oRelationships = oPerson.GetRelationships();
                    for(int nJ = 0; nJ < oRelationships.Length; nJ++)
                    {
                        // Check that the partner is included in the Gedcom file
                        clsPerson oPartner = new clsPerson(oRelationships[nJ].PartnerID, m_oDb);
                        if(oPartner.IncludeInGedcom)
                        {
                            clsFamily oMarriage = oFamilies.GetMarriageFamily(oRelationships[nJ].MaleID, oRelationships[nJ].FemaleID, oRelationships[nJ].ID);
                            oFile.WriteLine("1 FAMS @F" + oMarriage.GedComID.ToString("0000") + "@");

                            // Add to the list of partners
                            oPartners.Add(oRelationships[nJ].PartnerID);
                        }
                    }

                    // Add the partners in children not already picked up
                    int[] nChildren = oPerson.GetChildren();
                    for(int nJ = 0; nJ < nChildren.Length; nJ++)
                    {
                        clsPerson oChild = m_oDb.getPerson(nChildren[nJ]);

                        // Check that the childs parent is already a partner
                        if(oPerson.isMale)
                        {
                            if(!oPartners.Contains(oChild.MotherID))
                            {
                                int nMotherID = oChild.MotherID;
                                clsPerson oMother = new clsPerson(oChild.MotherID, m_oDb);
                                if(!oMother.IncludeInGedcom)
                                {
                                    // Use an unknown mother
                                    nMotherID = 0;
                                }

                                // Create families for new partner
                                clsFamily oMarriage = oFamilies.GetMarriageFamily(oPerson.ID, nMotherID, 0);
                                oFile.WriteLine("1 FAMS @F" + oMarriage.GedComID.ToString("0000") + "@");

                                // Add to the list of partners
                                oPartners.Add(nMotherID);
                            }
                        }
                        else
                        {
                            if(!oPartners.Contains(oChild.FatherID))
                            {
                                int nFatherID = oChild.FatherID;
                                clsPerson oFather = new clsPerson(oChild.FatherID, m_oDb);
                                if(!oFather.IncludeInGedcom)
                                {
                                    // Use an unknown father
                                    nFatherID = 0;
                                }

                                // Create families for new partner
                                clsFamily oMarriage = oFamilies.GetMarriageFamily(nFatherID, oPerson.ID, 0);
                                oFile.WriteLine("1 FAMS @F" + oMarriage.GedComID.ToString("0000") + "@");

                                // Add to the list of partners
                                oPartners.Add(nFatherID);
                            }
                        }
                    }

                    // Add this person to the parents family
                    if(oPerson.FatherID > 0 || oPerson.MotherID > 0)
                    {
                        // Check that the father is included in the gedcom file.
                        int nFatherID = oPerson.FatherID;
                        clsPerson oFather = new clsPerson(nFatherID, m_oDb);
                        if(!oFather.IncludeInGedcom)
                        {
                            nFatherID = 0;
                        }

                        // Check that the mother is included in the gedcom file.
                        int nMotherID = oPerson.MotherID;
                        clsPerson oMother = new clsPerson(nMotherID, m_oDb);
                        if(!oMother.IncludeInGedcom)
                        {
                            nMotherID = 0;
                        }

                        //  Get the parent family information
                        if(nMotherID != 0 || nFatherID != 0)
                        {
                            clsFamily oFamily = oFamilies.GetParentFamily(oPerson.FatherID, oPerson.MotherID);
                            oFamily.AddChild(oPerson);
                            oFile.WriteLine("1 FAMC @F" + oFamily.GedComID.ToString("0000") + "@");
                        }
                    }

                    // Get Census records
                    clsCensusPerson[] oCensui = m_oDb.CensusForPerson(oPerson.ID);
                    foreach(clsCensusPerson oCensus in oCensui)
                    {
                        oFile.WriteLine("1 CENS");
                        oFile.WriteLine("2 DATE " + oCensus.Date.ToString("d MMM yyyy"));
                        m_oDb.WriteGedcomPlace(oFile, 2, oCensus.HouseholdName, oOptions);
                        if(oCensus.Occupation != "")
                        {
                            oFile.WriteLine("2 OCCU " + oCensus.Occupation);
                        }
                        oFile.WriteLine("2 NOTE " + oCensus.LivingWith(m_oDb));
                        clsSources oSources = oCensus.GetSources(m_oDb);
                        if(oSources != null)
                        {
                            // oSources.WriteGedcom(2,oFile,null);
                            oSources.GedcomAdd(oPersonSources);
                        }
                    }

                    // Attach the  media
                    m_oDb.GedcomWritePersonMedia(oFile, oPerson.ID, oPerson.MediaID);

                    // Attached the list of sources
                    foreach(int nSourceID in oPersonSources)
                    {
                        oFile.WriteLine("1 SOUR @S" + nSourceID.ToString("0000") + "@");
                    }

                    // Write the last edit information
                    if(oPerson.LastEditBy != "")
                    {
                        oFile.WriteLine("1 CHAN");
                        oFile.WriteLine("2 DATE " + oPerson.LastEditDate.ToString("d MMM yyyy"));
                        oFile.WriteLine("3 TIME " + oPerson.LastEditDate.ToString("HH:mm:ss"));
                        if(oOptions.IncludePGVU)
                        {
                            oFile.WriteLine("2 _PGVU " + oPerson.LastEditBy);
                        }
                    }
                }

                // Progress Bar
                ProgressBarPerformStep();
            }

            // Write the family records @F1@...
            oFamilies.WriteGedcom(oFile, m_oDb, ProgressBarPerformStep, oOptions);

            // Write all the source records @S1@ etc...
            m_oDb.WriteSourcesGedcom(oFile, ProgressBarPerformStep, oOptions);

            // Write all the media records @M1@ etc...
            m_oDb.GedcomWriteMedia(oFile);

            // Write the repository records @R1@ etc ...
            m_oDb.WriteRepositoriesGedcom(oFile);

            // Close the Gedcom header
            oFile.WriteLine("0 TRLR");

            // Close the output file
            oFile.Close();

            // Save the number of steps for next time
            xmlGedcom.SetAttributeValue("steps", m_nNumSteps);

            // Restore the default cursor etc.
            ProgressBarVisible(false);
            CursorDefault();
        }

		#endregion

		#region Message Handlers

        #region Form Events

        /// <summary>Message handler for the paint event on the Panel tree control.
        /// Draw the lines from parents to children etc ...
        /// </summary>
        /// <remarks>
        /// This is basically the paint event for the form.
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PanelTree_Paint(object sender,PaintEventArgs e)
        {
            // Create a pen to draw connections
            Pen oPen = new Pen(Color.Black,2);

            // Draw a line up for the person
            int nPos = m_labPerson.Left + m_labPerson.Width / 2;
            e.Graphics.DrawLine(oPen,nPos,m_labPerson.Top,nPos,m_labPerson.Top - m_Padding.y);

            // Draw lines up for the siblings
            if(m_psnSiblings != null)
            {
                for(int nType = 1;nType <= 3;nType++)
                {
                    int nBarHeight = 0;
                    int nMax = 0;
                    int nMin = 0;
                    bool bDrawBar = false;
                    switch(nType)
                    {
                    case 1:
                        nBarHeight = m_labPerson.Top - (m_Padding.y / 2) + 4;
                        nPos = m_psnFather.Left + m_psnFather.Width / 2;
                        nMin = nPos;
                        nMax = nPos;
                        break;

                    case 2:
                        nBarHeight = m_labPerson.Top - (m_Padding.y / 2) - 4;
                        nPos = m_psnMother.Left + m_psnMother.Width / 2;
                        nMin = nPos;
                        nMax = nPos;
                        break;

                    case 3:
                        nBarHeight = m_labPerson.Top - (m_Padding.y / 2);
                        nPos = m_labPerson.Left + m_labPerson.Width / 2;
                        nMin = nPos;
                        nMax = nPos;
                        break;
                    }

                    for(int nI = 0;nI < m_psnSiblings.Length;nI++)
                    {
                        // TODO: Check this.  This was crashing.
                        if (m_psnSiblings[nI] != null)
                        {
                            if ((int)m_psnSiblings[nI].Tag == nType)
                            {
                                bDrawBar = true;

                                nPos = m_psnSiblings[nI].Left + m_psnSiblings[nI].Width / 2;
                                if (nPos < nMin)
                                {
                                    nMin = nPos;
                                }
                                if (nPos > nMax)
                                {
                                    nMax = nPos;
                                }

                                e.Graphics.DrawLine(oPen, nPos, m_labPerson.Top, nPos, nBarHeight);
                            }
                        }
                    }

                    // Draw a bar to hang the Siblings on
                    e.Graphics.DrawLine(oPen,nMin,nBarHeight,nMax,nBarHeight);

                    // Draw a line up to the parents relationship
                    switch(nType)
                    {
                    case 1:
                        if(bDrawBar)
                        {
                            nPos = m_psnFather.Left + m_psnFather.Width / 2;
                            e.Graphics.DrawLine(oPen,nPos,m_psnFather.Top + m_psnFather.Height,nPos,nBarHeight);
                        }
                        break;

                    case 2:
                        if(bDrawBar)
                        {
                            nPos = m_psnMother.Left + m_psnMother.Width / 2;
                            e.Graphics.DrawLine(oPen,nPos,m_psnMother.Top + m_psnMother.Height,nPos,nBarHeight);
                        }
                        break;
                    }
                }

                // Draw a line down for the siblings with descendants
                for(int nI = 0;nI < m_psnSiblings.Length;nI++)
                {
                    clsPerson oSibling = m_oDb.getPerson(m_psnSiblings[nI].GetPersonID());
                    if(oSibling.HasChildren())
                    {
                        e.Graphics.DrawLine(oPen,m_psnSiblings[nI].Left + m_psnSiblings[nI].Width / 2,m_psnSiblings[nI].Top + m_psnSiblings[nI].Height,m_psnSiblings[nI].Left + m_psnSiblings[nI].Width / 2,m_psnSiblings[nI].Top + m_psnSiblings[nI].Height + 3);
                    }
                }
            }

            // There is always a line up to the parents
            nPos = m_marParents.Left + m_marParents.Width / 2;
            e.Graphics.DrawLine(oPen,nPos,m_labPerson.Top - m_Padding.y,nPos,m_marParents.Top + m_marParents.Height);

            // Draw a line from the father to his parents
            if(m_psnFather.Visible)
            {
                e.Graphics.DrawLine(oPen,m_psnFather.Left + m_psnFather.Width / 2,m_psnFather.Top,m_psnFather.Left + m_psnFather.Width / 2,m_psnFather.Top - 8);
                e.Graphics.DrawLine(oPen,m_psnFather.Left + m_psnFather.Width / 2,m_psnFather.Top - 8,m_marFatherParents.Left + m_marFatherParents.Width / 2,m_psnFather.Top - 8);
                e.Graphics.DrawLine(oPen,m_marFatherParents.Left + m_marFatherParents.Width / 2,m_psnFather.Top - 8,m_marFatherParents.Left + m_marFatherParents.Width / 2,m_marFatherParents.Top + m_marFatherParents.Height);
            }

            // Draw a line from the mother to her parents
            if(m_psnMother.Visible)
            {
                e.Graphics.DrawLine(oPen,m_psnMother.Left + m_psnMother.Width / 2,m_psnMother.Top,m_psnMother.Left + m_psnMother.Width / 2,m_psnMother.Top - 8);
                e.Graphics.DrawLine(oPen,m_psnMother.Left + m_psnMother.Width / 2,m_psnMother.Top - 8,m_marMotherParents.Left + m_marMotherParents.Width / 2,m_psnMother.Top - 8);
                e.Graphics.DrawLine(oPen,m_marMotherParents.Left + m_marMotherParents.Width / 2,m_psnMother.Top - 8,m_marMotherParents.Left + m_marMotherParents.Width / 2,m_marMotherParents.Top + m_marMotherParents.Height);
            }

            // Draw lines up for the children
            if(m_psnChildren != null)
            {
                int nBarHeight = m_psnChildren[0].Top - (m_Padding.y / 2);
                int nNumMarriages = 0;
                if(m_marPartners != null)
                {
                    nNumMarriages = m_marPartners.Length;
                }

                for(int nType = -1;nType < nNumMarriages;nType++)
                {
                    bool bDrawBar = false;
                    if(nType == -1)
                    {
                        nPos = m_labPerson.Left + m_labPerson.Width / 2;
                    }
                    else
                    {
                        nPos = m_marPartners[nType].Left + m_marPartners[nType].Width / 2;
                    }
                    int nMin = nPos;
                    int nMax = nPos;

                    for(int nI = 0;nI < m_psnChildren.Length;nI++)
                    {
                        if((int)m_psnChildren[nI].Tag == nType)
                        {
                            bDrawBar = true;
                            nPos = m_psnChildren[nI].Left + m_psnChildren[nI].Width / 2;
                            if(nPos < nMin)
                            {
                                nMin = nPos;
                            }
                            if(nPos > nMax)
                            {
                                nMax = nPos;
                            }
                            e.Graphics.DrawLine(oPen,nPos,m_psnChildren[0].Top,nPos,nBarHeight);
                        }
                    }

                    if(bDrawBar)
                    {
                        // Draw a bar to hang the Siblings on
                        e.Graphics.DrawLine(oPen,nMin,nBarHeight,nMax,nBarHeight);

                        int nHeight;
                        if(nType == -1)
                        {
                            nPos = m_labPerson.Left + m_labPerson.Width / 2;
                            nHeight = m_labPerson.Top + m_labPerson.Height;
                        }
                        else
                        {
                            nPos = m_marPartners[nType].Left + m_marPartners[nType].Width / 2;
                            nHeight = m_marPartners[nType].Top + m_marPartners[nType].Height;
                        }
                        e.Graphics.DrawLine(oPen,nPos,nBarHeight,nPos,nHeight);

                        nBarHeight += 5;
                    }
                }

                // Draw a line down from the children who have descendants
                for(int nI = 0;nI < m_psnChildren.Length;nI++)
                {
                    clsPerson oChild = m_oDb.getPerson(m_psnChildren[nI].GetPersonID());
                    if(oChild.HasChildren())
                    {
                        e.Graphics.DrawLine(oPen,m_psnChildren[nI].Left + m_psnChildren[nI].Width / 2,m_psnChildren[nI].Top + m_psnChildren[nI].Height,m_psnChildren[nI].Left + m_psnChildren[nI].Width / 2,m_psnChildren[nI].Top + m_psnChildren[nI].Height + 5);
                    }
                }
            }
        }

        /// <summary>Message handler for the main person title paint event.
        /// Draw a border around three sides of the control.
        /// This is here because it is really to do with form painting.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void labPerson_Paint(object sender,PaintEventArgs e)
        {
            Pen oPen = new Pen(Color.Black,1);
            e.Graphics.DrawLine(oPen,0,m_labPerson.Height,0,0);
            e.Graphics.DrawLine(oPen,0,0,m_labPerson.Width - 1,0);
            e.Graphics.DrawLine(oPen,m_labPerson.Width - 1,0,m_labPerson.Width - 1,m_labPerson.Height);
        }

        /// <summary>Message handler for the main person details paint event.
        /// Draw a border around three sides of the control
        /// This is here because it is really to do with form painting.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void labPersonDates_Paint(object sender,PaintEventArgs e)
        {
            Pen oPen = new Pen(Color.Black,1);
            e.Graphics.DrawLine(oPen,0,0,0,m_labPersonDates.Height - 1);
            e.Graphics.DrawLine(oPen,0,m_labPersonDates.Height - 1,m_labPersonDates.Width - 1,m_labPersonDates.Height - 1);
            e.Graphics.DrawLine(oPen,m_labPersonDates.Width - 1,m_labPersonDates.Height - 1,m_labPersonDates.Width - 1,0);
        }

        /// <summary>Message handler for the main form loading.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_Load(object sender,System.EventArgs e)
        {
            // Size the main window
            Innoval.clsXmlNode oMain = m_oConfig.GetNode("windows/main");
            WindowState = (FormWindowState)oMain.GetAttributeValue("state",(int)FormWindowState.Normal,true);
            Width = oMain.GetAttributeValue("width",800,true);
            Height = oMain.GetAttributeValue("height",600,true);
        }

        /// <summary>Message handler for the shown event.
        /// This is like the post appear load event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_Shown(object sender,EventArgs e)
        {
            // Open the tree document if specified.
            if(m_sTreeToOpen != "")
            {
                // MessageBox.Show(this,m_sTreeToOpen,"TreeToOpen");
                OpenTree(m_sTreeToOpen);
            }
        }

        /// <summary>Message handler for the main window closing.
        /// Save the size of the window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_Closing(object sender,System.ComponentModel.CancelEventArgs e)
        {
            if(WindowState != FormWindowState.Minimized)
            {
                Innoval.clsXmlNode oMain = m_oConfig.GetNode("windows/main");
                oMain.SetAttributeValue("state",(int)WindowState);
                if(WindowState == FormWindowState.Normal)
                {
                    oMain.SetAttributeValue("width",Width);
                    oMain.SetAttributeValue("height",Height);
                }
                m_oConfig.Save();
            }
        }

        #endregion

        #region Menu System

        #region File Menu

        /// <summary>
        /// Message handler for the File -> Open menu point click.
        /// Open a family tree database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuOpen_Click(object sender,EventArgs e)
        {
            OpenDatabase();
        }

        /// <summary>
        /// Message handler for the File -> Home menu point click
        /// and the "Home" toolbar button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuHome_Click(object sender,EventArgs e)
        {
            GoHome();
        }

        // Message handler for the "Back" button click event.
        /// <summary>
        /// Message handler for the "Back" button click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuBack_Click(object sender,EventArgs e)
        {
            HistoryBack();
        }

        /// <summary>
        /// Message handler for the forward button click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuForward_Click(object sender,EventArgs e)
        {
            HistoryForward();
        }

        // Message handler for the all the recent file menu points.
        /// <summary>
        /// Message handler for the all the recent file menu points.
        /// Opens the file specified on the recent file menu point.
        /// </summary>
        /// <param name="oSender"></param>
        /// <param name="e"></param>
        private void menuRecentFile_Click(object oSender,System.EventArgs e)
        {
            // Find the index of the recent file menu
            ToolStripMenuItem oMenu = (ToolStripMenuItem)oSender;
            int nIndex = int.Parse(oMenu.Text.Substring(0,1)) - 1;

            // Find the selected file
            string sFilename = m_oRecentFiles.GetRecentFilename(nIndex);            

            // Open this file
            OpenDatabase(sFilename);
        }

        // Message handler for the "File" -> "Export Gedom" menu point
        /// <summary>
        /// Message handler for the "File" -> "Export Gedom" menu point
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuExportGedcom_Click(object sender,EventArgs e)
        {
            ExportGedcom();
        }

        // Message handler for the 'File' | 'Export SQL script' menu item.
        /// <summary>
        /// Message handler for the 'File' | 'Export SQL script' menu item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuExportSQLScript_Click(object sender, EventArgs e)
        {
            ExportSQL();
        }

        /// <summary>
        /// Message handler for the File -> Open Tree menu point click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuOpenTree_Click(object sender,EventArgs e)
        {
            OpenTree();
        }

        /// <summary>
        /// Message handler for File -> Exit menu point click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuExit_Click(object sender,EventArgs e)
        {
            AbsoluteEnd();
        }

        #endregion

        #region Edit Menu

        // Message handler for the "Edit" -> "Edit..." menu point click
        /// <summary>
        /// Message handler for the "Edit" -> "Edit..." menu point click
        /// and the "Edit" toolbar button click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuEdit_Click(object sender,EventArgs e)
        {
            Edit();
        }

        /// <summary>
        /// Message handler for the Edit -> Edit Sources... menu point click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuEditSources_Click(object sender,EventArgs e)
        {
            EditSources();
        }

        /// <summary>
        /// Message handler for the Edit -> Add Father menu point click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuAddFather_Click(object sender,EventArgs e)
        {
            AddPerson(enumRelatedPerson.Father);
        }

        /// <summary>
        /// Message handler for the Edit -> Add Mother menu point click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuAddMother_Click(object sender,EventArgs e)
        {
            AddPerson(enumRelatedPerson.Mother);
        }

        /// <summary>
        /// Message handler for the Edit -> Add Sibling menu point click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuAddSibling_Click(object sender,EventArgs e)
        {
            AddPerson(enumRelatedPerson.Sibling);
        }

        /// <summary>
        /// Message handler for the Edit -> Add Child menu point click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuAddChild_Click(object sender,EventArgs e)
        {
            AddPerson(enumRelatedPerson.Child);
        }

        /// <summary>
        /// Message handler for the Edit -> Add Partner menu point click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuAddPartner_Click(object sender,EventArgs e)
        {
            AddPerson(enumRelatedPerson.Partner);
        }

        /// <summary>
        /// Message handler for the Edit -> Edit Census Records... menu point click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuCensus_Click(object sender,EventArgs e)
        {
            EditCensus();
        }

        /// <summary>
        /// Message handler for the Edit -> Remove Unlinked Places menu point click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuUnlinkedPlaces_Click(object sender,EventArgs e)
        {
            int nCount = m_oDb.PlaceRemoveUnlinked();
            MessageBox.Show(this,nCount.ToString() + " unlinked place(s) were removed.","Unlinked Places",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        /// <summary>
        /// Message handler for the Edit -> User Options... menu point click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuOptions_Click(object sender,EventArgs e)
        {
            UserOptions();
        }

        /// <summary>Message handler for the Edit -> Add Media menu point click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuAddMedia_Click(object sender, EventArgs e)
        {
            frmEditMedia oDialog = new frmEditMedia(m_oDb);
            if (oDialog.ShowDialog(this) == DialogResult.OK)
            {
                int nMediaID = oDialog.MediaID;
                ShowMedia(nMediaID, true);
            }
        }

        #endregion

        #region View Menu

        // Message handler for the "View" -> "Goto Person" menu point click.
        /// <summary>
        /// Message handler for the "View" -> "Goto Person" menu point click.
        /// Also the Goto person toolbar button click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuGoto_Click(object sender,EventArgs e)
        {
            GotoPerson();
        }

        // Message handler for the "View" -> "Image" menu point click
        /// <summary>
        /// Message handler for the "View" -> "Image" menu point click
        /// and the images toolbar button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuImage_Click(object sender,EventArgs e)
        {
            m_menuImage.Checked = !m_menuImage.Checked;
            m_tsbImage.Checked = m_menuImage.Checked;
            if(Current.Content == enumPage.Person)
            {
                clsPerson oPerson = new clsPerson(Current.ID, m_oDb);
                m_oWebBrowser.DocumentText = m_oOptions.RenderHtml(oPerson.Description(true, true, m_menuImage.Checked, m_menuLocation.Checked, true));
            }
        }

        // Message handler for the "View" -> "Location" menu point click.
        /// <summary>
        /// Message handler for the "View" -> "Location" menu point click.
        /// And the location toolbar button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuLocation_Click(object sender, EventArgs e)
        {
            m_menuLocation.Checked = !m_menuLocation.Checked;
            m_tsbLocation.Checked = m_menuLocation.Checked;
            
            switch(Current.Content )
            {
            case enumPage.Person :
                clsPerson oPerson = new clsPerson(Current.ID, m_oDb);
                m_oWebBrowser.DocumentText = m_oOptions.RenderHtml(oPerson.Description(true, true, m_menuImage.Checked, m_menuLocation.Checked, true));
                break;
            
            case enumPage.Place :
                ShowPlace(Current.ID, false);
                break;
            }
        }

        // Message handler for the "View" -> "Recent Changes" menu point click
        /// <summary>
        /// Message handler for the "View" -> "Recent Changes" menu point click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuRecentChanges_Click(object sender,EventArgs e)
        {
            ShowRecentChanges();
        }

        /// <summary>
        /// Message handler for the View -> ToDo menu point click.
        /// Display the To Do on the main window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuToDo_Click(object sender,EventArgs e)
        {
            ShowToDo();
        }

        /// <summary>
        /// Message handler for the View -> Calculate age menu point click.
        /// and the Age toolbar button click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuCalcAge_Click(object sender,EventArgs e)
        {
            ShowAge();
        }

        /// <summary>
        /// Message handler for the View -> Calculate Birthday menu point click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuBirthday_Click(object sender,EventArgs e)
        {
            // Display the birthday dialog
            frmBirthday oDialog = new frmBirthday();
            oDialog.ShowDialog(this);
        }
        
        /// <summary>
        /// Message handler for the View -> Reduce Width menu point click.
        /// and the reduce width toolbar button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuReduceWidth_Click(object sender,EventArgs e)
        {
            if(Current.Content == enumPage.Person)
            {
                m_PersonSize.x -= 8;
                m_dFontSize = 7.0f;
                ShowPerson(Current.ID,false);
            }
        }

        /// <summary>
        /// Message handler for the View -> Standard Width menu point click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuStandardWidth_Click(object sender,EventArgs e)
        {
            if(Current.Content == enumPage.Person)
            {
                m_PersonSize.x = 130;
                m_dFontSize = m_oOptions.FontBase.Size;
                ShowPerson(Current.ID,false);
            }
        }

        // Message handler for the "View" -> "html Source" menu point click
        /// <summary>
        /// Message handler for the "View" -> "html Source" menu point click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuHtmlSource_Click(object sender,EventArgs e)
        {
            // Create a file to hold the html source

            string sFilename = Innoval.clsDataPaths.GetMyDocuments() + "\\family tree source.html";

            // Open the filename for output
            StreamWriter oFile = new StreamWriter(sFilename,false);
            oFile.Write(m_oWebBrowser.DocumentText);
            oFile.Close();

            // Open the new file in notepad
            try
            {
                System.Diagnostics.Process.Start("notepad.exe","\"" + sFilename + "\"");
            }
            catch { }
        }

        #endregion

        #region Reports Menu

        // Message handler for the "Reports" -> "To Tree" menu point click
        /// <summary>
        /// Message handler for the "Reports" -> "To Tree" menu point click
        /// and the "Tree" toolbar button click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuToTree_Click(object sender,EventArgs e)
        {
            CreateTree();		
        }

        // Message handler for the "Reports" -> "To Html" menu point click.
        /// <summary>
        /// Message handler for the "Reports" -> "To Html" menu point click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuReportToHtml_Click(object sender, EventArgs e)
        {
            ReportToHtml();
        }

        #endregion

        #endregion

        #region Natigation (WebBrowser and Person Tree clicks)

        /// <summary>Message handler for the click on a ucPerson object event.  Responds the user clicking on a person object by displaying the person shown in the person object.  This is a bit like a message handler, but for my own event with my own signiture.</summary>
		private void ucPerson_evtClick(object oSender)
		{
			FamilyTree.Viewer.ucPerson psnPerson = (FamilyTree.Viewer.ucPerson)oSender;
			ShowPerson(psnPerson.GetPersonID(),true);
		}

        /// <summary>Message handler for the web browser trying to follow a link.</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WebBrowser_Navigating(object sender,WebBrowserNavigatingEventArgs e)
        {
            string sNewUrl = e.Url.ToString();
            int nColon = sNewUrl.IndexOf(':');
            if(nColon>0)
            {
                string sType = sNewUrl.Substring(0,nColon).ToLower();
                switch(sType)
                {
                case "person":
                    e.Cancel = true;
                    int nPersonID = int.Parse(sNewUrl.Substring(nColon+1));
                    ShowPerson(nPersonID,true);
                    break;
                case "source":
                    e.Cancel = true;
                    int nSourceID = int.Parse(sNewUrl.Substring(nColon+1));
                    ShowSource(nSourceID, true);
                    break;
                case "place":
                    e.Cancel = true;
                    int nPlaceID = int.Parse(sNewUrl.Substring(nColon+1));
                    ShowPlace(nPlaceID,true);
                    break;
                case "media":
                    e.Cancel = true;
                    int nMediaID = int.Parse(sNewUrl.Substring(nColon + 1));
                    ShowMedia(nMediaID,true);
                    break;
                }
            }
        }

        /// <summary>Message handler for a individual back (or forward) item click.  This is the drop down combo next to the back and forward buttons.</summary>
        /// <param name="oSender"></param>
        /// <param name="e"></param>
        private void IndividualBackItem_Click(object oSender, EventArgs e)
        {
            // Find the label of the sending button.
            string sLabel = oSender.ToString();

            // Find the index of the button in the back section.
            int nIndex = -1;
            for(int nI = m_nHistoryIndex - 1; nI >= 0; nI--)
            {
                if(m_History[nI].Label == sLabel)
                {
                    nIndex = nI;
                    nI = -1;
                }
            }

            // Search in the forward section if nothing found so far.
            if(nIndex < 0)
            {
                for(int nI = m_nHistoryIndex; nI <= m_nHistoryLast; nI++)
                {
                    if(m_History[nI].Label == sLabel)
                    {
                        nIndex = nI;
                        nI = m_nHistoryLast + 1;
                    }
                }
            }

            // Show the specified page
            if(nIndex >= 0)
            {
                m_nHistoryIndex = nIndex;
                ShowCurrentPage();
            }
        }

        #endregion

        #endregion

    }
}
