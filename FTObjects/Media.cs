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
    public class Media
    {
        #region Member Variables

        /// <summary>The ID of the media object.</summary>
        public int index_;

        /// <summary>The database that this media object is attached to.</summary>
        private Database database_;

        /// <summary>The human readable title for the the media object.</summary>
        public string title;

        /// <summary>The local filename for the media object.</summary>
        public string fileName;

        /// <summary>Type of media object.  1 for an image.</summary>
        public int typeIndex;

        /// <summary>No idea what this is.  Required for the Gedom file.</summary>
        public bool isPrimary;

        /// <summary>No idea what this is.  Required for the Gedom file.</summary>
        public bool isThumbnail;

        /// <summary>The actual width of the source image.</summary>
        public int width;

        /// <summary>The actual height of the source image.</summary>
        public int height;

        /// <summary>The list of indexes of people who are attached to this media object.</summary>
        private ArrayList attachedPeople_;

        #endregion

        #region Class constructor



        /// <summary>Empty class constructor in the specified database.</summary>
        /// <param name="database">Specifies the database that contains the media object.</param>
        public Media(Database database)
        {
            // Save the input parameters.
            database_ = database;

            // Default values for the media object.
            index_ = 0;
            title = "";
            fileName = "";
            typeIndex = 1;
            isPrimary = true;
            isThumbnail = true;
            width = -1;
            height = -1;
            attachedPeople_ = null;
        }



        /// <summary>Creates a clsMedia object that represents the specified media object in the specified database.</summary>
        /// <param name="database">Specifies the database that contains the media object.</param>
        /// <param name="mediaIndex">Specifies the ID of the media object.</param>
        public Media(Database database, int mediaIndex)
        {
            // Save the input parameters.
            database_ = database;

            // Open the specified media object.
            OleDbCommand sqlCommand = new OleDbCommand("SELECT * FROM tbl_Media WHERE ID=" + mediaIndex.ToString() + ";", database.cndb);
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            if (dataReader.Read())
            {
                index_ = mediaIndex;
                title = walton.Database.getString(dataReader, "Title", "Title");
                fileName = walton.Database.getString(dataReader, "Filename", "Filename");
                typeIndex = walton.Database.getInt(dataReader, "TypeID", 1);
                isPrimary = walton.Database.getBool(dataReader, "Primary", true);
                isThumbnail = walton.Database.getBool(dataReader, "Thumbnail", true);
                width = walton.Database.getInt(dataReader, "Width", -1);
                height = walton.Database.getInt(dataReader, "Height", -1);
            }
            dataReader.Close();

            attachedPeople_ = null;
        }



        #endregion

        #region IO



        /// <summary>Writes this media object into the database.</summary>
        /// <returns></returns>
        public bool save()
        {
            // Assume success.
            bool isErrors = false;

            // Build the Sql command to update the media object
            StringBuilder sql = new StringBuilder();
            OleDbCommand sqlCommand;
            if (index_ > 0)
            {
                // Update an existing media object
                sql.Append("UPDATE tbl_Media SET ");
                sql.Append("Title = " + walton.Database.toDb(title) + ", ");
                sql.Append("Width = " + walton.Database.toDb(width, -1) + ", ");
                sql.Append("Height = " + walton.Database.toDb(height, -1) + ", ");
                sql.Append("[Primary] = " + walton.Database.toDb(isPrimary) + ", ");
                sql.Append("Thumbnail = " + walton.Database.toDb(isThumbnail) + " ");
                sql.Append("WHERE ID = ");
                sql.Append(index_);
                sql.Append(";");
            }
            else
            {
                // Find the ID for the new Media object.
                sqlCommand = new OleDbCommand("SELECT MAX(ID) AS NewID FROM tbl_Media;", database_.cndb);
                index_ = 1 + int.Parse(sqlCommand.ExecuteScalar().ToString());

                // Insert a new media object.
                sql.Append("INSERT INTO tbl_Media (ID, Filename, Title, Width, Height, [Primary], Thumbnail) VALUES (");
                sql.Append(index_.ToString() + ",");
                sql.Append(walton.Database.toDb(fileName) + ", ");
                sql.Append(walton.Database.toDb(title) + ", ");
                sql.Append(walton.Database.toDb(width, -1) + ", ");
                sql.Append(walton.Database.toDb(height, -1) + ", ");
                sql.Append(walton.Database.toDb(isPrimary) + ", ");
                sql.Append(walton.Database.toDb(isThumbnail) + ");");
            }
            sqlCommand = new OleDbCommand(sql.ToString(), database_.cndb);
            try
            {
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception.Message);
                isErrors = true;
            }

            // Save the attached people.
            if (attachedPeople_ != null)
            {
                // Remove all the previous people.
                sqlCommand = new OleDbCommand("DELETE FROM tbl_AdditionalMediaForPeople WHERE MediaID = " + index_.ToString() + ";",database_.cndb);
                sqlCommand.ExecuteNonQuery();

                // Attach the new people.
                int[] people = getAttachedPeople();
                foreach (int personIndex in people)
                {
                    sqlCommand = new OleDbCommand("INSERT INTO tbl_AdditionalMediaForPeople (PersonID, MediaID) VALUES (" + personIndex.ToString() + "," + index_.ToString() + ");", database_.cndb);
                    try
                    {
                        sqlCommand.ExecuteNonQuery();
                    }
                    catch
                    {
                        isErrors = true;
                    }
                }
            }

            // Return success or failure.
            return !isErrors;
        }



        #endregion

        #region Sizing



        /// <summary>Returns the required width to keep the aspect ratio for the specified height.</summary>
        /// <param name="height">Specifies the height required for the image.</param>
        /// <returns>The width that will keep the aspect ratio with the specified height.</returns>
        public int widthForSpecifiedHeight(int height)
        {
            // Check that the width and height are known.
            if (this.width <= 0 || this.height <= 0)
            {
                return height;
            }

            return (this.width * height) / this.height;
        }



        /// <summary>Returns the required height to keep the aspect ratio for the specified width.</summary>
        /// <param name="width">Specifies the width required for the image.</param>
        /// <returns>The height that will keep the aspect ratio with the specified width.</returns>
        public int heightForSpecifiedWidth(int width)
        {
            // Check that the width and height are known
            if (this.width <= 0 || this.height <= 0)
            {
                return width;
            }

            return (this.height * width) / this.width;
        }



        #endregion

        #region html



        /// <summary>Returns a html description of the media object. </summary>
        /// <returns></returns>
        public string toHtml()
        {
            StringBuilder html = new StringBuilder();

            html.Append("<body>");
            html.Append("<h1>" + title + "</h1>");
            html.Append("<img src=\"" + fullFileName + "\" />");

            // Show the people attached to this media object.
            int[] people = getAttachedPeople();
            if (people.Length > 0)
            {
                html.Append("<table>");
                foreach (int personIndex in people)
                {
                    Person person = new Person(personIndex, database_);
                    html.Append("<tr bgcolor=\"silver\"><td><span class=\"Small\">");
                    html.Append("<a href=\"person:" + personIndex.ToString() + "\">");
                    html.Append(person.getName(true, false));
                    html.Append("</a>");
                    html.Append("</span></td></tr>");
                }
                html.Append("</table>");
            }

            // Close the html.
            html.Append("</body></html>");

            // Return the html built.
            return html.ToString();
        }



        #endregion

        #region Attached People



        /// <summary>Load the list of attached people to this media object from the database.</summary>
        private void loadAttachedPeople()
        {
            // Find the people attached to this media object
            attachedPeople_ = new ArrayList();
            OleDbCommand sqlCommand = new OleDbCommand("SELECT PersonID FROM tbl_AdditionalMediaForPeople WHERE MediaID=" + index_.ToString() + ";", database_.cndb);
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                attachedPeople_.Add(dataReader.GetInt32(0));
            }
            dataReader.Close();
        }



        /// <summary>Returns the collection of indexes of people who are connected to this media object.</summary>
        /// <returns>A collection of indexes of people who are connected to this media object.</returns>
        public int[] getAttachedPeople()
        {
            // Check that the attached people are loaded
            if (attachedPeople_ == null)
            {
                loadAttachedPeople();
            }

            // Return the attached people.
            return (int[])attachedPeople_.ToArray(typeof(int));
        }



        /// <summary>Adds the specified person to the collection person attached to this media object.</summary>
        /// <param name="personIndex"></param>
        public void addPerson(int personIndex)
        {
            // Check that the attached people are loaded.
            if (attachedPeople_ == null)
            {
                loadAttachedPeople();
            }

            // Add the specified person.
            attachedPeople_.Add(personIndex);
        }



        /// <summary>Remove all the people attached to this media object.</summary>
        public void removeAllPeople()
        {
            attachedPeople_ = new ArrayList();
        }



        #endregion

        #region Public Properties



        /// <summary>The string description of the object.</summary>
        /// <returns>A string description of the object.</returns>
        public override string ToString()
        {
            return title;
        }



        /// <summary>The full path and filename for the media object.</summary>
        public string fullFileName
        {
            get
            {
                string fullFileName = database_.getMediaDirectory() + "\\" + fileName;
                if (File.Exists(fullFileName))
                {
                    return fullFileName;
                }
                return "";
            }
        }



        #endregion
    }
}
