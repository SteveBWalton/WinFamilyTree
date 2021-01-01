using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

// clsGedcomOptions
using FamilyTree.Objects;

namespace FamilyTree.Viewer
{
    public partial class frmGedcomOptions : Form
    {
        private GedcomOptions options_;

        public frmGedcomOptions(GedcomOptions options)
        {
            InitializeComponent();

            // Save the Gedcom options object
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
            options_.isIncludePGVU = m_chkPGVU.Checked;
            options_.isRemoveADDRfromPLAC = m_chkRemoveAddresses.Checked;
            options_.isUseADDR = m_chkUseADDR.Checked;
            options_.isUseCTRY = m_chkUseCTRY.Checked;
            options_.isUseLongitude = m_chkLongitude.Checked;
        }



        private void cmdOK_Click(object sender, EventArgs e)
        {
            populateOptions();
        }



        /// <summary>Message handler for the "Open" button click.</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdOpen_Click(object sender, EventArgs e)
        {
            // Initialise the select save file dialog
            saveFileDialog_.Title = "Select output file";
            saveFileDialog_.Filter = "Gedcom Files (*.ged)|*.ged";
            saveFileDialog_.FilterIndex = 1;
            saveFileDialog_.FileName = txtFilename_.Text;

            // Allow the user to select the output file
            if(saveFileDialog_.ShowDialog(this) == DialogResult.OK)
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
            case 0:// Custom do nothing
                break;

            case 1:// Neutral
                m_chkPGVU.Checked = false;
                m_chkRemoveAddresses.Checked = false;
                m_chkUseADDR.Checked = false;
                m_chkUseCTRY.Checked = false;
                m_chkLongitude.Checked = false;
                break;
            
            case 2:// Php GedView
                m_chkPGVU.Checked = true;
                m_chkRemoveAddresses.Checked = true;
                m_chkUseADDR.Checked = true;
                m_chkUseCTRY.Checked = true;
                m_chkLongitude.Checked = false;
                break;

            case 3:// Gramps
                m_chkPGVU.Checked = false;
                m_chkRemoveAddresses.Checked = false;
                m_chkUseADDR.Checked = false;
                m_chkUseCTRY.Checked = false;
                m_chkLongitude.Checked = true;
                break;
            }
        }
    }
}