using System;
using System.Collections;
using FamilyTree.Objects;

namespace FamilyTree.Viewer
{
	#region Supporting Types etc

	/// <summary>
	/// Reason this connection has come into the tree.
	/// </summary>
	public enum enumConMainPerson
	{
		/// <summary>Main person is the father.</summary>
		Father,
		/// <summary>Main person is the mother.</summary>
		Mother,
		/// <summary>Main person is child[0] he must be shown on the left.</summary>
		ChildBoy,
		/// <summary>Main person is child[0] she must be shown on the right.</summary>
		ChildGirl,
		/// <summary>Main person is child[0] he/she can be shown sysmetrically.</summary>
		Child
	}		

	#endregion
	
	/// <summary>
	/// Class to represent the connection between clsTreePerson objects in a clsTreeDocument.
	/// This will enable the connected people related to the main person to be drawn.
	/// This should take care NOT to draw the main person will be drawn already.
	/// </summary>
    public class clsTreeConnection
    {
        #region Member Variables

        /// <summary>Tree document that this contains this connection.</summary>
        private clsTreeDocument m_oTree;

        /// <summary>The reason this connection was created.</summary>
        private enumConMainPerson m_MainPersonType;

        /// <summary>The father in this connection.</summary>
        private clsTreePerson m_oFather;

        /// <summary>The mother in this connection.</summary>
        private clsTreePerson m_oMother;

        /// <summary>The children in this connection.</summary>
        private ArrayList m_oChildren;

        /// <summary>Position of the relationship marker.</summary>
        private System.Drawing.PointF m_PosRelationship;

        /// <summary>Height of the child bar.</summary>
        private float m_dChildBarHeight;

        /// <summary>Status of the relationship.</summary>
        private enumRelationshipStatus m_nStatus;

        /// <summary>Is the main person male.</summary>
        private bool m_bMale;

        /// <summary>Date of the start relationship.</summary>
        private CompoundDate m_dtStart;

        /// <summary>Index of the relationship in the context of the main person.  Ie is this a second marriage?</summary>
        private int m_nIndex;

        #endregion

        #region Constructors etc...

        /// <summary>
        /// Class constructor.
        /// Creates a new tree connection object.
        /// </summary>
        /// <param name="oTree">Specifies the tree document that contains this connection.</param>
        /// <param name="oPerson">Specifies the main person in this connection.</param>
        /// <param name="nPersonType">Specifies the role of the main person in this connection.</param>
        /// <param name="nIndex">Specify the count of this relationship in context of the main person.  Usually 0.</param>
        public clsTreeConnection
            (
            clsTreeDocument oTree,
            clsTreePerson oPerson,
            enumConMainPerson nPersonType,
            int nIndex
            )
        {
            // Save the input parametrs
            m_oTree = oTree;
            m_MainPersonType = nPersonType;
            m_nIndex = nIndex;

            // Initialise the object
            m_PosRelationship = new System.Drawing.PointF(0,0);
            m_nStatus = enumRelationshipStatus.Married;
            m_dtStart = null;

            switch(m_MainPersonType)
            {
            case enumConMainPerson.Father:
                m_oFather = oPerson;
                m_oMother = null;
                m_oChildren = null;
                m_bMale = true;
                break;

            case enumConMainPerson.Mother:
                m_oFather = null;
                m_oMother = oPerson;
                m_oChildren = null;
                m_bMale = false;
                break;

            case enumConMainPerson.ChildBoy:
            case enumConMainPerson.ChildGirl:
            case enumConMainPerson.Child:
                m_oFather = null;
                m_oMother = null;
                AddChild(oPerson);
                switch(m_MainPersonType)
                {
                case enumConMainPerson.ChildBoy:
                    m_bMale = true;
                    break;

                case enumConMainPerson.ChildGirl:
                    m_bMale = false;
                    break;

                case enumConMainPerson.Child:
                    Person oMainPerson = new Person(oPerson.PersonID,m_oTree.Database);
                    m_bMale = oMainPerson.isMale;
                    break;

                }
                break;
            }
        }

        #endregion

        #region Values / Adding Members

        /// <summary>
        /// Specifies the father in this connection object
        /// </summary>
        /// <param name="oFather">Specifies the father object.</param>
        /// <returns>True for success, false otherwise.</returns>
        public bool AddFather
            (
            clsTreePerson oFather
            )
        {
            m_oFather = oFather; // new clsTreePerson(m_oTree,nFatherID);

            // Return success
            return true;
        }

