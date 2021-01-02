using System;
using System.Collections;
using System.IO;

namespace FamilyTree.Objects
{
    /// <summary>Class to represent a GEDCOM family.  These are like clsRelationship objects but more frequent.</summary>
    public class Family
    {
        /// <summary>ID of the father in the family. 0 or negative indicates no father.</summary>
        public int fatherIndex;

        /// <summary>ID of the mother in the family.  0 or negative indicates no mother.</summary>
        public int motherIndex;

        /// <summary>Unique index for the gedcom file.</summary>
        public int gedcomIndex;

        /// <summary>ID of the relationship object between the mother and father.  0 indicates no relationship object.</summary>
        public int relationshipIndex;

        /// <summary>Collection of the children in this family.</summary>
        private ArrayList children;



        /// <summary>Empty class constructor.</summary>
        public Family()
        {
            fatherIndex = 0;
            motherIndex = 0;
            gedcomIndex = 0;
            relationshipIndex = 0;
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
                int index = children.Count - 1;
                Person insert = (Person)children[index];
                if (child.dob.date > insert.dob.date)
                {
                    // Add the child to the end of the list.
                    children.Add(child);
                }
                else
                {
                    index = 0;
                    insert = (Person)children[index];
                    if (child.dob.date < insert.dob.date)
                    {
                        // Add the child to the start of the list.
                        children.Insert(index, child);
                    }
                    else
                    {
                        // Find the existing child to add this child in front of.
                        while (child.dob.date > insert.dob.date)
                        {
                            index++;
                            insert = (Person)children[index];
                        }
                        children.Insert(index, child);
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

            file.WriteLine("0 @F" + gedcomIndex.ToString("0000") + "@ FAM");
            if (motherIndex > 0)
            {
                file.WriteLine("1 WIFE @I" + motherIndex.ToString("0000") + "@");
            }
            if (fatherIndex > 0)
            {
                file.WriteLine("1 HUSB @I" + fatherIndex.ToString("0000") + "@");
            }

            if (motherIndex > 0 && fatherIndex > 0)
            {
                Relationship marriage = database.getRelationship(fatherIndex, motherIndex);
                if (marriage != null)
                {
                    marriage.setDatabase(database);

                    if (marriage.isMarried())
                    {
                        file.WriteLine("1 MARR");
                        switch (marriage.typeIndex)
                        {
                        case 1:
                            file.WriteLine("2 TYPE Religious");
                            break;
                        case 2:
                            file.WriteLine("2 TYPE Civil");
                            break;
                        }

                        if (!marriage.start.isEmpty())
                        {
                            file.WriteLine("2 DATE " + marriage.start.format(DateFormat.GEDCOM));
                        }
                        database.writeGedcomPlace(file, 2, marriage.location, options);

                        // ArrayList oAlready = new ArrayList();
                        //oMarriage.SourceStart.GedcomWrite(2,oFile,oAlready);
                        //oMarriage.SourceLocation.GedcomWrite(2,oFile,oAlready);
                        marriage.sourceStart.gedcomAdd(familySources);
                        marriage.sourceLocation.gedcomAdd(familySources);

                        // Did the marriage end with a divorce.
                        if (marriage.terminatedIndex == 2)
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
                            // oAlready = new ArrayList();
                            // oMarriage.SourceTerminated.GedcomWrite(2,oFile,oAlready);
                            // oMarriage.SourceEnd.GedcomWrite(2,oFile,oAlready);
                            marriage.sourceTerminated.gedcomAdd(familySources);
                            marriage.sourceEnd.gedcomAdd(familySources);
                        }
                    }

                    // Write the sources for this family.
                    foreach (int sourceIndex in familySources)
                    {
                        file.WriteLine("1 SOUR @S" + sourceIndex.ToString("0000") + "@");
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

            if (children != null)
            {
                foreach (Person child in children)
                {
                    file.WriteLine("1 CHIL @I" + child.index.ToString("0000") + "@");
                }
            }
        }
    }
}
