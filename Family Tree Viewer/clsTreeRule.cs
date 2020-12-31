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
        /// <summary>The action that the rule should apply to the tree.</summary>
        public enum ERuleAction
        {
            /// <summary>Force the descendants of PersonID to be included in the tree.</summary>
            IncludeDescendants,

            /// <summary>Force the descedants of PersonID to be excluded from the tree.</summary>
            ExcludeDescendants,

            /// <summary>Force the ancestors of PersonID to be included in the tree.</summary>
            IncludeAncestors,

            /// <summary>Force the ancestors of PersonID to be excluded from the tree.</summary>
            ExcludeAncestors,

            /// <summary>Move a person in the horizontal direction.</summary>
            HorizontalOffset
        }

        /// <summary>The action that this rule applied to the tree</summary>
        public ERuleAction Action;

        /// <summary>If the action applies to an person this is the ID of the person.</summary>
        public int PersonID;

        // Additional parameter for the rule.
        /// <summary>
        /// Additional parameter for the rule.
        /// </summary>
        public string Parameter;

        /// <summary>Class constructor.
        /// </summary>
        public clsTreeRule()
        {
        }

        /// <summary>Convert a ERuleAction value into a human readable string.
        /// </summary>
        /// <param name="nAction">Specifies the value to return as a string.</param>
        /// <returns>A string that represents the specified value.</returns>
        public static string ActionToString(ERuleAction nAction)
        {
            switch(nAction)
            {
            case ERuleAction.IncludeDescendants:
                return "Include Descendants";
            case ERuleAction.ExcludeDescendants:
                return "Exclude Descendants";
            case ERuleAction.IncludeAncestors:
                return "Include Ancestors";
            case ERuleAction.ExcludeAncestors:
                return "Exclude Ancestors";
            case ERuleAction.HorizontalOffset:
                return "Horizontal Offset";
            default:
                return "Error - Unknown";
            }
        }

        #region File IO

        /// <summary>Save the rule in the specified .tree file.</summary>
        /// <param name="oRules">Specifies the node in the .tree file to add the rule into.</param>
        /// <returns>True for success, false otherwise.</returns>
        public bool Save(walton.XmlNode oRules)
        {
            walton.XmlNode oRule = oRules.addNode("rule");
            oRule.setAttributeValue("action",(int)Action);
            oRule.setAttributeValue("person",PersonID);
            oRule.setAttributeValue("parameter",Parameter);

            // Return success.
            return true;
        }



        /// <summary>Reads the rule from the specified node in a .tree file.</summary>
        /// <param name="oRule">Specifies the node in the .tree file to read the rule from.</param>
        /// <returns>True for success, false otherwise.</returns>
        public bool Load(walton.XmlNode oRule)
        {
            // Load the properties of this rule.
            Action = (ERuleAction)oRule.getAttributeValue("action",(int)ERuleAction.ExcludeDescendants,false);
            PersonID = oRule.getAttributeValue("person",0,false);
            Parameter = oRule.getAttributeValue("parameter","",false);

            // Return success.
            return true;
        }



        #endregion

        #region Public Properties

        // Returns the value of the additional parameter (string) as a float without raising an error.
        /// <summary>
        /// Returns the value of the additional parameter (string) as a float without raising an error.
        /// </summary>
        public float ParameterAsFloat
        {
            get
            {
                float dResult = 0;
                try
                {
                    dResult = float.Parse(Parameter);
                }
                catch { }
                return dResult;
            }
        }

        #endregion
    }
}
