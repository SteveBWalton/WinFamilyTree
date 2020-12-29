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

    // Object to represent a relationship (like a marriage) between 2 people
    /// <summary>
    /// Object to represent a relationship (like a marriage) between 2 people.
    /// In gedcom this is a family object.
    /// </summary>
	public class clsRelationship
	{	
		#region Member Variables

		/// <summary>ID of the record in the database.</summary>
		private int m_nID;

		/// <summary>True if this record should be removed from the database.</summary>
		private bool m_bDelete;

		/// <summary>Person who owns this relationship object.</summary>
		private clsPerson m_oOwner;

		/// <summary>ID of the partner.</summary>
		private int m_nPartnerID;

		/// <summary>Is the relationship terminated.</summary>
		private int m_nTerminated;

		/// <summary>ID of the male member of the relationship.</summary>
		private int m_nMaleID;

		/// <summary>ID of the female member of the relationship.</summary>
		private int m_nFemaleID;

		/// <summary>Start date of the relationship.</summary>
		private clsDate m_dtStart;

		/// <summary>Location of the start of the relationship.  Eg Married at ...</summary>
		private string m_sLocation;

		/// <summary>End date of the relationship.</summary>
		private clsDate m_dtEnd;

		/// <summary>User comments on this relationship.</summary>
		private string m_sComments;

		/// <summary>Sources for the start date.</summary>
		private clsSources m_sourcesStart;

		/// <summary>Sources for the terminated status.</summary>
		private clsSources m_sourcesTerminated;

		/// <summary>Sources for the location.</summary>
		private clsSources m_sourcesLocation;

		/// <summary>Sources for the end date.</summary>
		private clsSources m_sourcesEnd;

		/// <summary>Sources for the partner name.</summary>
		private clsSources m_sourcesPartner;

		/// <summary>The source database.  This is probably null.</summary>
		private Database m_oDb;

        // Type of relationship. 1- Religious, 2-Civil, 3-CoHabit.
        /// <summary>
        /// Type of relationship. 1- Religious, 2-Civil, 3-CoHabit.
        /// This is stored as RelationshipID in the database.
        /// </summary>
		private int m_nTypeID;

		/// <summary>Name of the user who wrote the last edit.</summary>
		private string m_sLastEditBy;

		/// <summary>Date and time of the last edit.</summary>
		private DateTime m_dtLastEditDate;

        /// <summary>True if this relationship is out of sync with the database and needs saving.</summary>
        private bool m_bDirty;

		#endregion

        // Class Constructor.
        /// <summary>
        /// Class Constructor.
        /// Builds an empty relationship object.
		/// This can be used to create relationship objects with no owner.
		/// </summary>		
		/// <param name="nID">Specifies the ID of the relationship object.</param>
        public clsRelationship(int nID)
        {
            m_nID = nID;
            m_bDelete = false;
            m_oOwner = null;
            m_nPartnerID = 0;

            m_dtStart = new clsDate();
            m_dtEnd = new clsDate();
            m_nTypeID = 1;
            m_nTerminated = 1;
            m_sourcesStart = null;
            m_sourcesLocation = null;
            m_sourcesTerminated = null;
            m_sourcesEnd = null;
            m_sLocation = "";
            m_sComments = "";
            m_oDb = null;
            m_bDirty = true;
        }

        // Class Constructor.  Creates a new relationship object for the specified person with the specified partner.
        /// <summary>
        /// Class Constructor.  Creates a new relationship object for the specified person with the specified partner.
        /// It is intended that this is a new relationship not in the database (yet) hence the ID is unknown.
		/// </summary>
		/// <param name="oOwner">Specifies the owner of this relationship object.</param>
		/// <param name="nPartnerID">Specifies the partner in this relationship..</param>
        public clsRelationship(clsPerson oOwner, int nPartnerID)
        {
            m_nID = 0;
            m_bDelete = false;
            m_oOwner = oOwner;
            m_nPartnerID = nPartnerID;
            if(m_oOwner.isMale)
            {
                m_nMaleID = m_oOwner.ID;
                m_nFemaleID = nPartnerID;
            }
            else
            {
                m_nFemaleID = m_oOwner.ID;
                m_nMaleID = nPartnerID;
            }

            m_dtStart = new clsDate();
            m_dtEnd = new clsDate();
            m_nTypeID = 1;
            m_nTerminated = 1;
            m_sourcesStart = null;
            m_sourcesLocation = null;
            m_sourcesTerminated = null;
            m_sourcesEnd = null;
            m_sLocation = "";
            m_sComments = "";
            m_bDirty = true;
        }

        // Class Constuctor.  Builds a relationship object for the specified person with the specified partner and the specified ID.
        /// <summary>
        /// Class Constuctor.  Builds a relationship object for the specified person with the specified partner and the specified ID.
        /// It is intended that this relationship should have been read from the database hence the ID is known.
		/// </summary>
		/// <param name="nID">Specifies the ID of the relationship in the database.</param>
		/// <param name="oOwner">Specifies the owner of this relationship object.</param>
		/// <param name="nPartnerID">Specifies the partner in this relationship..</param>
        public clsRelationship(int nID, clsPerson oOwner, int nPartnerID)
            : this(oOwner, nPartnerID)
        {
            m_nID = nID;
            m_bDirty = false;
        }

        // Writes this relationship into the database.
        /// <summary>
        /// Writes this relationship into the database.
        /// Deletes the relationship from the database if required.
		/// </summary>
		/// <returns>True for success, false otherwise.</returns>
        public bool Save()
        {
            OleDbCommand oSql;			

            if(m_bDelete)
            {
                // Check if the relationship is actually in the database
                if(m_nID == 0)
                {
                    // Nothing to do
                    return true;
                }

                // Delete any child records

                // Delete this record
                oSql = new OleDbCommand("DELETE FROM tbl_Relationships WHERE ID=" + m_nID.ToString() + ";",m_oOwner.Database.cnDB);
                oSql.ExecuteNonQuery();

                // Return success
                return true;
            }

            // Create a new record
            if(m_nID == 0)
            {
                // Find the new ID for this relationship
                oSql = new OleDbCommand("SELECT MAX(ID) AS NewID FROM tbl_Relationships;",m_oOwner.Database.cnDB);
                m_nID = (int)oSql.ExecuteScalar() + 1;

                // Add this relationship to the datbase
                oSql = new OleDbCommand("INSERT INTO tbl_Relationships (ID,MaleID,FemaleID) VALUES (" + m_nID.ToString() + "," + m_nMaleID + "," + m_nFemaleID + ");",m_oOwner.Database.cnDB);
                oSql.ExecuteNonQuery();

                // Update the sources with the new relationship ID				
                if(m_sourcesStart != null)
                {
                    m_sourcesStart.RelationshipID = m_nID;
                }
                if(m_sourcesLocation != null)
                {
                    m_sourcesLocation.RelationshipID = m_nID;
                }
                if(m_sourcesTerminated != null)
                {
                    m_sourcesTerminated.RelationshipID = m_nID;
                }
                if(m_sourcesPartner != null)
                {
                    m_sourcesPartner.RelationshipID = m_nID;
                }
                m_bDirty = true;
            }

            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("UPDATE tbl_Relationships SET ");
            sbSql.Append("RelationshipID=" + m_nTypeID.ToString() + "," );
            sbSql.Append("TheDate=" + Database.ToDb(m_dtStart) + "," );
            sbSql.Append("StartStatusID=" + m_dtStart.Status.ToString() + "," );
            sbSql.Append("TerminatedID=" + m_nTerminated.ToString() + "," );
            sbSql.Append("TerminateDate=" + Database.ToDb(m_dtEnd) + "," );
            sbSql.Append("TerminateStatusID=" + m_dtEnd.Status.ToString() + "," );
            sbSql.Append("Location=" + Database.ToDb(m_sLocation) + "," );
                
            // Not really sure that the data has changed so don't update the written by record.
            if(m_bDirty)
            {
                sbSql.Append("LastEditBy=" + Database.ToDb(m_sLastEditBy) + ",");
                sbSql.Append("LastEditDate=#" + DateTime.Now.ToString("d-MMM-yyyy HH:mm:ss") + "#,");
                m_bDirty = false;
            }
            sbSql.Append("Comments=" + Database.ToDb(m_sComments) + " ");
            sbSql.Append("WHERE ID=" + m_nID.ToString() + ";");

            // Update the relationship in the database
            oSql = new OleDbCommand(sbSql.ToString(),m_oOwner.Database.cnDB);
            oSql.ExecuteNonQuery();

            // Save the source information
            if(m_sourcesStart != null)
            {
                m_sourcesStart.Save();
            }
            if(m_sourcesLocation != null)
            {
                m_sourcesLocation.Save();
            }
            if(m_sourcesTerminated != null)
            {
                m_sourcesTerminated.Save();
            }
            if(m_sourcesEnd != null)
            {
                m_sourcesEnd.Save();
            }
            if(m_sourcesPartner != null)
            {
                m_sourcesPartner.Save();
            }

            // Return success
            return true;
        }

        // Marks this relationship for deletion at the next save.
        /// <summary>
        /// Marks this relationship for deletion at the next save.
        /// </summary>
		public void Delete()
		{
			m_bDelete = true;
		}

        // Sets the database that this relationship is attached to.
        /// <summary>
        /// Sets the database that this relationship is attached to.
        /// </summary>
		/// <param name="oDb">Specifies the clsDatabase object that this relationship belongs to.</param>
        public void SetDb(Database oDb)
        {
            m_oDb = oDb;
            ;
        }

        // Returns true if the relationship is valid.
        /// <summary>
        /// Returns true if the relationship is valid.
        /// Currently this means not deleted.
		/// </summary>
		/// <returns>True, if the relationship is valid.  False if the relationship is waiting for deletion.</returns>
		public bool IsValid()
		{
			return !m_bDelete;
		}

        // Returns true if this is a married relationship.
        /// <summary>
        /// Returns true if this is a married relationship.
        /// False otherwise.
        /// </summary>
        /// <returns>True for a married relationship, false otherwise.</returns>
        public bool IsMarried()
        {
            if(m_nTypeID == 1 || m_nTypeID == 2)
            {
                return true;
            }
            return false;
        }

        // Provides a default human readable summary of the relationship.
        /// <summary>
        /// Provides a default human readable summary of the relationship.
        /// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			clsPerson oPartner = m_oOwner.Database.getPerson(m_nPartnerID);
			return oPartner.GetName(false,true);
		}

        // The ID of the record in the database.
        /// <summary>
        /// The ID of the record in the database.
        /// </summary>
		public int ID { get { return m_nID; } }
		
		/// <summary>The ID of the person in the relationship who is not the owner.</summary>
		public int PartnerID { get { return m_nPartnerID; } set { m_nPartnerID = value; } }

		/// <summary>The ID of the male person in this relationship.</summary>
		public int MaleID { get { return m_nMaleID; } set { m_nMaleID = value; } }

		/// <summary>The ID the female person in this relationship.</summary>
		public int FemaleID { get { return m_nFemaleID; } set { m_nFemaleID = value; } }

		//		public bool	Terminated { get { return m_bTerminated; } set { m_bTerminated = value; } }

		/// <summary>The termination status of the relationship.  (2 - Divorced).</summary>
		public int TerminatedID { get { return m_nTerminated; } set { m_nTerminated = value; } }

		/// <summary>The start date of the relationship.</summary>
		public clsDate Start { get { return m_dtStart; } }

		/// <summary>The location for the relationship.</summary>
		public string Location { get { return m_sLocation; } set { m_sLocation = value; } }

		/// <summary>The end date of the relationship.</summary>
		public clsDate End { get { return m_dtEnd; } set { m_dtEnd = value; } }

		/// <summary>The comments attached to this relationship.</summary>
		public string Comments { get { return m_sComments; } set { m_sComments = value; } }

        // Type of relationship. 1- Religious, 2-Civil, 3-CoHabit.
        /// <summary>
        /// Type of relationship. 1- Religious, 2-Civil, 3-CoHabit.
        /// This is stored as RelationshipID in the database.
        /// </summary>
        public int TypeID
        {
            get
            {
                return m_nTypeID;
            }
            set
            {
                m_nTypeID = value;
            }
        }

        // Name of the user who wrote the last edit.
        /// <summary>
        /// Name of the user who wrote the last edit.
        /// </summary>
        public string LastEditBy
        {
            get
            {
                return m_sLastEditBy;
            }
            set
            {
                m_bDirty = true;
                m_sLastEditBy = value;
            }
        }

        // Date and time of the last edit.
        /// <summary>
        /// Date and time of the last edit.
        /// </summary>
        public DateTime LastEditDate
        {
            get
            {
                return m_dtLastEditDate;
            }
            set
            {
                m_dtLastEditDate = value;
            }
        }
		
		/// <summary>The sources for the start date.</summary>
        public clsSources SourceStart
        {
            get
            {
                if(m_sourcesStart == null)
                {
                    if(m_oDb == null)
                    {
                        m_sourcesStart = new clsSources(m_oOwner.Database,m_nID,1);
                    }
                    else
                    {
                        m_sourcesStart = new clsSources(m_oDb,m_nID,1);
                    }
                }
                return m_sourcesStart;
            }
        }

		/// <summary>The sources for the location.</summary>
		public clsSources SourceLocation
		{
			get
			{
				if(m_sourcesLocation==null)
				{
					if(m_oDb==null)
					{
						m_sourcesLocation = new clsSources(m_oOwner.Database,m_nID,2);
					}
					else
					{
						m_sourcesLocation = new clsSources(m_oDb,m_nID,2);
					}
				}
				return m_sourcesLocation;
			}
		}

		/// <summary>The sources for the termination status.</summary>
		public clsSources SourceTerminated
		{
			get
			{
				if(m_sourcesTerminated==null)
				{
					if(m_oDb==null)
					{
						m_sourcesTerminated = new clsSources(m_oOwner.Database,m_nID,3);
					}
					else
					{
						m_sourcesTerminated = new clsSources(m_oDb,m_nID,3);
					}
				}
				return m_sourcesTerminated;
			}
		}

		/// <summary>The sources for the end date.</summary>
		public clsSources SourceEnd
		{
			get
			{
				if(m_sourcesEnd==null)
				{
					if(m_oDb==null)
					{
						m_sourcesEnd = new clsSources(m_oOwner.Database,m_nID,4);
					}
					else
					{
						m_sourcesEnd = new clsSources(m_oDb,m_nID,4);
					}
				}
				return m_sourcesEnd;
			}
		}

		/// <summary>The sources for the partner.</summary>
		public clsSources SourcePartner
		{
			get
			{
				if(m_sourcesPartner==null)
				{
					if(m_oDb==null)
					{
						m_sourcesPartner = new clsSources(m_oOwner.Database,m_nID,5);
					}
					else
					{
						m_sourcesPartner = new clsSources(m_oDb,m_nID,5);
					}
				}
				return m_sourcesPartner;
			}
		}

        /// <summary>Returns the dirty state of the relationship record.</summary>
        public bool Dirty { get { return m_bDirty; } set { m_bDirty = value; } }
	}
}
