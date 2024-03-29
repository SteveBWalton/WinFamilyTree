﻿// Family tree objects
using family_tree.objects;
using System;
using System.Collections;
using System.Drawing;
// Disk Input Output
using System.IO;
// ParameterizedThreadStart
using System.Threading;
using System.Windows.Forms;

namespace family_tree.viewer
{
    #region Delegate Functions

    /// <summary>Delegate for functions that specify a single bool parameter.</summary>
    /// <param name="isTrue">Specify the boolean value.</param>
    public delegate void FuncSetBool(bool isTrue);

    /// <summary>Delegate for functions that specifiy a single string and bool parameter.</summary>
    /// <param name="textValue">Specify the string value.</param>
    /// <param name="isTrue">Specify the boolean value.</param>
    public delegate void FuncTextBool(string textValue, bool isTrue);

    /// <summary>Delegate for functions that specify a single integer parameter.</summary>
    /// <param name="integerValue">Specify the value of the integer parameter.</param>
    public delegate void FuncSetInt(int integerValue);

    #endregion

    /// <summary>This is the main window of the application.  This displays the current person and allows the user to move from person to person and select actions.</summary>
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
            public int idx;
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

        /// <summary>The page history.</summary>
        private Page[] history_;

        /// <summary>Position in the history array</summary>
        private int historyIdx_;

        /// <summary>Last valid position in the history array.</summary>
        private int historyLast_;

        /// <summary>Configuration file.</summary>
        private walton.XmlDocument config_;

        /// <summary>Current family tree database.</summary>
        private Database database_;

        /// <summary>User options.</summary>
        private UserOptions userOptions_;

        /// <summary>Array of person graphical controls to display siblings of the main person.</summary>
        private family_tree.viewer.PersonDisplay[] psnSiblings_;

        /// <summary>Array of person graphical controls to display children of the main person.</summary>
        private family_tree.viewer.PersonDisplay[] psnChildren_;

        /// <summary>Array of person graphical controls to display partners of the main person.</summary>
        private family_tree.viewer.PersonDisplay[] psnPartners_;

        /// <summary>Array of relationship graphical controls to display connections to partners of the main person.</summary>
        private family_tree.viewer.RelationshipDisplay[] partnersConntections_;

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
            // Required for Windows Form Designer support.
            InitializeComponent();

            // Add any constructor code after InitializeComponent call.
            psnFather_.eventClick += new FuncClick(ucPersonEvtClick);
            psnMother_.eventClick += new FuncClick(ucPersonEvtClick);
            psnFatherFather_.eventClick += new FuncClick(ucPersonEvtClick);
            psnFatherMother_.eventClick += new FuncClick(ucPersonEvtClick);
            psnMotherFather_.eventClick += new FuncClick(ucPersonEvtClick);
            psnMotherMother_.eventClick += new FuncClick(ucPersonEvtClick);

            // Open the configuration file.
            string configFile = walton.DataPaths.getUserDirectory("Walton", "Family Tree Viewer", "1.0");
            configFile += "\\config.xml";
            config_ = new walton.XmlDocument(configFile);

            // Open the recent files list.
            recentFiles_ = new walton.FileList(4, config_);
            updateRecentFiles();

            // Initialise objects / variables.
            database_ = null;
            userOptions_ = new UserOptions(config_);
            history_ = new Page[5];
            historyIdx_ = -1;

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

            string documentFileName = "";
            treeToOpen_ = "";

