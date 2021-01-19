namespace FamilyTree.Viewer
{
    public partial class AgeDialog : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboPerson;
        private FamilyTree.Viewer.CompoundDateEditBox ucDate1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labTheAge_;
        private System.Windows.Forms.Label labDoB_;
        private System.Windows.Forms.Button cmdOK;
        private System.ComponentModel.IContainer components;

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ImageList oImageList16;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AgeDialog));
            FamilyTree.Objects.CompoundDate clsDate1 = new FamilyTree.Objects.CompoundDate();
            this.label1 = new System.Windows.Forms.Label();
            this.cboPerson = new System.Windows.Forms.ComboBox();
            this.labDoB_ = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.labTheAge_ = new System.Windows.Forms.Label();
            this.ucDate1 = new FamilyTree.Viewer.CompoundDateEditBox();
            this.cmdOK = new System.Windows.Forms.Button();
            oImageList16 = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // oImageList16
            // 
            oImageList16.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("oImageList16.ImageStream")));
            oImageList16.TransparentColor = System.Drawing.Color.Silver;
            oImageList16.Images.SetKeyName(0, "");
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Person:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboPerson
            // 
            this.cboPerson.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPerson.Location = new System.Drawing.Point(112, 8);
            this.cboPerson.Name = "cboPerson";
            this.cboPerson.Size = new System.Drawing.Size(280, 21);
            this.cboPerson.TabIndex = 1;
            this.cboPerson.SelectedIndexChanged += new System.EventHandler(this.cboPerson_SelectedIndexChanged);
            // 
            // labDoB
            // 
            this.labDoB_.Location = new System.Drawing.Point(112, 32);
            this.labDoB_.Name = "labDoB";
            this.labDoB_.Size = new System.Drawing.Size(208, 23);
            this.labDoB_.TabIndex = 3;
            this.labDoB_.Text = "labDoB";
            this.labDoB_.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(8, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "Date of Birth:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(8, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 23);
            this.label4.TabIndex = 5;
            this.label4.Text = "Date:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(8, 96);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 23);
            this.label5.TabIndex = 6;
            this.label5.Text = "Age:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labTheAge
            // 
            this.labTheAge_.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labTheAge_.Location = new System.Drawing.Point(112, 96);
            this.labTheAge_.Name = "labTheAge";
            this.labTheAge_.Size = new System.Drawing.Size(144, 23);
            this.labTheAge_.TabIndex = 7;
            this.labTheAge_.Text = "label6";
            this.labTheAge_.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ucDate1
            // 
            this.ucDate1.Location = new System.Drawing.Point(112, 64);
            this.ucDate1.Name = "ucDate1";
            this.ucDate1.Size = new System.Drawing.Size(144, 24);
            this.ucDate1.TabIndex = 4;
            clsDate1.date = new System.DateTime(2008, 2, 20, 0, 0, 0, 0);
            clsDate1.status = 0;
            this.ucDate1.theDate = clsDate1;
            this.ucDate1.eventValueChanged += new FamilyTree.Viewer.FuncValueChanged(this.ucDate1_evtValueChanged);
            // 
            // cmdOK
            // 
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Image = global::FamilyTree.Viewer.Properties.Resources.OK;
            this.cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cmdOK.Location = new System.Drawing.Point(317, 92);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 27);
            this.cmdOK.TabIndex = 8;
            this.cmdOK.Text = "OK";
            this.cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frmAge
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(400, 125);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.labTheAge_);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ucDate1);
            this.Controls.Add(this.labDoB_);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboPerson);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAge";
            this.Text = "Calculate Age";
            this.ResumeLayout(false);

        }
        #endregion

    }
}
