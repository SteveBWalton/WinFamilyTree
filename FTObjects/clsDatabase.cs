using System;

// StringBuilder
using System.Text;	

// Access database ADO.NET
using System.Data;
using System.Data.OleDb;

// ArrayList
using System.Collections;

using System.IO;

namespace FamilyTree.Objects
{
	#region Supporting types, enums, etc...

    // Delegate for functions that refresh the UI.
    /// <summary>
    /// Delegate for functions that refresh the UI.
    /// </summary>
    public delegate void dgtVoid();

    // Sex when searching for people.
    /// <summary>
    /// When searching for people.
    /// Pre-select one sex or not.
    /// </summary>
	public enum enumChooseSex
	{
		/// <summary>Search for male people only.</summary>
		Male,
		/// <summary>Search for female people only.</summary>
		Female,
		/// <summary>Search through all people.  Don't care about the sex.</summary>
		Either
	}

    // The sort order for results of a search.
    /// <summary>
    /// The sort order for results of a search.
    /// </summary>
	public enum enumSortOrder
	{
		/// <summary>Return the data in date order.</summary>
		Date,
		/// <summary>Return the data in alphabetical order.</summary>
		Alphabetical
	}

	#endregion

    // Class to represent a database of family tree information
    /// <summary>
    /// Class to represent a database of family tree information
    /// </summary>
	public class clsDatabase
	{
		#region Member Variables

		/// <summary>Connection to a database.</summary>
		private OleDbConnection m_cnDb;

		/// <summary>List of fact types.</summary>
		private clsFactType[] m_oFactTypes;

		/// <summary>Range of birthdate difference used to search for marriage partners.</summary>
		private int m_nMarriedRange;

        /// <summary>Filename of the database.</summary>
        private string m_sFilename;

		#endregion
		
		#region Constructors Destructors etc...
		
        /// Class Constructor.
		/// <summary>
        /// Class constructor.
		/// Opens the specified database.
		/// Call Dispose() to close the database.
		/// </summary>
		/// <param name="sFilename">Specifies the filename of the family tree access database</param>
        public clsDatabase(string sFilename)
        {
            m_sFilename = sFilename;
            m_oFactTypes = null;
            m_nMarriedRange = 20;

            // Open the connection to the database				 
            m_cnDb = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + sFilename + ";");
            m_cnDb.Open();
        }

		/// <summary>
		/// Class destructor.  
		/// Do not depend on the timing of this function.
		/// .NET calls destructors in it's own sweet time.
		/// </summary>
		~clsDatabase()
		{
			Dispose();
		}

		/// <summary>
		/// Closes the database and releases all handles.
		/// </summary>
		public void Dispose()
		{
			if(m_cnDb!=null)
			{
				m_cnDb.Close();
				m_cnDb = null;
			}
		}

		#endregion
		
		#region People

		/// <summary>
		/// Returns the person object specified by nPersonID
		/// </summary>
		/// <param name="nPersonID">Specifies the ID of the person required</param>
		/// <returns>The person objects co-responding the the ID specified by nPersonID</returns>
		public clsPerson GetPerson
			(
			int nPersonID
			)
		{
			// Open the specified person ID
			clsPerson oPerson = new clsPerson(nPersonID,this);								

			// Return the person object
			return oPerson;
		}

		/// <summary>
		/// Returns a list of people in an array of clsIDName pairs who match the specfied criteria.
		/// </summary>
		/// <param name="nSex">Specify the sex of the required people</param>
		/// <param name="nOrder">Specify the order of the returned array.</param>
		/// <param name="nStartYear">Specify the earliest birth year of the required people</param>
		/// <param name="nEndYear">Specify the latest birth year of the required people.</param>
		/// <returns>An array of IDName pairs representing people who match the specified criteria.</returns>
		public clsIDName[] GetPeople
			(
			enumChooseSex nSex,
			enumSortOrder nOrder,
			int nStartYear,
			int nEndYear
			)
		{
			// Build the SQL command
			string sSql = "SELECT ID,Forenames,Surname,Maidenname,Born,BornStatusID,Died,DiedStatusID FROM tbl_People WHERE ";
			switch(nSex)
			{
			case enumChooseSex.Male:
				sSql = sSql + "Sex='M' AND ";
				break;
			case enumChooseSex.Female:
				sSql = sSql + "Sex='F' AND ";
				break;
			}
			sSql = sSql + "((Year(Born)>="+nStartYear.ToString()+" AND Year(Born)<="+nEndYear.ToString()+") OR Born IS NULL) ";			
			
			switch(nOrder)
			{
			case enumSortOrder.Date:
				sSql = sSql + "ORDER BY Born;";
				break;
			
			case enumSortOrder.Alphabetical:
				sSql = sSql + "ORDER BY Surname,Forenames;";
				break;
			}

			return GetPeople(sSql);
		}
		/// <summary>
		/// Returns a list of people in an array of clsIDName objects who are returned by the specified Sql command.
		/// </summary>
		/// <param name="sSql">Specifies an Sql command that will return a list of people.</param>
		/// <returns>An array of clsIDName objects representing the people in the specified Sql command.</returns>
		public clsIDName[] GetPeople
			(
			string sSql
			)
		{
			// Initialise the list of people to return
			ArrayList oItems = new ArrayList();

			// Open a list of people
			OleDbCommand oSQL = new OleDbCommand(sSql,m_cnDb);
			OleDbDataReader drPeople = oSQL.ExecuteReader();
			while(drPeople.Read())
			{
				StringBuilder sbName = new StringBuilder();
				if(!drPeople.IsDBNull(1))
				{
					sbName.Append(drPeople.GetString(1));
				}
				if(!drPeople.IsDBNull(2))
				{
					sbName.Append(" ");
					sbName.Append(drPeople.GetString(2));
				}
				if(!drPeople.IsDBNull(3))
				{
					sbName.Append(" nee ");
					sbName.Append(drPeople.GetString(3));
				}
				sbName.Append(" (");
				if(!drPeople.IsDBNull(4))
				{					
					int nBornYear = clsDate.GetYear(drPeople.GetDateTime(4).Year);
					if(nBornYear<0)
					{
						sbName.Append((-nBornYear).ToString());
						sbName.Append(" BC");
					}
					else
					{
						sbName.Append(nBornYear.ToString());
					}
				}
				if(!drPeople.IsDBNull(6))
				{
					sbName.Append("-");
					int nDiedYear = clsDate.GetYear(drPeople.GetDateTime(6).Year);
					if(nDiedYear<0)
					{
						sbName.Append((-nDiedYear).ToString());
						sbName.Append(" BC");
					}
					else
					{
						sbName.Append(nDiedYear.ToString());
					}					
				}
				sbName.Append(")");
				clsIDName oItem = new clsIDName(drPeople.GetInt32(0),sbName.ToString());
				oItems.Add(oItem);
			}
			drPeople.Close();

			// Return success
            return (clsIDName[])oItems.ToArray(typeof(clsIDName));
		}
		/// <summary>
		/// Returns a list of all the people in an array of clsIDName objects.
		/// </summary>
		/// <returns>An array of clsIDName objects representing all the people.</returns>
		public clsIDName[] GetPeople()
		{
			return GetPeople("SELECT ID,Forenames,Surname,Maidenname,Born,BornStatusID,Died,DiedStatusID FROM tbl_People ORDER BY ID;");
		}
		/// <summary>
		/// Returns a list of the people of the specified sex who are alive in the specified year.
		/// </summary>
        /// <param name="nSex">Specifies the sex of the required people.</param>
        /// <param name="nOrder">Specifies the order of the array to return.</param>
        /// <param name="nYearAlive">Specifies the year when the returned people must have been alive.</param>
		/// <returns>An array of clsIDName objects representing the people alive at the specified year.</returns>
		public clsIDName[] GetPeople
			(
			enumChooseSex nSex,
			enumSortOrder nOrder,
			int nYearAlive
			)
		{
            // Build the SQL command
            string sSql = "SELECT ID,Forenames,Surname,Maidenname,Born,BornStatusID,Died,DiedStatusID FROM tbl_People WHERE ";
            switch(nSex)
            {
            case enumChooseSex.Male:
                sSql = sSql + "Sex='M' AND ";
                break;
            case enumChooseSex.Female:
                sSql = sSql + "Sex='F' AND ";
                break;
            }
            sSql = sSql + "Year(Born)<=" + nYearAlive.ToString() + " AND Iif(IsNull(Died),Year(Born)+120,Year(Died))>=" + nYearAlive.ToString() + " ";

            switch(nOrder)
            {
            case enumSortOrder.Date:
                sSql = sSql + "ORDER BY Born;";
                break;

            case enumSortOrder.Alphabetical:
                sSql = sSql + "ORDER BY Surname,Forenames;";
                break;
            }

            return GetPeople(sSql);
        }
        /// <summary>
        /// Returns a list of people of the specified sex in the specified order.
        /// </summary>
        /// <param name="nSex">Specifies the sex of the required people.</param>
        /// <param name="nOrder">Specifies the order of the array to return.</param>
        /// <returns>An array of clsIDName objects representing the specified people in the specified order.</returns>
        public clsIDName[] GetPeople
            (
            enumChooseSex nSex,
            enumSortOrder nOrder
            )
        {
            // Build the SQL command
            string sSql = "SELECT ID,Forenames,Surname,Maidenname,Born,BornStatusID,Died,DiedStatusID FROM tbl_People ";
            switch(nSex)
            {
            case enumChooseSex.Male:
                sSql = sSql + "WHERE Sex='M' ";
                break;
            case enumChooseSex.Female:
                sSql = sSql + "WHERE Sex='F' ";
                break;
            }

            switch(nOrder)
            {
            case enumSortOrder.Date:
                sSql = sSql + "ORDER BY Born;";
                break;

            case enumSortOrder.Alphabetical:
                sSql = sSql + "ORDER BY Surname,Forenames;";
                break;
            }

            // Return the collection of people
            return GetPeople(sSql);
        }
		#endregion

		#region Relationships

