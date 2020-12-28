using System;
using System.Text;

// clsGedcomOptions
using FamilyTree.Objects;

namespace FamilyTree.Viewer
{
    // Class to represent the user options
    /// <summary>
    /// Class to represent the user options
    /// </summary>
	public class clsUserOptions
	{
		#region Member Variables

        /// <summary>The configuration file where the options are stored.</summary>
        private Innoval.clsXmlDocument m_oConfig;

        /// <summary>Name of the main (larger) font to use on tree diagrams.</summary>
		public string m_sTreeMainFontName;

		/// <summary>Name of the smaller (secondary) font to use on tree diagrams.</summary>
		public string	m_sTreeSubFontName;

		/// <summary>Size of the main (larger) font to use on tree diagrams.</summary>
		public float m_dTreeMainFontSize;

		/// <summary>Size of the smaller (secondary) font to use on tree diagrams.</summary>
		public float m_dTreeSubFontSize;

		/// <summary>True to draw a box around people on the tree diagrams.</summary>
		public bool m_bTreePersonBox;

        /// <summary>Font to use on the main window for the tree image.</summary>
        public CFont FontBase;

        /// <summary>Font to use for the person title on the main window for the tree image.</summary>
        public CFont FontBaseTitle;
        
        /// <summary>Font to use for the body text in the html.</summary>
        public CFont fontBody;

        /// <summary>Font to use for the header text in the html.</summary>
        public CFont fontHeader;

        /// <summary>Font to use for small text in the html.  This is often in tables as footnotes.</summary>
        public CFont fontSmall;

        /// <summary>Font to use for superscript text in the html.</summary>
        public CFont fontHtmlSuperscript;

        // The options for the Gedcom export.
        /// <summary>
        /// The options for the Gedcom export.
        /// </summary>
        private clsGedcomOptions m_oGedcomOptions;

		#endregion

		#region Constructors etc...

        // Class constructor. The values are loaded from the specified configuration file.
        /// <summary>
        /// Class constructor. The values are loaded from the specified configuration file.
        /// </summary>
		/// <param name="oConfig">Specifies the configuration file to load the settings from.</param>
        public clsUserOptions(Innoval.clsXmlDocument oConfig)
        {
            m_oConfig = oConfig;

            Innoval.clsXmlNode oUserOptions = m_oConfig.GetNode("useroptions");

            // Main window options
            Innoval.clsXmlNode oBaseFont = oUserOptions.GetNode("mainwindow/basefont");
            FontBase = new CFont(oBaseFont,"Tahoma",8.25f);
            Innoval.clsXmlNode oBaseFontTitle = oUserOptions.GetNode("mainwindow/basefonttitle");
            FontBaseTitle = new CFont(oBaseFontTitle,"Tahoma",10.25f);

            // Tree options
            Innoval.clsXmlNode oMainFont = oUserOptions.GetNode("tree/mainfont");
            m_sTreeMainFontName = oMainFont.GetAttributeValue("name","Tahoma",true);
            m_dTreeMainFontSize = oMainFont.GetAttributeValue("size",12.0f,true);
            Innoval.clsXmlNode oSubFont = oUserOptions.GetNode("tree/subfont");
            m_sTreeSubFontName = oSubFont.GetAttributeValue("name","Tahoma",true);
            m_dTreeSubFontSize = oSubFont.GetAttributeValue("size",7f,true);
            Innoval.clsXmlNode oPerson = oUserOptions.GetNode("tree/person");
            m_bTreePersonBox = oPerson.GetAttributeValue("boxaround",false,true);

            // The html format options            
            Innoval.clsXmlNode oBodyFont = oUserOptions.GetNode("html/bodyfont");
            fontBody = new CFont(oBodyFont,"Verdana",9f);
            Innoval.clsXmlNode oHeaderFont = oUserOptions.GetNode("html/headerfont");
            fontHeader = new CFont(oHeaderFont,"Verdana",12f);
            Innoval.clsXmlNode oSmallFont = oUserOptions.GetNode("html/smallfont");
            fontSmall = new CFont(oSmallFont,"Verdana",8f);
            Innoval.clsXmlNode oSuperscriptFont = oUserOptions.GetNode("html/superscript");
            fontHtmlSuperscript = new CFont(oSuperscriptFont,"Verdana",8f);

            // Load the gedcom options.
            Innoval.clsXmlNode xmlGedcomOptions = oUserOptions.GetNode("gedcom");
            m_oGedcomOptions = new clsGedcomOptions();
            m_oGedcomOptions.Load (xmlGedcomOptions);
        }

