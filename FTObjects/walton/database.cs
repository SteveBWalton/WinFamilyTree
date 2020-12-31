using System;

// Database.
using System.Data;

// Access databases.
using System.Data.OleDb;

// SqlServer databases.
using System.Data.SqlClient;

#if __WALTON_ORACLE__
// For ORACLE support add __WALTON_ORACLE__ to "Conditional complation symbols" on the "Build" Tab on the project properties.
// This is not a standard .NET library.
using System.Data.OracleClient; 
#endif

#if __WALTON_MYSQL__
// For MySQL support add __WALTON_MYSQL__ to "Conditional complation symbols" on the "Build" Tab on the project properties.
// This requires a reference to MySql.Data in the project references
// This requires that MySql.Data.dll is installed with the program.
// This is not a standard .NET library.
using MySql.Data.MySqlClient;
#endif

#if __WALTON_SQLITE__
// For SQLite support add __WALTON_SQLITE__ to "Conditional complation symbols" on the "Build" Tab on the project properties.
// This is not a standard .NET library.
using System.Data.SQLite;
#endif

// StringBuilder
using System.Text;

namespace walton
{
    /// <summary>
    /// Support class for database work.
    /// Conditionally supports MySQL, ORACLE and SQLITE.
    /// For MySQL support add __WALTON_MYSQL__ to "Conditional complation symbols" on the "Build" Tab on the project properties.
    /// For ORACLE support add __WALTON_ORACLE__ to "Conditional complation symbols" on the "Build" Tab on the project properties.
    /// For SQLite support add __WALTON_SQLITE__ to "Conditional complation symbols" on the "Build" Tab on the project properties. 
    /// Make sure that the symbols are applied to all configurations.
    /// </summary>
    public class Database
    {
        #region Supporting Types

        /// <summary>The type of databases the class supports.</summary>
        public enum enumDatabases
        {
            /// <summary>Microsoft Access</summary>
            Access,

            /// <summary>Microsoft Sql Server</summary>
            SqlServer,

            #if __INNOVAL_ORACLE__
            /// <summary>Oracle.  Not full supported unless System.Data.Oracle is in the project references.</summary>
            Oracle,
            #endif

            #if __INNOVAL_MYSQL__
            /// <summary>MySQL.  Not fully supported unless MySql.Data is in the project references.</summary>
            MySql,
            #endif

            #if __INNOVAL_SQLITE__
            /// <summary>SQLite.  Not fully supported unless System.Data.SQLite is in the project references.</summary>
            SQLite,
            #endif
        }

        #endregion

        #region Class constructor


        /// <summary>Class constructor.  This class is full of static members, don't really expect to have to create an instance object.</summary>
        public Database()
        {
        }


        #endregion

        #region Sql Server Connection


        /// <summary>Type to hold the information required for an SQL server connection.</summary>
        public struct typSqlConnection
        {
            /// <summary>Connection to the SQL server database.</summary>
            public SqlConnection db;

            /// <summary>Name of the SQL server.</summary>
            public string serverName;

            /// <summary>Name of the database on the server.</summary>
            public string databaseName;

            /// <summary>Use Windows Authentication otherwise use SQL server authentication.</summary>
            public bool isWindowsAuthentication;

            /// <summary>Login name for SQL server authenticatton.</summary>
            public string login;

            /// <summary>Password for SQL server authentication.</summary>
            public string password;
        }


        /// <summary>Returns the connection string for the specified SQL server connection.</summary>
        /// <param name="sqlServer">Specifies the options for the SQL server connection.</param>
        /// <returns>Connection string for the SQL server database.</returns>
        static private string sqlConnectionString(typSqlConnection sqlServer)
        {
            // Initialise the string.
            StringBuilder sbResult = new StringBuilder();

            // Security Informaton.
            if(sqlServer.isWindowsAuthentication)
            {
                sbResult.Append("Persist Security Info=False;Integrated Security=SSPI;");
            }
            else
            {
                sbResult.Append("Persist Security Info=False;Integrated Security=false;");
                sbResult.Append("User ID=" + sqlServer.login + ";");
                sbResult.Append("Password=" + sqlServer.password + ";");
            }

            // Database.
            sbResult.Append("Database=" + sqlServer.databaseName + ";");

            // Server.
            sbResult.Append("Server=" + sqlServer.serverName + ";");

            // Timeout.
            sbResult.Append("Connect Timeout=120;");

            // Return the connection string.
            return sbResult.ToString();
        }