		/// <summary>
		/// Returns the relationship between the 2 specified people.  Returns null if there is no relationship.
		/// This is a slightly strange relationship object because it has no owner.
		/// </summary>
		/// <param name="nMaleID">Specifies the male person in the relationship.</param>
		/// <param name="nFemaleID">Specifies the female person in the relationship.</param>
		/// <returns>The relationship object for the 2 people.  NULL if no relationshp exists between the 2 specified people.</returns>
		public clsRelationship GetRelationship
			(
			int nMaleID,
			int nFemaleID
			)
		{
			OleDbCommand oSQL = new OleDbCommand("SELECT ID,TerminatedID,TheDate,StartStatusID,TerminateDate,TerminateStatusID,Location,Comments,RelationshipID,LastEditBy,LastEditDate FROM tbl_Relationships WHERE MaleID=" + nMaleID.ToString() + " AND FemaleID=" + nFemaleID.ToString() + " ORDER BY TheDate DESC;",cnDB);
			OleDbDataReader drRelationship = oSQL.ExecuteReader();
			clsRelationship oRelationship = null;
			if(drRelationship.Read())
			{
				oRelationship = new clsRelationship(drRelationship.GetInt32(0));
				oRelationship.MaleID = nMaleID;
				oRelationship.FemaleID = nFemaleID;
				oRelationship.TerminatedID = drRelationship.GetInt32(1);
				if(drRelationship.IsDBNull(2))
				{					
					oRelationship.Start.Status = clsDate.EMPTY;
				}
				else
				{
					oRelationship.Start.Date = drRelationship.GetDateTime(2);
					oRelationship.Start.Status = drRelationship.GetInt16(3);
				}
				if(drRelationship.IsDBNull(4))
				{
					oRelationship.End.Status = clsDate.EMPTY;
				}
				else
				{
					oRelationship.End.Date = drRelationship.GetDateTime(4);
					oRelationship.End.Status = drRelationship.GetInt16(5);
				}
				if(drRelationship.IsDBNull(6))
				{
					oRelationship.Location = "";
				}
				else
				{
					oRelationship.Location = drRelationship.GetString(6);
				}
				if(drRelationship.IsDBNull(7))
				{
					oRelationship.Comments = "";
				}
				else
				{
					oRelationship.Comments = drRelationship.GetString(7);
				}
				oRelationship.TypeID = drRelationship.GetInt16(8);
				
				if(drRelationship.IsDBNull(9))
				{
					oRelationship.LastEditBy = "Steve Walton";
				}
				else
				{
					oRelationship.LastEditBy = drRelationship.GetString(9);
				}
				if(drRelationship.IsDBNull(10))
				{
					oRelationship.LastEditDate = DateTime.Now;
				}
				else
				{
					oRelationship.LastEditDate = drRelationship.GetDateTime(10);
				}
			}
			drRelationship.Close();

			// Return the relationship
			return oRelationship;
		}

		#endregion

		#region Sources

        // Returns all the repositories as an array of IDName pairs.
        /// <summary>
        /// Returns all the repositories as an array of IDName pairs.
        /// This can be used directly in combo boxes etc ...
		/// </summary>
		/// <returns>An array of clsIDName objects.</returns>
		public clsIDName[] GetRepositories()
		{
			// Create a list of repositories
			ArrayList oRepositories = new ArrayList();

			// Open a list of repositories
			OleDbCommand oSql = new OleDbCommand("SELECT ID,Name FROM tbl_Repositories ORDER BY ID;",m_cnDb);
			OleDbDataReader drRepositories = oSql.ExecuteReader();
			while(drRepositories.Read())
			{
				clsIDName oRepository = new clsIDName(drRepositories.GetInt32(0),drRepositories.GetString(1));
				oRepositories.Add(oRepository);
			}
			drRepositories.Close();
			
			// Get the list of fact types
			return (clsIDName[])(oRepositories.ToArray(typeof(clsIDName)));
		}

        // Returns all the sources as an array of IDName pairs.
        /// <summary>
        /// Returns all the sources as an array of IDName pairs.
        /// This can be used directly in comboboxes etc
		/// </summary>
		/// <returns>An array of clsIDName objects</returns>
        public clsIDName[] GetSources(enumSortOrder nOrder)
        {
            // Build a list of relivant facts
            ArrayList oSources = new ArrayList();

            // Open the list of fact types
            OleDbCommand oSQL;
            if(nOrder == enumSortOrder.Date)
            {
                oSQL = new OleDbCommand("SELECT ID,Name,TheDate,TheDateStatusID FROM tbl_Sources ORDER BY LastUsed DESC;", m_cnDb);
            }
            else
            {
                oSQL = new OleDbCommand("SELECT ID,Name,TheDate,TheDateStatusID FROM tbl_Sources ORDER BY Name;", m_cnDb);
            }
            OleDbDataReader drSources = oSQL.ExecuteReader();
            while(drSources.Read())
            {
                string sName;
                if(drSources.IsDBNull(2))
                {
                    sName = drSources.GetString(1);
                }
                else
                {
                    sName = drSources.GetString(1) + " (" + drSources.GetDateTime(2).Year.ToString() + ")";
                }
                clsIDName oSource = new clsIDName(drSources.GetInt32(0), sName);
                oSources.Add(oSource);
            }
            drSources.Close();

            // Get the list of fact types
            return (clsIDName[])(oSources.ToArray(typeof(clsIDName)));
        }

        // Writes a list of references to additional media objects (gedcom OBJE @M1@) attached to the specified person into the specified Gedcom file.
        /// <summary>
        /// Writes a list of references to additional media objects (gedcom OBJE @M1@) attached to the specified person into the specified Gedcom file.
        /// The primary media is not written again.  It is assumed to be already written into the Gedcom file.
		/// </summary>
		/// <param name="oFile">Specifies the gedcom file to write the media references into. </param>
		/// <param name="nPersonID">Specifies the ID of person to write the media references for.</param>
        /// <param name="nPrimaryID">Specifies the ID of the primary media reference for this person.</param>
		/// <returns>True for success, false otherwise.</returns>
        public bool GedcomWritePersonMedia(StreamWriter oFile, int nPersonID, int nPrimaryID)
        {
            // Write the primary media first
            if(nPrimaryID != 0)
            {
                oFile.WriteLine("1 OBJE @M" + nPrimaryID.ToString("0000") + "@");
            }

            // Now write any additional media objects do not repeat the primary media
            string sSql = "SELECT MediaID FROM tbl_AdditionalMediaForPeople WHERE PersonID=" + nPersonID.ToString() + ";";
            OleDbCommand oSql = new OleDbCommand(sSql, m_cnDb);
            OleDbDataReader drMedia = oSql.ExecuteReader();
            while(drMedia.Read())
            {
                int nMediaID = GetInt(drMedia, "MediaID", 0);
                if(nMediaID != 0 && nMediaID != nPrimaryID)
                {
                    oFile.WriteLine("1 OBJE @M" + nMediaID.ToString("0000") + "@");
                }
            }
            drMedia.Close();

            // Return success
            return true;
        }

        // Writes all the media objects (OBJE) into the Gedcom file.
        /// <summary>
        /// Writes all the media objects (OBJE) into the Gedcom file.
        /// </summary>
		/// <remarks>
		/// This should not be in this section move somewhere else.
		/// </remarks>
		/// <param name="oFile"></param>
		/// <returns></returns>
        public bool GedcomWriteMedia(StreamWriter oFile)
        {
            // Select all the media objects.
            OleDbCommand oSql = new OleDbCommand("SELECT * FROM tbl_Media ORDER BY ID;", m_cnDb);
            OleDbDataReader drMedia = oSql.ExecuteReader();
            while(drMedia.Read())
            {
                int nID = clsDatabase.GetInt(drMedia, "ID", 0);
                oFile.WriteLine("0 @M" + nID.ToString("0000") + "@ OBJE");
                string sFilename = clsDatabase.GetString(drMedia, "Filename", "Undefined");
                oFile.WriteLine("1 FILE media/" + sFilename);
                oFile.WriteLine("2 FORM");
                oFile.WriteLine("3 TYPE photo");
                string sTitle = clsDatabase.GetString(drMedia, "Title", "Undefined");
                oFile.WriteLine("2 TITL " + sTitle);
                bool bPrimary = drMedia.GetBoolean(drMedia.GetOrdinal("Primary"));
                if(bPrimary)
                {
                    oFile.WriteLine("1 _PRIM Y");
                }
                else
                {
                    oFile.WriteLine("1 _PRIM N");
                }
                bool bThumbnail = drMedia.GetBoolean(drMedia.GetOrdinal("Thumbnail"));
                if(bThumbnail)
                {
                    oFile.WriteLine("1 _THUM Y");
                }
                else
                {
                    oFile.WriteLine("1 _THUM N");
                }
            }
            drMedia.Close();

            // Return success
            return true;
        }

        // Write a Gedcom repository record (@R1@ REPO) record for all the repositories in this database.
        /// <summary>
        /// Write a Gedcom repository record (@R1@ REPO) record for all the repositories in this database.
        /// </summary>
		/// <param name="oFile">Specifies the file to write the record into.</param>
		/// <returns>True for success, false otherwise.</returns>
        public bool WriteRepositoriesGedcom(StreamWriter oFile)
        {
            // Select all the sources
            OleDbCommand oSql = new OleDbCommand("SELECT * FROM tbl_Repositories WHERE ID>0 ORDER BY ID;", m_cnDb);
            OleDbDataReader drRepositories = oSql.ExecuteReader();
            while(drRepositories.Read())
            {
                int nID = GetInt(drRepositories, "ID", 0);
                oFile.WriteLine("0 @R" + nID.ToString("0000") + "@ REPO");
                string sName = GetString(drRepositories, "Name", "");
                oFile.WriteLine("1 NAME " + sName);
                string sAddress = GetString(drRepositories, "Address", "");
                if(sAddress != "")
                {
                    GedcomMultiLine(oFile, 1, "ADDR", sAddress);
                }
                string sWebURL = GetString(drRepositories, "WebURL", "");
                if(sWebURL != "")
                {
                    oFile.WriteLine("1 WWW " + sWebURL);
                }
            }
            drRepositories.Close();

            // Return success
            return true;
        }

