using System;
using System.Data;
using System.Data.OleDb;

// StringBuilder
using System.Text;

namespace family_tree.objects
{
    /// <summary>Class to represent the additional information on a death certificate source.  This is closely related to the tbl_DeathCertificates table.</summary>
    public class DeathCertificate
    {
        #region Member Variables

        /// <summary>The ID of the death certificate record.  This should match with the ID the parent source.</summary>
        private int idx_;

        /// <summary>The registration district as specified on the birth certificate.</summary>
        public string registrationDistrict;

        /// <summary>The when field as specified on the birth certificate.</summary>
        public string when;

        /// <summary>The where field modified into a database place record.</summary>
        public string place;

        /// <summary>The name as specified on the birth certificate.</summary>
        public string name;

        /// <summary>The sex as specified on the birth certificate.</summary>
        public string sex;

        /// <summary>Date and place of birth.  Only on newer certificates.</summary>
        public string datePlaceOfBirth;

        /// <summary>Occupation of the dead person.</summary>
        public string occupation;

        /// <summary>Address of the dead person.</summary>
        public string usualAddress;

        /// <summary>Cause of death of the dead person.</summary>
        public string causeOfDeath;

        /// <summary>Name of the informant.</summary>
        public string informant;

        /// <summary>Description of the informant.  Wife, Husband, son etc ... </summary>
        public string informantDescription;

        /// <summary>Address of the informant.</summary>
        public string informantAddress;

        /// <summary>When the death certificate was created.</summary>
        public string whenRegistered;

        /// <summary>The GRO reference for the certificate.</summary>
        public string groReference;

        #endregion



        /// <summary>Class constructor.</summary>
        /// <param name="idx">Specifies the ID of the source record.</param>
        public DeathCertificate(int idx)
        {
            idx_ = idx;
        }



        /// <summary>Class constructor that loads the current values from the specified database.</summary>
        /// <param name="idx">Specifies the ID of the parent source record.</param>
        /// <param name="cndb">Specifies the database connection to load the information from.</param>
        public DeathCertificate(int idx, OleDbConnection cndb) : this(idx)
        {
            string sql = "SELECT * FROM tbl_DeathCertificates WHERE ID=" + idx_.ToString() + ";";
            OleDbCommand sqlCommand = new OleDbCommand(sql, cndb);
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            if (dataReader.Read())
            {
                registrationDistrict = Database.getString(dataReader, "RegistrationDistrict", "");
                when = Database.getString(dataReader, "WhenWhere", "");
                place = Database.getString(dataReader, "Place", "");
                name = Database.getString(dataReader, "Name", "");
                sex = Database.getString(dataReader, "Sex", "");
                datePlaceOfBirth = Database.getString(dataReader, "DatePlaceOfBirth", "");
                occupation = Database.getString(dataReader, "Occupation", "");
                usualAddress = Database.getString(dataReader, "UsualAddress", "");
                causeOfDeath = Database.getString(dataReader, "CauseOfDeath", "");
                informant = Database.getString(dataReader, "Informant", "");
                informantDescription = Database.getString(dataReader, "InformantDescription", "");
                informantAddress = Database.getString(dataReader, "InformantAddress", "");
                whenRegistered = Database.getString(dataReader, "WhenRegistered", "");
                groReference = Database.getString(dataReader, "GroReference", "");
            }
            dataReader.Close();
        }



