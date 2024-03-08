using System;
using System.Data;
using System.Data.OleDb;

// StringBuilder
using System.Text;

namespace family_tree.objects
{
    /// <summary>Class to represent the additional information on a marriage certificate source.  This is closely related to the tbl_MarriageCertificates table.</summary>
    public class MarriageCertificate
    {
        #region Member Variables

        /// <summary>The ID of the marriage certificate record.  This should match with the ID the parent source.</summary>
		private int idx_;

        /// <summary>The date of the marriage.</summary>
        public DateTime when;

        /// <summary>The location as specified on the marriage certificate.</summary>
        public string location;

        /// <summary>The name of the groom as specified on the marriage certificate.</summary>
        public string groomName;

        /// <summary>The age of the groom as specified on the marriage certificate.</summary>
        public string groomAge;

        /// <summary>The occupation of the groom as specified on the marriage certificate.</summary>
        public string groomOccupation;

        /// <summary>The location where the groom was living as specified on the marriage certificate.</summary>
        public string groomLiving;

        /// <summary>The name of the groom's father as specified on the marriage certificate.</summary>
        public string groomFather;

        /// <summary>The occupation of the groom's father as specified on the marriage certificate.</summary>
        public string groomFatherOccupation;

        /// <summary>The name of the bride as specified on the marriage certificate.</summary>
        public string brideName;

        /// <summary>The age of the bride as specified on the marriage certificate.</summary>
        public string brideAge;

        /// <summary>The occupation of the bride as specified on the marriage certificate.</summary>
        public string brideOccupation;

        /// <summary>The location where the bride was living a specified on the marriage certificate.</summary>
        public string brideLiving;

        /// <summary>The name of the bride's father as specified on the marriage certificate.</summary>
        public string brideFather;

        /// <summary>The occupation of the bride's father as specified on the marriage certificate.</summary>
        public string brideFatherOccupation;

        /// <summary>The names of the witnesses as specified on the marriage certificate.</summary>
        public string witness;

        /// <summary>The GRO reference for the certificate.</summary>
        public string groReference;

        #endregion

        #region Constructors etc ...



        /// <summary>Class constructor.</summary>
		/// <param name="sourceIdx">Specifies the ID of the parent source record and the ID of this object.</param>
		public MarriageCertificate(int sourceIdx)
        {
            idx_ = sourceIdx;
        }



        /// <summary>Class constructor that loads the current values from the database.</summary>
        /// <param name="sourceIdx">Specifies the ID of the parent source record and the ID of this object.</param>
        /// <param name="cndb">Specifies the database to load the values from.</param>
        public MarriageCertificate(int sourceIdx, OleDbConnection cndb) : this(sourceIdx)
        {
            string sql = "SELECT * FROM tbl_MarriageCertificates WHERE ID=" + idx_.ToString() + ";";
            OleDbCommand sqlCommand = new OleDbCommand(sql, cndb);
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            if (dataReader.Read())
            {
                when = walton.Database.getDateTime(dataReader, "WhenMarried", DateTime.Now);
                location = Database.getString(dataReader, "Location", "");
                groomName = Database.getString(dataReader, "GroomName", "");
                groomAge = Database.getString(dataReader, "GroomAge", "");
                groomOccupation = Database.getString(dataReader, "GroomOccupation", "");
                groomLiving = Database.getString(dataReader, "GroomLiving", "");
                groomFather = Database.getString(dataReader, "GroomFather", "");
                groomFatherOccupation = Database.getString(dataReader, "GroomFatherOccupation", "");
                brideName = Database.getString(dataReader, "BrideName", "");
                brideAge = Database.getString(dataReader, "BrideAge", "");
                brideOccupation = Database.getString(dataReader, "BrideOccupation", "");
                brideLiving = Database.getString(dataReader, "BrideLiving", "");
                brideFather = Database.getString(dataReader, "BrideFather", "");
                brideFatherOccupation = Database.getString(dataReader, "BrideFatherOccupation", "");
                witness = Database.getString(dataReader, "Witness", "");
                groReference = Database.getString(dataReader, "GroReference", "");
            }
            dataReader.Close();
        }



