using System;
using System.Data;
using System.Data.OleDb;

// StringBuilder
using System.Text;

namespace FamilyTree.Objects
{
    /// <summary>Class to represent the additional information on a census source.  The clsCensusPerson records are children of these objects.</summary>
	public class clsCensus
    {
        #region Member Variables

        /// <summary>The ID of the census record.  This should match with the ID the parent source.</summary>
        private int index_;

        /// <summary>The source database for this census record.</summary>
        private Database database_;

        /// <summary>The date of this census record.  In pratice all the census data from a particular year will be taken on the same date.</summary>
        public DateTime censusDate;

        /// <summary>The address of this household.</summary>
        public string address;

        /// <summary>This is also known as the RG number.</summary>
        public string series;

        /// <summary>The second part of the census record reference.</summary>
        public string piece;

        /// <summary>The third part of the census record reference.</summary>
        public string folio;

        /// <summary>The fouth and final part of the census record reference.</summary>
        public string page;

        #endregion

        #region Constructors etc ...



        /// <summary>Class constructor.  Can not have an empty constructor since these must always be attached to a source object.</summary>
        /// <param name="index">Specifies the ID of the parent source record and hence the ID of this census object.</param>
        public clsCensus(int index)
        {
            index_ = index;
            database_ = null;
        }



        /// <summary>Class constructor where the current data is loaded from the specified database.</summary>
        /// <param name="index">Specifies the ID of the parent source record and hence the ID of this census object.</param>
        /// <param name="database">Specifies the database to load the information from.</param>
        public clsCensus(int index, Database database) : this(index)
        {
            database_ = database;

            if (index_ != 0)
            {
                string sql = "SELECT CensusDate, Address, Series, Piece, Folio, Page FROM tbl_CensusHouseholds WHERE ID = " + index_.ToString() + ";";
                OleDbCommand sqlCommand = new OleDbCommand(sql, database.cndb);
                OleDbDataReader dataReader = sqlCommand.ExecuteReader();
                if (dataReader.Read())
                {
                    censusDate = dataReader.GetDateTime(0);
                    address = dataReader.GetString(1);
                    series = walton.Database.getString(dataReader, "Series", "");
                    piece = walton.Database.getString(dataReader, "Piece", "");
                    folio = walton.Database.getString(dataReader, "Folio", "");
                    page = walton.Database.getString(dataReader, "Page", "");
                }
                dataReader.Close();
            }
        }



        #endregion

        #region Database IO



        /// <summary>Writes the census record into the specified database.</summary>
        /// <param name="database">Specifies the database to write the census record into.</param>
        /// <returns>True for success, false otherwise.</returns>
        public bool save(Database database)
        {
            // Validate the ID.
            if (index_ == 0)
            {
                return false;
            }

            // Write the record into the database.
            string sql = "UPDATE tbl_CensusHouseholds SET CensusDate = #" + censusDate.ToString("d-MMM-yyyy") + "#, Address = '" + address + "', " + "Series = " + walton.Database.toDb(series) + "," + "Piece = " + walton.Database.toDb(piece) + "," + "Folio = " + walton.Database.toDb(folio) + "," + "Page = " + walton.Database.toDb(page) + " " + "WHERE ID = " + index_.ToString() + ";";
            OleDbCommand sqlCommand = new OleDbCommand(sql, database.cndb);
            int numRows = sqlCommand.ExecuteNonQuery();
            if (numRows == 0)
            {
                sql = "INSERT INTO tbl_CensusHouseholds (ID, CensusDate, Address, Series, Piece, Folio, Page) VALUES (" + index_.ToString() + ", #" + censusDate.ToString("d-MMM-yyyy") + "#, '" + address + "', " + walton.Database.toDb(series) + ", " + walton.Database.toDb(piece) + ", " + walton.Database.toDb(folio) + ", " + walton.Database.toDb(page) + ");";
                sqlCommand = new OleDbCommand(sql, database.cndb);
                sqlCommand.ExecuteNonQuery();
            }

            // Add the place (and links to this source) to the database.
            if (address != "")
            {
                database.addPlace(address, 2, index_);
            }

            // Return success.
            return true;
        }



        #endregion

        #region General Public Methods and Properties



