using System;
using System.Data;
using System.Data.OleDb;
using System.Collections;

// StringBuilder
using System.Text;

namespace family_tree.objects
{
    /// <summary>Class to represent a single source of information.</summary>
    public class Source
    {
        #region Member Variables

        /// <summary>Database that contains this source.</summary>
        private Database database_;

        /// <summary>ID of the source in the database.</summary>
        private int idx_;

        /// <summary>Text to display for the source.</summary>
        private string description_;

        /// <summary>Date of the source (document).</summary>
        private CompoundDate theDate_;

        /// <summary>Comments for the source.</summary>
        private string comments_;

        /// <summary>Date and time this source was last used.</summary>
        private DateTime lastUsed_;

        /// <summary>True, if this source needs writing to the database.  False, otherwise.</summary>
        private bool isDirty_;

        /// <summary>True if the source should be deleted at next save.  False, otherwise.</summary>
        private bool isDelete_;

        /// <summary>The type of additional information available for this source.  0 - None.</summary>
        private int additionalInfoTypeIdx_;

        /// <summary>Name of the user who wrote the last edit.</summary>
        private string lastEditBy_;

        /// <summary>Date and time of the last edit.</summary>
        private DateTime lastEditDate_;

        /// <summary>The optional additional census information.</summary>
        private Census additionCensus_;

        /// <summary>The optional additional marriage information.</summary>
        private MarriageCertificate additionMarriage_;

        /// <summary>The optional additional birth certificate information.</summary>
        private BirthCertificate additionalBirth_;

        /// <summary>The optional additional death certificate information.</summary>
        private DeathCertificate additionalDeath_;

        /// <summary>The optional additional free table information for this source.</summary>
        private SourceFreeTable freeTable_;

        /// <summary>The index for the source.  Zero represents no repository.</summary>
        private int repositoryIdx_;

        /// <summary>This is the ranking of the source within a collection.  This is not the property of the source but of the source collection table.</summary>
        private int ranking_;

        #endregion

        #region Constructors



        /// <summary>Class constructor for an empty source.</summary>
        /// <param name="database">Specify the database containing the source.</param>
        public Source(Database database)
        {
            database_ = database;
            description_ = "";
            comments_ = "";
            theDate_ = new CompoundDate();
            isDirty_ = false;
            isDelete_ = false;
            additionalInfoTypeIdx_ = 0;
            lastEditBy_ = "Steve Walton";
            lastEditDate_ = DateTime.Now;
            additionCensus_ = null;
            repositoryIdx_ = 0;
            ranking_ = 1;
        }



        /// <summary>Class constructor to load an existing source from the database.</summary>
        /// <param name="database">Specify the database containing the source.</param>
        /// <param name="index">Specify the ID of the source.</param>
        public Source(Database database, int index) : this(database)
        {
            // Open the database and get the source details.
            OleDbCommand sqlCommand = new OleDbCommand("SELECT Name, LastUsed, TheDate, TheDateStatusID, Comments, AdditionalInfoTypeID, LastEditBy, LastEditDate, RepositoryID FROM tbl_Sources WHERE ID = " + index.ToString() + ";", database_.cndb);
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            if (dataReader.Read())
            {
                idx_ = index;
                if (dataReader.IsDBNull(0))
                {
                    description_ = "";
                }
                else
                {
                    description_ = dataReader.GetString(0);
                }
                if (dataReader.IsDBNull(1))
                {
                }
                else
                {
                    lastUsed_ = dataReader.GetDateTime(1);
                }
                if (dataReader.IsDBNull(2))
                {
                    theDate_.status = CompoundDate.EMPTY;
                }
                else
                {
                    theDate_.date = dataReader.GetDateTime(2);
                    theDate_.status = dataReader.GetInt32(3);
                }
                if (dataReader.IsDBNull(4))
                {
                    comments_ = "";
                }
                else
                {
                    comments_ = dataReader.GetString(4);
                }
                if (dataReader.IsDBNull(5))
                {
                    additionalInfoTypeIdx_ = 0;
                }
                else
                {
                    additionalInfoTypeIdx_ = dataReader.GetInt32(5);
                }
                if (dataReader.IsDBNull(6))
                {
                    lastEditBy_ = "Steve Walton";
                }
                else
                {
                    lastEditBy_ = dataReader.GetString(6);
                }
                if (dataReader.IsDBNull(7))
                {
                    lastEditDate_ = DateTime.Now;
                }
                else
                {
                    lastEditDate_ = dataReader.GetDateTime(7);
                }
                repositoryIdx_ = Database.getInt(dataReader, "RepositoryID", 0);
            }
            dataReader.Close();
        }



