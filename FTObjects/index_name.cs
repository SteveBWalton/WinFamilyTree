using System;

namespace FamilyTree.Objects
{
    /// <summary>Class to represent generic index, Name pairs from a database.  This is most often to used create a list of lookups.</summary>
    public class IndexName
    {
        #region Member Variables

        /// <summary>ID of the object.</summary>
        private int index_;

        /// <summary>Human readable name of the object.</summary>
        private string name_;

        #endregion

        #region Constructors etc ...

        /// <summary>Class constructors.  Creates a new IndexName object with the specified properties.</summary>
        /// <param name="index">Specifies the ID of the object.</param>
        /// <param name="name">Specifies the name of the object.</param>
        public IndexName(int index, string name)
        {
            index_ = index;
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
        public int index { get { return index_; } }

        /// <summary>The human readable label for the clsIDName object.</summary>
        public string name { get { return name_; } }

        #endregion
    }
}
