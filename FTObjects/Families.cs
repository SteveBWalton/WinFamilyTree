using System;
using System.Collections;
using System.IO;

namespace FamilyTree.Objects
{
    /// <summary>Class to represent the collection of clsFamily objects in a Gedcom file.</summary>
	public class Families
    {
        #region Member Variables

        /// <summary>Number of clsFamily objects in the collection and the Gedcom index of the last one.</summary>
        private int count_;

        /// <summary>Collection of the clsFamily objects.</summary>
        private ArrayList collection_;

        #endregion

        /// <summary>Empty class constructor.</summary>
        public Families()
        {
            count_ = 0;
            collection_ = new ArrayList();
        }



        /// <summary>Returns the clsFamily (Gedcom) object that represents the specified mother and father.  The object is attached to the specifed clsRelationship object.</summary>
        /// <param name="fatherIndex">Specifies the ID of the father.</param>
        /// <param name="motherIndex">Specifies the ID of the mother.</param>
        /// <param name="relationshipIndex">Specifies the ID of the clsRelationship object.</param>
        /// <returns>A clsFamily (Gedcom) object.</returns>
        public Family getMarriageFamily(int fatherIndex, int motherIndex, int relationshipIndex)
        {
            // Search for a matching family.
            foreach (Family family in collection_)
            {
                if (family.fatherIndex == fatherIndex && family.motherIndex == motherIndex)
                {
                    return family;
                }
            }

            // Create a new family to match these conditions.
            Family newFamily = new Family();
            newFamily.motherIndex = motherIndex;
            newFamily.fatherIndex = fatherIndex;
            newFamily.relationshipIndex = relationshipIndex;
            count_++;
            newFamily.gedcomIndex = count_;

            // Add this family to the collection.
            collection_.Add(newFamily);

            // Return the new family.
            return newFamily;
        }



        /// <summary>Returns the clsFamily (Gedcom) object that represents the specified mother and father.  The object is not attached to any clsRelationship object.</summary>
        /// <param name="fatherIndex">Specifies the ID of the father.</param>
        /// <param name="motherIndex">Specifies the ID of the mother.</param>
        /// <returns>A clsFamily (Gedcom) object.</returns>
        public Family getParentFamily(int fatherIndex, int motherIndex)
        {
            return getMarriageFamily(fatherIndex, motherIndex, 0);
        }



        /// <summary>Writes all the family data into the specified GedCom file.</summary>
        /// <param name="file">Specifies the Gedcom file to write the data into.</param>
        /// <param name="database">Specifies the database to fetch additional information from.</param>
        /// <param name="lpfnProgressBar">Specifies a function to call to perform step the progress bar.</param>
        public void WriteGedcom(StreamWriter file, Database database, FuncVoid lpfnProgressBar, GedcomOptions options)
        {
            foreach (Family family in collection_)
            {
                family.writeGedcom(file, database, options);
                if (lpfnProgressBar != null)
                {
                    lpfnProgressBar();
                }
            }
        }
    }
}
