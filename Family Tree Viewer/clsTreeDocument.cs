using System;
using System.Collections;

using FamilyTree.Objects;

namespace FamilyTree.Viewer
{
	#region Supporting Types etc ...

    // The devices that the tree document can be rendered on.
    /// <summary>
    /// The devices that the tree document can be rendered on.
    /// Makes a difference to the size of fonts.
    /// </summary>
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

    // Class to represent a graphical tree document.
    /// <summary>
    /// Class to represent a graphical tree document.
    /// Device dependant information keep to a minimum.
    /// Devices that I have in mind are screen, printer, metafile.
    /// I don't think the screen zoom factor and (possibly) the fonts should be in this object but they are for now.
	/// </summary>
	public class clsTreeDocument
	{
		#region Member Variables

		/// <summary>Database that this person is attached to.</summary>
		private Database m_oDb;

        /// <summary>The options for this tree.</summary>
        private clsTreeOptions m_oOptions;

		/// <summary>Main person in the tree.  The person that starts the drawing.</summary>
		private clsTreePerson m_oBasePerson;

        /// <summary>The collection of people in this tree.</summary>
        private ArrayList m_oPeople;

        /// <summary>The collection of families in this tree.</summary>
        private ArrayList m_oFamilies;

		/// <summary>Co-ordinates of the top left of the document (minimum).</summary>
		private System.Drawing.PointF m_TopLeft;

		/// <summary>Co-ordinates of the bottom right of the document (maximum).</summary>
		private System.Drawing.PointF m_BottomRight;

		/// <summary>Device for which the positions are calculated.</summary>
		private enumDevice m_CurrentDevice;

		/// <summary>Height of a person.</summary>
		private float m_PersonHeight;

		/// <summary>Vertical offset for the relationship marker.</summary>
		private float m_RelationshipY;

		/// <summary>Horizontal space for the relationship marker.</summary>
		private float m_RelationshipX;

		/// <summary>Main font to write names etc ...</summary>
		private System.Drawing.Font m_oFontName;

		/// <summary>Secondary smaller font to write dates etc ...</summary>
		private System.Drawing.Font m_oFontDescription;

        /// <summary>Brush to paint black.</summary>
        private System.Drawing.Brush m_oBrushBlack;

		/// <summary>Pen to draw thick black lines.</summary>
		private System.Drawing.Pen m_oPenBlackThin;

		/// <summary>Pen to draw bold black lines.</summary>
		private System.Drawing.Pen m_oPenBlackThick;

		/// <summary>Current offset of the top-left of the window.</summary>
		private System.Drawing.PointF m_Offset;

        /// <summary>Brush to paint the background of a boy.</summary>
        private System.Drawing.Brush m_oBrushBoy;

        /// <summary>Brush to paint the background of a girl.</summary>
        private System.Drawing.Brush m_oBrushGirl;

        // The zoom factor to use on the screen display.
        /// <summary>
        /// The zoom factor to use on the screen display.
        /// The printer currently uses a zoom factor of 50%.
        /// </summary>
        private int m_nZoom;

		#endregion

		#region Constructors etc ...

        // Class constructor.
        /// <summary>
        /// Class constructor.
        /// </summary>
		/// <param name="oDb">Specify the database to work from.</param>
		/// <param name="oUserOptions">Specify the user options to initialise the tree with.</param>
		/// <param name="nPersonID">Specify the person to draw a tree for.</param>
        public clsTreeDocument(Database oDb,clsUserOptions oUserOptions,int nPersonID)
        {
            // Save the input parameters
            m_oDb = oDb;
            m_oOptions = new clsTreeOptions(oUserOptions);
            m_nZoom = 100;

            m_oBrushBlack = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
            m_oPenBlackThin = new System.Drawing.Pen(System.Drawing.Color.Black,0);
            m_oPenBlackThick = new System.Drawing.Pen(System.Drawing.Color.Black,2);
            m_oBrushBoy = new System.Drawing.SolidBrush(System.Drawing.Color.LightBlue);
            m_oBrushGirl = new System.Drawing.SolidBrush(System.Drawing.Color.Pink);
            m_CurrentDevice = enumDevice.None;

            // Initialise the tree document
            m_oPeople = new ArrayList();
            m_oFamilies = new ArrayList();
            m_Offset = new System.Drawing.PointF(0,0);
            m_oBasePerson = new clsTreePerson(this,nPersonID);
            m_oPeople.Add(m_oBasePerson);

            // Get the rules for this tree
            clsTreeRule[] oRules = m_oOptions.GetRules();

            // Add the descendants of the specified person
            m_oBasePerson.AddDescendants(oRules);

            // Add the ancestors of the specified person
            m_oBasePerson.AddAncestors(true,oRules);
        }

