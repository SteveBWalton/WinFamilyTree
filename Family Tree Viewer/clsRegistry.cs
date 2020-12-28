using System;
using Microsoft;				// Registry
using Microsoft.Win32;			// Registry

namespace Family_Tree_Viewer
{
	/// <summary>
	/// Class to handle the registry.
	/// </summary>
	public class clsRegistry
	{
		/// <summary>
		/// Master key for entries in the registry for this application.
		/// </summary>
		private const string HOMEKEY = "Family Tree";

		/// <summary>
		/// Class constructor.
		/// </summary>
		public clsRegistry()
		{
		}

		// *************************************************************************************************************
		/// <summary>
		/// Returns the home registry key for this application.  Must be closed by the calling function.
		/// </summary>
		/// <returns>The home registry key.</returns>
		static private RegistryKey GetHomeKey()
		{
			// Open the subkey and create if necessary (Software key always exists)
			RegistryKey rkSoftware = Registry.CurrentUser.OpenSubKey("Software",true);
			// RegistryKey rkSoftware = Registry.LocalMachine.OpenSubKey("Software",true);
			RegistryKey rkCompany = OpenCreateKey(rkSoftware,"Steve Walton");
			RegistryKey rkHome = OpenCreateKey(rkCompany,HOMEKEY);

			// Close the parent keys
			rkSoftware.Close();
			rkCompany.Close();

			// Return the registry key
			return rkHome;
		}
		
		// *************************************************************************************************************
		/// <summary>
		/// Opens the specified subkey of the supplied key.  If the subkey does not exist then it creates the subkey
		/// </summary>
		/// <param name="oKey">Parent key</param>
		/// <param name="sName">Subkey name</param>
		/// <returns>The specified subkey</returns>
		static private RegistryKey OpenCreateKey
			(
			RegistryKey		oKey,		// Key in registry
			string			sName		// Name of the required subkey
			)
		{
			RegistryKey		oResult;	// Key to return
			try
			{
				oResult = oKey.OpenSubKey(sName,true);

				if(oResult==null)
				{
					oResult = oKey.CreateSubKey(sName);
				}
			}
			catch
			{
				oResult = oKey.CreateSubKey(sName);
			}

			// Return the subkey
			return oResult;
		}
		
		// *************************************************************************************************************
		/// <summary>
		/// Write a string attribute value into the registry.  The key is the default key for the application.
		/// </summary>
		/// <param name="sName">Specify the name of the attribute.</param>
		/// <param name="sValue">Specify the value of the attribute.</param>
		/// <returns>True for success.  False for failure.</returns>
		static public bool WriteRegistryValue
			(
			string		sName,			// Name of the value to read
			string		sValue			// Value to write
			)
		{
			RegistryKey	rkHome;			// Home key for this application			

			// Open the home key
			rkHome = GetHomeKey();

			// Write a value to the registry
			rkHome.SetValue(sName,sValue);
			
			// Close the home key
			rkHome.Close();

			// return success
			return true;
		}
		/// <summary>
		/// Write an integer attribute value into the registry.  The key is the default key for the application.
		/// </summary>
		/// <param name="sName">Specify the name of the attribute.</param>
		/// <param name="nValue">Specify the value of the attribute.</param>
		/// <returns>True for success.  False for failure.</returns>
		static public bool WriteRegistryValue
			(
			string		sName,			// Name of the value to read
			int			nValue			// Value to write
			)
		{
			RegistryKey	rkHome;			// Home key for this application			

			// Open the home key
			rkHome = GetHomeKey();

			// Write a value to the registry
			rkHome.SetValue(sName,nValue);
			
			// Close the home key
			rkHome.Close();

			// return success
			return true;
		}

		// *************************************************************************************************************
		/// <summary>
		/// Reads a string attribute value from the registry.  The key is the default key for the application.
		/// </summary>
		/// <param name="sName">Name of the attribute.</param>
		/// <param name="sDefaultValue">Default value to use if the attribute is missing.</param>
		/// <returns>Value of the named attribute as a string.</returns>
		static public string ReadRegistryValue
			(
			string		sName,			// Name of the value to read
			string		sDefaultValue	// Value if none specifed
			)
		{
			RegistryKey	rkHome;			// Home key for this application			
			string		sReturn;		// Value to return

			// Open the home key
			rkHome = GetHomeKey();

			sReturn = (string)rkHome.GetValue(sName,sDefaultValue);
			rkHome.SetValue(sName,sReturn);

			// Close the home key
			rkHome.Close();

			// Return the value
			return sReturn;
		}
		/// <summary>
		/// Reads an integer attribute value from the registry.  The key is the default key for the application.
		/// </summary>
		/// <param name="sName">Name of the attribute</param>
		/// <param name="nDefaultValue">Value to use if the attribute is missing.</param>
		/// <returns>Value of the named attribute as an integer.</returns>
		static public int ReadRegistryValue
			(
			string		sName,			// Name of the value to read
			int			nDefaultValue	// Value if none specifed
			)
		{
			RegistryKey	rkHome;			// Home key for this application			
			int			nReturn;		// Value to return

			// Open the home key
			rkHome = GetHomeKey();

			nReturn = (int)rkHome.GetValue(sName,nDefaultValue);
			rkHome.SetValue(sName,nReturn);

			// Close the home key
			rkHome.Close();

			// Return the value
			return nReturn;
		}
	}
}
