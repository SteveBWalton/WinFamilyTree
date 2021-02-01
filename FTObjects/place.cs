using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.OleDb;

namespace family_tree.objects
{
    /// <summary>Class to represent a place in the database.</summary>
    public class Place
    {
        #region Member Variables

        /// <summary>Database that this place is attached to.</summary>
        private Database database_;

        /// <summary>ID of this place in the database.</summary>
        private int index_;

        /// <summary>Name of this place.</summary>
        private string name_;

        /// <summary>ID of the parent place for this place.  Zero indicates top level, no parent.</summary>
        private int parentIndex_;

        /// <summary>The status of this place.  0 - Place, 1 - Address.</summary>
        private int status_;

        /// <summary>The degrees to the east.</summary>
        private float longitude_;

        /// <summary>The degrees to the north.</summary>
        private float latitude_;

        /// <summary>The zoom level to use when displaying on google maps.</summary>
        private int googleZoom_;

        /// <summary>True to use the longitude and latitude of the parent location.</summary>
        private bool isUseParentLocation_;

        /// <summary>Comments about this location.</summary>
        private string privateComments_;

        #endregion

        #region Constructors and Database



        public Place(OleDbDataReader dataReader, Database database)
        {
            database_ = database;
            read(dataReader);
        }



        /// <summary>Class constructor to load a place from the database.  If the place is not found in the database then a new object is created.</summary>
        /// <param name="index">Specifies the ID of the place.</param>
        /// <param name="database">Specifies the database that contains the place.</param>
        public Place(int index, Database database)
        {
            database_ = database;
            index_ = index;

            string sql = "SELECT ID, Name, ParentID, Status, Longitude, Latitude, GoogleZoom, UseParentLocation, PrivateComments FROM tbl_Places WHERE ID = " + index_.ToString() + ";";
            OleDbCommand sqlCommand = new OleDbCommand(sql, database.cndb);
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            if (dataReader.Read())
            {
                read(dataReader);
            }
            else
            {
                name_ = "";
                parentIndex_ = 0;
                status_ = 0;
                longitude_ = -999;
                latitude_ = -999;
                googleZoom_ = 10;
                privateComments_ = string.Empty;
                isUseParentLocation_ = true;
            }
            dataReader.Close();
        }



        /// <summary>Saves the place object into the database.  Will only update an existing place record.  To add new places use addPlace().</summary>
        /// <returns>True for success, false otherwise.</returns>
        public bool save()
        {
            // Update the tbl_Places table.
            if (index_ > 0)
            {
                StringBuilder sql = new StringBuilder("UPDATE tbl_Places SET ");
                sql.Append("Status = " + status_.ToString() + ", ");
                sql.Append("Longitude = " + longitude_.ToString() + ", ");
                sql.Append("Latitude = " + latitude_.ToString() + ", ");
                sql.Append("GoogleZoom = " + googleZoom_.ToString() + ", ");
                sql.Append("UseParentLocation = " + walton.Database.toDb(isUseParentLocation_) + ", ");
                sql.Append("PrivateComments = " + walton.Database.toDb(privateComments_) + " ");
                sql.Append("WHERE ID = " + index_.ToString() + ";");
                OleDbCommand sqlCommand = new OleDbCommand(sql.ToString(), database_.cndb);
                sqlCommand.ExecuteNonQuery();
            }

            // Return success.
            return true;
        }



        private void read(OleDbDataReader dataReader)
        {
            index_ = Database.getInt(dataReader, "ID", 0);
            name_ = Database.getString(dataReader, "Name", "Error");
            parentIndex_ = Database.getInt(dataReader, "ParentID", 0);
            status_ = Database.getInt(dataReader, "Status", 0);
            longitude_ = walton.Database.getFloat(dataReader, "Longitude", -999);
            latitude_ = walton.Database.getFloat(dataReader, "Latitude", -999);
            googleZoom_ = walton.Database.getInt(dataReader, "GoogleZoom", 10);
            privateComments_ = walton.Database.getString(dataReader, "PrivateComments", string.Empty);
            isUseParentLocation_ = walton.Database.getBool(dataReader, "UseParentLocation", true);
        }



        #endregion

        #region Public Properties

        /// <summary>The ID of this place in the database.</summary>
        public int index { get { return index_; } }

        /// <summary>The short name of this place.</summary>
        public string name { get { return name_; } }

        /// <summary>The ID of the parent place.  Zero is top level and no parent.</summary>
        public int parentIndex { get { return parentIndex_; } }

        /// <summary>The parent place of this place.</summary>
        public Place parent
        {
            get
            {
                if (parentIndex_ == 0)
                {
                    return null;
                }
                return new Place(parentIndex_, database_);
            }
        }



        /// <summary>The status of this place.  0 - Place, 1 - Address.</summary>
        public int status
        {
            get { return status_; }
            set { status_ = value; }
        }



