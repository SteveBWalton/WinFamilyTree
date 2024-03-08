using System;

namespace family_tree.objects
{
    /// <summary>Class to represent generic index, Name pairs from a database.  This is most often to used create a list of lookups.</summary>
    public class IdxName
    {
        #region Member Variables

        /// <summary>ID of the object.</summary>
        private int idx_;

        /// <summary>Human readable name of the object.</summary>
        private string name_;

        #endregion

        #region Constructors etc ...

        /// <summary>Class constructors.  Creates a new IdxName object with the specified properties.</summary>
        /// <param name="idx">Specifies the ID of the object.</param>
        /// <param name="name">Specifies the name of the object.</param>
        public IdxName(int idx, string name)
        {
            idx_ = idx;
            name_ = name;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets a human readable name of the clsIDName object.
        /// </summary>
        /// <returns>A human readable name for the clsIDName object.</returns>
        public override string ToString()
        {
            return name_;
        }

        /// <summary>The ID of the clsIDName object.</summary>
        public int idx { get { return idx_; } }

        /// <summary>The human readable label for the clsIDName object.</summary>
        public string name { get { return name_; } }

        #endregion
    }
}
