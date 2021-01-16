using System;
using System.Collections.Generic;
using System.Text;

namespace FamilyTree.Viewer
{
    /// <summary>
    /// Object a represent a modification (rule) to apply to a tree document,
    /// These objects will be contained inside a clsTreeOptions object.
    /// </summary>
    public class clsTreeRule
    {
        #region Member Variables

        /// <summary>The action that the rule should apply to the tree.</summary>
        public enum RuleAction
        {
            /// <summary>Force the descendants of PersonID to be included in the tree.</summary>
            INCLUDE_DESCENDANTS,

            /// <summary>Force the descedants of PersonID to be excluded from the tree.</summary>
            EXCLUDE_DESCENDANTS,

            /// <summary>Force the ancestors of PersonID to be included in the tree.</summary>
            INCLUDE_ANCESTORS,

            /// <summary>Force the ancestors of PersonID to be excluded from the tree.</summary>
            EXCLUDE_ANCESTORS,

            /// <summary>Move a person in the horizontal direction.</summary>
            HORIZONTAL_OFFSET
        }

        /// <summary>The action that this rule applied to the tree</summary>
        public RuleAction action;

        /// <summary>If the action applies to an person this is the ID of the person.</summary>
        public int personIndex;

        /// <summary>Additional parameter for the rule.</summary>
        public string parameter;

        #endregion

        #region Constructors

        /// <summary>Class constructor.</summary>
        public clsTreeRule()
        {
        }

        #endregion

        #region Supporting Functions



        /// <summary>Convert a RuleAction value into a human readable string.</summary>
        /// <param name="action">Specifies the value to return as a string.</param>
        /// <returns>A string that represents the specified value.</returns>
        public static string actionToString(RuleAction action)
        {
            switch (action)
            {
            case RuleAction.INCLUDE_DESCENDANTS:
                return "Include Descendants";
            case RuleAction.EXCLUDE_DESCENDANTS:
                return "Exclude Descendants";
            case RuleAction.INCLUDE_ANCESTORS:
                return "Include Ancestors";
            case RuleAction.EXCLUDE_ANCESTORS:
                return "Exclude Ancestors";
            case RuleAction.HORIZONTAL_OFFSET:
                return "Horizontal Offset";
            default:
                return "Error - Unknown";
            }
        }



        #endregion

        #region File IO



        /// <summary>Save the rule in the specified .tree file.</summary>
        /// <param name="xmlRules">Specifies the node in the .tree file to add the rule into.</param>
        /// <returns>True for success, false otherwise.</returns>
        public bool save(walton.XmlNode xmlRules)
        {
            walton.XmlNode xmlRule = xmlRules.addNode("rule");
            xmlRule.setAttributeValue("action", (int)action);
            xmlRule.setAttributeValue("person", personIndex);
            xmlRule.setAttributeValue("parameter", parameter);

            // Return success.
            return true;
        }



        /// <summary>Reads the rule from the specified node in a .tree file.</summary>
        /// <param name="xmlRule">Specifies the node in the .tree file to read the rule from.</param>
        /// <returns>True for success, false otherwise.</returns>
        public bool load(walton.XmlNode xmlRule)
        {
            // Load the properties of this rule.
            action = (RuleAction)xmlRule.getAttributeValue("action", (int)RuleAction.EXCLUDE_DESCENDANTS, false);
            personIndex = xmlRule.getAttributeValue("person", 0, false);
            parameter = xmlRule.getAttributeValue("parameter", "", false);

            // Return success.
            return true;
        }



        #endregion

        #region Public Properties



        /// <summary>Returns the value of the additional parameter (string) as a float without raising an error.</summary>
        public float parameterAsFloat
        {
            get
            {
                float result = 0;
                try
                {
                    result = float.Parse(parameter);
                }
                catch { }
                return result;
            }
        }



        #endregion
    }
}
