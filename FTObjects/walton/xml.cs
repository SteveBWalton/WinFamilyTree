using System;
using System.Collections;
using System.Globalization;
// Can not include 'System.Xml' because I want to use XmlDocument and XmlNode as class names in the innoval namespace.
// using System.Xml;

using System.IO;
using System.Text;

/// <summary>Namespace for walton library modules.</summary>
namespace walton
{
    /// <summary>Class to represent a Xml document.  Use with XmlNode to navigate the document.  This supersedes clsXmlDocument.  This is in effective version two and replaces clsXmlBase, clsXml, clsXmlCursor.  There is no cursor, in order to be thread safe.</summary>
    // This class can not be renamed as XmlDocument easily because this name is used the System.Xml namespace.       
    public class XmlDocument
    {
        #region Member Variables

        /// <summary>The source of the xml document.</summary>
        public enum Source
        {
            /// <summary>An regular input xml file.  (default and usual).</summary>
            REGULAR_FILE,

            /// <summary>A compressed xml file.</summary>
            COMPRESSED_FILE,

            /// <summary>Stream input.  This is probably from a web page.</summary>
            STREAM
        }

        /// <summary>The xml document.</summary>
        protected System.Xml.XmlDocument document_;

        /// <summary>The filename of the Xml file.</summary>
        protected string fileName_;

        /// <summary>The one and only root node.</summary>
        private XmlNode root_;

        /// <summary>The source of the Xml document.  This was previously a boolean.</summary>
        protected Source mode_;

        /// <summary>True, if the document has changed since the last save, false otherwise.</summary>
        private bool isChanged_;

        /// <summary>Datetime of the document's last save.</summary>
        private DateTime lastWriteTime_;

        /// <summary>True to use binary search for nodes.</summary>
        protected bool isBinarySearch_;

        /// <summary>True to use case sensitive matches.</summary>
        private bool isCaseSensitive_ = true;

        /// <summary>True to force all keys (nodes and attributes) to be lower case.</summary>
        private bool isForceLowerCase_ = false;

        #endregion

        #region File Operations



        /// <summary>Save the xml to disk.  Only if changes have been recorded.</summary>
        public void save()
        {
            if (isChanged_)
            {
                save(fileName_);
            }
        }



        /// <summary>Save the Xml to disk with force parameter.</summary>
        /// <param name="isForce">If true then the Xml is saved regardless of changes.  If false then the same as Save().</param>
        public void save(bool isForce)
        {
            if (isForce)
            {
                isChanged_ = true;
            }
            save();
        }



        /// <summary>Save the xml file into a new file with the specified filename with optional compression.</summary>
        /// <param name="fileName">Specify the new filename for the xml file.</param>
        public void save(string fileName)
        {
            if (fileName != "" && mode_ != Source.STREAM)
            {
                try
                {
                    if (mode_ == Source.COMPRESSED_FILE)
                    {
                        FileStream destFile = File.Create(fileName);
                        System.IO.Compression.GZipStream compStream = new System.IO.Compression.GZipStream(destFile, System.IO.Compression.CompressionMode.Compress);
                        document_.Save(compStream);
                        compStream.Dispose();
                        destFile.Close();
                    }
                    else
                    {
                        document_.Save(fileName);
                    }

                    isChanged_ = false;
                    lastWriteTime_ = File.GetLastWriteTime(fileName_);
                    fileName_ = fileName;
                }
                catch
                {
                }
            }
        }



        /// <summary>Reload the xml from disk.  Pickup any new settings.</summary>
        public void open()
        {
            open("xml");
        }



        /// <summary>Reloads the xml from disk.  If no root node exists then one is created with the specified name.</summary>
        /// <param name="rootName">Specifies the name to create a root node if none exists.</param>
        private void open(string rootName)
        {
            if (!File.Exists(fileName_))
            {
                create(rootName);
            }
            try
            {
                bool isRegularXml = false;
                try
                {
                    document_.Load(fileName_);
                    isRegularXml = true;
                    mode_ = Source.REGULAR_FILE;
                }
                catch
                {
                }
                if (!isRegularXml)
                {
                    FileStream sourceFile = File.Open(fileName_, FileMode.Open);
                    System.IO.Compression.GZipStream zipInput = new System.IO.Compression.GZipStream(sourceFile, System.IO.Compression.CompressionMode.Decompress);
                    document_.Load(zipInput);
                    zipInput.Dispose();
                    sourceFile.Close();

                    mode_ = Source.COMPRESSED_FILE;
                }
            }
            catch
            {
            }
            isChanged_ = false;
            lastWriteTime_ = File.GetLastWriteTime(fileName_);

            // We only use one root node.
            root_ = null;
            foreach (System.Xml.XmlNode xmlNode in document_.ChildNodes)
            {
                if (xmlNode.NodeType == System.Xml.XmlNodeType.Element)
                {
                    root_ = new XmlNode(xmlNode, this);
                    break;
                }
            }

            // Create a root node if none exist.
            if (root_ == null)
            {
                createRootNode(rootName);
            }
        }



        /// <summary>Reload the xml from disk.  If the xml file looks newer than the current one.  Returns true if the xml file is reloaded, otherwise return false.</summary>
        /// <returns>True if the xml file is loaded, false if no action is taken.</returns>
        public bool refresh()
        {
            // Check the date on the xml file.
            try
            {
                if (lastWriteTime_ != File.GetLastWriteTime(fileName_))
                {
                    // Reload the xml file.
                    open();
                    return true;
                }
            }
            catch
            {
            }

            // Nothing to do.
            return false;
        }



        /// <summary>Creates the Xml file if it does not already exist.</summary>
        /// <returns></returns>
        public bool create(string rootName)
        {
            // Check if the file already exists.
            if (File.Exists(fileName_))
            {
                // Nothing to do.
                return true;
            }

            // Check if the directory exists.
            string directory = Path.GetDirectoryName(fileName_);
            if (!Directory.Exists(directory))
            {
                // Create the directory.
                if (!createDirectory(directory))
                {
                    // The directory could not be created.
                    return false;
                }
            }

            // Create the first version of the Xml file.
            createRootNode(rootName);

            // Return success.
            return false;
        }



