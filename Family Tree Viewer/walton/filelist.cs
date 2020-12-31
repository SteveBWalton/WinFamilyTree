using System;
using System.IO;

namespace walton
{
    /// <summary>Class to support a list of recent files on the file menu.  This now allows the selection of records within a database.  This superceeds the clsFileList class.  This depends on the innoval.xml module.</summary>
    public class FileList
    {
        #region Member Variables

        /// <summary>Class to represent a single item on a recent files menu.</summary>
        private class FileListItem
        {
            #region Member Variables

            /// <summary>The string to use in the display name when the filename is actually required as the displayname.
            /// This is the default value.
            /// </summary>
            public const string SAMEASFILENAME = "";

            /// <summary>The filename of the file list item.</summary>
            public string Filename;

            /// <summary>The display name of the file list item.</summary>
            private string m_sDisplayName;

            #endregion

            #region Constructors etc ...


            public FileListItem()
            {
                Filename = "";
                m_sDisplayName = SAMEASFILENAME;
            }


            public FileListItem(string sFilename)
            {
                Filename = sFilename;
                m_sDisplayName = SAMEASFILENAME;
            }


            public FileListItem(string sFilename, string sDisplayName)
            {
                Filename = sFilename;
                m_sDisplayName = sDisplayName;
            }


            public FileListItem(XmlNode xmlFile)
            {
                Open(xmlFile);
            }


            #endregion

            #region File IO


            public bool Open(XmlNode xmlFile)
            {
                Filename = xmlFile.getAttributeValue("filename", "", true);
                m_sDisplayName = xmlFile.getAttributeValue("display_name", SAMEASFILENAME, false);
                return true;
            }


            public bool Write(XmlNode xmlFile)
            {
                xmlFile.setAttributeValue("filename", Filename);
                if (m_sDisplayName == SAMEASFILENAME)
                {
                    xmlFile.deleteAttribute("display_name");
                }
                else
                {
                    xmlFile.setAttributeValue("display_name", m_sDisplayName);
                }
                return true;
            }


            #endregion

            #region Supporting Functions


            public void Copy(FileListItem oCopy)
            {
                Filename = oCopy.Filename;
                m_sDisplayName = oCopy.m_sDisplayName;
            }


            public string DisplayName
            {
                get
                {
                    if (m_sDisplayName == SAMEASFILENAME)
                    {
                        return Filename;
                    }
                    return m_sDisplayName;
                }
                set
                {
                    m_sDisplayName = value;
                }
            }


            #endregion
        }

        /// <summary>Maximum number of files in the list.</summary>
        private int size_;

        /// <summary>Configuration file to store the file names in.</summary>
        private XmlDocument config_;

        /// <summary>The name of the node to store the file name under.</summary>
        private string nodeName_;

        /// <summary>The file names of the recent files.</summary>
        private FileListItem[] files_;

        #endregion

        #region Constructors



        /// <summary>Class constructor.</summary>
        /// <param name="nSize">Number of files to keep in the list.</param>
        /// <param name="oConfig">Specifies the XML file to store the file list in.</param>
        public FileList(int nSize, XmlDocument oConfig)
            : this(nSize, oConfig, "recentfilelist")
        {
        }



        /// <summary>Class constructor with the node specified.</summary>
        /// <param name="nSize">Number of files to keep in the list.</param>
        /// <param name="oConfig">Specifies the XML file to store the file list in.</param>
        /// <param name="sNodeName">Specifies the node to store the file list in.</param>
        public FileList(int nSize, XmlDocument oConfig, string sNodeName)
        {
            // Store the input parameters
            size_ = nSize;
            config_ = oConfig;
            nodeName_ = sNodeName;

            // Initialise parameters
            files_ = new FileListItem[size_];

            // Load the file options
            XmlNode oRecentFileList = config_.getNode(nodeName_);
            for (int nI = 0; nI < size_; nI++)
            {
                XmlNode xmlFile = oRecentFileList.getNode("file" + nI.ToString("00"));
                files_[nI] = new FileListItem(xmlFile);
            }
        }



        #endregion

        #region Public Methods