        /// <summary>Returns the connection settings from the configuration file node.</summary>
        /// <param name="xmlNode">Specifies the configuration node to load the settings from.</param>
        /// <param name="serverName">Specifies the default server name.</param>
        /// <param name="databaseName">Specifies the default database name.</param>
        /// <param name="isWindowsAuth">Specifies the default authenication mode.</param>
        /// <param name="login">Specifies the default login name for Sql server authenication.</param>
        /// <param name="password">Specifies the default password for Sql server authenication.</param>
        /// <returns>The settings to connect to a Sql server database.</returns>
        static public typSqlConnection sqlGetConnectionSettings(XmlNode xmlNode, string serverName, string databaseName, bool isWindowsAuth, string login, string password)
        {
            typSqlConnection sqlServer = new typSqlConnection();
            sqlServer.serverName = xmlNode.getAttributeValue("server", serverName, true);
            sqlServer.databaseName = xmlNode.getAttributeValue("database", databaseName, true);
            sqlServer.isWindowsAuthentication = xmlNode.getAttributeValue("windowsauthentication", isWindowsAuth, true);
            if(!sqlServer.isWindowsAuthentication)
            {
                sqlServer.login = xmlNode.getAttributeValue("login", login, true);
                sqlServer.password = xmlNode.getAttributePassword("password", password);
            }

            // Return the settings as loaded.
            return sqlServer;
        }


        /// <summary>Returns a connection to SqlServer database from the settings in a configuration file.  The connection must be opened (and closed) by the calling program.</summary>
        /// <param name="sqlServer">Specifies the Sql server settings to use to open the connection.</param>
        /// <returns>A connection to a SqlServer database.</returns>
        static public SqlConnection sqlOpen(typSqlConnection sqlServer)
        {
            return new SqlConnection(sqlConnectionString(sqlServer));
        }


        #endregion

        #region ToDb



        /// <summary>Format the specified string ready for insertion into Sql commands.  A empty string is returned as NULL, otherwise the string is enclosed in single quotes.</summary>
        /// <param name="value">Specifies the string value to insert into a Sql command.</param>
        /// <returns>The specified value in Sql format.</returns>
        public static string toDb(string value)
        {
            if (value == null)
            {
                return "NULL";
            }
            if (value == "")
            {
                return "NULL";
            }

            // Remove any single quotes.
            value = value.Replace("'", "''");

            // Return the new string.
            return "'" + value + "'";
        }



        /// <summary>Format the specified string ready for insertion into Sql commands.  A empty string is returned as the specified null value, otherwise the string is enclosed in single quotes.</summary>
        /// <param name="value">Specifies the string value to insert into a Sql command.</param>
        /// <param name="nullValue">The value to return in the case of null or empty string.  Quotes are not added to this value.</param>
        /// <returns>The specified value in Sql format.</returns>
        public static string toDb(string value, string nullValue)
        {
            if (value == null)
            {
                return nullValue;
            }
            if (value == "")
            {
                return nullValue;
            }

            // Remove any single quotes.
            value = value.Replace("'", "''");

            // Return the new string.
            return "'" + value + "'";
        }



        /// <summary>Formats the specified date for insertion into a Sql command.</summary>
        /// <param name="theDateTime">Specifies the date to insert into a Sql command.</param>
        /// <param name="databaseType">Specifies the type of database for the style of the date.</param>
        /// <returns></returns>
        public static string toDb(DateTime theDateTime, enumDatabases databaseType)
        {
            switch (databaseType)
            {
#if __INNOVAL_MYSQL__
            case enumDatabases.MySql :
                return "'" + dtTheDateTime.ToString("yyyy-MM-dd HH:mm:ss") + "'";
#endif
            case enumDatabases.Access:
            default:
                // This does not work in German !
                // return "#"+dtTheDateTime.ToString("d-MMM-yyyy HH:mm:ss")+"#";

                // Use the American date format
                // The forward slash character "/" did not work in German either.
                return "#" + theDateTime.ToString("MM-dd-yyyy HH:mm:ss") + "#";
            }
        }