        #endregion

        #region Database



        /// <summary>Write the changes in this source to the database.</summary>
        /// <returns>True for success, false otherwise.</returns>
        public bool save()
        {
            OleDbCommand sqlCommand;

            // Check if a delete is required.
            if (isDelete_)
            {
                if (idx_ != 0)
                {
                    // Remove the links to places from this source.
                    database_.placeDelink(2, idx_);

                    // Delete the sources that contain this source.

                    // Delete this source.
                    sqlCommand = new OleDbCommand("DELETE FROM tbl_Sources WHERE ID = " + idx_ + ";", database_.cndb);
                    sqlCommand.ExecuteNonQuery();
                }

                // Return success.
                return true;
            }

            // Check if a save is required.
            if (isDirty_)
            {
                // Create a new record if required.
                if (idx_ == 0)
                {
                    // Create a new record.
                    sqlCommand = new OleDbCommand("INSERT INTO tbl_Sources (LastUsed) VALUES(Now());", database_.cndb);
                    sqlCommand.ExecuteNonQuery();

                    // Get the ID of the new record.
                    sqlCommand = new OleDbCommand("SELECT MAX(ID) AS NewID FROM tbl_Sources;", database_.cndb);
                    idx_ = (int)sqlCommand.ExecuteScalar();
                }

                // Update the record.
                sqlCommand = new OleDbCommand
                    (
                    "UPDATE tbl_Sources SET Name = " + Database.toDb(description_)
                    + ", TheDate = " + Database.toDb(theDate_)
                    + ", TheDateStatusID = " + theDate_.status.ToString()
                    + ", Comments = " + Database.toDb(comments_)
                    + ", AdditionalInfoTypeID = " + additionalInfoTypeIdx_.ToString()
                    + ", RepositoryID = " + repositoryIdx_.ToString()
                    + ", LastEditBy = 'Steve Walton'"
                    + ", LastEditDate = Now()"
                    + " WHERE ID = " + idx_.ToString() + ";"
                    , database_.cndb
                    );
                sqlCommand.ExecuteNonQuery();
                isDirty_ = false;

                // Remove the link to places from this source.
                database_.placeDelink(2, idx_);
            }

            switch (additionalInfoTypeIdx_)
            {
            case 1:
                if (additionalBirth_ != null)
                {
                    additionalBirth_.idx = idx_;
                    additionalBirth_.save(database_);
                }
                break;

            case 2:
                if (additionMarriage_ != null)
                {
                    additionMarriage_.idx = idx_;
                    additionMarriage_.save(database_);
                }
                break;

            case 3:
                if (additionalDeath_ != null)
                {
                    additionalDeath_.idx = idx_;
                    additionalDeath_.save(database_);
                }
                break;

            case 4:
                if (additionCensus_ != null)
                {
                    additionCensus_.idx = idx_;
                    additionCensus_.censusDate = theDate_.date;
                    additionCensus_.save(database_);
                }
                break;
            
            case 5:
                if (freeTable_!= null)
                {
                    freeTable.save();
                }
                break;
            }

            // Return success.
            return true;
        }



        /// <summary>Marks this source for deletion at the next save.</summary>
        public void delete()
        {
            isDelete_ = true;
        }



