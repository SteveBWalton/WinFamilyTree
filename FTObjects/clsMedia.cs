using System;
using System.Collections.Generic;
using System.Text;

// Access database ADO.NET
using System.Data;
using System.Data.OleDb;

using System.IO;

// ArrayList
using System.Collections;


namespace FamilyTree.Objects
{
    /// <summary>Class to represent a media object.</summary>
    public class clsMedia
    {
        #region Member Variables

        /// <summary>The ID of the media object.</summary>
        public int ID;

        /// <summary>The database that this media object is attached to.</summary>
        private Database m_oDb;

        /// <summary>The human readable title for the the media object.</summary>
        public string Title;

        /// <summary>The local filename for the media object.</summary>
        public string Filename;

        /// <summary>
        /// Type of media object.
        /// 1 for an image.
        /// </summary>
        public int TypeID;
        
        /// <summary>No idea what this is.  Required for the Gedom file.</summary>
        public bool Primary;

        /// <summary>No idea what this is.  Required for the Gedom file.</summary>
        public bool Thumbnail;

        /// <summary>The actual width of the source image.</summary>
        public int Width;

        /// <summary>The actual height of the source image.</summary>
        public int Height;

        /// <summary>The list of indexes of people who are attached to this media object.</summary>
        private ArrayList m_oAttachedPeople;

        #endregion

        #region Class constructor

        /// <summary>Empty class constructor in the specified database.
        /// </summary>
        /// <param name="oDb">Specifies the database that contains the media object.</param>
        public clsMedia(Database oDb)
        {
            // Save the input parameters
            m_oDb = oDb;

            // Default values for the media object.
            ID = 0;
            Title = "";
            Filename = "";
            TypeID = 1;
            Primary = true;
            Thumbnail = true;
            Width = -1;
            Height = -1;
            m_oAttachedPeople = null;
        }

        /// <summary>Creates a clsMedia object that represents the specified media object in the specified database.
        /// </summary>
        /// <param name="oDb">Specifies the database that contains the media object.</param>
        /// <param name="nMediaID">Specifies the ID of the media object.</param>
        public clsMedia(Database oDb,int nMediaID)
        {
            // Save the input parameters
            m_oDb = oDb;

            // Open the specified media object
            OleDbCommand oSql = new OleDbCommand("SELECT * FROM tbl_Media WHERE ID=" + nMediaID.ToString() + ";",oDb.cndb);
            OleDbDataReader drMedia = oSql.ExecuteReader();
            if(drMedia.Read())
            {
                ID = nMediaID;
                Title = Innoval.clsDatabase.GetString(drMedia,"Title","Title");
                Filename = Innoval.clsDatabase.GetString(drMedia,"Filename","Filename");
                TypeID = Innoval.clsDatabase.GetInt(drMedia,"TypeID",1);
                Primary = Innoval.clsDatabase.GetBool(drMedia,"Primary",true);
                Thumbnail = Innoval.clsDatabase.GetBool(drMedia,"Thumbnail",true);
                Width = Innoval.clsDatabase.GetInt(drMedia,"Width",-1);
                Height = Innoval.clsDatabase.GetInt(drMedia,"Height",-1);
            }
            drMedia.Close();

            m_oAttachedPeople = null;
        }

        #endregion



        /// <summary>Writes this media object into the database.</summary>
        /// <returns></returns>
        public bool Save()
        {
            // Assume success
            bool bErrors = false;

            // Build the Sql command to update the media object
            string sSql;
            StringBuilder sbSql = new StringBuilder();
            OleDbCommand oSql;
            if(ID > 0)
            {
                // Update an existing media object
                sbSql.Append("UPDATE tbl_Media SET ");
                sbSql.Append("Title=" + Innoval.clsDatabase.ToDb(Title) + ",");
                sbSql.Append("Width=" + Innoval.clsDatabase.ToDb(Width,-1) + ",");
                sbSql.Append("Height=" + Innoval.clsDatabase.ToDb(Height, -1) + ",");
                sbSql.Append("[Primary]=" + Innoval.clsDatabase.ToDb(Primary) + ",");
                sbSql.Append("Thumbnail=" + Innoval.clsDatabase.ToDb(Thumbnail) + " ");
                sbSql.Append("WHERE ID=");
                sbSql.Append(ID);
                sbSql.Append(";");
            }
            else
            {
                // Find the ID for the new Media object
                sSql = "SELECT MAX(ID) AS NewID FROM tbl_Media;";
                oSql = new OleDbCommand(sSql,m_oDb.cndb);
                ID = 1 + int.Parse(oSql.ExecuteScalar().ToString());

                // Insert a new media object
                sbSql.Append("INSERT INTO tbl_Media (ID,Filename,Title,Width,Height,[Primary],Thumbnail) VALUES (");
                sbSql.Append(ID.ToString() + ",");
                sbSql.Append(Innoval.clsDatabase.ToDb(Filename) + ",");
                sbSql.Append(Innoval.clsDatabase.ToDb(Title) + ",");
                sbSql.Append(Innoval.clsDatabase.ToDb(Width,-1) + ",");
                sbSql.Append(Innoval.clsDatabase.ToDb(Height, -1) + ",");
                sbSql.Append(Innoval.clsDatabase.ToDb(Primary) + ",");
                sbSql.Append(Innoval.clsDatabase.ToDb(Thumbnail) + ");");
            }
            oSql = new OleDbCommand(sbSql.ToString(),m_oDb.cndb);
            try
            {
                oSql.ExecuteNonQuery();
            }
            catch(Exception oError)
            {
                System.Diagnostics.Debug.WriteLine(oError.Message);
                bErrors = true;
            }

            // Save the attached people
            if(m_oAttachedPeople != null)
            {
                // Remove all the previous people
                sSql = "DELETE FROM tbl_AdditionalMediaForPeople WHERE MediaID=" + ID.ToString() + ";";
                oSql = new OleDbCommand(sSql,m_oDb.cndb);
                oSql.ExecuteNonQuery();

                // Attach the new people
                int[] oPeople = GetAttachedPeople();
                foreach(int nPersonID in oPeople)
                {
                    sSql = "INSERT INTO tbl_AdditionalMediaForPeople (PersonID,MediaID) VALUES ("+nPersonID.ToString()+","+ID.ToString()+");";
                    oSql = new OleDbCommand(sSql,m_oDb.cndb);
                    try
                    {
                        oSql.ExecuteNonQuery();
                    }
                    catch
                    {
                        bErrors = true;
                    }
                }
            }

            // Return success or failure
            return !bErrors;
        }



