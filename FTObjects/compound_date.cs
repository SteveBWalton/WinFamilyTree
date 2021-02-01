using System;

// StringBuilder
using System.Text;

namespace family_tree.objects
{
    #region Supporting Types etc ...

    /// <summary>Formats for human readable output of CompoundDate objects.</summary>
    public enum DateFormat
    {
        /// <summary>Only show the year.</summary>
        YEAR_ONLY,
        /// <summary>Show the full date with a short month eg. Jan.</summary>
        FULL_SHORT,
        /// <summary>Show the full date with a full month name eg January.</summary>
        FULL_LONG,
        /// <summary>Show the date in a GedCom compatible way.</summary>
        GEDCOM
    };

    #endregion

    /// <summary>Class to represent a date in FTObjects.  This is like a .NET DateTime but it has the additional flag to show which components of the date are known.  The class can also represent BC years unlike the .NET DateTime class.</summary>
    public class CompoundDate
    {
        #region Member Variables

        /// <summary>The type of automatic date prefixes.</summary>
        public enum DatePrefix
        {
            /// <summary>Use "on" unless "in", "before" or "after" is the information that we have.</summary>
            ON_IN_BEFORE_AFTER,

            /// <summary>Use "On" unless "In", "Before" or "After" is the information that we have.</summary>
            ON_IN_BEFORE_AFTER_CAPTIALS
        }

        /// <summary>Years above this are really BC years.</summary>
        public const int YEARBC = 8000;

        /// <summary>Year to count down from to store BC years.</summary>
        private const int BCBASE = 9800;

        /// <summary>Status to indicate that the day information in theDate_ is only a guess.</summary>
        public const int GUESSDAY = 1;

        /// <summary>Status to indicate that the month information in theDate_ is only a guess.</summary>
        public const int GUESSMONTH = 2;

        /// <summary>Status to indicate that the year information in theDate_ is only a guess.</summary>
        public const int GUESSYEAR = 4;

        /// <summary>Status to indicate that no information is stored in theDate_.  Not even a guess.</summary>
        private const int EMPTYFLAG = 8;

        /// <summary>Status to indicate that no information is stored in theDate_.</summary>
        public const int EMPTY = 15;

        /// <summary>Flag to indicate that actual date is before this date.</summary>
        public const int BEFORE = 16;

        /// <summary>Flag to indicate that the actual date is after this date.</summary>
        public const int AFTER = 32;

        /// <summary>Flags to indicate that the month component is only a quarter.</summary>
        public const int QUARTER = 64;

        /// <summary>The date and time value.  Not all may be valid or used.</summary>
        private DateTime theDate_;

        /// <summary>The components of theDate_ that are valid and in use.</summary>
        private int status_;

        #endregion

        #region Constructors etc ...



        /// <summary>Empty class constructor.</summary>
        public CompoundDate()
        {
            theDate_ = DateTime.Now;
            status_ = EMPTY;
        }



        /// <summary>Class constructor from a .NET DateTime value.</summary>
        /// <param name="theDate">Specify the DateTime value to initialise the value of the CompoundDate.</param>
        public CompoundDate(DateTime theDate)
        {
            theDate_ = theDate;
            status_ = 0;
        }



        #endregion

        #region Public Methods



        /// <summary>Returns true if the nothing is known about the date.  Not even a guess.</summary>
        /// <returns>Returns true if nothing is known about the date.  Not even a guess.</returns>
        public bool isEmpty()
        {
            if ((status_ & EMPTYFLAG) == EMPTYFLAG)
            {
                return true;
            }
            return false;
        }



