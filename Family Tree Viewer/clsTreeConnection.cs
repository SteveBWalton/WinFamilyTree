using System;
using System.Collections;
using FamilyTree.Objects;

namespace FamilyTree.Viewer
{
    #region Supporting Types etc

    /// <summary>Reason this connection has come into the tree.</summary>
    public enum ConnectionMainPerson
    {
        /// <summary>Main person is the father.</summary>
        FATHER,
        /// <summary>Main person is the mother.</summary>
        MOTHER,
        /// <summary>Main person is child[0] he must be shown on the left.</summary>
        CHILD_BOY,
        /// <summary>Main person is child[0] she must be shown on the right.</summary>
        CHILD_GIRL,
        /// <summary>Main person is child[0] he/she can be shown sysmetrically.</summary>
        CHILD
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
        private TreeDocument tree_;

        /// <summary>The reason this connection was created.</summary>
        private ConnectionMainPerson mainPersonType_;

        /// <summary>The father in this connection.</summary>
        private clsTreePerson father_;

        /// <summary>The mother in this connection.</summary>
        private clsTreePerson mother_;

        /// <summary>The children in this connection.</summary>
        private ArrayList children_;

        /// <summary>Position of the relationship marker.</summary>
        private System.Drawing.PointF posRelationship_;

        /// <summary>Height of the child bar.</summary>
        private float childBarHeight_;

        /// <summary>Status of the relationship.</summary>
        private enumRelationshipStatus status_;

        /// <summary>Is the main person male.</summary>
        private bool isMale_;

        /// <summary>Date of the start relationship.</summary>
        private CompoundDate start_;

        /// <summary>Index of the relationship in the context of the main person.  Ie is this a second marriage?</summary>
        private int index_;

        #endregion

        #region Constructors etc...

