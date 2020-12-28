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
		private clsDatabase m_oDb;

		/// <summary>ID of the person.</summary>
		private int m_nID;

		/// <summary>Surname of the person.</summary>
		private string m_sSurname;	

		/// <summary>Forenames of the person.</summary>
		private string m_sForenames;

		/// <summary>Maiden name of the person.</summary>
		private string m_sMaidenname;

		/// <summary>Date of the birth for the person.</summary>
		private clsDate m_DoB;

		/// <summary>Date of death for the person.</summary>
		private clsDate m_DoD;

		/// <summary>ID of the person's father.</summary>
		private int m_nFatherID;

		/// <summary>ID of the person's mother.</summary>
		private int m_nMotherID;

		/// <summary>True if the person is male.</summary>
		private bool	m_bMale;

		/// <summary>True if all the children of the person are known.</summary>
		private bool m_bAllChildren;

		/// <summary>Index of the media object attached to this person.</summary>
		private int m_nMediaID;

		/*
		/// <summary>Filename of an image for the person.  Not the full path.</summary>
		private string m_sImageFilename;

		/// <summary>Full filename of an image for the person.  This is not stored in the database.</summary>
		private string m_sImagePath;
         */ 

		/// <summary>User comments for the person.</summary>
		private string m_sComments;

        // True if this person should be included in the gedcom file, false otherwise.
        /// <summary>
        /// True if this person should be included in the gedcom file, false otherwise.
        /// </summary>
        private bool m_bIncludeGedcom;

		/// <summary>Short term comments / information for the person.  This is not saved in the database.</summary>
		private string m_sTag;

		/// <summary>Array of facts about this person.</summary>
		private ArrayList m_oFacts;
		
		/// <summary>Array of relationships for this person.</summary>
		private ArrayList m_oRelationships;
		
		/// <summary>Sources for the name data.</summary>
		private clsSources m_sourcesName;

		/// <summary>Sources for the date of birth data.</summary>
		private clsSources m_sourcesDoB;

		/// <summary>Sources for the date of death data.</summary>
		private clsSources m_sourcesDoD;

		/// <summary>All sources for this person.  Including the sources for non specific facts.</summary>
		private clsSources m_sourcesNonSpecific;

		/// <summary>Name of the user who wrote the last edit.</summary>
		private string m_sLastEditBy;

		/// <summary>Date and time of the last edit.</summary>
		private DateTime m_dtLastEditDate;

        /// <summary>Collection of ToDo items about this person.</summary>
        private ArrayList m_oToDo;

		#endregion

		#region Constructors

        // Creates an empty person object.
        /// <summary>
        /// Creates an empty person object.
        /// </summary>
		public clsPerson()
		{
			m_nFatherID = 0;
			m_nMotherID = 0;
			m_bAllChildren = false;
			m_oFacts = null;
			m_DoB = new clsDate();
			m_DoD = new clsDate();
            m_bIncludeGedcom = true;
			m_sTag = "";
//			m_sImageFilename = "";
//			m_sImagePath = "";
            m_oToDo = null;

			m_oRelationships = null;
			
			m_sourcesName = null;
			m_sourcesDoB = null;
			m_sourcesDoD = null;
			m_sourcesNonSpecific = null;
            
		}

        // Creates an empty person object in the specfied database.
        /// <summary>
        /// Creates an empty person object in the specfied database.
        /// </summary>
		/// <param name="oDb">Specify the database to contain this person</param>
        public clsPerson(clsDatabase oDb)
            : this() // This makes the program call the () constructor before the code is called.
        {
            m_oDb = oDb;
        }

        // Create a person object from the specified database record.
        /// <summary>
        /// Create a person object from the specified database record.
        /// Loads the specified person from the specified database.
		/// </summary>
		/// <param name="nPersonID">Specify the ID of the person to load.</param>
		/// <param name="oDb">Specify the family tree database to load the person from.</param>
		public clsPerson			(			int nPersonID,			clsDatabase oDb			):this(oDb) // This makes the program call the (clsDatabase) constructor before the code is called.
        {
            // Open the specified person			
            OleDbCommand oSql = new OleDbCommand("SELECT Surname,Forenames,MaidenName,Born,BornStatusID,Died,DiedStatusID,FatherID,MotherID,Sex,ChildrenKnown,GedCom,Comments,MediaID,LastEditBy,LastEditDate FROM tbl_People WHERE ID=" + nPersonID.ToString() + ";", oDb.cnDB);
            OleDbDataReader drPerson = oSql.ExecuteReader();
            if(drPerson.Read())
            {
                m_nID = nPersonID;
                if(drPerson.IsDBNull(0))
                {
                    m_sSurname = "";
                }
                else
                {
                    m_sSurname = drPerson.GetString(0);
                }
                if(drPerson.IsDBNull(1))
                {
                    m_sForenames = "";
                }
                else
                {
                    m_sForenames = drPerson.GetString(1);
                }
                if(drPerson.IsDBNull(2))
                {
                    m_sMaidenname = "";
                }
                else
                {
                    m_sMaidenname = drPerson.GetString(2);
                }
                if(drPerson.IsDBNull(3))
                {
                    m_DoB.Status = clsDate.EMPTY;
                }
                else
                {
                    m_DoB.Date = drPerson.GetDateTime(3);
                    m_DoB.Status = drPerson.GetInt16(4);
                }

                if(drPerson.IsDBNull(5))
                {
                    m_DoD.Status = clsDate.EMPTY;
                }
                else
                {
                    m_DoD.Date = drPerson.GetDateTime(5);
                    m_DoD.Status = drPerson.GetInt16(6);
                }
                if(drPerson.IsDBNull(7))
                {
                    m_nFatherID = 0;
                }
                else
                {
                    m_nFatherID = drPerson.GetInt32(7);
                }
                if(drPerson.IsDBNull(8))
                {
                    m_nMotherID = 0;
                }
                else
                {
                    m_nMotherID = drPerson.GetInt32(8);
                }
                if(drPerson.IsDBNull(9))
                {
                    m_bMale = true;
                }
                else
                {
                    if(drPerson.GetString(9) == "M")
                    {
                        m_bMale = true;
                    }
                    else
                    {
                        m_bMale = false;
                    }
                }

                m_bAllChildren = drPerson.GetBoolean(10);
                m_bIncludeGedcom = Innoval.clsDatabase.GetBool(drPerson, "Gedcom", true);
                m_sComments = clsDatabase.GetString(drPerson, "Comments", "");
                m_nMediaID = clsDatabase.GetInt(drPerson, "MediaID", 0);
                //				m_sImageFilename = "";
                m_sLastEditBy = clsDatabase.GetString(drPerson, "LastEditBy", "Steve Walton");
                m_dtLastEditDate = clsDatabase.GetDateTime(drPerson, "LastEditDate", DateTime.Now);
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
            if(m_nID == 0)
            {
                // Find the new ID
                oSql = new OleDbCommand("SELECT MAX(ID) AS NewID FROM tbl_People;", m_oDb.cnDB);
                m_nID = (int)oSql.ExecuteScalar() + 1;

                // Create a new person record
                oSql = new OleDbCommand("INSERT INTO tbl_People (ID,Surname) VALUES (" + m_nID.ToString() + "," + clsDatabase.ToDb(m_sSurname) + ");", m_oDb.cnDB);
                oSql.ExecuteNonQuery();

                // Update the related child records
                if(m_sourcesName != null)
                {
                    m_sourcesName.PersonID = m_nID;
                }
                if(m_sourcesDoB != null)
                {
                    m_sourcesDoB.PersonID = m_nID;
                }
                if(m_sourcesDoD != null)
                {
                    m_sourcesDoD.PersonID = m_nID;
                }
                if(m_sourcesNonSpecific != null)
                {
                    m_sourcesNonSpecific.PersonID = m_nID;
                }
            }
            else
            {
                // Update the places associated with this person
                m_oDb.PlaceDelink(1, m_nID);
            }

            // Update the existing record
            oSql = new OleDbCommand
                (
                    "UPDATE tbl_People SET " +
                    "Surname=" + clsDatabase.ToDb(m_sSurname) + "," +
                    "Forenames=" + clsDatabase.ToDb(m_sForenames) + "," +
                    "MaidenName=" + clsDatabase.ToDb(m_sMaidenname) + "," +
                    "Born=" + clsDatabase.ToDb(m_DoB) + "," +
                    "BornStatusID=" + m_DoB.Status.ToString() + "," +
                    "Died=" + clsDatabase.ToDb(m_DoD) + "," +
                    "DiedStatusID=" + m_DoD.Status.ToString() + "," +
                    "ChildrenKnown=" + clsDatabase.ToDb(m_bAllChildren) + "," +
                    "FatherID=" + clsDatabase.ToDb(m_nFatherID, 0) + "," +
                    "MotherID=" + clsDatabase.ToDb(m_nMotherID, 0) + "," +
                    "Sex=" + clsDatabase.Iif(m_bMale, "'M'", "'F'") + "," +
                    "GedCom=" + Innoval.clsDatabase.ToDb(m_bIncludeGedcom) + "," +
                    "MediaID=" + clsDatabase.ToDb(m_nMediaID, 0) + "," +
                    "LastEditBy=" + clsDatabase.ToDb(m_sLastEditBy) + "," +
                    "LastEditDate=#" + DateTime.Now.ToString("d-MMM-yyyy HH:mm:ss") + "#," +
                    "Comments=" + clsDatabase.ToDb(m_sComments) + " " +
                    "WHERE ID=" + m_nID.ToString() + ";",
                    m_oDb.cnDB
                );
            oSql.ExecuteNonQuery();

            // Save the sources
            if(m_sourcesName != null)
            {
                m_sourcesName.Save();
                foreach(int nSourceID in m_sourcesName.Get())
                {
                    SourceNonSpecific.Add(nSourceID);
                }
            }
            if(m_sourcesDoB != null)
            {
                m_sourcesDoB.Save();
                foreach(int nSourceID in m_sourcesDoB.Get())
                {
                    SourceNonSpecific.Add(nSourceID);
                }
            }
            if(m_sourcesDoD != null)
            {
                m_sourcesDoD.Save();
                foreach(int nSourceID in m_sourcesDoD.Get())
                {
                    SourceNonSpecific.Add(nSourceID);
                }
            }

            // Save the facts don't bother to attach them to the person. We are destroying the person object shortly
            if(m_oFacts != null)
            {
                foreach(clsFact oFact in m_oFacts)
                {
                    oFact.Save();

                    // Make sure that all the fact sources are included in the non specific sources for this person.
                    foreach(int nSourceID in oFact.Sources.Get())
                    {
                        SourceNonSpecific.Add(nSourceID);
                    }
                }
            }

            // Save the ToDo items
            if(m_oToDo != null)
            {
                foreach(clsToDo oToDo in m_oToDo)
                {
                    oToDo.Save(Database.cnDB);
                }
            }

            // Save the relationship records
            if(m_oRelationships != null)
            {
                foreach(clsRelationship oRelationship in m_oRelationships)
                {
                    oRelationship.Save();

                    // Add the location of this relationship
                    if(oRelationship.Location != "")
                    {
                        m_oDb.AddPlace(oRelationship.Location, 1, m_nID);
                    }

                    // Make sure that all the relationship sources are included in the non specific source for this person.
                    foreach(int nSourceID in oRelationship.SourcePartner.Get())
                    {
                        SourceNonSpecific.Add(nSourceID);
                    }
                    foreach(int nSourceID in oRelationship.SourceStart.Get())
                    {
                        SourceNonSpecific.Add(nSourceID);
                    }
                    foreach(int nSourceID in oRelationship.SourceLocation.Get())
                    {
                        SourceNonSpecific.Add(nSourceID);
                    }
                    foreach(int nSourceID in oRelationship.SourceTerminated.Get())
                    {
                        SourceNonSpecific.Add(nSourceID);
                    }
                    foreach(int nSourceID in oRelationship.SourceEnd.Get())
                    {
                        SourceNonSpecific.Add(nSourceID);
                    }
                }
            }

            // Add the locations associated with this person
            string sBornLocation = BornLocation(false, "");
            if(sBornLocation != "")
            {
                m_oDb.AddPlace(sBornLocation, 1, m_nID);
            }
            string sDiedLocation = GetSimpleFact(90);
            if(sDiedLocation != "")
            {
                m_oDb.AddPlace(sDiedLocation, 1, m_nID);
            }
            clsCensusPerson[] oNumCensus = m_oDb.CensusForPerson(m_nID);
            foreach(clsCensusPerson oCensus in oNumCensus)
            {
                m_oDb.AddPlace(oCensus.HouseholdName, 1, m_nID);
            }

            // Save the non specifiic sources.  This list may have been added to in the above.
            if(m_sourcesNonSpecific != null)
            {
                m_sourcesNonSpecific.Save();
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
            StringBuilder sbFullname = new StringBuilder(m_sForenames);
            if(sbFullname.Length > 0)
            {
                sbFullname.Append(" ");
            }
            if(bBirthName)
            {
                if(m_sMaidenname == null)
                {
                    sbFullname.Append(m_sSurname);
                }
                else if(m_sMaidenname.Length > 0)
                {
                    sbFullname.Append(m_sMaidenname);
                }
                else
                {
                    sbFullname.Append(m_sSurname);
                }
            }
            else
            {
                sbFullname.Append(m_sSurname);
                if(m_sMaidenname != null)
                {
                    if(m_sMaidenname.Length > 0)
                    {
                        sbFullname.Append(" neé ");
                        sbFullname.Append(m_sMaidenname);
                    }
                }
            }

            // Add the birth and death years
            if(bShowYears)
            {
                sbFullname.Append(" (");
                sbFullname.Append(m_DoB.Format(DateFormat.YearOnly,""));
                if(!m_DoD.IsEmpty())
                {
                    sbFullname.Append("-");
                    sbFullname.Append(m_DoD.Format(DateFormat.YearOnly,""));
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
		public clsIDName[] PossibleFathers()
		{
			int nStartYear = m_DoB.Date.Year - 100;
			int nEndYear = m_DoB.Date.Year - 10;
			return m_oDb.GetPeople(enumChooseSex.Male,enumSortOrder.Date,nStartYear,nEndYear);
		}

		/// <summary>
		/// Gets a collection of clsIDName pairs representing the peiple who could possibly be the father of this person.
		/// It is intended that this function will populate a list box.
		/// </summary>
		/// <returns>An array of clsIDName pairs representing people.</returns>
		public clsIDName[] PossibleMothers()
		{
			int nStartYear = m_DoB.Date.Year - 100;
			int nEndYear = m_DoB.Date.Year - 10;
			return m_oDb.GetPeople(enumChooseSex.Female,enumSortOrder.Date,nStartYear,nEndYear);
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
            if(m_bMale)
			{
				oSql = new OleDbCommand("SELECT ID FROM tbl_People WHERE FatherID=" + m_nID.ToString() + " ORDER BY Born;",m_oDb.cnDB);
			}
			else
			{
				oSql = new OleDbCommand("SELECT ID FROM tbl_People WHERE MotherID=" + m_nID.ToString() + " ORDER BY Born;",m_oDb.cnDB);
			}
			
			OleDbDataReader drChildren = oSql.ExecuteReader();
			while(drChildren.Read())
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
            if(m_bMale)
            {
                sbSql.Append("FatherID");
            }
            else
            {
                sbSql.Append("MotherID");
            }
            sbSql.Append("=" + m_nID.ToString() + ";");
            OleDbCommand oSql = new OleDbCommand(sbSql.ToString(),m_oDb.cnDB);
            Object oChildren = oSql.ExecuteScalar();
            if(oChildren == null)
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
            OleDbCommand oSql = new OleDbCommand("SELECT ID FROM tbl_People WHERE ID<>" + m_nID.ToString() + " AND (FatherID=" + m_nFatherID.ToString() + " OR MotherID=" + m_nMotherID.ToString() + ") ORDER BY Born;", m_oDb.cnDB);

            OleDbDataReader drSiblings = oSql.ExecuteReader();
            while(drSiblings.Read())
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
        public clsIDName[] PossiblePartners()
        {
            enumChooseSex nSex = m_bMale ? enumChooseSex.Female : enumChooseSex.Male;
            int nStartYear = m_DoB.Date.Year - m_oDb.RelationshipRange;
            int nEndYear = m_DoB.Date.Year + m_oDb.RelationshipRange;

            // Return the collection of people
            return m_oDb.GetPeople(nSex, enumSortOrder.Date, nStartYear, nEndYear);
        }

		/// <summary>Returns an array of clsRelationship objects representing the relationships for this person.
		/// </summary>
		/// <returns>An array of clsRelationships objects representing the relationships for this person.</returns>
        public clsRelationship[] GetRelationships()
        {
            if(m_oRelationships == null)
            {
                LoadRelationships();
            }

            // Return the relationships as an array			
            return (clsRelationship[])m_oRelationships.ToArray(typeof(clsRelationship));
        }
		
		/// <summary>Adds a relationship to the person.
		/// </summary>
		/// <param name="oRelationship">Specify the relationship to add to the collection of relationships/</param>
		/// <returns>True for success, false otherwise.</returns>
        public bool AddRelationship(clsRelationship oRelationship)
        {
            // Load the existing relationships if required
            if(m_oRelationships == null)
            {
                LoadRelationships();
            }

            // Add the new relationship
            m_oRelationships.Add(oRelationship);

            // Return success
            return true;
        }
		
		/// <summary>Loads the relationships for this person from the database.
		/// </summary>
		/// <returns>True for success, false otherwise.</returns>
        private bool LoadRelationships()
        {
            // Initialise variables
            m_oRelationships = new ArrayList();

            // Open the list of partners of this person
            OleDbCommand oSql = null;
            if(m_bMale)
            {
                oSql = new OleDbCommand("SELECT ID,FemaleID,TerminatedID,TheDate,StartStatusID,TerminateDate,TerminateStatusID,Location,Comments,RelationshipID,LastEditBy,LastEditDate FROM tbl_Relationships WHERE MaleID=" + m_nID.ToString() + " ORDER BY TheDate DESC;", m_oDb.cnDB);
            }
            else
            {
                oSql = new OleDbCommand("SELECT ID,MaleID,TerminatedID,TheDate,StartStatusID,TerminateDate,TerminateStatusID,Location,Comments,RelationshipID,LastEditBy,LastEditDate FROM tbl_Relationships WHERE FemaleID=" + m_nID.ToString() + " ORDER BY TheDate DESC;", m_oDb.cnDB);
            }

            OleDbDataReader drPartners = oSql.ExecuteReader();
            while(drPartners.Read())
            {
                clsRelationship oRelationship = new clsRelationship(drPartners.GetInt32(0), this, drPartners.GetInt32(1));
                oRelationship.TerminatedID = drPartners.GetInt32(2);
                if(drPartners.IsDBNull(3))
                {
                    oRelationship.Start.Status = clsDate.EMPTY;
                }
                else
                {
                    oRelationship.Start.Date = drPartners.GetDateTime(3);
                    oRelationship.Start.Status = drPartners.GetInt16(4);
                }
                if(drPartners.IsDBNull(5))
                {
                    oRelationship.End.Status = clsDate.EMPTY;
                }
                else
                {
                    oRelationship.End.Date = drPartners.GetDateTime(5);
                    oRelationship.End.Status = drPartners.GetInt16(6);
                }
                if(drPartners.IsDBNull(7))
                {
                    oRelationship.Location = "";
                }
                else
                {
                    oRelationship.Location = drPartners.GetString(7);
                }
                if(drPartners.IsDBNull(8))
                {
                    oRelationship.Comments = "";
                }
                else
                {
                    oRelationship.Comments = drPartners.GetString(8);
                }
                oRelationship.TypeID = drPartners.GetInt16(9);
                if(drPartners.IsDBNull(10))
                {
                    oRelationship.LastEditBy = "Steve Walton";
                }
                else
                {
                    oRelationship.LastEditBy = drPartners.GetString(10);
                }
                if(drPartners.IsDBNull(11))
                {
                    oRelationship.LastEditDate = DateTime.Now;
                }
                else
                {
                    oRelationship.LastEditDate = drPartners.GetDateTime(11);
                }
                oRelationship.Dirty = false;

                m_oRelationships.Add(oRelationship);
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
            if(m_oFacts == null)
            {
                m_oFacts = new ArrayList();
            }

            // Add to the list of facts
            m_oFacts.Add(oFact);

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
			m_oFacts = new ArrayList();

			// Get the list of facts from the database
			OleDbCommand oSQL = new OleDbCommand("SELECT ID,TypeID,Rank,Information FROM tbl_Facts WHERE PersonID=" + m_nID.ToString() + " ORDER BY Rank;",m_oDb.cnDB);
			OleDbDataReader drFact = oSQL.ExecuteReader();
			int nRank = 0;
			while(drFact.Read())
			{
				if(drFact.IsDBNull(2))
				{
					nRank++;
				}
				else
				{
					nRank = drFact.GetInt32(2);
				}
				clsFact oFact = new clsFact(drFact.GetInt32(0),this,drFact.GetInt32(1),nRank,drFact.GetString(3));
				m_oFacts.Add(oFact);
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
			if(m_oFacts==null)
			{
				// Open the facts
				GetAllFacts();
			}

			// Build a list of relivant facts
			ArrayList oReturn = new ArrayList();
			foreach(clsFact oFact in m_oFacts)
			{
				if(oFact.TypeID==nTypeID&&oFact.IsValid())
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
			if(m_oFacts==null)
			{
				// Open the facts
				GetAllFacts();
			}

			return (clsFact[])(m_oFacts.ToArray(typeof(clsFact)));
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
			if(oFact.Length==0)
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
			clsIDName[] oSources=null;
			char[] cFootnote=null;
			StringBuilder sbFootnote=null;
			char cNextChar='A';
			if(bFootnotes)
			{
				cNextChar = 'A';
				sbFootnote = new StringBuilder();
				oSources = m_oDb.GetSources(enumSortOrder.Date);
				cFootnote = new char[oSources.Length];
				for(int nI=0;nI<oSources.Length;nI++)
				{
					cFootnote[nI] = ' ';
				}

				// Add the non specific footnotes now.  To get the requested order.
				Footnote(this.SourceNonSpecific,oSources,cFootnote,sbFootnote,ref cNextChar);
			}
			
			// Initialise the html            
            if(bHtml && bIncludeToDo)
            {
                sbDescription.Append("<p>");
            }

            if(bHtml)
            {
                // Primary image
                if(bShowImages)
                {
                    if(m_nMediaID != 0)
                    {
                        clsMedia oPrimaryMedia = new clsMedia(m_oDb,m_nMediaID);
                        sbDescription.Append("<a href=\"media:" + m_nMediaID.ToString() + "\">");
                        sbDescription.Append("<img align=\"right\" src=\"" + oPrimaryMedia.FullFilename + "\" border=\"no\" alt=\"" + oPrimaryMedia.Title + "\" height=\"" + oPrimaryMedia.HeightForSpecifiedWidth(150) + "\" width=\"150\" />");
                        sbDescription.Append("</a>");
                    }
                }
            }

			// Name
            if(bHtml)
            {
                sbDescription.Append("<a href=\"Person:" + m_nID.ToString() + "\">");
            }
            sbDescription.Append(GetName(false, true));
            if(bHtml)
            {
                sbDescription.Append("</a>");
            }
            if(bFootnotes)
			{
				sbDescription.Append(Footnote(SourceName,oSources,cFootnote,sbFootnote,ref cNextChar));
			}

			// Born
			if(DoB.IsEmpty())
			{
				sbDescription.Append(" not known when " + ThirdPerson(false) + " was born");
			}
			else
			{
				sbDescription.Append(" was born ");
                sbDescription.Append(DoB.Format(DateFormat.FullLong,clsDate.enumPrefix.OnInBeforeAfter));
				if(bFootnotes)
				{
					sbDescription.Append(Footnote(SourceDoB,oSources,cFootnote,sbFootnote,ref cNextChar));
				}
			}
            clsFact[] oFacts = GetFacts(10);
			if(oFacts.Length>0)
			{
				sbDescription.Append(" in ");
                if(bHtml)
                {
                    sbDescription.Append(m_oDb.PlaceToHtml(oFacts[0].Information));
                }
                else
                {
                    sbDescription.Append(oFacts[0].Information);
                }
                if(bFootnotes)
				{
					sbDescription.Append(Footnote(oFacts[0].Sources,oSources,cFootnote,sbFootnote,ref cNextChar));
				}							
			}
			sbDescription.Append(". ");

			// Relationships			
			clsRelationship[] oRelationships = GetRelationships();
            for(int nI = oRelationships.Length - 1;nI >= 0;nI--)
            {
                if(oRelationships[nI].IsValid() && oRelationships[nI].IsMarried())
                {
                    clsPerson oRelation = m_oDb.GetPerson(oRelationships[nI].PartnerID);
                    if(oRelationships[nI].Start.IsEmpty())
                    {
                        sbDescription.Append(ThirdPerson(true));
                    }
                    else
                    {
                        sbDescription.Append(oRelationships[nI].Start.Format(DateFormat.FullLong,clsDate.enumPrefix.OnInBeforeAfterCaptials));
                        if(bFootnotes)
                        {
                            sbDescription.Append(Footnote(oRelationships[nI].SourceStart,oSources,cFootnote,sbFootnote,ref cNextChar));
                        }
                        if(!this.DoB.IsEmpty())
                        {
                            sbDescription.Append(" when " + ThirdPerson(false) + " was " + this.Age(oRelationships[nI].Start) + " old");
                        }
                        sbDescription.Append(", " + ThirdPerson(false));
                    }
                    sbDescription.Append(" married ");
                    if(bHtml)
                    {
                        sbDescription.Append("<a href=\"Person:" + oRelation.ID.ToString() + "\">");
                    }
                    sbDescription.Append(oRelation.GetName(false, true));
                    if(bHtml)
                    {
                        sbDescription.Append("</a>");
                    }
                    if(bFootnotes)
                    {
                        sbDescription.Append(Footnote(oRelationships[nI].SourcePartner,oSources,cFootnote,sbFootnote,ref cNextChar));
                    }
                    if(oRelationships[nI].Location.Length > 0)
                    {
                        sbDescription.Append(" at ");
                        if(bHtml)
                        {
                            sbDescription.Append(m_oDb.PlaceToHtml(oRelationships[nI].Location));
                        }
                        else
                        {
                            sbDescription.Append(oRelationships[nI].Location);
                        }

                        if(bFootnotes)
                        {
                            sbDescription.Append(Footnote(oRelationships[nI].SourceLocation,oSources,cFootnote,sbFootnote,ref cNextChar));
                        }
                    }

                    sbDescription.Append(". ");

                    if(oRelationships[nI].TerminatedID != 1)
                    {
                        bool bTerminated = true;
                        switch(oRelationships[nI].TerminatedID)
                        {
                        case 2:
                            sbDescription.Append("They got divorced");
                            break;
                        case 3:
                            if(m_bMale)
                            {
                                bTerminated = false;
                            }
                            else
                            {
                                sbDescription.Append("He died");
                            }
                            break;
                        case 4:
                            if(m_bMale)
                            {
                                sbDescription.Append("She died");
                            }
                            else
                            {
                                bTerminated = false;
                            }
                            break;
                        }
                        if(bTerminated)
                        {
                            if(bFootnotes)
                            {
                                sbDescription.Append(Footnote(oRelationships[nI].SourceTerminated,oSources,cFootnote,sbFootnote,ref cNextChar));
                            }

                            if(!oRelationships[nI].End.IsEmpty())
                            {
                                sbDescription.Append(" " + oRelationships[nI].End.Format(DateFormat.FullLong,clsDate.enumPrefix.OnInBeforeAfter));
                                if(bFootnotes)
                                {
                                    sbDescription.Append(Footnote(oRelationships[nI].SourceEnd,oSources,cFootnote,sbFootnote,ref cNextChar));
                                }
                            }

                            sbDescription.Append(". ");
                        }
                    }
                }
            }

			// Education
            sbDescription.Append(ShowFacts(40," was educated at ","and",bHtml,bFootnotes,oSources,cFootnote,sbFootnote,ref cNextChar));

			// Occupation
            sbDescription.Append(ShowFacts(20," worked as a ","and",bHtml,bFootnotes,oSources,cFootnote,sbFootnote,ref cNextChar));
		    
			// Interests
            sbDescription.Append(ShowFacts(30," was interested in ","",bHtml,bFootnotes,oSources,cFootnote,sbFootnote,ref cNextChar));
		    
			// Comments
            sbDescription.Append(ShowFacts(100," ","",bHtml,bFootnotes,oSources,cFootnote,sbFootnote,ref cNextChar));

			// Children
            // Don't display children information for people who are known be less than 14 years old.
            int nAge = 15;
            if(!DoD.IsEmpty())
            {
                nAge = DoD.Date.Year - DoB.Date.Year;
            }
            else
            {
                nAge = DateTime.Now.Year - DoB.Date.Year;
            }
            if(nAge > 14)
            {
                int[] Children = GetChildren();
                if(AllChildrenKnown)
                {
                    switch(Children.Length)
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
                    switch(Children.Length)
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
            clsCensusPerson[] oCensuses = m_oDb.CensusForPerson(m_nID);
            string sLastLocation = "";
            foreach(clsCensusPerson oCensus in oCensuses)
            {                
                string sLocation = "";
                if(bHtml)
                {
                    sLocation = m_oDb.PlaceToHtml(oCensus.HouseholdName);
                }
                else
                {
                    sLocation = oCensus.HouseholdName;
                }
                if(sLocation != sLastLocation)
                {
                    sbDescription.Append(ThirdPerson(true) + " lived at ");
                    sbDescription.Append(sLocation);
                    sLastLocation = sLocation;
                }
                else
                {
                    sbDescription.Remove(sbDescription.Length - 2,2);
                    sbDescription.Append(" and");
                }
                sbDescription.Append(" on " + oCensus.Date.ToString("d MMMM yyyy"));
                if(bFootnotes)
                {
                    sbDescription.Append(Footnote(oCensus.HouseholdID,oSources,cFootnote,sbFootnote,ref cNextChar,true));
                }
                sbDescription.Append(". ");
            }

			// Died
			if(!DoD.IsEmpty())
			{
				sbDescription.Append(ThirdPerson(true) + " died " + DoD.Format(DateFormat.FullLong,clsDate.enumPrefix.OnInBeforeAfter));
				if(bFootnotes)
				{
                    sbDescription.Append(Footnote(SourceDoD,oSources,cFootnote,sbFootnote,ref cNextChar));
				}
				oFacts=GetFacts(90);
				if(oFacts.Length>0)
				{
					sbDescription.Append(" in ");
                    if(bHtml)
                    {
                        sbDescription.Append(m_oDb.PlaceToHtml(oFacts[0].Information));
                    }
                    else
                    {
                        sbDescription.Append(oFacts[0].Information);
                    }
					if(bFootnotes)
					{
						sbDescription.Append(Footnote(oFacts[0].Sources,oSources,cFootnote,sbFootnote,ref cNextChar));
					}							
				}
				if(!DoB.IsEmpty())
				{
					sbDescription.Append(" when " + ThirdPerson(false) + " was " + Age(this.DoD) + " old");
				}
				sbDescription.Append(". ");
			}
            if(bHtml && bIncludeToDo)
            {
                sbDescription.AppendLine("</p>");
            }

            // Display the comments
            // bIncludeToDo is not really the "correct" flag
            if(bHtml && bIncludeToDo && m_sComments != string.Empty)
            {
                sbDescription.AppendLine("<p class=\"Small\" style=\"line-height: 100%\"><strong>Private Comments</strong>: " + m_sComments + "</p>");
            }

            // Include the places 
            if(bHtml && bIncludePlaces)
            {
                sbDescription.AppendLine(Places.GoogleMap(600,300));
            }

			// Show the footnotes
			if(bFootnotes)
			{
				if(sbFootnote.Length>0)
				{
                    sbDescription.AppendLine("<table cellpadding=\"3\" cellspacing=\"2\">");
                    sbDescription.AppendLine(sbFootnote.ToString());
                    sbDescription.AppendLine("</table> ");
				}
                sbDescription.AppendLine("<p align=\"left\"><span class=\"Small\">Last Edit by " + LastEditBy + " on " + LastEditDate.ToString("d-MMM-yyyy HH:mm:ss") + "</span></p>");
            }

            // Show all the non primary images
            if(bHtml && bShowImages)
            {
                int[] Media = GetMediaID(true);
                if(Media.Length > 0)
                {
                    sbDescription.AppendLine("<table>");                 
                    foreach(int nMediaID in Media)
                    {
                        clsMedia oMedia = new clsMedia(m_oDb,nMediaID);
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
            if(bHtml && bIncludeToDo)
            {
                clsToDo[] oToDo = this.GetToDo();
                if(oToDo.Length > 0)
                {
                    sbDescription.AppendLine("<p><b>To Do</b></p>");
                    sbDescription.AppendLine("<table cellpadding=\"3\" cellspacing=\"2\">");
                    foreach(clsToDo oDo in oToDo)
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
		private string ShowFacts			(			int nFactTypeID,			string sPrefix,			string sJoinWord,			bool bHtml,			bool bFootnotes,			clsIDName[] oSources,			char[] cFootnote,			StringBuilder sbFootnote,			ref char cNextChar			)
		{
			// Start to build a string to return as the result
			StringBuilder	sbDescription = new StringBuilder();

			// Get the collection of facts and loop through them
			clsFact[] oFacts = this.GetFacts(nFactTypeID);
			bool bFirst = true;
			bool bFullStop = false;			
			for(int nFact=0;nFact<oFacts.Length;nFact++)
			{
				if(bFirst)
				{
					sbDescription.Append(this.ThirdPerson(true)+sPrefix+oFacts[nFact].Information);
					if(nFact==oFacts.Length-1)
					{
						// This is already the last one
						bFullStop = true;
					}
					else if(sJoinWord=="")
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
					if(nFact==oFacts.Length-1)
					{
						// Last fact use the join word
						sbDescription.Append(" "+sJoinWord+" "+oFacts[nFact].Information);
						bFullStop = true;
					}
					else
					{
						// Imtermeadate fact just use a comma
						sbDescription.Append(", "+oFacts[nFact].Information);
					}
				}
				
				// Add a footnote (if required)
				if(bFootnotes)
				{
					sbDescription.Append(Footnote(oFacts[nFact].Sources,oSources,cFootnote,sbFootnote,ref cNextChar));					
				}

				// Add a full stop after the last fact
				if(bFullStop)
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
		private string Footnote			(			clsSources oSources,			clsIDName[] oAllSources,			char[] cFootnote,			StringBuilder sbFootnote,			ref char cNextChar			)
		{
			StringBuilder sbReturn = new StringBuilder();
			int[] nSourceID = oSources.Get();
			for(int nI=0;nI<nSourceID.Length;nI++)
			{
                sbReturn.Append(Footnote(nSourceID[nI],oAllSources,cFootnote,sbFootnote,ref cNextChar,false));
            }

			if(sbReturn.Length==0)
			{
				return "";
			}

            return "<span class=\"superscript\">" + sbReturn.ToString() + "</span> ";
		}

        private string Footnote            (            int nSourceID,            clsIDName[] oAllSources,            char[] cFootnote,            StringBuilder sbFootnote,            ref char cNextChar,            bool bHtml            )
        {
            int nIndex = 0;
            for(nIndex = 0;oAllSources[nIndex].ID != nSourceID;nIndex++) ;
            if(cFootnote[nIndex] == ' ')
            {
                cFootnote[nIndex] = cNextChar;
                cNextChar++;

                sbFootnote.Append("<tr bgcolor=\"silver\"><td><span class=\"Small\">" + cFootnote[nIndex].ToString() + "</span></td><td><a href=\"Source:" + oAllSources[nIndex].ID.ToString() + "\"><span class=\"Small\">" + oAllSources[nIndex].Name + "</span></a></td></tr>");
            }

            if(bHtml)
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
            if(DoB.IsEmpty())
            {
                return DoB.Format(DateFormat.FullLong, "b. ") + BornLocation(true, "\nb. ") + DoD.Format(DateFormat.FullLong, "\nd. ");
            }
            if(DoD.IsEmpty())
            {
                if(DoB.Date.Year > DateTime.Now.Year - 110 && bAge)
                {
                    return DoB.Format(DateFormat.FullLong, "b. ") + BornLocation(true, "\nb. ") + "\nage " + Age(DateTime.Now);
                }
                else
                {
                    return DoB.Format(DateFormat.FullLong, "b. ") + BornLocation(true, "\nb. ");
                }
            }
            return DoB.Format(DateFormat.FullLong, "b. ") + BornLocation(true, "\nb. ") + DoD.Format(DateFormat.FullShort, "\nd. ") + " (" + Age(DoD, false) + ")";
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
            int nYears = dtTheDate.Year - m_DoB.Date.Year;
            int nDayOfYearDiff = dtTheDate.DayOfYear - m_DoB.Date.DayOfYear;

            if(nDayOfYearDiff<-2)
            {
                nYears--;
            }
            else if(nDayOfYearDiff > 2)
            {
            }
            else
            {
                // Need an exact calculation here
                if(dtTheDate.Month <= m_DoB.Date.Month && dtTheDate.Day < m_DoB.Date.Day)
                {
                    nYears--;
                }
            }

            // Return the duration as a string
            if(nYears == 0)
            {
                //if(bUnits)
                //{
                    TimeSpan oAge = dtTheDate - m_DoB.Date;
                    return (oAge.Days+1).ToString() + " days";
                //}
                //else
                //{
                    //return "0";
                //}
            }
            else
            {
                if(bUnits)
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
            if(bCaptialLetter)
            {
                if(m_bMale)
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
                if(m_bMale)
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
            if(bShort)
            {
                sLocation = GetSimpleFact(11);
            }
            else
            {
                sLocation = GetSimpleFact(10);
            }
            if(sLocation == "")
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
                if(oFacts.Length > 0)
                {
                    string sBorn = oFacts[0].Information;
                    clsPlace oPlace = m_oDb.GetPlace(sBorn);
                    if(oPlace != null)
                    {
                        oPlaces.AddPlace(oPlace);
                    }
                }

                // Add the married places
                clsRelationship[] oRelationships = GetRelationships();
                foreach(clsRelationship oRelatonship in oRelationships)
                {
                    if(oRelatonship.IsValid())
                    {
                        clsPlace oMarried = m_oDb.GetPlace(oRelatonship.Location);
                        if(oMarried != null)
                        {
                            oPlaces.AddPlace(oMarried);
                        }
                    }
                }

                // Add the location of born children
                int[] nChildren = GetChildren();
                foreach(int nChild in nChildren)
                {
                    clsPerson oChild = new clsPerson(nChild, m_oDb);
                    clsFact [] oChildFacts  = oChild.GetFacts(10);
                    if(oChildFacts.Length > 0)
                    {
                        string sChildBorn = oChildFacts[0].Information;
                        clsPlace oPlace = m_oDb.GetPlace(sChildBorn);
                        if(oPlace != null)
                        {
                            oPlaces.AddPlace(oPlace);
                        }
                    }
                }

                // Add the census places
                clsCensusPerson[] oCensuses = m_oDb.CensusForPerson(m_nID);
                // string sLastLocation = "";
                foreach(clsCensusPerson oCensus in oCensuses)
                {
                    clsPlace oHousehold = m_oDb.GetPlace(oCensus.HouseholdName);
                    oPlaces.AddPlace(oHousehold);
                }

                // Add the died place
                oFacts = GetFacts(90);
                if(oFacts.Length > 0)
                {
                    string sDied = oFacts[0].Information;
                    clsPlace oPlace = m_oDb.GetPlace(sDied);
                    if(oPlace != null)
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
            if(bExcludePrimary)
            {
                nExcludeID = m_nMediaID;
            }

            ArrayList oMedia = new ArrayList();

            string sSql = "SELECT MediaID FROM tbl_AdditionalMediaForPeople WHERE PersonID=" + m_nID.ToString() + ";";
            OleDbCommand oSql = new OleDbCommand(sSql,m_oDb.cnDB);
            OleDbDataReader drMedia = oSql.ExecuteReader();
            while(drMedia.Read())
            {
                int nMediaID = Innoval.clsDatabase.GetInt(drMedia,"MediaID",0);
                if(nMediaID != 0 && nMediaID != nExcludeID)
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
            foreach(int nMediaID in nMediaIDs)
            {
                clsMedia oMedia = new clsMedia(m_oDb,nMediaID);
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
            if(m_oToDo == null)
            {
                LoadToDo();
            }

            return (clsToDo[])m_oToDo.ToArray(typeof(clsToDo));
        }

        /// <summary>
        /// Loads the collection of ToDo items related this person.
        /// </summary>
        private void LoadToDo()
        {
            // Create a list to hold the ToDo items.
            m_oToDo = new ArrayList();

            string sSql = "SELECT * FROM tbl_ToDo WHERE PersonID=" + this.ID + " ORDER BY Priority, ID;";
            OleDbCommand oSql = new OleDbCommand(sSql,m_oDb.cnDB);
            OleDbDataReader drToDo = oSql.ExecuteReader();
            while(drToDo.Read())
            {
                clsToDo oItem = new clsToDo(Innoval.clsDatabase.GetInt(drToDo,"ID",-1),ID,Innoval.clsDatabase.GetInt(drToDo,"Priority",0),Innoval.clsDatabase.GetString(drToDo,"Description",""));                
                
                // Add this item to the collection.
                m_oToDo.Add(oItem);
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
            if(m_oToDo == null)
            {
                LoadToDo();
            }

            // Add to the list of facts
            m_oToDo.Add(oNew);            

            // Return success
            return true;
        }

        #endregion

        #region General Public Properties

        #region Image

        /// <summary>Index of the media object attached to this person.</summary>
		public int MediaID { get { return m_nMediaID; } set { m_nMediaID = value; } }

		/// <summary>Full filename for an image for the person.  Empty string if no image is specified or can not be found on the hard disk.</summary>
		public string GetImageFilename()
		{
            // Check that a media object is attached to this person
            if(m_nMediaID == 0)
            {
                return "";
            }

            // Find the full filename of the media object
            clsMedia oMedia = new clsMedia(m_oDb,m_nMediaID);
            return oMedia.FullFilename;
		}

		#endregion
		
		/// <summary>The ID of the person in the database.</summary>
		public int ID { get { return m_nID; } set { m_nID = value;} }

		/// <summary>Database that this person is stored in.</summary>
		public clsDatabase Database { get { return m_oDb; } }

		/// <summary>
		/// The surname this person had at birth.
		/// </summary>
		public string BirthSurname
		{
			get
			{
				if(m_bMale)
				{
					return m_sSurname;
				}
				else
				{
					if(m_sMaidenname=="")
					{
						return m_sSurname;
					}
					else
					{
						return m_sMaidenname;
					}
				}
			}
		}
		
		/// <summary>Surname of this person.</summary>
		public string Surname { get { return m_sSurname; } set { m_sSurname = value; } }

		/// <summary>First names of this person.</summary>
		public string Forenames { get { return m_sForenames; } set { m_sForenames = value; } }

		/// <summary>Maiden name of this person.</summary>
		public string Maidenname { get { return m_sMaidenname; } set { m_sMaidenname = value; } }

		/// <summary>Date of birth of this person.</summary>
		public clsDate DoB { get { return m_DoB; } }

		/// <summary>Date of death of this person.</summary>
		public clsDate DoD { get { return m_DoD; } }

        // ID of the father of this person.
        /// <summary>
        /// ID of the father of this person.
        /// Zero is unknown father.
        /// </summary>
		public int FatherID 
        {
            get
            {
                return m_nFatherID;
            }
            set
            {
                m_nFatherID = value;
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
                return m_nMotherID;
            }
            set
            {
                m_nMotherID = value;
            }
        }

        // True if this person is male.  False, otherwise.
        /// <summary>
        /// True if this person is male.  False, otherwise.
        /// </summary>
        public bool Male
        {
            get
            {
                return m_bMale;
            }
            set
            {
                m_bMale = value;
            }
        }

        // True if this person is female.  False, otherwise.
        /// <summary>
        /// True if this person is female.  False, otherwise.
        /// </summary>
        public bool Female
        {
            get
            {
                return !m_bMale;
            }
            set
            {
                m_bMale = !value;
            }
        }

		/// <summary>True if all the children of this person are known.  False, otherwise.</summary>
		public bool AllChildrenKnown { get { return m_bAllChildren; } set { m_bAllChildren = value; } }

        // True if the person should be included in the gedcom file. False, otherwise.
        /// <summary>
        /// True if the person should be included in the gedcom file. False, otherwise.
        /// </summary>
        public bool IncludeInGedcom
        {
            get
            {
                return m_bIncludeGedcom;
            }
            set
            {
                m_bIncludeGedcom = value;
            }
        }

		/// <summary>User comments for this person.</summary>
		public string Comments { get { return m_sComments; } set { m_sComments = value; } }

        // A clsSources object for the person name.
        /// <summary>
        /// A clsSources object for the person name.
        /// </summary>
		public clsSources SourceName
		{
			get
			{
				if(m_sourcesName==null)
				{
					m_sourcesName = new clsSources(m_nID,1,m_oDb);
				}
				return m_sourcesName;
			}
		}

		/// <summary>A clsSources object for the person date of birth.</summary>
		public clsSources SourceDoB
		{
			get
			{
				if(m_sourcesDoB==null)
				{
					m_sourcesDoB = new clsSources(m_nID,2,m_oDb);
				}
				return m_sourcesDoB;
			}
		}

		/// <summary>A clsSources object for the person date of death.</summary>
		public clsSources SourceDoD
		{
			get
			{
				if(m_sourcesDoD==null)
				{
					m_sourcesDoD = new clsSources(m_nID,3,m_oDb);
				}
				return m_sourcesDoD;
			}
		}

		/// <summary>A collection of all the sources used for this person.  Including the non specific sources.</summary>
		public clsSources SourceNonSpecific
		{
			get
			{
				if(m_sourcesNonSpecific==null)
				{
					m_sourcesNonSpecific = new clsSources(m_nID,0,m_oDb);
				}
				return m_sourcesNonSpecific;
			}
		}
		
		/// <summary>Short term information about the person.  This is not saved in the database.</summary>
		public string Tag
		{
			get { return m_sTag; }
			set { m_sTag = value; }
		}

		/// <summary>Name of the user who wrote the last edit.</summary>
        public string LastEditBy { get { return m_sLastEditBy; } set { m_sLastEditBy = value; } }

		/// <summary>Date and time of the last edit.</summary>
		public DateTime LastEditDate { get { return m_dtLastEditDate; } set { m_dtLastEditDate = value; } }

		#endregion
	}
}