        /// <summary>Writes the death certificate record into the specified database.</summary>
        /// <param name="database">Specifies the database to write the birth certificate record into.</param>
        /// <returns>True for success, false otherwise.</returns>
        public bool save(Database database)
        {
            // Validate the ID.
            if (idx_ == 0)
            {
                return false;
            }

            // Write the record into the database.
            string sql = "UPDATE tbl_DeathCertificates SET "
                + "RegistrationDistrict = " + Database.toDb(registrationDistrict)
                + ", WhenWhere = " + Database.toDb(when)
                + ", Place = " + Database.toDb(place)
                + ", Name = " + Database.toDb(name)
                + ", Sex = " + Database.toDb(sex)
                + ", DatePlaceOfBirth = " + Database.toDb(datePlaceOfBirth)
                + ", Occupation = " + Database.toDb(occupation)
                + ", UsualAddress = " + Database.toDb(usualAddress)
                + ", CauseOfDeath = " + Database.toDb(causeOfDeath)
                + ", Informant = " + Database.toDb(informant)
                + ", InformantDescription = " + Database.toDb(informantDescription)
                + ", InformantAddress = " + Database.toDb(informantAddress)
                + ", WhenRegistered = " + Database.toDb(whenRegistered)
                + ", GroReference = " + Database.toDb(groReference)
                + " WHERE ID = " + idx_.ToString() + ";";
            OleDbCommand sqlCommand = new OleDbCommand(sql, database.cndb);
            int numRows = sqlCommand.ExecuteNonQuery();
            if (numRows == 0)
            {
                sql = "INSERT INTO tbl_DeathCertificates (ID, RegistrationDistrict, WhenWhere, Place, Name, Sex, DatePlaceOfBirth, Occupation, UsualAddress, CauseOfDeath, Informant, InformantDescription, InformantAddress, WhenRegistered) VALUES ("
                    + idx_.ToString()
                    + ", " + Database.toDb(registrationDistrict)
                    + ", " + Database.toDb(when)
                    + ", " + Database.toDb(place)
                    + ", " + Database.toDb(name)
                    + ", " + Database.toDb(sex)
                    + ", " + Database.toDb(datePlaceOfBirth)
                    + ", " + Database.toDb(occupation)
                    + ", " + Database.toDb(usualAddress)
                    + ", " + Database.toDb(causeOfDeath)
                    + ", " + Database.toDb(informant)
                    + ", " + Database.toDb(informantDescription)
                    + ", " + Database.toDb(informantAddress)
                    + ", " + Database.toDb(whenRegistered)
                    + ");";
                sqlCommand = new OleDbCommand(sql, database.cndb);
                sqlCommand.ExecuteNonQuery();
            }

            // Return success.
            return true;
        }



        /// <summary>Return the death certificate information in Html format.</summary>
        /// <returns>A description of the death certificate in Html format.</returns>
        public string toHtml(Database database)
        {
            // Initialise the Html description
            StringBuilder html = new StringBuilder();

            html.Append("<table style=\"background-color: thistle; border: 1px solid black;\" align=center cellpadding=5 cellspacing=0>");
            html.Append("<TR><TD align=right><SPAN class=\"Death\">Registration District</SPAN></TD><TD colspan=3>" + registrationDistrict + "</TD></TR>");
            html.Append("<TR><TD align=right><SPAN class=\"Death\">When and Where</SPAN></TD><TD colspan=3>" + when + "</TD></TR>");
            html.Append("<TR><TD align=right><SPAN class=\"Death\">Name</SPAN></TD><TD>" + name + "</TD>");
            html.Append("<TD align=right><SPAN class=\"Death\">Sex</SPAN></TD><TD>" + sex + "</TD></TR>");
            html.Append("<TR><TD align=right><SPAN class=\"Death\">Date Place of Birth</SPAN></TD><TD colspan=3>" + datePlaceOfBirth + "</TD></TR>");
            html.Append("<TR><TD align=right><SPAN class=\"Death\">Occupation</SPAN></TD><TD colspan=3>" + occupation + "</TD></TR>");
            html.Append("<TR><TD align=right><SPAN class=\"Death\">Usual Address</SPAN></TD><TD colspan=3>" + usualAddress + "</TD></TR>");
            html.Append("<TR><TD align=right><SPAN class=\"Death\">Cause of Death</SPAN></TD><TD colspan=3>" + Database.htmlString(causeOfDeath) + "</TD></TR>");
            html.Append("<TR><TD align=right><SPAN class=\"Death\">Informant</SPAN></TD><TD>" + informant + "</TD>");
            html.Append("<TD align=right><SPAN class=\"Death\">Informant Description</SPAN></TD><TD>" + informantDescription + "</TD></TR>");
            html.Append("<TR><TD align=right><SPAN class=\"Death\">Informant Address</SPAN></TD><TD colspan=3>" + informantAddress + "</TD></TR>");
            html.Append("<TR><TD align=right><SPAN class=\"Death\">When Registered</SPAN></TD><TD>" + whenRegistered + "</TD>");
            html.Append("<TD align=right><SPAN class=\"Death\">Reference</SPAN></TD><TD>" + groReference + "</TD></TR>");
            html.Append("</table>");

            // Return the Html description.
            return html.ToString();
        }



