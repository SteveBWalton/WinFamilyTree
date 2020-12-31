using System;
// ArrayList
using System.Collections;
// Access database ADO.NET
using System.Data.OleDb;
using System.IO;
// StringBuilder
using System.Text;

namespace FamilyTree.Objects
{
    #region Supporting types, enums, etc...

    /// <summary>Delegate for functions that refresh the UI.</summary>
    public delegate void funcVoid();

    /// <summary>When searching for people.  Pre-select one sex or not.</summary>
	public enum ChooseSex
    {
        /// <summary>Search for male people only.</summary>
        MALE,
        /// <summary>Search for female people only.</summary>
        FEMALE,
        /// <summary>Search through all people.  Don't care about the sex.</summary>
        EITHER
    }

    /// <summary>The sort order for results of a search.</summary>
	public enum SortOrder
    {
        /// <summary>Return the data in date order.</summary>
        DATE,
        /// <summary>Return the data in alphabetical order.</summary>
        ALPHABETICAL
    }

    #endregion

    /// <summary>Class to represent a database of family tree information.</summary>
    public class Database
    {
        #region Member Variables

        /// <summary>Connection to a database.</summary>
        private OleDbConnection cndb_;

        /// <summary>List of fact types.</summary>
        private clsFactType[] factTypes_;

        /// <summary>Range of birthdate difference used to search for marriage partners.</summary>
        private int marriedRange_;

        /// <summary>Filename of the database.</summary>
        private string fileName_;

        #endregion

        #region Constructors Destructors etc...



        /// <summary>Class constructor.  Opens the specified database.  Call Dispose() to close the database.</summary>
        /// <param name="fileName">Specifies the filename of the family tree access database</param>
        public Database(string fileName)
        {
            fileName_ = fileName;
            factTypes_ = null;
            marriedRange_ = 20;

            // Open the connection to the database				 
            cndb_ = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + fileName + ";");
            cndb_.Open();
        }



        /// <summary>Class destructor.  Do not depend on the timing of this function.  Dot net calls destructors in it's own sweet time.</summary>
        ~Database()
        {
            Dispose();
        }



        /// <summary>Closes the database and releases all handles.</summary>
        public void Dispose()
        {
            if (cndb_ != null)
            {
                cndb_.Close();
                cndb_ = null;
            }
        }



        #endregion

        #region People



        /// <summary>Returns the person object specified by nPersonID.</summary>
        /// <param name="personIndex">Specifies the ID of the person required</param>
        /// <returns>The person objects co-responding the the ID specified by nPersonID</returns>
        public Person getPerson(int personIndex)
        {
            // Open the specified person ID.
            Person person = new Person(personIndex, this);

            // Return the person object.
            return person;
        }



        /// <summary>Returns a list of people in an array of clsIDName pairs who match the specfied criteria.</summary>
        /// <param name="sex">Specify the sex of the required people</param>
        /// <param name="order">Specify the order of the returned array.</param>
        /// <param name="startYear">Specify the earliest birth year of the required people</param>
        /// <param name="endYear">Specify the latest birth year of the required people.</param>
        /// <returns>An array of IDName pairs representing people who match the specified criteria.</returns>
        public IndexName[] getPeople
            (
            ChooseSex sex,
            SortOrder order,
            int startYear,
            int endYear
            )
        {
            // Build the SQL command.
            string sql = "SELECT ID, Forenames, Surname, Maidenname, Born, BornStatusID, Died, DiedStatusID FROM tbl_People WHERE ";
            switch (sex)
            {
            case ChooseSex.MALE:
                sql += "Sex = 'M' AND ";
                break;
            case ChooseSex.FEMALE:
                sql += "Sex = 'F' AND ";
                break;
            }
            sql += "((Year(Born)>=" + startYear.ToString() + " AND Year(Born)<=" + endYear.ToString() + ") OR Born IS NULL) ";

            switch (order)
            {
            case SortOrder.DATE:
                sql += "ORDER BY Born;";
                break;

            case SortOrder.ALPHABETICAL:
                sql += "ORDER BY Surname,Forenames;";
                break;
            }

            return getPeople(sql);
        }



        /// <summary>Returns a list of people in an array of clsIDName objects who are returned by the specified Sql command.</summary>
        /// <param name="sql">Specifies an Sql command that will return a list of people.</param>
        /// <returns>An array of clsIDName objects representing the people in the specified Sql command.</returns>
        public IndexName[] getPeople
            (
            string sql
            )
        {
            // Initialise the list of people to return
            ArrayList items = new ArrayList();

            // Open a list of people
            OleDbCommand sqlCommand = new OleDbCommand(sql, cndb_);
            OleDbDataReader people = sqlCommand.ExecuteReader();
            while (people.Read())
            {
                StringBuilder name = new StringBuilder();
                if (!people.IsDBNull(1))
                {
                    name.Append(people.GetString(1));
                }
                if (!people.IsDBNull(2))
                {
                    name.Append(" ");
                    name.Append(people.GetString(2));
                }
                if (!people.IsDBNull(3))
                {
                    name.Append(" nee ");
                    name.Append(people.GetString(3));
                }
                name.Append(" (");
                if (!people.IsDBNull(4))
                {
                    int bornYear = CompoundDate.getYear(people.GetDateTime(4).Year);
                    if (bornYear < 0)
                    {
                        name.Append((-bornYear).ToString());
                        name.Append(" BC");
                    }
                    else
                    {
                        name.Append(bornYear.ToString());
                    }
                }
                if (!people.IsDBNull(6))
                {
                    name.Append("-");
                    int diedYear = CompoundDate.getYear(people.GetDateTime(6).Year);
                    if (diedYear < 0)
                    {
                        name.Append((-diedYear).ToString());
                        name.Append(" BC");
                    }
                    else
                    {
                        name.Append(diedYear.ToString());
                    }
                }
                name.Append(")");
                IndexName item = new IndexName(people.GetInt32(0), name.ToString());
                items.Add(item);
            }
            people.Close();

            // Return success
            return (IndexName[])items.ToArray(typeof(IndexName));
        }



        /// <summary>Returns a list of all the people in an array of clsIDName objects.</summary>
        /// <returns>An array of clsIDName objects representing all the people.</returns>
        public IndexName[] getPeople()
        {
            return getPeople("SELECT ID,Forenames,Surname,Maidenname,Born,BornStatusID,Died,DiedStatusID FROM tbl_People ORDER BY ID;");
        }



        /// <summary>Returns a list of the people of the specified sex who are alive in the specified year.</summary>
        /// <param name="sex">Specifies the sex of the required people.</param>
        /// <param name="order">Specifies the order of the array to return.</param>
        /// <param name="yearAlive">Specifies the year when the returned people must have been alive.</param>
        /// <returns>An array of clsIDName objects representing the people alive at the specified year.</returns>
        public IndexName[] GetPeople
            (
            ChooseSex sex,
            SortOrder order,
            int yearAlive
            )
        {
            // Build the SQL command.
            string sql = "SELECT ID, Forenames, Surname, Maidenname, Born, BornStatusID, Died, DiedStatusID FROM tbl_People WHERE ";
            switch (sex)
            {
            case ChooseSex.MALE:
                sql += "Sex = 'M' AND ";
                break;
            case ChooseSex.FEMALE:
                sql += "Sex = 'F' AND ";
                break;
            }
            sql += "Year(Born) <= " + yearAlive.ToString() + " AND Iif(IsNull(Died), Year(Born) + 120, Year(Died)) >= " + yearAlive.ToString() + " ";

            switch (order)
            {
            case SortOrder.DATE:
                sql += "ORDER BY Born;";
                break;

            case SortOrder.ALPHABETICAL:
                sql += "ORDER BY Surname,Forenames;";
                break;
            }

            return getPeople(sql);
        }



        /// <summary>Returns a list of people of the specified sex in the specified order.</summary>
        /// <param name="sex">Specifies the sex of the required people.</param>
        /// <param name="order">Specifies the order of the array to return.</param>
        /// <returns>An array of clsIDName objects representing the specified people in the specified order.</returns>
        public IndexName[] getPeople
            (
            ChooseSex sex,
            SortOrder order
            )
        {
            // Build the SQL command.
            string sql = "SELECT ID, Forenames, Surname, Maidenname, Born, BornStatusID, Died, DiedStatusID FROM tbl_People ";
            switch (sex)
            {
            case ChooseSex.MALE:
                sql += "WHERE Sex = 'M' ";
                break;
            case ChooseSex.FEMALE:
                sql += "WHERE Sex = 'F' ";
                break;
            }

            switch (order)
            {
            case SortOrder.DATE:
                sql += "ORDER BY Born;";
                break;

            case SortOrder.ALPHABETICAL:
                sql += "ORDER BY Surname,Forenames;";
                break;
            }

            // Return the collection of people.
            return getPeople(sql);
        }



