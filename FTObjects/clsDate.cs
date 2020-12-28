using System;

// StringBuilder
using System.Text;	

namespace FamilyTree.Objects
{
	#region Supporting Types etc ...

	/// <summary>Formats for human readable output of clsDate objects.</summary>
	public enum DateFormat
	{
		/// <summary>Only show the year.</summary>
		YearOnly,
		/// <summary>Show the full date with a short month eg. Jan.</summary>
		FullShort,
		/// <summary>Show the full date with a full month name eg January.</summary>
		FullLong,
		/// <summary>Show the date in a GedCom compatible way.</summary>
		Gedcom
	};	

	#endregion

	/// <summary>
	/// Class to represent a date in FTObjects.  This is like a .NET DateTime but it has the additional flag to show which
	/// components of the date are known.
	/// The class can also represent BC years unlike the .NET DateTime class.
	/// </summary>
	public class clsDate
	{
		#region Member Variables

        /// <summary>
        /// The type of automatic date prefixes.
        /// </summary>
        public enum enumPrefix
        {
            /// <summary>
            /// Use "on" unless "in", "before" or "after" is the information that we have.
            /// </summary>
            OnInBeforeAfter,

            /// <summary>
            /// Use "On" unless "In", "Before" or "After" is the information that we have.
            /// </summary>
            OnInBeforeAfterCaptials
        }

		/// <summary>Years above this are really BC years.</summary>
		public const int YEARBC = 8000;

		/// <summary>Year to count down from to store BC years.</summary>
		private const int BCBASE = 9800;
		
		/// <summary>Status to indicate that the day information in m_dtDate is only a guess.</summary>
        public const int GUESSDAY = 1;

		/// <summary>Status to indicate that the month information in m_dtDate is only a guess.</summary>
		public const int GUESSMONTH = 2;
		
		/// <summary>Status to indicate that the year information in m_dtDate is only a guess.</summary>
		public const int GUESSYEAR = 4;		

		/// <summary>Status to indicate that no information is stored in m_dtDate.  Not even a guess.</summary>
		private const int EMPTYFLAG = 8;
		
		/// <summary>Status to indicate that no information is stored in m_dtDate.</summary>
		public const int EMPTY = 15;

		/// <summary>Flag to indicate that actual date is before this date.</summary>
        public const int BEFORE = 16;

		/// <summary>Flag to indicate that the actual date is after this date.</summary>
		public const int AFTER = 32;

        /// <summary>Flags to indicate that the month component is only a quarter.</summary>
        public const int QUARTER = 64;

		/// <summary>The date and time value.  Not all may be valid or used.</summary>
		private DateTime m_dtDate;

		/// <summary>The components of m_dtDate that are valid and in use.</summary>
		private int m_nStatus;

		#endregion

		#region Constructors etc ...

		/// <summary>
		/// Empty class constructor.
		/// </summary>
		public clsDate()
		{		
			m_dtDate = DateTime.Now;
			m_nStatus = EMPTY;
		}
		/// <summary>
		/// Class constructor from a .NET DateTime value.
		/// </summary>
		/// <param name="dtTheDate">Specify the DateTime value to initialise the value of the clsDate.</param>
		public clsDate
			(
			DateTime	dtTheDate
			)
		{
			m_dtDate = dtTheDate;
			m_nStatus = 0;
		}

		#endregion

		#region Public Methods
		
