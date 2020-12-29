using System;
using System.Data;
using System.Data.OleDb;

// StringBuilder
using System.Text;

namespace FamilyTree.Objects
{
    // Class to represent the additional information on a census source.
    /// <summary>
    /// Class to represent the additional information on a census source.
    /// The clsCensusPerson records are children of these objects.
	/// </summary>
	public class clsCensus
	{
		#region Member Variables

		/// <summary>The ID of the census record.  This should match with the ID the parent source.</summary>
		private int m_nID;

        /// <summary>The source database for this census record.</summary>
        private Database m_oDb;

		/// <summary>The date of this census record.  In pratice all the census data from a particular year will be taken on the same date.</summary>
        public DateTime CensusDate;

		/// <summary>The address of this household.</summary>
        public string Address;

        /// <summary>This is also known as the RG number.</summary>
        public string Series;

        /// <summary>The second part of the census record reference.</summary>
        public string Piece;

        /// <summary>The third part of the census record reference.</summary>
        public string Folio;

        /// <summary>The fouth and final part of the census record reference.</summary>
        public string Page;

		#endregion

		#region Constructors etc ...

		/// <summary>
		/// Class constructor.
		/// Can not have an empty constructor since these must always be attached to a source object.
		/// </summary>
		/// <param name="nID">Specifies the ID of the parent source record and hence the ID of this census object.</param>
		public clsCensus
			(
			int nID
			)
		{
			m_nID = nID;
            m_oDb = null;
		}
		/// <summary>
		/// Class constructor where the current data is loaded from the specified database.
		/// </summary>
		/// <param name="nID">Specifies the ID of the parent source record and hence the ID of this census object.</param>
		/// <param name="oDb">Specifies the database to load the information from.</param>
		public clsCensus
			(
			int nID,
			Database oDb
			) : this(nID)
		{
            m_oDb = oDb;

			if(m_nID!=0)
			{
				string sSql = "SELECT CensusDate,Address,Series,Piece,Folio,Page FROM tbl_CensusHouseholds WHERE ID="+m_nID.ToString()+";";
				OleDbCommand oSql = new OleDbCommand(sSql,oDb.cnDB);
				OleDbDataReader drCensus = oSql.ExecuteReader();
				if(drCensus.Read())
				{
					CensusDate = drCensus.GetDateTime(0);
					Address = drCensus.GetString(1);
                    Series = Innoval.clsDatabase.GetString(drCensus,"Series","");
                    Piece = Innoval.clsDatabase.GetString(drCensus,"Piece","");
                    Folio = Innoval.clsDatabase.GetString(drCensus,"Folio","");
                    Page = Innoval.clsDatabase.GetString(drCensus,"Page","");
				}
				drCensus.Close();
			}
		}

		#endregion

        #region Database IO

        /// <summary>Writes the census record into the specified database.</summary>
		/// <param name="oDb">Specifies the database to write the census record into.</param>
		/// <returns>True for success, false otherwise.</returns>
		public bool Save			(			Database oDb			)
		{
			// Validate the ID
			if(m_nID==0)
			{
				return false;
			}

			// Write the record into the database
			string sSql = "UPDATE tbl_CensusHouseholds SET CensusDate=#"+CensusDate.ToString("d-MMM-yyyy")+"#, Address='"+Address+"',"+
                "Series="+Innoval.clsDatabase.ToDb(Series)+","+
                "Piece="+Innoval.clsDatabase.ToDb(Piece)+","+
                "Folio="+Innoval.clsDatabase.ToDb(Folio)+","+
                "Page="+Innoval.clsDatabase.ToDb(Page)+" "+
                "WHERE ID="+m_nID.ToString()+";";
			OleDbCommand oSql = new OleDbCommand(sSql,oDb.cnDB);
			int nNumRows = oSql.ExecuteNonQuery();
			if(nNumRows==0)
			{
				sSql = "INSERT INTO tbl_CensusHouseholds (ID,CensusDate,Address,Series,Piece,Folio,Page) VALUES ("+m_nID.ToString()+",#"+CensusDate.ToString("d-MMM-yyyy")+"#,'"+Address+"',"+
                    Innoval.clsDatabase.ToDb(Series) + "," +
                    Innoval.clsDatabase.ToDb(Piece) + "," +
                    Innoval.clsDatabase.ToDb(Folio) + "," +
                    Innoval.clsDatabase.ToDb(Page) + ");";
				oSql = new OleDbCommand(sSql,oDb.cnDB);
				oSql.ExecuteNonQuery();
			}

            // Add the place (and links to this source) to the database
            if(Address!="")
            {
                oDb.AddPlace(Address,2,m_nID);
            }

			// Return success
			return true;
        }

        #endregion

        #region General Public Methods and Properties

