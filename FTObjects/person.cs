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

namespace family_tree.objects
{
    /// <summary>Class to represent a person in a family tree database.</summary>
	public class Person
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
        private CompoundDate dob_;

        /// <summary>Date of death for the person.</summary>
        private CompoundDate dod_;

        /// <summary>ID of the person's father.</summary>
        private int fatherIndex_;

        /// <summary>ID of the person's mother.</summary>
        private int motherIndex_;

        /// <summary>True if the person is male.</summary>
        private bool isMale_;

        /// <summary>True if all the children of the person are known.</summary>
        private bool isAllChildrenKnown_;

        /// <summary>Index of the media object attached to this person.</summary>
        private int mediaIndex_;

        /// <summary>User comments for the person.</summary>
        private string comments_;

        /// <summary>True if this person should be included in the gedcom file, false otherwise.</summary>
        private bool isIncludeGedcom_;

        /// <summary>Short term comments / information for the person.  This is not saved in the database.</summary>
        private string tag_;

        /// <summary>Array of facts about this person.</summary>
        private ArrayList facts_;

        /// <summary>Array of relationships for this person.</summary>
        private ArrayList relationships_;

        /// <summary>Sources for the name data.</summary>
        private Sources sourcesName_;

        /// <summary>Sources for the date of birth data.</summary>
        private Sources sourcesDoB_;

        /// <summary>Sources for the date of death data.</summary>
        private Sources sourcesDoD_;

        /// <summary>All sources for this person.  Including the sources for non specific facts.</summary>
        private Sources sourcesNonSpecific_;

        /// <summary>Name of the user who wrote the last edit.</summary>
        private string lastEditBy_;

        /// <summary>Date and time of the last edit.</summary>
        private DateTime lastEditDate_;

        /// <summary>Collection of ToDo items about this person.</summary>
        private ArrayList toDo_;

        #endregion

        #region Constructors



