using System;
using System.Data.OleDb;

// StringBuilder
using System.Text;

namespace FamilyTree.Objects
{
    /// <summary>Class to represent the additional information on a birth certificate source.  This is closely related to the tbl_BirthCertificates table.</summary>
    public class clsBirthCertificate
	{
		#region Member Variables

		/// <summary>The ID of the birth certificate record.  This should match with the ID the parent source.</summary>
		private int index_;

		/// <summary>The registration district as specified on the birth certificate.</summary>
		public string registrationDistrict;

        /// <summary>The when field as specified on the birth certificate.</summary>
        public DateTime when;
        
        /// <summary>The when and where field as specified on the birth certificate.</summary>
        public string whenAndWhere;

		/// <summary>The name as specified on the birth certificate.</summary>
		public string name;

		/// <summary>The sex as specified on the birth certificate.</summary>
		public string sex;

		/// <summary>The name of the father as specified on the birth certificate.</summary>
		public string father;

        /// <summary>The name of the mother as specified on the birth certificate.</summary>
        public string mother;

        /// <summary>The name of the mother as specified on the birth certificate.</summary>
        public string motherDetails;

		/// <summary>The occupation of the father as specified on the birth certificate.</summary>
		public string fatherOccupation;

        /// <summary>The informant as specified on the birth certificate.</summary>
        public string informant;

        /// <summary>The informant as specified on the birth certificate.</summary>
        public string informantAddress;

		/// <summary>The when registered field as specified on the birth certificate.</summary>
		public string whenRegistered;

        /// <summary>The GRO reference for the certificate.</summary>
        public string groReference;

		#endregion

        #region Constructors etc ...



        /// <summary>Class constructor.</summary>
		/// <param name="index">Specifies the ID of the parent source record.</param>
        public clsBirthCertificate(int index)
        {
            index_ = index;
        }



        /// <summary>Class constructor that loads the current values from the specified database.</summary>
		/// <param name="index">Specifies the ID of the parent source record.</param>
		/// <param name="cndb">Specifies the database connection to load the information from.</param>
        public clsBirthCertificate(int index, OleDbConnection cndb)
            : this(index)
        {
            string sql = "SELECT * FROM tbl_BirthCertificates WHERE ID=" + index_.ToString() + ";";
            OleDbCommand sqlCommand = new OleDbCommand(sql, cndb);
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            if(dataReader.Read())
            {
                registrationDistrict = Database.getString(dataReader, "RegistrationDistrict", "");
                when = walton.Database.getDateTime(dataReader, "WhenBorn", DateTime.Now);
                whenAndWhere = Database.getString(dataReader, "WhenAndWhere", "");
                name = Database.getString(dataReader, "Name", "");
                sex = Database.getString(dataReader, "Sex", "");
                father = Database.getString(dataReader, "Father", "");
                mother = Database.getString(dataReader, "Mother", "");
                motherDetails = Database.getString(dataReader, "MotherDetails", "");
                fatherOccupation = Database.getString(dataReader, "FatherOccupation", "");
                informant = Database.getString(dataReader, "Informant", "");
                informantAddress = Database.getString(dataReader, "InformantAddress", "");
                whenRegistered = Database.getString(dataReader, "WhenRegistered", "");
                groReference = Database.getString(dataReader, "GroReference", "");
            }
            dataReader.Close();
        }

        #endregion

        /// <summary>Return the birth certificate information in Html format.</summary>
        /// <returns>A description of the birth certificate in Html format.</returns>
        public string toHtml()
        {
            // Initialise the Html description
            StringBuilder html = new StringBuilder();

            html.Append("<table align=\"center\" bgcolor=\"mistyrose\" border=\"0\" cellpadding=\"5\" cellspacing=\"0\">");
            html.Append("<tr><td colspan=\"8\">" + when.Year.ToString() + " <span class=\"Birth\">Birth in the registration district of</span> " + registrationDistrict + "</td></tr>");
            html.Append("<tr valign=\"bottom\"><td><span class=\"Birth\">When and<br/>Where Born</span></td>");
            html.Append("<td class=\"Birth\">Name</td>");
            html.Append("<TD><SPAN class=\"Birth\">Sex</SPAN></TD>");
            html.Append("<TD><SPAN class=\"Birth\">Father</SPAN></TD>");
            html.Append("<TD><SPAN class=\"Birth\">Mother</SPAN></TD>");
            html.Append("<TD><SPAN class=\"Birth\">Occupation<BR>of Father</SPAN></TD>");
            html.Append("<TD><SPAN class=\"Birth\">Informant</SPAN></TD>");
            html.Append("<TD><SPAN class=\"Birth\">When Registered</SPAN></TD></TR>");

            html.Append("<TR valign=top><TD>" + when.ToString("d MMM yyyy") + "<BR>" + whenAndWhere + "</TD>");
            html.Append("<TD>" + name + "</TD>");
            html.Append("<TD>" + sex + "</TD>");
            html.Append("<TD>" + father + "</TD>");
            html.Append("<TD>" + mother + "<BR>" + motherDetails + "</TD>");
            html.Append("<TD>" + fatherOccupation + "</TD>");
            html.Append("<TD>" + informant + "<BR>" + informantAddress + "</TD>");
            html.Append("<TD>" + whenRegistered + "</TD></TR>");
            html.Append("<TR><TD colspan=8 align=center><SPAN class=\"Birth\">GRO Reference</SPAN> " + groReference + "</TD></TR>");
            html.Append("</table>");

            // Return the Html description.
            return html.ToString();
        }



