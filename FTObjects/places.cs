using System;
using System.Collections.Generic;
using System.Text;

using System.Collections;

namespace family_tree.objects
{
    /// <summary>Class to represent a collection of places.  The original use for this objects was to be able to draw a googlemap with all the places marked on.</summary>
    public class Places
    {
        /// <summary>Collection of places.</summary>
        ArrayList places_;

        /// <summary>Class constructor.</summary>
        public Places()
        {
            places_ = new ArrayList();
        }



        /// <summary>Adds a new place to this collection.</summary>
        /// <param name="place">Specifies the place to add to the collection.</param>
        public void addPlace(Place place)
        {
            // Only add places with different latitude and longitude.
            bool isNewPlace = true;
            Place[] places = (Place[])places_.ToArray(typeof(Place));
            foreach (Place existingPlace in places)
            {
                if (existingPlace.latitude == place.latitude && existingPlace.longitude == place.longitude)
                {
                    isNewPlace = false;
                }
            }

            if (isNewPlace)
            {
                places_.Add(place);
            }
        }



        /// <summary>Builds a html script to add the googlemap of the place.</summary>
        /// <returns>A html script to add this place to a web page as a googlemap.</returns>
        public string googleMap(int width, int height)
        {
            // Check if any place in the collection is a known location.
            bool isAnyKnownPlace = false;
            float latitudeMax = 0;
            float latitudeMin = 0;
            float longitudeMax = 0;
            float longitudeMin = 0;
            Place[] places = (Place[])places_.ToArray(typeof(Place));
            foreach (Place place in places)
            {
                if (!isAnyKnownPlace)
                {
                    isAnyKnownPlace = true;
                    latitudeMax = place.latitude;
                    latitudeMin = place.latitude;
                    longitudeMax = place.longitude;
                    longitudeMin = place.longitude;
                }
                if (place.latitude > latitudeMax)
                {
                    latitudeMax = place.latitude;
                }
                if (place.latitude < latitudeMin)
                {
                    latitudeMin = place.latitude;
                }
                if (place.longitude > longitudeMax)
                {
                    longitudeMax = place.longitude;
                }
                if (place.longitude < longitudeMin)
                {
                    longitudeMin = place.longitude;
                }
            }

            // If nowhere is known then do nothing.
            if (!isAnyKnownPlace)
            {
                return "";
            }

            // Work out the maximum degrees difference.
            float latitudeRange = 1000 * (latitudeMax - latitudeMin) / height;
            float longitudeRange = 1000 * (longitudeMax - longitudeMin) / width;
            float maxRange = Math.Max(latitudeRange, longitudeRange);

            // Decide on the zoom factor.
            int googleZoom = 1;
            if (maxRange < 0.5)
            {
                googleZoom = 10;
                if (Math.Abs((longitudeMax + longitudeMin) / 2) > 3)
                {
                    // This is not the UK so lets zoom out a bit.
                    googleZoom = 4;
                }
            }
            else if (maxRange < 2.4)
            {
                googleZoom = 8;
            }
            else if (maxRange < 5)
            {
                googleZoom = 7;
            }
            else if (maxRange < 6)
            {
                googleZoom = 6;
            }
            else if (maxRange < 9)
            {
                googleZoom = 5;
            }

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
            html.AppendLine("var point = new google.maps.LatLng(" + ((latitudeMax + latitudeMin) / 2).ToString() + "," + ((longitudeMax + longitudeMin) / 2).ToString() + ");");
            html.AppendLine("map.setCenter(point," + googleZoom.ToString() + ");");

            // Display a marker at the each point.
            foreach (Place place in places)
            {
                html.AppendLine("map.addOverlay(new google.maps.Marker(new google.maps.LatLng(" + place.latitude.ToString() + "," + place.longitude.ToString() + ")));");
            }

            html.AppendLine("}");
            html.AppendLine("google.setOnLoadCallback(initialize);");
            html.AppendLine("</script>");

            html.AppendLine("<div id=\"map\" style=\"width: " + width.ToString() + "px; height: " + height.ToString() + "px\"></div>");

            //// Show the calculations.
            //if(nGoogleZoom != 10)
            //{
            //    sbHtml.Append("<p>Latitude Range (height) = " + (dLatitudeMax - dLatitudeMin).ToString("##0.0000") + ", Longitude Range (width) = " + (dLongitudeMax - dLongitudeMin).ToString("##0.0000") + "</p>");
            //    sbHtml.Append("<p>Latitude Range (height) = " + dLatitudeRange.ToString("##0.0000") + ", Longitude Range (width) = " + dLongitudeRange.ToString("##0.0000") + ", Range = " + dRange.ToString("##0.0000") + " zoom = " + nGoogleZoom.ToString() + "</p>");
            //}
            html.AppendLine("<!--\nLatitude Range (height) = " + (latitudeMax - latitudeMin).ToString("##0.0000") + ", Longitude Range (width) = " + (longitudeMax - longitudeMin).ToString("##0.0000"));
            html.AppendLine("Latitude Range (height) = " + latitudeRange.ToString("##0.0000") + ", Longitude Range (width) = " + longitudeRange.ToString("##0.0000") + ", Range = " + maxRange.ToString("##0.0000") + " zoom = " + googleZoom.ToString() + "\n-->");

            // Return the string built.
            return html.ToString();
        }
    }
}