            // Open the most recent database.
            if (recentFiles_.getRecentFilename(0) != "")
            {
                documentFileName = recentFiles_.getRecentFilename(0);
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
            if (documentFileName != "")
            {
                openDatabase(documentFileName);
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
        static void Main(string[] args)
        {
            // The following 2 lines enable XP styles in the applicaiton
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new MainWindow(args));
        }



        #endregion

        #region Supporting Functions



        /// <summary>Convert an object to an int without raising an error.</summary>
        /// <param name="value">Specifies the object to convert to an int.</param>
        /// <returns>An int which represents the specified object.</returns>
        int toInt(object value)
        {
            int result = 0;
            try
            {
                result = (int)value;
            }
            catch { }
            return result;
        }



        #endregion

        #region Thread Safe UI Functions



        /// <summary>Thread safe, set the cursor back to the default cursor.</summary>
        private void cursorDefault()
        {
            // Check that we are on the UI thread.
            if (InvokeRequired)
            {
                Invoke(new FuncVoid(cursorDefault));
                return;
            }

            // Now we are on the UI thread.
            // Important that the application wait cursor is reset before the form cursor and Tee chart cursor            
            Application.UseWaitCursor = false;
            Cursor.Current = Cursors.Default;
        }



        /// <summary>Thread safe, set the cursor to the wait cursor.</summary>
        private void cursorWait()
        {
            // Check that we are on the UI thread.
            if (InvokeRequired)
            {
                Invoke(new FuncVoid(cursorWait));
                return;
            }

            // Now we are on the UI thread.
            Application.UseWaitCursor = true;
            Cursor.Current = Cursors.WaitCursor;
        }



        /// <summary>Thread safe writes a message to the status bar.</summary>
        /// <param name="text">Specifies the text for the status bar.</param>
        /// <param name="isError">Specifies true for an error message, false otherwise.</param>
        public void setStatusBarText(string text, bool isError)
        {
            if (InvokeRequired)
            {
                Invoke(new FuncTextBool(setStatusBarText), new object[2] { text, isError });
            }

            // Now we are on the UI thread.
            tslabStatus_.Text = text;
        }



        /// <summary>Thread safe advance the progress bar one step.</summary>
        private void progressBarPerformStep()
        {
            // Check that we are no the UI thread.
            if (InvokeRequired)
            {
                Invoke(new FuncVoid(progressBarPerformStep));
                return;
            }

            // Now we are on the UI thread.
            numSteps_++;
            tsProgressBar_.PerformStep();
        }



        /// <summary>Thread safe, show / hide the progress bar on the status bar.</summary>
        /// <param name="isVisible">Specifies true to show the progress bar, false to hide the progress bar.</param>
        private void progressBarVisible(bool isVisible)
        {
            // Check that we are on the UI thread.
            if (InvokeRequired)
            {
                Invoke(new FuncSetBool(progressBarVisible), new object[1] { isVisible });
                return;
            }

            // Now we are on the UI thread.
            tsProgressBar_.Visible = isVisible;
        }



        /// <summary>Thread safe initialise the the progress bar.  This does not make the progress bar visible.</summary>
        /// <param name="maximum">Specify the maximum value for the progress bar.  This is the number of steps.</param>
        private void progressBarInitialise(int maximum)
        {
            // Check that we are on the UI thread.
            if (InvokeRequired)
            {
                Invoke(new FuncSetInt(progressBarInitialise), new object[1] { maximum });
                return;
            }

            // Now we are on the UI thread.
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
                        Person relationPerson = database_.getPerson(marriages[i].partnerIdx);

                        // Create a person control to show the partner
                        psnPartners_[i] = new family_tree.viewer.PersonDisplay();
                        psnPartners_[i].Location = new System.Drawing.Point(pos, labPerson_.Top);
                        psnPartners_[i].Size = new System.Drawing.Size(personSize_.x, personSize_.y);
                        psnPartners_[i].setPerson(relationPerson);
                        psnPartners_[i].eventClick += new FuncClick(ucPersonEvtClick);
                        psnPartners_[i].BackColor = backgroundBoy_;
                        psnPartners_[i].Font = font;
                        psnPartners_[i].setPerson(relationPerson);
                        panelTree_.Controls.Add(psnPartners_[i]);
                        pos += personSize_.x;

                        // Create a relationship control to show the relationship to the partner
                        partnersConntections_[i] = new family_tree.viewer.RelationshipDisplay();
                        partnersConntections_[i].Location = new System.Drawing.Point(pos, labPerson_.Top + 8);
                        partnersConntections_[i].Size = new System.Drawing.Size(marriedWidth_, 16);
                        partnersConntections_[i].setRelationship(marriages[i]);
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
                        Person relationPerson = database_.getPerson(marriages[i].partnerIdx);

                        // Create a relationship control to show the relationship to the partner.
                        partnersConntections_[i] = new family_tree.viewer.RelationshipDisplay();
                        partnersConntections_[i].Location = new System.Drawing.Point(pos, labPerson_.Top + 8);
                        partnersConntections_[i].Size = new System.Drawing.Size(marriedWidth_, 16);
                        partnersConntections_[i].setRelationship(marriages[i]);
                        panelTree_.Controls.Add(partnersConntections_[i]);
                        pos += marriedWidth_;

                        // Create a person control to show the partner.
                        psnPartners_[i] = new family_tree.viewer.PersonDisplay();
                        psnPartners_[i].Location = new System.Drawing.Point(pos, labPerson_.Top);
                        psnPartners_[i].Size = new System.Drawing.Size(personSize_.x, personSize_.y);
                        psnPartners_[i].setPerson(relationPerson);
                        psnPartners_[i].eventClick += new FuncClick(ucPersonEvtClick);
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
        /// <param name="mediaIdx">Specifies the ID of the media object.</param>
        /// <param name="isAddHistory">Specifies true to add the page to history.</param>
        /// <returns>True for success, false otherwise.</returns>
        private bool showMedia(int mediaIdx, bool isAddHistory)
        {
            // Hide the person tree panel.
            panelTree_.Visible = false;

            // Find the place.
            Media media = new Media(database_, mediaIdx);
            Text = media.fileName + " - Family Tree Viewer";
            if (isAddHistory)
            {
                historyAdd(Pages.MEDIA, mediaIdx, "Media: " + media.fileName);
            }

            // Display the Html description of the source.
            webBrowser_.DocumentText = userOptions_.renderHtml(media.toHtml());

            // Return success.
            return true;
        }



        /// <summary>Show the specifed place on the main window.</summary>
        /// <param name="placeIdx">Specifies the ID of the place to show.</param>
        /// <param name="isAddHistory">Specifies true to add the page to history.</param>
        /// <returns>True for success, false otherwise.</returns>
        private bool showPlace(int placeIdx, bool isAddHistory)
        {
            // Hide the person tree panel.
            panelTree_.Visible = false;

            // Find the place.
            Place place = new Place(placeIdx, database_);
            Text = place.name + " - Family Tree Viewer";
            if (isAddHistory)
            {
                historyAdd(Pages.PLACE, placeIdx, "Place: " + place.name);
            }

            // Display the Html description of the source.
            string placeHtml = place.toHtml(m_menuLocation.Checked);
            webBrowser_.DocumentText = userOptions_.renderHtml(placeHtml);

            // Return success.
            return true;
        }



        /// <summary>Show the specified source on the main window.</summary>
        /// <param name="sourceIdx">Specifies the ID of the source to show.</param>
        /// <param name="isAddHistory">Specifies true to add the page to history.</param>
        /// <returns>True for success, false otherwise.</returns>
        private bool showSource(int sourceIdx, bool isAddHistory)
        {
            // Hide the person tree panel.
            panelTree_.Visible = false;

            // Find the source.
            Source source = new Source(database_, sourceIdx);
            Text = source.description + " - Family Tree Viewer";
            if (isAddHistory)
            {
                historyAdd(Pages.SOURCE, sourceIdx, "Source: " + source.description);
            }

            // Display the Html description of the source.
            webBrowser_.DocumentText = userOptions_.renderHtml(source.toHtml());

            // Return success.
            return true;
        }



        /// <summary>Show the specified person.</summary>
        /// <param name="personIdx">Specifies the ID of the person to show.</param>
        /// <param name="isAddHistory">Specifies to add this page to the Back / Forward list.</param>
        /// <returns>True for sucess, false for failure.</returns>
        private bool showPerson(int personIdx, bool isAddHistory)
        {
            const int TOP = 2;

            // Show the person tree panel.
            panelTree_.Visible = true;

            // Show the specified person.
            Person person = database_.getPerson(personIdx);
            Text = person.getName(true, false) + " - Family Tree Viewer";

            // Build the rich text file description of the person
            webBrowser_.DocumentText = userOptions_.renderHtml(person.getDescription(true, true, m_menuImage.Checked, m_menuLocation.Checked, true));

            // Update the back button.
            if (isAddHistory)
            {
                historyAdd(Pages.PERSON, personIdx, person.getName(true, false));
            }
            Font font = userOptions_.fontBase.getFont();

            // Show the father.
            if (person.fatherIdx == 0)
            {
                psnFather_.Visible = false;
                psnFatherFather_.Visible = false;
                psnFatherMother_.Visible = false;
                marFatherParents_.Visible = false;
            }
            else
            {
                Person father = database_.getPerson(person.fatherIdx);
                psnFather_.Width = personSize_.x;
                psnFather_.Height = personSize_.y;
                psnFather_.setPerson(father);
                psnFather_.Top = personSize_.y + padding_.y + TOP;
                psnFather_.Font = font;
                psnFather_.Visible = true;

                // Show the father's father.
                if (father.fatherIdx == 0)
                {
                    psnFatherFather_.Visible = false;
                }
                else
                {
                    Person grandFather = database_.getPerson(father.fatherIdx);
                    psnFatherFather_.Width = personSize_.x;
                    psnFatherFather_.Height = personSize_.y;
                    psnFatherFather_.setPerson(grandFather);
                    psnFatherFather_.Top = TOP;
                    psnFatherFather_.Font = font;
                    psnFatherFather_.Visible = true;
                }

                // Show the father's mother.
                if (father.motherIdx == 0)
                {
                    psnFatherMother_.Visible = false;
                }
                else
                {
                    Person grandMother = database_.getPerson(father.motherIdx);
                    psnFatherMother_.Width = personSize_.x;
                    psnFatherMother_.Height = personSize_.y;
                    psnFatherMother_.setPerson(grandMother);
                    psnFatherMother_.Top = TOP;
                    psnFatherMother_.Font = font;
                    psnFatherMother_.Visible = true;
                }

                // Show the relationship between these two.
                if (father.fatherIdx == 0 || father.motherIdx == 0)
                {
                    marFatherParents_.Visible = false;
                }
                else
                {
                    marFatherParents_.Visible = true;
                    marFatherParents_.Width = marriedWidth_;
                    marFatherParents_.Top = TOP;
                }
            }

            // Show the mother.
            if (person.motherIdx == 0)
            {
                psnMother_.Visible = false;
                psnMotherFather_.Visible = false;
                psnMotherMother_.Visible = false;
                marMotherParents_.Visible = false;
            }
            else
            {
                Person mother = database_.getPerson(person.motherIdx);
                psnMother_.Width = personSize_.x;
                psnMother_.Height = personSize_.y;
                psnMother_.setPerson(mother);
                psnMother_.Top = personSize_.y + padding_.y + TOP;
                psnMother_.Font = font;
                psnMother_.Visible = true;

                // Show the mother's father.
                if (mother.fatherIdx == 0)
                {
                    psnMotherFather_.Visible = false;
                }
                else
                {
                    Person grandFather = database_.getPerson(mother.fatherIdx);
                    psnMotherFather_.Width = personSize_.x;
                    psnMotherFather_.Height = personSize_.y;
                    psnMotherFather_.setPerson(grandFather);
                    psnMotherFather_.Top = TOP;
                    psnMotherFather_.Font = font;
                    psnMotherFather_.Visible = true;
                }

                // Show the mother's mother.
                if (mother.motherIdx == 0)
                {
                    psnMotherMother_.Visible = false;
                }
                else
                {
                    Person grandMother = database_.getPerson(mother.motherIdx);
                    psnMotherMother_.Width = personSize_.x;
                    psnMotherMother_.Height = personSize_.y;
                    psnMotherMother_.setPerson(grandMother);
                    psnMotherMother_.Top = TOP;
                    psnMotherMother_.Font = font;
                    psnMotherMother_.Visible = true;
                }

                // Show the relationship between these two.
                if (mother.fatherIdx == 0 || mother.motherIdx == 0)
                {
                    marMotherParents_.Visible = false;
                }
                else
                {
                    marMotherParents_.Visible = true;
                    marMotherParents_.Width = marriedWidth_;
                    marMotherParents_.Top = TOP;
                }
            }

            // Show the actual person.
            labPerson_.Font = userOptions_.fontBaseTitle.getFont();
            labPerson_.Top = 2 * (personSize_.y + padding_.y) + TOP;
            labPerson_.Text = person.getName(false, false);
            labPerson_.Width = 2 * personSize_.x + marriedWidth_;
            labPersonDates_.Top = labPerson_.Top + 23;
            labPersonDates_.Text = person.shortDescription(true);
            labPersonDates_.Width = labPerson_.Width;
            labPersonDates_.Height = personSize_.y - 23;
            if (person.isMale)
            {
                labPerson_.BackColor = backgroundBoy_;
            }
            else
            {
                labPerson_.BackColor = backgroundGirl_;
            }
            labPersonDates_.BackColor = labPerson_.BackColor;

            cleanupDynamicControls();

            Relationship[] marriages = person.getRelationships();
            if (marriages.Length > 0)
            {
                psnPartners_ = new family_tree.viewer.PersonDisplay[marriages.Length];
                partnersConntections_ = new family_tree.viewer.RelationshipDisplay[marriages.Length];
            }

            // Show the siblings.
            int[] siblings = person.getSiblings();
            int pos = 3;
            bool isShownPerson = false;
            if (siblings.Length > 0)
            {
                psnSiblings_ = new family_tree.viewer.PersonDisplay[siblings.Length];

                for (int i = 0; i < siblings.Length; i++)
                {
                    Person relation = database_.getPerson(siblings[i]);

                    // Show the person if he is older than the current sibling (and not already shown).
                    if (person.dob.date < relation.dob.date && !isShownPerson)
                    {
                        drawMainPerson(ref person, ref marriages, ref pos, font);
                        isShownPerson = true;
                    }

                    // Show the sibling.
                    psnSiblings_[i] = new family_tree.viewer.PersonDisplay();
                    if (relation.isMale)
                    {
                        psnSiblings_[i].BackColor = backgroundBoy_;
                    }
                    else
                    {
                        psnSiblings_[i].BackColor = backgroundGirl_;
                    }
                    psnSiblings_[i].Font = font;
                    psnSiblings_[i].Location = new System.Drawing.Point(pos, labPerson_.Top);
                    psnSiblings_[i].Size = new System.Drawing.Size(personSize_.x, personSize_.y);
                    psnSiblings_[i].setPerson(relation);
                    psnSiblings_[i].eventClick += new FuncClick(ucPersonEvtClick);

                    // Build a tag value that represents which parents this sibling shares.
                    int tag = 0;
                    if (relation.fatherIdx == person.fatherIdx)
                    {
                        tag |= 1;
                    }
                    if (relation.motherIdx == person.motherIdx)
                    {
                        tag |= 2;
                    }
                    psnSiblings_[i].Tag = tag;

                    // this.Controls.Add(m_psnSiblings[nI]);
                    panelTree_.Controls.Add(psnSiblings_[i]);
                    pos += personSize_.x + padding_.x;
                }
            }
            if (!isShownPerson)
            {
                drawMainPerson(ref person, ref marriages, ref pos, font);
            }

            // Reposition the parents (X direction).
            psnFather_.Left = labPerson_.Left;
            marParents_.Left = psnFather_.Left + personSize_.x;
            marParents_.Top = psnFather_.Top;
            marParents_.Width = marriedWidth_;
            psnMother_.Left = labPerson_.Left + personSize_.x + marriedWidth_;

            // Reposition the grandparents (X direction).
            psnFatherMother_.Left = labPerson_.Left;
            psnFatherFather_.Left = psnFatherMother_.Left - marriedWidth_ - personSize_.x;
            psnMotherFather_.Left = labPerson_.Left + personSize_.x + marriedWidth_;
            psnMotherMother_.Left = psnMotherFather_.Left + psnMotherFather_.Width + marParents_.Width;
            if (psnFatherFather_.Left < 3)
            {
                int offset = 3 - psnFatherFather_.Left;
                psnFatherFather_.Left += offset;
                psnFatherMother_.Left += offset;
                psnMotherFather_.Left += offset;
                psnMotherMother_.Left += offset;
            }
            marFatherParents_.Left = psnFatherFather_.Left + personSize_.x;
            marFatherParents_.Top = psnFatherFather_.Top + 8;
            marMotherParents_.Left = psnMotherFather_.Left + personSize_.x;
            marMotherParents_.Top = psnMotherFather_.Top + 8;

            // Show the children.
            int[] children = person.getChildren();
            int height = labPerson_.Top + personSize_.y + padding_.y;
            if (person.isMale)
            {
                pos = labPerson_.Left;
            }
            else
            {
                if (partnersConntections_ != null)
                {
                    pos = psnPartners_[partnersConntections_.Length - 1].Left;
                }
                else
                {
                    pos = labPerson_.Left;
                }
            }

            if (children.Length > 0)
            {
                psnChildren_ = new family_tree.viewer.PersonDisplay[children.Length];

                for (int i = 0; i < children.Length; i++)
                {
                    Person relation = database_.getPerson(children[i]);

                    psnChildren_[i] = new family_tree.viewer.PersonDisplay();
                    if (relation.isMale)
                    {
                        psnChildren_[i].BackColor = backgroundBoy_;
                    }
                    else
                    {
                        psnChildren_[i].BackColor = backgroundGirl_;
                    }
                    psnChildren_[i].Font = font;
                    psnChildren_[i].Location = new System.Drawing.Point(pos, height);
                    psnChildren_[i].Size = new System.Drawing.Size(personSize_.x, personSize_.y);
                    psnChildren_[i].setPerson(relation);
                    psnChildren_[i].eventClick += new FuncClick(ucPersonEvtClick);

                    // Decide which relationship this child belongs to.
                    int tag = -1;
                    if (partnersConntections_ != null)
                    {
                        for (int j = 0; j < partnersConntections_.Length; j++)
                        {
                            if (partnersConntections_[j].motherIdx == relation.motherIdx && partnersConntections_[j].fatherIdx == relation.fatherIdx)
                            {
                                tag = j;
                            }
                        }
                    }
                    psnChildren_[i].Tag = tag;

                    // this.Controls.Add(m_psnChildren[nI]);
                    panelTree_.Controls.Add(psnChildren_[i]);
                    pos += personSize_.x + padding_.x;
                }
            }

            // Reposition the children if off the right edge.
            if (children.Length > 0)
            {
                if (psnChildren_[children.Length - 1].Left + personSize_.x > Width)
                {
                    int nOffset = psnChildren_[children.Length - 1].Left + personSize_.x - Width;
                    for (int i = 0; i < children.Length; i++)
                    {
                        psnChildren_[i].Left -= nOffset;
                    }
                }
            }

            // This causes the parent / child lines to be redrawn.
            Refresh();

            // Return success.
            return true;
        }



        #endregion

        #region Actions

        #region Back / Forward / History



        /// <summary>Returns the current page, according to the history.</summary>
        private Page currentPage
        {
            get
            {
                if (historyIdx_ >= 0)
                {
                    return history_[historyIdx_];
                }
                return history_[0];
            }
        }



        /// <summary>Add the current page to the history.</summary>
        /// <param name="pageType">Specifies the type of page.</param>
        /// <param name="pageIdx">Specifies the ID of the page.</param>
        /// <param name="pageLabel">Specifies a human readable label for the page.</param>
        private void historyAdd(Pages pageType, int pageIdx, string pageLabel)
        {
            // Check that some thing has changed.
            if (currentPage.content == pageType && currentPage.idx == pageIdx)
            {
                return;
            }

            // Add to the history.
            historyIdx_++;
            if (historyIdx_ >= history_.Length)
            {
                // Move all the page down one.
                for (int i = 0; i < history_.Length - 1; i++)
                {
                    history_[i] = history_[i + 1];
                }

                // Move back to the end of the history.
                historyIdx_--;
            }

            // Set the current page.
            history_[historyIdx_].idx = pageIdx;
            history_[historyIdx_].content = pageType;
            history_[historyIdx_].label = pageLabel;
            historyLast_ = historyIdx_;

            // Update the back button.
            updateBackButton();
        }



        /// <summary>Move to the page that is back in the history.</summary>
        private void historyBack()
        {
            // Check that there is some history to move back into.
            if (historyIdx_ < 1)
            {
                return;
            }

            // Move back in the history.
            historyIdx_--;
            showCurrentPage();
        }



        /// <summary>Move to the page that is forward in the history (if available).  If no page is available then nothing happens.</summary>
        private void historyForward()
        {
            // Check that there is some history to move into.
            if (historyIdx_ == historyLast_)
            {
                return;
            }

            // Move forward in the history.
            historyIdx_++;
            showCurrentPage();
        }



        /// <summary>Shows the current page.  Does not add the page to the history.  Does update the back history drop down list.</summary>
        private void showCurrentPage()
        {
            // Show the current page
            switch (history_[historyIdx_].content)
            {
            case Pages.PERSON:
                showPerson(history_[historyIdx_].idx, false);
                break;
            case Pages.SOURCE:
                showSource(history_[historyIdx_].idx, false);
                break;
            case Pages.PLACE:
                showPlace(history_[historyIdx_].idx, false);
                break;
            }

            // Update the back button
            updateBackButton();
        }



        /// <summary>Updates the back (and forward) buttons and combo boxes based on the current history.</summary>
        private void updateBackButton()
        {
            // Update the back button
            if (historyIdx_ > 0)
            {
                tsbBack_.Enabled = true;
                tsddbBack_.Enabled = true;

                tsddbBack_.DropDownItems.Clear();
                for (int i = historyIdx_ - 1; i >= 0; i--)
                {
                    tsddbBack_.DropDownItems.Add(history_[i].label, null, individualBackItemClick);
                }
            }
            else
            {
                tsbBack_.Enabled = false;
                tsddbBack_.Enabled = false;
            }

            // Update the forward button
            if (historyIdx_ == historyLast_)
            {
                tsbForward_.Enabled = false;
                tsddbForward_.Enabled = false;
            }
            else
            {
                tsbForward_.Enabled = true;
                tsddbForward_.Enabled = true;
                tsddbForward_.DropDownItems.Clear();
                for (int i = historyIdx_ + 1; i <= historyLast_; i++)
                {
                    tsddbForward_.DropDownItems.Add(history_[i].label, null, individualBackItemClick);
                }
            }
        }



        #endregion



        /// <summary>Displays the home / default person.</summary>
        /// <returns>True, if successful.  False, otherwise.</returns>
        private bool goHome()
        {
            // Find the home person and display them.
            walton.XmlNode xmlHome = config_.getNode("useroptions/home");
            int homePersonIdx = xmlHome.getAttributeValue("id", 1, true);
            showPerson(homePersonIdx, true);

            // Return success.
            return true;
        }



        /// <summary>Display a dialog box and allow the user to select the person to show.</summary>
        /// <returns></returns>
        private bool gotoPerson()
        {
            // Allow the user to select a person.
            SelectPersonDialog selectPersonDialog = new SelectPersonDialog();
            int personIdx = selectPersonDialog.selectPersonIdx(this, database_);

            // If the user did select a person then show that person.
            if (personIdx >= 0)
            {
                showPerson(personIdx, true);
            }

            // Return success
            return true;
        }



        /// <summary>Exits the program.</summary>
        /// <returns>True, if successful.  False, otherwise.</returns>
        private bool absoluteEnd()
        {
            // End the program by closing the main window
            Close();

            // return success
            return true;
        }



        /// <summary>Displays a dialog that allows the user to edit the currently shown person / object.</summary>
        /// <returns>True for success, false otherwise.</returns>
        private bool edit()
        {
            switch (currentPage.content)
            {
            case Pages.PERSON:
                // Create a dialog to edit this person.
                EditPersonDialog editPersonDialog = new EditPersonDialog(currentPage.idx, database_);

                // Show the dialog and wait for the dialog to close.
                if (editPersonDialog.ShowDialog(this) == DialogResult.OK)
                {
                    // Refresh the display of the current person
                    showPerson(currentPage.idx, false);
                }
                editPersonDialog.Dispose();
                break;

            case Pages.PLACE:
                // Create a dialog to edit this place.
                EditPlaceDialog editPlaceDialog = new EditPlaceDialog(currentPage.idx, database_);

                // Show the dialog and wait for the dialog to close.
                if (editPlaceDialog.ShowDialog(this) == DialogResult.OK)
                {
                    // Refresh the display of this place.
                    showPlace(currentPage.idx, false);
                }
                editPlaceDialog.Dispose();
                break;

            case Pages.SOURCE:
                // Create a dialog to edit this source.
                EditSourcesDialog editSourceDialog = new EditSourcesDialog(database_, currentPage.idx);
                if (editSourceDialog.ShowDialog(this) == DialogResult.OK)
                {
                    // Refresh the display of this source.
                    showSource(currentPage.idx, false);
                }
                editSourceDialog.Dispose();
                break;

            case Pages.MEDIA:
                // Create a dialog to edit this media.
                EditMediaDialog editMediaDialog = new EditMediaDialog(database_, currentPage.idx);
                if (editMediaDialog.ShowDialog(this) == DialogResult.OK)
                {
                    // Refresh the display of this source.
                    showMedia(currentPage.idx, false);
                }
                editMediaDialog.Dispose();
                break;
            }

            // Return success.
            return true;
        }



        /// <summary>Adds a new person to database.</summary>
        /// <param name="relation">Specifies the relationship of the new person to the current person.</param>
        /// <returns>True, if a new person was created.  False, otherwise.</returns>
        private bool addPerson(RelatedPerson relation)
        {
            // Create a dialog to edit the new person.
            EditPersonDialog editPersonDialog = new EditPersonDialog(database_);

            // Show the dialog and wait for the dialig to close.
            if (editPersonDialog.ShowDialog(this) == DialogResult.OK)
            {
                // A new person was created.  Link to the current person in the required way.
                int newPersonIdx = editPersonDialog.getPersonIdx();

                Person person;
                switch (relation)
                {
                case RelatedPerson.FATHER:
                    person = new Person(currentPage.idx, database_);
                    person.fatherIdx = newPersonIdx;
                    person.save();
                    break;

                case RelatedPerson.MOTHER:
                    person = new Person(currentPage.idx, database_);
                    person.motherIdx = newPersonIdx;
                    person.save();
                    break;

                case RelatedPerson.SIBLING:
                    Person newPerson = new Person(newPersonIdx, database_);
                    person = new Person(currentPage.idx, database_);
                    newPerson.fatherIdx = person.fatherIdx;
                    newPerson.motherIdx = person.motherIdx;
                    newPerson.save();
                    break;

                case RelatedPerson.PARTNER:
                    person = new Person(currentPage.idx, database_);
                    Relationship relationship = new Relationship(person, newPersonIdx);
                    relationship.save();
                    person.addRelationship(relationship);
                    break;

                case RelatedPerson.CHILD:
                    newPerson = new Person(newPersonIdx, database_);
                    person = new Person(currentPage.idx, database_);
                    if (person.isMale)
                    {
                        newPerson.fatherIdx = currentPage.idx;
                    }
                    else
                    {
                        newPerson.motherIdx = currentPage.idx;
                    }
                    newPerson.save();
                    break;

                }

                // Refresh the display of the current person.
                showPerson(currentPage.idx, false);
            }
            editPersonDialog.Dispose();

            // Return success.
            return true;
        }



        /// <summary>Edit all the sources.  This displays a dialog that allows the user to edit the existing sources and add new sources.</summary>
        /// <returns>True for success, false otherwise.</returns>
        private bool showEditSourcesDialog()
        {
            // Create a dialog to edit the sources.
            EditSourcesDialog editSourcesDialog = new EditSourcesDialog(database_);

            // Show the dialog and wait for the dialog to close.
            editSourcesDialog.ShowDialog(this);
            editSourcesDialog.Dispose();

            // Return success.
            return true;
        }



        /// <summary>Display a dialog that allows the user to change the user options.</summary>
        /// <returns>True for success, false otherwise.</returns>
        private bool showUserOptions()
        {
            // Create a dialog to edit the user options.
            UserOptionsDialog userOptionsDialog = new UserOptionsDialog();

            // Show the dialog and wait for the dialog to close.
            if (userOptionsDialog.updateOptions(this, ref userOptions_))
            {
                // Update the display for the new user options.
                showCurrentPage();
            }
            userOptionsDialog.Dispose();

            // Return success.
            return true;
        }



        /// <summary>Create a tree document for the current person and displays it in a tree view window.</summary>
        /// <returns>True for success, false otherwise.</returns>
        private bool createTree()
        {
            // Create the tree document.
            TreeDocument tree = new TreeDocument(database_, userOptions_, currentPage.idx);

            // Create a tree preview window.
            TreeViewDialog treeWindow = new TreeViewDialog(tree);
            treeWindow.Show(this);

            // Return success.
            return true;
        }



        /// <summary>Display a dialog to allow the user to select a tree file.  If the user selects one then display the tree in a tree view window.</summary>
        /// <returns>True for success, false otherwise.</returns>
        private bool openTree()
        {
            // Set the file open settings.
            openFileDialog_.Title = "Select Tree file";
            openFileDialog_.CheckFileExists = true;
            openFileDialog_.CheckPathExists = true;
            openFileDialog_.DefaultExt = "tree";
            openFileDialog_.Filter = "Tree Files (*.tree)|*.tree|All Files (*.*)|*.*";
            openFileDialog_.FilterIndex = 0;
            openFileDialog_.Multiselect = false;

            // Display the open file dialog.
            if (openFileDialog_.ShowDialog(this) == DialogResult.OK)
            {
                // Open the selected file.
                openTree(openFileDialog_.FileName);
            }

            // Return success.
            return true;
        }



        /// <summary>Opens the specified tree file in a new window.</summary>
        /// <param name="treeFile">Specifies the full filename of the tree file.</param>
        /// <returns>True for success, false otherwise.</returns>
        private bool openTree(string treeFile)
        {
            // Open the .tree file.
            walton.XmlDocument xmlTreeFile = new walton.XmlDocument(treeFile);

            // Create the tree document.
            TreeDocument tree = new TreeDocument(database_, xmlTreeFile);

            // Create a tree preview window.
            TreeViewDialog treeWindow = new TreeViewDialog(tree);
            treeWindow.Show();

            // Return success.
            return true;
        }



        /// <summary>Create a html report about the current person.</summary>
        /// <returns>True for success, false otherwise.</returns>
        private bool reportToHtml()
        {
            // Create a report object.
            Report report = new Report(currentPage.idx, database_, userOptions_);

            // Hide the person tree panel.
            panelTree_.Visible = false;

            // Display the Html description of the source.
            webBrowser_.DocumentText = userOptions_.renderHtml(report.getReport());

            // Return success.
            return true;
        }



        /// <summary>Displays a dialog that allows the age of a person to be calculated on specified dates.</summary>
        /// <returns>True of success, false otherwise.</returns>
        private bool showAgeDialog()
        {
            // Create and show the age dialog.
            AgeDialog ageDialog = new AgeDialog(database_, currentPage.idx);
            ageDialog.ShowDialog(this);
            ageDialog.Dispose();

            // Return success;
            return true;
        }



        /// <summary>Display the edit census dialog.  This allows the user to edit the census data.</summary>
        /// <returns>True for success, false otherwise.</returns>
        private bool showEditCensusDialog()
        {
            // Create a dialog to edit the sources.
            EditCensusDialog editCensusDialog = new EditCensusDialog(database_, 0);

            // Show the dialog and wait for the dialog to close.
            editCensusDialog.ShowDialog(this);
            editCensusDialog.Dispose();

            // Return success.
            return true;
        }



        /// <summary>Display the recent changes on the main window.</summary>
        /// <remarks>This was previously in a separate window.</remarks>
        /// <returns>True for success, false otherwise.</returns>
        private bool showRecentChanges()
        {
            // Hide the person tree panel.
            panelTree_.Visible = false;

            // Display the Html description of the source.
            webBrowser_.DocumentText = userOptions_.renderHtml(database_.getRecentChangesAsHtml());

            // Return success.
            return true;
        }



        /// <summary>Display the To Do list on the main window.</summary>
        /// <returns>True for success, false otherwise.</returns>
        private bool showToDo()
        {
            // Hide the person tree panel.
            panelTree_.Visible = false;

            // Display the Html description of the source.
            webBrowser_.DocumentText = userOptions_.renderHtml(database_.getToDoAsHtml());

            // Return success.
            return true;
        }



        /// <summary>Show a select file dialog and allow the user to select a new database file.  Open the selected database file if valid.  Returns true if a new database has been selected, false otherwise.</summary>
        /// <returns>True, if a new database was selected.  False, otherwise.</returns>		
        private bool openDatabase()
        {
            // Show the open file dialog
            openFileDialog_.Title = "Select Family Tree File";
            openFileDialog_.Filter = "Database Files (*.mdb)|*.mdb";
            openFileDialog_.FilterIndex = 1;
            if (openFileDialog_.ShowDialog(this) == DialogResult.Cancel)
            {
                // User selected cancel
                return false;
            }

            // Open the selected file
            return openDatabase(openFileDialog_.FileName);
        }



        /// <summary>Opens the specified database file (if valid).  Returns true if a new database has been selected, false otherwise.</summary>
        /// <param name="fileName">Specifies the filename of the database to open.</param>
        /// <returns>True, if a new database was selected.  False, otherwise.</returns>		
        private bool openDatabase(string fileName)
        {
            // Validate the selected file.
            if (!File.Exists(fileName))
            {
                return false;
            }

            // Close the old database
            if (database_ != null)
            {
                database_.Dispose();
            }

            // Open the new database.
            database_ = new Database(fileName, userOptions_.googleMapsKey);

            // Update the recent files.
            recentFiles_.openFile(fileName);
            updateRecentFiles();

            // Show the default person.
            goHome();

            // Update the status bar.
            setStatusBarText("Opened " + fileName, false);

            // Return new database.
            return true;
        }



        /// <summary>Updates the recent file menu.</summary>
        private void updateRecentFiles()
        {
            if (recentFiles_.getRecentFilename(0) != "")
            {
                menuRecentFile1_.Text = "1 " + recentFiles_.getDisplayName(0);
                menuRecentFile1_.Visible = true;
            }
            else
            {
                menuRecentFile1_.Visible = false;
            }
            if (recentFiles_.getRecentFilename(1) != "")
            {
                menuRecentFile2_.Text = "2 " + recentFiles_.getDisplayName(1);
                menuRecentFile2_.Visible = true;
            }
            else
            {
                menuRecentFile2_.Visible = false;
            }
            if (recentFiles_.getRecentFilename(2) != "")
            {
                menuRecentFile3_.Text = "3 " + recentFiles_.getDisplayName(2);
                menuRecentFile3_.Visible = true;
            }
            else
            {
                menuRecentFile3_.Visible = false;
            }
            if (recentFiles_.getRecentFilename(3) != "")
            {
                menuRecentFile4_.Text = "4 " + recentFiles_.getDisplayName(3);
                menuRecentFile4_.Visible = true;
            }
            else
            {
                menuRecentFile4_.Visible = false;
            }
        }



        /// <summary>Allows the user to select an output file to write gedcom into.  Currently this is a the gedcom of the whole database.</summary>
        private void exportGedcom()
        {
            GedcomOptionsDialog gedcomOptionsDialog = new GedcomOptionsDialog(userOptions_.gedcomOptions);
            if (gedcomOptionsDialog.ShowDialog(this) == DialogResult.OK)
            {
                // Save the user options.
                userOptions_.save();

                // Unreachable code.
#pragma warning disable 162
                if (true)
                {
                    // Create a new thread to do the work.
                    ParameterizedThreadStart threadMethod = new ParameterizedThreadStart(writeGedcom);
                    Thread thread = new Thread(threadMethod);
                    thread.Start(userOptions_.gedcomOptions);
                }
                else
                {
                    // Save the gedcom into the specified file.
                    writeGedcom(userOptions_.gedcomOptions);
                }
#pragma warning restore 162
            }
            gedcomOptionsDialog.Dispose();
        }



        /// <summary>Allows the user to export a SQL script.</summary>
        private void exportSql()
        {
            // Initialise the select save file dialog
            saveFileDialog_.Title = "Select output file";
            saveFileDialog_.Filter = "SQL Files (*.sql)|*.sql";
            saveFileDialog_.FilterIndex = 1;
            saveFileDialog_.FileName = "walton.sql";

            if (saveFileDialog_.ShowDialog(this) == DialogResult.OK)
            {
                database_.writeSql(saveFileDialog_.FileName);
            }
        }



        /// <summary>Writes gedcom data into the specified file.  Why is this in the main form would expect this to be in FTObjects?</summary>
        /// <param name="parameter">Specifies the filename of the gedcom file to create.</param>		
        private void writeGedcom(object parameter)
        {
            // Show the wait cursor.
            cursorWait();

            // Estimate the number of steps required.
            walton.XmlNode xmlGedcom = config_.getNode("gedcom");
            int numSteps = xmlGedcom.getAttributeValue("steps", 1000, true);
            progressBarInitialise(numSteps);
            progressBarVisible(true);

            // Decode the parameter.
            GedcomOptions options = (GedcomOptions)parameter;
            string fileName = options.fileName;

            // Open the output file.
            StreamWriter file = new StreamWriter(fileName, false);

            // Write the Gedcom header.
            file.WriteLine("0 HEAD");
            file.WriteLine("1 SOUR FamilyTree");
            file.WriteLine("2 NAME FamilyTree");
            file.WriteLine("2 VERS 1.0.0");
            file.WriteLine("1 DEST DISKETTE");
            file.WriteLine("1 DATE " + DateTime.Now.ToString("d MMM yyyy"));
            file.WriteLine("2 TIME " + DateTime.Now.ToString("HH:mm:ss"));
            file.WriteLine("1 CHAR UTF-8");
            file.WriteLine("1 FILE " + Path.GetFileName(options.fileName));

            // Create a list of Gedcom families objects @F1@ etc ...
            Families families = new Families();

            // Write the individuals.
            IdxName[] allPeople = database_.getPeople();
            for (int i = 0; i < allPeople.Length; i++)
            {
                // Create an object for this person.
                Person person = database_.getPerson(allPeople[i].idx);

                // Check that this person is included in the gedcom file.
                // The person will also need to be excluded from any families that try to reference him.
                bool isInclude = person.isIncludeInGedcom;
                if (options.isAllElements)
                {
                    isInclude = true;
                }

                // Export this person in the gedcom file.
                if (isInclude)
                {
                    // Create Gedcom record for this person
                    file.WriteLine("0 @I" + person.idx.ToString("0000") + "@ INDI");

                    // Create an intial list of sources for this person
                    ArrayList personSources = new ArrayList();
                    person.sourceNonSpecific.gedcomAdd(personSources);

                    file.WriteLine("1 NAME " + person.forenames + " /" + person.birthSurname + "/");
                    file.WriteLine("2 GIVN " + person.forenames);
                    file.WriteLine("2 SURN " + person.birthSurname);
                    if (options.isAllElements)
                    {
                        person.sourceName.writeGedcom(2, file, null);
                    }
                    else
                    {
                        person.sourceName.gedcomAdd(personSources);
                    }

                    file.Write("1 SEX ");
                    if (person.isMale)
                    {
                        file.WriteLine("M");
                    }
                    else
                    {
                        file.WriteLine("F");
                    }
                    file.WriteLine("1 BIRT Y");
                    file.WriteLine("2 DATE " + person.dob.format(DateFormat.GEDCOM));
                    if (options.isAllElements)
                    {
                        person.sourceDoB.writeGedcom(3, file, null);
                    }
                    else
                    {
                        person.sourceDoB.gedcomAdd(personSources);
                    }
                    Fact factLocation = person.getFirstFact(10);
                    if (factLocation != null)
                    {
                        database_.writeGedcomPlace(file, 2, factLocation.information, factLocation.sources, options);
                    }

                    if (!person.dod.isEmpty())
                    {
                        file.WriteLine("1 DEAT Y");
                        if (!person.dod.isEmpty())
                        {
                            file.WriteLine("2 DATE " + person.dod.format(DateFormat.GEDCOM));
                        }
                        if (options.isAllElements)
                        {
                            person.sourceDoD.writeGedcom(3, file, null);
                        }
                        else
                        {
                            person.sourceDoD.gedcomAdd(personSources);
                        }
                        factLocation = person.getFirstFact(90);
                        if (factLocation != null)
                        {
                            database_.writeGedcomPlace(file, 2, factLocation.information, factLocation.sources, options);
                        }
                        string causeOfDeath = person.getSimpleFact(92);
                        if (causeOfDeath != "")
                        {
                            file.WriteLine("2 CAUS " + causeOfDeath);
                        }
                    }

                    // Add this person to the parents family.
                    if (person.fatherIdx > 0 || person.motherIdx > 0)
                    {
                        // Check that the father is included in the gedcom file.
                        int fatherIdx = person.fatherIdx;
                        Person father = new Person(fatherIdx, database_);
                        if (!father.isIncludeInGedcom)
                        {
                            fatherIdx = 0;
                        }

                        // Check that the mother is included in the gedcom file.
                        int motherIdx = person.motherIdx;
                        Person mother = new Person(motherIdx, database_);
                        if (!mother.isIncludeInGedcom)
                        {
                            motherIdx = 0;
                        }

                        //  Get the parent family information.
                        if (motherIdx != 0 || fatherIdx != 0)
                        {
                            Family family = families.getParentFamily(person.fatherIdx, person.motherIdx);
                            family.addChild(person);
                            file.WriteLine("1 FAMC @F" + family.gedcomIdx.ToString("0000") + "@");
                        }
                    }

                    // Create a list of the partners.
                    ArrayList partners = new ArrayList();

                    // Get the relationship information.
                    Relationship[] relationships = person.getRelationships();
                    for (int j = 0; j < relationships.Length; j++)
                    {
                        // Check that the partner is included in the Gedcom file.
                        Person partner = new Person(relationships[j].partnerIdx, database_);
                        if (partner.isIncludeInGedcom)
                        {
                            Family family = families.getMarriageFamily(relationships[j].maleIdx, relationships[j].femaleIdx, relationships[j].idx);
                            file.WriteLine("1 FAMS @F" + family.gedcomIdx.ToString("0000") + "@");
                            relationships[j].sourcePartner.writeGedcom(2, file, null);

                            // Add to the list of partners.
                            partners.Add(relationships[j].partnerIdx);
                        }
                    }

                    // Add the partners in children not already picked up
                    int[] children = person.getChildren();
                    for (int j = 0; j < children.Length; j++)
                    {
                        Person child = database_.getPerson(children[j]);

                        // Check that the childs parent is already a partner
                        if (person.isMale)
                        {
                            if (!partners.Contains(child.motherIdx))
                            {
                                int motherIdx = child.motherIdx;
                                Person mother = new Person(child.motherIdx, database_);
                                if (!mother.isIncludeInGedcom && !options.isAllElements)
                                {
                                    // Use an unknown mother.
                                    motherIdx = 0;
                                }

                                // Create families for new partner.
                                Family family = families.getMarriageFamily(person.idx, motherIdx, 0);
                                file.WriteLine("1 FAMS @F" + family.gedcomIdx.ToString("0000") + "@");

                                // Add to the list of partners
                                partners.Add(motherIdx);
                            }
                        }
                        else
                        {
                            if (!partners.Contains(child.fatherIdx))
                            {
                                int fatherIdx = child.fatherIdx;
                                Person father = new Person(child.fatherIdx, database_);
                                if (!father.isIncludeInGedcom)
                                {
                                    // Use an unknown father
                                    fatherIdx = 0;
                                }

                                // Create families for new partner
                                Family family = families.getMarriageFamily(fatherIdx, person.idx, 0);
                                file.WriteLine("1 FAMS @F" + family.gedcomIdx.ToString("0000") + "@");

                                // Add to the list of partners
                                partners.Add(fatherIdx);
                            }
                        }
                    }

                    // Get Census records.
                    CensusPerson[] censui = database_.censusForPerson(person.idx);
                    foreach (CensusPerson census in censui)
                    {
                        file.WriteLine("1 CENS");
                        file.WriteLine("2 DATE " + census.date.ToString("d MMM yyyy"));
                        database_.writeGedcomPlace(file, 2, census.houseHoldName, null, options);
                        if (census.occupation != "")
                        {
                            file.WriteLine("2 OCCU " + census.occupation);
                        }
                        // file.WriteLine("2 NOTE " + census.livingWith(database_));
                        string[] livingWithPeople = census.livingWith(database_).Split(',');
                        bool isFirst = true;
                        foreach (string livingWithPerson in livingWithPeople)
                        {
                            if (isFirst)
                            {
                                isFirst = false;
                                file.WriteLine("2 NOTE " + livingWithPerson);
                            }
                            else
                            {
                                file.WriteLine("3 CONT " + livingWithPerson.Trim());
                            }
                        }
                        Sources sources = census.getSources(database_);
                        if (sources != null)
                        {
                            if (options.isAllElements)
                            {
                                sources.writeGedcom(2, file, null);
                            }
                            else
                            {
                                sources.gedcomAdd(personSources);
                            }
                        }
                    }

                    Fact[] facts;
                    if (options.isAllElements)
                    {
                        // Education (40).
                        facts = person.getFacts(40);
                        if (facts.Length > 0)
                        {
                            foreach (Fact fact in facts)
                            {
                                file.Write("1 EDUC ");
                                file.WriteLine(fact.information);
                                fact.sources.writeGedcom(2, file, null);
                            }
                        }
                    }

                    // Get the occupation information.
                    facts = person.getFacts(20);
                    if (facts.Length > 0)
                    {
                        foreach (Fact fact in facts)
                        {
                            file.Write("1 OCCU ");
                            file.WriteLine(fact.information);
                            if (options.isAllElements)
                            {
                                fact.sources.writeGedcom(2, file, null);
                            }
                        }
                    }

                    if (options.isAllElements)
                    {
                        // Interests (30).
                        // Comments (100).
                        facts = person.getFacts(100);
                        if (facts.Length > 0)
                        {
                            foreach (Fact fact in facts)
                            {
                                file.Write("1 NOTE ");
                                file.WriteLine(fact.information);
                                fact.sources.writeGedcom(2, file, null);
                            }
                        }
                    }

                    // Attach the media.
                    database_.gedcomWritePersonMedia(file, person.idx, person.mediaIdx);

                    // Attached the list of sources
                    foreach (int sourceIdx in personSources)
                    {
                        file.WriteLine("1 SOUR @S" + sourceIdx.ToString("0000") + "@");
                    }

                    // Write the To Do items in the all output mode.
                    if (options.isAllElements)
                    {
                        ToDo[] todos = person.getToDo();
                        foreach (ToDo todo in todos)
                        {
                            file.WriteLine("1 _TODO " + todo.priority.ToString() + " " + todo.description);
                        }
                    }

                    // Write the last edit information.
                    if (person.lastEditBy != "")
                    {
                        file.WriteLine("1 CHAN");
                        file.WriteLine("2 DATE " + person.lastEditDate.ToString("d MMM yyyy"));
                        file.WriteLine("3 TIME " + person.lastEditDate.ToString("HH:mm:ss"));
                        if (options.isIncludePGVU)
                        {
                            file.WriteLine("2 _PGVU " + person.lastEditBy);
                        }
                    }
                }

                // Progress Bar.
                progressBarPerformStep();
            }

            // Write the family records @F1@...
            families.writeGedcom(file, database_, progressBarPerformStep, options);

            // Write all the source records @S1@ etc...
            database_.writeSourcesGedcom(file, progressBarPerformStep, options);

            // Write all the media records @M1@ etc...
            database_.gedcomWriteMedia(file, options);

            // Write the repository records @R1@ etc ...
            database_.writeRepositoriesGedcom(file);

            // Close the Gedcom header
            file.WriteLine("0 TRLR");

            // Close the output file
            file.Close();

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
        private void panelTreePaint(object sender, PaintEventArgs e)
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
                        if (psnSiblings_[i] != null && psnSiblings_[i].Tag != null)
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
                    if (psnSiblings_[i] != null)
                    {
                        Person sibling = database_.getPerson(psnSiblings_[i].getPersonIdx());
                        if (sibling.hasChildren())
                        {
                            e.Graphics.DrawLine(pen, psnSiblings_[i].Left + psnSiblings_[i].Width / 2, psnSiblings_[i].Top + psnSiblings_[i].Height, psnSiblings_[i].Left + psnSiblings_[i].Width / 2, psnSiblings_[i].Top + psnSiblings_[i].Height + 3);
                        }
                    }
                }
            }

            // There is always a line up to the parents.
            pos = marParents_.Left + marParents_.Width / 2;
            e.Graphics.DrawLine(pen, pos, labPerson_.Top - padding_.y, pos, marParents_.Top + marParents_.Height);

            // Draw a line from the father to his parents.
            if (psnFather_.Visible)
            {
                e.Graphics.DrawLine(pen, psnFather_.Left + psnFather_.Width / 2, psnFather_.Top, psnFather_.Left + psnFather_.Width / 2, psnFather_.Top - 8);
                e.Graphics.DrawLine(pen, psnFather_.Left + psnFather_.Width / 2, psnFather_.Top - 8, marFatherParents_.Left + marFatherParents_.Width / 2, psnFather_.Top - 8);
                e.Graphics.DrawLine(pen, marFatherParents_.Left + marFatherParents_.Width / 2, psnFather_.Top - 8, marFatherParents_.Left + marFatherParents_.Width / 2, marFatherParents_.Top + marFatherParents_.Height);
            }

            // Draw a line from the mother to her parents
            if (psnMother_.Visible)
            {
                e.Graphics.DrawLine(pen, psnMother_.Left + psnMother_.Width / 2, psnMother_.Top, psnMother_.Left + psnMother_.Width / 2, psnMother_.Top - 8);
                e.Graphics.DrawLine(pen, psnMother_.Left + psnMother_.Width / 2, psnMother_.Top - 8, marMotherParents_.Left + marMotherParents_.Width / 2, psnMother_.Top - 8);
                e.Graphics.DrawLine(pen, marMotherParents_.Left + marMotherParents_.Width / 2, psnMother_.Top - 8, marMotherParents_.Left + marMotherParents_.Width / 2, marMotherParents_.Top + marMotherParents_.Height);
            }

            // Draw lines up for the children
            if (psnChildren_ != null)
            {
                int barHeight = psnChildren_[0].Top - (padding_.y / 2);
                int numMarriages = 0;
                if (partnersConntections_ != null)
                {
                    numMarriages = partnersConntections_.Length;
                }

                for (int m = -1; m < numMarriages; m++)
                {
                    bool isDrawBar = false;
                    if (m == -1)
                    {
                        pos = labPerson_.Left + labPerson_.Width / 2;
                    }
                    else
                    {
                        pos = partnersConntections_[m].Left + partnersConntections_[m].Width / 2;
                    }
                    int minPos = pos;
                    int maxPos = pos;

                    for (int i = 0; i < psnChildren_.Length; i++)
                    {
                        // TODO: check this, it crashes.
                        if (psnChildren_[i] != null && psnChildren_[i].Tag != null)
                        {
                            if ((int)psnChildren_[i].Tag == m)
                            {
                                isDrawBar = true;
                                pos = psnChildren_[i].Left + psnChildren_[i].Width / 2;
                                if (pos < minPos)
                                {
                                    minPos = pos;
                                }
                                if (pos > maxPos)
                                {
                                    maxPos = pos;
                                }
                                e.Graphics.DrawLine(pen, pos, psnChildren_[0].Top, pos, barHeight);
                            }
                        }
                    }

                    if (isDrawBar)
                    {
                        // Draw a bar to hang the Siblings on.
                        e.Graphics.DrawLine(pen, minPos, barHeight, maxPos, barHeight);

                        int nHeight;
                        if (m == -1)
                        {
                            pos = labPerson_.Left + labPerson_.Width / 2;
                            nHeight = labPerson_.Top + labPerson_.Height;
                        }
                        else
                        {
                            pos = partnersConntections_[m].Left + partnersConntections_[m].Width / 2;
                            nHeight = partnersConntections_[m].Top + partnersConntections_[m].Height;
                        }
                        e.Graphics.DrawLine(pen, pos, barHeight, pos, nHeight);

                        barHeight += 5;
                    }
                }

                // Draw a line down from the children who have descendants.
                for (int i = 0; i < psnChildren_.Length; i++)
                {
                    if (psnChildren_[i] != null)
                    {
                        Person child = database_.getPerson(psnChildren_[i].getPersonIdx());
                        if (child.hasChildren())
                        {
                            e.Graphics.DrawLine(pen, psnChildren_[i].Left + psnChildren_[i].Width / 2, psnChildren_[i].Top + psnChildren_[i].Height, psnChildren_[i].Left + psnChildren_[i].Width / 2, psnChildren_[i].Top + psnChildren_[i].Height + 5);
                        }
                    }
                }
            }
        }



