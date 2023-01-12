using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

// clsGedcomOptions
using family_tree.objects;

namespace family_tree.viewer
{
    public partial class GedcomOptionsDialog : Form
    {
        private GedcomOptions options_;

        public GedcomOptionsDialog(GedcomOptions options)
        {
            InitializeComponent();

            // Save the Gedcom options object.
            options_ = options;
        }



        private void populateForm()
        {
            txtFilename_.Text = options_.fileName;
        }



        /// <summary>Update the options from the values on the form.</summary>
        private void populateOptions()
        {
            options_.fileName = txtFilename_.Text;
            options_.isIncludePGVU = chkPgvu_.Checked;
            options_.isRemoveADDRfromPLAC = chkRemoveAddresses_.Checked;
            options_.isUseADDR = chkUseAddr_.Checked;
            options_.isUseCTRY = chkUseCtry_.Checked;
            options_.isUseLongitude = chkLongitude_.Checked;
            options_.isAllElements = checkboxIncludeEverything_.Checked;
        }



        private void cmdOK_Click(object sender, EventArgs e)
        {
            populateOptions();
        }



        /// <summary>Message handler for the "Open" button click.</summary>
        private void cmdOpen_Click(object sender, EventArgs e)
        {
            // Initialise the select save file dialog
            saveFileDialog_.Title = "Select output file";
            saveFileDialog_.Filter = "Gedcom Files (*.ged)|*.ged";
            saveFileDialog_.FilterIndex = 1;
            saveFileDialog_.FileName = txtFilename_.Text;

            // Allow the user to select the output file
            if (saveFileDialog_.ShowDialog(this) == DialogResult.OK)
            {
                txtFilename_.Text = saveFileDialog_.FileName;
            }
        }



        private void frmGedcomOptions_Load(object sender, EventArgs e)
        {
            populateForm();
            cboScheme_.SelectedIndex = 1;
        }



        private void cboScheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(cboScheme_.SelectedIndex)
            {
            case 0:// Custom do nothing.
                break;

            case 1:// Neutral.
                chkPgvu_.Checked = false;
                chkRemoveAddresses_.Checked = false;
                chkUseAddr_.Checked = false;
                chkUseCtry_.Checked = false;
                chkLongitude_.Checked = false;
                checkboxIncludeEverything_.Checked = false;

                break;
            
            case 2:// Php GedView.
                chkPgvu_.Checked = true;
                chkRemoveAddresses_.Checked = true;
                chkUseAddr_.Checked = true;
                chkUseCtry_.Checked = true;
                chkLongitude_.Checked = false;
                checkboxIncludeEverything_.Checked = false;

                break;

            case 3:// Gramps.
                chkPgvu_.Checked = false;
                chkRemoveAddresses_.Checked = false;
                chkUseAddr_.Checked = false;
                chkUseCtry_.Checked = false;
                chkLongitude_.Checked = true;
                checkboxIncludeEverything_.Checked = false;
                break;

            case 4://gedcom-py
                chkPgvu_.Checked = true;
                chkRemoveAddresses_.Checked = true;
                chkUseAddr_.Checked = true;
                chkUseCtry_.Checked = true;
                chkLongitude_.Checked = true;
                checkboxIncludeEverything_.Checked = true;
                break;
            }
        }
    }
}