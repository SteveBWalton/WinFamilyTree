using System;

namespace FamilyTree.Objects
{
	/// <summary>
	/// Class to represent generic ID , Name pairs from a database.
	/// This is most often to used create a list of lookups
	/// </summary>
	public class clsIDName
	{
		#region Member Variables

		/// <summary>ID of the object.</summary>
		private int m_nID;

		/// <summary>Human readable name of the object.</summary>
		private string m_sName;

		#endregion

		#region Constructors etc ...
		
		/// <summary>
		/// Class constructors.
		/// Creates a new clsIDName object with the specified properties.
		/// </summary>
		/// <param name="nID">Specifies the ID of the object.</param>
		/// <param name="sName">Specifies the name of the object.</param>
		public clsIDName
			(
			int nID,
			string sName
			)
		{
			m_nID = nID;
			m_sName = sName;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Gets a human readable name of the clsIDName object.
		/// </summary>
		/// <returns>A human readable name for the clsIDName object.</returns>
		public override string ToString()
		{
			return m_sName;
		}

		/// <summary>The ID of the clsIDName object.</summary>
		public int ID { get { return m_nID; } }

		/// <summary>The human readable label for the clsIDName object.</summary>
		public string Name { get { return m_sName; } }

		#endregion
	}
}
