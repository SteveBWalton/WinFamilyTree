using System;
using System.Collections.Generic;
using System.Text;

// Access database ADO.NET
using System.Data;
using System.Data.OleDb;

namespace family_tree.objects
{
    /// <summary>Class to represent a single to do record in the database.  Each item is linked to a single person.</summary>
    /// <remarks>It would be nice to attach a single ToDo item to a number of person.  But to keep the interface simple, I linked each ToDo to a single person.</remarks>
    public class ToDo
    {
        #region Member Variables

        /// <summary>ID of the record in the database.</summary>
        private int idx_;

        /// <summary>Human readable description of the item.</summary>
        private string description_;

        /// <summary>The priority of this to do item.</summary>
        private int priority_;

        /// <summary>The ID of the person that this item is attached to.</summary>
        public int personIdx_;

        /// <summary>True when the fact needs saving to the database.  False when the fact is in synchronised with the database.</summary>
        private bool isDirty_;

        /// <summary>True if this record should be deleted.</summary>
        private bool isDelete_;

        #endregion

        #region Class Constructors etc...



        /// <summary>Empty class constructor.</summary>
        public ToDo()
        {
            idx_ = -1;
            isDirty_ = true;
            isDelete_ = false;
        }



        /// <summary>Class constructor with ID specification.  This is intended to be used where a ToDo record has been read from the database.</summary>
        /// <param name="idx">Specifies the value of the ID.</param>
        /// <param name="personIdx">Specifies the ID of the person that owns this item.</param>
        /// <param name="priority">Specifies the priority of the item.</param>
        /// <param name="description">Specifies the description of the item.</param>
        public ToDo(int idx, int personIdx, int priority, string description)
        {
            isDirty_ = false;
            isDelete_ = false;
            idx_ = idx;
            personIdx_ = personIdx;
            priority_ = priority;
            description_ = description;
        }



        /// <summary>Human readable description of the item.</summary>
        public string description
        {
            get
            {
                if (isDelete_)
                {
                    return "[Deleted]";
                }
                return description_;
            }
            set { description_ = value; isDirty_ = true; }
        }



        /// <summary>The priority of this item.</summary>
        public int priority
        {
            get { return priority_; }
            set { priority_ = value; isDirty_ = true; }
        }



        /// <summary>Write the ToDo item into the specified database.</summary>
        /// <param name="cndb">Specifies an open connection to the database.</param>
        /// <returns>True for a write to the database, false otherwise.</returns>
        public bool save(OleDbConnection cndb)
        {
            // Check that the record needs saving.
            if (!isDirty_)
            {
                return false;
            }

            string sql = "";
            if (isDelete_)
            {
                if (idx_ != -1)
                {
                    // Delete this record.
                    sql = "DELETE FROM tbl_ToDo WHERE ID = " + idx_.ToString() + ";";
                }
            }
            else
            {
                if (idx_ == -1)
                {
                    // Create a new record.
                    sql = "INSERT INTO tbl_ToDo (PersonID, Priority, Description) VALUES (" + personIdx_.ToString() + ", " + priority_.ToString() + ", \"" + description_ + "\");";
                }
                else
                {
                    // Update the existing record.
                    sql = "UPDATE tbl_ToDo SET Priority = " + priority_.ToString() + ", Description = \"" + description_ + "\" WHERE ID = " + idx_.ToString() + ";";
                }
            }
            if (sql != "")
            {
                OleDbCommand sqlCommand = new OleDbCommand(sql, cndb);
                sqlCommand.ExecuteNonQuery();
            }

            isDirty_ = false;

            // Return a write to the database.
            return true;
        }



        /// <summary>Mark this item for deletion at the next write to database.</summary>
        public void delete()
        {
            isDirty_ = true;
            isDelete_ = true;
        }



        #endregion
    }
}
