using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using FamilyTree.Objects;

namespace FamilyTree.Viewer
{
    // Class to represent the options used on a tree diagram.
    /// <summary>
    /// Class to represent the options used on a tree diagram.
    /// This information could have simply been included in the TreeDocument object.
    /// </summary>
    public class clsTreeOptions
    {
		#region Member Variables

		/// <summary>Name of the main (larger) font to use on tree diagrams.</summary>
		public string m_sTreeMainFontName;

		/// <summary>Name of the smaller (secondary) font to use on tree diagrams.</summary>
		public string	m_sTreeSubFontName;

		/// <summary>Size of the main (larger) font to use on tree diagrams.</summary>
		public float m_dTreeMainFontSize;

		/// <summary>Size of teh smaller (secondary) font to use on tree diagrams.</summary>
		public float m_dTreeSubFontSize;

		/// <summary>True to draw a box around people on the tree diagrams.</summary>
		public bool m_bTreePersonBox;

        /// <summary>Collection of rules to apply to the tree.</summary>
        private ArrayList m_oRules;

		#endregion

		#region Constructors etc...

        // Class constructor with initial values taken from user options.
        /// <summary>
        /// Class constructor with initial values taken from user options.
        /// </summary>
        public clsTreeOptions(clsUserOptions oUserOptions)
        {
            // Copy the values from the user options
            m_sTreeMainFontName = oUserOptions.m_sTreeMainFontName;
            m_sTreeSubFontName = oUserOptions.m_sTreeSubFontName;
            m_dTreeMainFontSize = oUserOptions.m_dTreeMainFontSize;
            m_dTreeSubFontSize = oUserOptions.m_dTreeSubFontSize;
            m_bTreePersonBox = oUserOptions.m_bTreePersonBox;

            // Initialise the list of rules
            m_oRules = new ArrayList();
        }

        // Class constructor with initial value taken from the specified .tree file.
        /// <summary>
        /// Class constructor with initial value taken from the specified .tree file.
        /// </summary>
        /// <param name="oTreeOptions">Specifies the .tree file to load the options from.</param>
        public clsTreeOptions(Innoval.clsXmlDocument oTreeOptions)
        {
            Innoval.clsXmlNode oOptions = oTreeOptions.GetNode("options");

            Innoval.clsXmlNode oMainFont = oOptions.GetNode("mainfont");
            m_sTreeMainFontName = oMainFont.GetAttributeValue("name","Tahoma",false);
            m_dTreeMainFontSize = oMainFont.GetAttributeValue("size",10.0f,false);

            Innoval.clsXmlNode oSubFont = oOptions.GetNode("subfont");
            m_sTreeSubFontName = oSubFont.GetAttributeValue("name","Tahoma",false);
            m_dTreeSubFontSize = oSubFont.GetAttributeValue("size",8.0f,false);

            Innoval.clsXmlNode oPersonBox = oOptions.GetNode("personbox");
            m_bTreePersonBox = oPersonBox.GetAttributeValue("show",false,false);

            // Initialise the list of rules
            m_oRules = new ArrayList();
            Innoval.clsXmlNode oRules = oOptions.GetNode("rules");
            int nRules = oRules.GetNumNodes();
            for(int nRule = 0;nRule < nRules;nRule++)
            {
                Innoval.clsXmlNode xmlRule = oRules.GetNode(nRule);
                clsTreeRule oRule = new clsTreeRule();
                oRule.Load(xmlRule);
                AddRule(oRule);
            }
        }

        #endregion

        #region Save