		/// <summary>
		/// Returns true if the nothing is known about the date.  Not even a guess
		/// </summary>
		/// <returns>Returns true if nothing is known about the date.  Not even a guess.</returns>
		public bool IsEmpty()
		{
			if((m_nStatus & EMPTYFLAG)==EMPTYFLAG)
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// Formats the date for display using both the date and status information.
		/// </summary>
		/// <param name="nFormat">Specifies the style of the returned string.</param>
		/// <returns>A human readable string which represents the clsDate object.</returns>
		public string Format
			(
			DateFormat nFormat
			)
		{
			// Return unknown dates as an empty string
			if((m_nStatus & EMPTYFLAG)==EMPTYFLAG)
			{
				return "";			
			}

			StringBuilder sbDate = new StringBuilder();	
		
			// Is this the actual date or some information about the date
            if((m_nStatus & BEFORE) != 0)
            {
                if(nFormat == DateFormat.Gedcom)
                {
                    sbDate.Append("BEF ");
                }
                else
                {
                    sbDate.Append("<");
                }
            }
            else if((m_nStatus & AFTER) != 0)
            {
                if(nFormat == DateFormat.Gedcom)
                {
                    sbDate.Append("AFT ");
                }
                else
                {
                    sbDate.Append(">");
                }
            }

			if(nFormat==DateFormat.Gedcom)
			{
				if((m_nStatus & (GUESSDAY|GUESSMONTH|GUESSYEAR))!=0)
				{
					sbDate.Append("ABT ");
				}
			}

			// Display the day if it known and part of the style
			if(nFormat!=DateFormat.YearOnly)
			{
				if((m_nStatus & GUESSDAY)==0)
				{
					sbDate.Append(m_dtDate.Day.ToString());
					sbDate.Append(" ");					
				}
			}

            // Calculate the year
            int nYear = clsDate.GetYear(m_dtDate);
            string sYear;
            if(nYear > 0)
            {
                sYear = nYear.ToString();
            }
            else
            {
                sYear = (-nYear).ToString() + " BC";
            }

			// Display the month if it known and part of the style
			if((m_nStatus & GUESSMONTH)==0)
			{
                if((m_nStatus & QUARTER) == 0)
                {

                    switch(nFormat)
                    {
                    case DateFormat.FullLong:
                        sbDate.Append(m_dtDate.ToString("MMMM"));
                        sbDate.Append(" ");
                        break;
                    case DateFormat.FullShort:
                    case DateFormat.Gedcom:
                        sbDate.Append(m_dtDate.ToString("MMM"));
                        sbDate.Append(" ");
                        break;
                    }
                }
                else
                {
                    switch(nFormat)
                    {
                    case DateFormat.FullLong:
                        switch(m_dtDate.Month)
                        {
                        case 1:
                        case 2:
                        case 3:
                            sbDate.Append("Jan-Mar ");
                            break;
                        case 4:
                        case 5:
                        case 6:
                            sbDate.Append("Apr-Jun ");
                            break;
                        case 7:
                        case 8:
                        case 9:
                            sbDate.Append("Jul-Sep ");
                            break;
                        case 10:
                        case 11:
                        case 12:
                            sbDate.Append("Oct-Dec ");
                            break;
                        }
                        break;

                    case DateFormat.FullShort:
                        sbDate.Append("Q");
                        sbDate.Append((1+(m_dtDate.Month-1)/3).ToString());
                        sbDate.Append(" ");
                        break;

                    case DateFormat.Gedcom:
                        // BET JAN 1852 AND MAR 1852
                        sbDate.Append("BET ");
                        switch(m_dtDate.Month)
                        {
                        case 1:
                        case 2:
                        case 3:
                            sbDate.Append("JAN " + sYear + " AND MAR ");
                            break;
                        case 4:
                        case 5:
                        case 6:
                            sbDate.Append("APR " + sYear + " AND JUN ");
                            break;
                        case 7:
                        case 8:
                        case 9:
                            sbDate.Append("JUL " + sYear + " AND SEP ");
                            break;
                        case 10:
                        case 11:
                        case 12:
                            sbDate.Append("OCT " + sYear + " AND DEC ");
                            break;
                        }
                        break;

                    }
                }
			}

            // Display the year if is known and part of the style
            if((m_nStatus & GUESSYEAR) == GUESSYEAR)
			{
				if(nFormat==DateFormat.Gedcom)
				{
					// The about is already specified
					sbDate.Append(sYear);
				}
				else
				{
					sbDate.Append("c" + sYear);
				}
			}
			else
			{
				sbDate.Append(sYear);
			}

			// Return the string built
			return sbDate.ToString();
		}
		/// <summary>
		/// Formats the date for display using both the date and status information.
		/// A prefix is added to non empty clsDate values.
		/// </summary>
		/// <param name="nFormat">Specifies the style of the output string.</param>
		/// <param name="sPrefix">Specifies a prefix for non empty values.</param>
        /// <returns>A human readable representation of the clsDate value plus a prefix.</returns>
        public string Format
			(
			DateFormat nFormat,
			string sPrefix
			)
		{
            // Return unknown dates as an empty string
            if((m_nStatus & EMPTYFLAG) == EMPTYFLAG)
            {
                return "";
            }

            return sPrefix + Format(nFormat);
		}
        /// <summary>
        /// Formats the date for display using both the date and status information.
        /// A prefix is calculated from the information in the date and the options specified.
        /// </summary>
        /// <param name="nFormat">Specifies the style of the output date.</param>
        /// <param name="Prefix">Specifies the options for the prefix.</param>
        /// <returns>A human readable representation of the clsDate value plus a prefix.</returns>
        public string Format
            (
            DateFormat nFormat,
            enumPrefix Prefix
            )
        {
            // Return unknown dates as an empty string
            if((m_nStatus & EMPTYFLAG) == EMPTYFLAG)
            {
                return "";
            }

            string sPrefix = "on ";
            switch(Prefix)
            {
            case enumPrefix.OnInBeforeAfter:
                if((m_nStatus & BEFORE) == BEFORE)
                {
                    sPrefix = "before ";
                }
                else if((m_nStatus & AFTER) == AFTER)
                {
                    sPrefix = "after ";
                }
                else if((m_nStatus & (GUESSDAY|GUESSMONTH|GUESSYEAR))!=0)
                {
                    sPrefix="in ";
                }
                break;

            case enumPrefix.OnInBeforeAfterCaptials:
                sPrefix = "On ";
                if((m_nStatus & BEFORE) == BEFORE)
                {
                    sPrefix = "Before ";
                }
                else if((m_nStatus & AFTER) == AFTER)
                {
                    sPrefix = "After ";
                }
                else if((m_nStatus & (GUESSDAY|GUESSMONTH|GUESSYEAR))!=0)
                {
                    sPrefix="In ";
                }
                break;
            }

            // Remove the BEFORE and ATFER from the output
            int nStatus = m_nStatus;
            m_nStatus = m_nStatus & ~(AFTER|BEFORE);
            string sReturn = sPrefix + Format(nFormat);
            m_nStatus = nStatus;

            // Return the result
            return sReturn;
        }

		#endregion

		#region BC Years

		// *************************************************************************************************************
		/// <summary>
		/// Returns the actual year from the specified DateTime year.  Years above the threshold are actually BC years.
		/// </summary>
		/// <param name="nYear">Specifies the DateTime year.</param>
		/// <returns>The actual year that the DateTime year represents.</returns>
		public static int GetYear
			(
			int nYear
			)
		{
			// Check for a year above the threshold
			if(nYear>YEARBC)
			{
				nYear = nYear - BCBASE;
			}

			// Return the year calculated
			return nYear;
		}
		/// <summary>
		/// Returns the year from the specified date.  Allows for BC years using a upper threshold.
		/// </summary>
		/// <param name="dtDate">Specifies the date to return the year of.</param>
		/// <returns>The year inside the specified date object.</returns>
		public static int GetYear
			(
			DateTime dtDate
			)
		{
			return GetYear(dtDate.Year);
		}

		// *******************************************************************************************************************
		/// <summary>
		/// Returns the year to store in a DateTime object to represent the specified year.  These are mostly the same value
		/// but for negative years (BC) years above the threshold are used.
		/// </summary>
		/// <param name="nYear">Specifies the actual year to convert into a DateTime year.</param>
		/// <returns>The year to store in a DateTime object.</returns>
		public static int SetYear
			(
			int nYear
			)
		{
			if(nYear>100)
			{
				return nYear;
			}
			return BCBASE+nYear;
		}

		#endregion

		#region Properties

		/// <summary>The actual date value stored.  Not all of the date may be valid see Status.</summary>
		public DateTime Date { get { return m_dtDate; } set { m_dtDate = value; } }

		/// <summary>The components of the date that are actually valid.</summary>
		public int Status { get { return m_nStatus; } set { m_nStatus = value; } }

		/// <summary>
		/// Return true if this is an equal to date.
		/// This is the expected value.
		/// </summary>
		public bool IsEqualTo
		{
			get
			{
				if((m_nStatus & (BEFORE|AFTER))==0)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}

		/// <summary>
		/// True if the actual date is before this date but unknown.
		/// </summary>
		public bool IsBefore
		{
			get
			{
				if((m_nStatus & BEFORE)==0)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
		}

		/// <summary>
		/// True if the acutal date is after this date.
		/// </summary>
		public bool IsAfter
		{
			get
			{
				if((m_nStatus & AFTER)==0)
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
        public bool IsQuarterOnly
        {
            get
            {
                if((m_nStatus & QUARTER) == 0)
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