        /// <summary>Message handler for the main person title paint event.
        /// Draw a border around three sides of the control.
        /// This is here because it is really to do with form painting.
        /// </summary>
        private void labPersonPaint(object sender, PaintEventArgs e)
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
        private void labPersonDatesPaint(object sender, PaintEventArgs e)
        {
            Pen oPen = new Pen(Color.Black, 1);
            e.Graphics.DrawLine(oPen, 0, 0, 0, labPersonDates_.Height - 1);
            e.Graphics.DrawLine(oPen, 0, labPersonDates_.Height - 1, labPersonDates_.Width - 1, labPersonDates_.Height - 1);
            e.Graphics.DrawLine(oPen, labPersonDates_.Width - 1, labPersonDates_.Height - 1, labPersonDates_.Width - 1, 0);
        }



        /// <summary>Message handler for the main form loading.</summary>
        private void frmMainLoad(object sender, System.EventArgs e)
        {
            // Size the main window
            walton.XmlNode oMain = config_.getNode("windows/main");
            WindowState = (FormWindowState)oMain.getAttributeValue("state", (int)FormWindowState.Normal, true);
            Width = oMain.getAttributeValue("width", 800, true);
            Height = oMain.getAttributeValue("height", 600, true);
        }



        /// <summary>Message handler for the shown event.  This is like the post appear load event.</summary>
        private void frmMainShown(object sender, EventArgs e)
        {
            // Open the tree document if specified.
            if (treeToOpen_ != "")
            {
                // MessageBox.Show(this,m_sTreeToOpen,"TreeToOpen");
                openTree(treeToOpen_);
            }
        }



