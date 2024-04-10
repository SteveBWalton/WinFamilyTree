using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;

namespace family_tree.viewer
{
    /// <summary>Form to allow the user to change their preferences.  Their preferences are stored in a clsUserOptions object.</summary>
    public partial class UserOptionsDialog : System.Windows.Forms.Form
    {
        #region Member Variables

        /// <summary>The user options as shown in the dialog.</summary>
        private UserOptions userOptions_;

        #endregion

        #region Constructors



        /// <summary>Class constructor.</summary>
        public UserOptionsDialog()
        {
            // Required for Windows Form Designer support
            InitializeComponent();

            // Initialise the variables
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



        /// <summary>Displays a dialog that allows the user to change the program options.  Returns true if changes are made, false otherwise.</summary>
        /// <param name="parentWindow">Specifies the parent window to lock while this dialog is displayed.</param>
        /// <param name="userOptions">Specifies the current user options, returns the new user options.</param>
        /// <returns>True, if the user makes changes, false otherwise.</returns>
        public bool updateOptions(IWin32Window parentWindow, ref UserOptions userOptions)
        {
            // Make a copy of the current options to display and edit in this dialog.
            userOptions_ = new UserOptions(userOptions);

            // Update the form with the current options.
            labTreeMainFont_.Font = new System.Drawing.Font(userOptions.treeMainFontName, userOptions.treeMainFontSize);
            labTreeMainFont_.Text = userOptions.treeMainFontName + " " + userOptions.treeMainFontSize.ToString();
            labTreeSubFont_.Font = new System.Drawing.Font(userOptions.treeSubFontName, userOptions.treeSubFontSize);
            labTreeSubFont_.Text = userOptions.treeSubFontName + " " + userOptions.treeSubFontSize.ToString();
            chkTreePersonBox_.Checked = userOptions.isTreePersonBox;
            textBoxGoogleMapsKey_.Text = userOptions.googleMapsKey;

            // Populate the html preview.
            populateHtmlPreview();

            // Show the dialog.
            if (ShowDialog(parentWindow) == DialogResult.OK)
            {
                // Update the options with the selection.
                userOptions.treeMainFontName = labTreeMainFont_.Font.Name;
                userOptions.treeSubFontName = labTreeSubFont_.Font.Name;
                userOptions.treeMainFontSize = labTreeMainFont_.Font.Size;
                userOptions.treeSubFontSize = labTreeSubFont_.Font.Size;
                userOptions.isTreePersonBox = chkTreePersonBox_.Checked;

                userOptions.fontBase = userOptions_.fontBase;
                userOptions.fontBaseTitle = userOptions_.fontBaseTitle;
                userOptions.fontBody = userOptions_.fontBody;
                userOptions.fontHeader = userOptions_.fontHeader;
                userOptions.fontSmall = userOptions_.fontSmall;
                userOptions.fontHtmlSuperscript = userOptions_.fontHtmlSuperscript;

                userOptions.googleMapsKey = textBoxGoogleMapsKey_.Text;

                userOptions.save();

                // Return that changes have been made.
                return true;
            }
            // User did not select OK.
            return false;
        }



        /// <summary>Update the html preview box with the current styles.</summary>
        private void populateHtmlPreview()
        {
            StringBuilder html = new StringBuilder();
            html.Append("<html><head>");
            html.Append(userOptions_.htmlStyle());
            html.Append("</head><body>");
            html.Append("<h1>Header</h2>");
            html.Append("<p>This is standard<span class=\"Superscript\">A</span> text.</p>");
            html.Append("<p><span class=\"Small\">This is small text.</span></p>");
            html.Append("</body></html>");
            webBrowser_.DocumentText = html.ToString();
        }



        #endregion

        #region Message Handlers



        private void cmdTreeMainFontClick(object sender, System.EventArgs e)
        {
            fontDialog_.Font = labTreeMainFont_.Font;
            fontDialog_.ShowDialog(this);
            labTreeMainFont_.Font = fontDialog_.Font;
            labTreeMainFont_.Text = fontDialog_.Font.Name + " " + fontDialog_.Font.Size.ToString();
        }



        private void cmdTreeSubFontClick(object sender, System.EventArgs e)
        {
            fontDialog_.Font = labTreeSubFont_.Font;
            fontDialog_.ShowDialog(this);
            labTreeSubFont_.Font = fontDialog_.Font;
            labTreeSubFont_.Text = fontDialog_.Font.Name + " " + fontDialog_.Font.Size.ToString();
        }



        #region Form Events



        /// <summary>Message handler for the form load event.</summary>
        private void frmUserOptionsLoad(object sender, EventArgs e)
        {
            // Load the available fonts
            for (int i = 0; i < userOptions_.numFonts; i++)
            {
                cboFont_.Items.Add(userOptions_.getFontLabel(i));
            }

            cboFont_.SelectedIndex = 0;
        }



        #endregion

        #region Main Page Fonts



        /// <summary>Message handler for the selected index changing on the style selector combo.</summary>
        private void cboFontSelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected font
            SimpleFont simpleFont = userOptions_.getFont(cboFont_.SelectedIndex);

            // Update the font display
            labHtmlStyleFont_.Font = simpleFont.getFont();
            labHtmlStyleFont_.Text = simpleFont.name + " " + simpleFont.size.ToString();
        }



        /// <summary>Message handler for the change html style font button click.</summary>
        private void cmdChangeHtmlFontClick(object sender, EventArgs e)
        {
            fontDialog_.Font = labHtmlStyleFont_.Font;
            if (fontDialog_.ShowDialog(this) == DialogResult.OK)
            {
                // Update the display.
                labHtmlStyleFont_.Font = fontDialog_.Font;
                labHtmlStyleFont_.Text = fontDialog_.Font.Name + " " + fontDialog_.Font.Size.ToString();

                // Update the copy of the options.
                SimpleFont simpleFont = userOptions_.getFont(cboFont_.SelectedIndex);
                simpleFont.copy(fontDialog_.Font);

                // Update the preview box.
                populateHtmlPreview();
            }
        }



        #endregion

        #endregion
    }
}
