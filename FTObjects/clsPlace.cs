using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.OleDb;

namespace FamilyTree.Objects
{
    /// <summary>
    /// Class to represent a place in the database.
    /// </summary>
    public class clsPlace
    {
        #region Member Variables

        /// <summary>Database that this place is attached to.</summary>
        private clsDatabase m_oDb;

        /// <summary>ID of this place in the database.</summary>
        private int m_nID;

        /// <summary>Name of this place.</summary>
        private string m_sName;

        /// <summary>ID of the parent place for this place.  Zero indicates top level, no parent.</summary>
        private int m_nParentID;

        /// <summary>The status of this place.  0 - Place, 1 - Address.</summary>
        private int m_nStatus;

        // The degrees to the east.
        /// <summary>
        /// The degrees to the east.
        /// </summary>
        private float m_dLongitude;

        // The degrees to the north.
        /// <summary>
        /// The degrees to the north.
        /// </summary>
        private float m_dLatitude;

        // The zoom level to use when displaying on google maps.
        /// <summary>
        /// The zoom level to use when displaying on google maps.
        /// </summary>
        private int m_nGoogleZoom;

        // True to use the longitude and latitude of the parent location.
        /// <summary>
        /// True to use the longitude and latitude of the parent location.
        /// </summary>
        private bool m_bUseParentLocation;

        // Comments about this location.
        /// <summary>
        /// Comments about this location.
        /// </summary>
        private string m_sPrivateComments;

        #endregion

        #region Constructors and Database

        public clsPlace(OleDbDataReader drPlace,clsDatabase oDb)
        {
            m_oDb = oDb;            
            Read(drPlace);            
        }

        // Class constructor to load a place from the database.
        /// <summary>
        /// Class constructor to load a place from the database.
        /// If the place is not found in the database then a new object is created.
        /// </summary>
        /// <param name="nID">Specifies the ID of the place.</param>
        /// <param name="oDb">Specifies the database that contains the place.</param>
        public clsPlace(int nID, clsDatabase oDb)
        {
            m_oDb = oDb;
            m_nID = nID;

            string sSql = "SELECT ID,Name,ParentID,Status,Longitude,Latitude,GoogleZoom,UseParentLocation,PrivateComments FROM tbl_Places WHERE ID=" + m_nID.ToString() + ";";
            OleDbCommand oSql = new OleDbCommand(sSql, oDb.cnDB);
            OleDbDataReader drPlace = oSql.ExecuteReader();
            if(drPlace.Read())
            {
                Read(drPlace);
            }
            else
            {
                m_sName = "";
                m_nParentID = 0;
                m_nStatus = 0;
                m_dLongitude = -999;
                m_dLatitude = -999;
                m_nGoogleZoom = 10;
                m_sPrivateComments = string.Empty;
                m_bUseParentLocation = true;
            }
            drPlace.Close();
        }

        // Saves the place object into the database.
        /// <summary>
        /// Saves the place object into the database.
        /// Will only update an existing place record.
        /// To add new places use AddPlace().
        /// </summary>
        /// <returns>True for success, false otherwise.</returns>
        public bool Save()
        {
            // Update the tbl_Places table
            if(m_nID > 0)
            {
                StringBuilder sbSql = new StringBuilder("UPDATE tbl_Places SET ");
                sbSql.Append("Status=" + m_nStatus.ToString() + ",");
                sbSql.Append("Longitude=" + m_dLongitude.ToString() + ",");
                sbSql.Append("Latitude=" + m_dLatitude.ToString() + ",");
                sbSql.Append("GoogleZoom=" + m_nGoogleZoom.ToString() + ",");
                sbSql.Append("UseParentLocation=" + Innoval.clsDatabase.ToDb(m_bUseParentLocation) + ",");
                sbSql.Append("PrivateComments=" + Innoval.clsDatabase.ToDb(m_sPrivateComments) + " ");
                sbSql.Append("WHERE ID=" + m_nID.ToString() + ";");
                OleDbCommand oSql = new OleDbCommand(sbSql.ToString(), m_oDb.cnDB);
                oSql.ExecuteNonQuery();
            }

            // Return success
            return true;
        }

