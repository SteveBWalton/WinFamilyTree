using System;
using family_tree.objects;

namespace family_tree.viewer
{
    /// <summary>Class to represent a person in a tree document.</summary>
    public class TreePerson
    {
        #region Member Variables

        #region Supporting Types etc ...

        /// <summary>Type of connection to parents that this person has.</summary>
        public enum ParentConnection
        {
            /// <summary>Connection to a pair of parents.  Mother and Father.  Expected.</summary>
            BOTH,
            /// <summary>Connection to father only.</summary>
            FATHER_ONLY,
            /// <summary>Connection to mother only.</summary>
            MOTHER_ONLY
        }

        #endregion

        /// <summary>Tree document that this person is attached to.</summary>
        TreeDocument tree_;

        /// <summary>ID of this person in the database.</summary>
        int personIndex_;

        /// <summary>Name of this person.</summary>
        string name_;

        /// <summary>Name of this person including the lived years.</summary>
        private string nameWithYears_;

        /// <summary>Description of this person.</summary>
        string description_;

        /// <summary>True if this person is male.  False, otherwise.</summary>
        bool isMale_;

        /// <summary>Type of connection to parent(s).</summary>
        ParentConnection connection_;

        /// <summary>Position of this tree person.</summary>
        System.Drawing.PointF pos_;

        /// <summary>Array of connections to descendants.</summary>
        TreeConnection[] descendants_;

        /// <summary>Connection to the ancestors of this tree person.</summary>
        TreeConnection ancestors_;

        /// <summary>True if the position of this person is known.</summary>
        private bool isPositionKnown_;

        #endregion

        #region Constructors etc ...



        /// <summary>Create a person for a tree document.</summary>
        /// <param name="tree">Specify the tree document that contains the person.</param>
        /// <param name="personIndex">Specify the ID of the person.</param>
        public TreePerson(TreeDocument tree, int personIndex)
        {
            // Save the supplied values.
            tree_ = tree;
            personIndex_ = personIndex;

            // Initialise the object.
            pos_ = new System.Drawing.PointF(0, 0);
            descendants_ = null;
            ancestors_ = null;
            connection_ = ParentConnection.BOTH;

            // Get the information required from the database.
            Person person = new Person(personIndex, tree_.database);
            name_ = person.getName(false, true);
            description_ = person.shortDescription(false);
            nameWithYears_ = person.getName(true, true);
            isMale_ = person.isMale;
        }



        #endregion

        #region Calculate Widths

        // I put the widths into a different section to positions since they are much simpler.



        /// <summary>Returns the width required for the person.  If IncludeDescendants then the function works recursively to calculate the space siblings should leave for this person.  Otherwise the function simply calculates the space for the box around the person's name.</summary>
        /// <param name="graphics">Specify the device where the person will be drawn.</param>
        /// <param name="isIncludeDescendants">Returns the space required to fit the person's descendants in.</param>
        /// <param name="isIncludeAncestors">Returns the space required to fit the person's ancestors in.</param>
        /// <returns>Return the horizontal space required for this person.</returns>
        public float getWidth(System.Drawing.Graphics graphics, bool isIncludeDescendants, bool isIncludeAncestors)
        {
            // The maximum width of the person.
            float maxWidth = 0;

            // Check the width of this person.
            System.Drawing.SizeF size = graphics.MeasureString(name_, tree_.fontName);
            if (size.Width > maxWidth)
            {
                maxWidth = size.Width;
            }
            size = graphics.MeasureString(description_, tree_.fontDescription);
            if (size.Width > maxWidth)
            {
                maxWidth = size.Width;
            }

            // Check the width of descendants.
            if (isIncludeDescendants)
            {
                if (descendants_ != null)
                {
                    // Expect that the children will be wider than the parents but it may not be the case.
                    float widthChildren = 0;
                    float widthParents = maxWidth;
                    foreach (TreeConnection descendant in descendants_)
                    {
                        widthChildren += descendant.getWidthChildren(graphics);
                        widthParents += descendant.getWidthParents(graphics) - maxWidth;
                    }
                    float width = Math.Max(widthChildren, widthParents);

                    if (width > maxWidth)
                    {
                        maxWidth = width;
                    }
                }
            }

            // Check the width of ancestors.
            if (isIncludeAncestors)
            {
                // Check the width of the parents.
                if (ancestors_ != null)
                {
                    float width = 0;
                    if (ancestors_.father != null)
                    {
                        width += ancestors_.father.getWidth(graphics, false, true);
                        if (ancestors_.mother != null)
                        {
                            width += tree_.spcRelationshipSpace;
                        }
                    }
                    if (ancestors_.mother != null)
                    {
                        width += ancestors_.mother.getWidth(graphics, false, true);
                    }

                    if (width > maxWidth)
                    {
                        maxWidth = width;
                    }

                    // Check the width of siblings.
                    width = ancestors_.getChildrenSpace(graphics);
                    if (width > maxWidth)
                    {
                        maxWidth = width;
                    }
                }
            }

            // Return the calculated width.
            return maxWidth;
        }



