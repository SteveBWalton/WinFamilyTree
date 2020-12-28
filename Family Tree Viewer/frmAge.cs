using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using FamilyTree.Objects;

namespace FamilyTree.Viewer
{
	/// <summary>
	/// Form to quickly calculate the age of the specified person on a specified date.
	/// </summary>
	public partial class frmAge : System.Windows.Forms.Form
	{
		#region Member Variables

		/// <summary>Database that this form is attached to.  The person must come from this database.</summary>
		private clsDatabase m_oDB;

		/// <summary>Person to calculate the age of.</summary>
		private clsPerson m_oPerson;

		#endregion

		#region Constructors etc ...

		/// <summary>
		/// Class constructor for the dialog.
		/// </summary>
		/// <param name="oDB">Specify the database to connect this dialog to.</param>
		/// <param name="nPersonID">Specify the person to calculate the age of.</param>
		public frmAge
			(
			clsDatabase oDB,
			int nPersonID
			)
		{
			InitializeComponent();

			// Save the connection to the database
			m_oDB = oDB;

			// Load a list of all people into the combo box
			clsIDName[] oPeople = oDB.GetPeople(enumChooseSex.Either,enumSortOrder.Date,0,3000);
			for(int nI=0;nI<oPeople.Length;nI++)
			{
				this.cboPerson.Items.Add(oPeople[nI]);
				if(oPeople[nI].ID==nPersonID)
				{
					this.cboPerson.SelectedItem = oPeople[nI];
				}			 
			}

			// Find the current person
			m_oPerson = new clsPerson(nPersonID,m_oDB);
			this.labDoB.Text = m_oPerson.DoB.Format(DateFormat.FullLong);

			// Default date
			this.ucDate1.Value = new clsDate(new DateTime(1901,3,31));
			this.labTheAge.Text = m_oPerson.Age(this.ucDate1.Value);
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

		#region Message Handlers

		/// <summary>
		/// Message handler for the Date1 value changed event.
		/// Update the displayed age of the person, since the date has just changed.
		/// </summary>
		/// <param name="oSender"></param>
		private void ucDate1_evtValueChanged(object oSender)
		{
			labTheAge.Text = m_oPerson.Age(this.ucDate1.Value);
		}

		/// <summary>
		/// Message handler for the seleted person value changed event.
		/// Update the displayed age of the person, since the person has just changed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cboPerson_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			clsIDName oPerson = (clsIDName)this.cboPerson.SelectedItem;
			m_oPerson = new clsPerson(oPerson.ID,m_oDB);
			labDoB.Text = m_oPerson.DoB.Format(DateFormat.FullLong);
			labTheAge.Text = m_oPerson.Age(this.ucDate1.Value);
		}

		#endregion
	}
}
