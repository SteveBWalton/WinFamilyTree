using System;
using System.Collections;
using family_tree.objects;

namespace family_tree.viewer
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

    /// <summary>Class to represent the connection between TreePerson objects in a TreeDocument object.  This will enable the connected people related to the main person to be drawn.  This should take care NOT to draw the main person will be drawn already.</summary>
    public class TreeConnection
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



        /// <summary>Class constructor.  Creates a new tree connection object.</summary>
        /// <param name="tree">Specifies the tree document that contains this connection.</param>
        /// <param name="person">Specifies the main person in this connection.</param>
        /// <param name="personType">Specifies the role of the main person in this connection.</param>
        /// <param name="index">Specify the count of this relationship in context of the main person.  Usually 0.</param>
        public TreeConnection(TreeDocument tree, TreePerson person, ConnectionMainPerson personType, int index)
        {
            // Save the input parameters.
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
                addChild(person);
                switch (mainPersonType_)
                {
                case ConnectionMainPerson.CHILD_BOY:
                    isMale_ = true;
                    break;

                case ConnectionMainPerson.CHILD_GIRL:
                    isMale_ = false;
                    break;

                case ConnectionMainPerson.CHILD:
                    Person mainPerson = new Person(person.personIndex, tree_.database);
                    isMale_ = mainPerson.isMale;
                    break;

                }
                break;
            }
        }



        #endregion

        #region Values / Adding Members



        /// <summary>Specifies the father in this connection object.</summary>
        /// <param name="father">Specifies the father object.</param>
        /// <returns>True for success, false otherwise.</returns>
        public bool addFather(TreePerson father)
        {
            father_ = father;

            // Return success.
            return true;
        }



        /// <summary>Specifies the mother in this connection object.</summary>
        /// <param name="mother">Specifies the mother object.</param>
        /// <returns>True for success, false otherwise.</returns>
        public bool addMother(TreePerson mother)
        {
            mother_ = mother;

            // Return success.
            return true;
        }



        /// <summary>Adds a child this tree connection object.</summary>
        /// <param name="child">Specifies the child object.</param>
        /// <returns>True for success, false otherwise.</returns>
        public bool addChild(TreePerson child)
        {
            if (children_ == null)
            {
                children_ = new ArrayList();
            }
            children_.Add(child);

            // Return success.
            return true;
        }



        /// <summary>Returns the index of the father in this connection.  Returns 0 for no father.</summary>
        /// <returns></returns>
        public int getFatherIndex()
        {
            if (father_ == null)
            {
                return 0;
            }
            return father_.personIndex;
        }



        /// <summary>Returns the children in this connection as an array of TreePerson objects.</summary>
        /// <returns></returns>
        public TreePerson[] getChildren()
        {
            if (children_ == null)
            {
                return new TreePerson[0];
            }
            return (TreePerson[])children_.ToArray(typeof(TreePerson));
        }



        /// <summary>Returns the index of the mother in this connection.  Returns 0 for no mother.</summary>
        /// <returns></returns>
        public int getMotherIndex()
        {
            if (mother_ == null)
            {
                return 0;
            }
            return mother_.personIndex;
        }



        /// <summary>Returns the father in this connection.</summary>
        public TreePerson father { get { return father_; } }

        /// <summary>Returns the mother in this connection.</summary>
        public TreePerson mother { get { return mother_; } }

        /// <summary>Returns the major person in this connection.  Cause of the connection existance.</summary>
        private TreePerson mainPerson { get { return (TreePerson)children_[0]; } }

        /// <summary>Returns the status of this connection.</summary>
        public RelationshipStatus status { get { return status_; } set { status_ = value; } }

        /// <summary>Returns the start date of this connection.</summary>
        public CompoundDate start { get { return start_; } set { start_ = value; } }

        #endregion

        #region Width Calculations



        /// <summary>Calculate the width of this connection without knowing the position of any of the members.</summary>
        /// <param name="graphics">Specify the graphic to draw on</param>
        /// <returns>The width required for this connection</returns>
        public float getWidth(System.Drawing.Graphics graphics)
        {
            // Calculate the width of the 2 parents.
            float maxWidth = getWidthParents(graphics);

            // Calcualte the width of the children.
            float width = getWidthChildren(graphics);

            // If the children are wider than the parents then use the children width.
            if (width > maxWidth)
            {
                maxWidth = width;
            }

            // Return the calculated width.
            return maxWidth;
        }



        /// <summary>Calculate the width required for the parents in this connection without knowing the position of any of the members.</summary>
        /// <param name="graphics">Specify the graphic to draw on.</param>
        /// <returns>The width required for the children in this connection.</returns>
        public float getWidthParents(System.Drawing.Graphics graphics)
        {
            float width = 0;
            if (father_ != null)
            {
                width += father_.getWidth(graphics, false, false);
                if (mother_ != null)
                {
                    width += tree_.spcRelationshipSpace;
                }
            }
            if (mother_ != null)
            {
                width += mother_.getWidth(graphics, false, false);
            }

            // Return the calculated width.
            return width;
        }



        /// <summary>Calculate the width required for the children in this connection without knowing the position of any of the members.</summary>
        /// <param name="graphics">Specify the graphic to draw on.</param>
        /// <returns>The width required for the children in this connection.</returns>
        public float getWidthChildren(System.Drawing.Graphics graphics)
        {
            float width = 0;

            // Calcualte the width of the children.
            if (children_ != null)
            {
                TreePerson[] children = getChildren();
                foreach (TreePerson child in children)
                {
                    width += child.getWidth(graphics, true, false) + tree_.spcSiblingSpace;
                }
            }

            // Return the calculated width of the children.
            return width;
        }



        #endregion

        #region Position Calculations



        /// <summary>Returns the X position that the spouse in this relations takes.  This accounts for the sex of the spouse (males are positioned on the left, females are postioned on the right).  This assumes that the main person is the one of the parents.</summary>
        /// <param name="graphics">Specifies the graphics device to draw on.</param>
        /// <returns>Returns the X position of the spouse in this connection object.</returns>
        public float spousePosition(System.Drawing.Graphics graphics)
        {
            switch (mainPersonType_)
            {
            case ConnectionMainPerson.FATHER:
                return mother_.x + mother_.getWidth(graphics, false, false);

            case ConnectionMainPerson.MOTHER:
                return father_.x;
            }

            // This is an error.
            return 0;
        }



        /// <summary>Returns the position that the next child in this connection (or attached to the main person) should take.</summary>
        /// <param name="graphics">Specify the device to draw on</param>
        /// <returns>The next available position for a child of the main person</returns>
        public float nextChild(System.Drawing.Graphics graphics)
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
            TreePerson lastChild = (TreePerson)children_[children_.Count - 1];
            return lastChild.x + lastChild.getWidth(graphics, true, false);
        }



        /// <summary>Calculates and sets the positon of the partner of a person who is already fixed.  The partner is positioned only if they are not already positioned.</summary>
        /// <param name="graphics">Specify the graphics device to draw on.</param>
        /// <param name="index">Specify the index of the partner.</param>
        /// <returns>True for success, false otherwise.</returns>		
        public bool setPartnerPosition(System.Drawing.Graphics graphics, int index)
        {
            switch (mainPersonType_)
            {
            case ConnectionMainPerson.FATHER:
                // Position the mother relative to the father.
                posRelationship_.Y = father_.y;
                if (mother_ != null)
                {
                    if (!mother_.isPositionKnown)
                    {
                        posRelationship_.X = father_.GetSpousePosition(graphics, index) + tree_.spcRelationshipSpace / 2;

                        mother_.setPosition(graphics, posRelationship_.X + tree_.spcRelationshipSpace / 2, father_.y);
                    }
                }
                else
                {
                    posRelationship_.X = father_.x + father_.getWidth(graphics, false, false) / 2;
                }
                break;

            case ConnectionMainPerson.MOTHER:
                // Position the father realive to the mother.
                posRelationship_.Y = mother_.y;
                if (father_ != null)
                {
                    if (!father_.isPositionKnown)
                    {
                        posRelationship_.X = mother_.GetSpousePosition(graphics, index) - tree_.spcRelationshipSpace / 2;
                        father_.setPosition(graphics, posRelationship_.X - tree_.spcRelationshipSpace / 2 - father_.getWidth(graphics, false, false), mother_.y);
                    }
                }
                else
                {
                    posRelationship_.X = mother_.x + mother_.getWidth(graphics, false, false) / 2;
                }
                // CalculateChildPositionFixedParents(oGraphics,nIndex,ref dNextChild);
                break;

            case ConnectionMainPerson.CHILD_BOY:
            case ConnectionMainPerson.CHILD_GIRL:
            case ConnectionMainPerson.CHILD:
                // This is an error.
                throw (new System.Exception("Children not allowed here"));
            }

            // Return success.
            return true;
        }



        /// <summary>Calculate and sets the positons of the children of a person who already fixed.  The children are only positioned if they are not already positioned.</summary>
        /// <returns>True for success, false otherwise.</returns>
        public bool setChildrenPosition
            (
            System.Drawing.Graphics graphics,
            int index,
            ref float nextChildWith,
            ref float nextChildOut
            )
        {
            // Top of this connection.
            float top;

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
                    top = father_.y;
                }
                else
                {
                    top = mother_.y;
                }

                // The height of the bars changes for second marriages etc ...
                childBarHeight_ = top + tree_.spcHorizontalBarPos + 4 * index_;

                float y;
                float xBase;
                float xPerson;
                y = top + tree_.spcPersonY;

                // Get the collection of rules.
                TreeRule[] rules = tree_.options.getRules();

                TreePerson[] children = getChildren();
                foreach (TreePerson child in children)
                {
                    if (child.hasDescendants())
                    {
                        xBase = nextChildWith;
                    }
                    else
                    {
                        xBase = nextChildOut;
                    }

                    // Check for any rules to be applied.
                    foreach (TreeRule rule in rules)
                    {
                        if (rule.personIndex == child.personIndex && rule.action == TreeRule.RuleAction.HORIZONTAL_OFFSET)
                        {
                            xBase += rule.parameterAsFloat * tree_.scalingFactor;
                            nextChildWith += rule.parameterAsFloat * tree_.scalingFactor;
                        }
                    }

                    // Offetset female children to make space for their partners.
                    xPerson = xBase + child.getHusbandSpace(graphics);

                    // Position the child.
                    child.setPosition(graphics, xPerson, y);

                    // Calculate 2 positions for the next child (one for a child that needs space for descendants and one for a child that does not need dependant space).
                    // However, ensure that dNextChildWith does not decrease that space has already gone.
                    nextChildOut = xPerson + child.getWidth(graphics, false, false) + child.getWifeSpace(graphics) + tree_.spcSiblingSpace;
                    float minimumChildWith = xBase + child.getWidth(graphics, true, false) + tree_.spcSiblingSpace;
                    if (minimumChildWith > nextChildWith)
                    {
                        nextChildWith = minimumChildWith;
                    }
                }

                break;

            case ConnectionMainPerson.CHILD_BOY:
            case ConnectionMainPerson.CHILD_GIRL:
            case ConnectionMainPerson.CHILD:
                // This is an error.
                throw (new System.Exception("Children not allowed here"));
            }

            // Return success.
            return true;
        }



        /// <summary>Calculates the positions for the person parents.  This should only be called for a person's ancestors.</summary>
        /// <param name="graphics">Specifies the device where the connection will be drawn</param>
        /// <returns></returns>
        public bool calculatePositionsParents(System.Drawing.Graphics graphics)
        {
            // If no parents then nothing to do in all cases.
            if (father_ == null && mother_ == null)
            {
                // No Parents to position.
                return true;
            }

            // The y position of all types of parent will be the same.
            float x = 0;
            float y = mainPerson.y - tree_.spcPersonY;

            // Only one bar per ancestor so always the same height.
            // The height is varied when an "uncle" is only attached to one of the "parents" in the drawing routine.  The 4*m_nIndex has no effect since m_nIndex is alway 0.
            childBarHeight_ = y + tree_.spcHorizontalBarPos + 4 * index_;

            switch (mainPersonType_)
            {
            case ConnectionMainPerson.FATHER:
            case ConnectionMainPerson.MOTHER:
                throw (new System.Exception("Parents not allowed here"));

            case ConnectionMainPerson.CHILD:	// This is the main person in the tree.
                // If both parents are known.
                if (father_ != null && mother_ != null)
                {
                    // Both parents are known.
                    if (!father_.isPositionKnown && !mother_.isPositionKnown)
                    {
                        x = this.mainPerson.x + this.mainPerson.getWidth(graphics, false, false) / 2 - father_.getWidth(graphics, false, false) - tree_.spcRelationshipSpace / 2;
                        father_.setPosition(graphics, x, y);

                        x += father_.getWidth(graphics, false, false);
                        posRelationship_.Y = y;
                        posRelationship_.X = x + tree_.spcRelationshipSpace / 2;

                        x += tree_.spcRelationshipSpace;
                        mother_.setPosition(graphics, x, y);
                    }
                }
                else if (father_ != null)
                {
                    // Father only is known.
                    if (!father_.isPositionKnown)
                    {
                        x = mainPerson.x + mainPerson.getWidth(graphics, false, false) / 2 - father_.getWidth(graphics, false, false) / 2;
                        father_.setPosition(graphics, x, y);

                        posRelationship_.X = x + father_.getWidth(graphics, false, false) / 2;
                        posRelationship_.Y = y; //  + m_oTree.PersonHeight;
                    }
                }
                else
                {
                    // Mother only is known.
                    if (!mother_.isPositionKnown)
                    {
                        x = mainPerson.x + mainPerson.getWidth(graphics, false, false) / 2 - mother_.getWidth(graphics, false, false) / 2;
                        mother_.setPosition(graphics, x, y);

                        posRelationship_.X = x + mother_.getWidth(graphics, false, false);
                        posRelationship_.Y = y; // + m_oTree.PersonHeight;
                    }
                }
                break;

            case ConnectionMainPerson.CHILD_BOY:	// This is a random boy ancestor in the tree.
                // If both parents are known.
                if (father_ != null && mother_ != null)
                {
                    // Both parents are known.
                    if (!father_.isPositionKnown && !mother_.isPositionKnown)
                    {

                        x = mainPerson.x + mainPerson.getWidth(graphics, false, false) - mother_.getWidth(graphics, true, true);
                        mother_.setPosition(graphics, x, y);

                        x -= tree_.spcRelationshipSpace;
                        posRelationship_.X = x + tree_.spcRelationshipSpace / 2;
                        posRelationship_.Y = y;

                        x -= father_.getWidth(graphics, false, false);
                        father_.setPosition(graphics, x, y);
                    }
                }
                else if (father_ != null)
                {
                    // Father only is known.
                    if (!father_.isPositionKnown)
                    {

                        x = mainPerson.x + mainPerson.getWidth(graphics, false, false) - father_.getWidth(graphics, false, false);
                        father_.setPosition(graphics, x, y);

                        posRelationship_.X = x + father_.getWidth(graphics, false, false) / 2;
                        posRelationship_.Y = y; //  + m_oTree.PersonHeight;
                    }
                }
                else
                {
                    // Mother only is known.
                    if (!mother_.isPositionKnown)
                    {

                        x = mainPerson.x + mainPerson.getWidth(graphics, false, false) - mother_.getWidth(graphics, true, true);
                        mother_.setPosition(graphics, x, y);

                        posRelationship_.X = x + mother_.getWidth(graphics, false, false);
                        posRelationship_.Y = y; // + m_oTree.PersonHeight;
                    }
                }
                break;

            case ConnectionMainPerson.CHILD_GIRL:	// This is a random girl ancestor in the tree.
                // If both parents are known.
                if (father_ != null && mother_ != null)
                {
                    // Both parents are known.
                    if (!father_.isPositionKnown && !mother_.isPositionKnown)
                    {

                        x = this.mainPerson.x + father_.getWidth(graphics, true, true) - father_.getWidth(graphics, false, false);
                        father_.setPosition(graphics, x, y);

                        x += father_.getWidth(graphics, false, false);
                        posRelationship_.X = x + tree_.spcRelationshipSpace / 2;
                        posRelationship_.Y = y;

                        x += tree_.spcRelationshipSpace;
                        mother_.setPosition(graphics, x, y);
                    }
                }
                else if (father_ != null)
                {
                    // Father only is known.
                    if (!father_.isPositionKnown)
                    {

                        x = this.mainPerson.x + father_.getWidth(graphics, true, true) - father_.getWidth(graphics, false, false);
                        father_.setPosition(graphics, x, y);

                        posRelationship_.X = x + father_.getWidth(graphics, false, false) / 2;
                        posRelationship_.Y = y; //  + m_oTree.PersonHeight;
                    }
                }
                else
                {
                    // Mother only is known.
                    if (!mother_.isPositionKnown)
                    {
                        // TODO: Not really sure about this.  I just copied it from above.

                        x = this.mainPerson.x + this.mainPerson.getWidth(graphics, false, false) - mother_.getWidth(graphics, true, true);
                        mother_.setPosition(graphics, x, y);

                        posRelationship_.X = x + mother_.getWidth(graphics, false, false) / 2;
                        posRelationship_.Y = y; // + m_oTree.PersonHeight;
                    }
                }
                break;
            }

            // Position the siblings.
            // The first sibling will be the main person and is the reference position for the other siblings.
            float leftX = 0;
            float rightX = 0;
            bool isOnLeft = true;
            bool isFirstSibling = true;
            TreePerson[] siblings = getChildren();

            foreach (TreePerson sibling in siblings)
            {
                if (isFirstSibling)
                {
                    isFirstSibling = false;
                    if (isMale_)
                    {
                        leftX = sibling.x;
                        rightX = sibling.x + sibling.getWidth(graphics) + sibling.getPartnerSpace(graphics) + tree_.spcSiblingSpace;
                    }
                    else
                    {
                        leftX = sibling.x - sibling.getPartnerSpace(graphics);
                        rightX = sibling.x + sibling.getWidth(graphics) + tree_.spcSiblingSpace;
                    }
                    y = sibling.y;
                }
                else
                {
                    if (!sibling.isPositionKnown)
                    {
                        float dFullWidth = sibling.getWidth(graphics, true, false);
                        float dNameWidth = sibling.getWidth(graphics, false, false);

                        if (mainPersonType_ == ConnectionMainPerson.CHILD)
                        {
                            // Position the main person's siblings to left and right of him / according to age.
                            isOnLeft = !mainPerson.isOlder(sibling.personIndex);
                        }
                        else
                        {
                            // Position all siblings to the left of a boy and to the right of a girl to keep them out of the way of spouses.
                            isOnLeft = isMale_;
                        }

                        if (isOnLeft)
                        {
                            if (sibling.isMale())
                            {
                                leftX -= (dFullWidth + tree_.spcSiblingSpace);
                            }
                            else
                            {
                                leftX -= (dNameWidth + tree_.spcSiblingSpace);
                            }
                            sibling.setPosition(graphics, leftX, y);
                            if (!sibling.isMale())
                            {
                                leftX -= (dFullWidth - dNameWidth);
                            }
                        }
                        else
                        {
                            sibling.setPosition(graphics, rightX, y);
                            rightX += (sibling.getWidth(graphics, true, false) + tree_.spcSiblingSpace);
                        }
                    }
                }
            }

            // Return success.
            return true;
        }



        /// <summary>Returns the space that would be required to display the children on the specified graphics device.</summary>
        /// <param name="graphics">Specifies the graphics device that the children would be drawn on.</param>
        /// <returns>The horizontal space required to display the children.</returns>
        public float getChildrenSpace(System.Drawing.Graphics graphics)
        {
            float width = 0;
            if (children_ != null)
            {
                for (int i = 0; i < children_.Count; i++)
                {
                    TreePerson child = (TreePerson)children_[i];
                    width += child.getWidth(graphics, false, false);
                    width += child.getPartnerSpace(graphics);
                    if (i > 0)
                    {
                        width += tree_.spcSiblingSpace;
                    }
                }
            }
            return width;
        }



        #endregion

        #region Drawing



        /// <summary>Draws the connection object.  Draws the lines for the connection object.  Draws the people in the connection object except the MAIN person.</summary>
        /// <param name="graphics">Specifies the graphics device to draw the connection object on.</param>
        /// <returns>True for success, false otherwise</returns>
        public bool draw(System.Drawing.Graphics graphics)
        {
            // Draw a connection between the mother and father.
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

                    // Write the start date.
                    if (start_ != null)
                    {
                        if (!start_.isEmpty())
                        {
                            // TODO: Calculate the correct position for this text.
                            int theYear = CompoundDate.getYear(start_.date);
                            string displayYear;
                            if (theYear < 0)
                            {
                                displayYear = (-theYear).ToString() + "BC";
                            }
                            else
                            {
                                displayYear = theYear.ToString();
                            }
                            graphics.DrawString(displayYear, tree_.fontDescription, tree_.brushBlack, posRelationship_.X - tree_.spcRelationshipSpace / 2 - tree_.offsetX, posRelationship_.Y - 4 - tree_.offsetY);
                        }
                    }
                }
            }

            // Draw the children (need to check the mainperson flag).
            if (children_ != null)
            {
                int dropLinesRequired = 0;

                // Draw the children.
                for (int i = 0; i < children_.Count; i++)
                {
                    TreePerson child = (TreePerson)children_[i];

                    // Draw the connections to all children (including main children).
                    float x = child.x + child.getWidth(graphics, false, false) / 2;

                    switch (child.connection)
                    {
                    case TreePerson.ParentConnection.BOTH:
                        graphics.DrawLine(tree_.penBlackThick, x - tree_.offsetX, child.y - tree_.offsetY, x - tree_.offsetX, childBarHeight_ - tree_.offsetY);
                        graphics.DrawLine(tree_.penBlackThick, x - tree_.offsetX, childBarHeight_ - tree_.offsetY, posRelationship_.X - tree_.offsetX, childBarHeight_ - tree_.offsetY);
                        dropLinesRequired |= 1;
                        break;

                    case TreePerson.ParentConnection.FATHER_ONLY:
                        graphics.DrawLine(tree_.penBlackThick, x - tree_.offsetX, child.y - tree_.offsetY, x - tree_.offsetX, childBarHeight_ - 4 - tree_.offsetY);
                        graphics.DrawLine(tree_.penBlackThick, x - tree_.offsetX, childBarHeight_ - 4 - tree_.offsetY, father_.x + father_.getWidth(graphics, false, false) / 2 - tree_.offsetX, childBarHeight_ - 4 - tree_.offsetY);
                        dropLinesRequired |= 2;
                        break;

                    case TreePerson.ParentConnection.MOTHER_ONLY:
                        graphics.DrawLine(tree_.penBlackThick, x - tree_.offsetX, child.y - tree_.offsetY, x - tree_.offsetX, childBarHeight_ - 8 - tree_.offsetY);
                        graphics.DrawLine(tree_.penBlackThick, x - tree_.offsetX, childBarHeight_ - 8 - tree_.offsetY, mother_.x + mother_.getWidth(graphics, false, false) / 2 - tree_.offsetX, childBarHeight_ - 8 - tree_.offsetY);
                        dropLinesRequired |= 4;
                        break;

                    }
                }

                // Draw the drop lines.
                if ((dropLinesRequired & 1) == 1)
                {
                    if (father_ != null && mother_ != null)
                    {
                        // Draw a line down from the married parents to the horizontal bar.
                        graphics.DrawLine(tree_.penBlackThick, posRelationship_.X - tree_.offsetX, posRelationship_.Y + tree_.spcRelationsMarker + 10 - tree_.offsetY, posRelationship_.X - tree_.offsetX, childBarHeight_ - tree_.offsetY);
                    }
                    else
                    {
                        // Draw a line from the single parent to the horizontal bar.
                        graphics.DrawLine(tree_.penBlackThick, posRelationship_.X - tree_.offsetX, posRelationship_.Y + tree_.spcPersonHeight - tree_.offsetY, posRelationship_.X - tree_.offsetX, childBarHeight_ - tree_.offsetY);
                    }
                }
                if ((dropLinesRequired & 2) == 2)
                {
                    // Draw a line down from the father to the (offset) horizontal bar.
                    graphics.DrawLine(tree_.penBlackThick, father_.x + father_.getWidth(graphics, false, false) / 2 - tree_.offsetX, father_.y + tree_.spcPersonHeight - tree_.offsetY, father_.x + father_.getWidth(graphics, false, false) / 2 - tree_.offsetX, childBarHeight_ - 4 - tree_.offsetY);
                }
                if ((dropLinesRequired & 4) == 4)
                {
                    // Draw a line down from the mother to the (offset) horizontal bar.
                    graphics.DrawLine(tree_.penBlackThick, mother_.x + mother_.getWidth(graphics, false, false) / 2 - tree_.offsetX, mother_.y + tree_.spcPersonHeight - tree_.offsetY, mother_.x + mother_.getWidth(graphics, false, false) / 2 - tree_.offsetX, childBarHeight_ - 8 - tree_.offsetY);
                }
            }

            // Return success.
            return true;
        }



        #endregion

    }
}
