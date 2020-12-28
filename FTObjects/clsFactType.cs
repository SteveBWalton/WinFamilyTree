using System;

namespace FamilyTree.Objects
{
	/// <summary>
	/// Class to represent a single fact type.
	/// This is closely related to the <code>tlk_FactTypes</code> table.
	/// </summary>
	public class clsFactType
	{
		#region Member Variables

		/// <summary>ID of the fact type in the database.</summary>
		private int m_nID;

		/// <summary>Human readable name of the fact type.</summary>
		private string m_sName;

		#endregion

		#region Constructors etc ...

		/// <summary>
		/// Class constructor.
		/// Creates a new fact type with the specified properties.
		/// </summary>
		/// <param name="nID">Specifies the ID of the fact type.</param>
		/// <param name="sName">Specifies the human readable name of the fact type.</param>
		public clsFactType
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
		/// Gets a human readable name of the fact type.
		/// </summary>
		/// <returns>A human readable name for the fact type.</returns>
		public override string ToString()
		{
			return m_sName;
		}

		/// <summary>ID of the fact type.</summary>
		public int ID { get { return m_nID; } }

		/// <summary>Human readable name of the fact type.</summary>
		public string Name { get { return m_sName; } }

		#endregion
	}
}
