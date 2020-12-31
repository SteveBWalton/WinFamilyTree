using System;

// Path 
using System.IO;

using System.Security.AccessControl;

namespace walton
{
    /// <summary>Class to help with the use of standard data paths.  This is mostly to remove / control the version information.  This superceeds the clsDataPaths class.</summary>
    public class DataPaths
    {



        /// <summary>Class constructor.  Do not use.  The functions in this class are static.</summary>
        public DataPaths()
        {
        }



        /// <summary>Returns the non roaming user directory.  This is usually 'C:\Programs and Settings\Waltons\Local Settings\Application Data' under Windows XP and 'C:\Users\Waltons\AppData\Local' under Windows 7.</summary>
        /// <returns>The non roaming user's private base directory.</returns>
        static public string getUserDirectory()
        {
            // Get the base directory.
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            // Return the directory name.
            return directory;
        }



        /// <summary>Returns the specific user's directory with the specified company and application.  This is usually 'C:\Programs and Settings\Waltons\Local Settings\Application Data' under Windows XP and 'C:\Users\Waltons\AppData\Local' under Windows 7.</summary>
        /// <param name="companyName">Specifies the company name for the folder.</param>
        /// <param name="applicationName">Specifies the application name for the folder.</param>
        /// <returns>A non roaming user's private directory.</returns>
        static public string getUserDirectory(string companyName, string applicationName)
        {
            // Get the base directory.
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            directory += Path.DirectorySeparatorChar + companyName;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            directory += Path.DirectorySeparatorChar + applicationName;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Return the new directory name.
            return directory;
        }



        /// <summary>Returns the specific user's directory with the specified company, application and version.  This is usually 'C:\Programs and Settings\Waltons\Local Settings\Application Data' under Windows XP and 'C:\Users\Waltons\AppData\Local' under Windows 7.</summary>
        /// <param name="companyName">Specifies the company name for the folder.</param>
        /// <param name="applicationName">Specifies the application name for the folder.</param>
        /// <param name="versionNum">Specifies the application version number of the folder.</param>
        /// <returns>A non roaming user's private directory.</returns>
        static public string getUserDirectory(string companyName, string applicationName, string versionNum)
        {
            // Get the base directory.
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            directory += Path.DirectorySeparatorChar + companyName;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            directory += Path.DirectorySeparatorChar + applicationName;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            directory += Path.DirectorySeparatorChar + versionNum;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Return the new directory name.
            return directory;
        }



        /// <summary>Returns the specific user's directory with the specified company, group, application and version.  This is usually 'C:\Programs and Settings\Waltons\Local Settings\Application Data' under Windows XP and  'C:\Users\Waltons\AppData\Local' under Windows 7.</summary>
        /// <param name="companyName">Specifies the company name for the folder.</param>
        /// <param name="groupName">Specifies the group name for the folder.</param>
        /// <param name="applicationName">Specifies the application name for the folder.</param>
        /// <param name="versionNum">Specifies the application version number of the folder.</param>
        /// <returns>A non roaming user's private directory.</returns>
        static public string getUserDirectory(string companyName, string groupName, string applicationName, string versionNum)
        {
            // Get the base directory.
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            directory += Path.DirectorySeparatorChar + companyName;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            directory += Path.DirectorySeparatorChar + groupName;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            directory += Path.DirectorySeparatorChar + applicationName;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            directory += Path.DirectorySeparatorChar + versionNum;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Return the new directory name.
            return directory;
        }



        /// <summary>Returns the base all users directory on the system.</summary>
        /// <returns>The base all users directory on the system.</returns>
        static public string getAllUsersDirectory()
        {
            // Find the all user directory.
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

            // Return the directory name.
            return directory;
        }



        /// <summary>Returns all user directory with the specified company and application.  If the directory does not exist then it is created.</summary>
        /// <param name="companyName">Specifies the company name for the folder.</param>
        /// <param name="applicationName">Specifies the application name for the folder.</param>
        /// <returns>An all users directory.</returns>
        static public string getAllUsersDirectory(string companyName, string applicationName)
        {
            // Find the all user directory
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

            directory += Path.DirectorySeparatorChar + companyName;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                writeAccessForStandardUsers(directory);
            }

            directory += Path.DirectorySeparatorChar + applicationName;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                writeAccessForStandardUsers(directory);
            }

