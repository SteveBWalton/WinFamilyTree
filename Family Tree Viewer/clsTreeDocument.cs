using System;
using System.Collections;

using FamilyTree.Objects;

namespace FamilyTree.Viewer
{
    #region Supporting Types etc ...

    /// <summary>The devices that the tree document can be rendered on.  Makes a difference to the size of fonts.</summary>
    public enum enumDevice
    {
        /// <summary>Try to be device independent.</summary>
        None,
        /// <summary>The tree is been drawn directly onto a window.</summary>
        Screen,
        /// <summary>The tree is been draw directly onto the printer.</summary>
        Printer,
        /// <summary>The tree is been draw into a metafile.  The metafile will proably be used on the screen.</summary>
        Metafile
    }

    #endregion

    /// <summary>Class to represent a graphical tree document.  Device dependant information keep to a minimum.  Devices that I have in mind are screen, printer, metafile.  I don't think the screen zoom factor and (possibly) the fonts should be in this object but they are for now.
    /// </summary>
    public class clsTreeDocument
    {
        #region Member Variables

        /// <summary>Database that this person is attached to.</summary>
        private Database database_;

        /// <summary>The options for this tree.</summary>
        private clsTreeOptions treeOptions_;

        /// <summary>Main person in the tree.  The person that starts the drawing.</summary>
        private clsTreePerson basePerson_;

        /// <summary>The collection of people in this tree.</summary>
        private ArrayList people_;

        /// <summary>The collection of families in this tree.</summary>
        private ArrayList families_;

        /// <summary>Co-ordinates of the top left of the document (minimum).</summary>
        private System.Drawing.PointF topLeft_;

        /// <summary>Co-ordinates of the bottom right of the document (maximum).</summary>
        private System.Drawing.PointF bottomRight_;

        /// <summary>Device for which the positions are calculated.</summary>
        private enumDevice currentDevice_;

        /// <summary>Height of a person.</summary>
        private float personHeight_;

        /// <summary>Vertical offset for the relationship marker.</summary>
        private float relationshipY_;

        /// <summary>Horizontal space for the relationship marker.</summary>
        private float relationshipX_;

        /// <summary>Main font to write names etc ...</summary>
        private System.Drawing.Font fontName_;

        /// <summary>Secondary smaller font to write dates etc ...</summary>
        private System.Drawing.Font fontDescription_;

        /// <summary>Brush to paint black.</summary>
        private System.Drawing.Brush brushBlack_;

        /// <summary>Pen to draw thick black lines.</summary>
        private System.Drawing.Pen penBlackThin_;

        /// <summary>Pen to draw bold black lines.</summary>
        private System.Drawing.Pen penBlackThick_;

        /// <summary>Current offset of the top-left of the window.</summary>
        private System.Drawing.PointF offset;

        /// <summary>Brush to paint the background of a boy.</summary>
        private System.Drawing.Brush brushBoy_;

        /// <summary>Brush to paint the background of a girl.</summary>
        private System.Drawing.Brush brushGirl_;

        /// <summary>The zoom factor to use on the screen display.  The printer currently uses a zoom factor of 50%.</summary>
        private int zoom_;

        #endregion

        #region Constructors etc ...

        /// <summary>Class constructor.</summary>
        /// <param name="database">Specify the database to work from.</param>
        /// <param name="userOptions">Specify the user options to initialise the tree with.</param>
        /// <param name="personIndex">Specify the person to draw a tree for.</param>
        public clsTreeDocument(Database database, UserOptions userOptions, int personIndex)
        {
            // Save the input parameters.
            database_ = database;
            treeOptions_ = new clsTreeOptions(userOptions);
            zoom_ = 100;

            brushBlack_ = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
            penBlackThin_ = new System.Drawing.Pen(System.Drawing.Color.Black, 0);
            penBlackThick_ = new System.Drawing.Pen(System.Drawing.Color.Black, 2);
            brushBoy_ = new System.Drawing.SolidBrush(System.Drawing.Color.LightBlue);
            brushGirl_ = new System.Drawing.SolidBrush(System.Drawing.Color.Pink);
            currentDevice_ = enumDevice.None;

            // Initialise the tree document.
            people_ = new ArrayList();
            families_ = new ArrayList();
            offset = new System.Drawing.PointF(0, 0);
            basePerson_ = new clsTreePerson(this, personIndex);
            people_.Add(basePerson_);

            // Get the rules for this tree.
            clsTreeRule[] rules = treeOptions_.getRules();

            // Add the descendants of the specified person.
            basePerson_.addDescendants(rules);

            // Add the ancestors of the specified person.
            basePerson_.AddAncestors(true, rules);
        }



