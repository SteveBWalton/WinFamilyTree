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
        // The filename of the gedcom file.
        /// <summary>
        /// The filename of the gedcom file.
        /// </summary>
        public string sFilename;

        // Include the _PGVU (last edit by) tags in the gedcom file.
        /// <summary>
        /// Include the _PGVU (last edit by) tags in the gedcom file.
        /// </summary>
        public bool IncludePGVU;

        // True to remove the address from PLAC tags and include a ADDR tag
        /// <summary>
        /// True to remove the address from PLAC tags and include a ADDR tag
        /// </summary>
        public bool RemoveADDRfromPLAC;

        // Add ADDR (address) tags to PLAC tags
        /// <summary>
        /// Add ADDR (address) tags to PLAC tags
        /// </summary>
        public bool UseADDR;

        // Add CTRY (Country) tags to PLAC tags
        /// <summary>
        /// Add CTRY (Country) tags to PLAC tags
        /// </summary>
        public bool UseCTRY;

        // Add MAP tags with longitude and latitude to PLAC tags
        /// <summary>
        /// Add MAP tags with longitude and latitude to PLAC tags
        /// </summary>
        public bool UseLongitude;

        #region Class Constrcutors etc ...

        // Empty class constructor.
        /// <summary>
        /// Empty class constructor.
        /// </summary>
        public clsGedcomOptions()
        {
        }

        // Class constrcutor that copies and existing object.
        /// <summary>
        /// Class constrcutor that copies and existing object.
        /// </summary>
        /// <param name="oSource"></param>
        public clsGedcomOptions(clsGedcomOptions oSource)
        {
            sFilename = oSource.sFilename;
        }

        #endregion

        #region File IO

        // Save the gedcom options into the specified xml node.
        /// <summary>
        /// Save the gedcom options into the specified xml node.
        /// </summary>
        /// <param name="xmlGedcom">Specifies the xml node to save the gedcom options into.</param>
        /// <returns>True for success, false otherwise.</returns>
        public bool Save(Innoval.clsXmlNode xmlGedcom)
        {
            xmlGedcom.SetAttributeValue("filename", sFilename);

            // Return success
            return true;
        }

        // Load the gedcom options into the specified xml node.
        /// <summary>
        /// Load the gedcom options from the specified xml node.
        /// </summary>
        /// <param name="xmlGedcom">Specifies the xml node to load the gedcom options from.</param>
        /// <returns>True for success, false otherwise.</returns>
        public bool Load(Innoval.clsXmlNode xmlGedcom)
        {
            sFilename = xmlGedcom.GetAttributeValue("filename", "filename.ged", true);

            // Return success
            return true;
        }

        #endregion
    }
}
