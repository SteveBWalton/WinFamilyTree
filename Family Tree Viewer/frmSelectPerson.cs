using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using FamilyTree.Objects;

namespace FamilyTree.Viewer
{
	/// <summary>
	/// Form to allow the user to select a person.
	/// </summary>
	public class frmSelectPerson : System.Windows.Forms.Form
	{
		#region Member Variables

		/// <summary>Database that this dialog can select from.</summary>
		private	Database m_oDB;

		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.ListBox lstPeople;
		private System.Windows.Forms.RadioButton radioDate;
		private System.Windows.Forms.RadioButton radioAlpha;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#endregion

		#region Constructors
		
		/// <summary>
		/// Class constructor
		/// </summary>
		public frmSelectPerson()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		// *******************************************************************************************************************
		/// <summary>
		/// Displays the dialog, allows the user to select a person and returns the ID the selected person.  Returns
		/// -1 if the user selected cancel or no person is selected.
		/// </summary>
		/// <param name="oOwner">Specify the parent window</param>
		/// <param name="oDB">Specify the database to select a person from</param>
		/// <returns>ID of the person selected, or -1 for cancel</returns>
		public int SelectPerson
			(
			IWin32Window	oOwner,
			Database		oDB
			)
		{
			// Store the parameters
			m_oDB = oDB;

            // Load a list of all people into the listbox
            IndexName[] oPeople = oDB.getPeople(ChooseSex.EITHER, Objects.SortOrder.DATE,0,9999);

			// Populate the list box
			for(int nI=0;nI<oPeople.Length;nI++)
			{
				this.lstPeople.Items.Add(oPeople[nI]);
			}

			// Show the dialog
			if(this.ShowDialog(oOwner)==DialogResult.Cancel)
			{
				return -1;
			}

			// Check that a person is selected
			if(this.lstPeople.SelectedIndex<0)
			{
				return -1;
			}

			// Find the selected person
			IndexName oSelected = (IndexName)this.lstPeople.SelectedItem;

			// Close the form
			this.Dispose();

			// Return the selected person
			return oSelected.index;
		}

		// *******************************************************************************************************************
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSelectPerson));
            this.lstPeople = new System.Windows.Forms.ListBox();
            this.radioDate = new System.Windows.Forms.RadioButton();
            this.radioAlpha = new System.Windows.Forms.RadioButton();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstPeople
            // 
            this.lstPeople.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstPeople.Location = new System.Drawing.Point(8, 48);
            this.lstPeople.Name = "lstPeople";
            this.lstPeople.Size = new System.Drawing.Size(280, 277);
            this.lstPeople.TabIndex = 0;
            this.lstPeople.DoubleClick += new System.EventHandler(this.lstPeople_DoubleClick);
            // 
            // radioDate
            // 
            this.radioDate.Checked = true;
            this.radioDate.Location = new System.Drawing.Point(16, 16);
            this.radioDate.Name = "radioDate";
            this.radioDate.Size = new System.Drawing.Size(104, 24);
            this.radioDate.TabIndex = 3;
            this.radioDate.TabStop = true;
            this.radioDate.Text = "Date Order";
            this.radioDate.CheckedChanged += new System.EventHandler(this.radioDate_CheckedChanged);
            // 
            // radioAlpha
            // 
            this.radioAlpha.Location = new System.Drawing.Point(120, 16);
            this.radioAlpha.Name = "radioAlpha";
            this.radioAlpha.Size = new System.Drawing.Size(128, 24);
            this.radioAlpha.TabIndex = 4;
            this.radioAlpha.Text = "Alphabetical Order";
            this.radioAlpha.CheckedChanged += new System.EventHandler(this.radioAlpha_CheckedChanged);
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Image = global::FamilyTree.Viewer.Properties.Resources.OK;
            this.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cmdOK.Location = new System.Drawing.Point(114, 331);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(100, 30);
            this.cmdOK.TabIndex = 1;
            this.cmdOK.Text = "OK";
            this.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Image = global::FamilyTree.Viewer.Properties.Resources.Cancel;
            this.cmdCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cmdCancel.Location = new System.Drawing.Point(8, 331);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(100, 30);
            this.cmdCancel.TabIndex = 2;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frmSelectPerson
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(292, 368);
            this.Controls.Add(this.radioAlpha);
            this.Controls.Add(this.radioDate);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.lstPeople);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSelectPerson";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Person";
            this.ResumeLayout(false);

		}
		#endregion

		#region Message Handlers
		
		private void radioDate_CheckedChanged(object sender, System.EventArgs e)
		{
			if(this.radioDate.Checked)
			{
                // Load a list of all people into the listbox
                IndexName[] oPeople = m_oDB.getPeople(ChooseSex.EITHER, Objects.SortOrder.DATE,0,3000);

				// Populate the list box
				this.lstPeople.Items.Clear();
				for(int nI=0;nI<oPeople.Length;nI++)
				{
					this.lstPeople.Items.Add(oPeople[nI]);
				}
			}
		}

		private void radioAlpha_CheckedChanged(object sender, System.EventArgs e)
		{
			if(this.radioAlpha.Checked)
			{
                // Load a list of all people into the listbox
                IndexName[] oPeople = m_oDB.getPeople(ChooseSex.EITHER, Objects.SortOrder.ALPHABETICAL,0,3000);

				// Populate the list box
				this.lstPeople.Items.Clear();
				for(int nI=0;nI<oPeople.Length;nI++)
				{
					this.lstPeople.Items.Add(oPeople[nI]);
				}
			}
		}

		private void lstPeople_DoubleClick(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}

		#endregion
	}
}