        /// <summary>Returns a string that represents the specified integer ready for insertion into a Sql command.  Returns "NULL" if the specified integer matches the specified null value.</summary>
        /// <param name="value">Specifies the value of the integer to return.</param>
        /// <param name="nullValue">Specifies the value that represents null.</param>
        /// <returns>A string to represent the specified integer in a Sql command.</returns>
        public static string toDb(int value, int nullValue)
        {
            if (value == nullValue)
            {
                return "NULL";
            }
            return value.ToString();
        }



        /// <summary>Returns a string that represents the specified float ready for insertion into a sql command.  Returns "NULL"  if the specified float matches the specified null value.</summary>
        /// <param name="value">Specifies the value of the float to return.</param>
        /// <param name="nullValue">Specifies the value that represents null.</param>
        /// <returns>A string to represent the specified float in a sql command.</returns>
        public static string toDb(float value, float nullValue)
        {
            if (value == nullValue)
            {
                return "NULL";
            }
            return value.ToString();
        }



        /// <summary>Returns a string that represents the specified boolean ready for insertion into a sql command.  Returns "1" for true and "0" for false.</summary>
        /// <param name="isValue">Specifies the boolean to return.</param>
        /// <returns>"1" for true and "0" for false.</returns>
        public static string toDb(bool isValue)
        {
            if (isValue)
            {
                return "1";
            }
            return "0";
        }



        #endregion

        #region DataReader Support (Handle Nulls) etc ...

        #region Gets


        /// <summary>Returns a string from a OleDbDataReader handling the null posibility.</summary>
        /// <param name="dataReader">Specifies the datareader.</param>
        /// <param name="ordinal">Specifies the name of the column.</param>
        /// <param name="nullValue">Specifies the value to return if the database value is null.</param>
        /// <returns></returns>
        public static string getString(OleDbDataReader dataReader, string ordinal, string nullValue)
        {
            int ord = dataReader.GetOrdinal(ordinal);
            if(dataReader.IsDBNull(ord))
            {
                return nullValue;
            }
            else
            {
                return dataReader.GetString(ord).Trim();
            }
        }



        /// <summary>Returns a string from a SqlDataReader handling the null posibility.</summary>
        /// <param name="dataReader">Specifies the datareader.</param>
        /// <param name="ordinal">Specifies the name of the column.</param>
        /// <param name="nullValue">Specifies the value to return if the database value is null.</param>
        /// <returns></returns>
        public static string getString(SqlDataReader dataReader, string ordinal, string nullValue)
        {
            int ord = dataReader.GetOrdinal(ordinal);
            if(dataReader.IsDBNull(ord))
            {
                return nullValue;
            }
            else
            {
                return dataReader.GetString(ord).Trim();
            }
        }



        /// <summary>Returns an int from a OleDB datareader handling the null posibility and the slightly different database forms of integer values.</summary>
        /// <param name="dataReader">Specifies the datareader.</param>
        /// <param name="ordinal">Specifies the name of the column.</param>
        /// <param name="nullValue">Specifies the value to return if the database value is null.</param>
        /// <returns></returns>
        public static int getInt(OleDbDataReader dataReader, string ordinal, int nullValue)
        {
            int ord = dataReader.GetOrdinal(ordinal);
            if (dataReader.IsDBNull(ord))
            {
                return nullValue;
            }
            else
            {
                switch (dataReader.GetFieldType(ord).ToString())
                {
                case "System.Int16":
                    return dataReader.GetInt16(ord);
                case "System.Int32":
                    return dataReader.GetInt32(ord);
                case "System.Int64":
                    return (int)dataReader.GetInt64(ord);

                case "System.Decimal":
                    return (int)dataReader.GetDecimal(ord);

                case "System.Double":
                    return (int)dataReader.GetDouble(ord);

                case "System.Boolean":
                    if (dataReader.GetBoolean(ord))
                    {
                        return 1;
                    }
                    return 0;

                case "System.Byte":
                    return (int)dataReader.GetByte(ord);

                default:
                    return nullValue;
                }
            }
        }



