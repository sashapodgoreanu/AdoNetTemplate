using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetTemplate.Database.Support
{
    public static class Converter
    {
        /// <summary>
        /// This method convert the <paramref name="source"/> to <typeparamref name="T"/> type, and if it fails it returns default(T).
        /// setting <paramref name="defaultValue"/> to false, underlying methods will throw exception if the conversion fails.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <exception cref="InvalidCastException">Failed to convert to type</exception>
        /// <returns></returns>
        public static T Parse<T>(this string source, bool defaultValue = true)
            where T : IConvertible
        {
            dynamic converted = null;
            bool success = source.TryParse(out converted, typeof(T), defaultValue);
            if (!success && !defaultValue)
                throw new InvalidCastException($"Failed to convert {source} to {typeof(T)}");
            return converted;
        }

        /// <summary>
        /// This method convert the <paramref name="source"/> to <paramref name="type"/> and puts it to <paramref name="converted"/>.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="converted"></param>
        /// <param name="type"></param>
        /// <param name="useDefault">If the conversion fails, the default value of the <paramref name="type"/> will be used</param>
        /// <returns>true if <paramref name="source"/> was parsed successfully to given <paramref name="type"/></returns>
        public static bool TryParse(this string source, out dynamic converted, Type type, bool useDefault = false)
        {
            bool retVal = false;
            converted = null;
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Int64:
                    long lnum = 0;
                    retVal = !string.IsNullOrEmpty(source) ? long.TryParse(source, out lnum) : false;
                    converted = retVal || useDefault ? lnum : long.MinValue;
                    break;
                case TypeCode.Int32:
                    int num = 0;
                    retVal = !string.IsNullOrEmpty(source) ? int.TryParse(source, out num) : false;
                    converted = retVal || useDefault ? num : int.MinValue;
                    break;
                case TypeCode.Int16:
                    short snum = 0;
                    retVal = !string.IsNullOrEmpty(source) ? short.TryParse(source, out snum) : false;
                    converted = retVal || useDefault ? snum : short.MinValue;
                    break;
                case TypeCode.UInt64:
                    ulong ulnum = 0;
                    retVal = !string.IsNullOrEmpty(source) ? ulong.TryParse(source, out ulnum) : false;
                    converted = retVal || useDefault ? ulnum : ulong.MinValue;
                    break;
                case TypeCode.UInt32:
                    uint unum = 0;
                    retVal = !string.IsNullOrEmpty(source) ? uint.TryParse(source, out unum) : false;
                    converted = retVal || useDefault ? unum : uint.MinValue;
                    break;
                case TypeCode.UInt16:
                    ushort usnum = 0;
                    retVal = !string.IsNullOrEmpty(source) ? ushort.TryParse(source, out usnum) : false;
                    converted = retVal || useDefault ? usnum : ushort.MinValue;
                    break;
                case TypeCode.Double:
                    double dnum = 0;
                    retVal = !string.IsNullOrEmpty(source) ? double.TryParse(source, out dnum) : false;
                    converted = retVal || useDefault ? dnum : double.MinValue;
                    break;
                case TypeCode.Boolean:
                    bool val = false;
                    retVal = bool.TryParse(source, out val);
                    converted = val;
                    break;
                case TypeCode.Char:
                    char c = '\0';
                    retVal = char.TryParse(source, out c);
                    converted = c;
                    break;
                case TypeCode.String:
                    converted = source;
                    retVal = true;
                    break;
            }

            return retVal;
        }
    }


}
