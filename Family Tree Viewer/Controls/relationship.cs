using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using FamilyTree.Objects;

namespace FamilyTree.Viewer
{
    /// <summary>User control to display the relationship between 2 people.</summary>
    public class RelationshipDisplay : System.Windows.Forms.UserControl
    {
        #region Member Variables

        /// <summary>Terminated status for the relationship.</summary>
        private int terminated_;

        /// <summary>ID of the male in the relationship.</summary>
        private int fatherIndex_;

        /// <summary>ID of the female in the relationship.</summary>
        private int motherIndex_;

        /// <summary>Required designer variable.</summary>
        private System.ComponentModel.Container components = null;

        #endregion

        #region Constructors



        /// <summary>Class constructor.</summary>
        public RelationshipDisplay()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // Add any initialization after the InitializeComponent call
            terminated_ = 1;
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

        #region Public Functions



        /// <summary>Set the relationship shown in the control.</summary>
        /// <param name="relationship">Specify the relationship to show in the control.</param>
        /// <returns>True for success.  False, otherwise.</returns>
        public bool setRelationship(Relationship relationship)
        {
            // Save the values that this control needs.
            terminated_ = relationship.terminatedIndex;
            fatherIndex_ = relationship.maleIndex;
            motherIndex_ = relationship.femaleIndex;

            // Return success.
            return true;
        }



        #endregion

        #region Properties

        /// <summary>ID of the male in the relationship.</summary>
        public int fatherIndex { get { return fatherIndex_; } }

        /// <summary>ID of the female in the relationship.</summary>
        public int motherIndex { get { return motherIndex_; } }

        #endregion

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            // 
            // ucRelationship
            // 
            this.Name = "ucRelationship";
            this.Size = new System.Drawing.Size(176, 144);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.relationshipDisplay_Paint);

        }
        #endregion

        #region Message Handlers

        /// <summary>Message handler for the paint event.  Draw the relationship sybmol.</summary>
        private void relationshipDisplay_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            // Create a pen.
            Pen blackPen = new Pen(Color.Black, 2);

            // Draw a marriage symbol.
            e.Graphics.DrawLine(blackPen, e.ClipRectangle.Left, 6, e.ClipRectangle.Right, 6);
            e.Graphics.DrawLine(blackPen, e.ClipRectangle.Left, 9, e.ClipRectangle.Right, 9);

            // Draw the devorce line.
            if (terminated_ == 2)
            {
                blackPen = new Pen(Color.Black, 1);
                e.Graphics.DrawLine(blackPen, e.ClipRectangle.Left, e.ClipRectangle.Bottom, e.ClipRectangle.Right, e.ClipRectangle.Top);
            }
        }

        #endregion
    }
}