        // Class constructor. Creates a copy of the specified clsUserOptions object.
        /// <summary>
        /// Class constructor. Creates a copy of the specified clsUserOptions object.
        /// </summary>
        /// <param name="oOptions">Specifies the options to take the values from.</param>
        public clsUserOptions(clsUserOptions oOptions)
        {
            m_oConfig = oOptions.m_oConfig;

            // Main Window options            
            FontBase = new CFont(oOptions.FontBase);
            FontBaseTitle = new CFont(oOptions.FontBaseTitle);

            // Tree options
            m_sTreeMainFontName = oOptions.m_sTreeMainFontName;
            m_dTreeMainFontSize = oOptions.m_dTreeMainFontSize;
            m_sTreeSubFontName = oOptions.m_sTreeSubFontName;
            m_dTreeSubFontSize = oOptions.m_dTreeSubFontSize;
            m_bTreePersonBox = oOptions.m_bTreePersonBox;

            // html format options            
            fontBody = new CFont(oOptions.fontBody);
            fontHeader = new CFont(oOptions.fontHeader);
            fontSmall = new CFont(oOptions.fontSmall);
            fontHtmlSuperscript = new CFont(oOptions.fontHtmlSuperscript);

            // Gedcom options
            m_oGedcomOptions = new clsGedcomOptions(oOptions.GedcomOptions);
        }

        // Restores the default values to all members of the class.
        /// <summary>
        /// Restores the default values to all members of the class.
        /// </summary>
		public void RestoreDefaults()
		{
			m_sTreeMainFontName = "Tahoma";
			m_sTreeSubFontName = "Tahoma";
			m_dTreeMainFontSize = 12;
			m_dTreeSubFontSize = 7;
			m_bTreePersonBox = false;
		}

        // Writes the user options into the configuration file.
        /// <summary>
        /// Writes the user options into the configuration file.
        /// </summary>
        /// <returns>True for success, false otherwise.</returns>
        public bool Save()
        {
            Innoval.clsXmlNode oUserOptions = m_oConfig.GetNode("useroptions");
            
            // Tree options
            Innoval.clsXmlNode oMainFont = oUserOptions.GetNode("tree/mainfont");
            oMainFont.SetAttributeValue("name",m_sTreeMainFontName);
            oMainFont.SetAttributeValue("size",m_dTreeMainFontSize);
            Innoval.clsXmlNode oSubFont = oUserOptions.GetNode("tree/subfont");
            oSubFont.SetAttributeValue("name",m_sTreeSubFontName);
            oSubFont.SetAttributeValue("size",m_dTreeSubFontSize);
            Innoval.clsXmlNode oPerson = oUserOptions.GetNode("tree/person");
            oPerson.SetAttributeValue("boxaround",m_bTreePersonBox);

            // Main window options
            Innoval.clsXmlNode oBaseFont = oUserOptions.GetNode("mainwindow/basefont");
            FontBase.Save(oBaseFont);
            Innoval.clsXmlNode oBaseFontTitle = oUserOptions.GetNode("mainwindow/basefonttitle");
            FontBaseTitle.Save(oBaseFontTitle);
            
            // Write the html options
            Innoval.clsXmlNode oBodyFont = oUserOptions.GetNode("html/bodyfont");
            fontBody.Save(oBodyFont);
            Innoval.clsXmlNode oHeaderFont = oUserOptions.GetNode("html/headerfont");
            fontHeader.Save(oHeaderFont);
            Innoval.clsXmlNode oSmallFont = oUserOptions.GetNode("html/smallfont");
            fontSmall.Save(oSmallFont);
            Innoval.clsXmlNode oSuperscriptFont = oUserOptions.GetNode("html/superscript");
            fontHtmlSuperscript.Save(oSuperscriptFont);

            // Save the gedcom options.
            Innoval.clsXmlNode xmlGedcomOptions = oUserOptions.GetNode("gedcom");
            m_oGedcomOptions.Save(xmlGedcomOptions);

            // Save the configuration file to disk
            m_oConfig.Save();

            // Return success
            return true;
        }

		#endregion

        #region Public Methods

