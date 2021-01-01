using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;

namespace FamilyTree.Viewer
{
	/// <summary>
	/// Form to allow the user to change their preferences.
    /// Their preferences are stored in a clsUserOptions object.
	/// </summary>
    public partial class frmUserOptions : System.Windows.Forms.Form
    {
        #region Member Variables

        /// <summary>
        /// The user options as shown in the dialog.
        /// </summary>
        private clsUserOptions m_oOptions;

        #endregion

        #region Constructors

        /// <summary>
		/// Class constructor.
		/// </summary>
		public frmUserOptions()
		{
			// Required for Windows Form Designer support
			InitializeComponent();

			// Initialise the variables
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

        #region Supporting Functions

        /// <summary>
		/// Displays a dialog that allows the user to change the program options.
        /// Returns true if changes are made, false otherwise.
		/// </summary>
		/// <param name="oParentWindow">Specifies the parent window to lock while this dialog is displayed.</param>
		/// <param name="oOptions">Specifies the current user options, returns the new user options.</param>
		/// <returns>True, if the user makes changes, false otherwise.</returns>		
		public bool UpdateOptions
			(
			IWin32Window oParentWindow,
			ref clsUserOptions oOptions
			)
		{
            // Make a copy of the current options to display and edit in this dialog.
            m_oOptions = new clsUserOptions(oOptions);

            // Update the form with the current options
            m_labTreeMainFont.Font = new System.Drawing.Font(oOptions.treeMainFontName,oOptions.treeMainFontSize);
            m_labTreeMainFont.Text = oOptions.treeMainFontName + " " + oOptions.treeMainFontSize.ToString();
            m_labTreeSubFont.Font = new System.Drawing.Font(oOptions.treeSubFontName,oOptions.treeSubFontSize);
            m_labTreeSubFont.Text = oOptions.treeSubFontName + " " + oOptions.treeSubFontSize.ToString();
            m_chkTreePersonBox.Checked = oOptions.isTreePersonBox;

            // Populate the html preview
            PopulateHtmlPreview();

			// Show the dialog
            if(ShowDialog(oParentWindow) == DialogResult.OK)
            {
                // Update the options with the selection
                oOptions.treeMainFontName = m_labTreeMainFont.Font.Name;
                oOptions.treeSubFontName = m_labTreeSubFont.Font.Name;
                oOptions.treeMainFontSize = m_labTreeMainFont.Font.Size;
                oOptions.treeSubFontSize = m_labTreeSubFont.Font.Size;
                oOptions.isTreePersonBox = m_chkTreePersonBox.Checked;

                oOptions.fontBase = m_oOptions.fontBase;
                oOptions.fontBaseTitle = m_oOptions.fontBaseTitle;
                oOptions.fontBody = m_oOptions.fontBody;
                oOptions.fontHeader = m_oOptions.fontHeader;
                oOptions.fontSmall = m_oOptions.fontSmall;
                oOptions.fontHtmlSuperscript = m_oOptions.fontHtmlSuperscript;

                oOptions.save();

                // Return that changes have been made
                return true;
            }
			// User did not select OK
			return false;
		}

        /// <summary>
        /// Update the html preview box with the current styles.
        /// </summary>
        private void PopulateHtmlPreview()
        {
            StringBuilder sbHtml = new StringBuilder();
            sbHtml.Append("<html><head>");
            sbHtml.Append(m_oOptions.htmlStyle());
            sbHtml.Append("</head><body>");
            sbHtml.Append("<h1>Header</h2>");
            sbHtml.Append("<p>This is standard<span class=\"Superscript\">A</span> text.</p>");
            sbHtml.Append("<p><span class=\"Small\">This is small text.</span></p>");
            sbHtml.Append("</body></html>");
            m_WebBrowser.DocumentText = sbHtml.ToString();
        }

		#endregion

        #region Message Handlers

        private void cmdTreeMainFont_Click(object sender, System.EventArgs e)
		{
			m_FontDialog.Font = m_labTreeMainFont.Font;
			m_FontDialog.ShowDialog(this);
			m_labTreeMainFont.Font = m_FontDialog.Font;
			m_labTreeMainFont.Text = m_FontDialog.Font.Name + " " + m_FontDialog.Font.Size.ToString();
		}

		private void cmdTreeSubFont_Click(object sender, System.EventArgs e)
		{
            m_FontDialog.Font = m_labTreeSubFont.Font;
			m_FontDialog.ShowDialog(this);
			m_labTreeSubFont.Font = m_FontDialog.Font;
			m_labTreeSubFont.Text = m_FontDialog.Font.Name + " " + m_FontDialog.Font.Size.ToString();
        }

        #region Form Events

        /// <summary>
        /// Message handler for the form load event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmUserOptions_Load(object sender,EventArgs e)
        {            
            // Load the available fonts
            for(int nFont = 0;nFont < m_oOptions.numFonts;nFont++)
            {
                m_cboFont.Items.Add(m_oOptions.getFontLabel(nFont));
            }

            m_cboFont.SelectedIndex = 0;
        }

        #endregion

        #region Main Page Fonts

        /// <summary>
        /// Message handler for the selected index changing on the style selector combo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboFont_SelectedIndexChanged(object sender,EventArgs e)
        {
            // Get the selected font
            SimpleFont oFont = m_oOptions.getFont(m_cboFont.SelectedIndex);
            
            // Update the font display
            m_labHtmlStyleFont.Font = oFont.getFont();
            m_labHtmlStyleFont.Text = oFont.name + " " + oFont.size.ToString();
        }

        /// <summary>
        /// Message handler for the change html style font button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_cmdChangeHtmlFont_Click(object sender,EventArgs e)
        {
            m_FontDialog.Font = m_labHtmlStyleFont.Font;
            if(m_FontDialog.ShowDialog(this) == DialogResult.OK)
            {
                // Update the display
                m_labHtmlStyleFont.Font = m_FontDialog.Font;
                m_labHtmlStyleFont.Text = m_FontDialog.Font.Name + " " + m_FontDialog.Font.Size.ToString();

                // Update the copy of the options
                SimpleFont oFont = m_oOptions.getFont(m_cboFont.SelectedIndex);
                oFont.copy(m_FontDialog.Font);

                // Update the preview box.
                PopulateHtmlPreview();
            }
        }

        #endregion

        #endregion
    }
}