        // Write a Gedcom source (@S1@ SOUR) record for all the sources in this database.
        /// <summary>
        /// Write a Gedcom source (@S1@ SOUR) record for all the sources in this database.
        /// </summary>
		/// <param name="oFile">Specifies the file to write the record into.</param>
		/// <returns>True for success, false otherwise.</returns>
        public bool WriteSourcesGedcom(StreamWriter oFile, dgtVoid lpfnProgressBar, clsGedcomOptions oOptions)
        {
            // Select all the sources
            OleDbCommand oSql = new OleDbCommand("SELECT ID,Name,TheDate,TheDateStatusID,Comments,AdditionalInfoTypeID,Gedcom,RepositoryID FROM tbl_Sources ORDER BY ID;", m_cnDb);
            OleDbDataReader drSources = oSql.ExecuteReader();
            while(drSources.Read())
            {
                if(GetBool(drSources, "Gedcom", true))
                {
                    int nID = drSources.GetInt32(0);
                    oFile.WriteLine("0 @S" + nID.ToString("0000") + "@ SOUR");
                    string sName = drSources.GetString(1);
                    oFile.WriteLine("1 TITL " + sName);
                    if(!drSources.IsDBNull(2))
                    {
                        clsDate oDate = new clsDate();
                        oDate.Date = drSources.GetDateTime(2);
                        oDate.Status = drSources.GetInt32(3);
                        oFile.WriteLine("2 DATE " + oDate.Format(DateFormat.Gedcom));
                    }

                    // Additional Information for the source
                    if(!drSources.IsDBNull(5))
                    {
                        switch(drSources.GetInt32(5))
                        {
                        case 1: // Birth Certifcate
                            SourceBirthCertificate(oFile, nID);
                            break;
                        case 2: // Marriage Certifcate
                            SourceMarriageCertificate(oFile, nID,oOptions);
                            break;
                        case 3: // Death Certificate
                            SourceDeathCertificate(oFile, nID);
                            break;
                        case 4: // Census Information
                            SourceCensusInfo(oFile, nID, oOptions);
                            break;
                        }
                    }

                    // The note for the source.
                    if(!drSources.IsDBNull(4))
                    {
                        GedcomMultiLine(oFile, 1, "NOTE", drSources.GetString(4));
                    }

                    // The repository for this source
                    int nRepositoryID = clsDatabase.GetInt(drSources, "RepositoryID", 0);
                    if(nRepositoryID > 0)
                    {
                        oFile.WriteLine("1 REPO @R" + nRepositoryID.ToString("0000") + "@");
                    }
                }

                if(lpfnProgressBar != null)
                {
                    lpfnProgressBar();
                }
            }
            drSources.Close();

            // Return success
            return true;
        }

        // Write the additional birth certificate information for a source.
        /// <summary>
        /// Write the additional birth certificate information for a source.
        /// </summary>
		/// <param name="oFile">Specifies the file to write the information into.</param>
		/// <param name="nID">Specifies the ID of the birth certificate (and the parent source record).</param>
        private void SourceBirthCertificate(StreamWriter oFile, int nID)
        {
            // Connect to the database again (to open a second datareader)
            OleDbConnection cnDb = new OleDbConnection(m_cnDb.ConnectionString);
            cnDb.Open();

            // Create a birth certificate object
            clsBirthCertificate oBirth = new clsBirthCertificate(nID, cnDb);

            // Close the database
            cnDb.Close();

            // Write the details from the birth certificate
            bool bFirst = true;
            if(oBirth.RegistrationDistrict != "")
            {
                // oFile.WriteLine("2 PLAC "+oBirth.RegistrationDistrict);
                GedcomLongNote(ref bFirst, oFile, "Registration District: " + oBirth.RegistrationDistrict);
            }
            if(oBirth.WhenAndWhere != "")
            {
                GedcomLongNote(ref bFirst, oFile, "When and Where: " + oBirth.When.ToString("d MMM yyyy") + oBirth.WhenAndWhere);
            }
            if(oBirth.Name != "")
            {
                GedcomLongNote(ref bFirst, oFile, "Name: " + oBirth.Name + " (" + oBirth.Sex + ")");
            }
            if(oBirth.Mother != "")
            {
                GedcomLongNote(ref bFirst, oFile, "Mother: " + oBirth.Mother);
            }
            if(oBirth.Father != "")
            {
                GedcomLongNote(ref bFirst, oFile, "Father: " + oBirth.Father + " (" + oBirth.FatherOccupation + ")");
            }
            if(oBirth.Informant != "")
            {
                GedcomLongNote(ref bFirst, oFile, "Informant: " + oBirth.Informant);
            }
            if(oBirth.WhenRegistered != "")
            {
                GedcomLongNote(ref bFirst, oFile, "When Registered: " + oBirth.WhenRegistered);
            }
        }

        // Write the additional marriage certificate information for a source.
        /// <summary>
        /// Write the additional marriage certificate information for a source.
        /// </summary>
		/// <param name="oFile">Specifies the file to write the information into.</param>
		/// <param name="nSourceID">Specifies the ID of the marriage certificate (and the parent source record).</param>
        private void SourceMarriageCertificate(StreamWriter oFile, int nSourceID, clsGedcomOptions oOptions)
        {
            // Connect to the database again (to open a second datareader)
            OleDbConnection cnDb = new OleDbConnection(m_cnDb.ConnectionString);
            cnDb.Open();

            // Create a Marriage Certificate object
            clsMarriageCertificate oMarriage = new clsMarriageCertificate(nSourceID, cnDb);

            // Close the database
            cnDb.Close();

            // Write the details of the marriage certificate
            StringBuilder sbText;
            WriteGedcomPlace(oFile, 2, oMarriage.Location, oOptions);
            bool bFirst = true;
            if(oMarriage.GroomName != "")
            {
                sbText = new StringBuilder();
                sbText.Append(oMarriage.GroomName);
                if(oMarriage.GroomAge != "")
                {
                    sbText.Append(" (" + oMarriage.GroomAge + ")");
                }
                if(oMarriage.GroomOccupation != "")
                {
                    sbText.Append(" - " + oMarriage.GroomOccupation);
                }
                if(oMarriage.GroomLiving != "")
                {
                    sbText.Append(" - " + oMarriage.GroomLiving);
                }
                GedcomLongNote(ref bFirst, oFile, "Groom: " + sbText.ToString());
            }
            if(oMarriage.BrideName != "")
            {
                sbText = new StringBuilder();
                sbText.Append(oMarriage.BrideName);
                if(oMarriage.BrideAge != "")
                {
                    sbText.Append(" (" + oMarriage.BrideAge + ")");
                }
                if(oMarriage.BrideOccupation != "")
                {
                    sbText.Append(" - " + oMarriage.BrideOccupation);
                }
                if(oMarriage.BrideLiving != "")
                {
                    sbText.Append(" - " + oMarriage.BrideLiving);
                }
                GedcomLongNote(ref bFirst, oFile, "Bride: " + sbText.ToString());
            }
            if(oMarriage.GroomFather != "")
            {
                sbText = new StringBuilder(oMarriage.GroomFather);
                if(oMarriage.GroomFatherOccupation != "")
                {
                    sbText.Append(" - " + oMarriage.GroomFatherOccupation);
                }
                GedcomLongNote(ref bFirst, oFile, "Groom's Father: " + sbText.ToString());
            }
            if(oMarriage.BrideFather != "")
            {
                sbText = new StringBuilder(oMarriage.BrideFather);
                if(oMarriage.BrideFatherOccupation != "")
                {
                    sbText.Append(" - " + oMarriage.BrideFatherOccupation);
                }
                GedcomLongNote(ref bFirst, oFile, "Bride's Father: " + sbText.ToString());
            }
            if(oMarriage.Witness != "")
            {
                GedcomLongNote(ref bFirst, oFile, "Witness: " + oMarriage.Witness);
            }
        }

        // Write the additional death certificate information for a source.
        /// <summary>
        /// Write the additional death certificate information for a source.
        /// </summary>
		/// <param name="oFile">Specifies the file to write the information into.</param>
		/// <param name="nID">Specifies the ID of the marriage certificate (and the parent source record).</param>
        private void SourceDeathCertificate(StreamWriter oFile, int nID)
        {
            // Connect to the database again (to open a second datareader)
            OleDbConnection cnDb = new OleDbConnection(m_cnDb.ConnectionString);
            cnDb.Open();

            // Create a Marriage Certificate object
            clsDeathCertificate oDeath = new clsDeathCertificate(nID, cnDb);

            // Close the database
            cnDb.Close();

            // Write the details of the marriage certificate
            bool bFirst = true;
            if(oDeath.RegistrationDistrict != "")
            {
                // oFile.WriteLine("2 PLAC "+oDeath.RegistrationDistrict);
                GedcomLongNote(ref bFirst, oFile, "Registration District: " + oDeath.RegistrationDistrict);
            }
            if(oDeath.When != "")
            {
                GedcomLongNote(ref bFirst, oFile, "When: " + oDeath.When);
            }
            if(oDeath.Place != "")
            {
                GedcomLongNote(ref bFirst, oFile, "Where: " + oDeath.Place);
            }
            if(oDeath.Name != "")
            {
                GedcomLongNote(ref bFirst, oFile, "Name: " + oDeath.Name + " (" + oDeath.Sex + ")");
            }
            if(oDeath.DatePlaceOfBirth != "")
            {
                GedcomLongNote(ref bFirst, oFile, "Date & Place of Birth: " + oDeath.DatePlaceOfBirth);
            }
            if(oDeath.Occupation != "")
            {
                GedcomLongNote(ref bFirst, oFile, "Occupation: " + oDeath.Occupation);
            }
            if(oDeath.UsualAddress != "")
            {
                GedcomLongNote(ref bFirst, oFile, "Usual Address: " + oDeath.UsualAddress);
            }
            if(oDeath.CauseOfDeath != "")
            {
                GedcomLongNote(ref bFirst, oFile, "Cause of Death: " + oDeath.CauseOfDeath);
            }
            if(oDeath.Informant != "")
            {
                if(oDeath.InformantDescription == "")
                {
                    GedcomLongNote(ref bFirst, oFile, "Informant: " + oDeath.Informant);
                }
                else
                {
                    GedcomLongNote(ref bFirst, oFile, "Informant: " + oDeath.Informant + " (" + oDeath.InformantDescription + ")");
                }
            }
            if(oDeath.InformantAddress != "")
            {
                GedcomLongNote(ref bFirst, oFile, "Informant Address: " + oDeath.InformantAddress);
            }
            if(oDeath.WhenRegistered != "")
            {
                GedcomLongNote(ref bFirst, oFile, "When Registered: " + oDeath.WhenRegistered);
            }
        }