        // Adds the html headers to the specified html content, ready for display.
        /// <summary>
        /// Adds the html headers to the specified html content, ready for display.
        /// </summary>
        /// <param name="sHtmlContent">Specifies the body content of the html page.</param>
        /// <returns>Fully specified html ready for display.</returns>
        public string RenderHtml(string sHtmlContent)
        {
            StringBuilder sbHtml = new StringBuilder();
            sbHtml.AppendLine("<html>");
            sbHtml.AppendLine("<head>");
            sbHtml.Append(HtmlStyle());
            sbHtml.AppendLine("</head>");
            sbHtml.AppendLine("<body>");
            sbHtml.Append(sHtmlContent);
            sbHtml.AppendLine("</body>");
            sbHtml.Append("</html>");

            // Return the Html built
            return sbHtml.ToString();
        }

        // Returns the standard Html style block for Html output.
        /// <summary>
        /// Returns the standard Html style block for Html output.
        /// </summary>
        /// <returns>The standard Html style block.</returns>
        public string HtmlStyle()
        {
            StringBuilder sbStyles = new StringBuilder();

            sbStyles.AppendLine("<style><!--");
            sbStyles.AppendLine("p {font-family: '" + fontBody.Name + "'; font-size: " + fontBody.Size.ToString() + "pt; margin-top: 3pt; margin-bottom: 3pt;line-height: " + (fontBody.Size + 6).ToString() + "pt;}");
            sbStyles.AppendLine("td {font-family: '" + fontBody.Name + "'; font-size:" + fontBody.Size.ToString() + "pt; margin-top: 3pt; margin-bottom: 3pt}");
            sbStyles.AppendLine("h1 {font-family: '" + fontHeader.Name + "'; font-size:" + fontHeader.Size.ToString() + "pt; margin-top: 3pt; margin-bottom: 3pt}");
            sbStyles.AppendLine("a {text-decoration: none}");
            sbStyles.AppendLine("a:hover {text-decoration: underline}");
            sbStyles.AppendLine("a:visited {color: blue}");
            sbStyles.AppendLine(".Superscript {font-family: '" + fontHtmlSuperscript.Name + "'; font-size:" + fontHtmlSuperscript.Size.ToString() + "pt; vertical-align: super;}"); // line-height: " + (fontBody.Size + 6).ToString() + "pt;
            sbStyles.AppendLine(".Small {font-family: '" + fontSmall.Name + "'; font-size: " + fontSmall.Size.ToString() + "pt; margin-top: 3pt; margin-bottom: 3pt}");
            sbStyles.AppendLine(".Background {font-family: 'Verdana'; font-size: 8pt; color: silver; margin-top: 3pt; margin-bottom: 3pt}");
            sbStyles.AppendLine(".Census {font-family: 'Times New Roman'; font-size: 8pt; color: darkcyan; margin-top: 3pt; margin-bottom: 3pt}");
            sbStyles.AppendLine(".Marriage {font-family: 'Times New Roman'; font-size: 8pt; color: seagreen; margin-top: 3pt; margin-bottom: 3pt}");
            sbStyles.AppendLine(".Birth {font-family: 'Times New Roman'; font-size: 8pt; color: orangered; margin-top: 3pt; margin-bottom: 3pt}");
            sbStyles.AppendLine(".Death {font-family: 'Times New Roman'; font-size: 8pt; color: purple; margin-top: 3pt; margin-bottom: 3pt}");
            sbStyles.AppendLine("--> </style>");

            // Return the style block
            return sbStyles.ToString();
        }

        // Returns the number of fonts that the object will return in its font array.
        /// <summary>
        /// Returns the number of fonts that the object will return in its font array.
        /// </summary>
        public int NumFonts
        {
            get
            {
                return 6;
            }
        }

        // Returns the label for each font in the array of fonts.
        /// <summary>
        /// Returns the label for each font in the array of fonts.
        /// </summary>
        /// <param name="nIndex">Specifies the index of the font of interest.</param>
        /// <returns>The label for the specified font.</returns>
        public string GetFontLabel(int nIndex)
        {
            switch(nIndex)
            {
            case 0:
                return "Main Window Tree";
            case 1:
                return "Main Window Tree Title";
            case 2:
                return "Html Body";
            case 3:
                return "Html Title";
            case 4:
                return "Html Small";
            case 5:
                return "Html Superscript";
            default:
                return "Error";
            }
        }

        // Returns the font object for the specified index.
        /// <summary>
        /// Returns the font object for the specified index.
        /// </summary>
        /// <param name="nIndex">Specifies the index of the font.</param>
        /// <returns>The font for the specified index.</returns>
        public CFont GetFont(int nIndex)
        {
            switch(nIndex)
            {
            case 0:
                return FontBase;
            case 1:
                return FontBaseTitle;
            case 2:
                return fontBody;
            case 3:
                return fontHeader;
            case 4:
                return fontSmall;
            case 5:
                return fontHtmlSuperscript;
            default:
                return null;
            }
        }