        /// <summary>Return the census information in Html format.</summary>
        /// <returns>A desription of the census information in Html format.</returns>
        public string ToHtml()
        {
            // Initialise the Html description.
            StringBuilder sbHtml = new StringBuilder();

            sbHtml.Append("<table align=\"center\" bgcolor=\"lightcyan\" border=\"0\" cellpadding=\"5\" cellspacing=\"0\">");
            sbHtml.Append("<tr><td colspan=\"5\"><table width=\"100%\"><tr>");
            sbHtml.Append("<td align=\"center\"><span class=\"Census\">Series</span></td>");
            sbHtml.Append("<td align=\"center\"><span class=\"Census\">Piece</span></td>");
            sbHtml.Append("<td align=\"center\"><span class=\"Census\">Folio</span></td>");
            sbHtml.Append("<td align=\"center\"><span class=\"Census\">Page</span></td>");

            sbHtml.Append("</tr></tr>");

            sbHtml.Append("<td align=\"center\">" + Series + "</td>");
            sbHtml.Append("<td align=\"center\">" + Piece + "</td>");
            sbHtml.Append("<td align=\"center\">" + Folio + "</td>");
            sbHtml.Append("<td align=\"center\">" + Page + "</td>");
            sbHtml.Append("</tr></table></td></tr>");

            sbHtml.Append("<tr><td colspan=\"5\"><span class=\"Census\">Address</span> " + m_oDb.PlaceToHtml(Address) + "</td></tr>");
            sbHtml.Append("<tr valign=\"bottom\">");
            sbHtml.Append("<td><span class=\"Census\">Name</span></td>");
            sbHtml.Append("<td><span class=\"Census\">Relation<br/>To Head</span></td>");
            sbHtml.Append("<td><span class=\"Census\">Age</span></td>");
            sbHtml.Append("<td><span class=\"Census\">Occupation</span></td>");
            sbHtml.Append("<td><span class=\"Census\">Born Location</span></td>");
            sbHtml.Append("</tr>");

            clsCensusPerson[] oMembers = GetMembers();
            foreach(clsCensusPerson oMember in oMembers)
            {
                sbHtml.Append("<tr>");
                sbHtml.Append("<td>");
                if(oMember.PersonID>0)
                {
                    sbHtml.Append("<a href=\"Person:"+oMember.PersonID.ToString()+"\">");
                }
                sbHtml.Append(oMember.CensusName);
                if(oMember.PersonID>0)
                {
                    sbHtml.Append("</a>");
                }
                sbHtml.Append("</td>");
                sbHtml.Append("<td>"+oMember.RelationToHead +"</td>");
                sbHtml.Append("<td>"+oMember.Age+"</td>");
                sbHtml.Append("<td>"+oMember.Occupation+"</td>");
                sbHtml.Append("<td>"+oMember.BornLocation+"</td>");
                sbHtml.Append("</tr>");
            }

            sbHtml.Append("</table>");

            // Return the Html description.
            return sbHtml.ToString();
            
        }