        /// <summary>Message handler for the main window closing.  Save the size of the window.</summary>
        private void frmMainClosing(object sender, System.ComponentModel.CancelEventArgs e)
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



        /// <summary>Message handler for the File → Open menu point click.  Open a family tree database.</summary>
        private void menuOpenClick(object sender, EventArgs e)
        {
            openDatabase();
        }



        /// <summary>Message handler for the File → Home menu point click and the "Home" toolbar button.</summary>
        private void menuHomeClick(object sender, EventArgs e)
        {
            goHome();
        }



        /// <summary>Message handler for the "Back" button click event.</summary>
        private void menuBackClick(object sender, EventArgs e)
        {
            historyBack();
        }



        /// <summary>Message handler for the forward button click event.</summary>
        private void menuForwardClick(object sender, EventArgs e)
        {
            historyForward();
        }



        /// <summary>Message handler for the all the recent file menu points.  Opens the file specified on the recent file menu point.</summary>
        /// <param name="sender">Specifies the menu item that sends this signal.</param>
        /// <param name="e"></param>
        private void menuRecentFileClick(object sender, System.EventArgs e)
        {
            // Find the index of the recent file menu.
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
            int i = int.Parse(menuItem.Text.Substring(0, 1)) - 1;

            // Find the selected file.
            string fileName = recentFiles_.getRecentFilename(i);

            // Open this file.
            openDatabase(fileName);
        }



