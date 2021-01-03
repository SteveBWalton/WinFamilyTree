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
        /// <summary>The database that the control is attached to.</summary>
        private Database database_;

        /// <summary>The location selected by the control.</summary>
        private string locationName_;



        /// <summary>Class constructor.</summary>
        /// <param name="database">Specifies the database to fetch locations from.</param>
        /// <param name="initialValue">Specifies the initial value of the control.</param>
        public frmSelectLocation(Database database, string initialValue)
        {
            InitializeComponent();

            database_ = database;

            txtLocation_.Text = initialValue;
            locationName_ = locationToPath(initialValue);
        }



        /// <summary>The message handler for the form shown event.</summary>
        private void frmSelectLocation_Shown(object sender, EventArgs e)
        {
            Place[] places = database_.getPlaces(0);
            foreach (Place place in places)
            {
                TreeNode childNode = treeView_.Nodes.Add(place.name);
                if (childNode.FullPath == locationName_)
                {
                    treeView_.SelectedNode = childNode;
                }
                addTreeNode(childNode, place);
            }
        }



        /// <summary>Adds the specified place and its children to the specified tree node.</summary>
        /// <param name="parent">Specifies the tree node to add to.</param>
        /// <param name="place">Specifies the place to add to the node.</param>
        private void addTreeNode(TreeNode parent, Place place)
        {
            Place[] children = database_.getPlaces(place.index);
            foreach (Place child in children)
            {
                TreeNode childNode = parent.Nodes.Add(child.name, child.name, child.status, child.status);
                if (childNode.FullPath == locationName_)
                {
                    treeView_.SelectedNode = childNode;
                }
                addTreeNode(childNode, child);
            }
        }



        /// <summary>The location selected by the control.  Do not use "Location" this hides a property of the form.</summary>
        public string locationName
        {
            get { return locationName_; }
        }



        /// <summary>Converts a location "Morley, Yorkshire, England" into a path "England/Yorkshire/Morley".</summary>
        /// <param name="path">Specifies the path to convert.</param>
        /// <returns>A location that represents the specified path.</returns>
        private string pathToLocation(string path)
        {
            StringBuilder sbPath = new StringBuilder(path);
            StringBuilder sbLocation = new StringBuilder();

            int last = sbPath.ToString().LastIndexOf("\\");
            while (last > 0)
            {
                sbLocation.Append(sbPath.ToString().Substring(last + 1));
                sbPath.Remove(last, sbPath.Length - last);
                sbLocation.Append(", ");

                last = sbPath.ToString().LastIndexOf("\\");
            }
            sbLocation.Append(sbPath);

            return sbLocation.ToString();
        }



        /// <summary>Converts a path "England/Yorkshire/Morley" into a location "Morley, Yorkshire, England".</summary>
        /// <param name="location">Specifies the location to convert.</param>
        /// <returns>A path that represents the specified location.</returns>
        private string locationToPath(string location)
        {
            StringBuilder sbPath = new StringBuilder();
            StringBuilder sbLocation = new StringBuilder(location);

            int last = sbLocation.ToString().LastIndexOf(",");
            while (last > 0)
            {
                sbPath.Append(sbLocation.ToString().Substring(last + 2));
                sbLocation.Remove(last, sbLocation.Length - last);
                sbPath.Append(treeView_.PathSeparator);

                last = sbLocation.ToString().LastIndexOf(",");
            }
            sbPath.Append(sbLocation);

            return sbPath.ToString();
        }



        /// <summary>Message handler for the "OK" button click event.</summary>
        private void cmdOK_Click(object sender, EventArgs e)
        {
            locationName_ = pathToLocation(treeView_.SelectedNode.FullPath);
        }



        /// <summary>Message handler for the "After Select" event on the tree control.</summary>
        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            txtLocation_.Text = pathToLocation(treeView_.SelectedNode.FullPath);
        }
    }
}