        /// <summary>Class constructor for a .tree document.</summary>
        /// <param name="oDb">Specifies the database to work from.</param>
        /// <param name="oTreeOptions">Specifies the .tree file to load the options from.</param>
        public clsTreeDocument(Database oDb, walton.XmlDocument oTreeOptions)
        {
            // Save the input parameters
            database_ = oDb;
            treeOptions_ = new clsTreeOptions(oTreeOptions);
            zoom_ = 100;

            // Find the base person            
            walton.XmlNode xmlTree = oTreeOptions.getNode("tree");
            int nPersonID = xmlTree.getAttributeValue("mainperson", 1, false);

            brushBlack_ = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
            penBlackThin_ = new System.Drawing.Pen(System.Drawing.Color.Black, 0);
            penBlackThick_ = new System.Drawing.Pen(System.Drawing.Color.Black, 2);
            brushBoy_ = new System.Drawing.SolidBrush(System.Drawing.Color.LightBlue);
            brushGirl_ = new System.Drawing.SolidBrush(System.Drawing.Color.Pink);
            currentDevice_ = enumDevice.None;

            // Initialise the tree document
            people_ = new ArrayList();
            families_ = new ArrayList();
            offset = new System.Drawing.PointF(0, 0);
            basePerson_ = new clsTreePerson(this, nPersonID);
            people_.Add(basePerson_);

            // Get the rules for this tree
            clsTreeRule[] oRules = treeOptions_.getRules();

            // Add the descendants of the specified person
            basePerson_.addDescendants(oRules);

            // Add the ancestors of the specified person
            basePerson_.AddAncestors(true, oRules);
        }

        // Create the fonts that will be used on this device.
        /// <summary>
        /// Create the fonts that will be used on this device.
        /// This allows for different fonts on different devices.
        /// </summary>
        /// <param name="Device"></param>
        public void GenerateFonts(enumDevice Device)
        {
            /*
            if(Device == enumDevice.Printer)
            {
                m_oFontName = new System.Drawing.Font(m_oOptions.m_sTreeMainFontName,m_oOptions.m_dTreeMainFontSize / 2);
                m_oFontDescription = new System.Drawing.Font(m_oOptions.m_sTreeSubFontName,m_oOptions.m_dTreeSubFontSize / 2);
            }
            else
            {
                m_oFontName = new System.Drawing.Font(m_oOptions.m_sTreeMainFontName,m_oOptions.m_dTreeMainFontSize);
                m_oFontDescription = new System.Drawing.Font(m_oOptions.m_sTreeSubFontName,m_oOptions.m_dTreeSubFontSize);
            }
             */

            // The device is now handled by the scaling factor
            fontName_ = new System.Drawing.Font(treeOptions_.mainFontName_, treeOptions_.mainFontSize_ * scalingFactor);
            fontDescription_ = new System.Drawing.Font(treeOptions_.subFontName_, treeOptions_.subFontSize_ * scalingFactor);
        }

        // Regenerate the tree document.
        /// <summary>
        /// Regenerate the tree document.
        /// This would most usually be done after the options have changed.
        /// </summary>
        public void Regenerate()
        {
            brushBlack_ = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
            penBlackThin_ = new System.Drawing.Pen(System.Drawing.Color.Black, 0);
            penBlackThick_ = new System.Drawing.Pen(System.Drawing.Color.Black, 2);
            brushBoy_ = new System.Drawing.SolidBrush(System.Drawing.Color.LightBlue);
            brushGirl_ = new System.Drawing.SolidBrush(System.Drawing.Color.Pink);
            currentDevice_ = enumDevice.None;

            // Initialise the tree document
            people_ = new ArrayList();
            families_ = new ArrayList();
            int nPersonID = basePerson_.PersonID;
            offset = new System.Drawing.PointF(0, 0);
            basePerson_ = new clsTreePerson(this, nPersonID);
            people_.Add(basePerson_);

            // Get the rules for this tree
            clsTreeRule[] oRules = treeOptions_.getRules();

            // Add the descendants of the specified person
            basePerson_.addDescendants(oRules);

            // Add the ancestors of the specified person
            if (!treeOptions_.isInRules(clsTreeRule.RuleAction.EXCLUDE_ANCESTORS, basePerson_.PersonID))
            {
                basePerson_.AddAncestors(true, oRules);
            }

            // Force a recalculation of the positions
            currentDevice_ = enumDevice.None;
        }

        #endregion

        #region Building