        // Write the additional census information for the source.
        /// <summary>
        /// Write the additional census information for the source.
        /// </summary>
		/// <param name="oFile">Specifies the Gedcom file to write the additional information into.</param>
		/// <param name="nCensusHouseholdID">Specifies the additional census information to use.</param>
        private void SourceCensusInfo(StreamWriter oFile, int nCensusHouseholdID, clsGedcomOptions oOptions)
        {
            // Connect to the database again (to open a second datareader)
            OleDbConnection cnDb = new OleDbConnection(m_cnDb.ConnectionString);
            cnDb.Open();

            // Write the information from the census header
            string sSql = "SELECT Address FROM tbl_CensusHouseholds WHERE ID=" + nCensusHouseholdID.ToString() + ";";
            OleDbCommand oSql = new OleDbCommand(sSql, cnDb);
            object oAddress = oSql.ExecuteScalar();

            // Check that an address is specified.  If the address is not present then the record probably does not exist.
            if(oAddress != null)
            {
                string sAddress = oAddress.ToString();
                WriteGedcomPlace(oFile, 2, sAddress,oOptions);

                // Write the information about the members of this census record
                sSql = "SELECT NameGiven, Age, RelationToHead, Occupation, BornLocation FROM tbl_CensusPeople WHERE HouseHoldID=" + nCensusHouseholdID.ToString() + " ORDER BY ID;";
                oSql = new OleDbCommand(sSql, cnDb);
                OleDbDataReader drMembers = oSql.ExecuteReader();
                bool bFirst = true;
                while(drMembers.Read())
                {
                    string sName = GetString(drMembers, "NameGiven", "");
                    string sAge = GetString(drMembers, "Age", "");
                    string sRelation = GetString(drMembers, "RelationToHead", "");
                    string sOccupation = GetString(drMembers, "Occupation", "");
                    string sBorn = GetString(drMembers, "BornLocation", "");

                    StringBuilder sbMember = new StringBuilder();
                    sbMember.Append(sName);
                    if(sAge != "")
                    {
                        sbMember.Append(" (" + sAge + ")");
                    }
                    if(sRelation != "")
                    {
                        sbMember.Append(" - " + sRelation);
                    }
                    if(sOccupation != "")
                    {
                        sbMember.Append(" - " + sOccupation);
                    }
                    if(sBorn != "")
                    {
                        sbMember.Append(" - " + sBorn);
                    }

                    // I would prefer a better tag than NOTE but this works for now.
                    GedcomLongNote(ref bFirst, oFile, sbMember.ToString());
                    // oFile.WriteLine("2 NOTE "+sbMember.ToString());
                }
                drMembers.Close();
            }

            // Close the second connection to the database
            cnDb.Close();
        }

        // Write a Gedcom with line breaks over multilines using the CONT tag
        /// <summary>
        /// Write a Gedcom with line breaks over multilines using the CONT tag
        /// </summary>
		/// <param name="oFile">Specifies the Gedcom file to write to.</param>
		/// <param name="nLevel">Specifies the level of the tag.</param>
		/// <param name="sTag">Specifies the name of the tag.</param>
		/// <param name="sMessage">Specifies the message to write into the tag.</param>
        private void GedcomMultiLine(StreamWriter oFile, int nLevel, string sTag, string sMessage)
        {
            // Deal with multiple lines recurisively
            int nLineBreak = sMessage.IndexOf("\n");
            if(nLineBreak > 0)
            {
                if(sTag == "CONT")
                {
                    GedcomMultiLine(oFile, nLevel, "CONT", sMessage.Substring(0, nLineBreak - 1));
                    GedcomMultiLine(oFile, nLevel, "CONT", sMessage.Substring(nLineBreak + 1));
                }
                else
                {
                    GedcomMultiLine(oFile, nLevel, sTag, sMessage.Substring(0, nLineBreak - 1));
                    GedcomMultiLine(oFile, nLevel + 1, "CONT", sMessage.Substring(nLineBreak + 1));
                }
            }
            else
            {
                oFile.Write(nLevel.ToString());
                oFile.Write(" ");
                oFile.Write(sTag);
                oFile.Write(" ");
                oFile.WriteLine(sMessage);
            }
        }

        // Write a series of lines into a single note.
        /// <summary>
        /// Write a series of lines into a single note.
        /// The first line is tagged 1 NOTE, subsequent lines are tagged 2 CONT.
		/// </summary>
		/// <param name="bFirst">True for the first line and then reset.</param>
		/// <param name="oFile">Specifies the gedcom file to write the note into.</param>
		/// <param name="sMessage">Specifies the line of text for the gedcom file.</param>
        private void GedcomLongNote(ref bool bFirst, StreamWriter oFile, string sMessage)
        {
            // Deal with multiple lines recurisively
            int nLineBreak = sMessage.IndexOf("\n");
            if(nLineBreak > 0)
            {
                GedcomLongNote(ref bFirst, oFile, sMessage.Substring(0, nLineBreak - 1));
                GedcomLongNote(ref bFirst, oFile, sMessage.Substring(nLineBreak + 1));
            }
            else
            {
                if(bFirst)
                {
                    oFile.Write("1 NOTE ");
                    bFirst = false;
                }
                else
                {
                    oFile.Write("2 CONT ");
                }
                oFile.WriteLine(sMessage);
            }
        }

        // Returns an array of clsIDName objects that represent the available additional information types for sources.
        /// <summary>
        /// Returns an array of clsIDName objects that represent the available additional information types for sources.
        /// This is intended to populate a combo box.
		/// </summary>
		/// <returns>An array of clsIDName objects that represent the available additonal information types.</returns>
		public clsIDName [] GetSourceAdditionalTypes()
		{
			// Build a list of additional information types
			ArrayList oTypes = new ArrayList();

			// Open the list from the database
			OleDbCommand oSql = new OleDbCommand("SELECT ID,Name FROM tlk_AdditionalInfoTypes ORDER BY ID;",m_cnDb);
			OleDbDataReader drTypes = oSql.ExecuteReader();
			while(drTypes.Read())
			{

				clsIDName oType = new clsIDName(drTypes.GetInt32(0),drTypes.GetString(1));
				oTypes.Add(oType);
			}
			drTypes.Close();
			
			// Get the list of fact types
			return (clsIDName[])(oTypes.ToArray(typeof(clsIDName)));
		}

		#endregion

		#region Fact Types
		
		/// <summary>
		/// Returns an array of all the fact types.
		/// </summary>
		/// <returns>An array of fact types.</returns>
		public clsFactType[] GetFactTypes()
		{
			// Open the fact types (if required)
			if(m_oFactTypes==null)
			{
				LoadFactTypes();
			}

			// Return the array of fact types
			return m_oFactTypes;
		}

		/// <summary>
		/// Returns a clsFactType object with the specified ID or null if no matching object can be found.
		/// </summary>
		/// <param name="nID">Specifies the ID of the fact type object required.</param>
		/// <returns>A clsFactType object or null.</returns>
		public clsFactType GetFactType
			(
			int	 nID
			)
		{
			// Open the fact types (if required)
			if(m_oFactTypes==null)
			{
				LoadFactTypes();
			}

			// Return a matching fact type (if possible)
			for(int nI=0;nI<m_oFactTypes.Length;nI++)
			{
				if(m_oFactTypes[nI].ID==nID)
				{
					return m_oFactTypes[nI];
				}
			}

			// Return failure
			return null;
		}

		/// <summary>
		/// Loads the fact types from the database.
		/// </summary>
		/// <returns>True for success, false for failure.</returns>
		private bool LoadFactTypes()
		{
			// Build a list of relivant facts
			ArrayList oFactTypes = new ArrayList();
			
			// Open the list of fact types
			OleDbCommand oSQL = new OleDbCommand("SELECT ID,Name FROM tlk_FactTypes ORDER BY ID;",m_cnDb);
			OleDbDataReader drFactTypes = oSQL.ExecuteReader();
			while(drFactTypes.Read())
			{	
				clsFactType oFactType = new clsFactType(drFactTypes.GetInt32(0),drFactTypes.GetString(1));
				oFactTypes.Add(oFactType);
			}
			drFactTypes.Close();
			
			// Get the list of fact types
			m_oFactTypes = (clsFactType[])(oFactTypes.ToArray(typeof(clsFactType)));

			// Return success
			return true;
		}

		#endregion

		#region Census

        // Return all the households in the specified year.
        /// <summary>
        /// Return all the households in the specified year.
        /// This is intended to populate combo box etc...
		/// </summary>
		/// <param name="nYear">Specify the year to return the households of.</param>
		/// <returns>An array of ID and names of house records.</returns>
        public clsIDName[] CenusGetHouseholds(int nYear)
        {
            // Build a list of relivant facts
            ArrayList oHouseholds = new ArrayList();

            string sSql = "SELECT ID, Address FROM tbl_CensusHouseholds WHERE Year(CensusDate)=" + nYear.ToString() + " ORDER BY Address;";
            OleDbCommand oSql = new OleDbCommand(sSql, m_cnDb);
            OleDbDataReader drHouseholds = oSql.ExecuteReader();
            while(drHouseholds.Read())
            {
                clsIDName oHousehold = new clsIDName(drHouseholds.GetInt32(0), drHouseholds.GetString(1));
                oHouseholds.Add(oHousehold);
            }
            drHouseholds.Close();

            // Return the households found
            return (clsIDName[])oHouseholds.ToArray(typeof(clsIDName));
        }

        // Returns an array clsCensusPerson objects representing the member of the specified census household.
        /// <summary>
        /// Returns an array clsCensusPerson objects representing the member of the specified census household.
        /// </summary>
		/// <param name="nHouseholdID">Specifies the ID of the census household.</param>
		/// <returns>An array of clsCensusPerson objects representing the members of the specified census household.</returns>
        public clsCensusPerson[] CensusHouseholdMembers(int nHouseholdID)
        {
            string sSql = "SELECT tbl_CensusPeople.*, tbl_People.Forenames+' '+ Iif(IsNull(tbl_People.MaidenName),tbl_People.Surname, tbl_People.MaidenName) AS Name, tbl_CensusHouseholds.Address, tbl_CensusHouseholds.CensusDate " +
                "FROM (tbl_CensusPeople LEFT JOIN tbl_People ON tbl_CensusPeople.PersonID = tbl_People.ID) INNER JOIN tbl_CensusHouseholds ON tbl_CensusPeople.HouseHoldID = tbl_CensusHouseholds.ID " +
                "WHERE (((tbl_CensusPeople.HouseHoldID)=" + nHouseholdID.ToString() + ")) " +
                "ORDER BY tbl_CensusPeople.ID;";
            return CensusGetRecords(sSql);
        }