        private string hisHer(string sex)
        {
            if(sex.ToLower() =="girl")
            {
                return "Her";
            }
            return "His";
        }
        


        /// <summary>Return the birth certificate information in format for a webtrees birth certificate.</summary>
        /// <returns>The html to build a webtrees birth certificate.</returns>
        public string toWebtrees()
        {
            // Initialise the Html description.
            StringBuilder html = new StringBuilder();

            html.Append("&lt;a name=\""+name.ToLower().Replace(' ', '_')+"_"+when.Year.ToString()+"\"&gt;&lt;/a&gt;<br/>");
            html.Append("&lt;h2&gt;"+name+"&lt;/h2&gt;<br/>");

            html.Append("&lt;table class=\"birth\"&gt;<br/>");
            html.Append("&lt;tr&gt;&lt;td colspan=\"8\"&gt;&lt;span class=\"data\"&gt;" + when.Year.ToString() + "&lt;/span&gt; Birth in the registration district of &lt;span class=\"data\"&gt;" + registrationDistrict + "&lt;/span&gt;&lt;/td&gt;&lt;/tr&gt;<br/>");
            html.Append("&lt;tr style=\"vertical-align:bottom;\"&gt;&lt;td&gt;When and&lt;br/&gt;Where Born&lt;/td&gt;&lt;td&gt;Name&lt;/td&gt;&lt;td&gt;Sex&lt;/td&gt;&lt;td&gt;Father&lt;/td&gt;&lt;td>Mother&lt;/td&gt;&lt;td&gt;Occupation&lt;br/&gt;of Father&lt;/td&gt;&lt;td&gt;Informant&lt;/td&gt;&lt;td&gt;When Registered&lt;/td&gt;&lt;/tr&gt;<br/>");

            html.Append("&lt;tr style=\"vertical-align:top;\"&gt;&lt;td class=\"data\"&gt;" + when.ToString("d MMM yyyy") + "&lt;br/&gt;" + whenAndWhere + "&lt;/td&gt;");
            html.Append("&lt;td class=\"data\"&gt;" + name + "&lt;/td&gt;");
            html.Append("&lt;td class=\"data\"&gt;" + sex + "&lt;/td&gt;");
            html.Append("&lt;td class=\"data\"&gt;" + father + "&lt;/td&gt;");
            html.Append("&lt;td class=\"data\"&gt;" + mother + "&lt;br/&gt;" + motherDetails + "&lt;/td&gt;");
            html.Append("&lt;td class=\"data\"&gt;" + fatherOccupation + "&lt;/td&gt;");
            html.Append("&lt;td class=\"data\"&gt;" + informant + "&lt;br/&gt;" + informantAddress + "&lt;/td&gt;");
            html.Append("&lt;td class=\"data\"&gt;" + whenRegistered + "&lt;/td&gt;&lt;/tr&gt;<br/>");
            html.Append("&lt;tr&gt;&lt;td style=\"text-align:center;\" colspan=\"8\"&gt;GRO Reference &lt;span class=\"data\"&gt;" + groReference + "&lt;/span&gt;&lt;/td&gt;&lt;/tr&gt;<br/>");
            html.Append("&lt;/table&gt;<br/>");
            html.Append("&lt;table class=\"meta\"&gt;<br/>");
            html.Append("&lt;tr&gt;&lt;td class=\"label\"&gt;filename&lt;/td&gt;&lt;td class=\"value\"&gt;birth_"+when.Year.ToString()+"_"+name.ToLower().Replace(' ', '_')+".png&lt;/td&gt;&lt;/tr&gt;<br/>");
            html.Append("&lt;tr&gt;&lt;td class=\"label\"&gt;Media Title&lt;/td&gt;&lt;td class=\"value\"&gt;"+name+" Birth Certificate "+when.Year.ToString()+"&lt;/td&gt;&lt;/tr&gt;<br/>");
            html.Append("&lt;tr&gt;&lt;td class=\"label\"&gt;Source Title&lt;/td&gt;&lt;td class=\"value\"&gt;Birth Certificate: "+name+" "+when.Year.ToString()+"&lt;/td&gt;&lt;/tr&gt;<br/>");
            html.Append("&lt;tr&gt;&lt;td class=\"label\"&gt;Source Text&lt;/td&gt;&lt;td class=\"value\"&gt;On "+when.ToString("d MMM yyyy")+" at "+whenAndWhere+", "+name+" ("+sex+") was born.  "+hisHer(sex)+" father was "+father+", "+fatherOccupation+".  "+hisHer(sex)+" mother was "+ mother +" "+motherDetails +".  Registered on "+whenRegistered+" by "+informant +" of "+informantAddress +".&lt;/td&gt;&lt;/tr&gt;<br/>");
            html.Append("&lt;tr&gt;&lt;td class=\"label\"&gt;Source Note&lt;/td&gt;&lt;td class=\"value\"&gt;GRO Reference "+groReference +".&lt;/td&gt;&lt;/tr&gt;<br/>");
            html.Append("&lt;tr&gt;&lt;td class=\"label\"&gt;Citation Text&lt;/td&gt;&lt;td class=\"value\"&gt;"+name+"'s own birth certificate.  "+father+" was the father of "+name+".  "+mother+" was the mother of "+name+".&lt;/td&gt;&lt;/tr&gt;<br/>");
            html.Append("&lt;/table&gt;<br/>");

            // Return the Html description.
            return html.ToString();

        }



