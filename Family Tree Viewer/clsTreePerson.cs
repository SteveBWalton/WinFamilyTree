using System;
using FamilyTree.Objects;

namespace FamilyTree.Viewer
{
    /// <summary>Class to represent a person in a tree document.</summary>
    public class clsTreePerson
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
        clsTreeConnection[] descendants_;

        /// <summary>Connection to the ancestors of this tree person.</summary>
        clsTreeConnection ancestors_;

        /// <summary>True if the position of this person is known.</summary>
        private bool isPositionKnown_;

        #endregion

        #region Constructors etc ...



        /// <summary>Create a person for a tree document.</summary>
        /// <param name="tree">Specify the tree document that contains the person.</param>
        /// <param name="personIndex">Specify the ID of the person.</param>
        public clsTreePerson(TreeDocument tree, int personIndex)
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

        /// <summary>
        /// Returns the width required for the person.
        /// If IncludeDescendants then the function works recursively to calculate the space siblings should leave for this person.
        /// Otherwise the function simply calculates the space for the box around the person's name.
        /// </summary>
        /// <param name="oGraphics">Specify the device where the person will be drawn.</param>
        /// <param name="bIncludeDescendants">Returns the space required to fit the person's descendants in.</param>
        /// <param name="bIncludeAncestors">Returns the space required to fit the person's ancestors in.</param>
        /// <returns>Return the horizontal space required for this person.</returns>
        public float GetWidth
            (
            System.Drawing.Graphics oGraphics,
            bool bIncludeDescendants,
            bool bIncludeAncestors
            )
        {
            // The maximum width of the person
            float dMaxWidth = 0;

            // Check the width of this person
            System.Drawing.SizeF oSize = oGraphics.MeasureString(name_, tree_.fontName);
            if (oSize.Width > dMaxWidth)
            {
                dMaxWidth = oSize.Width;
            }
            oSize = oGraphics.MeasureString(description_, tree_.fontDescription);
            if (oSize.Width > dMaxWidth)
            {
                dMaxWidth = oSize.Width;
            }

            // Check the width of Descendants			
            if (bIncludeDescendants)
            {
                if (descendants_ != null)
                {
                    // Expect that the children will be wider than the parents but it may not be the case
                    float dWidthChildren = 0;
                    float dWidthParents = dMaxWidth;
                    foreach (clsTreeConnection oDescendant in descendants_)
                    {
                        dWidthChildren += oDescendant.GetWidthChildren(oGraphics);
                        dWidthParents += oDescendant.GetWidthParents(oGraphics) - dMaxWidth;
                    }
                    float dWidth = Math.Max(dWidthChildren, dWidthParents);

                    if (dWidth > dMaxWidth)
                    {
                        dMaxWidth = dWidth;
                    }
                }
            }

            // Check the width of ancestors
            if (bIncludeAncestors)
            {
                // Check the width of the parents
                if (ancestors_ != null)
                {
                    float dWidth = 0;
                    if (ancestors_.Father != null)
                    {
                        dWidth += ancestors_.Father.GetWidth(oGraphics, false, true);
                        if (ancestors_.Mother != null)
                        {
                            dWidth += tree_.spcRelationshipSpace;
                        }
                    }
                    if (ancestors_.Mother != null)
                    {
                        dWidth += ancestors_.Mother.GetWidth(oGraphics, false, true);
                    }

                    if (dWidth > dMaxWidth)
                    {
                        dMaxWidth = dWidth;
                    }

                    // Check the width of siblings
                    dWidth = ancestors_.GetChildrenSpace(oGraphics);
                    if (dWidth > dMaxWidth)
                    {
                        dMaxWidth = dWidth;
                    }
                }
            }

            // Return the calculated width
            return dMaxWidth;
        }

        /// <summary>
        /// Returns the width of the only the person not including descendants or ancestors.
        /// </summary>
        /// <param name="oGraphics">Specify the device where the person will be drawn.</param>
        /// <returns>The horizontal width of the name of this person (or the description).</returns>
        public float GetWidth
            (
            System.Drawing.Graphics oGraphics
            )
        {
            return GetWidth(oGraphics, false, false);
        }

        #endregion

        #region Calculate Positions