        /// <summary>Create a node at the top level with the specified name.</summary>
        /// <param name="rootName">Specifies the name of the node to create.</param>
        private void createRootNode(string rootName)
        {
            string nameSpace = "";
            if (document_.DocumentElement != null)
            {
                nameSpace = document_.DocumentElement.NamespaceURI;
            }
            System.Xml.XmlNode oNewNode = document_.CreateNode(System.Xml.XmlNodeType.Element, rootName, nameSpace);
            try
            {
                document_.AppendChild(oNewNode);
                isChanged_ = true;
            }
            catch
            {
            }
            // m_oDoc.Save(m_sFilename);
            root_ = new XmlNode(document_.FirstChild, this);
        }



        /// <summary>Creates the specified directory.</summary>
        /// <param name="directory">Specifies the name of the directory to create.</param>
        /// <returns>True for success, false otherwise.</returns>
        private bool createDirectory(string directory)
        {
            // Validate the inputs
            if (directory == null)
            {
                return false;
            }
            if (directory == "")
            {
                return false;
            }

            // Check that there is something to do.
            if (Directory.Exists(directory))
            {
                return true;
            }

            // Check that the parent directory exist and then make this directory.
            string parent = Path.GetDirectoryName(directory);
            if (createDirectory(parent))
            {
                try
                {
                    Directory.CreateDirectory(directory);
                    return true;
                }
                catch
                {
                }
            }

            // Some Error.
            return false;
        }



        /// <summary>The full filename of the Xml file.</summary>
        public string fileName
        {
            get { return fileName_; }
        }



        #endregion

        #region Constructors etc ...



        /// <overloads>Class constructor for the Xml reader / writer.  Call Dispose() to write any changes before destroying the object.</overloads>
        /// <summary>Class constructor specifing the filename.</summary>
        /// <param name="fileName">Specifies the filename of the Xml file.</param>
        public XmlDocument(string fileName)
            : this(fileName, "xml", false)
        {
        }



        /// <summary>Class constructor to set the filename and binary search status.</summary>
        /// <param name="fileName">Specifies the filename of the Xml file.</param>
        /// <param name="isBinarySearch">Specifies the binary search value of the object.</param>
        public XmlDocument(string fileName, bool isBinarySearch)
            : this(fileName, "xml", isBinarySearch)
        {
        }



        /// <summary>Class Constructor to set the filename and the default root element name.</summary>
        /// <param name="fileName">Specifies the filename of the xml file.</param>
        /// <param name="rootName">Specifies the root element name if a new document is created.</param>
        public XmlDocument(string fileName, string rootName)
            : this(fileName, rootName, false)
        {
        }



        /// <summary>Class constructor to set the filename, the default root node name and binary search status.</summary>
        /// <param name="fileName">Specifies the filename of the Xml file.</param>
        /// <param name="rootName">Specifies the name for the root node if a new document is created.</param>
        /// <param name="isBinarySearch">Specifies the binary search value of the object.</param>
        public XmlDocument(string fileName, string rootName, bool isBinarySearch)
        {
            isCaseSensitive_ = true;
            fileName_ = fileName;
            isBinarySearch_ = isBinarySearch;
            document_ = new System.Xml.XmlDocument();
            try
            {
                open(rootName);
            }
            catch
            {
            }
        }



        /// <summary>Class constructor with binary search and root name specified.  This constructor can be used to create new xml documents that overwrite existing files without taking any values from the existing file.</summary>
        /// <param name="isBinarySearch">Specifies the binary search value of the object.</param>
        /// <param name="rootName">Specifies the name for the root node if a new document is created.</param>
        public XmlDocument(bool isBinarySearch, string rootName)
        {
            isCaseSensitive_ = true;
            mode_ = Source.REGULAR_FILE;
            fileName_ = "";
            isBinarySearch_ = isBinarySearch;
            document_ = new System.Xml.XmlDocument();

            // Create a root node if none exists.
            createRootNode(rootName);
        }



        /// <summary>Class constructor specifing an input stream.  This is expected to be a result from a web request.</summary>
        /// <param name="inputStream">Specifies the input stream.</param>
        public XmlDocument(Stream inputStream)
        {
            isCaseSensitive_ = true;
            mode_ = Source.STREAM;
            fileName_ = "";
            isBinarySearch_ = false;
            document_ = new System.Xml.XmlDocument();
            try
            {
                document_.Load(inputStream);
            }
            catch
            {
            }
        }



        /// <summary>Empty class constructor.</summary>
        public XmlDocument()
        {
            isCaseSensitive_ = true;
            mode_ = Source.REGULAR_FILE;
            fileName_ = "";
            isBinarySearch_ = false;
            document_ = new System.Xml.XmlDocument();

            isChanged_ = false;
            lastWriteTime_ = DateTime.MinValue;

            // Create a root node if none exist.
            createRootNode("xml");
        }



        /// <summary>Release all resources held by the document.  Ready to delete the document object.</summary>
        public void Dispose()
        {
            save();
        }



        #endregion

        #region Nodes



        /// <summary>Returns the specified node from the document.  This is a childnode of the one root node.  Creates the node if it does not exist in the document.  Child nodes can be specified using the '/' character.</summary>
        /// <param name="name">Specifies the name of the node to return.  Use / to specifed parent and child nodes.</param>
        /// <returns>The node in the document with the specified name.</returns>
        public XmlNode getNode(string name)
        {
            return root_.getNode(name);
        }



        /// <summary>Returns the specified child node of the document.  Does not create the node if it does not exist.  The index is zero based.</summary>
        /// <param name="index">Specifies the zero based index of the node.</param>
        /// <returns>A clsXmlNode under the root node or null.</returns>
        public XmlNode getNode(int index)
        {
            return root_.getNode(index);
        }



        /// <summary>Returns the specified node from the document.  Returns null if the specified node is not in the document.  Child nodes can be specified using the '/' character.</summary>
        /// <param name="name">Specifies the name of the node to return.  Use / to specifed parent and child nodes.</param>
        /// <returns></returns>
        public XmlNode findNode(string name)
        {
            return root_.findNode(name);
        }