        // The gedcom options.
        /// <summary>
        /// The gedcom options.
        /// </summary>
        public clsGedcomOptions GedcomOptions
        {
            get
            {
                return m_oGedcomOptions;
            }
        }

        #endregion
    }

    #region Supporting Classes, Types etc ...

    /// <summary>Class to represent a font in the user options.
    /// Trying the notation of C{classname}.
    /// </summary>
    /// <remarks>
    /// Not able to inherit from Font class.
    /// </remarks>
    public class CFont
    {
        #region Member Variables

        /// <summary>Name of the font.</summary>
        public string Name;

        /// <summary>Size of the font.</summary>
        public float Size;

        /// <summary>
        /// The style of the font.  Eg. Bold etc ...
        /// </summary>
        public System.Drawing.FontStyle Style;

        #endregion

        #region Constructors etc ...

        /// <summary>Class constructor. Creates a copy of the specified CFont object.
        /// </summary>
        /// <param name="oFont">Specifies the CFont object to copy.</param>
        public CFont            (            CFont oFont            )
        {
            Name = oFont.Name;
            Size = oFont.Size;
            Style = oFont.Style;
        }

        /// <summary>Class constrctor. Loads the settings from the specified node in a configuration file.
        /// If the settings are not in the configuration file then the specified default settings are used.
        /// </summary>
        /// <param name="oNode">Specifies the node of the configuration file to load the settings from.</param>
        /// <param name="sDefaultName">Specifies the default name for the font.</param>
        /// <param name="dDefaultSize">Specifies the default size for the font.</param>
        public CFont(Innoval.clsXmlNode oNode,string sDefaultName,float dDefaultSize)
        {
            Load(oNode,sDefaultName,dDefaultSize,System.Drawing.FontStyle.Regular);
        }

        /// <summary>Class constrctor. Loads the settings from the specified node in a configuration file.
        /// If the settings are not in the configuration file then the specified default settings are used.
        /// </summary>
        /// <param name="oNode">Specifies the node of the configuration file to load the settings from.</param>
        /// <param name="sDefaultName">Specifies the default name for the font.</param>
        /// <param name="dDefaultSize">Specifies the default size for the font.</param>
        /// <param name="DefaultStyle">Specifies the default style for the font.  Eg Bold etc.</param>
        public CFont(Innoval.clsXmlNode oNode,string sDefaultName,float dDefaultSize,System.Drawing.FontStyle DefaultStyle)
        {
            Load(oNode,sDefaultName,dDefaultSize,DefaultStyle);
        }

        /// <summary>
        /// Load the font settings from the specified node in a configuration file.
        /// </summary>
        /// <param name="oNode">Specifies the node of the configuration file to load the settings from.</param>
        /// <param name="sDefaultName">Specifies the default name for the font.</param>
        /// <param name="dDefaultSize">Specifies the default size for the font.</param>
        /// <param name="DefaultStyle">Specifies the default style for the font.  Eg Bold etc.</param>
        public void Load(Innoval.clsXmlNode oNode,string sDefaultName,float dDefaultSize,System.Drawing.FontStyle DefaultStyle)
        {
            Name = oNode.GetAttributeValue("name",sDefaultName,true);
            Size = oNode.GetAttributeValue("size",dDefaultSize,true);
            Style = (System.Drawing.FontStyle)oNode.GetAttributeValue("style",(int)DefaultStyle,false);
        }

        /// <summary>Saves the font settings into the specified node in a configuration file.
        /// </summary>
        /// <param name="oNode">Specifies the node of the configuration file to write the settings into.</param>
        public void Save(Innoval.clsXmlNode oNode)
        {
            oNode.SetAttributeValue("name",Name);
            oNode.SetAttributeValue("size",Size);
            oNode.SetAttributeValue("style",(int)Style);
        }

        /// <summary>Update the CFont object from the specified font object.
        /// </summary>
        /// <param name="oFont"></param>
        public void Copy(System.Drawing.Font oFont)
        {
            Name = oFont.Name;
            Size = oFont.Size;
            Style = oFont.Style;
        }

        #endregion

        /// <summary>The size of the font as an integer.
        /// </summary>
        /// <returns></returns>
        public int FontSize()
        {
            return (int)Math.Round(Size);
        }

        /// <summary>Returns the font that this object represents.
        /// </summary>
        /// <returns></returns>
        public System.Drawing.Font GetFont()
        {
            return new System.Drawing.Font(Name,Size,Style);
        }
    }

    #endregion
}
