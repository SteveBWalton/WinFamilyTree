using System;
using System.Text;

namespace FamilyTree.Objects
{
	/// <summary>
	/// Class to represent the references to a source from a person.
	/// Each object represents a single person and collection of properties that the source has provided information about.
	/// 
	/// This is used to build a list of people (and their properties) that a particular source give information about.
	/// This is shown at the bottom of the edit source dialog.
	/// </summary>
	public class clsReferences
	{
		#region Member Variables

		/// <summary>ID of the person.</summary>
		private int m_nPersonID;

		/// <summary>Human readable name for the person.</summary>
		private string m_sPersonName;

		/// <summary>A human readable list of references.</summary>
		private StringBuilder m_sbReferences;

		#endregion

		#region Constructors etc ...
		
		/// <summary>
		/// Class constructor.
		/// Creates a new clsReferences object attached to the specified person.
		/// </summary>
		/// <param name="nPersonID">Specifies the ID of the person that this references collection is attached to.</param>		
		public clsReferences
			(
			int			nPersonID
			)
		{
			m_nPersonID = nPersonID;
			m_sbReferences = new StringBuilder();			
		}

		/// <summary>
		/// Add a reference to the collection.
		/// </summary>
		/// <param name="sReference">Specifies the human readable description of the reference.</param>
		public void AddReference
			(
			string	sReference
			)
		{
			if(m_sbReferences.Length==0)
			{
				m_sbReferences.Append(sReference);
			}
			else
			{
				m_sbReferences.Append(", ");
				m_sbReferences.Append(sReference);
			}
		}

		#endregion

		#region Public Properties
		
		/// <summary>ID of the person.</summary>
		public int PersonID { get { return m_nPersonID; } }

		/// <summary>Human readable name for the person.</summary>
		public string PersonName { get { return m_sPersonName; } set { m_sPersonName = value; } }

		/// <summary>Human readable list of references.</summary>
		public string References { get { return m_sbReferences.ToString(); } }

		#endregion
	}
}
