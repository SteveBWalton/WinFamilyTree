using System;
using System.Data;
using System.Data.OleDb;

// StringBuilder
using System.Text;

namespace FamilyTree.Objects
{
	/// <summary>Class to represent the additional information on a death certificate source.  This is closely related to the tbl_DeathCertificates table.</summary>
	public class clsDeathCertificate
	{
		#region Member Variables

		/// <summary>The ID of the death certificate record.  This should match with the ID the parent source.</summary>
		private int m_nID;

		/// <summary>The registration district as specified on the birth certificate.</summary>
		public string RegistrationDistrict;

		/// <summary>The when field as specified on the birth certificate.</summary>
		public string When;

        /// <summary>The where field modified into a database place record.</summary>
        public string Place;

		/// <summary>The name as specified on the birth certificate.</summary>
		public string Name;

		/// <summary>The sex as specified on the birth certificate.</summary>
		public string Sex;

		/// <summary>Date and place of birth.  Only on newer certificates.</summary>
		public string DatePlaceOfBirth;

		/// <summary>Occupation of the dead person.</summary>
		public string Occupation;

		/// <summary>Address of the dead person.</summary>
		public string UsualAddress;

		/// <summary>Cause of death of the dead person.</summary>
		public string CauseOfDeath;

		/// <summary>Name of the informant.</summary>
		public string Informant;

		/// <summary>Description of the informant.  Wife, Husband, son etc ... </summary>
		public string InformantDescription;

		/// <summary>Address of the informant.</summary>
		public string InformantAddress;

		/// <summary>When the death certificate was created.</summary>
		public string WhenRegistered;

        /// <summary>The GRO reference for the certificate.</summary>
        public string GroReference;

		#endregion

		/// <summary>Class constructor.</summary>
		/// <param name="nID">Specifies the ID of the source record.</param>
		public clsDeathCertificate			(			int nID			)
		{
			m_nID = nID;
		}

		/// <summary>Class constructor that loads the current values from the specified database.</summary>
		/// <param name="nID">Specifies the ID of the parent source record.</param>
		/// <param name="cnDb">Specifies the database connection to load the information from.</param>
		public clsDeathCertificate			(			int nID,			OleDbConnection cnDb			) : this(nID)
		{
			string sSql = "SELECT * FROM tbl_DeathCertificates WHERE ID=" + m_nID.ToString()+";";
			OleDbCommand oSql = new OleDbCommand(sSql,cnDb);
			OleDbDataReader drDeath = oSql.ExecuteReader();
			if(drDeath.Read())
			{
				RegistrationDistrict = clsDatabase.GetString(drDeath,"RegistrationDistrict","");
                When = clsDatabase.GetString(drDeath,"WhenWhere","");
                Place = clsDatabase.GetString(drDeath,"Place","");
                Name = clsDatabase.GetString(drDeath,"Name","");
				Sex = clsDatabase.GetString(drDeath,"Sex","");
				DatePlaceOfBirth = clsDatabase.GetString(drDeath,"DatePlaceOfBirth","");
				Occupation = clsDatabase.GetString(drDeath,"Occupation","");
				UsualAddress = clsDatabase.GetString(drDeath,"UsualAddress","");
				CauseOfDeath = clsDatabase.GetString(drDeath,"CauseOfDeath","");
				Informant = clsDatabase.GetString(drDeath,"Informant","");
				InformantDescription = clsDatabase.GetString(drDeath,"InformantDescription","");
				InformantAddress = clsDatabase.GetString(drDeath,"InformantAddress","");
				WhenRegistered = clsDatabase.GetString(drDeath,"WhenRegistered","");
                GroReference = clsDatabase.GetString(drDeath,"GroReference","");
			}
			drDeath.Close();
		}

