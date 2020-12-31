using System;
using System.Collections.Generic;
using System.Text;

namespace FamilyTree.Objects
{
    // Class to represent the options for a gedcom file.
    /// <summary>
    /// Class to represent the options for a gedcom file.
    /// </summary>
    public class clsGedcomOptions
    {
        /// <summary>The filename of the gedcom file.</summary>
        public string fileName;

        /// <summary>Include the _PGVU (last edit by) tags in the gedcom file.</summary>
        public bool IncludePGVU;

        /// <summary>True to remove the address from PLAC tags and include a ADDR tag.</summary>
        public bool RemoveADDRfromPLAC;

        /// <summary>Add ADDR (address) tags to PLAC tags.</summary>
        public bool UseADDR;

        /// <summary>Add CTRY (Country) tags to PLAC tags.</summary>
        public bool UseCTRY;

        /// <summary>Add MAP tags with longitude and latitude to PLAC tags.</summary>
        public bool UseLongitude;

        #region Class Constrcutors etc ...



        /// <summary>Empty class constructor.</summary>
        public clsGedcomOptions()
        {
        }



        /// <summary>Class constrcutor that copies and existing object.</summary>
        /// <param name="source"></param>
        public clsGedcomOptions(clsGedcomOptions source)
        {
            fileName = source.fileName;
        }



        #endregion

        #region File IO



        /// <summary>Save the gedcom options into the specified xml node.</summary>
        /// <param name="xmlGedcom">Specifies the xml node to save the gedcom options into.</param>
        /// <returns>True for success, false otherwise.</returns>
        public bool save(walton.XmlNode xmlGedcom)
        {
            xmlGedcom.setAttributeValue("filename", fileName);

            // Return success.
            return true;
        }



        /// <summary>Load the gedcom options from the specified xml node.</summary>
        /// <param name="xmlGedcom">Specifies the xml node to load the gedcom options from.</param>
        /// <returns>True for success, false otherwise.</returns>
        public bool load(walton.XmlNode xmlGedcom)
        {
            fileName = xmlGedcom.getAttributeValue("filename", "filename.ged", true);

            // Return success.
            return true;
        }

        #endregion
    }
}
