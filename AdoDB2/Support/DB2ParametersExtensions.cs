

using AdoDB2.Database;
using IBM.Data.DB2;
using IBM.Data.DB2Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoDB2.Support
{
    /// <summary>
    /// Provides support for <see cref="DB2ParametersWrapper{TKey}"/> class
    /// </summary>
    /// <remarks>
    /// Author: Sasha.
    /// </remarks>
    public static class DB2ParametersExtensions
    {
        #region SET
        public static void SetShort<TKey>(this DB2ParametersWrapper<TKey> paramWrapper, TKey key, short value) =>
            SetInValueInternal(paramWrapper, key, value);

        public static void SetShort<TKey>(this DB2ParametersWrapper<TKey> paramWrapper, TKey key,short? value) => 
            SetInValueInternal(paramWrapper, key, value);


        public static void SetShort<TKey>(this DB2ParametersWrapper<TKey> paramWrapper, TKey key, short[] value) =>
            SetInValueInternal<short, TKey>(paramWrapper, key, value);

        public static void SetShort<TKey>(this DB2ParametersWrapper<TKey> paramWrapper, TKey key, short?[] value) => 
            SetInValueInternal(paramWrapper, key, value);

        public static void SetInt<TKey>(this DB2ParametersWrapper<TKey> paramWrapper, TKey key, int value) =>
            SetInValueInternal(paramWrapper, key, value);

        public static void SetInt<TKey>(this DB2ParametersWrapper<TKey> paramWrapper, TKey key, int? value) =>
            SetInValueInternal(paramWrapper, key, value);

        public static void SetInt<TKey>(this DB2ParametersWrapper<TKey> paramWrapper, TKey key, int?[] value) =>
            SetInValueInternal(paramWrapper, key, value);

        public static void SetInt<TKey>(this DB2ParametersWrapper<TKey> paramWrapper, TKey key, int[] value) =>
            SetInValueInternal<int, TKey>(paramWrapper, key, value);

        public static void SetLong<TKey>(this DB2ParametersWrapper<TKey> paramWrapper, TKey key, long value) =>
            SetInValueInternal(paramWrapper, key, value);

        public static void SetLong<TKey>(this DB2ParametersWrapper<TKey> paramWrapper, TKey key, long? value) =>
            SetInValueInternal(paramWrapper, key, value);

        public static void SetLong<TKey>(this DB2ParametersWrapper<TKey> paramWrapper, TKey key, long?[] value) =>
            SetInValueInternal(paramWrapper, key, value);

        public static void SetLong<TKey>(this DB2ParametersWrapper<TKey> paramWrapper, TKey key, long[] value) =>
            SetInValueInternal<long, TKey>(paramWrapper, key, value);


        public static void SetDouble<TKey>(this DB2ParametersWrapper<TKey> paramWrapper, TKey key, double value) => 
            SetInValueInternal(paramWrapper, key, value);

        public static void SetDouble<TKey>(this DB2ParametersWrapper<TKey> paramWrapper, TKey key, double? value) =>
            SetInValueInternal(paramWrapper, key, value);

        public static void SetDouble<TKey>(this DB2ParametersWrapper<TKey> paramWrapper, TKey key, double[] value) =>
            SetInValueInternal<double, TKey>(paramWrapper, key, value);

        public static void SetDouble<TKey>(this DB2ParametersWrapper<TKey> paramWrapper, TKey key, double?[] value) => 
            SetInValueInternal(paramWrapper, key, value);

        public static void SetString<TKey>(this DB2ParametersWrapper<TKey> paramWrapper, TKey key, string value) => 
            SetInValueInternal(paramWrapper, key, value);

        public static void SetString<TKey>(this DB2ParametersWrapper<TKey> paramWrapper, TKey key, string[] value) =>
            SetInValueInternal<string, TKey>(paramWrapper, key, value);

        public static void SetDateTime<TKey>(this DB2ParametersWrapper<TKey> paramWrapper, TKey key, DateTime value) =>
            SetInValueInternal(paramWrapper, key, value);

        public static void SetDateTime<TKey>(this DB2ParametersWrapper<TKey> paramWrapper, TKey key, DateTime? value) => 
            SetInValueInternal(paramWrapper, key, value);

        public static void SetDateTime<TKey>(this DB2ParametersWrapper<TKey> paramWrapper, TKey key, DateTime[] value) => 
            SetInValueInternal<DateTime, TKey>(paramWrapper, key, value);

        public static void SetDateTime<TKey>(this DB2ParametersWrapper<TKey> paramWrapper, TKey key, DateTime?[] value) =>
            SetInValueInternal(paramWrapper, key, value);

        public static void SetClob<TKey>(this DB2ParametersWrapper<TKey> paramWrapper, TKey key, string value) =>
            SetInValueInternalWithOdbType(paramWrapper, key, value as object, DB2Type.Clob);

        public static void SetClob<TKey>(this DB2ParametersWrapper<TKey> paramWrapper, TKey key, string[] value) =>
            SetInValueInternalWithOdbType(paramWrapper, key, value, DB2Type.Clob);

        public static void SetBlob<TKey>(this DB2ParametersWrapper<TKey> paramWrapper, TKey key, byte[] value) =>
            SetInValueInternalWithOdbType(paramWrapper, key, value as object, DB2Type.Blob);

        public static void SetBlob<TKey>(this DB2ParametersWrapper<TKey> paramWrapper, TKey key, byte[][] value) =>
            SetInValueInternalWithOdbType(paramWrapper, key, value, DB2Type.Blob);

        /// <summary>
        /// Set parameter null value.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="paramWrapper"></param>
        /// <param name="key"></param>
        /// <param name="typeCode"></param>
        public static void SetNull<TKey>(this DB2ParametersWrapper<TKey> paramWrapper, TKey key, TypeCode typeCode) =>
            SetNullValueInternal(paramWrapper, key, typeCode);

        private static void SetNullValueInternal<TKey>(this DB2ParametersWrapper<TKey> paramWrapper, TKey key, TypeCode typeCode, ParameterDirection pDir = ParameterDirection.Input)
        {
            switch (typeCode)
            {
                case TypeCode.Int16:
                    SetInValueInternal(paramWrapper, key, (short?) null, pDir);
                    break;
                case TypeCode.Int32:
                    SetInValueInternal(paramWrapper, key, (int?) null, pDir);
                    break;
                case TypeCode.Int64:
                    SetInValueInternal(paramWrapper, key, (long?) null, pDir);
                    break;

                case TypeCode.Double:
                    SetInValueInternal(paramWrapper, key, (double?) null, pDir);
                    break;

                case TypeCode.Decimal:
                    SetInValueInternal(paramWrapper, key, (decimal?) null, pDir);
                    break;

                case TypeCode.String:
                    SetInValueInternal(paramWrapper, key, string.Empty, pDir);
                    break;

                case TypeCode.DateTime:
                    SetInValueInternal(paramWrapper, key, (DateTime?) null, pDir);
                    break;
                default:
                    throw new ArgumentException("Error to map specified type to an DB2Type.");
            }
        }

        private static void SetInValueInternal<TValue, TKey>(this DB2ParametersWrapper<TKey> paramWrapper, TKey key,TValue?[] value, ParameterDirection pDir = ParameterDirection.Input)
            where TValue : struct, IConvertible
        {
            var valuesContainsNull = new object[value.Length];

            for (int i = 0; i < value.Length; i++)
            {
                valuesContainsNull[i] = (object) value[i] ?? DBNull.Value;
            }

            SetInValueInternal<TValue, TKey>(paramWrapper, key, valuesContainsNull, pDir);
        }

        private static void SetInValueInternal<TValue, TKey>(this DB2ParametersWrapper<TKey> paramWrapper, TKey key, TValue? value, ParameterDirection pDir = ParameterDirection.Input)
            where TValue : struct, IConvertible
            => SetInValueInternal<TValue, TKey>(paramWrapper, key, value as object, pDir);

        private static void SetInValueInternal<TValue, TKey>(this DB2ParametersWrapper<TKey> paramWrapper, TKey key, TValue value, ParameterDirection pDir = ParameterDirection.Input)
            => SetInValueInternal<TValue, TKey>(paramWrapper, key, value as object, pDir);

        private static void SetInValueInternal<TValue, TKey>(this DB2ParametersWrapper<TKey> paramWrapper, TKey key, object value, ParameterDirection pDir = ParameterDirection.Input)
        {
            value = value ?? DBNull.Value;

            if (paramWrapper.ContainsKey(key))
            {
                paramWrapper[key].Value = value;
            }
            else
            {
                var p = new DB2Parameter
                {
                    Direction = pDir,
                    ParameterName = "@par_" + key
                };

                var type = typeof(TValue);

                switch (Type.GetTypeCode(type))
                {
                    case TypeCode.Int16:
                        p.DB2Type = DB2Type.SmallInt;
                        p.Value = value;
                        break;
                    case TypeCode.Int32:
                        p.DB2Type = DB2Type.Integer;
                        p.Value = value;
                        break;
                    case TypeCode.Int64:
                        p.DB2Type = DB2Type.BigInt;
                        p.Value = value;
                        break;

                    case TypeCode.Decimal:
                        p.DB2Type = DB2Type.Decimal;
                        p.Value = value;
                        break;

                    case TypeCode.Double:
                        p.DB2Type = DB2Type.Double;
                        p.Value = value;
                        break;

                    case TypeCode.String:
                        p.DB2Type = DB2Type.VarChar;
                        p.Value = value;
                        break;

                    case TypeCode.DateTime:
                        p.DB2Type = DB2Type.Date;
                        p.Value = value;
                        break;

                    default:
                        throw new ArgumentException("Error to map specified type to an DB2Type.");
                }

                paramWrapper[key] = p;
            }
        }

        private static void SetInValueInternalWithOdbType<TKey>(this DB2ParametersWrapper<TKey> paramWrapper, TKey key, object value, DB2Type odbType, ParameterDirection pDir = ParameterDirection.Input)
        {
            value = value ?? DBNull.Value;

            //aggiorno
            if (paramWrapper.ContainsKey(key))
            {
                paramWrapper[key].Value = value;
            }
            else
            {
                var p = new DB2Parameter
                {
                    Direction = pDir,
                    ParameterName = "@par_" + key,
                    DB2Type = odbType,
                    Value = value,
                };
                paramWrapper[key] = p;
            }
        }

        private static void SetOutValueInternal<TKey>(this DB2ParametersWrapper<TKey> paramWrapper, TKey key, TypeCode typeCode)
        {

            var p = new DB2Parameter
            {
                Direction = ParameterDirection.Output,
                ParameterName = "@par_" + key
            };

            switch (typeCode)
            {
                case TypeCode.Int16:
                    p.DB2Type = DB2Type.SmallInt;
                    break;
                case TypeCode.Int32:
                    p.DB2Type = DB2Type.Integer;
                    break;
                case TypeCode.Int64:
                    p.DB2Type = DB2Type.Int8;
                    break;

                case TypeCode.Decimal:
                    p.DB2Type = DB2Type.Decimal;
                    break;

                case TypeCode.Double:
                    p.DB2Type = DB2Type.Double;
                    break;

                case TypeCode.String:
                    p.DB2Type = DB2Type.VarChar;
                    break;

                case TypeCode.DateTime:
                    p.DB2Type = DB2Type.Date;
                    break;

                default:
                    throw new ArgumentException("Error to map specified type to an DB2Type.");
            }

            paramWrapper[key] = p;
            
        }
        #endregion SET

        #region GET

        /// <summary>
        /// Register a <see cref="DB2Parameter"/> with <see cref="DB2Parameter.Direction"/> out and as return type the <paramref name="typeCode"/>.
        /// Any binded parameter with <paramref name="key"/> will be replaced. 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="paramWrapper"></param>
        /// <param name="key"></param>
        /// <param name="typeCode"></param>
        public static void RegisterOutParameter<TKey>(this DB2ParametersWrapper<TKey> paramWrapper, TKey key, TypeCode typeCode) =>
            SetOutValueInternal(paramWrapper, key, typeCode);

        public static short GetShort<TKey>(this DB2ParametersWrapper<TKey> paramWrapper, TKey key) =>
            GetValueInternal<TKey, short>(paramWrapper, key);

        public static int GetInt<TKey>(this DB2ParametersWrapper<TKey> paramWrapper, TKey key) =>
            GetValueInternal<TKey, int>(paramWrapper, key);

        public static long GetLong<TKey>(this DB2ParametersWrapper<TKey> paramWrapper, TKey key) =>
            GetValueInternal<TKey, long>(paramWrapper, key);

        public static string GetString<TKey>(this DB2ParametersWrapper<TKey> paramWrapper, TKey key) =>
            GetValueInternal<TKey, string>(paramWrapper, key);

        public static DateTime GetDateTime<TKey>(this DB2ParametersWrapper<TKey> paramWrapper, TKey key) =>
            GetValueInternal<TKey, DateTime>(paramWrapper, key);

        public static TimeSpan GetTimeSpan<TKey>(this DB2ParametersWrapper<TKey> paramWrapper, TKey key) =>
            GetValueInternal<TKey, TimeSpan>(paramWrapper, key);

        private static T GetValueInternal<TKey, T>(this DB2ParametersWrapper<TKey> paramWrapper, TKey key) =>
            (T) GetValue(paramWrapper, key, Type.GetTypeCode(typeof(T)));

        public static object GetValue<TKey>(this DB2ParametersWrapper<TKey> paramWrapper, TKey key, TypeCode type)
        {
            object value = null;
            if (paramWrapper.ContainsKey(key))
            {
                switch (type)
                {
                    case TypeCode.Int16:
                        if (paramWrapper[key].DB2Type != DB2Type.SmallInt)
                            ThrowArgumentException(key, paramWrapper[key].DB2Type);
                        value = ((short) (DB2Int16) paramWrapper[key].Value);
                        break;
                    case TypeCode.Int32:
                        if (paramWrapper[key].DB2Type != DB2Type.Integer)
                            ThrowArgumentException(key, paramWrapper[key].DB2Type);
                        value = ((int) (DB2Int32) paramWrapper[key].Value);
                        break;
                    case TypeCode.Int64:
                        if (paramWrapper[key].DB2Type != DB2Type.BigInt)
                            ThrowArgumentException(key, paramWrapper[key].DB2Type);
                        value = ((long) (DB2Int64) paramWrapper[key].Value);
                        break;

                    case TypeCode.Decimal:
                        if (paramWrapper[key].DB2Type != DB2Type.Decimal)
                            ThrowArgumentException(key, paramWrapper[key].DB2Type);
                        value = ((decimal) (DB2Decimal) paramWrapper[key].Value);
                        break;

                    case TypeCode.Double:
                        if (paramWrapper[key].DB2Type != DB2Type.Double)
                            ThrowArgumentException(key, paramWrapper[key].DB2Type);
                        value = ((double) (DB2Double) paramWrapper[key].Value);
                        break;

                    case TypeCode.String:
                        var odbType = paramWrapper[key].DB2Type;
                        if (odbType != DB2Type.Char
                            || odbType != DB2Type.VarChar
                            || odbType != DB2Type.Graphic
                            || odbType != DB2Type.VarGraphic
                            || odbType != DB2Type.Clob  //CLOB is a string
                            || odbType != DB2Type.DbClob
                            )
                            ThrowArgumentException(key, paramWrapper[key].DB2Type);
                        value = ((string) (DB2String) paramWrapper[key].Value);
                        break;

                    case TypeCode.DateTime:
                        if (paramWrapper[key].DB2Type == DB2Type.Date)
                            value = ((DateTime)(DB2Date)paramWrapper[key].Value);
                        else if (paramWrapper[key].DB2Type == DB2Type.Timestamp)
                            value = (DateTime)((DB2TimeStamp)paramWrapper[key].Value);
                        else if (paramWrapper[key].DB2Type == DB2Type.Time)
                            value = (TimeSpan)((DB2Time)paramWrapper[key].Value);
                        else
                            ThrowArgumentException(key, paramWrapper[key].DB2Type);
                        break;

                    case TypeCode.DBNull:
                        //will return default value
                        break;

                    //IMPLEMENT OTHER TYPES HERE 
                    default:
                        throw new ArgumentException($"Not a valid TypeCode {Enum.GetName(typeof(TypeCode), type)}");
                }
                return value;
            }
            else
            {
                throw new DataException(string.Format($"Output parameter key {key} does not exist"));
            }
        }

        private static void ThrowArgumentException<TKey>(TKey key, DB2Type oraType)
        {
            throw new ArgumentException(string.Format(
                $"Output parameter key {key} was registered as {Enum.GetName(typeof(DB2Type), oraType)}"));
        }

        #endregion END
    }
}
