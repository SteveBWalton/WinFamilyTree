using System;
using System.Data;
using System.Data.OleDb;

// StringBuilder
using System.Text;

namespace FamilyTree.Objects
{
	/// <summary>
	/// Class to represent the additional information on a marriage certificate source.
	/// This is closely related to the tbl_MarriageCertificates table.
	/// </summary>
	public class clsMarriageCertificate
    {
        #region Member Variables

        /// <summary>The ID of the marriage certificate record.  This should match with the ID the parent source.</summary>
		private int m_nID;

        /// <summary>The date of the marriage.</summary>
        public DateTime When;

		/// <summary>The location as specified on the marriage certificate.</summary>
		public string Location;

		/// <summary>The name of the groom as specified on the marriage certificate.</summary>
		public string GroomName;

		/// <summary>The age of the groom as specified on the marriage certificate.</summary>
		public string GroomAge;

		/// <summary>The occupation of the groom as specified on the marriage certificate.</summary>
		public string GroomOccupation;

		/// <summary>The location where the groom was living as specified on the marriage certificate.</summary>
		public string GroomLiving;

		/// <summary>The name of the groom's father as specified on the marriage certificate.</summary>
		public string GroomFather;

		/// <summary>The occupation of the groom's father as specified on the marriage certificate.</summary>
		public string GroomFatherOccupation;

		/// <summary>The name of the bride as specified on the marriage certificate.</summary>
		public string BrideName;

		/// <summary>The age of the bride as specified on the marriage certificate.</summary>
		public string BrideAge;

		/// <summary>The occupation of the bride as specified on the marriage certificate.</summary>
		public string BrideOccupation;

		/// <summary>The location where the bride was living a specified on the marriage certificate.</summary>
		public string BrideLiving;

		/// <summary>The name of the bride's father as specified on the marriage certificate.</summary>
		public string BrideFather;

		/// <summary>The occupation of the bride's father as specified on the marriage certificate.</summary>
		public string BrideFatherOccupation;

		/// <summary>The names of the witnesses as specified on the marriage certificate.</summary>
		public string Witness;

        /// <summary>The GRO reference for the certificate.</summary>
        public string GroReference;

        #endregion

        #region Constructors etc ...

        /// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="nSourceID">Specifies the ID of the parent source record and the ID of this object.</param>
		public clsMarriageCertificate
			(
			int nSourceID
			)
		{            
            m_nID = nSourceID;
		}
		/// <summary>
		/// Class constructor that loads the current values from the database.
		/// </summary>
		/// <param name="nSourceID">Specifies the ID of the parent source record and the ID of this object.</param>
		/// <param name="cnDb">Specifies the database to load the values from.</param>
		public clsMarriageCertificate
			(
			int nSourceID,
			OleDbConnection cnDb			
			) : this (nSourceID)
		{            
			string sSql = "SELECT * FROM tbl_MarriageCertificates WHERE ID=" + m_nID.ToString()+";";
			OleDbCommand oSql = new OleDbCommand(sSql,cnDb);
			OleDbDataReader drMarriage = oSql.ExecuteReader();
			if(drMarriage.Read())
			{
                When = Innoval.clsDatabase.GetDateTime(drMarriage,"WhenMarried",DateTime.Now);
				Location = clsDatabase.GetString(drMarriage,"Location","");
				GroomName = clsDatabase.GetString(drMarriage,"GroomName","");
				GroomAge = clsDatabase.GetString(drMarriage,"GroomAge","");
				GroomOccupation = clsDatabase.GetString(drMarriage,"GroomOccupation","");
				GroomLiving = clsDatabase.GetString(drMarriage,"GroomLiving","");
				GroomFather = clsDatabase.GetString(drMarriage,"GroomFather","");
				GroomFatherOccupation = clsDatabase.GetString(drMarriage,"GroomFatherOccupation","");
				BrideName = clsDatabase.GetString(drMarriage,"BrideName","");
				BrideAge = clsDatabase.GetString(drMarriage,"BrideAge","");
				BrideOccupation = clsDatabase.GetString(drMarriage,"BrideOccupation","");
				BrideLiving = clsDatabase.GetString(drMarriage,"BrideLiving","");
				BrideFather = clsDatabase.GetString(drMarriage,"BrideFather","");
				BrideFatherOccupation = clsDatabase.GetString(drMarriage,"BrideFatherOccupation","");
				Witness = clsDatabase.GetString(drMarriage,"Witness","");
                GroReference = clsDatabase.GetString(drMarriage,"GroReference","");
			}
			drMarriage.Close();
        }

