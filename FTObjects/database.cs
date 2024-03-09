using System;
// ArrayList
using System.Collections;
// Access database ADO.NET
using System.Data.OleDb;
// Sqlite3 database.  This is via Manage NuGet Packages.
using System.Data.SQLite;
using System.IO;
// StringBuilder
using System.Text;

namespace family_tree.objects
{
    #region Supporting types, enums, etc...

    /// <summary>Delegate for functions that refresh the UI.</summary>
    public delegate void FuncVoid();

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

        /// <summary>Connection to an Access database.</summary>
        private OleDbConnection cndb_;

        /// <summary>Connection to a sqlite3 database.</summary>
        private SQLiteConnection sqlite_;

        /// <summary>List of fact types.</summary>
        private FactType[] factTypes_;

        /// <summary>Range of birthdate difference used to search for marriage partners.</summary>
        private int marriedRange_;

        /// <summary>Filename of the database.</summary>
        private string fileName_;

        /// <summary>This should really be something like user options.</summary>
        private string googleMapsKey_;

        #endregion

        #region Constructors Destructors etc...



        /// <summary>Class constructor.  Opens the specified database.  Call Dispose() to close the database.</summary>
        /// <param name="fileName">Specifies the filename of the family tree access database</param>
        public Database(string fileName, string googleMapsKey)
        {
            fileName_ = fileName;
            factTypes_ = null;
            marriedRange_ = 20;
            googleMapsKey_ = googleMapsKey;

            // Open the connection to the database.
            cndb_ = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + fileName + ";");
            cndb_.Open();

