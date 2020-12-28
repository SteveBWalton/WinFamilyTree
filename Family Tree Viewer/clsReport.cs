using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using FamilyTree.Objects;

namespace FamilyTree.Viewer
{
    public class clsReport
    {
        #region Member Variables

        // ID of the person this report will be about.
        /// <summary>
        /// ID of the person this report will be about.
        /// </summary>
        int m_nPersonID;

        // Database that contains the person and their relations.
        /// <summary>
        /// Database that contains the person and their relations.
        /// </summary>
        clsDatabase m_oDb;

        // User options to use to build this report.
        /// <summary>
        /// User options to use to build this report.
        /// </summary>
        clsUserOptions m_oOptions;

        #endregion

        #region Constructors etc ...

		// Class constructor.
        /// <summary>
		/// Class constructor.
		/// Specify the person this report is about, the database to gather information and the user options to use when building the document.
		/// </summary>
		/// <param name="nPersonID">Specify the ID of the person to build the document about</param>
		/// <param name="oDB">Specify the database from which to gather the information</param>
		/// <param name="oOptions">Specify the user option to use when building the document</param>
        public clsReport(int nPersonID, clsDatabase oDB, clsUserOptions oOptions)
        {
            // Record the construction parameters
            m_nPersonID = nPersonID;
            m_oDb = oDB;
            m_oOptions = oOptions;
        }

        #endregion

        #region Values and Building

        /// <summary>
        /// Adds the specified person to the specified collection of people.
        /// </summary>
        /// <remarks>
        /// The collection of people is keyed on the date of birth.
        /// No two entries can have the same date of birth.
        /// But we do not really care about the time of birth, so if the date of birth is already taken then simply use a slightly later time of birth.
        /// </remarks>
        /// <param name="oPerson">Specifies the person to add to the collection of people</param>
        /// <param name="oPeople">Specifies the collection of people to add to</param>
        private void AddPerson            (            clsPerson oPerson,            ref SortedList oPeople            )
        {
            // Add this person at their date of birth date specified.
            // Modify the time if the dates clash because we don't care about the time.
            DateTime oDate = oPerson.DoB.Date;
            bool bTryAgain = true;
            while(bTryAgain)
            {
                try
                {
                    oPeople.Add(oDate, oPerson);
                    bTryAgain = false;
                }
                catch
                {
                    oDate = oDate.AddSeconds(1);
                }
            }
        }

        /// <summary>
        /// Adds the forebears of the specified person to the specified collection of people
        /// </summary>
        /// <param name="oPerson">Specifies the person to add the forebears of</param>
        /// <param name="oPeople">Specifies the collection of people to add to</param>
        private void AddForebears            (            clsPerson oPerson,            ref SortedList oPeople            )
        {
            // Add the father to the list
            if(oPerson.FatherID != 0)
            {
                clsPerson oFather = new clsPerson(oPerson.FatherID, m_oDb);
                oFather.Tag = oPerson.Tag + "F";
                AddPerson(oFather, ref oPeople);

                // Add the father's parents
                AddForebears(oFather, ref oPeople);
            }

            // Add the mother to the list
            if(oPerson.MotherID != 0)
            {
                clsPerson oMother = new clsPerson(oPerson.MotherID, m_oDb);
                oMother.Tag = oPerson.Tag + "M";
                AddPerson(oMother, ref oPeople);

                // Add the mother's parents
                AddForebears(oMother, ref oPeople);
            }
        }

        /// <summary>
        /// Adds the descendants of the specified person to the specified collection of people
        /// </summary>
        /// <param name="oPerson">Specifies the person to add the descendants of</param>
        /// <param name="oPeople">Specifies the collection of people to add to</param>
        private void AddDescendants            (            clsPerson oPerson,            ref SortedList oPeople            )
        {
            int[] Children = oPerson.GetChildren();
            for(int nChild = 0; nChild < Children.Length; nChild++)
            {
                clsPerson oChild = new clsPerson(Children[nChild], m_oDb);
                oChild.Tag = oPerson.Tag + (nChild + 1).ToString() + ".";
                AddPerson(oChild, ref oPeople);

                // Add the child's children
                AddDescendants(oChild, ref oPeople);
            }
        }

        /// <summary>
        /// Add the siblings of the specified person to the specified collection of people
        /// </summary>
        /// <param name="oPerson">Specifies the person to add the siblings of</param>
        /// <param name="oPeople">Specifies the collection of people to add to</param>
        private void AddSiblings
            (
            clsPerson oPerson,
            ref SortedList oPeople
            )
        {
            int[] Siblings = oPerson.GetSiblings();
            for(int nChild = 0; nChild < Siblings.Length; nChild++)
            {
                clsPerson oChild = new clsPerson(Siblings[nChild], m_oDb);
                if(oChild.Male)
                {
                    oChild.Tag = "Brother";
                }
                else
                {
                    oChild.Tag = "Sister";
                }
                AddPerson(oChild, ref oPeople);
            }
        }

        #endregion

        #region Display

        /// <summary>
        /// Displays MS Word and shows the collection of people related to the main person
        /// </summary>
        /// <returns>True for success.  False otherwise.</returns>
        public string GetReport()
        {
            // Create (an empty) list of people to display
            SortedList oPeople = new SortedList();

            // Add the base person
            clsPerson oPerson = new clsPerson(m_nPersonID, m_oDb);
            oPeople.Add(oPerson.DoB.Date, oPerson);

            // Add the parents
            AddForebears(oPerson, ref oPeople);

            // Add the descendants
            AddDescendants(oPerson, ref oPeople);

            // Add the siblings
            AddSiblings(oPerson, ref oPeople);

            // Build html around the people found
            StringBuilder sbHtml = new StringBuilder();
            for(int nI = 0; nI < oPeople.Count; nI++)
            {
                oPerson = (clsPerson)oPeople.GetByIndex(nI);

                sbHtml.Append("<p>");
                if(oPerson.Tag != "")
                {
                    sbHtml.Append("(" + oPerson.Tag + ") ");
                }
                sbHtml.Append(oPerson.Description(true, false, false, false, false));
                sbHtml.AppendLine("</p>");
            }

            // return success
            return sbHtml.ToString();
        }

        #endregion
    }
}
