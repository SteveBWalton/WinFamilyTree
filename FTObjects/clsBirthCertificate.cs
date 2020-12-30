using System;
using System.Data;
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
		private int m_nID;

		/// <summary>The registration district as specified on the birth certificate.</summary>
		public string RegistrationDistrict;

        /// <summary>The when field as specified on the birth certificate.</summary>
        public DateTime When;
        
        /// <summary>The when and where field as specified on the birth certificate.</summary>
        public string WhenAndWhere;

		/// <summary>The name as specified on the birth certificate.</summary>
		public string Name;

		/// <summary>The sex as specified on the birth certificate.</summary>
		public string Sex;

		/// <summary>The name of the father as specified on the birth certificate.</summary>
		public string Father;

        /// <summary>The name of the mother as specified on the birth certificate.</summary>
        public string Mother;

        /// <summary>The name of the mother as specified on the birth certificate.</summary>
        public string MotherDetails;

		/// <summary>The occupation of the father as specified on the birth certificate.</summary>
		public string FatherOccupation;

        /// <summary>The informant as specified on the birth certificate.</summary>
        public string Informant;

        /// <summary>The informant as specified on the birth certificate.</summary>
        public string InformantAddress;

		/// <summary>The when registered field as specified on the birth certificate.</summary>
		public string WhenRegistered;

        /// <summary>The GRO reference for the certificate.</summary>
        public string GroReference;

		#endregion

        #region Constructors etc ...

        // Class constructor.
        /// <summary>
        /// Class constructor.
        /// </summary>
		/// <param name="nID">Specifies the ID of the parent source record.</param>
        public clsBirthCertificate(int nID)
        {
            m_nID = nID;
        }
        // Class constructor that loads the current values from the specified database.
        /// <summary>
        /// Class constructor that loads the current values from the specified database.
        /// </summary>
		/// <param name="nID">Specifies the ID of the parent source record.</param>
		/// <param name="cnDb">Specifies the database connection to load the information from.</param>
        public clsBirthCertificate(int nID, OleDbConnection cnDb)
            : this(nID)
        {
            string sSql = "SELECT * FROM tbl_BirthCertificates WHERE ID=" + m_nID.ToString() + ";";
            OleDbCommand oSql = new OleDbCommand(sSql, cnDb);
            OleDbDataReader drBirth = oSql.ExecuteReader();
            if(drBirth.Read())
            {
                RegistrationDistrict = Database.GetString(drBirth, "RegistrationDistrict", "");
                When = Innoval.clsDatabase.GetDateTime(drBirth, "WhenBorn", DateTime.Now);
                WhenAndWhere = Database.GetString(drBirth, "WhenAndWhere", "");
                Name = Database.GetString(drBirth, "Name", "");
                Sex = Database.GetString(drBirth, "Sex", "");
                Father = Database.GetString(drBirth, "Father", "");
                Mother = Database.GetString(drBirth, "Mother", "");
                MotherDetails = Database.GetString(drBirth, "MotherDetails", "");
                FatherOccupation = Database.GetString(drBirth, "FatherOccupation", "");
                Informant = Database.GetString(drBirth, "Informant", "");
                InformantAddress = Database.GetString(drBirth, "InformantAddress", "");
                WhenRegistered = Database.GetString(drBirth, "WhenRegistered", "");
                GroReference = Database.GetString(drBirth, "GroReference", "");
            }
            drBirth.Close();
        }

        #endregion

        /// <summary>Return the birth certificate information in Html format.</summary>
        /// <returns>A description of the birth certificate in Html format.</returns>
        public string ToHtml()
        {
            // Initialise the Html description
            StringBuilder sbHtml = new StringBuilder();

            sbHtml.Append("<table align=\"center\" bgcolor=\"mistyrose\" border=\"0\" cellpadding=\"5\" cellspacing=\"0\">");
            sbHtml.Append("<tr><td colspan=\"8\">" + When.Year.ToString() + " <span class=\"Birth\">Birth in the registration district of</span> " + RegistrationDistrict + "</td></tr>");
            sbHtml.Append("<tr valign=\"bottom\"><td><span class=\"Birth\">When and<br/>Where Born</span></td>");
            sbHtml.Append("<td class=\"Birth\">Name</td>");
            sbHtml.Append("<TD><SPAN class=\"Birth\">Sex</SPAN></TD>");
            sbHtml.Append("<TD><SPAN class=\"Birth\">Father</SPAN></TD>");
            sbHtml.Append("<TD><SPAN class=\"Birth\">Mother</SPAN></TD>");
            sbHtml.Append("<TD><SPAN class=\"Birth\">Occupation<BR>of Father</SPAN></TD>");
            sbHtml.Append("<TD><SPAN class=\"Birth\">Informant</SPAN></TD>");
            sbHtml.Append("<TD><SPAN class=\"Birth\">When Registered</SPAN></TD></TR>");

            sbHtml.Append("<TR valign=top><TD>" + When.ToString("d MMM yyyy") + "<BR>" + WhenAndWhere + "</TD>");
            sbHtml.Append("<TD>" + Name + "</TD>");
            sbHtml.Append("<TD>" + Sex + "</TD>");
            sbHtml.Append("<TD>" + Father + "</TD>");
            sbHtml.Append("<TD>" + Mother + "<BR>" + MotherDetails + "</TD>");
            sbHtml.Append("<TD>" + FatherOccupation + "</TD>");
            sbHtml.Append("<TD>" + Informant + "<BR>" + InformantAddress + "</TD>");
            sbHtml.Append("<TD>" + WhenRegistered + "</TD></TR>");
            sbHtml.Append("<TR><TD colspan=8 align=center><SPAN class=\"Birth\">GRO Reference</SPAN> " + GroReference + "</TD></TR>");
            sbHtml.Append("</table>");

            // Return the Html description
            return sbHtml.ToString();
        }

        private string HisHer(string Sex)
        {
            if(Sex.ToLower() =="girl")
            {
                return "Her";
            }
            return "His";
        }
        
        /// <summary>Return the birth certificate information in format for a webtrees birth certificate.</summary>
        /// <returns>The html to build a webtrees birth certificate.</returns>
        public string ToWebtrees()
        {
            // Initialise the Html description.
            StringBuilder sbHtml = new StringBuilder();

            sbHtml.Append("&lt;a name=\""+Name.ToLower().Replace(' ', '_')+"_"+When.Year.ToString()+"\"&gt;&lt;/a&gt;<br/>");
            sbHtml.Append("&lt;h2&gt;"+Name+"&lt;/h2&gt;<br/>");

            sbHtml.Append("&lt;table class=\"birth\"&gt;<br/>");
            sbHtml.Append("&lt;tr&gt;&lt;td colspan=\"8\"&gt;&lt;span class=\"data\"&gt;" + When.Year.ToString() + "&lt;/span&gt; Birth in the registration district of &lt;span class=\"data\"&gt;" + RegistrationDistrict + "&lt;/span&gt;&lt;/td&gt;&lt;/tr&gt;<br/>");
            sbHtml.Append("&lt;tr style=\"vertical-align:bottom;\"&gt;&lt;td&gt;When and&lt;br/&gt;Where Born&lt;/td&gt;&lt;td&gt;Name&lt;/td&gt;&lt;td&gt;Sex&lt;/td&gt;&lt;td&gt;Father&lt;/td&gt;&lt;td>Mother&lt;/td&gt;&lt;td&gt;Occupation&lt;br/&gt;of Father&lt;/td&gt;&lt;td&gt;Informant&lt;/td&gt;&lt;td&gt;When Registered&lt;/td&gt;&lt;/tr&gt;<br/>");

            sbHtml.Append("&lt;tr style=\"vertical-align:top;\"&gt;&lt;td class=\"data\"&gt;" + When.ToString("d MMM yyyy") + "&lt;br/&gt;" + WhenAndWhere + "&lt;/td&gt;");
            sbHtml.Append("&lt;td class=\"data\"&gt;" + Name + "&lt;/td&gt;");
            sbHtml.Append("&lt;td class=\"data\"&gt;" + Sex + "&lt;/td&gt;");
            sbHtml.Append("&lt;td class=\"data\"&gt;" + Father + "&lt;/td&gt;");
            sbHtml.Append("&lt;td class=\"data\"&gt;" + Mother + "&lt;br/&gt;" + MotherDetails + "&lt;/td&gt;");
            sbHtml.Append("&lt;td class=\"data\"&gt;" + FatherOccupation + "&lt;/td&gt;");
            sbHtml.Append("&lt;td class=\"data\"&gt;" + Informant + "&lt;br/&gt;" + InformantAddress + "&lt;/td&gt;");
            sbHtml.Append("&lt;td class=\"data\"&gt;" + WhenRegistered + "&lt;/td&gt;&lt;/tr&gt;<br/>");
            sbHtml.Append("&lt;tr&gt;&lt;td style=\"text-align:center;\" colspan=\"8\"&gt;GRO Reference &lt;span class=\"data\"&gt;" + GroReference + "&lt;/span&gt;&lt;/td&gt;&lt;/tr&gt;<br/>");
            sbHtml.Append("&lt;/table&gt;<br/>");
            sbHtml.Append("&lt;table class=\"meta\"&gt;<br/>");
            sbHtml.Append("&lt;tr&gt;&lt;td class=\"label\"&gt;filename&lt;/td&gt;&lt;td class=\"value\"&gt;birth_"+When.Year.ToString()+"_"+Name.ToLower().Replace(' ', '_')+".png&lt;/td&gt;&lt;/tr&gt;<br/>");
            sbHtml.Append("&lt;tr&gt;&lt;td class=\"label\"&gt;Media Title&lt;/td&gt;&lt;td class=\"value\"&gt;"+Name+" Birth Certificate "+When.Year.ToString()+"&lt;/td&gt;&lt;/tr&gt;<br/>");
            sbHtml.Append("&lt;tr&gt;&lt;td class=\"label\"&gt;Source Title&lt;/td&gt;&lt;td class=\"value\"&gt;Birth Certificate: "+Name+" "+When.Year.ToString()+"&lt;/td&gt;&lt;/tr&gt;<br/>");
            sbHtml.Append("&lt;tr&gt;&lt;td class=\"label\"&gt;Source Text&lt;/td&gt;&lt;td class=\"value\"&gt;On "+When.ToString("d MMM yyyy")+" at "+WhenAndWhere+", "+Name+" ("+Sex+") was born.  "+HisHer(Sex)+" father was "+Father+", "+FatherOccupation+".  "+HisHer(Sex)+" mother was "+ Mother +" "+MotherDetails +".  Registered on "+WhenRegistered+" by "+Informant +" of "+InformantAddress +".&lt;/td&gt;&lt;/tr&gt;<br/>");
            sbHtml.Append("&lt;tr&gt;&lt;td class=\"label\"&gt;Source Note&lt;/td&gt;&lt;td class=\"value\"&gt;GRO Reference "+GroReference +".&lt;/td&gt;&lt;/tr&gt;<br/>");
            sbHtml.Append("&lt;tr&gt;&lt;td class=\"label\"&gt;Citation Text&lt;/td&gt;&lt;td class=\"value\"&gt;"+Name+"'s own birth certificate.  "+Father+" was the father of "+Name+".  "+Mother+" was the mother of "+Name+".&lt;/td&gt;&lt;/tr&gt;<br/>");
            sbHtml.Append("&lt;/table&gt;<br/>");

            // Return the Html description
            return sbHtml.ToString();

        }

        /// <summary>Writes the birth certificate record into the specified database.</summary>
		/// <param name="oDb">Specifies the database to write the birth certificate record into.</param>
		/// <returns>True for success, false otherwise.</returns>
        public bool Save(Database oDb)
        {
            // Validate the ID
            if(m_nID == 0)
            {
                return false;
            }

            // Write the record into the database
            string sSql = "UPDATE tbl_BirthCertificates SET "
                + "RegistrationDistrict=" + Database.ToDb(RegistrationDistrict)
                + ",WhenBorn=" + Innoval.clsDatabase.ToDb(When, Innoval.clsDatabase.enumDatabases.Access)
                + ",WhenAndWhere=" + Database.ToDb(WhenAndWhere)
                + ",Name=" + Database.ToDb(Name)
                + ",Sex=" + Database.ToDb(Sex)
                + ",Father=" + Database.ToDb(Father)
                + ",Mother=" + Database.ToDb(Mother)
                + ",MotherDetails=" + Database.ToDb(MotherDetails)
                + ",FatherOccupation=" + Database.ToDb(FatherOccupation)
                + ",Informant=" + Database.ToDb(Informant)
                + ",InformantAddress=" + Database.ToDb(InformantAddress)
                + ",WhenRegistered=" + Database.ToDb(WhenRegistered)
                + ",GroReference=" + Database.ToDb(GroReference)
                + " WHERE ID=" + m_nID.ToString() + ";";
            OleDbCommand oSql = new OleDbCommand(sSql, oDb.cndb);
            int nNumRows = oSql.ExecuteNonQuery();
            if(nNumRows == 0)
            {
                sSql = "INSERT INTO tbl_BirthCertificates (ID,RegistrationDistrict,WhenBorn,WhenAndWhere,Name,Sex,Father,Mother,FatherOccupation,Informant,InformantAddress,WhenRegistered) VALUES ("
                    + m_nID.ToString()
                    + "," + Database.ToDb(RegistrationDistrict)
                    + "," + Innoval.clsDatabase.ToDb(When, Innoval.clsDatabase.enumDatabases.Access)
                    + "," + Database.ToDb(WhenAndWhere)
                    + "," + Database.ToDb(Name)
                    + "," + Database.ToDb(Sex)
                    + "," + Database.ToDb(Father)
                    + "," + Database.ToDb(Mother)
                    + "," + Database.ToDb(FatherOccupation)
                    + "," + Database.ToDb(Informant)
                    + "," + Database.ToDb(InformantAddress)
                    + "," + Database.ToDb(WhenRegistered)
                    + ");";
                oSql = new OleDbCommand(sSql, oDb.cndb);
                oSql.ExecuteNonQuery();
            }

            // Return success
            return true;
        }

        /// <summary>The ID of the birth certificate record.  This should match with the ID the parent source.</summary>
        public int ID { get { return m_nID; } set { m_nID = value; } }
	}
}
