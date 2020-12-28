using System;
using FamilyTree.Objects;

namespace FamilyTree.Viewer
{
	/// <summary>
	/// Class to represent a person in a tree document.
	/// </summary>
	public class clsTreePerson
	{
		#region Member Variables

        #region Supporting Types etc ...

        /// <summary>Type of connection to parents that this person has.</summary>
        public enum enumConnection
        {
            /// <summary>Connection to a pair of parents.  Mother and Father.  Expected.</summary>
            cnBoth,
            /// <summary>Connection to father only.</summary>
            cnFather,
            /// <summary>Connection to mother only.</summary>
            cnMother
        }

        #endregion

		/// <summary>Tree document that this person is attached to.</summary>
		clsTreeDocument m_oTree;

		/// <summary>ID of this person in the database.</summary>
		int m_nPersonID;

		/// <summary>Name of this person.</summary>
		string m_sName;

        /// <summary>Name of this person including the lived years.</summary>
        private string m_sNameWithYears;

		/// <summary>Description of this person.</summary>
		string m_sDescription;

		/// <summary>True if this person is male.  False, otherwise.</summary>
		bool m_bMale;

		/// <summary>Type of connection to parent(s).</summary>
		enumConnection m_Connection;

		/// <summary>Position of this tree person.</summary>
		System.Drawing.PointF m_Pos;

		/// <summary>Array of connections to descendants.</summary>
		clsTreeConnection[] m_oDescendants;

		/// <summary>Connection to the ancestors of this tree person.</summary>
		clsTreeConnection m_oAncestors;

        /// <summary>True if the position of this person is known.</summary>
        private bool m_bPositionKnown;

		#endregion

		#region Constructors etc ...
		