        /// <summary>Message handler for the "File" → "Export Gedom" menu point.</summary>
        private void menuExportGedcomClick(object sender, EventArgs e)
        {
            exportGedcom();
        }



        /// <summary>Message handler for the 'File' → 'Export SQL script' menu item.</summary>
        private void menuExportSqlScriptClick(object sender, EventArgs e)
        {
            exportSql();
        }



        /// <summary>Message handler for the File → Open Tree menu point click.</summary>
        private void menuOpenTreeClick(object sender, EventArgs e)
        {
            openTree();
        }



        /// <summary>Message handler for File → Exit menu point click.</summary>
        private void menuExitClick(object sender, EventArgs e)
        {
            absoluteEnd();
        }

        #endregion

        #region Edit Menu



        /// <summary>Message handler for the "Edit" -> "Edit..." menu point click and the "Edit" toolbar button click.</summary>
        private void menuEditClick(object sender, EventArgs e)
        {
            edit();
        }



        /// <summary>Message handler for the Edit -> Edit Sources... menu point click.</summary>
        private void menuEditSourcesClick(object sender, EventArgs e)
        {
            showEditSourcesDialog();
        }



        /// <summary>Message handler for the Edit -> Add Father menu point click.</summary>
        private void menuAddFatherClick(object sender, EventArgs e)
        {
            addPerson(RelatedPerson.FATHER);
        }



