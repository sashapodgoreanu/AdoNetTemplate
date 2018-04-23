
using AdoOracle.Database;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoOracle.Database
{
    /// <summary>
    /// Provides support for <see cref="OracleParametersWrapper{TKey}"/> class
    /// </summary>
    /// <remarks>
    /// Author: Sasha.
    /// </remarks>
    public static class OracleParametersExtensions
    {
        #region SET
        public static void SetShort<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key, short value) =>
            SetInValueInternal(paramWrapper, key, value);

        public static void SetShort<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key,short? value) => 
            SetInValueInternal(paramWrapper, key, value);


        public static void SetShort<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key, short[] value) =>
            SetInValueInternal<short, TKey>(paramWrapper, key, value);

        public static void SetShort<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key, short?[] value) => 
            SetInValueInternal(paramWrapper, key, value);

        public static void SetInt<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key, int value) =>
            SetInValueInternal(paramWrapper, key, value);

        public static void SetInt<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key, int? value) =>
            SetInValueInternal(paramWrapper, key, value);

        public static void SetInt<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key, int?[] value) =>
            SetInValueInternal(paramWrapper, key, value);

        public static void SetInt<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key, int[] value) =>
            SetInValueInternal<int, TKey>(paramWrapper, key, value);

        public static void SetLong<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key, long value) =>
            SetInValueInternal(paramWrapper, key, value);

        public static void SetLong<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key, long? value) =>
            SetInValueInternal(paramWrapper, key, value);

        public static void SetLong<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key, long?[] value) =>
            SetInValueInternal(paramWrapper, key, value);

        public static void SetLong<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key, long[] value) =>
            SetInValueInternal<long, TKey>(paramWrapper, key, value);


        public static void SetDouble<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key, double value) => 
            SetInValueInternal(paramWrapper, key, value);

        public static void SetDouble<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key, double? value) =>
            SetInValueInternal(paramWrapper, key, value);

        public static void SetDouble<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key, double[] value) =>
            SetInValueInternal<double, TKey>(paramWrapper, key, value);

        public static void SetDouble<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key, double?[] value) => 
            SetInValueInternal(paramWrapper, key, value);

        public static void SetDecimal<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key, decimal value) =>
            SetInValueInternal(paramWrapper, key, value);

        public static void SetDecimal<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key, decimal? value) =>
            SetInValueInternal(paramWrapper, key, value);

        public static void SetDecimal<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key, decimal[] value) =>
            SetInValueInternal<double, TKey>(paramWrapper, key, value);

        public static void SetDecimal<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key, decimal?[] value) =>
            SetInValueInternal(paramWrapper, key, value);

        public static void SetBoolean<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key, bool value) =>
            SetInValueInternal(paramWrapper, key, value);

        public static void SetBoolean<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key, bool? value) =>
            SetInValueInternal(paramWrapper, key, value);

        public static void SetBoolean<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key, bool?[] value) => 
            SetInValueInternal(paramWrapper, key, value);

        public static void SetBoolean<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key, bool[] value) =>
            SetInValueInternal<bool, TKey>(paramWrapper, key, value);

        public static void SetString<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key, string value) => 
            SetInValueInternal(paramWrapper, key, value);

        public static void SetString<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key, string[] value) =>
            SetInValueInternal<string, TKey>(paramWrapper, key, value);

        public static void SetDateTime<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key, DateTime value) =>
            SetInValueInternal(paramWrapper, key, value);

        public static void SetDateTime<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key, DateTime? value) => 
            SetInValueInternal(paramWrapper, key, value);

        public static void SetDateTime<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key, DateTime[] value) => 
            SetInValueInternal<DateTime, TKey>(paramWrapper, key, value);

        public static void SetDateTime<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key, DateTime?[] value) =>
            SetInValueInternal(paramWrapper, key, value);

        public static void SetClob<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key, string value) =>
            SetInValueInternalWithOdbType(paramWrapper, key, value as object, OracleDbType.Clob);

        public static void SetClob<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key, string[] value) =>
            SetInValueInternalWithOdbType(paramWrapper, key, value, OracleDbType.Clob);

        public static void SetBlob<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key, byte[] value) =>
            SetInValueInternalWithOdbType(paramWrapper, key, value as object, OracleDbType.Blob);

        public static void SetBlob<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key, byte[][] value) =>
            SetInValueInternalWithOdbType(paramWrapper, key, value, OracleDbType.Blob);

        /// <summary>
        /// Set parameter null value.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="paramWrapper"></param>
        /// <param name="key"></param>
        /// <param name="typeCode"></param>
        public static void SetNull<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key, TypeCode typeCode) =>
            SetNullValueInternal(paramWrapper, key, typeCode);

        private static void SetNullValueInternal<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key, TypeCode typeCode, ParameterDirection pDir = ParameterDirection.Input)
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

                case TypeCode.Boolean:
                    SetInValueInternal(paramWrapper, key, (bool?) null, pDir);
                    break;
                default:
                    throw new ArgumentException("Error to map specified type to an OracleDbType.");
            }
        }

        private static void SetInValueInternal<TValue, TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key,TValue?[] value, ParameterDirection pDir = ParameterDirection.Input)
            where TValue : struct, IConvertible
        {
            var valuesContainsNull = new object[value.Length];

            for (int i = 0; i < value.Length; i++)
            {
                valuesContainsNull[i] = (object) value[i] ?? DBNull.Value;
            }

            SetInValueInternal<TValue, TKey>(paramWrapper, key, valuesContainsNull, pDir);
        }

        private static void SetInValueInternal<TValue, TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key, TValue? value, ParameterDirection pDir = ParameterDirection.Input)
            where TValue : struct, IConvertible
            => SetInValueInternal<TValue, TKey>(paramWrapper, key, value as object, pDir);

        private static void SetInValueInternal<TValue, TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key, TValue value, ParameterDirection pDir = ParameterDirection.Input)
            => SetInValueInternal<TValue, TKey>(paramWrapper, key, value as object, pDir);

        private static void SetInValueInternal<TValue, TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key, object value, ParameterDirection pDir = ParameterDirection.Input)
        {
            value = value ?? DBNull.Value;

            if (paramWrapper.ContainsKey(key))
            {
                paramWrapper[key].Value = value;
            }
            else
            {
                var p = new OracleParameter
                {
                    Direction = pDir,
                    ParameterName = "@par_" + key
                };

                var type = typeof(TValue);

                switch (Type.GetTypeCode(type))
                {
                    case TypeCode.Int16:
                        p.OracleDbType = OracleDbType.Int16;
                        break;

                    case TypeCode.Int32:
                        p.OracleDbType = OracleDbType.Int32;
                        break;

                    case TypeCode.Int64:
                        p.OracleDbType = OracleDbType.Int64;
                        break;

                    case TypeCode.Decimal:
                        p.OracleDbType = OracleDbType.Decimal;
                        break;

                    case TypeCode.Double:
                        p.OracleDbType = OracleDbType.Double;
                        break;

                    case TypeCode.String:
                        p.OracleDbType = OracleDbType.Varchar2;
                        break;

                    case TypeCode.DateTime:
                        p.OracleDbType = OracleDbType.Date;
                        break;

                    case TypeCode.Boolean:
                        p.OracleDbType = OracleDbType.Boolean;
                        break;

                    default:
                        throw new ArgumentException("Error to map specified type to an OracleDbType.");
                }

                p.Value = value;
                paramWrapper[key] = p;
            }
        }

        private static void SetInValueInternalWithOdbType<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key, object value, OracleDbType odbType, ParameterDirection pDir = ParameterDirection.Input)
        {
            value = value ?? DBNull.Value;

            //aggiorno
            if (paramWrapper.ContainsKey(key))
            {
                paramWrapper[key].Value = value;
            }
            else
            {
                var p = new OracleParameter
                {
                    Direction = pDir,
                    ParameterName = "@par_" + key,
                    OracleDbType = odbType,
                    Value = value,
                };
                paramWrapper[key] = p;
            }
        }

        private static void SetOutValueInternal<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key, TypeCode typeCode)
        {

            OracleDbType OracleDbType;

            //Type type = typeof(TValue);

            switch (typeCode)
            {
                case TypeCode.Int16:
                    OracleDbType = OracleDbType.Int16;
                    break;
                case TypeCode.Int32:
                    OracleDbType = OracleDbType.Int32;
                    break;
                case TypeCode.Int64:
                    OracleDbType = OracleDbType.Int64;
                    break;

                case TypeCode.Decimal:
                    OracleDbType = OracleDbType.Decimal;
                    break;

                case TypeCode.Double:
                    OracleDbType = OracleDbType.Double;
                    break;

                case TypeCode.String:
                    OracleDbType = OracleDbType.Varchar2;
                    break;

                case TypeCode.DateTime:
                    OracleDbType = OracleDbType.Date;
                    break;

                case TypeCode.Boolean:
                    OracleDbType = OracleDbType.Boolean;
                    break;
                default:
                    throw new ArgumentException("Error to map specified type to an OracleDbType.");
            }

            paramWrapper._setOutValueInternal(key, OracleDbType);

        }

        private static void _setOutValueInternal<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key, OracleDbType oracleDbType)
        {
            var p = new OracleParameter
            {
                
                ParameterName = "@par_" + key,
                OracleDbType = oracleDbType,
                Direction = ParameterDirection.Output,
            };

            paramWrapper[key] = p;
        }
        #endregion SET

        #region GET

        /// <summary>
        /// Register a <see cref="OracleParameter"/> with <see cref="OracleParameter.Direction"/> out and as return type the <paramref name="typeCode"/>.
        /// Any binded parameter with <paramref name="key"/> will be replaced. 
        /// </summary>
        /// <remarks>
        /// 
        /// Type registration is for .Net return type and not for <see cref="OracleDbType"/>. 
        /// Follow this table to properly register Out Parameters.
        ///      _________________________________________
        ///     | OracleDbType            =>    .Net type |
        ///     |_________________________________________|
        ///     | Bfile                   =>	Binary    |
        ///     | Blob                    =>	Binary    |
        ///     | Char                    =>	String    |
        ///     | Clob                    =>	String    |
        ///     | Date                    =>	DateTime  |
        ///     | Float                   =>	Decimal   |
        ///     | Int                     =>	Int32     |
        ///     | Long                    =>	String    |
        ///     | LongRaw                 =>	Binary    |
        ///     | NChar                   =>	String    |
        ///     | NChar                   =>	String    |
        ///     | Number(1,0)             =>	Int16     |
        ///     | Number(2,0)             =>	Int16     |
        ///     | Number(3,0)             =>	Int16     |
        ///     | Number(4,0)             =>	Int16     |
        ///     | Number(5,0)             =>	Int16     |
        ///     | Number(6,0)             =>	Int32     |
        ///     | Number(7,0)             =>	Int32     |
        ///     | Number(8,0)             =>	Int32     |
        ///     | Number(9,0)             =>	Int32     |
        ///     | Number(10,0)            =>	Int32     |
        ///     | Number(11,0)            =>	Int64     |
        ///     | Number(12,0)            =>	Int64     |
        ///     | Number(13,0)            =>	Int64     |
        ///     | Number(14,0)            =>	Int64     |
        ///     | Number(15,0)            =>	Int64     |
        ///     | Number(16,0)            =>	Int64     |
        ///     | Number(17,0)            =>	Int64     |
        ///     | Number(18,0)            =>	Int64     |
        ///     | Number(19,0)            =>	Int64     |
        ///     | Number(all_other_cases) =>	Decimal   |
        ///     | NVarchar2               =>	String    |
        ///     | Raw                     =>	Binary    |
        ///     | Timestamp               =>	DateTime  |
        ///     | Varchar2                =>	String    |
        ///     |_________________________________________|
        ///     
        /// </remarks>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="paramWrapper"></param>
        /// <param name="key"></param>
        /// <param name="typeCode"></param>
        public static void RegisterOutParameter<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key, TypeCode typeCode) =>
            SetOutValueInternal(paramWrapper, key, typeCode);

        public static void RegisterRefCursor<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key) =>
            _setOutValueInternal(paramWrapper, key, OracleDbType.RefCursor);

        public static short GetShort<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key) =>
            GetValueInternal<TKey, short>(paramWrapper, key);

        public static int GetInt<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key) =>
            GetValueInternal<TKey, int>(paramWrapper, key);

        public static long GetLong<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key) =>
            GetValueInternal<TKey, long>(paramWrapper, key);

        public static string GetString<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key) =>
            GetValueInternal<TKey, string>(paramWrapper, key);

        public static bool GetBoolean<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key) =>
            GetValueInternal<TKey, bool>(paramWrapper, key);

        public static DateTime GetDateTime<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key) =>
            GetValueInternal<TKey, DateTime>(paramWrapper, key);

        private static T GetValueInternal<TKey, T>(this OracleParametersWrapper<TKey> paramWrapper, TKey key) =>
            (T) GetValue(paramWrapper, key, Type.GetTypeCode(typeof(T)));

        public static object GetValue<TKey>(this OracleParametersWrapper<TKey> paramWrapper, TKey key, TypeCode type)
        {
            object value = null;
            if (paramWrapper.ContainsKey(key))
            {
                switch (type)
                {
                    case TypeCode.Int16:
                        if (paramWrapper[key].OracleDbType != OracleDbType.Int16)
                            ThrowArgumentException(key, paramWrapper[key].OracleDbType);
                        value = ((short) (OracleDecimal) paramWrapper[key].Value);
                        break;
                    case TypeCode.Int32:
                        if (paramWrapper[key].OracleDbType != OracleDbType.Int32)
                            ThrowArgumentException(key, paramWrapper[key].OracleDbType);
                        value = ((int) (OracleDecimal) paramWrapper[key].Value);
                        break;
                    case TypeCode.Int64:
                        if (paramWrapper[key].OracleDbType != OracleDbType.Int64)
                            ThrowArgumentException(key, paramWrapper[key].OracleDbType);
                        value = ((long) (OracleDecimal) paramWrapper[key].Value);
                        break;

                    case TypeCode.Decimal:
                        if (paramWrapper[key].OracleDbType != OracleDbType.Decimal)
                            ThrowArgumentException(key, paramWrapper[key].OracleDbType);
                        value = ((decimal) (OracleDecimal) paramWrapper[key].Value);
                        break;

                    case TypeCode.Double:
                        if (paramWrapper[key].OracleDbType != OracleDbType.Double)
                            ThrowArgumentException(key, paramWrapper[key].OracleDbType);
                        value = ((double) (OracleDecimal) paramWrapper[key].Value);
                        break;

                    case TypeCode.String:
                        var odbType = paramWrapper[key].OracleDbType;
                        if (odbType != OracleDbType.Varchar2
                            || odbType != OracleDbType.NVarchar2
                            || odbType != OracleDbType.Char
                            || odbType != OracleDbType.NChar
                            || odbType != OracleDbType.Clob  //CLOB is a string
                            || odbType != OracleDbType.NClob
                            || odbType != OracleDbType.XmlType //XML is a string
                            )
                            ThrowArgumentException(key, paramWrapper[key].OracleDbType);
                        value = ((string) (OracleString) paramWrapper[key].Value);
                        break;

                    case TypeCode.DateTime:
                        if (paramWrapper[key].OracleDbType != OracleDbType.Date)
                            ThrowArgumentException(key, paramWrapper[key].OracleDbType);
                        value = ((DateTime) (OracleDate) paramWrapper[key].Value);
                        break;

                    case TypeCode.Boolean:
                        if (paramWrapper[key].OracleDbType != OracleDbType.Boolean)
                            ThrowArgumentException(key, paramWrapper[key].OracleDbType);
                        value = ((bool) (OracleBoolean) paramWrapper[key].Value);
                        break;
                    case TypeCode.DBNull:
                        //will return default value
                        break;

                    //IMPLEMENT OTHER TYPES HERE

                    //if (paramWrapper[key].OracleDbType != OracleDbType.Boolean)
                    //    throwArgumentException(key, paramWrapper[key].OracleDbType);
                    //value = ((bool)(OracleBoolean)paramWrapper[key].Value);
                    //break;
                    default:
                        throw new ArgumentException($"Not a valid TypeCode {type}");
                }
                return value;
            }
            else
            {
                throw new DataException(string.Format($"Output parameter key {key} does not exist"));
            }
        }

        private static void ThrowArgumentException<TKey>(TKey key, OracleDbType oraType)
        {
            throw new ArgumentException(string.Format(
                $"Output parameter key {key} was registered as {Enum.GetName(typeof(OracleDbType), oraType)}"));
        }

        #endregion END
    }
}
