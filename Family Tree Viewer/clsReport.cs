using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using FamilyTree.Objects;

namespace FamilyTree.Viewer
{
    public class clsReport
    {
        #region Member Variables

        /// <summary>ID of the person this report will be about.</summary>
        private int personIndex_;

        /// <summary>Database that contains the person and their relations.</summary>
        private Database database_;

        /// <summary>User options to use to build this report.</summary>
        private UserOptions userOptions_;

        #endregion

        #region Constructors etc ...



        /// <summary>Class constructor.  Specify the person this report is about, the database to gather information and the user options to use when building the document.</summary>
        /// <param name="personIndex">Specify the ID of the person to build the document about</param>
        /// <param name="database">Specify the database from which to gather the information</param>
        /// <param name="userOptions">Specify the user option to use when building the document</param>
        public clsReport(int personIndex, Database database, UserOptions userOptions)
        {
            // Record the construction parameters
            personIndex_ = personIndex;
            database_ = database;
            userOptions_ = userOptions;
        }



        #endregion

        #region Values and Building



        /// <summary>Adds the specified person to the specified collection of people.</summary>
        /// <remarks>The collection of people is keyed on the date of birth.  No two entries can have the same date of birth.  But we do not really care about the time of birth, so if the date of birth is already taken then simply use a slightly later time of birth.</remarks>
        /// <param name="person">Specifies the person to add to the collection of people</param>
        /// <param name="people">Specifies the collection of people to add to</param>
        private void addPerson(Person person, ref SortedList people)
        {
            // Add this person at their date of birth date specified.
            // Modify the time if the dates clash because we don't care about the time.
            DateTime theDate = person.dob.date;
            bool isTryAgain = true;
            while (isTryAgain)
            {
                try
                {
                    people.Add(theDate, person);
                    isTryAgain = false;
                }
                catch
                {
                    theDate = theDate.AddSeconds(1);
                }
            }
        }



        /// <summary>Adds the forebears of the specified person to the specified collection of people.</summary>
        /// <param name="person">Specifies the person to add the forebears of</param>
        /// <param name="people">Specifies the collection of people to add to</param>
        private void addForebears(Person person, ref SortedList people)
        {
            // Add the father to the list.
            if (person.fatherIndex != 0)
            {
                Person father = new Person(person.fatherIndex, database_);
                father.tag = person.tag + "F";
                addPerson(father, ref people);

                // Add the father's parents.
                addForebears(father, ref people);
            }

            // Add the mother to the list.
            if (person.motherIndex != 0)
            {
                Person mother = new Person(person.motherIndex, database_);
                mother.tag = person.tag + "M";
                addPerson(mother, ref people);

                // Add the mother's parents.
                addForebears(mother, ref people);
            }
        }



        /// <summary>Adds the descendants of the specified person to the specified collection of people.</summary>
        /// <param name="person">Specifies the person to add the descendants of</param>
        /// <param name="people">Specifies the collection of people to add to</param>
        private void addDescendants(Person person, ref SortedList people)
        {
            int[] children = person.getChildren();
            for (int i = 0; i < children.Length; i++)
            {
                Person child = new Person(children[i], database_);
                child.tag = person.tag + (i + 1).ToString() + ".";
                addPerson(child, ref people);

                // Add the child's children.
                addDescendants(child, ref people);
            }
        }



        /// <summary>Add the siblings of the specified person to the specified collection of people.</summary>
        /// <param name="person">Specifies the person to add the siblings of</param>
        /// <param name="people">Specifies the collection of people to add to</param>
        private void addSiblings(Person person, ref SortedList people)
        {
            int[] siblings = person.getSiblings();
            for (int i = 0; i < siblings.Length; i++)
            {
                Person child = new Person(siblings[i], database_);
                if (child.isMale)
                {
                    child.tag = "Brother";
                }
                else
                {
                    child.tag = "Sister";
                }
                addPerson(child, ref people);
            }
        }



        #endregion

        #region Display

        /// <summary>Displays MS Word and shows the collection of people related to the main person.</summary>
        /// <returns>True for success.  False otherwise.</returns>
        public string getReport()
        {
            // Create (an empty) list of people to display
            SortedList people = new SortedList();

            // Add the base person
            Person person = new Person(personIndex_, database_);
            people.Add(person.dob.date, person);

            // Add the parents
            addForebears(person, ref people);

            // Add the descendants
            addDescendants(person, ref people);

            // Add the siblings
            addSiblings(person, ref people);

            // Build html around the people found
            StringBuilder html = new StringBuilder();
            for (int i = 0; i < people.Count; i++)
            {
                person = (Person)people.GetByIndex(i);

                html.Append("<p>");
                if (person.tag != "")
                {
                    html.Append("(" + person.tag + ") ");
                }
                html.Append(person.getDescription(true, false, false, false, false));
                html.AppendLine("</p>");
            }

            // return success.
            return html.ToString();
        }



        #endregion
    }
}