        /// <summary>
        /// Specifies the mother in this connection object
        /// </summary>
        /// <param name="oMother">Specifies the mother object.</param>
        /// <returns>True for success, false otherwise.</returns>
        public bool AddMother
            (
            clsTreePerson oMother
            )
        {
            m_oMother = oMother;			// new clsTreePerson(m_oTree,nMotherID);

            // Return success
            return true;
        }

        /// <summary>
        /// Adds a child this tree connection object.
        /// </summary>
        /// <param name="oChild">Specifies the child object.</param>
        /// <returns>True for success, false otherwise.</returns>
        public bool AddChild
            (
            clsTreePerson oChild
            )
        {
            if(m_oChildren == null)
            {
                m_oChildren = new ArrayList();
            }
            m_oChildren.Add(oChild);

            // Return success
            return true;
        }

        /// <summary>
        /// Returns the ID of the father in this connection.  Returns 0 for no father.
        /// </summary>
        /// <returns></returns>
        public int GetFatherID()
        {
            if(m_oFather == null)
            {
                return 0;
            }
            return m_oFather.PersonID;
        }

        /// <summary>
        /// Returns the children in this connection as an array of clsTreePerson objects.
        /// </summary>
        /// <returns></returns>
        public clsTreePerson[] GetChildren()
        {
            if(m_oChildren == null)
            {
                return new clsTreePerson[0];
            }
            return (clsTreePerson[])m_oChildren.ToArray(typeof(clsTreePerson));
        }

        /// <summary>
        /// Returns the ID of the mother in this connection.  Returns 0 for no mother.
        /// </summary>
        /// <returns></returns>
        public int GetMotherID()
        {
            if(m_oMother == null)
            {
                return 0;
            }
            return m_oMother.PersonID;
        }

        /// <summary>Returns the father in this connection.</summary>
        public clsTreePerson Father { get { return m_oFather; } }

        /// <summary>Returns the mother in this connection.</summary>
        public clsTreePerson Mother { get { return m_oMother; } }

        /// <summary>Returns the major person in this connection.  Cause of the connection existance.</summary>
        private clsTreePerson MainPerson { get { return (clsTreePerson)m_oChildren[0]; } }

        /// <summary>Returns the status of this connection.</summary>
        public enumRelationshipStatus Status { get { return m_nStatus; } set { m_nStatus = value; } }

        /// <summary>Returns the start date of this connection.</summary>
        public CompoundDate Start { get { return m_dtStart; } set { m_dtStart = value; } }

        #endregion

        #region Width Calculations

        /// <summary>
        /// Calculate the width of this connection without knowing the position of any of the members
        /// </summary>
        /// <param name="oGraphics">Specify the graphic to draw on</param>
        /// <returns>The width required for this connection</returns>
        public float GetWidth
            (
            System.Drawing.Graphics oGraphics
            )
        {
            // Calculate the width of the 2 parents
            float dMaxWidth = GetWidthParents(oGraphics);

            // Calcualte the width of the children			
            float dWidth = GetWidthChildren(oGraphics);

            // If the children are wider than the parents then use the children width
            if(dWidth > dMaxWidth)
            {
                dMaxWidth = dWidth;
            }

            // Return the calculated width
            return dMaxWidth;
        }

        /// <summary>
        /// Calculate the width required for the parents in this connection without knowing the position of any of the members.
        /// </summary>
        /// <param name="oGraphics">Specify the graphic to draw on.</param>
        /// <returns>The width required for the children in this connection.</returns>
        public float GetWidthParents
            (
            System.Drawing.Graphics oGraphics
            )
        {
            float dWidth = 0;
            if(m_oFather != null)
            {
                dWidth += m_oFather.GetWidth(oGraphics,false,false);
                if(m_oMother != null)
                {
                    dWidth += m_oTree.spcRelationshipSpace;
                }
            }
            if(m_oMother != null)
            {
                dWidth += m_oMother.GetWidth(oGraphics,false,false);
            }

            // Return the calculated width
            return dWidth;
        }
        
