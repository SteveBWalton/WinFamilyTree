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
using FamilyTree.Objects;

namespace FamilyTree.Viewer
{
    // Dialog to allow the user edit an existing place.
    /// <summary>
    /// Dialog to allow the user edit an existing place.
    /// </summary>
    public partial class frmEditPlace : Form
    {
        // The place that the dialog is editing.
        /// <summary>
        /// The place that the dialog is editing.
        /// </summary>
        clsPlace m_oPlace;

        // Class constructor
        /// <summary>
        /// Class constructor
        /// </summary>
        public frmEditPlace(int nPlaceID, Database oDb)
        {
            InitializeComponent();

            // Find the name and type of the specified place
            m_oPlace = new clsPlace(nPlaceID, oDb);

            m_labName.Text = m_oPlace.Name;
            m_cboType.SelectedIndex = m_oPlace.Status;
            m_nudLatitude.Value = (decimal)m_oPlace.Latitude;
            m_nudLongitude.Value = (decimal)m_oPlace.Longitude;
            m_nudZoom.Value = (decimal)m_oPlace.GoogleZoom;
            m_chkUseParentLocation.Checked = m_oPlace.UseParentLocation;
            m_txtPrivateComments.Text = m_oPlace.PrivateComments;
        }

        // Message handler for the OK button click event.
        /// <summary>
        /// Message handler for the OK button click event.
        /// The form will close itself just update the place here.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_cmdOK_Click(object sender,EventArgs e)
        {
            GetValues();
            m_oPlace.Save();
        }

        // Load the values off the dialog and into the place object.
        /// <summary>
        /// Load the values off the dialog and into the place object.
        /// </summary>
        private void GetValues()
        {
            m_oPlace.Status = m_cboType.SelectedIndex;
            m_oPlace.Latitude = (float)m_nudLatitude.Value;
            m_oPlace.Longitude = (float)m_nudLongitude.Value;
            m_oPlace.GoogleZoom = (int)m_nudZoom.Value;
            m_oPlace.UseParentLocation = m_chkUseParentLocation.Checked;
            m_oPlace.PrivateComments = m_txtPrivateComments.Text;
        }

        // Message handler for the refresh button click event.
        /// <summary>
        /// Message handler for the refresh button click event.
        /// Redraw the google map of this place.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdRefresh_Click(object sender, EventArgs e)
        {
            GetValues();
            m_webBrowser.DocumentText = "<html>\n<head></head>\n<body bgcolor=#"+(this.BackColor.ToArgb() & 0xFFFFFF).ToString("X000000")+">\n" + m_oPlace.GoogleMap(400,200) + "</body>\n</html>";
        }

        // Message handler for the zoom level changing.
        /// <summary>
        /// Message handler for the zoom level changing.
        /// Change the step size on the longitude and latitude controls.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nudZoom_ValueChanged(object sender, EventArgs e)
        {
            int nZoom=(int)m_nudZoom.Value ;
            switch( nZoom )
            {
            case 1:
                m_nudLatitude.Increment = 5;
                m_nudLongitude.Increment = 5;
                break;
            case 2:
            case 3:
            case 4:
                m_nudLatitude.Increment = 1;
                m_nudLongitude.Increment = 1;
                break;
            case 5:
            case 6:
            case 7:
                m_nudLatitude.Increment = 0.1M;
                m_nudLongitude.Increment =0.1M;
                break;
            case 8:
            case 9:
            case 10:
                m_nudLatitude.Increment = 0.01M;
                m_nudLongitude.Increment =0.01M;
                break;
            case 11:
            case 12:
            case 13:
            case 14:
                m_nudLatitude.Increment =0.001M;
                m_nudLongitude.Increment =0.001M;
                break;
            default:
                m_nudLatitude.Increment =0.0001M;
                m_nudLongitude.Increment = 0.0001M;
                break;

            }
        }

        // Message handler for the left button click.
        /// <summary>
        /// Message handler for the left button click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdLeft_Click(object sender, EventArgs e)
        {
            m_nudLongitude.Value -= m_nudLongitude.Increment;
        }

        // Message handler for the up button click.
        /// <summary>
        /// Message handler for the up button click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdUp_Click(object sender, EventArgs e)
        {
            m_nudLatitude.Value += m_nudLatitude.Increment;
        }

        // Message handler for the down button click.
        /// <summary>
        /// Message handler for the down button click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdDown_Click(object sender, EventArgs e)
        {
            m_nudLatitude.Value -= m_nudLatitude.Increment;
        }

        // Message handler for the right button click.
        /// <summary>
        /// Message handler for the right button click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdRight_Click(object sender, EventArgs e)
        {
            m_nudLongitude.Value += m_nudLongitude.Increment;
        }
    }
}