        // Returns an array of clsCensusPerson objects as specified in the Sql command.
        /// <summary>
        /// Returns an array of clsCensusPerson objects as specified in the Sql command.
        /// </summary>
		/// <param name="sSql">Specifies a Sql command to fetch a collection of census members.</param>
		/// <returns>An array of clsCensusPerson objects as specified in the Sql command.</returns>
        private clsCensusPerson[] CensusGetRecords(string sSql)
        {
            // Build a list of household members
            ArrayList oMembers = new ArrayList();

            OleDbCommand oSql = new OleDbCommand(sSql, m_cnDb);
            OleDbDataReader drMembers = oSql.ExecuteReader();
            while(drMembers.Read())
            {
                clsCensusPerson oMember = new clsCensusPerson();
                oMember.ID = drMembers.GetInt32(0);
                oMember.HouseholdID = drMembers.GetInt32(1);
                if(!drMembers.IsDBNull(2))
                {
                    oMember.PersonID = drMembers.GetInt32(2);
                }
                if(!drMembers.IsDBNull(3))
                {
                    oMember.CensusName = drMembers.GetString(3);
                }
                if(!drMembers.IsDBNull(4))
                {
                    oMember.RelationToHead = drMembers.GetString(4);
                }
                if(!drMembers.IsDBNull(5))
                {
                    oMember.Age = drMembers.GetString(5);
                }
                if(!drMembers.IsDBNull(6))
                {
                    oMember.Occupation = drMembers.GetString(6);
                }
                if(!drMembers.IsDBNull(7))
                {
                    oMember.BornLocation = drMembers.GetString(7);
                }
                if(!drMembers.IsDBNull(8))
                {
                    oMember.PersonName = drMembers.GetString(8);
                }
                oMember.HouseholdName = drMembers.GetString(9);
                oMember.Date = drMembers.GetDateTime(10);

                oMembers.Add(oMember);
            }
            drMembers.Close();

            // Return the members found
            return (clsCensusPerson[])oMembers.ToArray(typeof(clsCensusPerson));
        }

        // Writes the specified clsCensusPerson record to the database.
        /// <summary>
        /// Writes the specified clsCensusPerson record to the database.
        /// </summary>
		/// <param name="oPerson">Specifies the clsCensusPerson record to write to the database.</param>
		/// <returns>True for success, false otherwise.</returns>
        public bool CensusSavePerson(clsCensusPerson oPerson)
        {
            // Delete this person if required.
            if(!oPerson.IsValid())
            {
                if(oPerson.ID != 0)
                {
                    OleDbCommand oDelete = new OleDbCommand("DELETE FROM tbl_CensusPeople WHERE ID=" + oPerson.ID.ToString() + ";", m_cnDb);
                    oDelete.ExecuteNonQuery();
                }
                return true;
            }

            // Create a new record if required.
            if(oPerson.ID == 0)
            {
                string sSql = "SELECT MAX(ID) AS MaxID FROM tbl_CensusPeople;";
                OleDbCommand oSql = new OleDbCommand(sSql, m_cnDb);
                oPerson.ID = int.Parse(oSql.ExecuteScalar().ToString()) + 1;

                // Create a new record
                sSql = "INSERT INTO tbl_CensusPeople (ID,HouseholdID) VALUES (" + oPerson.ID.ToString() + "," + oPerson.HouseholdID.ToString() + ");";
                oSql = new OleDbCommand(sSql, m_cnDb);
                oSql.ExecuteNonQuery();
            }

            // Update the record
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("UPDATE tbl_CensusPeople SET ");
            sbSql.Append("PersonID=" + oPerson.PersonID.ToString() + ",");
            sbSql.Append("NameGiven=" + ToDb(oPerson.CensusName) + ",");
            sbSql.Append("RelationToHead=" + ToDb(oPerson.RelationToHead) + ",");
            sbSql.Append("Age=" + ToDb(oPerson.Age) + ",");
            sbSql.Append("Occupation=" + ToDb(oPerson.Occupation) + ",");
            sbSql.Append("BornLocation=" + ToDb(oPerson.BornLocation) + " ");
            sbSql.Append("WHERE ID=" + oPerson.ID.ToString() + ";");
            OleDbCommand oUpdate = new OleDbCommand(sbSql.ToString(), m_cnDb);
            oUpdate.ExecuteNonQuery();

            // Return success
            return true;
        }

        // Returns an array of census records that contain the specified person.
        /// <summary>
        /// Returns an array of census records that contain the specified person.
        /// </summary>
		/// <param name="nPersonID"></param>
		/// <returns></returns>
        public clsCensusPerson[] CensusForPerson(int nPersonID)
        {
            string sSql = "SELECT tbl_CensusPeople.*, tbl_People.Forenames+' '+ Iif(IsNull(tbl_People.MaidenName),tbl_People.Surname, tbl_People.MaidenName) AS Name, tbl_CensusHouseholds.Address, tbl_CensusHouseholds.CensusDate " +
                "FROM (tbl_CensusPeople LEFT JOIN tbl_People ON tbl_CensusPeople.PersonID = tbl_People.ID) INNER JOIN tbl_CensusHouseholds ON tbl_CensusPeople.HouseHoldID = tbl_CensusHouseholds.ID " +
                "WHERE tbl_CensusPeople.PersonID=" + nPersonID.ToString() + " " +
                "ORDER BY tbl_CensusHouseholds.CensusDate;";
            return CensusGetRecords(sSql);
        }

        // Returns a human readable string representing the people that the specified person is living with according to the census record.
        /// <summary>
        /// Returns a human readable string representing the people that the specified person is living with according to the census record.
        /// </summary>
		/// <param name="oPerson">Specifies the person who should not be mentioned in the returned description.</param>
		/// <returns>A human readable string representing the people that the specified person is living with according to the census record.</returns>
        public string CensusLivingWith(clsCensusPerson oPerson)
        {
            StringBuilder sbLiving = new StringBuilder();
            string sSql = "SELECT NameGiven, Age FROM tbl_CensusPeople WHERE HouseHoldID=" + oPerson.HouseholdID.ToString() + " AND PersonID<>" + oPerson.PersonID.ToString() + " ORDER BY tbl_CensusPeople.ID;";
            OleDbCommand oSql = new OleDbCommand(sSql, m_cnDb);
            OleDbDataReader drLiving = oSql.ExecuteReader();
            while(drLiving.Read())
            {
                if(sbLiving.Length > 0)
                {
                    sbLiving.Append(", ");
                }
                sbLiving.Append(drLiving.GetString(0));
                if(!drLiving.IsDBNull(1))
                {
                    sbLiving.Append(" (" + drLiving.GetString(1) + ")");
                }
            }
            drLiving.Close();

            sbLiving.Insert(0, "Living with ");

            // Return the built string
            return sbLiving.ToString();
        }

		#endregion

        #region Places

        // Remove any digits or spaces from the front of the specified place name.
        /// <summary>
        /// Remove any digits or spaces from the front of the specified place name.
        /// </summary>
        /// <param name="sPlace">Specifies the place name to clean up.</param>
        /// <returns>A clean version of the place name.</returns>
        private string CleanPlaceName(string sPlace)
        {
            // Check that the input string is valid
            if(sPlace.Length == 0)
            {
                return "";
            }

            // Remove digits and spaces from the front of the place name
            int nI = 0;
            while(sPlace[nI] == ' ' || (sPlace[nI] >= '0' && sPlace[nI] <= '9'))
            {
                nI++;
            }
            return sPlace.Substring(nI);
        }

        // Returns the place object with the specified compound name under the specified parent.
        /// <summary>
        /// Returns the place object with the specified compound name under the specified parent.
        /// </summary>
        /// <param name="sPlace">Specifies the compound name of the place.</param>
        /// <param name="nParentID">Specifies the index of the parent place of the compound name.</param>
        /// <returns>The place object or null.</returns>
        private clsPlace GetPlace(string sPlace, int nParentID)
        {
            // Get the head and tail
            string sHead = sPlace.Trim();
            string sTail = string.Empty;
            int nComma = sPlace.LastIndexOf(',');
            if(nComma > 0)
            {
                sHead = sPlace.Substring(nComma + 1).Trim();
                sTail = sPlace.Substring(0, nComma).Trim();
            }

            // Get the ID of this place
            int nPlaceID = GetPlaceID(sHead, nParentID);
            if(nPlaceID == 0)
            {
                return null;
            }

            // Return this place if at the end of the string.
            if(sTail == string.Empty)
            {
                return new clsPlace(nPlaceID, this);
            }

            // Search further down the string.
            return GetPlace(sTail, nPlaceID);
        }

        // Returns the place object that represents the specified compound place name.
        /// <summary>
        /// Returns the place object that represents the specified compound place name.
        /// </summary>
        /// <param name="sName">Specifies the name of the place.  Eg. Leeds, Yorkshire, England.</param>
        /// <returns>The place object or null.</returns>
        public clsPlace GetPlace(string sPlace)
        {
            return GetPlace(sPlace, 0);
        }

        // Returns the ID of the required place.
        /// <summary>
        /// Returns the ID of the required place.
        /// Returns 0 if the required place does not exist.
        /// </summary>
        /// <param name="sPlace">Specifies the name of the place.  This is not the compound name with , characters.</param>
        /// <param name="nParentID">Specifies the parent of the place.</param>
        /// <returns>The ID of the requested place.  0 if no matching place can be found.</returns>
        private int GetPlaceID(string sPlace, int nParentID)
        {
            // Clean the place name
            sPlace = CleanPlaceName(sPlace);

            // Look for this place in the database
            string sSql = "SELECT ID FROM tbl_Places WHERE Name=" + Innoval.clsDatabase.ToDb(sPlace) + " AND ParentID=" + nParentID.ToString() + ";";
            OleDbCommand oSql = new OleDbCommand(sSql, m_cnDb);
            int nPlaceID = 0;
            try
            {
                nPlaceID = int.Parse(oSql.ExecuteScalar().ToString());
            }
            catch { }

            // Return the ID found.
            return nPlaceID;
        }

        // Adds a compound place to the database.
        /// <summary>
        /// Adds a compound place to the database.
        /// This might result in a number of place records in the tbl_Places table.
        /// It will create a link from the reference object to add the related tbl_Places records.
        /// </summary>
        /// <param name="sPlace">Specifies the compound place separated by , and starting at the top level.</param>
        /// <param name="nObjectTypeID">Specifies the type of object that created this compound place.</param>
        /// <param name="nObjectID">Specifies the ID of the reference object.</param>
        /// <returns>True for success, false otherwise.</returns>
        public bool AddPlace(string sPlace, int nObjectTypeID, int nObjectID)
        {
            // Add this place and link to the specified object
            return AddPlace(sPlace, nObjectTypeID, nObjectID, 0, 0);
        }