        /// <summary>Returns the number of nodes under this node.</summary>
        /// <returns>The number of nodes under this node.</returns>
        public int getNumNodes()
        {
            return root_.getNumNodes();
        }



        /// <summary>The one and only root node in the document.</summary>
        public XmlNode root
        {
            get { return root_; }
        }



        /// <summary>Get root (including non element) nodes that are not under the main root node.  This is intended to fetch xml declarations eg [?xml version="1.0" encoding="UTF-8"?] and xml processing instructions eg [?mso-application progid="Word.Document"?].  However it can handle a second standard element (XmlNodeType.element).  If the node is not found then it will be created.  Note that declaration and processing instructions do not have attributes, use value to modify / read their contents.</summary>
        /// <param name="name">Specifies the name of the node.</param>
        /// <param name="nodeType">Specifeis the type of the node.</param>
        /// <returns>The requested node at the top level of the document.</returns>
        public XmlNode getSpecialNode(string name, System.Xml.XmlNodeType nodeType)
        {
            XmlNode findNode = findSpecialNode(name, nodeType);
            if (findNode != null)
            {
                return findNode;
            }

            // No matching node found so create the node.
            System.Xml.XmlNode newNode = document_.CreateNode(nodeType, name, document_.DocumentElement.NamespaceURI);

            System.Xml.XmlNode beforeNode = null;
            switch (nodeType)
            {
            case System.Xml.XmlNodeType.XmlDeclaration:
                // Add the declarations before the processing instructions.
                foreach (System.Xml.XmlNode xmlNode in document_.ChildNodes)
                {
                    if (xmlNode.NodeType != System.Xml.XmlNodeType.XmlDeclaration)
                    {
                        beforeNode = xmlNode;
                        break;
                    }
                }
                break;

            case System.Xml.XmlNodeType.ProcessingInstruction:
                // Add the processing instructions before the real nodes.
                foreach (System.Xml.XmlNode xmlNode in document_.ChildNodes)
                {
                    if (xmlNode.NodeType != System.Xml.XmlNodeType.XmlDeclaration && xmlNode.NodeType != System.Xml.XmlNodeType.ProcessingInstruction)
                    {
                        beforeNode = xmlNode;
                        break;
                    }
                }
                break;

            }

            // Insert the new node at the specified position.
            if (beforeNode == null)
            {
                newNode = document_.AppendChild(newNode);
            }
            else
            {
                try
                {
                    newNode = document_.InsertBefore(newNode, beforeNode);
                }
                catch
                {
                    newNode = document_.AppendChild(newNode);
                }
            }

            // Mark the document as changed.
            isChanged_ = true;

            // Return this new node.
            return new XmlNode(newNode, this);
        }



        /// <summary>Get root (including non element) nodes that are not under the main root node.  This is intended to fetch xml declarations eg [?xml version="1.0" encoding="UTF-8"?] and xml processing instructions eg [?mso-application progid="Word.Document"?].  However it can handle a second standard element (XmlNodeType.element).  If the node is not found then null is returned.  Note that declaration and processing instructions do not have attributes, use value to modify / read their contents.</summary>
        /// <param name="name">Specifies the name of the node.</param>
        /// <param name="nodeType">Specifies the type of the node.</param>
        /// <returns>The requested node at the top level of the document or null.</returns>
        public XmlNode findSpecialNode(string name, System.Xml.XmlNodeType nodeType)
        {
            // Check of the node is forced to be lower case.
            if (isForceLowerCase_)
            {
                name = name.ToLower();
            }

            // Search for the node.
            if (isCaseSensitive_)
            {
                foreach (System.Xml.XmlNode xmlNode in document_.ChildNodes)
                {
                    if (xmlNode.Name == name && xmlNode.NodeType == nodeType)
                    {
                        return new XmlNode(xmlNode, this);
                    }
                }
            }
            else
            {
                foreach (System.Xml.XmlNode xmlNode in document_.ChildNodes)
                {

                    if (xmlNode.Name.ToLower() == name.ToLower() && xmlNode.NodeType == nodeType)
                    {
                        return new XmlNode(xmlNode, this);
                    }
                }
            }

            // No matching node found so return null.
            return null;
        }



        /// <summary>Get the root (including non element) node that are at the top level of the document.  This is intended to fetch xml declarations eg [?xml version="1.0" encoding="UTF-8"?] and xml processing instructions eg [?mso-application progid="Word.Document"?].  However it can handle a second standard element (XmlNodeType.element).  If the node is not found then null is returned.  Note that declaration and processing instructions do not have attributes, use value to modify / read their contents.</summary>
        /// <param name="index">Specifies the index of the node required.</param>
        /// <returns>The requested node at the top level of the document or null.</returns>
        public XmlNode findSpecialNode(int index)
        {
            // There is no node with this index.
            if (index > document_.ChildNodes.Count)
            {
                return null;
            }

            // Return this node.
            return new XmlNode(document_.ChildNodes[index], this);
        }



        /// <summary>Returns a valid node name from the specified node name.  Replaces spaces with _.  Replaces # with Hash.  Does not allow nodes to start with a numeric character.</summary>
        /// <param name="name"></param>
        /// <returns></returns>
        static public string validNodeName(string name)
        {
            // Create a string builder to hold the result.
            StringBuilder sbResult = new StringBuilder(name.ToLower());
            if (name.Length == 0)
            {
                sbResult.Append("empty");
            }

            // Replace some bad characters
            sbResult.Replace(" ", "_");
            sbResult.Replace(".", "dot");
            sbResult.Replace(",", "comma");
            sbResult.Replace("#", "hash");
            sbResult.Replace("(", "open");
            sbResult.Replace(")", "close");
            sbResult.Replace("'", "quote");
            sbResult.Replace("+", "plus");
            sbResult.Replace("&", "and");
            sbResult.Replace("\\", "backslash");
            sbResult.Replace("/", "forwardslash");
            sbResult.Replace("%", "percent");
            sbResult.Replace("~", "tilde");
            sbResult.Replace("[", "open");
            sbResult.Replace("]", "close");

            // Check if the first character is numeric.
            if (sbResult[0] >= '0' && sbResult[0] <= '9')
            {
                sbResult.Insert(0, "digit");
            }

            return sbResult.ToString();
        }