        /// <summary>Returns the width of the only the person not including descendants or ancestors.</summary>
        /// <param name="graphics">Specify the device where the person will be drawn.</param>
        /// <returns>The horizontal width of the name of this person (or the description).</returns>
        public float getWidth(System.Drawing.Graphics graphics)
        {
            return getWidth(graphics, false, false);
        }



        #endregion

        #region Calculate Positions



        /// <summary>Calculate the positions of all the people connected to this person in the tree.  This is a completely different approach to the prevous version.  Given that you know this position of this person then position his direct relations.</summary>
        /// <param name="graphics">Specify the device to calculate the positions on</param>
        /// <returns>True for success, false otherwise</returns>
        public bool calculatePosition(System.Drawing.Graphics graphics)
        {
            // Check that this person is already known
            if (!isPositionKnown_)
            {
                return false;
            }

            // Calculate the positions for descendant partners
            float nextChildWith = pos_.X;
            if (descendants_ != null)
            {
                int partnerCount = 0;
                foreach (TreeConnection descendant in descendants_)
                {
                    // Position the partner relative to this person.
                    descendant.setPartnerPosition(graphics, partnerCount);

                    // Line the children up with the leftmost man.
                    if (descendant.father != null)
                    {
                        if (descendant.father.x < nextChildWith)
                        {
                            nextChildWith = descendant.father.x;
                        }
                    }
                    partnerCount++;
                }
            }

            // Calculate the position for descendant children.
            float nextChildOut = nextChildWith;
            // Calculate the position.
            if (descendants_ != null)
            {
                int partnerCount = 0;
                foreach (TreeConnection descendant in descendants_)
                {
                    descendant.setChildrenPosition(graphics, partnerCount, ref nextChildWith, ref nextChildOut);
                    partnerCount++;
                }
            }

            // Calculate the position for the parents (ancestors).
            if (ancestors_ != null)
            {
                ancestors_.calculatePositionsParents(graphics);
            }

            return false;
        }



        /// <summary>Returns the X position that the spouse in the relationship specified by nIndex should take.  Returns the X position that the spouse in the relationship specified by nIndex should take.  This accounts for the sex of the spouse (males are positioned on the left, females on the right).</summary>
        /// <param name="graphics">Specifies the graphics device to draw on.</param>
        /// <param name="spouseIndex">Specifies the index of the spouse.</param>
        /// <returns>The X position that the spouse should take.</returns>
        public float GetSpousePosition(System.Drawing.Graphics graphics, int spouseIndex)
        {
            if (spouseIndex == 0)
            {
                if (isMale_)
                {
                    // Return the right edge of the person.
                    return pos_.X + getWidth(graphics);
                }
                else
                {
                    // Return the left edge of the person.
                    return pos_.X;
                }
            }
            // Check that a spouse exists.  If not then look at the earlier connection.
            TreeConnection treeConnection = (TreeConnection)descendants_[spouseIndex - 1];
            if (isMale_)
            {
                if (treeConnection.mother == null)
                {
                    return GetSpousePosition(graphics, spouseIndex - 1);
                }
            }
            else
            {
                if (treeConnection.father == null)
                {
                    return GetSpousePosition(graphics, spouseIndex - 1);
                }
            }

            // Return the position of the previous spouse
            return treeConnection.spousePosition(graphics);
        }



