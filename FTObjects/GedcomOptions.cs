using System;
using System.Collections.Generic;
using System.Text;

namespace FamilyTree.Objects
{
    /// <summary>Class to represent the options for a gedcom file.</summary>
    public class GedcomOptions
    {
        /// <summary>The filename of the gedcom file.</summary>
        public string fileName;

        /// <summary>Include the _PGVU (last edit by) tags in the gedcom file.</summary>
        public bool isIncludePGVU;

        /// <summary>True to remove the address from PLAC tags and include a ADDR tag.</summary>
        public bool isRemoveADDRfromPLAC;

        /// <summary>Add ADDR (address) tags to PLAC tags.</summary>
        public bool isUseADDR;

        /// <summary>Add CTRY (Country) tags to PLAC tags.</summary>
        public bool isUseCTRY;

        /// <summary>Add MAP tags with longitude and latitude to PLAC tags.</summary>
        public bool isUseLongitude;

        #region Class Constrcutors etc ...



        /// <summary>Empty class constructor.</summary>
        public GedcomOptions()
        {
        }



        /// <summary>Class constrcutor that copies and existing object.</summary>
        /// <param name="source"></param>
        public GedcomOptions(GedcomOptions source)
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