		/// <summary>Writes the death certificate record into the specified database.</summary>
		/// <param name="oDb">Specifies the database to write the birth certificate record into.</param>
		/// <returns>True for success, false otherwise.</returns>
		public bool Save			(			clsDatabase oDb			)
		{
			// Validate the ID
			if(m_nID==0)
			{
				return false;
			}

			// Write the record into the database
            string sSql = "UPDATE tbl_DeathCertificates SET "
                + "RegistrationDistrict=" + clsDatabase.ToDb(RegistrationDistrict)
                + ",WhenWhere=" + clsDatabase.ToDb(When)
                + ",Place=" + clsDatabase.ToDb(Place)
                + ",Name=" + clsDatabase.ToDb(Name)
                + ",Sex=" + clsDatabase.ToDb(Sex)
                + ",DatePlaceOfBirth=" + clsDatabase.ToDb(DatePlaceOfBirth)
                + ",Occupation=" + clsDatabase.ToDb(Occupation)
                + ",UsualAddress=" + clsDatabase.ToDb(UsualAddress)
                + ",CauseOfDeath=" + clsDatabase.ToDb(CauseOfDeath)
                + ",Informant=" + clsDatabase.ToDb(Informant)
                + ",InformantDescription=" + clsDatabase.ToDb(InformantDescription)
                + ",InformantAddress=" + clsDatabase.ToDb(InformantAddress)
                + ",WhenRegistered=" + clsDatabase.ToDb(WhenRegistered)
                + ",GroReference=" + clsDatabase.ToDb(GroReference)
                + " WHERE ID=" + m_nID.ToString() + ";";
			OleDbCommand oSql = new OleDbCommand(sSql,oDb.cnDB);
			int nNumRows = oSql.ExecuteNonQuery();
			if(nNumRows==0)
			{
				sSql = "INSERT INTO tbl_DeathCertificates (ID,RegistrationDistrict,WhenWhere,Place,Name,Sex,DatePlaceOfBirth,Occupation,UsualAddress,CauseOfDeath,Informant,InformantDescription,InformantAddress,WhenRegistered) VALUES ("
					+m_nID.ToString()
					+","+clsDatabase.ToDb(RegistrationDistrict)
					+","+clsDatabase.ToDb(When)
					+","+clsDatabase.ToDb(Place)
					+","+clsDatabase.ToDb(Name)
					+","+clsDatabase.ToDb(Sex)
					+","+clsDatabase.ToDb(DatePlaceOfBirth)
					+","+clsDatabase.ToDb(Occupation)
					+","+clsDatabase.ToDb(UsualAddress)
					+","+clsDatabase.ToDb(CauseOfDeath)
					+","+clsDatabase.ToDb(Informant)
					+","+clsDatabase.ToDb(InformantDescription)
					+","+clsDatabase.ToDb(InformantAddress)
					+","+clsDatabase.ToDb(WhenRegistered)
					+");";
				oSql = new OleDbCommand(sSql,oDb.cnDB);
				oSql.ExecuteNonQuery();
			}

			// Return success
			return true;
		}

        /// <summary>Return the death certificate information in Html format.</summary>
        /// <returns>A description of the death certificate in Html format.</returns>
        public string ToHtml            (            clsDatabase oDb            )
        {
            // Initialise the Html description
            StringBuilder sbHtml = new StringBuilder();

            sbHtml.Append("<TABLE align=center bgcolor=thistle border=0 cellpadding=5 cellspacing=0>");
            sbHtml.Append("<TR><TD align=right><SPAN class=\"Death\">Registration District</SPAN></TD><TD colspan=3>"+ RegistrationDistrict + "</TD></TR>");
            sbHtml.Append("<TR><TD align=right><SPAN class=\"Death\">When and Where</SPAN></TD><TD colspan=3>"+ When + "</TD></TR>");
            sbHtml.Append("<TR><TD align=right><SPAN class=\"Death\">Name</SPAN></TD><TD>"+ Name + "</TD>");
            sbHtml.Append("<TD align=right><SPAN class=\"Death\">Sex</SPAN></TD><TD>"+ Sex + "</TD></TR>");
            sbHtml.Append("<TR><TD align=right><SPAN class=\"Death\">Date Place of Birth</SPAN></TD><TD colspan=3>" + DatePlaceOfBirth + "</TD></TR>");
            sbHtml.Append("<TR><TD align=right><SPAN class=\"Death\">Occupation</SPAN></TD><TD colspan=3>" + Occupation + "</TD></TR>");
            sbHtml.Append("<TR><TD align=right><SPAN class=\"Death\">Usual Address</SPAN></TD><TD colspan=3>" + UsualAddress + "</TD></TR>");
            sbHtml.Append("<TR><TD align=right><SPAN class=\"Death\">Cause of Death</SPAN></TD><TD colspan=3>"+ clsDatabase.HtmlString(CauseOfDeath) + "</TD></TR>");
            sbHtml.Append("<TR><TD align=right><SPAN class=\"Death\">Informant</SPAN></TD><TD>"+ Informant + "</TD>");
            sbHtml.Append("<TD align=right><SPAN class=\"Death\">Informant Description</SPAN></TD><TD>"+ InformantDescription + "</TD></TR>");
            sbHtml.Append("<TR><TD align=right><SPAN class=\"Death\">Informant Address</SPAN></TD><TD colspan=3>"+ InformantAddress + "</TD></TR>");
            sbHtml.Append("<TR><TD align=right><SPAN class=\"Death\">When Registered</SPAN></TD><TD>" + WhenRegistered + "</TD>");
            sbHtml.Append("<TD align=right><SPAN class=\"Death\">Reference</SPAN></TD><TD>" + GroReference + "</TD></TR>");
            sbHtml.Append("</TABLE>");

            // Return the Html description
            return sbHtml.ToString();
        }

