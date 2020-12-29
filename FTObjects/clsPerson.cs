using System;

// StringBuilder
using System.Text;

// ArrayList
using System.Collections;

// Access database ADO.NET
using System.Data;
using System.Data.OleDb;

// File
using System.IO;

namespace FamilyTree.Objects
{
    // Class to represent a person in a family tree database.
    /// <summary>
    /// Class to represent a person in a family tree database.
    /// </summary>
	public class clsPerson
    {
        #region Member Variables

        /// <summary>Connection to the database.</summary>
        private Database database_;

        /// <summary>ID of the person.</summary>
        private int personIndex_;

        /// <summary>Surname of the person.</summary>
        private string personSurname_;

        /// <summary>Forenames of the person.</summary>
        private string foreNames_;

        /// <summary>Maiden name of the person.</summary>
        private string maidenName_;

        /// <summary>Date of the birth for the person.</summary>
        private clsDate dob_;

        /// <summary>Date of death for the person.</summary>
        private clsDate dod_;

        /// <summary>ID of the person's father.</summary>
        private int fatherIndex_;

        /// <summary>ID of the person's mother.</summary>
        private int motherIndex_;

        /// <summary>True if the person is male.</summary>
        private bool isMale_;

        /// <summary>True if all the children of the person are known.</summary>
        private bool isAllChildrenKnown_;

        /// <summary>Index of the media object attached to this person.</summary>
        private int mediaIndex;

        /*
		/// <summary>Filename of an image for the person.  Not the full path.</summary>
		private string m_sImageFilename;

		/// <summary>Full filename of an image for the person.  This is not stored in the database.</summary>
		private string m_sImagePath;
         */

        /// <summary>User comments for the person.</summary>
        private string comments_;

        // True if this person should be included in the gedcom file, false otherwise.
        /// <summary>
        /// True if this person should be included in the gedcom file, false otherwise.
        /// </summary>
        private bool isIncludeGedcom_;

        /// <summary>Short term comments / information for the person.  This is not saved in the database.</summary>
        private string tag_;

        /// <summary>Array of facts about this person.</summary>
        private ArrayList facts_;

        /// <summary>Array of relationships for this person.</summary>
        private ArrayList relationships_;

        /// <summary>Sources for the name data.</summary>
        private clsSources sourcesName_;

        /// <summary>Sources for the date of birth data.</summary>
        private clsSources sourcesDoB_;

        /// <summary>Sources for the date of death data.</summary>
        private clsSources sourcesDoD_;

        /// <summary>All sources for this person.  Including the sources for non specific facts.</summary>
        private clsSources sourcesNonSpecific_;

        /// <summary>Name of the user who wrote the last edit.</summary>
        private string lastEditBy_;

        /// <summary>Date and time of the last edit.</summary>
        private DateTime lastEditDate_;

        /// <summary>Collection of ToDo items about this person.</summary>
        private ArrayList toDo_;

        #endregion

        #region Constructors

        // Creates an empty person object.
        /// <summary>
        /// Creates an empty person object.
        /// </summary>
        public clsPerson()
        {
            fatherIndex_ = 0;
            motherIndex_ = 0;
            isAllChildrenKnown_ = false;
            facts_ = null;
            dob_ = new clsDate();
            dod_ = new clsDate();
            isIncludeGedcom_ = true;
            tag_ = "";
            //			m_sImageFilename = "";
            //			m_sImagePath = "";
            toDo_ = null;

            relationships_ = null;

            sourcesName_ = null;
            sourcesDoB_ = null;
            sourcesDoD_ = null;
            sourcesNonSpecific_ = null;

        }

        // Creates an empty person object in the specfied database.
        /// <summary>
        /// Creates an empty person object in the specfied database.
        /// </summary>
		/// <param name="oDb">Specify the database to contain this person</param>
        public clsPerson(Database oDb)
            : this() // This makes the program call the () constructor before the code is called.
        {
            database_ = oDb;
        }

        // Create a person object from the specified database record.
        /// <summary>
        /// Create a person object from the specified database record.
        /// Loads the specified person from the specified database.
		/// </summary>
		/// <param name="nPersonID">Specify the ID of the person to load.</param>
		/// <param name="oDb">Specify the family tree database to load the person from.</param>
		public clsPerson(int nPersonID, Database oDb) : this(oDb) // This makes the program call the (clsDatabase) constructor before the code is called.
        {
            // Open the specified person			
            OleDbCommand oSql = new OleDbCommand("SELECT Surname,Forenames,MaidenName,Born,BornStatusID,Died,DiedStatusID,FatherID,MotherID,Sex,ChildrenKnown,GedCom,Comments,MediaID,LastEditBy,LastEditDate FROM tbl_People WHERE ID=" + nPersonID.ToString() + ";", oDb.cnDB);
            OleDbDataReader drPerson = oSql.ExecuteReader();
            if (drPerson.Read())
            {
                personIndex_ = nPersonID;
                if (drPerson.IsDBNull(0))
                {
                    personSurname_ = "";
                }
                else
                {
                    personSurname_ = drPerson.GetString(0);
                }
                if (drPerson.IsDBNull(1))
                {
                    foreNames_ = "";
                }
                else
                {
                    foreNames_ = drPerson.GetString(1);
                }
                if (drPerson.IsDBNull(2))
                {
                    maidenName_ = "";
                }
                else
                {
                    maidenName_ = drPerson.GetString(2);
                }
                if (drPerson.IsDBNull(3))
                {
                    dob_.Status = clsDate.EMPTY;
                }
                else
                {
                    dob_.Date = drPerson.GetDateTime(3);
                    dob_.Status = drPerson.GetInt16(4);
                }

                if (drPerson.IsDBNull(5))
                {
                    dod_.Status = clsDate.EMPTY;
                }
                else
                {
                    dod_.Date = drPerson.GetDateTime(5);
                    dod_.Status = drPerson.GetInt16(6);
                }
                if (drPerson.IsDBNull(7))
                {
                    fatherIndex_ = 0;
                }
                else
                {
                    fatherIndex_ = drPerson.GetInt32(7);
                }
                if (drPerson.IsDBNull(8))
                {
                    motherIndex_ = 0;
                }
                else
                {
                    motherIndex_ = drPerson.GetInt32(8);
                }
                if (drPerson.IsDBNull(9))
                {
                    isMale_ = true;
                }
                else
                {
                    if (drPerson.GetString(9) == "M")
                    {
                        isMale_ = true;
                    }
                    else
                    {
                        isMale_ = false;
                    }
                }

                isAllChildrenKnown_ = drPerson.GetBoolean(10);
                isIncludeGedcom_ = Innoval.clsDatabase.GetBool(drPerson, "Gedcom", true);
                comments_ = Database.GetString(drPerson, "Comments", "");
                mediaIndex = Database.GetInt(drPerson, "MediaID", 0);
                //				m_sImageFilename = "";
                lastEditBy_ = Database.GetString(drPerson, "LastEditBy", "Steve Walton");
                lastEditDate_ = Database.GetDateTime(drPerson, "LastEditDate", DateTime.Now);
            }
            drPerson.Close();
        }

        #endregion

        #region Save & GetName