        /// Calculate the positions of all the people connected to this person in the tree.
        /// <summary>
        /// This is a completely different approach to the prevous version.
        /// Given that you know this position of this person then position his direct relations.
        /// </summary>
        /// <param name="oGraphics">Specify the device to calculate the positions on</param>
        /// <returns>True for success, false otherwise</returns>
        public bool CalculatePosition(System.Drawing.Graphics oGraphics)
        {
            // Check that this person is already known
            if (!isPositionKnown_)
            {
                return false;
            }

            // Calculate the positions for descendant partners
            float dNextChildWith = pos_.X;
            if (descendants_ != null)
            {
                int nPartner = 0;
                foreach (clsTreeConnection oDescendant in descendants_)
                {
                    // Position the partner relative to this person
                    oDescendant.SetPartnerPosition(oGraphics, nPartner);

                    // Line the children up with the leftmost man 
                    if (oDescendant.Father != null)
                    {
                        if (oDescendant.Father.X < dNextChildWith)
                        {
                            dNextChildWith = oDescendant.Father.X;
                        }
                    }
                    nPartner++;
                }
            }

            // Calculate the position for descendant children
            float dNextChildOut = dNextChildWith;
            // Calculate the position
            if (descendants_ != null)
            {
                int nPartner = 0;
                foreach (clsTreeConnection oDescendant in descendants_)
                {
                    oDescendant.SetChildrenPosition(oGraphics, nPartner, ref dNextChildWith, ref dNextChildOut);
                    nPartner++;
                }
            }

            // Calculate the position for the parents (ancestors)
            if (ancestors_ != null)
            {
                ancestors_.CalculatePositionsParents(oGraphics);
            }

            return false;
        }

        /// Returns the X position that the spouse in the relationship specified by nIndex should take.
        /// <summary>
        /// Returns the X position that the spouse in the relationship specified by nIndex should take.
        /// This accounts for the sex of the spouse (males are positioned on the left, females on the right).
        /// </summary>
        /// <param name="oGraphics">Specifies the graphics device to draw on.</param>
        /// <param name="nIndex">Specifies the index of the spouse.</param>
        /// <returns>The X position that the spouse should take.</returns>
        public float GetSpousePosition(System.Drawing.Graphics oGraphics, int nIndex)
        {
            if (nIndex == 0)
            {
                if (isMale_)
                {
                    // Return the right edge of the person
                    return pos_.X + GetWidth(oGraphics);
                }
                else
                {
                    // Return the left edge of the person
                    return pos_.X;
                }
            }
            // Check that a spouse exists.  If not then look at the earlier connection
            clsTreeConnection oConnection = (clsTreeConnection)descendants_[nIndex - 1];
            if (isMale_)
            {
                if (oConnection.Mother == null)
                {
                    return GetSpousePosition(oGraphics, nIndex - 1);
                }
            }
            else
            {
                if (oConnection.Father == null)
                {
                    return GetSpousePosition(oGraphics, nIndex - 1);
                }
            }

            // Return the position of the previous spouse
            return oConnection.SpousePosition(oGraphics);
        }

        /// Returns the position that the first child in the relationship specified by nIndex should take.
        /// <summary>
        /// NOT IMPLEMENTED
        /// 
        /// Returns the position that the first child in the relationship specified by nIndex should take.
        /// </summary>
        /// <param name="oGraphics"></param>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        public float GetFirstChildPosition(System.Drawing.Graphics oGraphics, int nIndex)
        {
            return 0;
        }

        /// Return the extra space that the person needs to make space for her husbands on the left
        /// <summary>
        /// Return the extra space that the person needs to make space for her husbands on the left
        /// </summary>
        /// <param name="oGraphics">Device that the husbands would be draw on</param>
        /// <returns>Width of the graphical representation of the husbands</returns>
        public float GetHusbandSpace(System.Drawing.Graphics oGraphics)
        {
            if (isMale_)
            {
                return 0;
            }
            return GetPartnerSpace(oGraphics);
        }

        /// Returns the extra space that the person needs to make space for his wives on the right.
        /// <summary>
        /// Returns the extra space that the person needs to make space for his wives on the right.
        /// </summary>
        /// <param name="oGraphics">Device that the wives would be draw on</param>
        /// <returns>Width of the graphical representation of the wives</returns>
        public float GetWifeSpace(System.Drawing.Graphics oGraphics)
        {
            if (!isMale_)
            {
                return 0;
            }
            return GetPartnerSpace(oGraphics);
        }