        #endregion

        /// <summary>
		/// Writes the marriage record into the specified database.
		/// </summary>
		/// <param name="oDb">Specifies the database to write the marriage record into.</param>
		/// <returns>True for success, false otherwise.</returns>
		public bool Save
			(
			clsDatabase oDb
			)
		{
			// Validate the ID
			if(m_nID==0)
			{
				return false;
			}

			// Write the record into the database
			string sSql = "UPDATE tbl_MarriageCertificates SET "+
                "GroReference="+clsDatabase.ToDb(GroReference)+","+
                "WhenMarried=" + clsDatabase.ToDb(When) + "," +
				"Location="+clsDatabase.ToDb(Location)
				+",GroomName="+clsDatabase.ToDb(GroomName)
				+",GroomAge="+clsDatabase.ToDb(GroomAge)
				+",GroomOccupation="+clsDatabase.ToDb(GroomOccupation)
				+",GroomLiving="+clsDatabase.ToDb(GroomLiving)
				+",GroomFather="+clsDatabase.ToDb(GroomFather)
				+",GroomFatherOccupation="+clsDatabase.ToDb(GroomFatherOccupation)
				+",BrideName="+clsDatabase.ToDb(BrideName)
				+",BrideAge="+clsDatabase.ToDb(BrideAge)
				+",BrideOccupation="+clsDatabase.ToDb(BrideOccupation)
				+",BrideLiving="+clsDatabase.ToDb(BrideLiving)
				+",BrideFather="+clsDatabase.ToDb(BrideFather)
				+",BrideFatherOccupation="+clsDatabase.ToDb(BrideFatherOccupation)
				+",Witness="+clsDatabase.ToDb(Witness)
				+" WHERE ID="+m_nID.ToString()+";";
			OleDbCommand oSql = new OleDbCommand(sSql,oDb.cnDB);
			int nNumRows = oSql.ExecuteNonQuery();
			if(nNumRows==0)
			{
                sSql = "INSERT INTO tbl_MarriageCertificates (ID,GroReference,WhenMarried,Location,GroomName,GroomAge,GroomOccupation,GroomLiving,GroomFather,GroomFatherOccupation,BrideName,BrideAge,BrideOccupation,BrideLiving,BrideFather,BrideFatherOccupation,Witness) VALUES ("+
					m_nID.ToString()+","+
                    clsDatabase.ToDb(GroReference)+","+
                    clsDatabase.ToDb(When)+","+
					clsDatabase.ToDb(Location)
					+","+clsDatabase.ToDb(GroomName)
					+","+clsDatabase.ToDb(GroomAge)
					+","+clsDatabase.ToDb(GroomOccupation)
					+","+clsDatabase.ToDb(GroomLiving)
					+","+clsDatabase.ToDb(GroomFather)
					+","+clsDatabase.ToDb(GroomFatherOccupation)
					+","+clsDatabase.ToDb(BrideName)
					+","+clsDatabase.ToDb(BrideAge)
					+","+clsDatabase.ToDb(BrideOccupation)
					+","+clsDatabase.ToDb(BrideLiving)
					+","+clsDatabase.ToDb(BrideFather)
					+","+clsDatabase.ToDb(BrideFatherOccupation)
					+","+clsDatabase.ToDb(Witness)
					+");";
				oSql = new OleDbCommand(sSql,oDb.cnDB);
				oSql.ExecuteNonQuery();
			}

            // Add the place (and links to this source) to the database
            if(Location!="")
            {
                oDb.AddPlace(Location,2,m_nID);
            }

			// Return success
			return true;
		}