        /// <summary>Returns an int from a SQL server datareader handling the null posibility and the slightly different database forms of integer values.</summary>
        /// <param name="dataReader">Specifies the datareader.</param>
        /// <param name="ordinal">Specifies the name of the column.</param>
        /// <param name="nullValue">Specifies the value to return if the database value is null.</param>
        /// <returns></returns>
        public static int getInt(SqlDataReader dataReader, string ordinal, int nullValue)
        {
            int ord = dataReader.GetOrdinal(ordinal);
            if (dataReader.IsDBNull(ord))
            {
                return nullValue;
            }
            else
            {
                switch (dataReader.GetFieldType(ord).ToString())
                {
                case "System.Int16":
                    return dataReader.GetInt16(ord);
                case "System.Int32":
                    return dataReader.GetInt32(ord);
                case "System.Int64":
                    return (int)dataReader.GetInt64(ord);

                case "System.Decimal":
                    return (int)dataReader.GetDecimal(ord);

                case "System.Double":
                    return (int)dataReader.GetDouble(ord);

                default:
                    return nullValue;
                }
            }
        }



#if __INNOVAL_MYSQL__
        /// Returns an int from a MySQL datareader handling the null posibility and the slightly different database forms of integer values.
        /// <summary>
        /// Returns an int from a MySQL datareader handling the null posibility and the slightly different database forms of integer values.
        /// </summary>
        /// <param name="drSource">Specifies the datareader.</param>
        /// <param name="sOrdinal">Specifies the name of the column.</param>
        /// <param name="nNull">Specifies the value to return if the database value is null.</param>
        /// <returns></returns>
        public static int GetInt(MySqlDataReader drSource, string sOrdinal, int nNull)
        {
            int nOrdinal = drSource.GetOrdinal(sOrdinal);
            if(drSource.IsDBNull(nOrdinal))
            {
                return nNull;
            }
            else
            {
                switch(drSource.GetFieldType(nOrdinal).ToString())
                {
                case "System.Int32":
                    return drSource.GetInt32(nOrdinal);

                case "System.UInt32":
                    return (int)drSource.GetUInt32(nOrdinal);

                case "System.Decimal":
                    return (int)drSource.GetDecimal(nOrdinal);

                case "System.Double":
                    return (int)drSource.GetDouble(nOrdinal);

                default:
                    return nNull;
                }
            }
        }
#endif



        /// <summary>Returns a double from a OleDbDataReader.  The value in the database can be single or a double.</summary>
        /// <param name="dataReader">Specifies the datareader.</param>
        /// <param name="ordinal">Specifies the name of the column.</param>
        /// <param name="nullValue">Specifies the value to return if the database value is null.</param>
        /// <returns>The value of the specified field as a double.</returns>
        public static double getDouble(OleDbDataReader dataReader, string ordinal, double nullValue)
        {
            int ord = dataReader.GetOrdinal(ordinal);
            if (dataReader.IsDBNull(ord))
            {
                return nullValue;
            }
            else
            {
                switch (dataReader.GetFieldType(ord).ToString())
                {
                case "System.Double":
                    return dataReader.GetDouble(ord);

                case "System.Single":
                    float fValue = dataReader.GetFloat(ord);
                    return Math.Round(fValue, 6);

                case "System.Decimal":
                    return (double)dataReader.GetDecimal(ord);

                default:
                    return nullValue;
                }
            }
        }



