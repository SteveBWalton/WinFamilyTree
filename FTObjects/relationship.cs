using System;
// Database.
using System.Data;
// Access database via ADO.NET.
using System.Data.OleDb;	
// Sqlite database.
// using System.Data.SQLite;
using System.Text;



namespace family_tree.objects
{
    #region Supporting Types etc

    /// <summary>Possible status of a relationship.</summary>
    public enum RelationshipStatus
    {
        /// <summary>Married.</summary>
        MARRIED,
        
        /// <summary>The relationship ended with a divorce.</summary>
        DIVORCED,
        
        /// <summary>Not really relevant here.</summary>
        NONE
    }

    #endregion

    /// <summary>Class to represent a relationship (like a marriage) between 2 people.  In gedcom this is a family object.</summary>
    public class Relationship
    {
        #region Member Variables

        /// <summary>ID of the record in the database.</summary>
        private int idx_;

        /// <summary>True if this record should be removed from the database.</summary>
        private bool isDelete_;

        /// <summary>Person who owns this relationship object.</summary>
        private Person owner_;

        /// <summary>ID of the partner.</summary>
        private int partnerIdx_;

        /// <summary>Is the relationship terminated.</summary>
        private int terminated_;

        /// <summary>ID of the male member of the relationship.</summary>
        private int maleIdx_;

        /// <summary>ID of the female member of the relationship.</summary>
        private int femaleIdx_;

        /// <summary>Start date of the relationship.</summary>
        private CompoundDate start_;

        /// <summary>Location of the start of the relationship.  Eg Married at ...</summary>
        private string location_;

        /// <summary>End date of the relationship.</summary>
        private CompoundDate end_;

        /// <summary>User comments on this relationship.</summary>
        private string comments_;

        /// <summary>Sources for the start date.</summary>
        private Sources sourcesStart_;

        /// <summary>Sources for the terminated status.</summary>
        private Sources sourcesTerminated_;

        /// <summary>Sources for the location.</summary>
        private Sources sourcesLocation_;

        /// <summary>Sources for the end date.</summary>
        private Sources sourcesEnd_;

        /// <summary>Sources for the partner name.</summary>
        private Sources sourcesPartner_;

        /// <summary>The source database.  This is probably null.</summary>
        private Database database_;

        /// <summary>Type of relationship. 1- Religious, 2-Civil, 3-CoHabit.  This is stored as RelationshipID in the database.</summary>
        private int typeIdx_;

        /// <summary>Name of the user who wrote the last edit.</summary>
        private string lastEditBy_;

        /// <summary>Date and time of the last edit.</summary>
        private DateTime lastEditDate_;

        /// <summary>True if this relationship is out of sync with the database and needs saving.</summary>
        private bool isDirty_;

        #endregion

        #region Constructors

        /// <summary>Class Constructor.  Builds an empty relationship object.  This can be used to create relationship objects with no owner.</summary>
        /// <param name="idx">Specifies the ID of the relationship object.</param>
        public Relationship(int idx)
        {
            idx_ = idx;
            isDelete_ = false;
            owner_ = null;
            partnerIdx_ = 0;

            start_ = new CompoundDate();
            end_ = new CompoundDate();
            typeIdx_ = 1;
            terminated_ = 1;
            sourcesStart_ = null;
            sourcesLocation_ = null;
            sourcesTerminated_ = null;
            sourcesEnd_ = null;
            location_ = "";
            comments_ = "";
            database_ = null;
            isDirty_ = true;
        }



        /// <summary>Class Constructor.  Creates a new relationship object for the specified person with the specified partner.  It is intended that this is a new relationship not in the database (yet) hence the ID is unknown.</summary>
        /// <param name="owner">Specifies the owner of this relationship object.</param>
        /// <param name="partnerIdx">Specifies the partner in this relationship..</param>
        public Relationship(Person owner, int partnerIdx)
        {
            idx_ = 0;
            isDelete_ = false;
            owner_ = owner;
            partnerIdx_ = partnerIdx;
            if (owner_.isMale)
            {
                maleIdx_ = owner_.idx;
                femaleIdx_ = partnerIdx;
            }
            else
            {
                femaleIdx_ = owner_.idx;
                maleIdx_ = partnerIdx;
            }

            start_ = new CompoundDate();
            end_ = new CompoundDate();
            typeIdx_ = 1;
            terminated_ = 1;
            sourcesStart_ = null;
            sourcesLocation_ = null;
            sourcesTerminated_ = null;
            sourcesEnd_ = null;
            location_ = "";
            comments_ = "";
            isDirty_ = true;
        }



