using System;
using System.Data;
using System.Data.OleDb;
using System.Collections;

// StringBuilder
using System.Text;

using FamilyTree.Objects;

namespace FamilyTree.Objects
{
	/// <summary>
	/// Class to represent a single source of information
	/// </summary>
	public class clsSource
	{
		#region Member Variables

		/// <summary>Database that contains this source.</summary>
		private Database m_oDb;

		/// <summary>ID of the source in the database.</summary>
		private int m_nID;

		/// <summary>Text to display for the source.</summary>
        private string m_sDescription;

		/// <summary>Date of the source (document).</summary>
        private clsDate m_dtTheDate;

		/// <summary>Comments for the source.</summary>
        private string m_sComments;

		/// <summary>Date and time this source was last used.</summary>
		private DateTime m_dtLastUsed;

		/// <summary>True, if this source needs writing to the database.  False, otherwise.</summary>
		private bool m_bDirty;

		/// <summary>True if the source should be deleted at next save.  False, otherwise.</summary>
		private bool m_bDelete;

		/// <summary>The type of additional information available for this source.  0 - None.</summary>
        private int m_nAdditionalInfoTypeID;

		/// <summary>Name of the user who wrote the last edit.</summary>
        private string m_sLastEditBy;

        /// <summary>Date and time of the last edit.</summary>
        private DateTime m_dtLastEditDate;

		/// <summary>The optional additional census information.</summary>
		private clsCensus m_oAdditionCensus;

		/// <summary>The optional additional marriage information.</summary>
		private clsMarriageCertificate m_oAdditionMarriage;

		/// <summary>The optional additional birth certificate information.</summary>
		private clsBirthCertificate m_oAdditionalBirth;

		/// <summary>The optional additional death certificate information.</summary>
		private clsDeathCertificate m_oAdditionalDeath;

		/// <summary>The index for the source.  Zero represents no repository.</summary>
		private int m_nRepositoryID;

		/// <summary>This is the ranking of the source within a collection.  This is not the property of the source but of the source collection table.</summary>
		private int m_nRanking;

		#endregion

		#region Constructors
		
		/// <summary>
		/// Class constructor for an empty source.
		/// </summary>
		/// <param name="oDB">Specify the database containing the source.</param>
		public clsSource
			(
			Database oDB
			)
		{
			m_oDb = oDB;
			m_sDescription = "";
			m_sComments = "";
			m_dtTheDate = new clsDate();
			m_bDirty = false;
			m_bDelete = false;
			m_nAdditionalInfoTypeID = 0;
			m_sLastEditBy = "Steve Walton";
			m_dtLastEditDate = DateTime.Now;
			m_oAdditionCensus = null;
			m_nRepositoryID = 0;
			m_nRanking = 1;
		}
		/// <summary>
		/// Class constructor to load an existing source from the database.
		/// </summary>
		/// <param name="oDB">Specify the database containing the source.</param>
		/// <param name="nID">Specify the ID of the source.</param>
		public clsSource
			(
			Database oDB,
			int nID
			):this(oDB)
		{
			// Open the database and get the source details			
			OleDbCommand oSQL = new OleDbCommand("SELECT Name,LastUsed,TheDate,TheDateStatusID,Comments,AdditionalInfoTypeID,LastEditBy,LastEditDate,RepositoryID FROM tbl_Sources WHERE ID=" + nID.ToString() + ";",m_oDb.cnDB);
			OleDbDataReader drSource = oSQL.ExecuteReader();
			if(drSource.Read())
			{
				m_nID = nID;
				if(drSource.IsDBNull(0))
				{
					m_sDescription = "";
				}
				else
				{
					m_sDescription = drSource.GetString(0);
				}
				if(drSource.IsDBNull(1))
				{
				}
				else
				{
					m_dtLastUsed = drSource.GetDateTime(1);
				}
				if(drSource.IsDBNull(2))
				{
					m_dtTheDate.Status = clsDate.EMPTY;
				}
				else
				{
					m_dtTheDate.Date = drSource.GetDateTime(2);
					m_dtTheDate.Status = drSource.GetInt32(3);
				}
				if(drSource.IsDBNull(4))
				{
					m_sComments = "";
				}
				else
				{
					m_sComments = drSource.GetString(4);
				}
				if(drSource.IsDBNull(5))
				{
					m_nAdditionalInfoTypeID = 0;
				}
				else
				{
					m_nAdditionalInfoTypeID = drSource.GetInt32(5);
				}
				if(drSource.IsDBNull(6))
				{
					m_sLastEditBy = "Steve Walton";
				}
				else
				{
					m_sLastEditBy = drSource.GetString(6);
				}
				if(drSource.IsDBNull(7))
				{
					m_dtLastEditDate = DateTime.Now;
				}
				else
				{
					m_dtLastEditDate = drSource.GetDateTime(7);
				}
				m_nRepositoryID = Database.GetInt(drSource,"RepositoryID",0);
			}
			drSource.Close();			
		}

