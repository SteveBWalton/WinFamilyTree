using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace family_tree.viewer
{
    /// <summary>Class to represent a dialog to allow the range of possible birthdays to be estimated from a collection of known ages on known dates.</summary>
    public partial class BirthdayDialog : Form
    {
        #region Constructors etc...



        /// <summary>Empty class constructor.</summary>
        public BirthdayDialog()
        {
            InitializeComponent();
        }



        #endregion

        #region Event Handlers

        /// <summary>Message handler for any control that wants to recalcule the age if it's value changes.</summary>
        private void CalculateBirthday(object sender, EventArgs e)
        {
            // Initialise a couple variables to hold the result.
            DateTime minDate = DateTime.MinValue;
            DateTime maxDate = DateTime.MaxValue;
            int minSource = 0;
            int maxSource = 0;

            if (nudAge1_.Value > 0)
            {
                DateTime theDate = dtpDate1_.Value.AddYears((int)-nudAge1_.Value);
                if (theDate < maxDate)
                {
                    maxDate = theDate;
                    maxSource = 1;
                }
                theDate = theDate.AddYears(-1).AddDays(1);
                if (theDate > minDate)
                {
                    minDate = theDate;
                    minSource = 1;
                }
            }

            if (nudAge2_.Value > 0)
            {
                DateTime theDate = dtpDate2_.Value.AddYears((int)-nudAge2_.Value);
                if (theDate < maxDate)
                {
                    maxDate = theDate;
                    maxSource = 2;
                }
                theDate = theDate.AddYears(-1).AddDays(1);
                if (theDate > minDate)
                {
                    minDate = theDate;
                    minSource = 2;
                }
            }

            if (nudAge3_.Value > 0)
            {
                DateTime theDate = dtpDate3_.Value.AddYears((int)-nudAge3_.Value);
                if (theDate < maxDate)
                {
                    maxDate = theDate;
                    maxSource = 3;
                }
                theDate = theDate.AddYears(-1).AddDays(1);
                if (theDate > minDate)
                {
                    minDate = theDate;
                    minSource = 3;
                }
            }

            // Display the result.
            labReport_.Text = "The birthday must be between " + minDate.ToString("d MMMM yyyy") + " and " + maxDate.ToString("d MMMM yyyy");

            if (minSource == 1 || maxSource == 1)
            {
                picTick1_.Visible = true;
            }
            else
            {
                picTick1_.Visible = false;
            }
            if (minSource == 2 || maxSource == 2)
            {
                picTick2_.Visible = true;
            }
            else
            {
                picTick2_.Visible = false;
            }
            if (minSource == 3 || maxSource == 3)
            {
                picTick3_.Visible = true;
            }
            else
            {
                picTick3_.Visible = false;
            }
        }



        /// <summary>Message handler for the Copy button click event.</summary>
        private void cmdCopy_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(labReport_.Text);
        }



        #endregion

    }
}