        /// <summary>Class Constuctor.  Builds a relationship object for the specified person with the specified partner and the specified ID.  It is intended that this relationship should have been read from the database hence the ID is known.</summary>
        /// <param name="idx">Specifies the ID of the relationship in the database.</param>
        /// <param name="owner">Specifies the owner of this relationship object.</param>
        /// <param name="partnerIdx">Specifies the partner in this relationship..</param>
        public Relationship(int idx, Person owner, int partnerIdx)
            : this(owner, partnerIdx)
        {
            idx_ = idx;
            isDirty_ = false;
        }



        #endregion

        #region IO

        /// <summary>Writes this relationship into the database.  Deletes the relationship from the database if required.</summary>
        /// <returns>True for success, false otherwise.</returns>
        public bool save()
        {
            OleDbCommand sqlCommand;

            if (isDelete_)
            {
                // Check if the relationship is actually in the database.
                if (idx_ == 0)
                {
                    // Nothing to do.
                    return true;
                }

                // Delete any child records.

                // Delete this record.
                sqlCommand = new OleDbCommand("DELETE FROM tbl_Relationships WHERE ID = " + idx_.ToString() + ";", owner_.database.cndb);
                sqlCommand.ExecuteNonQuery();

                // Return success.
                return true;
            }

            // Create a new record.
            if (idx_ == 0)
            {
                // Find the new ID for this relationship.
                sqlCommand = new OleDbCommand("SELECT MAX(ID) AS NewID FROM tbl_Relationships;", owner_.database.cndb);
                idx_ = (int)sqlCommand.ExecuteScalar() + 1;

                // Add this relationship to the datbase.
                sqlCommand = new OleDbCommand("INSERT INTO tbl_Relationships (ID, MaleID, FemaleID) VALUES (" + idx_.ToString() + ", " + maleIdx_ + ", " + femaleIdx_ + ");", owner_.database.cndb);
                sqlCommand.ExecuteNonQuery();

                // Update the sources with the new relationship ID.
                if (sourcesStart_ != null)
                {
                    sourcesStart_.relationshipIdx = idx_;
                }
                if (sourcesLocation_ != null)
                {
                    sourcesLocation_.relationshipIdx = idx_;
                }
                if (sourcesTerminated_ != null)
                {
                    sourcesTerminated_.relationshipIdx = idx_;
                }
                if (sourcesPartner_ != null)
                {
                    sourcesPartner_.relationshipIdx = idx_;
                }
                isDirty_ = true;
            }

            StringBuilder sql = new StringBuilder();
            sql.Append("UPDATE tbl_Relationships SET ");
            sql.Append("RelationshipID = " + typeIdx_.ToString() + ", ");
            sql.Append("TheDate = " + Database.toDb(start_) + ", ");
            sql.Append("StartStatusID = " + start_.status.ToString() + ", ");
            sql.Append("TerminatedID = " + terminated_.ToString() + ", ");
            sql.Append("TerminateDate = " + Database.toDb(end_) + ", ");
            sql.Append("TerminateStatusID = " + end_.status.ToString() + ", ");
            sql.Append("Location = " + Database.toDb(location_) + ", ");

            // Not really sure that the data has changed so don't update the written by record.
            bool isDirtyInSqlite = false;
            if (isDirty_)
            {
                sql.Append("LastEditBy=" + Database.toDb(lastEditBy_) + ", ");
                sql.Append("LastEditDate=#" + DateTime.Now.ToString("d-MMM-yyyy HH:mm:ss") + "#, ");
                isDirty_ = false;
                isDirtyInSqlite = true;
            }
            sql.Append("Comments = " + Database.toDb(comments_) + " ");
            sql.Append("WHERE ID = " + idx_.ToString() + ";");

            // Update the relationship in the database.
            sqlCommand = new OleDbCommand(sql.ToString(), owner_.database.cndb);
            sqlCommand.ExecuteNonQuery();

            // Update the relationship in the sqlite database.
            //SQLiteCommand sqliteCommand = owner_.database.sqlite.CreateCommand();
            //sqliteCommand.CommandText = "INSERT INTO RELATIONSHIPS (ID, RELATIONSHIP_ID, START_DATE, START_STATUS_ID, TERMINATED, TERMINATED_DATE, TERMINATED_STATUS_ID, LOCATION, COMMENTS, LAST_EDIT_BY, LAST_EDIT_DATE) VALUES (" + idx_.ToString() + ", " + typeIdx_.ToString() + ", " + Database.toDate(start_) + ", " + start_.status.ToString() + ", " + terminated_.ToString() + ", " + Database.toDate(end_) + ", " + end_.status.ToString() + ", " + Database.toDb(location_) + ", " + Database.toDb(comments_) + ", " + Database.toDb(lastEditBy_) + ", '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "');";            
            //try
            //{
            //    sqliteCommand.ExecuteNonQuery();
            //}
            //catch (System.Data.SQLite.SQLiteException error)
            //{
            //    if (error.ErrorCode == 19)
            //    {
            //        // Update the existing record in the sqlite3 database.
            //        sql = new StringBuilder();
            //        sql.Append("UPDATE RELATIONSHIPS SET ");
            //        sql.Append("RELATIONSHIP_ID = " + typeIdx_.ToString() + ", ");
            //        sql.Append("START_DATE = " + Database.toDate(start_) + ", ");
            //        sql.Append("START_STATUS_ID = " + start_.status.ToString() + ", ");
            //        sql.Append("TERMINATED_DATE = " + Database.toDate(end_) + ", ");
            //        sql.Append("TERMINATED_STATUS_ID = " + end_.status.ToString() + ", ");
            //        sql.Append("TERMINATED = " + terminated_.ToString() + ", ");
            //        sql.Append("LOCATION = " + Database.toDb(location_) + ", ");
            //        sql.Append("COMMENTS = " + Database.toDb(comments_));
            //        if (isDirtyInSqlite)
            //        {
            //            sql.Append(", LAST_EDIT_BY = " + Database.toDb(lastEditBy_) + ", ");
            //            sql.Append(", LAST_EDIT_DATE = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            //            isDirty_ = false;
            //        }
            //        sql.Append(" WHERE ID = " + idx_.ToString() + ";");
            //        sqliteCommand.CommandText = sql.ToString();
            //        sqliteCommand.ExecuteNonQuery();
            //    }
            //}

            // Save the source information.
            if (sourcesStart_ != null)
            {
                sourcesStart_.save();
            }
            if (sourcesLocation_ != null)
            {
                sourcesLocation_.save();
            }
            if (sourcesTerminated_ != null)
            {
                sourcesTerminated_.save();
            }
            if (sourcesEnd_ != null)
            {
                sourcesEnd_.save();
            }
            if (sourcesPartner_ != null)
            {
                sourcesPartner_.save();
            }

            // Return success.
            return true;
        }