        /// <summary>Return the census information in Html format.</summary>
        /// <returns>A desription of the census information in Html format.</returns>
        public string toHtml()
        {
            // Initialise the Html description.
            StringBuilder html = new StringBuilder();

            html.Append("<table align=\"center\" bgcolor=\"lightcyan\" border=\"0\" cellpadding=\"5\" cellspacing=\"0\">");
            html.Append("<tr><td colspan=\"5\"><table width=\"100%\"><tr>");
            html.Append("<td align=\"center\"><span class=\"Census\">Series</span></td>");
            html.Append("<td align=\"center\"><span class=\"Census\">Piece</span></td>");
            html.Append("<td align=\"center\"><span class=\"Census\">Folio</span></td>");
            html.Append("<td align=\"center\"><span class=\"Census\">Page</span></td>");

            html.Append("</tr></tr>");

            html.Append("<td align=\"center\">" + series + "</td>");
            html.Append("<td align=\"center\">" + piece + "</td>");
            html.Append("<td align=\"center\">" + folio + "</td>");
            html.Append("<td align=\"center\">" + page + "</td>");
            html.Append("</tr></table></td></tr>");

            html.Append("<tr><td colspan=\"5\"><span class=\"Census\">Address</span> " + database_.placeToHtml(address) + "</td></tr>");
            html.Append("<tr valign=\"bottom\">");
            html.Append("<td><span class=\"Census\">Name</span></td>");
            html.Append("<td><span class=\"Census\">Relation<br/>To Head</span></td>");
            html.Append("<td><span class=\"Census\">Age</span></td>");
            html.Append("<td><span class=\"Census\">Occupation</span></td>");
            html.Append("<td><span class=\"Census\">Born Location</span></td>");
            html.Append("</tr>");

            clsCensusPerson[] members = getMembers();
            foreach (clsCensusPerson member in members)
            {
                html.Append("<tr>");
                html.Append("<td>");
                if (member.personIndex > 0)
                {
                    html.Append("<a href=\"Person:" + member.personIndex.ToString() + "\">");
                }
                html.Append(member.censusName);
                if (member.personIndex > 0)
                {
                    html.Append("</a>");
                }
                html.Append("</td>");
                html.Append("<td>" + member.relationToHead + "</td>");
                html.Append("<td>" + member.age + "</td>");
                html.Append("<td>" + member.occupation + "</td>");
                html.Append("<td>" + member.bornLocation + "</td>");
                html.Append("</tr>");
            }

            html.Append("</table>");

            // Return the Html description.
            return html.ToString();
        }