        // Class constructor for a .tree document.
        /// <summary>
        /// Class constructor for a .tree document.
        /// </summary>
        /// <param name="oDb">Specifies the database to work from.</param>
        /// <param name="oTreeOptions">Specifies the .tree file to load the options from.</param>
        public clsTreeDocument(Database oDb,Innoval.clsXmlDocument oTreeOptions)
        {
            // Save the input parameters
            m_oDb = oDb;
            m_oOptions = new clsTreeOptions(oTreeOptions);
            m_nZoom = 100;

            // Find the base person            
            Innoval.clsXmlNode xmlTree = oTreeOptions.GetNode("tree");
            int nPersonID = xmlTree.GetAttributeValue("mainperson",1,false);

            m_oBrushBlack = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
            m_oPenBlackThin = new System.Drawing.Pen(System.Drawing.Color.Black,0);
            m_oPenBlackThick = new System.Drawing.Pen(System.Drawing.Color.Black,2);
            m_oBrushBoy = new System.Drawing.SolidBrush(System.Drawing.Color.LightBlue);
            m_oBrushGirl = new System.Drawing.SolidBrush(System.Drawing.Color.Pink);
            m_CurrentDevice = enumDevice.None;

            // Initialise the tree document
            m_oPeople = new ArrayList();
            m_oFamilies = new ArrayList();
            m_Offset = new System.Drawing.PointF(0,0);
            m_oBasePerson = new clsTreePerson(this,nPersonID);
            m_oPeople.Add(m_oBasePerson);

            // Get the rules for this tree
            clsTreeRule[] oRules = m_oOptions.GetRules();

            // Add the descendants of the specified person
            m_oBasePerson.AddDescendants(oRules);

            // Add the ancestors of the specified person
            m_oBasePerson.AddAncestors(true,oRules);
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
            m_oFontName = new System.Drawing.Font(m_oOptions.m_sTreeMainFontName, m_oOptions.m_dTreeMainFontSize * ScalingFactor);
            m_oFontDescription = new System.Drawing.Font(m_oOptions.m_sTreeSubFontName, m_oOptions.m_dTreeSubFontSize *ScalingFactor );
        }