        /// <summary>
        /// Save the person into the database
        /// </summary>
        /// <returns>True for success, false otherwise.</returns>
        public bool Save()
        {
            // If you create a new record (ID changes from 0) then (does no harm in any case)			
            OleDbCommand oSql;
            if (personIndex_ == 0)
            {
                // Find the new ID
                oSql = new OleDbCommand("SELECT MAX(ID) AS NewID FROM tbl_People;", database_.cnDB);
                personIndex_ = (int)oSql.ExecuteScalar() + 1;

                // Create a new person record
                oSql = new OleDbCommand("INSERT INTO tbl_People (ID,Surname) VALUES (" + personIndex_.ToString() + "," + Database.ToDb(personSurname_) + ");", database_.cnDB);
                oSql.ExecuteNonQuery();

                // Update the related child records
                if (sourcesName_ != null)
                {
                    sourcesName_.PersonID = personIndex_;
                }
                if (sourcesDoB_ != null)
                {
                    sourcesDoB_.PersonID = personIndex_;
                }
                if (sourcesDoD_ != null)
                {
                    sourcesDoD_.PersonID = personIndex_;
                }
                if (sourcesNonSpecific_ != null)
                {
                    sourcesNonSpecific_.PersonID = personIndex_;
                }
            }
            else
            {
                // Update the places associated with this person
                database_.PlaceDelink(1, personIndex_);
            }

            // Update the existing record
            oSql = new OleDbCommand
                (
                    "UPDATE tbl_People SET " +
                    "Surname=" + Database.ToDb(personSurname_) + "," +
                    "Forenames=" + Database.ToDb(foreNames_) + "," +
                    "MaidenName=" + Database.ToDb(maidenName_) + "," +
                    "Born=" + Database.ToDb(dob_) + "," +
                    "BornStatusID=" + dob_.Status.ToString() + "," +
                    "Died=" + Database.ToDb(dod_) + "," +
                    "DiedStatusID=" + dod_.Status.ToString() + "," +
                    "ChildrenKnown=" + Database.ToDb(isAllChildrenKnown_) + "," +
                    "FatherID=" + Database.ToDb(fatherIndex_, 0) + "," +
                    "MotherID=" + Database.ToDb(motherIndex_, 0) + "," +
                    "Sex=" + Database.Iif(isMale_, "'M'", "'F'") + "," +
                    "GedCom=" + Innoval.clsDatabase.ToDb(isIncludeGedcom_) + "," +
                    "MediaID=" + Database.ToDb(mediaIndex, 0) + "," +
                    "LastEditBy=" + Database.ToDb(lastEditBy_) + "," +
                    "LastEditDate=#" + DateTime.Now.ToString("d-MMM-yyyy HH:mm:ss") + "#," +
                    "Comments=" + Database.ToDb(comments_) + " " +
                    "WHERE ID=" + personIndex_.ToString() + ";",
                    database_.cnDB
                );
            oSql.ExecuteNonQuery();

            // Save the sources
            if (sourcesName_ != null)
            {
                sourcesName_.Save();
                foreach (int nSourceID in sourcesName_.Get())
                {
                    SourceNonSpecific.Add(nSourceID);
                }
            }
            if (sourcesDoB_ != null)
            {
                sourcesDoB_.Save();
                foreach (int nSourceID in sourcesDoB_.Get())
                {
                    SourceNonSpecific.Add(nSourceID);
                }
            }
            if (sourcesDoD_ != null)
            {
                sourcesDoD_.Save();
                foreach (int nSourceID in sourcesDoD_.Get())
                {
                    SourceNonSpecific.Add(nSourceID);
                }
            }

            // Save the facts don't bother to attach them to the person. We are destroying the person object shortly
            if (facts_ != null)
            {
                foreach (clsFact oFact in facts_)
                {
                    oFact.Save();

                    // Make sure that all the fact sources are included in the non specific sources for this person.
                    foreach (int nSourceID in oFact.Sources.Get())
                    {
                        SourceNonSpecific.Add(nSourceID);
                    }
                }
            }

            // Save the ToDo items
            if (toDo_ != null)
            {
                foreach (clsToDo oToDo in toDo_)
                {
                    oToDo.Save(Database.cnDB);
                }
            }

            // Save the relationship records
            if (relationships_ != null)
            {
                foreach (clsRelationship oRelationship in relationships_)
                {
                    oRelationship.Save();

                    // Add the location of this relationship
                    if (oRelationship.Location != "")
                    {
                        database_.AddPlace(oRelationship.Location, 1, personIndex_);
                    }

                    // Make sure that all the relationship sources are included in the non specific source for this person.
                    foreach (int nSourceID in oRelationship.SourcePartner.Get())
                    {
                        SourceNonSpecific.Add(nSourceID);
                    }
                    foreach (int nSourceID in oRelationship.SourceStart.Get())
                    {
                        SourceNonSpecific.Add(nSourceID);
                    }
                    foreach (int nSourceID in oRelationship.SourceLocation.Get())
                    {
                        SourceNonSpecific.Add(nSourceID);
                    }
                    foreach (int nSourceID in oRelationship.SourceTerminated.Get())
                    {
                        SourceNonSpecific.Add(nSourceID);
                    }
                    foreach (int nSourceID in oRelationship.SourceEnd.Get())
                    {
                        SourceNonSpecific.Add(nSourceID);
                    }
                }
            }

            // Add the locations associated with this person
            string sBornLocation = BornLocation(false, "");
            if (sBornLocation != "")
            {
                database_.AddPlace(sBornLocation, 1, personIndex_);
            }
            string sDiedLocation = GetSimpleFact(90);
            if (sDiedLocation != "")
            {
                database_.AddPlace(sDiedLocation, 1, personIndex_);
            }
            clsCensusPerson[] oNumCensus = database_.CensusForPerson(personIndex_);
            foreach (clsCensusPerson oCensus in oNumCensus)
            {
                database_.AddPlace(oCensus.HouseholdName, 1, personIndex_);
            }

            // Save the non specifiic sources.  This list may have been added to in the above.
            if (sourcesNonSpecific_ != null)
            {
                sourcesNonSpecific_.Save();
            }

            // Return success
            return true;
        }

        /// <summary>
        /// Returns the full name of the person.  If bShowYears is true then the birth and death year are shown in
        /// brackets after the name.  If bBirthName is true, then for women the original name is shown otherwise the
        /// married name with a nee is shown.
        /// </summary>
        /// <param name="bShowYears">Specify true for the DoB-DoD years to be added to the string.</param>
        /// <param name="bBirthName">Specify true for the birth name.  False for nee maiden name.  For females only.</param>
        /// <returns>The full name of the person.</returns>
        public string GetName
            (
            bool bShowYears,
            bool bBirthName
            )
        {
            StringBuilder sbFullname = new StringBuilder(foreNames_);
            if (sbFullname.Length > 0)
            {
                sbFullname.Append(" ");
            }
            if (bBirthName)
            {
                if (maidenName_ == null)
                {
                    sbFullname.Append(personSurname_);
                }
                else if (maidenName_.Length > 0)
                {
                    sbFullname.Append(maidenName_);
                }
                else
                {
                    sbFullname.Append(personSurname_);
                }
            }
            else
            {
                sbFullname.Append(personSurname_);
                if (maidenName_ != null)
                {
                    if (maidenName_.Length > 0)
                    {
                        sbFullname.Append(" neé ");
                        sbFullname.Append(maidenName_);
                    }
                }
            }

            // Add the birth and death years
            if (bShowYears)
            {
                sbFullname.Append(" (");
                sbFullname.Append(dob_.Format(DateFormat.YearOnly, ""));
                if (!dod_.IsEmpty())
                {
                    sbFullname.Append("-");
                    sbFullname.Append(dod_.Format(DateFormat.YearOnly, ""));
                }
                sbFullname.Append(")");
            }

            // Return the built string
            return sbFullname.ToString();
        }

        #endregion

        #region Parents

        /// <summary>
        /// Gets a collection of clsIDName pairs representing the peiple who could possibly be the father of this person.
        /// It is intended that this function will populate a list box.
        /// </summary>
        /// <returns>An array of clsIDName pairs representing people.</returns>
        public IndexName[] PossibleFathers()
        {
            int startYear = dob_.Date.Year - 100;
            int endYear = dob_.Date.Year - 10;
            return database_.getPeople(ChooseSex.MALE, enumSortOrder.Date, startYear, endYear);
        }