        /// <summary>
        /// Calculate the width required for the children in this connection without knowing the position of any of the members.
        /// </summary>
        /// <param name="oGraphics">Specify the graphic to draw on.</param>
        /// <returns>The width required for the children in this connection.</returns>
        public float GetWidthChildren
            (
            System.Drawing.Graphics oGraphics
            )
        {
            float dWidth = 0;
            
            // Calcualte the width of the children			
            if(m_oChildren != null)
            {
                clsTreePerson[] oChildren = GetChildren();
                foreach(clsTreePerson oChild in oChildren)
                {
                    dWidth += oChild.GetWidth(oGraphics,true,false) + m_oTree.spcSiblingSpace;
                }
            }

            // Return the calculated width of the children
            return dWidth;
        }


        #endregion

        #region Position Calculations

        /// <summary>
        /// Returns the X position that the spouse in this relations takes.
        /// This accounts for the sex of the spouse (males are positioned on the left, females are postioned on the right).
        /// This assumes that the main person is the one of the parents.
        /// </summary>
        /// <param name="oGraphics">Specifies the graphics device to draw on.</param>
        /// <returns>Returns the X position of the spouse in this connection object.</returns>
        public float SpousePosition
            (
            System.Drawing.Graphics oGraphics
            )
        {
            switch(m_MainPersonType)
            {
            case enumConMainPerson.Father:
                return m_oMother.X + m_oMother.GetWidth(oGraphics,false,false);

            case enumConMainPerson.Mother:
                return m_oFather.X;
            }

            // This is an error

            return 0;
        }

        /// <summary>
        /// Returns the position that the next child in this connection (or attached to the main person) should take.
        /// </summary>
        /// <param name="oGraphics">Specify the device to draw on</param>
        /// <returns>The next available position for a child of the main person</returns>
        public float NextChild
            (
            System.Drawing.Graphics oGraphics
            )
        {
            // If no children then the position of the father / mother
            if(m_oChildren == null)
            {
                if(m_oFather != null)
                {
                    return m_oFather.X;
                }
                return m_oMother.X;
            }
            clsTreePerson oLastChild = (clsTreePerson)m_oChildren[m_oChildren.Count - 1];
            return oLastChild.X + oLastChild.GetWidth(oGraphics,true,false);
        }

        /// <summary>
        /// Calculates and sets the positon of the partner of a person who is already fixed.
        /// The partner is positioned only if they are not already positioned.
        /// </summary>
        /// <param name="oGraphics">Specify the graphics device to draw on.</param>
        /// <param name="nIndex">Specify the index of the partner.</param>
        /// <returns>True for success, false otherwise.</returns>		
        public bool SetPartnerPosition
            (
            System.Drawing.Graphics oGraphics,
            int nIndex
            )
        {
            switch(m_MainPersonType)
            {
            case enumConMainPerson.Father:
                // Position the mother relative to the father				
                m_PosRelationship.Y = m_oFather.Y;
                if(m_oMother != null)
                {
                    if(!m_oMother.PositionKnown)
                    {
                        m_PosRelationship.X = m_oFather.GetSpousePosition(oGraphics,nIndex) + m_oTree.spcRelationshipSpace / 2;

                        m_oMother.SetPosition(oGraphics,m_PosRelationship.X + m_oTree.spcRelationshipSpace / 2,m_oFather.Y);
                    }
                }
                else
                {
                    m_PosRelationship.X = m_oFather.X + m_oFather.GetWidth(oGraphics,false,false) / 2;
                }
                break;

            case enumConMainPerson.Mother:
                // Position the father realive to the mother				
                m_PosRelationship.Y = m_oMother.Y;
                if(m_oFather != null)
                {
                    if(!m_oFather.PositionKnown)
                    {
                        m_PosRelationship.X = m_oMother.GetSpousePosition(oGraphics,nIndex) - m_oTree.spcRelationshipSpace / 2;
                        m_oFather.SetPosition(oGraphics,m_PosRelationship.X - m_oTree.spcRelationshipSpace / 2 - m_oFather.GetWidth(oGraphics,false,false),m_oMother.Y);
                    }
                }
                else
                {
                    m_PosRelationship.X = m_oMother.X + m_oMother.GetWidth(oGraphics,false,false) / 2;
                }
                // CalculateChildPositionFixedParents(oGraphics,nIndex,ref dNextChild);
                break;

            case enumConMainPerson.ChildBoy:
            case enumConMainPerson.ChildGirl:
            case enumConMainPerson.Child:
                // This is an error
                throw (new System.Exception("Children not allowed here"));
            }

            // Return success
            return true;
        }

