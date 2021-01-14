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
        private TreePerson father_;

        /// <summary>The mother in this connection.</summary>
        private TreePerson mother_;

        /// <summary>The children in this connection.</summary>
        private ArrayList children_;

        /// <summary>Position of the relationship marker.</summary>
        private System.Drawing.PointF posRelationship_;

        /// <summary>Height of the child bar.</summary>
        private float childBarHeight_;

        /// <summary>Status of the relationship.</summary>
        private RelationshipStatus status_;

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
        public clsTreeConnection(TreeDocument tree, TreePerson person, ConnectionMainPerson personType, int index)
        {
            // Save the input parametrs.
            tree_ = tree;
            mainPersonType_ = personType;
            index_ = index;

            // Initialise the object.
            posRelationship_ = new System.Drawing.PointF(0, 0);
            status_ = RelationshipStatus.MARRIED;
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
                    Person oMainPerson = new Person(person.personIndex, tree_.database);
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
            TreePerson oFather
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
            TreePerson oMother
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
            TreePerson oChild
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
            return father_.personIndex;
        }

        /// <summary>
        /// Returns the children in this connection as an array of clsTreePerson objects.
        /// </summary>
        /// <returns></returns>
        public TreePerson[] GetChildren()
        {
            if (children_ == null)
            {
                return new TreePerson[0];
            }
            return (TreePerson[])children_.ToArray(typeof(TreePerson));
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
            return mother_.personIndex;
        }

        /// <summary>Returns the father in this connection.</summary>
        public TreePerson Father { get { return father_; } }

        /// <summary>Returns the mother in this connection.</summary>
        public TreePerson Mother { get { return mother_; } }

        /// <summary>Returns the major person in this connection.  Cause of the connection existance.</summary>
        private TreePerson MainPerson { get { return (TreePerson)children_[0]; } }

        /// <summary>Returns the status of this connection.</summary>
        public RelationshipStatus Status { get { return status_; } set { status_ = value; } }

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
                dWidth += father_.getWidth(oGraphics, false, false);
                if (mother_ != null)
                {
                    dWidth += tree_.spcRelationshipSpace;
                }
            }
            if (mother_ != null)
            {
                dWidth += mother_.getWidth(oGraphics, false, false);
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
                TreePerson[] oChildren = GetChildren();
                foreach (TreePerson oChild in oChildren)
                {
                    dWidth += oChild.getWidth(oGraphics, true, false) + tree_.spcSiblingSpace;
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
                return mother_.x + mother_.getWidth(oGraphics, false, false);

            case ConnectionMainPerson.MOTHER:
                return father_.x;
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
                    return father_.x;
                }
                return mother_.x;
            }
            TreePerson oLastChild = (TreePerson)children_[children_.Count - 1];
            return oLastChild.x + oLastChild.getWidth(oGraphics, true, false);
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
                posRelationship_.Y = father_.y;
                if (mother_ != null)
                {
                    if (!mother_.isPositionKnown)
                    {
                        posRelationship_.X = father_.GetSpousePosition(oGraphics, nIndex) + tree_.spcRelationshipSpace / 2;

                        mother_.setPosition(oGraphics, posRelationship_.X + tree_.spcRelationshipSpace / 2, father_.y);
                    }
                }
                else
                {
                    posRelationship_.X = father_.x + father_.getWidth(oGraphics, false, false) / 2;
                }
                break;

            case ConnectionMainPerson.MOTHER:
                // Position the father realive to the mother				
                posRelationship_.Y = mother_.y;
                if (father_ != null)
                {
                    if (!father_.isPositionKnown)
                    {
                        posRelationship_.X = mother_.GetSpousePosition(oGraphics, nIndex) - tree_.spcRelationshipSpace / 2;
                        father_.setPosition(oGraphics, posRelationship_.X - tree_.spcRelationshipSpace / 2 - father_.getWidth(oGraphics, false, false), mother_.y);
                    }
                }
                else
                {
                    posRelationship_.X = mother_.x + mother_.getWidth(oGraphics, false, false) / 2;
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
                    dTop = father_.y;
                }
                else
                {
                    dTop = mother_.y;
                }

                // The height of the bars changes for second marriages etc ...
                childBarHeight_ = dTop + tree_.spcHorizontalBarPos + 4 * index_;

                float dY;
                float dX_Base;
                float dX_Person;
                dY = dTop + tree_.spcPersonY;

                // Get the collection of rules
                clsTreeRule[] oRules = tree_.options.getRules();

                TreePerson[] oChildren = GetChildren();
                foreach (TreePerson oChild in oChildren)
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
                        if (oRule.personIndex == oChild.personIndex && oRule.action == clsTreeRule.RuleAction.HORIZONTAL_OFFSET)
                        {
                            dX_Base += oRule.parameterAsFloat * tree_.scalingFactor;
                            dNextChildWith += oRule.parameterAsFloat * tree_.scalingFactor;
                        }
                    }

                    // Offetset female children to make space for their partners
                    dX_Person = dX_Base + oChild.getHusbandSpace(oGraphics);

                    // Position the child
                    oChild.setPosition(oGraphics, dX_Person, dY);

                    // Calculate 2 positions for the next child (one for a child that needs space for descendants and one for a child that does not need dependant space)
                    // However, ensure that dNextChildWith does not decrease that space has already gone.
                    dNextChildOut = dX_Person + oChild.getWidth(oGraphics, false, false) + oChild.getWifeSpace(oGraphics) + tree_.spcSiblingSpace;
                    float dMinimumChildWith = dX_Base + oChild.getWidth(oGraphics, true, false) + tree_.spcSiblingSpace;
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
            float dY = MainPerson.y - tree_.spcPersonY;

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
                    if (!father_.isPositionKnown && !mother_.isPositionKnown)
                    {
                        dX = this.MainPerson.x + this.MainPerson.getWidth(oGraphics, false, false) / 2 - father_.getWidth(oGraphics, false, false) - tree_.spcRelationshipSpace / 2;
                        father_.setPosition(oGraphics, dX, dY);

                        dX += father_.getWidth(oGraphics, false, false);
                        posRelationship_.Y = dY;
                        posRelationship_.X = dX + tree_.spcRelationshipSpace / 2;

                        dX += tree_.spcRelationshipSpace;
                        mother_.setPosition(oGraphics, dX, dY);
                    }
                }
                else if (father_ != null)
                {
                    // Father only is known
                    if (!father_.isPositionKnown)
                    {
                        dX = MainPerson.x + MainPerson.getWidth(oGraphics, false, false) / 2 - father_.getWidth(oGraphics, false, false) / 2;
                        father_.setPosition(oGraphics, dX, dY);

                        posRelationship_.X = dX + father_.getWidth(oGraphics, false, false) / 2;
                        posRelationship_.Y = dY; //  + m_oTree.PersonHeight;
                    }
                }
                else
                {
                    // Mother only is known
                    if (!mother_.isPositionKnown)
                    {
                        dX = MainPerson.x + MainPerson.getWidth(oGraphics, false, false) / 2 - mother_.getWidth(oGraphics, false, false) / 2;
                        mother_.setPosition(oGraphics, dX, dY);

                        posRelationship_.X = dX + mother_.getWidth(oGraphics, false, false);
                        posRelationship_.Y = dY; // + m_oTree.PersonHeight;
                    }
                }
                break;

            case ConnectionMainPerson.CHILD_BOY:	// This is a random boy ancestor in the tree
                // If both parents are known
                if (father_ != null && mother_ != null)
                {
                    // Both parents are known
                    if (!father_.isPositionKnown && !mother_.isPositionKnown)
                    {

                        dX = MainPerson.x + MainPerson.getWidth(oGraphics, false, false) - mother_.getWidth(oGraphics, true, true);
                        mother_.setPosition(oGraphics, dX, dY);

                        dX -= tree_.spcRelationshipSpace;
                        posRelationship_.X = dX + tree_.spcRelationshipSpace / 2;
                        posRelationship_.Y = dY;

                        dX -= father_.getWidth(oGraphics, false, false);
                        father_.setPosition(oGraphics, dX, dY);
                    }
                }
                else if (father_ != null)
                {
                    // Father only is known
                    if (!father_.isPositionKnown)
                    {

                        dX = MainPerson.x + MainPerson.getWidth(oGraphics, false, false) - father_.getWidth(oGraphics, false, false);
                        father_.setPosition(oGraphics, dX, dY);

                        posRelationship_.X = dX + father_.getWidth(oGraphics, false, false) / 2;
                        posRelationship_.Y = dY; //  + m_oTree.PersonHeight;
                    }
                }
                else
                {
                    // Mother only is known
                    if (!mother_.isPositionKnown)
                    {

                        dX = MainPerson.x + MainPerson.getWidth(oGraphics, false, false) - mother_.getWidth(oGraphics, true, true);
                        mother_.setPosition(oGraphics, dX, dY);

                        posRelationship_.X = dX + mother_.getWidth(oGraphics, false, false);
                        posRelationship_.Y = dY; // + m_oTree.PersonHeight;
                    }
                }
                break;

            case ConnectionMainPerson.CHILD_GIRL:	// This is a random girl ancestor in the tree
                // If both parents are known
                if (father_ != null && mother_ != null)
                {
                    // Both parents are known
                    if (!father_.isPositionKnown && !mother_.isPositionKnown)
                    {

                        dX = this.MainPerson.x + father_.getWidth(oGraphics, true, true) - father_.getWidth(oGraphics, false, false);
                        father_.setPosition(oGraphics, dX, dY);

                        dX += father_.getWidth(oGraphics, false, false);
                        posRelationship_.X = dX + tree_.spcRelationshipSpace / 2;
                        posRelationship_.Y = dY;

                        dX += tree_.spcRelationshipSpace;
                        mother_.setPosition(oGraphics, dX, dY);
                    }
                }
                else if (father_ != null)
                {
                    // Father only is known
                    if (!father_.isPositionKnown)
                    {

                        dX = this.MainPerson.x + father_.getWidth(oGraphics, true, true) - father_.getWidth(oGraphics, false, false);
                        father_.setPosition(oGraphics, dX, dY);

                        posRelationship_.X = dX + father_.getWidth(oGraphics, false, false) / 2;
                        posRelationship_.Y = dY; //  + m_oTree.PersonHeight;
                    }
                }
                else
                {
                    // Mother only is known
                    if (!mother_.isPositionKnown)
                    {
                        // TODO: Not really sure about this.  I just copied it from above

                        dX = this.MainPerson.x + this.MainPerson.getWidth(oGraphics, false, false) - mother_.getWidth(oGraphics, true, true);
                        mother_.setPosition(oGraphics, dX, dY);

                        posRelationship_.X = dX + mother_.getWidth(oGraphics, false, false) / 2;
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
            TreePerson[] oSiblings = GetChildren();

            foreach (TreePerson oSibling in oSiblings)
            {
                if (bFirstSibling)
                {
                    bFirstSibling = false;
                    if (isMale_)
                    {
                        dLeftX = oSibling.x;
                        dRightX = oSibling.x + oSibling.getWidth(oGraphics) + oSibling.getPartnerSpace(oGraphics) + tree_.spcSiblingSpace;
                    }
                    else
                    {
                        dLeftX = oSibling.x - oSibling.getPartnerSpace(oGraphics);
                        dRightX = oSibling.x + oSibling.getWidth(oGraphics) + tree_.spcSiblingSpace;
                    }
                    dY = oSibling.y;
                }
                else
                {
                    if (!oSibling.isPositionKnown)
                    {
                        float dFullWidth = oSibling.getWidth(oGraphics, true, false);
                        float dNameWidth = oSibling.getWidth(oGraphics, false, false);

                        if (mainPersonType_ == ConnectionMainPerson.CHILD)
                        {
                            // Position the main person's siblings to left and right of him / according to age.
                            bOnLeft = !MainPerson.isOlder(oSibling.personIndex);
                        }
                        else
                        {
                            // Position all siblings to the left of a boy and to the right of a girl to keep them out of the way of spouses
                            bOnLeft = isMale_;
                        }

                        if (bOnLeft)
                        {
                            if (oSibling.isMale())
                            {
                                dLeftX -= (dFullWidth + tree_.spcSiblingSpace);
                            }
                            else
                            {
                                dLeftX -= (dNameWidth + tree_.spcSiblingSpace);
                            }
                            oSibling.setPosition(oGraphics, dLeftX, dY);
                            if (!oSibling.isMale())
                            {
                                dLeftX -= (dFullWidth - dNameWidth);
                            }
                        }
                        else
                        {
                            oSibling.setPosition(oGraphics, dRightX, dY);
                            dRightX += (oSibling.getWidth(oGraphics, true, false) + tree_.spcSiblingSpace);
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
                    TreePerson oChild = (TreePerson)children_[nChild];
                    dWidth += oChild.getWidth(oGraphics, false, false);
                    dWidth += oChild.getPartnerSpace(oGraphics);
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
                if (status_ == RelationshipStatus.MARRIED || status_ == RelationshipStatus.DIVORCED)
                {
                    graphics.DrawLine(tree_.penBlackThick, posRelationship_.X - tree_.relationshipSymbol - tree_.offsetX, posRelationship_.Y + tree_.spcRelationsMarker - tree_.offsetY, posRelationship_.X + tree_.relationshipSymbol - tree_.offsetX, posRelationship_.Y + tree_.spcRelationsMarker - tree_.offsetY);
                    graphics.DrawLine(tree_.penBlackThick, posRelationship_.X - tree_.relationshipSymbol - tree_.offsetX, posRelationship_.Y + tree_.spcRelationsMarker + 4 - tree_.offsetY, posRelationship_.X + tree_.relationshipSymbol - tree_.offsetX, posRelationship_.Y + tree_.spcRelationsMarker + 4 - tree_.offsetY);
                    if (status_ == RelationshipStatus.DIVORCED)
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
                    TreePerson oChild = (TreePerson)children_[nI];

                    // Draw the connections to all children (including main children)					
                    float X = oChild.x + oChild.getWidth(graphics, false, false) / 2;

                    switch (oChild.connection)
                    {
                    case TreePerson.ParentConnection.BOTH:
                        graphics.DrawLine(tree_.penBlackThick, X - tree_.offsetX, oChild.y - tree_.offsetY, X - tree_.offsetX, childBarHeight_ - tree_.offsetY);
                        graphics.DrawLine(tree_.penBlackThick, X - tree_.offsetX, childBarHeight_ - tree_.offsetY, posRelationship_.X - tree_.offsetX, childBarHeight_ - tree_.offsetY);
                        nDropLinesRequired |= 1;
                        break;

                    case TreePerson.ParentConnection.FATHER_ONLY:
                        graphics.DrawLine(tree_.penBlackThick, X - tree_.offsetX, oChild.y - tree_.offsetY, X - tree_.offsetX, childBarHeight_ - 4 - tree_.offsetY);
                        graphics.DrawLine(tree_.penBlackThick, X - tree_.offsetX, childBarHeight_ - 4 - tree_.offsetY, father_.x + father_.getWidth(graphics, false, false) / 2 - tree_.offsetX, childBarHeight_ - 4 - tree_.offsetY);
                        nDropLinesRequired |= 2;
                        break;

                    case TreePerson.ParentConnection.MOTHER_ONLY:
                        graphics.DrawLine(tree_.penBlackThick, X - tree_.offsetX, oChild.y - tree_.offsetY, X - tree_.offsetX, childBarHeight_ - 8 - tree_.offsetY);
                        graphics.DrawLine(tree_.penBlackThick, X - tree_.offsetX, childBarHeight_ - 8 - tree_.offsetY, mother_.x + mother_.getWidth(graphics, false, false) / 2 - tree_.offsetX, childBarHeight_ - 8 - tree_.offsetY);
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
                    graphics.DrawLine(tree_.penBlackThick, father_.x + father_.getWidth(graphics, false, false) / 2 - tree_.offsetX, father_.y + tree_.spcPersonHeight - tree_.offsetY, father_.x + father_.getWidth(graphics, false, false) / 2 - tree_.offsetX, childBarHeight_ - 4 - tree_.offsetY);
                }
                if ((nDropLinesRequired & 4) == 4)
                {
                    // Draw a line down from the mother to the (offset) horizontal bar
                    graphics.DrawLine(tree_.penBlackThick, mother_.x + mother_.getWidth(graphics, false, false) / 2 - tree_.offsetX, mother_.y + tree_.spcPersonHeight - tree_.offsetY, mother_.x + mother_.getWidth(graphics, false, false) / 2 - tree_.offsetX, childBarHeight_ - 8 - tree_.offsetY);
                }
            }

            // Return success
            return true;
        }

        #endregion

    }
}
