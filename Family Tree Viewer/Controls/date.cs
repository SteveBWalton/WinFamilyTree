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
	public delegate void dgtValueChanged(object oSender);   // delegate declaration

	/// <summary>
	/// Class to display a control to allow the user to edit clsDate values.
	/// </summary>
	public class ucDate : System.Windows.Forms.UserControl
	{
		#region Member Variables

		/// <summary>Event to raise the value changed delegate.</summary>
		public event dgtValueChanged evtValueChanged;

		/// <summary>False when value changed events should be blocked.  True if they should be raised to the delegate.</summary>
		private bool m_bAllowChangeEvents;

		private System.Windows.Forms.NumericUpDown m_nudDay;
		private System.Windows.Forms.NumericUpDown m_nudYear;
        private System.Windows.Forms.ComboBox m_cboMonth;
        private TextBox m_txtWholeThing;		
        private ContextMenuStrip m_ContextMenuStrip;
        private ToolStripMenuItem m_tsmDayUnknown;
        private ToolStripMenuItem m_tsmMonthUnknown;
        private ToolStripMenuItem m_tsmYearUnknown;
        private ToolStripMenuItem m_tsmIsBefore;
        private ToolStripMenuItem m_tsmIsAfter;
        private ToolStripMenuItem m_tsmNull;
        private ToolStripMenuItem m_tsmQuarter;
        private IContainer components;

        #endregion

        #region Public Functions

        /// <summary>
		/// Class constructor.
		/// </summary>
		public ucDate()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// Position the controls
            m_txtWholeThing.Left = 0;
            m_txtWholeThing.Top = 0;
            
            // Add any initialization after the InitializeComponent call
			m_bAllowChangeEvents = true;
		}

		/// <summary>
		/// The date and status that the control is displaying
		/// </summary>
		public CompoundDate Value
		{
			set
			{
				// Disable evtChangeEvent
				m_bAllowChangeEvents = false;

                // Deal with the month / quarters status
                if((value.status & CompoundDate.QUARTER) == 0)
                {
                    if(m_tsmQuarter.Checked)
                    {
                        PopulateMonthCombo(false);
                    m_tsmQuarter.Checked = false;
                    }
                }
                else
                {
                    if(!m_tsmQuarter.Checked)
                    {
                        PopulateMonthCombo(true);
                        m_tsmQuarter.Checked = true;
                    }
                }                

				// Deal with the date
                m_txtWholeThing.Text = value.format(DateFormat.FULL_SHORT);
				m_nudDay.Value = value.date.Day;
                if(IsQuarter)
                {
                    m_cboMonth.SelectedIndex = (value.date.Month - 1) / 3;
                }
                else
                {
                    m_cboMonth.SelectedIndex = value.date.Month - 1;
                }
				int nYear = CompoundDate.getYear(value.date);
				m_nudYear.Value = (decimal)nYear;
								
				// Deal with the status
				if((value.status&8)==8)
				{
                    IsNull = true;					
				}
				else
				{
					ContextMenuUnNull();

					// Deal with the status
                    IsDayKnown = ((value.status & 1) == 0);                    
                    IsMonthKnown = ((value.status&2)==0);
                    IsYearKnown=((value.status&4)==0);

					if((value.status & CompoundDate.AFTER)==0)
					{
                        m_tsmIsAfter.Checked = false;						
					}
					else
					{
                        m_tsmIsAfter.Checked = true;												
					}
					if((value.status & CompoundDate.BEFORE)==0)
					{
                        m_tsmIsBefore.Checked = false;
					}
					else
					{
                        m_tsmIsBefore.Checked = true;
					}
				}

				// Enable evtChangeEvent
				m_bAllowChangeEvents = true;
			}
			get
			{
				CompoundDate dtReturn = new CompoundDate();

				dtReturn.date = GetDate();
				dtReturn.status = GetStatus();
				return dtReturn;
			}
		}

		/// <summary>
		/// Return the date that the control is using.
		/// It might not be displaying the whole date because of the status value.
		/// </summary>
		/// <returns></returns>
		public DateTime GetDate()
		{
			try
			{
                int nMonthFactor = 1;
                if(IsQuarter)
                {
                    nMonthFactor = 3;
                }
				return new DateTime(CompoundDate.setYear((int)m_nudYear.Value),nMonthFactor*(int)m_cboMonth.SelectedIndex+1,(int)m_nudDay.Value);
			}
			catch
			{
				return DateTime.Now;
			}
		}

		/// <summary>
		/// Returns the status of the date in the control.
		/// </summary>
		/// <returns></returns>
		public int GetStatus()
		{
			int nStatus=0;

			if(m_tsmDayUnknown.Checked)
			{
				nStatus |= 1;
			}
			if(m_tsmMonthUnknown.Checked)
			{
				nStatus |= 2;
			}
			if(m_tsmYearUnknown.Checked)
			{
				nStatus |= 4;
			}
			if(m_tsmNull.Checked)
			{
				nStatus |= 8;
			}
			if(m_tsmIsAfter.Checked)
			{
				nStatus |= CompoundDate.AFTER;
			}
			if(m_tsmIsBefore.Checked)
			{
                nStatus |= CompoundDate.BEFORE;
			}
            if(m_tsmQuarter.Checked)
            {
                nStatus |= CompoundDate.QUARTER;
            }

			// Return the calculated value
			return nStatus;
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#endregion

        #region Supporting Functions

        /// <summary>
        /// Update the display of the summary text if it is visible.
        /// This happens when the context menu modifies the value without taking the focus into the control.
        /// </summary>
        private void UpdateDateDisplay()
        {
            if(m_txtWholeThing.Visible)
            {
                CompoundDate oDate = Value;
                if(oDate.isEmpty())
                {
                    m_txtWholeThing.Text = "";
                }
                else
                {
                    m_txtWholeThing.Text = oDate.format(DateFormat.FULL_SHORT);
                }
            }
        }

        /// <summary>
        /// Mark the context menu for a non null value.
        /// </summary>
        private void ContextMenuUnNull()
        {
            m_nudYear.Visible = true;
            m_tsmNull.Checked = false;
        }

        /// <summary>
        /// True, if the day is known.
        /// False, otherwise.
        /// </summary>
        private bool IsDayKnown
        {
            get { return !m_tsmDayUnknown.Checked; }
            set
            {
                m_tsmDayUnknown.Checked = !value;

                if(m_tsmDayUnknown.Checked)
                {
                    m_nudDay.Visible = false;
                }
                else
                {
                    ContextMenuUnNull();
                    m_nudDay.Visible = true;
                }

                // Update the date display if the control does not have the focus
                UpdateDateDisplay();
            }
        }

        /// <summary>
        /// True, if the month is known.
        /// False, otherwise.
        /// </summary>
        private bool IsMonthKnown
        {
            get { return !m_tsmMonthUnknown.Checked; }
            set
            {
                m_tsmMonthUnknown.Checked = !value;

                if(m_tsmMonthUnknown.Checked)
                {
                    m_cboMonth.Visible = false;
                }
                else
                {
                    ContextMenuUnNull();
                    m_cboMonth.Visible = true;
                }

                // Update the date display if the control does not have the focus
                UpdateDateDisplay();
            }
        }

        /// <summary>
        /// True, if the year is known.
        /// False, otherwise.
        /// </summary>
        private bool IsYearKnown
        {
            get { return !m_tsmYearUnknown.Checked; }
            set
            {
                m_tsmYearUnknown.Checked = !value;

                if(m_tsmYearUnknown.Checked)
                {
                    m_nudYear.BackColor = Color.LightGray;
                }
                else
                {
                    ContextMenuUnNull();
                    m_nudYear.BackColor = Color.White;
                }

                // Update the date display if the control does not have the focus
                UpdateDateDisplay();
            }
        }

        /// <summary>
        /// True if the month is only known to the quarter.
        /// </summary>
        private bool IsQuarter
        {
            get { return m_tsmQuarter.Checked; }
            set
            {
                m_tsmQuarter.Checked = value;

                // Update the date display if the control does not have the focus
                UpdateDateDisplay();
            }
        }

        /// <summary>
        /// True, if the date is null.
        /// False, otherwise.
        /// </summary>
        private bool IsNull
        {
            get { return m_tsmNull.Checked; }
            set
            {
                m_tsmNull.Checked = value;

                if(m_tsmNull.Checked)
                {
                    IsDayKnown = false;
                    IsMonthKnown = false;
                    IsYearKnown = false;
                    m_nudYear.Visible = false;
                }
                else
                {
                    ContextMenuUnNull();
                }
            }
        }

        /// <summary>
        /// Reset the the contents of the month control
        /// </summary>
        private void PopulateMonthCombo
            (
            bool bQuarter
            )
        {
            DateTime dtCurrent = GetDate();

            m_cboMonth.Items.Clear();
            if(bQuarter)
            {
                m_cboMonth.Items.Add("Q1");
                m_cboMonth.Items.Add("Q2");
                m_cboMonth.Items.Add("Q3");
                m_cboMonth.Items.Add("Q4");

                m_cboMonth.SelectedIndex = (dtCurrent.Month - 1) / 3;
            }
            else
            {
                for(int nMonth = 1;nMonth <= 12;nMonth++)
                {
                    DateTime dtThisYear = new DateTime(2000,nMonth,1);
                    m_cboMonth.Items.Add(dtThisYear.ToString("MMM"));
                }
                m_cboMonth.SelectedIndex = dtCurrent.Month - 1;
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
            this.m_nudDay = new System.Windows.Forms.NumericUpDown();
            this.m_ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.m_tsmDayUnknown = new System.Windows.Forms.ToolStripMenuItem();
            this.m_tsmMonthUnknown = new System.Windows.Forms.ToolStripMenuItem();
            this.m_tsmYearUnknown = new System.Windows.Forms.ToolStripMenuItem();
            this.m_tsmIsBefore = new System.Windows.Forms.ToolStripMenuItem();
            this.m_tsmIsAfter = new System.Windows.Forms.ToolStripMenuItem();
            this.m_tsmNull = new System.Windows.Forms.ToolStripMenuItem();
            this.m_nudYear = new System.Windows.Forms.NumericUpDown();
            this.m_cboMonth = new System.Windows.Forms.ComboBox();
            this.m_txtWholeThing = new System.Windows.Forms.TextBox();
            this.m_tsmQuarter = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudDay)).BeginInit();
            this.m_ContextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudYear)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new System.Drawing.Size(170,6);
            // 
            // toolStripMenuItem4
            // 
            toolStripMenuItem4.Name = "toolStripMenuItem4";
            toolStripMenuItem4.Size = new System.Drawing.Size(170,6);
            // 
            // m_nudDay
            // 
            this.m_nudDay.ContextMenuStrip = this.m_ContextMenuStrip;
            this.m_nudDay.Location = new System.Drawing.Point(0,0);
            this.m_nudDay.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.m_nudDay.Name = "m_nudDay";
            this.m_nudDay.Size = new System.Drawing.Size(40,20);
            this.m_nudDay.TabIndex = 0;
            this.m_nudDay.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.m_nudDay.ValueChanged += new System.EventHandler(this.nudDay_ValueChanged);
            // 
            // m_ContextMenuStrip
            // 
            this.m_ContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_tsmDayUnknown,
            this.m_tsmMonthUnknown,
            this.m_tsmYearUnknown,
            toolStripMenuItem3,
            this.m_tsmIsBefore,
            this.m_tsmIsAfter,
            toolStripMenuItem4,
            this.m_tsmQuarter,
            this.m_tsmNull});
            this.m_ContextMenuStrip.Name = "m_ContextMenuStrip";
            this.m_ContextMenuStrip.Size = new System.Drawing.Size(174,192);
            // 
            // m_tsmDayUnknown
            // 
            this.m_tsmDayUnknown.Name = "m_tsmDayUnknown";
            this.m_tsmDayUnknown.Size = new System.Drawing.Size(173,22);
            this.m_tsmDayUnknown.Text = "Day Unknown";
            this.m_tsmDayUnknown.Click += new System.EventHandler(this.menuDayUnknown_Click);
            // 
            // m_tsmMonthUnknown
            // 
            this.m_tsmMonthUnknown.Name = "m_tsmMonthUnknown";
            this.m_tsmMonthUnknown.Size = new System.Drawing.Size(173,22);
            this.m_tsmMonthUnknown.Text = "Month Unknown";
            this.m_tsmMonthUnknown.Click += new System.EventHandler(this.menuMonthUnknown_Click);
            // 
            // m_tsmYearUnknown
            // 
            this.m_tsmYearUnknown.Name = "m_tsmYearUnknown";
            this.m_tsmYearUnknown.Size = new System.Drawing.Size(173,22);
            this.m_tsmYearUnknown.Text = "Year Unknown";
            this.m_tsmYearUnknown.Click += new System.EventHandler(this.menuYearUnknown_Click);
            // 
            // m_tsmIsBefore
            // 
            this.m_tsmIsBefore.Name = "m_tsmIsBefore";
            this.m_tsmIsBefore.Size = new System.Drawing.Size(173,22);
            this.m_tsmIsBefore.Text = "Is Before";
            this.m_tsmIsBefore.Click += new System.EventHandler(this.menuIsBefore_Click);
            // 
            // m_tsmIsAfter
            // 
            this.m_tsmIsAfter.Name = "m_tsmIsAfter";
            this.m_tsmIsAfter.Size = new System.Drawing.Size(173,22);
            this.m_tsmIsAfter.Text = "Is After";
            this.m_tsmIsAfter.Click += new System.EventHandler(this.menuIsAfter_Click);
            // 
            // m_tsmNull
            // 
            this.m_tsmNull.Name = "m_tsmNull";
            this.m_tsmNull.Size = new System.Drawing.Size(173,22);
            this.m_tsmNull.Text = "Null";
            this.m_tsmNull.Click += new System.EventHandler(this.menuNull_Click);
            // 
            // m_nudYear
            // 
            this.m_nudYear.ContextMenuStrip = this.m_ContextMenuStrip;
            this.m_nudYear.Location = new System.Drawing.Point(96,0);
            this.m_nudYear.Maximum = new decimal(new int[] {
            2100,
            0,
            0,
            0});
            this.m_nudYear.Minimum = new decimal(new int[] {
            500,
            0,
            0,
            -2147483648});
            this.m_nudYear.Name = "m_nudYear";
            this.m_nudYear.Size = new System.Drawing.Size(48,20);
            this.m_nudYear.TabIndex = 1;
            this.m_nudYear.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.m_nudYear.ValueChanged += new System.EventHandler(this.nudYear_ValueChanged);
            // 
            // m_cboMonth
            // 
            this.m_cboMonth.ContextMenuStrip = this.m_ContextMenuStrip;
            this.m_cboMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cboMonth.Items.AddRange(new object[] {
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
            this.m_cboMonth.Location = new System.Drawing.Point(40,0);
            this.m_cboMonth.MaxDropDownItems = 12;
            this.m_cboMonth.Name = "m_cboMonth";
            this.m_cboMonth.Size = new System.Drawing.Size(56,21);
            this.m_cboMonth.TabIndex = 2;
            this.m_cboMonth.SelectedIndexChanged += new System.EventHandler(this.cboMonth_SelectedIndexChanged);
            // 
            // m_txtWholeThing
            // 
            this.m_txtWholeThing.ContextMenuStrip = this.m_ContextMenuStrip;
            this.m_txtWholeThing.Location = new System.Drawing.Point(0,24);
            this.m_txtWholeThing.Name = "m_txtWholeThing";
            this.m_txtWholeThing.Size = new System.Drawing.Size(144,20);
            this.m_txtWholeThing.TabIndex = 4;
            this.m_txtWholeThing.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // m_tsmQuarter
            // 
            this.m_tsmQuarter.Name = "m_tsmQuarter";
            this.m_tsmQuarter.Size = new System.Drawing.Size(173,22);
            this.m_tsmQuarter.Text = "Month as Quarter";
            this.m_tsmQuarter.Click += new System.EventHandler(this.menuQuarter_Click);
            // 
            // ucDate
            // 
            this.ContextMenuStrip = this.m_ContextMenuStrip;
            this.Controls.Add(this.m_txtWholeThing);
            this.Controls.Add(this.m_cboMonth);
            this.Controls.Add(this.m_nudYear);
            this.Controls.Add(this.m_nudDay);
            this.Name = "ucDate";
            this.Size = new System.Drawing.Size(144,69);
            this.Leave += new System.EventHandler(this.ucDate_Leave);
            this.Enter += new System.EventHandler(this.ucDate_Enter);
            ((System.ComponentModel.ISupportInitialize)(this.m_nudDay)).EndInit();
            this.m_ContextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_nudYear)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		#region Message Handlers

		/// <summary>
		/// Message handler for the Day Unknown context menu click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void menuDayUnknown_Click(object sender, System.EventArgs e)
		{
            // Toggle the status of the menu button
            IsDayKnown = m_tsmDayUnknown.Checked;

			// Value Changed event
			if(sender!=null&&m_bAllowChangeEvents)
			{
				this.evtValueChanged(this);
			}
		}

		/// <summary>
		/// Message handler for the Month unknown context menu click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void menuMonthUnknown_Click(object sender, System.EventArgs e)
		{
            // Toggle the month known status
            IsMonthKnown = m_tsmMonthUnknown.Checked;

			// Value Changed event
			if(sender!=null&&m_bAllowChangeEvents)
			{
				this.evtValueChanged(this);
			}
		}

		/// <summary>
		/// Message handler for the year unknown context menu click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void menuYearUnknown_Click(object sender, System.EventArgs e)
		{
            // Toggle the year known status
            IsYearKnown = m_tsmYearUnknown.Checked;

            // Update the date display if the control does not have the focus
            UpdateDateDisplay();

            // Value Changed event
            if(sender != null && m_bAllowChangeEvents)
            {
                this.evtValueChanged(this);
            }
        }

		/// <summary>
		/// Message handler for the Null context menu click.
        /// Toggle the null status of the date.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void menuNull_Click(object sender, System.EventArgs e)
		{
            // Toggle the null status
            IsNull = !m_tsmNull.Checked;

            // Value Changed event
            if(sender != null && m_bAllowChangeEvents)
            {
                this.evtValueChanged(this);
            }
		}

        /// <summary>
        /// Message handler for the month as quarter menu click.
        /// Toggle the month as quarter status.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuQuarter_Click(object sender,EventArgs e)
        {
            // Update Month combo Box
            PopulateMonthCombo(!m_tsmQuarter.Checked);

            // Update the menu point
            IsQuarter = !m_tsmQuarter.Checked;

            // Update the date display if the control does not have the focus
            UpdateDateDisplay();

            // Value Changed event
            if(sender != null && m_bAllowChangeEvents)
            {
                this.evtValueChanged(this);
            }		
        }

		private void nudDay_ValueChanged(object sender, System.EventArgs e)
		{
			// Value Changed event
			if(sender!=null&&m_bAllowChangeEvents)
			{
				this.evtValueChanged(this);
			}		
		}

		private void nudYear_ValueChanged(object sender, System.EventArgs e)
		{
			// Value Changed event
			if(sender!=null&&m_bAllowChangeEvents)
			{
				this.evtValueChanged(this);
			}		
		}

		private void cboMonth_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			// Value Changed event
			if(sender!=null&&m_bAllowChangeEvents)
			{
				this.evtValueChanged(this);
			}		
		}

		/// <summary>
		/// Message handler for the IsBefore menu point.
		/// Change the IsBefore status and clear the IsAfter status.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuIsBefore_Click(object sender, System.EventArgs e)
		{
            // Update the menu check boxes
            m_tsmIsBefore.Checked = !m_tsmIsBefore.Checked;
            m_tsmIsAfter.Checked = false;

            // Update the display of the date
            UpdateDateDisplay();
			
			// Value Changed event
			if(sender!=null&&m_bAllowChangeEvents)
			{
				this.evtValueChanged(this);
			}		
		}

		/// <summary>
		/// Message handler for the IsAfter menu point.
		/// Change the IsAfter status and clear the IsBefore status.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuIsAfter_Click(object sender, System.EventArgs e)
		{
            // Update the menu check boxes
            m_tsmIsAfter.Checked = !m_tsmIsAfter.Checked;
            m_tsmIsBefore.Checked = false;

            // Update the display of the date
            UpdateDateDisplay();            

            // Value Changed event
			if(sender!=null&&m_bAllowChangeEvents)
			{
				this.evtValueChanged(this);
			}		
		}

        /// <summary>
        /// Message handler for the control getting the focus.
        /// Show the edit controls.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucDate_Enter(object sender,EventArgs e)
        {
            m_txtWholeThing.Visible = false;
            if(!m_tsmDayUnknown.Checked)
            {
                m_nudDay.Visible = true;
            }
            if(!m_tsmMonthUnknown.Checked)
            {
                m_cboMonth.Visible = true;
            }
            if(!m_tsmNull.Checked)
            {
                m_nudYear.Visible = true;
            }
        }

        /// <summary>
        /// Message handler for the control losing the focus.
        /// Show the summary control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void ucDate_Leave(object sender,EventArgs e)
        {
            m_txtWholeThing.Text = Value.format(DateFormat.FULL_SHORT);

            m_txtWholeThing.Visible = true;
            m_nudDay.Visible = false;
            m_cboMonth.Visible = false;
            m_nudYear.Visible = false;
        }

        #endregion

    }
}
