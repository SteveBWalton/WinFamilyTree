using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using FamilyTree.Objects;

namespace FamilyTree.Viewer
{
    /// <summary>Delegate for the value changed event.</summary>
    public delegate void FuncValueChanged(object sender);

    /// <summary>Class to display a control to allow the user to edit CompoundDate values.</summary>
    public class CompoundDateEditBox : System.Windows.Forms.UserControl
    {
        #region Member Variables

        /// <summary>Event to raise the value changed delegate.</summary>
        public event FuncValueChanged eventValueChanged;

        /// <summary>False when value changed events should be blocked.  True if they should be raised to the delegate.</summary>
        private bool isAllowChangeEvents_;

        private System.Windows.Forms.NumericUpDown nudDay_;
        private System.Windows.Forms.NumericUpDown nudYear_;
        private System.Windows.Forms.ComboBox cboMonth_;
        private TextBox txtWholeThing_;
        private ContextMenuStrip m_ContextMenuStrip;
        private ToolStripMenuItem tsmDayUnknown_;
        private ToolStripMenuItem tsmMonthUnknown_;
        private ToolStripMenuItem tsmYearUnknown_;
        private ToolStripMenuItem tsmIsBefore_;
        private ToolStripMenuItem tsmIsAfter_;
        private ToolStripMenuItem tsmNull_;
        private ToolStripMenuItem tsmQuarter_;
        private IContainer components;

        #endregion

        #region Public Functions



        /// <summary>Class constructor.</summary>
        public CompoundDateEditBox()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // Position the controls.
            txtWholeThing_.Left = 0;
            txtWholeThing_.Top = 0;

            // Add any initialization after the InitializeComponent call.
            isAllowChangeEvents_ = true;
        }



        /// <summary>The date and status that the control is displaying.</summary>
        public CompoundDate theDate
        {
            set
            {
                // Disable change events.
                isAllowChangeEvents_ = false;

                // Deal with the month / quarters status.
                if ((value.status & CompoundDate.QUARTER) == 0)
                {
                    if (tsmQuarter_.Checked)
                    {
                        populateMonthCombo(false);
                        tsmQuarter_.Checked = false;
                    }
                }
                else
                {
                    if (!tsmQuarter_.Checked)
                    {
                        populateMonthCombo(true);
                        tsmQuarter_.Checked = true;
                    }
                }

                // Deal with the date.
                txtWholeThing_.Text = value.format(DateFormat.FULL_SHORT);
                nudDay_.Value = value.date.Day;
                if (isQuarter)
                {
                    cboMonth_.SelectedIndex = (value.date.Month - 1) / 3;
                }
                else
                {
                    cboMonth_.SelectedIndex = value.date.Month - 1;
                }
                int nYear = CompoundDate.getYear(value.date);
                nudYear_.Value = (decimal)nYear;

                // Deal with the status.
                if ((value.status & 8) == 8)
                {
                    isNull = true;
                }
                else
                {
                    contextMenuUnNull();

                    // Deal with the status.
                    isDayKnown = ((value.status & 1) == 0);
                    isMonthKnown = ((value.status & 2) == 0);
                    isYearKnown = ((value.status & 4) == 0);

                    if ((value.status & CompoundDate.AFTER) == 0)
                    {
                        tsmIsAfter_.Checked = false;
                    }
                    else
                    {
                        tsmIsAfter_.Checked = true;
                    }
                    if ((value.status & CompoundDate.BEFORE) == 0)
                    {
                        tsmIsBefore_.Checked = false;
                    }
                    else
                    {
                        tsmIsBefore_.Checked = true;
                    }
                }

                // Enable change events.
                isAllowChangeEvents_ = true;
            }
            get
            {
                CompoundDate result = new CompoundDate();

                result.date = getDate();
                result.status = getStatus();
                return result;
            }
        }



        /// <summary>Return the date that the control is using.  It might not be displaying the whole date because of the status value.</summary>
        /// <returns>The date that the control is using.</returns>
        public DateTime getDate()
        {
            try
            {
                int monthFactor = 1;
                if (isQuarter)
                {
                    monthFactor = 3;
                }
                return new DateTime(CompoundDate.setYear((int)nudYear_.Value), monthFactor * (int)cboMonth_.SelectedIndex + 1, (int)nudDay_.Value);
            }
            catch
            {
                return DateTime.Now;
            }
        }