        /// <summary>Returns the death certificate information format for a webtree's certificate..</summary>
        /// <returns>A description of the death certificate in webtree's format.</returns>
        public string ToWebtrees(clsDatabase oDb)
        {
            // Initialise the Html description
            StringBuilder sbHtml = new StringBuilder();

            sbHtml.Append("&lt;a name=\""+Name.ToLower().Replace(' ', '_')+"_"+When.Substring(When.Length-4)+"\"&gt;<br/>");
            sbHtml.Append("&lt;h2&gt;"+Name+"&lt;/h2&gt;<br/>");
            sbHtml.Append("&lt;table class=\"death\"&gt;<br/>");
            sbHtml.Append("&lt;tr&gt;&lt;td&gt;Registration District&lt;/td&gt;&lt;<TD colspan=3>"+ RegistrationDistrict + "&lt;/td&gt;&lt;/tr&gt;&lt;<br/>");
            sbHtml.Append("&lt;tr&gt;&lt;td&gt;When and Where</SPAN>&lt;/td&gt;&lt;<TD colspan=3>"+ When + "&lt;/td&gt;&lt;/tr&gt;&lt;<br/>");
            sbHtml.Append("&lt;tr&gt;&lt;td&gt;Name&lt;/td&gt;&lt;td&gt;"+ Name + "&lt;/td&gt;");
            sbHtml.Append("&lt;td&gt;Sex&lt;/td&gt;&lt;td&gt;"+ Sex + "&lt;/td&gt;&lt;/tr&gt;&lt;<br/>");
            sbHtml.Append("&lt;tr&gt;&lt;td&gt;Date Place of Birth&lt;/td&gt;&lt;td colspan=3>" + DatePlaceOfBirth + "&lt;/td&gt;&lt;/tr&gt;&lt;<br/>");
            sbHtml.Append("&lt;tr&gt;&lt;td&gt;Occupation&lt;/td&gt;&lt;td colspan=3>" + Occupation + "&lt;/td&gt;&lt;/tr&gt;&lt;<br/>");
            sbHtml.Append("&lt;tr&gt;&lt;td&gt;Usual Address&lt;/td&gt;&lt;td colspan=\"3\"&gt;" + UsualAddress + "&lt;/td&gt;&lt;/tr&gt;&lt;<br/>");
            sbHtml.Append("&lt;tr&gt;&lt;td&gt;Cause of Death&lt;/td&gt;&lt;td colspan=\"3\"&gt;"+ clsDatabase.HtmlString(CauseOfDeath) + "&lt;/td&gt;&lt;/tr&gt;<br/>");
            sbHtml.Append("&lt;tr&gt;&lt;td&gt;Informant&lt;/td&gt;&lt;TD>"+ Informant + "&lt;/td&gt;");
            sbHtml.Append("<TD align=right>Informant Description&lt;/td&gt;&lt;td&gt;"+ InformantDescription + "&lt;/td&gt;&lt;/tr&gt;<br/>");
            sbHtml.Append("&lt;tr&gt;&lt;td&gt;Informant Address&lt;/td&gt;&lt;td class=\"data\" colspan=\"3\"&gt;"+ InformantAddress + "&lt;/td&gt;&lt;/tr&gt;<br/>");
            sbHtml.Append("&lt;tr&gt;&lt;td&gt;When Registered&lt;/td&gt;&lt;td&gt;" + WhenRegistered + "&lt;/td&gt;");
            sbHtml.Append("&lt;td&gt;Reference&lt;td&gt;&lt;td class=\"data\"&gt;" + GroReference + "&lt;/td&gt;&lt;/tr&gt;<br/>");            
            sbHtml.Append("&lt;/table class=\"death\"&gt;<br/>");

            // Return the Html description
            return sbHtml.ToString();
        }


		/// <summary>The ID of the birth certificate record.  This should match with the ID the parent source.</summary>
		public int ID { get { return m_nID; } set { m_nID = value; } }

	}
}
