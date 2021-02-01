using System.Text;

namespace family_tree.objects
{
    /// <summary>
    /// Class to represent the references to a source from a person.
    /// Each object represents a single person and collection of properties that the source has provided information about.
    /// 
    /// This is used to build a list of people (and their properties) that a particular source give information about.
    /// This is shown at the bottom of the edit source dialog.
    /// </summary>
    public class References
    {
        #region Member Variables

        /// <summary>ID of the person.</summary>
        private int personIndex_;

        /// <summary>Human readable name for the person.</summary>
        private string personName_;

        /// <summary>A human readable list of references.</summary>
        private StringBuilder references_;

        #endregion

        #region Constructors etc ...



        /// <summary>Class constructor.  Creates a new clsReferences object attached to the specified person.</summary>
        /// <param name="personIndex">Specifies the ID of the person that this references collection is attached to.</param>
        public References(int personIndex)
        {
            personIndex_ = personIndex;
            references_ = new StringBuilder();
        }



        /// <summary>Add a reference to the collection.</summary>
        /// <param name="reference">Specifies the human readable description of the reference.</param>
        public void addReference(string reference)
        {
            if (references_.Length == 0)
            {
                references_.Append(reference);
            }
            else
            {
                references_.Append(", ");
                references_.Append(reference);
            }
        }



        #endregion

        #region Public Properties

        /// <summary>ID of the person.</summary>
        public int personIndex { get { return personIndex_; } }

        /// <summary>Human readable name for the person.</summary>
        public string personName { get { return personName_; } set { personName_ = value; } }

        /// <summary>Human readable list of references.</summary>
        public string references { get { return references_.ToString(); } }

        #endregion
    }
}
