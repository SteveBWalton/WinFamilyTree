using System;
using System.Collections.Generic;
using System.Text;

// Access database ADO.NET
using System.Data;
using System.Data.OleDb;

namespace FamilyTree.Objects
{
    /// <summary>
    /// Class to represent a single to do record in the database.
    /// Each item is linked to a single person.
    /// </summary>
    /// <remarks>
    /// It would be nice to attach a single ToDo item to a number of person.
    /// But to keep the interface simple, I linked each ToDo to a single person.
    /// </remarks>
    public class clsToDo
    {
        #region Member Variables

        /// <summary>
        /// ID of the record in the database.
        /// </summary>
        private int m_nID;

        /// <summary>
        /// Human readable description of the item.
        /// </summary>
        private string m_sDescription;

        /// <summary>
        /// The priority of this item.
        /// </summary>
        private int m_nPriority;

        /// <summary>
        /// The ID of the person that this item is attached to.
        /// </summary>
        public int PersonID;

        /// <summary>True when the fact needs saving to the database.  False when the fact is in synchronised with the database.</summary>
        private bool m_bDirty;

        /// <summary>True if this record should be deleted.</summary>
        private bool m_bDelete;

        #endregion

        #region Class Constructors etc...

        /// <summary>
        /// Empty class constructor.
        /// </summary>
        public clsToDo()
        {
            m_nID = -1;
            m_bDirty = true;
            m_bDelete = false;
        }

        /// <summary>
        /// Class constructor with ID specification.
        /// This is intended to be used where a ToDo record has been read from the database.
        /// </summary>
        /// <param name="nID">Specifies the value of the ID.</param>
        /// <param name="nPersonID">Specifies the ID of the person that owns this item.</param>
        /// <param name="nPriority">Specifies the priority of the item.</param>
        /// <param name="sDescription">Specifies the description of the item.</param>
        public clsToDo
            (
            int nID,
            int nPersonID,
            int nPriority,
            string sDescription
            )
        {
            m_bDirty = false;
            m_bDelete = false;
            m_nID = nID;
            PersonID = nPersonID;
            m_nPriority = nPriority;
            m_sDescription = sDescription;
        }

        /// <summary>
        /// Human readable description of the item.
        /// </summary>
        public string Description
        {
            get
            {
                if(m_bDelete)
                {
                    return "[Deleted]";
                }
                return m_sDescription; 
            }
            set { m_sDescription = value; m_bDirty = true; }
        }

        /// <summary>
        /// The priority of this item.
        /// </summary>
        public int Priority
        {
            get { return m_nPriority; }
            set { m_nPriority = value; m_bDirty = true; }
        }

        /// <summary>
        /// Write the ToDo item into the specified database.
        /// </summary>
        /// <param name="cnDb"></param>
        /// <returns></returns>
        public bool Save
            (
            OleDbConnection cnDb
            )
        {
            // Check that the record needs saving
            if(!m_bDirty)
            {
                return false;
            }

            string sSql="";
            if(m_bDelete)
            {
                if(m_nID != -1)
                {
                    // Delete this record
                    sSql = "DELETE FROM tbl_ToDo WHERE ID=" + m_nID.ToString() + ";";
                }
            }
            else
            {
                if(m_nID == -1)
                {
                    // Create a new record
                    sSql = "INSERT INTO tbl_ToDo (PersonID,Priority,Description) VALUES (" + PersonID.ToString() + "," + m_nPriority.ToString() + ",\"" + m_sDescription + "\");";
                }
                else
                {
                    // Update the existing record
                    sSql = "UPDATE tbl_ToDo SET Priority=" + m_nPriority.ToString() + ",Description=\"" + m_sDescription + "\" WHERE ID=" + m_nID.ToString() + ";";
                }
            }
            if(sSql != "")
            {
                OleDbCommand oSql = new OleDbCommand(sSql,cnDb);
                oSql.ExecuteNonQuery();
            }

            m_bDirty = false;

            // Return a write to the database
            return true;
        }

        /// <summary>
        /// Mark this item for deletion at the next write to database.
        /// </summary>
        public void Delete()
        {
            m_bDirty = true;
            m_bDelete = true;
        }

        #endregion
    }
}