        /// <summary>
        /// Add the person to the collection of people in this tree.
        /// </summary>
        /// <param name="oPerson">Specifies the person to add to the tree.</param>
        public void AddPerson
            (
            clsTreePerson oPerson
            )
        {
            people_.Add(oPerson);
        }

        /// <summary>
        /// Add a family to the collection of families in this tree.
        /// </summary>
        /// <param name="oFamily">Specifies the the family to add to the tree.</param>
        public void AddFamily
            (
            clsTreeConnection oFamily
            )
        {
            families_.Add(oFamily);
        }

        #endregion

        #region Position Calculations

        /// <summary>
		/// Calculate the positions for all the objects in the document given the specified graphics object (device).
		/// The tree can be drawn many times on the device once the positions have been calculated.  But the positions
		/// need to be recalculated for a different device.
		/// </summary>
		/// <param name="oGraphics">Specify the graphic object (device) that the tree will be drawn on</param>
		/// <param name="Device">Specify the device that we calculating the positions for</param>
		/// <returns>True for success, false otherwise.</returns>
		public bool CalculatePositions
            (
            System.Drawing.Graphics oGraphics,
            enumDevice Device
            )
        {
            // Check if the position have already been calculated for this device
            if (currentDevice_ == Device)
            {
                // The positions are already calculated
                return true;
            }
            currentDevice_ = Device;

            // Rework the fonts
            GenerateFonts(Device);

            // Calculate the size of some standard objects
            System.Drawing.SizeF oSize = oGraphics.MeasureString("A", this.fontName);
            personHeight_ = oSize.Height;
            relationshipY_ = oSize.Height / 2;
            oSize = oGraphics.MeasureString("1900", this.fontDescription);
            relationshipX_ = oSize.Width;
            personHeight_ += oSize.Height * 3;

            // Calculate the position of the objects in the document
            topLeft_ = new System.Drawing.PointF(0, 0);
            bottomRight_ = new System.Drawing.PointF(0, 0);

            // Reset the position known flag
            foreach (clsTreePerson oPerson in getPeople())
            {
                oPerson.PositionKnown = false;
            }

            // Loop through the people and position them relative to the people around them                        
            bool bFirstPerson = true;
            foreach (clsTreePerson oPerson in getPeople())
            {
                // The first person is at the origin
                if (bFirstPerson)
                {
                    bFirstPerson = false;
                    oPerson.SetPosition(oGraphics, 0, 0);
                }

                // Fix the relations of this person
                oPerson.CalculatePosition(oGraphics);
            }

            // Return success
            return true;
        }

        // Notify the tree of the position of an object.
        /// <summary>
        /// Notify the tree of the position of an object.
        /// This is so the tree can know the extent of itself
		/// </summary>
		/// <param name="dLeft">Specifies the left position of the object.</param>
		/// <param name="dTop">Specifies the top position of the object.</param>
		/// <param name="dRight">Specifies the right position of the object.</param>
		/// <param name="dBottom">Specifies the bottom position of the object.</param>
        public void NotifyPosition(float dLeft, float dTop, float dRight, float dBottom)
        {
            if (dLeft < topLeft_.X)
            {
                topLeft_.X = dLeft;
            }
            if (dTop < topLeft_.Y)
            {
                topLeft_.Y = dTop;
            }
            if (dRight > bottomRight_.X)
            {
                bottomRight_.X = dRight;
            }
            if (dBottom > bottomRight_.Y)
            {
                bottomRight_.Y = dBottom;
            }
        }

        #endregion

        #region Drawing



        /// <summary>
        /// Draw the tree onto the specified graphic object.
        /// 
        /// Probably will end up drawing slightly different things for screen, printer and metafile.
        /// </summary>
        /// <param name="graphics">Specify the graphic to draw the tree on</param>
        /// <returns>True for success, false otherwise</returns>
        public bool draw(System.Drawing.Graphics graphics)
        {
            clsTreeConnection[] families = getFamilies();
            foreach (clsTreeConnection family in families)
            {
                family.draw(graphics);
            }

            clsTreePerson[] people = getPeople();
            foreach (clsTreePerson person in people)
            {
                person.Draw(graphics);
            }

            return true; //  m_oBasePerson.Draw(oGraphics,enumTreeDirection.Both);
        }



        #endregion

        #region Properties

        /// <summary>The database that this tree is attached to.</summary>
        public Database database { get { return database_; } }

        /// <summary>The options specified for this tree document.</summary>
        public clsTreeOptions options { get { return treeOptions_; } }

        /// <summary> The ID of the person that started this tree.</summary>
        public int basePersonIndex { get { return basePerson_.PersonID; } }

