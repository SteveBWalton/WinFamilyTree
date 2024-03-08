using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using family_tree.objects;

namespace family_tree.viewer
{
    /// <summary>Dialog to allow the user to select a person.</summary>
    public class SelectPersonDialog : System.Windows.Forms.Form
    {
        #region Member Variables

        /// <summary>Database that this dialog can select from.</summary>
        private Database database_;

        // Controls added by the designer.
        private System.Windows.Forms.ListBox lstPeople_;
        private System.Windows.Forms.RadioButton radioDate_;
        private System.Windows.Forms.RadioButton radioAlpha_;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        #endregion

        #region Constructors


        /// <summary>Class constructor.</summary>
        public SelectPersonDialog()
        {
            // Required for Windows Form Designer support.
            InitializeComponent();
        }




        /// <summary>Displays the dialog, allows the user to select a person and returns the ID the selected person.  Returns -1 if the user selected cancel or no person is selected.</summary>
        /// <param name="parentWindow">Specify the parent window</param>
        /// <param name="database">Specify the database to select a person from</param>
        /// <returns>ID of the person selected, or -1 for cancel</returns>
        public int selectPerson(IWin32Window parentWindow, Database database)
        {
            // Store the parameters.
            database_ = database;

            // Load a list of all people into the listbox.
            IdxName[] people = database.getPeople(ChooseSex.EITHER, family_tree.objects.SortOrder.DATE, 0, 9999);

            // Populate the list box.
            for (int i = 0; i < people.Length; i++)
            {
                lstPeople_.Items.Add(people[i]);
            }

            // Show the dialog.
            if (ShowDialog(parentWindow) == DialogResult.Cancel)
            {
                return -1;
            }

            // Check that a person is selected.
            if (lstPeople_.SelectedIndex < 0)
            {
                return -1;
            }

            // Find the selected person.
            IdxName selectedPerson = (IdxName)this.lstPeople_.SelectedItem;

            // Close the form.
            Dispose();

            // Return the selected person.
            return selectedPerson.idx;
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

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Button cmdOK;
            System.Windows.Forms.Button cmdCancel;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectPersonDialog));
            this.lstPeople_ = new System.Windows.Forms.ListBox();
            this.radioDate_ = new System.Windows.Forms.RadioButton();
            this.radioAlpha_ = new System.Windows.Forms.RadioButton();
            cmdOK = new System.Windows.Forms.Button();
            cmdCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstPeople_
            // 
            this.lstPeople_.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstPeople_.Location = new System.Drawing.Point(8, 48);
            this.lstPeople_.Name = "lstPeople_";
            this.lstPeople_.Size = new System.Drawing.Size(280, 277);
            this.lstPeople_.TabIndex = 0;
            this.lstPeople_.DoubleClick += new System.EventHandler(this.lstPeople_DoubleClick);
            // 
            // radioDate_
            // 
            this.radioDate_.Checked = true;
            this.radioDate_.Location = new System.Drawing.Point(16, 16);
            this.radioDate_.Name = "radioDate_";
            this.radioDate_.Size = new System.Drawing.Size(104, 24);
            this.radioDate_.TabIndex = 3;
            this.radioDate_.TabStop = true;
            this.radioDate_.Text = "Date Order";
            this.radioDate_.CheckedChanged += new System.EventHandler(this.radioDate_CheckedChanged);
            // 
            // radioAlpha_
            // 
            this.radioAlpha_.Location = new System.Drawing.Point(120, 16);
            this.radioAlpha_.Name = "radioAlpha_";
            this.radioAlpha_.Size = new System.Drawing.Size(128, 24);
            this.radioAlpha_.TabIndex = 4;
            this.radioAlpha_.Text = "Alphabetical Order";
            this.radioAlpha_.CheckedChanged += new System.EventHandler(this.radioAlpha_CheckedChanged);
            // 
            // cmdOK
            // 
            cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            cmdOK.Image = global::family_tree.viewer.Properties.Resources.OK;
            cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            cmdOK.Location = new System.Drawing.Point(114, 331);
            cmdOK.Name = "cmdOK";
            cmdOK.Size = new System.Drawing.Size(100, 30);
            cmdOK.TabIndex = 1;
            cmdOK.Text = "OK";
            cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmdCancel
            // 
            cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cmdCancel.Image = global::family_tree.viewer.Properties.Resources.Cancel;
            cmdCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            cmdCancel.Location = new System.Drawing.Point(8, 331);
            cmdCancel.Name = "cmdCancel";
            cmdCancel.Size = new System.Drawing.Size(100, 30);
            cmdCancel.TabIndex = 2;
            cmdCancel.Text = "Cancel";
            cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SelectPersonDialog
            // 
            this.AcceptButton = cmdOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.CancelButton = cmdCancel;
            this.ClientSize = new System.Drawing.Size(292, 368);
            this.Controls.Add(this.radioAlpha_);
            this.Controls.Add(this.radioDate_);
            this.Controls.Add(cmdCancel);
            this.Controls.Add(cmdOK);
            this.Controls.Add(this.lstPeople_);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectPersonDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Person";
            this.ResumeLayout(false);

        }
        #endregion

        #region Message Handlers



        private void radioDate_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radioDate_.Checked)
            {
                // Load a list of all people into the listbox.
                IdxName[] people = database_.getPeople(ChooseSex.EITHER, family_tree.objects.SortOrder.DATE, 0, 3000);

                // Populate the list box.
                lstPeople_.Items.Clear();
                for (int i = 0; i < people.Length; i++)
                {
                    lstPeople_.Items.Add(people[i]);
                }
            }
        }



        private void radioAlpha_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radioAlpha_.Checked)
            {
                // Load a list of all people into the listbox.
                IdxName[] people = database_.getPeople(ChooseSex.EITHER, family_tree.objects.SortOrder.ALPHABETICAL, 0, 3000);

                // Populate the list box
                lstPeople_.Items.Clear();
                for (int i = 0; i < people.Length; i++)
                {
                    lstPeople_.Items.Add(people[i]);
                }
            }
        }



        private void lstPeople_DoubleClick(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }



        #endregion
    }
}