        #endregion



        /// <summary>Returns a Html description of the source.</summary>
        /// <returns></returns>
        public string toHtml()
        {
            // Build a Html description of the source.
            StringBuilder html = new StringBuilder();

            // Intialise the Html document.
            html.Append("<body>");

            // The basic description of the source.
            html.Append("<p>");
            html.Append(description_);
            html.Append(" (" + theDate_.format(DateFormat.FULL_LONG) + ")");
            html.Append("</p>");

            // Show the document specified information.
            switch (additionalInfoTypeIdx_)
            {
            case 1:
                if (additionalBirth != null)
                {
                    html.Append(additionalBirth.toHtml());
                }
                break;
            case 2:
                if (additionalMarriage != null)
                {
                    html.Append(additionalMarriage.toHtml(database_));
                }
                break;
            case 3:
                if (additionalDeath != null)
                {
                    html.Append(additionalDeath.toHtml(database_));
                }
                break;
            case 4:
                if (additionalCensus != null)
                {
                    html.Append(additionalCensus.toHtml());
                }
                break;
            case 5:
                if (freeTable != null)
                {
                    html.Append(freeTable.toHtml());
                }
                break;
            }

            // Show the public comments.
            if (comments_ != "")
            {
                html.Append("<p><strong>Public comments</strong>: " + comments_ + "</p>");
            }

            // Show the objects that are linked to this document.
            html.Append("<p>References</p><table cellpadding=3 cellspacing=2>");
            References[] references = getReferences();
            foreach (References reference in references)
            {
                html.Append("<TR>");
                html.Append("<TD bgcolor=silver><SPAN class=\"Small\"><A href=\"Person:" + reference.personIdx.ToString() + "\">" + reference.personName + "</A></SPAN></TD>");
                html.Append("<TD bgcolor=silver><SPAN class=\"Small\">" + reference.references + "</SPAN></TD>");
                html.Append("</TR>");
            }
            html.Append("</TABLE>");

            // Development Only.
            // Show a version of this document in the format that we will use on webtrees.
#pragma warning disable 162
            if (false)
            {
                switch (additionalInfoTypeIdx_)
                {
                case 1:
                    if (additionalBirth != null)
                    {
                        html.Append("<h2>Webtrees</h2><p class=\"small\">");
                        html.Append(additionalBirth.toWebtrees());
                        html.Append("</p>\n");
                    }
                    break;
                case 2:
                    if (additionalMarriage != null)
                    {
                        html.Append("<h2>Webtrees</h2><p class=\"small\">");
                        html.Append(additionalMarriage.toWebtrees(database_));
                        html.Append("</p>\n");
                    }
                    break;
                case 3:
                    if (additionalDeath != null)
                    {
                        html.Append("<h2>Webtrees</h2><p class=\"small\">");
                        html.Append(additionalDeath.toWebtrees(database_));
                        html.Append("</p>\n");
                    }
                    break;
                case 4:
                    if (additionalCensus != null)
                    {
                        html.Append("<h2>Webtrees</h2><p class=\"small\">");
                        html.Append(additionalCensus.toWebtrees());
                        html.Append("</p>\n");
                    }
                    break;
                }
            }
#pragma warning restore 162

            // Finish the html document.
            html.Append("<p style=\"text-align: right;\"><span class=\"small\">Last edit by " + lastEditBy_ + " on " + lastEditDate_.ToString("d-MMM-yyyy HH:mm:ss") + "</span></p>");
            html.Append("</body>");
            html.Append("</html>");

            // Return the html description.
            return html.ToString();
        }


        /// <summary>Returns true for a valid source.  Returns false for a source marked for deletion currently.</summary>
        /// <returns>True for a valid source, false otherwise</returns>
        public bool isValid()
        {
            return !isDelete_;
        }