            // Return the directory name.
            return directory;
        }


        
        /// <summary>Returns all user directory with the specified company, application and version.  If the directory does not exist then it is created.</summary>
        /// <param name="companyName">Specifies the company name for the folder.</param>
        /// <param name="applicationName">Specifies the application name for the folder.</param>
        /// <param name="versionNum">Specifies the application version number of the folder.</param>
        /// <returns>An all users directory.</returns>
        static public string getAllUsersDirectory(string companyName, string applicationName, string versionNum)
        {
            // Find the all user directory
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

            directory += Path.DirectorySeparatorChar + companyName;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            // Can move this up into the if() condition once the Sapa problem has gone away.
            writeAccessForStandardUsers(directory);

            directory += Path.DirectorySeparatorChar + applicationName;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            // Can move this up into the if() condition once the Sapa problem has gone away.
            writeAccessForStandardUsers(directory);

            directory += Path.DirectorySeparatorChar + versionNum;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            // Can move this up into the if() condition once the Sapa problem has gone away.
            writeAccessForStandardUsers(directory);

            // Return the directory name
            return directory;
        }



        /// <summary>Returns an all users directory with the specified company, group, application and version.  If the directory does not exist then it is created.</summary>
        /// <param name="companyName">Specifies the company name for the folder.</param>
        /// <param name="groupName">Specifies the group name for the folder.</param>
        /// <param name="applicationName">Specifies the application name for the folder.</param>
        /// <param name="versionNum">Specifies the application version number of the folder.</param>
        /// <returns>An all users directory.</returns>
        static public string getAllUsersDirectory(string companyName, string groupName, string applicationName, string versionNum)
        {
            // Find the all user directory
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

            directory += Path.DirectorySeparatorChar + companyName;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                writeAccessForStandardUsers(directory);
            }

            directory += Path.DirectorySeparatorChar + groupName;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                writeAccessForStandardUsers(directory);
            }

            directory += Path.DirectorySeparatorChar + applicationName;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                writeAccessForStandardUsers(directory);
            }

            directory += Path.DirectorySeparatorChar + versionNum;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                writeAccessForStandardUsers(directory);
            }

            // Return the directory name
            return directory;
        }


        
        /// <summary>Returns the My Documents directory.</summary>
        /// <returns>The filename of the My Documents directory without a trailing \ character.</returns>
        static public string getMyDocuments()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        }


        
        /// <summary>Returns the specified application directory inside the my documents directory.</summary>
        /// <param name="application">Specifies the name of the application.</param>
        /// <returns>The filename of the My Documents\Application directory without a trailing \ character.</returns>
        static public string getMyDocuments(string application)
        {
            // Find the My Documents directory
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            directory += Path.DirectorySeparatorChar + application;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Return the directory name
            return directory;
        }


        
        /// <summary>Returns the specified company name\application directory inside the my documents directory.</summary>
        /// <param name="companyName">Specifies the company name for the folder.</param>
        /// <param name="applicationName">Specifies the application name for the folder.</param>
        /// <returns>The filename of the My Documents\CompanyName\Application directory without a trailing \ character.</returns>
        static public string getMyDocuments(string companyName, string applicationName)
        {
            // Find the My Documents directory
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            directory += Path.DirectorySeparatorChar + companyName;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            directory += Path.DirectorySeparatorChar + applicationName;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Return the directory name
            return directory;
        }


        
        /// <summary>Returns the user's MyPictures directory.</summary>
        /// <returns>The user's MyPictures directory.</returns>
        static public string getMyPictures()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        }



        #region Supporting Functions



        /// <summary>Resets the ACL to allow all users the read / write the specified directory.</summary>
        /// <param name="directory">Specifies the full pathname of the directory to allow all users to read / write.</param>
        /// <returns>True for success, false otherwise.</returns>
        static private Boolean writeAccessForStandardUsers(string directory)
        {
            try
            {
                // Get the security information for the directory.
                DirectoryInfo directoryInfo = new DirectoryInfo(directory);
                DirectorySecurity directorySecurity = directoryInfo.GetAccessControl(AccessControlSections.Access);

                // System.Security.Principal.SecurityIdentifier oUser = new System.Security.Principal.SecurityIdentifier(System.Security.Principal.WellKnownSidType.AccountDomainUsersSid, DOMAIN_SID);
                //System.Security.Principal.SecurityIdentifier oBultinUser = new System.Security.Principal.SecurityIdentifier(System.Security.Principal.WellKnownSidType.BuiltinUsersSid, null);

                // This is Everyone.
                System.Security.Principal.SecurityIdentifier users = new System.Security.Principal.SecurityIdentifier(System.Security.Principal.WellKnownSidType.WorldSid, null);

                if (users != null)
                {
                    bool bResult = false;

                    // Add the rule to the directory itself.
                    System.Security.AccessControl.FileSystemAccessRule newRule = new FileSystemAccessRule(users, System.Security.AccessControl.FileSystemRights.Modify, AccessControlType.Allow);
                    directorySecurity.ModifyAccessRule(AccessControlModification.Set, newRule, out bResult);

                    // Add the inheritance version of the rule.
                    newRule = new FileSystemAccessRule(users, System.Security.AccessControl.FileSystemRights.Modify, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.InheritOnly, AccessControlType.Allow);
                    directorySecurity.ModifyAccessRule(AccessControlModification.Add, newRule, out bResult);

                    // Write these changes to the disk.
                    directoryInfo.SetAccessControl(directorySecurity);
                }

                // Return success.
                return true;
            }
            catch
            {
            }

            // Return failure.
            return false;
        }



        #endregion

    }
}