        /// <summary>NOT IMPLEMENTED.  Returns the position that the first child in the relationship specified by nIndex should take.  Returns the position that the first child in the relationship specified by nIndex should take.</summary>
        /// <param name="graphics"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public float getFirstChildPosition(System.Drawing.Graphics graphics, int index)
        {
            return 0;
        }



        /// <summary>Return the extra space that the person needs to make space for her husbands on the left.</summary>
        /// <param name="graphics">Device that the husbands would be draw on</param>
        /// <returns>Width of the graphical representation of the husbands</returns>
        public float getHusbandSpace(System.Drawing.Graphics graphics)
        {
            if (isMale_)
            {
                return 0;
            }
            return getPartnerSpace(graphics);
        }



        /// <summary>Returns the extra space that the person needs to make space for his wives on the right.</summary>
        /// <param name="graphics">Device that the wives would be draw on</param>
        /// <returns>Width of the graphical representation of the wives</returns>
        public float getWifeSpace(System.Drawing.Graphics graphics)
        {
            if (!isMale_)
            {
                return 0;
            }
            return getPartnerSpace(graphics);
        }



        /// <summary>Returns the space required for the partners of this person.</summary>
        /// <param name="graphics">Device to that the partners would be draw on</param>
        /// <returns>The width that would be required to draw the partner of the current person</returns>
        public float getPartnerSpace(System.Drawing.Graphics graphics)
        {
            if (descendants_ == null)
            {
                return 0;
            }
            float offset = 0;
            TreePerson partner;
            for (int i = 0; i < descendants_.Length; i++)
            {
                if (isMale_)
                {
                    partner = ((TreeConnection)descendants_[i]).mother;
                }
                else
                {
                    partner = ((TreeConnection)descendants_[i]).father;
                }
                if (partner != null)
                {
                    offset += partner.getWidth(graphics, false, false);

                    // Not sure this should be inside or outside this if condition
                    // Do we need a relationship marker if don't have the person
                    offset += tree_.spcRelationshipSpace;
                }
            }
            return offset;
        }



        /// <summary>Set the position of this person on the tree once it has been calculated.  The tree is notified so that it can scroll to include this person.  The person is marked as positioned so that other people can locate themselves in relation to this person.</summary>
        /// <param name="graphics">Specify the device.</param>
        /// <param name="x">Specify the X position of the person.</param>
        /// <param name="y">Specify the Y position of the person.</param>
        public void setPosition(System.Drawing.Graphics graphics, float x, float y)
        {
            // Debuging message
            // Console.WriteLine(m_sName + " is at (" + X.ToString() + "," + Y.ToString() + ")");

            // Set the position
            isPositionKnown_ = true;
            pos_.X = x;
            pos_.Y = y;

            // Notify the tree with the position of this person
            tree_.notifyPosition(pos_.X, pos_.Y, pos_.X + getWidth(graphics), pos_.Y + tree_.spcPersonHeight);
        }



        #endregion

        #region Drawing



