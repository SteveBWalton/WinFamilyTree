using System;
using System.Data;
using System.Data.OleDb;	// Access database ADO.NET
using System.Text;

namespace FamilyTree.Objects
{
    #region Supporting Types etc

    /// <summary>
    /// Possible status of a relationship
    /// </summary>
    public enum enumRelationshipStatus
    {
        /// <summary>Married.</summary>
        Married,
        /// <summary>The relationship ended with a divorce.</summary>
        Divorced,
        /// <summary>
        /// Not really relevant here.
        /// </summary>
        None
    }

    #endregion

    /// <summary>Class to represent a relationship (like a marriage) between 2 people.  In gedcom this is a family object.</summary>
    public class clsRelationship
    {
        #region Member Variables

        /// <summary>ID of the record in the database.</summary>
        private int index_;

        /// <summary>True if this record should be removed from the database.</summary>
        private bool isDelete_;

        /// <summary>Person who owns this relationship object.</summary>
        private Person owner_;

        /// <summary>ID of the partner.</summary>
        private int partnerIndex_;

        /// <summary>Is the relationship terminated.</summary>
        private int terminated_;

        /// <summary>ID of the male member of the relationship.</summary>
        private int maleIndex_;

        /// <summary>ID of the female member of the relationship.</summary>
        private int femaleIndex_;

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
        private int typeIndex_;

        /// <summary>Name of the user who wrote the last edit.</summary>
        private string lastEditBy_;

        /// <summary>Date and time of the last edit.</summary>
        private DateTime lastEditDate_;

        /// <summary>True if this relationship is out of sync with the database and needs saving.</summary>
        private bool isDirty_;

        #endregion

