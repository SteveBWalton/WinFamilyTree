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
        private FactType[] factTypes_;

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
        public Relationship getRelationship(int maleIndex, int femaleIndex)
        {
            OleDbCommand sqlCommand = new OleDbCommand("SELECT ID, TerminatedID, TheDate, StartStatusID, TerminateDate, TerminateStatusID, Location, Comments, RelationshipID, LastEditBy, LastEditDate FROM tbl_Relationships WHERE MaleID = " + maleIndex.ToString() + " AND FemaleID = " + femaleIndex.ToString() + " ORDER BY TheDate DESC;", cndb);
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            Relationship relationship = null;
            if (dataReader.Read())
            {
                relationship = new Relationship(dataReader.GetInt32(0));
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



        /// <summary>Returns all the repositories as an array of IndexName pairs.  This can be used directly in combo boxes etc ...</summary>
        /// <returns>An array of IndexName objects.</returns>
        public IndexName[] getRepositories()
        {
            // Create a list of repositories
            ArrayList repositories = new ArrayList();

            // Open a list of repositories
            OleDbCommand sqlCommand = new OleDbCommand("SELECT ID,Name FROM tbl_Repositories ORDER BY ID;", cndb_);
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                IndexName repository = new IndexName(dataReader.GetInt32(0), dataReader.GetString(1));
                repositories.Add(repository);
            }
            dataReader.Close();

            // Get the list of fact types.
            return (IndexName[])(repositories.ToArray(typeof(IndexName)));
        }



        /// <summary>Returns all the sources as an array of IndexName pairs.  This can be used directly in comboboxes etc.</summary>
		/// <returns>An array of clsIDName objects</returns>
        public IndexName[] getSources(SortOrder sortOrder)
        {
            // Build a list of relivant facts
            ArrayList sources = new ArrayList();

            // Open the list of fact types
            OleDbCommand sqlCommand;
            if (sortOrder == SortOrder.DATE)
            {
                sqlCommand = new OleDbCommand("SELECT ID,Name,TheDate,TheDateStatusID FROM tbl_Sources ORDER BY LastUsed DESC;", cndb_);
            }
            else
            {
                sqlCommand = new OleDbCommand("SELECT ID,Name,TheDate,TheDateStatusID FROM tbl_Sources ORDER BY Name;", cndb_);
            }
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                string name;
                if (dataReader.IsDBNull(2))
                {
                    name = dataReader.GetString(1);
                }
                else
                {
                    name = dataReader.GetString(1) + " (" + dataReader.GetDateTime(2).Year.ToString() + ")";
                }
                IndexName source = new IndexName(dataReader.GetInt32(0), name);
                sources.Add(source);
            }
            dataReader.Close();

            // Get the list of fact types.
            return (IndexName[])(sources.ToArray(typeof(IndexName)));
        }



        /// <summary>Writes a list of references to additional media objects (gedcom OBJE @M1@) attached to the specified person into the specified Gedcom file.  The primary media is not written again.  It is assumed to be already written into the Gedcom file.</summary>
		/// <param name="file">Specifies the gedcom file to write the media references into. </param>
		/// <param name="personIndex">Specifies the ID of person to write the media references for.</param>
        /// <param name="primaryIndex">Specifies the ID of the primary media reference for this person.</param>
		/// <returns>True for success, false otherwise.</returns>
        public bool gedcomWritePersonMedia(StreamWriter file, int personIndex, int primaryIndex)
        {
            // Write the primary media first
            if (primaryIndex != 0)
            {
                file.WriteLine("1 OBJE @M" + primaryIndex.ToString("0000") + "@");
            }

            // Now write any additional media objects do not repeat the primary media
            string sql = "SELECT MediaID FROM tbl_AdditionalMediaForPeople WHERE PersonID=" + personIndex.ToString() + ";";
            OleDbCommand sqlCommand = new OleDbCommand(sql, cndb_);
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                int mediaIndex = getInt(dataReader, "MediaID", 0);
                if (mediaIndex != 0 && mediaIndex != primaryIndex)
                {
                    file.WriteLine("1 OBJE @M" + mediaIndex.ToString("0000") + "@");
                }
            }
            dataReader.Close();

            // Return success.
            return true;
        }



        /// <summary> Writes all the media objects (OBJE) into the Gedcom file.</summary>
		/// <remarks>This should not be in this section move somewhere else.</remarks>
		/// <param name="file"></param>
		/// <returns></returns>
        public bool gedcomWriteMedia(StreamWriter file)
        {
            // Select all the media objects.
            OleDbCommand sqlCommand = new OleDbCommand("SELECT * FROM tbl_Media ORDER BY ID;", cndb_);
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                int index = Database.getInt(dataReader, "ID", 0);
                file.WriteLine("0 @M" + index.ToString("0000") + "@ OBJE");
                string fileName = Database.getString(dataReader, "Filename", "Undefined");
                file.WriteLine("1 FILE media/" + fileName);
                file.WriteLine("2 FORM");
                file.WriteLine("3 TYPE photo");
                string title = Database.getString(dataReader, "Title", "Undefined");
                file.WriteLine("2 TITL " + title);
                bool isPrimary = dataReader.GetBoolean(dataReader.GetOrdinal("Primary"));
                if (isPrimary)
                {
                    file.WriteLine("1 _PRIM Y");
                }
                else
                {
                    file.WriteLine("1 _PRIM N");
                }
                bool isThumbnail = dataReader.GetBoolean(dataReader.GetOrdinal("Thumbnail"));
                if (isThumbnail)
                {
                    file.WriteLine("1 _THUM Y");
                }
                else
                {
                    file.WriteLine("1 _THUM N");
                }
            }
            dataReader.Close();

            // Return success.
            return true;
        }



        /// <summary>Write a Gedcom repository record (@R1@ REPO) record for all the repositories in this database.</summary>
		/// <param name="file">Specifies the file to write the record into.</param>
		/// <returns>True for success, false otherwise.</returns>
        public bool writeRepositoriesGedcom(StreamWriter file)
        {
            // Select all the sources.
            OleDbCommand sqlCommand = new OleDbCommand("SELECT * FROM tbl_Repositories WHERE ID>0 ORDER BY ID;", cndb_);
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                int index = getInt(dataReader, "ID", 0);
                file.WriteLine("0 @R" + index.ToString("0000") + "@ REPO");
                string name = getString(dataReader, "Name", "");
                file.WriteLine("1 NAME " + name);
                string address = getString(dataReader, "Address", "");
                if (address != "")
                {
                    gedcomMultiLine(file, 1, "ADDR", address);
                }
                string webUrl = getString(dataReader, "WebURL", "");
                if (webUrl != "")
                {
                    file.WriteLine("1 WWW " + webUrl);
                }
            }
            dataReader.Close();

            // Return success.
            return true;
        }



        /// <summary>Write a Gedcom source (@S1@ SOUR) record for all the sources in this database.</summary>
		/// <param name="file">Specifies the file to write the record into.</param>
		/// <returns>True for success, false otherwise.</returns>
        public bool writeSourcesGedcom(StreamWriter file, funcVoid lpfnProgressBar, GedcomOptions options)
        {
            // Select all the sources.
            OleDbCommand sqlCommand = new OleDbCommand("SELECT ID, Name, TheDate, TheDateStatusID, Comments, AdditionalInfoTypeID, Gedcom, RepositoryID FROM tbl_Sources ORDER BY ID;", cndb_);
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                if (GetBool(dataReader, "Gedcom", true))
                {
                    int index = dataReader.GetInt32(0);
                    file.WriteLine("0 @S" + index.ToString("0000") + "@ SOUR");
                    string name = dataReader.GetString(1);
                    file.WriteLine("1 TITL " + name);
                    if (!dataReader.IsDBNull(2))
                    {
                        CompoundDate compoundDate = new CompoundDate();
                        compoundDate.date = dataReader.GetDateTime(2);
                        compoundDate.status = dataReader.GetInt32(3);
                        file.WriteLine("2 DATE " + compoundDate.format(DateFormat.GEDCOM));
                    }

                    // Additional Information for the source.
                    if (!dataReader.IsDBNull(5))
                    {
                        switch (dataReader.GetInt32(5))
                        {
                        case 1: // Birth Certifcate.
                            sourceBirthCertificate(file, index);
                            break;
                        case 2: // Marriage Certifcate.
                            sourceMarriageCertificate(file, index, options);
                            break;
                        case 3: // Death Certificate.
                            sourceDeathCertificate(file, index);
                            break;
                        case 4: // Census Information.
                            sourceCensusInfo(file, index, options);
                            break;
                        }
                    }

                    // The note for the source.
                    if (!dataReader.IsDBNull(4))
                    {
                        gedcomMultiLine(file, 1, "NOTE", dataReader.GetString(4));
                    }

                    // The repository for this source.
                    int repositoryIndex = Database.getInt(dataReader, "RepositoryID", 0);
                    if (repositoryIndex > 0)
                    {
                        file.WriteLine("1 REPO @R" + repositoryIndex.ToString("0000") + "@");
                    }
                }

                if (lpfnProgressBar != null)
                {
                    lpfnProgressBar();
                }
            }
            dataReader.Close();

            // Return success.
            return true;
        }

        // Write the additional birth certificate information for a source.
        /// <summary>
        /// Write the additional birth certificate information for a source.
        /// </summary>
		/// <param name="oFile">Specifies the file to write the information into.</param>
		/// <param name="nID">Specifies the ID of the birth certificate (and the parent source record).</param>
        private void sourceBirthCertificate(StreamWriter oFile, int nID)
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
                gedcomLongNote(ref bFirst, oFile, "Registration District: " + oBirth.registrationDistrict);
            }
            if (oBirth.whenAndWhere != "")
            {
                gedcomLongNote(ref bFirst, oFile, "When and Where: " + oBirth.when.ToString("d MMM yyyy") + oBirth.whenAndWhere);
            }
            if (oBirth.name != "")
            {
                gedcomLongNote(ref bFirst, oFile, "Name: " + oBirth.name + " (" + oBirth.sex + ")");
            }
            if (oBirth.mother != "")
            {
                gedcomLongNote(ref bFirst, oFile, "Mother: " + oBirth.mother);
            }
            if (oBirth.father != "")
            {
                gedcomLongNote(ref bFirst, oFile, "Father: " + oBirth.father + " (" + oBirth.fatherOccupation + ")");
            }
            if (oBirth.informant != "")
            {
                gedcomLongNote(ref bFirst, oFile, "Informant: " + oBirth.informant);
            }
            if (oBirth.whenRegistered != "")
            {
                gedcomLongNote(ref bFirst, oFile, "When Registered: " + oBirth.whenRegistered);
            }
        }



        /// <summary>Write the additional marriage certificate information for a source.</summary>
		/// <param name="file">Specifies the file to write the information into.</param>
		/// <param name="sourceIndex">Specifies the ID of the marriage certificate (and the parent source record).</param>
        private void sourceMarriageCertificate(StreamWriter file, int sourceIndex, GedcomOptions options)
        {
            // Connect to the database again (to open a second datareader).
            OleDbConnection cndb = new OleDbConnection(cndb_.ConnectionString);
            cndb.Open();

            // Create a Marriage Certificate object.
            MarriageCertificate marriageCertificate = new MarriageCertificate(sourceIndex, cndb);

            // Close the database.
            cndb.Close();

            // Write the details of the marriage certificate.
            StringBuilder text;
            writeGedcomPlace(file, 2, marriageCertificate.location, options);
            bool isFirst = true;
            if (marriageCertificate.groomName != "")
            {
                text = new StringBuilder();
                text.Append(marriageCertificate.groomName);
                if (marriageCertificate.groomAge != "")
                {
                    text.Append(" (" + marriageCertificate.groomAge + ")");
                }
                if (marriageCertificate.groomOccupation != "")
                {
                    text.Append(" - " + marriageCertificate.groomOccupation);
                }
                if (marriageCertificate.groomLiving != "")
                {
                    text.Append(" - " + marriageCertificate.groomLiving);
                }
                gedcomLongNote(ref isFirst, file, "Groom: " + text.ToString());
            }
            if (marriageCertificate.brideName != "")
            {
                text = new StringBuilder();
                text.Append(marriageCertificate.brideName);
                if (marriageCertificate.brideAge != "")
                {
                    text.Append(" (" + marriageCertificate.brideAge + ")");
                }
                if (marriageCertificate.brideOccupation != "")
                {
                    text.Append(" - " + marriageCertificate.brideOccupation);
                }
                if (marriageCertificate.brideLiving != "")
                {
                    text.Append(" - " + marriageCertificate.brideLiving);
                }
                gedcomLongNote(ref isFirst, file, "Bride: " + text.ToString());
            }
            if (marriageCertificate.groomFather != "")
            {
                text = new StringBuilder(marriageCertificate.groomFather);
                if (marriageCertificate.groomFatherOccupation != "")
                {
                    text.Append(" - " + marriageCertificate.groomFatherOccupation);
                }
                gedcomLongNote(ref isFirst, file, "Groom's Father: " + text.ToString());
            }
            if (marriageCertificate.brideFather != "")
            {
                text = new StringBuilder(marriageCertificate.brideFather);
                if (marriageCertificate.brideFatherOccupation != "")
                {
                    text.Append(" - " + marriageCertificate.brideFatherOccupation);
                }
                gedcomLongNote(ref isFirst, file, "Bride's Father: " + text.ToString());
            }
            if (marriageCertificate.witness != "")
            {
                gedcomLongNote(ref isFirst, file, "Witness: " + marriageCertificate.witness);
            }
        }



        /// <summary>Write the additional death certificate information for a source.</summary>
		/// <param name="file">Specifies the file to write the information into.</param>
		/// <param name="index">Specifies the ID of the marriage certificate (and the parent source record).</param>
        private void sourceDeathCertificate(StreamWriter file, int index)
        {
            // Connect to the database again (to open a second datareader).
            OleDbConnection cndb = new OleDbConnection(cndb_.ConnectionString);
            cndb.Open();

            // Create a Marriage Certificate object.
            clsDeathCertificate deathCertificate = new clsDeathCertificate(index, cndb);

            // Close the database.
            cndb.Close();

            // Write the details of the marriage certificate.
            bool isFirst = true;
            if (deathCertificate.registrationDistrict != "")
            {
                // oFile.WriteLine("2 PLAC "+oDeath.RegistrationDistrict);
                gedcomLongNote(ref isFirst, file, "Registration District: " + deathCertificate.registrationDistrict);
            }
            if (deathCertificate.when != "")
            {
                gedcomLongNote(ref isFirst, file, "When: " + deathCertificate.when);
            }
            if (deathCertificate.place != "")
            {
                gedcomLongNote(ref isFirst, file, "Where: " + deathCertificate.place);
            }
            if (deathCertificate.name != "")
            {
                gedcomLongNote(ref isFirst, file, "Name: " + deathCertificate.name + " (" + deathCertificate.sex + ")");
            }
            if (deathCertificate.datePlaceOfBirth != "")
            {
                gedcomLongNote(ref isFirst, file, "Date & Place of Birth: " + deathCertificate.datePlaceOfBirth);
            }
            if (deathCertificate.occupation != "")
            {
                gedcomLongNote(ref isFirst, file, "Occupation: " + deathCertificate.occupation);
            }
            if (deathCertificate.usualAddress != "")
            {
                gedcomLongNote(ref isFirst, file, "Usual Address: " + deathCertificate.usualAddress);
            }
            if (deathCertificate.causeOfDeath != "")
            {
                gedcomLongNote(ref isFirst, file, "Cause of Death: " + deathCertificate.causeOfDeath);
            }
            if (deathCertificate.informant != "")
            {
                if (deathCertificate.informantDescription == "")
                {
                    gedcomLongNote(ref isFirst, file, "Informant: " + deathCertificate.informant);
                }
                else
                {
                    gedcomLongNote(ref isFirst, file, "Informant: " + deathCertificate.informant + " (" + deathCertificate.informantDescription + ")");
                }
            }
            if (deathCertificate.informantAddress != "")
            {
                gedcomLongNote(ref isFirst, file, "Informant Address: " + deathCertificate.informantAddress);
            }
            if (deathCertificate.whenRegistered != "")
            {
                gedcomLongNote(ref isFirst, file, "When Registered: " + deathCertificate.whenRegistered);
            }
        }



        /// <summary>Write the additional census information for the source.</summary>
		/// <param name="file">Specifies the Gedcom file to write the additional information into.</param>
		/// <param name="censusHouseholdIndex">Specifies the additional census information to use.</param>
        private void sourceCensusInfo(StreamWriter file, int censusHouseholdIndex, GedcomOptions options)
        {
            // Connect to the database again (to open a second datareader)
            OleDbConnection cndb = new OleDbConnection(cndb_.ConnectionString);
            cndb.Open();

            // Write the information from the census header.
            string sql = "SELECT Address FROM tbl_CensusHouseholds WHERE ID=" + censusHouseholdIndex.ToString() + ";";
            OleDbCommand sqlCommand = new OleDbCommand(sql, cndb);
            object sqlAddress = sqlCommand.ExecuteScalar();

            // Check that an address is specified.  If the address is not present then the record probably does not exist.
            if (sqlAddress != null)
            {
                string address = sqlAddress.ToString();
                writeGedcomPlace(file, 2, address, options);

                // Write the information about the members of this census record.
                sql = "SELECT NameGiven, Age, RelationToHead, Occupation, BornLocation FROM tbl_CensusPeople WHERE HouseHoldID=" + censusHouseholdIndex.ToString() + " ORDER BY ID;";
                sqlCommand = new OleDbCommand(sql, cndb);
                OleDbDataReader dataReader = sqlCommand.ExecuteReader();
                bool isFirst = true;
                while (dataReader.Read())
                {
                    string name = getString(dataReader, "NameGiven", "");
                    string age = getString(dataReader, "Age", "");
                    string relation = getString(dataReader, "RelationToHead", "");
                    string occupation = getString(dataReader, "Occupation", "");
                    string born = getString(dataReader, "BornLocation", "");

                    StringBuilder member = new StringBuilder();
                    member.Append(name);
                    if (age != "")
                    {
                        member.Append(" (" + age + ")");
                    }
                    if (relation != "")
                    {
                        member.Append(" - " + relation);
                    }
                    if (occupation != "")
                    {
                        member.Append(" - " + occupation);
                    }
                    if (born != "")
                    {
                        member.Append(" - " + born);
                    }

                    // I would prefer a better tag than NOTE but this works for now.
                    gedcomLongNote(ref isFirst, file, member.ToString());
                    // oFile.WriteLine("2 NOTE "+sbMember.ToString());
                }
                dataReader.Close();
            }

            // Close the second connection to the database.
            cndb.Close();
        }



        /// <summary>Write a Gedcom with line breaks over multilines using the CONT tag.</summary>
		/// <param name="file">Specifies the Gedcom file to write to.</param>
		/// <param name="level">Specifies the level of the tag.</param>
		/// <param name="tag">Specifies the name of the tag.</param>
		/// <param name="message">Specifies the message to write into the tag.</param>
        private void gedcomMultiLine(StreamWriter file, int level, string tag, string message)
        {
            // Deal with multiple lines recurisively.
            int lineBreak = message.IndexOf("\n");
            if (lineBreak > 0)
            {
                if (tag == "CONT")
                {
                    gedcomMultiLine(file, level, "CONT", message.Substring(0, lineBreak - 1));
                    gedcomMultiLine(file, level, "CONT", message.Substring(lineBreak + 1));
                }
                else
                {
                    gedcomMultiLine(file, level, tag, message.Substring(0, lineBreak - 1));
                    gedcomMultiLine(file, level + 1, "CONT", message.Substring(lineBreak + 1));
                }
            }
            else
            {
                file.Write(level.ToString());
                file.Write(" ");
                file.Write(tag);
                file.Write(" ");
                file.WriteLine(message);
            }
        }



        /// <summary>Write a series of lines into a single note.  The first line is tagged 1 NOTE, subsequent lines are tagged 2 CONT.</summary>
		/// <param name="isFirst">True for the first line and then reset.</param>
		/// <param name="file">Specifies the gedcom file to write the note into.</param>
		/// <param name="message">Specifies the line of text for the gedcom file.</param>
        private void gedcomLongNote(ref bool isFirst, StreamWriter file, string message)
        {
            // Deal with multiple lines recurisively.
            int lineBreak = message.IndexOf("\n");
            if (lineBreak > 0)
            {
                gedcomLongNote(ref isFirst, file, message.Substring(0, lineBreak - 1));
                gedcomLongNote(ref isFirst, file, message.Substring(lineBreak + 1));
            }
            else
            {
                if (isFirst)
                {
                    file.Write("1 NOTE ");
                    isFirst = false;
                }
                else
                {
                    file.Write("2 CONT ");
                }
                file.WriteLine(message);
            }
        }



        /// <summary>Returns an array of IndexName objects that represent the available additional information types for sources.  This is intended to populate a combo box.</summary>
		/// <returns>An array of clsIDName objects that represent the available additonal information types.</returns>
		public IndexName[] getSourceAdditionalTypes()
        {
            // Build a list of additional information types.
            ArrayList types = new ArrayList();

            // Open the list from the database.
            OleDbCommand sqlCommand = new OleDbCommand("SELECT ID,Name FROM tlk_AdditionalInfoTypes ORDER BY ID;", cndb_);
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            while (dataReader.Read())
            {

                IndexName type = new IndexName(dataReader.GetInt32(0), dataReader.GetString(1));
                types.Add(type);
            }
            dataReader.Close();

            // Get the list of fact types.
            return (IndexName[])(types.ToArray(typeof(IndexName)));
        }



        #endregion

        #region Fact Types



        /// <summary>Returns an array of all the fact types.</summary>
        /// <returns>An array of fact types.</returns>
        public FactType[] getFactTypes()
        {
            // Open the fact types (if required).
            if (factTypes_ == null)
            {
                loadFactTypes();
            }

            // Return the array of fact types.
            return factTypes_;
        }



        /// <summary>Returns a clsFactType object with the specified ID or null if no matching object can be found.</summary>
        /// <param name="index">Specifies the ID of the fact type object required.</param>
        /// <returns>A clsFactType object or null.</returns>
        public FactType getFactType(int index)
        {
            // Open the fact types (if required).
            if (factTypes_ == null)
            {
                loadFactTypes();
            }

            // Return a matching fact type (if possible).
            for (int i = 0; i < factTypes_.Length; i++)
            {
                if (factTypes_[i].index == index)
                {
                    return factTypes_[i];
                }
            }

            // Return failure.
            return null;
        }



        /// <summary>Loads the fact types from the database.</summary>
        /// <returns>True for success, false for failure.</returns>
        private bool loadFactTypes()
        {
            // Build a list of relivant facts
            ArrayList factTypes = new ArrayList();

            // Open the list of fact types
            OleDbCommand sqlCommand = new OleDbCommand("SELECT ID,Name FROM tlk_FactTypes ORDER BY ID;", cndb_);
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                FactType factType = new FactType(dataReader.GetInt32(0), dataReader.GetString(1));
                factTypes.Add(factType);
            }
            dataReader.Close();

            // Get the list of fact types.
            factTypes_ = (FactType[])(factTypes.ToArray(typeof(FactType)));

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
        private Place getPlace(string placeName, int parentIndex)
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
                return new Place(placeIndex, this);
            }

            // Search further down the string.
            return getPlace(tail, placeIndex);
        }



        /// <summary>Returns the place object that represents the specified compound place name.</summary>
        /// <param name="placeName">Specifies the name of the place.  Eg. Leeds, Yorkshire, England.</param>
        /// <returns>The place object or null.</returns>
        public Place getPlace(string placeName)
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
            string sql = "SELECT ID FROM tbl_Places WHERE Name = " + walton.Database.toDb(placeName) + " AND ParentID = " + parentIndex.ToString() + ";";
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
        public bool writeGedcomPlace(StreamWriter file, int level, string fullPlace, GedcomOptions options)
        {
            // Optionally split the address off from the PLAC tag.
            if (options.isRemoveADDRfromPLAC)
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
            if (options.isUseLongitude)
            {
                Place place = getPlace(fullPlace);
                if (place != null)
                {
                    file.WriteLine((level + 1).ToString() + " MAP");
                    if (place.latitude >= 0)
                    {
                        file.WriteLine((level + 2).ToString() + " LATI N" + place.latitude.ToString());
                    }
                    else
                    {
                        file.WriteLine((level + 2).ToString() + " LATI S" + Math.Abs(place.latitude).ToString());
                    }
                    if (place.longitude > 0)
                    {
                        file.WriteLine((level + 2).ToString() + " LONG E" + place.longitude.ToString());
                    }
                    else
                    {
                        file.WriteLine((level + 2).ToString() + " LONG W" + Math.Abs(place.longitude).ToString());
                    }
                }
            }

            // Optionally include an ADDR tag.
            if (options.isUseADDR)
            {
                string address = placeToGedcom(fullPlace, 1);
                if (address != "")
                {
                    file.WriteLine((level + 1).ToString() + " ADDR " + address);
                }
            }

            // Include the optional CTRY (Country) tag.
            if (options.isUseCTRY)
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



        public Place[] getPlaces(int placeIndex)
        {
            // Build a list to contain the places.
            ArrayList places = new ArrayList();

            // Build the list of child places.
            string sql = "SELECT * FROM tbl_Places WHERE ParentID = " + placeIndex.ToString() + " ORDER BY Name;";
            OleDbCommand sqlCommand = new OleDbCommand(sql, cndb_);
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                Place place = new Place(dataReader, this);
                places.Add(place);
            }
            dataReader.Close();

            // Return the list of places.
            return (Place[])places.ToArray(typeof(Place));
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
                int nPersonID = getInt(dataReader, "PersonID", 0);
                html.Append("<a href=\"person:" + nPersonID.ToString() + "\">");
                Person oPerson = new Person(nPersonID, this);
                html.Append(oPerson.getName(true, true));
                html.Append("</a>");
                html.Append("</span></td>");
                //sbHtml.Append("<td><span class=\"Small\">" + GetInt(drChanges,"Priority",0).ToString() + "</span></td>");
                //sbHtml.Append("<td><span class=\"Small\">" + GetString(drChanges,"Description","") + "</span></td>");
                html.Append("<td align=\"right\">" + getInt(dataReader, "Priority", 0).ToString() + "</td>");
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
                    html.Append("<a href=\"person:" + getInt(drChanges, "ID", 0) + "\">");
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
		public static int getInt(OleDbDataReader dataReader, string columnName, int nullValue)
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
                file.Write("INSERT INTO PEOPLE (ID, SURNAME, FORENAMES, MAIDEN_NAME) VALUES(");
                file.Write(walton.Database.toDb(walton.Database.getInt(dataReader, "ID", 0), 0) + ", ");
                file.Write(walton.Database.toDb(walton.Database.getString(dataReader, "Surname", "")) + ", ");
                file.Write(walton.Database.toDb(walton.Database.getString(dataReader, "Forenames", "")) + ", ");
                file.Write(walton.Database.toDb(walton.Database.getString(dataReader, "MaidenName", "")) + "");
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
                file.Write(walton.Database.toDb(walton.Database.getFloat(dataReader, "Longitude", -999f), -999f) + ",");
                file.Write(walton.Database.toDb(walton.Database.getFloat(dataReader, "Latitude", -999f), -999f) + ",");
                file.Write(walton.Database.toDb(walton.Database.getInt(dataReader, "GoogleZoom", 0), 0) + ",");
                file.Write(walton.Database.toDb(walton.Database.getBool(dataReader, "UseParentLocation", false)) + ",");
                file.Write(walton.Database.toDb(walton.Database.getString(dataReader, "PrivateComments", "")));
                file.WriteLine(");");
            }
            dataReader.Close();
            file.Write("END TRANSACTION;");

            // Close the output file.
            file.Close();
        }
    }
}