        /// <summary>Message handler for the Edit -> Add Mother menu point click.</summary>
        private void menuAddMotherClick(object sender, EventArgs e)
        {
            addPerson(RelatedPerson.MOTHER);
        }



        /// <summary>Message handler for the Edit -> Add Sibling menu point click.</summary>
        private void menuAddSiblingClick(object sender, EventArgs e)
        {
            addPerson(RelatedPerson.SIBLING);
        }



        /// <summary>Message handler for the Edit -> Add Child menu point click.</summary>
        private void menuAddChildClick(object sender, EventArgs e)
        {
            addPerson(RelatedPerson.CHILD);
        }



        /// <summary>Message handler for the Edit -> Add Partner menu point click.</summary>
        private void menuAddPartnerClick(object sender, EventArgs e)
        {
            addPerson(RelatedPerson.PARTNER);
        }



        /// <summary>Message handler for the Edit -> Edit Census Records... menu point click.</summary>
        private void menuCensusClick(object sender, EventArgs e)
        {
            showEditCensusDialog();
        }



        /// <summary>Message handler for the Edit -> Remove Unlinked Places menu point click.</summary>
        private void menuUnlinkedPlacesClick(object sender, EventArgs e)
        {
            int nCount = database_.placeRemoveUnlinked();
            MessageBox.Show(this, nCount.ToString() + " unlinked place(s) were removed.", "Unlinked Places", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }



        /// <summary>Message handler for the Edit -> User Options... menu point click.</summary>
        private void menuOptionsClick(object sender, EventArgs e)
        {
            showUserOptions();
        }



        /// <summary>Message handler for the Edit -> Add Media menu point click.</summary>
        private void menuAddMediaClick(object sender, EventArgs e)
        {
            EditMediaDialog editMediaDialog = new EditMediaDialog(database_);
            if (editMediaDialog.ShowDialog(this) == DialogResult.OK)
            {
                int mediaIdx = editMediaDialog.mediaIdx;
                showMedia(mediaIdx, true);
            }
        }



        #endregion

        #region View Menu



        /// <summary>Message handler for the "View" → "Goto Person" menu point click.  Also the Goto person toolbar button click event.</summary>
        private void menuGotoClick(object sender, EventArgs e)
        {
            gotoPerson();
        }



        /// <summary>Message handler for the "View" → "Image" menu point click and the images toolbar button.</summary>
        private void menuImageClick(object sender, EventArgs e)
        {
            m_menuImage.Checked = !m_menuImage.Checked;
            m_tsbImage.Checked = m_menuImage.Checked;
            if (currentPage.content == Pages.PERSON)
            {
                Person oPerson = new Person(currentPage.idx, database_);
                webBrowser_.DocumentText = userOptions_.renderHtml(oPerson.getDescription(true, true, m_menuImage.Checked, m_menuLocation.Checked, true));
            }
        }



        /// <summary>Message handler for the "View" → "Location" menu point click and the location toolbar button.</summary>
        private void menuLocationClick(object sender, EventArgs e)
        {
            m_menuLocation.Checked = !m_menuLocation.Checked;
            m_tsbLocation.Checked = m_menuLocation.Checked;

            switch (currentPage.content)
            {
            case Pages.PERSON:
                Person oPerson = new Person(currentPage.idx, database_);
                webBrowser_.DocumentText = userOptions_.renderHtml(oPerson.getDescription(true, true, m_menuImage.Checked, m_menuLocation.Checked, true));
                break;

            case Pages.PLACE:
                showPlace(currentPage.idx, false);
                break;
            }
        }



        /// <summary>Message handler for the "View" → "Recent Changes" menu point click.</summary>
        private void menuRecentChangesClick(object sender, EventArgs e)
        {
            showRecentChanges();
        }



        /// <summary>Message handler for the View → ToDo menu point click.  Display the To Do on the main window.</summary>
        private void menuToDoClick(object sender, EventArgs e)
        {
            showToDo();
        }



        /// <summary>Message handler for the View → Calculate age menu point click and the Age toolbar button click.</summary>
        private void menuCalcAgeClick(object sender, EventArgs e)
        {
            showAgeDialog();
        }



        /// <summary>Message handler for the View → Calculate Birthday menu point click.</summary>
        private void menuBirthdayClick(object sender, EventArgs e)
        {
            // Display the birthday dialog.
            BirthdayDialog birthdayDialog = new BirthdayDialog();
            birthdayDialog.ShowDialog(this);
            birthdayDialog.Dispose();
        }



        /// <summary>Message handler for the View → Reduce Width menu point click and the reduce width toolbar button.</summary>
        private void menuReduceWidthClick(object sender, EventArgs e)
        {
            if (currentPage.content == Pages.PERSON)
            {
                personSize_.x -= 8;
                fontSize_ = 7.0f;
                showPerson(currentPage.idx, false);
            }
        }



        /// <summary>Message handler for the View → Standard Width menu point click.</summary>
        private void menuStandardWidthClick(object sender, EventArgs e)
        {
            if (currentPage.content == Pages.PERSON)
            {
                personSize_.x = 130;
                fontSize_ = userOptions_.fontBase.size;
                showPerson(currentPage.idx, false);
            }
        }



        /// <summary>Message handler for the "View" → "html Source" menu point.</summary>
        private void menuHtmlSourceClick(object sender, EventArgs e)
        {
            // Create a file to hold the html source

            string fileName = walton.DataPaths.getMyDocuments() + "\\family tree source.html";

            // Open the filename for output.
            StreamWriter streamWriter = new StreamWriter(fileName, false);
            streamWriter.Write(webBrowser_.DocumentText);
            streamWriter.Close();

            // Open the new file in notepad.
            try
            {
                System.Diagnostics.Process.Start("notepad.exe", "\"" + fileName + "\"");
            }
            catch { }
        }



        #endregion

        #region Reports Menu



        /// <summary>Message handler for the "Reports" -> "To Tree" menu point click and the "Tree" toolbar button click.</summary>
        private void menuToTreeClick(object sender, EventArgs e)
        {
            createTree();
        }



        /// <summary>Message handler for the "Reports" -> "To Html" menu point click.</summary>
        private void menuReportToHtmlClick(object sender, EventArgs e)
        {
            reportToHtml();
        }



        #endregion

        #endregion

        #region Natigation (WebBrowser and Person Tree clicks)



        /// <summary>Message handler for the click on a ucPerson object event.  Responds the user clicking on a person object by displaying the person shown in the person object.  This is a bit like a message handler, but for my own event with my own signiture.</summary>
		private void ucPersonEvtClick(object sender)
        {
            family_tree.viewer.PersonDisplay psnPerson = (family_tree.viewer.PersonDisplay)sender;
            showPerson(psnPerson.getPersonIdx(), true);
        }



        /// <summary>Message handler for the web browser trying to follow a link.</summary>
        private void webBrowserNavigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            string newUrl = e.Url.ToString();
            int colonPos = newUrl.IndexOf(':');
            if (colonPos > 0)
            {
                string urlType = newUrl.Substring(0, colonPos).ToLower();
                switch (urlType)
                {
                case "person":
                    e.Cancel = true;
                    int personIdx = int.Parse(newUrl.Substring(colonPos + 1));
                    showPerson(personIdx, true);
                    break;
                case "source":
                    e.Cancel = true;
                    int sourceIdx = int.Parse(newUrl.Substring(colonPos + 1));
                    showSource(sourceIdx, true);
                    break;
                case "place":
                    e.Cancel = true;
                    int placeIdx = int.Parse(newUrl.Substring(colonPos + 1));
                    showPlace(placeIdx, true);
                    break;
                case "media":
                    e.Cancel = true;
                    int mediaIdx = int.Parse(newUrl.Substring(colonPos + 1));
                    showMedia(mediaIdx, true);
                    break;
                }
            }
        }



        /// <summary>Message handler for a individual back (or forward) item click.  This is the drop down combo next to the back and forward buttons.</summary>
        private void individualBackItemClick(object sender, EventArgs e)
        {
            // Find the label of the sending button.
            string buttonLabel = sender.ToString();

            // Find the index of the button in the back section.
            int buttonIdx = -1;
            for (int i = historyIdx_ - 1; i >= 0; i--)
            {
                if (history_[i].label == buttonLabel)
                {
                    buttonIdx = i;
                    i = -1;
                }
            }

            // Search in the forward section if nothing found so far.
            if (buttonIdx < 0)
            {
                for (int i = historyIdx_; i <= historyLast_; i++)
                {
                    if (history_[i].label == buttonLabel)
                    {
                        buttonIdx = i;
                        i = historyLast_ + 1;
                    }
                }
            }

            // Show the specified page.
            if (buttonIdx >= 0)
            {
                historyIdx_ = buttonIdx;
                showCurrentPage();
            }
        }



        #endregion

        #endregion

    }
}