        // Adds a compound place to the database.
        /// <summary>
        /// Adds a compound place to the database.
        /// This results in a number of place records in the tbl_Places table.
        /// </summary>
        /// <param name="sPlace">Specifies the compound place separated by , but not nessercary starting with the top level.</param>
        /// <param name="nObjectTypeID">Specifies the type of object that created this compound place.</param>
        /// <param name="nObjectID">Specifies the ID of the reference object.</param>
        /// <param name="nParentID">Specifies the ID of the place that is the parent above the compound place string.</param>
        /// <param name="nLevel">Specifies the level from the top level.</param>
        /// <returns></returns>
        private bool AddPlace(string sPlace, int nObjectTypeID, int nObjectID, int nParentID, int nLevel)
        {
            // Validate the inputs
            if(sPlace == null)
            {
                return false;
            }

            // Split the place into a list of places
            string sHead = sPlace.Trim();
            string sTail = "";
            int nComma = sPlace.LastIndexOf(',');
            if(nComma > 0)
            {
                sHead = sPlace.Substring(nComma + 1).Trim();
                sTail = sPlace.Substring(0, nComma).Trim();
            }

            // Get the ID of this place
            int nPlaceID = GetPlaceID(sHead, nParentID);

            // Add this place (if required)
            if(nPlaceID == 0)
            {
                nPlaceID = AddPlace(nParentID, sHead);
            }

            // Link the object to this place
            string sSql = "INSERT INTO tbl_ToPlaces (PlaceID,TypeID,ObjectID) VALUES(" + nPlaceID.ToString() + "," + nObjectTypeID.ToString() + "," + nObjectID.ToString() + ");";
            OleDbCommand oSql = new OleDbCommand(sSql, m_cnDb);
            try
            {
                oSql.ExecuteNonQuery();
            }
            catch { }

            // Deal with the tail if any
            if(sTail != "")
            {
                AddPlace(sTail, nObjectTypeID, nObjectID, nPlaceID, nLevel + 1);
            }

            // Return success
            return true;
        }

        // Add the specified place to the database.
        /// <summary>
        /// Add the specified place to the database.
        /// </summary>
        /// <param name="nParentID">Specifies the ID of the parent place.</param>
        /// <param name="sName">Specifies the name of the place.</param>
        /// <returns>True for success, false otherwise.</returns>
        private int AddPlace(int nParentID, string sName)
        {
            // Find the ID of the next available place record
            string sSql = "SELECT MAX(ID) AS NewID FROM tbl_Places;";
            OleDbCommand oSql = new OleDbCommand(sSql, m_cnDb);
            int nPlaceID = 0;
            try
            {
                nPlaceID = int.Parse(oSql.ExecuteScalar().ToString());
            }
            catch { }
            nPlaceID++;

            // Clean the name
            sName = CleanPlaceName(sName);

            // Insert the new place
            sSql = "INSERT INTO tbl_Places (ID,Name,ParentID,Status) VALUES (" + nPlaceID.ToString() + ",\"" + sName + "\"," + nParentID.ToString() + ",0);";
            oSql = new OleDbCommand(sSql, m_cnDb);
            oSql.ExecuteNonQuery();

            // Return the ID of this new place
            return nPlaceID;
        }

        // Returns the specified compound place string in html format.
        /// <summary>
        /// Returns the specified compound place string in html format.
        /// Each place in the compound place with have a html link.
        /// </summary>
        /// <param name="sPlace">Specifies the compound place string.</param>
        /// <param name="nParentID">Specifies the ID of the place above the compound place string.</param>
        /// <returns>A html string to represent the specified compound place.</returns>
        private string PlaceToHtml(string sPlace, int nParentID)
        {
            // Get the head and tail
            string sHead = sPlace.Trim();
            string sTail = "";
            int nComma = sPlace.LastIndexOf(',');
            if(nComma > 0)
            {
                sHead = sPlace.Substring(nComma + 1).Trim();
                sTail = sPlace.Substring(0, nComma).Trim();
            }

            // Get the ID of this place
            int nPlaceID = GetPlaceID(sHead, nParentID);

            // Can not encode this place.
            if(nPlaceID == 0)
            {
                if(sTail == "")
                {
                    return sHead;
                }
                return sTail + ", " + sHead;
            }

            // Encode this place into linked html
            string sPlaceHtml = "<A href=\"place:" + nPlaceID.ToString() + "\">" + sHead + "</A>";

            // Deal with the tail
            if(sTail != "")
            {
                return PlaceToHtml(sTail, nPlaceID) + ", " + sPlaceHtml;
            }

            // Return the place in html format
            return sPlaceHtml;
        }

        // Returns the specified compound place string in html format.
        /// <summary>
        /// Returns the specified compound place string in html format.
        /// </summary>
        /// <param name="sPlace">Specifies the full compound place string including the top level.</param>
        /// <returns>A html string to represent the specified compound place string.</returns>
        public string PlaceToHtml(string sPlace)
        {
            return PlaceToHtml(sPlace, 0);
        }

        // Returns the specified compound place string in Gedcom format.
        /// <summary>
        /// Returns the specified compound place string in Gedcom format.
        /// The full compound place will not be returned.
        /// The portion of the compound place will depend if a place record or an address record is required.
        /// </summary>
        /// <param name="sPlace">Specifies the compound location string.</param>
        /// <param name="nStatus">Specify 0 to use the place in a PLAC record.  Specify 1 to use the place in an ADDR record.</param>
        /// <param name="nParentID">Specifies the ID parent place above the compound location string.</param>
        /// <returns>A string to represent the place in a gedcom file.</returns>
        private string PlaceToGedcom(string sPlace, int nStatus, int nParentID)
        {
            // Get the head and tail
            string sHead = sPlace.Trim();
            string sTail = "";
            int nComma = sPlace.LastIndexOf(',');
            if(nComma > 0)
            {
                sHead = sPlace.Substring(nComma + 1).Trim();
                sTail = sPlace.Substring(0, nComma).Trim();
            }

            // Get the ID of this place
            int nPlaceID = GetPlaceID(sHead, nParentID);

            // Can not encode this place.
            if(nPlaceID == 0)
            {
                return "";
            }

            // Get the status of this place
            string sSql = "SELECT Status FROM tbl_Places WHERE ID=" + nPlaceID.ToString() + ";";
            OleDbCommand oSql = new OleDbCommand(sSql, m_cnDb);
            int nPlaceStatus = 0;
            try
            {
                nPlaceStatus = int.Parse(oSql.ExecuteScalar().ToString());
            }
            catch { }

            // Deal with the tail
            if(sTail != "")
            {
                sTail = PlaceToGedcom(sTail, nStatus, nPlaceID);
            }

            // If the status is correct then return the head.
            if(nPlaceStatus == nStatus)
            {
                if(sTail != "")
                {
                    return sTail + ", " + sHead;
                }
                return sHead;
            }
            else
            {
                if(sTail != "")
                {
                    return sTail;
                }
            }

            // Nothing
            return "";
        }

        // Returns the specified compound place string in gedcom format.
        /// <summary>
        /// Returns the specified compound place string in gedcom format.
        /// The full compound place will not be returned.
        /// The portion of the compound place will depend if a place record or an address record is required.
        /// </summary>
        /// <param name="sPlace">Specifies the full compound location string.</param>
        /// <param name="nStatus">Specify 0 to use the place in a PLAC record.  Specify 1 to use the place in an ADDR record.</param>
        /// <returns>A string to represent the place in a gedcom file.</returns>
        public string PlaceToGedcom(string sPlace, int nStatus)
        {
            return PlaceToGedcom(sPlace, nStatus, 0);
        }

        // Write the gedcom PLAC record. and ADDR records from a single place record in the database.
        /// <summary>
        /// Write the gedcom PLAC record.
        /// This can also include optional ADDR, MAP, and CTRY records.
        /// Originally this was only PLAC and ADDR records.
        /// </summary>
        /// <param name="oFile">Specifies the file to write the PLAC and ADDR records into.</param>
        /// <param name="nLevel">Specifies the level to write the records at.</param>
        /// <param name="sFullPlace">Specifies the database place record to be decoded into gedcom PLAC and ADDR records.</param>
        /// <param name="oOptions">Specifies the Gedcom options to apply to this place.</param>
        /// <returns>True for success, false otherwise.</returns>
        public bool WriteGedcomPlace(StreamWriter oFile, int nLevel, string sFullPlace,clsGedcomOptions oOptions)
        {
            // Optionally split the address off from the PLAC tag
            if(oOptions.RemoveADDRfromPLAC)
            {
                string sPlace = PlaceToGedcom(sFullPlace, 0);
                if(sPlace != "")
                {
                    oFile.WriteLine(nLevel.ToString() + " PLAC " + sPlace);
                }
            }
            else
            {
                if(sFullPlace != "")
                {
                    oFile.WriteLine(nLevel.ToString() + " PLAC " + sFullPlace);
                }
            }

            // Really expect that some tag was created above.
            // If not then the level should not increase.
            // PhpGedView did not increase the level originally.

            // Add the optional MAP tag with longitude and latitude
            if(oOptions.UseLongitude)
            {
                clsPlace oPlace = GetPlace(sFullPlace);
                if(oPlace != null)
                {
                    oFile.WriteLine((nLevel + 1).ToString() + " MAP");
                    if(oPlace.Latitude >= 0)
                    {
                        oFile.WriteLine((nLevel + 2).ToString() + " LATI N" + oPlace.Latitude.ToString());
                    }
                    else
                    {
                        oFile.WriteLine((nLevel + 2).ToString() + " LATI S" + Math.Abs(oPlace.Latitude).ToString());
                    }
                    if(oPlace.Longitude > 0)
                    {
                        oFile.WriteLine((nLevel + 2).ToString() + " LONG E" + oPlace.Longitude.ToString());
                    }
                    else
                    {
                        oFile.WriteLine((nLevel + 2).ToString() + " LONG W" + Math.Abs(oPlace.Longitude).ToString());
                    }
                }
            }

            // Optionally include an ADDR tag
            if(oOptions.UseADDR )
            {
                string sAddress = PlaceToGedcom(sFullPlace, 1);
                if(sAddress != "")
                {
                    oFile.WriteLine((nLevel+1).ToString() + " ADDR " + sAddress);
                }
            }
            
            // Include the optional CTRY (Country) tag
            if(oOptions.UseCTRY)
            {
                int nComma = sFullPlace.LastIndexOf (',');
                if(nComma > 0)
                {
                    string sCountry = sFullPlace.Substring(nComma + 2);
                    oFile.WriteLine((nLevel+1).ToString() + " CTRY " + sCountry);
                }
            }

            // Return success
            return true;
        }

        // Removes all the place links for the specified object.
        /// <summary>
        /// Removes all the place links for the specified object.
        /// </summary>
        /// <param name="nObjectTypeID">Specifies the type of object.</param>
        /// <param name="nObjectID">Specifies the ID of the object.</param>
        public void PlaceDelink(int nObjectTypeID, int nObjectID)
        {
            string sSql = "DELETE FROM tbl_ToPlaces WHERE TypeID=" + nObjectTypeID.ToString() + " AND ObjectID=" + nObjectID.ToString() + ";";
            OleDbCommand oSql = new OleDbCommand(sSql, m_cnDb);
            try
            {
                oSql.ExecuteNonQuery();
            }
            catch { }
        }