        /// <summary>Returns a double from a SqlDataReader.  The value in the database can be single or a double.</summary>
        /// <param name="dataReader">Specifies the datareader.</param>
        /// <param name="ordinal">Specifies the name of the column.</param>
        /// <param name="nullValue">Specifies the value to return if the database value is null.</param>
        /// <returns>The value of the specified field as a double.</returns>
        public static double getDouble(SqlDataReader dataReader, string ordinal, double nullValue)
        {
            int ord = dataReader.GetOrdinal(ordinal);
            if (dataReader.IsDBNull(ord))
            {
                return nullValue;
            }
            else
            {
                switch (dataReader.GetFieldType(ord).ToString())
                {
                case "System.Double":
                    return dataReader.GetDouble(ord);

                case "System.Single":
                    return dataReader.GetFloat(ord);

                case "System.Decimal":
                    return (double)dataReader.GetDecimal(ord);

                default:
                    return nullValue;
                }
            }
        }



        /// <summary>Returns a DateTime from a OleDbDataReader handling the null posibility.</summary>
        /// <param name="dataReader">Specifies the datareader.</param>
        /// <param name="ordinal">Specifies the name of the column.</param>
        /// <param name="nullValue">Specifies the value to return if the database value is null.</param>
        /// <returns>A DateTime representation of the field value.</returns>
        public static DateTime getDateTime(OleDbDataReader dataReader, string ordinal, DateTime nullValue)
        {
            try
            {
                int ord = dataReader.GetOrdinal(ordinal);
                if (dataReader.IsDBNull(ord))
                {
                    return nullValue;
                }
                else
                {
                    return dataReader.GetDateTime(ord);
                }
            }
            catch { }
            return nullValue;
        }



        /// <summary>Returns a string from SqlDataReader handling the null posibility.</summary>
        /// <param name="dataReader">Specifies the datareader.</param>
        /// <param name="ordinal">Specifies the name of the column.</param>
        /// <param name="nullValue">Specifies the value to return if the database value is null.</param>
        /// <returns>A DateTime representation of the field value.</returns>
        public static DateTime getDateTime(SqlDataReader dataReader, string ordinal, DateTime nullValue)
        {
            int ord = dataReader.GetOrdinal(ordinal);
            if (dataReader.IsDBNull(ord))
            {
                return nullValue;
            }
            else
            {
                return dataReader.GetDateTime(ord);
            }
        }



        /// <summary>Returns a float from a OleDBDataReader.</summary>
        /// <param name="dataReader">Specifies the datareader.</param>
        /// <param name="ordinal">Specifies the name of the column.</param>
        /// <param name="nullValue">Specifies the value to return if the database value is null.</param>
        /// <returns>The value of the specified field as a double.</returns>
        public static float getFloat(OleDbDataReader dataReader, string ordinal, float nullValue)
        {
            int ord = dataReader.GetOrdinal(ordinal);
            if (dataReader.IsDBNull(ord))
            {
                return nullValue;
            }
            else
            {
                switch (dataReader.GetFieldType(ord).ToString())
                {
                case "System.Single":
                    return dataReader.GetFloat(ord);

                case "System.Double":
                    return (float)dataReader.GetDouble(ord);

                case "System.Decimal":
                    return (float)dataReader.GetDecimal(ord);

                default:
                    return nullValue;
                }
            }
        }


        /// <summary>Returns a float from a SqlDataReader.</summary>
        /// <param name="dataReader">Specifies the datareader.</param>
        /// <param name="ordinal">Specifies the name of the column.</param>
        /// <param name="nullValue">Specifies the value to return if the database value is null.</param>
        /// <returns>The value of the specified field as a double.</returns>
        public static float getFloat(SqlDataReader dataReader, string ordinal, float nullValue)
        {
            int ord = dataReader.GetOrdinal(ordinal);
            if (dataReader.IsDBNull(ord))
            {
                return nullValue;
            }
            else
            {
                switch (dataReader.GetFieldType(ord).ToString())
                {
                case "System.Single":
                    return dataReader.GetFloat(ord);

                case "System.Double":
                    return (float)dataReader.GetDouble(ord);

                case "System.Decimal":
                    return (float)dataReader.GetDecimal(ord);

                default:
                    return nullValue;
                }
            }
        }