        /// <summary>Returns a human readable summary of the source.</summary>
        /// <returns>A human readable summary of the source.</returns>
        public override string ToString()
        {
            if (theDate_.isEmpty())
            {
                return description_;
            }
            return description_ + " (" + theDate_.date.Year.ToString() + ")";
        }



        /// <summary>Returns an array of clsReferences objects that list all the "facts" that reference this source.</summary>
        /// <returns>An array of clsReferences objects</returns>
        public References[] getReferences()
        {
            // Create an empty list.
            ArrayList references = new ArrayList();

            // Get all the references in the people objects.
            string sql = "SELECT tbl_PeopleToSources.PersonID, tlk_PeopleInfo.Name AS FactName, tbl_PeopleToSources.FactID " +
                "FROM tbl_People INNER JOIN (tbl_PeopleToSources INNER JOIN tlk_PeopleInfo ON tbl_PeopleToSources.FactID = tlk_PeopleInfo.ID) ON tbl_People.ID = tbl_PeopleToSources.PersonID " +
                "WHERE (((tbl_PeopleToSources.SourceID)=" + idx_.ToString() + ")) " +
                "ORDER BY tbl_People.Born;";
            OleDbCommand sqlCommand = new OleDbCommand(sql, database_.cndb);
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                int idx = getReferenceIdx(references, dataReader.GetInt32(0));
                // Don't add the non specific references.
                if (Database.getInt(dataReader, "FactID", 0) != 0)
                {
                    ((References)references[idx]).addReference(dataReader.GetString(1));
                }
            }
            dataReader.Close();

            // Get all the references in the fact objects.
            sql = "SELECT tbl_Facts.PersonID, tlk_FactTypes.Name FROM tlk_FactTypes INNER JOIN (tbl_Facts INNER JOIN tbl_FactsToSources ON tbl_Facts.ID = tbl_FactsToSources.FactID) ON tlk_FactTypes.ID = tbl_Facts.TypeID WHERE (((tbl_FactsToSources.SourceID)=" + idx_.ToString() + "));";
            sqlCommand = new OleDbCommand(sql, database_.cndb);
            dataReader = sqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                int idx = getReferenceIdx(references, dataReader.GetInt32(0));
                ((References)references[idx]).addReference(dataReader.GetString(1));
            }
            dataReader.Close();

