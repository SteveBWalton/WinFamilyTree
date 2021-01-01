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
    // Form to allow the user to edit the options for a specific tree.
    /// <summary>
    /// Form to allow the user to edit the options for a specific tree.
    /// </summary>
    public partial class frmTreeOptions : Form
    {
        #region Member Variables

        // The tree document that these options apply to.
        /// <summary>
        /// The tree document that these options apply to.
        /// </summary>
        private clsTreeDocument m_oTree;

        #endregion

        #region Constructors etc ...

        // Class constructor.
        /// <summary>
        /// Class constructor.
        /// Copies the values for the specified user options to initialise the tree options.
        /// </summary>
        /// <param name="oTree">Specifies the user options.</param>
        public frmTreeOptions(clsTreeDocument oTree)
        {
            InitializeComponent();

            // Save the options to change if the user clicks OK
            m_oTree = oTree;

            // Update the form with the current options
            this.labTreeMainFont.Font = new System.Drawing.Font(m_oTree.options.mainFontName_, m_oTree.options.mainFontSize_);
            this.labTreeMainFont.Text = m_oTree.options.mainFontName_ + " " + m_oTree.options.mainFontSize_.ToString();
            this.labTreeSubFont.Font = new System.Drawing.Font(m_oTree.options.subFontName_, m_oTree.options.subFontSize_);
            this.labTreeSubFont.Text = m_oTree.options.subFontName_ + " " + m_oTree.options.subFontSize_.ToString();
            this.chkTreePersonBox.Checked = m_oTree.options.isTreePersonBox_;

            UpdateRulesDisplay();
        }

        #endregion

        #region Supporting Functions

        // Update the display of the existing rules.
        /// <summary>
        /// Update the display of the existing rules.
        /// This is probably not working correctly since I removed the styles.
        /// </summary>
        private void UpdateRulesDisplay()
        {
            // Build a html description of the tree rules
            StringBuilder sbHtml = new StringBuilder();
            sbHtml.Append("<html><head>");                        
            // sbHtml.Append(m_oTree.Database.HtmlStyles());
            sbHtml.Append("</head><body>");
            sbHtml.Append(m_oTree.options.rulesToHtml(m_oTree.database));
            sbHtml.Append("</body></html>");
            
            // Display the html description
            m_oRules.DocumentText = sbHtml.ToString();
        }

        #endregion

        #region Message Handlers

        #region Form Events

        /// <summary>
        /// Message handler for the form loading event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmTreeOptions_Load(object sender,EventArgs e)
        {
            // Populate the type of rules combo box
            for(clsTreeRule.RuleAction Action = clsTreeRule.RuleAction.INCLUDE_DESCENDANTS;Action <= clsTreeRule.RuleAction.HORIZONTAL_OFFSET;Action++)
            {
                m_cboRules.Items.Add(clsTreeRule.actionToString(Action));
            }

            // Populate the people combo box
            clsTreePerson[] oPeople = m_oTree.getPeople();
            foreach(clsTreePerson oPerson in oPeople)
            {
                m_cboRulePeople.Items.Add(oPerson);
            }
        }

        #endregion

        private void cmdTreeMainFont_Click(object sender,System.EventArgs e)
        {
            this.fontDialog1.Font = this.labTreeMainFont.Font;
            this.fontDialog1.ShowDialog(this);
            this.labTreeMainFont.Font = fontDialog1.Font;
            this.labTreeMainFont.Text = this.fontDialog1.Font.Name + " " + this.fontDialog1.Font.Size.ToString();
        }

        private void cmdTreeSubFont_Click(object sender,System.EventArgs e)
        {
            this.fontDialog1.Font = this.labTreeSubFont.Font;
            this.fontDialog1.ShowDialog(this);
            this.labTreeSubFont.Font = fontDialog1.Font;
            this.labTreeSubFont.Text = this.fontDialog1.Font.Name + " " + this.fontDialog1.Font.Size.ToString();
        }

        private void m_cmdOK_Click(object sender,EventArgs e)
        {
            m_oTree.options.mainFontName_ = this.labTreeMainFont.Font.Name;
            m_oTree.options.subFontName_ = this.labTreeSubFont.Font.Name;
            m_oTree.options.mainFontSize_ = this.labTreeMainFont.Font.Size;
            m_oTree.options.subFontSize_ = this.labTreeSubFont.Font.Size;
            m_oTree.options.isTreePersonBox_ = this.chkTreePersonBox.Checked;
        }

        /// <summary>
        /// Message handler for the action rule button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdAdd_Click(object sender,EventArgs e)
        {
            // Check that there is a selection
            if(m_cboRules.SelectedIndex == -1)
            {
                return;
            }

            clsTreeRule.RuleAction nAction = (clsTreeRule.RuleAction)m_cboRules.SelectedIndex;
            clsTreePerson oPerson = (clsTreePerson)m_cboRulePeople.SelectedItem;

            clsTreeRule oNewRule = new clsTreeRule();
            oNewRule.action = nAction;
            oNewRule.personIndex = oPerson.PersonID;
            oNewRule.parameter = m_txtRuleParameter.Text;
            m_oTree.options.addRule(oNewRule);

            UpdateRulesDisplay();
        }

        #endregion

    }
}