        /// <summary>Return the census certificate information in format for a webtrees birth certificate.</summary>
        /// <returns>The html to build a webtrees census certificate.</returns>
        public string toWebtrees()
        {
            // Get the first person in this census
            clsCensusPerson[] members = getMembers();
            string head = members[0].censusName;

            // Initialise the Html description.
            StringBuilder html = new StringBuilder();

            html.Append("&lt;a name=\"" + head.ToLower().Replace(' ', '_') + "_" + censusDate.Year.ToString() + "\"&gt;&lt;/a&gt;<br/>");
            html.Append("&lt;h2&gt;Census " + censusDate.Year.ToString() + " " + head + "&lt;/h2&gt;<br/>");

            html.Append("&lt;table class=\"census\"&gt;<br/>");
            html.Append("&lt;tr&gt;&lt;td colspan=\"5\"&gt;<br/>&lt;table class=\"data\" width=\"100%\"&gt;<br/>&lt;tr&gt;");
            html.Append("&lt;td&gt;Series&lt;/td&gt;");
            html.Append("&lt;td&gt;Piece&lt;/td&gt;");
            html.Append("&lt;td&gt;Folio&lt;/td&gt;");
            html.Append("&lt;td&gt;Page&lt;/td&gt;");

            html.Append("&lt;/tr&gt;<br/>&lt;tr&gt;");

            html.Append("&lt;td class=\"data\"&gt;" + series + "&lt;/td&gt;");
            html.Append("&lt;td class=\"data\"&gt;" + piece + "&lt;/td&gt;");
            html.Append("&lt;td class=\"data\"&gt;" + folio + "&lt;/td&gt;");
            html.Append("&lt;td class=\"data\"&gt;" + page + "&lt;/td&gt;");
            html.Append("&lt;/tr&gt;<br/>&lt;/table&gt;&lt;/td&gt;&lt;/tr&gt;<br/>");

            html.Append("&lt;tr&gt;&lt;td colspan=\"5\"&gt;Address &lt;span class=\"data\"&gt;" + address + "&lt;/td&gt;&lt;/tr&gt;<br/>");
            html.Append("&lt;tr&gt;&lt;td&gt;Name&lt;/td&gt;&lt;td&gt;Relation&lt;br/&gt;To Head&lt;/td&gt;&lt;td&gt;Age&lt;/td&gt;&lt;td&gt;Occupation&lt;/td&gt;&lt;td&gt;Born Location&lt;/td&gt;&lt;/tr&gt;<br/>");


            foreach (clsCensusPerson member in members)
            {
                html.Append("&lt;tr&gt;");
                html.Append("&lt;td class=\"data\"&gt;");
                html.Append(member.censusName);
                html.Append("&lt;/td&gt;");
                html.Append("&lt;td class=\"data\"&gt;" + member.relationToHead + "&lt;/td&gt;");
                html.Append("&lt;td class=\"data\"&gt;" + member.age + "&lt;/td&gt;");
                html.Append("&lt;td class=\"data\"&gt;" + member.occupation + "&lt;/td&gt;");
                html.Append("&lt;td class=\"data\"&gt;" + member.bornLocation + "&lt;/td&gt;");
                html.Append("&lt;/tr&gt;<br/>");
            }

            html.Append("&lt;/table&gt;<br/>");
            html.Append("&lt;table class=\"meta\"&gt;<br/>");
            html.Append("&lt;tr&gt;&lt;td class=\"label\"&gt;Filename&lt;/td&gt;&lt;td class=\"value\"&gt;census_" + censusDate.Year.ToString() + "_" + head.ToLower().Replace(' ', '_') + ".png&lt;/td&gt;&lt;/tr&gt;<br/>");
            html.Append("&lt;tr&gt;&lt;td class=\"label\"&gt;Media Title&lt;/td&gt;&lt;td class=\"value\"&gt;" + head + " Census " + censusDate.Year.ToString() + "&lt;/td&gt;&lt;/tr&gt;<br/>");
            html.Append("&lt;tr&gt;&lt;td class=\"label\"&gt;Source Title&lt;/td&gt;&lt;td class=\"value\"&gt;Birth Certificate: " + head + " " + censusDate.Year.ToString() + "&lt;/td&gt;&lt;/tr&gt;<br/>");
            html.Append("&lt;tr&gt;&lt;td class=\"label\"&gt;Source Text&lt;/td&gt;&lt;td class=\"value\"&gt;");
            foreach (clsCensusPerson member in members)
            {
                html.Append(member.censusName + " (" + member.age + ") ");
                if (member.occupation != "")
                {
                    html.Append(" - " + member.occupation);
                }
                if (member.bornLocation != "")
                {
                    html.Append(" - " + member.bornLocation);
                }
                html.Append("&lt;br/&gt;");
            }
            html.Append("&lt;/td&gt;&lt;/tr&gt;<br/>");
            html.Append("&lt;tr&gt;&lt;td class=\"label\"&gt;Source Note&lt;/td&gt;&lt;td class=\"value\"&gt;Series " + series + " Piece " + piece + " Folio " + folio + " Page " + page + ".&lt;/td&gt;&lt;/tr&gt;<br/>");
            html.Append("&lt;tr&gt;&lt;td class=\"label\"&gt;Citation Text&lt;/td&gt;&lt;td class=\"value\"&gt;Living with ");
            foreach (clsCensusPerson member in members)
            {
                html.Append(member.censusName + " (" + member.age + "), ");
            }
            html.Append("&lt;/td&gt;&lt;/tr&gt;<br/>");
            html.Append("&lt;/table&gt;<br/>");

            // Return the Html description.
            return html.ToString();
        }



        /// <summary>Return the members of this census record as clsCensusPerson records.</summary>
        /// <returns>A collection of clsCensusPerson records representing people in the census record.</returns>
        public clsCensusPerson[] getMembers()
        {
            return database_.censusHouseholdMembers(index_);
        }



        /// <summary>The ID of the census record.  This should match with the ID the parent source.</summary>
		public int index { get { return index_; } set { index_ = value; } }

        #endregion
    }
}