        /// <summary>Writes the birth certificate record into the specified database.</summary>
		/// <param name="database">Specifies the database to write the birth certificate record into.</param>
		/// <returns>True for success, false otherwise.</returns>
        public bool save(Database database)
        {
            // Validate the ID.
            if(index_ == 0)
            {
                return false;
            }

            // Write the record into the database
            string sql = "UPDATE tbl_BirthCertificates SET "
                + "RegistrationDistrict = " + Database.toDb(registrationDistrict)
                + ", WhenBorn = " + walton.Database.toDb(when, walton.Database.enumDatabases.Access)
                + ", WhenAndWhere = " + Database.toDb(whenAndWhere)
                + ", Name = " + Database.toDb(name)
                + ", Sex = " + Database.toDb(sex)
                + ", Father = " + Database.toDb(father)
                + ", Mother = " + Database.toDb(mother)
                + ", MotherDetails = " + Database.toDb(motherDetails)
                + ", FatherOccupation = " + Database.toDb(fatherOccupation)
                + ", Informant = " + Database.toDb(informant)
                + ", InformantAddress = " + Database.toDb(informantAddress)
                + ", WhenRegistered = " + Database.toDb(whenRegistered)
                + ", GroReference = " + Database.toDb(groReference)
                + " WHERE ID = " + index_.ToString() + ";";
            OleDbCommand sqlCommand = new OleDbCommand(sql, database.cndb);
            int numRows = sqlCommand.ExecuteNonQuery();
            if(numRows == 0)
            {
                sql = "INSERT INTO tbl_BirthCertificates (ID, RegistrationDistrict, WhenBorn, WhenAndWhere, Name, Sex, Father, Mother, FatherOccupation, Informant, InformantAddress, WhenRegistered) VALUES ("
                    + index_.ToString()
                    + ", " + Database.toDb(registrationDistrict)
                    + ", " + walton.Database.toDb(when, walton.Database.enumDatabases.Access)
                    + ", " + Database.toDb(whenAndWhere)
                    + ", " + Database.toDb(name)
                    + ", " + Database.toDb(sex)
                    + ", " + Database.toDb(father)
                    + ", " + Database.toDb(mother)
                    + ", " + Database.toDb(fatherOccupation)
                    + ", " + Database.toDb(informant)
                    + ", " + Database.toDb(informantAddress)
                    + ", " + Database.toDb(whenRegistered)
                    + ");";
                sqlCommand = new OleDbCommand(sql, database.cndb);
                sqlCommand.ExecuteNonQuery();
            }

            // Return success.
            return true;
        }

        /// <summary>The ID of the birth certificate record.  This should match with the ID the parent source.</summary>
        public int index { get { return index_; } set { index_ = value; } }
	}
}