        /// <summary>Marks this relationship for deletion at the next save.</summary>
        public void delete()
        {
            isDelete_ = true;
        }



        /// <summary>Sets the database that this relationship is attached to.</summary>
        /// <param name="database">Specifies the database object that this relationship belongs to.</param>
        public void setDatabase(Database database)
        {
            database_ = database;
        }

        #endregion

        #region Properties


        /// <summary>Returns true if the relationship is valid.  Currently this means not deleted.</summary>
        /// <returns>True, if the relationship is valid.  False if the relationship is waiting for deletion.</returns>
        public bool isValid()
        {
            return !isDelete_;
        }



        /// <summary>Returns true if this is a married relationship.  False otherwise.</summary>
        /// <returns>True for a married relationship, false otherwise.</returns>
        public bool isMarried()
        {
            if (typeIdx_ == 1 || typeIdx_ == 2)
            {
                return true;
            }
            return false;
        }



        /// <summary>Provides a default human readable summary of the relationship.</summary>
        /// <returns></returns>
        public override string ToString()
        {
            Person partner = owner_.database.getPerson(partnerIdx_);
            return partner.getName(false, true);
        }



        /// <summary>The ID of the record in the database.</summary>
        public int idx { get { return idx_; } }

        /// <summary>The ID of the person in the relationship who is not the owner.</summary>
        public int partnerIdx { get { return partnerIdx_; } set { partnerIdx_ = value; } }