        /// <summary>Draws the specified person.  Previously this would also draw his relations but it does not anymore.</summary>
        /// <param name="graphics">Specifies the graphics device to draw the person on to.</param>
        /// <returns>True for success, false otherwise.</returns>
        public bool draw(System.Drawing.Graphics graphics)
        {
            if (isPositionKnown_)
            {
                // Console.WriteLine("Drawing " + m_sName);

                // Draw a box around the person (useful debugging)
                if (tree_.isPersonBox)
                {
                    if (isMale_)
                    {
                        graphics.FillRectangle(tree_.brushBoy, pos_.X - tree_.offsetX, pos_.Y - tree_.offsetY, getWidth(graphics), tree_.spcPersonHeight);
                    }
                    else
                    {
                        graphics.FillRectangle(tree_.brushGirl, pos_.X - tree_.offsetX, pos_.Y - tree_.offsetY, getWidth(graphics), tree_.spcPersonHeight);
                    }

                    graphics.DrawLine(tree_.penBlackThin, pos_.X - tree_.offsetX, pos_.Y - tree_.offsetY, pos_.X - tree_.offsetX + getWidth(graphics), pos_.Y - tree_.offsetY);
                    graphics.DrawLine(tree_.penBlackThin, pos_.X - tree_.offsetX + getWidth(graphics), pos_.Y - tree_.offsetY, pos_.X - tree_.offsetX + getWidth(graphics), pos_.Y - tree_.offsetY + tree_.spcPersonHeight);
                    graphics.DrawLine(tree_.penBlackThin, pos_.X - tree_.offsetX + getWidth(graphics), pos_.Y - tree_.offsetY + tree_.spcPersonHeight, pos_.X - tree_.offsetX, pos_.Y - tree_.offsetY + tree_.spcPersonHeight);
                    graphics.DrawLine(tree_.penBlackThin, pos_.X - tree_.offsetX, pos_.Y - tree_.offsetY + tree_.spcPersonHeight, pos_.X - tree_.offsetX, pos_.Y - tree_.offsetY);
                }

                // Draw the person's name and description
                graphics.DrawString(name_, tree_.fontName, tree_.brushBlack, pos_.X - tree_.offsetX, pos_.Y - tree_.offsetY);
                graphics.DrawString(description_, tree_.fontDescription, tree_.brushBlack, pos_.X - tree_.offsetX, pos_.Y + tree_.fontName.Height - tree_.offsetY);
            }
            else
            {
                Console.WriteLine("Can't draw " + name_ + " position unknown.");
            }

            // Return success
            return true;
        }



        #endregion

        #region Building and Values



        /// <summary>Returns true if this person has any descendants in the database.  False, otherwise.</summary>
        /// <returns>True if this person has descendants, false otherwise.</returns>
        public bool hasDescendants()
        {
            if (descendants_ == null)
            {
                return false;
            }
            if (descendants_.Length == 0)
            {
                return false;
            }
            return true;
        }



        /// <summary>Add the descendants of this person to the current document.</summary>
        /// <param name="rules">Specify the current set of document rules.</param>
        public void addDescendants(TreeRule[] rules)
        {
            foreach (TreeRule rule in rules)
            {
                // Check that this person's descendants have not been excluded.
                if (rule.action == TreeRule.RuleAction.EXCLUDE_DESCENDANTS && rule.personIndex == personIndex_)
                {
                    return;
                }
            }

            // Get this person.
            Person person = new Person(personIndex_, tree_.database);

            // Add the partners to the person
            ConnectionMainPerson personType;
            if (person.isMale)
            {
                personType = ConnectionMainPerson.FATHER;
            }
            else
            {
                personType = ConnectionMainPerson.MOTHER;
            }
            Relationship[] relationships = person.getRelationships();
            descendants_ = new TreeConnection[relationships.Length];
            int relationshipIndex;
            for (int i = 0; i < relationships.Length; i++)
            {
                if (isMale())
                {
                    relationshipIndex = relationships.Length - 1 - i;
                }
                else
                {
                    relationshipIndex = i;
                }
                descendants_[i] = new TreeConnection(tree_, this, personType, i);
                tree_.addFamily(descendants_[i]);
                if (person.isMale)
                {
                    TreePerson mother = new TreePerson(tree_, relationships[relationshipIndex].partnerIndex);
                    descendants_[i].addMother(mother);
                    tree_.addPerson(mother);
                }
                else
                {
                    TreePerson father = new TreePerson(tree_, relationships[relationshipIndex].partnerIndex);
                    descendants_[i].addFather(father);
                    tree_.addPerson(father);
                }
                if (relationships[relationshipIndex].terminatedIndex == 2)
                {
                    descendants_[i].status = RelationshipStatus.DIVORCED;
                }
                if (!relationships[relationshipIndex].start.isEmpty())
                {
                    descendants_[i].start = relationships[relationshipIndex].start;
                }
            }

            // Add children to the person.
            int[] children = person.getChildren();
            for (int i = 0; i < children.Length; i++)
            {
                // Decide which relationship / connections this child belongs to.
                int connection = getDescendantsIndexForChild(children[i]);

                // Create a person in the tree document for this child.
                TreePerson child = new TreePerson(tree_, children[i]);
                tree_.addPerson(child);

                // Add the child to the selected relationship / connection.
                descendants_[connection].addChild(child);
                
                // Add the descendants of this child.
                child.addDescendants(rules);
            }
        }



