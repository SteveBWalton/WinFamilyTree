using System;
using System.Collections;
using System.IO;

namespace family_tree.objects
{
    /// <summary>Class to represent a GEDCOM family.  These are like Relationship objects but more frequent.</summary>
    public class Family
    {
        /// <summary>ID of the father in the family. 0 or negative indicates no father.</summary>
        public int fatherIdx;

        /// <summary>ID of the mother in the family.  0 or negative indicates no mother.</summary>
        public int motherIdx;

        /// <summary>Unique index for the gedcom file.</summary>
        public int gedcomIdx;

        /// <summary>ID of the relationship object between the mother and father.  0 indicates no relationship object.</summary>
        public int relationshipIdx;

        /// <summary>Collection of the children in this family.</summary>
        private ArrayList children;



        /// <summary>Empty class constructor.</summary>
        public Family()
        {
            fatherIdx = 0;
            motherIdx = 0;
            gedcomIdx = 0;
            relationshipIdx = 0;
            children = null;
        }



        /// <summary>Adds a child to this family.  The child is added into list in birth order.</summary>
        /// <param name="child">Specifies the child to add to this family.</param>
        public void addChild(Person child)
        {
            if (children == null)
            {
                // Create a new list and add the child to the end of the list.
                children = new ArrayList();
                children.Add(child);
            }
            else
            {
                int idx = children.Count - 1;
                Person insert = (Person)children[idx];
                if (child.dob.date > insert.dob.date)
                {
                    // Add the child to the end of the list.
                    children.Add(child);
                }
                else
                {
                    idx = 0;
                    insert = (Person)children[idx];
                    if (child.dob.date < insert.dob.date)
                    {
                        // Add the child to the start of the list.
                        children.Insert(idx, child);
                    }
                    else
                    {
                        // Find the existing child to add this child in front of.
                        while (child.dob.date > insert.dob.date)
                        {
                            idx++;
                            insert = (Person)children[idx];
                        }
                        children.Insert(idx, child);
                    }
                }
            }
        }



        /// <summary>Writes the details of this family into the specified Gedcom file.</summary>
        /// <param name="file">Specifies the Gedcom file to write the details into.</param>
        /// <param name="database">Specifies the database to fetch additional information from.</param>
        public void writeGedcom(StreamWriter file, Database database, GedcomOptions options)
        {
            // Create a list of the sources.
            ArrayList familySources = new ArrayList();

            file.WriteLine("0 @F" + gedcomIdx.ToString("0000") + "@ FAM");
            if (motherIdx > 0)
            {
                file.WriteLine("1 WIFE @I" + motherIdx.ToString("0000") + "@");
            }
            if (fatherIdx > 0)
            {
                file.WriteLine("1 HUSB @I" + fatherIdx.ToString("0000") + "@");
            }

            if (motherIdx > 0 && fatherIdx > 0)
            {
                Relationship marriage = database.getRelationship(fatherIdx, motherIdx);
                if (marriage != null)
                {
                    marriage.setDatabase(database);

                    if (marriage.isMarried())
                    {
                        file.WriteLine("1 MARR Y");
                        switch (marriage.typeIdx)
                        {
                        case 1:
                            file.WriteLine("2 TYPE RELIGIOUS");
                            break;
                        case 2:
                            file.WriteLine("2 TYPE CIVIL");
                            break;
                        }

                        if (!marriage.start.isEmpty())
                        {
                            file.WriteLine("2 DATE " + marriage.start.format(DateFormat.GEDCOM));
                            if (options.isAllElements)
                            {
                                marriage.sourceStart.writeGedcom(3, file, null);
                            }
                        }
                        database.writeGedcomPlace(file, 2, marriage.location, marriage.sourceLocation, options);
                        if (options.isAllElements)
                        {
                            // Nothing to do.  Done above.
                        }
                        else
                        {
                            marriage.sourceStart.gedcomAdd(familySources);
                            marriage.sourceLocation.gedcomAdd(familySources);
                        }
                    }

                    // Did the marriage end with a divorce.
                    if (marriage.terminatedIdx == 2)
                    {
                        if (marriage.end.isEmpty())
                        {
                            file.WriteLine("1 DIV Y");
                        }
                        else
                        {
                            file.WriteLine("1 DIV");
                            file.WriteLine("2 DATE " + marriage.end.format(DateFormat.GEDCOM));
                        }
                        if (options.isAllElements)
                        {
                            // oAlready = new ArrayList();
                            marriage.sourceEnd.writeGedcom(3, file, null);
                            marriage.sourceTerminated.writeGedcom(2, file, null);
                        }
                        else
                        {
                            marriage.sourceTerminated.gedcomAdd(familySources);
                            marriage.sourceEnd.gedcomAdd(familySources);
                        }
                    }

                    // Write the children for this family.
                    if (children != null)
                    {
                        foreach (Person child in children)
                        {
                            file.WriteLine("1 CHIL @I" + child.idx.ToString("0000") + "@");
                        }
                    }

                    // Write the sources for this family.
                    if (options.isAllElements)
                    {
                        marriage.sourcePartner.writeGedcom(1, file, null);
                    }
                    else
                    {
                        marriage.sourcePartner.gedcomAdd(familySources);
                        foreach (int sourceIdx in familySources)
                        {
                            file.WriteLine("1 SOUR @S" + sourceIdx.ToString("0000") + "@");
                        }
                    }

                    // Last Edit.
                    if (marriage.lastEditBy != "")
                    {
                        file.WriteLine("1 CHAN");
                        file.WriteLine("2 DATE " + marriage.lastEditDate.ToString("d MMM yyyy"));
                        file.WriteLine("3 TIME " + marriage.lastEditDate.ToString("HH:mm:ss"));
                        if (options.isIncludePGVU)
                        {
                            file.WriteLine("2 _PGVU " + marriage.lastEditBy);
                        }
                    }
                }
            }
            else
            {
                if (children != null)
                {
                    foreach (Person child in children)
                    {
                        file.WriteLine("1 CHIL @I" + child.idx.ToString("0000") + "@");
                    }
                }
            }
        }
    }
}
