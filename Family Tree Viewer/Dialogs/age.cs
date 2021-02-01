using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using family_tree.objects;

namespace family_tree.viewer
{
    /// <summary>Class to represent a dialog to quickly calculate the age of the specified person on a specified date.</summary>
    public partial class AgeDialog : System.Windows.Forms.Form
    {
        #region Member Variables

        /// <summary>Database that this form is attached to.  The person must come from this database.</summary>
        private Database database_;

        /// <summary>Person to calculate the age of.</summary>
        private Person person_;

        #endregion

        #region Constructors etc ...



        /// <summary>Class constructor for the dialog.</summary>
        /// <param name="database">Specify the database to connect this dialog to.</param>
        /// <param name="personIndex">Specify the person to calculate the age of.</param>
        public AgeDialog(Database database, int personIndex)
        {
            InitializeComponent();

            // Save the connection to the database.
            database_ = database;

            // Load a list of all people into the combo box.
            IndexName[] people = database.getPeople(ChooseSex.EITHER, family_tree.objects.SortOrder.DATE, 0, 3000);
            for (int i = 0; i < people.Length; i++)
            {
                cboPerson_.Items.Add(people[i]);
                if (people[i].index == personIndex)
                {
                    cboPerson_.SelectedItem = people[i];
                }
            }

            // Find the current person.
            person_ = new Person(personIndex, database_);
            labDoB_.Text = person_.dob.format(DateFormat.FULL_LONG);

            // Default date.
            ucDate_.theDate = new CompoundDate(new DateTime(1901, 3, 31));
            labTheAge_.Text = person_.getAge(this.ucDate_.theDate);
        }



        /// <summary>Clean up any resources being used.</summary>
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

        #region Message Handlers



        /// <summary>Message handler for the Date1 value changed event.  Update the displayed age of the person, since the date has just changed.</summary>
        private void ucDate1_evtValueChanged(object sender)
        {
            labTheAge_.Text = person_.getAge(this.ucDate_.theDate);
        }



        /// <summary>Message handler for the seleted person value changed event.  Update the displayed age of the person, since the person has just changed.</summary>
        private void cboPerson_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            IndexName person = (IndexName)this.cboPerson_.SelectedItem;
            person_ = new Person(person.index, database_);
            labDoB_.Text = person_.dob.format(DateFormat.FULL_LONG);
            labTheAge_.Text = person_.getAge(this.ucDate_.theDate);
        }



        #endregion
    }
}