        #endregion

        #region Relationships



        /// <summary>Returns the relationship between the 2 specified people.  Returns null if there is no relationship.  This is a slightly strange relationship object because it has no owner.</summary>
        /// <param name="maleIndex">Specifies the male person in the relationship.</param>
        /// <param name="femaleIndex">Specifies the female person in the relationship.</param>
        /// <returns>The relationship object for the 2 people.  NULL if no relationshp exists between the 2 specified people.</returns>
        public clsRelationship getRelationship(int maleIndex, int femaleIndex)
        {
            OleDbCommand sqlCommand = new OleDbCommand("SELECT ID, TerminatedID, TheDate, StartStatusID, TerminateDate, TerminateStatusID, Location, Comments, RelationshipID, LastEditBy, LastEditDate FROM tbl_Relationships WHERE MaleID = " + maleIndex.ToString() + " AND FemaleID = " + femaleIndex.ToString() + " ORDER BY TheDate DESC;", cndb);
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            clsRelationship relationship = null;
            if (dataReader.Read())
            {
                relationship = new clsRelationship(dataReader.GetInt32(0));
                relationship.maleIndex = maleIndex;
                relationship.femaleIndex = femaleIndex;
                relationship.terminatedIndex = dataReader.GetInt32(1);
                if (dataReader.IsDBNull(2))
                {
                    relationship.start.status = CompoundDate.EMPTY;
                }
                else
                {
                    relationship.start.date = dataReader.GetDateTime(2);
                    relationship.start.status = dataReader.GetInt16(3);
                }
                if (dataReader.IsDBNull(4))
                {
                    relationship.end.status = CompoundDate.EMPTY;
                }
                else
                {
                    relationship.end.date = dataReader.GetDateTime(4);
                    relationship.end.status = dataReader.GetInt16(5);
                }
                if (dataReader.IsDBNull(6))
                {
                    relationship.location = "";
                }
                else
                {
                    relationship.location = dataReader.GetString(6);
                }
                if (dataReader.IsDBNull(7))
                {
                    relationship.comments = "";
                }
                else
                {
                    relationship.comments = dataReader.GetString(7);
                }
                relationship.typeIndex = dataReader.GetInt16(8);

                if (dataReader.IsDBNull(9))
                {
                    relationship.lastEditBy = "Steve Walton";
                }
                else
                {
                    relationship.lastEditBy = dataReader.GetString(9);
                }
                if (dataReader.IsDBNull(10))
                {
                    relationship.lastEditDate = DateTime.Now;
                }
                else
                {
                    relationship.lastEditDate = dataReader.GetDateTime(10);
                }
            }
            dataReader.Close();

            // Return the relationship.
            return relationship;
        }



        #endregion

        #region Sources

        // Returns all the repositories as an array of IDName pairs.
        /// <summary>
        /// Returns all the repositories as an array of IDName pairs.
        /// This can be used directly in combo boxes etc ...
        /// </summary>
        /// <returns>An array of clsIDName objects.</returns>
        public IndexName[] GetRepositories()
        {
            // Create a list of repositories
            ArrayList oRepositories = new ArrayList();

            // Open a list of repositories
            OleDbCommand oSql = new OleDbCommand("SELECT ID,Name FROM tbl_Repositories ORDER BY ID;", cndb_);
            OleDbDataReader drRepositories = oSql.ExecuteReader();
            while (drRepositories.Read())
            {
                IndexName oRepository = new IndexName(drRepositories.GetInt32(0), drRepositories.GetString(1));
                oRepositories.Add(oRepository);
            }
            drRepositories.Close();

            // Get the list of fact types
            return (IndexName[])(oRepositories.ToArray(typeof(IndexName)));
        }