        /// <summary>The ID of the male person in this relationship.</summary>
        public int maleIdx { get { return maleIdx_; } set { maleIdx_ = value; } }

        /// <summary>The ID the female person in this relationship.</summary>
        public int femaleIdx { get { return femaleIdx_; } set { femaleIdx_ = value; } }

        /// <summary>The termination status of the relationship.  (2 - Divorced).</summary>
        public int terminatedIdx { get { return terminated_; } set { terminated_ = value; } }

        /// <summary>The start date of the relationship.</summary>
        public CompoundDate start { get { return start_; } }

        /// <summary>The location for the relationship.</summary>
        public string location { get { return location_; } set { location_ = value; } }

        /// <summary>The end date of the relationship.</summary>
        public CompoundDate end { get { return end_; } set { end_ = value; } }

        /// <summary>The comments attached to this relationship.</summary>
        public string comments { get { return comments_; } set { comments_ = value; } }



        /// <summary>Type of relationship. 1- Religious, 2-Civil, 3-CoHabit.  This is stored as RelationshipID in the database.</summary>
        public int typeIdx
        {
            get { return typeIdx_; }
            set { typeIdx_ = value; }
        }



        /// <summary>Name of the user who wrote the last edit.</summary>
        public string lastEditBy
        {
            get
            {
                return lastEditBy_;
            }
            set
            {
                isDirty_ = true;
                lastEditBy_ = value;
            }
        }



        /// <summary>Date and time of the last edit.</summary>
        public DateTime lastEditDate
        {
            get { return lastEditDate_; }
            set { lastEditDate_ = value; }
        }



        /// <summary>The sources for the start date.</summary>
        public Sources sourceStart
        {
            get
            {
                if (sourcesStart_ == null)
                {
                    if (database_ == null)
                    {
                        sourcesStart_ = new Sources(owner_.database, idx_, 1);
                    }
                    else
                    {
                        sourcesStart_ = new Sources(database_, idx_, 1);
                    }
                }
                return sourcesStart_;
            }
        }



        /// <summary>The sources for the location.</summary>
        public Sources sourceLocation
        {
            get
            {
                if (sourcesLocation_ == null)
                {
                    if (database_ == null)
                    {
                        sourcesLocation_ = new Sources(owner_.database, idx_, 2);
                    }
                    else
                    {
                        sourcesLocation_ = new Sources(database_, idx_, 2);
                    }
                }
                return sourcesLocation_;
            }
        }



        /// <summary>The sources for the termination status.</summary>
        public Sources sourceTerminated
        {
            get
            {
                if (sourcesTerminated_ == null)
                {
                    if (database_ == null)
                    {
                        sourcesTerminated_ = new Sources(owner_.database, idx_, 3);
                    }
                    else
                    {
                        sourcesTerminated_ = new Sources(database_, idx_, 3);
                    }
                }
                return sourcesTerminated_;
            }
        }



        /// <summary>The sources for the end date.</summary>
        public Sources sourceEnd
        {
            get
            {
                if (sourcesEnd_ == null)
                {
                    if (database_ == null)
                    {
                        sourcesEnd_ = new Sources(owner_.database, idx_, 4);
                    }
                    else
                    {
                        sourcesEnd_ = new Sources(database_, idx_, 4);
                    }
                }
                return sourcesEnd_;
            }
        }



        /// <summary>The sources for the partner.</summary>
        public Sources sourcePartner
        {
            get
            {
                if (sourcesPartner_ == null)
                {
                    if (database_ == null)
                    {
                        sourcesPartner_ = new Sources(owner_.database, idx_, 5);
                    }
                    else
                    {
                        sourcesPartner_ = new Sources(database_, idx_, 5);
                    }
                }
                return sourcesPartner_;
            }
        }



        /// <summary>Returns the dirty state of the relationship record.</summary>
        public bool isDirty
        {
            get { return isDirty_; }
            set { isDirty_ = value; }
        }



        #endregion
    }
}
