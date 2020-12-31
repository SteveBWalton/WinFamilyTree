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
    /// <summary>
    /// Form to edit a media object.
    /// Currently media objects have to be images.
    /// </summary>
    public partial class frmEditMedia : Form
    {
        #region Member Varibles

        /// <summary>The media object to edit.</summary>
        private clsMedia m_oMedia;

        /// <summary>The bitmap image of the specified media.</summary>
        private Bitmap m_oImage;

        /// <summary>The directory that contains the media files.</summary>
        private string m_sMediaDirectory;

        #endregion

        #region Class Constructors etc ...

        /// <summary>
        /// Initialise the form to edit an existing media object.
        /// </summary>
        /// <param name="oDb">Specifies the database to load / save the media object in.</param>
        /// <param name="nMediaID">Specifies the ID of an existing media object.</param>
        public frmEditMedia
            (
            Database oDb,
            int nMediaID
            )
        {
            InitializeComponent();
            
            // Save the input parameters
            if(nMediaID > 0)
            {
                m_oMedia = new clsMedia(oDb,nMediaID);
            }
            else
            {
                m_oMedia = new clsMedia(oDb);
            }
            m_sMediaDirectory = oDb.getMediaDirectory();

            m_txtTitle.Text  = m_oMedia.Title;
            m_txtFilename.Text = m_oMedia.Filename;
            m_chkPrimary.Checked = m_oMedia.Primary;
            m_chkThumbnail.Checked = m_oMedia.Thumbnail;
            OpenImage(m_oMedia.FullFilename);

            // Populate the list of people combo box
            int[] oAttachedPeople = m_oMedia.GetAttachedPeople();
            IndexName[] oPeople = oDb.getPeople(ChooseSex.EITHER, Objects.SortOrder.DATE);
            foreach(IndexName oPerson in oPeople)
            {
                m_cboPeople.Items.Add(oPerson);
                foreach(int nAttachedID in oAttachedPeople)
                {
                    if(nAttachedID == oPerson.index)
                    {
                        m_lstPeople.Items.Add(oPerson);
                    }
                }
            }
        }

        /// <summary>
        /// Initialise the form to edit a new media object.
        /// </summary>
        /// <param name="oDb">Specifies the database to add the media object to.</param>
        public frmEditMedia
            (
            Database oDb
            ) : this(oDb,0)
        {
        }

        #endregion

        /// <summary>
        /// Opens the specified image file onto the media form.
        /// </summary>
        /// <param name="sFilename">Specifies the full filename of the image file.</param>
        private void OpenImage
            (
            string sFilename
            )
        {
            try
            {
                m_oImage = new Bitmap(sFilename);
            }
            catch
            {
                m_oImage = null;
            }
            if(m_oImage != null)
            {
                m_PictureBox.Image = m_oImage;
                m_txtWidth.Text = m_oImage.Width.ToString();
                m_txtHeight.Text = m_oImage.Height.ToString();
            }
            else
            {
                m_PictureBox.Image = null;
                m_txtWidth.Text = "";
                m_txtHeight.Text = "";
            }
        }

        /// <summary>
        /// The ID of the media object on the form
        /// </summary>
        public int MediaID
        {
            get { return m_oMedia.ID; }
        }

        #region Message Handlers

        /// <summary>
        /// Message handler for the OK button click.
        /// Update the media object in the database.
        /// The Framework handles closing the form etc.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdOK_Click(object sender,EventArgs e)
        {
            // Update the media object
            m_oMedia.Filename = m_txtFilename.Text;
            m_oMedia.Title = m_txtTitle.Text;
            try
            {
                m_oMedia.Width = int.Parse(m_txtWidth.Text);
            }
            catch
            {
                m_oMedia.Width = -1;
            }
            try
            {
                m_oMedia.Height = int.Parse(m_txtHeight.Text);
            }
            catch
            {
                m_oMedia.Height = -1;
            }
            m_oMedia.Primary = m_chkPrimary.Checked;
            m_oMedia.Thumbnail = m_chkThumbnail.Checked;

            // Update the attached people
            m_oMedia.RemoveAllPeople();
            foreach(IndexName oPerson in m_lstPeople.Items)
            {
                m_oMedia.AddPerson(oPerson.index);
            }

            // Save this media object
            m_oMedia.Save();
        }

        /// <summary>
        /// Message handler for the open button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdOpen_Click(object sender,EventArgs e)
        {
            // Initialise the Open File Dialog
            m_OpenFileDialog.Title = "Select Media File";
            m_OpenFileDialog.Filter = "jpeg files (*.jpg)|*.jpg|All Files (*.*)|*.*";
            m_OpenFileDialog.CheckFileExists = true;
            m_OpenFileDialog.InitialDirectory = m_sMediaDirectory;

            // Show the dialog and allow the user to select the file
            if(m_OpenFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                m_txtFilename.Text = Path.GetFileName(m_OpenFileDialog.FileName);
                OpenImage(m_sMediaDirectory + "\\" + m_txtFilename.Text);
            }
        }

        #endregion

        /// <summary>
        /// Message handler for the form load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmEditMedia_Load(object sender,EventArgs e)
        {
        }

        /// <summary>
        /// Message handler for the Add person button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdAddPerson_Click(object sender,EventArgs e)
        {
            IndexName oPerson = (IndexName)m_cboPeople.SelectedItem;
            if(oPerson != null)
            {
                m_lstPeople.Items.Add(oPerson);
            }
        }

        /// <summary>
        /// Message handler for the remove person button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdRemovePerson_Click(object sender,EventArgs e)
        {
            if(m_lstPeople.SelectedIndex >= 0)
            {
                m_lstPeople.Items.RemoveAt(m_lstPeople.SelectedIndex);
            }
        }
    }
}