        // Removes all the places that have no links to them.
        /// <summary>
        /// Removes all the places that have no links to them.
        /// </summary>
        public int PlaceRemoveUnlinked()
        {
            // Get the list of places to delete
            string sSql = "SELECT tbl_Places.ID, Count(tbl_ToPlaces.TypeID) AS CountOfTypeID "+
                "FROM tbl_Places LEFT JOIN tbl_ToPlaces ON tbl_Places.ID = tbl_ToPlaces.PlaceID "+
                "GROUP BY tbl_Places.ID "+
                "HAVING (((Count(tbl_ToPlaces.TypeID))=0));";
            OleDbCommand oSql = new OleDbCommand(sSql,m_cnDb);
            ArrayList oDelete = new ArrayList();
            OleDbDataReader drDelete = oSql.ExecuteReader();
            while(drDelete.Read())
            {
                oDelete.Add(drDelete.GetInt32(0));
            }
            drDelete.Close();

            // Delete the places
            foreach(int nPlaceID in oDelete)
            {
                sSql = "DELETE FROM tbl_Places WHERE ID="+nPlaceID.ToString()+";";
                oSql = new OleDbCommand(sSql,m_cnDb);
                oSql.ExecuteNonQuery();
            }

            // Return the number of places that are removed.
            return oDelete.Count;
        }

        public clsPlace[] GetPlaces(int nPlaceID)
        {
            // Build a list to contain the places
            ArrayList oPlaces = new ArrayList();

            // Build the list of child places
            string sSql = "SELECT * FROM tbl_Places WHERE ParentID=" + nPlaceID.ToString() + " ORDER BY Name;";
            OleDbCommand oSql = new OleDbCommand(sSql, m_cnDb);
            OleDbDataReader drPlaces = oSql.ExecuteReader();
            while(drPlaces.Read())
            {
                clsPlace oPlace = new clsPlace(drPlaces, this);
                oPlaces.Add(oPlace);
            }
            drPlaces.Close();

            // Return the list of places
            return (clsPlace[])oPlaces.ToArray(typeof(clsPlace));
        }

        #endregion

        // Returns a list of the editors on this database.
        /// <summary>
        /// Returns a list of the editors on this database.
        /// </summary>
        /// <returns>A list of the editors on this database.</returns>
        public string[] GetEditors()
        {
            // Open a dataset of editors
            string sSql = "SELECT Name FROM tbl_Editors ORDER BY Name;";
            OleDbCommand oSql = new OleDbCommand(sSql,m_cnDb);
            OleDbDataReader drEditors = oSql.ExecuteReader();

            // Create an array list of the editors
            ArrayList oEditors = new ArrayList();
            while(drEditors.Read())
            {
                oEditors.Add(drEditors.GetString(0));
            }

            // Return the list of editors
            return (string[])oEditors.ToArray(typeof(string));
        }

        // Returns a html description of the ToDo items.
        /// <summary>
        /// Returns a html description of the ToDo items.
        /// </summary>
        /// <returns>A html description of the ToDo items.</returns>
        public string GetToDoAsHtml()
        {
            // Create a data adapter to load the information.
            string sSql = "SELECT Description, Priority, PersonID, ID FROM tbl_ToDo ORDER BY Priority, ID;";
            OleDbCommand oSql = new OleDbCommand(sSql,m_cnDb);
            OleDbDataReader drChanges = oSql.ExecuteReader();

            // Create a html document to return
            StringBuilder sbHtml = new StringBuilder();
            sbHtml.Append("<h1>To Do List</h1>");
            sbHtml.Append("<table cellpadding=\"3\" cellspacing=\"2\">");

            while(drChanges.Read())
            {
                sbHtml.Append("<tr bgcolor=\"silver\">");
                sbHtml.Append("<td><span class=\"Small\">");
                int nPersonID = GetInt(drChanges,"PersonID",0);
                sbHtml.Append("<a href=\"person:" + nPersonID.ToString() + "\">");
                clsPerson oPerson = new clsPerson(nPersonID,this);
                sbHtml.Append(oPerson.GetName(true,true));
                sbHtml.Append("</a>");
                sbHtml.Append("</span></td>");
                //sbHtml.Append("<td><span class=\"Small\">" + GetInt(drChanges,"Priority",0).ToString() + "</span></td>");
                //sbHtml.Append("<td><span class=\"Small\">" + GetString(drChanges,"Description","") + "</span></td>");
                sbHtml.Append("<td align=\"right\">" + GetInt(drChanges,"Priority",0).ToString() + "</td>");
                sbHtml.Append("<td>" + GetString(drChanges,"Description","") + "</td>");
                sbHtml.Append("</tr>");
            }
            drChanges.Close();
            sbHtml.Append("</table>");

            // Return the html that has been built
            return sbHtml.ToString();
        }

        // Returns a html description of the recent changes.
        /// <summary>
        /// Returns a html description of the recent changes.
        /// </summary>
		/// <returns>A html description of the recent changes.</returns>
		public string GetRecentChangesAsHtml()
		{
            // Create a data adapter to load the information.
            string sSql = "SELECT Format(LastEditDate,'d-mmm-yyyy hh:mm:ss') AS EditDate,LastEditBy,Type,Name,ID FROM ("
                + "SELECT tbl_Relationships.LastEditDate, tbl_Relationships.LastEditBy, 'Relationship' AS Type, tbl_People.Forenames+' '+ tbl_People.Surname+' & '+ tbl_People_1.Forenames+' '+ tbl_People_1.Surname AS Name, tbl_Relationships.ID "
                + "FROM (tbl_People INNER JOIN tbl_Relationships ON tbl_People.ID = tbl_Relationships.MaleID) INNER JOIN tbl_People AS tbl_People_1 ON tbl_Relationships.FemaleID = tbl_People_1.ID "
                + "UNION "
                + "SELECT LastEditDate, LastEditBy, 'Person' AS Type, Forenames +' '+Surname AS Name, ID FROM tbl_People "
                + ")"
                + "ORDER BY LastEditDate DESC;";
            OleDbCommand oSql = new OleDbCommand(sSql,m_cnDb);
            OleDbDataReader drChanges = oSql.ExecuteReader();

            // Create a html document to return
            StringBuilder sbHtml = new StringBuilder();

            sbHtml.Append("<h1>Recent Changes</h1>");
            sbHtml.Append("<table cellpadding=\"3\" cellspacing=\"2\">");

            while(drChanges.Read())
            {
                sbHtml.Append("<tr bgcolor=\"silver\">");
                /*
                sbHtml.Append("<td><span class=\"Small\">" + GetString(drChanges,"EditDate","") + "</span></td>");
                sbHtml.Append("<td><span class=\"Small\">" + GetString(drChanges,"LastEditBy","") + "</span></td>");
                sbHtml.Append("<td><span class=\"Small\">" + GetString(drChanges,"Type","") + "</span></td>");
                sbHtml.Append("<td><span class=\"Small\">");
                 */
                sbHtml.Append("<td>" + GetString(drChanges,"EditDate","") + "</td>");
                sbHtml.Append("<td>" + GetString(drChanges,"LastEditBy","") + "</td>");
                sbHtml.Append("<td>" + GetString(drChanges,"Type","") + "</td>");
                sbHtml.Append("<td>");
                switch(GetString(drChanges,"Type",""))
                {
                case "Person":
                    sbHtml.Append("<a href=\"person:" + GetInt(drChanges,"ID",0) + "\">");
                    break;
                }
                sbHtml.Append(GetString(drChanges,"Name",""));
                switch(GetString(drChanges,"Type",""))
                {
                case "Person":
                    sbHtml.Append("</a>");
                    break;
                }

                sbHtml.Append("</td>");
                sbHtml.Append("</tr>");
            }
            drChanges.Close();
            sbHtml.Append("</table>");

            // Return the html that has been built
            return sbHtml.ToString();
        }

        // Returns the input string with Html linebreaks as required.
        /// <summary>
        /// Returns the input string with Html linebreaks as required.
        /// </summary>
        /// <param name="sText">Specifies the input string.</param>
        /// <returns>The specified input string with Html linebreaks.</returns>
        public static string HtmlString(string sText)
        {
            StringBuilder sbHtml = new StringBuilder(sText);
            sbHtml.Replace("\n", "<br />");

            return sbHtml.ToString();
        }

        // Returns the media directory for this database.
        /// <summary>
        /// Returns the media directory for this database.
        /// </summary>
        /// <returns>The full path name of the directory that contains the media files for this database.</returns>
        // TODO: Make this into a database list and remember a value once it works.
        public string GetMediaDirectory()
        {
            // Directory to use at home
            if(Directory.Exists("\\\\Qnap119\\waltonsdocuments\\Family Tree\\Media"))
            {
                return "\\\\Qnap119\\waltonsdocuments\\Family Tree\\Media";
            }

            // Directory to use on work network
            if(Directory.Exists("\\\\Innovalsrv21\\personal\\gramps\\Media"))
            {
                return "\\\\Innovalsrv21\\personal\\gramps\\Media";
            }

            // Directory to use on work laptop
            if(Directory.Exists("C:\\Documents and Settings\\waltons\\My Documents\\Local\\Steve Walton\\Family Tree Media"))
            {
                return "C:\\Documents and Settings\\waltons\\My Documents\\Local\\Steve Walton\\Family Tree Media";
            }

            return "";
        }

		#region Static helper functions

        // Returns a string that can be inserted into a SQL command as the value for a string field.
        /// <summary>
        /// Returns a string that can be inserted into a SQL command as the value for a string field.
        /// </summary>
		/// <param name="sValue">Specifies the value for a SQL string field.</param>
		/// <returns>A string that can inserted into a SQL command.</returns>
		public static string ToDb			(			string sValue			)
		{
			// Check for any empty string
			if(sValue==null)
			{
				return "NULL";
			}
			if(sValue.Length==0)
			{
				return "NULL";
			}

			// Remove any double quotes
            if(sValue.Contains("\""))
            {
                // Replace all the double quotes with single quotes.
                sValue = sValue.Replace("\"","'");
            }

			// Return the string
			return "\""+sValue+"\"";
		}
        
        // Returns a string that can be inserted into a SQL command as the value for a date time field.
        /// <summary>
        /// Returns a string that can be inserted into a SQL command as the value for a date time field.
        /// </summary>
		/// <param name="oValue">Specifies the value for a SQL date/time field.</param>
		/// <returns>A string that can be inserted into a SQL command.</returns>
		public static string ToDb			(			clsDate oValue			)
		{
			// Check for a NULL date
			if(oValue.Status==15)
			{
				return "NULL";
			}

			// return the date
			return "#"+oValue.Date.ToString("d-MMM-yyyy")+"#";
		}
        