        /// Returns the space required for the partners of this person.
        /// <summary>
        /// Returns the space required for the partners of this person.
        /// </summary>
        /// <param name="oGraphics">Device to that the partners would be draw on</param>
        /// <returns>The width that would be required to draw the partner of the current person</returns>
        public float GetPartnerSpace(System.Drawing.Graphics oGraphics)
        {
            if (descendants_ == null)
            {
                return 0;
            }
            float dOffset = 0;
            clsTreePerson oPartner;
            for (int nI = 0; nI < descendants_.Length; nI++)
            {
                if (isMale_)
                {
                    oPartner = ((clsTreeConnection)descendants_[nI]).Mother;
                }
                else
                {
                    oPartner = ((clsTreeConnection)descendants_[nI]).Father;
                }
                if (oPartner != null)
                {
                    dOffset += oPartner.GetWidth(oGraphics, false, false);

                    // Not sure this should be inside or outside this if condition
                    // Do we need a relationship marker if don't have the person
                    dOffset += tree_.spcRelationshipSpace;
                }
            }
            return dOffset;
        }

        /// Set the position of this person on the tree once it has been calculated.
        /// <summary>
        /// Set the position of this person on the tree once it has been calculated.
        /// The tree is notified so that it can scroll to include this person.
        /// The person is marked as positioned so that other people can locate themselves in relation to this person.
        /// </summary>
        /// <param name="oGraphics">Specify the device.</param>
        /// <param name="X">Specify the X position of the person.</param>
        /// <param name="Y">Specify the Y position of the person.</param>
        public void SetPosition(System.Drawing.Graphics oGraphics, float X, float Y)
        {
            // Debuging message
            // Console.WriteLine(m_sName + " is at (" + X.ToString() + "," + Y.ToString() + ")");

            // Set the position
            isPositionKnown_ = true;
            pos_.X = X;
            pos_.Y = Y;

            // Notify the tree with the position of this person
            tree_.notifyPosition(pos_.X, pos_.Y, pos_.X + GetWidth(oGraphics), pos_.Y + tree_.spcPersonHeight);
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
                        graphics.FillRectangle(tree_.brushBoy, pos_.X - tree_.offsetX, pos_.Y - tree_.offsetY, GetWidth(graphics), tree_.spcPersonHeight);
                    }
                    else
                    {
                        graphics.FillRectangle(tree_.brushGirl, pos_.X - tree_.offsetX, pos_.Y - tree_.offsetY, GetWidth(graphics), tree_.spcPersonHeight);
                    }

                    graphics.DrawLine(tree_.penBlackThin, pos_.X - tree_.offsetX, pos_.Y - tree_.offsetY, pos_.X - tree_.offsetX + GetWidth(graphics), pos_.Y - tree_.offsetY);
                    graphics.DrawLine(tree_.penBlackThin, pos_.X - tree_.offsetX + GetWidth(graphics), pos_.Y - tree_.offsetY, pos_.X - tree_.offsetX + GetWidth(graphics), pos_.Y - tree_.offsetY + tree_.spcPersonHeight);
                    graphics.DrawLine(tree_.penBlackThin, pos_.X - tree_.offsetX + GetWidth(graphics), pos_.Y - tree_.offsetY + tree_.spcPersonHeight, pos_.X - tree_.offsetX, pos_.Y - tree_.offsetY + tree_.spcPersonHeight);
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
        public void addDescendants(clsTreeRule[] rules)
        {
            foreach (clsTreeRule rule in rules)
            {
                // Check that this person's descendants have not been excluded.
                if (rule.action == clsTreeRule.RuleAction.EXCLUDE_DESCENDANTS && rule.personIndex == personIndex_)
                {
                    return;
                }
            }

            // Get this person.
            Person person = new Person(personIndex_, tree_.database);

            // Add the partners to the person
            ConnectionMainPerson nType;
            if (person.isMale)
            {
                nType = ConnectionMainPerson.FATHER;
            }
            else
            {
                nType = ConnectionMainPerson.MOTHER;
            }
            Relationship[] relationships = person.getRelationships();
            descendants_ = new clsTreeConnection[relationships.Length];
            int nIndex;
            for (int nI = 0; nI < relationships.Length; nI++)
            {
                if (IsMale())
                {
                    nIndex = relationships.Length - 1 - nI;
                }
                else
                {
                    nIndex = nI;
                }
                descendants_[nI] = new clsTreeConnection(tree_, this, nType, nI);
                tree_.addFamily(descendants_[nI]);
                if (person.isMale)
                {
                    clsTreePerson oMother = new clsTreePerson(tree_, relationships[nIndex].partnerIndex);
                    descendants_[nI].AddMother(oMother);
                    tree_.addPerson(oMother);
                }
                else
                {
                    clsTreePerson oFather = new clsTreePerson(tree_, relationships[nIndex].partnerIndex);
                    descendants_[nI].AddFather(oFather);
                    tree_.addPerson(oFather);
                }
                if (relationships[nIndex].terminatedIndex == 2)
                {
                    descendants_[nI].Status = enumRelationshipStatus.Divorced;
                }
                if (!relationships[nIndex].start.isEmpty())
                {
                    descendants_[nI].Start = relationships[nIndex].start;
                }
            }

            // Add children to the person
            int[] Children = person.getChildren();
            for (int nI = 0; nI < Children.Length; nI++)
            {
                // Decide which relationship / connections this child belongs to
                int nConnection = GetDescendantsIndexForChild(Children[nI]);

                // Create a person in the tree document for this child
                clsTreePerson oChild = new clsTreePerson(tree_, Children[nI]);
                tree_.addPerson(oChild);

                // Add the child to the selected relationship / connection
                descendants_[nConnection].AddChild(oChild);

                // Add the descendants of this child
                oChild.addDescendants(rules);
            }
        }