        /// <summary>
        /// Gets a collection of clsIDName pairs representing the peiple who could possibly be the father of this person.
        /// It is intended that this function will populate a list box.
        /// </summary>
        /// <returns>An array of clsIDName pairs representing people.</returns>
        public IndexName[] PossibleMothers()
        {
            int nStartYear = dob_.Date.Year - 100;
            int nEndYear = dob_.Date.Year - 10;
            return database_.getPeople(ChooseSex.FEMALE, enumSortOrder.Date, nStartYear, nEndYear);
        }

        #endregion

        #region Children

        // Returns an array of person IDs representing the children of this person.
        /// <summary>
        /// Returns an array of person IDs representing the children of this person.
        /// </summary>
        /// <returns>An array of person ID representing the children of this person.</returns>
        public int[] GetChildren()
        {
            // Start a list of the children
            ArrayList oList = new ArrayList();

            // Open the list of children of this person
            OleDbCommand oSql;
            if (isMale_)
            {
                oSql = new OleDbCommand("SELECT ID FROM tbl_People WHERE FatherID=" + personIndex_.ToString() + " ORDER BY Born;", database_.cnDB);
            }
            else
            {
                oSql = new OleDbCommand("SELECT ID FROM tbl_People WHERE MotherID=" + personIndex_.ToString() + " ORDER BY Born;", database_.cnDB);
            }

            OleDbDataReader drChildren = oSql.ExecuteReader();
            while (drChildren.Read())
            {
                oList.Add(drChildren.GetInt32(0));
            }
            drChildren.Close();

            // Return the list as a integer array
            return (int[])oList.ToArray(typeof(int));
        }

        // Returns true if the person has children, false otherwise.
        /// <summary>
        /// Returns true if the person has children, false otherwise.
        /// </summary>
        /// <returns></returns>
        public bool HasChildren()
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT ID FROM tbl_People WHERE ");
            if (isMale_)
            {
                sbSql.Append("FatherID");
            }
            else
            {
                sbSql.Append("MotherID");
            }
            sbSql.Append("=" + personIndex_.ToString() + ";");
            OleDbCommand oSql = new OleDbCommand(sbSql.ToString(), database_.cnDB);
            Object oChildren = oSql.ExecuteScalar();
            if (oChildren == null)
            {
                return false;
            }
            return true;
        }

        #endregion

        #region Siblings

        /// <summary>Returns an array of person ID represents the siblings of this person.
        /// This includes half-siblings.
        /// </summary>
        /// <returns>An array of person ID represents the siblings of this person.</returns>
        public int[] GetSiblings()
        {
            // Initialise variables
            ArrayList oList = new ArrayList();

            // Open the list of siblings of this person
            OleDbCommand oSql = new OleDbCommand("SELECT ID FROM tbl_People WHERE ID<>" + personIndex_.ToString() + " AND (FatherID=" + fatherIndex_.ToString() + " OR MotherID=" + motherIndex_.ToString() + ") ORDER BY Born;", database_.cnDB);

            OleDbDataReader drSiblings = oSql.ExecuteReader();
            while (drSiblings.Read())
            {
                oList.Add(drSiblings.GetInt32(0));
            }
            drSiblings.Close();

            // Return the list as a integer array
            return (int[])oList.ToArray(typeof(int));
        }

        #endregion

        #region Relationships

        /// <summary>Gets a collection of clsIDName pairs representing the people who could possibly be in a relationship with the person.
        /// It is intended that this function will populate a list box.
        /// </summary>
        /// <returns>An array clsIDName[] pairs representing people</returns>
        public IndexName[] PossiblePartners()
        {
            ChooseSex nSex = isMale_ ? ChooseSex.FEMALE : ChooseSex.MALE;
            int nStartYear = dob_.Date.Year - database_.RelationshipRange;
            int nEndYear = dob_.Date.Year + database_.RelationshipRange;

            // Return the collection of people
            return database_.getPeople(nSex, enumSortOrder.Date, nStartYear, nEndYear);
        }

        /// <summary>Returns an array of clsRelationship objects representing the relationships for this person.
        /// </summary>
        /// <returns>An array of clsRelationships objects representing the relationships for this person.</returns>
        public clsRelationship[] GetRelationships()
        {
            if (relationships_ == null)
            {
                LoadRelationships();
            }

            // Return the relationships as an array			
            return (clsRelationship[])relationships_.ToArray(typeof(clsRelationship));
        }

        /// <summary>Adds a relationship to the person.
        /// </summary>
        /// <param name="oRelationship">Specify the relationship to add to the collection of relationships/</param>
        /// <returns>True for success, false otherwise.</returns>
        public bool AddRelationship(clsRelationship oRelationship)
        {
            // Load the existing relationships if required
            if (relationships_ == null)
            {
                LoadRelationships();
            }

            // Add the new relationship
            relationships_.Add(oRelationship);

            // Return success
            return true;
        }

        /// <summary>Loads the relationships for this person from the database.
        /// </summary>
        /// <returns>True for success, false otherwise.</returns>
        private bool LoadRelationships()
        {
            // Initialise variables
            relationships_ = new ArrayList();

            // Open the list of partners of this person
            OleDbCommand oSql = null;
            if (isMale_)
            {
                oSql = new OleDbCommand("SELECT ID,FemaleID,TerminatedID,TheDate,StartStatusID,TerminateDate,TerminateStatusID,Location,Comments,RelationshipID,LastEditBy,LastEditDate FROM tbl_Relationships WHERE MaleID=" + personIndex_.ToString() + " ORDER BY TheDate DESC;", database_.cnDB);
            }
            else
            {
                oSql = new OleDbCommand("SELECT ID,MaleID,TerminatedID,TheDate,StartStatusID,TerminateDate,TerminateStatusID,Location,Comments,RelationshipID,LastEditBy,LastEditDate FROM tbl_Relationships WHERE FemaleID=" + personIndex_.ToString() + " ORDER BY TheDate DESC;", database_.cnDB);
            }

            OleDbDataReader drPartners = oSql.ExecuteReader();
            while (drPartners.Read())
            {
                clsRelationship oRelationship = new clsRelationship(drPartners.GetInt32(0), this, drPartners.GetInt32(1));
                oRelationship.TerminatedID = drPartners.GetInt32(2);
                if (drPartners.IsDBNull(3))
                {
                    oRelationship.Start.Status = clsDate.EMPTY;
                }
                else
                {
                    oRelationship.Start.Date = drPartners.GetDateTime(3);
                    oRelationship.Start.Status = drPartners.GetInt16(4);
                }
                if (drPartners.IsDBNull(5))
                {
                    oRelationship.End.Status = clsDate.EMPTY;
                }
                else
                {
                    oRelationship.End.Date = drPartners.GetDateTime(5);
                    oRelationship.End.Status = drPartners.GetInt16(6);
                }
                if (drPartners.IsDBNull(7))
                {
                    oRelationship.Location = "";
                }
                else
                {
                    oRelationship.Location = drPartners.GetString(7);
                }
                if (drPartners.IsDBNull(8))
                {
                    oRelationship.Comments = "";
                }
                else
                {
                    oRelationship.Comments = drPartners.GetString(8);
                }
                oRelationship.TypeID = drPartners.GetInt16(9);
                if (drPartners.IsDBNull(10))
                {
                    oRelationship.LastEditBy = "Steve Walton";
                }
                else
                {
                    oRelationship.LastEditBy = drPartners.GetString(10);
                }
                if (drPartners.IsDBNull(11))
                {
                    oRelationship.LastEditDate = DateTime.Now;
                }
                else
                {
                    oRelationship.LastEditDate = drPartners.GetDateTime(11);
                }
                oRelationship.Dirty = false;

                relationships_.Add(oRelationship);
            }
            drPartners.Close();

            // Return success
            return true;
        }

        #endregion

        #region Facts

