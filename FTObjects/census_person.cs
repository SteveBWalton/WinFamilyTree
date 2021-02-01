using System;

namespace family_tree.objects
{
    /// <summary>Class to represent a member of a census household.  This is usually a one to one relationship with a clsPerson object.  However, some people can be a member of a census without being in the database eg Boarders, Servants.</summary>
    public class CensusPerson
    {
        #region Member Variables

        /// <summary>The Key ID of this record in the database.</summary>
        private int index_;

        /// <summary>The key ID of the parent Census household record that this person is a member of.</summary>
        private int houseHoldIndex_;

        /// <summary>The name of the parent census household record.</summary>
        private string houseHoldName_;

        /// <summary>The key ID of the person that this record refers too.  Can be 0 (null in database) for someone in the census report but not the database.</summary>
        private int personIndex_;

        /// <summary>The name of the person specified in m_nPersonID.  Can be empty.</summary>
        private string personName_;

        /// <summary>The name of the person as written in the census record.</summary>
        private string censusName_;

        /// <summary>The relation of this person to the head of the household as specified on the census record.</summary>
        private string relationToHead_;

        /// <summary>The age of the person as specified on the census record.</summary>
        private string age_;

        /// <summary>The occupation of the person as specified on the census record.</summary>
        private string occupation_;

        /// <summary>The location the person was born as specified on the census record.</summary>
        private string bornLocation_;

        /// <summary>The date of birth of the person as specified on the census record.  This is only specified on the 1939 register.</summary>
        private string dateOfBirth_;

        private string sex_;
        private string maritalStatus_;

        /// <summary>True if the record should be removed from the database.</summary>
        private bool isDelete_;

        /// <summary>The date that the parent census was taken.</summary>
        private DateTime date_;

        #endregion

        #region Constructors



        /// <summary>Empty class constructor.</summary>
        public CensusPerson()
        {
            houseHoldName_ = "";
            personName_ = "";
            censusName_ = "";
            relationToHead_ = "";
            age_ = "";
            occupation_ = "";
            bornLocation_ = "";
            dateOfBirth_ = "";
            isDelete_ = false;
        }



        #endregion



        /// <summary>Updates this census member in the database.</summary>
        /// <param name="database">Specifies the database to write the record into.</param>
        /// <returns></returns>
        public bool save(Database database)
        {
            return database.censusSavePerson(this);
        }



        /// <summary>Returns a human readable string describing the other members of the household.</summary>
        /// <param name="database">Specifies the database containing the household.</param>
        /// <returns>A human readable string desribing the other members of the household.</returns>
        public string livingWith(Database database)
        {
            return database.censusLivingWith(this);
        }



        /// <summary>Marks this record for delete.</summary>
        public void delete()
        {
            isDelete_ = true;
        }



        /// <summary>Returns true if this record is valid.  Otherwise this record should be deleted.</summary>
        /// <returns>True if the record is valid, false if the record is scheduled for delete.</returns>
        public bool isValid()
        {
            return !isDelete_;
        }



        /// <summary>Returns the sources for this census member.  In fact the single source attached to census household will be returned.</summary>
        /// <param name="database">Specifies the database to read the sources from.</param>
        /// <returns>A clsSources object containing all the sources for this piece of information.</returns>
        public Sources getSources(Database database)
        {
            return new Sources(this, database);
        }



        #region Properties

        /// <summary>The Key ID of this record in the database.</summary>
        public int index { get { return index_; } set { index_ = value; } }

        /// <summary>The key ID of the parent Census household record that this person is a member of.</summary>
        public int houseHoldIndex { get { return houseHoldIndex_; } set { houseHoldIndex_ = value; } }

        /// <summary>The name of the parent census household record.</summary>
        public string houseHoldName { get { return houseHoldName_; } set { houseHoldName_ = value; } }

        /// <summary>The key ID of the person that this record refers too.  Can be 0 (null in database) for someone in the census report but not the database.</summary>
        public int personIndex { get { return personIndex_; } set { personIndex_ = value; } }

        /// <summary>The name of the person specified in PersonID.  Can be empty.</summary>
        public string personName { get { return personName_; } set { personName_ = value; } }

        /// <summary>The name of the person as written in the census record.</summary>
        public string censusName { get { return censusName_; } set { censusName_ = value; } }

        /// <summary>The relation of this person to the head of the household as specified on the census record.</summary>
        public string relationToHead { get { return relationToHead_; } set { relationToHead_ = value; } }

        /// <summary>The age of the person as specified on the census record.</summary>
        public string age { get { return age_; } set { age_ = value; } }

        /// <summary>The occupation of the person as specified on the census record.</summary>
        public string occupation { get { return occupation_; } set { occupation_ = value; } }

        /// <summary>The location the person was born as specified on the census record.</summary>
        public string bornLocation { get { return bornLocation_; } set { bornLocation_ = value; } }

        /// <summary>The date of birth of the person as specified on the census record.  This is only available on the 1939 register.</summary>
        public string dateOfBirth { get { return dateOfBirth_; } set { dateOfBirth_ = value; } }
        public string sex { get { return sex_; } set { sex_ = value; } }

        public string maritalStatus { get { return maritalStatus_; } set { maritalStatus_ = value; } }

        

        /// <summary>The date that the parent census was taken.</summary>
        public DateTime date { get { return date_; } set { date_ = value; } }

        #endregion
    }
}