        #endregion

        #region Public Properties



        /// <summary>The acutal XmlDocument that this document represents.</summary>
        public System.Xml.XmlDocument document
        {
            get { return document_; }
        }



        /// <summary>True when the document has changed since the last write to disk.</summary>
        public bool isChanged
        {
            get { return isChanged_; }
            set { isChanged_ = value; }
        }



        /// <summary>True then keys and attribute names will be taken as lower case regardless of the case of the string specified.</summary>
        public bool isForceLowerCase
        {
            get { return isForceLowerCase_; }
            set { isForceLowerCase_ = value; }
        }



        /// <summary>True to force keys and attribute names to match case.  False to ignore upper and lower case mismatches.</summary>
        public bool isCaseSensitive
        {
            get { return isCaseSensitive_; }
            set { isCaseSensitive_ = value; }
        }



        /// <summary>The source of the Xml document.</summary>
        public Source mode
        {
            get { return mode_; }
            set { mode_ = value; }
        }



        #endregion
    }



    /// <summary>Class to represent a node in a XmlDocument.  This supersedes the 'clsXmlNode' class.</summary>
    // This class can not be renamed as XmlNode easily because this name is used the System.Xml namespace.       
    public class XmlNode
    {
        #region Member Variables

        /// <summary>The actual node that this object represents.</summary>
        /// <remarks>Shame that inheriting from XmlNode covers some important members.</remarks>
        private System.Xml.XmlNode xmlNode_;

        /// <summary>The document that contains this node.</summary>
        private XmlDocument document_;

        #endregion

        #region Constructors etc ...



        /// <summary>Class Constructor.</summary>
        /// <param name="xmlNode">The actual node to use.</param>
        /// <param name="document">The document to attach this node to.</param>
        public XmlNode(System.Xml.XmlNode xmlNode, XmlDocument document)
        {
            xmlNode_ = xmlNode;
            document_ = document;
        }



        #endregion

        #region Properties



        /// <summary>The name of this node.</summary>
        public string name
        {
            get { return xmlNode_.Name; }
        }



        /// <summary>The acutal XmlNode that this object represents.</summary>
        /// <remarks>This can be used to corrupt the document.</remarks>
        public System.Xml.XmlNode xmlNode
        {
            get { return xmlNode_; }
        }



        /// <summary>The xml document that this node is attached to.</summary>
        public XmlDocument document
        {
            get { return document_; }
        }



        /// <summary>True if the node is a comment.</summary>
        public bool isComment
        {
            get { return xmlNode_.NodeType == System.Xml.XmlNodeType.Comment; }
        }



        #endregion

        #region Nodes



        /// <summary>Returns the specified child node of this node.  Creates the node if it does not exist.  Child nodes can be specified using the '/' character.</summary>
        /// <param name="name">Specifies the name of the required node.  Use the '/' character to specify parent and child nodes.</param>
        /// <returns>A XmlNode with the specified name under this node.</returns>
        public XmlNode getNode(string name)
        {
            // Find the requested node.
            XmlNode node = findNode(name);

            // Create the node if not in the document.
            if (node == null)
            {
                // Check for the name case
                if (document_.isForceLowerCase)
                {
                    name = name.ToLower();
                }

                document_.isChanged = true;
                System.Xml.XmlNode newNode = document_.document.CreateNode(System.Xml.XmlNodeType.Element, name, document_.document.DocumentElement.NamespaceURI);
                int index = insertNode(newNode);
                node = new XmlNode(xmlNode_.ChildNodes[index], document_);
            }

            // Return the node.
            return node;
        }



        /// <summary>Returns the specified child node of this node.  Does not create the node if does not exist.  The index is zero based.</summary>
        /// <param name="index">Specifies the zero based index of the node.</param>
        /// <returns>A clsXmlNode under this node or null.</returns>
        public XmlNode getNode(int index)
        {
            if (index >= 0 && index < xmlNode_.ChildNodes.Count)
            {
                XmlNode node = new XmlNode(xmlNode_.ChildNodes[index], document_);
                return node;
            }
            return null;
        }



        /// <summary>Returns the specified child node of this node.  Returns null if the specified node does not exist.  Child nodes can be specified using the '/' character.</summary>
        /// <param name="name">Specifies the name of the required node.  Use the '/' character to specify parent and child nodes.</param>
        /// <returns>A clsXmlNode with the specified name under this node or Nothing if the specified node does not exist.</returns>
        public XmlNode findNode(string name)
        {
            // Check for the name case.
            if (document_.isForceLowerCase)
            {
                name = name.ToLower();
            }

            // Check for compound names.
            if (name.Contains("/"))
            {
                int split = name.LastIndexOf("/");
                string parentName = name.Substring(0, split);
                name = name.Substring(split + 1);
                XmlNode parentNode = getNode(parentName);
                return parentNode.getNode(name);
            }

            // Search for a node that matches the name.
            foreach (System.Xml.XmlNode xmlNode in xmlNode_.ChildNodes)
            {
                if (xmlNode.Name == name)
                {
                    XmlNode childNode = new XmlNode(xmlNode, document_);
                    return childNode;
                }
            }

            // Search in a non case senstive way.
            if (!document_.isCaseSensitive)
            {
                name = name.ToLower();
                foreach (System.Xml.XmlNode xmlNode in xmlNode_.ChildNodes)
                {
                    if (xmlNode.Name.ToLower() == name)
                    {
                        XmlNode childNode = new XmlNode(xmlNode, document_);
                        return childNode;
                    }
                }
            }

            // No matching node.
            return null;
        }



        /// <summary>Returns the specified child node of this node.  Returns null if the specified node does not exist.</summary>
        /// <param name="index">Specifies the zero based index of the node.</param>
        /// <returns>A clsXmlNode under this node or null.</returns>
        public XmlNode findNode(int index)
        {
            return getNode(index);
        }