        /// <summary>Returns a bool from a OleDB datareader handling the null posibility and the slightly different database forms.</summary>
        /// <param name="dataReader">Specifies the datareader.</param>
        /// <param name="ordinal">Specifies the name of the column.</param>
        /// <param name="nullValue">Specifies the value to return if the database value is null.</param>
        /// <returns>The value of the specified field as a bool.</returns>
        public static bool getBool(OleDbDataReader dataReader, string ordinal, bool nullValue)
        {
            int ord = dataReader.GetOrdinal(ordinal);
            if (dataReader.IsDBNull(ord))
            {
                return nullValue;
            }
            else
            {
                switch (dataReader.GetFieldType(ord).ToString())
                {
                case "System.Boolean":
                    return dataReader.GetBoolean(ord);

                default:
                    return nullValue;
                }
            }
        }



#if __INNOVAL_MYSQL__
        // Returns a bool from a MySql datareader handling the null posibility and the slightly different database forms.
        /// <summary>
        /// Returns a bool from a MySql datareader handling the null posibility and the slightly different database forms.
        /// </summary>
        /// <param name="drSource">Specifies the datareader.</param>
        /// <param name="sOrdinal">Specifies the name of the column.</param>
        /// <param name="bNull">Specifies the value to return if the database value is null.</param>
        /// <returns>The value of the specified field as a bool.</returns>
        public static bool GetBool(MySqlDataReader drSource, string sOrdinal, bool bNull)
        {
            int nOrdinal = drSource.GetOrdinal(sOrdinal);
            if(drSource.IsDBNull(nOrdinal))
            {
                return bNull;
            }
            else
            {
                switch(drSource.GetFieldType(nOrdinal).ToString())
                {
                case "System.Boolean":
                    return drSource.GetBoolean(nOrdinal);

                case "System.Byte":
                    byte byteValue = drSource.GetByte(nOrdinal);
                    return byteValue != 0;

                default:
                    return bNull;
                }
            }
        }
#endif



        /// <summary>Returns a decimal from a OleDbDataReader handling the null posibility and slightly different database forms.  The value in the database can be decimal, single or a double.</summary>
        /// <param name="dataReader">Specifies the datareader.</param>
        /// <param name="ordinal">Specifies the name of the column.</param>
        /// <param name="nullValue">Specifies the value to return if the database value is null.</param>
        /// <returns>The value of the specified field as a double.</returns>
        public static decimal getDecimal(OleDbDataReader dataReader, string ordinal, decimal nullValue)
        {
            int ord = dataReader.GetOrdinal(ordinal);
            if (dataReader.IsDBNull(ord))
            {
                return nullValue;
            }
            else
            {
                switch (dataReader.GetFieldType(ord).ToString())
                {
                case "System.Double":
                    return (decimal)dataReader.GetDouble(ord);

                case "System.Single":
                    return (decimal)dataReader.GetFloat(ord);

                case "System.Decimal":
                    return dataReader.GetDecimal(ord);

                default:
                    return nullValue;
                }
            }
        }



        #if __INNOVAL_MYSQL__
        // Returns a decimal from a MySqlDataReader handling the null posibility and slightly different database forms.
        /// <summary>
        /// Returns a decimal from a MySqlDataReader handling the null posibility and slightly different database forms.
        /// </summary>
        /// <remarks>
        /// The value in the database can be decimal, single or a double.
        /// </remarks>
        /// <param name="drSource">Specifies the datareader.</param>
        /// <param name="sOrdinal">Specifies the name of the column.</param>
        /// <param name="dNull">Specifies the value to return if the database value is null.</param>
        /// <returns>The value of the specified field as a double.</returns>
        public static decimal GetDecimal(MySqlDataReader drSource, string sOrdinal, decimal dNull)
        {
            int nOrdinal = drSource.GetOrdinal(sOrdinal);
            if(drSource.IsDBNull(nOrdinal))
            {
                return dNull;
            }
            else
            {
                switch(drSource.GetFieldType(nOrdinal).ToString())
                {
                case "System.Double":
                    return (decimal)drSource.GetDouble(nOrdinal);

                case "System.Single":
                    return (decimal)drSource.GetFloat(nOrdinal);

                case "System.Decimal":
                    return drSource.GetDecimal(nOrdinal);

                default:
                    return dNull;
                }
            }
        }
        #endif

