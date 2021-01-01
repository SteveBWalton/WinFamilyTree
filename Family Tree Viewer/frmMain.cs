// Family tree objects
using FamilyTree.Objects;
using System;
using System.Collections;
using System.Drawing;
// Disk Input Output
using System.IO;
// ParameterizedThreadStart
using System.Threading;
using System.Windows.Forms;

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
    public delegate void funcTextBool(string sText, bool bTrueFalse);

    // Delefate for functions that specify a single integer parameter.
    /// <summary>
    /// Delefate for functions that specify a single integer parameter.
    /// </summary>
    /// <param name="nValue">Specify the value of the integer parameter.</param>
    public delegate void dgtSetInt(int nValue);

    #endregion

    /// <summary>
    /// This is the main window of the application.
    /// This displays the current person and allows the user to move from person to person and select actions.
    /// </summary>
    public partial class MainWindow : System.Windows.Forms.Form
    {
        #region Member Variables and Types

        /// <summary>Enumeration to indicate the relationship of a new person to an existing person.</summary>
        private enum RelatedPerson
        {
            /// <summary>The new person is the father of the current person.</summary>
            FATHER,
            /// <summary>The new person is the mother of the current person.</summary>
            MOTHER,
            /// <summary>The new person has the same mother and father of the current person.</summary>
            SIBLING,
            /// <summary>The new person is a partner of the current person.</summary>
            PARTNER,
            /// <summary>The new person is a child of the current person.</summary>
            CHILD
        }

        /// <summary>Enumeration of indicate the type of the current main page.</summary>
        private enum Pages
        {
            /// <summary>The main page is displaying a person.</summary>
            PERSON,

            /// <summary>The main page is displaying a source document.</summary>
            SOURCE,

            /// <summary>The main page is displayed a place.</summary>
            PLACE,

            /// <summary>The main page is displaying a media object.</summary>
            MEDIA,

            /// <summary>The main page is displaying something else.</summary>
            OTHER
        }

        /// <summary>Type to represent a page on the main window.</summary>
        private struct Page
        {
            /// <summary>The type content on the page.</summary>
            public Pages content;

            /// <summary>The key to the type of content.</summary>
            public int index;

            /// <summary>A human readable label for the page.</summary>
            public string label;
        }

        /// <summary>Type to represent a point in integer space.</summary>
        private struct IntSize
        {
            /// <summary>Horizontal position.</summary>
            public int x;
            /// <summary>Vertical position.</summary>
            public int y;
        }

        /// <summary>ID of the current person.</summary>
        // private int m_nID;
        private Page[] history_;

        /// <summary>Position in the history array</summary>
        private int historyIndex_;

        /// <summary>Last valid position in the history array.</summary>
        private int historyLast_;

        /// <summary>Configuration file.</summary>
        private walton.XmlDocument config_;

        /// <summary>Current family tree database.</summary>
        private Database database_;

        /// <summary>User options.</summary>
        private clsUserOptions userOptions_;

        /// <summary>Array of person graphical controls to display siblings of the main person.</summary>
        private FamilyTree.Viewer.ucPerson[] psnSiblings_;

        /// <summary>Array of person graphical controls to display children of the main person.</summary>
        private FamilyTree.Viewer.ucPerson[] psnChildren_;

        /// <summary>Array of person graphical controls to display partners of the main person.</summary>
        private FamilyTree.Viewer.ucPerson[] psnPartners_;

        /// <summary>Array of relationship graphical controls to display connections to partners of the main person.</summary>
        private FamilyTree.Viewer.ucRelationship[] partnersConntections_;

        /// <summary>Background colour for a boy.</summary>
        private System.Drawing.Color backgroundBoy_;

        /// <summary>Background colour for a female.</summary>
        private System.Drawing.Color backgroundGirl_;

        /// <summary>Size of person control.</summary>
        private IntSize personSize_;

        /// <summary>Padding arround person controls.</summary>
        private IntSize padding_;

        /// <summary>Space between people who are married (or have a relationship).</summary>
        private int marriedWidth_;

        /// <summary>Size of the font used on the main person area.</summary>
        private float fontSize_;

        /// <summary>The list of recent files.</summary>
        private walton.FileList recentFiles_;

        /// <summary>The filename of a tree document to open at load.</summary>
        private string treeToOpen_;

        /// <summary>Count of the number of times that the progress bar has been progressed.</summary>
        private int numSteps_;

        #endregion

        #region Constructors etc



        /// <summary>Constructor for the main form.</summary>
        public MainWindow(string[] args)
        {
            // Required for Windows Form Designer support
            InitializeComponent();

            // Add any constructor code after InitializeComponent call
            psnFather_.evtClick += new dgtClick(ucPerson_evtClick);
            psnMother_.evtClick += new dgtClick(ucPerson_evtClick);
            psnFatherFather_.evtClick += new dgtClick(ucPerson_evtClick);
            psnFatherMother_.evtClick += new dgtClick(ucPerson_evtClick);
            psnMotherFather_.evtClick += new dgtClick(ucPerson_evtClick);
            psnMotherMother_.evtClick += new dgtClick(ucPerson_evtClick);

            // Open the configuration file			
            string configFile = walton.DataPaths.getUserDirectory("Walton", "Family Tree Viewer", "1.0");
            configFile += "\\config.xml";
            config_ = new walton.XmlDocument(configFile);

            // Open the recent files list
            recentFiles_ = new walton.FileList(4, config_);
            UpdateRecentFiles();

            // Initialise objects / variables
            database_ = null;
            userOptions_ = new clsUserOptions(config_);
            history_ = new Page[5];
            historyIndex_ = -1;

            psnSiblings_ = null;
            psnChildren_ = null;
            psnPartners_ = null;
            partnersConntections_ = null;
            backgroundBoy_ = System.Drawing.Color.LightSkyBlue;
            backgroundGirl_ = System.Drawing.Color.LightPink;
            personSize_.x = 130;
            personSize_.y = 70;
            padding_.x = 8;
            padding_.y = 16;
            marriedWidth_ = 8;
            fontSize_ = userOptions_.fontBase.size;

            string document = "";
            treeToOpen_ = "";

            // Open the most recent database.
            if (recentFiles_.getRecentFilename(0) != "")
            {
                document = recentFiles_.getRecentFilename(0);
            }

            // Loop through the program arguments.
            foreach (string argument in args)
            {
                if (Path.GetExtension(argument).ToLower() == ".tree")
                {
                    treeToOpen_ = argument;
                }
                /*
                else
                {
                    MessageBox.Show(this,sArgument,"Argument");
                }
                 */
            }

            // Open the database.
            if (document != "")
            {
                OpenDatabase(document);
            }
        }



        /// <summary>Clean up any resources being used.</summary>
		protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (database_ != null)
                {
                    database_.Dispose();
                    database_ = null;
                }

                cleanupDynamicControls();

                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }



        /// <summary>The main entry point for the application.</summary>
        [STAThread]
        static void Main(string[] sArgs)
        {
            // The following 2 lines enable XP styles in the applicaiton
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new MainWindow(sArgs));
        }



        #endregion

        #region Thread Safe UI Functions



        /// <summary>Thread safe, set the cursor back to the default cursor.</summary>
        private void cursorDefault()
        {
            // Check that we are on the UI thread.
            if (InvokeRequired)
            {
                Invoke(new funcVoid(cursorDefault));
                return;
            }

            // Now we are on the UI thread.
            // Important that the application wait cursor is reset before the form cursor and Tee chart cursor            
            Application.UseWaitCursor = false;
            // Cursor = Cursors.Default;
            Cursor.Current = Cursors.Default;
        }



        /// <summary>Thread safe, set the cursor to the wait cursor.</summary>
        private void cursorWait()
        {
            // Check that we are on the UI thread.
            if (InvokeRequired)
            {
                Invoke(new funcVoid(cursorWait));
                return;
            }

            // Now we are on the UI thread.    
            Application.UseWaitCursor = true;
            Cursor.Current = Cursors.WaitCursor;
            // Cursor = Cursors.WaitCursor;                        
        }



        /// <summary>Thread safe writes a message to the status bar.</summary>
        /// <param name="text"></param>
        /// <param name="isError"></param>
        public void setStatusBarText(string text, bool isError)
        {
            if (InvokeRequired)
            {
                Invoke(new funcTextBool(setStatusBarText), new object[2] { text, isError });
            }

            // Now we are on the UI thread
            tslabStatus_.Text = text;
        }



        /// <summary>Thread safe advance the progress bar one step.</summary>
        private void progressBarPerformStep()
        {
            // Check that we are no the UI thread
            if (InvokeRequired)
            {
                Invoke(new funcVoid(progressBarPerformStep));
                return;
            }

            // Now we are on the UI thread.       
            numSteps_++;
            tsProgressBar_.PerformStep();
        }



        /// <summary>Thread safe, show / hide the progress bar on the status bar.</summary>
        /// <param name="isVisible"></param>
        private void progressBarVisible(bool isVisible)
        {
            // Check that we are on the UI thread
            if (InvokeRequired)
            {
                Invoke(new dgtSetBool(progressBarVisible), new object[1] { isVisible });
                return;
            }

            // Now we are on the UI thread
            tsProgressBar_.Visible = isVisible;
        }



        /// <summary>Thread safe initialise the the progress bar.  This does not make the progress bar visible.</summary>
        /// <param name="maximum">Specify the maximum value for the progress bar.  This is the number of steps.</param>
        private void progressBarInitialise(int maximum)
        {
            // Check that we are on the UI thread
            if (InvokeRequired)
            {
                Invoke(new dgtSetInt(progressBarInitialise), new object[1] { maximum });
                return;
            }

            // Now we are on the UI thread
            numSteps_ = 0;
            tsProgressBar_.Value = 0;
            tsProgressBar_.Maximum = maximum;
        }



        #endregion

        #region Drawing the form



        /// <summary>Releases the resources for the dynamic controls on the form.  Ready for the controls to be reallocated to another person.</summary>
		/// <returns>True for success.  False for an error.</returns>
		private bool cleanupDynamicControls()
        {
            // Release the controls for siblings.
            if (psnSiblings_ != null)
            {
                for (int i = 0; i < psnSiblings_.Length; i++)
                {
                    if (psnSiblings_[i] != null)
                    {
                        panelTree_.Controls.Remove(psnSiblings_[i]);
                        psnSiblings_[i].Dispose();
                        psnSiblings_[i] = null;
                    }
                }
                psnSiblings_ = null;
            }

            // Release the controls for children.
            if (psnChildren_ != null)
            {
                for (int i = 0; i < psnChildren_.Length; i++)
                {
                    if (psnChildren_[i] != null)
                    {
                        panelTree_.Controls.Remove(psnChildren_[i]);
                        psnChildren_[i].Dispose();
                        psnChildren_[i] = null;
                    }
                }
                psnChildren_ = null;
            }

            // Release the controls for the partners (people controls).
            if (psnPartners_ != null)
            {
                for (int i = 0; i < psnPartners_.Length; i++)
                {
                    if (psnPartners_[i] != null)
                    {
                        panelTree_.Controls.Remove(psnPartners_[i]);
                        psnPartners_[i].Dispose();
                        psnPartners_[i] = null;
                    }
                }
                psnPartners_ = null;
            }

            // Release the controls for the partners (relationship controls).
            if (partnersConntections_ != null)
            {
                for (int i = 0; i < partnersConntections_.Length; i++)
                {
                    if (partnersConntections_[i] != null)
                    {
                        panelTree_.Controls.Remove(partnersConntections_[i]);
                        partnersConntections_[i].Dispose();
                        partnersConntections_[i] = null;
                    }
                }
                partnersConntections_ = null;
            }

            // return success.
            return true;
        }



        /// <summary>Draws the specified main person and their relationships.</summary>
		/// <param name="person">Specifies the person to be drawn</param>
		/// <param name="marriages">Specifies the relationships for the person</param>
		/// <param name="pos">Specifies the position to draw the person (number of older siblings)</param>
        /// <param name="font">Specifies the font to use for the standard text.</param>
        /// <returns>True for success, false otherwise</returns>
        private bool drawMainPerson(ref Person person, ref Relationship[] marriages, ref int pos, Font font)
        {
            // Draw the marriage partners if female.
            if (marriages.Length > 0)
            {
                if (person.isFemale)
                {
                    for (int i = marriages.Length - 1; i >= 0; i--)
                    {
                        Person relationPerson = database_.getPerson(marriages[i].partnerIndex);

                        // Create a person control to show the partner
                        psnPartners_[i] = new FamilyTree.Viewer.ucPerson();
                        psnPartners_[i].Location = new System.Drawing.Point(pos, labPerson_.Top);
                        psnPartners_[i].Size = new System.Drawing.Size(personSize_.x, personSize_.y);
                        psnPartners_[i].setPerson(relationPerson);
                        psnPartners_[i].evtClick += new dgtClick(ucPerson_evtClick);
                        psnPartners_[i].BackColor = backgroundBoy_;
                        psnPartners_[i].Font = font;
                        psnPartners_[i].setPerson(relationPerson);
                        panelTree_.Controls.Add(psnPartners_[i]);
                        pos += personSize_.x;

                        // Create a relationship control to show the relationship to the partner
                        partnersConntections_[i] = new FamilyTree.Viewer.ucRelationship();
                        partnersConntections_[i].Location = new System.Drawing.Point(pos, labPerson_.Top + 8);
                        partnersConntections_[i].Size = new System.Drawing.Size(marriedWidth_, 16);
                        partnersConntections_[i].SetRelationship(marriages[i]);
                        panelTree_.Controls.Add(partnersConntections_[i]);
                        pos += marriedWidth_;
                    }
                }
            }

            // Draw the actual person.
            labPerson_.Left = pos;
            labPerson_.Width = 2 * personSize_.x + marriedWidth_;
            labPersonDates_.Left = pos;
            labPersonDates_.Width = labPerson_.Width;
            labPersonDates_.Top = labPerson_.Top + labPerson_.Height;
            labPersonDates_.Font = font;
            pos += labPerson_.Width;

            // Draw the marriages if male.
            if (marriages.Length > 0)
            {
                if (person.isMale)
                {
                    for (int i = 0; i < marriages.Length; i++)
                    {
                        Person relationPerson = database_.getPerson(marriages[i].partnerIndex);

                        // Create a relationship control to show the relationship to the partner.
                        partnersConntections_[i] = new FamilyTree.Viewer.ucRelationship();
                        partnersConntections_[i].Location = new System.Drawing.Point(pos, labPerson_.Top + 8);
                        partnersConntections_[i].Size = new System.Drawing.Size(marriedWidth_, 16);
                        partnersConntections_[i].SetRelationship(marriages[i]);
                        panelTree_.Controls.Add(partnersConntections_[i]);
                        pos += marriedWidth_;

                        // Create a person control to show the partner.
                        psnPartners_[i] = new FamilyTree.Viewer.ucPerson();
                        psnPartners_[i].Location = new System.Drawing.Point(pos, labPerson_.Top);
                        psnPartners_[i].Size = new System.Drawing.Size(personSize_.x, personSize_.y);
                        psnPartners_[i].setPerson(relationPerson);
                        psnPartners_[i].evtClick += new dgtClick(ucPerson_evtClick);
                        psnPartners_[i].BackColor = backgroundGirl_;
                        psnPartners_[i].Font = font;
                        psnPartners_[i].setPerson(relationPerson);
                        panelTree_.Controls.Add(psnPartners_[i]);
                        pos += personSize_.x;
                    }
                }
            }

            pos += padding_.x;

            // Return success.
            return true;
        }



        /// <summary>Show the specified media object on the main window.</summary>
        /// <param name="nMediaID">Specifies the ID of the media object.</param>
        /// <param name="bAddHistory">Specifies true to add the page to history.</param>
        /// <returns>True for success, false otherwise.</returns>
        private bool ShowMedia(int nMediaID, bool bAddHistory)
        {
            // Hide the person tree panel
            panelTree_.Visible = false;

            // Find the place
            clsMedia oMedia = new clsMedia(database_, nMediaID);
            Text = oMedia.Filename + " - Family Tree Viewer";
            if (bAddHistory)
            {
                HistoryAdd(Pages.MEDIA, nMediaID, "Media: " + oMedia.Filename);
            }

            // Display the Html description of the source.
            m_oWebBrowser.DocumentText = userOptions_.renderHtml(oMedia.ToHtml());

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
            panelTree_.Visible = false;

            // Find the place
            Place oPlace = new Place(nPlaceID, database_);
            Text = oPlace.name + " - Family Tree Viewer";
            if (bAddHistory)
            {
                HistoryAdd(Pages.PLACE, nPlaceID, "Place: " + oPlace.name);
            }

            // Display the Html description of the source.
            string sPlace = oPlace.toHtml(m_menuLocation.Checked);
            m_oWebBrowser.DocumentText = userOptions_.renderHtml(sPlace);

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
            panelTree_.Visible = false;

            // Find the source ID.
            Source oSource = new Source(database_, nSourceID);
            Text = oSource.description + " - Family Tree Viewer";
            if (bAddHistory)
            {
                HistoryAdd(Pages.SOURCE, nSourceID, "Source: " + oSource.description);
            }

            // Display the Html description of the source.
            m_oWebBrowser.DocumentText = userOptions_.renderHtml(oSource.toHtml());

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
            Person oRelation;
            int nI;				// Loop variable
            int nJ;				// Loop variable
            int nPos;			// Horizontal position for the next control
            bool bShownPerson;	// True when the actual person has been shown
            int nTag;			// Workspace for the tag value for the person controls

            // Show the person tree panel
            panelTree_.Visible = true;

            // Show the specified person
            Person oPerson = database_.getPerson(nID);
            Text = oPerson.getName(true, false) + " - Family Tree Viewer";

            // Build the rich text file description of the person
            m_oWebBrowser.DocumentText = userOptions_.renderHtml(oPerson.Description(true, true, m_menuImage.Checked, m_menuLocation.Checked, true));

            // Update the back button            
            if (bAddHistory)
            {
                HistoryAdd(Pages.PERSON, nID, oPerson.getName(true, false));
            }
            // Font oFont = new System.Drawing.Font("Tahoma",m_dFontSize,System.Drawing.FontStyle.Regular,System.Drawing.GraphicsUnit.Point,((System.Byte)(0)));
            Font oFont = userOptions_.fontBase.getFont();

            // Show the father
            if (oPerson.fatherIndex == 0)
            {
                psnFather_.Visible = false;
                psnFatherFather_.Visible = false;
                psnFatherMother_.Visible = false;
                m_marFatherParents.Visible = false;
            }
            else
            {
                Person oFather = database_.getPerson(oPerson.fatherIndex);
                psnFather_.Width = personSize_.x;
                psnFather_.Height = personSize_.y;
                psnFather_.setPerson(oFather);
                psnFather_.Top = personSize_.y + padding_.y + TOP;
                psnFather_.Font = oFont;
                psnFather_.Visible = true;

                // Show the father's father
                if (oFather.fatherIndex == 0)
                {
                    psnFatherFather_.Visible = false;
                }
                else
                {
                    Person oGrandFather = database_.getPerson(oFather.fatherIndex);
                    psnFatherFather_.Width = personSize_.x;
                    psnFatherFather_.Height = personSize_.y;
                    psnFatherFather_.setPerson(oGrandFather);
                    psnFatherFather_.Top = TOP;
                    psnFatherFather_.Font = oFont;
                    psnFatherFather_.Visible = true;
                }

                // Show the father's mother
                if (oFather.motherIndex == 0)
                {
                    psnFatherMother_.Visible = false;
                }
                else
                {
                    Person oGrandMother = database_.getPerson(oFather.motherIndex);
                    psnFatherMother_.Width = personSize_.x;
                    psnFatherMother_.Height = personSize_.y;
                    psnFatherMother_.setPerson(oGrandMother);
                    psnFatherMother_.Top = TOP;
                    psnFatherMother_.Font = oFont;
                    psnFatherMother_.Visible = true;
                }

                // Show the relationship between these 2
                if (oFather.fatherIndex == 0 || oFather.motherIndex == 0)
                {
                    m_marFatherParents.Visible = false;
                }
                else
                {
                    m_marFatherParents.Visible = true;
                    m_marFatherParents.Width = marriedWidth_;
                    m_marFatherParents.Top = TOP;
                }
            }

            // Show the mother
            if (oPerson.motherIndex == 0)
            {
                psnMother_.Visible = false;
                psnMotherFather_.Visible = false;
                psnMotherMother_.Visible = false;
                m_marMotherParents.Visible = false;
            }
            else
            {
                Person oMother = database_.getPerson(oPerson.motherIndex);
                psnMother_.Width = personSize_.x;
                psnMother_.Height = personSize_.y;
                psnMother_.setPerson(oMother);
                psnMother_.Top = personSize_.y + padding_.y + TOP;
                psnMother_.Font = oFont;
                psnMother_.Visible = true;

                // Show the mother's father
                if (oMother.fatherIndex == 0)
                {
                    psnMotherFather_.Visible = false;
                }
                else
                {
                    Person oGrandFather = database_.getPerson(oMother.fatherIndex);
                    psnMotherFather_.Width = personSize_.x;
                    psnMotherFather_.Height = personSize_.y;
                    psnMotherFather_.setPerson(oGrandFather);
                    psnMotherFather_.Top = TOP;
                    psnMotherFather_.Font = oFont;
                    psnMotherFather_.Visible = true;
                }

                // Show the mother's mother
                if (oMother.motherIndex == 0)
                {
                    psnMotherMother_.Visible = false;
                }
                else
                {
                    Person oGrandMother = database_.getPerson(oMother.motherIndex);
                    psnMotherMother_.Width = personSize_.x;
                    psnMotherMother_.Height = personSize_.y;
                    psnMotherMother_.setPerson(oGrandMother);
                    psnMotherMother_.Top = TOP;
                    psnMotherMother_.Font = oFont;
                    psnMotherMother_.Visible = true;
                }

                // Show the relationship between these 2
                if (oMother.fatherIndex == 0 || oMother.motherIndex == 0)
                {
                    m_marMotherParents.Visible = false;
                }
                else
                {
                    m_marMotherParents.Visible = true;
                    m_marMotherParents.Width = marriedWidth_;
                    m_marMotherParents.Top = TOP;
                }
            }

            // Show the actual person
            labPerson_.Font = userOptions_.fontBaseTitle.getFont();
            labPerson_.Top = 2 * (personSize_.y + padding_.y) + TOP;
            labPerson_.Text = oPerson.getName(false, false);
            labPerson_.Width = 2 * personSize_.x + marriedWidth_;
            labPersonDates_.Top = labPerson_.Top + 23;
            labPersonDates_.Text = oPerson.shortDescription(true);
            labPersonDates_.Width = labPerson_.Width;
            labPersonDates_.Height = personSize_.y - 23;
            if (oPerson.isMale)
            {
                labPerson_.BackColor = backgroundBoy_;
            }
            else
            {
                labPerson_.BackColor = backgroundGirl_;
            }
            labPersonDates_.BackColor = labPerson_.BackColor;

            cleanupDynamicControls();

            Relationship[] Marriages = oPerson.getRelationships();
            if (Marriages.Length > 0)
            {
                psnPartners_ = new FamilyTree.Viewer.ucPerson[Marriages.Length];
                partnersConntections_ = new FamilyTree.Viewer.ucRelationship[Marriages.Length];
            }

            // Show the siblings            
            int[] Siblings = oPerson.getSiblings();
            nPos = 3;
            bShownPerson = false;
            if (Siblings.Length > 0)
            {
                psnSiblings_ = new FamilyTree.Viewer.ucPerson[Siblings.Length];

                for (nI = 0; nI < Siblings.Length; nI++)
                {
                    oRelation = database_.getPerson(Siblings[nI]);

                    // Show the person if he is older than the current sibling (and not already shown)
                    if (oPerson.dob.date < oRelation.dob.date && !bShownPerson)
                    {
                        drawMainPerson(ref oPerson, ref Marriages, ref nPos, oFont);
                        bShownPerson = true;
                    }

                    // Show the sibling
                    psnSiblings_[nI] = new FamilyTree.Viewer.ucPerson();
                    if (oRelation.isMale)
                    {
                        psnSiblings_[nI].BackColor = backgroundBoy_;
                    }
                    else
                    {
                        psnSiblings_[nI].BackColor = backgroundGirl_;
                    }
                    psnSiblings_[nI].Font = oFont;
                    psnSiblings_[nI].Location = new System.Drawing.Point(nPos, labPerson_.Top);
                    psnSiblings_[nI].Size = new System.Drawing.Size(personSize_.x, personSize_.y);
                    psnSiblings_[nI].setPerson(oRelation);
                    psnSiblings_[nI].evtClick += new dgtClick(ucPerson_evtClick);

                    // Build a tag value that represents which parents this sibling shares
                    nTag = 0;
                    if (oRelation.fatherIndex == oPerson.fatherIndex)
                    {
                        nTag |= 1;
                    }
                    if (oRelation.motherIndex == oPerson.motherIndex)
                    {
                        nTag |= 2;
                    }
                    psnSiblings_[nI].Tag = nTag;

                    // this.Controls.Add(m_psnSiblings[nI]);
                    panelTree_.Controls.Add(psnSiblings_[nI]);
                    nPos += personSize_.x + padding_.x;
                }
            }
            if (!bShownPerson)
            {
                drawMainPerson(ref oPerson, ref Marriages, ref nPos, oFont);
            }

            // Reposition the parents (X direction)
            psnFather_.Left = labPerson_.Left;
            m_marParents.Left = psnFather_.Left + personSize_.x;
            m_marParents.Top = psnFather_.Top;
            m_marParents.Width = marriedWidth_;
            psnMother_.Left = labPerson_.Left + personSize_.x + marriedWidth_;

            // Reposition the grandparents (X direction)
            psnFatherMother_.Left = labPerson_.Left;
            psnFatherFather_.Left = psnFatherMother_.Left - marriedWidth_ - personSize_.x;
            psnMotherFather_.Left = labPerson_.Left + personSize_.x + marriedWidth_;
            psnMotherMother_.Left = psnMotherFather_.Left + psnMotherFather_.Width + m_marParents.Width;
            if (psnFatherFather_.Left < 3)
            {
                int nOffset = 3 - psnFatherFather_.Left;
                psnFatherFather_.Left += nOffset;
                psnFatherMother_.Left += nOffset;
                psnMotherFather_.Left += nOffset;
                psnMotherMother_.Left += nOffset;
            }
            m_marFatherParents.Left = psnFatherFather_.Left + personSize_.x;
            m_marFatherParents.Top = psnFatherFather_.Top + 8;
            m_marMotherParents.Left = psnMotherFather_.Left + personSize_.x;
            m_marMotherParents.Top = psnMotherFather_.Top + 8;

            // Show the children
            int[] Children = oPerson.getChildren();
            int nHeight = labPerson_.Top + personSize_.y + padding_.y;
            if (oPerson.isMale)
            {
                nPos = labPerson_.Left;
            }
            else
            {
                if (partnersConntections_ != null)
                {
                    nPos = psnPartners_[partnersConntections_.Length - 1].Left;
                }
                else
                {
                    nPos = labPerson_.Left;
                }
            }

            if (Children.Length > 0)
            {
                psnChildren_ = new FamilyTree.Viewer.ucPerson[Children.Length];

                for (nI = 0; nI < Children.Length; nI++)
                {
                    oRelation = database_.getPerson(Children[nI]);

                    psnChildren_[nI] = new FamilyTree.Viewer.ucPerson();
                    if (oRelation.isMale)
                    {
                        psnChildren_[nI].BackColor = backgroundBoy_;
                    }
                    else
                    {
                        psnChildren_[nI].BackColor = backgroundGirl_;
                    }
                    psnChildren_[nI].Font = oFont;
                    psnChildren_[nI].Location = new System.Drawing.Point(nPos, nHeight);
                    psnChildren_[nI].Size = new System.Drawing.Size(personSize_.x, personSize_.y);
                    psnChildren_[nI].setPerson(oRelation);
                    psnChildren_[nI].evtClick += new dgtClick(ucPerson_evtClick);

                    // Decide which relationship this child belongs to
                    nTag = -1;
                    if (partnersConntections_ != null)
                    {
                        for (nJ = 0; nJ < partnersConntections_.Length; nJ++)
                        {
                            if (partnersConntections_[nJ].MotherID == oRelation.motherIndex && partnersConntections_[nJ].FatherID == oRelation.fatherIndex)
                            {
                                nTag = nJ;
                            }
                        }
                    }
                    psnChildren_[nI].Tag = nTag;

                    // this.Controls.Add(m_psnChildren[nI]);
                    panelTree_.Controls.Add(psnChildren_[nI]);
                    nPos += personSize_.x + padding_.x;
                }
            }

            // Reposition the children if off the right edge
            if (Children.Length > 0)
            {
                if (psnChildren_[Children.Length - 1].Left + personSize_.x > Width)
                {
                    int nOffset = psnChildren_[Children.Length - 1].Left + personSize_.x - Width;
                    for (nI = 0; nI < Children.Length; nI++)
                    {
                        psnChildren_[nI].Left -= nOffset;
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
        private Page Current
        {
            get
            {
                if (historyIndex_ >= 0)
                {
                    return history_[historyIndex_];
                }
                return history_[0];
            }
        }

        /// <summary>
        /// Add the current page to the history.
        /// </summary>
        /// <param name="nType">Specifies the type of page.</param>
        /// <param name="nID">Specifies the ID of the page.</param>
        /// <param name="sLabel">Specifies a human readable label for the page.</param>
        private void HistoryAdd(Pages nType, int nID, string sLabel)
        {
            // Check that some thing has changed.
            if (Current.content == nType && Current.index == nID)
            {
                return;
            }

            // Add to the history
            historyIndex_++;
            if (historyIndex_ >= history_.Length)
            {
                // Move all the page down one.
                for (int nIndex = 0; nIndex < history_.Length - 1; nIndex++)
                {
                    history_[nIndex] = history_[nIndex + 1];
                }

                // Move back to the end of the history
                historyIndex_--;
            }

            // Set the current page
            history_[historyIndex_].index = nID;
            history_[historyIndex_].content = nType;
            history_[historyIndex_].label = sLabel;
            historyLast_ = historyIndex_;

            // Update the back button
            UpdateBackButton();
        }

        /// <summary>
        /// Move to the page that is back in the history.
        /// </summary>
        private void HistoryBack()
        {
            // Check that there is some history to move back into
            if (historyIndex_ < 1)
            {
                return;
            }

            // Move back in the history            
            historyIndex_--;
            ShowCurrentPage();
        }

        /// <summary>
        /// Move to the page that is forward in the history (if available).
        /// If no page is available then nothing happens.
        /// </summary>
        private void HistoryForward()
        {
            // Check that there is some history to move into.
            if (historyIndex_ == historyLast_)
            {
                return;
            }

            // Move forward in the history
            historyIndex_++;
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
            switch (history_[historyIndex_].content)
            {
            case Pages.PERSON:
                ShowPerson(history_[historyIndex_].index, false);
                break;
            case Pages.SOURCE:
                ShowSource(history_[historyIndex_].index, false);
                break;
            case Pages.PLACE:
                ShowPlace(history_[historyIndex_].index, false);
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
            if (historyIndex_ > 0)
            {
                m_tsbBack.Enabled = true;
                m_tsddbBack.Enabled = true;

                m_tsddbBack.DropDownItems.Clear();
                for (int nI = historyIndex_ - 1; nI >= 0; nI--)
                {
                    m_tsddbBack.DropDownItems.Add(history_[nI].label, null, IndividualBackItem_Click);
                }
            }
            else
            {
                m_tsbBack.Enabled = false;
                m_tsddbBack.Enabled = false;
            }

            // Update the forward button
            if (historyIndex_ == historyLast_)
            {
                m_tsbForward.Enabled = false;
                m_tsddbForward.Enabled = false;
            }
            else
            {
                m_tsbForward.Enabled = true;
                m_tsddbForward.Enabled = true;
                m_tsddbForward.DropDownItems.Clear();
                for (int nI = historyIndex_ + 1; nI <= historyLast_; nI++)
                {
                    m_tsddbForward.DropDownItems.Add(history_[nI].label, null, IndividualBackItem_Click);
                }
            }
        }

        #endregion

        /// <summary>Displays the home / default person.</summary>
        /// <returns>True, if successful.  False, otherwise.</returns>
        private bool GoHome()
        {
            // Find the home person and display them
            walton.XmlNode oHome = config_.getNode("useroptions/home");
            int nHomeID = oHome.getAttributeValue("id", 1, true);
            ShowPerson(nHomeID, true);

            // Return success
            return true;
        }



        /// <summary>Display a dialog box and allow the user to select the person to show.</summary>
        /// <returns></returns>
        private bool GotoPerson()
        {
            // Allow the user to select a person
            frmSelectPerson oDialog = new frmSelectPerson();
            int nPersonID = oDialog.SelectPerson(this, database_);

            // If the user did select a person then show that person
            if (nPersonID >= 0)
            {
                ShowPerson(nPersonID, true);
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
            switch (Current.content)
            {
            case Pages.PERSON:
                // Create a dialog to edit this person
                frmEditPerson dlgEdit = new frmEditPerson(Current.index, database_);

                // Show the dialog and wait for the dialog to close
                if (dlgEdit.ShowDialog(this) == DialogResult.OK)
                {
                    // Refresh the display of the current person
                    ShowPerson(Current.index, false);
                }
                dlgEdit.Dispose();
                break;

            case Pages.PLACE:
                // Create a dialog to edit this place
                frmEditPlace oEditPlace = new frmEditPlace(Current.index, database_);

                // Show the dialog and wait for the dialog to close
                if (oEditPlace.ShowDialog(this) == DialogResult.OK)
                {
                    // Refresh the display of this place
                    ShowPlace(Current.index, false);
                }
                oEditPlace.Dispose();
                break;

            case Pages.SOURCE:
                // Create a dialog to edit this source
                frmEditSources oEditSource = new frmEditSources(database_, Current.index);
                if (oEditSource.ShowDialog(this) == DialogResult.OK)
                {
                    // Refresh the display of this source
                    ShowSource(Current.index, false);
                }
                oEditSource.Dispose();
                break;

            case Pages.MEDIA:
                // Create a dialog to edit this media
                frmEditMedia oEditMedia = new frmEditMedia(database_, Current.index);
                if (oEditMedia.ShowDialog(this) == DialogResult.OK)
                {
                    // Refresh the display of this source
                    ShowMedia(Current.index, false);
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
        private bool AddPerson(RelatedPerson Relation)
        {
            // Create a dialog to edit the new person
            frmEditPerson dlgEdit = new frmEditPerson(database_);

            // Show the dialog and wait for the dialig to close
            if (dlgEdit.ShowDialog(this) == DialogResult.OK)
            {
                // A new person was created link to the current person in the required way
                int nNewPersonID = dlgEdit.GetPersonID();

                Person oPerson;
                switch (Relation)
                {
                case RelatedPerson.FATHER:
                    oPerson = new Person(Current.index, database_);
                    oPerson.fatherIndex = nNewPersonID;
                    oPerson.save();
                    break;

                case RelatedPerson.MOTHER:
                    oPerson = new Person(Current.index, database_);
                    oPerson.motherIndex = nNewPersonID;
                    oPerson.save();
                    break;

                case RelatedPerson.SIBLING:
                    Person oNewPerson = new Person(nNewPersonID, database_);
                    oPerson = new Person(Current.index, database_);
                    oNewPerson.fatherIndex = oPerson.fatherIndex;
                    oNewPerson.motherIndex = oPerson.motherIndex;
                    oNewPerson.save();
                    break;

                case RelatedPerson.PARTNER:
                    oPerson = new Person(Current.index, database_);
                    Relationship oRelationship = new Relationship(oPerson, nNewPersonID);
                    oRelationship.save();
                    oPerson.addRelationship(oRelationship);
                    break;

                case RelatedPerson.CHILD:
                    oNewPerson = new Person(nNewPersonID, database_);
                    oPerson = new Person(Current.index, database_);
                    if (oPerson.isMale)
                    {
                        oNewPerson.fatherIndex = Current.index;
                    }
                    else
                    {
                        oNewPerson.motherIndex = Current.index;
                    }
                    oNewPerson.save();
                    break;

                }

                // Refresh the display of the current person
                ShowPerson(Current.index, false);
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
            frmEditSources oEdit = new frmEditSources(database_);

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
            if (oOptions.UpdateOptions(this, ref userOptions_))
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
            clsTreeDocument oTree = new clsTreeDocument(database_, userOptions_, Current.index);

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
            if (m_OpenFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                // Open the selected file
                OpenTree(m_OpenFileDialog.FileName);
            }

            // Return success
            return true;
        }



        /// <summary>Opens the specified tree file in a new window.</summary>
        /// <param name="sTreeFile">Specifies the full filename of the tree file.</param>
        /// <returns>True for success, false otherwise.</returns>
        private bool OpenTree(string sTreeFile)
        {
            // Open the .tree file.
            walton.XmlDocument oTreeFile = new walton.XmlDocument(sTreeFile);

            // Create the tree document.
            clsTreeDocument oTree = new clsTreeDocument(database_, oTreeFile);

            // Create a tree preview window.
            frmViewTree oTreeWindow = new frmViewTree(oTree);
            oTreeWindow.Show();

            // Return success.
            return true;
        }



        /// <summary>Create a html report about the current person.</summary>
        /// <returns>True for success, false otherwise.</returns>
        private bool ReportToHtml()
        {
            // Create a report object
            clsReport oReport = new clsReport(Current.index, database_, userOptions_);

            // Hide the person tree panel
            panelTree_.Visible = false;

            // Display the Html description of the source.
            m_oWebBrowser.DocumentText = userOptions_.renderHtml(oReport.GetReport());

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
            frmAge oAge = new frmAge(database_, Current.index);
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
            frmEditCensus oCensus = new frmEditCensus(database_, 0);

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
            panelTree_.Visible = false;

            // Display the Html description of the source.
            m_oWebBrowser.DocumentText = userOptions_.renderHtml(database_.getRecentChangesAsHtml());

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
            panelTree_.Visible = false;

            // Display the Html description of the source.
            m_oWebBrowser.DocumentText = userOptions_.renderHtml(database_.getToDoAsHtml());

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
            if (m_OpenFileDialog.ShowDialog(this) == DialogResult.Cancel)
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
            if (!File.Exists(sFilename))
            {
                return false;
            }

            // Close the old database
            if (database_ != null)
            {
                database_.Dispose();
            }

            // Open the new database
            database_ = new Database(sFilename);

            // Update the recent files
            recentFiles_.openFile(sFilename);
            UpdateRecentFiles();

            // Show the default person
            GoHome();

            // Update the status bar
            setStatusBarText("Opened " + sFilename, false);

            // Return new database
            return true;
        }



        /// <summary>Updates the recent file menu.</summary>
        private void UpdateRecentFiles()
        {
            if (recentFiles_.getRecentFilename(0) != "")
            {
                m_menuRecentFile1.Text = "1 " + recentFiles_.getDisplayName(0);
                m_menuRecentFile1.Visible = true;
            }
            else
            {
                m_menuRecentFile1.Visible = false;
            }
            if (recentFiles_.getRecentFilename(1) != "")
            {
                m_menuRecentFile2.Text = "2 " + recentFiles_.getDisplayName(1);
                m_menuRecentFile2.Visible = true;
            }
            else
            {
                m_menuRecentFile2.Visible = false;
            }
            if (recentFiles_.getRecentFilename(2) != "")
            {
                m_menuRecentFile3.Text = "3 " + recentFiles_.getDisplayName(2);
                m_menuRecentFile3.Visible = true;
            }
            else
            {
                m_menuRecentFile3.Visible = false;
            }
            if (recentFiles_.getRecentFilename(3) != "")
            {
                m_menuRecentFile4.Text = "4 " + recentFiles_.getDisplayName(3);
                m_menuRecentFile4.Visible = true;
            }
            else
            {
                m_menuRecentFile4.Visible = false;
            }
        }



        /// <summary>Allows the user to select an output file to write gedcom into.  Currently this is a the gedcom of the whole database.</summary>
        private void ExportGedcom()
        {
            frmGedcomOptions oDialog = new frmGedcomOptions(userOptions_.gedcomOptions);
            if (oDialog.ShowDialog(this) == DialogResult.OK)
            {
                // Save the user options
                userOptions_.save();

                // Unreachable code
#pragma warning disable 162
                if (true)
                {
                    // Create a new thread to do the work.
                    ParameterizedThreadStart oThreadMethod = new ParameterizedThreadStart(WriteGedcom);
                    Thread oThread = new Thread(oThreadMethod);
                    oThread.Start(userOptions_.gedcomOptions);
                }
                else
                {
                    // Save the gedcom into the specified file				
                    WriteGedcom(userOptions_.gedcomOptions);
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

            if (m_SaveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                database_.writeSql(m_SaveFileDialog.FileName);
            }
        }

        /// <summary>Writes gedcom data into the specified file.  Why is this in the main form would expect this to be in FTObjects?</summary>
        /// <param name="oParameter">Specifies the filename of the gedcom file to create.</param>		
        private void WriteGedcom(object oParameter)
        {
            // Show the wait cursor.
            cursorWait();

            // Estimate the number of steps required.
            walton.XmlNode xmlGedcom = config_.getNode("gedcom");
            int nNumSteps = xmlGedcom.getAttributeValue("steps", 1000, true);
            progressBarInitialise(nNumSteps);
            progressBarVisible(true);

            // Decode the parameter.
            GedcomOptions oOptions = (GedcomOptions)oParameter;
            string sFilename = oOptions.fileName;

            // Open the output file.
            StreamWriter oFile = new StreamWriter(sFilename, false);

            // Write the Gedcom header.
            oFile.WriteLine("0 HEAD");
            oFile.WriteLine("1 SOUR FamilyTree");
            oFile.WriteLine("2 NAME FamilyTree");
            oFile.WriteLine("2 VERS 1.0.0");
            oFile.WriteLine("1 DEST DISKETTE");
            oFile.WriteLine("1 DATE " + DateTime.Now.ToString("d MMM yyyy"));
            oFile.WriteLine("2 TIME " + DateTime.Now.ToString("HH:mm:ss"));
            oFile.WriteLine("1 CHAR UTF-8");
            oFile.WriteLine("1 FILE " + Path.GetFileName(oOptions.fileName));

            // Create a list of Gedcom families objects @F1@ etc ...
            clsFamilies oFamilies = new clsFamilies();

            // Write the individuals.
            IndexName[] oAllPeople = database_.getPeople();
            for (int nI = 0; nI < oAllPeople.Length; nI++)
            {
                // Create an object for this person.
                Person oPerson = database_.getPerson(oAllPeople[nI].index);

                // Check that this person is included in the gedcom file.
                // The person will also need to be excluded from any families that try to reference him.
                if (oPerson.isIncludeInGedcom)
                {
                    // Create Gedcom record for this person
                    oFile.WriteLine("0 @I" + oPerson.index.ToString("0000") + "@ INDI");

                    // Create an intial list of sources for this person
                    ArrayList oPersonSources = new ArrayList();
                    oPerson.sourceNonSpecific.gedcomAdd(oPersonSources);

                    oFile.WriteLine("1 NAME " + oPerson.forenames + " /" + oPerson.birthSurname + "/");
                    oFile.WriteLine("2 GIVN " + oPerson.forenames);
                    oFile.WriteLine("2 SURN " + oPerson.birthSurname);
                    // oPerson.SourceName.WriteGedcom(2,oFile,null);
                    oPerson.sourceName.gedcomAdd(oPersonSources);

                    // Get the occupation information
                    clsFact[] oFacts = oPerson.getFacts(20);
                    if (oFacts.Length > 0)
                    {
                        oFile.Write("2 OCCU ");
                        bool bFirst = true;
                        foreach (clsFact oFact in oFacts)
                        {
                            if (bFirst)
                            {
                                bFirst = false;
                            }
                            else
                            {
                                oFile.Write(", ");
                            }
                            oFile.Write(oFact.information);
                            // oFact.Sources.WriteGedcom(3,oFile,null);
                        }
                        oFile.WriteLine();
                    }

                    oFile.Write("1 SEX ");
                    if (oPerson.isMale)
                    {
                        oFile.WriteLine("M");
                    }
                    else
                    {
                        oFile.WriteLine("F");
                    }
                    oFile.WriteLine("1 BIRT");
                    oFile.WriteLine("2 DATE " + oPerson.dob.format(DateFormat.GEDCOM));
                    database_.writeGedcomPlace(oFile, 2, oPerson.getSimpleFact(10), oOptions);

                    // oPerson.SourceDoB.WriteGedcom(2,oFile,null);
                    oPerson.sourceDoB.gedcomAdd(oPersonSources);

                    if (!oPerson.dod.isEmpty())
                    {
                        oFile.WriteLine("1 DEAT Y");
                        if (!oPerson.dod.isEmpty())
                        {
                            oFile.WriteLine("2 DATE " + oPerson.dod.format(DateFormat.GEDCOM));
                        }
                        database_.writeGedcomPlace(oFile, 2, oPerson.getSimpleFact(90), oOptions);
                        string sCauseOfDeath = oPerson.getSimpleFact(92);
                        if (sCauseOfDeath != "")
                        {
                            oFile.WriteLine("2 CAUS " + sCauseOfDeath);
                        }
                        // oPerson.SourceDoD.WriteGedcom(2,oFile,null);
                        oPerson.SourceDoD.gedcomAdd(oPersonSources);
                    }

                    // Create a list of the partners
                    ArrayList oPartners = new ArrayList();

                    // Get the relationship information				
                    Relationship[] oRelationships = oPerson.getRelationships();
                    for (int nJ = 0; nJ < oRelationships.Length; nJ++)
                    {
                        // Check that the partner is included in the Gedcom file
                        Person oPartner = new Person(oRelationships[nJ].partnerIndex, database_);
                        if (oPartner.isIncludeInGedcom)
                        {
                            clsFamily oMarriage = oFamilies.getMarriageFamily(oRelationships[nJ].maleIndex, oRelationships[nJ].femaleIndex, oRelationships[nJ].index);
                            oFile.WriteLine("1 FAMS @F" + oMarriage.gedcomIndex.ToString("0000") + "@");

                            // Add to the list of partners
                            oPartners.Add(oRelationships[nJ].partnerIndex);
                        }
                    }

                    // Add the partners in children not already picked up
                    int[] nChildren = oPerson.getChildren();
                    for (int nJ = 0; nJ < nChildren.Length; nJ++)
                    {
                        Person oChild = database_.getPerson(nChildren[nJ]);

                        // Check that the childs parent is already a partner
                        if (oPerson.isMale)
                        {
                            if (!oPartners.Contains(oChild.motherIndex))
                            {
                                int nMotherID = oChild.motherIndex;
                                Person oMother = new Person(oChild.motherIndex, database_);
                                if (!oMother.isIncludeInGedcom)
                                {
                                    // Use an unknown mother
                                    nMotherID = 0;
                                }

                                // Create families for new partner
                                clsFamily oMarriage = oFamilies.getMarriageFamily(oPerson.index, nMotherID, 0);
                                oFile.WriteLine("1 FAMS @F" + oMarriage.gedcomIndex.ToString("0000") + "@");

                                // Add to the list of partners
                                oPartners.Add(nMotherID);
                            }
                        }
                        else
                        {
                            if (!oPartners.Contains(oChild.fatherIndex))
                            {
                                int nFatherID = oChild.fatherIndex;
                                Person oFather = new Person(oChild.fatherIndex, database_);
                                if (!oFather.isIncludeInGedcom)
                                {
                                    // Use an unknown father
                                    nFatherID = 0;
                                }

                                // Create families for new partner
                                clsFamily oMarriage = oFamilies.getMarriageFamily(nFatherID, oPerson.index, 0);
                                oFile.WriteLine("1 FAMS @F" + oMarriage.gedcomIndex.ToString("0000") + "@");

                                // Add to the list of partners
                                oPartners.Add(nFatherID);
                            }
                        }
                    }

                    // Add this person to the parents family
                    if (oPerson.fatherIndex > 0 || oPerson.motherIndex > 0)
                    {
                        // Check that the father is included in the gedcom file.
                        int nFatherID = oPerson.fatherIndex;
                        Person oFather = new Person(nFatherID, database_);
                        if (!oFather.isIncludeInGedcom)
                        {
                            nFatherID = 0;
                        }

                        // Check that the mother is included in the gedcom file.
                        int nMotherID = oPerson.motherIndex;
                        Person oMother = new Person(nMotherID, database_);
                        if (!oMother.isIncludeInGedcom)
                        {
                            nMotherID = 0;
                        }

                        //  Get the parent family information
                        if (nMotherID != 0 || nFatherID != 0)
                        {
                            clsFamily oFamily = oFamilies.getParentFamily(oPerson.fatherIndex, oPerson.motherIndex);
                            oFamily.addChild(oPerson);
                            oFile.WriteLine("1 FAMC @F" + oFamily.gedcomIndex.ToString("0000") + "@");
                        }
                    }

                    // Get Census records
                    clsCensusPerson[] oCensui = database_.censusForPerson(oPerson.index);
                    foreach (clsCensusPerson oCensus in oCensui)
                    {
                        oFile.WriteLine("1 CENS");
                        oFile.WriteLine("2 DATE " + oCensus.date.ToString("d MMM yyyy"));
                        database_.writeGedcomPlace(oFile, 2, oCensus.houseHoldName, oOptions);
                        if (oCensus.occupation != "")
                        {
                            oFile.WriteLine("2 OCCU " + oCensus.occupation);
                        }
                        oFile.WriteLine("2 NOTE " + oCensus.livingWith(database_));
                        Sources oSources = oCensus.getSources(database_);
                        if (oSources != null)
                        {
                            // oSources.WriteGedcom(2,oFile,null);
                            oSources.gedcomAdd(oPersonSources);
                        }
                    }

                    // Attach the  media
                    database_.GedcomWritePersonMedia(oFile, oPerson.index, oPerson.MediaID);

                    // Attached the list of sources
                    foreach (int nSourceID in oPersonSources)
                    {
                        oFile.WriteLine("1 SOUR @S" + nSourceID.ToString("0000") + "@");
                    }

                    // Write the last edit information
                    if (oPerson.lastEditBy != "")
                    {
                        oFile.WriteLine("1 CHAN");
                        oFile.WriteLine("2 DATE " + oPerson.lastEditDate.ToString("d MMM yyyy"));
                        oFile.WriteLine("3 TIME " + oPerson.lastEditDate.ToString("HH:mm:ss"));
                        if (oOptions.isIncludePGVU)
                        {
                            oFile.WriteLine("2 _PGVU " + oPerson.lastEditBy);
                        }
                    }
                }

                // Progress Bar
                progressBarPerformStep();
            }

            // Write the family records @F1@...
            oFamilies.WriteGedcom(oFile, database_, progressBarPerformStep, oOptions);

            // Write all the source records @S1@ etc...
            database_.WriteSourcesGedcom(oFile, progressBarPerformStep, oOptions);

            // Write all the media records @M1@ etc...
            database_.GedcomWriteMedia(oFile);

            // Write the repository records @R1@ etc ...
            database_.WriteRepositoriesGedcom(oFile);

            // Close the Gedcom header
            oFile.WriteLine("0 TRLR");

            // Close the output file
            oFile.Close();

            // Save the number of steps for next time
            xmlGedcom.setAttributeValue("steps", numSteps_);

            // Restore the default cursor etc.
            progressBarVisible(false);
            cursorDefault();
        }

        #endregion

        #region Message Handlers

        #region Form Events



        /// <summary>Message handler for the paint event on the Panel tree control.  Draw the lines from parents to children etc ...</summary>
        /// <remarks>This is basically the paint event for the form.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PanelTree_Paint(object sender, PaintEventArgs e)
        {
            // Create a pen to draw connections.
            Pen pen = new Pen(Color.Black, 2);

            // Draw a line up for the person.
            int pos = labPerson_.Left + labPerson_.Width / 2;
            e.Graphics.DrawLine(pen, pos, labPerson_.Top, pos, labPerson_.Top - padding_.y);

            // Draw lines up for the siblings
            if (psnSiblings_ != null)
            {
                for (int connectionType = 1; connectionType <= 3; connectionType++)
                {
                    int barHeight = 0;
                    int maxPos = 0;
                    int minPos = 0;
                    bool isDrawBar = false;
                    switch (connectionType)
                    {
                    case 1:
                        barHeight = labPerson_.Top - (padding_.y / 2) + 4;
                        pos = psnFather_.Left + psnFather_.Width / 2;
                        minPos = pos;
                        maxPos = pos;
                        break;

                    case 2:
                        barHeight = labPerson_.Top - (padding_.y / 2) - 4;
                        pos = psnMother_.Left + psnMother_.Width / 2;
                        minPos = pos;
                        maxPos = pos;
                        break;

                    case 3:
                        barHeight = labPerson_.Top - (padding_.y / 2);
                        pos = labPerson_.Left + labPerson_.Width / 2;
                        minPos = pos;
                        maxPos = pos;
                        break;
                    }

                    for (int i = 0; i < psnSiblings_.Length; i++)
                    {
                        // TODO: Check this.  This was crashing.
                        if (psnSiblings_[i].Tag != null)
                        {
                            if ((int)psnSiblings_[i].Tag == connectionType)
                            {
                                isDrawBar = true;

                                pos = psnSiblings_[i].Left + psnSiblings_[i].Width / 2;
                                if (pos < minPos)
                                {
                                    minPos = pos;
                                }
                                if (pos > maxPos)
                                {
                                    maxPos = pos;
                                }

                                e.Graphics.DrawLine(pen, pos, labPerson_.Top, pos, barHeight);
                            }
                        }
                    }

                    // Draw a bar to hang the Siblings on.
                    e.Graphics.DrawLine(pen, minPos, barHeight, maxPos, barHeight);

                    // Draw a line up to the parents relationship.
                    switch (connectionType)
                    {
                    case 1:
                        if (isDrawBar)
                        {
                            pos = psnFather_.Left + psnFather_.Width / 2;
                            e.Graphics.DrawLine(pen, pos, psnFather_.Top + psnFather_.Height, pos, barHeight);
                        }
                        break;

                    case 2:
                        if (isDrawBar)
                        {
                            pos = psnMother_.Left + psnMother_.Width / 2;
                            e.Graphics.DrawLine(pen, pos, psnMother_.Top + psnMother_.Height, pos, barHeight);
                        }
                        break;
                    }
                }

                // Draw a line down for the siblings with descendants.
                for (int i = 0; i < psnSiblings_.Length; i++)
                {
                    Person sibling = database_.getPerson(psnSiblings_[i].getPersonIndex());
                    if (sibling.hasChildren())
                    {
                        e.Graphics.DrawLine(pen, psnSiblings_[i].Left + psnSiblings_[i].Width / 2, psnSiblings_[i].Top + psnSiblings_[i].Height, psnSiblings_[i].Left + psnSiblings_[i].Width / 2, psnSiblings_[i].Top + psnSiblings_[i].Height + 3);
                    }
                }
            }

            // There is always a line up to the parents
            pos = m_marParents.Left + m_marParents.Width / 2;
            e.Graphics.DrawLine(pen, pos, labPerson_.Top - padding_.y, pos, m_marParents.Top + m_marParents.Height);

            // Draw a line from the father to his parents
            if (psnFather_.Visible)
            {
                e.Graphics.DrawLine(pen, psnFather_.Left + psnFather_.Width / 2, psnFather_.Top, psnFather_.Left + psnFather_.Width / 2, psnFather_.Top - 8);
                e.Graphics.DrawLine(pen, psnFather_.Left + psnFather_.Width / 2, psnFather_.Top - 8, m_marFatherParents.Left + m_marFatherParents.Width / 2, psnFather_.Top - 8);
                e.Graphics.DrawLine(pen, m_marFatherParents.Left + m_marFatherParents.Width / 2, psnFather_.Top - 8, m_marFatherParents.Left + m_marFatherParents.Width / 2, m_marFatherParents.Top + m_marFatherParents.Height);
            }

            // Draw a line from the mother to her parents
            if (psnMother_.Visible)
            {
                e.Graphics.DrawLine(pen, psnMother_.Left + psnMother_.Width / 2, psnMother_.Top, psnMother_.Left + psnMother_.Width / 2, psnMother_.Top - 8);
                e.Graphics.DrawLine(pen, psnMother_.Left + psnMother_.Width / 2, psnMother_.Top - 8, m_marMotherParents.Left + m_marMotherParents.Width / 2, psnMother_.Top - 8);
                e.Graphics.DrawLine(pen, m_marMotherParents.Left + m_marMotherParents.Width / 2, psnMother_.Top - 8, m_marMotherParents.Left + m_marMotherParents.Width / 2, m_marMotherParents.Top + m_marMotherParents.Height);
            }

            // Draw lines up for the children
            if (psnChildren_ != null)
            {
                int nBarHeight = psnChildren_[0].Top - (padding_.y / 2);
                int nNumMarriages = 0;
                if (partnersConntections_ != null)
                {
                    nNumMarriages = partnersConntections_.Length;
                }

                for (int nType = -1; nType < nNumMarriages; nType++)
                {
                    bool bDrawBar = false;
                    if (nType == -1)
                    {
                        pos = labPerson_.Left + labPerson_.Width / 2;
                    }
                    else
                    {
                        pos = partnersConntections_[nType].Left + partnersConntections_[nType].Width / 2;
                    }
                    int nMin = pos;
                    int nMax = pos;

                    for (int i = 0; i < psnChildren_.Length; i++)
                    {

                        // Todo: check this it crashes.
                        if (psnChildren_[i] != null)
                        {
                            if ((int)psnChildren_[i].Tag == nType)
                            {
                                bDrawBar = true;
                                pos = psnChildren_[i].Left + psnChildren_[i].Width / 2;
                                if (pos < nMin)
                                {
                                    nMin = pos;
                                }
                                if (pos > nMax)
                                {
                                    nMax = pos;
                                }
                                e.Graphics.DrawLine(pen, pos, psnChildren_[0].Top, pos, nBarHeight);
                            }
                        }
                    }

                    if (bDrawBar)
                    {
                        // Draw a bar to hang the Siblings on
                        e.Graphics.DrawLine(pen, nMin, nBarHeight, nMax, nBarHeight);

                        int nHeight;
                        if (nType == -1)
                        {
                            pos = labPerson_.Left + labPerson_.Width / 2;
                            nHeight = labPerson_.Top + labPerson_.Height;
                        }
                        else
                        {
                            pos = partnersConntections_[nType].Left + partnersConntections_[nType].Width / 2;
                            nHeight = partnersConntections_[nType].Top + partnersConntections_[nType].Height;
                        }
                        e.Graphics.DrawLine(pen, pos, nBarHeight, pos, nHeight);

                        nBarHeight += 5;
                    }
                }

                // Draw a line down from the children who have descendants
                for (int nI = 0; nI < psnChildren_.Length; nI++)
                {
                    Person oChild = database_.getPerson(psnChildren_[nI].getPersonIndex());
                    if (oChild.hasChildren())
                    {
                        e.Graphics.DrawLine(pen, psnChildren_[nI].Left + psnChildren_[nI].Width / 2, psnChildren_[nI].Top + psnChildren_[nI].Height, psnChildren_[nI].Left + psnChildren_[nI].Width / 2, psnChildren_[nI].Top + psnChildren_[nI].Height + 5);
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
        private void labPerson_Paint(object sender, PaintEventArgs e)
        {
            Pen oPen = new Pen(Color.Black, 1);
            e.Graphics.DrawLine(oPen, 0, labPerson_.Height, 0, 0);
            e.Graphics.DrawLine(oPen, 0, 0, labPerson_.Width - 1, 0);
            e.Graphics.DrawLine(oPen, labPerson_.Width - 1, 0, labPerson_.Width - 1, labPerson_.Height);
        }

        /// <summary>Message handler for the main person details paint event.
        /// Draw a border around three sides of the control
        /// This is here because it is really to do with form painting.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void labPersonDates_Paint(object sender, PaintEventArgs e)
        {
            Pen oPen = new Pen(Color.Black, 1);
            e.Graphics.DrawLine(oPen, 0, 0, 0, labPersonDates_.Height - 1);
            e.Graphics.DrawLine(oPen, 0, labPersonDates_.Height - 1, labPersonDates_.Width - 1, labPersonDates_.Height - 1);
            e.Graphics.DrawLine(oPen, labPersonDates_.Width - 1, labPersonDates_.Height - 1, labPersonDates_.Width - 1, 0);
        }



        /// <summary>Message handler for the main form loading.</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_Load(object sender, System.EventArgs e)
        {
            // Size the main window
            walton.XmlNode oMain = config_.getNode("windows/main");
            WindowState = (FormWindowState)oMain.getAttributeValue("state", (int)FormWindowState.Normal, true);
            Width = oMain.getAttributeValue("width", 800, true);
            Height = oMain.getAttributeValue("height", 600, true);
        }



        /// <summary>Message handler for the shown event.  This is like the post appear load event.</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_Shown(object sender, EventArgs e)
        {
            // Open the tree document if specified.
            if (treeToOpen_ != "")
            {
                // MessageBox.Show(this,m_sTreeToOpen,"TreeToOpen");
                OpenTree(treeToOpen_);
            }
        }



        /// <summary>Message handler for the main window closing.  Save the size of the window.</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (WindowState != FormWindowState.Minimized)
            {
                walton.XmlNode oMain = config_.getNode("windows/main");
                oMain.setAttributeValue("state", (int)WindowState);
                if (WindowState == FormWindowState.Normal)
                {
                    oMain.setAttributeValue("width", Width);
                    oMain.setAttributeValue("height", Height);
                }
                config_.save();
            }
        }



        #endregion

        #region Menu System

        #region File Menu



        /// <summary>Message handler for the File -> Open menu point click.  Open a family tree database.</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuOpen_Click(object sender, EventArgs e)
        {
            OpenDatabase();
        }



        /// <summary>Message handler for the File → Home menu point click and the "Home" toolbar button.</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuHome_Click(object sender, EventArgs e)
        {
            GoHome();
        }



        /// <summary>Message handler for the "Back" button click event.</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuBack_Click(object sender, EventArgs e)
        {
            HistoryBack();
        }



        /// <summary>Message handler for the forward button click event.</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuForward_Click(object sender, EventArgs e)
        {
            HistoryForward();
        }



        /// <summary>Message handler for the all the recent file menu points.  Opens the file specified on the recent file menu point.</summary>
        /// <param name="oSender"></param>
        /// <param name="e"></param>
        private void menuRecentFile_Click(object oSender, System.EventArgs e)
        {
            // Find the index of the recent file menu.
            ToolStripMenuItem oMenu = (ToolStripMenuItem)oSender;
            int nIndex = int.Parse(oMenu.Text.Substring(0, 1)) - 1;

            // Find the selected file.
            string sFilename = recentFiles_.getRecentFilename(nIndex);

            // Open this file.
            OpenDatabase(sFilename);
        }



        /// <summary>Message handler for the "File" → "Export Gedom" menu point.</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuExportGedcom_Click(object sender, EventArgs e)
        {
            ExportGedcom();
        }



        /// <summary>Message handler for the 'File' → 'Export SQL script' menu item.</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuExportSQLScript_Click(object sender, EventArgs e)
        {
            ExportSQL();
        }



        /// <summary>Message handler for the File → Open Tree menu point click.</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuOpenTree_Click(object sender, EventArgs e)
        {
            OpenTree();
        }



        /// <summary>Message handler for File → Exit menu point click.</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuExit_Click(object sender, EventArgs e)
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
        private void menuEdit_Click(object sender, EventArgs e)
        {
            Edit();
        }

        /// <summary>
        /// Message handler for the Edit -> Edit Sources... menu point click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuEditSources_Click(object sender, EventArgs e)
        {
            EditSources();
        }

        /// <summary>
        /// Message handler for the Edit -> Add Father menu point click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuAddFather_Click(object sender, EventArgs e)
        {
            AddPerson(RelatedPerson.FATHER);
        }

        /// <summary>
        /// Message handler for the Edit -> Add Mother menu point click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuAddMother_Click(object sender, EventArgs e)
        {
            AddPerson(RelatedPerson.MOTHER);
        }

        /// <summary>
        /// Message handler for the Edit -> Add Sibling menu point click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuAddSibling_Click(object sender, EventArgs e)
        {
            AddPerson(RelatedPerson.SIBLING);
        }

        /// <summary>
        /// Message handler for the Edit -> Add Child menu point click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuAddChild_Click(object sender, EventArgs e)
        {
            AddPerson(RelatedPerson.CHILD);
        }

        /// <summary>
        /// Message handler for the Edit -> Add Partner menu point click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuAddPartner_Click(object sender, EventArgs e)
        {
            AddPerson(RelatedPerson.PARTNER);
        }

        /// <summary>
        /// Message handler for the Edit -> Edit Census Records... menu point click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuCensus_Click(object sender, EventArgs e)
        {
            EditCensus();
        }

        /// <summary>
        /// Message handler for the Edit -> Remove Unlinked Places menu point click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuUnlinkedPlaces_Click(object sender, EventArgs e)
        {
            int nCount = database_.placeRemoveUnlinked();
            MessageBox.Show(this, nCount.ToString() + " unlinked place(s) were removed.", "Unlinked Places", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Message handler for the Edit -> User Options... menu point click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuOptions_Click(object sender, EventArgs e)
        {
            UserOptions();
        }

        /// <summary>Message handler for the Edit -> Add Media menu point click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuAddMedia_Click(object sender, EventArgs e)
        {
            frmEditMedia oDialog = new frmEditMedia(database_);
            if (oDialog.ShowDialog(this) == DialogResult.OK)
            {
                int nMediaID = oDialog.MediaID;
                ShowMedia(nMediaID, true);
            }
        }

        #endregion

        #region View Menu



        /// <summary>Message handler for the "View" → "Goto Person" menu point click.  Also the Goto person toolbar button click event.</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuGoto_Click(object sender, EventArgs e)
        {
            GotoPerson();
        }



        /// <summary>Message handler for the "View" → "Image" menu point click and the images toolbar button.</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuImage_Click(object sender, EventArgs e)
        {
            m_menuImage.Checked = !m_menuImage.Checked;
            m_tsbImage.Checked = m_menuImage.Checked;
            if (Current.content == Pages.PERSON)
            {
                Person oPerson = new Person(Current.index, database_);
                m_oWebBrowser.DocumentText = userOptions_.renderHtml(oPerson.Description(true, true, m_menuImage.Checked, m_menuLocation.Checked, true));
            }
        }



        /// <summary>Message handler for the "View" → "Location" menu point click and the location toolbar button.</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuLocation_Click(object sender, EventArgs e)
        {
            m_menuLocation.Checked = !m_menuLocation.Checked;
            m_tsbLocation.Checked = m_menuLocation.Checked;

            switch (Current.content)
            {
            case Pages.PERSON:
                Person oPerson = new Person(Current.index, database_);
                m_oWebBrowser.DocumentText = userOptions_.renderHtml(oPerson.Description(true, true, m_menuImage.Checked, m_menuLocation.Checked, true));
                break;

            case Pages.PLACE:
                ShowPlace(Current.index, false);
                break;
            }
        }



        /// <summary>Message handler for the "View" → "Recent Changes" menu point click.</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuRecentChanges_Click(object sender, EventArgs e)
        {
            ShowRecentChanges();
        }



        /// <summary>Message handler for the View → ToDo menu point click.  Display the To Do on the main window.</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuToDo_Click(object sender, EventArgs e)
        {
            ShowToDo();
        }



        /// <summary>Message handler for the View → Calculate age menu point click and the Age toolbar button click.</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuCalcAge_Click(object sender, EventArgs e)
        {
            ShowAge();
        }



        /// <summary>Message handler for the View → Calculate Birthday menu point click.</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuBirthday_Click(object sender, EventArgs e)
        {
            // Display the birthday dialog
            frmBirthday oDialog = new frmBirthday();
            oDialog.ShowDialog(this);
        }



        /// <summary>Message handler for the View → Reduce Width menu point click and the reduce width toolbar button.</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuReduceWidth_Click(object sender, EventArgs e)
        {
            if (Current.content == Pages.PERSON)
            {
                personSize_.x -= 8;
                fontSize_ = 7.0f;
                ShowPerson(Current.index, false);
            }
        }



        /// <summary>Message handler for the View → Standard Width menu point click.</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuStandardWidth_Click(object sender, EventArgs e)
        {
            if (Current.content == Pages.PERSON)
            {
                personSize_.x = 130;
                fontSize_ = userOptions_.fontBase.size;
                ShowPerson(Current.index, false);
            }
        }



        /// <summary>Message handler for the "View" → "html Source" menu point.</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuHtmlSource_Click(object sender, EventArgs e)
        {
            // Create a file to hold the html source

            string sFilename = walton.DataPaths.getMyDocuments() + "\\family tree source.html";

            // Open the filename for output.
            StreamWriter oFile = new StreamWriter(sFilename, false);
            oFile.Write(m_oWebBrowser.DocumentText);
            oFile.Close();

            // Open the new file in notepad.
            try
            {
                System.Diagnostics.Process.Start("notepad.exe", "\"" + sFilename + "\"");
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
        private void menuToTree_Click(object sender, EventArgs e)
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
            ShowPerson(psnPerson.getPersonIndex(), true);
        }

        /// <summary>Message handler for the web browser trying to follow a link.</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WebBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            string sNewUrl = e.Url.ToString();
            int nColon = sNewUrl.IndexOf(':');
            if (nColon > 0)
            {
                string sType = sNewUrl.Substring(0, nColon).ToLower();
                switch (sType)
                {
                case "person":
                    e.Cancel = true;
                    int nPersonID = int.Parse(sNewUrl.Substring(nColon + 1));
                    ShowPerson(nPersonID, true);
                    break;
                case "source":
                    e.Cancel = true;
                    int nSourceID = int.Parse(sNewUrl.Substring(nColon + 1));
                    ShowSource(nSourceID, true);
                    break;
                case "place":
                    e.Cancel = true;
                    int nPlaceID = int.Parse(sNewUrl.Substring(nColon + 1));
                    ShowPlace(nPlaceID, true);
                    break;
                case "media":
                    e.Cancel = true;
                    int nMediaID = int.Parse(sNewUrl.Substring(nColon + 1));
                    ShowMedia(nMediaID, true);
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
            for (int nI = historyIndex_ - 1; nI >= 0; nI--)
            {
                if (history_[nI].label == sLabel)
                {
                    nIndex = nI;
                    nI = -1;
                }
            }

            // Search in the forward section if nothing found so far.
            if (nIndex < 0)
            {
                for (int nI = historyIndex_; nI <= historyLast_; nI++)
                {
                    if (history_[nI].label == sLabel)
                    {
                        nIndex = nI;
                        nI = historyLast_ + 1;
                    }
                }
            }

            // Show the specified page
            if (nIndex >= 0)
            {
                historyIndex_ = nIndex;
                ShowCurrentPage();
            }
        }

        #endregion

        #endregion

    }
}