        /// Returns the index of the connection object that the specified child should be added to
        /// <summary>
        /// Returns the index of the connection object that the specified child should be added to
        /// </summary>
		/// <param name="nChildID">Specifies the ID of the child</param>
		/// <returns>The index of a connection object</returns>
        private int GetDescendantsIndexForChild(int nChildID)
        {
            // Add a empty connection object to mop up children
            if (descendants_.Length == 0)
            {
                // Child of no relationship
                descendants_ = new clsTreeConnection[1];
                if (isMale_)
                {
                    descendants_[0] = new clsTreeConnection(tree_, this, ConnectionMainPerson.FATHER, 0);
                }
                else
                {
                    descendants_[0] = new clsTreeConnection(tree_, this, ConnectionMainPerson.MOTHER, 0);
                }
                tree_.addFamily(descendants_[0]);
            }

            // Create the child person object
            Person oChild = new Person(nChildID, tree_.database);

            // Search for the connection that matches with this person
            for (int nI = 0; nI < descendants_.Length; nI++)
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
                if (oChild.motherIndex == descendants_[nI].GetMotherID() && oChild.fatherIndex == descendants_[nI].GetFatherID())
                {
                    return nI;
                }
            }

            // This adds a new partner-less connection to pick the children for whom both parents are not known		
            clsTreeConnection[] oNewDescendants = new clsTreeConnection[descendants_.Length + 1];
            for (int nI = 0; nI < descendants_.Length; nI++)
            {
                oNewDescendants[nI + 1] = descendants_[nI];
            }
            if (isMale_)
            {
                oNewDescendants[0] = new clsTreeConnection(tree_, this, ConnectionMainPerson.FATHER, descendants_.Length);
            }
            else
            {
                oNewDescendants[0] = new clsTreeConnection(tree_, this, ConnectionMainPerson.MOTHER, descendants_.Length);
            }
            descendants_ = oNewDescendants;
            tree_.addFamily(descendants_[0]);