        #endregion

        /// <summary>Writes the marriage record into the specified database.</summary>
		/// <param name="database">Specifies the database to write the marriage record into.</param>
		/// <returns>True for success, false otherwise.</returns>
		public bool save(Database database)
        {
            // Validate the ID.
            if (idx_ == 0)
            {
                return false;
            }

            // Write the record into the database.
            string sql = "UPDATE tbl_MarriageCertificates SET " +
                "GroReference=" + Database.toDb(groReference) + ", " +
                "WhenMarried=" + Database.toDb(when) + ", " +
                "Location=" + Database.toDb(location)
                + ", GroomName=" + Database.toDb(groomName)
                + ", GroomAge=" + Database.toDb(groomAge)
                + ", GroomOccupation=" + Database.toDb(groomOccupation)
                + ", GroomLiving=" + Database.toDb(groomLiving)
                + ", GroomFather=" + Database.toDb(groomFather)
                + ", GroomFatherOccupation=" + Database.toDb(groomFatherOccupation)
                + ", BrideName=" + Database.toDb(brideName)
                + ", BrideAge=" + Database.toDb(brideAge)
                + ", BrideOccupation=" + Database.toDb(brideOccupation)
                + ", BrideLiving=" + Database.toDb(brideLiving)
                + ", BrideFather=" + Database.toDb(brideFather)
                + ", BrideFatherOccupation=" + Database.toDb(brideFatherOccupation)
                + ", Witness=" + Database.toDb(witness)
                + " WHERE ID=" + idx_.ToString() + ";";
            OleDbCommand sqlCommand = new OleDbCommand(sql, database.cndb);
            int numRows = sqlCommand.ExecuteNonQuery();
            if (numRows == 0)
            {
                sql = "INSERT INTO tbl_MarriageCertificates (ID, GroReference, WhenMarried, Location, GroomName, GroomAge, GroomOccupation, GroomLiving, GroomFather, GroomFatherOccupation, BrideName, BrideAge, BrideOccupation, BrideLiving, BrideFather, BrideFatherOccupation, Witness) VALUES (" +
                    idx_.ToString() + "," +
                    Database.toDb(groReference) + ", " +
                    Database.toDb(when) + ", " +
                    Database.toDb(location)
                    + ", " + Database.toDb(groomName)
                    + ", " + Database.toDb(groomAge)
                    + ", " + Database.toDb(groomOccupation)
                    + ", " + Database.toDb(groomLiving)
                    + ", " + Database.toDb(groomFather)
                    + ", " + Database.toDb(groomFatherOccupation)
                    + ", " + Database.toDb(brideName)
                    + ", " + Database.toDb(brideAge)
                    + ", " + Database.toDb(brideOccupation)
                    + ", " + Database.toDb(brideLiving)
                    + ", " + Database.toDb(brideFather)
                    + ", " + Database.toDb(brideFatherOccupation)
                    + ", " + Database.toDb(witness)
                    + ");";
                sqlCommand = new OleDbCommand(sql, database.cndb);
                sqlCommand.ExecuteNonQuery();
            }

            // Add the place (and links to this source) to the database.
            if (location != "")
            {
                database.addPlace(location, 2, idx_);
            }

            // Return success.
            return true;
        }



