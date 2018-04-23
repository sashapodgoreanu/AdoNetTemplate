using AdoNetTemplate.Database.Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace AdoNetTemplate.Database.Support
{
    public static class DataReaderExtensions
    {

        /// <summary>
        /// get int field value by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int? GetInt32(this IDataReader dataReader, string name)
        {
            int i = dataReader.GetOrdinal(name);
            if (!dataReader.IsDBNull(i))
            {
                return dataReader.GetInt32(i);
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// get field double value by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static double? GetDouble(this IDataReader dataReader, string name)
        {
            var i = dataReader.GetOrdinal(name);
            if (!dataReader.IsDBNull(i))
            {
                return dataReader.GetDouble(i);
            }
            else
            {
                return null;
            }
         }

        /// <summary>
        /// get field long value by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static long? GetLong(this IDataReader dataReader, string name)
        {
            var i = dataReader.GetOrdinal(name);
            if (!dataReader.IsDBNull(i))
            {
                return dataReader.GetInt64(i);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        ///  get field string value by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetString(this IDataReader dataReader, string name)
        {
            int i = dataReader.GetOrdinal(name);
            if (!dataReader.IsDBNull(i))
            {
                return dataReader.GetString(i);
            }
            else
            {
                return null;
            }
        }


        public static decimal? GetDecimal(this IDataReader dataReader, string name)
        {
            int i = dataReader.GetOrdinal(name);
            if (!dataReader.IsDBNull(i))
            {
                return dataReader.GetDecimal(i);
            }
            else
            { 
                return null;
            }
        }

        public static DateTime? GetDateTime(this IDataReader dataReader, string name)
        {
            var i = dataReader.GetOrdinal(name);
            if (!dataReader.IsDBNull(i))
            {
                return dataReader.GetDateTime(i);
            }
            else
            {
                return null;
            } 
        }

        public static object GetValue(this IDataReader dataReader, string name)
        {
            var i = dataReader.GetOrdinal(name);

            if (!dataReader.IsDBNull(i))
            {
                return dataReader.GetValue(i);
            }
            else
            {
                return null;
            }
        }

    }
}