		#endregion

		#region Database

		/// <summary>
		/// Write the changes in this source to the database
		/// </summary>
		/// <returns>True for success, false otherwise.</returns>
		public bool Save()
		{
			OleDbCommand oSQL;

			// Check if a delete is required
			if(m_bDelete)
			{
				if(m_nID!=0)
				{
                    // Remove the links to places from this source
                    m_oDb.PlaceDelink(2,m_nID);

					// Delete the sources that contain this source

					// Delete this source
					oSQL = new OleDbCommand("DELETE FROM tbl_Sources WHERE ID="+m_nID+";",m_oDb.cnDB);
					oSQL.ExecuteNonQuery();
				}

				// Return success
				return true;
			}

			// Check if a save is required
			if(m_bDirty)
			{
				// Create a new record if required
				if(m_nID==0)
				{
					// Create a new record
					oSQL = new OleDbCommand("INSERT INTO tbl_Sources (LastUsed) VALUES(Now());",m_oDb.cnDB);
					oSQL.ExecuteNonQuery();

					// Get the ID of the new record
					oSQL = new OleDbCommand("SELECT MAX(ID) AS NewID FROM tbl_Sources;",m_oDb.cnDB);
					m_nID = (int)oSQL.ExecuteScalar();
				}

				// Update the record
				oSQL = new OleDbCommand
					(
					"UPDATE tbl_Sources SET Name="+Database.ToDb(m_sDescription)
					+",TheDate="+Database.ToDb(m_dtTheDate)
					+",TheDateStatusID="+m_dtTheDate.Status.ToString()
					+",Comments="+Database.ToDb(m_sComments)
					+",AdditionalInfoTypeID="+m_nAdditionalInfoTypeID.ToString()
					+",RepositoryID="+m_nRepositoryID.ToString()
					+",LastEditBy='Steve Walton'"
					+",LastEditDate=Now()"
					+" WHERE ID=" + m_nID.ToString() +";"
					,m_oDb.cnDB
					);			
				oSQL.ExecuteNonQuery();								
				m_bDirty = false;

                // Remove the link to places from this source.
                m_oDb.PlaceDelink(2,m_nID);
			}

			switch(m_nAdditionalInfoTypeID)
			{
			case 1:
				if(m_oAdditionalBirth!=null)
				{					
					m_oAdditionalBirth.ID = m_nID;
					m_oAdditionalBirth.Save(m_oDb);
				}
				break;
			
			case 2:
				if(m_oAdditionMarriage!=null)
				{
					m_oAdditionMarriage.ID = m_nID;
					m_oAdditionMarriage.Save(m_oDb);
				}
				break;
			
			case 3:
				if(m_oAdditionalDeath!=null)
				{
					m_oAdditionalDeath.ID = m_nID;
					m_oAdditionalDeath.Save(m_oDb);
				}
				break;
			
			case 4:
				if(m_oAdditionCensus!=null)
				{
					m_oAdditionCensus.ID = m_nID;
					m_oAdditionCensus.CensusDate = m_dtTheDate.Date;
					m_oAdditionCensus.Save(m_oDb);
				}
				break;
			}

			// Return success
			return true;
		}

		/// <summary>
		/// Marks this source for deletion at the next save
		/// </summary>
		public void Delete()
		{
			m_bDelete = true;
		}

		#endregion

