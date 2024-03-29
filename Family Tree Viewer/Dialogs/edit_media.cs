using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.IO;

using family_tree.objects;

namespace family_tree.viewer
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
        /// <param name="mediaIdx">Specifies the ID of an existing media object.</param>
        public EditMediaDialog(Database database, int mediaIdx)
        {
            InitializeComponent();

            // Save the input parameters
            if (mediaIdx > 0)
            {
                media_ = new Media(database, mediaIdx);
            }
            else
            {
                media_ = new Media(database);
            }
            mediaDirectory_ = database.getMediaDirectory();

            txtTitle_.Text = media_.title;
            txtFilename_.Text = media_.fileName;
            chkPrimary_.Checked = media_.isPrimary;
            chkThumbnail_.Checked = media_.isThumbnail;
            openImage(media_.fullFileName);

            // Populate the list of people combo box
            int[] attachedPeople = media_.getAttachedPeople();
            IdxName[] people = database.getPeople(ChooseSex.EITHER, family_tree.objects.SortOrder.DATE);
            foreach (IdxName person in people)
            {
                cboPeople_.Items.Add(person);
                foreach (int attachedIdx in attachedPeople)
                {
                    if (attachedIdx == person.idx)
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
        public int mediaIdx
        {
            get { return media_.idx_; }
        }



        #region Message Handlers



        /// <summary>Message handler for the OK button click.  Update the media object in the database.  The Framework handles closing the form etc.</summary>
        private void cmdOkClick(object sender, EventArgs e)
        {
            // Update the media object.
            media_.fileName = txtFilename_.Text;
            media_.title = txtTitle_.Text;
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
            media_.isPrimary = chkPrimary_.Checked;
            media_.isThumbnail = chkThumbnail_.Checked;

            // Update the attached people.
            media_.removeAllPeople();
            foreach (IdxName person in lstPeople_.Items)
            {
                media_.addPerson(person.idx);
            }

            // Save this media object.
            media_.save();
        }



        /// <summary>Message handler for the open button click.</summary>
        private void cmdOpenClick(object sender, EventArgs e)
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
        private void frmEditMediaLoad(object sender, EventArgs e)
        {
        }



        /// <summary>Message handler for the Add person button click.</summary>
        private void cmdAddPersonClick(object sender, EventArgs e)
        {
            IdxName person = (IdxName)cboPeople_.SelectedItem;
            if (person != null)
            {
                lstPeople_.Items.Add(person);
            }
        }



        /// <summary>Message handler for the remove person button click.</summary>
        private void cmdRemovePersonClick(object sender, EventArgs e)
        {
            if (lstPeople_.SelectedIndex >= 0)
            {
                lstPeople_.Items.RemoveAt(lstPeople_.SelectedIndex);
            }
        }



        #endregion

    }
}