        /// <summary>True to draw boxes around people in this tree.</summary>
        public bool isPersonBox { get { return treeOptions_.isTreePersonBox_; } }

        /// <summary>The last device that the tree was drawn on.  If the current device is different then the width need recalulating for the current device.</summary>
        public enumDevice lastDevice { get { return currentDevice_; } }

        /// <summary>The co-ordinates of the top-left corner of the tree.</summary>
        public System.Drawing.PointF topLeft { get { return topLeft_; } }

        /// <summary>The co-ordinates of the bottom-right corner of the tree.</summary>
        public System.Drawing.PointF bottomRight { get { return bottomRight_; } }

        /// <summary>The width of the tree.</summary>
        public float width { get { return bottomRight_.X - topLeft_.X; } }

        /// <summary>The height of the tree.</summary>
        public float height { get { return bottomRight_.Y - topLeft_.Y; } }

        #region Object Spacing Properties

        /// <summary>NOT SURE!  Half the width of the symbol used to mark a relationship.</summary>
        public float RelationshipSymbol { get { return 6; } }

        /// <summary>The height of box around a person.</summary>
        public float spcPersonHeight { get { return personHeight_; } }

        /// <summary>The vertical space between generations of people.</summary>
        public float spcPersonY { get { return personHeight_ + spcChildDropLine + spcParentDropLine; } }

        /// <summary>The vertical length of the line down from the horizontal bar to the child.</summary>
        public float spcChildDropLine { get { return 12; } }

        /// <summary>The vertical length of the line down from a parent to the horizontal bar.  Usually the line is much longer than this because it starts at the relationship marker.</summary>
        public float spcParentDropLine { get { return 12; } }

        /// <summary>The position below the parent that the horizontal bar to hang children appears.</summary>
        public float spcHorizontalBarPos { get { return personHeight_ + spcParentDropLine; } }

        /// <summary>The horizontal space between siblings.</summary>
        public float spcSiblingSpace { get { return 5; } }

        /// <summary>The vertical offset for the relationship marker.</summary>
        public float spcRelationsMarker { get { return relationshipY_; } }

        /// <summary>The horizontal space between people in a relationship.</summary>
        public float spcRelationshipSpace { get { return relationshipX_; } }

        #endregion

        /// <summary>The main larger font.</summary>
        public System.Drawing.Font fontName { get { return fontName_; } }

        /// <summary>The secondary smaller font.</summary>
        public System.Drawing.Font fontDescription { get { return fontDescription_; } }

        /// <summary>A black brush.</summary>
        public System.Drawing.Brush brushBlack { get { return brushBlack_; } }

        /// <summary>A brush for the background of a boy.</summary>
        public System.Drawing.Brush brushBoy { get { return brushBoy_; } }

        /// <summary>A brush for the background of a boy.</summary>
        public System.Drawing.Brush brushGirl { get { return brushGirl_; } }

        /// <summary>A pen for a thin line.</summary>
        public System.Drawing.Pen penBlackThin { get { return penBlackThin_; } }

        /// <summary>A pen for a thick line.</summary>
        public System.Drawing.Pen penBlackThick { get { return penBlackThick_; } }

        /// <summary>Current horizontal offset for the window.</summary>
        public float offsetX { get { return offset.X; } set { offset.X = value; } }

        /// <summary>Current vertical offset for the window.</summary>
        public float offsetY { get { return offset.Y; } set { offset.Y = value; } }

        /// <summary>Returns the collection of clsTreePeople objects in this tree document.</summary>
        /// <returns>An array of people who are in this tree document.</returns>
        public clsTreePerson[] getPeople()
        {
            return (clsTreePerson[])people_.ToArray(typeof(clsTreePerson));
        }

        /// <summary>Returns the collection of families in this tree document.</summary>
        /// <returns>An array of the families in this tree document.</returns>
        public clsTreeConnection[] getFamilies()
        {
            return (clsTreeConnection[])families_.ToArray(typeof(clsTreeConnection));
        }

        /// <summary>The zoom factor for the screen display in percent.</summary>
        public int screenZoom { get { return zoom_; } set { zoom_ = value; } }

        /// <summary>The scaling factor for the current device.  Units that are not scaled against the font size should apply this factor.</summary>
        public float scalingFactor
        {
            get
            {
                switch (currentDevice_)
                {
                case enumDevice.Screen:
                    return ((float)zoom_) / 100f;

                case enumDevice.Printer:
                    //return 0.43f;
                    return 0.5f;

                default:
                    return 1f;
                }
            }
        }

        #endregion

    }
}