        /// <summary>Formats the date for display using both the date and status information.</summary>
        /// <param name="dateFormat">Specifies the style of the returned string.</param>
        /// <returns>A human readable string which represents the CompoundDate object.</returns>
        public string format
            (
            DateFormat dateFormat
            )
        {
            // Return unknown dates as an empty string.
            if ((status_ & EMPTYFLAG) == EMPTYFLAG)
            {
                return "";
            }

            StringBuilder formatDate = new StringBuilder();

            // Is this the actual date or some information about the date
            if ((status_ & BEFORE) != 0)
            {
                if (dateFormat == DateFormat.GEDCOM)
                {
                    formatDate.Append("BEF ");
                }
                else
                {
                    formatDate.Append("<");
                }
            }
            else if ((status_ & AFTER) != 0)
            {
                if (dateFormat == DateFormat.GEDCOM)
                {
                    formatDate.Append("AFT ");
                }
                else
                {
                    formatDate.Append(">");
                }
            }

            if (dateFormat == DateFormat.GEDCOM)
            {
                if ((status_ & (GUESSDAY | GUESSMONTH | GUESSYEAR)) != 0)
                {
                    formatDate.Append("ABT ");
                }
            }

            // Display the day if it known and part of the style.
            if (dateFormat != DateFormat.YEAR_ONLY)
            {
                if ((status_ & GUESSDAY) == 0)
                {
                    formatDate.Append(theDate_.Day.ToString());
                    formatDate.Append(" ");
                }
            }

            // Calculate the year.
            int theYear = CompoundDate.getYear(theDate_);
            string formatYear;
            if (theYear > 0)
            {
                formatYear = theYear.ToString();
            }
            else
            {
                formatYear = (-theYear).ToString() + " BC";
            }

            // Display the month if it known and part of the style
            if ((status_ & GUESSMONTH) == 0)
            {
                if ((status_ & QUARTER) == 0)
                {

                    switch (dateFormat)
                    {
                    case DateFormat.FULL_LONG:
                        formatDate.Append(theDate_.ToString("MMMM"));
                        formatDate.Append(" ");
                        break;
                    case DateFormat.FULL_SHORT:
                    case DateFormat.GEDCOM:
                        formatDate.Append(theDate_.ToString("MMM"));
                        formatDate.Append(" ");
                        break;
                    }
                }
                else
                {
                    switch (dateFormat)
                    {
                    case DateFormat.FULL_LONG:
                        switch (theDate_.Month)
                        {
                        case 1:
                        case 2:
                        case 3:
                            formatDate.Append("Jan-Mar ");
                            break;
                        case 4:
                        case 5:
                        case 6:
                            formatDate.Append("Apr-Jun ");
                            break;
                        case 7:
                        case 8:
                        case 9:
                            formatDate.Append("Jul-Sep ");
                            break;
                        case 10:
                        case 11:
                        case 12:
                            formatDate.Append("Oct-Dec ");
                            break;
                        }
                        break;

                    case DateFormat.FULL_SHORT:
                        formatDate.Append("Q");
                        formatDate.Append((1 + (theDate_.Month - 1) / 3).ToString());
                        formatDate.Append(" ");
                        break;

                    case DateFormat.GEDCOM:
                        // BET JAN 1852 AND MAR 1852
                        formatDate.Append("BET ");
                        switch (theDate_.Month)
                        {
                        case 1:
                        case 2:
                        case 3:
                            formatDate.Append("JAN " + formatYear + " AND MAR ");
                            break;
                        case 4:
                        case 5:
                        case 6:
                            formatDate.Append("APR " + formatYear + " AND JUN ");
                            break;
                        case 7:
                        case 8:
                        case 9:
                            formatDate.Append("JUL " + formatYear + " AND SEP ");
                            break;
                        case 10:
                        case 11:
                        case 12:
                            formatDate.Append("OCT " + formatYear + " AND DEC ");
                            break;
                        }
                        break;
                    }
                }
            }

            // Display the year if is known and part of the style
            if ((status_ & GUESSYEAR) == GUESSYEAR)
            {
                if (dateFormat == DateFormat.GEDCOM)
                {
                    // The about is already specified
                    formatDate.Append(formatYear);
                }
                else
                {
                    formatDate.Append("c" + formatYear);
                }
            }
            else
            {
                formatDate.Append(formatYear);
            }

            // Return the string built.
            return formatDate.ToString();
        }



        /// <summary>Formats the date for display using both the date and status information.  A prefix is added to non empty CompoundDate values.</summary>
        /// <param name="dateFormat">Specifies the style of the output string.</param>
        /// <param name="prefix">Specifies a prefix for non empty values.</param>
        /// <returns>A human readable representation of the CompoundDate value plus a prefix.</returns>
        public string format(DateFormat dateFormat, string prefix)
        {
            // Return unknown dates as an empty string.
            if ((status_ & EMPTYFLAG) == EMPTYFLAG)
            {
                return "";
            }

            return prefix + format(dateFormat);
        }