        /// <summary>Records the specified file in the recent file list as if it had just been openned.  Client programs are expected to call this just after they open a file.</summary>
        /// <param name="filename">Specifies the filename the openned file.</param>
        /// <returns>True for success.  False, otherwise.</returns>
        public bool openFile(string filename)
        {
            return openFile(filename, FileListItem.SAMEASFILENAME);
        }



        /// <summary>Records the specified file in the recent file list as if it had just been openned.  Client programs are expected to call this just after they open a file.  This allows the display name to set.  This is intended to be used when a database record is accessed not actually a file.  The filename can contain record identifier and the displayname a human readable description of the record.</summary>
        /// <param name="filename">Specifies the filename the openned file or the identifier of a database record.</param>
        /// <param name="displayName">Specifies the human readable description of the file/record.</param>
        /// <returns>True for success.  False, otherwise.</returns>
        public bool openFile(string filename, string displayName)
        {
            // Check that a filename has been specified.
            if (filename == "")
            {
                return false;
            }

            // Is the file already in the list.
            int index = -1;
            for (int i = 0; i < size_; i++)
            {
                if (filename == files_[i].Filename)
                {
                    index = i;
                }
            }

            if (index == -1)
            {
                // New entry on the file list.
                index = size_ - 1;
            }

            // Move the files down.
            for (int nI = index; nI > 0; nI--)
            {
                files_[nI].Copy(files_[nI - 1]);
            }

            // Replace the top file.
            files_[0].Filename = filename;
            files_[0].DisplayName = displayName;

            // Save the changes.
            XmlNode xmlRecentFileList = config_.getNode(nodeName_);
            for (int i = 0; i < size_; i++)
            {
                XmlNode xmlFile = xmlRecentFileList.getNode("file" + i.ToString("00"));
                files_[i].Write(xmlFile);
            }
            config_.save();

            // Return a change.
            return true;
        }



        /// <summary>Records the specified file in the recent files list, as if it had just been saved.  Client programs are expected to call this just after they 'SaveAs' a file.</summary>
        /// <param name="filename">Specifies the full filename of the saved file.</param>
        /// <returns>True for success, false otherwise.</returns>
        public bool saveAsFile(string filename)
        {
            return openFile(filename);
        }



        /// <summary>Records the specified file in the recent files list, as if it had just been saved.  Client programs are expected to call this just after they 'SaveAs' a file.  This allows the display name to set.  This is intended to be used when a database record is accessed not actually a file.  The filename can contain record identifier and the displayname a human readable description of the record.</summary>
        /// <param name="filename">Specifies the filename the openned file or the identifier of a database record.</param>
        /// <param name="displayName">Specifies the human readable description of the file/record.</param>
        /// <returns>True for success, false otherwise.</returns>
        public bool saveAsFile(string filename, string displayName)
        {
            return openFile(filename, displayName);
        }



        /// <summary>Returns the filename for the specified specified recent file.</summary>
        /// <param name="index">Specifies the index of the recent file.</param>
        /// <returns>The filename for the specified specified recent file.</returns>
        public string getRecentFilename(int index)
        {
            return files_[index].Filename;
        }



        /// <summary>Returns the display name of the specified recent file.</summary>
        /// <param name="index">Specifies the index of the recent file.</param>
        /// <param name="path">Specifies the path to ignore.</param>
        /// <returns>The display name of the specified recent file.</returns>
        public string getDisplayName(int index, string path)
        {
            // Return only the post path part if the paths match.
            if (files_[index].Filename.StartsWith(path))
            {
                return files_[index].Filename.Substring(path.Length);
            }

            // Return the full filename when the paths don't match.
            return files_[index].DisplayName;
        }



        /// <summary>Returns the display name of the specified recent file.</summary>
        /// <param name="index">Specifies the index of the recent file.</param>
        /// <returns>The display name of the specified recent file.</returns>
        public string getDisplayName(int index)
        {
            // Find the path of the first file
            string defaultPath = "";
            try
            {
                defaultPath = Path.GetDirectoryName(files_[0].Filename);
                if (!defaultPath.EndsWith("\\"))
                {
                    defaultPath += "\\";
                }
            }
            catch
            {
            }

            // Return the display name without this path.
            return getDisplayName(index, defaultPath);
        }



        #endregion
    }
}