        // Returns a string that can be inserted into a Sql command as the value for a date field.
        /// <summary>
        /// Returns a string that can be inserted into a Sql command as the value for a date field.
        /// </summary>
        /// <param name="oValue">Specifies the value for a Sql date field.</param>
        /// <returns>A string that can be inserted into a Sql command.</returns>
        public static string ToDb            (            DateTime oValue            )
        {
            return "#" + oValue.ToString("d-MMM-yyyy") + "#";
        }

        // Returns a string that can be inserted into a SQL command as the value for a boolean field.  This is a 0 or 1.
        /// <summary>
        /// Returns a string that can be inserted into a SQL command as the value for a boolean field.  This is a 0 or 1.
        /// </summary>
		/// <param name="bValue">Specifies the value for a SQL boolean field.</param>
		/// <returns>A string that can be inserted into a SQL command.</returns>
		public static string ToDb			(			bool	bValue			)
		{
			if(bValue)
			{
				return "1";
			}
			return "0";
		}

        // Returns a string representing the specified integer in a SQL command.  Returns NULL if the specified integer matches the specified
        /// <summary>
        /// Returns a string representing the specified integer in a SQL command.  Returns NULL if the specified integer matches the specified
        /// null code.
		/// </summary>
		/// <param name="nValue">Specifies the value to return.</param>
		/// <param name="nNullValue">Specifies the value that represents NULL</param>
		/// <returns>A string to represent the specified integer in SQL command.</returns>
		public static string ToDb			(			int nValue,			int nNullValue			)
		{
			if(nValue==nNullValue)
			{
				return "NULL";
			}
			return nValue.ToString();
		}

        // Returns the specified string sTrue if the specified bCondition is true otherwise returns the specified string sFalse.
        /// <summary>
        /// Returns the specified string sTrue if the specified bCondition is true otherwise returns the specified string sFalse.
        /// </summary>
		/// <param name="bCondition">Specifies the condition to test.</param>
		/// <param name="sTrue">Specifies the string to return if the specified condition is true.</param>
		/// <param name="sFalse">Specifies the string to return if the specified condition is false.</param>
		/// <returns>Either the sTrue or sFalse dependant on bCondition.</returns>
		public static string Iif			(			bool bCondition,			string sTrue,			string sFalse			)
		{
			if(bCondition)
			{
				return sTrue;
			}
			else
			{
				return sFalse;
			}
		}

        // Returns the string in a data reader column without raising an error.
        /// <summary>
        /// Returns the string in a data reader column without raising an error.
        /// </summary>
		/// <param name="oReader">Specifies the data reader to read from.</param>
		/// <param name="sColumn">Specifies the label of the column to read.</param>
		/// <param name="sNull">Specifies the value to return is the field is null.</param>
		/// <returns>The string value of the specified column.</returns>
		public static string GetString			(			OleDbDataReader oReader,			string sColumn,			string sNull			)
		{
			int nOrdinal = oReader.GetOrdinal(sColumn);
			if(oReader.IsDBNull(nOrdinal))
			{
				return sNull;
			}
			return oReader.GetString(nOrdinal);
		}

        // Returns a fields from the specified data reader as a int without raising an error.
        /// <summary>
        /// Returns a fields from the specified data reader as a int without raising an error.
        /// </summary>
		/// <param name="oReader">Specifies the data reader to read from.</param>
		/// <param name="sColumn">Specifies the label of the column to read.</param>
		/// <param name="nNull">Specifies the value to return if the field is null.</param>
		/// <returns>The int value of the specified column.</returns>
		public static int GetInt			(			OleDbDataReader oReader,			string sColumn,			int nNull			)
		{
			int nOrdinal = oReader.GetOrdinal(sColumn);
			if(oReader.IsDBNull(nOrdinal))
			{
				return nNull;
			}
			// Will need to switch on the actual type here.
			return oReader.GetInt32(nOrdinal);
		}

        // Returns a field from the specified data reader as a DateTime without raising an error.
        /// <summary>
        /// Returns a field from the specified data reader as a DateTime without raising an error.
        /// </summary>
		/// <param name="oReader">Specifies the data reader to read from.</param>
		/// <param name="sColumn">Specifies the label of the column to read.</param>
		/// <param name="dtNull">Specifies the value to return if the field is null.</param>
		/// <returns>The DateTime value of the specified column.</returns>
		public static DateTime GetDateTime			(			OleDbDataReader oReader,			string sColumn,			DateTime dtNull			)
		{
			int nOrdinal = oReader.GetOrdinal(sColumn);
			if(oReader.IsDBNull(nOrdinal))
			{
				return dtNull;
			}
			return oReader.GetDateTime(nOrdinal);
		}

        // Returns the value of the specified field in the specified data reader as a boolean.
        /// <summary>
        /// Returns the value of the specified field in the specified data reader as a boolean.
        /// </summary>
		/// <param name="oReader">Specifies the data reader.</param>
		/// <param name="sColumn">Specifies the name of the field.</param>
		/// <param name="bNull">Specifies the value to return if the field is null.</param>
		/// <returns>The value of the specified field as a bool.</returns>
		public static bool GetBool			(			OleDbDataReader oReader,			string sColumn,			bool bNull			)
		{
			int nOrdinal = oReader.GetOrdinal(sColumn);
			if(oReader.IsDBNull(nOrdinal))
			{
				return bNull;
			}
			return oReader.GetBoolean(nOrdinal);
		}

		#endregion
		
		#region Public Properties

		/// <summary>Connection to the database.</summary>
		internal OleDbConnection cnDB { get { return m_cnDb; } }

		/// <summary>Range of differing ages to present people as possible marriage partners.</summary>
		public int RelationshipRange { get { return m_nMarriedRange; } set { m_nMarriedRange = value; } }

        /// <summary>The filename of the source database file.</summary>
        public string Filename { get { return m_sFilename; } }

		#endregion

        // Writes SQL script into the specified file
        /// <summary>
        /// Writes SQL script into the specified file
        /// </summary>
        /// <param name="sFilename">Specifies the filename of the SQL Script file to create.</param>
        public void WriteSQL(string sFilename)
        {
            // Open the output file
            StreamWriter oFile = new StreamWriter(sFilename, false);

            // Write the table of people
            oFile.WriteLine("--");
            oFile.WriteLine("-- Insert the table of people.");
            oFile.WriteLine("--");
            oFile.WriteLine("DROP TABLE IF EXISTS PEOPLE;");
            oFile.WriteLine("CREATE TABLE PEOPLE ('ID' INTEGER PRIMARY KEY,'SURNAME' TEXT,'FORENAMES' TEXT,'MAIDEN_NAME' TEXT);");
            oFile.WriteLine("BEGIN TRANSACTION;");
            // Get the list of people
            string sSql = "SELECT * FROM tbl_People ORDER BY ID;";
            OleDbCommand oSql = new OleDbCommand(sSql, m_cnDb);
            OleDbDataReader drPlaces = oSql.ExecuteReader();
            while(drPlaces.Read())
            {
                oFile.Write("INSERT INTO PEOPLE (ID,SURNAME,FORENAMES,MAIDEN_NAME) VALUES(");
                oFile.Write(Innoval.clsDatabase.ToDb(Innoval.clsDatabase.GetInt(drPlaces, "ID", 0), 0) + ",");
                oFile.Write(Innoval.clsDatabase.ToDb(Innoval.clsDatabase.GetString(drPlaces, "Surname", "")) + ",");
                oFile.Write(Innoval.clsDatabase.ToDb(Innoval.clsDatabase.GetString(drPlaces, "Forenames", "")) + ",");
                oFile.Write(Innoval.clsDatabase.ToDb(Innoval.clsDatabase.GetString(drPlaces, "MaidenName", "")) + "");
                oFile.WriteLine(");");
            }
            drPlaces.Close();
            oFile.WriteLine("END TRANSACTION;");
            oFile.WriteLine();

            // Write the table of places
            oFile.WriteLine("--");
            oFile.WriteLine("-- Insert the table of places.");
            oFile.WriteLine("--");
            oFile.WriteLine("DROP TABLE IF EXISTS PLACES;");
            oFile.WriteLine("CREATE TABLE PLACES ('ID' INTEGER PRIMARY KEY,'NAME' TEXT,'PARENT_ID' INTEGER,'STATUS' INTEGER,'LONGITUDE' REAL,'LATITUDE' REAL,'GOOGLE_ZOOM' INTEGER,'USE_PARENT_LOCATION' INTEGER,'PRIVATE_COMMENTS' TEXT);");
            oFile.WriteLine("BEGIN TRANSACTION;");
            // Get the list of places
            sSql = "SELECT ID,Name,ParentID,Status,Longitude,Latitude,GoogleZoom,UseParentLocation,PrivateComments FROM tbl_Places ORDER BY ID;";
            oSql = new OleDbCommand(sSql, m_cnDb);
            drPlaces = oSql.ExecuteReader();
            while(drPlaces.Read())
            {
                oFile.Write("INSERT INTO PLACES (ID,NAME,PARENT_ID,STATUS,LONGITUDE,LATITUDE,GOOGLE_ZOOM,USE_PARENT_LOCATION,PRIVATE_COMMENTS) VALUES(");
                oFile.Write(ToDb(drPlaces.GetInt32(0), 0) + ",");
                oFile.Write(ToDb(drPlaces.GetString(1)) + ",");
                oFile.Write(ToDb(drPlaces.GetInt32(2), 0) + ",");
                oFile.Write(drPlaces.GetInt32(3).ToString() + ",");
                oFile.Write(Innoval.clsDatabase.ToDb(Innoval.clsDatabase.GetFloat(drPlaces, "Longitude", -999f), -999f) + ",");
                oFile.Write(Innoval.clsDatabase.ToDb(Innoval.clsDatabase.GetFloat(drPlaces, "Latitude", -999f), -999f) + ",");
                oFile.Write(Innoval.clsDatabase.ToDb(Innoval.clsDatabase.GetInt(drPlaces, "GoogleZoom", 0),0) + ",");
                oFile.Write(Innoval.clsDatabase.ToDb(Innoval.clsDatabase.GetBool(drPlaces, "UseParentLocation", false)) + ",");
                oFile.Write(Innoval.clsDatabase.ToDb(Innoval.clsDatabase.GetString(drPlaces, "PrivateComments", "")));
                oFile.WriteLine(");");
            }
            drPlaces.Close();
            oFile.Write("END TRANSACTION;");

            // Close the output file
            oFile.Close();
        }

    }
}
