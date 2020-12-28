using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using FTObjects;

namespace Family_Tree_Viewer
{
	/// <summary>
	/// Dialog to show the recent changes in the database.
	/// </summary>
	public class frmRecentChanges : System.Windows.Forms.Form
	{
		#region Member Variables

		private System.Windows.Forms.Button m_cmdOK;
		private System.Windows.Forms.Button m_cmdCancel;
		private System.Windows.Forms.DataGrid m_oGrid;
        private WebBrowser m_oWebBrowser;

		#endregion
        private ImageList m_oImageList16x16;
        private Button button1;
        private Button button2;
        private IContainer components;

		#region Constructors etc ...
		
		/// <summary>
		/// Class constructor.
		/// </summary>
		public frmRecentChanges
			(
			clsDatabase oDb
			)
		{
			// Required for Windows Form Designer support
			InitializeComponent();

			// Populate the grid
			m_oGrid.SetDataBinding(oDb.GetRecentChanges(),"Table");
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRecentChanges));
            this.m_cmdOK = new System.Windows.Forms.Button();
            this.m_cmdCancel = new System.Windows.Forms.Button();
            this.m_oGrid = new System.Windows.Forms.DataGrid();
            this.m_oWebBrowser = new System.Windows.Forms.WebBrowser();
            this.m_oImageList16x16 = new System.Windows.Forms.ImageList(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.m_oGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // m_cmdOK
            // 
            this.m_cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.m_cmdOK.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.m_cmdOK.ImageIndex = 0;
            this.m_cmdOK.ImageList = this.m_oImageList16x16;
            this.m_cmdOK.Location = new System.Drawing.Point(568,412);
            this.m_cmdOK.Name = "m_cmdOK";
            this.m_cmdOK.Size = new System.Drawing.Size(75,32);
            this.m_cmdOK.TabIndex = 0;
            this.m_cmdOK.Text = "OK";
            this.m_cmdOK.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_cmdCancel
            // 
            this.m_cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_cmdCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.m_cmdCancel.ImageIndex = 1;
            this.m_cmdCancel.ImageList = this.m_oImageList16x16;
            this.m_cmdCancel.Location = new System.Drawing.Point(487,412);
            this.m_cmdCancel.Name = "m_cmdCancel";
            this.m_cmdCancel.Size = new System.Drawing.Size(75,32);
            this.m_cmdCancel.TabIndex = 1;
            this.m_cmdCancel.Text = "Cancel";
            this.m_cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_oGrid
            // 
            this.m_oGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_oGrid.DataMember = "";
            this.m_oGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.m_oGrid.Location = new System.Drawing.Point(8,16);
            this.m_oGrid.Name = "m_oGrid";
            this.m_oGrid.Size = new System.Drawing.Size(632,138);
            this.m_oGrid.TabIndex = 2;
            // 
            // m_oWebBrowser
            // 
            this.m_oWebBrowser.Location = new System.Drawing.Point(8,160);
            this.m_oWebBrowser.MinimumSize = new System.Drawing.Size(20,20);
            this.m_oWebBrowser.Name = "m_oWebBrowser";
            this.m_oWebBrowser.Size = new System.Drawing.Size(429,250);
            this.m_oWebBrowser.TabIndex = 3;
            this.m_oWebBrowser.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.m_oWebBrowser_Navigating);
            // 
            // m_oImageList16x16
            // 
            this.m_oImageList16x16.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_oImageList16x16.ImageStream")));
            this.m_oImageList16x16.TransparentColor = System.Drawing.Color.Silver;
            this.m_oImageList16x16.Images.SetKeyName(0,"Tick.bmp");
            this.m_oImageList16x16.Images.SetKeyName(1,"W95MBX01.bmp");
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(524,193);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75,23);
            this.button1.TabIndex = 4;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(486,258);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75,23);
            this.button2.TabIndex = 5;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // frmRecentChanges
            // 
            this.AcceptButton = this.m_cmdOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5,14);
            this.CancelButton = this.m_cmdCancel;
            this.ClientSize = new System.Drawing.Size(648,451);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.m_oWebBrowser);
            this.Controls.Add(this.m_oGrid);
            this.Controls.Add(this.m_cmdCancel);
            this.Controls.Add(this.m_cmdOK);
            this.Font = new System.Drawing.Font("Tahoma",8.25F,System.Drawing.FontStyle.Regular,System.Drawing.GraphicsUnit.Point,((byte)(0)));
            this.Name = "frmRecentChanges";
            this.Text = "Recent Changes";
            this.Load += new System.EventHandler(this.frmRecentChanges_Load);
            ((System.ComponentModel.ISupportInitialize)(this.m_oGrid)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

        private void frmRecentChanges_Load(object sender,EventArgs e)
        {

        }

        private void button1_Click(object sender,EventArgs e)
        {
            m_oWebBrowser.DocumentText = "<HTML><HEAD><STYLE><!-- P {font-family: Verdana; font-size: 9pt;  Margin-top: 3pt; Margin-Bottom: 3pt} .Superscript { font-size: 6pt; vertical-align: super } --> </STYLE> </HEAD><BODY><P>Steve Brian Walton<SPAN class=\"Superscript\">ABC</SPAN> was born on 29 December 1968 in Morley Hall, Morley, Yorkshire, England. On 27 September 1997 when he was 28 years old,</P><TABLE border=0><TR><TD>A</TD><TD>Marriage Certificate: Steven Brian Walton</TD></TD><TR><TD>B</TD><TD>Letter: Sandra Walton (1999)</TD></TD><TR><TD>C</TD><TD><A href=\"Source:1\">Interview: Steve Walton</A></TD></TD></TABLE></BODY></HTML>";
        }

        private void button2_Click(object sender,EventArgs e)
        {
            m_oWebBrowser.Url = new Uri("http://waltonhome.co.uk");
        }

        private void m_oWebBrowser_Navigating(object sender,WebBrowserNavigatingEventArgs e)
        {
            MessageBox.Show(e.Url.ToString());
            // e.Cancel = true;
        }

	}
}