        /// <summary>
        /// Class constructor.
        /// Creates a new tree connection object.
        /// </summary>
        /// <param name="tree">Specifies the tree document that contains this connection.</param>
        /// <param name="person">Specifies the main person in this connection.</param>
        /// <param name="personType">Specifies the role of the main person in this connection.</param>
        /// <param name="index">Specify the count of this relationship in context of the main person.  Usually 0.</param>
        public clsTreeConnection(TreeDocument tree, clsTreePerson person, ConnectionMainPerson personType, int index)
        {
            // Save the input parametrs.
            tree_ = tree;
            mainPersonType_ = personType;
            index_ = index;

            // Initialise the object.
            posRelationship_ = new System.Drawing.PointF(0, 0);
            status_ = enumRelationshipStatus.Married;
            start_ = null;

            switch (mainPersonType_)
            {
            case ConnectionMainPerson.FATHER:
                father_ = person;
                mother_ = null;
                children_ = null;
                isMale_ = true;
                break;

            case ConnectionMainPerson.MOTHER:
                father_ = null;
                mother_ = person;
                children_ = null;
                isMale_ = false;
                break;

            case ConnectionMainPerson.CHILD_BOY:
            case ConnectionMainPerson.CHILD_GIRL:
            case ConnectionMainPerson.CHILD:
                father_ = null;
                mother_ = null;
                AddChild(person);
                switch (mainPersonType_)
                {
                case ConnectionMainPerson.CHILD_BOY:
                    isMale_ = true;
                    break;

                case ConnectionMainPerson.CHILD_GIRL:
                    isMale_ = false;
                    break;

                case ConnectionMainPerson.CHILD:
                    Person oMainPerson = new Person(person.PersonID, tree_.database);
                    isMale_ = oMainPerson.isMale;
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
            father_ = oFather; // new clsTreePerson(m_oTree,nFatherID);

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
            mother_ = oMother;			// new clsTreePerson(m_oTree,nMotherID);

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
            if (children_ == null)
            {
                children_ = new ArrayList();
            }
            children_.Add(oChild);

            // Return success
            return true;
        }

        /// <summary>
        /// Returns the ID of the father in this connection.  Returns 0 for no father.
        /// </summary>
        /// <returns></returns>
        public int GetFatherID()
        {
            if (father_ == null)
            {
                return 0;
            }
            return father_.PersonID;
        }

        /// <summary>
        /// Returns the children in this connection as an array of clsTreePerson objects.
        /// </summary>
        /// <returns></returns>
        public clsTreePerson[] GetChildren()
        {
            if (children_ == null)
            {
                return new clsTreePerson[0];
            }
            return (clsTreePerson[])children_.ToArray(typeof(clsTreePerson));
        }

        /// <summary>
        /// Returns the ID of the mother in this connection.  Returns 0 for no mother.
        /// </summary>
        /// <returns></returns>
        public int GetMotherID()
        {
            if (mother_ == null)
            {
                return 0;
            }
            return mother_.PersonID;
        }

        /// <summary>Returns the father in this connection.</summary>
        public clsTreePerson Father { get { return father_; } }

        /// <summary>Returns the mother in this connection.</summary>
        public clsTreePerson Mother { get { return mother_; } }

        /// <summary>Returns the major person in this connection.  Cause of the connection existance.</summary>
        private clsTreePerson MainPerson { get { return (clsTreePerson)children_[0]; } }

        /// <summary>Returns the status of this connection.</summary>
        public enumRelationshipStatus Status { get { return status_; } set { status_ = value; } }

        /// <summary>Returns the start date of this connection.</summary>
        public CompoundDate Start { get { return start_; } set { start_ = value; } }

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
            if (dWidth > dMaxWidth)
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
            if (father_ != null)
            {
                dWidth += father_.GetWidth(oGraphics, false, false);
                if (mother_ != null)
                {
                    dWidth += tree_.spcRelationshipSpace;
                }
            }
            if (mother_ != null)
            {
                dWidth += mother_.GetWidth(oGraphics, false, false);
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
            if (children_ != null)
            {
                clsTreePerson[] oChildren = GetChildren();
                foreach (clsTreePerson oChild in oChildren)
                {
                    dWidth += oChild.GetWidth(oGraphics, true, false) + tree_.spcSiblingSpace;
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
            switch (mainPersonType_)
            {
            case ConnectionMainPerson.FATHER:
                return mother_.X + mother_.GetWidth(oGraphics, false, false);

            case ConnectionMainPerson.MOTHER:
                return father_.X;
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
            if (children_ == null)
            {
                if (father_ != null)
                {
                    return father_.X;
                }
                return mother_.X;
            }
            clsTreePerson oLastChild = (clsTreePerson)children_[children_.Count - 1];
            return oLastChild.X + oLastChild.GetWidth(oGraphics, true, false);
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
            switch (mainPersonType_)
            {
            case ConnectionMainPerson.FATHER:
                // Position the mother relative to the father				
                posRelationship_.Y = father_.Y;
                if (mother_ != null)
                {
                    if (!mother_.PositionKnown)
                    {
                        posRelationship_.X = father_.GetSpousePosition(oGraphics, nIndex) + tree_.spcRelationshipSpace / 2;

                        mother_.SetPosition(oGraphics, posRelationship_.X + tree_.spcRelationshipSpace / 2, father_.Y);
                    }
                }
                else
                {
                    posRelationship_.X = father_.X + father_.GetWidth(oGraphics, false, false) / 2;
                }
                break;

            case ConnectionMainPerson.MOTHER:
                // Position the father realive to the mother				
                posRelationship_.Y = mother_.Y;
                if (father_ != null)
                {
                    if (!father_.PositionKnown)
                    {
                        posRelationship_.X = mother_.GetSpousePosition(oGraphics, nIndex) - tree_.spcRelationshipSpace / 2;
                        father_.SetPosition(oGraphics, posRelationship_.X - tree_.spcRelationshipSpace / 2 - father_.GetWidth(oGraphics, false, false), mother_.Y);
                    }
                }
                else
                {
                    posRelationship_.X = mother_.X + mother_.GetWidth(oGraphics, false, false) / 2;
                }
                // CalculateChildPositionFixedParents(oGraphics,nIndex,ref dNextChild);
                break;

            case ConnectionMainPerson.CHILD_BOY:
            case ConnectionMainPerson.CHILD_GIRL:
            case ConnectionMainPerson.CHILD:
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

            switch (mainPersonType_)
            {
            case ConnectionMainPerson.FATHER:
            case ConnectionMainPerson.MOTHER:
                if (children_ == null)
                {
                    childBarHeight_ = 0;
                    break;
                }

                if (father_ != null)
                {
                    dTop = father_.Y;
                }
                else
                {
                    dTop = mother_.Y;
                }

                // The height of the bars changes for second marriages etc ...
                childBarHeight_ = dTop + tree_.spcHorizontalBarPos + 4 * index_;

                float dY;
                float dX_Base;
                float dX_Person;
                dY = dTop + tree_.spcPersonY;

                // Get the collection of rules
                clsTreeRule[] oRules = tree_.options.getRules();

                clsTreePerson[] oChildren = GetChildren();
                foreach (clsTreePerson oChild in oChildren)
                {
                    if (oChild.hasDescendants())
                    {
                        dX_Base = dNextChildWith;
                    }
                    else
                    {
                        dX_Base = dNextChildOut;
                    }

                    // Check for any rules to be applied
                    foreach (clsTreeRule oRule in oRules)
                    {
                        if (oRule.personIndex == oChild.PersonID && oRule.action == clsTreeRule.RuleAction.HORIZONTAL_OFFSET)
                        {
                            dX_Base += oRule.parameterAsFloat * tree_.scalingFactor;
                            dNextChildWith += oRule.parameterAsFloat * tree_.scalingFactor;
                        }
                    }

                    // Offetset female children to make space for their partners
                    dX_Person = dX_Base + oChild.GetHusbandSpace(oGraphics);

                    // Position the child
                    oChild.SetPosition(oGraphics, dX_Person, dY);

                    // Calculate 2 positions for the next child (one for a child that needs space for descendants and one for a child that does not need dependant space)
                    // However, ensure that dNextChildWith does not decrease that space has already gone.
                    dNextChildOut = dX_Person + oChild.GetWidth(oGraphics, false, false) + oChild.GetWifeSpace(oGraphics) + tree_.spcSiblingSpace;
                    float dMinimumChildWith = dX_Base + oChild.GetWidth(oGraphics, true, false) + tree_.spcSiblingSpace;
                    if (dMinimumChildWith > dNextChildWith)
                    {
                        dNextChildWith = dMinimumChildWith;
                    }
                }

                break;

            case ConnectionMainPerson.CHILD_BOY:
            case ConnectionMainPerson.CHILD_GIRL:
            case ConnectionMainPerson.CHILD:
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
            if (father_ == null && mother_ == null)
            {
                // No Parents to position
                return true;
            }

            // The y position of all types of parent will be the same
            float dX = 0;
            float dY = MainPerson.Y - tree_.spcPersonY;

            // Only one bar per ancestor so always the same height.
            // The height is varied when an "uncle" is only attached to one of the "parents" in the drawing routine.  The 4*m_nIndex has no effect since m_nIndex is alway 0.
            childBarHeight_ = dY + tree_.spcHorizontalBarPos + 4 * index_;

            switch (mainPersonType_)
            {
            case ConnectionMainPerson.FATHER:
            case ConnectionMainPerson.MOTHER:
                throw (new System.Exception("Parents not allowed here"));

            case ConnectionMainPerson.CHILD:	// This is the main person in the tree
                // If both parents are known
                if (father_ != null && mother_ != null)
                {
                    // Both parents are known
                    if (!father_.PositionKnown && !mother_.PositionKnown)
                    {
                        dX = this.MainPerson.X + this.MainPerson.GetWidth(oGraphics, false, false) / 2 - father_.GetWidth(oGraphics, false, false) - tree_.spcRelationshipSpace / 2;
                        father_.SetPosition(oGraphics, dX, dY);

                        dX += father_.GetWidth(oGraphics, false, false);
                        posRelationship_.Y = dY;
                        posRelationship_.X = dX + tree_.spcRelationshipSpace / 2;

                        dX += tree_.spcRelationshipSpace;
                        mother_.SetPosition(oGraphics, dX, dY);
                    }
                }
                else if (father_ != null)
                {
                    // Father only is known
                    if (!father_.PositionKnown)
                    {
                        dX = MainPerson.X + MainPerson.GetWidth(oGraphics, false, false) / 2 - father_.GetWidth(oGraphics, false, false) / 2;
                        father_.SetPosition(oGraphics, dX, dY);

                        posRelationship_.X = dX + father_.GetWidth(oGraphics, false, false) / 2;
                        posRelationship_.Y = dY; //  + m_oTree.PersonHeight;
                    }
                }
                else
                {
                    // Mother only is known
                    if (!mother_.PositionKnown)
                    {
                        dX = MainPerson.X + MainPerson.GetWidth(oGraphics, false, false) / 2 - mother_.GetWidth(oGraphics, false, false) / 2;
                        mother_.SetPosition(oGraphics, dX, dY);

                        posRelationship_.X = dX + mother_.GetWidth(oGraphics, false, false);
                        posRelationship_.Y = dY; // + m_oTree.PersonHeight;
                    }
                }
                break;

            case ConnectionMainPerson.CHILD_BOY:	// This is a random boy ancestor in the tree
                // If both parents are known
                if (father_ != null && mother_ != null)
                {
                    // Both parents are known
                    if (!father_.PositionKnown && !mother_.PositionKnown)
                    {

                        dX = MainPerson.X + MainPerson.GetWidth(oGraphics, false, false) - mother_.GetWidth(oGraphics, true, true);
                        mother_.SetPosition(oGraphics, dX, dY);

                        dX -= tree_.spcRelationshipSpace;
                        posRelationship_.X = dX + tree_.spcRelationshipSpace / 2;
                        posRelationship_.Y = dY;

                        dX -= father_.GetWidth(oGraphics, false, false);
                        father_.SetPosition(oGraphics, dX, dY);
                    }
                }
                else if (father_ != null)
                {
                    // Father only is known
                    if (!father_.PositionKnown)
                    {

                        dX = MainPerson.X + MainPerson.GetWidth(oGraphics, false, false) - father_.GetWidth(oGraphics, false, false);
                        father_.SetPosition(oGraphics, dX, dY);

                        posRelationship_.X = dX + father_.GetWidth(oGraphics, false, false) / 2;
                        posRelationship_.Y = dY; //  + m_oTree.PersonHeight;
                    }
                }
                else
                {
                    // Mother only is known
                    if (!mother_.PositionKnown)
                    {

                        dX = MainPerson.X + MainPerson.GetWidth(oGraphics, false, false) - mother_.GetWidth(oGraphics, true, true);
                        mother_.SetPosition(oGraphics, dX, dY);

                        posRelationship_.X = dX + mother_.GetWidth(oGraphics, false, false);
                        posRelationship_.Y = dY; // + m_oTree.PersonHeight;
                    }
                }
                break;

            case ConnectionMainPerson.CHILD_GIRL:	// This is a random girl ancestor in the tree
                // If both parents are known
                if (father_ != null && mother_ != null)
                {
                    // Both parents are known
                    if (!father_.PositionKnown && !mother_.PositionKnown)
                    {

                        dX = this.MainPerson.X + father_.GetWidth(oGraphics, true, true) - father_.GetWidth(oGraphics, false, false);
                        father_.SetPosition(oGraphics, dX, dY);

                        dX += father_.GetWidth(oGraphics, false, false);
                        posRelationship_.X = dX + tree_.spcRelationshipSpace / 2;
                        posRelationship_.Y = dY;

                        dX += tree_.spcRelationshipSpace;
                        mother_.SetPosition(oGraphics, dX, dY);
                    }
                }
                else if (father_ != null)
                {
                    // Father only is known
                    if (!father_.PositionKnown)
                    {

                        dX = this.MainPerson.X + father_.GetWidth(oGraphics, true, true) - father_.GetWidth(oGraphics, false, false);
                        father_.SetPosition(oGraphics, dX, dY);

                        posRelationship_.X = dX + father_.GetWidth(oGraphics, false, false) / 2;
                        posRelationship_.Y = dY; //  + m_oTree.PersonHeight;
                    }
                }
                else
                {
                    // Mother only is known
                    if (!mother_.PositionKnown)
                    {
                        // TODO: Not really sure about this.  I just copied it from above

                        dX = this.MainPerson.X + this.MainPerson.GetWidth(oGraphics, false, false) - mother_.GetWidth(oGraphics, true, true);
                        mother_.SetPosition(oGraphics, dX, dY);

                        posRelationship_.X = dX + mother_.GetWidth(oGraphics, false, false) / 2;
                        posRelationship_.Y = dY; // + m_oTree.PersonHeight;
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

            foreach (clsTreePerson oSibling in oSiblings)
            {
                if (bFirstSibling)
                {
                    bFirstSibling = false;
                    if (isMale_)
                    {
                        dLeftX = oSibling.X;
                        dRightX = oSibling.X + oSibling.GetWidth(oGraphics) + oSibling.GetPartnerSpace(oGraphics) + tree_.spcSiblingSpace;
                    }
                    else
                    {
                        dLeftX = oSibling.X - oSibling.GetPartnerSpace(oGraphics);
                        dRightX = oSibling.X + oSibling.GetWidth(oGraphics) + tree_.spcSiblingSpace;
                    }
                    dY = oSibling.Y;
                }
                else
                {
                    if (!oSibling.PositionKnown)
                    {
                        float dFullWidth = oSibling.GetWidth(oGraphics, true, false);
                        float dNameWidth = oSibling.GetWidth(oGraphics, false, false);

                        if (mainPersonType_ == ConnectionMainPerson.CHILD)
                        {
                            // Position the main person's siblings to left and right of him / according to age.
                            bOnLeft = !MainPerson.IsOlder(oSibling.PersonID);
                        }
                        else
                        {
                            // Position all siblings to the left of a boy and to the right of a girl to keep them out of the way of spouses
                            bOnLeft = isMale_;
                        }

                        if (bOnLeft)
                        {
                            if (oSibling.IsMale())
                            {
                                dLeftX -= (dFullWidth + tree_.spcSiblingSpace);
                            }
                            else
                            {
                                dLeftX -= (dNameWidth + tree_.spcSiblingSpace);
                            }
                            oSibling.SetPosition(oGraphics, dLeftX, dY);
                            if (!oSibling.IsMale())
                            {
                                dLeftX -= (dFullWidth - dNameWidth);
                            }
                        }
                        else
                        {
                            oSibling.SetPosition(oGraphics, dRightX, dY);
                            dRightX += (oSibling.GetWidth(oGraphics, true, false) + tree_.spcSiblingSpace);
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
            if (children_ != null)
            {
                for (int nChild = 0; nChild < children_.Count; nChild++)
                {
                    clsTreePerson oChild = (clsTreePerson)children_[nChild];
                    dWidth += oChild.GetWidth(oGraphics, false, false);
                    dWidth += oChild.GetPartnerSpace(oGraphics);
                    if (nChild > 0)
                    {
                        dWidth += tree_.spcSiblingSpace;
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
        /// <param name="graphics">Specifies the graphics device to draw the connection object on.</param>
        /// <returns>True for success, false otherwise</returns>
        public bool draw(System.Drawing.Graphics graphics)
        {
            // Draw a connection between the mother and father
            if (father_ != null && mother_ != null)
            {
                if (status_ == enumRelationshipStatus.Married || status_ == enumRelationshipStatus.Divorced)
                {
                    graphics.DrawLine(tree_.penBlackThick, posRelationship_.X - tree_.relationshipSymbol - tree_.offsetX, posRelationship_.Y + tree_.spcRelationsMarker - tree_.offsetY, posRelationship_.X + tree_.relationshipSymbol - tree_.offsetX, posRelationship_.Y + tree_.spcRelationsMarker - tree_.offsetY);
                    graphics.DrawLine(tree_.penBlackThick, posRelationship_.X - tree_.relationshipSymbol - tree_.offsetX, posRelationship_.Y + tree_.spcRelationsMarker + 4 - tree_.offsetY, posRelationship_.X + tree_.relationshipSymbol - tree_.offsetX, posRelationship_.Y + tree_.spcRelationsMarker + 4 - tree_.offsetY);
                    if (status_ == enumRelationshipStatus.Divorced)
                    {
                        graphics.DrawLine(tree_.penBlackThin, posRelationship_.X - tree_.relationshipSymbol - tree_.offsetX + 1, posRelationship_.Y + tree_.spcRelationsMarker + 7 - tree_.offsetY, posRelationship_.X + tree_.relationshipSymbol - tree_.offsetX - 2, posRelationship_.Y + tree_.spcRelationsMarker - 3 - tree_.offsetY);
                    }

                    // Write the start date
                    if (start_ != null)
                    {
                        if (!start_.isEmpty())
                        {
                            // TODO: Calculate the correct position for this text
                            int nYear = CompoundDate.getYear(start_.date);
                            string sYear;
                            if (nYear < 0)
                            {
                                sYear = (-nYear).ToString() + "BC";
                            }
                            else
                            {
                                sYear = nYear.ToString();
                            }
                            graphics.DrawString(sYear, tree_.fontDescription, tree_.brushBlack, posRelationship_.X - tree_.spcRelationshipSpace / 2 - tree_.offsetX, posRelationship_.Y - 4 - tree_.offsetY);
                        }
                    }
                }
            }

            // Draw the children (need to check the mainperson flag)
            if (children_ != null)
            {
                int nDropLinesRequired = 0;

                // Draw the children
                for (int nI = 0; nI < children_.Count; nI++)
                {
                    clsTreePerson oChild = (clsTreePerson)children_[nI];

                    // Draw the connections to all children (including main children)					
                    float X = oChild.X + oChild.GetWidth(graphics, false, false) / 2;

                    switch (oChild.Connection)
                    {
                    case clsTreePerson.ParentConnection.BOTH:
                        graphics.DrawLine(tree_.penBlackThick, X - tree_.offsetX, oChild.Y - tree_.offsetY, X - tree_.offsetX, childBarHeight_ - tree_.offsetY);
                        graphics.DrawLine(tree_.penBlackThick, X - tree_.offsetX, childBarHeight_ - tree_.offsetY, posRelationship_.X - tree_.offsetX, childBarHeight_ - tree_.offsetY);
                        nDropLinesRequired |= 1;
                        break;

                    case clsTreePerson.ParentConnection.FATHER_ONLY:
                        graphics.DrawLine(tree_.penBlackThick, X - tree_.offsetX, oChild.Y - tree_.offsetY, X - tree_.offsetX, childBarHeight_ - 4 - tree_.offsetY);
                        graphics.DrawLine(tree_.penBlackThick, X - tree_.offsetX, childBarHeight_ - 4 - tree_.offsetY, father_.X + father_.GetWidth(graphics, false, false) / 2 - tree_.offsetX, childBarHeight_ - 4 - tree_.offsetY);
                        nDropLinesRequired |= 2;
                        break;

                    case clsTreePerson.ParentConnection.MOTHER_ONLY:
                        graphics.DrawLine(tree_.penBlackThick, X - tree_.offsetX, oChild.Y - tree_.offsetY, X - tree_.offsetX, childBarHeight_ - 8 - tree_.offsetY);
                        graphics.DrawLine(tree_.penBlackThick, X - tree_.offsetX, childBarHeight_ - 8 - tree_.offsetY, mother_.X + mother_.GetWidth(graphics, false, false) / 2 - tree_.offsetX, childBarHeight_ - 8 - tree_.offsetY);
                        nDropLinesRequired |= 4;
                        break;

                    }
                }

                // Draw the drop lines
                if ((nDropLinesRequired & 1) == 1)
                {
                    if (father_ != null && mother_ != null)
                    {
                        // Draw a line down from the married parents to the horizontal bar
                        graphics.DrawLine(tree_.penBlackThick, posRelationship_.X - tree_.offsetX, posRelationship_.Y + tree_.spcRelationsMarker + 10 - tree_.offsetY, posRelationship_.X - tree_.offsetX, childBarHeight_ - tree_.offsetY);
                    }
                    else
                    {
                        // Draw a line from the single parent to the horizontal bar 
                        graphics.DrawLine(tree_.penBlackThick, posRelationship_.X - tree_.offsetX, posRelationship_.Y + tree_.spcPersonHeight - tree_.offsetY, posRelationship_.X - tree_.offsetX, childBarHeight_ - tree_.offsetY);
                    }
                }
                if ((nDropLinesRequired & 2) == 2)
                {
                    // Draw a line down from the father to the (offset) horizontal bar
                    graphics.DrawLine(tree_.penBlackThick, father_.X + father_.GetWidth(graphics, false, false) / 2 - tree_.offsetX, father_.Y + tree_.spcPersonHeight - tree_.offsetY, father_.X + father_.GetWidth(graphics, false, false) / 2 - tree_.offsetX, childBarHeight_ - 4 - tree_.offsetY);
                }
                if ((nDropLinesRequired & 4) == 4)
                {
                    // Draw a line down from the mother to the (offset) horizontal bar
                    graphics.DrawLine(tree_.penBlackThick, mother_.X + mother_.GetWidth(graphics, false, false) / 2 - tree_.offsetX, mother_.Y + tree_.spcPersonHeight - tree_.offsetY, mother_.X + mother_.GetWidth(graphics, false, false) / 2 - tree_.offsetX, childBarHeight_ - 8 - tree_.offsetY);
                }
            }

            // Return success
            return true;
        }

        #endregion

    }
}