            // Open the connection to a sqlite3 database.
            /*
            string sqliteFileName = Path.GetDirectoryName(fileName_) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(fileName_) + ".db";
            sqlite_ = new SQLiteConnection("Data Source=" + sqliteFileName + ";Version=3;Compress=True;");
            sqlite_.Open();
            */
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
            if (sqlite_ != null)
            {
                sqlite_.Close();
                sqlite_ = null;
            }
        }



        #endregion

        #region People



        /// <summary>Returns the person object specified by nPersonID.</summary>
        /// <param name="personIdx">Specifies the ID of the person required</param>
        /// <returns>The person objects co-responding the the ID specified by nPersonID</returns>
        public Person getPerson(int personIdx)
        {
            // Open the specified person ID.
            Person person = new Person(personIdx, this);

            // Return the person object.
            return person;
        }



        /// <summary>Returns a list of people in an array of IdxName pairs who match the specfied criteria.</summary>
        /// <param name="sex">Specify the sex of the required people</param>
        /// <param name="order">Specify the order of the returned array.</param>
        /// <param name="startYear">Specify the earliest birth year of the required people</param>
        /// <param name="endYear">Specify the latest birth year of the required people.</param>
        /// <returns>An array of IDName pairs representing people who match the specified criteria.</returns>
        public IdxName[] getPeople(ChooseSex sex, SortOrder order, int startYear, int endYear)
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
        public IdxName[] getPeople(string sql)
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
                IdxName item = new IdxName(people.GetInt32(0), name.ToString());
                items.Add(item);
            }
            people.Close();

            // Return success
            return (IdxName[])items.ToArray(typeof(IdxName));
        }



        /// <summary>Returns a list of all the people in an array of IdxName objects.</summary>
        /// <returns>An array of clsIDName objects representing all the people.</returns>
        public IdxName[] getPeople()
        {
            return getPeople("SELECT ID,Forenames,Surname,Maidenname,Born,BornStatusID,Died,DiedStatusID FROM tbl_People ORDER BY ID;");
        }



        /// <summary>Returns a list of the people of the specified sex who are alive in the specified year.</summary>
        /// <param name="sex">Specifies the sex of the required people.</param>
        /// <param name="order">Specifies the order of the array to return.</param>
        /// <param name="yearAlive">Specifies the year when the returned people must have been alive.</param>
        /// <returns>An array of clsIDName objects representing the people alive at the specified year.</returns>
        public IdxName[] getPeople(ChooseSex sex, SortOrder order, int yearAlive)
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
        public IdxName[] getPeople(ChooseSex sex, SortOrder order)
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
        /// <param name="maleIdx">Specifies the male person in the relationship.</param>
        /// <param name="femaleIdx">Specifies the female person in the relationship.</param>
        /// <returns>The relationship object for the 2 people.  NULL if no relationshp exists between the 2 specified people.</returns>
        public Relationship getRelationship(int maleIdx, int femaleIdx)
        {
            OleDbCommand sqlCommand = new OleDbCommand("SELECT ID, TerminatedID, TheDate, StartStatusID, TerminateDate, TerminateStatusID, Location, Comments, RelationshipID, LastEditBy, LastEditDate FROM tbl_Relationships WHERE MaleID = " + maleIdx.ToString() + " AND FemaleID = " + femaleIdx.ToString() + " ORDER BY TheDate DESC;", cndb);
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            Relationship relationship = null;
            if (dataReader.Read())
            {
                relationship = new Relationship(dataReader.GetInt32(0));
                relationship.maleIdx = maleIdx;
                relationship.femaleIdx = femaleIdx;
                relationship.terminatedIdx = dataReader.GetInt32(1);
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
                relationship.typeIdx = dataReader.GetInt16(8);

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



        /// <summary>Returns all the repositories as an array of IdxName pairs.  This can be used directly in combo boxes etc ...</summary>
        /// <returns>An array of IdxName objects.</returns>
        public IdxName[] getRepositories()
        {
            // Create a list of repositories
            ArrayList repositories = new ArrayList();

            // Open a list of repositories
            OleDbCommand sqlCommand = new OleDbCommand("SELECT ID,Name FROM tbl_Repositories ORDER BY ID;", cndb_);
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                IdxName repository = new IdxName(dataReader.GetInt32(0), dataReader.GetString(1));
                repositories.Add(repository);
            }
            dataReader.Close();

            // Get the list of fact types.
            return (IdxName[])(repositories.ToArray(typeof(IdxName)));
        }



        /// <summary>Returns all the sources as an array of IdxName pairs.  This can be used directly in comboboxes etc.</summary>
		/// <returns>An array of clsIDName objects</returns>
        public IdxName[] getSources(SortOrder sortOrder)
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
                IdxName source = new IdxName(dataReader.GetInt32(0), name);
                sources.Add(source);
            }
            dataReader.Close();

            // Get the list of fact types.
            return (IdxName[])(sources.ToArray(typeof(IdxName)));
        }



        /// <summary>Writes a list of references to additional media objects (gedcom OBJE @M1@) attached to the specified person into the specified Gedcom file.  The primary media is not written again.  It is assumed to be already written into the Gedcom file.</summary>
		/// <param name="file">Specifies the gedcom file to write the media references into. </param>
		/// <param name="personIdx">Specifies the ID of person to write the media references for.</param>
        /// <param name="primaryIdx">Specifies the ID of the primary media reference for this person.</param>
		/// <returns>True for success, false otherwise.</returns>
        public bool gedcomWritePersonMedia(StreamWriter file, int personIdx, int primaryIdx)
        {
            // Write the primary media first
            if (primaryIdx != 0)
            {
                file.WriteLine("1 OBJE @M" + primaryIdx.ToString("0000") + "@");
            }

            // Now write any additional media objects do not repeat the primary media
            string sql = "SELECT MediaID FROM tbl_AdditionalMediaForPeople WHERE PersonID=" + personIdx.ToString() + ";";
            OleDbCommand sqlCommand = new OleDbCommand(sql, cndb_);
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                int mediaIdx = getInt(dataReader, "MediaID", 0);
                if (mediaIdx != 0 && mediaIdx != primaryIdx)
                {
                    file.WriteLine("1 OBJE @M" + mediaIdx.ToString("0000") + "@");
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
        public bool gedcomWriteMedia(StreamWriter file, GedcomOptions options)
        {
            // Select all the media objects.
            OleDbCommand sqlCommand = new OleDbCommand("SELECT * FROM tbl_Media ORDER BY ID;", cndb_);
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                int idx = Database.getInt(dataReader, "ID", 0);
                file.WriteLine("0 @M" + idx.ToString("0000") + "@ OBJE");
                string fileName = Database.getString(dataReader, "Filename", "Undefined");
                file.WriteLine("1 FILE Media/" + fileName);
                file.WriteLine("2 FORM " + fileName.Substring(fileName.Length - 3));
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
                // Write the last edit information
                string lastEditBy = Database.getString(dataReader, "LastEditBy", "Steve Walton");
                DateTime lastEditDate = Database.getDateTime(dataReader, "LastEditDate", DateTime.Now);
                if (lastEditBy != "")
                {
                    file.WriteLine("1 CHAN");
                    file.WriteLine("2 DATE " + lastEditDate.ToString("d MMM yyyy"));
                    file.WriteLine("3 TIME " + lastEditDate.ToString("HH:mm:ss"));
                    if (options.isIncludePGVU)
                    {
                        file.WriteLine("2 _PGVU " + lastEditBy);
                    }
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
                int idx = getInt(dataReader, "ID", 0);
                file.WriteLine("0 @R" + idx.ToString("0000") + "@ REPO");
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
        public bool writeSourcesGedcom(StreamWriter file, FuncVoid lpfnProgressBar, GedcomOptions options)
        {
            // Select all the sources.
            OleDbCommand sqlCommand = new OleDbCommand("SELECT ID, Name, TheDate, TheDateStatusID, Comments, AdditionalInfoTypeID, Gedcom, RepositoryID, LastEditBy, LastEditDate FROM tbl_Sources ORDER BY ID;", cndb_);
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                if (GetBool(dataReader, "Gedcom", true) || options.isAllElements)
                {
                    int idx = dataReader.GetInt32(0);
                    file.WriteLine("0 @S" + idx.ToString("0000") + "@ SOUR");
                    string name = dataReader.GetString(1);
                    file.WriteLine("1 TITL " + name);
                    if (!dataReader.IsDBNull(2))
                    {
                        CompoundDate compoundDate = new CompoundDate();
                        compoundDate.date = dataReader.GetDateTime(2);
                        compoundDate.status = dataReader.GetInt32(3);
                        file.WriteLine("1 DATE " + compoundDate.format(DateFormat.GEDCOM));
                    }

                    // Additional Information for the source.
                    if (!dataReader.IsDBNull(5))
                    {
                        switch (dataReader.GetInt32(5))
                        {
                        case 1: // Birth Certifcate.
                            sourceBirthCertificate(file, idx);
                            break;
                        case 2: // Marriage Certifcate.
                            sourceMarriageCertificate(file, idx, options);
                            break;
                        case 3: // Death Certificate.
                            sourceDeathCertificate(file, idx);
                            break;
                        case 4: // Census Information.
                            sourceCensusInfo(file, idx, options);
                            break;
                        }
                    }

                    // The note for the source.
                    if (!dataReader.IsDBNull(4))
                    {
                        gedcomMultiLine(file, 1, "NOTE", dataReader.GetString(4));
                    }

                    // The repository for this source.
                    int repositoryIdx = Database.getInt(dataReader, "RepositoryID", 0);
                    if (repositoryIdx > 0)
                    {
                        file.WriteLine("1 REPO @R" + repositoryIdx.ToString("0000") + "@");
                    }

                    // Last Edit.
                    file.WriteLine("1 CHAN");
                    DateTime lastEditDate = Database.getDateTime(dataReader, "LastEditDate", DateTime.Now);
                    file.WriteLine("2 DATE " + lastEditDate.ToString("d MMM yyyy"));
                    file.WriteLine("3 TIME " + lastEditDate.ToString("HH:mm:ss"));
                    if (options.isIncludePGVU)
                    {
                        string lastEditBy = Database.getString(dataReader, "LastEditBy", "unknown");
                        file.WriteLine("2 _PGVU " + lastEditBy);
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



        /// <summary>Write the additional birth certificate information for a source.</summary>
        /// <param name="file">Specifies the file to write the information into.</param>
        /// <param name="idx">Specifies the ID of the birth certificate (and the parent source record).</param>
        private void sourceBirthCertificate(StreamWriter file, int idx)
        {
            // Connect to the database again (to open a second datareader).
            OleDbConnection cnDb = new OleDbConnection(cndb_.ConnectionString);
            cnDb.Open();

            // Create a birth certificate object.
            BirthCertificate birth = new BirthCertificate(idx, cnDb);

            // Close the database
            cnDb.Close();

            // Write the details from the birth certificate.
            gedcomLongNote(file, 1, "NOTE", "GRID: GRO Reference: " + birth.groReference);
            gedcomLongNote(file, 2, "CONT", "Registration District: " + birth.registrationDistrict);
            gedcomLongNote(file, 2, "CONT", "When and Where: " + birth.when.ToString("d MMM yyyy") + ": " + birth.whenAndWhere);
            gedcomLongNote(file, 2, "CONT", "Name: " + birth.name + ": " + birth.sex);
            gedcomLongNote(file, 2, "CONT", "Mother: " + birth.mother + ": " + birth.motherDetails);
            gedcomLongNote(file, 2, "CONT", "Father: " + birth.father + ": " + birth.fatherOccupation);
            gedcomLongNote(file, 2, "CONT", "Informant: " + birth.informant + ": " + birth.informantAddress);
            gedcomLongNote(file, 2, "CONT", "When Registered: " + birth.whenRegistered);
        }



        /// <summary>Write the additional marriage certificate information for a source.</summary>
		/// <param name="file">Specifies the file to write the information into.</param>
		/// <param name="sourceIdx">Specifies the ID of the marriage certificate (and the parent source record).</param>
        private void sourceMarriageCertificate(StreamWriter file, int sourceIdx, GedcomOptions options)
        {
            // Connect to the database again (to open a second datareader).
            OleDbConnection cndb = new OleDbConnection(cndb_.ConnectionString);
            cndb.Open();

            // Create a Marriage Certificate object.
            MarriageCertificate marriageCertificate = new MarriageCertificate(sourceIdx, cndb);

            // Close the database.
            cndb.Close();

            // Write the details of the marriage certificate.
            writeGedcomPlace(file, 1, marriageCertificate.location, null, options);
            gedcomLongNote( file, 1, "NOTE", "GRID: GRO Reference: " + marriageCertificate.groReference);
            StringBuilder text = new StringBuilder();
            text.Append(marriageCertificate.groomName);
            text.Append(": ");  // This would be the identity of the groom in the database but the source does not know it.
            text.Append(": " + marriageCertificate.groomAge);
            text.Append(": " + marriageCertificate.groomOccupation);
            text.Append(": " + marriageCertificate.groomLiving);
            gedcomLongNote(file, 2, "CONT", "Groom: " + text.ToString());
            text = new StringBuilder();
            text.Append(marriageCertificate.brideName);
            text.Append(": ");  // This would be the identity of the bride in the database but the source does not know it.
            text.Append(": " + marriageCertificate.brideAge);
            text.Append(": " + marriageCertificate.brideOccupation);
            text.Append(": " + marriageCertificate.brideLiving);
            gedcomLongNote(file, 2, "CONT", "Bride: " + text.ToString());
            text = new StringBuilder(marriageCertificate.groomFather);
            text.Append(": ");  // This would be the identity of the groom father in the database but the source does not know it.
            text.Append(": " + marriageCertificate.groomFatherOccupation);
            gedcomLongNote(file, 2, "CONT", "Groom's Father: " + text.ToString());
            text = new StringBuilder(marriageCertificate.brideFather);
            text.Append(": ");  // This would be the identity of the bride father in the database but the source does not know it.
            text.Append(": " + marriageCertificate.brideFatherOccupation);
            gedcomLongNote(file, 2, "CONT", "Bride's Father: " + text.ToString());
            gedcomLongNote(file, 2, "CONT", "Witness: " + marriageCertificate.witness);
        }



        /// <summary>Write the additional death certificate information for a source.</summary>
		/// <param name="file">Specifies the file to write the information into.</param>
		/// <param name="idx">Specifies the ID of the marriage certificate (and the parent source record).</param>
        private void sourceDeathCertificate(StreamWriter file, int idx)
        {
            // Connect to the database again (to open a second datareader).
            OleDbConnection cndb = new OleDbConnection(cndb_.ConnectionString);
            cndb.Open();

            // Create a Marriage Certificate object.
            DeathCertificate deathCertificate = new DeathCertificate(idx, cndb);

            // Close the database.
            cndb.Close();

            // Write the details of the marriage certificate.
            gedcomLongNote(file, 1, "NOTE", "GRID: GRO Reference: " + deathCertificate.groReference);            
            gedcomLongNote(file, 2, "CONT", "Registration District: " + deathCertificate.registrationDistrict);
            gedcomLongNote(file, 2, "CONT", "When: " + deathCertificate.when);
            gedcomLongNote(file, 2, "CONT", "Where: " + deathCertificate.place);
            gedcomLongNote(file, 2, "CONT", "Name: " + deathCertificate.name + ": " + deathCertificate.sex);
            gedcomLongNote(file, 2, "CONT", "Date & Place of Birth: " + deathCertificate.datePlaceOfBirth+": ");
            gedcomLongNote(file, 2, "CONT", "Occupation: " + deathCertificate.occupation);
            gedcomLongNote(file, 2, "CONT", "Usual Address: " + deathCertificate.usualAddress);
            gedcomLongNote(file, 2, "CONT", "Cause of Death: " + deathCertificate.causeOfDeath);
            gedcomLongNote(file, 2, "CONT", "Informant: " + deathCertificate.informant + ": " + deathCertificate.informantDescription);
            gedcomLongNote(file, 2, "CONT", "Informant Address: " + deathCertificate.informantAddress);
            gedcomLongNote(file, 2, "CONT", "When Registered: " + deathCertificate.whenRegistered);
        }



        /// <summary>Write the additional census information for the source.</summary>
		/// <param name="file">Specifies the Gedcom file to write the additional information into.</param>
		/// <param name="censusHouseholdIdx">Specifies the additional census information to use.</param>
        private void sourceCensusInfo(StreamWriter file, int censusHouseholdIdx, GedcomOptions options)
        {
            // Connect to the database again (to open a second datareader)
            OleDbConnection cndb = new OleDbConnection(cndb_.ConnectionString);
            cndb.Open();

            // Write the information from the census header.
            string sql = "SELECT Address, Series, Piece, Folio, Page FROM tbl_CensusHouseholds WHERE ID = " + censusHouseholdIdx.ToString() + ";";
            OleDbCommand sqlCommand = new OleDbCommand(sql, cndb);
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            string sqlAddress = "";
            string reference = "";
            if (dataReader.Read())
            {
                sqlAddress = getString(dataReader, "Address", "");
                reference = "Series: " + getString(dataReader, "Series", "") + ": Piece: " + getString(dataReader, "Piece", "") + ": Folio: " + getString(dataReader, "Folio", "") + ": Page: " + getString(dataReader, "Page", "");
            }

            // Check that an address is specified.  If the address is not present then the record probably does not exist.
            if (sqlAddress != null)
            {
                string address = sqlAddress.ToString();
                writeGedcomPlace(file, 1, address, null, options);

                gedcomLongNote(file, 1, "NOTE", "GRID: Reference: " + reference);

                // Write the information about the members of this census record.
                sql = "SELECT NameGiven, Age, RelationToHead, Occupation, BornLocation, PersonID FROM tbl_CensusPeople WHERE HouseHoldID = " + censusHouseholdIdx.ToString() + " ORDER BY ID;";
                sqlCommand = new OleDbCommand(sql, cndb);
                dataReader = sqlCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    string name = getString(dataReader, "NameGiven", "");
                    string age = getString(dataReader, "Age", "");
                    string relation = getString(dataReader, "RelationToHead", "");
                    string occupation = getString(dataReader, "Occupation", "");
                    string born = getString(dataReader, "BornLocation", "");
                    string actualPerson = "";
                    int actualPersonIdx = getInt(dataReader, "PersonID", 0);
                    if (actualPersonIdx != 0)
                    {
                        actualPerson = "I" + actualPersonIdx.ToString("0000");
                    }

                    StringBuilder member = new StringBuilder();
                    member.Append(name);
                    member.Append(": " + actualPerson);
                    member.Append(": " + age);
                    member.Append(": " + relation);
                    member.Append(": " + occupation);
                    member.Append(": " + born);

                    // Write the person details.
                    gedcomLongNote(file, 2, "CONT", member.ToString());
                }
                dataReader.Close();
            }

            // Close the second connection to the database.
            cndb.Close();
        }



        /// <summary>Encode a linefeed character as a non line feed character.  This is kind of the opposite of the CONT tag.</summary>
        /// <param name="message">Specifies the text to remove linefeed characters from.</param>
        /// <returns>Returns the text with the linefeed characters removed.</returns>
        private string encodeLineBreaks(string message)
        {
            int lineBreak = message.IndexOf("\n");
            while (lineBreak > 0)
            {
                message = message.Substring(0, lineBreak - 1) + "<br />" + message.Substring(lineBreak + 1);

                // Search for the next line break.
                lineBreak = message.IndexOf("\n");
            }
            return message;
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
		/// <param name="file">Specifies the gedcom file to write the note into.</param>
        /// <param name="level">Specifies the level of the tag.  Expected to be 1.</param>
        /// <param name="tag">Specifies the name of the tag.  Expected to be 'NOTE'.</param>
		/// <param name="message">Specifies the line of text for the gedcom file.</param>
        private void gedcomLongNote(StreamWriter file, int level, string tag,  string message)
        {
            // Deal with multiple lines recurisively.
            int lineBreak = message.IndexOf("\n");
            if (lineBreak > 0)
            {
                gedcomLongNote(file, level, tag, message.Substring(0, lineBreak - 1));

                string[] contLines = message.Substring(lineBreak + 1).Split('\n');
                foreach (string line in contLines)
                {
                    if (line.EndsWith("\r"))
                    {
                        // Often Windows uses "\r\n" as linefeed character.
                        gedcomLongNote(file, level + 1, "CONT", line.Substring(0, line.Length - 1));
                    }
                    else
                    {
                        gedcomLongNote(file, level + 1, "CONT", line);
                    }
                }
            }
            else
            {
                /*
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
                */
                file.WriteLine(level.ToString() + " " + tag + " " + message);
            }
        }



        /// <summary>Returns an array of IdxName objects that represent the available additional information types for sources.  This is intended to populate a combo box.</summary>
		/// <returns>An array of clsIDName objects that represent the available additonal information types.</returns>
		public IdxName[] getSourceAdditionalTypes()
        {
            // Build a list of additional information types.
            ArrayList types = new ArrayList();

            // Open the list from the database.
            OleDbCommand sqlCommand = new OleDbCommand("SELECT ID, Name FROM tlk_AdditionalInfoTypes ORDER BY ID;", cndb_);
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            while (dataReader.Read())
            {

                IdxName type = new IdxName(dataReader.GetInt32(0), dataReader.GetString(1));
                types.Add(type);
            }
            dataReader.Close();

            // Get the list of fact types.
            return (IdxName[])(types.ToArray(typeof(IdxName)));
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
        /// <param name="idx">Specifies the ID of the fact type object required.</param>
        /// <returns>A clsFactType object or null.</returns>
        public FactType getFactType(int idx)
        {
            // Open the fact types (if required).
            if (factTypes_ == null)
            {
                loadFactTypes();
            }

            // Return a matching fact type (if possible).
            for (int i = 0; i < factTypes_.Length; i++)
            {
                if (factTypes_[i].index == idx)
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
        public IdxName[] cenusGetHouseholds(int theYear)
        {
            // Build a list of relivant facts.
            ArrayList houseHolds = new ArrayList();

            string sql = "SELECT ID, Address FROM tbl_CensusHouseholds WHERE Year(CensusDate)=" + theYear.ToString() + " ORDER BY Address;";
            OleDbCommand sqlCommand = new OleDbCommand(sql, cndb_);
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                IdxName houseHold = new IdxName(dataReader.GetInt32(0), dataReader.GetString(1));
                houseHolds.Add(houseHold);
            }
            dataReader.Close();

            // Return the households found.
            return (IdxName[])houseHolds.ToArray(typeof(IdxName));
        }



        /// <summary>Returns an array CensusPerson objects representing the member of the specified census household.</summary>
        /// <param name="houseHoldIdx">Specifies the ID of the census household.</param>
        /// <returns>An array of CensusPerson objects representing the members of the specified census household.</returns>
        public CensusPerson[] censusHouseholdMembers(int houseHoldIdx)
        {
            string sql = "SELECT tbl_CensusPeople.*, tbl_People.Forenames+' '+ Iif(IsNull(tbl_People.MaidenName),tbl_People.Surname, tbl_People.MaidenName) AS Name, tbl_CensusHouseholds.Address, tbl_CensusHouseholds.CensusDate " +
                "FROM (tbl_CensusPeople LEFT JOIN tbl_People ON tbl_CensusPeople.PersonID = tbl_People.ID) INNER JOIN tbl_CensusHouseholds ON tbl_CensusPeople.HouseHoldID = tbl_CensusHouseholds.ID " +
                "WHERE (((tbl_CensusPeople.HouseHoldID)=" + houseHoldIdx.ToString() + ")) " +
                "ORDER BY tbl_CensusPeople.ID;";
            return censusGetRecords(sql);
        }



        /// <summary>Returns an array of CensusPerson objects as specified in the Sql command.</summary>
        /// <param name="sql">Specifies a Sql command to fetch a collection of census members.</param>
        /// <returns>An array of CensusPerson objects as specified in the Sql command.</returns>
        private CensusPerson[] censusGetRecords(string sql)
        {
            // Build a list of household members
            ArrayList members = new ArrayList();

            OleDbCommand sqlCommand = new OleDbCommand(sql, cndb_);
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                CensusPerson censusPerson = new CensusPerson();
                censusPerson.idx = dataReader.GetInt32(0);
                censusPerson.houseHoldIdx = dataReader.GetInt32(1);
                if (!dataReader.IsDBNull(2))
                {
                    censusPerson.personIdx = dataReader.GetInt32(2);
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
                    censusPerson.dateOfBirth = dataReader.GetString(8);
                }
                if (!dataReader.IsDBNull(9))
                {
                    censusPerson.sex = dataReader.GetString(9);
                }
                if (!dataReader.IsDBNull(10))
                {
                    censusPerson.maritalStatus = dataReader.GetString(10);
                }
                if (!dataReader.IsDBNull(11))
                {
                    censusPerson.personName = dataReader.GetString(11);
                }
                censusPerson.houseHoldName = dataReader.GetString(12);
                censusPerson.date = dataReader.GetDateTime(13);

                members.Add(censusPerson);
            }
            dataReader.Close();

            // Return the members found.
            return (CensusPerson[])members.ToArray(typeof(CensusPerson));
        }



        /// <summary>Writes the specified CensusPerson record to the database.</summary>
        /// <param name="censusPerson">Specifies the CensusPerson record to write to the database.</param>
        /// <returns>True for success, false otherwise.</returns>
        public bool censusSavePerson(CensusPerson censusPerson)
        {
            // Delete this person if required.
            if (!censusPerson.isValid())
            {
                if (censusPerson.idx != 0)
                {
                    OleDbCommand sqlCommand = new OleDbCommand("DELETE FROM tbl_CensusPeople WHERE ID = " + censusPerson.idx.ToString() + ";", cndb_);
                    sqlCommand.ExecuteNonQuery();
                }
                return true;
            }

            // Create a new record if required.
            if (censusPerson.idx == 0)
            {
                string sql = "SELECT MAX(ID) AS MaxID FROM tbl_CensusPeople;";
                OleDbCommand sqlCommand = new OleDbCommand(sql, cndb_);
                censusPerson.idx = int.Parse(sqlCommand.ExecuteScalar().ToString()) + 1;

                // Create a new record.
                sql = "INSERT INTO tbl_CensusPeople (ID, HouseholdID) VALUES (" + censusPerson.idx.ToString() + ", " + censusPerson.houseHoldIdx.ToString() + ");";
                sqlCommand = new OleDbCommand(sql, cndb_);
                sqlCommand.ExecuteNonQuery();
            }

            // Update the record.
            StringBuilder updateSql = new StringBuilder();
            updateSql.Append("UPDATE tbl_CensusPeople SET ");
            updateSql.Append("PersonID=" + censusPerson.personIdx.ToString() + ", ");
            updateSql.Append("NameGiven=" + toDb(censusPerson.censusName) + ", ");
            updateSql.Append("RelationToHead=" + toDb(censusPerson.relationToHead) + ", ");
            updateSql.Append("Age=" + toDb(censusPerson.age) + ", ");
            updateSql.Append("Occupation=" + toDb(censusPerson.occupation) + ", ");
            updateSql.Append("BornLocation=" + toDb(censusPerson.bornLocation) + ", ");
            updateSql.Append("DATE_OF_BIRTH = " + toDb(censusPerson.dateOfBirth) + ", ");
            updateSql.Append("SEX = " + toDb(censusPerson.sex) + ", ");
            updateSql.Append("MARITAL_STATUS = " + toDb(censusPerson.maritalStatus) + " ");
            updateSql.Append("WHERE ID=" + censusPerson.idx.ToString() + ";");
            OleDbCommand updateCommand = new OleDbCommand(updateSql.ToString(), cndb_);
            updateCommand.ExecuteNonQuery();

            // Add a reference to the place.
            {
                Census census = new Census(censusPerson.houseHoldIdx, this);
                string address = census.address;
                Place place = this.getPlace(address);
                string sql = "INSERT INTO tbl_ToPlaces (PlaceID, TypeID, ObjectID) VALUES (" + place.idx.ToString() + ", 1, " + censusPerson.personIdx.ToString() + ");";
                OleDbCommand sqlCommand = new OleDbCommand(sql, cndb_);
                try
                {
                    // This will fail if the record already exists, which is likely.
                    sqlCommand.ExecuteNonQuery();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }

            // Return success.
            return true;
        }



        /// <summary>Returns an array of census records that contain the specified person.</summary>
        /// <param name="personIdx"></param>
        /// <returns></returns>
        public CensusPerson[] censusForPerson(int personIdx)
        {
            string sSql = "SELECT tbl_CensusPeople.*, tbl_People.Forenames+' '+ Iif(IsNull(tbl_People.MaidenName),tbl_People.Surname, tbl_People.MaidenName) AS Name, tbl_CensusHouseholds.Address, tbl_CensusHouseholds.CensusDate " +
                "FROM (tbl_CensusPeople LEFT JOIN tbl_People ON tbl_CensusPeople.PersonID = tbl_People.ID) INNER JOIN tbl_CensusHouseholds ON tbl_CensusPeople.HouseHoldID = tbl_CensusHouseholds.ID " +
                "WHERE tbl_CensusPeople.PersonID=" + personIdx.ToString() + " " +
                "ORDER BY tbl_CensusHouseholds.CensusDate;";
            return censusGetRecords(sSql);
        }



        /// <summary>Returns a human readable string representing the people that the specified person is living with according to the census record.</summary>
        /// <param name="censusPerson">Specifies the person who should not be mentioned in the returned description.</param>
        /// <returns>A human readable string representing the people that the specified person is living with according to the census record.</returns>
        public string censusLivingWith(CensusPerson censusPerson)
        {
            StringBuilder livingWith = new StringBuilder();
            string sql = "SELECT NameGiven, Age FROM tbl_CensusPeople WHERE HouseHoldID=" + censusPerson.houseHoldIdx.ToString() + " AND PersonID<>" + censusPerson.personIdx.ToString() + " ORDER BY tbl_CensusPeople.ID;";
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
        /// <param name="parentIdx">Specifies the index of the parent place of the compound name.</param>
        /// <returns>The place object or null.</returns>
        private Place getPlace(string placeName, int parentIdx)
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
            int placeIdx = getPlaceIdx(head, parentIdx);
            if (placeIdx == 0)
            {
                return null;
            }

            // Return this place if at the end of the string.
            if (tail == string.Empty)
            {
                return new Place(placeIdx, this);
            }

            // Search further down the string.
            return getPlace(tail, placeIdx);
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
        /// <param name="parentIdx">Specifies the parent of the place.</param>
        /// <returns>The ID of the requested place.  0 if no matching place can be found.</returns>
        private int getPlaceIdx(string placeName, int parentIdx)
        {
            // Clean the place name.
            placeName = cleanPlaceName(placeName);

            // Look for this place in the database.
            string sql = "SELECT ID FROM tbl_Places WHERE Name = " + walton.Database.toDb(placeName) + " AND ParentID = " + parentIdx.ToString() + ";";
            OleDbCommand sqlCommand = new OleDbCommand(sql, cndb_);
            int placeIdx = 0;
            try
            {
                placeIdx = int.Parse(sqlCommand.ExecuteScalar().ToString());
            }
            catch { }

            // Return the ID found.
            return placeIdx;
        }



        /// <summary>Adds a compound place to the database.  This might result in a number of place records in the tbl_Places table.  It will create a link from the reference object to add the related tbl_Places records.</summary>
        /// <param name="placeName">Specifies the compound place separated by , and starting at the top level.</param>
        /// <param name="objectTypeIdx">Specifies the type of object that created this compound place.</param>
        /// <param name="objectIdx">Specifies the ID of the reference object.</param>
        /// <returns>True for success, false otherwise.</returns>
        public bool addPlace(string placeName, int objectTypeIdx, int objectIdx)
        {
            // Add this place and link to the specified object.
            return addPlace(placeName, objectTypeIdx, objectIdx, 0, 0);
        }



        /// <summary>Adds a compound place to the database.  This results in a number of place records in the tbl_Places table.</summary>
        /// <param name="placeName">Specifies the compound place separated by , but not nessercary starting with the top level.</param>
        /// <param name="objectTypeIdx">Specifies the type of object that created this compound place.</param>
        /// <param name="objectIdx">Specifies the ID of the reference object.</param>
        /// <param name="parentIdx">Specifies the ID of the place that is the parent above the compound place string.</param>
        /// <param name="level">Specifies the level from the top level.</param>
        /// <returns></returns>
        private bool addPlace(string placeName, int objectTypeIdx, int objectIdx, int parentIdx, int level)
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
            int placeIdx = getPlaceIdx(head, parentIdx);

            // Add this place (if required).
            if (placeIdx == 0)
            {
                placeIdx = addPlace(parentIdx, head);
            }

            // Link the object to this place.
            string sql = "INSERT INTO tbl_ToPlaces (PlaceID, TypeID, ObjectID) VALUES(" + placeIdx.ToString() + ", " + objectTypeIdx.ToString() + ", " + objectIdx.ToString() + ");";
            OleDbCommand sqlCommand = new OleDbCommand(sql, cndb_);
            try
            {
                sqlCommand.ExecuteNonQuery();
            }
            catch { }

            // Deal with the tail if any.
            if (tail != "")
            {
                addPlace(tail, objectTypeIdx, objectIdx, placeIdx, level + 1);
            }

            // Return success.
            return true;
        }



        /// <summary>Add the specified place to the database.</summary>
        /// <param name="parentIdx">Specifies the ID of the parent place.</param>
        /// <param name="placeName">Specifies the name of the place.</param>
        /// <returns>True for success, false otherwise.</returns>
        private int addPlace(int parentIdx, string placeName)
        {
            // Find the ID of the next available place record.
            string sql = "SELECT MAX(ID) AS NewID FROM tbl_Places;";
            OleDbCommand sqlCommand = new OleDbCommand(sql, cndb_);
            int placeIdx = 0;
            try
            {
                placeIdx = int.Parse(sqlCommand.ExecuteScalar().ToString());
            }
            catch { }
            placeIdx++;

            // Clean the name.
            placeName = cleanPlaceName(placeName);

            // Insert the new place.
            sql = "INSERT INTO tbl_Places (ID, Name, ParentID, Status) VALUES (" + placeIdx.ToString() + ", \"" + placeName + "\", " + parentIdx.ToString() + ", 0);";
            sqlCommand = new OleDbCommand(sql, cndb_);
            sqlCommand.ExecuteNonQuery();

            // Return the ID of this new place.
            return placeIdx;
        }



        /// <summary>Returns the specified compound place string in html format.  Each place in the compound place with have a html link.</summary>
        /// <param name="placeName">Specifies the compound place string.</param>
        /// <param name="parentIdx">Specifies the ID of the place above the compound place string.</param>
        /// <returns>A html string to represent the specified compound place.</returns>
        private string placeToHtml(string placeName, int parentIdx)
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
            int placeIdx = getPlaceIdx(head, parentIdx);

            // Can not encode this place.
            if (placeIdx == 0)
            {
                if (tail == "")
                {
                    return head;
                }
                return tail + ", " + head;
            }

            // Encode this place into linked html.
            string placeHtml = "<a href=\"place:" + placeIdx.ToString() + "\">" + head + "</a>";

            // Deal with the tail.
            if (tail != "")
            {
                return placeToHtml(tail, placeIdx) + ", " + placeHtml;
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
        /// <param name="parentIdx">Specifies the ID parent place above the compound location string.</param>
        /// <returns>A string to represent the place in a gedcom file.</returns>
        private string placeToGedcom(string placeName, int status, int parentIdx)
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
            int placeIdx = getPlaceIdx(head, parentIdx);

            // Can not encode this place.
            if (placeIdx == 0)
            {
                return "";
            }

            // Get the status of this place.
            string sql = "SELECT Status FROM tbl_Places WHERE ID = " + placeIdx.ToString() + ";";
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
                tail = placeToGedcom(tail, status, placeIdx);
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
        public bool writeGedcomPlace(StreamWriter file, int level, string fullPlace, Sources sources, GedcomOptions options)
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
                else if (fullPlace != "")
                {
                    file.WriteLine((level + 1).ToString() + " CTRY " + fullPlace);
                }
            }

            if (options.isAllElements && sources != null)
            {
                int[] idxs = sources.get();
                foreach (int idx in idxs)
                {
                    file.WriteLine((level + 1).ToString() + " SOUR @S" + idx.ToString("0000") + "@");
                }
            }

            // Return success.
            return true;
        }



        /// <summary>Removes all the place links for the specified object.</summary>
        /// <param name="objectTypeIdx">Specifies the type of object.</param>
        /// <param name="objectIdx">Specifies the ID of the object.</param>
        public void placeDelink(int objectTypeIdx, int objectIdx)
        {
            string sql = "DELETE FROM tbl_ToPlaces WHERE TypeID = " + objectTypeIdx.ToString() + " AND ObjectID = " + objectIdx.ToString() + ";";
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
            foreach (int placeIdx in placesDelete)
            {
                sql = "DELETE FROM tbl_Places WHERE ID = " + placeIdx.ToString() + ";";
                sqlCommand = new OleDbCommand(sql, cndb_);
                sqlCommand.ExecuteNonQuery();
            }

            // Return the number of places that are removed.
            return placesDelete.Count;
        }



        public Place[] getPlaces(int placeIdx)
        {
            // Build a list to contain the places.
            ArrayList places = new ArrayList();

            // Build the list of child places.
            string sql = "SELECT * FROM tbl_Places WHERE ParentID = " + placeIdx.ToString() + " ORDER BY Name;";
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
            // Open a dataset of editors.
            string sql = "SELECT Name FROM tbl_Editors ORDER BY Name;";
            OleDbCommand sqlCommand = new OleDbCommand(sql, cndb_);
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();

            // Create an array list of the editors.
            ArrayList editors = new ArrayList();
            while (dataReader.Read())
            {
                editors.Add(dataReader.GetString(0));
            }

            // Return the list of editors.
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
                int personIdx = getInt(dataReader, "PersonID", 0);
                html.Append("<a href=\"person:" + personIdx.ToString() + "\">");
                Person person = new Person(personIdx, this);
                html.Append(person.getName(true, true));
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



        /// <summary>Returns a string that can be inserted into an Access SQL command as the value for a date time field.</summary>
        /// <param name="value">Specifies the value for a SQL date/time field.</param>
        /// <returns>A string that can be inserted into an Access SQL command.</returns>
        public static string toDb(CompoundDate value)
        {
            // Check for a NULL date
            if (value.status == 15)
            {
                return "NULL";
            }

            // Return the date.
            return "#" + value.date.ToString("d-MMM-yyyy") + "#";
        }



        /// <summary>Returns a string that can be inserted into an Access Sql command as the value for a date field.</summary>
        /// <param name="value">Specifies the value for a Sql date field.</param>
        /// <returns>A string that can be inserted into an Access Sql command.</returns>
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



        /// <summary>Returns a string that can be inserted into a sqlite3 SQL command as the value for a date time field.</summary>
        /// <param name="value">Specifies the value for a SQL date/time field.</param>
        /// <returns>A string that can be inserted into an sqlite3 SQL command.</returns>
        public static string toDate(CompoundDate value)
        {
            // Check for a NULL date
            if (value.status == 15)
            {
                return "NULL";
            }

            // Return the date.
            return "'" + value.date.ToString("yyyy-MM-dd") + "'";
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

        /// <summary>Connection to the sqlite3 database.</summary>
        internal SQLiteConnection sqlite { get { return sqlite_; } }

        /// <summary>Range of differing ages to present people as possible marriage partners.</summary>
        public int relationshipRange { get { return marriedRange_; } set { marriedRange_ = value; } }

        /// <summary>The filename of the source database file.</summary>
        public string fileName { get { return fileName_; } }

        /// <summary>This should really be something like user options.</summary>
        public string googleMapsKey { get { return googleMapsKey_; } }



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
            file.WriteLine("CREATE TABLE PEOPLE ('ID' INTEGER PRIMARY KEY, 'SURNAME' TEXT, 'FORENAMES' TEXT, 'MAIDEN_NAME' TEXT, 'BORN' DATE, BORN_STATUS INTEGER, DIED DATE, DIED_STATUS INTEGER, FATHER_ID INTEGER, MOTHER_ID INTEGER);");
            file.WriteLine("BEGIN TRANSACTION;");
            // Get the list of people.
            string sql = "SELECT ID, Surname, Forenames, MaidenName, Born, BornStatusID, Died, DiedStatusID, FatherID, MotherID FROM tbl_People ORDER BY ID;";
            OleDbCommand sqlCommand = new OleDbCommand(sql, cndb_);
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                file.Write("INSERT INTO PEOPLE (ID, SURNAME, FORENAMES, MAIDEN_NAME) VALUES(");
                file.Write(walton.Database.toDb(walton.Database.getInt(dataReader, "ID", 0), 0) + ", ");
                file.Write(walton.Database.toDb(walton.Database.getString(dataReader, "Surname", "")) + ", ");
                file.Write(walton.Database.toDb(walton.Database.getString(dataReader, "Forenames", "")) + ", ");
                file.Write(walton.Database.toDb(walton.Database.getString(dataReader, "MaidenName", "")) + ", ");
                if (dataReader.IsDBNull(4))
                {
                    file.Write("NULL, ");
                }
                else
                {
                    file.Write(dataReader.GetDateTime(4).ToString("yyyy-MM-dd") + ", ");
                }
                file.Write(dataReader.GetInt16(5).ToString() + ", ");
                if (dataReader.IsDBNull(6))
                {
                    file.Write("NULL, ");
                }
                else
                {
                    file.Write(dataReader.GetDateTime(6).ToString("yyyy-MM-dd") + ", ");
                }
                file.Write(dataReader.GetInt16(7).ToString() + ", ");
                if (dataReader.IsDBNull(8))
                {
                    file.Write("NULL, ");
                }
                else
                {
                    file.Write(walton.Database.getInt(dataReader, "FatherID", 0).ToString() + ", ");
                }
                if (dataReader.IsDBNull(9))
                {
                    file.Write("NULL, ");
                }
                else
                {
                    file.Write(walton.Database.getInt(dataReader, "MotherID", 0).ToString() + ", ");
                }

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
            file.WriteLine("CREATE TABLE PLACES ('ID' INTEGER PRIMARY KEY, 'NAME' TEXT, 'PARENT_ID' INTEGER, 'STATUS' INTEGER, 'LONGITUDE' REAL, 'LATITUDE' REAL, 'GOOGLE_ZOOM' INTEGER, 'USE_PARENT_LOCATION' INTEGER, 'PRIVATE_COMMENTS' TEXT);");
            file.WriteLine("BEGIN TRANSACTION;");
            // Get the list of places.
            sql = "SELECT ID, Name, ParentID, Status, Longitude, Latitude, GoogleZoom, UseParentLocation, PrivateComments FROM tbl_Places ORDER BY ID;";
            sqlCommand = new OleDbCommand(sql, cndb_);
            dataReader = sqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                file.Write("INSERT INTO PLACES (ID, NAME, PARENT_ID, STATUS, LONGITUDE, LATITUDE, GOOGLE_ZOOM, USE_PARENT_LOCATION, PRIVATE_COMMENTS) VALUES(");
                file.Write(toDb(dataReader.GetInt32(0), 0) + ", ");
                file.Write(toDb(dataReader.GetString(1)) + ", ");
                file.Write(toDb(dataReader.GetInt32(2), 0) + ", ");
                file.Write(dataReader.GetInt32(3).ToString() + ", ");
                file.Write(walton.Database.toDb(walton.Database.getFloat(dataReader, "Longitude", -999f), -999f) + ", ");
                file.Write(walton.Database.toDb(walton.Database.getFloat(dataReader, "Latitude", -999f), -999f) + ", ");
                file.Write(walton.Database.toDb(walton.Database.getInt(dataReader, "GoogleZoom", 0), 0) + ", ");
                file.Write(walton.Database.toDb(walton.Database.getBool(dataReader, "UseParentLocation", false)) + ", ");
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