        /// <summary>
        /// Adds a fact to the list of facts for this person.
        /// </summary>
        /// <param name="oFact">Specifies the fact to add to this person.</param>
        /// <returns>True for success.  False, otherwise.</returns>
        public bool AddFact
            (
            clsFact oFact
            )
        {
            // Check if the facts array exists
            if (facts_ == null)
            {
                facts_ = new ArrayList();
            }

            // Add to the list of facts
            facts_.Add(oFact);

            // Return success
            return true;
        }

        /// <summary>
        /// Loads all the facts from the database.
        /// </summary>
        /// <returns>True for success.  False, otherwise.</returns>
        private bool GetAllFacts()
        {
            // Initailise the list of facts 
            facts_ = new ArrayList();

            // Get the list of facts from the database
            OleDbCommand oSQL = new OleDbCommand("SELECT ID,TypeID,Rank,Information FROM tbl_Facts WHERE PersonID=" + personIndex_.ToString() + " ORDER BY Rank;", database_.cnDB);
            OleDbDataReader drFact = oSQL.ExecuteReader();
            int nRank = 0;
            while (drFact.Read())
            {
                if (drFact.IsDBNull(2))
                {
                    nRank++;
                }
                else
                {
                    nRank = drFact.GetInt32(2);
                }
                clsFact oFact = new clsFact(drFact.GetInt32(0), this, drFact.GetInt32(1), nRank, drFact.GetString(3));
                facts_.Add(oFact);
            }
            drFact.Close();

            // Return success
            return true;
        }

        /// <summary>
        /// Returns an array of facts of the specified type.
        /// </summary>
        /// <param name="nTypeID">Specify the type of fact.</param>
        /// <returns>An array of facts of the required type.</returns>
        public clsFact[] GetFacts
            (
            int nTypeID
            )
        {
            // Check that some facts exist
            if (facts_ == null)
            {
                // Open the facts
                GetAllFacts();
            }

            // Build a list of relivant facts
            ArrayList oReturn = new ArrayList();
            foreach (clsFact oFact in facts_)
            {
                if (oFact.TypeID == nTypeID && oFact.IsValid())
                {
                    oReturn.Add(oFact);
                }
            }

            // Return the list of facts
            return (clsFact[])(oReturn.ToArray(typeof(clsFact)));
        }
        /// <summary>
        /// Returns all the facts.
        /// </summary>
        /// <returns>Array of all facts for this person.</returns>
        public clsFact[] GetFacts()
        {
            // Check that some facts exist
            if (facts_ == null)
            {
                // Open the facts
                GetAllFacts();
            }

            return (clsFact[])(facts_.ToArray(typeof(clsFact)));
        }

        /// <summary>
        /// Returns the information field from the first fact of the specified type.
        /// This is intended to be used where a fact type only has a single value.
        /// </summary>
        /// <param name="nTypeID">Specifies the type of fact to return.</param>
        /// <returns>The information field as a string.</returns>
        public string GetSimpleFact
            (
            int nTypeID
            )
        {
            clsFact[] oFact = GetFacts(nTypeID);
            if (oFact.Length == 0)
            {
                return "";
            }
            return oFact[0].Information;
        }

        #endregion

        #region Description