        /// <summary>Returns the marriage certificate information in html format.</summary>
        /// <returns>A description of the marriage certificate in html format.</returns>
        public string toHtml(Database database)
        {
            // Initialise the Html description.
            StringBuilder html = new StringBuilder();

            html.Append("<table style=\"background-color: #ccff99; border: 1px solid black;\" align=\"center\" cellpadding=\"5\" cellspacing=\"0\">");
            html.Append("<TR><TD colspan=7>" + when.Year.ToString() + " <SPAN class=\"Marriage\">Marriage solemnized at</SPAN> ");
            html.Append(database.placeToHtml(location) + "</td></tr>");
            html.Append("<TR>");
            html.Append("<TD><SPAN class=\"Marriage\">When Married</SPAN></TD>");
            html.Append("<TD><SPAN class=\"Marriage\">Name</SPAN></TD>");
            html.Append("<TD><SPAN class=\"Marriage\">Age</SPAN></TD>");
            html.Append("<TD><SPAN class=\"Marriage\">Rank or Profession</SPAN></TD>");
            html.Append("<TD><SPAN class=\"Marriage\">Residence at the time of marriage</SPAN></TD>");
            html.Append("<TD><SPAN class=\"Marriage\">Father's Name</SPAN></TD>");
            html.Append("<TD><SPAN class=\"Marriage\">Rank of Profession of Father</SPAN></TD>");
            html.Append("</TR>");
            html.Append("<TR>");
            html.Append("<TD rowspan=2>" + when.ToString("d MMM yyyy") + "</TD>");
            html.Append("<TD>" + groomName + "</TD>");
            html.Append("<TD>" + groomAge + "</TD>");
            html.Append("<TD>" + groomOccupation + "</TD>");
            html.Append("<TD>" + groomLiving + "</TD>");
            html.Append("<TD>" + groomFather + "</TD>");
            html.Append("<TD>" + groomFatherOccupation + "</TD>");
            html.Append("</TR>");
            html.Append("<TR>");
            // sbHtml.Append("<TD><SPAN class=\"Small\">Bride</SPAN></TD>");
            html.Append("<TD>" + brideName + "</TD>");
            html.Append("<TD>" + brideAge + "</TD>");
            html.Append("<TD>" + brideOccupation + "</TD>");
            html.Append("<TD>" + brideLiving + "</TD>");
            html.Append("<TD>" + brideFather + "</TD>");
            html.Append("<TD>" + brideFatherOccupation + "</TD>");
            html.Append("</TR>");
            html.Append("<TR><TD colspan=7><SPAN class=\"Marriage\">in the Presence of us,</SPAN> ");
            html.Append(witness + "</TD></TR>");
            html.Append("<TR><TD colspan=7 align=center><SPAN class=\"Marriage\">GRO Reference</SPAN> ");
            html.Append(groReference + "</TD></TR>");
            html.Append("</table>");

            // Return the Html description.
            return html.ToString();
        }