        /// <summary>Returns the index of the connection object that the specified child should be added to.</summary>
		/// <param name="childIndex">Specifies the ID of the child</param>
		/// <returns>The index of a connection object</returns>
        private int getDescendantsIndexForChild(int childIndex)
        {
            // Add a empty connection object to mop up children.
            if (descendants_.Length == 0)
            {
                // Child of no relationship
                descendants_ = new TreeConnection[1];
                if (isMale_)
                {
                    descendants_[0] = new TreeConnection(tree_, this, ConnectionMainPerson.FATHER, 0);
                }
                else
                {
                    descendants_[0] = new TreeConnection(tree_, this, ConnectionMainPerson.MOTHER, 0);
                }
                tree_.addFamily(descendants_[0]);
            }

            // Create the child person object.
            Person child = new Person(childIndex, tree_.database);

            // Search for the connection that matches with this person
            for (int i = 0; i < descendants_.Length; i++)
            {
                /*
                if(m_bMale)
                {
                    if(oChild.MotherID==m_oDescendants[nI].GetMotherID())
                    {
                        return nI;
                    }
                }
                else
                {
                    if(oChild.FatherID==m_oDescendants[nI].GetFatherID())
                    {
                        return nI;
                    }
                }
                */
                if (child.motherIndex == descendants_[i].getMotherIndex() && child.fatherIndex == descendants_[i].getFatherIndex())
                {
                    return i;
                }
            }

            // This adds a new partner-less connection to pick the children for whom both parents are not known.
            TreeConnection[] newDescendants = new TreeConnection[descendants_.Length + 1];
            for (int i = 0; i < descendants_.Length; i++)
            {
                newDescendants[i + 1] = descendants_[i];
            }
            if (isMale_)
            {
                newDescendants[0] = new TreeConnection(tree_, this, ConnectionMainPerson.FATHER, descendants_.Length);
            }
            else
            {
                newDescendants[0] = new TreeConnection(tree_, this, ConnectionMainPerson.MOTHER, descendants_.Length);
            }
            descendants_ = newDescendants;
            tree_.addFamily(descendants_[0]);

            // Default to the first connection object.
            return 0;
        }