        // A long description of the person, covering most all of the facts about this person.
        /// <summary>
        /// A long description of the person, covering most all of the facts about this person.
        /// </summary>
        /// <param name="bHtml">Specify true for a html string, false for a plain ASCII text string.</param>
        /// <param name="bFootnotes">Specify true for footnotes that detail the sources.  (Really RTF only).</param>
        /// <param name="bShowImages">Specify true to show images in the description.  Only in html format output.</param>
        /// <returns>A long description of the person.</returns>
        public string Description(bool bHtml, bool bFootnotes, bool bShowImages, bool bIncludePlaces, bool bIncludeToDo)
        {
            // Initialise a string to hold the result.
            StringBuilder sbDescription = new StringBuilder();

            // Initialise the footnotes
            IndexName[] oSources = null;
            char[] cFootnote = null;
            StringBuilder sbFootnote = null;
            char cNextChar = 'A';
            if (bFootnotes)
            {
                cNextChar = 'A';
                sbFootnote = new StringBuilder();
                oSources = database_.GetSources(enumSortOrder.Date);
                cFootnote = new char[oSources.Length];
                for (int nI = 0; nI < oSources.Length; nI++)
                {
                    cFootnote[nI] = ' ';
                }

                // Add the non specific footnotes now.  To get the requested order.
                Footnote(this.SourceNonSpecific, oSources, cFootnote, sbFootnote, ref cNextChar);
            }

            // Initialise the html            
            if (bHtml && bIncludeToDo)
            {
                sbDescription.Append("<p>");
            }

            if (bHtml)
            {
                // Primary image
                if (bShowImages)
                {
                    if (mediaIndex != 0)
                    {
                        clsMedia oPrimaryMedia = new clsMedia(database_, mediaIndex);
                        sbDescription.Append("<a href=\"media:" + mediaIndex.ToString() + "\">");
                        sbDescription.Append("<img align=\"right\" src=\"" + oPrimaryMedia.FullFilename + "\" border=\"no\" alt=\"" + oPrimaryMedia.Title + "\" height=\"" + oPrimaryMedia.HeightForSpecifiedWidth(150) + "\" width=\"150\" />");
                        sbDescription.Append("</a>");
                    }
                }
            }

            // Name
            if (bHtml)
            {
                sbDescription.Append("<a href=\"Person:" + personIndex_.ToString() + "\">");
            }
            sbDescription.Append(GetName(false, true));
            if (bHtml)
            {
                sbDescription.Append("</a>");
            }
            if (bFootnotes)
            {
                sbDescription.Append(Footnote(SourceName, oSources, cFootnote, sbFootnote, ref cNextChar));
            }

            // Born
            if (dob.IsEmpty())
            {
                sbDescription.Append(" not known when " + ThirdPerson(false) + " was born");
            }
            else
            {
                sbDescription.Append(" was born ");
                sbDescription.Append(dob.Format(DateFormat.FullLong, clsDate.enumPrefix.OnInBeforeAfter));
                if (bFootnotes)
                {
                    sbDescription.Append(Footnote(SourceDoB, oSources, cFootnote, sbFootnote, ref cNextChar));
                }
            }
            clsFact[] oFacts = GetFacts(10);
            if (oFacts.Length > 0)
            {
                sbDescription.Append(" in ");
                if (bHtml)
                {
                    sbDescription.Append(database_.PlaceToHtml(oFacts[0].Information));
                }
                else
                {
                    sbDescription.Append(oFacts[0].Information);
                }
                if (bFootnotes)
                {
                    sbDescription.Append(Footnote(oFacts[0].Sources, oSources, cFootnote, sbFootnote, ref cNextChar));
                }
            }
            sbDescription.Append(". ");

            // Relationships			
            clsRelationship[] oRelationships = GetRelationships();
            for (int nI = oRelationships.Length - 1; nI >= 0; nI--)
            {
                if (oRelationships[nI].IsValid() && oRelationships[nI].IsMarried())
                {
                    clsPerson oRelation = database_.getPerson(oRelationships[nI].PartnerID);
                    if (oRelationships[nI].Start.IsEmpty())
                    {
                        sbDescription.Append(ThirdPerson(true));
                    }
                    else
                    {
                        sbDescription.Append(oRelationships[nI].Start.Format(DateFormat.FullLong, clsDate.enumPrefix.OnInBeforeAfterCaptials));
                        if (bFootnotes)
                        {
                            sbDescription.Append(Footnote(oRelationships[nI].SourceStart, oSources, cFootnote, sbFootnote, ref cNextChar));
                        }
                        if (!this.dob.IsEmpty())
                        {
                            sbDescription.Append(" when " + ThirdPerson(false) + " was " + this.Age(oRelationships[nI].Start) + " old");
                        }
                        sbDescription.Append(", " + ThirdPerson(false));
                    }
                    sbDescription.Append(" married ");
                    if (bHtml)
                    {
                        sbDescription.Append("<a href=\"Person:" + oRelation.ID.ToString() + "\">");
                    }
                    sbDescription.Append(oRelation.GetName(false, true));
                    if (bHtml)
                    {
                        sbDescription.Append("</a>");
                    }
                    if (bFootnotes)
                    {
                        sbDescription.Append(Footnote(oRelationships[nI].SourcePartner, oSources, cFootnote, sbFootnote, ref cNextChar));
                    }
                    if (oRelationships[nI].Location.Length > 0)
                    {
                        sbDescription.Append(" at ");
                        if (bHtml)
                        {
                            sbDescription.Append(database_.PlaceToHtml(oRelationships[nI].Location));
                        }
                        else
                        {
                            sbDescription.Append(oRelationships[nI].Location);
                        }

                        if (bFootnotes)
                        {
                            sbDescription.Append(Footnote(oRelationships[nI].SourceLocation, oSources, cFootnote, sbFootnote, ref cNextChar));
                        }
                    }

                    sbDescription.Append(". ");

                    if (oRelationships[nI].TerminatedID != 1)
                    {
                        bool bTerminated = true;
                        switch (oRelationships[nI].TerminatedID)
                        {
                        case 2:
                            sbDescription.Append("They got divorced");
                            break;
                        case 3:
                            if (isMale_)
                            {
                                bTerminated = false;
                            }
                            else
                            {
                                sbDescription.Append("He died");
                            }
                            break;
                        case 4:
                            if (isMale_)
                            {
                                sbDescription.Append("She died");
                            }
                            else
                            {
                                bTerminated = false;
                            }
                            break;
                        }
                        if (bTerminated)
                        {
                            if (bFootnotes)
                            {
                                sbDescription.Append(Footnote(oRelationships[nI].SourceTerminated, oSources, cFootnote, sbFootnote, ref cNextChar));
                            }

                            if (!oRelationships[nI].End.IsEmpty())
                            {
                                sbDescription.Append(" " + oRelationships[nI].End.Format(DateFormat.FullLong, clsDate.enumPrefix.OnInBeforeAfter));
                                if (bFootnotes)
                                {
                                    sbDescription.Append(Footnote(oRelationships[nI].SourceEnd, oSources, cFootnote, sbFootnote, ref cNextChar));
                                }
                            }

                            sbDescription.Append(". ");
                        }
                    }
                }
            }

            // Education
            sbDescription.Append(ShowFacts(40, " was educated at ", "and", bHtml, bFootnotes, oSources, cFootnote, sbFootnote, ref cNextChar));

            // Occupation
            sbDescription.Append(ShowFacts(20, " worked as a ", "and", bHtml, bFootnotes, oSources, cFootnote, sbFootnote, ref cNextChar));

            // Interests
            sbDescription.Append(ShowFacts(30, " was interested in ", "", bHtml, bFootnotes, oSources, cFootnote, sbFootnote, ref cNextChar));

            // Comments
            sbDescription.Append(ShowFacts(100, " ", "", bHtml, bFootnotes, oSources, cFootnote, sbFootnote, ref cNextChar));

            // Children
            // Don't display children information for people who are known be less than 14 years old.
            int nAge = 15;
            if (!DoD.IsEmpty())
            {
                nAge = DoD.Date.Year - dob.Date.Year;
            }
            else
            {
                nAge = DateTime.Now.Year - dob.Date.Year;
            }
            if (nAge > 14)
            {
                int[] Children = GetChildren();
                if (AllChildrenKnown)
                {
                    switch (Children.Length)
                    {
                    case 0:
                        sbDescription.Append(ThirdPerson(true) + " had no children. ");
                        break;
                    case 1:
                        sbDescription.Append(ThirdPerson(true) + " had 1 child. ");
                        break;
                    default:
                        sbDescription.Append(ThirdPerson(true) + " had " + Children.Length.ToString() + " children. ");
                        break;
                    }
                }
                else
                {
                    switch (Children.Length)
                    {
                    case 0:
                        break;
                    case 1:
                        sbDescription.Append(ThirdPerson(true) + " had at least 1 child. ");
                        break;
                    default:
                        sbDescription.Append(ThirdPerson(true) + " had at least " + Children.Length.ToString() + " children. ");
                        break;
                    }
                }
            }

            // Census information.
            clsCensusPerson[] oCensuses = database_.CensusForPerson(personIndex_);
            string sLastLocation = "";
            foreach (clsCensusPerson oCensus in oCensuses)
            {
                string sLocation = "";
                if (bHtml)
                {
                    sLocation = database_.PlaceToHtml(oCensus.HouseholdName);
                }
                else
                {
                    sLocation = oCensus.HouseholdName;
                }
                if (sLocation != sLastLocation)
                {
                    sbDescription.Append(ThirdPerson(true) + " lived at ");
                    sbDescription.Append(sLocation);
                    sLastLocation = sLocation;
                }
                else
                {
                    sbDescription.Remove(sbDescription.Length - 2, 2);
                    sbDescription.Append(" and");
                }
                sbDescription.Append(" on " + oCensus.Date.ToString("d MMMM yyyy"));
                if (bFootnotes)
                {
                    sbDescription.Append(Footnote(oCensus.HouseholdID, oSources, cFootnote, sbFootnote, ref cNextChar, true));
                }
                sbDescription.Append(". ");
            }

            // Died
            if (!DoD.IsEmpty())
            {
                sbDescription.Append(ThirdPerson(true) + " died " + DoD.Format(DateFormat.FullLong, clsDate.enumPrefix.OnInBeforeAfter));
                if (bFootnotes)
                {
                    sbDescription.Append(Footnote(SourceDoD, oSources, cFootnote, sbFootnote, ref cNextChar));
                }
                oFacts = GetFacts(90);
                if (oFacts.Length > 0)
                {
                    sbDescription.Append(" in ");
                    if (bHtml)
                    {
                        sbDescription.Append(database_.PlaceToHtml(oFacts[0].Information));
                    }
                    else
                    {
                        sbDescription.Append(oFacts[0].Information);
                    }
                    if (bFootnotes)
                    {
                        sbDescription.Append(Footnote(oFacts[0].Sources, oSources, cFootnote, sbFootnote, ref cNextChar));
                    }
                }
                if (!dob.IsEmpty())
                {
                    sbDescription.Append(" when " + ThirdPerson(false) + " was " + Age(this.DoD) + " old");
                }
                sbDescription.Append(". ");
            }
            if (bHtml && bIncludeToDo)
            {
                sbDescription.AppendLine("</p>");
            }

            // Display the comments
            // bIncludeToDo is not really the "correct" flag
            if (bHtml && bIncludeToDo && comments_ != string.Empty)
            {
                sbDescription.AppendLine("<p class=\"Small\" style=\"line-height: 100%\"><strong>Private Comments</strong>: " + comments_ + "</p>");
            }

            // Include the places 
            if (bHtml && bIncludePlaces)
            {
                sbDescription.AppendLine(Places.GoogleMap(600, 300));
            }

            // Show the footnotes
            if (bFootnotes)
            {
                if (sbFootnote.Length > 0)
                {
                    sbDescription.AppendLine("<table cellpadding=\"3\" cellspacing=\"2\">");
                    sbDescription.AppendLine(sbFootnote.ToString());
                    sbDescription.AppendLine("</table> ");
                }
                sbDescription.AppendLine("<p align=\"left\"><span class=\"Small\">Last Edit by " + LastEditBy + " on " + LastEditDate.ToString("d-MMM-yyyy HH:mm:ss") + "</span></p>");
            }

            // Show all the non primary images
            if (bHtml && bShowImages)
            {
                int[] Media = GetMediaID(true);
                if (Media.Length > 0)
                {
                    sbDescription.AppendLine("<table>");
                    foreach (int nMediaID in Media)
                    {
                        clsMedia oMedia = new clsMedia(database_, nMediaID);
                        sbDescription.Append("<tr valign=\"top\">");
                        sbDescription.Append("<td>");
                        sbDescription.Append("<a href=\"media:" + nMediaID.ToString() + "\">");
                        sbDescription.Append("<img src=\"" + oMedia.FullFilename + "\" border=\"no\" height=\"" + oMedia.HeightForSpecifiedWidth(150) + "\" width=\"150\">");
                        sbDescription.Append("</a>");
                        sbDescription.Append("</td>");
                        sbDescription.Append("<td>" + oMedia.Title + "</td>");
                        sbDescription.AppendLine("</tr>");
                    }
                    sbDescription.AppendLine("</table>");
                }
            }

            // Show the ToDo items
            if (bHtml && bIncludeToDo)
            {
                clsToDo[] oToDo = this.GetToDo();
                if (oToDo.Length > 0)
                {
                    sbDescription.AppendLine("<p><b>To Do</b></p>");
                    sbDescription.AppendLine("<table cellpadding=\"3\" cellspacing=\"2\">");
                    foreach (clsToDo oDo in oToDo)
                    {
                        sbDescription.Append("<tr bgcolor=\"silver\">");
                        sbDescription.Append("<td><span class=\"Small\">[" + oDo.Priority.ToString() + "]</span></td>");
                        sbDescription.Append("<td><span class=\"Small\">" + oDo.Description + "</span></td>");
                        sbDescription.AppendLine("</tr>");
                    }
                    sbDescription.AppendLine("</table>");
                }
            }

            // Return the description that has been built
            return sbDescription.ToString();
        }