		/// <summary>
		/// Create a person for a tree document.
		/// </summary>
		/// <param name="oTree">Specify the tree document that contains the person.</param>
		/// <param name="nPersonID">Specify the ID of the person.</param>
		public clsTreePerson
			(
			clsTreeDocument oTree,
			int nPersonID
			)
		{
			// Save the supplied values
			m_oTree = oTree;
			m_nPersonID = nPersonID;

			// Initialise the object
			m_Pos = new System.Drawing.PointF(0,0);
			m_oDescendants = null;
			m_oAncestors = null;
			m_Connection = enumConnection.cnBoth;
			
			// Get the information required from the database
			clsPerson oPerson = new clsPerson(nPersonID,m_oTree.Database);
			m_sName = oPerson.GetName(false,true);
			m_sDescription = oPerson.ShortDescription(false);
            m_sNameWithYears = oPerson.GetName(true,true);
			m_bMale = oPerson.Male;
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
            System.Drawing.SizeF oSize = oGraphics.MeasureString(m_sName,m_oTree.FontName);
            if(oSize.Width > dMaxWidth)
            {
                dMaxWidth = oSize.Width;
            }
            oSize = oGraphics.MeasureString(m_sDescription,m_oTree.FontDescription);
            if(oSize.Width > dMaxWidth)
            {
                dMaxWidth = oSize.Width;
            }

            // Check the width of Descendants			
            if(bIncludeDescendants)
            {
                if(m_oDescendants != null)
                {                    
                    // Expect that the children will be wider than the parents but it may not be the case
                    float dWidthChildren = 0;
                    float dWidthParents = dMaxWidth;
                    foreach(clsTreeConnection oDescendant in m_oDescendants)
                    {
                        dWidthChildren += oDescendant.GetWidthChildren(oGraphics);
                        dWidthParents += oDescendant.GetWidthParents(oGraphics) - dMaxWidth;
                    }
                    float dWidth = Math.Max(dWidthChildren,dWidthParents);

                    if(dWidth > dMaxWidth)
                    {
                        dMaxWidth = dWidth;
                    }
                }
            }

            // Check the width of ancestors
            if(bIncludeAncestors)
            {
                // Check the width of the parents
                if(m_oAncestors != null)
                {
                    float dWidth = 0;
                    if(m_oAncestors.Father != null)
                    {
                        dWidth += m_oAncestors.Father.GetWidth(oGraphics,false,true);
                        if(m_oAncestors.Mother != null)
                        {
                            dWidth += m_oTree.spcRelationshipSpace;
                        }
                    }
                    if(m_oAncestors.Mother != null)
                    {
                        dWidth += m_oAncestors.Mother.GetWidth(oGraphics,false,true);
                    }

                    if(dWidth > dMaxWidth)
                    {
                        dMaxWidth = dWidth;
                    }

                    // Check the width of siblings
                    dWidth = m_oAncestors.GetChildrenSpace(oGraphics);
                    if(dWidth > dMaxWidth)
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
            return GetWidth(oGraphics,false,false);
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
            if(!m_bPositionKnown)
            {
                return false;
            }

            // Calculate the positions for descendant partners
            float dNextChildWith = m_Pos.X;
            if(m_oDescendants != null)
            {
                int nPartner = 0;
                foreach(clsTreeConnection oDescendant in m_oDescendants)
                {
                    // Position the partner relative to this person
                    oDescendant.SetPartnerPosition(oGraphics, nPartner);

                    // Line the children up with the leftmost man 
                    if(oDescendant.Father != null)
                    {
                        if(oDescendant.Father.X < dNextChildWith)
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
            if(m_oDescendants != null)
            {
                int nPartner = 0;
                foreach(clsTreeConnection oDescendant in m_oDescendants)
                {
                    oDescendant.SetChildrenPosition(oGraphics, nPartner, ref dNextChildWith, ref dNextChildOut);
                    nPartner++;
                }
            }

            // Calculate the position for the parents (ancestors)
            if(m_oAncestors != null)
            {
                m_oAncestors.CalculatePositionsParents(oGraphics);
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
            if(nIndex == 0)
            {
                if(m_bMale)
                {
                    // Return the right edge of the person
                    return m_Pos.X + GetWidth(oGraphics);
                }
                else
                {
                    // Return the left edge of the person
                    return m_Pos.X;
                }
            }
            // Check that a spouse exists.  If not then look at the earlier connection
            clsTreeConnection oConnection = (clsTreeConnection)m_oDescendants[nIndex - 1];
            if(m_bMale)
            {
                if(oConnection.Mother == null)
                {
                    return GetSpousePosition(oGraphics, nIndex - 1);
                }
            }
            else
            {
                if(oConnection.Father == null)
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
            if(m_bMale)
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
            if(!m_bMale)
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
            if(m_oDescendants == null)
            {
                return 0;
            }
            float dOffset = 0;
            clsTreePerson oPartner;
            for(int nI = 0; nI < m_oDescendants.Length; nI++)
            {
                if(m_bMale)
                {
                    oPartner = ((clsTreeConnection)m_oDescendants[nI]).Mother;
                }
                else
                {
                    oPartner = ((clsTreeConnection)m_oDescendants[nI]).Father;
                }
                if(oPartner != null)
                {
                    dOffset += oPartner.GetWidth(oGraphics, false, false);

                    // Not sure this should be inside or outside this if condition
                    // Do we need a relationship marker if don't have the person
                    dOffset += m_oTree.spcRelationshipSpace;
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
            m_bPositionKnown = true;
            m_Pos.X = X;
            m_Pos.Y = Y;

            // Notify the tree with the position of this person
            m_oTree.NotifyPosition(m_Pos.X, m_Pos.Y, m_Pos.X + GetWidth(oGraphics), m_Pos.Y + m_oTree.spcPersonHeight);
        }

        #endregion

        #region Drawing

        /// <summary>
        /// Draws the specified person.
        /// Previously this would also draw his relations but it does not anymore.
        /// </summary>
        /// <param name="oGraphics">Specifies the graphics device to draw the person on to.</param>
        /// <returns>True for success, false otherwise.</returns>        
        public bool Draw
        (
        System.Drawing.Graphics oGraphics
        )
        {
            if(m_bPositionKnown)
            {
                // Console.WriteLine("Drawing " + m_sName);

                // Draw a box around the person (useful debugging)
                if(m_oTree.PersonBox)
                {
                    if(m_bMale)
                    {
                        oGraphics.FillRectangle(m_oTree.BrushBoy,m_Pos.X - m_oTree.OffsetX,m_Pos.Y - m_oTree.OffsetY,GetWidth(oGraphics),m_oTree.spcPersonHeight);
                    }
                    else
                    {
                        oGraphics.FillRectangle(m_oTree.BrushGirl,m_Pos.X - m_oTree.OffsetX,m_Pos.Y - m_oTree.OffsetY,GetWidth(oGraphics),m_oTree.spcPersonHeight);
                    }

                    oGraphics.DrawLine(m_oTree.PenBlackThin,m_Pos.X - m_oTree.OffsetX,m_Pos.Y - m_oTree.OffsetY,m_Pos.X - m_oTree.OffsetX + GetWidth(oGraphics),m_Pos.Y - m_oTree.OffsetY);
                    oGraphics.DrawLine(m_oTree.PenBlackThin,m_Pos.X - m_oTree.OffsetX + GetWidth(oGraphics),m_Pos.Y - m_oTree.OffsetY,m_Pos.X - m_oTree.OffsetX + GetWidth(oGraphics),m_Pos.Y - m_oTree.OffsetY + m_oTree.spcPersonHeight);
                    oGraphics.DrawLine(m_oTree.PenBlackThin,m_Pos.X - m_oTree.OffsetX + GetWidth(oGraphics),m_Pos.Y - m_oTree.OffsetY + m_oTree.spcPersonHeight,m_Pos.X - m_oTree.OffsetX,m_Pos.Y - m_oTree.OffsetY + m_oTree.spcPersonHeight);
                    oGraphics.DrawLine(m_oTree.PenBlackThin,m_Pos.X - m_oTree.OffsetX,m_Pos.Y - m_oTree.OffsetY + m_oTree.spcPersonHeight,m_Pos.X - m_oTree.OffsetX,m_Pos.Y - m_oTree.OffsetY);
                }

                // Draw the person's name and description
                oGraphics.DrawString(m_sName,m_oTree.FontName,m_oTree.BrushBlack,m_Pos.X - m_oTree.OffsetX,m_Pos.Y - m_oTree.OffsetY);
                oGraphics.DrawString(m_sDescription,m_oTree.FontDescription,m_oTree.BrushBlack,m_Pos.X - m_oTree.OffsetX,m_Pos.Y + m_oTree.FontName.Height - m_oTree.OffsetY);
            }
            else
            {
                Console.WriteLine("Can't draw " + m_sName+" position unknown.");
            }

            // Return success
            return true;
        }

        #endregion

        #region Building and Values

        /// Returns true if this person has any descendants in the database.  False, otherwise.
        /// <summary>
        /// Returns true if this person has any descendants in the database.  False, otherwise.
        /// </summary>
		/// <returns>True if this person has descendants, false otherwise.</returns>
		public bool HasDescendants()
		{
			if(m_oDescendants==null)
			{
				return false;
			}
			if(m_oDescendants.Length==0)
			{
				return false;
			}
			return true;
		}

        // Add the descendants of this person to the current document.
        /// <summary>
        /// Add the descendants of this person to the current document.
        /// </summary>
        /// <param name="oRules">Specify the current set of document rules.</param>
        public void AddDescendants(clsTreeRule[] oRules)
        {
            foreach(clsTreeRule oRule in oRules)
            {
                // Check that this person's descendants have not been excluded.
                if(oRule.Action == clsTreeRule.ERuleAction.ExcludeDescendants && oRule.PersonID == m_nPersonID)
                {
                    return;
                }
            }

            // Get this person
            clsPerson oPerson = new clsPerson(m_nPersonID, m_oTree.Database);

            // Add the partners to the person
            enumConMainPerson nType;
            if(oPerson.Male)
            {
                nType = enumConMainPerson.Father;
            }
            else
            {
                nType = enumConMainPerson.Mother;
            }
            clsRelationship[] oRelationships = oPerson.GetRelationships();
            m_oDescendants = new clsTreeConnection[oRelationships.Length];
            int nIndex;
            for(int nI = 0; nI < oRelationships.Length; nI++)
            {
                if(IsMale())
                {
                    nIndex = oRelationships.Length - 1 - nI;
                }
                else
                {
                    nIndex = nI;
                }
                m_oDescendants[nI] = new clsTreeConnection(m_oTree, this, nType, nI);
                m_oTree.AddFamily(m_oDescendants[nI]);
                if(oPerson.Male)
                {
                    clsTreePerson oMother = new clsTreePerson(m_oTree, oRelationships[nIndex].PartnerID);
                    m_oDescendants[nI].AddMother(oMother);
                    m_oTree.AddPerson(oMother);
                }
                else
                {
                    clsTreePerson oFather = new clsTreePerson(m_oTree, oRelationships[nIndex].PartnerID);
                    m_oDescendants[nI].AddFather(oFather);
                    m_oTree.AddPerson(oFather);
                }
                if(oRelationships[nIndex].TerminatedID == 2)
                {
                    m_oDescendants[nI].Status = enumRelationshipStatus.Divorced;
                }
                if(!oRelationships[nIndex].Start.IsEmpty())
                {
                    m_oDescendants[nI].Start = oRelationships[nIndex].Start;
                }
            }

            // Add children to the person
            int[] Children = oPerson.GetChildren();
            for(int nI = 0; nI < Children.Length; nI++)
            {
                // Decide which relationship / connections this child belongs to
                int nConnection = GetDescendantsIndexForChild(Children[nI]);

                // Create a person in the tree document for this child
                clsTreePerson oChild = new clsTreePerson(m_oTree, Children[nI]);
                m_oTree.AddPerson(oChild);

                // Add the child to the selected relationship / connection
                m_oDescendants[nConnection].AddChild(oChild);

                // Add the descendants of this child
                oChild.AddDescendants(oRules);
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
            if(m_oDescendants.Length == 0)
            {
                // Child of no relationship
                m_oDescendants = new clsTreeConnection[1];
                if(m_bMale)
                {
                    m_oDescendants[0] = new clsTreeConnection(m_oTree, this, enumConMainPerson.Father, 0);
                }
                else
                {
                    m_oDescendants[0] = new clsTreeConnection(m_oTree, this, enumConMainPerson.Mother, 0);
                }
                m_oTree.AddFamily(m_oDescendants[0]);
            }

            // Create the child person object
            clsPerson oChild = new clsPerson(nChildID, m_oTree.Database);

            // Search for the connection that matches with this person
            for(int nI = 0; nI < m_oDescendants.Length; nI++)
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
                if(oChild.MotherID == m_oDescendants[nI].GetMotherID() && oChild.FatherID == m_oDescendants[nI].GetFatherID())
                {
                    return nI;
                }
            }

            // This adds a new partner-less connection to pick the children for whom both parents are not known		
            clsTreeConnection[] oNewDescendants = new clsTreeConnection[m_oDescendants.Length + 1];
            for(int nI = 0; nI < m_oDescendants.Length; nI++)
            {
                oNewDescendants[nI + 1] = m_oDescendants[nI];
            }
            if(m_bMale)
            {
                oNewDescendants[0] = new clsTreeConnection(m_oTree, this, enumConMainPerson.Father, m_oDescendants.Length);
            }
            else
            {
                oNewDescendants[0] = new clsTreeConnection(m_oTree, this, enumConMainPerson.Mother, m_oDescendants.Length);
            }
            m_oDescendants = oNewDescendants;
            m_oTree.AddFamily(m_oDescendants[0]);

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
            clsPerson oPerson = new clsPerson(m_nPersonID, m_oTree.Database);
            if(oPerson.FatherID == 0 && oPerson.MotherID == 0)
            {
                // Nothing to do
                return;
            }

            // Create an ancestors object for this person
            if(bPrimaryPerson)
            {
                m_oAncestors = new clsTreeConnection(m_oTree, this, enumConMainPerson.Child, 0);
            }
            else if(m_bMale)
            {
                m_oAncestors = new clsTreeConnection(m_oTree, this, enumConMainPerson.ChildBoy, 0);
            }
            else
            {
                m_oAncestors = new clsTreeConnection(m_oTree, this, enumConMainPerson.ChildGirl, 0);
            }
            m_oTree.AddFamily(m_oAncestors);

            // Add the father of this person
            if(oPerson.FatherID != 0)
            {
                clsTreePerson oFather = new clsTreePerson(m_oTree, oPerson.FatherID);
                m_oTree.AddPerson(oFather);
                m_oAncestors.AddFather(oFather);

                // Add the ancestors for the father
                oFather.AddAncestors(false, oRules);
            }

            // Add the mother of this person
            if(oPerson.MotherID != 0)
            {
                clsTreePerson oMother = new clsTreePerson(m_oTree, oPerson.MotherID);
                m_oTree.AddPerson(oMother);
                m_oAncestors.AddMother(oMother);

                // Add the ancestors for the mother
                oMother.AddAncestors(false, oRules);
            }

            // Add the relationship between the father and mother
            if(oPerson.FatherID != 0 && oPerson.MotherID != 0)
            {
                clsRelationship oRelationship = m_oTree.Database.GetRelationship(oPerson.FatherID, oPerson.MotherID);
                if(oRelationship != null)
                {
                    m_oAncestors.Start = oRelationship.Start;
                    if(oRelationship.TerminatedID == 2)
                    {
                        m_oAncestors.Status = enumRelationshipStatus.Divorced;
                    }
                }
            }

            // Add the sublings of this person
            int nIndex;
            int[] Siblings = oPerson.GetSiblings();
            for(int nI = 0; nI < Siblings.Length; nI++)
            {
                if(IsMale())
                {
                    nIndex = Siblings.Length - 1 - nI;
                }
                else
                {
                    nIndex = nI;
                }
                clsTreePerson oSibling = new clsTreePerson(m_oTree, Siblings[nIndex]);
                m_oTree.AddPerson(oSibling);

                clsPerson oHalfSibling = new clsPerson(Siblings[nIndex], m_oTree.Database);
                if(oHalfSibling.FatherID != oPerson.FatherID)
                {
                    oSibling.Connection = enumConnection.cnMother;
                }
                if(oHalfSibling.MotherID != oPerson.MotherID)
                {
                    oSibling.Connection = enumConnection.cnFather;
                }

                // Apply the rules to the siblings
                foreach(clsTreeRule oRule in oRules)
                {
                    if(oRule.Action == clsTreeRule.ERuleAction.IncludeDescendants && oRule.PersonID == oSibling.PersonID)
                    {
                        oSibling.AddDescendants(oRules);
                    }
                }

                m_oAncestors.AddChild(oSibling);
            }
        }

        /// Returns true if the person is male.  Returns false if the person is female.
        /// <summary>
        /// Returns true if the person is male.  Returns false if the person is female.
        /// </summary>
		/// <returns>True if this person is male.  False, if this person is female.</returns>
		public bool IsMale()
		{
			return m_bMale;
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
		public int PersonID { get { return m_nPersonID; } }

		/// <summary>Position of this person in the tree.</summary>
		public System.Drawing.PointF Position { get { return m_Pos; } }

		/// <summary>X-Position of this person in the tree.  Same as Position.X.</summary>
		public float X { get { return m_Pos.X; } /* set { m_Pos.X = value; } */ }

		/// <summary>Y-Position of this person in the tree.  Same as Position.Y</summary>
		public float Y { get { return m_Pos.Y; } /* set { m_Pos.Y = value; } */ }
		
		/// <summary>Type of connection this person has to there parents.</summary>
		public enumConnection Connection { get { return m_Connection; } set { m_Connection = value; } }

        /// <summary>True if the position of this person in tree is set.</summary>
        public bool PositionKnown { get { return m_bPositionKnown; } set { m_bPositionKnown = value; } }

        /// Returns the name with years of this person.
        /// <summary>
        /// Returns the name with years of this person.
        /// </summary>
        /// <returns>Returns the name with years of this person.</returns>
        public override string ToString()
        {
            return m_sNameWithYears; 
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
            clsPerson oPerson = new clsPerson(m_nPersonID, m_oTree.Database);
            clsPerson oOtherPerson = new clsPerson(nPersonID, m_oTree.Database);
            if(oPerson.DoB.Date < oOtherPerson.DoB.Date)
            {
                return true;
            }
            return false;
        }

		#endregion
	}
}