        /// <summary>Returns the marriage certificate information in html format.</summary>
        /// <returns>A description of the marriage certificate in html format.</returns>
        public string ToHtml            (            clsDatabase oDb            )
        {
            // Initialise the Html description.
            StringBuilder sbHtml = new StringBuilder();

            sbHtml.Append("<TABLE align=center bgcolor=#ccff99 border=0 cellpadding=5 cellspacing=0>");
            sbHtml.Append("<TR><TD colspan=7>" + When.Year.ToString() + " <SPAN class=\"Marriage\">Marriage solemnized at</SPAN> ");
            sbHtml.Append(oDb.PlaceToHtml(Location) + "</TD></TR>");
            sbHtml.Append("<TR>");
            sbHtml.Append("<TD><SPAN class=\"Marriage\">When Married</SPAN></TD>");
            sbHtml.Append("<TD><SPAN class=\"Marriage\">Name</SPAN></TD>");
            sbHtml.Append("<TD><SPAN class=\"Marriage\">Age</SPAN></TD>");
            sbHtml.Append("<TD><SPAN class=\"Marriage\">Rank or Profession</SPAN></TD>");
            sbHtml.Append("<TD><SPAN class=\"Marriage\">Residence at the time of marriage</SPAN></TD>");
            sbHtml.Append("<TD><SPAN class=\"Marriage\">Father's Name</SPAN></TD>");
            sbHtml.Append("<TD><SPAN class=\"Marriage\">Rank of Profession of Father</SPAN></TD>");
            sbHtml.Append("</TR>");
            sbHtml.Append("<TR>");
            sbHtml.Append("<TD rowspan=2>"+When.ToString("d MMM yyyy")+"</TD>");
            sbHtml.Append("<TD>"+GroomName+"</TD>");
            sbHtml.Append("<TD>"+GroomAge+"</TD>");
            sbHtml.Append("<TD>"+GroomOccupation+"</TD>");
            sbHtml.Append("<TD>"+GroomLiving+"</TD>");
            sbHtml.Append("<TD>"+GroomFather+"</TD>");
            sbHtml.Append("<TD>"+GroomFatherOccupation+"</TD>");
            sbHtml.Append("</TR>");
            sbHtml.Append("<TR>");
            // sbHtml.Append("<TD><SPAN class=\"Small\">Bride</SPAN></TD>");
            sbHtml.Append("<TD>"+BrideName+"</TD>");
            sbHtml.Append("<TD>"+BrideAge+"</TD>");
            sbHtml.Append("<TD>"+BrideOccupation+"</TD>");
            sbHtml.Append("<TD>"+BrideLiving+"</TD>");
            sbHtml.Append("<TD>"+BrideFather+"</TD>");
            sbHtml.Append("<TD>"+BrideFatherOccupation+"</TD>");
            sbHtml.Append("</TR>");
            sbHtml.Append("<TR><TD colspan=7><SPAN class=\"Marriage\">in the Presence of us,</SPAN> ");
            sbHtml.Append(Witness+"</TD></TR>");
            sbHtml.Append("<TR><TD colspan=7 align=center><SPAN class=\"Marriage\">GRO Reference</SPAN> ");
            sbHtml.Append(GroReference + "</TD></TR>");
            sbHtml.Append("</TABLE>");

            // Return the Html description.
            return sbHtml.ToString();
        }