        /// <summary>Returns the number of nodes under this node.</summary>
        /// <returns>The number of nodes under this node.</returns>
        public int getNumNodes()
        {
            return xmlNode_.ChildNodes.Count;
        }



        /// <summary>Add a new node under this node with the specified name.  The newly created node is returned.</summary>
        /// <param name="name">Specify a name for the node.</param>
        /// <returns>The node that is created.</returns>
        /// <remarks></remarks>
        public XmlNode addNode(string name)
        {
            document_.isChanged = true;
            System.Xml.XmlNode newXmlNode = document_.document.CreateNode(System.Xml.XmlNodeType.Element, name, document_.document.DocumentElement.NamespaceURI);
            int index = insertNode(newXmlNode);
            XmlNode newNode = new XmlNode(xmlNode_.ChildNodes[index], document_);
            return newNode;
        }



        /// <summary>Deletes the current node and all the attributes that are attached to it.  The Xml file needs saving afterwards otherwise all changes will not be recorded.</summary>
        /// <returns>True if the current node is deleted.  False, otherwise.</returns>
        public bool delete()
        {
            xmlNode_.ParentNode.RemoveChild(xmlNode_);
            document_.isChanged = true;
            return true;
        }



        /// <summary>Delete all the child nodes of this node.</summary>
        /// <returns>True for success, false otherwise.</returns>
        public bool deleteChildNodes()
        {
            while (xmlNode_.ChildNodes.Count > 0)
            {
                xmlNode_.RemoveChild(xmlNode_.ChildNodes[0]);
            }
            return true;
        }



        /// <summary>Delete the specified child node, if it exists.</summary>
        /// <param name="nodeName">Specifies the name of the child node to delete.</param>
        /// <returns>True if a node is deleted, false if the node did not exist.</returns>
        public bool deleteChildNode(string nodeName)
        {
            XmlNode xmlNode = findNode(nodeName);
            if (xmlNode == null)
            {
                return false;
            }
            xmlNode.delete();
            return true;
        }



        /// <summary>Inserts the specified node inside the this node in alphabetical order.</summary>
        /// <param name="xmlNode">Specifies the child node to insert into the parent node.</param>
        /// <returns>The index of the new node, or -1 for failure</returns>
        public int insertNode(System.Xml.XmlNode xmlNode)
        {
            // Find the node to store the new node after.
            System.Xml.XmlNode afterMe = null;
            int index = 0;
            if (xmlNode_.ChildNodes.Count > 0)
            {
                if (xmlNode.Name.CompareTo(xmlNode_.ChildNodes[0].Name) < 0)
                {
                    // Do nothing we want the new node at the start of the list.
                }
                else
                {
                    while (xmlNode.Name.CompareTo(xmlNode_.ChildNodes[index].Name) >= 0)
                    {
                        index++;
                        if (index == xmlNode_.ChildNodes.Count)
                        {
                            break;
                        }
                    }
                    afterMe = xmlNode_.ChildNodes[index - 1];
                }
            }

            // Save the new node inside the parent node.
            xmlNode_.InsertAfter(xmlNode, afterMe);

            // Return success.
            return index;
        }



        /// <summary>Renames the current node with the new name as specified.  Actually copies the existing node to the new name and deletes the existing node.  The reference to the node is lost after this operation, use GetNode again.</summary>
        /// <param name="newName">Specifies the new name for the active node.</param>
        /// <returns>True for success.  False, otherwise.</returns>
        public bool rename(string newName)
        {
            // Check if lower case is enforced.
            if (document_.isForceLowerCase)
            {
                newName = newName.ToLower();
            }

            // Create a new node to copy the values into.
            XmlNode newNode = parent.getNode(newName);

            // Copy the attributes from xmlNode to newNode.
            for (int i = 0; i < xmlNode_.Attributes.Count; i++)
            {
                System.Xml.XmlAttribute xmlAttribute = document_.document.CreateAttribute(xmlNode_.Attributes[i].Name);
                xmlAttribute.Value = xmlNode_.Attributes[i].Value;
                newNode.xmlNode_.Attributes.InsertAfter(xmlAttribute, null);
            }

            // Copy the subnodes (this deletes them from the xmlNode).
            while (xmlNode_.ChildNodes.Count > 0)
            {
                newNode.xmlNode_.AppendChild(xmlNode_.ChildNodes[0]);
            }

            // Delete the original node.
            delete();

            document_.isChanged = true;
            return true;

            // The current node could not be renamed
            // return false;
        }



        /// <summary>The parent clsXmlNode to this node.</summary>
        public XmlNode parent
        {
            get { return new XmlNode(xmlNode_.ParentNode, document_); }
        }



        /// <summary>The content of this node.</summary>
        public string innerText
        {
            get { return xmlNode_.InnerText; }
            set { xmlNode_.InnerText = value; }
        }



        /// <summary>Returns the inner text of this node as a string.  Can use InnerText instead, hard to imagine when this would raise and error and return the default.</summary>
        /// <param name="defaultText">The value of return in the case of an error.</param>
        /// <returns>The inner text of this node.</returns>
        public string getInnerText(string defaultText)
        {
            try
            {
                return xmlNode_.InnerText;
            }
            catch
            {
                return defaultText;
            }
        }



        /// <summary>Returns the inner text of this node as an integer.</summary>
        /// <param name="defaultInteger">The value to return in the case of an error.</param>
        /// <returns>The inner text of this node as an integer.</returns>
        public int getInnerText(int defaultInteger)
        {
            try
            {
                return int.Parse(xmlNode_.InnerText, CultureInfo.InvariantCulture.NumberFormat);
            }
            catch
            {
                return defaultInteger;
            }
        }



