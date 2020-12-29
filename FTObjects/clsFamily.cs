using System;
using System.Collections;
using System.IO;

namespace FamilyTree.Objects
{
	/// <summary>
	/// Class to represent a GEDCOM family.
	/// These are like clsRelationship objects but more frequent.
	/// </summary>
	public class clsFamily
	{
		/// <summary>ID of the father in the family. 0 or negative indicates no father.</summary>
		public int FatherID;

		/// <summary>ID of the mother in the family.  0 or negative indicates no mother.</summary>
		public int MotherID;

		/// <summary>Unique index for the gedcom file.</summary>
		public int GedComID;

		/// <summary>
		/// ID of the relationship object between the mother and father.  0 indicates no clsRelationship object.
		/// </summary>
		public int RelationshipID;

		/// <summary>Collection of the children in this family.</summary>
		private ArrayList m_oChildren;

		/// <summary>
		/// Empty class constructor.
		/// </summary>
		public clsFamily()
		{
			FatherID = 0;
			MotherID = 0;
			GedComID = 0;
			RelationshipID = 0;
			m_oChildren = null;
		}

		/// <summary>
		/// Adds a child to this family.
		/// The child is added into list in birth order.
		/// </summary>
		/// <param name="oChild">Specifies the child to add to this family.</param>
		public void AddChild
			(
			clsPerson oChild
			)
		{
			if(m_oChildren==null)
			{
				// Create a new list and add the child to the end of the list.
				m_oChildren = new ArrayList();
				m_oChildren.Add(oChild);			
			}
			else
			{
				int nIndex = m_oChildren.Count-1;
				clsPerson oInsert = (clsPerson)m_oChildren[nIndex];
				if(oChild.dob.Date>oInsert.dob.Date)
				{
					// Add the child to the end of the list
					m_oChildren.Add(oChild);			
				}
				else
				{
					nIndex = 0;
					oInsert = (clsPerson)m_oChildren[nIndex];
					if(oChild.dob.Date<oInsert.dob.Date)
					{
						// Add the child to the start of the list
						m_oChildren.Insert(nIndex,oChild);
					}
					else
					{
						// Find the existing child to add this child in front of.
						while(oChild.dob.Date>oInsert.dob.Date)
						{
							nIndex++;
							oInsert = (clsPerson)m_oChildren[nIndex];
						}
						m_oChildren.Insert(nIndex,oChild);
					}
				}
			}
		}

        // Writes the details of this family into the specified Gedcom file.
        /// <summary>
        /// Writes the details of this family into the specified Gedcom file.
        /// </summary>
		/// <param name="oFile">Specifies the Gedcom file to write the details into.</param>
		/// <param name="oDb">Specifies the database to fetch additional information from.</param>
        public void WriteGedcom(StreamWriter oFile, Database oDb, clsGedcomOptions oOptions)
        {
            // Create a list of the sources
            ArrayList oFamilySources = new ArrayList();

            oFile.WriteLine("0 @F" + GedComID.ToString("0000") + "@ FAM");
            if(MotherID > 0)
            {
                oFile.WriteLine("1 WIFE @I" + MotherID.ToString("0000") + "@");
            }
            if(FatherID > 0)
            {
                oFile.WriteLine("1 HUSB @I" + FatherID.ToString("0000") + "@");
            }

            if(MotherID > 0 && FatherID > 0)
            {
                clsRelationship oMarriage = oDb.GetRelationship(FatherID, MotherID);
                if(oMarriage != null)
                {
                    oMarriage.SetDb(oDb);

                    if(oMarriage.IsMarried())
                    {
                        oFile.WriteLine("1 MARR");
                        switch(oMarriage.TypeID)
                        {
                        case 1:
                            oFile.WriteLine("2 TYPE Religious");
                            break;
                        case 2:
                            oFile.WriteLine("2 TYPE Civil");
                            break;
                        }

                        if(!oMarriage.Start.IsEmpty())
                        {
                            oFile.WriteLine("2 DATE " + oMarriage.Start.Format(DateFormat.Gedcom));
                        }
                        oDb.WriteGedcomPlace(oFile, 2, oMarriage.Location,oOptions);

                        // ArrayList oAlready = new ArrayList();
                        //oMarriage.SourceStart.GedcomWrite(2,oFile,oAlready);
                        //oMarriage.SourceLocation.GedcomWrite(2,oFile,oAlready);					
                        oMarriage.SourceStart.GedcomAdd(oFamilySources);
                        oMarriage.SourceLocation.GedcomAdd(oFamilySources);

                        // Did the marriage end with a divorce
                        if(oMarriage.TerminatedID == 2)
                        {
                            if(oMarriage.End.IsEmpty())
                            {
                                oFile.WriteLine("1 DIV Y");
                            }
                            else
                            {
                                oFile.WriteLine("1 DIV");
                                oFile.WriteLine("2 DATE " + oMarriage.End.Format(DateFormat.Gedcom));
                            }
                            // oAlready = new ArrayList();
                            // oMarriage.SourceTerminated.GedcomWrite(2,oFile,oAlready);
                            // oMarriage.SourceEnd.GedcomWrite(2,oFile,oAlready);
                            oMarriage.SourceTerminated.GedcomAdd(oFamilySources);
                            oMarriage.SourceEnd.GedcomAdd(oFamilySources);
                        }
                    }

                    // Write the sources for this family
                    foreach(int nSourceID in oFamilySources)
                    {
                        oFile.WriteLine("1 SOUR @S" + nSourceID.ToString("0000") + "@");
                    }

                    // Last Edit
                    if(oMarriage.LastEditBy != "")
                    {
                        oFile.WriteLine("1 CHAN");
                        oFile.WriteLine("2 DATE " + oMarriage.LastEditDate.ToString("d MMM yyyy"));
                        oFile.WriteLine("3 TIME " + oMarriage.LastEditDate.ToString("HH:mm:ss"));
                        if(oOptions.IncludePGVU)
                        {
                            oFile.WriteLine("2 _PGVU " + oMarriage.LastEditBy);
                        }
                    }
                }
            }

            if(m_oChildren != null)
            {
                foreach(clsPerson oChild in m_oChildren)
                {
                    oFile.WriteLine("1 CHIL @I" + oChild.ID.ToString("0000") + "@");
                }
            }
        }
	}
}