        /// <summary>Returns the death certificate information format for a webtree's certificate..</summary>
        /// <returns>A description of the death certificate in webtree's format.</returns>
        public string toWebtrees(Database database)
        {
            // Initialise the Html description.
            StringBuilder html = new StringBuilder();

            html.Append("&lt;a name=\"" + name.ToLower().Replace(' ', '_') + "_" + when.Substring(when.Length - 4) + "\"&gt;<br/>");
            html.Append("&lt;h2&gt;" + name + "&lt;/h2&gt;<br/>");
            html.Append("&lt;table class=\"death\"&gt;<br/>");
            html.Append("&lt;tr&gt;&lt;td&gt;Registration District&lt;/td&gt;&lt;<TD colspan=3>" + registrationDistrict + "&lt;/td&gt;&lt;/tr&gt;&lt;<br/>");
            html.Append("&lt;tr&gt;&lt;td&gt;When and Where</SPAN>&lt;/td&gt;&lt;<TD colspan=3>" + when + "&lt;/td&gt;&lt;/tr&gt;&lt;<br/>");
            html.Append("&lt;tr&gt;&lt;td&gt;Name&lt;/td&gt;&lt;td&gt;" + name + "&lt;/td&gt;");
            html.Append("&lt;td&gt;Sex&lt;/td&gt;&lt;td&gt;" + sex + "&lt;/td&gt;&lt;/tr&gt;&lt;<br/>");
            html.Append("&lt;tr&gt;&lt;td&gt;Date Place of Birth&lt;/td&gt;&lt;td colspan=3>" + datePlaceOfBirth + "&lt;/td&gt;&lt;/tr&gt;&lt;<br/>");
            html.Append("&lt;tr&gt;&lt;td&gt;Occupation&lt;/td&gt;&lt;td colspan=3>" + occupation + "&lt;/td&gt;&lt;/tr&gt;&lt;<br/>");
            html.Append("&lt;tr&gt;&lt;td&gt;Usual Address&lt;/td&gt;&lt;td colspan=\"3\"&gt;" + usualAddress + "&lt;/td&gt;&lt;/tr&gt;&lt;<br/>");
            html.Append("&lt;tr&gt;&lt;td&gt;Cause of Death&lt;/td&gt;&lt;td colspan=\"3\"&gt;" + Database.htmlString(causeOfDeath) + "&lt;/td&gt;&lt;/tr&gt;<br/>");
            html.Append("&lt;tr&gt;&lt;td&gt;Informant&lt;/td&gt;&lt;TD>" + informant + "&lt;/td&gt;");
            html.Append("<TD align=right>Informant Description&lt;/td&gt;&lt;td&gt;" + informantDescription + "&lt;/td&gt;&lt;/tr&gt;<br/>");
            html.Append("&lt;tr&gt;&lt;td&gt;Informant Address&lt;/td&gt;&lt;td class=\"data\" colspan=\"3\"&gt;" + informantAddress + "&lt;/td&gt;&lt;/tr&gt;<br/>");
            html.Append("&lt;tr&gt;&lt;td&gt;When Registered&lt;/td&gt;&lt;td&gt;" + whenRegistered + "&lt;/td&gt;");
            html.Append("&lt;td&gt;Reference&lt;td&gt;&lt;td class=\"data\"&gt;" + groReference + "&lt;/td&gt;&lt;/tr&gt;<br/>");
            html.Append("&lt;/table class=\"death\"&gt;<br/>");

            // Return the Html description.
            return html.ToString();
        }


        /// <summary>The ID of the birth certificate record.  This should match with the ID the parent source.</summary>
        public int idx { get { return idx_; } set { idx_ = value; } }

    }
}