        /*
        private bool GetBooleanFromString
            (
            OleDbDataReader		drSource,
            string				sOrdinal,
            string				sTrue
            )
        {
            int nOrdinal = drSource.GetOrdinal(sOrdinal);
            if(drSource.IsDBNull(nOrdinal))
            {
                return false;
            }
            else
            {
                string sValue = drSource.GetString(nOrdinal);
                if(sValue==sTrue)
                {
                    return true;
                }
                return false;
            }
        }
        */

        #endregion

        #region IsDBNull



        /// <summary>Returns true if the specified field contains a null value.  Otherwise returns false.</summary>
        /// <param name="dataReader">Specifies the datareader.</param>
        /// <param name="ordinal">Specifies the name of the field / column.</param>
        /// <returns>True for a null value, false otherwise.</returns>
        public static bool isDbNull(OleDbDataReader dataReader, string ordinal)
        {
            int ord = dataReader.GetOrdinal(ordinal);
            return dataReader.IsDBNull(ord);
        }



        #endregion

        #region DbValue



        /// <summary>Returns the value of a DataSet field as a string.  If the field is null then "NULL" is returned.  This is intended for building SQL commands.</summary>
        /// <param name="row">Specifies the row in a dataset containing the field.</param>
        /// <param name="ordinal">Specifies the name of the field.</param>
        /// <param name="size">Specifies the maximum size for a text field.</param>
        /// <param name="isQuotes">Specifies a text field with quotes around non null values.</param>
        /// <param name="isBoolean">Specifies a boolean field forced to return "0" false or "1" true.</param>
        /// <returns>The value of the field in the data row as a string.</returns>
        public static string dbValue(DataRow row, string ordinal, int size, bool isQuotes, bool isBoolean)
        {
            if (row.IsNull(ordinal))
            {
                return "NULL";
            }
            if (isQuotes)
            {
                string value = row[ordinal].ToString();
                value = value.Replace("'", "\"");
                if (value.Length > size)
                {
                    return "'" + value.Substring(0, size) + "'";
                }
                return "'" + value + "'";
            }
            return row[ordinal].ToString();
        }



        /// <summary>Returns the value of a DataReader field as a string.  If the field is null then "NULL" is returned.</summary>
        /// <param name="dataReader">Specifies the datareader containing the field.</param>
        /// <param name="ordinal">Specifies the name of the field.</param>
        /// <param name="size">Specifies the maximum size for a text field.</param>
        /// <param name="isQuotes">Specifies a text field with quotes around non null values.</param>
        /// <param name="isBoolean">Specifies a boolean field forced to return "0" false or "1" true.</param>
        /// <returns>The value of the field in the datareader row as a string..</returns>
        public static string dbValue(IDataReader dataReader, string ordinal, int size, bool isQuotes, bool isBoolean)
        {
            int nOrdinal = dataReader.GetOrdinal(ordinal);
            if (dataReader.IsDBNull(nOrdinal))
            {
                return "NULL";
            }
            if (isQuotes)
            {
                string value = dataReader.GetValue(nOrdinal).ToString();
                value = value.Replace("'", "\"");
                if (value.Length > size)
                {
                    return "'" + value.Substring(0, size) + "'";
                }
                return "'" + value + "'";
            }
            if (isBoolean)
            {
                bool isTrue = bool.Parse(dataReader.GetValue(nOrdinal).ToString());
                if (isTrue)
                {
                    return "1";
                }
                return "0";
            }
            return dataReader.GetValue(nOrdinal).ToString();
        }



        /// <summary>Returns the value of the specified string ready for use in a SQL command.  If the specified string is empty then NULL is returned.  Otherwise the string enclosed in quotes is returned.</summary>
        /// <param name="value">Specifiy the string to insert into a SQL command.</param>
        /// <returns>The specified string in a SQL ready format.</returns>
        public static string dbValue(string value)
        {
            if (value.Length == 0)
            {
                return "NULL";
            }
            return "'" + value + "'";
        }



        #endregion

        #endregion

    }
}
