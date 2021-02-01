using System;
using System.Collections.Generic;
using System.Text;

// ArrayList.
using System.Collections;

// Database.
using System.Data;
using System.Data.OleDb;


namespace family_tree.objects
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



        /// <summary>Write this free table to the database.</summary>
        public void save()
        {
            // Remove the existing rows.
            string sql = "DELETE FROM SOURCES_FREE_TABLE_ROWS WHERE SOURCE_ID = " + source_.index.ToString() + ";";
            OleDbCommand sqlCommand = new OleDbCommand(sql, source_.database.cndb);
            sqlCommand.ExecuteNonQuery();

            // Add the current rows in the collection.
            SourceFreeTableRow[] rows = getRows();
            for (int i = 0; i < rows.Length; i++)
            {
                sql = "INSERT INTO SOURCES_FREE_TABLE_ROWS (SOURCE_ID, ROW, LABEL, FREE_TEXT) VALUES (" + source_.index.ToString() + ", " + (i + 1).ToString() + ", " + Database.toDb(rows[i].labelText) + ", +" + Database.toDb(rows[i].freeText) + ")";
                sqlCommand = new OleDbCommand(sql, source_.database.cndb);
                sqlCommand.ExecuteNonQuery();
            }
        }



        #endregion

        #region Rows



        /// <summary>Add a row to the collection.</summary>
        /// <param name="row">Specifies the row to add to the collection.</param>
        /// <returns>True for success, false otherwise.</returns>
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



        /// <summary>Remove the specified free table row.</summary>
        /// <param name="index">Specifies the free table row to remove.</param>
        /// <returns>True for success, false otherwise.</returns>
        public bool deleteRow(int index)
        {
            rows_.RemoveAt(index);
            return true;
        }



        public SourceFreeTableRow[] getRows()
        {
            return (SourceFreeTableRow[])rows_.ToArray(typeof(SourceFreeTableRow));
        }



        #endregion



        public string toHtml()
        {
            StringBuilder html = new StringBuilder();
            html.AppendLine("<table style=\"margin-left: 50px; border: 1pt solid black; background-color: lightgray;\">");
            SourceFreeTableRow[] rows = getRows();
            foreach (SourceFreeTableRow row in rows)
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

            switch (labelText_)
            {
            case "Header Row":
                string[] headerCells = freeText_.Split('|');
                html.Append("<tr>");
                foreach (string cell in headerCells)
                {
                    html.Append("<td style=\"font-family: 'Times New Roman'; font-size: 8pt; color: grey;\">");
                    html.Append(cell);
                    html.Append("</td>");
                }
                html.Append("</tr>");
                break;

            case "Row":
                string[] cells = freeText_.Split('|');
                html.Append("<tr>");
                foreach (string cell in cells)
                {
                    if (cell.Length <= 3)
                    {
                        html.Append("<td style=\"text-align: center;\">");
                    }
                    else
                    {
                        html.Append("<td>");
                    }
                    html.Append(cell);
                    html.Append("</td>");
                }
                html.Append("</tr>");
                break;

            case "Multi Row":
                string[] multiCells = freeText_.Split('|');
                int numColumns = 2;
                try
                {
                    numColumns = int.Parse(multiCells[0]);
                }
                catch { }
                html.Append("<tr><td colspan=\"" + numColumns.ToString() + "\">");
                if (multiCells.Length >= 2)
                {
                    html.Append(multiCells[1]);
                }
                html.Append("</td></tr>");
                break;

            default:
                // This is the expected 2 column render.
                html.Append("<tr><td style=\"font-family: 'Times New Roman'; font-size: 8pt; color: grey;\">");
                html.Append(labelText_);
                html.Append("</td><td>");
                html.Append(freeText_);
                html.Append("</td></tr>");
                break;
            }

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
