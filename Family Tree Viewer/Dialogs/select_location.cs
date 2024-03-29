using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using family_tree.objects;

namespace family_tree.viewer
{
    public partial class SelectLocationDialog : Form
    {
        /// <summary>The database that the control is attached to.</summary>
        private Database database_;

        /// <summary>The location selected by the control.</summary>
        private string locationName_;



        /// <summary>Class constructor.</summary>
        /// <param name="database">Specifies the database to fetch locations from.</param>
        /// <param name="initialValue">Specifies the initial value of the control.</param>
        public SelectLocationDialog(Database database, string initialValue)
        {
            InitializeComponent();

            database_ = database;

            txtLocation_.Text = initialValue;
            locationName_ = locationToPath(initialValue);
        }



        /// <summary>The message handler for the form shown event.</summary>
        private void frmSelectLocationShown(object sender, EventArgs e)
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
            Place[] children = database_.getPlaces(place.idx);
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
            StringBuilder remainingPath = new StringBuilder(path);
            StringBuilder location = new StringBuilder();

            int last = remainingPath.ToString().LastIndexOf("\\");
            while (last > 0)
            {
                location.Append(remainingPath.ToString().Substring(last + 1));
                remainingPath.Remove(last, remainingPath.Length - last);
                location.Append(", ");

                last = remainingPath.ToString().LastIndexOf("\\");
            }
            location.Append(remainingPath);

            return location.ToString();
        }



        /// <summary>Converts a path "England/Yorkshire/Morley" into a location "Morley, Yorkshire, England".</summary>
        /// <param name="location">Specifies the location to convert.</param>
        /// <returns>A path that represents the specified location.</returns>
        private string locationToPath(string location)
        {
            StringBuilder path = new StringBuilder();
            StringBuilder remainingLocation = new StringBuilder(location);

            int last = remainingLocation.ToString().LastIndexOf(",");
            while (last > 0)
            {
                path.Append(remainingLocation.ToString().Substring(last + 2));
                remainingLocation.Remove(last, remainingLocation.Length - last);
                path.Append(treeView_.PathSeparator);

                last = remainingLocation.ToString().LastIndexOf(",");
            }
            path.Append(remainingLocation);

            return path.ToString();
        }



        /// <summary>Message handler for the "OK" button click event.</summary>
        private void cmdOkClick(object sender, EventArgs e)
        {
            locationName_ = pathToLocation(treeView_.SelectedNode.FullPath);
        }



        /// <summary>Message handler for the "After Select" event on the tree control.</summary>
        private void treeViewAfterSelect(object sender, TreeViewEventArgs e)
        {
            txtLocation_.Text = pathToLocation(treeView_.SelectedNode.FullPath);
        }
    }
}