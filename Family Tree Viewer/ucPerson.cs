using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using FamilyTree.Objects;

namespace FamilyTree.Viewer
{
    // Delegate type for the Click event.
    /// <summary>
    /// Delegate type for the Click event.
    /// </summary>
	public delegate void dgtClick(object oSender);

	/// <summary>
	/// User control to display a person 
	/// </summary>
	public class ucPerson : System.Windows.Forms.UserControl
	{
		#region Member Variables

		/// <summary>ID of the person to display.</summary>
		private int 	m_nPersonID;

        // This is the evtClick event of with signiture dgtClick() (delegate)
        /// <summary>
        /// This is the evtClick event of with signiture dgtClick() (delegate)
        /// </summary>
		public event dgtClick evtClick;

		private System.Windows.Forms.Label m_labName;
		private System.Windows.Forms.Label m_labBorn;		

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#endregion

		#region Public Functions
		
		/// <summary>
		/// Class Constructor
		/// </summary>
		public ucPerson()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// Add any initialization after the InitializeComponent call
			m_labName.Text = "Unknown";
			m_labBorn.Text = "";
		}

		/// <summary>
		/// Set the person that the control should display
		/// </summary>
		/// <param name="oPerson">Specify the person object to be displayed</param>
		/// <returns>True for success, false otherwise.</returns>
		public bool SetPerson
			(
			clsPerson oPerson
			)
		{
			m_nPersonID = oPerson.ID;
			m_labName.Text = oPerson.GetName(false,true);
			m_labBorn.Text = oPerson.ShortDescription(true);

			// Return success
			return true;
		}

		/// <summary>
		/// Gets the ID of the person displayed.
		/// </summary>
		/// <returns></returns>
		public int GetPersonID()
		{
			return m_nPersonID;
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.m_labName = new System.Windows.Forms.Label();
            this.m_labBorn = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // m_labName
            // 
            this.m_labName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_labName.Location = new System.Drawing.Point(0,0);
            this.m_labName.Name = "m_labName";
            this.m_labName.Size = new System.Drawing.Size(150,19);
            this.m_labName.TabIndex = 0;
            this.m_labName.Text = "label1";
            this.m_labName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.m_labName.Click += new System.EventHandler(this.labName_Click);
            // 
            // m_labBorn
            // 
            this.m_labBorn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_labBorn.Location = new System.Drawing.Point(0,20);
            this.m_labBorn.Name = "m_labBorn";
            this.m_labBorn.Size = new System.Drawing.Size(150,44);
            this.m_labBorn.TabIndex = 2;
            this.m_labBorn.Text = "label2";
            this.m_labBorn.Click += new System.EventHandler(this.labBorn_Click);
            // 
            // ucPerson
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.m_labBorn);
            this.Controls.Add(this.m_labName);
            this.Font = new System.Drawing.Font("Tahoma",8.25F,System.Drawing.FontStyle.Regular,System.Drawing.GraphicsUnit.Point,((byte)(0)));
            this.Name = "ucPerson";
            this.Size = new System.Drawing.Size(148,78);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ucPerson_Paint);
            this.Click += new System.EventHandler(this.ucPerson_Click);
            this.Resize += new System.EventHandler(this.ucPerson_Resize);
            this.ResumeLayout(false);

		}
		#endregion

		#region Message Handlers
		
		private void ucPerson_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			// e.Graphics.DrawLine(new Pen(Color.Black),0,0,this.Width,this.Height);
		}

		private void ucPerson_Resize(object sender, System.EventArgs e)
		{
			/*
			this.labBorn.Width = this.Width / 2;
			this.labDied.Width = this.labBorn.Width;
			this.labDied.Left = this.labBorn.Width;
			*/
		}

        /// <summary>
        /// Message handler for the click event on the name label.
        /// Raise the click event to the parent control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void labName_Click(object sender,EventArgs e)
        {
            // Raise the click event in the parent.  Call the delegates that the parent has assigned
            this.evtClick(this);
        }

        /// <summary>
        /// Message handler for the click event on the born label.
        /// Raise the click event to the parent control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void labBorn_Click(object sender,EventArgs e)
        {
            // Raise the click event in the parent.  Call the delegates that the parent has assigned
            this.evtClick(this);
        }

        /// <summary>
        /// Message handler for the click event on the background of the control.
        /// Raise the click event to the parent control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucPerson_Click(object sender,EventArgs e)
        {
            // Raise the click event in the parent.  Call the delegates that the parent has assigned
            this.evtClick(this);
        }

        #endregion

    }
}
