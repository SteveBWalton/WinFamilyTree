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
    /// <summary>Dialog to allow the user to edit the options for a specific tree.</summary>
    public partial class TreeOptionsDialog : Form
    {
        #region Member Variables

        /// <summary>The tree document that these options apply to.</summary>
        private TreeDocument tree_;

        #endregion

        #region Constructors etc ...



        /// <summary>Class constructor.  Copies the values for the specified user options to initialise the tree options.</summary>
        /// <param name="tree">Specifies the user options.</param>
        public TreeOptionsDialog(TreeDocument tree)
        {
            InitializeComponent();

            // Save the options to change if the user clicks OK.
            tree_ = tree;

            // Update the form with the current options.
            labTreeMainFont_.Font = new System.Drawing.Font(tree_.options.mainFontName_, tree_.options.mainFontSize_);
            labTreeMainFont_.Text = tree_.options.mainFontName_ + " " + tree_.options.mainFontSize_.ToString();
            labTreeSubFont_.Font = new System.Drawing.Font(tree_.options.subFontName_, tree_.options.subFontSize_);
            labTreeSubFont_.Text = tree_.options.subFontName_ + " " + tree_.options.subFontSize_.ToString();
            chkTreePersonBox_.Checked = tree_.options.isTreePersonBox_;

            updateRulesDisplay();
        }



        #endregion

        #region Supporting Functions



        /// <summary>Update the display of the existing rules.  This is probably not working correctly since I removed the styles.</summary>
        private void updateRulesDisplay()
        {
            // Build a html description of the tree rules.
            StringBuilder html = new StringBuilder();
            html.Append("<html><head>");
            // sbHtml.Append(m_oTree.Database.HtmlStyles());
            html.Append("</head><body>");
            html.Append(tree_.options.rulesToHtml(tree_.database));
            html.Append("</body></html>");

            // Display the html description.
            webRules_.DocumentText = html.ToString();
        }



        #endregion

        #region Message Handlers

        #region Form Events



        /// <summary>Message handler for the form loading event.</summary>
        private void frmTreeOptions_Load(object sender, EventArgs e)
        {
            // Populate the type of rules combo box.
            for (TreeRule.RuleAction action = TreeRule.RuleAction.INCLUDE_DESCENDANTS; action <= TreeRule.RuleAction.HORIZONTAL_OFFSET; action++)
            {
                cboRules_.Items.Add(TreeRule.actionToString(action));
            }

            // Populate the people combo box.
            TreePerson[] people = tree_.getPeople();
            foreach (TreePerson person in people)
            {
                cboRulePeople_.Items.Add(person);
            }
        }



        #endregion



        private void cmdTreeMainFont_Click(object sender, System.EventArgs e)
        {
            fontDialog_.Font = labTreeMainFont_.Font;
            fontDialog_.ShowDialog(this);
            labTreeMainFont_.Font = fontDialog_.Font;
            labTreeMainFont_.Text = fontDialog_.Font.Name + " " + fontDialog_.Font.Size.ToString();
        }



        private void cmdTreeSubFont_Click(object sender, System.EventArgs e)
        {
            fontDialog_.Font = this.labTreeSubFont_.Font;
            fontDialog_.ShowDialog(this);
            labTreeSubFont_.Font = fontDialog_.Font;
            labTreeSubFont_.Text = fontDialog_.Font.Name + " " + fontDialog_.Font.Size.ToString();
        }



        private void cmdOK_Click(object sender, EventArgs e)
        {
            tree_.options.mainFontName_ = labTreeMainFont_.Font.Name;
            tree_.options.subFontName_ = labTreeSubFont_.Font.Name;
            tree_.options.mainFontSize_ = labTreeMainFont_.Font.Size;
            tree_.options.subFontSize_ = labTreeSubFont_.Font.Size;
            tree_.options.isTreePersonBox_ = chkTreePersonBox_.Checked;
        }



        /// <summary>Message handler for the action rule button click.</summary>
        private void cmdAdd_Click(object sender, EventArgs e)
        {
            // Check that there is a selection.
            if (cboRules_.SelectedIndex == -1)
            {
                return;
            }

            TreeRule.RuleAction action = (TreeRule.RuleAction)cboRules_.SelectedIndex;
            TreePerson person = (TreePerson)cboRulePeople_.SelectedItem;

            TreeRule newRule = new TreeRule();
            newRule.action = action;
            newRule.personIndex = person.personIndex;
            newRule.parameter = txtRuleParameter_.Text;
            tree_.options.addRule(newRule);

            updateRulesDisplay();
        }



        #endregion

    }
}