        /// <summary>Returns the marriarge certificate information in the webtrees format.</summary>
        /// <param name="oDb">Specifies a connection to the database.</param>
        /// <returns>A description of the marriage certificate in webtrees format.</returns>
        public string ToWebtrees(clsDatabase oDb)
        {
            // Initialise the Html description.
            StringBuilder sbHtml = new StringBuilder();

            sbHtml.Append("&lt;a name=\""+GroomName.ToLower().Replace(' ','_')+"_"+BrideName.ToLower().Replace(' ','_')+"\"&gt;&lt;/a&gt;<br/>");
            sbHtml.Append("&lt;h2&gt;"+When.Year.ToString()+" "+GroomName+" &amp; "+BrideName+"&lt;/h2&gt;<br/>");
            sbHtml.Append("&lt;table class=\"marriage\"&gt;<br/>");
            sbHtml.Append("&lt;tr&gt;&lt;td colspan=\"7\"&gt;&lt;span class=\"data\"&gt;" + When.Year.ToString() + "&lt;/span&gt; Marriage solemnized at &lt;span class=\"data\"&gt;");
            sbHtml.Append(Location + "&lt;/span&gt;&lt;/td&gt;&lt;/tr&gt;<br/>");
            sbHtml.Append("&lt;tr&gt;&lt;td&gt;When Married&lt;/td&gt;&lt;td&gt;Name&lt;/td&gt;&lt;td&gt;Age&lt;/td&gt;&lt;td>Rank or Profession&lt;/td&gt;&lt;td&gt;Residence at the time of marriage&lt;/td&gt;&lt;td&gt;Father's Name&lt;/td&gt;&lt;td&gt;Rank of Profession of Father&lt;/td&gt;&lt;/tr&gt;<br/>");
            sbHtml.Append("&lt;tr&gt;");
            sbHtml.Append("&lt;td class=\"data\" rowspan=\"2\"&gt;"+When.ToString("d MMM yyyy")+"&lt;/td&gt;");
            sbHtml.Append("&lt;td class=\"data\"&gt;"+GroomName+"&lt;/td&gt;");
            sbHtml.Append("&lt;td class=\"data\"&gt;"+GroomAge+"&lt;/td&gt;");
            sbHtml.Append("&lt;td class=\"data\"&gt;"+GroomOccupation+"&lt;/td&gt;");
            sbHtml.Append("&lt;td class=\"data\"&gt;"+GroomLiving+"&lt;/td&gt;");
            sbHtml.Append("&lt;td class=\"data\"&gt;"+GroomFather+"&lt;/td&gt;");
            sbHtml.Append("&lt;td class=\"data\"&gt;"+GroomFatherOccupation+"&lt;/td&gt;");
            sbHtml.Append("&lt;/tr&gt;<br/>");
            sbHtml.Append("&lt;tr&gt;");
            sbHtml.Append("&lt;td class=\"data\"&gt;"+BrideName+"&lt;/td&gt;");
            sbHtml.Append("&lt;td class=\"data\"&gt;"+BrideAge+"&lt;/td&gt;");
            sbHtml.Append("&lt;td class=\"data\"&gt;"+BrideOccupation+"&lt;/td&gt;");
            sbHtml.Append("&lt;td class=\"data\"&gt;"+BrideLiving+"&lt;/td&gt;");
            sbHtml.Append("&lt;td class=\"data\"&gt;"+BrideFather+"&lt;/td&gt;");
            sbHtml.Append("&lt;td class=\"data\"&gt;"+BrideFatherOccupation+"&lt;/td&gt;");
            sbHtml.Append("&lt;/tr&gt;<br/>");
            sbHtml.Append("&lt;tr&gt;&lt;td colspan=\"7\"&gt;in the Presence of us, &lt;span class=\"data\"&gt;");
            sbHtml.Append(Witness+"&lt;/span&gt;&lt;/td&gt;&lt;/tr&gt;<br/>");
            sbHtml.Append("&lt;tr&gt;&lt;td style=\"text-align:center\" colspan=\"7\"&gt;GRO Reference &lt;span class=\"data\"&gt;");
            sbHtml.Append(GroReference + "&lt;/span&gt;&lt;/td&gt;&lt;/tr&gt;<br/>");
            sbHtml.Append("&lt;/table&gt;<br/>");
            sbHtml.Append("&lt;table class=\"meta\"&gt;<br/>");
            sbHtml.Append("&lt;td class=\"label\"&gt;filename&lt;/td&gt;&lt;td class=\"value\"&gt;marriage_"+When.Year.ToString()+"_"+GroomName.ToLower().Replace(' ','_')+".png&lt;/td&gt;&lt;/tr&gt;<br/>");
            sbHtml.Append("&lt;td class=\"label\"&gt;Source Text&lt;/td&gt;&lt;td class=\"value\"&gt;On "+When.ToString("d MMM yyyy")+" at "+Location+". "+GroomName+" ("+GroomAge+"), "+GroomOccupation+" of "+GroomLiving+", son of "+GroomFather+", "+GroomFatherOccupation +" married "+BrideName+" ("+BrideAge +"), "+BrideOccupation +" of "+BrideLiving+", daughter of "+BrideFather+", "+BrideFatherOccupation +".  In the presence of "+Witness+".&lt;/td&gt;&lt;/tr&gt;<br/>");
            sbHtml.Append("&lt;td class=\"label\"&gt;Source Note&lt;/td&gt;&lt;td class=\"value\"&gt;GRO Reference "+GroReference+".&lt;/td&gt;&lt;/tr&gt;<br/>");
            sbHtml.Append("&lt;td class=\"label\"&gt;Citation Text&lt;/td&gt;&lt;td class=\"value\"&gt;"+GroomName+" married "+BrideName+".  "+GroomFather+" was father of the groom.  "+BrideFather+" was father of the bride.  "+Witness+" was a witness.&lt;/td&gt;&lt;/tr&gt;<br/>");
            sbHtml.Append("&lt;/table&gt;");
            // Return the Html description
            return sbHtml.ToString();
        }

		/// <summary>The ID of the marraige record.  This should match with the ID the parent source.</summary>
		public int ID { get { return m_nID; } set { m_nID = value; } }

	}
}