        /// <summary>Returns a Html description of the source.</summary>
        /// <returns></returns>
        public string ToHtml()
        {
            // Build a Html description of the source.
            StringBuilder sbHtml = new StringBuilder();

            // Intialise the Html document.
            sbHtml.Append("<body>");

            // The basic description of the source.
            sbHtml.Append("<p>");
            sbHtml.Append(m_sDescription);
            sbHtml.Append(" ("+m_dtTheDate.Format(DateFormat.FullLong)+")");
            sbHtml.Append("</p>");

            // Show the document specified information.
            switch(m_nAdditionalInfoTypeID)
            {
            case 1:
                if(AdditionalBirth!=null)
                {
                    sbHtml.Append(AdditionalBirth.ToHtml());
                }
                break;
            case 2:
                if(AdditionalMarriage!=null)
                {
                    sbHtml.Append(AdditionalMarriage.ToHtml(m_oDb));
                }
                break;
            case 3:
                if(AdditionalDeath!=null)
                {
                    sbHtml.Append(AdditionalDeath.ToHtml(m_oDb));
                }
                break;
            case 4:
                if(AdditionalCensus!=null)
                {
                    sbHtml.Append(AdditionalCensus.ToHtml());
                }
                break;
            }

            // Show the public comments.
            if(m_sComments!="")
            {
                sbHtml.Append("<p><strong>Public comments</strong>: " + m_sComments + "</p>");
            }

            // Show the objects that are linked to this document.
            sbHtml.Append("<p>References</p><table cellpadding=3 cellspacing=2>");
            clsReferences[] oReferences = GetReferences();
            foreach(clsReferences oReference in oReferences)
            {
                sbHtml.Append("<TR>");
                sbHtml.Append("<TD bgcolor=silver><SPAN class=\"Small\"><A href=\"Person:"+oReference.PersonID.ToString()+"\">"+oReference.PersonName+"</A></SPAN></TD>");
                sbHtml.Append("<TD bgcolor=silver><SPAN class=\"Small\">"+oReference.References+"</SPAN></TD>");
                sbHtml.Append("</TR>");
            }
            sbHtml.Append("</TABLE>");

            // Development Only.
            // Show a version of this document in the format that we will use on webtrees.            
            switch(m_nAdditionalInfoTypeID)
            {
            case 1:
                if(AdditionalBirth!=null)
                {
                    sbHtml.Append("<h2>Webtrees</h2><p class=\"small\">");
                    sbHtml.Append(AdditionalBirth.ToWebtrees());
                    sbHtml.Append("</p>\n");
                }
                break;
            case 2:
                if(AdditionalMarriage!=null)
                {
                    sbHtml.Append("<h2>Webtrees</h2><p class=\"small\">");
                    sbHtml.Append(AdditionalMarriage.ToWebtrees(m_oDb));
                    sbHtml.Append("</p>\n");
                }
                break;
            case 3:
                if(AdditionalDeath!=null)
                {
                    sbHtml.Append("<h2>Webtrees</h2><p class=\"small\">");
                    sbHtml.Append(AdditionalDeath.ToWebtrees(m_oDb));
                    sbHtml.Append("</p>\n");
                }
                break;
            case 4:
                if(AdditionalCensus!=null)
                {
                    sbHtml.Append("<h2>Webtrees</h2><p class=\"small\">");
                    sbHtml.Append(AdditionalCensus.ToWebtrees());
                    sbHtml.Append("</p>\n");
                }
                break;
            }

            // Finish the Html document.
            sbHtml.Append("<P align=right><SPAN class=\"Small\">Last edit by "+m_sLastEditBy+" on "+m_dtLastEditDate.ToString("d-MMM-yyyy HH:mm:ss")+"</SPAN></P>");
            sbHtml.Append("</body>");
            sbHtml.Append("</html>");

            // Return the Html description
            return sbHtml.ToString();
        }
        
        
        /// <summary>
		/// Returns true for a valid source.
        /// Returns false for a source marked for deletion currently.
		/// </summary>
		/// <returns>True for a valid source, false otherwise</returns>
		public bool IsValid()
		{
			return !m_bDelete;
		}
		
		/// <summary>
		/// Returns a human readable summary of the source.
		/// </summary>
		/// <returns>A human readable summary of the source.</returns>
		public override string ToString()
		{
			if(m_dtTheDate.IsEmpty())
			{
				return m_sDescription;
			}
			return m_sDescription+" ("+m_dtTheDate.Date.Year.ToString()+")";
		}