        /// <summary>Returns the inner text of this node as a date time.</summary>
        /// <param name="defaultDateTime">The value to return in the case of an error.</param>
        /// <returns>The inner text of this node as a date time.</returns>
        public DateTime getInnerText(DateTime defaultDateTime)
        {
            try
            {
                return DateTime.ParseExact(xmlNode_.InnerText, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                return defaultDateTime;
            }
        }



        #endregion

        #region Attributes



        /// <summary>Returns the attribute with the specified name.  If the attribute does not exist then it is either created or null is returned.</summary>
        /// <param name="name">Specifies the name of the attribute.</param>
        /// <param name="isCreate">Specifies the action to take if the attribute does not exist.  True to create the attribute, false to return null.</param>
        /// <returns>Returns the named attribute or null.</returns>
        private System.Xml.XmlAttribute getAttribute(string name, bool isCreate)
        {
            // Force the attribute to have a lower case name.
            if (document_.isForceLowerCase)
            {
                name = name.ToLower();
            }

            // Don't really expect this.
            if (xmlNode_.Attributes == null)
            {
                return null;
            }

            // Search for a node that matches the name.
            if (document_.isCaseSensitive)
            {
                foreach (System.Xml.XmlAttribute xmlAttribute in xmlNode_.Attributes)
                {
                    if (xmlAttribute.Name == name)
                    {
                        return xmlAttribute;
                    }
                }
            }
            else
            {
                // Search in a non case senstive way.
                string nameToLower = name.ToLower();
                foreach (System.Xml.XmlAttribute xmlAttribute in xmlNode_.Attributes)
                {
                    if (xmlAttribute.Name.ToLower() == nameToLower)
                    {
                        return xmlAttribute;
                    }
                }
            }

            // Return no matching attibute.
            if (!isCreate)
            {
                return null;
            }

            // Create an attribute with this name.
            document_.isChanged = true;
            System.Xml.XmlAttribute newAttribute = document_.document.CreateAttribute(name);

            System.Xml.XmlAttribute afterMe = null;
            if (xmlNode_.Attributes.Count > 0)
            {
                if (newAttribute.Name.CompareTo(xmlNode_.Attributes[0].Name) < 0)
                {
                    // Do nothing we want the new node at the start of the list
                }
                else
                {
                    int index = 0;
                    while (newAttribute.Name.CompareTo(xmlNode_.Attributes[index].Name) > 0)
                    {
                        index++;
                        if (index == xmlNode_.Attributes.Count)
                        {
                            break;
                        }
                    }
                    index -= 1;
                    afterMe = xmlNode_.Attributes[index];
                }
            }

            // Save the new attribute on the current node
            xmlNode_.Attributes.InsertAfter(newAttribute, afterMe);

            return newAttribute;
        }



        /// <summary>Set the specified attribute with the string value specified.</summary>
        /// <overloads>Sets the specified attribute with the value specified.</overloads>
        /// <param name="attributeName">Specifies the name of the attribute.</param>
        /// <param name="value">Specifies the value of the attribute.</param>
        public void setAttributeValue(string attributeName, string value)
        {
            System.Xml.XmlAttribute attribute = getAttribute(attributeName, true);
            if (attribute != null)
            {
                attribute.Value = value;
                document_.isChanged = true;
            }
        }



        /// <summary>Sets the specified attribute with the specified string value unless the specified value is the default.  In this case, the attribute is removed.</summary>
        /// <param name="attribute">Specifies the name of the attribute.</param>
        /// <param name="value">Specifies the value of the attribute.</param>
        /// <param name="defaultValue">Specifies the default value of the attribute.</param>
        public void SetAttributeValue(string attribute, string value, string defaultValue)
        {
            if (value == defaultValue)
            {
                // Remove the attribute.
                deleteAttribute(attribute);
            }
            else
            {
                // Store the non default attribute value.
                setAttributeValue(attribute, value);
            }
        }



        /// <summary>Sets the specified attribute with the integer value specified.</summary>
        /// <param name="attribute">Specifies the name of the attribute.</param>
        /// <param name="value">Specifies the value of the attribute.</param>
        public void setAttributeValue(string attribute, int value)
        {
            setAttributeValue(attribute, value.ToString(CultureInfo.InvariantCulture.NumberFormat));
        }



        /// <summary>Sets the specified attribute with the specified integer value unless the specified value is the default.  In this case, the attribute is removed.</summary>
        /// <param name="attribute">Specifies the name of the attribute.</param>
        /// <param name="value">Specifies the value of the attribute.</param>
        /// <param name="defaultValue">Specifies the default value of the attribute.</param>
        public void setAttributeValue(string attribute, int value, int defaultValue)
        {
            if (value == defaultValue)
            {
                // Remove the attribute.
                deleteAttribute(attribute);
            }
            else
            {
                // Store the non default attribute value.
                setAttributeValue(attribute, value);
            }
        }



        /// <summary>Sets the specified attribute with the double value specified.</summary>
        /// <param name="attribute">Specifies the name of the attribute.</param>
        /// <param name="value">Specifies the value of the attribute.</param>
        public void setAttributeValue(string attribute, double value)
        {
            setAttributeValue(attribute, value.ToString(CultureInfo.InvariantCulture.NumberFormat));
        }



        /// <summary>Sets the specified attribute with the specified double value unless the specified value is the default.  In this case, the attribute is removed.</summary>
        /// <param name="attribute">Specifies the name of the attribute.</param>
        /// <param name="value">Specifies the value of the attribute.</param>
        /// <param name="defaultValue">Specifies the default value of the attribute.</param>
        public void setAttributeValue(string attribute, double value, double defaultValue)
        {
            if (value == defaultValue)
            {
                // Remove the attribute.
                deleteAttribute(attribute);
            }
            else
            {
                // Store the non default attribute value.
                setAttributeValue(attribute, value);
            }
        }



        /// <summary>Sets the specified attribute with the single value specified.</summary>
        /// <param name="attribute">Specifies the name of the attribute.</param>
        /// <param name="value">Specifies the value of the attribute.</param>
        public void setAttributeValue(string attribute, float value)
        {
            setAttributeValue(attribute, value.ToString(CultureInfo.InvariantCulture.NumberFormat));
        }



        /// <summary>Sets the specified attribute with the specified value unless the specified value is the default.  In this case, the attribute is removed.</summary>
        /// <param name="attribute">Specifies the name of the attribute.</param>
        /// <param name="value">Specifies the value of the attribute.</param>
        /// <param name="defaultValue">Specifies the default value of the attribute.</param>
        public void setAttributeValue(string attribute, float value, float defaultValue)
        {
            if (value == defaultValue)
            {
                // Remove the attribute.
                deleteAttribute(attribute);
            }
            else
            {
                // Store the non default attribute value.
                setAttributeValue(attribute, value);
            }
        }



        /// <summary>Sets the specified attribute with the boolean value specified.</summary>
        /// <param name="attribute">Specifies the name of the attribute.</param>
        /// <param name="isValue">Specifies the value of the attribute.</param>
        public void setAttributeValue(string attribute, bool isValue)
        {
            if (isValue)
            {
                setAttributeValue(attribute, "True");
            }
            else
            {
                setAttributeValue(attribute, "False");
            }
        }



        /// <summary>Sets the specified attribute in the current node with the DateTime value specified.</summary>
        /// <param name="attribute">Specifies the name of the attribute.</param>
        /// <param name="value">Specifies the value of the attribute.</param>
        public void setAttributeValue(string attribute, DateTime value)
        {
            setAttributeValue(attribute, value.ToString("dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture));
        }



        /// <summary>Returns the specified attribute value as a string.</summary>
        /// <param name="attributeName">Specifies the name of the attribute.</param>
        /// <param name="defaultValue">Specifies the default value of the attribute.</param>
        /// <param name="isCreate">Specifies to create the attribute if it does not exist.</param>
        /// <returns>The value of the specified attribute or the default value.</returns>
        public string getAttributeValue(string attributeName, string defaultValue, bool isCreate)
        {
            System.Xml.XmlAttribute attribute = getAttribute(attributeName, false);
            if (attribute == null)
            {
                if (isCreate)
                {
                    setAttributeValue(attributeName, defaultValue);
                }

                // Return the default value.
                return defaultValue;
            }

            // Return the value found.
            return attribute.Value;
        }



        /// <summary>Returns the specified attribute value as a integer.</summary>
        /// <param name="attribute">Specifies the name of the attribute.</param>
        /// <param name="defaultValue">Specifies the default value of the attribute.</param>
        /// <param name="isCreate">Specifies to create the attribute if it does not exist.</param>
        /// <returns>The value of the attribute or the default value.</returns>
        public int getAttributeValue(string attribute, int defaultValue, bool isCreate)
        {
            string text = getAttributeValue(attribute, defaultValue.ToString(), isCreate);
            try
            {
                return int.Parse(text, CultureInfo.InvariantCulture.NumberFormat);
            }
            catch
            {
                return defaultValue;
            }
        }



        /// <summary>Returns the specified attribute value as a integer.  If the specified attribute name is missing then the alternative attribute name is tried.  This is intended to be used for short periods when an attribute name changes between versions of the xml file.</summary>
        /// <param name="attributeName">Specifies the name of the attribute.</param>
        /// <param name="alternateAttributeName">Specifies an alternative name for the attribute.</param>
        /// <param name="defaultValue">Specifies the default value of the attribute.</param>
        /// <param name="isCreate">Specifies to create the attribute if it does not exist.</param>
        /// <returns>The value of the attribute or the default value.</returns>
        public int getAttributeValue(string attributeName, string alternateAttributeName, int defaultValue, bool isCreate)
        {
            // Find the attribute or alternate attribute
            System.Xml.XmlAttribute attribute = getAttribute(attributeName, false);
            if (attribute == null)
            {
                attribute = getAttribute(alternateAttributeName, false);
            }

            // Return the default value if no attribute is found.
            if (attribute == null)
            {
                if (isCreate)
                {
                    setAttributeValue(attributeName, defaultValue);
                }
                return defaultValue;
            }

            // Return the attribute value as an integer.
            try
            {
                return int.Parse(attribute.Value, CultureInfo.InvariantCulture.NumberFormat);
            }
            catch
            {
                return defaultValue;
            }
        }



        /// <summary>Returns the specified attribute value as a double.</summary>
        /// <param name="attribute">Specifies the name of the attribute.</param>
        /// <param name="defaultValue">Specifies the default value of the attribute.</param>
        /// <param name="isCreate">Specifies to create the attribute if it does not exist.</param>
        /// <returns>The value of the attribute or the default value.</returns>
        public double getAttributeValue(string attribute, double defaultValue, bool isCreate)
        {
            string text = getAttributeValue(attribute, defaultValue.ToString(CultureInfo.InvariantCulture.NumberFormat), isCreate);
            try
            {
                return double.Parse(text, CultureInfo.InvariantCulture.NumberFormat);
            }
            catch
            {
                return defaultValue;
            }
        }



        /// <summary>Returns the specified attribute value as a single.</summary>
        /// <param name="attribute">Specifies the name of the attribute.</param>
        /// <param name="defaultValue">Specifies the default value of the attribute.</param>
        /// <param name="isCreate">Specifies to create the attribute if it does not exist.</param>
        /// <returns>The value of the attribute or the default value.</returns>
        public float getAttributeValue(string attribute, float defaultValue, bool isCreate)
        {
            string text = getAttributeValue(attribute, defaultValue.ToString(CultureInfo.InvariantCulture.NumberFormat), isCreate);
            try
            {
                return float.Parse(text, CultureInfo.InvariantCulture.NumberFormat);
            }
            catch
            {
                return defaultValue;
            }
        }



        /// <summary>Returns the specified attribute value as a double.  If the specified attribute name is missing then the alternative attribute name is tried.  This is intended to be used for short periods when an attribute name changes between versions of the xml file.  This was originally working with float not double.</summary>
        /// <param name="attributeName">Specifies the name of the attribute.</param>
        /// <param name="alternateAttributeName">Specifies an alternative name for the attribute.</param>
        /// <param name="defaultValue">Specifies the default value of the attribute.</param>
        /// <param name="isCreate">Specifies to create the attribute if it does not exist.</param>
        /// <returns>The value of the attribute or the default value.</returns>
        public double getAttributeValue(string attributeName, string alternateAttributeName, double defaultValue, bool isCreate)
        {
            // Find the attribute or alternate attribute
            System.Xml.XmlAttribute attribute = getAttribute(attributeName, false);
            if (attribute == null)
            {
                attribute = getAttribute(alternateAttributeName, false);
            }

            // Return the default value if no attribute is found.
            if (attribute == null)
            {
                if (isCreate)
                {
                    setAttributeValue(attributeName, defaultValue);
                }
                return defaultValue;
            }

            // Return the attribute value as a float
            try
            {
                return double.Parse(attribute.Value, CultureInfo.InvariantCulture.NumberFormat);
            }
            catch
            {
                return defaultValue;
            }
        }



        /// <summary>Returns the specified attribute value as a boolean.</summary>
        /// <param name="attribute">Specifies the name of the attribute.</param>
        /// <param name="isDefault">Specifies the default value of the attribute.</param>
        /// <param name="isCreate">Specifies to create the attribute if it does not exist.</param>
        /// <returns>The value of the attribute or the default value.</returns>
        public bool getAttributeValue(string attribute, bool isDefault, bool isCreate)
        {
            string text;
            if (isDefault)
            {
                text = getAttributeValue(attribute, "True", isCreate);
            }
            else
            {
                text = getAttributeValue(attribute, "False", isCreate);
            }
            if (text == "True")
            {
                return true;
            }
            return false;
        }



        /// <summary>Returns the specified attribute value as a datetime.</summary>
        /// <param name="attribute">Specifies the name of the attribute.</param>
        /// <param name="defaultValue">Specifies the default value of the attribute.</param>
        /// <param name="isCreate">Specifies true to create the attribute if it does not exist.</param>
        /// <returns>The value of the attribute or the default value.</returns>
        public DateTime getAttributeValue(string attribute, DateTime defaultValue, bool isCreate)
        {
            string text = getAttributeValue(attribute, defaultValue.ToString("dd/MM/yyyy HH:mm:ss"), isCreate);
            try
            {
                return DateTime.ParseExact(text, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                return defaultValue;
            }
        }



        /// <summary>Returns the list of attributes present on this node as an array of string names.</summary>
        /// <returns>The list of attributes present on this node as an array of string names.</returns>
        public string[] listAttributes()
        {
            ArrayList attributes = new ArrayList();
            foreach (System.Xml.XmlAttribute xmlAttribute in xmlNode_.Attributes)
            {
                attributes.Add(xmlAttribute.Name);
            }

            // Return the list of attributes as an array of string
            return (string[])attributes.ToArray(typeof(string));
        }



        /// <summary>Deletes the specified attribute.  The attribute name is always case senstive.</summary>
        /// <param name="name">Specifies the name of the attribute to remove.</param>
        /// <returns>True if an attribute was removed, false otherwise.</returns>
        public bool deleteAttribute(string name)
        {
            foreach (System.Xml.XmlAttribute xmlAttribute in xmlNode_.Attributes)
            {
                if (xmlAttribute.Name == name)
                {
                    xmlNode_.Attributes.Remove(xmlAttribute);
                    document_.isChanged = true;
                    return true;
                }
            }
            return false;
        }



        /// <summary>Returns true if the specified attribute exists on this node, false otherwise.</summary>
        /// <param name="attributeName">Specifies the name of the required attribute.</param>
        /// <returns>True if the specified attribute exists on this node, false otherwise.</returns>
        public bool isAttributeExists(string attributeName)
        {
            System.Xml.XmlAttribute xmlAttribute = getAttribute(attributeName, false);
            if (xmlAttribute == null)
            {
                return false;
            }
            return true;
        }



        #region Password



        /// <summary>Encode the specified string.</summary>
        /// <remarks>This is not a strong code, it just means that the config file can not be read immediately.</remarks>
        /// <param name="value">Specifies the value to encode.</param>
        /// <returns>The encoded equalivant value of the specified value.</returns>
        private string passwordEncode(string value)
        {
            StringBuilder sbEncoded = new StringBuilder();
            sbEncoded.Append("@");
            if (value != null)
            {
                for (int i = 0; i < value.Length; i++)
                {
                    int character = (int)value[i];
                    character = character ^ 255;
                    sbEncoded.Append(character.ToString("000"));
                }
            }

            // Return the encoded string.
            return sbEncoded.ToString();
        }



        /// <summary>Decode the specified string.</summary>
        /// <remarks>This is not a strong code, it just means that the config file can not be read immediately.</remarks>
        /// <param name="value">Specifies the value to decode.</param>
        /// <returns>The decoded equalivant value of the specified string.</returns>
        private string passwordDecode(string value)
        {
            // Check that the value is encoded.
            if (value[0] != '@')
            {
                return value;
            }

            StringBuilder sbDecoded = new StringBuilder();

            for (int i = 0; i < value.Length / 3; i++)
            {
                string encodedValue = value.Substring(1 + i * 3, 3);
                int character = int.Parse(encodedValue);
                character = character ^ 255;
                sbDecoded.Append((char)character);
            }

            // Return the decoded string.
            return sbDecoded.ToString();
        }



        /// <summary>Returns the value of the specified attribute decoded from the non-human readable form in the config file.</summary>
        /// <param name="attribute">Specifies the name of the attribute.</param>
        /// <param name="defaultValue">Specifies the default value for the attribute.</param>
        /// <returns>The value of the specified attribute.</returns>
        public string getAttributePassword(string attribute, string defaultValue)
        {
            // Get the value of the attribute.
            string value = getAttributeValue(attribute, defaultValue, false);

            // Decode the attribute value.
            if (value.Length > 0)
            {
                if (value[0] != '@')
                {
                    setAttributePassword(attribute, value);
                }
                else
                {
                    value = passwordDecode(value);
                }
            }

            // Return the decoded value.
            return value;
        }



        /// <summary>Sets the specified attribute in the current node with the string value specified.  The value is decoded to not be directly human readable.</summary>
        /// <param name="attribute">Specifies the name of the attribute.</param>
        /// <param name="value">Specifies the value of the attribute.</param>
        public void setAttributePassword(string attribute, string value)
        {
            setAttributeValue(attribute, passwordEncode(value));
        }



        #endregion

        #endregion
    }
}