        /// <summary>Returns the marriarge certificate information in the webtrees format.</summary>
        /// <param name="database">Specifies a connection to the database.</param>
        /// <returns>A description of the marriage certificate in webtrees format.</returns>
        public string toWebtrees(Database database)
        {
            // Initialise the Html description.
            StringBuilder html = new StringBuilder();

            html.Append("&lt;a name=\"" + groomName.ToLower().Replace(' ', '_') + "_" + brideName.ToLower().Replace(' ', '_') + "\"&gt;&lt;/a&gt;<br/>");
            html.Append("&lt;h2&gt;" + when.Year.ToString() + " " + groomName + " &amp; " + brideName + "&lt;/h2&gt;<br/>");
            html.Append("&lt;table class=\"marriage\"&gt;<br/>");
            html.Append("&lt;tr&gt;&lt;td colspan=\"7\"&gt;&lt;span class=\"data\"&gt;" + when.Year.ToString() + "&lt;/span&gt; Marriage solemnized at &lt;span class=\"data\"&gt;");
            html.Append(location + "&lt;/span&gt;&lt;/td&gt;&lt;/tr&gt;<br/>");
            html.Append("&lt;tr&gt;&lt;td&gt;When Married&lt;/td&gt;&lt;td&gt;Name&lt;/td&gt;&lt;td&gt;Age&lt;/td&gt;&lt;td>Rank or Profession&lt;/td&gt;&lt;td&gt;Residence at the time of marriage&lt;/td&gt;&lt;td&gt;Father's Name&lt;/td&gt;&lt;td&gt;Rank of Profession of Father&lt;/td&gt;&lt;/tr&gt;<br/>");
            html.Append("&lt;tr&gt;");
            html.Append("&lt;td class=\"data\" rowspan=\"2\"&gt;" + when.ToString("d MMM yyyy") + "&lt;/td&gt;");
            html.Append("&lt;td class=\"data\"&gt;" + groomName + "&lt;/td&gt;");
            html.Append("&lt;td class=\"data\"&gt;" + groomAge + "&lt;/td&gt;");
            html.Append("&lt;td class=\"data\"&gt;" + groomOccupation + "&lt;/td&gt;");
            html.Append("&lt;td class=\"data\"&gt;" + groomLiving + "&lt;/td&gt;");
            html.Append("&lt;td class=\"data\"&gt;" + groomFather + "&lt;/td&gt;");
            html.Append("&lt;td class=\"data\"&gt;" + groomFatherOccupation + "&lt;/td&gt;");
            html.Append("&lt;/tr&gt;<br/>");
            html.Append("&lt;tr&gt;");
            html.Append("&lt;td class=\"data\"&gt;" + brideName + "&lt;/td&gt;");
            html.Append("&lt;td class=\"data\"&gt;" + brideAge + "&lt;/td&gt;");
            html.Append("&lt;td class=\"data\"&gt;" + brideOccupation + "&lt;/td&gt;");
            html.Append("&lt;td class=\"data\"&gt;" + brideLiving + "&lt;/td&gt;");
            html.Append("&lt;td class=\"data\"&gt;" + brideFather + "&lt;/td&gt;");
            html.Append("&lt;td class=\"data\"&gt;" + brideFatherOccupation + "&lt;/td&gt;");
            html.Append("&lt;/tr&gt;<br/>");
            html.Append("&lt;tr&gt;&lt;td colspan=\"7\"&gt;in the Presence of us, &lt;span class=\"data\"&gt;");
            html.Append(witness + "&lt;/span&gt;&lt;/td&gt;&lt;/tr&gt;<br/>");
            html.Append("&lt;tr&gt;&lt;td style=\"text-align:center\" colspan=\"7\"&gt;GRO Reference &lt;span class=\"data\"&gt;");
            html.Append(groReference + "&lt;/span&gt;&lt;/td&gt;&lt;/tr&gt;<br/>");
            html.Append("&lt;/table&gt;<br/>");
            html.Append("&lt;table class=\"meta\"&gt;<br/>");
            html.Append("&lt;td class=\"label\"&gt;filename&lt;/td&gt;&lt;td class=\"value\"&gt;marriage_" + when.Year.ToString() + "_" + groomName.ToLower().Replace(' ', '_') + ".png&lt;/td&gt;&lt;/tr&gt;<br/>");
            html.Append("&lt;td class=\"label\"&gt;Source Text&lt;/td&gt;&lt;td class=\"value\"&gt;On " + when.ToString("d MMM yyyy") + " at " + location + ". " + groomName + " (" + groomAge + "), " + groomOccupation + " of " + groomLiving + ", son of " + groomFather + ", " + groomFatherOccupation + " married " + brideName + " (" + brideAge + "), " + brideOccupation + " of " + brideLiving + ", daughter of " + brideFather + ", " + brideFatherOccupation + ".  In the presence of " + witness + ".&lt;/td&gt;&lt;/tr&gt;<br/>");
            html.Append("&lt;td class=\"label\"&gt;Source Note&lt;/td&gt;&lt;td class=\"value\"&gt;GRO Reference " + groReference + ".&lt;/td&gt;&lt;/tr&gt;<br/>");
            html.Append("&lt;td class=\"label\"&gt;Citation Text&lt;/td&gt;&lt;td class=\"value\"&gt;" + groomName + " married " + brideName + ".  " + groomFather + " was father of the groom.  " + brideFather + " was father of the bride.  " + witness + " was a witness.&lt;/td&gt;&lt;/tr&gt;<br/>");
            html.Append("&lt;/table&gt;");
            // Return the Html description
            return html.ToString();
        }



        /// <summary>The ID of the marraige record.  This should match with the ID the parent source.</summary>
        public int idx { get { return idx_; } set { idx_ = value; } }

    }
}
