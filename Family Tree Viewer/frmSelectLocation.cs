using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using FamilyTree.Objects;

namespace FamilyTree.Viewer
{
    public partial class frmSelectLocation : Form
    {
        // The database that the control is attached to.
        /// <summary>
        /// The database that the control is attached to.
        /// </summary>
        private Database m_oDb;

        // The location selected by the control.
        /// <summary>
        /// The location selected by the control.
        /// </summary>
        private string m_sLocation;

        // Class constructor.
        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="oDb">Specifies the database to fetch locations from.</param>
        /// <param name="sInitialValue">Specifies the initial value of the control.</param>
        public frmSelectLocation(Database oDb,string sInitialValue)
        {
            InitializeComponent();

            m_oDb = oDb;

            m_txtLocation.Text = sInitialValue;
            m_sLocation = LocationToPath(sInitialValue);
        }

        // The message handler for the form shown event.
        /// <summary>
        /// The message handler for the form shown event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmSelectLocation_Shown(object sender, EventArgs e)
        {
            clsPlace[] oPlaces = m_oDb.getPlaces(0);
            foreach(clsPlace oPlace in oPlaces)
            {                
                TreeNode oChildNode = m_TreeView.Nodes.Add(oPlace.Name);
                if(oChildNode.FullPath == m_sLocation)
                {
                    m_TreeView.SelectedNode = oChildNode;
                }
                AddTreeNode(oChildNode, oPlace);
            }
        }

        // Adds the specified place and its children to the specified node.
        /// <summary>
        /// Adds the specified place and its children to the specified tree node.
        /// </summary>
        /// <param name="oParent">Specifies the tree node to add to.</param>
        /// <param name="oPlace">Specifies the place to add to the node.</param>
        private void AddTreeNode(TreeNode oParent, clsPlace oPlace)
        {
            clsPlace[] oChildren = m_oDb.getPlaces(oPlace.ID );
            foreach(clsPlace oChild in oChildren )
            {
                TreeNode oChildNode = oParent.Nodes.Add(oChild.Name, oChild.Name, oChild.Status, oChild.Status);
                if(oChildNode.FullPath == m_sLocation)
                {
                    m_TreeView.SelectedNode = oChildNode;
                }
                AddTreeNode(oChildNode, oChild);
            }
        }

        // The location selected by the control.
        /// <summary>
        /// The location selected by the control.
        /// Do not use "Location" this hides a property of the form.
        /// </summary>
        public string LocationName
        {
            get
            {
                return m_sLocation;
            }
        }

        // Converts a location "Morley, Yorkshire, England" into a path "England/Yorkshire/Morley"
        /// <summary>
        /// Converts a location "Morley, Yorkshire, England" into a path "England/Yorkshire/Morley"
        /// </summary>
        /// <param name="sPath">Specifies the path to convert.</param>
        /// <returns>A location that represents the specified path.</returns>
        private string PathToLocation(string sPath)
        {
            StringBuilder sbPath = new StringBuilder(sPath);
            StringBuilder sbLocation = new StringBuilder();

            int nLast = sbPath.ToString().LastIndexOf("\\");
            while(nLast > 0)
            {
                sbLocation.Append(sbPath.ToString().Substring(nLast + 1));
                sbPath.Remove(nLast, sbPath.Length - nLast);
                sbLocation.Append(", ");
                
                nLast = sbPath.ToString().LastIndexOf("\\");
            }
            sbLocation.Append(sbPath);

            return sbLocation.ToString();
        }

        // Converts a path "England/Yorkshire/Morley" into a location "Morley, Yorkshire, England"
        /// <summary>
        /// Converts a path "England/Yorkshire/Morley" into a location "Morley, Yorkshire, England"
        /// </summary>
        /// <param name="sLocation">Specifies the location to convert.</param>
        /// <returns>A path that represents the specified location.</returns>
        private string LocationToPath(string sLocation)
        {
            StringBuilder sbPath = new StringBuilder();
            StringBuilder sbLocation = new StringBuilder(sLocation);

            int nLast = sbLocation.ToString().LastIndexOf(",");
            while(nLast > 0)
            {
                sbPath.Append(sbLocation.ToString().Substring(nLast + 2));
                sbLocation.Remove(nLast, sbLocation.Length - nLast);
                sbPath.Append(m_TreeView.PathSeparator);
                
                nLast = sbLocation.ToString().LastIndexOf(",");
            }
            sbPath.Append(sbLocation);

            return sbPath.ToString();
        }

        // Message handler for the "OK" button click event.
        /// <summary>
        /// Message handler for the "OK" button click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdOK_Click(object sender, EventArgs e)
        {
            m_sLocation = PathToLocation(m_TreeView.SelectedNode.FullPath);
        }

        // Message handler for the "After Select" event on the tree control.
        /// <summary>
        /// Message handler for the "After Select" event on the tree control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            m_txtLocation.Text = PathToLocation(m_TreeView.SelectedNode.FullPath);
        }
    }
}