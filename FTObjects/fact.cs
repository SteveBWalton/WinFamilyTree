using System.Data.OleDb;		// Access database ADO.NET

namespace family_tree.objects
{
    /// <summary>Class to represent a fact about a person.</summary>
    public class Fact
    {
        #region Member Variables

        /// <summary>Database key for the fact.</summary>
        private int idx_;

        /// <summary>Type of fact.</summary>
        private int typeIdx_;

        /// <summary>An ordering for the facts attached to the person.</summary>
        private int rank_;

        /// <summary>Information in the fact.</summary>
        private string description_;

        /// <summary>Sources for this fact.</summary>
        private Sources sources_;

        /// <summary>Person this fact relates to.</summary>
        private Person person_;

        /// <summary>True when the fact needs saving to the database.  False when the fact is in synchronised with the database.</summary>
        private bool isDirty_;

        /// <summary>True when the fact needs removing from the database.  False, usually.</summary>
        private bool isDelete_;

        #endregion

        #region Constructors etc ...

        /// <overloads>Class Constructor.</overloads>
        /// <summary>
        /// Creates an empty fact object.
        /// It is intended that this creates new fact objects.
        /// </summary>
        public Fact()
        {
            person_ = null;
            sources_ = null;
            idx_ = 0;
            rank_ = 0;
            isDirty_ = true;
            isDelete_ = false;
        }
        /// <summary>
        /// Creates a populated fact object attached to a person object.
        /// It is intended that this fact will have come from the database.
        /// </summary>
        /// <param name="idx">Specifies the ID of the fact in the database.</param>
        /// <param name="person">Specifies the person this fact is attached to.</param>
        /// <param name="typeIdx">Specifies the type of this fact.</param>
        /// <param name="rank">Specifies the rank order of the fact within the specified person.</param>
        /// <param name="description">Specifies the description (data) for this fact.</param>
        public Fact(int idx, Person person, int typeIdx, int rank, string description)
        {
            isDirty_ = false;
            person_ = person;
            sources_ = null;
            idx_ = idx;
            typeIdx_ = typeIdx;
            rank_ = rank;
            description_ = description;
            isDelete_ = false;
        }

        #endregion

        #region Database



        /// <summary>Writes this fact into the database.</summary>
        /// <returns>True for success, false for failure.</returns>
        public bool save()
        {
            // Delete this fact (if required).
            if (isDelete_)
            {
                // Check if the fact is actually in the database.
                if (idx_ == 0)
                {
                    // Nothing to do
                    return true;
                }

                // Delete any child records.

                // Delete the sources for this fact.
                string sql = "DELETE FROM tbl_FactsToSources WHERE FactID = " + idx_.ToString() + ";";
                OleDbCommand sqlCommand = new OleDbCommand(sql, person_.database.cndb);
                sqlCommand.ExecuteNonQuery();

                // Delete the record
                sql = "DELETE FROM tbl_Facts WHERE ID=" + idx_.ToString() + ";";
                sqlCommand = new OleDbCommand(sql, person_.database.cndb);
                sqlCommand.ExecuteNonQuery();

                // Return success.
                return true;
            }

            // Save this fact (if required).
            if (isDirty_)
            {
                if (idx_ == 0)
                {
                    // Create a new record.
                    OleDbCommand sqlCommand = new OleDbCommand("INSERT INTO tbl_Facts (PersonID, TypeID, Rank, Information) VALUES (" + person_.idx.ToString() + ", " + typeIdx_.ToString() + ", " + rank_.ToString() + ", \"" + description_ + "\");", person_.database.cndb);
                    sqlCommand.ExecuteNonQuery();

                    // Find the ID of the new record.
                    sqlCommand = new OleDbCommand("SELECT MAX(ID) AS NewID FROM tbl_Facts;", person_.database.cndb);
                    idx_ = (int)sqlCommand.ExecuteScalar();

                    // Update the child sources before they are saved.
                    if (sources_ != null)
                    {
                        sources_.factIdx = idx_;
                    }
                }
                else
                {
                    // Update add existing record				
                    OleDbCommand sqlCommand = new OleDbCommand("UPDATE tbl_Facts SET Information = \"" + description_ + "\", Rank = " + rank_.ToString() + " WHERE ID = " + idx_.ToString() + ";", person_.database.cndb);
                    sqlCommand.ExecuteNonQuery();
                }
                isDirty_ = false;
            }

            // Save the sources (if required).
            if (sources_ != null)
            {
                sources_.save();
            }

            // Return success.
            return true;
        }



        /// <summary>Marks this relationship for deletion at the next save.</summary>
        public void delete()
        {
            isDelete_ = true;
        }



        /// <summary>Returns true if the fact is valid.  Currently this means not deleted.</summary>
        /// <returns>True, if the fact is valid.  False if the relationship is waiting for deletion.</returns>
        public bool isValid()
        {
            return !isDelete_;
        }



        #endregion

        #region Sources



        /// <summary>Returns a sources object for this fact.</summary>
        /// <returns>Returns a sources object for this fact</returns>
        public Sources sources
        {
            get
            {
                if (sources_ == null)
                {
                    sources_ = new Sources(idx_, person_.database);
                }
                return sources_;
            }
        }



        #endregion

        #region Public Properties



        /// <summary>ID of the fact in the database.</summary>
        public int idx { get { return idx_; } set { idx_ = value; } }



        /// <summary>Person this fact relates to.</summary>
        public Person person { get { return person_; } }



        /// <summary>Type of this fact.  eg Location of Birth, Occupartion.</summary>
        public int typeIdx { get { return typeIdx_; } set { typeIdx_ = value; } }



        /// <summary>The ordering for this fact inside the person.</summary>
        public int rank
        {
            get
            {
                return rank_;
            }
            set
            {
                // I expect the rank will be set to it's current value alot so I do not want dirty in this case.
                if (value != rank_)
                {
                    rank_ = value;
                    isDirty_ = true;
                }
            }
        }



        /// <summary>Human readable name of the fact type.</summary>
        public string typeName
        {
            get
            {
                FactType factType = person_.database.getFactType(typeIdx_);
                if (factType == null)
                {
                    return null;
                }
                return factType.name;
            }
        }



        /// <summary>The value of the fact.  This is the information in the fact.  This is usually the description field from the database.</summary>
        public string information
        {
            get
            {
                if (isDelete_)
                {
                    return "[Deleted]";
                }
                if (description_.EndsWith("."))
                {
                    description_ = description_.Substring(0, description_.Length - 1);
                }
                return description_;
            }
            set
            {
                isDirty_ = true;
                description_ = value;
            }
        }



        /// <summary>True if this fact has changed since last written to the database.</summary>
        public bool isDirty { get { return isDirty_; } }



        /// <summary>True if this fact should be used to build the description of the person.</summary>
        public bool isUseInDescription { get { return true; } }



        #endregion
    }
}