        // Adds all the facts of the specified type to the description in human readable form.
        /// <summary>
        /// Adds all the facts of the specified type to the description in human readable form.
        /// </summary>
		/// <param name="nFactTypeID">Specifies the fact type to add.</param>
		/// <param name="sPrefix">Specifies the prefix to make the fact human readable</param>
		/// <param name="sJoinWord">Specifies the join word for the last fact in a single sentence.  Use "" for a separate sentence for each fact.</param>
		/// <param name="bHtml">Specify true for a description in Html, false for plain ASCII text.</param>
		/// <param name="bFootnotes">Specify true to add footnote information.</param>
		/// <param name="oSources">Specifies all the available sources of information.</param>
		/// <param name="cFootnote">Specifies the character to use for each source.</param>
		/// <param name="sbFootnote">Footnote sources text for the bottom of the page.</param>
		/// <param name="cNextChar">Character to use for the next footnote marker.</param>
		/// <returns>Human readable string about the fact</returns>
		private string ShowFacts(int nFactTypeID, string sPrefix, string sJoinWord, bool bHtml, bool bFootnotes, IndexName[] oSources, char[] cFootnote, StringBuilder sbFootnote, ref char cNextChar)
        {
            // Start to build a string to return as the result
            StringBuilder sbDescription = new StringBuilder();

            // Get the collection of facts and loop through them
            clsFact[] oFacts = this.GetFacts(nFactTypeID);
            bool bFirst = true;
            bool bFullStop = false;
            for (int nFact = 0; nFact < oFacts.Length; nFact++)
            {
                if (bFirst)
                {
                    sbDescription.Append(this.ThirdPerson(true) + sPrefix + oFacts[nFact].Information);
                    if (nFact == oFacts.Length - 1)
                    {
                        // This is already the last one
                        bFullStop = true;
                    }
                    else if (sJoinWord == "")
                    {
                        // Each fact has it's own sentense
                        bFullStop = true;
                    }
                    else
                    {
                        // Use the join word for subsequent facts
                        bFirst = false;
                    }
                }
                else
                {
                    if (nFact == oFacts.Length - 1)
                    {
                        // Last fact use the join word
                        sbDescription.Append(" " + sJoinWord + " " + oFacts[nFact].Information);
                        bFullStop = true;
                    }
                    else
                    {
                        // Imtermeadate fact just use a comma
                        sbDescription.Append(", " + oFacts[nFact].Information);
                    }
                }

                // Add a footnote (if required)
                if (bFootnotes)
                {
                    sbDescription.Append(Footnote(oFacts[nFact].Sources, oSources, cFootnote, sbFootnote, ref cNextChar));
                }

                // Add a full stop after the last fact
                if (bFullStop)
                {
                    sbDescription.Append(". ");
                }
            }

            // Return the string built
            return sbDescription.ToString();
        }

        // Returns a footnote to include in the description.
        /// <summary>
        /// Returns a footnote to include in the description.
        /// The footnote is selected from the oSources collection.
		/// The oAllSources collection gives the name of all sources.
        /// The cFootnote gives the footnote character for each source.
        /// The sbFootnote contains the footnote text for the bottom of the page.
        /// The cNextChar contains the next footnote character to use for new sources.
		/// </summary>
		/// <param name="oSources">Specifies the sources of information.</param>
		/// <param name="oAllSources">Specifies all the sources for this database.</param>
		/// <param name="cFootnote">Specifies the footnote character for the database sources</param>
		/// <param name="sbFootnote">Returns the text to include at the bottom of the description.</param>
		/// <param name="cNextChar">Returns the next character to use as footnote marker.</param>
		/// <returns>The footnote to include in the description</returns>
		private string Footnote(clsSources oSources, IndexName[] oAllSources, char[] cFootnote, StringBuilder sbFootnote, ref char cNextChar)
        {
            StringBuilder sbReturn = new StringBuilder();
            int[] nSourceID = oSources.Get();
            for (int nI = 0; nI < nSourceID.Length; nI++)
            {
                sbReturn.Append(Footnote(nSourceID[nI], oAllSources, cFootnote, sbFootnote, ref cNextChar, false));
            }

            if (sbReturn.Length == 0)
            {
                return "";
            }

            return "<span class=\"superscript\">" + sbReturn.ToString() + "</span> ";
        }

        private string Footnote(int nSourceID, IndexName[] oAllSources, char[] cFootnote, StringBuilder sbFootnote, ref char cNextChar, bool bHtml)
        {
            int nIndex = 0;
            for (nIndex = 0; oAllSources[nIndex].index != nSourceID; nIndex++) ;
            if (cFootnote[nIndex] == ' ')
            {
                cFootnote[nIndex] = cNextChar;
                cNextChar++;

                sbFootnote.Append("<tr bgcolor=\"silver\"><td><span class=\"Small\">" + cFootnote[nIndex].ToString() + "</span></td><td><a href=\"Source:" + oAllSources[nIndex].index.ToString() + "\"><span class=\"Small\">" + oAllSources[nIndex].name + "</span></a></td></tr>");
            }

            if (bHtml)
            {
                return "<span class=\"superscript\">" + cFootnote[nIndex].ToString() + "</span> ";
            }
            return cFootnote[nIndex].ToString();
        }

        // Returns a 3 line description covering the DoB, birth location and DoD or age.
        /// <summary>
        /// Returns a 3 line description covering the DoB, birth location and DoD or age.
        /// </summary>
		/// <param name="bAge">Specify true to have the age of living people shown</param>
		/// <returns>A short description of the person</returns>
        public string ShortDescription(bool bAge)
        {
            if (dob.IsEmpty())
            {
                return dob.Format(DateFormat.FullLong, "b. ") + BornLocation(true, "\nb. ") + DoD.Format(DateFormat.FullLong, "\nd. ");
            }
            if (DoD.IsEmpty())
            {
                if (dob.Date.Year > DateTime.Now.Year - 110 && bAge)
                {
                    return dob.Format(DateFormat.FullLong, "b. ") + BornLocation(true, "\nb. ") + "\nage " + Age(DateTime.Now);
                }
                else
                {
                    return dob.Format(DateFormat.FullLong, "b. ") + BornLocation(true, "\nb. ");
                }
            }
            return dob.Format(DateFormat.FullLong, "b. ") + BornLocation(true, "\nb. ") + DoD.Format(DateFormat.FullShort, "\nd. ") + " (" + Age(DoD, false) + ")";
        }

