using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using family_tree.objects;

namespace family_tree.viewer
{
    /// <summary>Class to represent the options used on a tree diagram.  This information could have simply been included in the TreeDocument object.</summary>
    public class TreeOptions
    {
        #region Member Variables

        /// <summary>Name of the main (larger) font to use on tree diagrams.</summary>
        public string mainFontName_;

        /// <summary>Name of the smaller (secondary) font to use on tree diagrams.</summary>
        public string subFontName_;

        /// <summary>Size of the main (larger) font to use on tree diagrams.</summary>
        public float mainFontSize_;

        /// <summary>Size of the smaller (secondary) font to use on tree diagrams.</summary>
        public float subFontSize_;

        /// <summary>True to draw a box around people on the tree diagrams.</summary>
        public bool isTreePersonBox_;

        /// <summary>Collection of rules to apply to the tree.</summary>
        private ArrayList rules_;

        #endregion

        #region Constructors etc...

        /// <summary>Class constructor with initial values taken from user options.</summary>
        public TreeOptions(UserOptions userOptions)
        {
            // Copy the values from the user options.
            mainFontName_ = userOptions.treeMainFontName;
            subFontName_ = userOptions.treeSubFontName;
            mainFontSize_ = userOptions.treeMainFontSize;
            subFontSize_ = userOptions.treeSubFontSize;
            isTreePersonBox_ = userOptions.isTreePersonBox;

            // Initialise the list of rules.
            rules_ = new ArrayList();
        }



        /// <summary>Class constructor with initial value taken from the specified .tree file.</summary>
        /// <param name="xmlTreeOptions">Specifies the .tree file to load the options from.</param>
        public TreeOptions(walton.XmlDocument xmlTreeOptions)
        {
            walton.XmlNode xmlOptions = xmlTreeOptions.getNode("options");

            walton.XmlNode xmlMainFont = xmlOptions.getNode("mainfont");
            mainFontName_ = xmlMainFont.getAttributeValue("name", "Tahoma", false);
            mainFontSize_ = xmlMainFont.getAttributeValue("size", 10.0f, false);

            walton.XmlNode xmlSubFont = xmlOptions.getNode("subfont");
            subFontName_ = xmlSubFont.getAttributeValue("name", "Tahoma", false);
            subFontSize_ = xmlSubFont.getAttributeValue("size", 8.0f, false);

            walton.XmlNode xmlPersonBox = xmlOptions.getNode("personbox");
            isTreePersonBox_ = xmlPersonBox.getAttributeValue("show", false, false);

            // Initialise the list of rules.
            rules_ = new ArrayList();
            walton.XmlNode rules = xmlOptions.getNode("rules");
            int numRules = rules.getNumNodes();
            for (int ruleIndex = 0; ruleIndex < numRules; ruleIndex++)
            {
                walton.XmlNode xmlRule = rules.getNode(ruleIndex);
                TreeRule rule = new TreeRule();
                rule.load(xmlRule);
                addRule(rule);
            }
        }



        #endregion

        #region Save



        /// <summary>Write the tree options into the specified .tree file.</summary>
        /// <param name="xmlTreeFile">Specifies the .tree file to write the options into.</param>
        public bool save(walton.XmlDocument xmlTreeFile)
        {
            // Write the basic options
            walton.XmlNode xmlOptions = xmlTreeFile.getNode("options");

            walton.XmlNode xmlMainFont = xmlOptions.getNode("mainfont");
            xmlMainFont.setAttributeValue("name", mainFontName_);
            xmlMainFont.setAttributeValue("size", (double)mainFontSize_);

            walton.XmlNode xmlSubFont = xmlOptions.getNode("subfont");
            xmlSubFont.setAttributeValue("name", subFontName_);
            xmlSubFont.setAttributeValue("size", (double)subFontSize_);

            walton.XmlNode xmlPersonBox = xmlOptions.getNode("personbox");
            xmlPersonBox.setAttributeValue("show", isTreePersonBox_);

            // Write the rules.
            walton.XmlNode xmlRules = xmlOptions.findNode("rules");
            if (xmlRules != null)
            {
                xmlRules.delete();
            }
            xmlRules = xmlOptions.getNode("rules");
            TreeRule[] rules = getRules();
            foreach (TreeRule rule in rules)
            {
                rule.save(xmlRules);
            }

            // Return success.
            return true;
        }



        #endregion

        #region Rules

        /// <summary>Returns an array of rules that apply to this tree.</summary>
        /// <returns>An array of rules that apply to this tree.</returns>
        public TreeRule[] getRules()
        {
            return (TreeRule[])rules_.ToArray(typeof(TreeRule));
        }



        /// <summary>Add the specified rule to the collection of rules.</summary>
        /// <param name="rule">Specifies the rule to add to the collection.</param>
        /// <returns>True for success, false otherwise.</returns>
        public bool addRule(TreeRule rule)
        {
            // Add the rule to the collection.
            rules_.Add(rule);

            // Return success.
            return true;
        }



        /// <summary>Removes the specified rule from the collection of rules.</summary>
        /// <param name="rule">Specifies the rule to remove from the collection.</param>
        /// <returns>True for success, false otherwise.</returns>
        public bool removeRule(TreeRule rule)
        {
            // Remove the rule from the collection.
            rules_.Remove(rule);

            // Return success.
            return true;
        }



        /// <summary>Return a html representation of the rules.</summary>
        /// <param name="database">Specifies the database to look up human readable information from.</param>
        /// <returns>A html representation of the rules.</returns>
        public string rulesToHtml(Database database)
        {
            StringBuilder html = new StringBuilder("<table>");
            bool isEven = true;
            foreach (TreeRule rule in getRules())
            {
                if (isEven)
                {
                    html.Append("<tr bgcolor=\"#BDD6DE\">");
                    isEven = false;
                }
                else
                {
                    html.Append("<tr bgcolor=\"#9DB6BE\">");
                    isEven = true;
                }
                html.Append("<td>");
                html.Append(TreeRule.actionToString(rule.action));
                html.Append("</td><td>");
                Person oPerson = new Person(rule.personIndex, database);
                html.Append(oPerson.getName(true, true));
                html.Append("</td><td>");
                html.Append(rule.parameter);
                html.Append("</td></tr>");
            }
            html.Append("</table>");

            // Return the string built.
            return html.ToString();
        }



        public bool isInRules(TreeRule.RuleAction action, int personIndex)
        {
            TreeRule[] rules = getRules();

            foreach (TreeRule rule in rules)
            {
                if (rule.action == action && rule.personIndex == personIndex)
                {
                    return true;
                }
            }

            return false;
        }



        #endregion
    }
}
