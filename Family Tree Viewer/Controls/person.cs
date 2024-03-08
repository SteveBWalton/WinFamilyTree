using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using family_tree.objects;

namespace family_tree.viewer
{
    /// <summary>Delegate type for the Click event.</summary>
    public delegate void FuncClick(object sender);

    /// <summary>User control to display a person.</summary>
    public class PersonDisplay : System.Windows.Forms.UserControl
    {
        #region Member Variables

        /// <summary>ID of the person to display.</summary>
        private int personIndex_;

        /// <summary>This is the click event for the control.</summary>
        public event FuncClick eventClick;

        private System.Windows.Forms.Label labName_;
        private System.Windows.Forms.Label labBorn_;

        /// <summary>Required designer variable.</summary>
        private System.ComponentModel.Container components = null;

        #endregion

        #region Public Functions



        /// <summary>Class Constructor.</summary>
        public PersonDisplay()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // Add any initialization after the InitializeComponent call
            labName_.Text = "Unknown";
            labBorn_.Text = "";
        }



        /// <summary>Set the person that the control should display.</summary>
        /// <param name="person">Specify the person object to be displayed</param>
        /// <returns>True for success, false otherwise.</returns>
        public bool setPerson(Person person)
        {
            personIndex_ = person.idx;
            labName_.Text = person.getName(false, true);
            labBorn_.Text = person.shortDescription(true);

            // Return success.
            return true;
        }



        /// <summary>Gets the ID of the person displayed.</summary>
        /// <returns></returns>
        public int getPersonIndex()
        {
            return personIndex_;
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

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labName_ = new System.Windows.Forms.Label();
            this.labBorn_ = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // m_labName
            // 
            this.labName_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labName_.Location = new System.Drawing.Point(0, 0);
            this.labName_.Name = "m_labName";
            this.labName_.Size = new System.Drawing.Size(150, 19);
            this.labName_.TabIndex = 0;
            this.labName_.Text = "label1";
            this.labName_.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labName_.Click += new System.EventHandler(this.labName_Click);
            // 
            // m_labBorn
            // 
            this.labBorn_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labBorn_.Location = new System.Drawing.Point(0, 20);
            this.labBorn_.Name = "m_labBorn";
            this.labBorn_.Size = new System.Drawing.Size(150, 44);
            this.labBorn_.TabIndex = 2;
            this.labBorn_.Text = "label2";
            this.labBorn_.Click += new System.EventHandler(this.labBorn_Click);
            // 
            // ucPerson
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.labBorn_);
            this.Controls.Add(this.labName_);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ucPerson";
            this.Size = new System.Drawing.Size(148, 78);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.personDisplay_Paint);
            this.Click += new System.EventHandler(this.personDisplay_Click);
            this.Resize += new System.EventHandler(this.personDisplay_Resize);
            this.ResumeLayout(false);

        }
        #endregion

        #region Message Handlers



        private void personDisplay_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            // e.Graphics.DrawLine(new Pen(Color.Black),0,0,this.Width,this.Height);
        }



        private void personDisplay_Resize(object sender, System.EventArgs e)
        {
            /*
            this.labBorn.Width = this.Width / 2;
            this.labDied.Width = this.labBorn.Width;
            this.labDied.Left = this.labBorn.Width;
            */
        }



        /// <summary>Message handler for the click event on the name label.  Raise the click event to the parent control.</summary>
        private void labName_Click(object sender, EventArgs e)
        {
            // Raise the click event in the parent.  Call the delegates that the parent has assigned.
            this.eventClick(this);
        }



        /// <summary>Message handler for the click event on the born label.  Raise the click event to the parent control.</summary>
        private void labBorn_Click(object sender, EventArgs e)
        {
            // Raise the click event in the parent.  Call the delegates that the parent has assigned.
            this.eventClick(this);
        }



        /// <summary>Message handler for the click event on the background of the control.  Raise the click event to the parent control.</summary>
        private void personDisplay_Click(object sender, EventArgs e)
        {
            // Raise the click event in the parent.  Call the delegates that the parent has assigned.
            this.eventClick(this);
        }



        #endregion

    }
}