        #region Sizing

        /// <summary>Returns the required width to keep the aspect ratio for the specified height.
        /// </summary>
        /// <param name="nHeight">Specifies the height required for the image.</param>
        /// <returns>The width that will keep the aspect ratio with the specified height.</returns>
        public int WidthForSpecifiedHeight(int nHeight)
        {
            // Check that the width and height are known
            if(Width <= 0 || Height <= 0)
            {
                return nHeight;
            }

            return (Width * nHeight) / Height;
        }

        /// <summary>Returns the required height to keep the aspect ratio for the specified width.
        /// </summary>
        /// <param name="nWidth">Specifies the width required for the image.</param>
        /// <returns>The height that will keep the aspect ratio with the specified width.</returns>
        public int HeightForSpecifiedWidth(int nWidth)
        {
            // Check that the width and height are known
            if(Width <= 0 || Height <= 0)
            {
                return nWidth;
            }

            return (Height * nWidth) / Width;
        }

        #endregion



        /// <summary>Returns a html description of the media object. </summary>
        /// <returns></returns>
        public string ToHtml()
        {
            StringBuilder sbHtml = new StringBuilder();

            sbHtml.Append("<body>");
            sbHtml.Append("<h1>" + Title + "</h1>");
            sbHtml.Append("<img src=\"" + FullFilename + "\" />");

            // Show the person attached to this media object
            int[] oPeople = GetAttachedPeople();
            if(oPeople.Length > 0)
            {
                sbHtml.Append("<table>");
                foreach(int nPersonID in oPeople)
                {
                    Person oPerson = new Person(nPersonID,m_oDb);
                    sbHtml.Append("<tr bgcolor=\"silver\"><td><span class=\"Small\">");
                    sbHtml.Append("<a href=\"person:" + nPersonID.ToString() + "\">");
                    sbHtml.Append(oPerson.GetName(true,false));
                    sbHtml.Append("</a>");
                    sbHtml.Append("</span></td></tr>");
                }
                sbHtml.Append("</table>");
            }
            
            // Close the html
            sbHtml.Append("</body></html>");

            // Return the html built
            return sbHtml.ToString();
        }



        #region Attached People

        /// <summary>
        /// Load the list of attached people to this media object from the database.
        /// </summary>
        private void LoadAttachedPeople()
        {
            // Find the people attached to this media object
            m_oAttachedPeople = new ArrayList();
            OleDbCommand oSql = new OleDbCommand("SELECT PersonID FROM tbl_AdditionalMediaForPeople WHERE MediaID=" + ID.ToString() + ";",m_oDb.cndb);
            OleDbDataReader drPeople = oSql.ExecuteReader();
            while(drPeople.Read())
            {
                m_oAttachedPeople.Add(drPeople.GetInt32(0));
            }
            drPeople.Close();
        }
        
        /// <summary>
        /// Returns the collection of indexes of people who are connected to this media object.
        /// </summary>
        /// <returns>A collection of indexes of people who are connected to this media object.</returns>
        public int[] GetAttachedPeople()
        {
            // Check that the attached people are loaded
            if(m_oAttachedPeople == null)
            {
                LoadAttachedPeople();
            }

            // Return the attached people
            return (int[])m_oAttachedPeople.ToArray(typeof(int));
        }

        /// <summary>
        /// Adds the specified person to the collection person attached to this media object.
        /// </summary>
        /// <param name="nPersonID"></param>
        public void AddPerson
            (
            int nPersonID
            )
        {
            // Check that the attached people are loaded
            if(m_oAttachedPeople == null)
            {
                LoadAttachedPeople();
            }

            // Add the specified person
            m_oAttachedPeople.Add(nPersonID);
        }

        /// <summary>
        /// Remove all the people attached to this media object.
        /// </summary>
        public void RemoveAllPeople()
        {
            m_oAttachedPeople = new ArrayList();
        }

        #endregion

        #region Public Properties

        // The string description of the object.
        /// <summary>
        /// The string description of the object.
        /// </summary>
        /// <returns>A string description of the object.</returns>
        public override string ToString()
        {
            return Title;
        }

        // The full path and filename for the media object.
        /// <summary>
        /// The full path and filename for the media object.
        /// </summary>
        public string FullFilename
        {
            get
            {
                string sFullFilename = m_oDb.getMediaDirectory() + "\\" + Filename;
                if(File.Exists(sFullFilename))
                {
                    return sFullFilename;
                }
                return "";
            }
        }

        #endregion
    }
}
