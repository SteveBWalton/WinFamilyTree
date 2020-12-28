using System;
using System.Data;
using System.Data.OleDb;		// Access database ADO.NET

namespace FamilyTree.Objects
{
	/// <summary>
	/// Class to represent a fact about a person
	/// </summary>
	public class clsFact
	{
		#region Member Variables

		/// <summary>Database key for the fact.</summary>
		private int 	m_nID;

		/// <summary>Type of fact.</summary>
		private int m_nTypeID;
		
		/// <summary>An ordering for the facts attached to the person.</summary>
		private int m_nRank;

		/// <summary>Information in the fact.</summary>
		private string m_sDescription;

		/// <summary>Sources for this fact.</summary>
		private clsSources m_oSources;
		
		/// <summary>Person this fact relates to.</summary>
		private clsPerson m_oPerson;

        /// <summary>True when the fact needs saving to the database.  False when the fact is in synchronised with the database.</summary>
        private bool m_bDirty;

		/// <summary>True when the fact needs removing from the database.  False, usually.</summary>
		private bool m_bDelete;

		#endregion

		#region Constructors etc ...
		
		/// <overloads>Class Constructor.</overloads>
		/// <summary>
		/// Creates an empty fact object.
		/// It is intended that this creates new fact objects.
		/// </summary>
		public clsFact()
		{			
			m_oPerson = null;
			m_oSources = null;
			m_nID = 0;
			m_nRank = 0;
			m_bDirty = true;			
			m_bDelete = false;
		}
		/// <summary>
		/// Creates a populated fact object attached to a person object.
		/// It is intended that this fact will have come from the database.
		/// </summary>
		/// <param name="nID">Specifies the ID of the fact in the database.</param>
		/// <param name="oPerson">Specifies the person this fact is attached to.</param>
		/// <param name="nTypeID">Specifies the type of this fact.</param>
		/// <param name="nRank">Specifies the rank order of the fact within the specified person.</param>
		/// <param name="sDescription">Specifies the description (data) for this fact.</param>
		public clsFact
			(
			int nID,
			clsPerson oPerson,
			int nTypeID,
			int nRank,
			string sDescription
			)
		{
			m_bDirty = false;			
			m_oPerson = oPerson;
			m_oSources = null;
			m_nID = nID;
			m_nTypeID = nTypeID;
			m_nRank = nRank;
			m_sDescription = sDescription;
			m_bDelete = false;
		}

		#endregion

		#region Database

		/// <summary>
		/// Writes this fact into the database.
		/// </summary>
		/// <returns>True for success, false for failure.</returns>
		public bool Save()
		{
			// Delete this fact (if required)
			if(m_bDelete)
			{
				// Check if the fact is actually in the database
				if(m_nID==0)
				{
					// Nothing to do
					return true;
				}

				// Delete any child records

				// Delete the sources for this fact.
				string sSql = "DELETE FROM tbl_FactsToSources WHERE FactID="+m_nID.ToString()+";";
				OleDbCommand oSql = new OleDbCommand(sSql,m_oPerson.Database.cnDB);
				oSql.ExecuteNonQuery();

				// Delete the record
				sSql = "DELETE FROM tbl_Facts WHERE ID=" + m_nID.ToString() + ";";
				oSql = new OleDbCommand(sSql,m_oPerson.Database.cnDB);
				oSql.ExecuteNonQuery();			

				// Return success
				return true;
			}

			// Save this fact (if required)
			if(m_bDirty)
			{
				if(m_nID==0)
				{
					// Create a new record
					OleDbCommand oSQL = new OleDbCommand("INSERT INTO tbl_Facts (PersonID,TypeID,Rank,Information) VALUES (" + m_oPerson.ID.ToString() + "," + m_nTypeID.ToString() +"," + m_nRank.ToString() +",\"" + m_sDescription + "\");",m_oPerson.Database.cnDB);
					oSQL.ExecuteNonQuery();			
					
					// Find the ID of the new record
					oSQL = new OleDbCommand("SELECT MAX(ID) AS NewID FROM tbl_Facts;",m_oPerson.Database.cnDB);
					m_nID = (int)oSQL.ExecuteScalar();

					// Update the child sources before they are saved
					if(m_oSources!=null)
					{
						m_oSources.FactID = m_nID;
					}
				}
				else
				{
					// Update add existing record				
					OleDbCommand oSQL = new OleDbCommand("UPDATE tbl_Facts SET Information=\"" + m_sDescription + "\",Rank="+m_nRank.ToString()+" WHERE ID=" + m_nID.ToString() +";",m_oPerson.Database.cnDB);
					oSQL.ExecuteNonQuery();								
				}
				m_bDirty = false;
			}

			// Save the sources (if required)
			if(m_oSources!=null)
			{
				m_oSources.Save();
			}

			// Return success
			return true;
		}
		
		/// <summary>
		/// Marks this relationship for deletion at the next save
		/// </summary>
		public void Delete()
		{
			m_bDelete = true;
		}
						
		/// <summary>
		/// Returns true if the fact is valid.
		/// Currently this means not deleted.
		/// </summary>
		/// <returns>True, if the fact is valid.  False if the relationship is waiting for deletion.</returns>
		public bool IsValid()
		{
			return !m_bDelete;
		}

		#endregion
				
		#region Sources
		
		// *************************************************************************************************************
		/// <summary>
		/// Returns a sources object for this fact
		/// </summary>
		/// <returns>Returns a sources object for this fact</returns>
		public clsSources Sources
		{
			get
			{
				if(m_oSources==null)
				{
					m_oSources = new clsSources(m_nID,m_oPerson.Database);
				}
				return m_oSources;
			}
		}

		#endregion

		#region Public Properties

		/// <summary>ID of the fact in the database.</summary>
		public int ID { get { return m_nID; } set { m_nID = value; } }

		/// <summary>Person this fact relates to.</summary>
		public clsPerson Person { get { return m_oPerson; } }

		/// <summary>Type of this fact.  eg Location of Birth, Occupartion.</summary>
		public int TypeID { get { return m_nTypeID; } set { m_nTypeID = value; } }
		
		/// <summary>The ordering for this fact inside the person.</summary>
		public int Rank
		{
			get
			{
				return m_nRank;
			}
			set
			{
				// I expect the rank will be set to it's current value alot so I do not want dirty in this case.
				if(value!=m_nRank)
				{
					m_nRank = value;
					m_bDirty = true;
				}
			}
		}
		
		/// <summary>Human readable name of the fact type.</summary>
		public string TypeName
		{
			get
			{
				clsFactType oFactType = m_oPerson.Database.GetFactType(m_nTypeID);
				if(oFactType==null)
				{
					return null;
				}
				return oFactType.Name;
			}
		}
		
		/// <summary>
		/// The value of the fact.
		/// This is the information in the fact.
		/// This is usually the description field from the database.
		/// </summary>
		public string Information 
		{ 
			get 
			{
				if(m_bDelete)
				{
					return "[Deleted]";
				}
				if(m_sDescription.EndsWith("."))
				{
					m_sDescription = m_sDescription.Substring(0,m_sDescription.Length-1);
				}
				return m_sDescription; 
			}
			set
			{
				m_bDirty = true;
				m_sDescription = value;
			}
		}

		/// <summary>True if this fact has changed since last written to the database.</summary>
		public bool Dirty { get { return m_bDirty; } }

		/// <summary>True if this fact should be used to build the description of the person.</summary>
		public bool UseInDescription { get { return true; } }

		#endregion
	}
}
