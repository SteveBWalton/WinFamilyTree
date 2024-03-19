using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

// Database access
using System.Data.OleDb;

// Family tree objects
using family_tree.objects;

namespace family_tree.viewer
{
    /// <summary>Dialog to allow the user edit an existing place.</summary>
    public partial class EditPlaceDialog : Form
    {
        /// <summary>The place that the dialog is editing.</summary>
        Place place_;



        /// <summary>Class constructor.</summary>
        public EditPlaceDialog(int placeIdx, Database database)
        {
            InitializeComponent();

            // Find the name and type of the specified place.
            place_ = new Place(placeIdx, database);

            labName_.Text = place_.name;
            cboType_.SelectedIndex = place_.status;
            nudLatitude_.Value = (decimal)place_.latitude;
            nudLongitude_.Value = (decimal)place_.longitude;
            nudZoom_.Value = (decimal)place_.googleZoom;
            chkUseParentLocation_.Checked = place_.isUseParentLocation;
            txtPrivateComments_.Text = place_.privateComments;
        }



        /// <summary>Message handler for the OK button click event.  The form will close itself just update the place here.</summary>
        private void cmdOkClick(object sender, EventArgs e)
        {
            getValues();
            place_.save();
        }



        /// <summary>Load the values off the dialog and into the place object.</summary>
        private void getValues()
        {
            place_.status = cboType_.SelectedIndex;
            place_.latitude = (float)nudLatitude_.Value;
            place_.longitude = (float)nudLongitude_.Value;
            place_.googleZoom = (int)nudZoom_.Value;
            place_.isUseParentLocation = chkUseParentLocation_.Checked;
            place_.privateComments = txtPrivateComments_.Text;
        }



        /// <summary>Message handler for the refresh button click event.  Redraw the google map of this place.</summary>
        private void cmdRefreshClick(object sender, EventArgs e)
        {
            getValues();
            webBrowser_.DocumentText = "<html>\n<head></head>\n<body bgcolor=#" + (this.BackColor.ToArgb() & 0xFFFFFF).ToString("X000000") + ">\n" + place_.googleMap(400, 200) + "</body>\n</html>";
        }



        /// <summary>Message handler for the zoom level changing.  Change the step size on the longitude and latitude controls.</summary>
        private void nudZoomValueChanged(object sender, EventArgs e)
        {
            int zoom = (int)nudZoom_.Value;
            switch (zoom)
            {
            case 1:
                nudLatitude_.Increment = 5;
                nudLongitude_.Increment = 5;
                break;
            case 2:
            case 3:
            case 4:
                nudLatitude_.Increment = 1;
                nudLongitude_.Increment = 1;
                break;
            case 5:
            case 6:
            case 7:
                nudLatitude_.Increment = 0.1M;
                nudLongitude_.Increment = 0.1M;
                break;
            case 8:
            case 9:
            case 10:
                nudLatitude_.Increment = 0.01M;
                nudLongitude_.Increment = 0.01M;
                break;
            case 11:
            case 12:
            case 13:
            case 14:
                nudLatitude_.Increment = 0.001M;
                nudLongitude_.Increment = 0.001M;
                break;
            default:
                nudLatitude_.Increment = 0.0001M;
                nudLongitude_.Increment = 0.0001M;
                break;

            }
        }



        /// <summary>Message handler for the left button click.</summary>
        private void cmdLeftClick(object sender, EventArgs e)
        {
            nudLongitude_.Value -= nudLongitude_.Increment;
        }



        /// <summary>Message handler for the up button click.</summary>
        private void cmdUpClick(object sender, EventArgs e)
        {
            nudLatitude_.Value += nudLatitude_.Increment;
        }



        /// <summary>Message handler for the down button click.</summary>
        private void cmdDownClick(object sender, EventArgs e)
        {
            nudLatitude_.Value -= nudLatitude_.Increment;
        }



        /// <summary>Message handler for the right button click.</summary>
        private void cmdRightClick(object sender, EventArgs e)
        {
            nudLongitude_.Value += nudLongitude_.Increment;
        }
    }
}