        /// <summary>The longitude of this place in degrees to the east.</summary>
        public float longitude
        {
            get
            {
                if (isUseParentLocation_)
                {
                    if (parentIndex_ != 0)
                    {
                        return parent.longitude;
                    }
                    return 0;
                }
                return longitude_;
            }
            set
            {
                longitude_ = value;
            }
        }



        /// <summary>The latitude of this place in degrees to the north.</summary>
        public float latitude
        {
            get
            {
                if (isUseParentLocation_)
                {
                    if (parentIndex_ != 0)
                    {
                        return parent.latitude;
                    }
                    return 0;
                }
                return latitude_;
            }
            set
            {
                latitude_ = value;
            }
        }



        /// <summary>The zoom to use on a google map of this place.</summary>
        public int googleZoom
        {
            get
            {
                if (isUseParentLocation_)
                {
                    if (parentIndex_ != 0)
                    {
                        return parent.googleZoom;
                    }
                    return 1;
                }
                return googleZoom_;
            }
            set
            {
                googleZoom_ = value;
            }
        }



        /// <summary>True to use the longitude and latitude of the parent location.</summary>
        public bool isUseParentLocation
        {
            get { return isUseParentLocation_; }
            set { isUseParentLocation_ = value; }
        }



        /// <summary>The private comments attached to this location.</summary>
        public string privateComments
        {
            get { return privateComments_; }
            set { privateComments_ = value; }
        }



        /// <summary>Returns a html description of the place.</summary>
        /// <returns>A description of the place in html format.</returns>
        public string toHtml(bool isGoogleMap)
        {
            // Build a html description of the place
            StringBuilder html = new StringBuilder();

            // Build a name for the place.
            StringBuilder fullName = new StringBuilder();
            int parentIndex = parentIndex_;
            while (parentIndex != 0)
            {
                Place parent = new Place(parentIndex, database_);
                fullName.Insert(0, "<a href=\"place:" + parentIndex.ToString() + "\">" + parent.name + "</a>, ");
                parentIndex = parent.parentIndex;
            }
            fullName.Insert(0, "<a href=\"place:0\">Top Level</a>, ");

            html.Append("<h1>");
            html.Append(fullName);
            html.Append(name_);
            html.Append("<span class=\"Small\"> (");
            switch (status_)
            {
            case 0:
                html.Append("Place");
                break;
            case 1:
                html.Append("Address");
                break;
            }
            html.Append(")</span>");
            html.AppendLine("</h1>");

            // Display the private comments.
            if (privateComments_ != string.Empty)
            {
                html.AppendLine("<p><strong>Private comments</strong>: " + privateComments_ + "</p>");
            }

            // Add a goggle map of the place.
            if (isGoogleMap)
            {
                html.AppendLine(googleMap(400, 200));
            }

            html.AppendLine("<table border=\"0\">\n<tr valign=\"top\">");

            // Show the child places from this place.
            string sql = "SELECT ID, Name, Status FROM tbl_Places WHERE ParentID = " + index_.ToString() + " ORDER BY Status, Name;";
            OleDbCommand sqlCommand = new OleDbCommand(sql, database_.cndb);
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            bool isFirst = true;
            while (dataReader.Read())
            {
                if (isFirst)
                {
                    isFirst = false;
                    html.Append("<td>");
                    html.Append("<span class=\"Background\">Child Locations</span>");
                    html.Append("<table cellpadding=\"3\" cellspacing=\"2\">");
                }
                html.Append("<tr bgcolor=\"silver\"><td>");
                html.Append("<span class=\"Small\">");
                html.Append("<a href=\"place:" + dataReader.GetInt32(0).ToString() + "\">");
                html.Append(dataReader.GetString(1));
                html.Append("</a>");
                if (Database.getInt(dataReader, "Status", 0) == 0)
                {
                    html.Append(" (Place)");
                }
                else
                {
                    html.Append(" (Address)");
                }

                html.Append("</span>");
                html.AppendLine("</td></tr>");
            }
            if (!isFirst)
            {
                html.AppendLine("</table>");
                html.Append("</td>");
            }
            dataReader.Close();

            // Show the people with a connection to this place.
            sql = "SELECT tbl_ToPlaces.ObjectID, tbl_People.Forenames, tbl_People.MaidenName, tbl_People.Surname, tbl_People.Born, tbl_People.Died FROM tbl_ToPlaces INNER JOIN tbl_People ON tbl_ToPlaces.ObjectID = tbl_People.ID WHERE tbl_ToPlaces.PlaceID = " + index_.ToString() + " AND tbl_ToPlaces.TypeID = 1 ORDER BY tbl_People.Born;";
            sqlCommand = new OleDbCommand(sql, database_.cndb);
            dataReader = sqlCommand.ExecuteReader();
            isFirst = true;
            while (dataReader.Read())
            {
                if (isFirst)
                {
                    isFirst = false;
                    html.Append("<td>");
                    html.Append("<span class=\"Background\">People</span>");
                    html.Append("<table cellpadding=\"3\" cellspacing=\"2\">");
                }
                html.Append("<tr bgcolor=\"silver\"><td>");
                html.Append("<span class=\"Small\">");
                html.Append("<a href=\"person:" + dataReader.GetInt32(0).ToString() + "\">");
                if (!dataReader.IsDBNull(1))
                {
                    html.Append(dataReader.GetString(1));
                    html.Append(" ");
                }
                if (dataReader.IsDBNull(2))
                {
                    if (dataReader.IsDBNull(3))
                    {
                    }
                    else
                    {
                        html.Append(dataReader.GetString(3));
                    }
                }
                else
                {
                    html.Append(dataReader.GetString(2));
                }
                html.Append("</a>");
                if (dataReader.IsDBNull(4))
                {
                    html.Append(" (<i>Error</i>");
                }
                else
                {
                    DateTime dtBorn = dataReader.GetDateTime(4);
                    html.Append(" (" + dtBorn.Year.ToString() + "-");
                }
                if (!dataReader.IsDBNull(5))
                {
                    html.Append(dataReader.GetDateTime(5).Year.ToString());
                }
                html.Append(")");
                html.Append("</span>");
                html.AppendLine("</td></tr>");
            }
            if (!isFirst)
            {
                html.AppendLine("</table>");
                html.Append("</td>");
            }
            dataReader.Close();

            // Show the sources with a connection to this place.
            sql = "SELECT tbl_Sources.ID, tbl_Sources.Name " +
                "FROM tbl_ToPlaces INNER JOIN tbl_Sources ON tbl_ToPlaces.ObjectID = tbl_Sources.ID " +
                "WHERE (((tbl_ToPlaces.PlaceID)=" + index_.ToString() + ") AND ((tbl_ToPlaces.TypeID)=2)) " +
                "ORDER BY tbl_Sources.Name;";
            sqlCommand = new OleDbCommand(sql, database_.cndb);
            dataReader = sqlCommand.ExecuteReader();
            isFirst = true;
            while (dataReader.Read())
            {
                if (isFirst)
                {
                    isFirst = false;
                    html.Append("<td>");
                    html.Append("<span class=\"Background\">Sources</span>");
                    html.Append("<table cellpadding=\"3\" cellspacing=\"2\">");
                }
                html.Append("<tr bgcolor=\"silver\"><td>");
                html.Append("<span class=\"Small\">");
                html.Append("<a href=\"source:" + dataReader.GetInt32(0).ToString() + "\">");
                html.Append(dataReader.GetString(1));
                html.Append("</a>");
                html.Append("</span>");
                html.Append("</td></tr>");
            }
            if (!isFirst)
            {
                html.Append("</table>");
                html.Append("</td>");
            }
            dataReader.Close();

            html.Append("</tr>\n</table>\n");

            // Return the string built.
            return html.ToString();
        }



