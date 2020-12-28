using System;
using System.Collections;
using System.IO;

namespace FamilyTree.Objects
{
    // Class to represent the collection of clsFamily objects in a Gedcom file.
    /// <summary>
    /// Class to represent the collection of clsFamily objects in a Gedcom file.
    /// </summary>
	public class clsFamilies
	{
		#region Member Variables

        // Number of clsFamily objects in the collection and the Gedcom index of the last one.
        /// <summary>
        /// Number of clsFamily objects in the collection and the Gedcom index of the last one.
        /// </summary>
		private int m_nCount;

        // Collection of the clsFamily objects.
        /// <summary>
        /// Collection of the clsFamily objects.
        /// </summary>
		private ArrayList m_oCollection;

		#endregion

        // Empty class constructor.
        /// <summary>
        /// Empty class constructor.
        /// </summary>
		public clsFamilies()
		{
			m_nCount = 0;
			m_oCollection = new ArrayList();
		}

        // Returns the clsFamily (Gedcom) object that represents the specified mother and father.
        /// <summary>
        /// Returns the clsFamily (Gedcom) object that represents the specified mother and father.
        /// The object is attached to the specifed clsRelationship object.
		/// </summary>
		/// <param name="nFatherID">Specifies the ID of the father.</param>
		/// <param name="nMotherID">Specifies the ID of the mother.</param>
		/// <param name="nRelationshipID">Specifies the ID of the clsRelationship object.</param>
		/// <returns>A clsFamily (Gedcom) object.</returns>
        public clsFamily GetMarriageFamily(int nFatherID, int nMotherID, int nRelationshipID)
        {
            // Search for a matching family
            foreach(clsFamily oFamily in m_oCollection)
            {
                if(oFamily.FatherID == nFatherID && oFamily.MotherID == nMotherID)
                {
                    return oFamily;
                }
            }

            // Create a new family to match these conditions
            clsFamily oNewFamily = new clsFamily();
            oNewFamily.MotherID = nMotherID;
            oNewFamily.FatherID = nFatherID;
            oNewFamily.RelationshipID = nRelationshipID;
            m_nCount++;
            oNewFamily.GedComID = m_nCount;

            // Add this family to the collection
            m_oCollection.Add(oNewFamily);

            // Return the new family
            return oNewFamily;
        }

        // Returns the clsFamily (Gedcom) object that represents the specified mother and father.
        /// <summary>
        /// Returns the clsFamily (Gedcom) object that represents the specified mother and father.
        /// The object is not attached to any clsRelationship object.
		/// </summary>
		/// <param name="nFatherID">Specifies the ID of the father.</param>
		/// <param name="nMotherID">Specifies the ID of the mother.</param>
		/// <returns>A clsFamily (Gedcom) object.</returns>
        public clsFamily GetParentFamily(int nFatherID, int nMotherID)
        {
            return GetMarriageFamily(nFatherID, nMotherID, 0);
        }

        // Writes all the family data into the specified GedCom file
        /// <summary>
        /// Writes all the family data into the specified GedCom file
        /// </summary>
		/// <param name="oFile">Specifies the Gedcom file to write the data into.</param>
		/// <param name="oDb">Specifies the database to fetch additional information from.</param>
        /// <param name="lpfnProgressBar">Specifies a function to call to perform step the progress bar.</param>        
        public void WriteGedcom(StreamWriter oFile, clsDatabase oDb, dgtVoid lpfnProgressBar,clsGedcomOptions oOptions)
        {
            foreach(clsFamily oFamily in m_oCollection)
            {
                oFamily.WriteGedcom(oFile, oDb,oOptions);
                if(lpfnProgressBar != null)
                {
                    lpfnProgressBar();
                }
            }
        }
	}
}