        private void Read(OleDbDataReader drPlace)
        {
            m_nID = clsDatabase.GetInt(drPlace, "ID", 0);
            m_sName = clsDatabase.GetString(drPlace, "Name", "Error");
            m_nParentID = clsDatabase.GetInt(drPlace, "ParentID", 0);
            m_nStatus = clsDatabase.GetInt(drPlace, "Status", 0);
            m_dLongitude = Innoval.clsDatabase.GetFloat(drPlace, "Longitude", -999);
            m_dLatitude = Innoval.clsDatabase.GetFloat(drPlace, "Latitude", -999);
            m_nGoogleZoom = Innoval.clsDatabase.GetInt(drPlace, "GoogleZoom", 10);
            m_sPrivateComments = Innoval.clsDatabase.GetString(drPlace, "PrivateComments", string.Empty);
            m_bUseParentLocation = Innoval.clsDatabase.GetBool(drPlace, "UseParentLocation", true);
        }
        #endregion

        #region Public Properties

        // The ID of this place in the database.
        /// <summary>
        /// The ID of this place in the database.
        /// </summary>
        public int ID { get { return m_nID; } }

        // The short name of this place.
        /// <summary>
        /// The short name of this place.
        /// </summary>
        public string Name { get { return m_sName; } }

        // The ID of the parent place.
        /// <summary>
        /// The ID of the parent place.
        /// Zero is top level and no parent.
        /// </summary>
        public int ParentID
        {
            get
            {
                return m_nParentID;
            }
        }

        // The parent place of this place.
        /// <summary>
        /// The parent place of this place.
        /// </summary>
        public clsPlace Parent
        {
            get
            {
                if(m_nParentID == 0)
                {
                    return null;
                }
                return new clsPlace(m_nParentID, m_oDb);
            }
        }

        // The status of this place.  0 - Place, 1 - Address.
        /// <summary>
        /// The status of this place.  0 - Place, 1 - Address.
        /// </summary>
        public int Status
        {
            get
            {
                return m_nStatus;
            }
            set
            {
                m_nStatus = value;
            }
        }

        // The longitude of this place in degrees to the east.
        /// <summary>
        /// The longitude of this place in degrees to the east.
        /// </summary>
        public float Longitude
        {
            get
            {
                if(m_bUseParentLocation)
                {
                    if(m_nParentID != 0)
                    {
                        return Parent.Longitude;
                    }
                    return 0;
                }
                return m_dLongitude;
            }
            set
            {
                m_dLongitude = value;
            }
        }

        // The latitude of this place in degrees to the north.
        /// <summary>
        /// The latitude of this place in degrees to the north.
        /// </summary>
        public float Latitude
        {
            get
            {
                if(m_bUseParentLocation)
                {
                    if(m_nParentID != 0)
                    {
                        return Parent.Latitude;
                    }
                    return 0;
                }
                return m_dLatitude;
            }
            set
            {
                m_dLatitude = value;
            }
        }

        // The zoom to use on a google map of this place.
        /// <summary>
        /// The zoom to use on a google map of this place.
        /// </summary>
        public int GoogleZoom
        {
            get
            {
                if(m_bUseParentLocation)
                {
                    if(m_nParentID != 0)
                    {
                        return Parent.GoogleZoom;
                    }
                    return 1;
                }
                return m_nGoogleZoom;
            }
            set
            {
                m_nGoogleZoom = value;
            }
        }

        // True to use the longitude and latitude of the parent location.
        /// <summary>
        /// True to use the longitude and latitude of the parent location.
        /// </summary>
        public bool UseParentLocation
        {
            get
            {
                return m_bUseParentLocation;
            }
            set
            {
                m_bUseParentLocation = value;
            }
        }

        // The private comments attached to this location.
        /// <summary>
        /// The private comments attached to this location.
        /// </summary>
        public string PrivateComments
        {
            get
            {
                return m_sPrivateComments;
            }
            set
            {
                m_sPrivateComments = value;
            }
        }