        // Regenerate the tree document.
        /// <summary>
        /// Regenerate the tree document.
        /// This would most usually be done after the options have changed.
        /// </summary>
        public void Regenerate()
        {            
            m_oBrushBlack = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
            m_oPenBlackThin = new System.Drawing.Pen(System.Drawing.Color.Black,0);
            m_oPenBlackThick = new System.Drawing.Pen(System.Drawing.Color.Black,2);
            m_oBrushBoy = new System.Drawing.SolidBrush(System.Drawing.Color.LightBlue);
            m_oBrushGirl = new System.Drawing.SolidBrush(System.Drawing.Color.Pink);
            m_CurrentDevice = enumDevice.None;

            // Initialise the tree document
            m_oPeople = new ArrayList();
            m_oFamilies = new ArrayList();
            int nPersonID = m_oBasePerson.PersonID;
            m_Offset = new System.Drawing.PointF(0,0);
            m_oBasePerson = new clsTreePerson(this,nPersonID);
            m_oPeople.Add(m_oBasePerson);

            // Get the rules for this tree
            clsTreeRule[] oRules = m_oOptions.GetRules();

            // Add the descendants of the specified person
            m_oBasePerson.AddDescendants(oRules);

            // Add the ancestors of the specified person
            if(!m_oOptions.IsInRules(clsTreeRule.ERuleAction.ExcludeAncestors ,m_oBasePerson.PersonID ))
            {
            m_oBasePerson.AddAncestors(true,oRules);
            }

            // Force a recalculation of the positions
            m_CurrentDevice = enumDevice.None;
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
            m_oPeople.Add(oPerson);
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
            m_oFamilies.Add(oFamily);
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
            if(m_CurrentDevice == Device)
			{
				// The positions are already calculated
				return true;
			}
            m_CurrentDevice = Device;

            // Rework the fonts
            GenerateFonts(Device);
            
            // Calculate the size of some standard objects
            System.Drawing.SizeF oSize = oGraphics.MeasureString("A",this.FontName);
            m_PersonHeight = oSize.Height;
            m_RelationshipY = oSize.Height / 2;
            oSize = oGraphics.MeasureString("1900",this.FontDescription);
            m_RelationshipX = oSize.Width;
            m_PersonHeight += oSize.Height * 3;

            // Calculate the position of the objects in the document
            m_TopLeft = new System.Drawing.PointF(0,0);
            m_BottomRight = new System.Drawing.PointF(0,0);

            // Reset the position known flag
            foreach(clsTreePerson oPerson in GetPeople())
            {
                oPerson.PositionKnown = false;
            }

            // Loop through the people and position them relative to the people around them                        
            bool bFirstPerson = true;
            foreach(clsTreePerson oPerson in GetPeople())
            {
                // The first person is at the origin
                if(bFirstPerson)
                {
                    bFirstPerson = false;
                    oPerson.SetPosition(oGraphics,0,0);                    
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
            if(dLeft < m_TopLeft.X)
            {
                m_TopLeft.X = dLeft;
            }
            if(dTop < m_TopLeft.Y)
            {
                m_TopLeft.Y = dTop;
            }
            if(dRight > m_BottomRight.X)
            {
                m_BottomRight.X = dRight;
            }
            if(dBottom > m_BottomRight.Y)
            {
                m_BottomRight.Y = dBottom;
            }
        }

		#endregion
		
		#region Drawing

        // Draw the tree onto the specified graphic object.
        /// <summary>
        /// Draw the tree onto the specified graphic object.
        /// 
		/// Probably will end up drawing slightly different things for screen, printer and metafile.
		/// </summary>
		/// <param name="oGraphics">Specify the graphic to draw the tree on</param>
		/// <returns>True for success, false otherwise</returns>
        public bool Draw(System.Drawing.Graphics oGraphics)
        {
            clsTreeConnection[] oFamilies = GetFamilies();
            foreach(clsTreeConnection oFamily in oFamilies)
            {
                oFamily.Draw(oGraphics);
            }

            clsTreePerson[] oPeople = GetPeople();
            foreach(clsTreePerson oPerson in oPeople)
            {
                oPerson.Draw(oGraphics);
            }

            return true; //  m_oBasePerson.Draw(oGraphics,enumTreeDirection.Both);
        }

		#endregion

		#region Properties

        // The database that this tree is attached to.
        /// <summary>
        /// The database that this tree is attached to.
        /// </summary>
		public Database Database
        {
            get
            {
                return m_oDb;
            }
        }

        // The options specified for this tree document.
        /// <summary>
        /// The options specified for this tree document.
        /// </summary>
        public clsTreeOptions Options
        {
            get
            {
                return m_oOptions;
            }
        }

		/// <summary> The ID of the person that started this tree.</summary>
		public int BasePersonID { get { return m_oBasePerson.PersonID; } }		

		/// <summary>True to draw boxes around people in this tree.</summary>
        public bool PersonBox { get { return m_oOptions.m_bTreePersonBox; } }
		
		/// <summary>The last device that the tree was drawn on.  If the current device is different then the width need recalulating for the current device.</summary>
		public enumDevice LastDevice { get { return m_CurrentDevice; } }

		/// <summary>The co-ordinates of the top-left corner of the tree.</summary>
		public System.Drawing.PointF TopLeft { get { return m_TopLeft; } }

		/// <summary>The co-ordinates of the bottom-right corner of the tree.</summary>
		public System.Drawing.PointF BottomRight { get { return m_BottomRight; } }

        // The width of the tree.
        /// <summary>
        /// The width of the tree.
        /// </summary>
		public float Width
        {
            get 
            {
                return m_BottomRight.X-m_TopLeft.X;
            }
        }

        // The height of the tree.
        /// <summary>
        /// The height of the tree.
        /// </summary>
		public float Height
        {
            get
            {
                return m_BottomRight.Y-m_TopLeft.Y;
            }
        }
	
		#region Object Spacing Properties
		
		/// <summary>NOT SURE!  Half the width of the symbol used to mark a relationship.</summary>
		public float RelationshipSymbol { get { return 6; } }
				
		/// <summary>The height of box around a person.</summary>
		public float spcPersonHeight { get { return m_PersonHeight; } }

		/// <summary>The vertical space between generations of people.</summary>
		public float spcPersonY { get { return m_PersonHeight + spcChildDropLine + spcParentDropLine; } }
		
		/// <summary>The vertical length of the line down from the horizontal bar to the child.</summary>
		public float spcChildDropLine { get { return 12; } }

		/// <summary>The vertical length of the line down from a parent to the horizontal bar.  Usually the line is much longer than this because it starts at the relationship marker.</summary>
		public float spcParentDropLine { get { return 12; } }

		/// <summary>The position below the parent that the horizontal bar to hang children appears.</summary>
		public float spcHorizontalBarPos { get { return m_PersonHeight + spcParentDropLine; } }		
		
		/// <summary>The horizontal space between siblings.</summary>
		public float spcSiblingSpace { get { return 5; } }

		/// <summary>The vertical offset for the relationship marker.</summary>
		public float spcRelationsMarker { get { return m_RelationshipY; } }

		/// <summary>The horizontal space between people in a relationship.</summary>
		public float spcRelationshipSpace { get { return m_RelationshipX; } }

		#endregion
		
		/// <summary>The main larger font.</summary>
		public System.Drawing.Font FontName { get { return m_oFontName; } }

		/// <summary>The secondary smaller font.</summary>
		public System.Drawing.Font FontDescription { get { return m_oFontDescription; } }

        /// <summary>A black brush.</summary>
        public System.Drawing.Brush BrushBlack { get { return m_oBrushBlack; } }

        /// <summary>A brush for the background of a boy.</summary>
        public System.Drawing.Brush BrushBoy { get { return m_oBrushBoy; } }

        /// <summary>A brush for the background of a boy.</summary>
        public System.Drawing.Brush BrushGirl { get { return m_oBrushGirl; } }

		/// <summary>A pen for a thin line.</summary>
		public System.Drawing.Pen PenBlackThin { get { return m_oPenBlackThin; } }

		/// <summary>A pen for a thick line.</summary>
		public System.Drawing.Pen PenBlackThick { get { return m_oPenBlackThick; } }

		/// <summary>Current horizontal offset for the window.</summary>
		public float OffsetX { get { return m_Offset.X; } set { m_Offset.X = value; } }

		/// <summary>Current vertical offset for the window.</summary>
		public float OffsetY { get { return m_Offset.Y; } set { m_Offset.Y = value; } }

        // Returns the collection of clsTreePeople objects in this tree document.
        /// <summary>
        /// Returns the collection of clsTreePeople objects in this tree document.
        /// </summary>
        /// <returns>An array of people who are in this tree document.</returns>
        public clsTreePerson[] GetPeople()
        {
            return (clsTreePerson[])m_oPeople.ToArray(typeof(clsTreePerson));
        }

        // Returns the collection of families in this tree document.
        /// <summary>
        /// Returns the collection of families in this tree document.
        /// </summary>
        /// <returns>An array of the families in this tree document.</returns>
        public clsTreeConnection[] GetFamilies()
        {
            return (clsTreeConnection[]) m_oFamilies.ToArray(typeof(clsTreeConnection));
        }

        // The zoom factor for the screen display in percent.
        /// <summary>
        /// The zoom factor for the screen display in percent.
        /// </summary>
        public int ScreenZoom
        {
            get
            {
                return m_nZoom;
            }
            set
            {
                m_nZoom = value;
            }
        }

        // The scaling factor for the current device.
        /// <summary>
        /// The scaling factor for the current device.
        /// Units that are not scaled against the font size should apply this factor.
        /// </summary>
        public float ScalingFactor
        {
            get
            {
                switch(m_CurrentDevice)
                {
                case enumDevice.Screen:
                    return ((float)m_nZoom) / 100f;

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
