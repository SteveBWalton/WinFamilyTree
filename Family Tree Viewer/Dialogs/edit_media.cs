using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.IO;

using FamilyTree.Objects;

namespace FamilyTree.Viewer
{
    /// <summary>Form to edit a media object.  Currently media objects have to be images.</summary>
    public partial class EditMediaDialog : Form
    {
        #region Member Varibles

        /// <summary>The media object to edit.</summary>
        private Media media_;

        /// <summary>The bitmap image of the specified media.</summary>
        private Bitmap image_;

        /// <summary>The directory that contains the media files.</summary>
        private string mediaDirectory_;

        #endregion

        #region Class Constructors etc ...



        /// <summary>Initialise the form to edit an existing media object.</summary>
        /// <param name="database">Specifies the database to load / save the media object in.</param>
        /// <param name="mediaIndex">Specifies the ID of an existing media object.</param>
        public EditMediaDialog(Database database, int mediaIndex)
        {
            InitializeComponent();

            // Save the input parameters
            if (mediaIndex > 0)
            {
                media_ = new Media(database, mediaIndex);
            }
            else
            {
                media_ = new Media(database);
            }
            mediaDirectory_ = database.getMediaDirectory();

            m_txtTitle.Text = media_.title;
            txtFilename_.Text = media_.fileName;
            m_chkPrimary.Checked = media_.isPrimary;
            m_chkThumbnail.Checked = media_.isThumbnail;
            openImage(media_.fullFileName);

            // Populate the list of people combo box
            int[] attachedPeople = media_.getAttachedPeople();
            IndexName[] people = database.getPeople(ChooseSex.EITHER, Objects.SortOrder.DATE);
            foreach (IndexName person in people)
            {
                cboPeople_.Items.Add(person);
                foreach (int attachedIndex in attachedPeople)
                {
                    if (attachedIndex == person.index)
                    {
                        lstPeople_.Items.Add(person);
                    }
                }
            }
        }



        /// <summary>Initialise the form to edit a new media object.</summary>
        /// <param name="database">Specifies the database to add the media object to.</param>
        public EditMediaDialog(Database database) : this(database, 0)
        {
        }



        #endregion



        /// <summary>Opens the specified image file onto the media form.</summary>
        /// <param name="fileName">Specifies the full filename of the image file.</param>
        private void openImage(string fileName)
        {
            try
            {
                image_ = new Bitmap(fileName);
            }
            catch
            {
                image_ = null;
            }
            if (image_ != null)
            {
                pictureBox_.Image = image_;
                txtWidth_.Text = image_.Width.ToString();
                txtHeight_.Text = image_.Height.ToString();
            }
            else
            {
                pictureBox_.Image = null;
                txtWidth_.Text = "";
                txtHeight_.Text = "";
            }
        }



        /// <summary>The ID of the media object on the form.</summary>
        public int mediaIndex
        {
            get { return media_.index_; }
        }



        #region Message Handlers



        /// <summary>Message handler for the OK button click.  Update the media object in the database.  The Framework handles closing the form etc.</summary>
        private void cmdOK_Click(object sender, EventArgs e)
        {
            // Update the media object.
            media_.fileName = txtFilename_.Text;
            media_.title = m_txtTitle.Text;
            try
            {
                media_.width = int.Parse(txtWidth_.Text);
            }
            catch
            {
                media_.width = -1;
            }
            try
            {
                media_.height = int.Parse(txtHeight_.Text);
            }
            catch
            {
                media_.height = -1;
            }
            media_.isPrimary = m_chkPrimary.Checked;
            media_.isThumbnail = m_chkThumbnail.Checked;

            // Update the attached people.
            media_.removeAllPeople();
            foreach (IndexName person in lstPeople_.Items)
            {
                media_.addPerson(person.index);
            }

            // Save this media object.
            media_.save();
        }



        /// <summary>Message handler for the open button click.</summary>
        private void cmdOpen_Click(object sender, EventArgs e)
        {
            // Initialise the Open File Dialog
            openFileDialog_.Title = "Select Media File";
            openFileDialog_.Filter = "jpeg files (*.jpg)|*.jpg|All Files (*.*)|*.*";
            openFileDialog_.CheckFileExists = true;
            openFileDialog_.InitialDirectory = mediaDirectory_;

            // Show the dialog and allow the user to select the file
            if (openFileDialog_.ShowDialog(this) == DialogResult.OK)
            {
                txtFilename_.Text = Path.GetFileName(openFileDialog_.FileName);
                openImage(mediaDirectory_ + "\\" + txtFilename_.Text);
            }
        }



        /// <summary>Message handler for the form load event.</summary>
        private void frmEditMedia_Load(object sender, EventArgs e)
        {
        }



        /// <summary>Message handler for the Add person button click.</summary>
        private void cmdAddPerson_Click(object sender, EventArgs e)
        {
            IndexName person = (IndexName)cboPeople_.SelectedItem;
            if (person != null)
            {
                lstPeople_.Items.Add(person);
            }
        }



        /// <summary>Message handler for the remove person button click.</summary>
        private void cmdRemovePerson_Click(object sender, EventArgs e)
        {
            if (lstPeople_.SelectedIndex >= 0)
            {
                lstPeople_.Items.RemoveAt(lstPeople_.SelectedIndex);
            }
        }



        #endregion

    }
}