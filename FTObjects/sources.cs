using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace family_tree.objects
{
    /// <summary>
    /// Class to represent a collection of sources for pieces of information.
    /// The pieces of information can be distinguished by the ID field.
    /// This is the links between the owning fact and a collection of source objects.
    /// </summary>
    public class Sources
    {
        #region Member Variables

        #region QuickSource

        /// <summary>
        /// Class to store the minimum required information on each source in the collection.
        /// This information is stored in memory to link to the actual source objects (which are dropped from memory once they are used).
        /// 
        /// Class to store the link between objects and sources.
        /// These objects store information to identfy the source and the linked specific information on an object.
        /// </summary>
        private class QuickSource
        {
            /// <summary>ID of the source link in the source link table.</summary>
            public int idx;

            /// <summary>ID of the source record in the database.</summary>
            public int sourceIdx;

            /// <summary>The ranking of the source in this list.</summary>
            public int ranking;

            /// <summary>True to delete this record.  False, otherwise.</summary>
            public bool isDelete;

            /// <summary>Class constructor for an empty clsQuickSource object.</summary>
            public QuickSource()
            {
                idx = 0;
                sourceIdx = 0;
                ranking = 1;
                isDelete = false;
            }
            /// <summary>Class constructor for a clsQuickSource object where the SourceID is known.</summary>
            /// <param name="paraSourceIdx">Specify the SourceID value.</param>
            public QuickSource(int paraSourceIdx)
            {
                idx = 0;
                sourceIdx = paraSourceIdx;
                ranking = 1;
                isDelete = false;
            }
            /// <summary>Class constructor for a clsQuickSource object where the ID and sourceID are known.</summary>
            /// <param name="paraIdx">Specify the ID of the source link in the source link table.</param>
            /// <param name="paraSourceIdx">Specify the sourceID of the clsQuickSource object.</param>
            /// <param name="paraRanking">Specify the ranking for the source.</param>
            public QuickSource(int paraIdx, int paraSourceIdx, int paraRanking)
            {
                idx = paraIdx;
                sourceIdx = paraSourceIdx;
                ranking = paraRanking;
                isDelete = false;
            }
        }

        #endregion

        /// <summary>Store which table contains list of sources.</summary>
        private enum SourceTable
        {
            /// <summary>Source for a person (m_nPersonID,m_nFactID {1-Name,2-DoB,3-DoD} )</summary>
            PEOPLE_TO_SOURCES,
            /// <summary>Source for a fact (m_nFactID)</summary>
            FACTS_TO_SOURCES,
            /// <summary>Source for a relationship (m_nRelationID,m_nFactID {1-Date,2-Location,3-TerminationStatus,4-TerminationDate,5-Partner} )</summary>
            RELATIONSHIPS_TO_SOURCES,
            /// <summary>Source for a census record.</summary>
            CENSUS_RECORDS
        }

        /// <summary>Table that contains the links to sources.</summary>
        private SourceTable sourceTable_;

        /// <summary>Person that sources refer to.</summary>
        private int personIdx_;

        /// <summary>Relationship that sources refer to.</summary>
		private int relationIdx_;

        /// <summary>Fact that this source refers to.</summary>
        private int factIdx_;

        /// <summary>Census record that this source refers to.</summary>
        private int censusHouseholdIdx_;

        /// <summary>Database that these sources are attached to.</summary>
        private Database database_;

        /// <summary>Source for the fact.</summary>
        private ArrayList sources_;

        #endregion

        #region Constructors



        /// <summary>Class constructor to initialise the clsSources object to look at the tbl_FactsToSources table.</summary>
        /// <param name="factIdx">Specify ID of the fact that these sources are attached to</param>
        /// <param name="database">Specify the database that these sources are attached to</param>
        public Sources(int factIdx, Database database)
        {
            sourceTable_ = SourceTable.FACTS_TO_SOURCES;
            sources_ = null;
            personIdx_ = 0;
            relationIdx_ = 0;
            factIdx_ = factIdx;
            database_ = database;
        }



        /// <summary>Class constructor to initialise the clsSources object to look at the tbl_PeopleToSources table.</summary>
        /// <param name="personIdx">Specifiy the ID of the person these sources are attached to</param>
        /// <param name="factIdx">Specifiy the fact that these sources are attached to.  1-Name, 2-DoB, 3-DoD</param>
        /// <param name="database">Specify the database that these sources are attached to</param>
        public Sources(int personIdx, int factIdx, Database database)
        {
            sourceTable_ = SourceTable.PEOPLE_TO_SOURCES;
            sources_ = null;
            personIdx_ = personIdx;
            factIdx_ = factIdx;
            relationIdx_ = 0;
            database_ = database;
        }



        /// <summary>Class constructor to initialise the clsSources object to look at the tbl_RelationshipsToSources table.</summary>
        /// <param name="relationshipIdx">Specifiy the ID of the relationship these sources are attached to</param>
        /// <param name="factIdx">Specifiy the fact that these sources are attached to.  1-Date, 2-Location, 3-TerminationStatus, 4-TerminationDate, 5-Partner</param>
        /// <param name="database">Specify the database that these sources are attached to</param>
        public Sources(Database database, int relationshipIdx, int factIdx)
        {
            sourceTable_ = SourceTable.RELATIONSHIPS_TO_SOURCES;
            sources_ = null;
            personIdx_ = 0;
            factIdx_ = factIdx;
            relationIdx_ = relationshipIdx;
            database_ = database;
        }



        /// <summary>Class constructor to initialise the clsSources object to look at the census records.</summary>
        /// <param name="censusPerson">Specifies the census record.</param>
        /// <param name="database">Specify the database that these sources are attached to</param>
        public Sources(CensusPerson censusPerson, Database database)
        {
            sourceTable_ = SourceTable.CENSUS_RECORDS;
            censusHouseholdIdx_ = censusPerson.houseHoldIdx;
            database_ = database;
        }



        #endregion

        #region Database



        /// <summary>Loads the sources for this information from the database.  The data is loaded from tbl_PeopleToSources when m_nPersonID is known, otherwise tbl_FactsToSources is used.</summary>
        /// <returns>True for success, false otherwise.</returns>
        private bool loadSources()
        {
            // Create a list of sources.
            sources_ = new ArrayList();

            // Create a list of the sources in the database.
            string sql = "";
            switch (sourceTable_)
            {
            case SourceTable.FACTS_TO_SOURCES:
                // Open the specified fact.
                sql = "SELECT ID, SourceID FROM tbl_FactsToSources WHERE FactID = " + factIdx_.ToString() + " ORDER BY Rank;";
                break;

            case SourceTable.PEOPLE_TO_SOURCES:
                // Open the specified person.
                sql = "SELECT ID, SourceID FROM tbl_PeopleToSources WHERE PersonID = " + personIdx_.ToString() + " AND FactID = " + factIdx_.ToString() + " ORDER BY Rank;";
                break;

            case SourceTable.CENSUS_RECORDS:
                sql = "SELECT 1 AS Ingnore, ID FROM tbl_Sources WHERE AdditionalInfoTypeID = 4 AND ID = " + censusHouseholdIdx_ + ";";
                break;

            case SourceTable.RELATIONSHIPS_TO_SOURCES:
            default:
                sql = "SELECT ID, SourceID FROM tbl_RelationshipsToSources WHERE RelationshipID = " + relationIdx_.ToString() + " AND FactID = " + factIdx_.ToString() + " ORDER BY Rank;";
                break;
            }
            OleDbCommand sqlCommand = new OleDbCommand(sql, database_.cndb);

            // Read the list of sources from the database.
            int ranking = 0;
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                int idx = dataReader.GetInt32(0);
                int sourceIdx = dataReader.GetInt32(1);
                ranking++;
                add(idx, sourceIdx, ranking);
            }
            dataReader.Close();

            // Return success.
            return true;
        }



        /// <summary>Links this collection of sources to the owning fact.  Does not save or create sources.  Creates or updates the links between facts and sources.</summary>
        /// <returns>True for success.  False, otherwise.</returns>
        public bool save()
        {
            OleDbCommand sqlCommand;
            QuickSource quickSource;

            for (int i = 0; i < sources_.Count; i++)
            {
                quickSource = (QuickSource)sources_[i];
                if (quickSource.isDelete)
                {
                    if (quickSource.idx != 0)
                    {
                        switch (sourceTable_)
                        {
                        case SourceTable.FACTS_TO_SOURCES:
                            sqlCommand = new OleDbCommand("DELETE FROM tbl_FactsToSources WHERE ID = " + quickSource.idx.ToString() + ";", database_.cndb);
                            sqlCommand.ExecuteNonQuery();
                            break;

                        case SourceTable.PEOPLE_TO_SOURCES:
                            sqlCommand = new OleDbCommand("DELETE FROM tbl_PeopleToSources WHERE ID = " + quickSource.idx.ToString() + ";", database_.cndb);
                            sqlCommand.ExecuteNonQuery();
                            break;

                        case SourceTable.RELATIONSHIPS_TO_SOURCES:
                            sqlCommand = new OleDbCommand("DELETE FROM tbl_RelationshipsToSources WHERE ID = " + quickSource.idx.ToString() + ";", database_.cndb);
                            sqlCommand.ExecuteNonQuery();
                            break;
                        }
                    }
                }
                else
                {
                    if (quickSource.idx == 0)
                    {
                        // Add a record.
                        switch (sourceTable_)
                        {
                        case SourceTable.FACTS_TO_SOURCES:
                            sqlCommand = new OleDbCommand("INSERT INTO tbl_FactsToSources (FactID, Rank, SourceID) VALUES (" + factIdx_.ToString() + ", " + quickSource.ranking.ToString() + ", " + quickSource.sourceIdx.ToString() + ");", database_.cndb);
                            sqlCommand.ExecuteNonQuery();

                            sqlCommand = new OleDbCommand("SELECT MAX(ID) AS NewID FROM tbl_FactsToSources;", database_.cndb);
                            quickSource.idx = (int)sqlCommand.ExecuteScalar();
                            break;
                        case SourceTable.PEOPLE_TO_SOURCES:
                            sqlCommand = new OleDbCommand("INSERT INTO tbl_PeopleToSources (PersonID, FactID, Rank, SourceID) VALUES (" + personIdx_.ToString() + ", " + factIdx_.ToString() + ", " + quickSource.ranking.ToString() + ", " + quickSource.sourceIdx.ToString() + ");", database_.cndb);
                            sqlCommand.ExecuteNonQuery();

                            sqlCommand = new OleDbCommand("SELECT MAX(ID) AS NewID FROM tbl_PeopleToSources;", database_.cndb);
                            quickSource.idx = (int)sqlCommand.ExecuteScalar();
                            break;

                        case SourceTable.RELATIONSHIPS_TO_SOURCES:
                            sqlCommand = new OleDbCommand("INSERT INTO tbl_RelationshipsToSources (RelationshipID, FactID, Rank, SourceID) VALUES (" + relationIdx_.ToString() + ", " + factIdx_.ToString() + ", " + quickSource.ranking.ToString() + ", " + quickSource.sourceIdx.ToString() + ");", database_.cndb);
                            sqlCommand.ExecuteNonQuery();

                            sqlCommand = new OleDbCommand("SELECT MAX(ID) AS NewID FROM tbl_RelationshipsToSources;", database_.cndb);
                            quickSource.idx = (int)sqlCommand.ExecuteScalar();
                            break;
                        }

                        // Update the actual source with last used date.
                        sqlCommand = new OleDbCommand("UPDATE tbl_Sources SET LastUsed = Now() WHERE ID = " + quickSource.sourceIdx.ToString() + ";", database_.cndb);
                        sqlCommand.ExecuteNonQuery();
                    }
                    else
                    {
                        // Update the record.
                        // Only the rank can need changing.  The link really only exists or doesn't exist.
                        string sql = "";
                        switch (sourceTable_)
                        {
                        case SourceTable.FACTS_TO_SOURCES:
                            sql = "UPDATE tbl_FactsToSources SET Rank = " + quickSource.ranking.ToString() + " WHERE ID = " + quickSource.idx.ToString() + ";";
                            break;

                        case SourceTable.PEOPLE_TO_SOURCES:
                            sql = "UPDATE tbl_PeopleToSources SET Rank = " + quickSource.ranking.ToString() + " WHERE ID = " + quickSource.idx.ToString() + ";";
                            break;

                        case SourceTable.RELATIONSHIPS_TO_SOURCES:
                            sql = "UPDATE tbl_RelationshipsToSources SET Rank = " + quickSource.ranking.ToString() + " WHERE ID = " + quickSource.idx.ToString() + ";";
                            break;
                        }
                        if (sql != "")
                        {
                            sqlCommand = new OleDbCommand(sql, database_.cndb);
                            sqlCommand.ExecuteNonQuery();
                        }
                    }
                }
            }

            // Return success.
            return true;
        }



        #endregion



        /// <summary>Returns an array of source ID for this fact.</summary>
        /// <returns>Returns an array of source ID for this fact.</returns>
        public int[] get()
        {
            // If no sources then an empty array.
            if (sources_ == null)
            {
                loadSources();
            }

            // Build an array of int to hold these results.
            QuickSource quickSource;
            int[] results = new int[sources_.Count];
            for (int i = 0; i < sources_.Count; i++)
            {
                quickSource = (QuickSource)sources_[i];
                if (quickSource.isDelete)
                {
                    results[i] = -1;
                }
                else
                {
                    results[i] = quickSource.sourceIdx;
                }
            }

            // Return the array.
            return results;
        }



        /// <summary>Returns the sources as an array of source objects.</summary>
        /// <returns>An array of source objects.</returns>
        public Source[] getAsSources()
        {
            // If no sources then an empty array.
            if (sources_ == null)
            {
                loadSources();
            }

            // Build an array of source objects.
            Source[] sources;
            QuickSource quickSource;
            sources = new Source[sources_.Count];
            for (int i = 0; i < sources_.Count; i++)
            {
                quickSource = (QuickSource)sources_[i];
                if (quickSource.isDelete)
                {
                    sources[i] = null;
                }
                else
                {
                    sources[i] = new Source(database_, (int)quickSource.sourceIdx);
                    sources[i].ranking = quickSource.ranking;
                }
            }

            // Return the array of source objects.
            return sources;
        }



        /// <summary>Adds a source ID to the collection of sources for this information.</summary>
        /// <param name="sourceIdx">Specifies the index of the source to add.</param>
        /// <returns>True if the source is added to the collection.  False, otherwise.</returns>
        public bool add(int sourceIdx)
        {
            return add(0, sourceIdx, 100);
        }



        /// <summary>Adds a sourceIndex and index to the collection of sources for this.</summary>
        /// <param name="idx">Specify the information key to add.</param>
        /// <param name="sourceIdx">Specify the ID of the source to add.</param>
        /// <param name="ranking">Specify the ranking for the new source.</param>
        /// <returns>True if the source is added to the collection.  False, otherwise.</returns>
        public bool add(int idx, int sourceIdx, int ranking)
        {
            // Check if there are any existing members
            if (sources_ == null)
            {
                loadSources();
            }

            // Check that the source is a valid source
            if (sourceIdx <= 0)
            {
                return false;
            }

            // Check if the source is already in this collection.
            foreach (QuickSource existing in sources_)
            {
                if (existing.sourceIdx == sourceIdx)
                {
                    return false;
                }
            }

            // Add the source ID to the collection.
            QuickSource quickSource = new QuickSource(idx, sourceIdx, ranking);
            sources_.Add(quickSource);

            // return success.
            return true;
        }



        /// <summary>Marks the connection between the fact and the specified source for deletion.  The actual source will not be deleted.</summary>
        /// <param name="sourceIdx">Specify the source to be removed from this collection.</param>
        /// <returns>True if a source is removed.  False, otherwise.</returns>
        public bool delete(int sourceIdx)
        {
            // Validate the index.
            if (sources_ == null)
            {
                return false;
            }
            if (sourceIdx < 0 || sourceIdx >= sources_.Count)
            {
                return false;
            }

            // Mark the entry as deleted.
            QuickSource quickSource = (QuickSource)sources_[sourceIdx];
            quickSource.isDelete = true;

            // Return success.
            return true;
        }



        /// <summary>Adds this collection of sources to the specifeid list of sources.</summary>
        /// <param name="sources">Specifies the list to receive the addtional sources.</param>
        public void gedcomAdd(ArrayList sources)
        {
            int[] idxs = get();
            for (int i = 0; i < idxs.Length; i++)
            {
                if (!sources.Contains(idxs[i]))
                {
                    // Check that the source is Gedcom enabled.
                    if (isGedcomEnabled(idxs[i]))
                    {
                        sources.Add(idxs[i]);
                    }
                }
            }
        }

        /// <summary>
        /// Writes this collection of sources into a Gedcom file.        
        /// </summary>
        /// <param name="level">Specifies the level on the tag in the gedcom file.</param>
        /// <param name="file">Specifies the Gedcom file to write the tag into.</param>
        /// <param name="already">Specifies the list of sources already used.  The source is added to this list.  Use NULL to ignore.</param>
        public void writeGedcom(int level, StreamWriter file, ArrayList already)
        {
            int[] idxs = get();
            for (int i = 0; i < idxs.Length; i++)
            {
                bool isInclude = true;
                if (already != null)
                {
                    if (already.Contains(idxs[i]))
                    {
                        isInclude = false;
                    }
                    else
                    {
                        already.Add(idxs[i]);
                    }
                }
                // I think this function is only used now if 'All Elements' is true.
                //if (isInclude)
                //{
                //    // Check that the source is Gedcom enabled.
                //    string sql = "SELECT Gedcom FROM tbl_Sources WHERE ID=" + ids[i].ToString() + ";";
                //    OleDbCommand oSql = new OleDbCommand(sql, database_.cndb);
                //    isInclude = bool.Parse(oSql.ExecuteScalar().ToString());
                //}
                if (isInclude)
                {
                    file.WriteLine(level.ToString() + " SOUR @S" + idxs[i].ToString("0000") + "@");
                }
            }
        }


        ///// <summary>
        ///// Writes this collection of sources into a Gedcom file.
        ///// OLD STYLE - NOT USING ANY MORE.
        ///// </summary>
        ///// <param name="nLevel">Specifies the level on the tag in the gedcom file.</param>
        ///// <param name="oFile">Specifies the Gedcom file to write the tag into.</param>
        ///// <param name="oAlready">Specifies the list of sources already used.  The source is added to this list.  Use NULL to ignore.</param>
        //public void GedcomWrite
        //    (
        //    int nLevel,
        //    StreamWriter oFile,
        //    ArrayList oAlready
        //    )
        //{
        //    int[] nID = get();
        //    for (int nI = 0; nI < nID.Length; nI++)
        //    {
        //        bool bInclude = true;
        //        if (oAlready != null)
        //        {
        //            if (oAlready.Contains(nID[nI]))
        //            {
        //                bInclude = false;
        //            }
        //            else
        //            {
        //                oAlready.Add(nID[nI]);
        //            }
        //        }
        //        if (bInclude)
        //        {
        //            // Check that the source is Gedcom enabled.
        //            string sSql = "SELECT Gedcom FROM tbl_Sources WHERE ID=" + nID[nI].ToString() + ";";
        //            OleDbCommand oSql = new OleDbCommand(sSql, database_.cndb);
        //            bInclude = bool.Parse(oSql.ExecuteScalar().ToString());
        //        }
        //        if (bInclude)
        //        {
        //            oFile.WriteLine(nLevel.ToString() + " SOUR @S" + nID[nI] + "@");
        //        }
        //    }
        //}



        /// <summary>Returns true if the specified sourceID is allowed into Gedcom files.  Returns false otherwise.</summary>
        /// <param name="sourceIdx">Specifies the ID of the source to test for Gedcom export.</param>
        /// <returns>True, if the source is allowed into Gedcom files.  False, otherwise.</returns>
        private bool isGedcomEnabled(int sourceIdx)
        {
            string sql = "SELECT Gedcom FROM tbl_Sources WHERE ID = " + sourceIdx.ToString() + ";";
            OleDbCommand sqlCommand = new OleDbCommand(sql, database_.cndb);
            return bool.Parse(sqlCommand.ExecuteScalar().ToString());
        }



        /// <summary>Returns the current ranking for the source at the specified index.</summary>
        /// <param name="sourceIdx">Specifies the index of the source that the index is required for.</param>
        /// <returns>The ranking of the specified source.</returns>
        public int getRanking(int sourceIdx)
        {
            return ((QuickSource)sources_[sourceIdx]).ranking;
        }



        /// <summary>Change the ranking on the specified (by index) source to the ranking specified.</summary>
        /// <remarks>The ranking is written to the database immediately because the source object may lose the focus and the changes are not cached in this case.</remarks>
        /// <param name="sourceIdx">Specifies the index of the source to change the ranking of.</param>
        /// <param name="newRanking">Specifies the new ranking to give to the specified source.</param>
        /// <returns>True for success, false otherwise.</returns>
        public bool changeRanking(int sourceIdx, int newRanking)
        {
            QuickSource quickSource = (QuickSource)sources_[sourceIdx];
            quickSource.ranking = newRanking;

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

            // Return success.
            return true;
        }



        #region Public Properties



        /// <summary>This is only really intended to be used by facts that enter the database.  That is their ID changes from 0 to a valid ID.</summary>
        public int factIdx
        {
            get { return factIdx_; }
            set { factIdx_ = value; }
        }



        /// <summary>This is only really intended to be used by people that enter the database.  That is their ID changes from 0 to a valid ID.</summary>
        public int personIdx
        {
            get { return personIdx_; }
            set { personIdx_ = value; }
        }



        /// <summary>This is only really intended to be used by relationships that enter the database.  That is their ID changes from 0 to a valid ID.</summary>
        public int relationshipIdx
        {
            get { return relationIdx_; }
            set { relationIdx_ = value; }
        }



        #endregion
    }
}
