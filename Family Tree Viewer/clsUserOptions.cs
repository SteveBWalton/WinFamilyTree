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
        private walton.XmlDocument config_;

        /// <summary>Name of the main (larger) font to use on tree diagrams.</summary>
        public string treeMainFontName;

        /// <summary>Name of the smaller (secondary) font to use on tree diagrams.</summary>
        public string treeSubFontName;

        /// <summary>Size of the main (larger) font to use on tree diagrams.</summary>
        public float treeMainFontSize;

        /// <summary>Size of the smaller (secondary) font to use on tree diagrams.</summary>
        public float treeSubFontSize;

        /// <summary>True to draw a box around people on the tree diagrams.</summary>
        public bool isTreePersonBox;

        /// <summary>Font to use on the main window for the tree image.</summary>
        public SimpleFont fontBase;

        /// <summary>Font to use for the person title on the main window for the tree image.</summary>
        public SimpleFont fontBaseTitle;

        /// <summary>Font to use for the body text in the html.</summary>
        public SimpleFont fontBody;

        /// <summary>Font to use for the header text in the html.</summary>
        public SimpleFont fontHeader;

        /// <summary>Font to use for small text in the html.  This is often in tables as footnotes.</summary>
        public SimpleFont fontSmall;

        /// <summary>Font to use for superscript text in the html.</summary>
        public SimpleFont fontHtmlSuperscript;

        /// <summary>The options for the Gedcom export.</summary>
        private GedcomOptions gedcomOptions_;

        #endregion

        #region Constructors etc...



        /// <summary>Class constructor. The values are loaded from the specified configuration file.</summary>
        /// <param name="xmlConfig">Specifies the configuration file to load the settings from.</param>
        public clsUserOptions(walton.XmlDocument xmlConfig)
        {
            config_ = xmlConfig;

            walton.XmlNode xmlUserOptions = config_.getNode("useroptions");

            // Main window options.
            walton.XmlNode xmlBaseFont = xmlUserOptions.getNode("mainwindow/basefont");
            fontBase = new SimpleFont(xmlBaseFont, "Tahoma", 8.25f);
            walton.XmlNode oBaseFontTitle = xmlUserOptions.getNode("mainwindow/basefonttitle");
            fontBaseTitle = new SimpleFont(oBaseFontTitle, "Tahoma", 10.25f);

            // Tree options.
            walton.XmlNode xmlMainFont = xmlUserOptions.getNode("tree/mainfont");
            treeMainFontName = xmlMainFont.getAttributeValue("name", "Tahoma", true);
            treeMainFontSize = xmlMainFont.getAttributeValue("size", 12.0f, true);
            walton.XmlNode xmlSubFont = xmlUserOptions.getNode("tree/subfont");
            treeSubFontName = xmlSubFont.getAttributeValue("name", "Tahoma", true);
            treeSubFontSize = xmlSubFont.getAttributeValue("size", 7f, true);
            walton.XmlNode xmlPerson = xmlUserOptions.getNode("tree/person");
            isTreePersonBox = xmlPerson.getAttributeValue("boxaround", false, true);

            // The html format options.
            walton.XmlNode xmlBodyFont = xmlUserOptions.getNode("html/bodyfont");
            fontBody = new SimpleFont(xmlBodyFont, "Verdana", 9f);
            walton.XmlNode xmlHeaderFont = xmlUserOptions.getNode("html/headerfont");
            fontHeader = new SimpleFont(xmlHeaderFont, "Verdana", 12f);
            walton.XmlNode xmlSmallFont = xmlUserOptions.getNode("html/smallfont");
            fontSmall = new SimpleFont(xmlSmallFont, "Verdana", 8f);
            walton.XmlNode xmlSuperscriptFont = xmlUserOptions.getNode("html/superscript");
            fontHtmlSuperscript = new SimpleFont(xmlSuperscriptFont, "Verdana", 8f);

            // Load the gedcom options.
            walton.XmlNode xmlGedcomOptions = xmlUserOptions.getNode("gedcom");
            gedcomOptions_ = new GedcomOptions();
            gedcomOptions_.load(xmlGedcomOptions);
        }



        /// <summary>Class constructor. Creates a copy of the specified clsUserOptions object.</summary>
        /// <param name="userOptions">Specifies the options to take the values from.</param>
        public clsUserOptions(clsUserOptions userOptions)
        {
            config_ = userOptions.config_;

            // Main Window options.
            fontBase = new SimpleFont(userOptions.fontBase);
            fontBaseTitle = new SimpleFont(userOptions.fontBaseTitle);

            // Tree options.
            treeMainFontName = userOptions.treeMainFontName;
            treeMainFontSize = userOptions.treeMainFontSize;
            treeSubFontName = userOptions.treeSubFontName;
            treeSubFontSize = userOptions.treeSubFontSize;
            isTreePersonBox = userOptions.isTreePersonBox;

            // html format options.
            fontBody = new SimpleFont(userOptions.fontBody);
            fontHeader = new SimpleFont(userOptions.fontHeader);
            fontSmall = new SimpleFont(userOptions.fontSmall);
            fontHtmlSuperscript = new SimpleFont(userOptions.fontHtmlSuperscript);

            // Gedcom options.
            gedcomOptions_ = new GedcomOptions(userOptions.gedcomOptions);
        }



        /// <summary>Restores the default values to all members of the class.</summary>
        public void restoreDefaults()
        {
            treeMainFontName = "Tahoma";
            treeSubFontName = "Tahoma";
            treeMainFontSize = 12;
            treeSubFontSize = 7;
            isTreePersonBox = false;
        }



        /// <summary>Writes the user options into the configuration file.</summary>
        /// <returns>True for success, false otherwise.</returns>
        public bool save()
        {
            walton.XmlNode oUserOptions = config_.getNode("useroptions");

            // Tree options.
            walton.XmlNode oMainFont = oUserOptions.getNode("tree/mainfont");
            oMainFont.setAttributeValue("name", treeMainFontName);
            oMainFont.setAttributeValue("size", treeMainFontSize);
            walton.XmlNode oSubFont = oUserOptions.getNode("tree/subfont");
            oSubFont.setAttributeValue("name", treeSubFontName);
            oSubFont.setAttributeValue("size", treeSubFontSize);
            walton.XmlNode oPerson = oUserOptions.getNode("tree/person");
            oPerson.setAttributeValue("boxaround", isTreePersonBox);

            // Main window options.
            walton.XmlNode oBaseFont = oUserOptions.getNode("mainwindow/basefont");
            fontBase.save(oBaseFont);
            walton.XmlNode oBaseFontTitle = oUserOptions.getNode("mainwindow/basefonttitle");
            fontBaseTitle.save(oBaseFontTitle);

            // Write the html options.
            walton.XmlNode oBodyFont = oUserOptions.getNode("html/bodyfont");
            fontBody.save(oBodyFont);
            walton.XmlNode oHeaderFont = oUserOptions.getNode("html/headerfont");
            fontHeader.save(oHeaderFont);
            walton.XmlNode oSmallFont = oUserOptions.getNode("html/smallfont");
            fontSmall.save(oSmallFont);
            walton.XmlNode oSuperscriptFont = oUserOptions.getNode("html/superscript");
            fontHtmlSuperscript.save(oSuperscriptFont);

            // Save the gedcom options.
            walton.XmlNode xmlGedcomOptions = oUserOptions.getNode("gedcom");
            gedcomOptions_.save(xmlGedcomOptions);

            // Save the configuration file to disk.
            config_.save();

            // Return success.
            return true;
        }



        #endregion

        #region Public Methods

        /// <summary>Adds the html headers to the specified html content, ready for display.</summary>
        /// <param name="htmlContent">Specifies the body content of the html page.</param>
        /// <returns>Fully specified html ready for display.</returns>
        public string renderHtml(string htmlContent)
        {
            StringBuilder html = new StringBuilder();
            html.AppendLine("<html>");
            html.AppendLine("<head>");
            html.Append(htmlStyle());
            html.AppendLine("</head>");
            html.AppendLine("<body>");
            html.Append(htmlContent);
            html.AppendLine("</body>");
            html.Append("</html>");

            // Return the Html built.
            return html.ToString();
        }



        /// <summary>Returns the standard Html style block for Html output.</summary>
        /// <returns>The standard Html style block.</returns>
        public string htmlStyle()
        {
            StringBuilder htmlStyles = new StringBuilder();

            htmlStyles.AppendLine("<style><!--");
            htmlStyles.AppendLine("p {font-family: '" + fontBody.name + "'; font-size: " + fontBody.size.ToString() + "pt; margin-top: 3pt; margin-bottom: 3pt;line-height: " + (fontBody.size + 6).ToString() + "pt;}");
            htmlStyles.AppendLine("td {font-family: '" + fontBody.name + "'; font-size:" + fontBody.size.ToString() + "pt; margin-top: 3pt; margin-bottom: 3pt}");
            htmlStyles.AppendLine("h1 {font-family: '" + fontHeader.name + "'; font-size:" + fontHeader.size.ToString() + "pt; margin-top: 3pt; margin-bottom: 3pt}");
            htmlStyles.AppendLine("a {text-decoration: none}");
            htmlStyles.AppendLine("a:hover {text-decoration: underline}");
            htmlStyles.AppendLine("a:visited {color: blue}");
            htmlStyles.AppendLine(".Superscript {font-family: '" + fontHtmlSuperscript.name + "'; font-size:" + fontHtmlSuperscript.size.ToString() + "pt; vertical-align: super;}"); // line-height: " + (fontBody.Size + 6).ToString() + "pt;
            htmlStyles.AppendLine(".Small {font-family: '" + fontSmall.name + "'; font-size: " + fontSmall.size.ToString() + "pt; margin-top: 3pt; margin-bottom: 3pt}");
            htmlStyles.AppendLine(".Background {font-family: 'Verdana'; font-size: 8pt; color: silver; margin-top: 3pt; margin-bottom: 3pt}");
            htmlStyles.AppendLine(".Census {font-family: 'Times New Roman'; font-size: 8pt; color: darkcyan; margin-top: 3pt; margin-bottom: 3pt}");
            htmlStyles.AppendLine(".Marriage {font-family: 'Times New Roman'; font-size: 8pt; color: seagreen; margin-top: 3pt; margin-bottom: 3pt}");
            htmlStyles.AppendLine(".Birth {font-family: 'Times New Roman'; font-size: 8pt; color: orangered; margin-top: 3pt; margin-bottom: 3pt}");
            htmlStyles.AppendLine(".Death {font-family: 'Times New Roman'; font-size: 8pt; color: purple; margin-top: 3pt; margin-bottom: 3pt}");
            htmlStyles.AppendLine("--> </style>");

            // Return the style block.
            return htmlStyles.ToString();
        }



        /// <summary>Returns the number of fonts that the object will return in its font array.</summary>
        public int numFonts
        {
            get
            {
                return 6;
            }
        }



        /// <summary>Returns the label for each font in the array of fonts.</summary>
        /// <param name="fontIndex">Specifies the index of the font of interest.</param>
        /// <returns>The label for the specified font.</returns>
        public string getFontLabel(int fontIndex)
        {
            switch (fontIndex)
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



        /// <summary>Returns the font object for the specified index.</summary>
        /// <param name="fontIndex">Specifies the index of the font.</param>
        /// <returns>The font for the specified index.</returns>
        public SimpleFont getFont(int fontIndex)
        {
            switch (fontIndex)
            {
            case 0:
                return fontBase;
            case 1:
                return fontBaseTitle;
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

        /// <summary>The gedcom options.</summary>
        public GedcomOptions gedcomOptions { get { return gedcomOptions_; } }

        #endregion
    }



    #region Supporting Classes, Types etc ...

    /// <summary>Class to represent a font in the user options.</summary>
    /// <remarks>Not able to inherit from Font class.</remarks>
    public class SimpleFont
    {
        #region Member Variables

        /// <summary>Name of the font.</summary>
        public string name;

        /// <summary>Size of the font.</summary>
        public float size;

        /// <summary>The style of the font.  Eg. Bold etc ...</summary>
        public System.Drawing.FontStyle style;

        #endregion

        #region Constructors etc ...

        /// <summary>Class constructor. Creates a copy of the specified simple font object.
        /// </summary>
        /// <param name="font">Specifies the simple font object to copy.</param>
        public SimpleFont(SimpleFont font)
        {
            name = font.name;
            size = font.size;
            style = font.style;
        }



        /// <summary>Class constrctor. Loads the settings from the specified node in a configuration file.  If the settings are not in the configuration file then the specified default settings are used.</summary>
        /// <param name="xmlNode">Specifies the node of the configuration file to load the settings from.</param>
        /// <param name="defaultName">Specifies the default name for the font.</param>
        /// <param name="defaultSize">Specifies the default size for the font.</param>
        public SimpleFont(walton.XmlNode xmlNode, string defaultName, float defaultSize)
        {
            load(xmlNode, defaultName, defaultSize, System.Drawing.FontStyle.Regular);
        }



        /// <summary>Class constrctor. Loads the settings from the specified node in a configuration file.
        /// If the settings are not in the configuration file then the specified default settings are used.
        /// </summary>
        /// <param name="xmlNode">Specifies the node of the configuration file to load the settings from.</param>
        /// <param name="defaultName">Specifies the default name for the font.</param>
        /// <param name="defaultSize">Specifies the default size for the font.</param>
        /// <param name="defaultStyle">Specifies the default style for the font.  Eg Bold etc.</param>
        public SimpleFont(walton.XmlNode xmlNode, string defaultName, float defaultSize, System.Drawing.FontStyle defaultStyle)
        {
            load(xmlNode, defaultName, defaultSize, defaultStyle);
        }



        /// <summary>Load the font settings from the specified node in a configuration file.</summary>
        /// <param name="xmlNode">Specifies the node of the configuration file to load the settings from.</param>
        /// <param name="defaultName">Specifies the default name for the font.</param>
        /// <param name="defaultSize">Specifies the default size for the font.</param>
        /// <param name="defaultStyle">Specifies the default style for the font.  Eg Bold etc.</param>
        public void load(walton.XmlNode xmlNode, string defaultName, float defaultSize, System.Drawing.FontStyle defaultStyle)
        {
            name = xmlNode.getAttributeValue("name", defaultName, true);
            size = xmlNode.getAttributeValue("size", defaultSize, true);
            style = (System.Drawing.FontStyle)xmlNode.getAttributeValue("style", (int)defaultStyle, false);
        }



        /// <summary>Saves the font settings into the specified node in a configuration file.
        /// </summary>
        /// <param name="oNode">Specifies the node of the configuration file to write the settings into.</param>
        public void save(walton.XmlNode oNode)
        {
            oNode.setAttributeValue("name", name);
            oNode.setAttributeValue("size", size);
            oNode.setAttributeValue("style", (int)style);
        }



        /// <summary>Update the simple font object from the specified font object.</summary>
        /// <param name="font">Specifies the font to copy.</param>
        public void copy(System.Drawing.Font font)
        {
            name = font.Name;
            size = font.Size;
            style = font.Style;
        }



        #endregion



        /// <summary>The size of the font as an integer.</summary>
        /// <returns></returns>
        public int fontSize()
        {
            return (int)Math.Round(size);
        }



        /// <summary>Returns the actual font that this object represents.</summary>
        /// <returns></returns>
        public System.Drawing.Font getFont()
        {
            return new System.Drawing.Font(name, size, style);
        }
    }

    #endregion
}