        // Returns the age of the person on the specified date.
        /// <summary>
        /// Returns the age of the person on the specified date.
        /// A person who on the day that they are born should be considered to be 1 day old.
        /// Ie no one is ever 0 days old.
        /// </summary>
        /// <param name="dtTheDate">Specify the date to return the age of the person on.</param>
        /// <param name="bUnits">Specify true to add a label for the units.  Specify false to force a unlabelled number of years.</param>
        /// <returns>The age of the person on the specified date as a string.  Usually need to add the word "old" after the string.</returns>
        public string Age(DateTime dtTheDate, bool bUnits)
        {
            // Find the number of years between the two dates
            int nYears = dtTheDate.Year - dob_.Date.Year;
            int nDayOfYearDiff = dtTheDate.DayOfYear - dob_.Date.DayOfYear;

            if (nDayOfYearDiff < -2)
            {
                nYears--;
            }
            else if (nDayOfYearDiff > 2)
            {
            }
            else
            {
                // Need an exact calculation here
                if (dtTheDate.Month <= dob_.Date.Month && dtTheDate.Day < dob_.Date.Day)
                {
                    nYears--;
                }
            }

            // Return the duration as a string
            if (nYears == 0)
            {
                //if(bUnits)
                //{
                TimeSpan oAge = dtTheDate - dob_.Date;
                return (oAge.Days + 1).ToString() + " days";
                //}
                //else
                //{
                //return "0";
                //}
            }
            else
            {
                if (bUnits)
                {
                    return nYears.ToString() + " years";
                }
                else
                {
                    return nYears.ToString();
                }
            }
        }

        // Returns the age of the person on the specified date.
        /// <summary>
        /// Returns the age of the person on the specified date.
        /// </summary>
        /// <param name="dtDate">Specifies the date on which to return the age of the person.</param>
        /// <param name="bUnits">Specify true to add a label for the units.  Specify false to force a unlabelled number of years.</param>
        /// <returns>The age of the person on the specified date as a string.  Usually need to add the word "old" after the string.</returns>
        public string Age(clsDate dtDate, bool bUnits)
        {
            return Age(dtDate.Date, bUnits);
        }

        // Returns the age of the person on the specified date.
        /// <summary>
        /// Returns the age of the person on the specified date.
        /// </summary>
        /// <param name="dtDate">Specifies the date on which to return the age of the person.</param>
        /// <returns>The age of the person on the specified date as a string.  Usually need to add the word "old" after the string.</returns>
        public string Age(clsDate dtDate)
        {
            return Age(dtDate.Date, true);
        }

        // Returns the age of the person on the specified date.
        /// <summary>
        /// Returns the age of the person on the specified date.
        /// </summary>
        /// <param name="dtTheDate">Specify the date to return the age of the person on.</param>
        /// <returns>The age of the person on the specified date as a string.  Usually need to add the word "old" after the string.</returns>
        public string Age(DateTime dtTheDate)
        {
            return Age(dtTheDate, true);
        }

        // Returns he or she according to the person's sex.
        /// <summary>
        /// Returns he or she according to the person's sex.
        /// </summary>
		/// <param name="bCaptialLetter">True for He / She.  False for he / she.</param>
		/// <returns>Returns he or she according to the person's sex.</returns>
        public string ThirdPerson(bool bCaptialLetter)
        {
            if (bCaptialLetter)
            {
                if (isMale_)
                {
                    return "He";
                }
                else
                {
                    return "She";
                }
            }
            else
            {
                if (isMale_)
                {
                    return "he";
                }
                else
                {
                    return "she";
                }
            }
        }

        // Returns the born location fact.
        /// <summary>
        /// Returns the born location fact.
        /// </summary>
		/// <param name="bShort">Specify true for a short location and false for the long location.</param>
		/// <param name="sPrefix">Specify a string to prefix a non null location with.</param>
		/// <returns>Location born.</returns>		
        public string BornLocation(bool bShort, string sPrefix)
        {
            string sLocation;
            if (bShort)
            {
                sLocation = GetSimpleFact(11);
            }
            else
            {
                sLocation = GetSimpleFact(10);
            }
            if (sLocation == "")
            {
                return "";
            }
            return sPrefix + sLocation;
        }

        // The collection of places associated with this person.
        /// <summary>
        /// The collection of places associated with this person.
        /// </summary>
        public clsPlaces Places
        {
            get
            {
                // Start a new collection of places
                clsPlaces oPlaces = new clsPlaces();

                // Add the born place
                clsFact[] oFacts = GetFacts(10);
                if (oFacts.Length > 0)
                {
                    string sBorn = oFacts[0].Information;
                    clsPlace oPlace = database_.GetPlace(sBorn);
                    if (oPlace != null)
                    {
                        oPlaces.AddPlace(oPlace);
                    }
                }

                // Add the married places
                clsRelationship[] oRelationships = GetRelationships();
                foreach (clsRelationship oRelatonship in oRelationships)
                {
                    if (oRelatonship.IsValid())
                    {
                        clsPlace oMarried = database_.GetPlace(oRelatonship.Location);
                        if (oMarried != null)
                        {
                            oPlaces.AddPlace(oMarried);
                        }
                    }
                }

                // Add the location of born children
                int[] nChildren = GetChildren();
                foreach (int nChild in nChildren)
                {
                    clsPerson oChild = new clsPerson(nChild, database_);
                    clsFact[] oChildFacts = oChild.GetFacts(10);
                    if (oChildFacts.Length > 0)
                    {
                        string sChildBorn = oChildFacts[0].Information;
                        clsPlace oPlace = database_.GetPlace(sChildBorn);
                        if (oPlace != null)
                        {
                            oPlaces.AddPlace(oPlace);
                        }
                    }
                }

                // Add the census places
                clsCensusPerson[] oCensuses = database_.CensusForPerson(personIndex_);
                // string sLastLocation = "";
                foreach (clsCensusPerson oCensus in oCensuses)
                {
                    clsPlace oHousehold = database_.GetPlace(oCensus.HouseholdName);
                    oPlaces.AddPlace(oHousehold);
                }

                // Add the died place
                oFacts = GetFacts(90);
                if (oFacts.Length > 0)
                {
                    string sDied = oFacts[0].Information;
                    clsPlace oPlace = database_.GetPlace(sDied);
                    if (oPlace != null)
                    {
                        oPlaces.AddPlace(oPlace);
                    }
                }

                // Return the collection of places.
                return oPlaces;
            }
        }

        #endregion

        #region Media

        /// <summary>
        /// Returns an array of media object indexes associated with this person.
        /// </summary>
        /// <param name="bExcludePrimary">Specify true not to include the primary media object index.</param>
        /// <returns>An array of indexes of media objects.</returns>
        public int[] GetMediaID
            (
            bool bExcludePrimary
            )
        {
            // Decide if to exclude the primary media object
            int nExcludeID = -1;
            if (bExcludePrimary)
            {
                nExcludeID = mediaIndex;
            }

            ArrayList oMedia = new ArrayList();

            string sSql = "SELECT MediaID FROM tbl_AdditionalMediaForPeople WHERE PersonID=" + personIndex_.ToString() + ";";
            OleDbCommand oSql = new OleDbCommand(sSql, database_.cnDB);
            OleDbDataReader drMedia = oSql.ExecuteReader();
            while (drMedia.Read())
            {
                int nMediaID = Innoval.clsDatabase.GetInt(drMedia, "MediaID", 0);
                if (nMediaID != 0 && nMediaID != nExcludeID)
                {
                    oMedia.Add(nMediaID);
                }
            }
            drMedia.Close();

            // Return the collection an array 
            return (int[])oMedia.ToArray(typeof(int));
        }