        /// <summary>Returns the status of the date in the control.</summary>
        /// <returns>The status of the date in the control.</returns>
        public int getStatus()
        {
            int status = 0;

            if (tsmDayUnknown_.Checked)
            {
                status |= 1;
            }
            if (tsmMonthUnknown_.Checked)
            {
                status |= 2;
            }
            if (tsmYearUnknown_.Checked)
            {
                status |= 4;
            }
            if (tsmNull_.Checked)
            {
                status |= 8;
            }
            if (tsmIsAfter_.Checked)
            {
                status |= CompoundDate.AFTER;
            }
            if (tsmIsBefore_.Checked)
            {
                status |= CompoundDate.BEFORE;
            }
            if (tsmQuarter_.Checked)
            {
                status |= CompoundDate.QUARTER;
            }

            // Return the calculated value.
            return status;
        }



        /// <summary> Clean up any resources being used.</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }



        #endregion

        #region Supporting Functions



        /// <summary>Update the display of the summary text if it is visible.  This happens when the context menu modifies the value without taking the focus into the control.</summary>
        private void updateDateDisplay()
        {
            if (txtWholeThing_.Visible)
            {
                CompoundDate compoundDate = theDate;
                if (compoundDate.isEmpty())
                {
                    txtWholeThing_.Text = "";
                }
                else
                {
                    txtWholeThing_.Text = compoundDate.format(DateFormat.FULL_SHORT);
                }
            }
        }



        /// <summary>Mark the context menu for a non null value.</summary>
        private void contextMenuUnNull()
        {
            nudYear_.Visible = true;
            tsmNull_.Checked = false;
        }



        /// <summary>True, if the day is known.  False, otherwise.</summary>
        private bool isDayKnown
        {
            get { return !tsmDayUnknown_.Checked; }
            set
            {
                tsmDayUnknown_.Checked = !value;

                if (tsmDayUnknown_.Checked)
                {
                    nudDay_.Visible = false;
                }
                else
                {
                    contextMenuUnNull();
                    nudDay_.Visible = true;
                }

                // Update the date display if the control does not have the focus.
                updateDateDisplay();
            }
        }



        /// <summary>True, if the month is known.  False, otherwise.</summary>
        private bool isMonthKnown
        {
            get { return !tsmMonthUnknown_.Checked; }
            set
            {
                tsmMonthUnknown_.Checked = !value;

                if (tsmMonthUnknown_.Checked)
                {
                    cboMonth_.Visible = false;
                }
                else
                {
                    contextMenuUnNull();
                    cboMonth_.Visible = true;
                }

                // Update the date display if the control does not have the focus
                updateDateDisplay();
            }
        }



        /// <summary>True, if the year is known.  False, otherwise.</summary>
        private bool isYearKnown
        {
            get { return !tsmYearUnknown_.Checked; }
            set
            {
                tsmYearUnknown_.Checked = !value;

                if (tsmYearUnknown_.Checked)
                {
                    nudYear_.BackColor = Color.LightGray;
                }
                else
                {
                    contextMenuUnNull();
                    nudYear_.BackColor = Color.White;
                }

                // Update the date display if the control does not have the focus
                updateDateDisplay();
            }
        }



        /// <summary>True if the month is only known to the quarter.</summary>
        private bool isQuarter
        {
            get { return tsmQuarter_.Checked; }
            set
            {
                tsmQuarter_.Checked = value;

                // Update the date display if the control does not have the focus
                updateDateDisplay();
            }
        }



        /// <summary>True, if the date is null.  False, otherwise.</summary>
        private bool isNull
        {
            get { return tsmNull_.Checked; }
            set
            {
                tsmNull_.Checked = value;

                if (tsmNull_.Checked)
                {
                    isDayKnown = false;
                    isMonthKnown = false;
                    isYearKnown = false;
                    nudYear_.Visible = false;
                }
                else
                {
                    contextMenuUnNull();
                }
            }
        }