        /// <summary>Return the census certificate information in format for a webtrees birth certificate.</summary>
        /// <returns>The html to build a webtrees census certificate.</returns>
        public string ToWebtrees()
        {
            // Get the first person in this census
            clsCensusPerson[] oMembers = GetMembers();
            string sHead = oMembers[0].CensusName;

            // Initialise the Html description.
            StringBuilder sbHtml = new StringBuilder();

            sbHtml.Append("&lt;a name=\""+sHead.ToLower().Replace(' ', '_')+"_"+CensusDate.Year.ToString()+"\"&gt;&lt;/a&gt;<br/>");
            sbHtml.Append("&lt;h2&gt;Census "+CensusDate.Year.ToString()+" "+sHead+"&lt;/h2&gt;<br/>");

            sbHtml.Append("&lt;table class=\"census\"&gt;<br/>");
            sbHtml.Append("&lt;tr&gt;&lt;td colspan=\"5\"&gt;<br/>&lt;table class=\"data\" width=\"100%\"&gt;<br/>&lt;tr&gt;");
            sbHtml.Append("&lt;td&gt;Series&lt;/td&gt;");
            sbHtml.Append("&lt;td&gt;Piece&lt;/td&gt;");
            sbHtml.Append("&lt;td&gt;Folio&lt;/td&gt;");
            sbHtml.Append("&lt;td&gt;Page&lt;/td&gt;");

            sbHtml.Append("&lt;/tr&gt;<br/>&lt;tr&gt;");

            sbHtml.Append("&lt;td class=\"data\"&gt;" + Series + "&lt;/td&gt;");
            sbHtml.Append("&lt;td class=\"data\"&gt;" + Piece + "&lt;/td&gt;");
            sbHtml.Append("&lt;td class=\"data\"&gt;" + Folio + "&lt;/td&gt;");
            sbHtml.Append("&lt;td class=\"data\"&gt;" + Page + "&lt;/td&gt;");
            sbHtml.Append("&lt;/tr&gt;<br/>&lt;/table&gt;&lt;/td&gt;&lt;/tr&gt;<br/>");

            sbHtml.Append("&lt;tr&gt;&lt;td colspan=\"5\"&gt;Address &lt;span class=\"data\"&gt;" + Address + "&lt;/td&gt;&lt;/tr&gt;<br/>");
            sbHtml.Append("&lt;tr&gt;&lt;td&gt;Name&lt;/td&gt;&lt;td&gt;Relation&lt;br/&gt;To Head&lt;/td&gt;&lt;td&gt;Age&lt;/td&gt;&lt;td&gt;Occupation&lt;/td&gt;&lt;td&gt;Born Location&lt;/td&gt;&lt;/tr&gt;<br/>");


            foreach(clsCensusPerson oMember in oMembers)
            {
                sbHtml.Append("&lt;tr&gt;");
                sbHtml.Append("&lt;td class=\"data\"&gt;");
                sbHtml.Append(oMember.CensusName);
                sbHtml.Append("&lt;/td&gt;");
                sbHtml.Append("&lt;td class=\"data\"&gt;"+oMember.RelationToHead +"&lt;/td&gt;");
                sbHtml.Append("&lt;td class=\"data\"&gt;"+oMember.Age+"&lt;/td&gt;");
                sbHtml.Append("&lt;td class=\"data\"&gt;"+oMember.Occupation+"&lt;/td&gt;");
                sbHtml.Append("&lt;td class=\"data\"&gt;"+oMember.BornLocation+"&lt;/td&gt;");
                sbHtml.Append("&lt;/tr&gt;<br/>");
            }

            sbHtml.Append("&lt;/table&gt;<br/>");
            sbHtml.Append("&lt;table class=\"meta\"&gt;<br/>");
            sbHtml.Append("&lt;tr&gt;&lt;td class=\"label\"&gt;Filename&lt;/td&gt;&lt;td class=\"value\"&gt;census_"+CensusDate.Year.ToString()+"_"+sHead.ToLower().Replace(' ', '_')+".png&lt;/td&gt;&lt;/tr&gt;<br/>");
            sbHtml.Append("&lt;tr&gt;&lt;td class=\"label\"&gt;Media Title&lt;/td&gt;&lt;td class=\"value\"&gt;"+sHead+" Census "+CensusDate.Year.ToString()+"&lt;/td&gt;&lt;/tr&gt;<br/>");
            sbHtml.Append("&lt;tr&gt;&lt;td class=\"label\"&gt;Source Title&lt;/td&gt;&lt;td class=\"value\"&gt;Birth Certificate: "+sHead+" "+CensusDate.Year.ToString()+"&lt;/td&gt;&lt;/tr&gt;<br/>");
            sbHtml.Append("&lt;tr&gt;&lt;td class=\"label\"&gt;Source Text&lt;/td&gt;&lt;td class=\"value\"&gt;");
            foreach(clsCensusPerson oMember in oMembers)
            {
                sbHtml.Append(oMember.CensusName+" ("+oMember.Age+") ");
                if(oMember.Occupation !="")
                {
                    sbHtml.Append(" - "+oMember.Occupation);
                }
                if(oMember.BornLocation !="")
                {
                    sbHtml.Append(" - "+oMember.BornLocation);
                }
                sbHtml.Append("&lt;br/&gt;");
            }
            sbHtml.Append("&lt;/td&gt;&lt;/tr&gt;<br/>");
            sbHtml.Append("&lt;tr&gt;&lt;td class=\"label\"&gt;Source Note&lt;/td&gt;&lt;td class=\"value\"&gt;Series "+Series+" Piece "+Piece+" Folio "+Folio+" Page "+Page+".&lt;/td&gt;&lt;/tr&gt;<br/>");
            sbHtml.Append("&lt;tr&gt;&lt;td class=\"label\"&gt;Citation Text&lt;/td&gt;&lt;td class=\"value\"&gt;Living with ");
            foreach(clsCensusPerson oMember in oMembers)
            {
                sbHtml.Append(oMember.CensusName+" ("+oMember.Age+"), ");
            }
            sbHtml.Append("&lt;/td&gt;&lt;/tr&gt;<br/>");
            sbHtml.Append("&lt;/table&gt;<br/>");

            // Return the Html description.
            return sbHtml.ToString();
        }
        
        /// <summary>Return the members of this census record as clsCensusPerson records.</summary>
        /// <returns>A collection of clsCensusPerson records representing people in the census record.</returns>
        public clsCensusPerson[] GetMembers()
        {
            return m_oDb.CensusHouseholdMembers(m_nID);
        }

        /// <summary>The ID of the census record.  This should match with the ID the parent source.</summary>
		public int ID { get { return m_nID; } set { m_nID = value; } }

        #endregion
    }
}