        /// <summary>
        /// Returns an array of media objects associated with this person.
        /// </summary>
        /// <param name="bExcludePrimary">Specify true not to include the primary media object index.</param>
        /// <returns>An array of media objects.</returns>
        public clsMedia[] GetMedia
            (
            bool bExcludePrimary
            )
        {
            int[] nMediaIDs = GetMediaID(bExcludePrimary);
            clsMedia[] oReturn = new clsMedia[nMediaIDs.Length];
            int nI = 0;
            foreach (int nMediaID in nMediaIDs)
            {
                clsMedia oMedia = new clsMedia(database_, nMediaID);
                oReturn[nI++] = oMedia;
            }

            return oReturn;
        }


        #endregion

        #region ToDo Items

        /// <summary>
        /// Returns the collection of ToDo items attached to this person.
        /// </summary>
        /// <returns>An array of ToDo items attached to this person.</returns>
        public clsToDo[] GetToDo()
        {
            // Check if the ToDo array exists
            if (toDo_ == null)
            {
                LoadToDo();
            }

            return (clsToDo[])toDo_.ToArray(typeof(clsToDo));
        }

        /// <summary>
        /// Loads the collection of ToDo items related this person.
        /// </summary>
        private void LoadToDo()
        {
            // Create a list to hold the ToDo items.
            toDo_ = new ArrayList();

            string sSql = "SELECT * FROM tbl_ToDo WHERE PersonID=" + this.ID + " ORDER BY Priority, ID;";
            OleDbCommand oSql = new OleDbCommand(sSql, database_.cnDB);
            OleDbDataReader drToDo = oSql.ExecuteReader();
            while (drToDo.Read())
            {
                clsToDo oItem = new clsToDo(Innoval.clsDatabase.GetInt(drToDo, "ID", -1), ID, Innoval.clsDatabase.GetInt(drToDo, "Priority", 0), Innoval.clsDatabase.GetString(drToDo, "Description", ""));

                // Add this item to the collection.
                toDo_.Add(oItem);
            }
        }

        /// <summary>
        /// Adds the specified ToDo item to the collection for this person.
        /// </summary>
        /// <param name="oNew">Specifies the ToDo item to add to this person.</param>
        /// <returns>True for success, false otherwise.</returns>
        public bool AddToDo
            (
            clsToDo oNew
            )
        {
            // Check if the ToDo array exists
            if (toDo_ == null)
            {
                LoadToDo();
            }

            // Add to the list of facts
            toDo_.Add(oNew);

            // Return success
            return true;
        }

        #endregion

        #region General Public Properties

        #region Image

        /// <summary>Index of the media object attached to this person.</summary>
		public int MediaID { get { return mediaIndex; } set { mediaIndex = value; } }

        /// <summary>Full filename for an image for the person.  Empty string if no image is specified or can not be found on the hard disk.</summary>
        public string GetImageFilename()
        {
            // Check that a media object is attached to this person
            if (mediaIndex == 0)
            {
                return "";
            }

            // Find the full filename of the media object
            clsMedia oMedia = new clsMedia(database_, mediaIndex);
            return oMedia.FullFilename;
        }

        #endregion

        /// <summary>The ID of the person in the database.</summary>
        public int ID { get { return personIndex_; } set { personIndex_ = value; } }

        /// <summary>Database that this person is stored in.</summary>
        public Database Database { get { return database_; } }

        /// <summary>
        /// The surname this person had at birth.
        /// </summary>
        public string BirthSurname
        {
            get
            {
                if (isMale_)
                {
                    return personSurname_;
                }
                else
                {
                    if (maidenName_ == "")
                    {
                        return personSurname_;
                    }
                    else
                    {
                        return maidenName_;
                    }
                }
            }
        }

        /// <summary>Surname of this person.</summary>
        public string surname { get { return personSurname_; } set { personSurname_ = value; } }

        /// <summary>First names of this person.</summary>
        public string forenames { get { return foreNames_; } set { foreNames_ = value; } }

        /// <summary>Maiden name of this person.</summary>
        public string maidenname { get { return maidenName_; } set { maidenName_ = value; } }

        /// <summary>Date of birth of this person.</summary>
        public clsDate dob { get { return dob_; } }

        /// <summary>Date of death of this person.</summary>
        public clsDate DoD { get { return dod_; } }

        // ID of the father of this person.
        /// <summary>
        /// ID of the father of this person.
        /// Zero is unknown father.
        /// </summary>
		public int FatherID
        {
            get
            {
                return fatherIndex_;
            }
            set
            {
                fatherIndex_ = value;
            }
        }

        // ID of the mother of this person.
        /// <summary>
        /// ID of the mother of this person.
        /// Zero is unknown mother.
        /// </summary>
		public int MotherID
        {
            get
            {
                return motherIndex_;
            }
            set
            {
                motherIndex_ = value;
            }
        }



        /// <summary>True if this person is male.  False, otherwise.</summary>
        public bool isMale
        {
            get { return isMale_; }
            set { isMale_ = value; }
        }

        // True if this person is female.  False, otherwise.
        /// <summary>
        /// True if this person is female.  False, otherwise.
        /// </summary>
        public bool Female
        {
            get
            {
                return !isMale_;
            }
            set
            {
                isMale_ = !value;
            }
        }

        /// <summary>True if all the children of this person are known.  False, otherwise.</summary>
        public bool AllChildrenKnown { get { return isAllChildrenKnown_; } set { isAllChildrenKnown_ = value; } }

        // True if the person should be included in the gedcom file. False, otherwise.
        /// <summary>
        /// True if the person should be included in the gedcom file. False, otherwise.
        /// </summary>
        public bool IncludeInGedcom
        {
            get
            {
                return isIncludeGedcom_;
            }
            set
            {
                isIncludeGedcom_ = value;
            }
        }

        /// <summary>User comments for this person.</summary>
        public string Comments { get { return comments_; } set { comments_ = value; } }

        // A clsSources object for the person name.
        /// <summary>
        /// A clsSources object for the person name.
        /// </summary>
		public clsSources SourceName
        {
            get
            {
                if (sourcesName_ == null)
                {
                    sourcesName_ = new clsSources(personIndex_, 1, database_);
                }
                return sourcesName_;
            }
        }

        /// <summary>A clsSources object for the person date of birth.</summary>
        public clsSources SourceDoB
        {
            get
            {
                if (sourcesDoB_ == null)
                {
                    sourcesDoB_ = new clsSources(personIndex_, 2, database_);
                }
                return sourcesDoB_;
            }
        }

        /// <summary>A clsSources object for the person date of death.</summary>
        public clsSources SourceDoD
        {
            get
            {
                if (sourcesDoD_ == null)
                {
                    sourcesDoD_ = new clsSources(personIndex_, 3, database_);
                }
                return sourcesDoD_;
            }
        }

        /// <summary>A collection of all the sources used for this person.  Including the non specific sources.</summary>
        public clsSources SourceNonSpecific
        {
            get
            {
                if (sourcesNonSpecific_ == null)
                {
                    sourcesNonSpecific_ = new clsSources(personIndex_, 0, database_);
                }
                return sourcesNonSpecific_;
            }
        }

        /// <summary>Short term information about the person.  This is not saved in the database.</summary>
        public string Tag
        {
            get { return tag_; }
            set { tag_ = value; }
        }

        /// <summary>Name of the user who wrote the last edit.</summary>
        public string LastEditBy { get { return lastEditBy_; } set { lastEditBy_ = value; } }

        /// <summary>Date and time of the last edit.</summary>
        public DateTime LastEditDate { get { return lastEditDate_; } set { lastEditDate_ = value; } }

        #endregion
    }
}