        /// <summary>Reset the the contents of the month control.</summary>
        /// <param name="useQuarters">Specifies true to use quaters not months in the combo box.</param>
        private void populateMonthCombo(bool useQuarters)
        {
            DateTime currentDate = getDate();

            cboMonth_.Items.Clear();
            if (useQuarters)
            {
                cboMonth_.Items.Add("Q1");
                cboMonth_.Items.Add("Q2");
                cboMonth_.Items.Add("Q3");
                cboMonth_.Items.Add("Q4");

                cboMonth_.SelectedIndex = (currentDate.Month - 1) / 3;
            }
            else
            {
                for (int month = 1; month <= 12; month++)
                {
                    DateTime thisYear = new DateTime(2000, month, 1);
                    cboMonth_.Items.Add(thisYear.ToString("MMM"));
                }
                cboMonth_.SelectedIndex = currentDate.Month - 1;
            }
        }



        #endregion

        #region Component Designer generated code
        /// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
            System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
            this.nudDay_ = new System.Windows.Forms.NumericUpDown();
            this.m_ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmDayUnknown_ = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmMonthUnknown_ = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmYearUnknown_ = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmIsBefore_ = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmIsAfter_ = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmNull_ = new System.Windows.Forms.ToolStripMenuItem();
            this.nudYear_ = new System.Windows.Forms.NumericUpDown();
            this.cboMonth_ = new System.Windows.Forms.ComboBox();
            this.txtWholeThing_ = new System.Windows.Forms.TextBox();
            this.tsmQuarter_ = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            ((System.ComponentModel.ISupportInitialize)(this.nudDay_)).BeginInit();
            this.m_ContextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudYear_)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new System.Drawing.Size(170, 6);
            // 
            // toolStripMenuItem4
            // 
            toolStripMenuItem4.Name = "toolStripMenuItem4";
            toolStripMenuItem4.Size = new System.Drawing.Size(170, 6);
            // 
            // m_nudDay
            // 
            this.nudDay_.ContextMenuStrip = this.m_ContextMenuStrip;
            this.nudDay_.Location = new System.Drawing.Point(0, 0);
            this.nudDay_.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.nudDay_.Name = "m_nudDay";
            this.nudDay_.Size = new System.Drawing.Size(40, 20);
            this.nudDay_.TabIndex = 0;
            this.nudDay_.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudDay_.ValueChanged += new System.EventHandler(this.nudDay_ValueChanged);
            // 
            // m_ContextMenuStrip
            // 
            this.m_ContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmDayUnknown_,
            this.tsmMonthUnknown_,
            this.tsmYearUnknown_,
            toolStripMenuItem3,
            this.tsmIsBefore_,
            this.tsmIsAfter_,
            toolStripMenuItem4,
            this.tsmQuarter_,
            this.tsmNull_});
            this.m_ContextMenuStrip.Name = "m_ContextMenuStrip";
            this.m_ContextMenuStrip.Size = new System.Drawing.Size(174, 192);
            // 
            // m_tsmDayUnknown
            // 
            this.tsmDayUnknown_.Name = "m_tsmDayUnknown";
            this.tsmDayUnknown_.Size = new System.Drawing.Size(173, 22);
            this.tsmDayUnknown_.Text = "Day Unknown";
            this.tsmDayUnknown_.Click += new System.EventHandler(this.menuDayUnknown_Click);
            // 
            // m_tsmMonthUnknown
            // 
            this.tsmMonthUnknown_.Name = "m_tsmMonthUnknown";
            this.tsmMonthUnknown_.Size = new System.Drawing.Size(173, 22);
            this.tsmMonthUnknown_.Text = "Month Unknown";
            this.tsmMonthUnknown_.Click += new System.EventHandler(this.menuMonthUnknown_Click);
            // 
            // m_tsmYearUnknown
            // 
            this.tsmYearUnknown_.Name = "m_tsmYearUnknown";
            this.tsmYearUnknown_.Size = new System.Drawing.Size(173, 22);
            this.tsmYearUnknown_.Text = "Year Unknown";
            this.tsmYearUnknown_.Click += new System.EventHandler(this.menuYearUnknown_Click);
            // 
            // m_tsmIsBefore
            // 
            this.tsmIsBefore_.Name = "m_tsmIsBefore";
            this.tsmIsBefore_.Size = new System.Drawing.Size(173, 22);
            this.tsmIsBefore_.Text = "Is Before";
            this.tsmIsBefore_.Click += new System.EventHandler(this.menuIsBefore_Click);
            // 
            // m_tsmIsAfter
            // 
            this.tsmIsAfter_.Name = "m_tsmIsAfter";
            this.tsmIsAfter_.Size = new System.Drawing.Size(173, 22);
            this.tsmIsAfter_.Text = "Is After";
            this.tsmIsAfter_.Click += new System.EventHandler(this.menuIsAfter_Click);
            // 
            // m_tsmNull
            // 
            this.tsmNull_.Name = "m_tsmNull";
            this.tsmNull_.Size = new System.Drawing.Size(173, 22);
            this.tsmNull_.Text = "Null";
            this.tsmNull_.Click += new System.EventHandler(this.menuNull_Click);
            // 
            // m_nudYear
            // 
            this.nudYear_.ContextMenuStrip = this.m_ContextMenuStrip;
            this.nudYear_.Location = new System.Drawing.Point(96, 0);
            this.nudYear_.Maximum = new decimal(new int[] {
            2100,
            0,
            0,
            0});
            this.nudYear_.Minimum = new decimal(new int[] {
            500,
            0,
            0,
            -2147483648});
            this.nudYear_.Name = "m_nudYear";
            this.nudYear_.Size = new System.Drawing.Size(48, 20);
            this.nudYear_.TabIndex = 1;
            this.nudYear_.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.nudYear_.ValueChanged += new System.EventHandler(this.nudYear_ValueChanged);
            // 
            // m_cboMonth
            // 
            this.cboMonth_.ContextMenuStrip = this.m_ContextMenuStrip;
            this.cboMonth_.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMonth_.Items.AddRange(new object[] {
            "Jan",
            "Feb",
            "Mar",
            "Apr",
            "May",
            "Jun",
            "Jul",
            "Aug",
            "Sep",
            "Oct",
            "Nov",
            "Dec"});
            this.cboMonth_.Location = new System.Drawing.Point(40, 0);
            this.cboMonth_.MaxDropDownItems = 12;
            this.cboMonth_.Name = "m_cboMonth";
            this.cboMonth_.Size = new System.Drawing.Size(56, 21);
            this.cboMonth_.TabIndex = 2;
            this.cboMonth_.SelectedIndexChanged += new System.EventHandler(this.cboMonth_SelectedIndexChanged);
            // 
            // m_txtWholeThing
            // 
            this.txtWholeThing_.ContextMenuStrip = this.m_ContextMenuStrip;
            this.txtWholeThing_.Location = new System.Drawing.Point(0, 24);
            this.txtWholeThing_.Name = "m_txtWholeThing";
            this.txtWholeThing_.Size = new System.Drawing.Size(144, 20);
            this.txtWholeThing_.TabIndex = 4;
            this.txtWholeThing_.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // m_tsmQuarter
            // 
            this.tsmQuarter_.Name = "m_tsmQuarter";
            this.tsmQuarter_.Size = new System.Drawing.Size(173, 22);
            this.tsmQuarter_.Text = "Month as Quarter";
            this.tsmQuarter_.Click += new System.EventHandler(this.menuQuarter_Click);
            // 
            // ucDate
            // 
            this.ContextMenuStrip = this.m_ContextMenuStrip;
            this.Controls.Add(this.txtWholeThing_);
            this.Controls.Add(this.cboMonth_);
            this.Controls.Add(this.nudYear_);
            this.Controls.Add(this.nudDay_);
            this.Name = "ucDate";
            this.Size = new System.Drawing.Size(144, 69);
            this.Leave += new System.EventHandler(this.compoundDate_Leave);
            this.Enter += new System.EventHandler(this.compoundDate_Enter);
            ((System.ComponentModel.ISupportInitialize)(this.nudDay_)).EndInit();
            this.m_ContextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudYear_)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        #region Message Handlers



        /// <summary>Message handler for the Day Unknown context menu click.</summary>
        private void menuDayUnknown_Click(object sender, System.EventArgs e)
        {
            // Toggle the status of the menu button.
            isDayKnown = tsmDayUnknown_.Checked;

            // Value Changed event.
            if (sender != null && isAllowChangeEvents_)
            {
                this.eventValueChanged(this);
            }
        }



        /// <summary>Message handler for the Month unknown context menu click.</summary>
        private void menuMonthUnknown_Click(object sender, System.EventArgs e)
        {
            // Toggle the month known status.
            isMonthKnown = tsmMonthUnknown_.Checked;

            // Value Changed event.
            if (sender != null && isAllowChangeEvents_)
            {
                this.eventValueChanged(this);
            }
        }



        /// <summary>Message handler for the year unknown context menu click.</summary>
        private void menuYearUnknown_Click(object sender, System.EventArgs e)
        {
            // Toggle the year known status.
            isYearKnown = tsmYearUnknown_.Checked;

            // Update the date display if the control does not have the focus.
            updateDateDisplay();

            // Value Changed event.
            if (sender != null && isAllowChangeEvents_)
            {
                this.eventValueChanged(this);
            }
        }



        /// <summary>Message handler for the Null context menu click.  Toggle the null status of the date.</summary>
        private void menuNull_Click(object sender, System.EventArgs e)
        {
            // Toggle the null status.
            isNull = !tsmNull_.Checked;

            // Value Changed event.
            if (sender != null && isAllowChangeEvents_)
            {
                this.eventValueChanged(this);
            }
        }



        /// <summary>Message handler for the month as quarter menu click.  Toggle the month as quarter status.</summary>
        private void menuQuarter_Click(object sender, EventArgs e)
        {
            // Update Month combo Box.
            populateMonthCombo(!tsmQuarter_.Checked);

            // Update the menu point.
            isQuarter = !tsmQuarter_.Checked;

            // Update the date display if the control does not have the focus.
            updateDateDisplay();

            // Value Changed event.
            if (sender != null && isAllowChangeEvents_)
            {
                this.eventValueChanged(this);
            }
        }



        private void nudDay_ValueChanged(object sender, System.EventArgs e)
        {
            // Value Changed event.
            if (sender != null && isAllowChangeEvents_)
            {
                this.eventValueChanged(this);
            }
        }



        private void nudYear_ValueChanged(object sender, System.EventArgs e)
        {
            // Value Changed event.
            if (sender != null && isAllowChangeEvents_)
            {
                this.eventValueChanged(this);
            }
        }



        private void cboMonth_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // Value Changed event.
            if (sender != null && isAllowChangeEvents_)
            {
                this.eventValueChanged(this);
            }
        }



        /// <summary>Message handler for the IsBefore menu point.  Change the IsBefore status and clear the IsAfter status.</summary>
        private void menuIsBefore_Click(object sender, System.EventArgs e)
        {
            // Update the menu check boxes.
            tsmIsBefore_.Checked = !tsmIsBefore_.Checked;
            tsmIsAfter_.Checked = false;

            // Update the display of the date.
            updateDateDisplay();

            // Value Changed event.
            if (sender != null && isAllowChangeEvents_)
            {
                this.eventValueChanged(this);
            }
        }



        /// <summary>Message handler for the IsAfter menu point.  Change the IsAfter status and clear the IsBefore status.</summary>
        private void menuIsAfter_Click(object sender, System.EventArgs e)
        {
            // Update the menu check boxes.
            tsmIsAfter_.Checked = !tsmIsAfter_.Checked;
            tsmIsBefore_.Checked = false;

            // Update the display of the date.
            updateDateDisplay();

            // Value Changed event.
            if (sender != null && isAllowChangeEvents_)
            {
                this.eventValueChanged(this);
            }
        }



        /// <summary>Message handler for the control getting the focus.  Show the edit controls.</summary>
        private void compoundDate_Enter(object sender, EventArgs e)
        {
            txtWholeThing_.Visible = false;
            if (!tsmDayUnknown_.Checked)
            {
                nudDay_.Visible = true;
            }
            if (!tsmMonthUnknown_.Checked)
            {
                cboMonth_.Visible = true;
            }
            if (!tsmNull_.Checked)
            {
                nudYear_.Visible = true;
            }
        }



        /// <summary>Message handler for the control losing the focus.  Show the summary control.</summary>
        private void compoundDate_Leave(object sender, EventArgs e)
        {
            txtWholeThing_.Text = theDate.format(DateFormat.FULL_SHORT);

            txtWholeThing_.Visible = true;
            nudDay_.Visible = false;
            cboMonth_.Visible = false;
            nudYear_.Visible = false;
        }



        #endregion

    }
}
