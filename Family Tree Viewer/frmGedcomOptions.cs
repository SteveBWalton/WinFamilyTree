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
        private clsGedcomOptions m_oOptions;

        public frmGedcomOptions(clsGedcomOptions oOptions)
        {
            InitializeComponent();

            // Save the Gedcom options object
            m_oOptions = oOptions;
        }

        private void PopulateForm()
        {
            m_txtFilename.Text = m_oOptions.fileName;
        }



        /// <summary>Update the options from the values on the form.</summary>
        private void populateOptions()
        {
            m_oOptions.fileName = m_txtFilename.Text;
            m_oOptions.IncludePGVU = m_chkPGVU.Checked;
            m_oOptions.RemoveADDRfromPLAC = m_chkRemoveAddresses.Checked;
            m_oOptions.UseADDR = m_chkUseADDR.Checked;
            m_oOptions.UseCTRY = m_chkUseCTRY.Checked;
            m_oOptions.UseLongitude = m_chkLongitude.Checked;
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
            m_SaveFileDialog.Title = "Select output file";
            m_SaveFileDialog.Filter = "Gedcom Files (*.ged)|*.ged";
            m_SaveFileDialog.FilterIndex = 1;
            m_SaveFileDialog.FileName = m_txtFilename.Text;

            // Allow the user to select the output file
            if(m_SaveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                m_txtFilename.Text = m_SaveFileDialog.FileName;
            }
        }

        private void frmGedcomOptions_Load(object sender, EventArgs e)
        {
            PopulateForm();
            m_cboScheme.SelectedIndex = 1;
        }

        private void m_cboScheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(m_cboScheme.SelectedIndex)
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