        /// <summary>Creates an empty person object.</summary>
        public Person()
        {
            fatherIndex_ = 0;
            motherIndex_ = 0;
            isAllChildrenKnown_ = false;
            facts_ = null;
            dob_ = new CompoundDate();
            dod_ = new CompoundDate();
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



        /// <summary>Creates an empty person object in the specfied database.</summary>
        /// <param name="database">Specify the database to contain this person</param>
        public Person(Database database)
            : this() // This makes the program call the () constructor before the code is called.
        {
            database_ = database;
        }



        /// <summary>Create a person object from the specified database record.  Loads the specified person from the specified database.</summary>
        /// <param name="personIndex">Specify the ID of the person to load.</param>
        /// <param name="database">Specify the family tree database to load the person from.</param>
        public Person(int personIndex, Database database) : this(database) // This makes the program call the (Database) constructor before the code is called.
        {
            // Open the specified person.
            OleDbCommand sqlCommand = new OleDbCommand("SELECT Surname, Forenames, MaidenName, Born, BornStatusID, Died, DiedStatusID, FatherID, MotherID, Sex, ChildrenKnown, GedCom, Comments, MediaID, LastEditBy, LastEditDate FROM tbl_People WHERE ID = " + personIndex.ToString() + ";", database.cndb);
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            if (dataReader.Read())
            {
                personIndex_ = personIndex;
                if (dataReader.IsDBNull(0))
                {
                    personSurname_ = "";
                }
                else
                {
                    personSurname_ = dataReader.GetString(0);
                }
                if (dataReader.IsDBNull(1))
                {
                    foreNames_ = "";
                }
                else
                {
                    foreNames_ = dataReader.GetString(1);
                }
                if (dataReader.IsDBNull(2))
                {
                    maidenName_ = "";
                }
                else
                {
                    maidenName_ = dataReader.GetString(2);
                }
                if (dataReader.IsDBNull(3))
                {
                    dob_.status = CompoundDate.EMPTY;
                }
                else
                {
                    dob_.date = dataReader.GetDateTime(3);
                    dob_.status = dataReader.GetInt16(4);
                }

                if (dataReader.IsDBNull(5))
                {
                    dod_.status = CompoundDate.EMPTY;
                }
                else
                {
                    dod_.date = dataReader.GetDateTime(5);
                    dod_.status = dataReader.GetInt16(6);
                }
                if (dataReader.IsDBNull(7))
                {
                    fatherIndex_ = 0;
                }
                else
                {
                    fatherIndex_ = dataReader.GetInt32(7);
                }
                if (dataReader.IsDBNull(8))
                {
                    motherIndex_ = 0;
                }
                else
                {
                    motherIndex_ = dataReader.GetInt32(8);
                }
                if (dataReader.IsDBNull(9))
                {
                    isMale_ = true;
                }
                else
                {
                    if (dataReader.GetString(9) == "M")
                    {
                        isMale_ = true;
                    }
                    else
                    {
                        isMale_ = false;
                    }
                }

                isAllChildrenKnown_ = dataReader.GetBoolean(10);
                isIncludeGedcom_ = walton.Database.getBool(dataReader, "Gedcom", true);
                comments_ = Database.getString(dataReader, "Comments", "");
                mediaIndex_ = Database.getInt(dataReader, "MediaID", 0);
                lastEditBy_ = Database.getString(dataReader, "LastEditBy", "Steve Walton");
                lastEditDate_ = Database.getDateTime(dataReader, "LastEditDate", DateTime.Now);
            }
            dataReader.Close();
        }

        #endregion

        #region Save & GetName



        /// <summary>Save the person into the database.</summary>
        /// <returns>True for success, false otherwise.</returns>
        public bool save()
        {
            // If you create a new record (ID changes from 0) then (does no harm in any case).
            OleDbCommand sqlCommand;
            if (personIndex_ == 0)
            {
                // Find the new ID.
                sqlCommand = new OleDbCommand("SELECT MAX(ID) AS NewID FROM tbl_People;", database_.cndb);
                personIndex_ = (int)sqlCommand.ExecuteScalar() + 1;

                // Create a new person record.
                sqlCommand = new OleDbCommand("INSERT INTO tbl_People (ID,Surname) VALUES (" + personIndex_.ToString() + "," + Database.toDb(personSurname_) + ");", database_.cndb);
                sqlCommand.ExecuteNonQuery();

                // Update the related child records.
                if (sourcesName_ != null)
                {
                    sourcesName_.personIndex = personIndex_;
                }
                if (sourcesDoB_ != null)
                {
                    sourcesDoB_.personIndex = personIndex_;
                }
                if (sourcesDoD_ != null)
                {
                    sourcesDoD_.personIndex = personIndex_;
                }
                if (sourcesNonSpecific_ != null)
                {
                    sourcesNonSpecific_.personIndex = personIndex_;
                }
            }
            else
            {
                // Update the places associated with this person.
                database_.placeDelink(1, personIndex_);
            }

            // Update the existing record.
            sqlCommand = new OleDbCommand
                (
                    "UPDATE tbl_People SET " +
                    "Surname = " + Database.toDb(personSurname_) + ", " +
                    "Forenames = " + Database.toDb(foreNames_) + ", " +
                    "MaidenName = " + Database.toDb(maidenName_) + ", " +
                    "Born = " + Database.toDb(dob_) + ", " +
                    "BornStatusID = " + dob_.status.ToString() + ", " +
                    "Died = " + Database.toDb(dod_) + ", " +
                    "DiedStatusID = " + dod_.status.ToString() + ", " +
                    "ChildrenKnown = " + Database.toDb(isAllChildrenKnown_) + ", " +
                    "FatherID = " + Database.toDb(fatherIndex_, 0) + ", " +
                    "MotherID = " + Database.toDb(motherIndex_, 0) + ", " +
                    "Sex = " + Database.iif(isMale_, "'M'", "'F'") + ", " +
                    "GedCom = " + walton.Database.toDb(isIncludeGedcom_) + ", " +
                    "MediaID = " + Database.toDb(mediaIndex_, 0) + ", " +
                    "LastEditBy = " + Database.toDb(lastEditBy_) + ", " +
                    "LastEditDate = #" + DateTime.Now.ToString("d-MMM-yyyy HH:mm:ss") + "#, " +
                    "Comments = " + Database.toDb(comments_) + " " +
                    "WHERE ID = " + personIndex_.ToString() + ";",
                    database_.cndb
                );
            sqlCommand.ExecuteNonQuery();

            // Save the sources
            if (sourcesName_ != null)
            {
                sourcesName_.save();
                foreach (int nSourceID in sourcesName_.get())
                {
                    sourceNonSpecific.add(nSourceID);
                }
            }
            if (sourcesDoB_ != null)
            {
                sourcesDoB_.save();
                foreach (int nSourceID in sourcesDoB_.get())
                {
                    sourceNonSpecific.add(nSourceID);
                }
            }
            if (sourcesDoD_ != null)
            {
                sourcesDoD_.save();
                foreach (int nSourceID in sourcesDoD_.get())
                {
                    sourceNonSpecific.add(nSourceID);
                }
            }

            // Save the facts don't bother to attach them to the person. We are destroying the person object shortly.
            if (facts_ != null)
            {
                foreach (Fact fact in facts_)
                {
                    fact.save();

                    // Make sure that all the fact sources are included in the non specific sources for this person.
                    foreach (int sourceIndex in fact.sources.get())
                    {
                        sourceNonSpecific.add(sourceIndex);
                    }
                }
            }

            // Save the ToDo items.
            if (toDo_ != null)
            {
                foreach (ToDo toDo in toDo_)
                {
                    toDo.save(database.cndb);
                }
            }

            // Save the relationship records.
            if (relationships_ != null)
            {
                foreach (Relationship relationship in relationships_)
                {
                    relationship.save();

                    // Add the location of this relationship.
                    if (relationship.location != "")
                    {
                        database_.addPlace(relationship.location, 1, personIndex_);
                    }

                    // Make sure that all the relationship sources are included in the non specific source for this person.
                    foreach (int sourceIndex in relationship.sourcePartner.get())
                    {
                        sourceNonSpecific.add(sourceIndex);
                    }
                    foreach (int sourceIndex in relationship.sourceStart.get())
                    {
                        sourceNonSpecific.add(sourceIndex);
                    }
                    foreach (int sourceIndex in relationship.sourceLocation.get())
                    {
                        sourceNonSpecific.add(sourceIndex);
                    }
                    foreach (int sourceIndex in relationship.sourceTerminated.get())
                    {
                        sourceNonSpecific.add(sourceIndex);
                    }
                    foreach (int sourceIndex in relationship.sourceEnd.get())
                    {
                        sourceNonSpecific.add(sourceIndex);
                    }
                }
            }

            // Add the locations associated with this person.
            string bornLocation = getBornLocation(false, "");
            if (bornLocation != "")
            {
                database_.addPlace(bornLocation, 1, personIndex_);
            }
            string diedLocation = getSimpleFact(90);
            if (diedLocation != "")
            {
                database_.addPlace(diedLocation, 1, personIndex_);
            }
            CensusPerson[] censusPeople = database_.censusForPerson(personIndex_);
            foreach (CensusPerson censusPerson in censusPeople)
            {
                database_.addPlace(censusPerson.houseHoldName, 1, personIndex_);
            }

            // Save the non specifiic sources.  This list may have been added to in the above.
            if (sourcesNonSpecific_ != null)
            {
                sourcesNonSpecific_.save();
            }

            // Return success.
            return true;
        }



        /// <summary>Returns the full name of the person.  If bShowYears is true then the birth and death year are shown in brackets after the name.  If bBirthName is true, then for women the original name is shown otherwise the married name with a nee is shown.</summary>
        /// <param name="isShowYears">Specify true for the DoB-DoD years to be added to the string.</param>
        /// <param name="isBirthName">Specify true for the birth name.  False for nee maiden name.  For females only.</param>
        /// <returns>The full name of the person.</returns>
        public string getName(bool isShowYears, bool isBirthName)
        {
            StringBuilder fullName = new StringBuilder(foreNames_);
            if (fullName.Length > 0)
            {
                fullName.Append(" ");
            }
            if (isBirthName)
            {
                if (maidenName_ == null)
                {
                    fullName.Append(personSurname_);
                }
                else if (maidenName_.Length > 0)
                {
                    fullName.Append(maidenName_);
                }
                else
                {
                    fullName.Append(personSurname_);
                }
            }
            else
            {
                fullName.Append(personSurname_);
                if (maidenName_ != null)
                {
                    if (maidenName_.Length > 0)
                    {
                        fullName.Append(" neé ");
                        fullName.Append(maidenName_);
                    }
                }
            }

            // Add the birth and death years.
            if (isShowYears)
            {
                fullName.Append(" (");
                fullName.Append(dob_.format(DateFormat.YEAR_ONLY, ""));
                if (!dod_.isEmpty())
                {
                    fullName.Append("-");
                    fullName.Append(dod_.format(DateFormat.YEAR_ONLY, ""));
                }
                fullName.Append(")");
            }

            // Return the built string.
            return fullName.ToString();
        }



        #endregion

        #region Parents



        /// <summary>Gets a collection of IndexName pairs representing the peiple who could possibly be the father of this person.  It is intended that this function will populate a list box.</summary>
        /// <returns>An array of clsIDName pairs representing people.</returns>
        public IndexName[] possibleFathers()
        {
            int startYear = dob_.date.Year - 100;
            int endYear = dob_.date.Year - 10;
            return database_.getPeople(ChooseSex.MALE, SortOrder.DATE, startYear, endYear);
        }



        /// <summary>Gets a collection of clsIDName pairs representing the peiple who could possibly be the father of this person.  It is intended that this function will populate a list box.</summary>
        /// <returns>An array of IndexName pairs representing people.</returns>
        public IndexName[] possibleMothers()
        {
            int startYear = dob_.date.Year - 100;
            int endYear = dob_.date.Year - 10;
            return database_.getPeople(ChooseSex.FEMALE, SortOrder.DATE, startYear, endYear);
        }



        #endregion

        #region Children



        /// <summary>Returns an array of person IDs representing the children of this person.</summary>
        /// <returns>An array of person ID representing the children of this person.</returns>
        public int[] getChildren()
        {
            // Start a list of the children.
            ArrayList children = new ArrayList();

            // Open the list of children of this person.
            OleDbCommand sqlCommand;
            if (isMale_)
            {
                sqlCommand = new OleDbCommand("SELECT ID FROM tbl_People WHERE FatherID = " + personIndex_.ToString() + " ORDER BY Born;", database_.cndb);
            }
            else
            {
                sqlCommand = new OleDbCommand("SELECT ID FROM tbl_People WHERE MotherID = " + personIndex_.ToString() + " ORDER BY Born;", database_.cndb);
            }

            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                children.Add(dataReader.GetInt32(0));
            }
            dataReader.Close();

            // Return the list as a integer array
            return (int[])children.ToArray(typeof(int));
        }