        // Returns all the sources as an array of IDName pairs.
        /// <summary>
        /// Returns all the sources as an array of IDName pairs.
        /// This can be used directly in comboboxes etc
		/// </summary>
		/// <returns>An array of clsIDName objects</returns>
        public IndexName[] GetSources(SortOrder nOrder)
        {
            // Build a list of relivant facts
            ArrayList oSources = new ArrayList();

            // Open the list of fact types
            OleDbCommand oSQL;
            if (nOrder == SortOrder.DATE)
            {
                oSQL = new OleDbCommand("SELECT ID,Name,TheDate,TheDateStatusID FROM tbl_Sources ORDER BY LastUsed DESC;", cndb_);
            }
            else
            {
                oSQL = new OleDbCommand("SELECT ID,Name,TheDate,TheDateStatusID FROM tbl_Sources ORDER BY Name;", cndb_);
            }
            OleDbDataReader drSources = oSQL.ExecuteReader();
            while (drSources.Read())
            {
                string sName;
                if (drSources.IsDBNull(2))
                {
                    sName = drSources.GetString(1);
                }
                else
                {
                    sName = drSources.GetString(1) + " (" + drSources.GetDateTime(2).Year.ToString() + ")";
                }
                IndexName oSource = new IndexName(drSources.GetInt32(0), sName);
                oSources.Add(oSource);
            }
            drSources.Close();

            // Get the list of fact types
            return (IndexName[])(oSources.ToArray(typeof(IndexName)));
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
            if (nPrimaryID != 0)
            {
                oFile.WriteLine("1 OBJE @M" + nPrimaryID.ToString("0000") + "@");
            }

            // Now write any additional media objects do not repeat the primary media
            string sSql = "SELECT MediaID FROM tbl_AdditionalMediaForPeople WHERE PersonID=" + nPersonID.ToString() + ";";
            OleDbCommand oSql = new OleDbCommand(sSql, cndb_);
            OleDbDataReader drMedia = oSql.ExecuteReader();
            while (drMedia.Read())
            {
                int nMediaID = GetInt(drMedia, "MediaID", 0);
                if (nMediaID != 0 && nMediaID != nPrimaryID)
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
            OleDbCommand oSql = new OleDbCommand("SELECT * FROM tbl_Media ORDER BY ID;", cndb_);
            OleDbDataReader drMedia = oSql.ExecuteReader();
            while (drMedia.Read())
            {
                int nID = Database.GetInt(drMedia, "ID", 0);
                oFile.WriteLine("0 @M" + nID.ToString("0000") + "@ OBJE");
                string sFilename = Database.getString(drMedia, "Filename", "Undefined");
                oFile.WriteLine("1 FILE media/" + sFilename);
                oFile.WriteLine("2 FORM");
                oFile.WriteLine("3 TYPE photo");
                string sTitle = Database.getString(drMedia, "Title", "Undefined");
                oFile.WriteLine("2 TITL " + sTitle);
                bool bPrimary = drMedia.GetBoolean(drMedia.GetOrdinal("Primary"));
                if (bPrimary)
                {
                    oFile.WriteLine("1 _PRIM Y");
                }
                else
                {
                    oFile.WriteLine("1 _PRIM N");
                }
                bool bThumbnail = drMedia.GetBoolean(drMedia.GetOrdinal("Thumbnail"));
                if (bThumbnail)
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
            OleDbCommand oSql = new OleDbCommand("SELECT * FROM tbl_Repositories WHERE ID>0 ORDER BY ID;", cndb_);
            OleDbDataReader drRepositories = oSql.ExecuteReader();
            while (drRepositories.Read())
            {
                int nID = GetInt(drRepositories, "ID", 0);
                oFile.WriteLine("0 @R" + nID.ToString("0000") + "@ REPO");
                string sName = getString(drRepositories, "Name", "");
                oFile.WriteLine("1 NAME " + sName);
                string sAddress = getString(drRepositories, "Address", "");
                if (sAddress != "")
                {
                    GedcomMultiLine(oFile, 1, "ADDR", sAddress);
                }
                string sWebURL = getString(drRepositories, "WebURL", "");
                if (sWebURL != "")
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
        public bool WriteSourcesGedcom(StreamWriter oFile, funcVoid lpfnProgressBar, clsGedcomOptions oOptions)
        {
            // Select all the sources
            OleDbCommand oSql = new OleDbCommand("SELECT ID,Name,TheDate,TheDateStatusID,Comments,AdditionalInfoTypeID,Gedcom,RepositoryID FROM tbl_Sources ORDER BY ID;", cndb_);
            OleDbDataReader drSources = oSql.ExecuteReader();
            while (drSources.Read())
            {
                if (GetBool(drSources, "Gedcom", true))
                {
                    int nID = drSources.GetInt32(0);
                    oFile.WriteLine("0 @S" + nID.ToString("0000") + "@ SOUR");
                    string sName = drSources.GetString(1);
                    oFile.WriteLine("1 TITL " + sName);
                    if (!drSources.IsDBNull(2))
                    {
                        CompoundDate oDate = new CompoundDate();
                        oDate.date = drSources.GetDateTime(2);
                        oDate.status = drSources.GetInt32(3);
                        oFile.WriteLine("2 DATE " + oDate.format(DateFormat.GEDCOM));
                    }

                    // Additional Information for the source
                    if (!drSources.IsDBNull(5))
                    {
                        switch (drSources.GetInt32(5))
                        {
                        case 1: // Birth Certifcate
                            SourceBirthCertificate(oFile, nID);
                            break;
                        case 2: // Marriage Certifcate
                            SourceMarriageCertificate(oFile, nID, oOptions);
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
                    if (!drSources.IsDBNull(4))
                    {
                        GedcomMultiLine(oFile, 1, "NOTE", drSources.GetString(4));
                    }

                    // The repository for this source
                    int nRepositoryID = Database.GetInt(drSources, "RepositoryID", 0);
                    if (nRepositoryID > 0)
                    {
                        oFile.WriteLine("1 REPO @R" + nRepositoryID.ToString("0000") + "@");
                    }
                }

                if (lpfnProgressBar != null)
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
            OleDbConnection cnDb = new OleDbConnection(cndb_.ConnectionString);
            cnDb.Open();

            // Create a birth certificate object
            clsBirthCertificate oBirth = new clsBirthCertificate(nID, cnDb);

            // Close the database
            cnDb.Close();

            // Write the details from the birth certificate
            bool bFirst = true;
            if (oBirth.registrationDistrict != "")
            {
                // oFile.WriteLine("2 PLAC "+oBirth.RegistrationDistrict);
                GedcomLongNote(ref bFirst, oFile, "Registration District: " + oBirth.registrationDistrict);
            }
            if (oBirth.whenAndWhere != "")
            {
                GedcomLongNote(ref bFirst, oFile, "When and Where: " + oBirth.when.ToString("d MMM yyyy") + oBirth.whenAndWhere);
            }
            if (oBirth.name != "")
            {
                GedcomLongNote(ref bFirst, oFile, "Name: " + oBirth.name + " (" + oBirth.sex + ")");
            }
            if (oBirth.mother != "")
            {
                GedcomLongNote(ref bFirst, oFile, "Mother: " + oBirth.mother);
            }
            if (oBirth.father != "")
            {
                GedcomLongNote(ref bFirst, oFile, "Father: " + oBirth.father + " (" + oBirth.fatherOccupation + ")");
            }
            if (oBirth.informant != "")
            {
                GedcomLongNote(ref bFirst, oFile, "Informant: " + oBirth.informant);
            }
            if (oBirth.whenRegistered != "")
            {
                GedcomLongNote(ref bFirst, oFile, "When Registered: " + oBirth.whenRegistered);
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
            OleDbConnection cnDb = new OleDbConnection(cndb_.ConnectionString);
            cnDb.Open();

            // Create a Marriage Certificate object
            clsMarriageCertificate oMarriage = new clsMarriageCertificate(nSourceID, cnDb);

            // Close the database
            cnDb.Close();

            // Write the details of the marriage certificate
            StringBuilder sbText;
            writeGedcomPlace(oFile, 2, oMarriage.location, oOptions);
            bool bFirst = true;
            if (oMarriage.groomName != "")
            {
                sbText = new StringBuilder();
                sbText.Append(oMarriage.groomName);
                if (oMarriage.groomAge != "")
                {
                    sbText.Append(" (" + oMarriage.groomAge + ")");
                }
                if (oMarriage.groomOccupation != "")
                {
                    sbText.Append(" - " + oMarriage.groomOccupation);
                }
                if (oMarriage.groomLiving != "")
                {
                    sbText.Append(" - " + oMarriage.groomLiving);
                }
                GedcomLongNote(ref bFirst, oFile, "Groom: " + sbText.ToString());
            }
            if (oMarriage.brideName != "")
            {
                sbText = new StringBuilder();
                sbText.Append(oMarriage.brideName);
                if (oMarriage.brideAge != "")
                {
                    sbText.Append(" (" + oMarriage.brideAge + ")");
                }
                if (oMarriage.brideOccupation != "")
                {
                    sbText.Append(" - " + oMarriage.brideOccupation);
                }
                if (oMarriage.brideLiving != "")
                {
                    sbText.Append(" - " + oMarriage.brideLiving);
                }
                GedcomLongNote(ref bFirst, oFile, "Bride: " + sbText.ToString());
            }
            if (oMarriage.groomFather != "")
            {
                sbText = new StringBuilder(oMarriage.groomFather);
                if (oMarriage.groomFatherOccupation != "")
                {
                    sbText.Append(" - " + oMarriage.groomFatherOccupation);
                }
                GedcomLongNote(ref bFirst, oFile, "Groom's Father: " + sbText.ToString());
            }
            if (oMarriage.brideFather != "")
            {
                sbText = new StringBuilder(oMarriage.brideFather);
                if (oMarriage.brideFatherOccupation != "")
                {
                    sbText.Append(" - " + oMarriage.brideFatherOccupation);
                }
                GedcomLongNote(ref bFirst, oFile, "Bride's Father: " + sbText.ToString());
            }
            if (oMarriage.witness != "")
            {
                GedcomLongNote(ref bFirst, oFile, "Witness: " + oMarriage.witness);
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
            OleDbConnection cnDb = new OleDbConnection(cndb_.ConnectionString);
            cnDb.Open();

            // Create a Marriage Certificate object
            clsDeathCertificate oDeath = new clsDeathCertificate(nID, cnDb);

            // Close the database
            cnDb.Close();

            // Write the details of the marriage certificate
            bool bFirst = true;
            if (oDeath.RegistrationDistrict != "")
            {
                // oFile.WriteLine("2 PLAC "+oDeath.RegistrationDistrict);
                GedcomLongNote(ref bFirst, oFile, "Registration District: " + oDeath.RegistrationDistrict);
            }
            if (oDeath.When != "")
            {
                GedcomLongNote(ref bFirst, oFile, "When: " + oDeath.When);
            }
            if (oDeath.Place != "")
            {
                GedcomLongNote(ref bFirst, oFile, "Where: " + oDeath.Place);
            }
            if (oDeath.Name != "")
            {
                GedcomLongNote(ref bFirst, oFile, "Name: " + oDeath.Name + " (" + oDeath.Sex + ")");
            }
            if (oDeath.DatePlaceOfBirth != "")
            {
                GedcomLongNote(ref bFirst, oFile, "Date & Place of Birth: " + oDeath.DatePlaceOfBirth);
            }
            if (oDeath.Occupation != "")
            {
                GedcomLongNote(ref bFirst, oFile, "Occupation: " + oDeath.Occupation);
            }
            if (oDeath.UsualAddress != "")
            {
                GedcomLongNote(ref bFirst, oFile, "Usual Address: " + oDeath.UsualAddress);
            }
            if (oDeath.CauseOfDeath != "")
            {
                GedcomLongNote(ref bFirst, oFile, "Cause of Death: " + oDeath.CauseOfDeath);
            }
            if (oDeath.Informant != "")
            {
                if (oDeath.InformantDescription == "")
                {
                    GedcomLongNote(ref bFirst, oFile, "Informant: " + oDeath.Informant);
                }
                else
                {
                    GedcomLongNote(ref bFirst, oFile, "Informant: " + oDeath.Informant + " (" + oDeath.InformantDescription + ")");
                }
            }
            if (oDeath.InformantAddress != "")
            {
                GedcomLongNote(ref bFirst, oFile, "Informant Address: " + oDeath.InformantAddress);
            }
            if (oDeath.WhenRegistered != "")
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
            OleDbConnection cnDb = new OleDbConnection(cndb_.ConnectionString);
            cnDb.Open();

            // Write the information from the census header
            string sSql = "SELECT Address FROM tbl_CensusHouseholds WHERE ID=" + nCensusHouseholdID.ToString() + ";";
            OleDbCommand oSql = new OleDbCommand(sSql, cnDb);
            object oAddress = oSql.ExecuteScalar();

            // Check that an address is specified.  If the address is not present then the record probably does not exist.
            if (oAddress != null)
            {
                string sAddress = oAddress.ToString();
                writeGedcomPlace(oFile, 2, sAddress, oOptions);

                // Write the information about the members of this census record
                sSql = "SELECT NameGiven, Age, RelationToHead, Occupation, BornLocation FROM tbl_CensusPeople WHERE HouseHoldID=" + nCensusHouseholdID.ToString() + " ORDER BY ID;";
                oSql = new OleDbCommand(sSql, cnDb);
                OleDbDataReader drMembers = oSql.ExecuteReader();
                bool bFirst = true;
                while (drMembers.Read())
                {
                    string sName = getString(drMembers, "NameGiven", "");
                    string sAge = getString(drMembers, "Age", "");
                    string sRelation = getString(drMembers, "RelationToHead", "");
                    string sOccupation = getString(drMembers, "Occupation", "");
                    string sBorn = getString(drMembers, "BornLocation", "");

                    StringBuilder sbMember = new StringBuilder();
                    sbMember.Append(sName);
                    if (sAge != "")
                    {
                        sbMember.Append(" (" + sAge + ")");
                    }
                    if (sRelation != "")
                    {
                        sbMember.Append(" - " + sRelation);
                    }
                    if (sOccupation != "")
                    {
                        sbMember.Append(" - " + sOccupation);
                    }
                    if (sBorn != "")
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
            if (nLineBreak > 0)
            {
                if (sTag == "CONT")
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
            if (nLineBreak > 0)
            {
                GedcomLongNote(ref bFirst, oFile, sMessage.Substring(0, nLineBreak - 1));
                GedcomLongNote(ref bFirst, oFile, sMessage.Substring(nLineBreak + 1));
            }
            else
            {
                if (bFirst)
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
		public IndexName[] GetSourceAdditionalTypes()
        {
            // Build a list of additional information types
            ArrayList oTypes = new ArrayList();

            // Open the list from the database
            OleDbCommand oSql = new OleDbCommand("SELECT ID,Name FROM tlk_AdditionalInfoTypes ORDER BY ID;", cndb_);
            OleDbDataReader drTypes = oSql.ExecuteReader();
            while (drTypes.Read())
            {

                IndexName oType = new IndexName(drTypes.GetInt32(0), drTypes.GetString(1));
                oTypes.Add(oType);
            }
            drTypes.Close();

            // Get the list of fact types
            return (IndexName[])(oTypes.ToArray(typeof(IndexName)));
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
            if (factTypes_ == null)
            {
                LoadFactTypes();
            }

            // Return the array of fact types
            return factTypes_;
        }

        /// <summary>
        /// Returns a clsFactType object with the specified ID or null if no matching object can be found.
        /// </summary>
        /// <param name="nID">Specifies the ID of the fact type object required.</param>
        /// <returns>A clsFactType object or null.</returns>
        public clsFactType GetFactType
            (
            int nID
            )
        {
            // Open the fact types (if required)
            if (factTypes_ == null)
            {
                LoadFactTypes();
            }

            // Return a matching fact type (if possible)
            for (int nI = 0; nI < factTypes_.Length; nI++)
            {
                if (factTypes_[nI].ID == nID)
                {
                    return factTypes_[nI];
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
            OleDbCommand oSQL = new OleDbCommand("SELECT ID,Name FROM tlk_FactTypes ORDER BY ID;", cndb_);
            OleDbDataReader drFactTypes = oSQL.ExecuteReader();
            while (drFactTypes.Read())
            {
                clsFactType oFactType = new clsFactType(drFactTypes.GetInt32(0), drFactTypes.GetString(1));
                oFactTypes.Add(oFactType);
            }
            drFactTypes.Close();

            // Get the list of fact types.
            factTypes_ = (clsFactType[])(oFactTypes.ToArray(typeof(clsFactType)));

            // Return success.
            return true;
        }

        #endregion

        #region Census



        /// <summary>Return all the households in the specified year.  This is intended to populate combo box etc...</summary>
        /// <param name="theYear">Specify the year to return the households of.</param>
        /// <returns>An array of ID and names of house records.</returns>
        public IndexName[] cenusGetHouseholds(int theYear)
        {
            // Build a list of relivant facts.
            ArrayList houseHolds = new ArrayList();

            string sql = "SELECT ID, Address FROM tbl_CensusHouseholds WHERE Year(CensusDate)=" + theYear.ToString() + " ORDER BY Address;";
            OleDbCommand sqlCommand = new OleDbCommand(sql, cndb_);
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                IndexName houseHold = new IndexName(dataReader.GetInt32(0), dataReader.GetString(1));
                houseHolds.Add(houseHold);
            }
            dataReader.Close();

            // Return the households found.
            return (IndexName[])houseHolds.ToArray(typeof(IndexName));
        }



        /// <summary>Returns an array clsCensusPerson objects representing the member of the specified census household.</summary>
		/// <param name="houseHoldIndex">Specifies the ID of the census household.</param>
		/// <returns>An array of clsCensusPerson objects representing the members of the specified census household.</returns>
        public clsCensusPerson[] censusHouseholdMembers(int houseHoldIndex)
        {
            string sql = "SELECT tbl_CensusPeople.*, tbl_People.Forenames+' '+ Iif(IsNull(tbl_People.MaidenName),tbl_People.Surname, tbl_People.MaidenName) AS Name, tbl_CensusHouseholds.Address, tbl_CensusHouseholds.CensusDate " +
                "FROM (tbl_CensusPeople LEFT JOIN tbl_People ON tbl_CensusPeople.PersonID = tbl_People.ID) INNER JOIN tbl_CensusHouseholds ON tbl_CensusPeople.HouseHoldID = tbl_CensusHouseholds.ID " +
                "WHERE (((tbl_CensusPeople.HouseHoldID)=" + houseHoldIndex.ToString() + ")) " +
                "ORDER BY tbl_CensusPeople.ID;";
            return censusGetRecords(sql);
        }



        /// <summary>Returns an array of clsCensusPerson objects as specified in the Sql command.</summary>
		/// <param name="sql">Specifies a Sql command to fetch a collection of census members.</param>
		/// <returns>An array of clsCensusPerson objects as specified in the Sql command.</returns>
        private clsCensusPerson[] censusGetRecords(string sql)
        {
            // Build a list of household members
            ArrayList members = new ArrayList();

            OleDbCommand sqlCommand = new OleDbCommand(sql, cndb_);
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                clsCensusPerson censusPerson = new clsCensusPerson();
                censusPerson.index = dataReader.GetInt32(0);
                censusPerson.houseHoldIndex = dataReader.GetInt32(1);
                if (!dataReader.IsDBNull(2))
                {
                    censusPerson.personIndex = dataReader.GetInt32(2);
                }
                if (!dataReader.IsDBNull(3))
                {
                    censusPerson.censusName = dataReader.GetString(3);
                }
                if (!dataReader.IsDBNull(4))
                {
                    censusPerson.relationToHead = dataReader.GetString(4);
                }
                if (!dataReader.IsDBNull(5))
                {
                    censusPerson.age = dataReader.GetString(5);
                }
                if (!dataReader.IsDBNull(6))
                {
                    censusPerson.occupation = dataReader.GetString(6);
                }
                if (!dataReader.IsDBNull(7))
                {
                    censusPerson.bornLocation = dataReader.GetString(7);
                }
                if (!dataReader.IsDBNull(8))
                {
                    censusPerson.personName = dataReader.GetString(8);
                }
                censusPerson.houseHoldName = dataReader.GetString(9);
                censusPerson.date = dataReader.GetDateTime(10);

                members.Add(censusPerson);
            }
            dataReader.Close();

            // Return the members found.
            return (clsCensusPerson[])members.ToArray(typeof(clsCensusPerson));
        }



        /// <summary>Writes the specified clsCensusPerson record to the database.</summary>
		/// <param name="censusPerson">Specifies the clsCensusPerson record to write to the database.</param>
		/// <returns>True for success, false otherwise.</returns>
        public bool censusSavePerson(clsCensusPerson censusPerson)
        {
            // Delete this person if required.
            if (!censusPerson.isValid())
            {
                if (censusPerson.index != 0)
                {
                    OleDbCommand sqlCommand = new OleDbCommand("DELETE FROM tbl_CensusPeople WHERE ID=" + censusPerson.index.ToString() + ";", cndb_);
                    sqlCommand.ExecuteNonQuery();
                }
                return true;
            }

            // Create a new record if required.
            if (censusPerson.index == 0)
            {
                string sql = "SELECT MAX(ID) AS MaxID FROM tbl_CensusPeople;";
                OleDbCommand sqlCommand = new OleDbCommand(sql, cndb_);
                censusPerson.index = int.Parse(sqlCommand.ExecuteScalar().ToString()) + 1;

                // Create a new record.
                sql = "INSERT INTO tbl_CensusPeople (ID,HouseholdID) VALUES (" + censusPerson.index.ToString() + "," + censusPerson.houseHoldIndex.ToString() + ");";
                sqlCommand = new OleDbCommand(sql, cndb_);
                sqlCommand.ExecuteNonQuery();
            }

            // Update the record.
            StringBuilder updateSql = new StringBuilder();
            updateSql.Append("UPDATE tbl_CensusPeople SET ");
            updateSql.Append("PersonID=" + censusPerson.personIndex.ToString() + ",");
            updateSql.Append("NameGiven=" + toDb(censusPerson.censusName) + ",");
            updateSql.Append("RelationToHead=" + toDb(censusPerson.relationToHead) + ",");
            updateSql.Append("Age=" + toDb(censusPerson.age) + ",");
            updateSql.Append("Occupation=" + toDb(censusPerson.occupation) + ",");
            updateSql.Append("BornLocation=" + toDb(censusPerson.bornLocation) + " ");
            updateSql.Append("WHERE ID=" + censusPerson.index.ToString() + ";");
            OleDbCommand updateCommand = new OleDbCommand(updateSql.ToString(), cndb_);
            updateCommand.ExecuteNonQuery();

            // Return success.
            return true;
        }



        /// <summary>Returns an array of census records that contain the specified person.</summary>
		/// <param name="personIndex"></param>
		/// <returns></returns>
        public clsCensusPerson[] censusForPerson(int personIndex)
        {
            string sSql = "SELECT tbl_CensusPeople.*, tbl_People.Forenames+' '+ Iif(IsNull(tbl_People.MaidenName),tbl_People.Surname, tbl_People.MaidenName) AS Name, tbl_CensusHouseholds.Address, tbl_CensusHouseholds.CensusDate " +
                "FROM (tbl_CensusPeople LEFT JOIN tbl_People ON tbl_CensusPeople.PersonID = tbl_People.ID) INNER JOIN tbl_CensusHouseholds ON tbl_CensusPeople.HouseHoldID = tbl_CensusHouseholds.ID " +
                "WHERE tbl_CensusPeople.PersonID=" + personIndex.ToString() + " " +
                "ORDER BY tbl_CensusHouseholds.CensusDate;";
            return censusGetRecords(sSql);
        }



        /// <summary>Returns a human readable string representing the people that the specified person is living with according to the census record.</summary>
		/// <param name="censusPerson">Specifies the person who should not be mentioned in the returned description.</param>
		/// <returns>A human readable string representing the people that the specified person is living with according to the census record.</returns>
        public string censusLivingWith(clsCensusPerson censusPerson)
        {
            StringBuilder livingWith = new StringBuilder();
            string sql = "SELECT NameGiven, Age FROM tbl_CensusPeople WHERE HouseHoldID=" + censusPerson.houseHoldIndex.ToString() + " AND PersonID<>" + censusPerson.personIndex.ToString() + " ORDER BY tbl_CensusPeople.ID;";
            OleDbCommand sqlCommand = new OleDbCommand(sql, cndb_);
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                if (livingWith.Length > 0)
                {
                    livingWith.Append(", ");
                }
                livingWith.Append(dataReader.GetString(0));
                if (!dataReader.IsDBNull(1))
                {
                    livingWith.Append(" (" + dataReader.GetString(1) + ")");
                }
            }
            dataReader.Close();

            livingWith.Insert(0, "Living with ");

            // Return the built string.
            return livingWith.ToString();
        }

        #endregion

        #region Places



        /// <summary>Remove any digits or spaces from the front of the specified place name.</summary>
        /// <param name="placeName">Specifies the place name to clean up.</param>
        /// <returns>A clean version of the place name.</returns>
        private string cleanPlaceName(string placeName)
        {
            // Check that the input string is valid.
            if (placeName.Length == 0)
            {
                return "";
            }

            // Remove digits and spaces from the front of the place name.
            int nI = 0;
            while (placeName[nI] == ' ' || (placeName[nI] >= '0' && placeName[nI] <= '9'))
            {
                nI++;
            }
            return placeName.Substring(nI);
        }



        /// <summary>Returns the place object with the specified compound name under the specified parent.</summary>
        /// <param name="placeName">Specifies the compound name of the place.</param>
        /// <param name="parentIndex">Specifies the index of the parent place of the compound name.</param>
        /// <returns>The place object or null.</returns>
        private clsPlace getPlace(string placeName, int parentIndex)
        {
            // Get the head and tail.
            string head = placeName.Trim();
            string tail = string.Empty;
            int comma = placeName.LastIndexOf(',');
            if (comma > 0)
            {
                head = placeName.Substring(comma + 1).Trim();
                tail = placeName.Substring(0, comma).Trim();
            }

            // Get the ID of this place.
            int placeIndex = getPlaceIndex(head, parentIndex);
            if (placeIndex == 0)
            {
                return null;
            }

            // Return this place if at the end of the string.
            if (tail == string.Empty)
            {
                return new clsPlace(placeIndex, this);
            }

            // Search further down the string.
            return getPlace(tail, placeIndex);
        }



        /// <summary>Returns the place object that represents the specified compound place name.</summary>
        /// <param name="placeName">Specifies the name of the place.  Eg. Leeds, Yorkshire, England.</param>
        /// <returns>The place object or null.</returns>
        public clsPlace GetPlace(string placeName)
        {
            return getPlace(placeName, 0);
        }



        /// <summary>Returns the ID of the required place.  Returns 0 if the required place does not exist.</summary>
        /// <param name="placeName">Specifies the name of the place.  This is not the compound name with , characters.</param>
        /// <param name="parentIndex">Specifies the parent of the place.</param>
        /// <returns>The ID of the requested place.  0 if no matching place can be found.</returns>
        private int getPlaceIndex(string placeName, int parentIndex)
        {
            // Clean the place name.
            placeName = cleanPlaceName(placeName);

            // Look for this place in the database.
            string sql = "SELECT ID FROM tbl_Places WHERE Name = " + Innoval.clsDatabase.ToDb(placeName) + " AND ParentID = " + parentIndex.ToString() + ";";
            OleDbCommand sqlCommand = new OleDbCommand(sql, cndb_);
            int placeIndex = 0;
            try
            {
                placeIndex = int.Parse(sqlCommand.ExecuteScalar().ToString());
            }
            catch { }

            // Return the ID found.
            return placeIndex;
        }



        /// <summary>Adds a compound place to the database.  This might result in a number of place records in the tbl_Places table.  It will create a link from the reference object to add the related tbl_Places records.</summary>
        /// <param name="placeName">Specifies the compound place separated by , and starting at the top level.</param>
        /// <param name="objectTypeIndex">Specifies the type of object that created this compound place.</param>
        /// <param name="objectIndex">Specifies the ID of the reference object.</param>
        /// <returns>True for success, false otherwise.</returns>
        public bool addPlace(string placeName, int objectTypeIndex, int objectIndex)
        {
            // Add this place and link to the specified object.
            return addPlace(placeName, objectTypeIndex, objectIndex, 0, 0);
        }



        /// <summary>Adds a compound place to the database.  This results in a number of place records in the tbl_Places table.</summary>
        /// <param name="placeName">Specifies the compound place separated by , but not nessercary starting with the top level.</param>
        /// <param name="objectTypeIndex">Specifies the type of object that created this compound place.</param>
        /// <param name="objectIndex">Specifies the ID of the reference object.</param>
        /// <param name="parentIndex">Specifies the ID of the place that is the parent above the compound place string.</param>
        /// <param name="level">Specifies the level from the top level.</param>
        /// <returns></returns>
        private bool addPlace(string placeName, int objectTypeIndex, int objectIndex, int parentIndex, int level)
        {
            // Validate the inputs.
            if (placeName == null)
            {
                return false;
            }

            // Split the place into a list of places.
            string head = placeName.Trim();
            string tail = "";
            int comma = placeName.LastIndexOf(',');
            if (comma > 0)
            {
                head = placeName.Substring(comma + 1).Trim();
                tail = placeName.Substring(0, comma).Trim();
            }

            // Get the ID of this place.
            int placeIndex = getPlaceIndex(head, parentIndex);

            // Add this place (if required).
            if (placeIndex == 0)
            {
                placeIndex = addPlace(parentIndex, head);
            }

            // Link the object to this place.
            string sql = "INSERT INTO tbl_ToPlaces (PlaceID, TypeID, ObjectID) VALUES(" + placeIndex.ToString() + ", " + objectTypeIndex.ToString() + ", " + objectIndex.ToString() + ");";
            OleDbCommand sqlCommand = new OleDbCommand(sql, cndb_);
            try
            {
                sqlCommand.ExecuteNonQuery();
            }
            catch { }

            // Deal with the tail if any.
            if (tail != "")
            {
                addPlace(tail, objectTypeIndex, objectIndex, placeIndex, level + 1);
            }

            // Return success.
            return true;
        }



        /// <summary>Add the specified place to the database.</summary>
        /// <param name="parentIndex">Specifies the ID of the parent place.</param>
        /// <param name="placeName">Specifies the name of the place.</param>
        /// <returns>True for success, false otherwise.</returns>
        private int addPlace(int parentIndex, string placeName)
        {
            // Find the ID of the next available place record.
            string sql = "SELECT MAX(ID) AS NewID FROM tbl_Places;";
            OleDbCommand sqlCommand = new OleDbCommand(sql, cndb_);
            int placeIndex = 0;
            try
            {
                placeIndex = int.Parse(sqlCommand.ExecuteScalar().ToString());
            }
            catch { }
            placeIndex++;

            // Clean the name.
            placeName = cleanPlaceName(placeName);

            // Insert the new place.
            sql = "INSERT INTO tbl_Places (ID, Name, ParentID, Status) VALUES (" + placeIndex.ToString() + ", \"" + placeName + "\", " + parentIndex.ToString() + ", 0);";
            sqlCommand = new OleDbCommand(sql, cndb_);
            sqlCommand.ExecuteNonQuery();

            // Return the ID of this new place.
            return placeIndex;
        }



        /// <summary>Returns the specified compound place string in html format.  Each place in the compound place with have a html link.</summary>
        /// <param name="placeName">Specifies the compound place string.</param>
        /// <param name="parentIndex">Specifies the ID of the place above the compound place string.</param>
        /// <returns>A html string to represent the specified compound place.</returns>
        private string placeToHtml(string placeName, int parentIndex)
        {
            // Get the head and tail.
            string head = placeName.Trim();
            string tail = "";
            int comma = placeName.LastIndexOf(',');
            if (comma > 0)
            {
                head = placeName.Substring(comma + 1).Trim();
                tail = placeName.Substring(0, comma).Trim();
            }

            // Get the ID of this place.
            int placeIndex = getPlaceIndex(head, parentIndex);

            // Can not encode this place.
            if (placeIndex == 0)
            {
                if (tail == "")
                {
                    return head;
                }
                return tail + ", " + head;
            }

            // Encode this place into linked html.
            string placeHtml = "<a href=\"place:" + placeIndex.ToString() + "\">" + head + "</a>";

            // Deal with the tail.
            if (tail != "")
            {
                return placeToHtml(tail, placeIndex) + ", " + placeHtml;
            }

            // Return the place in html format.
            return placeHtml;
        }



        /// <summary>Returns the specified compound place string in html format.</summary>
        /// <param name="placeName">Specifies the full compound place string including the top level.</param>
        /// <returns>A html string to represent the specified compound place string.</returns>
        public string placeToHtml(string placeName)
        {
            return placeToHtml(placeName, 0);
        }



        /// <summary>Returns the specified compound place string in Gedcom format.  The full compound place will not be returned.  The portion of the compound place will depend if a place record or an address record is required.</summary>
        /// <param name="placeName">Specifies the compound location string.</param>
        /// <param name="status">Specify 0 to use the place in a PLAC record.  Specify 1 to use the place in an ADDR record.</param>
        /// <param name="parentIndex">Specifies the ID parent place above the compound location string.</param>
        /// <returns>A string to represent the place in a gedcom file.</returns>
        private string placeToGedcom(string placeName, int status, int parentIndex)
        {
            // Get the head and tail.
            string head = placeName.Trim();
            string tail = "";
            int comma = placeName.LastIndexOf(',');
            if (comma > 0)
            {
                head = placeName.Substring(comma + 1).Trim();
                tail = placeName.Substring(0, comma).Trim();
            }

            // Get the ID of this place.
            int placeIndex = getPlaceIndex(head, parentIndex);

            // Can not encode this place.
            if (placeIndex == 0)
            {
                return "";
            }

            // Get the status of this place.
            string sql = "SELECT Status FROM tbl_Places WHERE ID = " + placeIndex.ToString() + ";";
            OleDbCommand sqlCommand = new OleDbCommand(sql, cndb_);
            int placeStatus = 0;
            try
            {
                placeStatus = int.Parse(sqlCommand.ExecuteScalar().ToString());
            }
            catch { }

            // Deal with the tail.
            if (tail != "")
            {
                tail = placeToGedcom(tail, status, placeIndex);
            }

            // If the status is correct then return the head.
            if (placeStatus == status)
            {
                if (tail != "")
                {
                    return tail + ", " + head;
                }
                return head;
            }
            else
            {
                if (tail != "")
                {
                    return tail;
                }
            }

            // Nothing.
            return "";
        }



        /// <summary>Returns the specified compound place string in gedcom format.  The full compound place will not be returned.  The portion of the compound place will depend if a place record or an address record is required.</summary>
        /// <param name="placeName">Specifies the full compound location string.</param>
        /// <param name="status">Specify 0 to use the place in a PLAC record.  Specify 1 to use the place in an ADDR record.</param>
        /// <returns>A string to represent the place in a gedcom file.</returns>
        public string placeToGedcom(string placeName, int status)
        {
            return placeToGedcom(placeName, status, 0);
        }



        /// <summary>Write the gedcom PLAC record.  This can also include optional ADDR, MAP, and CTRY records.  Originally this was only PLAC and ADDR records.</summary>
        /// <param name="file">Specifies the file to write the PLAC and ADDR records into.</param>
        /// <param name="level">Specifies the level to write the records at.</param>
        /// <param name="fullPlace">Specifies the database place record to be decoded into gedcom PLAC and ADDR records.</param>
        /// <param name="options">Specifies the Gedcom options to apply to this place.</param>
        /// <returns>True for success, false otherwise.</returns>
        public bool writeGedcomPlace(StreamWriter file, int level, string fullPlace, clsGedcomOptions options)
        {
            // Optionally split the address off from the PLAC tag.
            if (options.RemoveADDRfromPLAC)
            {
                string placeName = placeToGedcom(fullPlace, 0);
                if (placeName != "")
                {
                    file.WriteLine(level.ToString() + " PLAC " + placeName);
                }
            }
            else
            {
                if (fullPlace != "")
                {
                    file.WriteLine(level.ToString() + " PLAC " + fullPlace);
                }
            }

            // Really expect that some tag was created above.
            // If not then the level should not increase.
            // PhpGedView did not increase the level originally.

            // Add the optional MAP tag with longitude and latitude.
            if (options.UseLongitude)
            {
                clsPlace place = GetPlace(fullPlace);
                if (place != null)
                {
                    file.WriteLine((level + 1).ToString() + " MAP");
                    if (place.Latitude >= 0)
                    {
                        file.WriteLine((level + 2).ToString() + " LATI N" + place.Latitude.ToString());
                    }
                    else
                    {
                        file.WriteLine((level + 2).ToString() + " LATI S" + Math.Abs(place.Latitude).ToString());
                    }
                    if (place.Longitude > 0)
                    {
                        file.WriteLine((level + 2).ToString() + " LONG E" + place.Longitude.ToString());
                    }
                    else
                    {
                        file.WriteLine((level + 2).ToString() + " LONG W" + Math.Abs(place.Longitude).ToString());
                    }
                }
            }

            // Optionally include an ADDR tag.
            if (options.UseADDR)
            {
                string address = placeToGedcom(fullPlace, 1);
                if (address != "")
                {
                    file.WriteLine((level + 1).ToString() + " ADDR " + address);
                }
            }

            // Include the optional CTRY (Country) tag.
            if (options.UseCTRY)
            {
                int comma = fullPlace.LastIndexOf(',');
                if (comma > 0)
                {
                    string country = fullPlace.Substring(comma + 2);
                    file.WriteLine((level + 1).ToString() + " CTRY " + country);
                }
            }

            // Return success.
            return true;
        }



        /// <summary>Removes all the place links for the specified object.</summary>
        /// <param name="objectTypeIndex">Specifies the type of object.</param>
        /// <param name="objectIndex">Specifies the ID of the object.</param>
        public void placeDelink(int objectTypeIndex, int objectIndex)
        {
            string sql = "DELETE FROM tbl_ToPlaces WHERE TypeID = " + objectTypeIndex.ToString() + " AND ObjectID = " + objectIndex.ToString() + ";";
            OleDbCommand sqlCommand = new OleDbCommand(sql, cndb_);
            try
            {
                sqlCommand.ExecuteNonQuery();
            }
            catch { }
        }



        /// <summary>Removes all the places that have no links to them.</summary>
        public int placeRemoveUnlinked()
        {
            // Get the list of places to delete.
            string sql = "SELECT tbl_Places.ID, Count(tbl_ToPlaces.TypeID) AS CountOfTypeID " +
                "FROM tbl_Places LEFT JOIN tbl_ToPlaces ON tbl_Places.ID = tbl_ToPlaces.PlaceID " +
                "GROUP BY tbl_Places.ID " +
                "HAVING (((Count(tbl_ToPlaces.TypeID))=0));";
            OleDbCommand sqlCommand = new OleDbCommand(sql, cndb_);
            ArrayList placesDelete = new ArrayList();
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                placesDelete.Add(dataReader.GetInt32(0));
            }
            dataReader.Close();

            // Delete the places.
            foreach (int placeIndex in placesDelete)
            {
                sql = "DELETE FROM tbl_Places WHERE ID = " + placeIndex.ToString() + ";";
                sqlCommand = new OleDbCommand(sql, cndb_);
                sqlCommand.ExecuteNonQuery();
            }

            // Return the number of places that are removed.
            return placesDelete.Count;
        }



        public clsPlace[] getPlaces(int placeIndex)
        {
            // Build a list to contain the places.
            ArrayList places = new ArrayList();

            // Build the list of child places.
            string sql = "SELECT * FROM tbl_Places WHERE ParentID = " + placeIndex.ToString() + " ORDER BY Name;";
            OleDbCommand sqlCommand = new OleDbCommand(sql, cndb_);
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                clsPlace place = new clsPlace(dataReader, this);
                places.Add(place);
            }
            dataReader.Close();

            // Return the list of places.
            return (clsPlace[])places.ToArray(typeof(clsPlace));
        }



        #endregion

        /// <summary>Returns a list of the editors on this database.</summary>
        /// <returns>A list of the editors on this database.</returns>
        public string[] getEditors()
        {
            // Open a dataset of editors
            string sql = "SELECT Name FROM tbl_Editors ORDER BY Name;";
            OleDbCommand sqlCommand = new OleDbCommand(sql, cndb_);
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();

            // Create an array list of the editors
            ArrayList editors = new ArrayList();
            while (dataReader.Read())
            {
                editors.Add(dataReader.GetString(0));
            }

            // Return the list of editors
            return (string[])editors.ToArray(typeof(string));
        }



        /// <summary>Returns a html description of the ToDo items.</summary>
        /// <returns>A html description of the ToDo items.</returns>
        public string getToDoAsHtml()
        {
            // Create a data adapter to load the information.
            string sql = "SELECT Description, Priority, PersonID, ID FROM tbl_ToDo ORDER BY Priority, ID;";
            OleDbCommand sqlCommand = new OleDbCommand(sql, cndb_);
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();

            // Create a html document to return.
            StringBuilder html = new StringBuilder();
            html.Append("<h1>To Do List</h1>");
            html.Append("<table cellpadding=\"3\" cellspacing=\"2\">");

            while (dataReader.Read())
            {
                html.Append("<tr bgcolor=\"silver\">");
                html.Append("<td><span class=\"Small\">");
                int nPersonID = GetInt(dataReader, "PersonID", 0);
                html.Append("<a href=\"person:" + nPersonID.ToString() + "\">");
                Person oPerson = new Person(nPersonID, this);
                html.Append(oPerson.getName(true, true));
                html.Append("</a>");
                html.Append("</span></td>");
                //sbHtml.Append("<td><span class=\"Small\">" + GetInt(drChanges,"Priority",0).ToString() + "</span></td>");
                //sbHtml.Append("<td><span class=\"Small\">" + GetString(drChanges,"Description","") + "</span></td>");
                html.Append("<td align=\"right\">" + GetInt(dataReader, "Priority", 0).ToString() + "</td>");
                html.Append("<td>" + getString(dataReader, "Description", "") + "</td>");
                html.Append("</tr>");
            }
            dataReader.Close();
            html.Append("</table>");

            // Return the html that has been built.
            return html.ToString();
        }



        /// <summary>Returns a html description of the recent changes.</summary>
		/// <returns>A html description of the recent changes.</returns>
		public string getRecentChangesAsHtml()
        {
            // Create a data adapter to load the information.
            string sql = "SELECT Format(LastEditDate, 'd-mmm-yyyy hh:mm:ss') AS EditDate,LastEditBy,Type,Name,ID FROM ("
                + "SELECT tbl_Relationships.LastEditDate, tbl_Relationships.LastEditBy, 'Relationship' AS Type, tbl_People.Forenames + ' ' + tbl_People.Surname + ' & ' + tbl_People_1.Forenames + ' ' + tbl_People_1.Surname AS Name, tbl_Relationships.ID "
                + "FROM (tbl_People INNER JOIN tbl_Relationships ON tbl_People.ID = tbl_Relationships.MaleID) INNER JOIN tbl_People AS tbl_People_1 ON tbl_Relationships.FemaleID = tbl_People_1.ID "
                + "UNION "
                + "SELECT LastEditDate, LastEditBy, 'Person' AS Type, Forenames +' '+Surname AS Name, ID FROM tbl_People "
                + ")"
                + "ORDER BY LastEditDate DESC;";
            OleDbCommand sqlCommand = new OleDbCommand(sql, cndb_);
            OleDbDataReader drChanges = sqlCommand.ExecuteReader();

            // Create a html document to return
            StringBuilder html = new StringBuilder();

            html.Append("<h1>Recent Changes</h1>");
            html.Append("<table cellpadding=\"3\" cellspacing=\"2\">");

            while (drChanges.Read())
            {
                html.Append("<tr bgcolor=\"silver\">");
                /*
                sbHtml.Append("<td><span class=\"Small\">" + GetString(drChanges,"EditDate","") + "</span></td>");
                sbHtml.Append("<td><span class=\"Small\">" + GetString(drChanges,"LastEditBy","") + "</span></td>");
                sbHtml.Append("<td><span class=\"Small\">" + GetString(drChanges,"Type","") + "</span></td>");
                sbHtml.Append("<td><span class=\"Small\">");
                 */
                html.Append("<td>" + getString(drChanges, "EditDate", "") + "</td>");
                html.Append("<td>" + getString(drChanges, "LastEditBy", "") + "</td>");
                html.Append("<td>" + getString(drChanges, "Type", "") + "</td>");
                html.Append("<td>");
                switch (getString(drChanges, "Type", ""))
                {
                case "Person":
                    html.Append("<a href=\"person:" + GetInt(drChanges, "ID", 0) + "\">");
                    break;
                }
                html.Append(getString(drChanges, "Name", ""));
                switch (getString(drChanges, "Type", ""))
                {
                case "Person":
                    html.Append("</a>");
                    break;
                }

                html.Append("</td>");
                html.Append("</tr>");
            }
            drChanges.Close();
            html.Append("</table>");

            // Return the html that has been built
            return html.ToString();
        }



        /// <summary>Returns the input string with Html linebreaks as required.</summary>
        /// <param name="text">Specifies the input string.</param>
        /// <returns>The specified input string with Html linebreaks.</returns>
        public static string htmlString(string text)
        {
            StringBuilder html = new StringBuilder(text);
            html.Replace("\n", "<br />");

            return html.ToString();
        }



        /// <summary>Returns the media directory for this database.</summary>
        /// <returns>The full path name of the directory that contains the media files for this database.</returns>
        // TODO: Make this into a database list and remember a value once it works.
        public string getMediaDirectory()
        {
            // Directory to use at home.
            if (Directory.Exists("\\\\qnap210\\waltons\\Family Tree\\Media"))
            {
                return "\\\\qnap210\\waltons\\Family Tree\\Media";
            }
            if (Directory.Exists("\\\\Qnap119\\waltonsdocuments\\Family Tree\\Media"))
            {
                return "\\\\Qnap119\\waltonsdocuments\\Family Tree\\Media";
            }

            // Directory to use on work network
            if (Directory.Exists("\\\\Innovalsrv21\\personal\\gramps\\Media"))
            {
                return "\\\\Innovalsrv21\\personal\\gramps\\Media";
            }

            // Directory to use on work laptop
            if (Directory.Exists("C:\\Documents and Settings\\waltons\\My Documents\\Local\\Steve Walton\\Family Tree Media"))
            {
                return "C:\\Documents and Settings\\waltons\\My Documents\\Local\\Steve Walton\\Family Tree Media";
            }

            return "";
        }



        #region Static helper functions



        /// <summary>Returns a string that can be inserted into a SQL command as the value for a string field.</summary>
        /// <param name="value">Specifies the value for a SQL string field.</param>
        /// <returns>A string that can inserted into a SQL command.</returns>
        public static string toDb(string value)
        {
            // Check for any empty string.
            if (value == null)
            {
                return "NULL";
            }
            if (value.Length == 0)
            {
                return "NULL";
            }

            // Remove any double quotes.
            if (value.Contains("\""))
            {
                // Replace all the double quotes with single quotes.
                value = value.Replace("\"", "'");
            }

            // Return the string.
            return "\"" + value + "\"";
        }



        /// <summary>Returns a string that can be inserted into a SQL command as the value for a date time field.</summary>
        /// <param name="value">Specifies the value for a SQL date/time field.</param>
        /// <returns>A string that can be inserted into a SQL command.</returns>
        public static string toDb(CompoundDate value)
        {
            // Check for a NULL date
            if (value.status == 15)
            {
                return "NULL";
            }

            // return the date
            return "#" + value.date.ToString("d-MMM-yyyy") + "#";
        }



        /// <summary>Returns a string that can be inserted into a Sql command as the value for a date field.</summary>
        /// <param name="value">Specifies the value for a Sql date field.</param>
        /// <returns>A string that can be inserted into a Sql command.</returns>
        public static string toDb(DateTime value)
        {
            return "#" + value.ToString("d-MMM-yyyy") + "#";
        }



        /// <summary>Returns a string that can be inserted into a SQL command as the value for a boolean field.  This is a 0 or 1.</summary>
		/// <param name="isValue">Specifies the value for a SQL boolean field.</param>
		/// <returns>A string that can be inserted into a SQL command.</returns>
		public static string toDb(bool isValue)
        {
            if (isValue)
            {
                return "1";
            }
            return "0";
        }



        /// <summary>Returns a string representing the specified integer in a SQL command.  Returns NULL if the specified integer matches the specified null code.</summary>
		/// <param name="value">Specifies the value to return.</param>
		/// <param name="nullValue">Specifies the value that represents NULL</param>
		/// <returns>A string to represent the specified integer in SQL command.</returns>
		public static string toDb(int value, int nullValue)
        {
            if (value == nullValue)
            {
                return "NULL";
            }
            return value.ToString();
        }



        /// <summary>Returns the specified string trueValue if the specified isCondition is true otherwise returns the specified string falseValue.</summary>
		/// <param name="isCondition">Specifies the condition to test.</param>
		/// <param name="trueValue">Specifies the string to return if the specified condition is true.</param>
		/// <param name="falseValue">Specifies the string to return if the specified condition is false.</param>
		/// <returns>Either the sTrue or sFalse dependant on bCondition.</returns>
		public static string iif(bool isCondition, string trueValue, string falseValue)
        {
            if (isCondition)
            {
                return trueValue;
            }
            else
            {
                return falseValue;
            }
        }



        /// <summary>Returns the string in a data reader column without raising an error.</summary>
		/// <param name="dataReader">Specifies the data reader to read from.</param>
		/// <param name="columnName">Specifies the label of the column to read.</param>
		/// <param name="nullValue">Specifies the value to return is the field is null.</param>
		/// <returns>The string value of the specified column.</returns>
		public static string getString(OleDbDataReader dataReader, string columnName, string nullValue)
        {
            int ordinal = dataReader.GetOrdinal(columnName);
            if (dataReader.IsDBNull(ordinal))
            {
                return nullValue;
            }
            return dataReader.GetString(ordinal);
        }



        /// <summary>Returns a fields from the specified data reader as a int without raising an error.</summary>
		/// <param name="dataReader">Specifies the data reader to read from.</param>
		/// <param name="columnName">Specifies the label of the column to read.</param>
		/// <param name="nullValue">Specifies the value to return if the field is null.</param>
		/// <returns>The int value of the specified column.</returns>
		public static int GetInt(OleDbDataReader dataReader, string columnName, int nullValue)
        {
            int ordinal = dataReader.GetOrdinal(columnName);
            if (dataReader.IsDBNull(ordinal))
            {
                return nullValue;
            }
            // Will need to switch on the actual type here.
            return dataReader.GetInt32(ordinal);
        }



        /// <summary>Returns a field from the specified data reader as a DateTime without raising an error.</summary>
		/// <param name="dataReader">Specifies the data reader to read from.</param>
		/// <param name="columnName">Specifies the label of the column to read.</param>
		/// <param name="nullValue">Specifies the value to return if the field is null.</param>
		/// <returns>The DateTime value of the specified column.</returns>
		public static DateTime getDateTime(OleDbDataReader dataReader, string columnName, DateTime nullValue)
        {
            int ordinal = dataReader.GetOrdinal(columnName);
            if (dataReader.IsDBNull(ordinal))
            {
                return nullValue;
            }
            return dataReader.GetDateTime(ordinal);
        }



        /// <summary>Returns the value of the specified field in the specified data reader as a boolean.</summary>
		/// <param name="dataReader">Specifies the data reader.</param>
		/// <param name="columnName">Specifies the name of the field.</param>
		/// <param name="nullValue">Specifies the value to return if the field is null.</param>
		/// <returns>The value of the specified field as a bool.</returns>
		public static bool GetBool(OleDbDataReader dataReader, string columnName, bool nullValue)
        {
            int ordinal = dataReader.GetOrdinal(columnName);
            if (dataReader.IsDBNull(ordinal))
            {
                return nullValue;
            }
            return dataReader.GetBoolean(ordinal);
        }



        #endregion

        #region Public Properties

        /// <summary>Connection to the database.</summary>
        internal OleDbConnection cndb { get { return cndb_; } }

        /// <summary>Range of differing ages to present people as possible marriage partners.</summary>
        public int relationshipRange { get { return marriedRange_; } set { marriedRange_ = value; } }

        /// <summary>The filename of the source database file.</summary>
        public string fileName { get { return fileName_; } }

        #endregion



        /// <summary>Writes SQL script into the specified file.</summary>
        /// <param name="fileName">Specifies the filename of the SQL Script file to create.</param>
        public void writeSql(string fileName)
        {
            // Open the output file.
            StreamWriter file = new StreamWriter(fileName, false);

            // Write the table of people.
            file.WriteLine("--");
            file.WriteLine("-- Insert the table of people.");
            file.WriteLine("--");
            file.WriteLine("DROP TABLE IF EXISTS PEOPLE;");
            file.WriteLine("CREATE TABLE PEOPLE ('ID' INTEGER PRIMARY KEY,'SURNAME' TEXT,'FORENAMES' TEXT,'MAIDEN_NAME' TEXT);");
            file.WriteLine("BEGIN TRANSACTION;");
            // Get the list of people.
            string sql = "SELECT * FROM tbl_People ORDER BY ID;";
            OleDbCommand sqlCommand = new OleDbCommand(sql, cndb_);
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                file.Write("INSERT INTO PEOPLE (ID,SURNAME,FORENAMES,MAIDEN_NAME) VALUES(");
                file.Write(Innoval.clsDatabase.ToDb(Innoval.clsDatabase.GetInt(dataReader, "ID", 0), 0) + ",");
                file.Write(Innoval.clsDatabase.ToDb(Innoval.clsDatabase.GetString(dataReader, "Surname", "")) + ",");
                file.Write(Innoval.clsDatabase.ToDb(Innoval.clsDatabase.GetString(dataReader, "Forenames", "")) + ",");
                file.Write(Innoval.clsDatabase.ToDb(Innoval.clsDatabase.GetString(dataReader, "MaidenName", "")) + "");
                file.WriteLine(");");
            }
            dataReader.Close();
            file.WriteLine("END TRANSACTION;");
            file.WriteLine();

            // Write the table of places.
            file.WriteLine("--");
            file.WriteLine("-- Insert the table of places.");
            file.WriteLine("--");
            file.WriteLine("DROP TABLE IF EXISTS PLACES;");
            file.WriteLine("CREATE TABLE PLACES ('ID' INTEGER PRIMARY KEY,'NAME' TEXT,'PARENT_ID' INTEGER,'STATUS' INTEGER,'LONGITUDE' REAL,'LATITUDE' REAL,'GOOGLE_ZOOM' INTEGER,'USE_PARENT_LOCATION' INTEGER,'PRIVATE_COMMENTS' TEXT);");
            file.WriteLine("BEGIN TRANSACTION;");
            // Get the list of places.
            sql = "SELECT ID,Name,ParentID,Status,Longitude,Latitude,GoogleZoom,UseParentLocation,PrivateComments FROM tbl_Places ORDER BY ID;";
            sqlCommand = new OleDbCommand(sql, cndb_);
            dataReader = sqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                file.Write("INSERT INTO PLACES (ID,NAME,PARENT_ID,STATUS,LONGITUDE,LATITUDE,GOOGLE_ZOOM,USE_PARENT_LOCATION,PRIVATE_COMMENTS) VALUES(");
                file.Write(toDb(dataReader.GetInt32(0), 0) + ",");
                file.Write(toDb(dataReader.GetString(1)) + ",");
                file.Write(toDb(dataReader.GetInt32(2), 0) + ",");
                file.Write(dataReader.GetInt32(3).ToString() + ",");
                file.Write(Innoval.clsDatabase.ToDb(Innoval.clsDatabase.GetFloat(dataReader, "Longitude", -999f), -999f) + ",");
                file.Write(Innoval.clsDatabase.ToDb(Innoval.clsDatabase.GetFloat(dataReader, "Latitude", -999f), -999f) + ",");
                file.Write(Innoval.clsDatabase.ToDb(Innoval.clsDatabase.GetInt(dataReader, "GoogleZoom", 0), 0) + ",");
                file.Write(Innoval.clsDatabase.ToDb(Innoval.clsDatabase.GetBool(dataReader, "UseParentLocation", false)) + ",");
                file.Write(Innoval.clsDatabase.ToDb(Innoval.clsDatabase.GetString(dataReader, "PrivateComments", "")));
                file.WriteLine(");");
            }
            dataReader.Close();
            file.Write("END TRANSACTION;");

            // Close the output file.
            file.Close();
        }
    }
}