        // Returns a html description of the place.
        /// <summary>
        /// Returns a html description of the place.
        /// </summary>
        /// <returns>A description of the place in html format.</returns>
        public string ToHtml(bool bGoogleMap)
        {
            // Build a html description of the place
            StringBuilder sbHtml = new StringBuilder();

            // Build a name for the place.
            StringBuilder sbName = new StringBuilder();
            int nParentID = m_nParentID;
            while(nParentID != 0)
            {
                clsPlace oParent = new clsPlace(nParentID, m_oDb);
                sbName.Insert(0, "<a href=\"place:" + nParentID.ToString() + "\">" + oParent.Name + "</a>, ");
                nParentID = oParent.ParentID;
            }
            sbName.Insert(0, "<a href=\"place:0\">Top Level</a>, ");

            sbHtml.Append("<h1>");
            sbHtml.Append(sbName);
            sbHtml.Append(m_sName);
            sbHtml.Append("<span class=\"Small\"> (");
            switch(m_nStatus)
            {
            case 0:
                sbHtml.Append("Place");
                break;
            case 1:
                sbHtml.Append("Address");
                break;
            }
            sbHtml.Append(")</span>");
            sbHtml.AppendLine("</h1>");

            // Display the private comments
            if(m_sPrivateComments != string.Empty)
            {
                sbHtml.AppendLine("<p><strong>Private comments</strong>: " + m_sPrivateComments + "</p>");
            }

            // Add a goggle map of the place
            if(bGoogleMap)
            {
                sbHtml.AppendLine(GoogleMap(400, 200));
            }

            sbHtml.AppendLine("<table border=\"0\">\n<tr valign=\"top\">");

            // Show the child places from this place.
            string sSql = "SELECT ID,Name,Status FROM tbl_Places WHERE ParentID=" + m_nID.ToString() + " ORDER BY Status, Name;";
            OleDbCommand oSql = new OleDbCommand(sSql, m_oDb.cnDB);
            OleDbDataReader drChildren = oSql.ExecuteReader();
            bool bFirst = true;
            while(drChildren.Read())
            {
                if(bFirst)
                {
                    bFirst = false;
                    sbHtml.Append("<td>");
                    sbHtml.Append("<span class=\"Background\">Child Locations</span>");
                    sbHtml.Append("<table cellpadding=\"3\" cellspacing=\"2\">");
                }
                sbHtml.Append("<tr bgcolor=\"silver\"><td>");
                sbHtml.Append("<span class=\"Small\">");
                sbHtml.Append("<a href=\"place:" + drChildren.GetInt32(0).ToString() + "\">");
                sbHtml.Append(drChildren.GetString(1));
                sbHtml.Append("</a>");
                if(clsDatabase.GetInt(drChildren, "Status", 0) == 0)
                {
                    sbHtml.Append(" (Place)");
                }
                else
                {
                    sbHtml.Append(" (Address)");
                }

                sbHtml.Append("</span>");
                sbHtml.AppendLine("</td></tr>");
            }
            if(!bFirst)
            {
                sbHtml.AppendLine("</table>");
                sbHtml.Append("</td>");
            }
            drChildren.Close();

            // Show the people with a connection to this place
            sSql = "SELECT tbl_ToPlaces.ObjectID, tbl_People.Forenames, tbl_People.MaidenName, tbl_People.Surname, tbl_People.Born, tbl_People.Died " +
                "FROM tbl_ToPlaces INNER JOIN tbl_People ON tbl_ToPlaces.ObjectID = tbl_People.ID " +
                "WHERE (((tbl_ToPlaces.PlaceID)=" + m_nID.ToString() + ") AND ((tbl_ToPlaces.TypeID)=1)) " +
                "ORDER BY tbl_People.Born;";
            oSql = new OleDbCommand(sSql, m_oDb.cnDB);
            OleDbDataReader drPeople = oSql.ExecuteReader();
            bFirst = true;
            while(drPeople.Read())
            {
                if(bFirst)
                {
                    bFirst = false;
                    sbHtml.Append("<td>");
                    sbHtml.Append("<span class=\"Background\">People</span>");
                    sbHtml.Append("<table cellpadding=\"3\" cellspacing=\"2\">");
                }
                sbHtml.Append("<tr bgcolor=\"silver\"><td>");
                sbHtml.Append("<span class=\"Small\">");
                sbHtml.Append("<a href=\"person:" + drPeople.GetInt32(0).ToString() + "\">");
                if(!drPeople.IsDBNull(1))
                {
                    sbHtml.Append(drPeople.GetString(1));
                    sbHtml.Append(" ");
                }
                if(drPeople.IsDBNull(2))
                {
                    if(drPeople.IsDBNull(3))
                    {
                    }
                    else
                    {
                        sbHtml.Append(drPeople.GetString(3));
                    }
                }
                else
                {
                    sbHtml.Append(drPeople.GetString(2));
                }
                sbHtml.Append("</a>");
                if(drPeople.IsDBNull(4))
                {
                    sbHtml.Append(" (<i>Error</i>");
                }
                else
                {
                    DateTime dtBorn = drPeople.GetDateTime(4);
                    sbHtml.Append(" (" + dtBorn.Year.ToString() + "-");
                }
                if(!drPeople.IsDBNull(5))
                {
                    sbHtml.Append(drPeople.GetDateTime(5).Year.ToString());
                }
                sbHtml.Append(")");
                sbHtml.Append("</span>");
                sbHtml.AppendLine("</td></tr>");
            }
            if(!bFirst)
            {
                sbHtml.AppendLine("</table>");
                sbHtml.Append("</td>");
            }
            drPeople.Close();

            // Show the sources with a connection to this place
            sSql = "SELECT tbl_Sources.ID, tbl_Sources.Name " +
                "FROM tbl_ToPlaces INNER JOIN tbl_Sources ON tbl_ToPlaces.ObjectID = tbl_Sources.ID " +
                "WHERE (((tbl_ToPlaces.PlaceID)=" + m_nID.ToString() + ") AND ((tbl_ToPlaces.TypeID)=2)) " +
                "ORDER BY tbl_Sources.Name;";
            oSql = new OleDbCommand(sSql, m_oDb.cnDB);
            OleDbDataReader drSources = oSql.ExecuteReader();
            bFirst = true;
            while(drSources.Read())
            {
                if(bFirst)
                {
                    bFirst = false;
                    sbHtml.Append("<td>");
                    sbHtml.Append("<span class=\"Background\">Sources</span>");
                    sbHtml.Append("<table cellpadding=\"3\" cellspacing=\"2\">");
                }
                sbHtml.Append("<tr bgcolor=\"silver\"><td>");
                sbHtml.Append("<span class=\"Small\">");
                sbHtml.Append("<a href=\"source:" + drSources.GetInt32(0).ToString() + "\">");
                sbHtml.Append(drSources.GetString(1));
                sbHtml.Append("</a>");
                sbHtml.Append("</span>");
                sbHtml.Append("</td></tr>");
            }
            if(!bFirst)
            {
                sbHtml.Append("</table>");
                sbHtml.Append("</td>");
            }
            drSources.Close();

            sbHtml.Append("</tr>\n</table>\n");

            // Return the string built
            return sbHtml.ToString();
        }