        /// <summary>Builds a html script to add the googlemap of the place.</summary>
        /// <returns>A html script to add this place to a web page as a googlemap.</returns>
        public string googleMap(int width, int height)
        {
            StringBuilder html = new StringBuilder();

            html.AppendLine("<script type=\"text/javascript\" src=\"http://www.google.com/jsapi?key=ABQIAAAAELN21ukYS-dXUgY1q2-cYBRi_j0U6kJrkFvY4-OX2XYmEAa76BSTo1rKlErW-r00FyfvS8W-w8OnPg\"></script>");
            html.AppendLine("<script type=\"text/javascript\">");
            html.AppendLine("google.load(\"maps\", \"2.x\");");
            html.AppendLine("// Call this function when the page has been loaded");
            html.AppendLine("function initialize() {");
            html.AppendLine("var map = new google.maps.Map2(document.getElementById(\"map\"));");
            //             sbHtml.AppendLine("map.setCenter(new google.maps.LatLng(37.4419, -122.1419), 10);");

            // Add a zooming control.
            html.AppendLine("map.addControl(new GSmallMapControl());");

            // Add a point to the map.
            html.AppendLine("var point = new google.maps.LatLng(" + latitude.ToString() + "," + longitude.ToString() + ");");
            html.AppendLine("map.setCenter(point," + googleZoom.ToString() + ");");

            // Display a marker at the point.
            html.AppendLine("var marker = new google.maps.Marker(point);");
            html.AppendLine("map.addOverlay(marker);");

            html.AppendLine("}");
            html.AppendLine("google.setOnLoadCallback(initialize);");
            html.AppendLine("</script>");

            html.Append("<div id=\"map\" style=\"width: " + width.ToString() + "px; height: " + height.ToString() + "px\"></div>");

            // Return the string built.
            return html.ToString();
        }



        #endregion
    }
}
