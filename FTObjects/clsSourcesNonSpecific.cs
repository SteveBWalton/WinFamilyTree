using System;

// DO NOT USE THIS  DO NOT USE THIS  DO NOT USE THIS  DO NOT USE THIS  DO NOT USE THIS  DO NOT USE THIS 

namespace FTObjects
{
	/// <summary>
	/// Class to represent all the sources attached to a person.
	/// This is not sources linked to a specific fact as a normal source.
	/// This includes the sources linked to a person but not to any specific fact.
	/// </summary>
	public class clsSourcesNonSpecific
	{
		#region Member Variables

		/// <summary>The ID of the person that this collection of sources is attached to.</summary>
		private int m_nPersonID;

//		private typNonSpecificSouce[] m_Sources;

		/// <summary>Number of sources.</summary>
		private int m_nNumSources;

		#endregion

		#region Constructors etc ...

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="nPersonID">Specifies the ID of the person that is linked to these sources.</param>
		public clsSourcesNonSpecific
			(
			int nPersonID
			)
		{
			m_nPersonID = nPersonID;
			m_nNumSources = 0;
		}

		#endregion

		public void Add
			(
			int nSourceID
			)
		{
			// Create a new array to hold the extra source
			m_nNumSources++;
//			typNonSpecificSouce[] m_NewSources = new typSourceNonSpecific[m_nNumSources];
		}
	}

	/// <summary>
	/// Class to represent a single source attached to a person but not a specific fact as a normal source.
	/// This is not sources linked to a specific fact as a normal source.
	/// This includes the sources linked to a person but not to any specific fact.
	/// </summary>
	public struct clsSourceNonSpecific
	{
		int nSourceID;
		int nRank;
	}

}