        /// <summary>Class Constructor.  Builds an empty relationship object.  This can be used to create relationship objects with no owner.</summary>
        /// <param name="index">Specifies the ID of the relationship object.</param>
        public clsRelationship(int index)
        {
            index_ = index;
            isDelete_ = false;
            owner_ = null;
            partnerIndex_ = 0;

            start_ = new CompoundDate();
            end_ = new CompoundDate();
            typeIndex_ = 1;
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
        /// <param name="partnerIndex">Specifies the partner in this relationship..</param>
        public clsRelationship(Person owner, int partnerIndex)
        {
            index_ = 0;
            isDelete_ = false;
            owner_ = owner;
            partnerIndex_ = partnerIndex;
            if (owner_.isMale)
            {
                maleIndex_ = owner_.index;
                femaleIndex_ = partnerIndex;
            }
            else
            {
                femaleIndex_ = owner_.index;
                maleIndex_ = partnerIndex;
            }

            start_ = new CompoundDate();
            end_ = new CompoundDate();
            typeIndex_ = 1;
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
        /// <param name="index">Specifies the ID of the relationship in the database.</param>
        /// <param name="owner">Specifies the owner of this relationship object.</param>
        /// <param name="partnerIndex">Specifies the partner in this relationship..</param>
        public clsRelationship(int index, Person owner, int partnerIndex)
            : this(owner, partnerIndex)
        {
            index_ = index;
            isDirty_ = false;
        }



        /// <summary>Writes this relationship into the database.  Deletes the relationship from the database if required.</summary>
        /// <returns>True for success, false otherwise.</returns>
        public bool save()
        {
            OleDbCommand sqlCommand;

            if (isDelete_)
            {
                // Check if the relationship is actually in the database.
                if (index_ == 0)
                {
                    // Nothing to do.
                    return true;
                }

                // Delete any child records.

                // Delete this record.
                sqlCommand = new OleDbCommand("DELETE FROM tbl_Relationships WHERE ID = " + index_.ToString() + ";", owner_.database.cndb);
                sqlCommand.ExecuteNonQuery();

                // Return success.
                return true;
            }

            // Create a new record.
            if (index_ == 0)
            {
                // Find the new ID for this relationship.
                sqlCommand = new OleDbCommand("SELECT MAX(ID) AS NewID FROM tbl_Relationships;", owner_.database.cndb);
                index_ = (int)sqlCommand.ExecuteScalar() + 1;

                // Add this relationship to the datbase.
                sqlCommand = new OleDbCommand("INSERT INTO tbl_Relationships (ID, MaleID, FemaleID) VALUES (" + index_.ToString() + ", " + maleIndex_ + ", " + femaleIndex_ + ");", owner_.database.cndb);
                sqlCommand.ExecuteNonQuery();

                // Update the sources with the new relationship ID.
                if (sourcesStart_ != null)
                {
                    sourcesStart_.relationshipIndex = index_;
                }
                if (sourcesLocation_ != null)
                {
                    sourcesLocation_.relationshipIndex = index_;
                }
                if (sourcesTerminated_ != null)
                {
                    sourcesTerminated_.relationshipIndex = index_;
                }
                if (sourcesPartner_ != null)
                {
                    sourcesPartner_.relationshipIndex = index_;
                }
                isDirty_ = true;
            }

            StringBuilder sql = new StringBuilder();
            sql.Append("UPDATE tbl_Relationships SET ");
            sql.Append("RelationshipID = " + typeIndex_.ToString() + ", ");
            sql.Append("TheDate = " + Database.toDb(start_) + ", ");
            sql.Append("StartStatusID = " + start_.status.ToString() + ", ");
            sql.Append("TerminatedID = " + terminated_.ToString() + ", ");
            sql.Append("TerminateDate = " + Database.toDb(end_) + ", ");
            sql.Append("TerminateStatusID = " + end_.status.ToString() + ", ");
            sql.Append("Location = " + Database.toDb(location_) + ", ");

            // Not really sure that the data has changed so don't update the written by record.
            if (isDirty_)
            {
                sql.Append("LastEditBy=" + Database.toDb(lastEditBy_) + ", ");
                sql.Append("LastEditDate=#" + DateTime.Now.ToString("d-MMM-yyyy HH:mm:ss") + "#, ");
                isDirty_ = false;
            }
            sql.Append("Comments = " + Database.toDb(comments_) + " ");
            sql.Append("WHERE ID = " + index_.ToString() + ";");

            // Update the relationship in the database.
            sqlCommand = new OleDbCommand(sql.ToString(), owner_.database.cndb);
            sqlCommand.ExecuteNonQuery();

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
            if (typeIndex_ == 1 || typeIndex_ == 2)
            {
                return true;
            }
            return false;
        }



        /// <summary>Provides a default human readable summary of the relationship.</summary>
        /// <returns></returns>
        public override string ToString()
        {
            Person partner = owner_.database.getPerson(partnerIndex_);
            return partner.getName(false, true);
        }



        /// <summary>The ID of the record in the database.</summary>
        public int index { get { return index_; } }

        /// <summary>The ID of the person in the relationship who is not the owner.</summary>
        public int partnerIndex { get { return partnerIndex_; } set { partnerIndex_ = value; } }

        /// <summary>The ID of the male person in this relationship.</summary>
        public int maleIndex { get { return maleIndex_; } set { maleIndex_ = value; } }

        /// <summary>The ID the female person in this relationship.</summary>
        public int femaleIndex { get { return femaleIndex_; } set { femaleIndex_ = value; } }

        /// <summary>The termination status of the relationship.  (2 - Divorced).</summary>
        public int terminatedIndex { get { return terminated_; } set { terminated_ = value; } }

        /// <summary>The start date of the relationship.</summary>
        public CompoundDate start { get { return start_; } }

        /// <summary>The location for the relationship.</summary>
        public string location { get { return location_; } set { location_ = value; } }

        /// <summary>The end date of the relationship.</summary>
        public CompoundDate end { get { return end_; } set { end_ = value; } }

        /// <summary>The comments attached to this relationship.</summary>
        public string comments { get { return comments_; } set { comments_ = value; } }



        /// <summary>Type of relationship. 1- Religious, 2-Civil, 3-CoHabit.  This is stored as RelationshipID in the database.</summary>
        public int typeIndex
        {
            get
            {
                return typeIndex_;
            }
            set
            {
                typeIndex_ = value;
            }
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
            get
            {
                return lastEditDate_;
            }
            set
            {
                lastEditDate_ = value;
            }
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
                        sourcesStart_ = new Sources(owner_.database, index_, 1);
                    }
                    else
                    {
                        sourcesStart_ = new Sources(database_, index_, 1);
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
                        sourcesLocation_ = new Sources(owner_.database, index_, 2);
                    }
                    else
                    {
                        sourcesLocation_ = new Sources(database_, index_, 2);
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
                        sourcesTerminated_ = new Sources(owner_.database, index_, 3);
                    }
                    else
                    {
                        sourcesTerminated_ = new Sources(database_, index_, 3);
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
                        sourcesEnd_ = new Sources(owner_.database, index_, 4);
                    }
                    else
                    {
                        sourcesEnd_ = new Sources(database_, index_, 4);
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
                        sourcesPartner_ = new Sources(owner_.database, index_, 5);
                    }
                    else
                    {
                        sourcesPartner_ = new Sources(database_, index_, 5);
                    }
                }
                return sourcesPartner_;
            }
        }



        /// <summary>Returns the dirty state of the relationship record.</summary>
        public bool isDirty { get { return isDirty_; } set { isDirty_ = value; } }
    }
}