            // Get all the references in the relationship objects.
            sql = "SELECT tbl_Relationships.MaleID, tbl_Relationships.FemaleID, tlk_RelationshipInfo.Name FROM tbl_Relationships INNER JOIN (tbl_RelationshipsToSources INNER JOIN tlk_RelationshipInfo ON tbl_RelationshipsToSources.FactID = tlk_RelationshipInfo.ID) ON tbl_Relationships.ID = tbl_RelationshipsToSources.RelationshipID WHERE (((tbl_RelationshipsToSources.SourceID)=" + idx_.ToString() + "));";
            sqlCommand = new OleDbCommand(sql, database_.cndb);
            dataReader = sqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                int idx = getReferenceIdx(references, dataReader.GetInt32(0));
                ((References)references[idx]).addReference("Relationship " + dataReader.GetString(2));
                idx = getReferenceIdx(references, dataReader.GetInt32(1));
                ((References)references[idx]).addReference("Relationship " + dataReader.GetString(2));
            }
            dataReader.Close();

            // Get names for all the people.
            for (int i = 0; i < references.Count; i++)
            {
                References reference = (References)references[i];
                Person person = new Person(reference.personIdx, database_);
                reference.personName = person.getName(true, false);
            }

            // Return the references found.
            return (References[])references.ToArray(typeof(References));
        }



        /// <summary>Returns the index of the specified person in the specified list of references.  Added the person to the specified list if required.</summary>
        /// <param name="references">Specifies the list of existing references.</param>
        /// <param name="personIdx">Specifies the person to locate in / add to the reference list.</param>
        /// <returns>The index of the specified person in the specified list.</returns>
        private int getReferenceIdx(ArrayList references, int personIdx)
        {
            int idx;

            // Find the index of the person.
            idx = -1;
            for (int i = 0; i < references.Count; i++)
            {
                if (personIdx == ((References)references[i]).personIdx)
                {
                    idx = i;
                    break;
                }
            }
            if (idx == -1)
            {
                // Add a new reference.
                References reference = new References(personIdx);

                references.Add(reference);
                idx = references.Count - 1;
            }

            return idx;
        }



        #region Public Properties

        /// <summary>ID to this source record in the database.</summary>
        public int idx { get { return idx_; } }

        /// <summary>The database that contains this source.</summary>
        public Database database {  get { return database_; } }

        /// <summary>Human readable summary of this source.</summary>
        public string label { get { return this.ToString(); } }

        /// <summary> Detailed description of this source.</summary>
        public string description { get { return description_; } set { description_ = value; isDirty_ = true; } }

        /// <summary>The date of this source.</summary>
        public CompoundDate theDate { get { return theDate_; } }

        /// <summary>User comments on this source.</summary>
        public string comments { get { return comments_; } set { comments_ = value; isDirty_ = true; } }

        /// <summary>The date / time that this source was last cited for a fact.</summary>
        public DateTime lastUsed { get { return lastUsed_; } set { lastUsed_ = value; isDirty_ = true; } }

        /// <summary>The type of additional information available for this source.  0 - None.</summary>
        public int additionalInfoTypeIdx { get { return additionalInfoTypeIdx_; } set { additionalInfoTypeIdx_ = value; isDirty_ = true; } }

        /// <summary>Name of the user who wrote the last edit.</summary>
        public string lastEditBy { get { return lastEditBy_; } set { lastEditBy_ = value; } }

        /// <summary>Date and time of the last edit.</summary>
        public DateTime lastEditDate { get { return lastEditDate_; } set { lastEditDate_ = value; } }

        /// <summary>This is the ranking of the source within a collection.  This is not the property of the source but of the source collection table.</summary>
        public int ranking { get { return ranking_; } set { ranking_ = value; } }



        /// <summary>The additional census information.  Only valid if AdditionalTypeID == Census (4).</summary>
        public Census additionalCensus
        {
            get
            {
                if (additionCensus_ == null)
                {
                    additionCensus_ = new Census(idx_, database_);
                }
                return additionCensus_;
            }
        }



        /// <summary>The optional additional marriage information.  Only valid in AdditionalTypeID == Marriage (2).</summary>
        public MarriageCertificate additionalMarriage
        {
            get
            {
                if (additionMarriage_ == null)
                {
                    additionMarriage_ = new MarriageCertificate(idx_, database_.cndb);
                }
                return additionMarriage_;
            }
        }



        /// <summary>The optional additional birth certificate information.  Only valid if the AdditionalTypeID == Birth (1).</summary>
        public BirthCertificate additionalBirth
        {
            get
            {
                if (additionalBirth_ == null)
                {
                    additionalBirth_ = new BirthCertificate(idx_, database_.cndb);
                }
                return additionalBirth_;
            }
        }



        /// <summary>The optional additional death certificate information.  Only valid if the AdditionalTypeID == Death (3).</summary>
        public DeathCertificate additionalDeath
        {
            get
            {
                if (additionalDeath_ == null)
                {
                    additionalDeath_ = new DeathCertificate(idx_, database_.cndb);
                }
                return additionalDeath_;
            }
        }



        /// <summary>The optional additional free table information.  Only valid if the additionalTypeIdx == Free Table (5).</summary>
        public SourceFreeTable freeTable
        {
            get 
            {
                if (freeTable_ == null)
                {
                    freeTable_ = new SourceFreeTable(this);
                }
                return freeTable_;
            }
        }



        /// <summary>The ID for repository of the the source.  Zero represents no repository.</summary>
        public int repositoryIdx { get { return repositoryIdx_; } set { repositoryIdx_ = value; } }

        #endregion
    }
}
