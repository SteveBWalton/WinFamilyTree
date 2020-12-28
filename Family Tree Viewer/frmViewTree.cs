using System;
using System.Drawing;
// using System.Drawing.Imaging;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace FamilyTree.Viewer
{
    // Form to display clsTreeDocument objects.
    /// <summary>
    /// Form to display clsTreeDocument objects.
    /// </summary>
    public partial class frmViewTree : System.Windows.Forms.Form
	{
		#region Member Variables

        // Tree to display in this window.
        /// <summary>
        /// Tree to display in this window.
        /// </summary>
        private clsTreeDocument m_oTree;

        // The point at the centre of the window.
        /// <summary>
        /// The point at the centre of the window.
        /// </summary>
        private System.Drawing.Point m_Centre;

        // True if the user is dragging the display.
        /// <summary>
        /// True if the user is dragging the display.
        /// </summary>
        private bool m_bDragging;

        // The point that we are dragging from.
        /// <summary>
        /// The point that we are dragging from.
        /// </summary>
        private System.Drawing.Point m_DragPoint;

        #endregion

		#region Constructors etc ...

        // Constructor for the window that displays the specified tree document.
        /// <summary>
        /// Constructor for the window that displays the specified tree document.
        /// </summary>
		/// <param name="oTree">Specifies the tree document to display in the window</param>
        public frmViewTree(clsTreeDocument oTree)
        {
            // Required for Windows Form Designer support
            InitializeComponent();

            // Initialise member variables
            m_oTree = oTree;
            m_Centre.X = 0;
            m_Centre.Y = 0;

            // Initialise the form
            FamilyTree.Objects.clsPerson oPerson = new FamilyTree.Objects.clsPerson(oTree.BasePersonID, oTree.Database);
            Text = oPerson.GetName(true, true) + " - Tree";

            // Initialise the print document
            System.Drawing.Printing.Margins oMargins = new System.Drawing.Printing.Margins(40, 40, 40, 40);
            m_oPrintDocument.DefaultPageSettings.Margins = oMargins;
            m_oPrintDocument.DefaultPageSettings.Landscape = true;
        }

        // Clean up any resources being used.
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                if(components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

		#endregion

		#region Supporting Functions

        /// <summary>
        /// Display the dialog to allow the user to select an output file.
        /// If the user selected a file then write the tree options into the file.
        /// </summary>
        /// <returns>True for success, false otherwise.</returns>
        private bool Save()
        {
            // Set the common dialog options
            m_SaveFileDialog.Title = "Save Tree";
            m_SaveFileDialog.Filter = "Tree File (*.tree)|*.tree";
            m_SaveFileDialog.OverwritePrompt = true;
            m_SaveFileDialog.ValidateNames = true;
            m_SaveFileDialog.AddExtension = true;
            m_SaveFileDialog.DefaultExt = "tree";
            
            // Display the select save file dialog
            if(m_SaveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                // Create the save file
                Innoval.clsXmlDocument oSave = new Innoval.clsXmlDocument(m_SaveFileDialog.FileName);

                // Save the main person in the tree
                Innoval.clsXmlNode oTree = oSave.GetNode("tree");
                oTree.SetAttributeValue("mainperson",m_oTree.BasePersonID);
                oTree.SetAttributeValue("document",m_oTree.Database.Filename);

                // Save the options on the tree
                m_oTree.Options.Save(oSave);

                // Save the tree options
                oSave.Save(true);
            }

            // Return success
            return true;
        }
        
        /// <summary>
		/// Copies the tree onto the clipboard as a metafile
		/// </summary>
		/// <returns>True for success, false otherwise.</returns>
		private bool Copy()
		{
			// Create a metafile to record the drawing			
			System.Drawing.Graphics oGraphics = this.CreateGraphics();
			IntPtr hDC = oGraphics.GetHdc();
			System.Drawing.Imaging.Metafile oMetafile = new System.Drawing.Imaging.Metafile(hDC,System.Drawing.Imaging.EmfType.EmfPlusDual,"");			
			oGraphics.ReleaseHdc(hDC);
			oGraphics.Dispose();

			System.Drawing.Graphics oMetaGraphics = Graphics.FromImage(oMetafile);
			m_oTree.CalculatePositions(oMetaGraphics,enumDevice.Metafile);
			m_oTree.Draw(oMetaGraphics);
			oMetaGraphics.Dispose();

			// The following does actually work but no "real" application can understand .NET metafiles
			// Clipboard.SetDataObject(oMetafile,true);

			// This converts the .NET metafile into a Win32 metafile
			ClipboardMetafileHelper.PutEnhMetafileOnClipboard(this.Handle, oMetafile );

			oMetafile.Dispose();

			// return sucess
			return true;
		}

        /// <summary>
        /// Calculate the new position of the tree document.
        /// and draw the tree document at that position.
        /// </summary>
        private void TreeResized()
        {
            if(m_oTree == null)
            {
                return;
            }

            // Calculate if the window is wide enough to display the whole document
            if(m_PictureBox.Width > m_oTree.Width)
            {
                m_hScrollBar.Enabled = false;
                // Calculate the offset to centre the tree in the window.
                m_oTree.OffsetX = m_oTree.TopLeft.X - ((m_PictureBox.Width - m_oTree.Width) / 2);
                m_PictureBox.Refresh();
            }
            else
            {
                m_hScrollBar.Enabled = true;
                m_hScrollBar.Minimum = (int)m_oTree.TopLeft.X;
                m_hScrollBar.Maximum = (int)m_oTree.BottomRight.X;
                m_hScrollBar.LargeChange = m_PictureBox.Width;

                // Calculate the scroll bar position to try and keep the current centre at the centre
                SetHScrollBarValue(m_Centre.X - (m_PictureBox.Width / 2));
                m_oTree.OffsetX = m_hScrollBar.Value;
            }

            // Calculate if the window is high enough to display the whole document
            if(m_PictureBox.Height > m_oTree.Height)
            {
                m_vScrollBar.Enabled = false;
                // Calculate the offset to centre the tree in the window
                m_oTree.OffsetY = m_oTree.TopLeft.Y - ((m_PictureBox.Height - m_oTree.Height) / 2);
                m_PictureBox.Refresh();
            }
            else
            {
                m_vScrollBar.Enabled = true;
                m_vScrollBar.Minimum = (int)m_oTree.TopLeft.Y;
                m_vScrollBar.Maximum = (int)m_oTree.BottomRight.Y;
                m_vScrollBar.LargeChange = m_PictureBox.Height;

                // Calculate the scroll bar position to try and keep the current centre at the centre
                SetVScrollBarValue(m_Centre.Y - (m_PictureBox.Height / 2));                
                m_oTree.OffsetY = m_vScrollBar.Value;
            }
        }

        // Update the screen zoom level and redisplay the screen.
        /// <summary>
        /// Update the screen zoom level and redisplay the screen.
        /// </summary>
        /// <param name="nNewLevel"></param>
        private void SetNewZoomLevel(int nNewLevel)
        {
            if(nNewLevel > 0)
            {
                m_oTree.ScreenZoom = nNewLevel;
            }
            m_tsLabel.Text = "Zoom: " + m_oTree.ScreenZoom.ToString() + "%";
            
            // Redraw the tree
            m_oTree.Regenerate();
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
            if(nNewValue < m_hScrollBar.Minimum)
            {
                m_hScrollBar.Value = m_hScrollBar.Minimum;
            }
            else if(nNewValue > m_hScrollBar.Maximum)
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
            if(nNewValue < m_vScrollBar.Minimum)
            {
                m_vScrollBar.Value = m_vScrollBar.Minimum;
            }
            else if(nNewValue > m_vScrollBar.Maximum)
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
        private void frmViewTree_Shown(object sender,EventArgs e)
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
        private void frmViewTree_Resize(object sender,System.EventArgs e)
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
        private void menuSave_Click(object sender,EventArgs e)
        {
            Save();
        }

        /// <summary>
        /// Message handler for the File -> Print Preview menu point.
        /// and the print preview toolbar button click.
        /// </summary>
        private void menuPrintPreview_Click(object sender,System.EventArgs e)
        {
            PrintPreview();
        }

        /// <summary>
        /// Message handler for the File -> Close menu point click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuClose_Click(object sender,System.EventArgs e)
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
        private void menuOptions_Click(object sender,EventArgs e)
        {
            // Show the tree options dialog
            frmTreeOptions oDialog = new frmTreeOptions(m_oTree);
            if(oDialog.ShowDialog(this) == DialogResult.OK)
            {
                // Enact the new options
                m_oTree.Regenerate();
            }
        }

        /// <summary>
        /// Message handler for the Edit -> Copy menu point click
        /// and the toolbar Copy button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuCopy_Click(object sender,EventArgs e)
        {
            Copy();
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
            SetNewZoomLevel(m_oTree.ScreenZoom - 10);
        }

        // Message handler for the "View" -> "Zoom In" menu point click.
        /// <summary>
        /// Message handler for the "View" -> "Zoom In" menu point click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuZoomIn_Click(object sender, EventArgs e)
        {
            SetNewZoomLevel(m_oTree.ScreenZoom + 10);
        }

        #endregion

        #endregion

        #region Print and Print Preview

        /// <summary>Current page in the print job.
        /// </summary>
        private int	m_nPageNum;
		
		/// <summary>This function is called for each page in a print document.
		/// The static variable m_nPageNum counts the page.
		/// When the function decides that the current page is the last in the document then it sets m_nPageNum back to zero, assuming that the next time the function is called is a new print job starting from on page 1.
		/// </summary>
		private void PrintDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
		{
			// This switches the document into printer mode if not already in printer mode
			m_oTree.CalculatePositions(e.Graphics,enumDevice.Printer);

//			// This gives the page width
//			nPageWidth = e.PageSettings.PaperSize.Height; 

            // This give the width in the centre of the page that should be used
            int nPageWidth = e.MarginBounds.Width;			
			
			// Calculate the current offset in the document
			if(m_nPageNum==0)
			{
				m_oTree.OffsetX = m_oTree.TopLeft.X;
				m_oTree.OffsetY = m_oTree.TopLeft.Y;
			}
			else
			{				
				m_oTree.OffsetX = m_oTree.TopLeft.X + m_nPageNum * nPageWidth;
				m_oTree.OffsetY = m_oTree.TopLeft.Y;
			}

			// Allow for the printer margin
			m_oTree.OffsetX	-= e.MarginBounds.Left;
			m_oTree.OffsetY -= e.MarginBounds.Top;

			// Draw the document on the printer page			
			m_oTree.Draw(e.Graphics);

			// Decide if this is the last page
            if(m_oTree.OffsetX + e.MarginBounds.Left + nPageWidth < m_oTree.BottomRight.X)
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
            if(m_oTree.LastDevice != enumDevice.Screen)
            {
                m_oTree.CalculatePositions(e.Graphics,enumDevice.Screen);
                TreeResized();
            }
			m_oTree.Draw(e.Graphics);
		}

        // Message handler for the horizontal scroll bar value changing.
        /// <summary>
        /// Message handler for the horizontal scroll bar value changing.
        /// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void hScrollBar_ValueChanged(object sender, System.EventArgs e)
		{
            m_oTree.OffsetX = m_hScrollBar.Value;
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
			m_oTree.OffsetY = m_vScrollBar.Value;
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
            if(e.Button == MouseButtons.Left)
            {
                m_bDragging = true;
                m_DragPoint = e.Location;
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
            m_bDragging = false;
        }

        // Message handler for the picturebox mouse move event.
        /// <summary>
        /// Message handler for the picturebox mouse move event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if(m_bDragging)
            {
                if(m_hScrollBar.Enabled)
                {
                    if(e.Location.X != m_DragPoint.X)
                    {
                        SetHScrollBarValue(m_hScrollBar.Value - (e.Location.X - m_DragPoint.X));
                        m_DragPoint.X = e.Location.X;
                    }
                }
                if(m_vScrollBar.Enabled)
                {
                    if(e.Location.Y != m_DragPoint.Y)
                    {
                        SetVScrollBarValue(m_vScrollBar.Value - (e.Location.Y - m_DragPoint.Y));
                        m_DragPoint.Y = e.Location.Y;
                    }
                }
            }
        }
        
        #endregion
    }
}
