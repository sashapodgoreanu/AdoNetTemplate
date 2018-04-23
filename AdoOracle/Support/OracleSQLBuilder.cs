using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoOracle.Support
{
    /// <summary>
    /// Utility class. 
    /// </summary>
    public static class OracleSQLBuilder
    {
        /// <summary>
        /// Builds a insert SQL statement: INSERT INTO <see cref="intoTable"/> (COLUMN1, COLUMN2, ... COLUMNK) VALUES (:COLUMN1, :COLUMN2, ... :COLUMNK)
        /// </summary>
        /// <param name="intoTable"></param>
        /// <param name="columnNames"></param>
        /// <returns></returns>
        public static string BuildInsert(string intoTable, IEnumerable<string> columnNames)
        {
            if (string.IsNullOrEmpty(intoTable))
                throw new ArgumentException("intoTable cannto be null or empty");

            if (columnNames?.Count() == 0)
                throw new ArgumentException("columnNames cannto be null or empty");

            var queryString = "INSERT INTO "
                + intoTable
                + " ("
                + string.Join(",", columnNames.Select( i => Escape(i) ))
                + ") VALUES ("
                + string.Join(",", columnNames.Select( i => i = ":" + i ).ToList())
                + ")";
            return queryString;
        }

        /// <summary>
        /// Builds a insert SQL statement: INSERT INTO <see cref="intoTable"/> (COLUMN1, COLUMN2, ... COLUMNK) VALUES (valCOLUMN1, valCOLUMN2, ... valCOLUMNK)
        /// </summary>
        /// <param name="intoTable">table name</param>
        /// <param name="columnNames">dictionary { key: name of column, value: parameter name or value }</param>
        /// <returns></returns>
        public static string BuildInsert(string intoTable, Dictionary<string, string> columnNames)
        {
            if (string.IsNullOrEmpty(intoTable))
                throw new ArgumentException("intoTable can not be null or empty");

            if (columnNames.Count == 0)
                throw new ArgumentException("columnNames can not be null or empty");

            var queryString = "INSERT INTO "
                + intoTable
                + " ("
                + string.Join(",", columnNames.Keys.Select(i => Escape(i)))
                + ") VALUES ("
                + string.Join(",", columnNames.Values.ToList())
                + ")";
            return queryString;
        }

        /// <summary>
        /// Builds a delete SQL statement: 
        /// DELETE FROM <see cref="deleteFromTable"/> 
        /// WHERE W_COLUMN1 = valW_COLUMN1 AND W_COLUMN2 = valW_COLUMN2 ... AND W_COLUMNK = valW_COLUMN
        /// </summary>
        /// <param name="deleteFromTable">NAME OF TABLE FROM WHERE TO DELETE</param>
        /// <param name="whereCol">dictionary { key: name of column, value: parameter name or value } for sql where condition</param>
        /// <returns></returns>
        public static string BuildDelete(string deleteFromTable, Dictionary<string, string> whereCol)
        {
            if (string.IsNullOrEmpty(deleteFromTable))
                throw new ArgumentException("deleteFromTable can not be null or empty");

            var whereClause = WhereClause(whereCol);
            var queryString = string.Format($"DELETE FROM  {deleteFromTable} {whereClause}");

            return queryString;
        }

        /// <summary>
        /// Builds an update sql: 
        /// UPDATE TABLE <see cref="updateTable"/> 
        /// SET COLUMN1 = valCOLUMN1, COLUMN2 = valCOLUMN2 , ... COLUMNK = valCOLUMNK 
        /// WHERE W_COLUMN1 = valW_COLUMN1 AND W_COLUMN2 = valW_COLUMN2 ... AND W_COLUMNK = valW_COLUMN
        /// </summary>
        /// <param name="updateTable">NAME OF TABLE TO UPDATE</param>
        /// <param name="setCol">dictionary { key: name of column, value: parameter name or value } to set</param>
        /// <param name="whereCol">dictionary { key: name of column, value: parameter name or value } for sql where condition</param>
        /// <returns></returns>
        public static string BuildUpdate(string updateTable, Dictionary<string, string> setCol, Dictionary<string, string> whereCol)
        {

            if (string.IsNullOrEmpty(updateTable))
                throw new ArgumentException("updateTable can not be null or empty");

            if (setCol.Count == 0)
                throw new ArgumentException("setCol can not be null or empty");

            var setQuery = "SET " + string.Join(", ", setCol.Select(i => Escape(i.Key) + " = " + i.Value));
            var whereClause = WhereClause(whereCol);
            var queryString = string.Format($"UPDATE {updateTable} {setQuery} {whereClause}");

            return queryString;
        }

        /// <summary>
        /// Build a select SQL : 
        /// <para>
        /// SELECT FROM <typeparamref name="fromtable"/> SEL_COLUMN1, SEL_COLUMN2, ..., SEL_COLUMNK 
        /// </para>
        /// <para>
        /// WHERE CONDITION1 = VALCOND1 AND CONDITION2 = VALCOND2 AND ...
        /// </para>
        /// </summary>
        /// <param name="selectColumns"></param>
        /// <param name="fromtable"></param>
        /// <param name="whereCol_AND">Clause in AND</param>
        /// <returns></returns>
        public static string BuildSelect(IEnumerable<string> selectColumns, string fromtable, Dictionary<string, string> whereCol_AND = null)
        {
            if (selectColumns == null || selectColumns.Count() == 0)
                throw new ArgumentException("SELECT can not be null or empty");

            if (string.IsNullOrEmpty(fromtable))
                throw new ArgumentException("FROM can not be null or empty");

            var selectString = string.Join(", ", selectColumns.Select(i => Escape(i)));
            var whereClause_AND = WhereClause(whereCol_AND);
            var queryString = string.Format($"SELECT {selectString} FROM {fromtable} {whereClause_AND}");
            return queryString;
        }

        private static string WhereClause(Dictionary<string, string> whereCol)
        {
            if (whereCol == null)
                return string.Empty;
            return whereCol.Count != 0 ? 
                "WHERE " + string.Join(" AND ", whereCol.Select(i => Escape(i.Key) + " = " + i.Value)) 
                : string.Empty;
        }

        public static string Escape(string input)
        {
            using (var writer = new StringWriter())
            {
                using (var provider = CodeDomProvider.CreateProvider("CSharp"))
                {
                    provider.GenerateCodeFromExpression(new CodePrimitiveExpression(input), writer, null);
                    return writer.ToString();
                }
            }
        }
    }
}