            // Default to the first connection object
            return 0;
        }

        // Add the ancestors of this person to the current tree document.
        /// <summary>
        /// Add the ancestors of this person to the current tree document.
        /// <remarks>
		/// The primary person has ancestors symetrically above him, everyone else has to be space efficiently.
		/// </remarks>
		/// </summary>
		/// <param name="bPrimaryPerson">Specify true for the primary person, false otherwise (usually).</param>
        /// <param name="oRules">Specify the rules that apply to this tree.</param>
        public void AddAncestors(bool bPrimaryPerson, clsTreeRule[] oRules)
        {
            // Get this person
            Person oPerson = new Person(personIndex_, tree_.database);
            if (oPerson.fatherIndex == 0 && oPerson.motherIndex == 0)
            {
                // Nothing to do
                return;
            }

            // Create an ancestors object for this person
            if (bPrimaryPerson)
            {
                ancestors_ = new clsTreeConnection(tree_, this, ConnectionMainPerson.CHILD, 0);
            }
            else if (isMale_)
            {
                ancestors_ = new clsTreeConnection(tree_, this, ConnectionMainPerson.CHILD_BOY, 0);
            }
            else
            {
                ancestors_ = new clsTreeConnection(tree_, this, ConnectionMainPerson.CHILD_GIRL, 0);
            }
            tree_.addFamily(ancestors_);

            // Add the father of this person
            if (oPerson.fatherIndex != 0)
            {
                clsTreePerson oFather = new clsTreePerson(tree_, oPerson.fatherIndex);
                tree_.addPerson(oFather);
                ancestors_.AddFather(oFather);

                // Add the ancestors for the father
                oFather.AddAncestors(false, oRules);
            }

            // Add the mother of this person
            if (oPerson.motherIndex != 0)
            {
                clsTreePerson oMother = new clsTreePerson(tree_, oPerson.motherIndex);
                tree_.addPerson(oMother);
                ancestors_.AddMother(oMother);

                // Add the ancestors for the mother
                oMother.AddAncestors(false, oRules);
            }

            // Add the relationship between the father and mother
            if (oPerson.fatherIndex != 0 && oPerson.motherIndex != 0)
            {
                Relationship oRelationship = tree_.database.getRelationship(oPerson.fatherIndex, oPerson.motherIndex);
                if (oRelationship != null)
                {
                    ancestors_.Start = oRelationship.start;
                    if (oRelationship.terminatedIndex == 2)
                    {
                        ancestors_.Status = enumRelationshipStatus.Divorced;
                    }
                }
            }

            // Add the sublings of this person
            int nIndex;
            int[] Siblings = oPerson.getSiblings();
            for (int nI = 0; nI < Siblings.Length; nI++)
            {
                if (IsMale())
                {
                    nIndex = Siblings.Length - 1 - nI;
                }
                else
                {
                    nIndex = nI;
                }
                clsTreePerson oSibling = new clsTreePerson(tree_, Siblings[nIndex]);
                tree_.addPerson(oSibling);

                Person oHalfSibling = new Person(Siblings[nIndex], tree_.database);
                if (oHalfSibling.fatherIndex != oPerson.fatherIndex)
                {
                    oSibling.Connection = ParentConnection.MOTHER_ONLY;
                }
                if (oHalfSibling.motherIndex != oPerson.motherIndex)
                {
                    oSibling.Connection = ParentConnection.FATHER_ONLY;
                }

                // Apply the rules to the siblings
                foreach (clsTreeRule oRule in oRules)
                {
                    if (oRule.action == clsTreeRule.RuleAction.INCLUDE_DESCENDANTS && oRule.personIndex == oSibling.PersonID)
                    {
                        oSibling.addDescendants(oRules);
                    }
                }

                ancestors_.AddChild(oSibling);
            }
        }

        /// Returns true if the person is male.  Returns false if the person is female.
        /// <summary>
        /// Returns true if the person is male.  Returns false if the person is female.
        /// </summary>
		/// <returns>True if this person is male.  False, if this person is female.</returns>
		public bool IsMale()
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
        public int PersonID { get { return personIndex_; } }

        /// <summary>Position of this person in the tree.</summary>
        public System.Drawing.PointF Position { get { return pos_; } }

        /// <summary>X-Position of this person in the tree.  Same as Position.X.</summary>
        public float X { get { return pos_.X; } /* set { m_Pos.X = value; } */ }

        /// <summary>Y-Position of this person in the tree.  Same as Position.Y</summary>
        public float Y { get { return pos_.Y; } /* set { m_Pos.Y = value; } */ }

        /// <summary>Type of connection this person has to there parents.</summary>
        public ParentConnection Connection { get { return connection_; } set { connection_ = value; } }

        /// <summary>True if the position of this person in tree is set.</summary>
        public bool PositionKnown { get { return isPositionKnown_; } set { isPositionKnown_ = value; } }

        /// Returns the name with years of this person.
        /// <summary>
        /// Returns the name with years of this person.
        /// </summary>
        /// <returns>Returns the name with years of this person.</returns>
        public override string ToString()
        {
            return nameWithYears_;
        }

        /// Returns true if the person is older than the specified person.
        /// <summary>
        /// Returns true if the person is older than the specified person.
        /// Return false, otherwise.
        /// </summary>
        /// <param name="nPersonID">Specifies the ID of the person to compare against.</param>
        /// <returns>True if the person is older than the specified person, false otherwise.</returns>
        public bool IsOlder(int nPersonID)
        {
            Person oPerson = new Person(personIndex_, tree_.database);
            Person oOtherPerson = new Person(nPersonID, tree_.database);
            if (oPerson.dob.date < oOtherPerson.dob.date)
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}
