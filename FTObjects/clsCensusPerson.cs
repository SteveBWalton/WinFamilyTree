using System;

namespace FamilyTree.Objects
{
    // Class to represent a member of a census household.
    /// <summary>
    /// Class to represent a member of a census household.
    /// This is usually a one to one relationship with a clsPerson object.
	/// However, some people can be a member of a census without being in the database eg Boarders, Servants.
	/// </summary>
	public class clsCensusPerson
	{
		#region Member Variables

		/// <summary>The Key ID of this record in the database.</summary>
		private int m_nID;
		
		/// <summary>The key ID of the parent Census household record that this person is a member of.</summary>
		private int m_nHouseholdID;
		
		/// <summary>The name of the parent census household record.</summary>
		private string m_sHouseholdName;
		
		/// <summary>The key ID of the person that this record refers too.  Can be 0 (null in database) for someone in the census report but not the database.</summary>
		private int m_nPersonID;
		
		/// <summary>The name of the person specified in m_nPersonID.  Can be empty.</summary>
		private string m_sPersonName;

		/// <summary>The name of the person as written in the census record.</summary>
		private string m_sCensusName;
		
		/// <summary>The relation of this person to the head of the household as specified on the census record.</summary>
		private string m_sRelationToHead;

		/// <summary>The age of the person as specified on the census record.</summary>
		private string m_sAge;

		/// <summary>The occupation of the person as specified on the census record.</summary>
		private string m_sOccupation;

		/// <summary>The location the person was born as specified on the census record.</summary>
		private string m_sBornLocation;
		
		/// <summary>True if the record should be removed from the database.</summary>
		private bool m_bDelete;

		/// <summary>The date that the parent census was taken.</summary>
		private DateTime m_dtDate;

		#endregion

		#region Constructors

        // Empty class constructor
        /// <summary>
        /// Empty class constructor
        /// </summary>
		public clsCensusPerson()
		{
			m_sHouseholdName = "";
			m_sPersonName = "";
			m_sCensusName = "";
			m_sRelationToHead = "";
			m_sAge = "";
			m_sOccupation = "";
			m_sBornLocation = "";
			m_bDelete = false;
		}

		#endregion

        // Updates this census member in the database.
        /// <summary>
        /// Updates this census member in the database.
        /// </summary>
		/// <param name="oDb">Specifies the database to write the record into.</param>
		/// <returns></returns>
        public bool Save(Database oDb)
        {
            return oDb.CensusSavePerson(this);
        }

        // Returns a human readable string describing the other members of the household.
        /// <summary>
        /// Returns a human readable string describing the other members of the household.
        /// </summary>
		/// <param name="oDb">Specifies the database containing the household.</param>
		/// <returns>A human readable string desribing the other members of the household.</returns>
        public string LivingWith(Database oDb)
        {
            return oDb.CensusLivingWith(this);
        }

        // Marks this record for delete.
        /// <summary>
        /// Marks this record for delete.
        /// </summary>
		public void Delete()
		{
			m_bDelete = true;
		}

        // Returns true if this record is valid.
        /// <summary>
        /// Returns true if this record is valid.
        /// Otherwise this record should be deleted.
		/// </summary>
		/// <returns>True if the record is valid, false if the record is scheduled for delete.</returns>
		public bool IsValid()
		{
			return !m_bDelete;
		}

        // Returns the sources for this census member.
        /// <summary>
        /// Returns the sources for this census member.
        /// In fact the single source attached to census household will be returned.
		/// </summary>
		/// <param name="oDb">Specifies the database to read the sources from.</param>
		/// <returns>A clsSources object containing all the sources for this piece of information.</returns>
        public clsSources GetSources(Database oDb)
        {
            return new clsSources(this, oDb);
        }

		#region Properties

		/// <summary>The Key ID of this record in the database.</summary>
		public int ID { get { return m_nID; } set { m_nID=value; } }

		/// <summary>The key ID of the parent Census household record that this person is a member of.</summary>
		public int HouseholdID { get { return m_nHouseholdID; } set { m_nHouseholdID = value; } }

		/// <summary>The name of the parent census household record.</summary>
		public string HouseholdName { get { return m_sHouseholdName; } set { m_sHouseholdName = value; } }

		/// <summary>The key ID of the person that this record refers too.  Can be 0 (null in database) for someone in the census report but not the database.</summary>
		public int PersonID { get { return m_nPersonID; } set { m_nPersonID = value; } }
		
		/// <summary>The name of the person specified in PersonID.  Can be empty.</summary>
		public string PersonName { get { return m_sPersonName; } set { m_sPersonName = value; } }

		/// <summary>The name of the person as written in the census record.</summary>
		public string CensusName { get { return m_sCensusName; } set { m_sCensusName = value; } }
		
		/// <summary>The relation of this person to the head of the household as specified on the census record.</summary>
		public string RelationToHead { get { return m_sRelationToHead; } set { m_sRelationToHead = value; } }

		/// <summary>The age of the person as specified on the census record.</summary>
		public string Age { get { return m_sAge; } set { m_sAge = value; } }
		
		/// <summary>The occupation of the person as specified on the census record.</summary>
		public string Occupation { get { return m_sOccupation; } set { m_sOccupation = value; } }

		/// <summary>The location the person was born as specified on the census record.</summary>
		public string BornLocation { get { return m_sBornLocation; } set { m_sBornLocation = value; } }

		/// <summary>The date that the parent census was taken.</summary>
		public DateTime Date { get { return m_dtDate; } set { m_dtDate = value; } }

		#endregion
	}
}
