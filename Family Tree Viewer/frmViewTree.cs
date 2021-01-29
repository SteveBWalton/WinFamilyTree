using System;
using System.Drawing;
// using System.Drawing.Imaging;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace FamilyTree.Viewer
{
    /// <summary>Form to display TreeDocument objects.</summary>
    public partial class frmViewTree : System.Windows.Forms.Form
    {
        #region Member Variables

        /// <summary>Tree to display in this window.</summary>
        private TreeDocument tree_;

        /// <summary>The point at the centre of the window.</summary>
        private System.Drawing.Point centre_;

        /// <summary>True if the user is dragging the display.</summary>
        private bool isDragging_;

        /// <summary>The point that we are dragging from.</summary>
        private System.Drawing.Point dragPoint_;

        #endregion

        #region Constructors etc ...

        /// <summary>Constructor for the window that displays the specified tree document.</summary>
        /// <param name="tree">Specifies the tree document to display in the window</param>
        public frmViewTree(TreeDocument tree)
        {
            // Required for Windows Form Designer support
            InitializeComponent();

            // Initialise member variables
            tree_ = tree;
            centre_.X = 0;
            centre_.Y = 0;

            // Initialise the form.
            FamilyTree.Objects.Person person = new FamilyTree.Objects.Person(tree.basePersonIndex, tree.database);
            Text = person.getName(true, true) + " - Tree";

            // Initialise the print document.
            System.Drawing.Printing.Margins margins = new System.Drawing.Printing.Margins(40, 40, 40, 40);
            m_oPrintDocument.DefaultPageSettings.Margins = margins;
            m_oPrintDocument.DefaultPageSettings.Landscape = true;
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

        #region Supporting Functions



        /// <summary>Display the dialog to allow the user to select an output file.  If the user selected a file then write the tree options into the file.</summary>
        /// <returns>True for success, false otherwise.</returns>
        private bool save()
        {
            // Set the common dialog options.
            saveFileDialog_.Title = "Save Tree";
            saveFileDialog_.Filter = "Tree File (*.tree)|*.tree";
            saveFileDialog_.OverwritePrompt = true;
            saveFileDialog_.ValidateNames = true;
            saveFileDialog_.AddExtension = true;
            saveFileDialog_.DefaultExt = "tree";

            // Display the select save file dialog.
            if (saveFileDialog_.ShowDialog(this) == DialogResult.OK)
            {
                // Create the save file.
                walton.XmlDocument xmlSave = new walton.XmlDocument(saveFileDialog_.FileName);

                // Save the main person in the tree.
                walton.XmlNode xxmlTree = xmlSave.getNode("tree");
                xxmlTree.setAttributeValue("mainperson", tree_.basePersonIndex);
                xxmlTree.setAttributeValue("document", tree_.database.fileName);

                // Save the options on the tree.
                tree_.options.save(xmlSave);

                // Save the tree options.
                xmlSave.save(true);
            }

            // Return success.
            return true;
        }



        /// <summary>Copies the tree onto the clipboard as a metafile.</summary>
        /// <returns>True for success, false otherwise.</returns>
        private bool copy()
        {
            // Create a metafile to record the drawing.
            System.Drawing.Graphics graphics = this.CreateGraphics();
            IntPtr hDC = graphics.GetHdc();
            System.Drawing.Imaging.Metafile metafile = new System.Drawing.Imaging.Metafile(hDC, System.Drawing.Imaging.EmfType.EmfPlusDual, "");
            graphics.ReleaseHdc(hDC);
            graphics.Dispose();

            System.Drawing.Graphics metaGraphics = Graphics.FromImage(metafile);
            tree_.calculatePositions(metaGraphics, DisplayDevice.METAFILE);
            tree_.draw(metaGraphics);
            metaGraphics.Dispose();

            // The following does actually work but no "real" application can understand .NET metafiles
            // Clipboard.SetDataObject(oMetafile,true);

            // This converts the .NET metafile into a Win32 metafile.
            ClipboardMetafileHelper.putEnhMetafileOnClipboard(this.Handle, metafile);

            metafile.Dispose();

            // return sucess
            return true;
        }

        /// <summary>
        /// Calculate the new position of the tree document.
        /// and draw the tree document at that position.
        /// </summary>
        private void TreeResized()
        {
            if (tree_ == null)
            {
                return;
            }

            // Calculate if the window is wide enough to display the whole document
            if (m_PictureBox.Width > tree_.width)
            {
                m_hScrollBar.Enabled = false;
                // Calculate the offset to centre the tree in the window.
                tree_.offsetX = tree_.topLeft.X - ((m_PictureBox.Width - tree_.width) / 2);
                m_PictureBox.Refresh();
            }
            else
            {
                m_hScrollBar.Enabled = true;
                m_hScrollBar.Minimum = (int)tree_.topLeft.X;
                m_hScrollBar.Maximum = (int)tree_.bottomRight.X;
                m_hScrollBar.LargeChange = m_PictureBox.Width;

                // Calculate the scroll bar position to try and keep the current centre at the centre
                SetHScrollBarValue(centre_.X - (m_PictureBox.Width / 2));
                tree_.offsetX = m_hScrollBar.Value;
            }

            // Calculate if the window is high enough to display the whole document
            if (m_PictureBox.Height > tree_.height)
            {
                m_vScrollBar.Enabled = false;
                // Calculate the offset to centre the tree in the window
                tree_.offsetY = tree_.topLeft.Y - ((m_PictureBox.Height - tree_.height) / 2);
                m_PictureBox.Refresh();
            }
            else
            {
                m_vScrollBar.Enabled = true;
                m_vScrollBar.Minimum = (int)tree_.topLeft.Y;
                m_vScrollBar.Maximum = (int)tree_.bottomRight.Y;
                m_vScrollBar.LargeChange = m_PictureBox.Height;

                // Calculate the scroll bar position to try and keep the current centre at the centre
                SetVScrollBarValue(centre_.Y - (m_PictureBox.Height / 2));
                tree_.offsetY = m_vScrollBar.Value;
            }
        }

        // Update the screen zoom level and redisplay the screen.
        /// <summary>
        /// Update the screen zoom level and redisplay the screen.
        /// </summary>
        /// <param name="nNewLevel"></param>
        private void SetNewZoomLevel(int nNewLevel)
        {
            if (nNewLevel > 0)
            {
                tree_.screenZoom = nNewLevel;
            }
            m_tsLabel.Text = "Zoom: " + tree_.screenZoom.ToString() + "%";

            // Redraw the tree
            tree_.regenerate();
            TreeResized();
            Refresh();
        }

        // Set the Horizontal scroll bar (m_vScrollbar) value within minimum and maximum.
        /// <summary>
        /// Set the Horizontal scroll bar (m_vScrollbar) value within minimum and maximum.
        /// </summary>
        /// <param name="nNewValue"></param>
        private void SetHScrollBarValue(int nNewValue)
        {
            if (nNewValue < m_hScrollBar.Minimum)
            {
                m_hScrollBar.Value = m_hScrollBar.Minimum;
            }
            else if (nNewValue > m_hScrollBar.Maximum)
            {
                m_hScrollBar.Value = m_hScrollBar.Maximum;
            }
            else
            {
                m_hScrollBar.Value = nNewValue;
            }
        }

        // Set the Vertical scroll bar (m_vScrollbar) value within minimum and maximum.
        /// <summary>
        /// Set the Vertical scroll bar (m_vScrollbar) value within minimum and maximum.
        /// </summary>
        /// <param name="nNewValue"></param>
        private void SetVScrollBarValue(int nNewValue)
        {
            if (nNewValue < m_vScrollBar.Minimum)
            {
                m_vScrollBar.Value = m_vScrollBar.Minimum;
            }
            else if (nNewValue > m_vScrollBar.Maximum)
            {
                m_vScrollBar.Value = m_vScrollBar.Maximum;
            }
            else
            {
                m_vScrollBar.Value = nNewValue;
            }
        }

        #endregion

        #region Message Handlers

        #region Form Events

        // Message handler for the form Shown event.
        /// <summary>
        /// Message handler for the form Shown event.
        /// This is post appear load event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmViewTree_Shown(object sender, EventArgs e)
        {
            // Resize the tree now the window has appeared.
            TreeResized();
        }

        // When the form resizes, resize the tree document.
        /// <summary>
        /// When the form resizes, resize the tree document.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmViewTree_Resize(object sender, System.EventArgs e)
        {
            // Well the tree sizing is the same difference
            TreeResized();
        }

        #endregion

        #region Menu System

        /// <summary>
        /// Message handler for the File -> Save menu point click.
        /// Prompt the user for a filename and save the tree settings.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuSave_Click(object sender, EventArgs e)
        {
            save();
        }

        /// <summary>
        /// Message handler for the File -> Print Preview menu point.
        /// and the print preview toolbar button click.
        /// </summary>
        private void menuPrintPreview_Click(object sender, System.EventArgs e)
        {
            PrintPreview();
        }

        /// <summary>
        /// Message handler for the File -> Close menu point click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuClose_Click(object sender, System.EventArgs e)
        {
            // Close this window
            Close();
        }

        /// <summary>
        /// Message handler for the Edit -> Options menu point click.
        /// Display the edit options dialog and enact any changes made.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuOptions_Click(object sender, EventArgs e)
        {
            // Show the tree options dialog
            TreeOptionsDialog oDialog = new TreeOptionsDialog(tree_);
            if (oDialog.ShowDialog(this) == DialogResult.OK)
            {
                // Enact the new options
                tree_.regenerate();
            }
        }

        /// <summary>
        /// Message handler for the Edit -> Copy menu point click
        /// and the toolbar Copy button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuCopy_Click(object sender, EventArgs e)
        {
            copy();
        }

        #region View Menu

        // Message handler for the "View" -> "Zoom Reset" menu point click
        /// <summary>
        /// Message handler for the "View" -> "Zoom Reset" menu point click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuZoomReset_Click(object sender, EventArgs e)
        {
            SetNewZoomLevel(100);
        }

        // Message handler for the "View" -> "Zoom Out" menu point click.
        /// <summary>
        /// Message handler for the "View" -> "Zoom Out" menu point click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuZoomOut_Click(object sender, EventArgs e)
        {
            SetNewZoomLevel(tree_.screenZoom - 10);
        }

        // Message handler for the "View" -> "Zoom In" menu point click.
        /// <summary>
        /// Message handler for the "View" -> "Zoom In" menu point click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuZoomIn_Click(object sender, EventArgs e)
        {
            SetNewZoomLevel(tree_.screenZoom + 10);
        }

        #endregion

        #endregion

        #region Print and Print Preview

        /// <summary>Current page in the print job.
        /// </summary>
        private int m_nPageNum;

        /// <summary>This function is called for each page in a print document.
        /// The static variable m_nPageNum counts the page.
        /// When the function decides that the current page is the last in the document then it sets m_nPageNum back to zero, assuming that the next time the function is called is a new print job starting from on page 1.
        /// </summary>
        private void PrintDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            // This switches the document into printer mode if not already in printer mode
            tree_.calculatePositions(e.Graphics, DisplayDevice.PRINTER);

            //			// This gives the page width
            //			nPageWidth = e.PageSettings.PaperSize.Height; 

            // This give the width in the centre of the page that should be used
            int nPageWidth = e.MarginBounds.Width;

            // Calculate the current offset in the document
            if (m_nPageNum == 0)
            {
                tree_.offsetX = tree_.topLeft.X;
                tree_.offsetY = tree_.topLeft.Y;
            }
            else
            {
                tree_.offsetX = tree_.topLeft.X + m_nPageNum * nPageWidth;
                tree_.offsetY = tree_.topLeft.Y;
            }

            // Allow for the printer margin
            tree_.offsetX -= e.MarginBounds.Left;
            tree_.offsetY -= e.MarginBounds.Top;

            // Draw the document on the printer page			
            tree_.draw(e.Graphics);

            // Decide if this is the last page
            if (tree_.offsetX + e.MarginBounds.Left + nPageWidth < tree_.bottomRight.X)
            {
                e.HasMorePages = true;
                m_nPageNum++;
            }
            else
            {
                e.HasMorePages = false;
                m_nPageNum = 0;
            }
        }

        /// <summary>Displays the print preview window.
        /// </summary>
        private void PrintPreview()
        {
            // Switch the document to landscape
            // this.printDocument1.DefaultPageSettings.Landscape = true;			

            // Start with first page
            m_nPageNum = 0;

            // Copied these settings from the PrintPreviewDialog object that the framework made (deleted now).
            // However, the dialog is unloaded so the second time this function was called the framework object would fail.
            // So I made my own object here

            // Show the print preview dialog
            PrintPreviewDialog oPrintPreviewDialog = new PrintPreviewDialog();
            oPrintPreviewDialog.AutoScrollMargin = new System.Drawing.Size(0, 0);
            oPrintPreviewDialog.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            oPrintPreviewDialog.ClientSize = new System.Drawing.Size(400, 300);
            oPrintPreviewDialog.Document = this.m_oPrintDocument;
            oPrintPreviewDialog.Enabled = true;
            //            oPrintPreviewDialog.Icon = ((System.Drawing.Icon)(resources.GetObject("m_PrintPreviewDialog.Icon")));
            oPrintPreviewDialog.Name = "printPreviewDialog1";
            oPrintPreviewDialog.Visible = false;
            oPrintPreviewDialog.Show(this);
        }

        #endregion

        // Draw the tree document on the picture box.
        /// <summary>
        /// Draw the tree document on the picture box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PictureBox_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (tree_.lastDevice != DisplayDevice.SCREEN)
            {
                tree_.calculatePositions(e.Graphics, DisplayDevice.SCREEN);
                TreeResized();
            }
            tree_.draw(e.Graphics);
        }

        // Message handler for the horizontal scroll bar value changing.
        /// <summary>
        /// Message handler for the horizontal scroll bar value changing.
        /// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void hScrollBar_ValueChanged(object sender, System.EventArgs e)
        {
            tree_.offsetX = m_hScrollBar.Value;
            m_PictureBox.Refresh();
        }

        // Message handler for the vertical scroll bar value chaning.
        /// <summary>
        /// Message handler for the vertical scroll bar value chaning.
        /// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void vScrollBar_ValueChanged(object sender, System.EventArgs e)
        {
            tree_.offsetY = m_vScrollBar.Value;
            m_PictureBox.Refresh();
        }

        // Message handler for the picturebox mouse down event.
        /// <summary>
        /// Message handler for the picturebox mouse down event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging_ = true;
                dragPoint_ = e.Location;
            }
        }

        // Message handler for the picturebox mouse up event.
        /// <summary>
        /// Message handler for the picturebox mouse up event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging_ = false;
        }

        // Message handler for the picturebox mouse move event.
        /// <summary>
        /// Message handler for the picturebox mouse move event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging_)
            {
                if (m_hScrollBar.Enabled)
                {
                    if (e.Location.X != dragPoint_.X)
                    {
                        SetHScrollBarValue(m_hScrollBar.Value - (e.Location.X - dragPoint_.X));
                        dragPoint_.X = e.Location.X;
                    }
                }
                if (m_vScrollBar.Enabled)
                {
                    if (e.Location.Y != dragPoint_.Y)
                    {
                        SetVScrollBarValue(m_vScrollBar.Value - (e.Location.Y - dragPoint_.Y));
                        dragPoint_.Y = e.Location.Y;
                    }
                }
            }
        }

        #endregion
    }
}