        /// <summary>Returns true if the person has children, false otherwise.</summary>
        /// <returns></returns>
        public bool hasChildren()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT ID FROM tbl_People WHERE ");
            if (isMale_)
            {
                sql.Append("FatherID");
            }
            else
            {
                sql.Append("MotherID");
            }
            sql.Append(" = " + personIndex_.ToString() + ";");
            OleDbCommand sqlCommand = new OleDbCommand(sql.ToString(), database_.cndb);
            Object children = sqlCommand.ExecuteScalar();
            if (children == null)
            {
                return false;
            }
            return true;
        }



        #endregion

        #region Siblings



        /// <summary>Returns an array of person ID represents the siblings of this person.  This includes half-siblings.</summary>
        /// <returns>An array of person ID represents the siblings of this person.</returns>
        public int[] getSiblings()
        {
            // Initialise variables.
            ArrayList siblings = new ArrayList();

            // Open the list of siblings of this person.
            OleDbCommand sqlCommand = new OleDbCommand("SELECT ID FROM tbl_People WHERE ID <> " + personIndex_.ToString() + " AND (FatherID = " + fatherIndex_.ToString() + " OR MotherID = " + motherIndex_.ToString() + ") ORDER BY Born;", database_.cndb);

            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                siblings.Add(dataReader.GetInt32(0));
            }
            dataReader.Close();

            // Return the list as a integer array
            return (int[])siblings.ToArray(typeof(int));
        }



        #endregion

        #region Relationships



        /// <summary>Gets a collection of clsIDName pairs representing the people who could possibly be in a relationship with the person.
        /// It is intended that this function will populate a list box.
        /// </summary>
        /// <returns>An array clsIDName[] pairs representing people</returns>
        public IndexName[] possiblePartners()
        {
            ChooseSex sex = isMale_ ? ChooseSex.FEMALE : ChooseSex.MALE;
            int startYear = dob_.date.Year - database_.relationshipRange;
            int endYear = dob_.date.Year + database_.relationshipRange;

            // Return the collection of people.
            return database_.getPeople(sex, SortOrder.DATE, startYear, endYear);
        }



        /// <summary>Returns an array of clsRelationship objects representing the relationships for this person.</summary>
        /// <returns>An array of clsRelationships objects representing the relationships for this person.</returns>
        public Relationship[] getRelationships()
        {
            if (relationships_ == null)
            {
                loadRelationships();
            }

            // Return the relationships as an array			
            return (Relationship[])relationships_.ToArray(typeof(Relationship));
        }



        /// <summary>Adds a relationship to the person.</summary>
        /// <param name="relationship">Specify the relationship to add to the collection of relationships/</param>
        /// <returns>True for success, false otherwise.</returns>
        public bool addRelationship(Relationship relationship)
        {
            // Load the existing relationships if required.
            if (relationships_ == null)
            {
                loadRelationships();
            }

            // Add the new relationship.
            relationships_.Add(relationship);

            // Return success.
            return true;
        }



        /// <summary>Loads the relationships for this person from the database.</summary>
        /// <returns>True for success, false otherwise.</returns>
        private bool loadRelationships()
        {
            // Initialise variables.
            relationships_ = new ArrayList();

            // Open the list of partners of this person.
            OleDbCommand sqlCommand = null;
            if (isMale_)
            {
                sqlCommand = new OleDbCommand("SELECT ID, FemaleID, TerminatedID, TheDate, StartStatusID, TerminateDate, TerminateStatusID, Location, Comments, RelationshipID, LastEditBy, LastEditDate FROM tbl_Relationships WHERE MaleID = " + personIndex_.ToString() + " ORDER BY TheDate DESC;", database_.cndb);
            }
            else
            {
                sqlCommand = new OleDbCommand("SELECT ID, MaleID, TerminatedID, TheDate, StartStatusID, TerminateDate, TerminateStatusID, Location, Comments, RelationshipID, LastEditBy, LastEditDate FROM tbl_Relationships WHERE FemaleID = " + personIndex_.ToString() + " ORDER BY TheDate DESC;", database_.cndb);
            }

            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                Relationship relationship = new Relationship(dataReader.GetInt32(0), this, dataReader.GetInt32(1));
                relationship.terminatedIndex = dataReader.GetInt32(2);
                if (dataReader.IsDBNull(3))
                {
                    relationship.start.status = CompoundDate.EMPTY;
                }
                else
                {
                    relationship.start.date = dataReader.GetDateTime(3);
                    relationship.start.status = dataReader.GetInt16(4);
                }
                if (dataReader.IsDBNull(5))
                {
                    relationship.end.status = CompoundDate.EMPTY;
                }
                else
                {
                    relationship.end.date = dataReader.GetDateTime(5);
                    relationship.end.status = dataReader.GetInt16(6);
                }
                if (dataReader.IsDBNull(7))
                {
                    relationship.location = "";
                }
                else
                {
                    relationship.location = dataReader.GetString(7);
                }
                if (dataReader.IsDBNull(8))
                {
                    relationship.comments = "";
                }
                else
                {
                    relationship.comments = dataReader.GetString(8);
                }
                relationship.typeIndex = dataReader.GetInt16(9);
                if (dataReader.IsDBNull(10))
                {
                    relationship.lastEditBy = "Steve Walton";
                }
                else
                {
                    relationship.lastEditBy = dataReader.GetString(10);
                }
                if (dataReader.IsDBNull(11))
                {
                    relationship.lastEditDate = DateTime.Now;
                }
                else
                {
                    relationship.lastEditDate = dataReader.GetDateTime(11);
                }
                relationship.isDirty = false;

                relationships_.Add(relationship);
            }
            dataReader.Close();

            // Return success.
            return true;
        }



        #endregion

        #region Facts



        /// <summary>Adds a fact to the list of facts for this person.</summary>
        /// <param name="fact">Specifies the fact to add to this person.</param>
        /// <returns>True for success.  False, otherwise.</returns>
        public bool addFact(Fact fact)
        {
            // Check if the facts array exists
            if (facts_ == null)
            {
                facts_ = new ArrayList();
            }

            // Add to the list of facts.
            facts_.Add(fact);

            // Return success.
            return true;
        }



        /// <summary>Loads all the facts from the database.</summary>
        /// <returns>True for success.  False, otherwise.</returns>
        private bool getAllFacts()
        {
            // Initailise the list of facts.
            facts_ = new ArrayList();

            // Get the list of facts from the database.
            OleDbCommand sqlCommand = new OleDbCommand("SELECT ID, TypeID, Rank, Information FROM tbl_Facts WHERE PersonID = " + personIndex_.ToString() + " ORDER BY Rank;", database_.cndb);
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            int rank = 0;
            while (dataReader.Read())
            {
                if (dataReader.IsDBNull(2))
                {
                    rank++;
                }
                else
                {
                    rank = dataReader.GetInt32(2);
                }
                Fact fact = new Fact(dataReader.GetInt32(0), this, dataReader.GetInt32(1), rank, dataReader.GetString(3));
                facts_.Add(fact);
            }
            dataReader.Close();

            // Return success.
            return true;
        }



        /// <summary>Returns an array of facts of the specified type.</summary>
        /// <param name="factTypeIndex">Specify the type of fact.</param>
        /// <returns>An array of facts of the required type.</returns>
        public Fact[] getFacts(int factTypeIndex)
        {
            // Check that some facts exist.
            if (facts_ == null)
            {
                // Open the facts
                getAllFacts();
            }

            // Build a list of relivant facts.
            ArrayList result = new ArrayList();
            foreach (Fact fact in facts_)
            {
                if (fact.typeIndex == factTypeIndex && fact.isValid())
                {
                    result.Add(fact);
                }
            }

            // Return the list of facts.
            return (Fact[])(result.ToArray(typeof(Fact)));
        }



        /// <summary>Returns all the facts.</summary>
        /// <returns>Array of all facts for this person.</returns>
        public Fact[] getFacts()
        {
            // Check that some facts exist.
            if (facts_ == null)
            {
                // Open the facts.
                getAllFacts();
            }

            return (Fact[])(facts_.ToArray(typeof(Fact)));
        }



        /// <summary>Returns the information field from the first fact of the specified type.  This is intended to be used where a fact type only has a single value.</summary>
        /// <param name="factTypeIndex">Specifies the type of fact to return.</param>
        /// <returns>The information field as a string.</returns>
        public string getSimpleFact(int factTypeIndex)
        {
            Fact[] facts = getFacts(factTypeIndex);
            if (facts.Length == 0)
            {
                return "";
            }
            return facts[0].information;
        }



        /// <summary>Returns the first fact of the specified type or null.</summary>
        /// <param name="factTypeIndex">Specifies the type of fact to return.</param>
        /// <returns>The first fact of the specified type or null.</returns>
        public Fact getFirstFact(int factTypeIndex)
        {
            Fact[] facts = getFacts(factTypeIndex);
            // Check that some facts exist.
            if (facts.Length == 0)
            {
                return null;
            }

            return facts[0];

        }

        #endregion

        #region Description



        /// <summary>A long description of the person, covering most all of the facts about this person.</summary>
        /// <param name="isHtml">Specify true for a html string, false for a plain ASCII text string.</param>
        /// <param name="isFootnotes">Specify true for footnotes that detail the sources.  (Really RTF only).</param>
        /// <param name="isShowImages">Specify true to show images in the description.  Only in html format output.</param>
        /// <returns>A long description of the person.</returns>
        public string getDescription(bool isHtml, bool isFootnotes, bool isShowImages, bool isIncludePlaces, bool isIncludeToDo)
        {
            // Initialise a string to hold the result.
            StringBuilder description = new StringBuilder();

            // Initialise the footnotes.
            IndexName[] sources = null;
            char[] footnoteCharacter = null;
            StringBuilder footnote = null;
            char nextChar = 'A';
            if (isFootnotes)
            {
                nextChar = 'A';
                footnote = new StringBuilder();
                sources = database_.getSources(SortOrder.DATE);
                footnoteCharacter = new char[sources.Length];
                for (int i = 0; i < sources.Length; i++)
                {
                    footnoteCharacter[i] = ' ';
                }

                // Add the non specific footnotes now.  To get the requested order.
                getFootnote(this.sourceNonSpecific, sources, footnoteCharacter, footnote, ref nextChar);
            }

            // Initialise the html.
            if (isHtml && isIncludeToDo)
            {
                description.Append("<p>");
            }

            if (isHtml)
            {
                // Primary image.
                if (isShowImages)
                {
                    if (mediaIndex_ != 0)
                    {
                        Media primaryMedia = new Media(database_, mediaIndex_);
                        description.Append("<a href=\"media:" + mediaIndex_.ToString() + "\">");
                        description.Append("<img align=\"right\" src=\"" + primaryMedia.fullFileName + "\" border=\"no\" alt=\"" + primaryMedia.title + "\" height=\"" + primaryMedia.heightForSpecifiedWidth(150) + "\" width=\"150\" />");
                        description.Append("</a>");
                    }
                }
            }

            // Name.
            if (isHtml)
            {
                description.Append("<a href=\"Person:" + personIndex_.ToString() + "\">");
            }
            description.Append(getName(false, true));
            if (isHtml)
            {
                description.Append("</a>");
            }
            if (isFootnotes)
            {
                description.Append(getFootnote(sourceName, sources, footnoteCharacter, footnote, ref nextChar));
            }

            // Born.
            if (dob.isEmpty())
            {
                description.Append(" not known when " + thirdPerson(false) + " was born");
            }
            else
            {
                description.Append(" was born ");
                description.Append(dob.format(DateFormat.FULL_LONG, CompoundDate.DatePrefix.ON_IN_BEFORE_AFTER));
                if (isFootnotes)
                {
                    description.Append(getFootnote(sourceDoB, sources, footnoteCharacter, footnote, ref nextChar));
                }
            }
            Fact[] facts = getFacts(10);
            if (facts.Length > 0)
            {
                description.Append(" in ");
                if (isHtml)
                {
                    description.Append(database_.placeToHtml(facts[0].information));
                }
                else
                {
                    description.Append(facts[0].information);
                }
                if (isFootnotes)
                {
                    description.Append(getFootnote(facts[0].sources, sources, footnoteCharacter, footnote, ref nextChar));
                }
            }
            description.Append(". ");

            // Relationships.
            Relationship[] relationships = getRelationships();
            for (int i = relationships.Length - 1; i >= 0; i--)
            {
                if (relationships[i].isValid() && relationships[i].isMarried())
                {
                    Person oRelation = database_.getPerson(relationships[i].partnerIndex);
                    if (relationships[i].start.isEmpty())
                    {
                        description.Append(thirdPerson(true));
                    }
                    else
                    {
                        description.Append(relationships[i].start.format(DateFormat.FULL_LONG, CompoundDate.DatePrefix.ON_IN_BEFORE_AFTER_CAPTIALS));
                        if (isFootnotes)
                        {
                            description.Append(getFootnote(relationships[i].sourceStart, sources, footnoteCharacter, footnote, ref nextChar));
                        }
                        if (!this.dob.isEmpty())
                        {
                            description.Append(" when " + thirdPerson(false) + " was " + this.getAge(relationships[i].start) + " old");
                        }
                        description.Append(", " + thirdPerson(false));
                    }
                    description.Append(" married ");
                    if (isHtml)
                    {
                        description.Append("<a href=\"Person:" + oRelation.index.ToString() + "\">");
                    }
                    description.Append(oRelation.getName(false, true));
                    if (isHtml)
                    {
                        description.Append("</a>");
                    }
                    if (isFootnotes)
                    {
                        description.Append(getFootnote(relationships[i].sourcePartner, sources, footnoteCharacter, footnote, ref nextChar));
                    }
                    if (relationships[i].location.Length > 0)
                    {
                        description.Append(" at ");
                        if (isHtml)
                        {
                            description.Append(database_.placeToHtml(relationships[i].location));
                        }
                        else
                        {
                            description.Append(relationships[i].location);
                        }

                        if (isFootnotes)
                        {
                            description.Append(getFootnote(relationships[i].sourceLocation, sources, footnoteCharacter, footnote, ref nextChar));
                        }
                    }

                    description.Append(". ");

                    if (relationships[i].terminatedIndex != 1)
                    {
                        bool bTerminated = true;
                        switch (relationships[i].terminatedIndex)
                        {
                        case 2:
                            description.Append("They got divorced");
                            break;
                        case 3:
                            if (isMale_)
                            {
                                bTerminated = false;
                            }
                            else
                            {
                                description.Append("He died");
                            }
                            break;
                        case 4:
                            if (isMale_)
                            {
                                description.Append("She died");
                            }
                            else
                            {
                                bTerminated = false;
                            }
                            break;
                        }
                        if (bTerminated)
                        {
                            if (isFootnotes)
                            {
                                description.Append(getFootnote(relationships[i].sourceTerminated, sources, footnoteCharacter, footnote, ref nextChar));
                            }

                            if (!relationships[i].end.isEmpty())
                            {
                                description.Append(" " + relationships[i].end.format(DateFormat.FULL_LONG, CompoundDate.DatePrefix.ON_IN_BEFORE_AFTER));
                                if (isFootnotes)
                                {
                                    description.Append(getFootnote(relationships[i].sourceEnd, sources, footnoteCharacter, footnote, ref nextChar));
                                }
                            }

                            description.Append(". ");
                        }
                    }
                }
            }

            // Education
            description.Append(showFacts(40, " was educated at ", "and", isHtml, isFootnotes, sources, footnoteCharacter, footnote, ref nextChar));

            // Occupation
            description.Append(showFacts(20, " worked as a ", "and", isHtml, isFootnotes, sources, footnoteCharacter, footnote, ref nextChar));

            // Interests
            description.Append(showFacts(30, " was interested in ", "", isHtml, isFootnotes, sources, footnoteCharacter, footnote, ref nextChar));

            // Comments
            description.Append(showFacts(100, " ", "", isHtml, isFootnotes, sources, footnoteCharacter, footnote, ref nextChar));

            // Children
            // Don't display children information for people who are known be less than 14 years old.
            int age = 15;
            if (!dod.isEmpty())
            {
                age = dod.date.Year - dob.date.Year;
            }
            else
            {
                age = DateTime.Now.Year - dob.date.Year;
            }
            if (age > 14)
            {
                int[] children = getChildren();
                if (isAllChildrenKnown)
                {
                    switch (children.Length)
                    {
                    case 0:
                        description.Append(thirdPerson(true) + " had no children. ");
                        break;
                    case 1:
                        description.Append(thirdPerson(true) + " had 1 child. ");
                        break;
                    default:
                        description.Append(thirdPerson(true) + " had " + children.Length.ToString() + " children. ");
                        break;
                    }
                }
                else
                {
                    switch (children.Length)
                    {
                    case 0:
                        break;
                    case 1:
                        description.Append(thirdPerson(true) + " had at least 1 child. ");
                        break;
                    default:
                        description.Append(thirdPerson(true) + " had at least " + children.Length.ToString() + " children. ");
                        break;
                    }
                }
            }

            // Census information.
            CensusPerson[] censuses = database_.censusForPerson(personIndex_);
            string lastLocation = "";
            foreach (CensusPerson census in censuses)
            {
                string location = "";
                if (isHtml)
                {
                    location = database_.placeToHtml(census.houseHoldName);
                }
                else
                {
                    location = census.houseHoldName;
                }
                if (location != lastLocation)
                {
                    description.Append(thirdPerson(true) + " lived at ");
                    description.Append(location);
                    lastLocation = location;
                }
                else
                {
                    description.Remove(description.Length - 2, 2);
                    description.Append(" and");
                }
                description.Append(" on " + census.date.ToString("d MMMM yyyy"));
                if (isFootnotes)
                {
                    description.Append(getFootnote(census.houseHoldIndex, sources, footnoteCharacter, footnote, ref nextChar, true));
                }
                description.Append(". ");
            }

            // Died.
            if (!dod.isEmpty())
            {
                description.Append(thirdPerson(true) + " died " + dod.format(DateFormat.FULL_LONG, CompoundDate.DatePrefix.ON_IN_BEFORE_AFTER));
                if (isFootnotes)
                {
                    description.Append(getFootnote(sourceDoD, sources, footnoteCharacter, footnote, ref nextChar));
                }
                facts = getFacts(90);
                if (facts.Length > 0)
                {
                    description.Append(" in ");
                    if (isHtml)
                    {
                        description.Append(database_.placeToHtml(facts[0].information));
                    }
                    else
                    {
                        description.Append(facts[0].information);
                    }
                    if (isFootnotes)
                    {
                        description.Append(getFootnote(facts[0].sources, sources, footnoteCharacter, footnote, ref nextChar));
                    }
                }
                if (!dob.isEmpty())
                {
                    description.Append(" when " + thirdPerson(false) + " was " + getAge(this.dod) + " old");
                }
                description.Append(". ");
            }
            if (isHtml && isIncludeToDo)
            {
                description.AppendLine("</p>");
            }

            // Display the comments.
            // isIncludeToDo is not really the "correct" flag.
            if (isHtml && isIncludeToDo && comments_ != string.Empty)
            {
                description.AppendLine("<p class=\"Small\" style=\"line-height: 100%\"><strong>Private Comments</strong>: " + comments_ + "</p>");
            }

            // Include the places.
            if (isHtml && isIncludePlaces)
            {
                description.AppendLine(places.googleMap(600, 300));
            }

            // Show the footnotes.
            if (isFootnotes)
            {
                if (footnote.Length > 0)
                {
                    description.AppendLine("<table cellpadding=\"3\" cellspacing=\"2\">");
                    description.AppendLine(footnote.ToString());
                    description.AppendLine("</table> ");
                }
                description.AppendLine("<p align=\"left\"><span class=\"Small\">Last Edit by " + lastEditBy + " on " + lastEditDate.ToString("d-MMM-yyyy HH:mm:ss") + "</span></p>");
            }

            // Show all the non primary images.
            if (isHtml && isShowImages)
            {
                int[] mediaIndexes = getMediaIndexes(true);
                if (mediaIndexes.Length > 0)
                {
                    description.AppendLine("<table>");
                    foreach (int mediaIndex in mediaIndexes)
                    {
                        Media media = new Media(database_, mediaIndex);
                        description.Append("<tr valign=\"top\">");
                        description.Append("<td>");
                        description.Append("<a href=\"media:" + mediaIndex.ToString() + "\">");
                        description.Append("<img src=\"" + media.fullFileName + "\" border=\"no\" height=\"" + media.heightForSpecifiedWidth(150) + "\" width=\"150\">");
                        description.Append("</a>");
                        description.Append("</td>");
                        description.Append("<td>" + media.title + "</td>");
                        description.AppendLine("</tr>");
                    }
                    description.AppendLine("</table>");
                }
            }

            // Show the ToDo items.
            if (isHtml && isIncludeToDo)
            {
                ToDo[] toDos = this.getToDo();
                if (toDos.Length > 0)
                {
                    description.AppendLine("<p><b>To Do</b></p>");
                    description.AppendLine("<table cellpadding=\"3\" cellspacing=\"2\">");
                    foreach (ToDo toDo in toDos)
                    {
                        description.Append("<tr bgcolor=\"silver\">");
                        description.Append("<td><span class=\"Small\">[" + toDo.priority.ToString() + "]</span></td>");
                        description.Append("<td><span class=\"Small\">" + toDo.description + "</span></td>");
                        description.AppendLine("</tr>");
                    }
                    description.AppendLine("</table>");
                }
            }

            // Return the description that has been built.
            return description.ToString();
        }



        /// <summary>Adds all the facts of the specified type to the description in human readable form.</summary>
		/// <param name="factTypeIndex">Specifies the fact type to add.</param>
		/// <param name="prefix">Specifies the prefix to make the fact human readable</param>
		/// <param name="joinWord">Specifies the join word for the last fact in a single sentence.  Use "" for a separate sentence for each fact.</param>
		/// <param name="isHtml">Specify true for a description in Html, false for plain ASCII text.</param>
		/// <param name="isFootnotes">Specify true to add footnote information.</param>
		/// <param name="sources">Specifies all the available sources of information.</param>
		/// <param name="footnoteCharacter">Specifies the character to use for each source.</param>
		/// <param name="footnote">Footnote sources text for the bottom of the page.</param>
		/// <param name="nextChar">Character to use for the next footnote marker.</param>
		/// <returns>Human readable string about the fact</returns>
		private string showFacts(int factTypeIndex, string prefix, string joinWord, bool isHtml, bool isFootnotes, IndexName[] sources, char[] footnoteCharacter, StringBuilder footnote, ref char nextChar)
        {
            // Start to build a string to return as the result.
            StringBuilder description = new StringBuilder();

            // Get the collection of facts and loop through them.
            Fact[] facts = this.getFacts(factTypeIndex);
            bool isFirst = true;
            bool isFullStop = false;
            for (int factCount = 0; factCount < facts.Length; factCount++)
            {
                if (isFirst)
                {
                    description.Append(this.thirdPerson(true) + prefix + facts[factCount].information);
                    if (factCount == facts.Length - 1)
                    {
                        // This is already the last one.
                        isFullStop = true;
                    }
                    else if (joinWord == "")
                    {
                        // Each fact has it's own sentense.
                        isFullStop = true;
                    }
                    else
                    {
                        // Use the join word for subsequent facts.
                        isFirst = false;
                    }
                }
                else
                {
                    if (factCount == facts.Length - 1)
                    {
                        // Last fact use the join word.
                        description.Append(" " + joinWord + " " + facts[factCount].information);
                        isFullStop = true;
                    }
                    else
                    {
                        // Imtermeadate fact just use a comma.
                        description.Append(", " + facts[factCount].information);
                    }
                }

                // Add a footnote (if required).
                if (isFootnotes)
                {
                    description.Append(getFootnote(facts[factCount].sources, sources, footnoteCharacter, footnote, ref nextChar));
                }

                // Add a full stop after the last fact.
                if (isFullStop)
                {
                    description.Append(". ");
                }
            }

            // Return the string built.
            return description.ToString();
        }



        /// <summary>Returns a footnote to include in the description.  The footnote is selected from the oSources collection.  The oAllSources collection gives the name of all sources.  The cFootnote gives the footnote character for each source.  The sbFootnote contains the footnote text for the bottom of the page.  The cNextChar contains the next footnote character to use for new sources.</summary>
        /// <param name="sources">Specifies the sources of information.</param>
        /// <param name="allSources">Specifies all the sources for this database.</param>
        /// <param name="footnoteCharacters">Specifies the footnote character for the database sources</param>
        /// <param name="footnote">Returns the text to include at the bottom of the description.</param>
        /// <param name="nextChar">Returns the next character to use as footnote marker.</param>
        /// <returns>The footnote to include in the description</returns>
        private string getFootnote(Sources sources, IndexName[] allSources, char[] footnoteCharacters, StringBuilder footnote, ref char nextChar)
        {
            StringBuilder result = new StringBuilder();
            int[] sourceIndexes = sources.get();
            for (int i = 0; i < sourceIndexes.Length; i++)
            {
                result.Append(getFootnote(sourceIndexes[i], allSources, footnoteCharacters, footnote, ref nextChar, false));
            }

            if (result.Length == 0)
            {
                return "";
            }

            return "<span class=\"superscript\">" + result.ToString() + "</span> ";
        }

        private string getFootnote(int sourceIndex, IndexName[] allSources, char[] footnoteCharacters, StringBuilder footnote, ref char nextChar, bool isHtml)
        {
            int index = 0;
            for (index = 0; allSources[index].index != sourceIndex; index++) ;
            if (footnoteCharacters[index] == ' ')
            {
                footnoteCharacters[index] = nextChar;
                nextChar++;

                footnote.Append("<tr bgcolor=\"silver\"><td><span class=\"Small\">" + footnoteCharacters[index].ToString() + "</span></td><td><a href=\"Source:" + allSources[index].index.ToString() + "\"><span class=\"Small\">" + allSources[index].name + "</span></a></td></tr>");
            }

            if (isHtml)
            {
                return "<span class=\"superscript\">" + footnoteCharacters[index].ToString() + "</span> ";
            }
            return footnoteCharacters[index].ToString();
        }



        /// <summary>Returns a 3 line description covering the DoB, birth location and DoD or age.</summary>
        /// <param name="isIncludeAge">Specify true to have the age of living people shown.</param>
        /// <returns>A short description of the person</returns>
        public string shortDescription(bool isIncludeAge)
        {
            if (dob.isEmpty())
            {
                return dob.format(DateFormat.FULL_LONG, "b. ") + getBornLocation(true, "\nb. ") + dod.format(DateFormat.FULL_LONG, "\nd. ");
            }
            if (dod.isEmpty())
            {
                if (dob.date.Year > DateTime.Now.Year - 110 && isIncludeAge)
                {
                    return dob.format(DateFormat.FULL_LONG, "b. ") + getBornLocation(true, "\nb. ") + "\nage " + getAge(DateTime.Now);
                }
                else
                {
                    return dob.format(DateFormat.FULL_LONG, "b. ") + getBornLocation(true, "\nb. ");
                }
            }
            return dob.format(DateFormat.FULL_LONG, "b. ") + getBornLocation(true, "\nb. ") + dod.format(DateFormat.FULL_SHORT, "\nd. ") + " (" + getAge(dod, false) + ")";
        }



        /// <summary>Returns the age of the person on the specified date.  A person who on the day that they are born should be considered to be 1 day old.  Ie no one is ever 0 days old.</summary>
        /// <param name="theDate">Specify the date to return the age of the person on.</param>
        /// <param name="isShowUnits">Specify true to add a label for the units.  Specify false to force a unlabelled number of years.</param>
        /// <returns>The age of the person on the specified date as a string.  Usually need to add the word "old" after the string.</returns>
        public string getAge(DateTime theDate, bool isShowUnits)
        {
            // Find the number of years between the two dates
            int theYears = theDate.Year - dob_.date.Year;
            int dayOfYearDiff = theDate.DayOfYear - dob_.date.DayOfYear;

            if (dayOfYearDiff < -2)
            {
                theYears--;
            }
            else if (dayOfYearDiff > 2)
            {
            }
            else
            {
                // Need an exact calculation here.
                if (theDate.Month <= dob_.date.Month && theDate.Day < dob_.date.Day)
                {
                    theYears--;
                }
            }

            // Return the duration as a string.
            if (theYears == 0)
            {
                //if(bUnits)
                //{
                TimeSpan theAge = theDate - dob_.date;
                return (theAge.Days + 1).ToString() + " days";
                //}
                //else
                //{
                //return "0";
                //}
            }
            else
            {
                if (isShowUnits)
                {
                    return theYears.ToString() + " years";
                }
                else
                {
                    return theYears.ToString();
                }
            }
        }



        /// <summary>Returns the age of the person on the specified date.</summary>
        /// <param name="compoundDate">Specifies the date on which to return the age of the person.</param>
        /// <param name="isShowUnits">Specify true to add a label for the units.  Specify false to force a unlabelled number of years.</param>
        /// <returns>The age of the person on the specified date as a string.  Usually need to add the word "old" after the string.</returns>
        public string getAge(CompoundDate compoundDate, bool isShowUnits)
        {
            return getAge(compoundDate.date, isShowUnits);
        }



        /// <summary>Returns the age of the person on the specified date.</summary>
        /// <param name="compoundDate">Specifies the date on which to return the age of the person.</param>
        /// <returns>The age of the person on the specified date as a string.  Usually need to add the word "old" after the string.</returns>
        public string getAge(CompoundDate compoundDate)
        {
            return getAge(compoundDate.date, true);
        }



        /// <summary>Returns the age of the person on the specified date.</summary>
        /// <param name="theDate">Specify the date to return the age of the person on.</param>
        /// <returns>The age of the person on the specified date as a string.  Usually need to add the word "old" after the string.</returns>
        public string getAge(DateTime theDate)
        {
            return getAge(theDate, true);
        }



        /// <summary>Returns he or she according to the person's sex.</summary>
        /// <param name="isCaptialLetter">True for He / She.  False for he / she.</param>
        /// <returns>Returns he or she according to the person's sex.</returns>
        public string thirdPerson(bool isCaptialLetter)
        {
            if (isCaptialLetter)
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



        /// <summary>Returns the born location fact.</summary>
        /// <param name="isShort">Specify true for a short location and false for the long location.</param>
        /// <param name="prefix">Specify a string to prefix a non null location with.</param>
        /// <returns>Location born.</returns>
        public string getBornLocation(bool isShort, string prefix)
        {
            string location;
            if (isShort)
            {
                location = getSimpleFact(11);
            }
            else
            {
                location = getSimpleFact(10);
            }
            if (location == "")
            {
                return "";
            }
            return prefix + location;
        }



        /// <summary>The collection of places associated with this person.</summary>
        public Places places
        {
            get
            {
                // Start a new collection of places.
                Places places = new Places();

                // Add the born place.
                Fact[] facts = getFacts(10);
                if (facts.Length > 0)
                {
                    string sBorn = facts[0].information;
                    Place oPlace = database_.getPlace(sBorn);
                    if (oPlace != null)
                    {
                        places.addPlace(oPlace);
                    }
                }

                // Add the married places
                Relationship[] relationships = getRelationships();
                foreach (Relationship relatonship in relationships)
                {
                    if (relatonship.isValid())
                    {
                        Place marriedPlace = database_.getPlace(relatonship.location);
                        if (marriedPlace != null)
                        {
                            places.addPlace(marriedPlace);
                        }
                    }
                }

                // Add the location of born children.
                int[] childrenIndexes = getChildren();
                foreach (int childIndex in childrenIndexes)
                {
                    Person child = new Person(childIndex, database_);
                    Fact[] childFacts = child.getFacts(10);
                    if (childFacts.Length > 0)
                    {
                        string childBorn = childFacts[0].information;
                        Place place = database_.getPlace(childBorn);
                        if (place != null)
                        {
                            places.addPlace(place);
                        }
                    }
                }

                // Add the census places.
                CensusPerson[] censuses = database_.censusForPerson(personIndex_);
                // string sLastLocation = "";
                foreach (CensusPerson census in censuses)
                {
                    Place household = database_.getPlace(census.houseHoldName);
                    places.addPlace(household);
                }

                // Add the died place.
                facts = getFacts(90);
                if (facts.Length > 0)
                {
                    string diedPlace = facts[0].information;
                    Place place = database_.getPlace(diedPlace);
                    if (place != null)
                    {
                        places.addPlace(place);
                    }
                }

                // Return the collection of places.
                return places;
            }
        }

        #endregion

        #region Media



        /// <summary>Returns an array of media object indexes associated with this person.</summary>
        /// <param name="isExcludePrimary">Specify true not to include the primary media object index.</param>
        /// <returns>An array of indexes of media objects.</returns>
        public int[] getMediaIndexes
            (
            bool isExcludePrimary
            )
        {
            // Decide if to exclude the primary media object
            int excludeIndex = -1;
            if (isExcludePrimary)
            {
                excludeIndex = mediaIndex_;
            }

            ArrayList media = new ArrayList();

            string sql = "SELECT MediaID FROM tbl_AdditionalMediaForPeople WHERE PersonID=" + personIndex_.ToString() + ";";
            OleDbCommand sqlCommand = new OleDbCommand(sql, database_.cndb);
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                int mediaIndex = walton.Database.getInt(dataReader, "MediaID", 0);
                if (mediaIndex != 0 && mediaIndex != excludeIndex)
                {
                    media.Add(mediaIndex);
                }
            }
            dataReader.Close();

            // Return the collection an array .
            return (int[])media.ToArray(typeof(int));
        }



        /// <summary>Returns an array of media objects associated with this person.</summary>
        /// <param name="isExcludePrimary">Specify true not to include the primary media object index.</param>
        /// <returns>An array of media objects.</returns>
        public Media[] getMedia(bool isExcludePrimary)
        {
            int[] mediaIndexes = getMediaIndexes(isExcludePrimary);
            Media[] result = new Media[mediaIndexes.Length];
            int i = 0;
            foreach (int mediaIndex in mediaIndexes)
            {
                Media media = new Media(database_, mediaIndex);
                result[i++] = media;
            }

            return result;
        }



        #endregion

        #region ToDo Items



        /// <summary>Returns the collection of ToDo items attached to this person.</summary>
        /// <returns>An array of ToDo items attached to this person.</returns>
        public ToDo[] getToDo()
        {
            // Check if the ToDo array exists.
            if (toDo_ == null)
            {
                loadToDo();
            }

            return (ToDo[])toDo_.ToArray(typeof(ToDo));
        }



        /// <summary>Loads the collection of ToDo items related this person.</summary>
        private void loadToDo()
        {
            // Create a list to hold the ToDo items.
            toDo_ = new ArrayList();

            string sql = "SELECT * FROM tbl_ToDo WHERE PersonID = " + this.index + " ORDER BY Priority, ID;";
            OleDbCommand sqlCommand = new OleDbCommand(sql, database_.cndb);
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                ToDo toDo = new ToDo(walton.Database.getInt(dataReader, "ID", -1), index, walton.Database.getInt(dataReader, "Priority", 0), walton.Database.getString(dataReader, "Description", ""));

                // Add this item to the collection.
                toDo_.Add(toDo);
            }
        }



        /// <summary>Adds the specified ToDo item to the collection for this person.</summary>
        /// <param name="toDo">Specifies the ToDo item to add to this person.</param>
        /// <returns>True for success, false otherwise.</returns>
        public bool addToDo(ToDo toDo)
        {
            // Check if the ToDo array exists.
            if (toDo_ == null)
            {
                loadToDo();
            }

            // Add to the list of to do.
            toDo_.Add(toDo);

            // Return success.
            return true;
        }



        #endregion

        #region General Public Properties

        #region Image



        /// <summary>Index of the media object attached to this person.</summary>
		public int mediaIndex { get { return mediaIndex_; } set { mediaIndex_ = value; } }



        /// <summary>Full filename for an image for the person.  Empty string if no image is specified or can not be found on the hard disk.</summary>
        public string getImageFilename()
        {
            // Check that a media object is attached to this person
            if (mediaIndex_ == 0)
            {
                return "";
            }

            // Find the full filename of the media object
            Media media = new Media(database_, mediaIndex_);
            return media.fullFileName;
        }



        #endregion

        /// <summary>The ID of the person in the database.</summary>
        public int index { get { return personIndex_; } set { personIndex_ = value; } }

        /// <summary>Database that this person is stored in.</summary>
        public Database database { get { return database_; } }

        /// <summary>The surname this person had at birth.</summary>
        public string birthSurname
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
        public CompoundDate dob { get { return dob_; } }

        /// <summary>Date of death of this person.</summary>
        public CompoundDate dod { get { return dod_; } }

        /// <summary>ID of the father of this person.  Zero is unknown father.</summary>
		public int fatherIndex { get { return fatherIndex_; } set { fatherIndex_ = value; } }

        /// <summary>ID of the mother of this person.  Zero is unknown mother.</summary>
		public int motherIndex { get { return motherIndex_; } set { motherIndex_ = value; } }

        /// <summary>True if this person is male.  False, otherwise.</summary>
        public bool isMale { get { return isMale_; } set { isMale_ = value; } }

        /// <summary>True if this person is female.  False, otherwise.</summary>
        public bool isFemale { get { return !isMale_; } set { isMale_ = !value; } }

        /// <summary>True if all the children of this person are known.  False, otherwise.</summary>
        public bool isAllChildrenKnown { get { return isAllChildrenKnown_; } set { isAllChildrenKnown_ = value; } }

        /// <summary>True if the person should be included in the gedcom file. False, otherwise.</summary>
        public bool isIncludeInGedcom { get { return isIncludeGedcom_; } set { isIncludeGedcom_ = value; } }

        /// <summary>User comments for this person.</summary>
        public string comments { get { return comments_; } set { comments_ = value; } }



        /// <summary>A sources object for the person name.</summary>
		public Sources sourceName
        {
            get
            {
                if (sourcesName_ == null)
                {
                    sourcesName_ = new Sources(personIndex_, 1, database_);
                }
                return sourcesName_;
            }
        }



        /// <summary>A clsSources object for the person date of birth.</summary>
        public Sources sourceDoB
        {
            get
            {
                if (sourcesDoB_ == null)
                {
                    sourcesDoB_ = new Sources(personIndex_, 2, database_);
                }
                return sourcesDoB_;
            }
        }



        /// <summary>A clsSources object for the person date of death.</summary>
        public Sources sourceDoD
        {
            get
            {
                if (sourcesDoD_ == null)
                {
                    sourcesDoD_ = new Sources(personIndex_, 3, database_);
                }
                return sourcesDoD_;
            }
        }



        /// <summary>A collection of all the sources used for this person.  Including the non specific sources.</summary>
        public Sources sourceNonSpecific
        {
            get
            {
                if (sourcesNonSpecific_ == null)
                {
                    sourcesNonSpecific_ = new Sources(personIndex_, 0, database_);
                }
                return sourcesNonSpecific_;
            }
        }



        /// <summary>Short term information about the person.  This is not saved in the database.</summary>
        public string tag
        {
            get { return tag_; }
            set { tag_ = value; }
        }



        /// <summary>Name of the user who wrote the last edit.</summary>
        public string lastEditBy { get { return lastEditBy_; } set { lastEditBy_ = value; } }

        /// <summary>Date and time of the last edit.</summary>
        public DateTime lastEditDate { get { return lastEditDate_; } set { lastEditDate_ = value; } }

        #endregion
    }
}
