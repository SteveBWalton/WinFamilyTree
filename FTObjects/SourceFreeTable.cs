using System;
using System.Collections.Generic;
using System.Text;

// ArrayList.
using System.Collections;

// Database.
using System.Data;
using System.Data.OleDb;


namespace FamilyTree.Objects
{
    /// <summary>Class to represent the optional free table additional information for a source.</summary>
    public class SourceFreeTable
    {
        #region Member Variables

        /// <summary>The source that contains this addional free table.</summary>
        private Source source_;

        /// <summary>The rows in this source free table.</summary>
        private ArrayList rows_;

        #endregion

        #region Constructors

        /// <summary>Class constructor.</summary>
        public SourceFreeTable(Source source)
        {
            // Store the parameters.
            source_ = source;
            rows_ = new ArrayList();
            
            // Fetch any existing rows in the database.
            string sql = "SELECT LABEL, FREE_TEXT FROM SOURCES_FREE_TABLE_ROWS WHERE SOURCE_ID = " + source.index.ToString() + " ORDER BY ROW;";
            OleDbCommand sqlCommand = new OleDbCommand(sql, source.database.cndb);
            OleDbDataReader dataReader = sqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                SourceFreeTableRow row = new SourceFreeTableRow(this, Database.getString(dataReader, "LABEL", "Error"), Database.getString(dataReader, "FREE_TEXT", "Error"));
                addRow(row);
            }
            dataReader.Close();
        }

        #endregion

        public bool addRow(SourceFreeTableRow row)
        {
            rows_.Add(row);
            return true;
        }



        public bool addRow(string label, string freeText)
        {
            SourceFreeTableRow newRow = new SourceFreeTableRow(this, label, freeText);
            return addRow(newRow);
        }



        public SourceFreeTableRow[] getRows()
        {
            return (SourceFreeTableRow[])rows_.ToArray(typeof(SourceFreeTableRow));
        }



        public string toHtml()
        {
            StringBuilder html = new StringBuilder();
            html.AppendLine("<table style=\"margin-left: 50px; border: 1pt solid black; background-color: lightgray;\">");
            SourceFreeTableRow[] rows = getRows();
            foreach(SourceFreeTableRow row in rows)
            {
                html.AppendLine(row.toHtml());
            }
            html.AppendLine("</table>");

            // Return the built html.
            return html.ToString();
        }



        #region Properties

        /// <summary>The ID of this source free table.  It is also the ID of the parent source object.</summary>
        public int index { get { return source_.index; } }

        #endregion
    }


    /// <summary>Class to represent a row in an source free table.</summary>
    public class SourceFreeTableRow
    {
        #region Member Variables

        /// <summary>The parent source free table object.</summary>
        private SourceFreeTable sourceFreeTable_;

        /// <summary>The label for the row of free text.  [Think of a better variable name.]</summary>
        public string labelText_;

        /// <summary>The text for the row of free text.  [Think of a better variable name.]</summary>
        public string freeText_;

        #endregion

        #region Constructors



        public SourceFreeTableRow(SourceFreeTable sourceFreeTable, string label, string freeText)
        {
            sourceFreeTable_ = sourceFreeTable;
            this.labelText_ = label;
            this.freeText_ = freeText;
        }



        #endregion



        public string toHtml()
        {
            StringBuilder html = new StringBuilder();
            html.Append("<tr><td style=\"font-family: 'Times New Roman'; font-size: 8pt; color: grey;\">");
            html.Append(labelText_);
            html.Append("</td><td>");
            html.Append(freeText_);
            html.Append("</td></tr>");

            // Return the built html.
            return html.ToString();
        }



        #region Properties

        /// <summary>The ID of the parent source free table and the parent source object.</summary>
        public int sourceIndex { get { return sourceFreeTable_.index; } }

        /// <summary>The label for the row of free text.</summary>
        public string labelText { get { return labelText_; } set { labelText_ = value; } }
 

        /// <summary>The text for the row of free text.</summary>
        public string freeText { get { return freeText_; } set { freeText_ = value; } }


        #endregion
    }
}