        /// <summary>Add the ancestors of this person to the current tree document.</summary>
        /// <remarks>The primary person has ancestors symetrically above him, everyone else has to be space efficiently.</remarks>
        /// <param name="isPrimaryPerson">Specify true for the primary person, false otherwise (usually).</param>
        /// <param name="rules">Specify the rules that apply to this tree.</param>
        public void addAncestors(bool isPrimaryPerson, TreeRule[] rules)
        {
            // Get this person.
            Person person = new Person(personIndex_, tree_.database);
            if (person.fatherIndex == 0 && person.motherIndex == 0)
            {
                // Nothing to do.
                return;
            }

            // Create an ancestors object for this person.
            if (isPrimaryPerson)
            {
                ancestors_ = new TreeConnection(tree_, this, ConnectionMainPerson.CHILD, 0);
            }
            else if (isMale_)
            {
                ancestors_ = new TreeConnection(tree_, this, ConnectionMainPerson.CHILD_BOY, 0);
            }
            else
            {
                ancestors_ = new TreeConnection(tree_, this, ConnectionMainPerson.CHILD_GIRL, 0);
            }
            tree_.addFamily(ancestors_);

            // Add the father of this person.
            if (person.fatherIndex != 0)
            {
                TreePerson father = new TreePerson(tree_, person.fatherIndex);
                tree_.addPerson(father);
                ancestors_.addFather(father);

                // Add the ancestors for the father.
                father.addAncestors(false, rules);
            }

            // Add the mother of this person.
            if (person.motherIndex != 0)
            {
                TreePerson mother = new TreePerson(tree_, person.motherIndex);
                tree_.addPerson(mother);
                ancestors_.addMother(mother);

                // Add the ancestors for the mother.
                mother.addAncestors(false, rules);
            }

            // Add the relationship between the father and mother.
            if (person.fatherIndex != 0 && person.motherIndex != 0)
            {
                Relationship relationship = tree_.database.getRelationship(person.fatherIndex, person.motherIndex);
                if (relationship != null)
                {
                    ancestors_.start = relationship.start;
                    if (relationship.terminatedIndex == 2)
                    {
                        ancestors_.status = RelationshipStatus.DIVORCED;
                    }
                }
            }

            // Add the sublings of this person.
            int siblingIndex;
            int[] siblings = person.getSiblings();
            for (int i = 0; i < siblings.Length; i++)
            {
                if (isMale())
                {
                    siblingIndex = siblings.Length - 1 - i;
                }
                else
                {
                    siblingIndex = i;
                }
                TreePerson sibling = new TreePerson(tree_, siblings[siblingIndex]);
                tree_.addPerson(sibling);

                Person halfSibling = new Person(siblings[siblingIndex], tree_.database);
                if (halfSibling.fatherIndex != person.fatherIndex)
                {
                    sibling.connection = ParentConnection.MOTHER_ONLY;
                }
                if (halfSibling.motherIndex != person.motherIndex)
                {
                    sibling.connection = ParentConnection.FATHER_ONLY;
                }

                // Apply the rules to the siblings.
                foreach (TreeRule rule in rules)
                {
                    if (rule.action == TreeRule.RuleAction.INCLUDE_DESCENDANTS && rule.personIndex == sibling.personIndex)
                    {
                        sibling.addDescendants(rules);
                    }
                }

                ancestors_.addChild(sibling);
            }
        }



        /// <summary>Returns true if the person is male.  Returns false if the person is female.</summary>
        /// <returns>True if this person is male.  False, if this person is female.</returns>
        public bool isMale()
        {
            return isMale_;
        }



        /*
        public int GetNumRelationship()
        {
            if(m_oDescendants==null)
            {
                return 0;
            }
            return m_oDescendants.Length;
        }
        */



        #endregion

        #region Public Properties

        /// <summary>ID of the corresponding person in the database.</summary>
        public int personIndex { get { return personIndex_; } }

        /// <summary>Position of this person in the tree.</summary>
        public System.Drawing.PointF position { get { return pos_; } }

        /// <summary>X-Position of this person in the tree.  Same as Position.X.</summary>
        public float x { get { return pos_.X; } /* set { m_Pos.X = value; } */ }

        /// <summary>Y-Position of this person in the tree.  Same as Position.Y</summary>
        public float y { get { return pos_.Y; } /* set { m_Pos.Y = value; } */ }

        /// <summary>Type of connection this person has to there parents.</summary>
        public ParentConnection connection { get { return connection_; } set { connection_ = value; } }

        /// <summary>True if the position of this person in tree is set.</summary>
        public bool isPositionKnown { get { return isPositionKnown_; } set { isPositionKnown_ = value; } }



        /// <summary>Returns the name with years of this person.</summary>
        /// <returns>Returns the name with years of this person.</returns>
        public override string ToString()
        {
            return nameWithYears_;
        }



        /// <summary>Returns true if the person is older than the specified person.  Return false, otherwise.</summary>
        /// <param name="otherPersonIndex">Specifies the ID of the person to compare against.</param>
        /// <returns>True if the person is older than the specified person, false otherwise.</returns>
        public bool isOlder(int otherPersonIndex)
        {
            Person person = new Person(personIndex_, tree_.database);
            Person otherPerson = new Person(otherPersonIndex, tree_.database);
            if (person.dob.date < otherPerson.dob.date)
            {
                return true;
            }
            return false;
        }



        #endregion
    }
}