        /// <summary>
        /// Calculate and sets the positons of the children of a person who already fixed.
        /// The children are only positioned if they are not already positioned.
        /// </summary>
        /// <returns>True for success, false otherwise.</returns>
        public bool SetChildrenPosition
            (
            System.Drawing.Graphics oGraphics,
            int nIndex,
            ref float dNextChildWith,
            ref float dNextChildOut
            )
        {
            float dTop;			// Top of this connection

            switch(m_MainPersonType)
            {
            case enumConMainPerson.Father:
            case enumConMainPerson.Mother:
                if(m_oChildren == null)
                {
                    m_dChildBarHeight = 0;
                    break;
                }

                if(m_oFather != null)
                {
                    dTop = m_oFather.Y;
                }
                else
                {
                    dTop = m_oMother.Y;
                }

                // The height of the bars changes for second marriages etc ...
                m_dChildBarHeight = dTop + m_oTree.spcHorizontalBarPos + 4 * m_nIndex;

                float dY;
                float dX_Base;
                float dX_Person;
                dY = dTop + m_oTree.spcPersonY;

                // Get the collection of rules
                clsTreeRule[] oRules = m_oTree.Options.GetRules();                
                
                clsTreePerson[] oChildren = GetChildren();
                foreach(clsTreePerson oChild in oChildren)
                {
                    if(oChild.HasDescendants())
                    {
                        dX_Base = dNextChildWith;
                    }
                    else
                    {
                        dX_Base = dNextChildOut;
                    }

                    // Check for any rules to be applied
                    foreach(clsTreeRule oRule in oRules)
                    {
                        if(oRule.PersonID == oChild.PersonID && oRule.Action == clsTreeRule.ERuleAction.HorizontalOffset)
                        {
                            dX_Base += oRule.ParameterAsFloat * m_oTree.ScalingFactor;
                            dNextChildWith += oRule.ParameterAsFloat * m_oTree.ScalingFactor;
                        }
                    }
                    
                    // Offetset female children to make space for their partners
                    dX_Person = dX_Base + oChild.GetHusbandSpace(oGraphics);

                    // Position the child
                    oChild.SetPosition(oGraphics,dX_Person,dY);                    

                    // Calculate 2 positions for the next child (one for a child that needs space for descendants and one for a child that does not need dependant space)
                    // However, ensure that dNextChildWith does not decrease that space has already gone.
                    dNextChildOut = dX_Person + oChild.GetWidth(oGraphics,false,false) + oChild.GetWifeSpace(oGraphics) + m_oTree.spcSiblingSpace;                    
                    float dMinimumChildWith = dX_Base + oChild.GetWidth(oGraphics,true,false) + m_oTree.spcSiblingSpace;
                    if(dMinimumChildWith > dNextChildWith)
                    {
                        dNextChildWith = dMinimumChildWith;
                    }
                }

                break;

            case enumConMainPerson.ChildBoy:
            case enumConMainPerson.ChildGirl:
            case enumConMainPerson.Child:
                // This is an error
                throw (new System.Exception("Children not allowed here"));
            }

            // Return success
            return true;
        }

