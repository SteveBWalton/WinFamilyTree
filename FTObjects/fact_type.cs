using System;

namespace family_tree.objects
{
    /// <summary>Class to represent a single fact type.  This is closely related to the <code>tlk_FactTypes</code> table.</summary>
    public class FactType
    {
        #region Member Variables

        /// <summary>ID of the fact type in the database.</summary>
        private int idx_;

        /// <summary>Human readable name of the fact type.</summary>
        private string name_;

        #endregion

        #region Constructors etc ...



        /// <summary>Class constructor.  Creates a new fact type with the specified properties.</summary>
        /// <param name="index">Specifies the ID of the fact type.</param>
        /// <param name="name">Specifies the human readable name of the fact type.</param>
        public FactType(int index, string name)
        {
            idx_ = index;
            name_ = name;
        }



        #endregion

        #region Public Methods



        /// <summary>Gets a human readable name of the fact type.</summary>
        /// <returns>A human readable name for the fact type.</returns>
        public override string ToString()
        {
            return name_;
        }



        /// <summary>ID of the fact type.</summary>
        public int index { get { return idx_; } }



        /// <summary>Human readable name of the fact type.</summary>
        public string name { get { return name_; } }



        #endregion
    }
}