        // Builds a html script to add the googlemap of the place.
        /// <summary>
        /// Builds a html script to add the googlemap of the place.
        /// </summary>
        /// <returns>A html script to add this place to a web page as a googlemap.</returns>
        public string GoogleMap(int nWidth, int nHeight)
        {
            StringBuilder sbHtml = new StringBuilder();

            sbHtml.AppendLine("<script type=\"text/javascript\" src=\"http://www.google.com/jsapi?key=ABQIAAAAELN21ukYS-dXUgY1q2-cYBRi_j0U6kJrkFvY4-OX2XYmEAa76BSTo1rKlErW-r00FyfvS8W-w8OnPg\"></script>");
            sbHtml.AppendLine("<script type=\"text/javascript\">");
            sbHtml.AppendLine("google.load(\"maps\", \"2.x\");");
            sbHtml.AppendLine("// Call this function when the page has been loaded");
            sbHtml.AppendLine("function initialize() {");
            sbHtml.AppendLine("var map = new google.maps.Map2(document.getElementById(\"map\"));");
            //             sbHtml.AppendLine("map.setCenter(new google.maps.LatLng(37.4419, -122.1419), 10);");

            // Add a zooming control
            sbHtml.AppendLine("map.addControl(new GSmallMapControl());");

            // Add a point to the map
            sbHtml.AppendLine("var point = new google.maps.LatLng(" + Latitude.ToString() + "," + Longitude.ToString() + ");");
            sbHtml.AppendLine("map.setCenter(point," + GoogleZoom.ToString() + ");");

            // Display a marker at the point
            sbHtml.AppendLine("var marker = new google.maps.Marker(point);");
            sbHtml.AppendLine("map.addOverlay(marker);");

            sbHtml.AppendLine("}");
            sbHtml.AppendLine("google.setOnLoadCallback(initialize);");
            sbHtml.AppendLine("</script>");

            sbHtml.Append("<div id=\"map\" style=\"width: " + nWidth.ToString() + "px; height: " + nHeight.ToString() + "px\"></div>");

            // Return the string built
            return sbHtml.ToString();
        }

        #endregion
    }
}