        /// <summary>
        /// Calculates the positions for the person parents.
        /// This should only be called for a person's ancestors
        /// </summary>
        /// <param name="oGraphics">Specifies the device where the connection will be drawn</param>
        /// <returns></returns>
        public bool CalculatePositionsParents
            (
            System.Drawing.Graphics oGraphics
            )
        {
            // If no parents then nothing to do in all cases
            if(m_oFather == null && m_oMother == null)
            {
                // No Parents to position
                return true;
            }

            // The y position of all types of parent will be the same
            float dX = 0;
            float dY = MainPerson.Y - m_oTree.spcPersonY;

            // Only one bar per ancestor so always the same height.
            // The height is varied when an "uncle" is only attached to one of the "parents" in the drawing routine.  The 4*m_nIndex has no effect since m_nIndex is alway 0.
            m_dChildBarHeight = dY + m_oTree.spcHorizontalBarPos + 4 * m_nIndex;

            switch(m_MainPersonType)
            {
            case enumConMainPerson.Father:
            case enumConMainPerson.Mother:
                throw (new System.Exception("Parents not allowed here"));

            case enumConMainPerson.Child:	// This is the main person in the tree
                // If both parents are known
                if(m_oFather != null && m_oMother != null)
                {
                    // Both parents are known
                    if(!m_oFather.PositionKnown && !m_oMother.PositionKnown)
                    {
                        dX = this.MainPerson.X + this.MainPerson.GetWidth(oGraphics,false,false) / 2 - m_oFather.GetWidth(oGraphics,false,false) - m_oTree.spcRelationshipSpace / 2;
                        m_oFather.SetPosition(oGraphics,dX,dY);

                        dX += m_oFather.GetWidth(oGraphics,false,false);
                        m_PosRelationship.Y = dY;
                        m_PosRelationship.X = dX + m_oTree.spcRelationshipSpace / 2;

                        dX += m_oTree.spcRelationshipSpace;
                        m_oMother.SetPosition(oGraphics,dX,dY);
                    }
                }
                else if(m_oFather != null)
                {
                    // Father only is known
                    if(!m_oFather.PositionKnown)
                    {
                        dX = MainPerson.X + MainPerson.GetWidth(oGraphics,false,false) / 2 - m_oFather.GetWidth(oGraphics,false,false) / 2;
                        m_oFather.SetPosition(oGraphics,dX,dY);

                        m_PosRelationship.X = dX + m_oFather.GetWidth(oGraphics,false,false) / 2;
                        m_PosRelationship.Y = dY; //  + m_oTree.PersonHeight;
                    }
                }
                else
                {
                    // Mother only is known
                    if(!m_oMother.PositionKnown)
                    {
                        dX = MainPerson.X + MainPerson.GetWidth(oGraphics,false,false) / 2 - m_oMother.GetWidth(oGraphics,false,false) / 2;
                        m_oMother.SetPosition(oGraphics,dX,dY);

                        m_PosRelationship.X = dX + m_oMother.GetWidth(oGraphics,false,false);
                        m_PosRelationship.Y = dY; // + m_oTree.PersonHeight;
                    }
                }
                break;

            case enumConMainPerson.ChildBoy:	// This is a random boy ancestor in the tree
                // If both parents are known
                if(m_oFather != null && m_oMother != null)
                {
                    // Both parents are known
                    if(!m_oFather.PositionKnown && !m_oMother.PositionKnown)
                    {

                        dX = MainPerson.X + MainPerson.GetWidth(oGraphics,false,false) - m_oMother.GetWidth(oGraphics,true,true);
                        m_oMother.SetPosition(oGraphics,dX,dY);

                        dX -= m_oTree.spcRelationshipSpace;
                        m_PosRelationship.X = dX + m_oTree.spcRelationshipSpace / 2;
                        m_PosRelationship.Y = dY;

                        dX -= m_oFather.GetWidth(oGraphics,false,false);
                        m_oFather.SetPosition(oGraphics,dX,dY);
                    }
                }
                else if(m_oFather != null)
                {
                    // Father only is known
                    if(!m_oFather.PositionKnown)
                    {

                        dX = MainPerson.X + MainPerson.GetWidth(oGraphics,false,false) - m_oFather.GetWidth(oGraphics,false,false);
                        m_oFather.SetPosition(oGraphics,dX,dY);

                        m_PosRelationship.X = dX + m_oFather.GetWidth(oGraphics,false,false) / 2;
                        m_PosRelationship.Y = dY; //  + m_oTree.PersonHeight;
                    }
                }
                else
                {
                    // Mother only is known
                    if(!m_oMother.PositionKnown)
                    {

                        dX = MainPerson.X + MainPerson.GetWidth(oGraphics,false,false) - m_oMother.GetWidth(oGraphics,true,true);
                        m_oMother.SetPosition(oGraphics,dX,dY);

                        m_PosRelationship.X = dX + m_oMother.GetWidth(oGraphics,false,false);
                        m_PosRelationship.Y = dY; // + m_oTree.PersonHeight;
                    }
                }
                break;

            case enumConMainPerson.ChildGirl:	// This is a random girl ancestor in the tree
                // If both parents are known
                if(m_oFather != null && m_oMother != null)
                {
                    // Both parents are known
                    if(!m_oFather.PositionKnown && !m_oMother.PositionKnown)
                    {

                        dX = this.MainPerson.X + m_oFather.GetWidth(oGraphics,true,true) - m_oFather.GetWidth(oGraphics,false,false);
                        m_oFather.SetPosition(oGraphics,dX,dY);

                        dX += m_oFather.GetWidth(oGraphics,false,false);
                        m_PosRelationship.X = dX + m_oTree.spcRelationshipSpace / 2;
                        m_PosRelationship.Y = dY;

                        dX += m_oTree.spcRelationshipSpace;
                        m_oMother.SetPosition(oGraphics,dX,dY);
                    }
                }
                else if(m_oFather != null)
                {
                    // Father only is known
                    if(!m_oFather.PositionKnown)
                    {

                        dX = this.MainPerson.X + m_oFather.GetWidth(oGraphics,true,true) - m_oFather.GetWidth(oGraphics,false,false);
                        m_oFather.SetPosition(oGraphics,dX,dY);

                        m_PosRelationship.X = dX + m_oFather.GetWidth(oGraphics,false,false) / 2;
                        m_PosRelationship.Y = dY; //  + m_oTree.PersonHeight;
                    }
                }
                else
                {
                    // Mother only is known
                    if(!m_oMother.PositionKnown)
                    {
                        // TODO: Not really sure about this.  I just copied it from above

                        dX = this.MainPerson.X + this.MainPerson.GetWidth(oGraphics,false,false) - m_oMother.GetWidth(oGraphics,true,true);
                        m_oMother.SetPosition(oGraphics,dX,dY);

                        m_PosRelationship.X = dX + m_oMother.GetWidth(oGraphics,false,false) / 2;
                        m_PosRelationship.Y = dY; // + m_oTree.PersonHeight;
                    }
                }
                break;
            }

            // Position the siblings
            // The first sibling will be the main person and is the reference position for the other siblings.
            float dLeftX = 0;
            float dRightX = 0;
            bool bOnLeft = true;
            bool bFirstSibling = true; 
            clsTreePerson[] oSiblings = GetChildren();

            foreach(clsTreePerson oSibling in oSiblings)
            {
                if(bFirstSibling)
                {
                    bFirstSibling = false;
                    if(m_bMale)
                    {
                        dLeftX = oSibling.X;
                        dRightX = oSibling.X + oSibling.GetWidth(oGraphics)+oSibling.GetPartnerSpace(oGraphics) + m_oTree.spcSiblingSpace;
                    }
                    else
                    {
                        dLeftX = oSibling.X - oSibling.GetPartnerSpace(oGraphics);
                        dRightX = oSibling.X + oSibling.GetWidth(oGraphics) + m_oTree.spcSiblingSpace;
                    }
                    dY = oSibling.Y;
                }
                else
                {
                    if(!oSibling.PositionKnown)
                    {
                        float dFullWidth = oSibling.GetWidth(oGraphics,true,false);
                        float dNameWidth = oSibling.GetWidth(oGraphics,false,false);

                        if(m_MainPersonType == enumConMainPerson.Child)
                        {
                            // Position the main person's siblings to left and right of him / according to age.
                            bOnLeft = !MainPerson.IsOlder(oSibling.PersonID);                             
                        }
                        else
                        {
                            // Position all siblings to the left of a boy and to the right of a girl to keep them out of the way of spouses
                            bOnLeft = m_bMale;
                        }

                        if(bOnLeft)
                        {
                            if(oSibling.IsMale())
                            {
                                dLeftX -= (dFullWidth + m_oTree.spcSiblingSpace);
                            }
                            else
                            {
                                dLeftX -= (dNameWidth + m_oTree.spcSiblingSpace);
                            }
                            oSibling.SetPosition(oGraphics,dLeftX,dY);
                            if(!oSibling.IsMale())
                            {
                                dLeftX -= (dFullWidth - dNameWidth);
                            }
                        }
                        else
                        {
                            oSibling.SetPosition(oGraphics,dRightX,dY);
                            dRightX += (oSibling.GetWidth(oGraphics,true,false) + m_oTree.spcSiblingSpace);
                        }
                    }
                }
            }

            // Return success
            return true;
        }