		/// <summary>
		/// Returns an array of clsReferences objects that list all the "facts" that reference this source.
		/// </summary>
		/// <returns>An array of clsReferences objects</returns>
        public clsReferences[] GetReferences()
		{
			// Create an empty list
			ArrayList oReferences = new ArrayList();

			// Get all the references in the people objects
            // string sSql = "SELECT tbl_PeopleToSources.PersonID, tlk_PeopleInfo.Name AS FactName,tbl_PeopleToSources.FactID FROM tbl_PeopleToSources INNER JOIN tlk_PeopleInfo ON tbl_PeopleToSources.FactID = tlk_PeopleInfo.ID WHERE tbl_PeopleToSources.SourceID=" + m_nID.ToString() + ";";
            string sSql = "SELECT tbl_PeopleToSources.PersonID, tlk_PeopleInfo.Name AS FactName, tbl_PeopleToSources.FactID " +
                "FROM tbl_People INNER JOIN (tbl_PeopleToSources INNER JOIN tlk_PeopleInfo ON tbl_PeopleToSources.FactID = tlk_PeopleInfo.ID) ON tbl_People.ID = tbl_PeopleToSources.PersonID " +
                "WHERE (((tbl_PeopleToSources.SourceID)=" + m_nID.ToString() + ")) " +
                "ORDER BY tbl_People.Born;";
			OleDbCommand oSql = new OleDbCommand(sSql,m_oDb.cnDB);
			OleDbDataReader drReferences = oSql.ExecuteReader();
			while(drReferences.Read())
			{
				int nIndex = GetReferenceIndex(oReferences,drReferences.GetInt32(0));
				// Don't add the non specific references
				if(Database.GetInt(drReferences,"FactID",0)!=0)
				{
					((clsReferences)oReferences[nIndex]).AddReference(drReferences.GetString(1));
				}
			}
			drReferences.Close();

			// Get all the references in the fact objects
			sSql = "SELECT tbl_Facts.PersonID, tlk_FactTypes.Name FROM tlk_FactTypes INNER JOIN (tbl_Facts INNER JOIN tbl_FactsToSources ON tbl_Facts.ID = tbl_FactsToSources.FactID) ON tlk_FactTypes.ID = tbl_Facts.TypeID WHERE (((tbl_FactsToSources.SourceID)="+m_nID.ToString()+"));";
			oSql = new OleDbCommand(sSql,m_oDb.cnDB);
			drReferences = oSql.ExecuteReader();
			while(drReferences.Read())
			{
				int nIndex = GetReferenceIndex(oReferences,drReferences.GetInt32(0));
				((clsReferences)oReferences[nIndex]).AddReference(drReferences.GetString(1));
			}
			drReferences.Close();

			// Get all the references in the relationship objects
			sSql = "SELECT tbl_Relationships.MaleID, tbl_Relationships.FemaleID, tlk_RelationshipInfo.Name FROM tbl_Relationships INNER JOIN (tbl_RelationshipsToSources INNER JOIN tlk_RelationshipInfo ON tbl_RelationshipsToSources.FactID = tlk_RelationshipInfo.ID) ON tbl_Relationships.ID = tbl_RelationshipsToSources.RelationshipID WHERE (((tbl_RelationshipsToSources.SourceID)="+m_nID.ToString()+"));";
			oSql = new OleDbCommand(sSql,m_oDb.cnDB);
			drReferences = oSql.ExecuteReader();
			while(drReferences.Read())
			{
				int nIndex = GetReferenceIndex(oReferences,drReferences.GetInt32(0));
				((clsReferences)oReferences[nIndex]).AddReference("Relationship "+drReferences.GetString(2));
				nIndex = GetReferenceIndex(oReferences,drReferences.GetInt32(1));
				((clsReferences)oReferences[nIndex]).AddReference("Relationship "+drReferences.GetString(2));
			}
			drReferences.Close();

			// Get names for all the people
			for(int nIndex=0;nIndex<oReferences.Count;nIndex++)
			{
				clsReferences oReference = (clsReferences)oReferences[nIndex];
				clsPerson oPerson = new clsPerson(oReference.PersonID,m_oDb);
				oReference.PersonName = oPerson.GetName(true,false);
			}

			// Return the references found
			return (clsReferences[])oReferences.ToArray(typeof(clsReferences));
		}

