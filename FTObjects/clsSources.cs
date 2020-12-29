using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace FamilyTree.Objects
{
	/// <summary>
	/// Class to represent a collection of sources for pieces of information.
	/// The pieces of information can be distinguished by the ID field.
	/// This is the links between the owning fact and a collection of source objects.
	/// </summary>
	public class clsSources
	{
		#region Member Variables

		#region clsSourceLink

		/// <summary>
		/// Class to store the minimum required information on each source in the collection.
		/// This information is stored in memory to link to the actual source objects (which are dropped from memory once they are used).
		/// 
		/// Class to store the link between objects and sources.
		/// These objects store information to identfy the source and the linked specific information on an object.
		/// </summary>
		private class clsQuickSource
		{
			/// <summary>ID of the source link in the source link table.</summary>
			public int ID;

			/// <summary>ID of the source record in the database.</summary>
			public int SourceID;

			/// <summary>The ranking of the source in this list.</summary>
			public int Ranking;

			/// <summary>True to delete this record.  False, otherwise.</summary>
			public bool Delete; 

			/// <summary>
			/// Class constructor for an empty clsQuickSource object.
			/// </summary>
			public clsQuickSource()
			{
				ID = 0;
				SourceID = 0;
				Ranking = 1;
				Delete = false;
			}
			/// <summary>
			/// Class constructor for a clsQuickSource object where the SourceID is known.
			/// </summary>
			/// <param name="nSourceID">Specify the SourceID value.</param>
			public clsQuickSource
				(
				int nSourceID
				)
			{
				ID = 0;
				SourceID = nSourceID;
				Ranking = 1;
				Delete = false;
			}
			/// <summary>
			/// Class constructor for a clsQuickSource object where the ID and sourceID are known.
			/// </summary>
			/// <param name="nID">Specify the ID of the source link in the source link table.</param>
			/// <param name="nSourceID">Specify the sourceID of the clsQuickSource object.</param>
			/// <param name="nRanking">Specify the ranking for the source.</param>
			public clsQuickSource
				(
				int	nID,
				int nSourceID,
				int nRanking
				)
			{
				ID = nID;
				SourceID = nSourceID;
				Ranking = nRanking;
				Delete = false;
			}
		}

		#endregion

		/// <summary>Store which table contains list of sources.</summary>
		private enum enumSourceTable
		{
			/// <summary>Source for a person (m_nPersonID,m_nFactID {1-Name,2-DoB,3-DoD} )</summary>
			tbl_PeopleToSources,
			/// <summary>Source for a fact (m_nFactID)</summary>
			tbl_FactsToSources,
			/// <summary>Source for a relationship (m_nRelationID,m_nFactID {1-Date,2-Location,3-TerminationStatus,4-TerminationDate,5-Partner} )</summary>
			tbl_RelationshipsToSources,
			/// <summary>Source for a census record.</summary>
			tbl_CensusRecords
		}
		
		/// <summary>Table that contains the links to sources.</summary>
		private enumSourceTable m_SourceTable;

		/// <summary>Person that sources refer to.</summary>
		private int m_nPersonID;

        /// <summary>Relationship that sources refer to.</summary>
		private int m_nRelationID;

		/// <summary>Fact that this source refers to.</summary>
		private int m_nFactID;

		/// <summary>Census record that this source refers to.</summary>
		private int m_nCensusHouseholdID;
		
		/// <summary>Database that these sources are attached to.</summary>
		private Database m_oDB;
		
		/// <summary>Source for the fact.</summary>
		private ArrayList m_oSources;

		#endregion

		#region Constructors

		/// <summary>
		/// Class constructor to initialise the clsSources object to look at the tbl_FactsToSources table.
		/// </summary>
		/// <param name="nFactID">Specify ID of the fact that these sources are attached to</param>
		/// <param name="oDB">Specify the database that these sources are attached to</param>
		public clsSources
			(
			int nFactID,
			Database oDB
			)
		{
			m_SourceTable = enumSourceTable.tbl_FactsToSources;
			m_oSources = null;
			m_nPersonID = 0;
			m_nRelationID = 0;
			m_nFactID = nFactID;
			m_oDB = oDB;
		}
		/// <summary>
		/// Class constructor to initialise the clsSources object to look at the tbl_PeopleToSources table.
		/// </summary>
		/// <param name="nPersonID">Specifiy the ID of the person these sources are attached to</param>
		/// <param name="nFactID">Specifiy the fact that these sources are attached to.  1-Name, 2-DoB, 3-DoD</param>
		/// <param name="oDB">Specify the database that these sources are attached to</param>
		public clsSources
			(
			int nPersonID,
			int nFactID,
			Database oDB
			)
		{
			m_SourceTable = enumSourceTable.tbl_PeopleToSources;
			m_oSources = null;
			m_nPersonID = nPersonID;
			m_nFactID = nFactID;
			m_nRelationID = 0;
			m_oDB = oDB;
		}
		/// <summary>
		/// Class constructor to initialise the clsSources object to look at the tbl_RelationshipsToSources table.
		/// </summary>
		/// <param name="nRelationshipID">Specifiy the ID of the relationship these sources are attached to</param>
		/// <param name="nFactID">Specifiy the fact that these sources are attached to.  1-Date, 2-Location, 3-TerminationStatus, 4-TerminationDate, 5-Partner</param>
		/// <param name="oDB">Specify the database that these sources are attached to</param>
		public clsSources
			(						
			Database oDB,
			int nRelationshipID,
			int nFactID
			)
		{
			m_SourceTable = enumSourceTable.tbl_RelationshipsToSources;
			m_oSources = null;
			m_nPersonID = 0;
			m_nFactID = nFactID;
			m_nRelationID = nRelationshipID;
			m_oDB = oDB;
		}
		/// <summary>
		/// Class constructor to initialise the clsSources object to look at the census records.
		/// </summary>
		/// <param name="oCensus">Specifies the census record.</param>
		/// <param name="oDb">Specify the database that these sources are attached to</param>
		public clsSources
			(
			clsCensusPerson oCensus,
			Database oDb
			)
		{
			m_SourceTable = enumSourceTable.tbl_CensusRecords;
			m_nCensusHouseholdID = oCensus.HouseholdID;
			m_oDB = oDb;
		}
	
		#endregion
		
		#region Database

		/// <summary>
		/// Loads the sources for this information from the database.
		/// The data is loaded from tbl_PeopleToSources when m_nPersonID is known, otherwise tbl_FactsToSources is used.
		/// </summary>
		/// <returns>True for success, false otherwise.</returns>
		private bool LoadSources()
		{
			// Create a list of sources
			m_oSources = new ArrayList();
			
			// Create a list of the sources in the database.			
			string sSql = "";
			switch(m_SourceTable)
			{
			case enumSourceTable.tbl_FactsToSources:
				// Open the specified fact
				sSql = "SELECT ID,SourceID FROM tbl_FactsToSources WHERE FactID=" + m_nFactID.ToString() + " ORDER BY Rank;";
				break;
			
			case enumSourceTable.tbl_PeopleToSources:
				// Open the specified person			
				sSql = "SELECT ID,SourceID FROM tbl_PeopleToSources WHERE PersonID=" + m_nPersonID.ToString() + " AND FactID=" + m_nFactID.ToString() + " ORDER BY Rank;";
				break;
			
			case enumSourceTable.tbl_CensusRecords:
				sSql = "SELECT 1 AS Ingnore, ID FROM tbl_Sources WHERE AdditionalInfoTypeID=4 AND ID="+m_nCensusHouseholdID+";";
				break;
			
			case enumSourceTable.tbl_RelationshipsToSources:
			default:
				sSql = "SELECT ID,SourceID FROM tbl_RelationshipsToSources WHERE RelationshipID=" + m_nRelationID.ToString() + " AND FactID=" + m_nFactID.ToString() + " ORDER BY Rank;";
				break;
			}
			OleDbCommand oSql = new OleDbCommand(sSql,m_oDB.cnDB);

			// Read the list of sources from the database
			int nRanking = 0;
			OleDbDataReader drSources = oSql.ExecuteReader();
			while(drSources.Read())
			{
				int nID = drSources.GetInt32(0);
				int nSourceID = drSources.GetInt32(1);
				nRanking++;
				Add(nID,nSourceID,nRanking);
			}
			drSources.Close();

			// Return success
			return true;
		}

		/// <summary>
		/// Links this collection of sources to the owning fact.
		/// Does not save or create sources.
		/// Creates or updates the links between facts and sources.
		/// </summary>
		/// <returns>True for success.  False, otherwise.</returns>
		public bool Save()
		{
			OleDbCommand oSQL;			// OLE Command object
			clsQuickSource oQSource;
				
			for(int nI=0;nI<m_oSources.Count;nI++)
			{
				oQSource = (clsQuickSource)m_oSources[nI];
				if(oQSource.Delete)
				{
					if(oQSource.ID!=0)
					{
						switch(m_SourceTable)
						{
						case enumSourceTable.tbl_FactsToSources:
							oSQL = new OleDbCommand("DELETE FROM tbl_FactsToSources WHERE ID="+oQSource.ID.ToString()+";",m_oDB.cnDB);
							oSQL.ExecuteNonQuery();
							break;

						case enumSourceTable.tbl_PeopleToSources:
							oSQL = new OleDbCommand("DELETE FROM tbl_PeopleToSources WHERE ID="+oQSource.ID.ToString()+";",m_oDB.cnDB);
							oSQL.ExecuteNonQuery();
							break;

						case enumSourceTable.tbl_RelationshipsToSources:
							oSQL = new OleDbCommand("DELETE FROM tbl_RelationshipsToSources WHERE ID="+oQSource.ID.ToString()+";",m_oDB.cnDB);
							oSQL.ExecuteNonQuery();
							break;
						}
					}
				}
				else
				{				
					if(oQSource.ID==0)
					{				
						// Add a record
						switch(m_SourceTable)
						{
						case enumSourceTable.tbl_FactsToSources:
							oSQL = new OleDbCommand("INSERT INTO tbl_FactsToSources (FactID,Rank,SourceID) VALUES ("+m_nFactID.ToString()+","+oQSource.Ranking.ToString()+","+oQSource.SourceID.ToString()+");",m_oDB.cnDB);
							oSQL.ExecuteNonQuery();

							oSQL = new OleDbCommand("SELECT MAX(ID) AS NewID FROM tbl_FactsToSources;",m_oDB.cnDB);
							oQSource.ID = (int)oSQL.ExecuteScalar();
							break;
						case enumSourceTable.tbl_PeopleToSources:
							oSQL = new OleDbCommand("INSERT INTO tbl_PeopleToSources (PersonID,FactID,Rank,SourceID) VALUES ("+m_nPersonID.ToString()+","+m_nFactID.ToString()+","+oQSource.Ranking.ToString()+","+oQSource.SourceID.ToString()+");",m_oDB.cnDB);
							oSQL.ExecuteNonQuery();

							oSQL = new OleDbCommand("SELECT MAX(ID) AS NewID FROM tbl_PeopleToSources;",m_oDB.cnDB);
							oQSource.ID = (int)oSQL.ExecuteScalar();
							break;

						case enumSourceTable.tbl_RelationshipsToSources:
							oSQL = new OleDbCommand("INSERT INTO tbl_RelationshipsToSources (RelationshipID,FactID,Rank,SourceID) VALUES ("+m_nRelationID.ToString()+","+m_nFactID.ToString()+","+oQSource.Ranking.ToString()+","+oQSource.SourceID.ToString()+");",m_oDB.cnDB);
							oSQL.ExecuteNonQuery();

							oSQL = new OleDbCommand("SELECT MAX(ID) AS NewID FROM tbl_RelationshipsToSources;",m_oDB.cnDB);
							oQSource.ID = (int)oSQL.ExecuteScalar();
							break;
						}

						// Update the actual source with last used date
						oSQL = new OleDbCommand("UPDATE tbl_Sources SET LastUsed=Now() WHERE ID="+oQSource.SourceID.ToString()+";",m_oDB.cnDB);
						oSQL.ExecuteNonQuery();					
					}
					else
					{
						// Update the record
						// Only the rank can need changing.  The link really only exists or doesn't exist.
						string sSql = "";
						switch(m_SourceTable)
						{
						case enumSourceTable.tbl_FactsToSources:
							sSql = "UPDATE tbl_FactsToSources SET Rank="+oQSource.Ranking.ToString()+" WHERE ID="+oQSource.ID.ToString()+";";
							break;

						case enumSourceTable.tbl_PeopleToSources:
							sSql = "UPDATE tbl_PeopleToSources SET Rank="+oQSource.Ranking.ToString()+" WHERE ID="+oQSource.ID.ToString()+";";
							break;

						case enumSourceTable.tbl_RelationshipsToSources:
							sSql = "UPDATE tbl_RelationshipsToSources SET Rank="+oQSource.Ranking.ToString()+" WHERE ID="+oQSource.ID.ToString()+";";
							break;
						}
						if(sSql!="")
						{
							OleDbCommand oSql = new OleDbCommand(sSql,m_oDB.cnDB);
							oSql.ExecuteNonQuery();
						}
					}
				}
			}

			// Return success
			return true;
		}

		#endregion
		
		/// <summary>
		/// Returns an array of source ID for this fact.
		/// </summary>
		/// <returns>Returns an array of source ID for this fact.</returns>
		public int[] Get()
		{
			// If no sources then an empty array
			if(m_oSources==null)
			{
				LoadSources();
			}

			// Build an array of int to hold these results
			clsQuickSource oQSource;
			int[] nResults = new int[m_oSources.Count];
			for(int nI=0;nI<m_oSources.Count;nI++)
			{
				oQSource = (clsQuickSource)m_oSources[nI];
				if(oQSource.Delete)
				{
					nResults[nI] = -1;
				}
				else
				{
					nResults[nI] = oQSource.SourceID;
				}
			}

			// Return the array
			return nResults;
		}

		/// <summary>
		/// Returns the sources as an array of clsSource objects
		/// </summary>
		/// <returns>An array of clsSource objects</returns>
		public clsSource[] GetAsSources()
		{
			// If no sources then an empty array
			if(m_oSources==null)
			{
				LoadSources();
			}

			// Build an array of clsSource objects
			clsSource[] oSources;
			clsQuickSource oQSource;
			oSources = new clsSource[m_oSources.Count];
			for(int nI=0;nI<m_oSources.Count;nI++)
			{
				oQSource = (clsQuickSource)m_oSources[nI];
				if(oQSource.Delete)
				{
					oSources[nI] = null; // clsSource("[Deleted]");
				}
				else
				{
					oSources[nI] = new clsSource(m_oDB,(int)oQSource.SourceID);
					oSources[nI].Ranking = oQSource.Ranking;
				}
			}

			// Return the array of clsSource objects
			return oSources;
		}

		/// <summary>
		/// Adds a source ID to the collection of sources for this information
		/// </summary>
		/// <param name="nSourceID"></param>
		/// <returns></returns>
		public bool Add
			(
			int nSourceID
			)
		{
			return Add(0,nSourceID,100);
		}
		/// <summary>
		/// Adds a sourceID and nID to the collection of sources for this 
		/// </summary>
		/// <param name="nID">Specify the information key to add.</param>
		/// <param name="nSourceID">Specify the ID of the source to add.</param>
		/// <param name="nRanking">Specify the ranking for the new source.</param>
		/// <returns>True if the source is added to the collection.  False, otherwise.</returns>
		public bool Add
			(
			int nID,
			int nSourceID,
			int nRanking
			)
		{
			// Check if there are any existing members
			if(m_oSources==null)
			{
				LoadSources();
			}

			// Check that the source is a valid source
			if(nSourceID<=0)
			{
				return false;
			}

			// Check if the source is already in this collection.
			foreach(clsQuickSource oExisting in m_oSources)
			{
				if(oExisting.SourceID==nSourceID)
				{
					return false;
				}
			}

			// Add the source ID to the collection
			clsQuickSource oSource = new clsQuickSource(nID,nSourceID,nRanking);
			m_oSources.Add(oSource);

			// return success
			return true;
		}

		/// <summary>
		/// Marks the connection between the fact and the specified source for deletion.
		/// The actual source will not be deleted.
		/// </summary>
		/// <param name="nIndex">Specify the source to be removed from this collection.</param>
		/// <returns>True if a source is removed.  False, otherwise.</returns>
		public bool Delete
			(
			int nIndex
			)
		{
			// Validate the nIndex
			if(m_oSources==null)
			{
				return false;
			}
			if(nIndex<0||nIndex>=m_oSources.Count)
			{
				return false;
			}

			// Mark the entry as deleted
			clsQuickSource oQSource = (clsQuickSource)m_oSources[nIndex];
			oQSource.Delete = true;

			// Return success
			return true;
		}

		/// <summary>
		/// Adds this collection of sources to the specifeid list of sources.
		/// </summary>
		/// <param name="oSources">Specifies the list to receive the addtional sources.</param>
		public void GedcomAdd
			(
			ArrayList oSources
			)
		{
			int [] nID = Get();
			for(int nI=0;nI<nID.Length;nI++)
			{
				if(!oSources.Contains(nID[nI]))
				{	
					// Check that the source is Gedcom enabled
					if(IsGedcomEnabled(nID[nI]))
					{
						oSources.Add(nID[nI]);
					}
				}
			}
		}
		
		/// <summary>
		/// Writes this collection of sources into a Gedcom file.
		/// OLD STYLE - NOT USING ANY MORE.
		/// </summary>
		/// <param name="nLevel">Specifies the level on the tag in the gedcom file.</param>
		/// <param name="oFile">Specifies the Gedcom file to write the tag into.</param>
		/// <param name="oAlready">Specifies the list of sources already used.  The source is added to this list.  Use NULL to ignore.</param>
		public void GedcomWrite
			(
			int nLevel,
			StreamWriter oFile,
			ArrayList oAlready
			)
		{
			int [] nID = Get();
			for(int nI=0;nI<nID.Length;nI++)
			{
				bool bInclude = true;
				if(oAlready!=null)
				{
					if(oAlready.Contains(nID[nI]))
					{
						bInclude = false;
					}
					else
					{
						oAlready.Add(nID[nI]);
					}
				}
				if(bInclude)
				{
					// Check that the source is Gedcom enabled.
					string sSql = "SELECT Gedcom FROM tbl_Sources WHERE ID="+nID[nI].ToString()+";";
					OleDbCommand oSql = new OleDbCommand(sSql,m_oDB.cnDB);
					bInclude = bool.Parse(oSql.ExecuteScalar().ToString());
				}
				if(bInclude)
				{
					oFile.WriteLine(nLevel.ToString()+" SOUR @S"+nID[nI]+"@");
				}
			}
		}

		/// <summary>
		/// Returns true if the specified sourceID is allowed into Gedcom files.
		/// Returns false otherwise.
		/// </summary>
		/// <param name="nSourceID">Specifies the ID of the source to test for Gedcom export.</param>
		/// <returns>True, if the source is allowed into Gedcom files.  False, otherwise.</returns>
		private bool IsGedcomEnabled
			(
			int nSourceID
			)
		{
			string sSql = "SELECT Gedcom FROM tbl_Sources WHERE ID="+nSourceID.ToString()+";";
			OleDbCommand oSql = new OleDbCommand(sSql,m_oDB.cnDB);
			return bool.Parse(oSql.ExecuteScalar().ToString());
		}

		/// <summary>
		/// Returns the current ranking for the source at the specified index.
		/// </summary>
		/// <param name="nIndex">Specifies the index of the source that the index is required for.</param>
		/// <returns>The ranking of the specified source.</returns>
		public int GetRanking
			(
			int nIndex
			)
		{
			return ((clsQuickSource)m_oSources[nIndex]).Ranking;
		}
		
		/// <summary>
		/// Change the ranking on the specified (by index) source to the ranking specified.
		/// </summary>
		/// <remarks>
		/// The ranking is written to the database immediately because the source object may lose the focus and the changes are not cached in this case.
		/// </remarks>
		/// <param name="nIndex">Specifies the index of the source to change the ranking of.</param>
		/// <param name="nNewRanking">Specifies the new ranking to give to the specified source.</param>
		/// <returns>True for success, false otherwise.</returns>
		public bool ChangeRanking
			(
			int nIndex,
			int nNewRanking
			)
		{
			clsQuickSource oSource = (clsQuickSource)m_oSources[nIndex];
			oSource.Ranking = nNewRanking;

			/*
			switch(m_SourceTable)
			{
			case enumSourceTable.tbl_PeopleToSources:
				clsQuickSource oSource = (clsQuickSource)m_oSources[nIndex];
				oSource.Ranking = nNewRanking;

				int nID = oSource.ID;
				OleDbCommand oSql = new OleDbCommand("UPDATE tbl_PeopleToSources SET Rank="+nNewRanking.ToString()+" WHERE ID="+nID.ToString()+";",m_oDB.cnDB);
				oSql.ExecuteNonQuery();
				
				break;
			}
			
			*/

			// Return success
			return true;
		}

		#region Public Properties

		/// <summary>
		/// This is only really intended to be used by facts that enter the database.  That is their ID changes from 0
		/// to a valid ID
		/// </summary>
		public int FactID
		{
			get { return m_nFactID; }
			set { m_nFactID = value; }
		}

		/// <summary>
		/// This is only really intended to be used by people that enter the database.  That is their ID changes from 0
		/// to a valid ID.
		/// </summary>
		public int PersonID
		{
			get { return m_nPersonID; }
			set { m_nPersonID = value; }
	    }

		/// <summary>
		/// This is only really intended to be used by relationships that enter the database.  That is their ID changes
		/// from 0 to a valid ID.
		/// </summary>
		public int RelationshipID
		{
			get { return m_nRelationID; }
			set { m_nRelationID = value; }
		}

		#endregion
	}
}