        /// <summary>Formats the date for display using both the date and status information.  A prefix is calculated from the information in the date and the options specified.</summary>
        /// <param name="dateFormat">Specifies the style of the output date.</param>
        /// <param name="datePrefix">Specifies the options for the prefix.</param>
        /// <returns>A human readable representation of the CompoundDate value plus a prefix.</returns>
        public string format(DateFormat dateFormat, DatePrefix datePrefix)
        {
            // Return unknown dates as an empty string.
            if ((status_ & EMPTYFLAG) == EMPTYFLAG)
            {
                return "";
            }

            string prefix = "on ";
            switch (datePrefix)
            {
            case DatePrefix.ON_IN_BEFORE_AFTER:
                if ((status_ & BEFORE) == BEFORE)
                {
                    prefix = "before ";
                }
                else if ((status_ & AFTER) == AFTER)
                {
                    prefix = "after ";
                }
                else if ((status_ & (GUESSDAY | GUESSMONTH | GUESSYEAR)) != 0)
                {
                    prefix = "in ";
                }
                break;

            case DatePrefix.ON_IN_BEFORE_AFTER_CAPTIALS:
                prefix = "On ";
                if ((status_ & BEFORE) == BEFORE)
                {
                    prefix = "Before ";
                }
                else if ((status_ & AFTER) == AFTER)
                {
                    prefix = "After ";
                }
                else if ((status_ & (GUESSDAY | GUESSMONTH | GUESSYEAR)) != 0)
                {
                    prefix = "In ";
                }
                break;
            }

            // Remove the BEFORE and ATFER from the output.
            int status = status_;
            status_ = status_ & ~(AFTER | BEFORE);
            string result = prefix + format(dateFormat);
            status_ = status;

            // Return the result.
            return result;
        }



        #endregion

        #region BC Years



        /// <summary>Returns the actual year from the specified DateTime year.  Years above the threshold are actually BC years.</summary>
        /// <param name="theYear">Specifies the DateTime year.</param>
        /// <returns>The actual year that the DateTime year represents.</returns>
        public static int getYear(int theYear)
        {
            // Check for a year above the threshold.
            if (theYear > YEARBC)
            {
                theYear = theYear - BCBASE;
            }

            // Return the year calculated.
            return theYear;
        }



        /// <summary>Returns the year from the specified date.  Allows for BC years using a upper threshold.</summary>
        /// <param name="theDate">Specifies the date to return the year of.</param>
        /// <returns>The year inside the specified date object.</returns>
        public static int getYear(DateTime theDate)
        {
            return getYear(theDate.Year);
        }



        /// <summary>Returns the year to store in a DateTime object to represent the specified year.  These are mostly the same value  but for negative years (BC) years above the threshold are used.</summary>
        /// <param name="theYear">Specifies the actual year to convert into a DateTime year.</param>
        /// <returns>The year to store in a DateTime object.</returns>
        public static int setYear(int theYear)
        {
            if (theYear > 100)
            {
                return theYear;
            }
            return BCBASE + theYear;
        }



        #endregion

        #region Properties

        /// <summary>The actual date value stored.  Not all of the date may be valid see Status.</summary>
        public DateTime date { get { return theDate_; } set { theDate_ = value; } }

        /// <summary>The components of the date that are actually valid.</summary>
        public int status { get { return status_; } set { status_ = value; } }



        /// <summary>Return true if this is an equal to date.  This is the expected value.</summary>
        public bool isEqualTo
        {
            get
            {
                if ((status_ & (BEFORE | AFTER)) == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }



        /// <summary>True if the actual date is before this date but unknown.</summary>
        public bool isBefore
        {
            get
            {
                if ((status_ & BEFORE) == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }



        /// <summary>True if the acutal date is after this date.</summary>
        public bool isAfter
        {
            get
            {
                if ((status_ & AFTER) == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }



        /// <summary>True if the date only contains a quarter for the month.</summary>
        public bool isQuarterOnly
        {
            get
            {
                if ((status_ & QUARTER) == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }



        #endregion

    }
}