        /// <summary>
        /// Returns the space that would be required to display the children on the specified graphics device
        /// </summary>
        /// <param name="oGraphics">Specifies the graphics device that the children would be drawn on.</param>
        /// <returns>The horizontal space required to display the children.</returns>
        public float GetChildrenSpace
            (
            System.Drawing.Graphics oGraphics
            )
        {
            float dWidth = 0;
            if(m_oChildren != null)
            {
                for(int nChild = 0;nChild < m_oChildren.Count;nChild++)
                {
                    clsTreePerson oChild = (clsTreePerson)m_oChildren[nChild];
                    dWidth += oChild.GetWidth(oGraphics,false,false);
                    dWidth += oChild.GetPartnerSpace(oGraphics);
                    if(nChild > 0)
                    {
                        dWidth += m_oTree.spcSiblingSpace;
                    }
                }
            }
            return dWidth;
        }

        #endregion

        #region Drawing

        /// <summary>
        /// Draws the connection object.
        /// Draws the lines for the connection object.
        /// Draws the people in the connection object except the MAIN person.
        /// </summary>
        /// <param name="oGraphics">Specifies the graphics device to draw the connection object on.</param>
        /// <returns>True for success, false otherwise</returns>
        public bool Draw
            (
            System.Drawing.Graphics oGraphics
            )
        {
            // Draw a connection between the mother and father
            if(m_oFather != null && m_oMother != null)
            {
                if(m_nStatus == enumRelationshipStatus.Married || m_nStatus == enumRelationshipStatus.Divorced)
                {
                    oGraphics.DrawLine(m_oTree.PenBlackThick,m_PosRelationship.X - m_oTree.RelationshipSymbol - m_oTree.OffsetX,m_PosRelationship.Y + m_oTree.spcRelationsMarker - m_oTree.OffsetY,m_PosRelationship.X + m_oTree.RelationshipSymbol - m_oTree.OffsetX,m_PosRelationship.Y + m_oTree.spcRelationsMarker - m_oTree.OffsetY);
                    oGraphics.DrawLine(m_oTree.PenBlackThick,m_PosRelationship.X - m_oTree.RelationshipSymbol - m_oTree.OffsetX,m_PosRelationship.Y + m_oTree.spcRelationsMarker + 4 - m_oTree.OffsetY,m_PosRelationship.X + m_oTree.RelationshipSymbol - m_oTree.OffsetX,m_PosRelationship.Y + m_oTree.spcRelationsMarker + 4 - m_oTree.OffsetY);
                    if(m_nStatus == enumRelationshipStatus.Divorced)
                    {
                        oGraphics.DrawLine(m_oTree.PenBlackThin,m_PosRelationship.X - m_oTree.RelationshipSymbol - m_oTree.OffsetX + 1,m_PosRelationship.Y + m_oTree.spcRelationsMarker + 7 - m_oTree.OffsetY,m_PosRelationship.X + m_oTree.RelationshipSymbol - m_oTree.OffsetX - 2,m_PosRelationship.Y + m_oTree.spcRelationsMarker - 3 - m_oTree.OffsetY);
                    }

                    // Write the start date
                    if(m_dtStart != null)
                    {
                        if(!m_dtStart.isEmpty())
                        {
                            // TODO: Calculate the correct position for this text
                            int nYear = CompoundDate.getYear(m_dtStart.date);
                            string sYear;
                            if(nYear < 0)
                            {
                                sYear = (-nYear).ToString() + "BC";
                            }
                            else
                            {
                                sYear = nYear.ToString();
                            }
                            oGraphics.DrawString(sYear,m_oTree.FontDescription,m_oTree.BrushBlack,m_PosRelationship.X - m_oTree.spcRelationshipSpace / 2 - m_oTree.OffsetX,m_PosRelationship.Y - 4 - m_oTree.OffsetY);
                        }
                    }
                }
            }

            // Draw the children (need to check the mainperson flag)
            if(m_oChildren != null)
            {
                int nDropLinesRequired = 0;

                // Draw the children
                for(int nI = 0;nI < m_oChildren.Count;nI++)
                {
                    clsTreePerson oChild = (clsTreePerson)m_oChildren[nI];

                    // Draw the connections to all children (including main children)					
                    float X = oChild.X + oChild.GetWidth(oGraphics,false,false) / 2;

                    switch(oChild.Connection)
                    {
                    case clsTreePerson.enumConnection.cnBoth:
                        oGraphics.DrawLine(m_oTree.PenBlackThick,X - m_oTree.OffsetX,oChild.Y - m_oTree.OffsetY,X - m_oTree.OffsetX,m_dChildBarHeight - m_oTree.OffsetY);
                        oGraphics.DrawLine(m_oTree.PenBlackThick,X - m_oTree.OffsetX,m_dChildBarHeight - m_oTree.OffsetY,m_PosRelationship.X - m_oTree.OffsetX,m_dChildBarHeight - m_oTree.OffsetY);
                        nDropLinesRequired |= 1;
                        break;

                    case clsTreePerson.enumConnection.cnFather:
                        oGraphics.DrawLine(m_oTree.PenBlackThick,X - m_oTree.OffsetX,oChild.Y - m_oTree.OffsetY,X - m_oTree.OffsetX,m_dChildBarHeight - 4 - m_oTree.OffsetY);
                        oGraphics.DrawLine(m_oTree.PenBlackThick,X - m_oTree.OffsetX,m_dChildBarHeight - 4 - m_oTree.OffsetY,m_oFather.X + m_oFather.GetWidth(oGraphics,false,false) / 2 - m_oTree.OffsetX,m_dChildBarHeight - 4 - m_oTree.OffsetY);
                        nDropLinesRequired |= 2;
                        break;

                    case clsTreePerson.enumConnection.cnMother:
                        oGraphics.DrawLine(m_oTree.PenBlackThick,X - m_oTree.OffsetX,oChild.Y - m_oTree.OffsetY,X - m_oTree.OffsetX,m_dChildBarHeight - 8 - m_oTree.OffsetY);
                        oGraphics.DrawLine(m_oTree.PenBlackThick,X - m_oTree.OffsetX,m_dChildBarHeight - 8 - m_oTree.OffsetY,m_oMother.X + m_oMother.GetWidth(oGraphics,false,false) / 2 - m_oTree.OffsetX,m_dChildBarHeight - 8 - m_oTree.OffsetY);
                        nDropLinesRequired |= 4;
                        break;

                    }
                }

                // Draw the drop lines
                if((nDropLinesRequired & 1) == 1)
                {
                    if(m_oFather != null && m_oMother != null)
                    {
                        // Draw a line down from the married parents to the horizontal bar
                        oGraphics.DrawLine(m_oTree.PenBlackThick,m_PosRelationship.X - m_oTree.OffsetX,m_PosRelationship.Y + m_oTree.spcRelationsMarker + 10 - m_oTree.OffsetY,m_PosRelationship.X - m_oTree.OffsetX,m_dChildBarHeight - m_oTree.OffsetY);
                    }
                    else
                    {
                        // Draw a line from the single parent to the horizontal bar 
                        oGraphics.DrawLine(m_oTree.PenBlackThick,m_PosRelationship.X - m_oTree.OffsetX,m_PosRelationship.Y + m_oTree.spcPersonHeight - m_oTree.OffsetY,m_PosRelationship.X - m_oTree.OffsetX,m_dChildBarHeight - m_oTree.OffsetY);
                    }
                }
                if((nDropLinesRequired & 2) == 2)
                {
                    // Draw a line down from the father to the (offset) horizontal bar
                    oGraphics.DrawLine(m_oTree.PenBlackThick,m_oFather.X + m_oFather.GetWidth(oGraphics,false,false) / 2 - m_oTree.OffsetX,m_oFather.Y + m_oTree.spcPersonHeight - m_oTree.OffsetY,m_oFather.X + m_oFather.GetWidth(oGraphics,false,false) / 2 - m_oTree.OffsetX,m_dChildBarHeight - 4 - m_oTree.OffsetY);
                }
                if((nDropLinesRequired & 4) == 4)
                {
                    // Draw a line down from the mother to the (offset) horizontal bar
                    oGraphics.DrawLine(m_oTree.PenBlackThick,m_oMother.X + m_oMother.GetWidth(oGraphics,false,false) / 2 - m_oTree.OffsetX,m_oMother.Y + m_oTree.spcPersonHeight - m_oTree.OffsetY,m_oMother.X + m_oMother.GetWidth(oGraphics,false,false) / 2 - m_oTree.OffsetX,m_dChildBarHeight - 8 - m_oTree.OffsetY);
                }
            }

            // Return success
            return true;
        }

        #endregion

    }
}