		/// <summary>
		/// Returns the index of the specified person in the specified list of references.
		/// Added the person to the specified list if required.
		/// </summary>
		/// <param name="oReferences">Specifies the list of existing references.</param>
		/// <param name="nPersonID">Specifies the person to locate in / add to the reference list.</param>
		/// <returns>The index of the specified person in the specified list.</returns>
		private int GetReferenceIndex
			(
			ArrayList	oReferences,
			int nPersonID
			)
		{
			int	nIndex;

			// Find the index of the person
			nIndex = -1;
			for(int nI=0;nI<oReferences.Count;nI++)
			{
				if(nPersonID==((clsReferences)oReferences[nI]).PersonID)
				{
					nIndex = nI;
					break;
				}
			}
			if(nIndex==-1)
			{
				// Add a new reference
				clsReferences oReference = new clsReferences(nPersonID);
				
				oReferences.Add(oReference);
				nIndex = oReferences.Count-1;
			}

			return nIndex;
		}

		#region Public Properties

		/// <summary>ID to this source record in the database.</summary>
		public int ID { get { return m_nID; } }
		
		/// <summary>Human readable summary of this source.</summary>
		public string Label { get { return this.ToString(); } }

		/// <summary> Detailed description of this source.</summary>
		public string Description { get { return m_sDescription; } set { m_sDescription = value; m_bDirty = true; } }

		/// <summary>The date of this source.</summary>
		public clsDate TheDate { get { return m_dtTheDate; } }

		/// <summary>User comments on this source.</summary>
		public string Comments { get { return m_sComments; } set { m_sComments = value; m_bDirty = true; } }

		/// <summary>The date / time that this source was last cited for a fact.</summary>
		public DateTime LastUsed { get { return m_dtLastUsed; } set { m_dtLastUsed = value; m_bDirty = true; } }

		/// <summary>The type of additional information available for this source.  0 - None.</summary>
		public int AdditionalInfoTypeID { get { return m_nAdditionalInfoTypeID; } set { m_nAdditionalInfoTypeID = value; m_bDirty = true; } }

		/// <summary>Name of the user who wrote the last edit.</summary>
		public string LastEditBy { get { return m_sLastEditBy; } set { m_sLastEditBy = value; } }

		/// <summary>Date and time of the last edit.</summary>
		public DateTime LastEditDate { get { return m_dtLastEditDate; } set { m_dtLastEditDate = value; } }

		/// <summary>This is the ranking of the source within a collection.  This is not the property of the source but of the source collection table.</summary>
		public int Ranking { get { return m_nRanking; } set { m_nRanking = value; } }

		/// <summary>
		/// The additional census information.
		/// Only valid if AdditionalTypeID == Census (4).
		/// </summary>
		public clsCensus AdditionalCensus
		{
			get
			{
				if(m_oAdditionCensus==null)
				{
					m_oAdditionCensus = new clsCensus(m_nID,m_oDb);
				}
				return m_oAdditionCensus;
			}
		}

		/// <summary>
		/// The optional additional marriage information.
		/// Only valid in AdditionalTypeID == Marriage (2).
		/// </summary>
		public clsMarriageCertificate AdditionalMarriage
		{
			get
			{
				if(m_oAdditionMarriage==null)
				{
					m_oAdditionMarriage = new clsMarriageCertificate(m_nID,m_oDb.cnDB);
				}
				return m_oAdditionMarriage;
			}
		}

		/// <summary>
		/// The optional additional birth certificate information.
		/// Only valid if the AdditionalTypeID == Birth (1).
		/// </summary>
		public clsBirthCertificate AdditionalBirth
		{
			get
			{
				if(m_oAdditionalBirth==null)
				{
					m_oAdditionalBirth = new clsBirthCertificate(m_nID,m_oDb.cnDB);
				}
				return m_oAdditionalBirth;
			}
		}

		/// <summary>
		/// The optional additional death certificate information.
		/// Only valid if the AdditionalTypeID == Death (3)
		/// </summary>
		public clsDeathCertificate AdditionalDeath
		{
			get
			{
				if(m_oAdditionalDeath==null)
				{
					m_oAdditionalDeath = new clsDeathCertificate(m_nID,m_oDb.cnDB);
				}
				return m_oAdditionalDeath;
			}
		}

		/// <summary>The index for the source.  Zero represents no repository.</summary>
		public int Repository { get { return m_nRepositoryID; } set { m_nRepositoryID = value; } }

		#endregion
	}
}
