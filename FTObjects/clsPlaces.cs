using System;
using System.Collections.Generic;
using System.Text;

using System.Collections;

namespace FamilyTree.Objects
{
    // Class to represent a collection of places.
    /// <summary>
    /// Class to represent a collection of places.
    /// The original use for this objects was to be able to draw a googlemap with all the places marked on.
    /// </summary>
    public class clsPlaces
    {
        // Collection of places
        /// <summary>
        /// Collection of places
        /// </summary>
        ArrayList m_oPlaces;

        // Class constructor.
        /// <summary>
        /// Class constructor.
        /// </summary>
        public clsPlaces()
        {
            m_oPlaces = new ArrayList();
        }

        // Adds a new place to this collection.
        /// <summary>
        /// Adds a new place to this collection.
        /// </summary>
        /// <param name="oPlace"></param>
        public void AddPlace(clsPlace oPlace)
        {
            // Only add places with different la
            bool bNewPlace = true;
            clsPlace[] oPlaces = (clsPlace[])m_oPlaces.ToArray(typeof(clsPlace));
            foreach(clsPlace oExistingPlace in oPlaces)
            {
                if(oExistingPlace.Latitude == oPlace.Latitude && oExistingPlace.Longitude == oPlace.Longitude)
                {
                    bNewPlace = false;
                }
            }

            if(bNewPlace)
            {
                m_oPlaces.Add(oPlace);
            }
        }

        // Builds a html script to add the googlemap of the place.
        /// <summary>
        /// Builds a html script to add the googlemap of the place.
        /// </summary>
        /// <returns>A html script to add this place to a web page as a googlemap.</returns>
        public string GoogleMap(int nWidth, int nHeight)
        {
            // Check if any place in the collection is a known location.
            bool bAnyKnownPlace = false;
            float dLatitudeMax = 0;
            float dLatitudeMin = 0;
            float dLongitudeMax = 0;
            float dLongitudeMin = 0;
            clsPlace[] oPlaces = (clsPlace[])m_oPlaces.ToArray(typeof(clsPlace));
            foreach(clsPlace oPlace in oPlaces)
            {
                if(!bAnyKnownPlace)
                {
                    bAnyKnownPlace = true;
                    dLatitudeMax = oPlace.Latitude;
                    dLatitudeMin = oPlace.Latitude;
                    dLongitudeMax = oPlace.Longitude;
                    dLongitudeMin = oPlace.Longitude;
                }
                if(oPlace.Latitude > dLatitudeMax)
                {
                    dLatitudeMax = oPlace.Latitude;
                }
                if(oPlace.Latitude < dLatitudeMin)
                {
                    dLatitudeMin = oPlace.Latitude;
                }
                if(oPlace.Longitude > dLongitudeMax)
                {
                    dLongitudeMax = oPlace.Longitude;
                }
                if(oPlace.Longitude < dLongitudeMin)
                {
                    dLongitudeMin = oPlace.Longitude;
                }
            }

            // If nowhere is known then do nothing.
            if(!bAnyKnownPlace)
            {
                return "";
            }

            // Work out the maximum degrees difference
            float dLatitudeRange = 1000 * (dLatitudeMax - dLatitudeMin) / nHeight;
            float dLongitudeRange = 1000 * (dLongitudeMax - dLongitudeMin) / nWidth;
            float dRange = Math.Max(dLatitudeRange, dLongitudeRange);

            // Decide on the zoom factor
            int nGoogleZoom = 1;
            if(dRange < 0.5)
            {
                nGoogleZoom = 10;
                if(Math.Abs((dLongitudeMax + dLongitudeMin) / 2) > 3)
                {
                    // This is not the UK so lets zoom out a bit
                    nGoogleZoom = 4;
                }
            }
            else if(dRange < 2.4)
            {
                nGoogleZoom = 8;
            }
            else if(dRange < 5)
            {
                nGoogleZoom = 7;
            }
            else if(dRange < 6)
            {
                nGoogleZoom = 6;
            }
            else if(dRange < 9)
            {
                nGoogleZoom = 5;
            }

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
            sbHtml.AppendLine("var point = new google.maps.LatLng(" + ((dLatitudeMax + dLatitudeMin) / 2).ToString() + "," + ((dLongitudeMax + dLongitudeMin) / 2).ToString() + ");");
            sbHtml.AppendLine("map.setCenter(point," + nGoogleZoom.ToString() + ");");

            // Display a marker at the each point
            foreach(clsPlace oPlace in oPlaces)
            {
                sbHtml.AppendLine("map.addOverlay(new google.maps.Marker(new google.maps.LatLng(" + oPlace.Latitude.ToString() + "," + oPlace.Longitude.ToString() + ")));");
            }

            sbHtml.AppendLine("}");
            sbHtml.AppendLine("google.setOnLoadCallback(initialize);");
            sbHtml.AppendLine("</script>");

            sbHtml.AppendLine("<div id=\"map\" style=\"width: " + nWidth.ToString() + "px; height: " + nHeight.ToString() + "px\"></div>");

            //// Show the calculations
            //if(nGoogleZoom != 10)
            //{
            //    sbHtml.Append("<p>Latitude Range (height) = " + (dLatitudeMax - dLatitudeMin).ToString("##0.0000") + ", Longitude Range (width) = " + (dLongitudeMax - dLongitudeMin).ToString("##0.0000") + "</p>");
            //    sbHtml.Append("<p>Latitude Range (height) = " + dLatitudeRange.ToString("##0.0000") + ", Longitude Range (width) = " + dLongitudeRange.ToString("##0.0000") + ", Range = " + dRange.ToString("##0.0000") + " zoom = " + nGoogleZoom.ToString() + "</p>");
            //}
            sbHtml.AppendLine("<!--\nLatitude Range (height) = " + (dLatitudeMax - dLatitudeMin).ToString("##0.0000") + ", Longitude Range (width) = " + (dLongitudeMax - dLongitudeMin).ToString("##0.0000"));
            sbHtml.AppendLine("Latitude Range (height) = " + dLatitudeRange.ToString("##0.0000") + ", Longitude Range (width) = " + dLongitudeRange.ToString("##0.0000") + ", Range = " + dRange.ToString("##0.0000") + " zoom = " + nGoogleZoom.ToString() + "\n-->");

            // Return the string built
            return sbHtml.ToString();
        }
    }
}
