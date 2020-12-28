using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FamilyTree.Viewer
{
    /// <summary>
    /// Form to allow the range of possible birthdays to be estimated from a collection of known ages on known dates.
    /// </summary>
    public partial class frmBirthday : Form
    {
        #region Constructors etc...

        /// <summary>
        /// Empty class constructor.
        /// </summary>
        public frmBirthday()
        {
            InitializeComponent();
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Message handler for any control that wants to recalcule the age if it's value changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CalculateBirthday(object sender,EventArgs e)
        {
            // Initialise a couple variables to hold the result
            DateTime dtMin = DateTime.MinValue;
            DateTime dtMax = DateTime.MaxValue;
            int nSourceMin = 0;
            int nSourceMax = 0;

            if(m_nudAge1.Value > 0)
            {
                DateTime dtDate = m_dtpDate1.Value.AddYears((int)-m_nudAge1.Value);
                if(dtDate < dtMax)
                {
                    dtMax = dtDate;
                    nSourceMax = 1;
                }
                dtDate = dtDate.AddYears(-1).AddDays(1);
                if(dtDate > dtMin)
                {
                    dtMin = dtDate;
                    nSourceMin = 1;
                }
            }

            if(m_nudAge2.Value > 0)
            {
                DateTime dtDate = m_dtpDate2.Value.AddYears((int)-m_nudAge2.Value);
                if(dtDate < dtMax)
                {
                    dtMax = dtDate;
                    nSourceMax = 2;
                }
                dtDate = dtDate.AddYears(-1).AddDays(1);
                if(dtDate > dtMin)
                {
                    dtMin = dtDate;
                    nSourceMin = 2;
                }
            }

            if(m_nudAge3.Value > 0)
            {
                DateTime dtDate = m_dtpDate3.Value.AddYears((int)-m_nudAge3.Value);
                if(dtDate < dtMax)
                {
                    dtMax = dtDate;
                    nSourceMax = 3;
                }
                dtDate = dtDate.AddYears(-1).AddDays(1);
                if(dtDate > dtMin)
                {
                    dtMin = dtDate;
                    nSourceMin = 3;
                }
            }
            
            // Display the result
            m_labReport.Text = "The birthday must be between " + dtMin.ToString("d MMMM yyyy") + " and " + dtMax.ToString("d MMMM yyyy");

            if(nSourceMin == 1 || nSourceMax == 1)
            {
                m_picTick1.Visible = true;
            }
            else
            {
                m_picTick1.Visible = false;
            }
            if(nSourceMin == 2 || nSourceMax == 2)
            {
                m_picTick2.Visible = true;
            }
            else
            {
                m_picTick2.Visible = false;
            }
            if(nSourceMin == 3 || nSourceMax == 3)
            {
                m_picTick3.Visible = true;
            }
            else
            {
                m_picTick3.Visible = false;
            }
        }

        /// <summary>
        /// Message handler for the Copy button click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdCopy_Click(object sender,EventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(m_labReport.Text);
        }

        #endregion

    }
}