        /// <summary>Write the tree options into the specified .tree file
        /// </summary>
        /// <param name="oTreeFile">Specifies the .tree file to write the options into.</param>
        public bool Save(Innoval.clsXmlDocument oTreeFile)
        {
            // Write the basic options
            Innoval.clsXmlNode oOptions = oTreeFile.GetNode("options");

            Innoval.clsXmlNode oMainFont = oOptions.GetNode("mainfont");
            oMainFont.SetAttributeValue("name",m_sTreeMainFontName);
            oMainFont.SetAttributeValue("size",(double)m_dTreeMainFontSize);

            Innoval.clsXmlNode oSubFont = oOptions.GetNode("subfont");
            oSubFont.SetAttributeValue("name",m_sTreeSubFontName);
            oSubFont.SetAttributeValue("size",(double)m_dTreeSubFontSize);

            Innoval.clsXmlNode oPersonBox = oOptions.GetNode("personbox");
            oPersonBox.SetAttributeValue("show",m_bTreePersonBox);

            // Write the rules
            Innoval.clsXmlNode xmlRules = oOptions.FindNode("rules");
            if(xmlRules != null)
            {
                xmlRules.Delete();
            }
            xmlRules = oOptions.GetNode("rules");
            clsTreeRule[] oRules = GetRules();
            foreach(clsTreeRule oRule in oRules)
            {
                oRule.Save(xmlRules);
            }

            // Return success
            return true;
        }

        #endregion

        #region Rules

        // Returns an array of rules that apply to this tree.
        /// <summary>
        /// Returns an array of rules that apply to this tree.
        /// </summary>
        /// <returns>An array of rules that apply to this tree.</returns>
        public clsTreeRule [] GetRules()
        {
            return (clsTreeRule [])m_oRules.ToArray(typeof(clsTreeRule));
        }

        // Add the specified rule to the collection of rules.
        /// <summary>
        /// Add the specified rule to the collection of rules.
        /// </summary>
        /// <param name="oRule">Specifies the rule to add to the collection.</param>
        /// <returns>True for success, false otherwise.</returns>
        public bool AddRule(clsTreeRule oRule)
        {
            // Add the rule to the collection
            m_oRules.Add(oRule);

            // Return success
            return true;
        }

        // Removes the specified rule from the collection of rules.
        /// <summary>
        /// Removes the specified rule from the collection of rules.
        /// </summary>
        /// <param name="oRule">Specifies the rule to remove from the collection.</param>
        /// <returns>True for success, false otherwise.</returns>
        public bool RemoveRule(clsTreeRule oRule)
        {
            // Remove the rule from the collection
            m_oRules.Remove(oRule);

            // Return success
            return true;
        }

        // Return a Html representation of the rules.
        /// <summary>
        /// Return a Html representation of the rules.
        /// </summary>
        /// <param name="oDb">Specifies the database to look up human readable information from.</param>
        /// <returns>A html representation of the rules.</returns>
        public string RulesToHtml(Database oDb)
        {
            StringBuilder sbHtml = new StringBuilder("<table>");
            bool bEven = true;
            foreach(clsTreeRule oRule in GetRules())
            {
                if(bEven)
                {
                    sbHtml.Append("<tr bgcolor=\"#BDD6DE\">");
                    bEven = false;
                }
                else
                {
                    sbHtml.Append("<tr bgcolor=\"#9DB6BE\">");
                    bEven = true;
                }
                sbHtml.Append("<td>");
                sbHtml.Append(clsTreeRule.ActionToString(oRule.Action));
                sbHtml.Append("</td><td>");
                clsPerson oPerson = new clsPerson(oRule.PersonID, oDb);
                sbHtml.Append(oPerson.GetName(true, true));
                sbHtml.Append("</td><td>");
                sbHtml.Append(oRule.Parameter);
                sbHtml.Append("</td></tr>");
            }
            sbHtml.Append("</table>");

            // Return the string built
            return sbHtml.ToString();
        }

        public bool IsInRules(clsTreeRule.ERuleAction nAction, int nPersonID)
        {
            clsTreeRule [] oRules = GetRules();

            foreach(clsTreeRule oRule in oRules)
            {
                if(oRule.Action == nAction && oRule.PersonID == nPersonID)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
