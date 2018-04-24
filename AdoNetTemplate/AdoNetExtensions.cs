using AdoNetTemplate.Database.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetTemplate
{
    /// <summary>
    /// Provides extensions to <see cref="AdoTemplate"/>
    /// </summary>
    /// <remarks>
    /// Sasha
    /// </remarks>
    public static class AdoTemplateExtensions
    {
        /// <summary>
        /// Executes the query, and returns the first column of the first row in the resultset
        /// returned by the query if exists otherwise null. Extra columns or rows are ignored.
        /// </summary>
        /// <param name="adoTemplate">The ado template</param>
        /// <param name="cmdText">The command text (Select)</param>
        /// <param name="dbParameterSetter">The parameter setter to bind to the query</param>
        /// <returns></returns>
        public static T? ExecuteScalar<T>(this AdoTemplate adoTemplate, string cmdText, IDbParameterSetter dbParameterSetter = null) 
            where T : struct, IConvertible
        {
            var returned = adoTemplate.ExecuteScalar(CommandType.Text, cmdText, dbParameterSetter);
            return returned.ToNullable<T>();
        }

        /// <summary>
        /// Executes the query, and returns the first column of the first row in the resultset
        /// returned by the query. Extra columns or rows are ignored.
        /// </summary>
        /// <param name="adoTemplate">The ado template</param>
        /// <param name="cmdType">Text or Stored Procedure</param>
        /// <param name="cmdText">The command text (Select)</param>
        /// <returns></returns>
        public static T? ExecuteScalar<T>(this AdoTemplate adoTemplate, CommandType cmdType, string cmdText)
            where T : struct, IConvertible
        {
            var returned = adoTemplate.ExecuteScalar(cmdType, cmdText, null);
            return returned.ToNullable<T>();
        }

        /// <summary>
        /// Executes the query, and returns the first column of the first row in the resultset
        /// returned by the query. Extra columns or rows are ignored.
        /// <see cref="IAdoOperations.ExecuteScalar(CommandType, string)"/>
        /// </summary>
        /// <param name="cmdType">Text or Stored Procedure</param>
        /// <param name="cmdText">The command text (Select)</param>
        /// <param name="dbParameterSetter">The parameter setter to bind to the query</param>
        /// <param name="adoTemplate">The ado template</param>
        /// <returns></returns>
        public static T? ExecuteScalar<T>(this AdoTemplate adoTemplate, CommandType cmdType, string cmdText, IDbParameterSetter dbParameterSetter) 
            where T : struct, IConvertible
        {
            var returned = adoTemplate.ExecuteScalar(cmdType, cmdText, dbParameterSetter);
            return returned.ToNullable<T>();
        }




        /// <summary>
        /// Converts the specified input to a nullable type.
        /// </summary>
        /// <typeparam name="T">Nullable type</typeparam>
        /// <param name="input">the input to be converted</param>
        /// <returns></returns>
        private static T? ToNullable<T>(this object input)
        where T : struct
        {
            if (input == null || input == DBNull.Value)
                return null;
            else
            {
                try
                {
                    return Convert.ChangeType(input, typeof(T)) as T?;
                }
                catch (InvalidCastException ex)
                {
                    throw new InvalidCastException($"Cannot convert {input} to type {typeof(T?)}", ex) ;
                }
            }
        }

        /// <summary>
        /// Executes the query, and returns the first column of the first row in the resultset
        /// returned by the query. Extra columns or rows are ignored.
        /// </summary>
        /// <param name="adoTemplate">The ado template</param>
        /// <param name="cmdText">The command text (Select)</param>
        /// <param name="dbParameterSetter">The parameter setter to bind to the query</param>
        /// <returns></returns>
        public static object ExecuteScalar(this AdoTemplate adoTemplate, string cmdText, IDbParameterSetter dbParameterSetter = null)
        {
            var retVal = adoTemplate.ExecuteScalar(CommandType.Text, cmdText, dbParameterSetter);
            return retVal == DBNull.Value ? null : retVal;
        }

        /// <summary>
        /// Executes the query, and returns the first column of the first row in the resultset
        /// returned by the query. Extra columns or rows are ignored.
        /// </summary>
        /// <param name="adoTemplate">The ado template</param>
        /// <param name="cmdType">Text or Stored Procedure</param>
        /// <param name="cmdText">The command text (Select)</param>
        /// <returns></returns>
        public static object ExecuteScalar(this AdoTemplate adoTemplate, CommandType cmdType, string cmdText)
        {
            var retVal = adoTemplate.ExecuteScalar(cmdType, cmdText, null);
            return retVal == DBNull.Value ? null : retVal;
        }


        /// <summary>
        ///  Executes a non query with the specified command text, returning the number of rows affected.
        /// </summary>
        /// <param name="adoTemplate">The ado template</param>
        /// <param name="cmdText">The command text (Insert, Update, Delete)</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(this AdoTemplate adoTemplate, string cmdText)
        {
            return adoTemplate.ExecuteNonQuery(CommandType.Text, cmdText, null);
        }

        /// <summary>
        /// Executes a non query with the specified command text, returning the number of rows affected.
        /// </summary>
        /// <param name="adoTemplate">The ado template</param>
        /// <param name="cmdType">Text or Stored Procedure</param>
        /// <param name="cmdText">The command text (Insert, Update, Delete)</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(this AdoTemplate adoTemplate, CommandType cmdType, string cmdText)
        {
            return adoTemplate.ExecuteNonQuery(cmdType, cmdText, null);
        }

        /// <summary>
        /// Executes a non query with the specified command text, returning the number of rows affected.
        /// </summary>
        /// <param name="adoTemplate">The ado template</param>
        /// <param name="cmdText">The command text (Insert, Update, Delete)</param>
        /// <param name="dbParameterSetter">The parameter setter to bind to the query</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(this AdoTemplate adoTemplate, string cmdText, IDbParameterSetter dbParameterSetter)
        {
            return adoTemplate.ExecuteNonQuery(CommandType.Text, cmdText, dbParameterSetter);
        }

        /// <summary>
        ///  Executes a query with the specified command text and <typeparamref name="dbParameterSetter"/>,  mapping a set of result to a list of object via a <typeparamref name="setMapperFunc"/>.
        /// </summary>
        /// <param name="adoTemplate">The ado template</param>
        /// <param name="cmdText">The command text (Select)</param>
        /// <param name="setMapperFunc">CallBack function that maps a set of result to list of objects</param>
        /// <param name="dbParameterSetter">todo: describe dbParameterSetter parameter on QueryForObject</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T QueryForObject<T>(this AdoTemplate adoTemplate, string cmdText, IRowMapper<T> rowMapper)
        {
            return adoTemplate.QueryForObject(CommandType.Text, cmdText, null, rowMapper);
        }

        /// <summary>
        ///  Executes a query with the specified command text and <typeparamref name="dbParameterSetter"/>,  mapping a set of result to a list of object via a <typeparamref name="setMapperFunc"/>.
        /// </summary>
        /// <param name="adoTemplate">The ado template</param>
        /// <param name="cmdText">The command text (Select)</param>
        /// <param name="setMapperFunc">CallBack function that maps a set of result to list of objects</param>
        /// <param name="dbParameterSetter">todo: describe dbParameterSetter parameter on QueryForObject</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T QueryForObject<T>(this AdoTemplate adoTemplate, string cmdText, IDbParameterSetter dbParameterSetter, IRowMapper<T> rowMapper)
        {
            return adoTemplate.QueryForObject(CommandType.Text, cmdText, dbParameterSetter, rowMapper); 
        }

        /// <summary>
        ///  Executes a query with the specified command text,  mapping a single result row to a object via a <typeparamref name="rowMapperFunc"/>.
        /// <para>
        /// Don't Call <see cref="IDataReader.Read"/> to get next result.
        /// </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="adoTemplate">The ado template</param>
        /// <param name="cmdText">The command text (Select)</param>
        /// <param name="rowMapperFunc">CallBack function that maps a single result to a single object in a rowNum</param>
        /// <returns></returns>
        public static T QueryForObject<T>(this AdoTemplate adoTemplate, string cmdText, Func<IDataReader, int, T> rowMapperFunc)
        {
            var rowMapper = new IRowMapImplementor<T>(rowMapperFunc);
            return adoTemplate.QueryForObject(CommandType.Text, cmdText, null, rowMapper);
        }

        /// <summary>
        ///  Executes a query with the specified command text, mapping a set of result to a list of object via a <typeparamref name="setMapperFunc"/>.
        /// <para>
        /// Call <see cref="IDataReader.Read"/> to get next result.
        /// </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="adoTemplate">The ado template</param>
        /// <param name="cmdText">The command text (Select)</param>
        /// <param name="setMapperFunc">CallBack function that maps a set of result to list of objects</param>
        /// <returns></returns>
        public static T QueryForObject<T>(this AdoTemplate adoTemplate, string cmdText, Func<IDataReader, IList<T>> setMapperFunc)
        {
            var rowMapper = new IResultSetImplementor<T>(setMapperFunc);
            return adoTemplate.QueryForObject(CommandType.Text, cmdText, null, rowMapper);
        }

        /// <summary>
        ///  Executes a query with the specified command text and <typeparamref name="dbParameterSetter"/>,  mapping a single result row to a object via a <typeparamref name="rowMapperFunc"/>.
        /// <para>
        /// Don't Call <see cref="IDataReader.Read"/> to get next result.
        /// </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="adoTemplate">The ado template</param>
        /// <param name="cmdText">The command text (Select)</param>
        /// <param name="rowMapperFunc">CallBack function that maps a single result to a single object in a rowNum</param>
        /// <param name="dbParameterSetter">The parameter setter to bind to the query</param>
        /// <returns></returns>
        public static T QueryForObject<T>(this AdoTemplate adoTemplate, string cmdText, IDbParameterSetter dbParameterSetter, Func<IDataReader, int, T> rowMapperFunc)
        {
            IRowMapImplementor<T> rowMapper = new IRowMapImplementor<T>(rowMapperFunc);
            return adoTemplate.QueryForObject(CommandType.Text, cmdText, dbParameterSetter, rowMapper);
        }

        /// <summary>
        ///  Executes a query with the specified command text and <typeparamref name="dbParameterSetter"/>,  mapping a set of result to a list of object via a <typeparamref name="setMapperFunc"/>.
        /// <para>
        /// Call <see cref="IDataReader.Read"/> to get next result.
        /// </para>
        /// </summary>
        /// <param name="adoTemplate">The ado template</param>
        /// <param name="cmdText">The command text (Select)</param>
        /// <param name="setMapperFunc">CallBack function that maps a set of result to list of objects</param>
        /// <param name="dbParameterSetter">todo: describe dbParameterSetter parameter on QueryForObject</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T QueryForObject<T>(this AdoTemplate adoTemplate, string cmdText, IDbParameterSetter dbParameterSetter, Func<IDataReader, IList<T>> setMapperFunc)
        {
            var rowMapper = new IResultSetImplementor<T>(setMapperFunc);
            return adoTemplate.QueryForObject(CommandType.Text, cmdText, dbParameterSetter, rowMapper);
        }

        /// <summary>
        /// Executes a query with the specified command text, mapping a single result row to a object via a <typeparamref name="rowMapperFunc"/>.
        /// <para>
        /// Don't Call <see cref="IDataReader.Read"/> to get next result.
        /// </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="adoTemplate">The ado template</param>
        /// <param name="cmdText">The command text (Select)</param>
        /// <param name="rowMapperFunc">CallBack function that maps a single result to a single object in a rowNum</param>
        /// <returns></returns>
        public static IList<T> QueryForList<T>(this AdoTemplate adoTemplate, string cmdText, Func<IDataReader, int, T> rowMapperFunc)
        {
            var rowMapper = new IRowMapImplementor<T>(rowMapperFunc);
            return adoTemplate.QueryForList(CommandType.Text, cmdText, null, rowMapper);
        }

        /// <summary>
        /// Executes a query with the specified command text,mapping a set of result to a list of object via a <typeparamref name="setMapperFunc"/>.
        /// <para>
        /// Call <see cref="IDataReader.Read"/> to get next result.
        /// </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="adoTemplate">The ado template</param>
        /// <param name="cmdText">The command text (Select)</param>
        /// <param name="setMapperFunc">CallBack function that maps a set of result to list of objects</param>
        /// <returns></returns>
        public static IList<T> QueryForList<T>(this AdoTemplate adoTemplate, string cmdText, Func<IDataReader, IList<T>> setMapperFunc)
        {
            var rowMapper = new IResultSetImplementor<T>(setMapperFunc);
            return adoTemplate.QueryForList(CommandType.Text, cmdText,null, rowMapper);
        }

        /// <summary>
        /// Executes a query with the specified command text, mapping a single result row to a object via a <typeparamref name="rowMapper"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="adoTemplate">The ado template</param>
        /// <param name="cmdText">The command text (Select)</param>
        /// <param name="rowMapper">maps a single result to a single object</param>
        /// <returns></returns>
        public static IList<T> QueryForList<T>(this AdoTemplate adoTemplate, string cmdText, IRowMapper<T> rowMapper)
        {
            return adoTemplate.QueryForList(CommandType.Text, cmdText, null, rowMapper);
        }

        /// <summary>
        /// Executes a query with the specified command text and <typeparamref name="dbParameterSetter"/>, mapping a single result row to a object via a <typeparamref name="rowMapperFunc"/>.
        /// <para>
        /// Don't Call <see cref="IDataReader.Read"/> to get next result.
        /// </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="adoTemplate">The ado template</param>
        /// <param name="cmdText">The command text (Select)</param>
        /// <param name="dbParameterSetter">The parameter setter to bind to the query</param>
        /// <param name="rowMapperFunc">CallBack function that maps a single result to a single object in a rowNum</param>
        /// <returns></returns>
        public static IList<T> QueryForList<T>(this AdoTemplate adoTemplate, string cmdText, IDbParameterSetter dbParameterSetter, Func<IDataReader, int, T> rowMapperFunc)
        {
            var rowMapper = new IRowMapImplementor<T>(rowMapperFunc);
            return adoTemplate.QueryForList(CommandType.Text, cmdText, dbParameterSetter, rowMapper);
        }

        /// <summary>
        /// Executes a query with the specified command text and <typeparamref name="dbParameterSetter"/>, mapping a set of result to a list of object via a <typeparamref name="setMapperFunc"/>.
        /// <para>
        /// Call <see cref="IDataReader.Read"/> to get next result.
        /// </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="adoTemplate">The ado template</param>
        /// <param name="cmdText">The command text (Select)</param>
        /// <param name="dbParameterSetter">The parameter setter to bind to the query</param>
        /// <param name="setMapperFunc">CallBack function that maps a set of result to list of objects</param>
        /// <returns></returns>
        public static IList<T> QueryForList<T>(this AdoTemplate adoTemplate, string cmdText, IDbParameterSetter dbParameterSetter, Func<IDataReader,IList<T>> setMapperFunc)
        {
            var rowMapper = new IResultSetImplementor<T>(setMapperFunc);
            return adoTemplate.QueryForList(CommandType.Text, cmdText, dbParameterSetter, rowMapper);
        }

        /// <summary>
        /// Executes a query with the specified command text and <typeparamref name="dbParameterSetter"/>, mapping a single result row to a object via a <typeparamref name="rowMapper"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="adoTemplate">The ado template</param>
        /// <param name="cmdText">The command text (Select)</param>
        /// <param name="dbParameterSetter">The parameter setter to bind to the query</param>
        /// <param name="rowMapper">maps a single result to a single object</param>
        /// <returns></returns>
        public static IList<T> QueryForList<T>(this AdoTemplate adoTemplate, string cmdText, IDbParameterSetter dbParameterSetter, IRowMapper<T> rowMapper)
        {
            return adoTemplate.QueryForList(CommandType.Text, cmdText, dbParameterSetter, rowMapper);
        }

        /// <summary>
        /// Executes a query with the specified command text, mapping a single result row to a object via a <typeparamref name="rowMapperFunc"/>.
        /// <para>
        /// Don't Call <see cref="IDataReader.Read"/> to get next result.
        /// </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="adoTemplate">The ado template</param>
        /// <param name="cmdType">Text or Stored Procedure</param>
        /// <param name="cmdText">The command text (Select)</param>
        /// <param name="rowMapperFunc">CallBack function that maps a single result to a single object in a rowNum</param>
        /// <returns></returns>
        public static IList<T> QueryForList<T>(this AdoTemplate adoTemplate, CommandType cmdType, string cmdText, Func<IDataReader, int, T> rowMapperFunc)
        {
            var rowMapper = new IRowMapImplementor<T>(rowMapperFunc);
            return adoTemplate.QueryForList(cmdType, cmdText, null, rowMapper);
        }

        /// <summary>
        /// Executes a query with the specified command text, mapping a set of result to a list of object via a <typeparamref name="setMapperFunc"/>.
        /// <para>
        /// Call <see cref="IDataReader.Read"/> to get next result.
        /// </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="adoTemplate">The ado template</param>
        /// <param name="cmdType">Text or Stored Procedure</param>
        /// <param name="cmdText">The command text (Select)</param>
        /// <param name="setMapperFunc">CallBack function that maps a set of result to list of objects</param>
        /// <returns></returns>
        public static IList<T> QueryForList<T>(this AdoTemplate adoTemplate, CommandType cmdType, string cmdText, Func<IDataReader, IList<T>> setMapperFunc)
        {
            var rowMapper = new IResultSetImplementor<T>(setMapperFunc);
            return adoTemplate.QueryForList(cmdType, cmdText, null, rowMapper);
        }



        /// <summary>
        /// Executes a query with the specified command text, mapping a single result row to a object via a <typeparamref name="rowMapper"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="adoTemplate">The ado template</param>
        /// <param name="cmdType">Text or Stored Procedure</param>
        /// <param name="cmdText">The command text (Select)</param>
        /// <param name="rowMapper">maps a single result to a single object</param>
        /// <returns></returns>
        public static IList<T> QueryForList<T>(this AdoTemplate adoTemplate, CommandType cmdType, string cmdText, IRowMapper<T> rowMapper)
        {
            return adoTemplate.QueryForList(cmdType, cmdText, null, rowMapper);
        }

        /// <summary>
        /// Executes a query with the specified command text and <typeparamref name="dbParameterSetter"/>, mapping a single result row to a object via a <typeparamref name="rowMapper"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="adoTemplate">The ado template</param>
        /// <param name="cmdType">Text or Stored Procedure</param>
        /// <param name="cmdText">The command text (Select)</param>
        /// <param name="dbParameterSetter">The parameter setter to bind to the query</param>
        /// <param name="rowMapper">maps a single result to a single object</param>
        /// <returns></returns>
        public static IList<T> QueryForList<T>(this AdoTemplate adoTemplate, CommandType cmdType, string cmdText, IDbParameterSetter dbParameterSetter, IRowMapper<T> rowMapper)
        {
            return adoTemplate.QueryForList(cmdType, cmdText, dbParameterSetter, rowMapper);
        }

        /// <summary>
        /// Executes a query with the specified command text and <typeparamref name="dbParameterSetter"/>, mapping a single result row to a object via a <typeparamref name="rowMapperFunc"/>.
        /// <para>
        /// Don't Call <see cref="IDataReader.Read"/> to get next result.
        /// </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="adoTemplate">The ado template</param>
        /// <param name="cmdType">Text or Stored Procedure</param>
        /// <param name="cmdText">The command text (Select)</param>
        /// <param name="dbParameterSetter">The parameter setter to bind to the query</param>
        /// <param name="rowMapperFunc">CallBack function that maps a single result to a single object in a rowNum</param>
        /// <returns></returns>
        public static IList<T> QueryForList<T>(this AdoTemplate adoTemplate, CommandType cmdType, string cmdText, IDbParameterSetter dbParameterSetter, Func<IDataReader, int, T> rowMapperFunc)
        {
            var rowMapper = new IRowMapImplementor<T>(rowMapperFunc);
            return adoTemplate.QueryForList(cmdType, cmdText, dbParameterSetter, rowMapper);
        }

        /// <summary>
        /// Executes a query with the specified command text and <typeparamref name="dbParameterSetter"/>, mapping a set of result to a list of object via a <typeparamref name="setMapperFunc"/>.
        /// <para>
        /// Call <see cref="IDataReader.Read"/> to get next result.
        /// </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="adoTemplate">The ado template</param>
        /// <param name="cmdType">Text or Stored Procedure</param>
        /// <param name="cmdText">The command text (Select)</param>
        /// <param name="dbParameterSetter">The parameter setter to bind to the query</param>
        /// <param name="setMapperFunc">CallBack function that maps a set of result to list of objects</param>
        /// <returns></returns>
        public static IList<T> QueryForList<T>(this AdoTemplate adoTemplate, CommandType cmdType, string cmdText, IDbParameterSetter dbParameterSetter, Func<IDataReader, IList<T>> setMapperFunc)
        {
            var rowMapper = new IResultSetImplementor<T>(setMapperFunc);
            return adoTemplate.QueryForList(cmdType, cmdText, dbParameterSetter, rowMapper);
        }
        /// <summary>
        /// Executes a query with the specified command text, <paramref name="dbParameterSetter"/>, exposing a <see cref="IDataReader"/> via a callback action
        /// </summary>
        /// <param name="adoTemplate"></param>
        /// <param name="cmdType">Text or Stored Procedure</param>
        /// <param name="cmdText">The command text (Select)</param>
        /// <param name="dbParameterSetter">The parameter setter to bind to the query</param>
        /// <param name="accessor">callback action that expose a <see cref="IDataReader"/></param>
        public static void ExecuteQuery(this AdoTemplate adoTemplate, CommandType cmdType, string cmdText, IDbParameterSetter dbParameterSetter, Action<IDataReader> accessor)
        {
            var accessorImp = new IDataReaderAccessorImplementor(accessor);
            adoTemplate.ExecuteQuery(cmdType, cmdText, dbParameterSetter, accessorImp);
        }

        /// <summary>
        /// Executes a non query with the specified command text, exposing a <see cref="IDataReader"/> via a callback action
        /// </summary>
        /// <param name="adoTemplate"></param>
        /// <param name="cmdType">Text or Stored Procedure</param>
        /// <param name="cmdText">The command text (Select)</param>
        /// <param name="accessor">callback action that expose a <see cref="IDataReader"/></param>
        public static void ExecuteQuery(this AdoTemplate adoTemplate, CommandType cmdType, string cmdText, Action<IDataReader> accessor)
        {
            var accessorImp = new IDataReaderAccessorImplementor(accessor);
            adoTemplate.ExecuteQuery(cmdType, cmdText, null, accessorImp);
        }

        /// <summary>
        /// Executes a query with the specified command text, exposing a <see cref="IDataReader"/> via a callback action
        /// </summary>
        /// <param name="adoTemplate"></param>
        /// <param name="cmdText">The command text (Select)</param>
        /// <param name="dbParameterSetter">The parameter setter to bind to the query</param>
        /// <param name="accessor">callback action that expose a <see cref="IDataReader"/></param>
        public static void ExecuteQuery(this AdoTemplate adoTemplate, string cmdText, IDbParameterSetter dbParameterSetter, Action<IDataReader> accessor)
        {
            var accessorImp = new IDataReaderAccessorImplementor(accessor);
            adoTemplate.ExecuteQuery(CommandType.Text, cmdText, dbParameterSetter, accessorImp);
        }

        /// <summary>
        /// Executes a query with the specified command text, exposing a <see cref="IDataReader"/> via a callback action
        /// </summary>
        /// <param name="adoTemplate"></param>
        /// <param name="cmdText">The command text (Select)</param>
        /// <param name="accessor">callback action that expose a <see cref="IDataReader"/></param>
        public static void ExecuteQuery(this AdoTemplate adoTemplate, string cmdText, Action<IDataReader> accessor)
        {
            var accessorImp = new IDataReaderAccessorImplementor(accessor);
            adoTemplate.ExecuteQuery(CommandType.Text, cmdText, null, accessorImp);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="adoTemplate">The ado template</param>
        /// <param name="cmdText">The command text (Select, Insert, Update, Delete)</param>
        /// <returns></returns>
        public static DataTable QueryForDataTable(this AdoTemplate adoTemplate, string cmdText)
        {
            return adoTemplate.QueryForDataTable(CommandType.Text, cmdText, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="adoTemplate">The ado template</param>
        /// <param name="cmdText">The command text (Select, Insert, Update, Delete)</param>
        /// <param name="dbParameterSetter">The parameter setter to bind to the query</param>
        /// <returns></returns>
        public static DataTable QueryForDataTable(this AdoTemplate adoTemplate, string cmdText, IDbParameterSetter dbParameterSetter)
        {
            return adoTemplate.QueryForDataTable(CommandType.Text, cmdText, dbParameterSetter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="adoTemplate">The ado template</param>
        /// <param name="cmdType">Text or Stored Procedure</param>
        /// <param name="cmdText">The command text (Select, Insert, Update, Delete)</param>
        /// <returns></returns>
        public static DataTable QueryForDataTable(this AdoTemplate adoTemplate, CommandType cmdType, string cmdText)
        {
            return adoTemplate.QueryForDataTable(cmdType, cmdText, null);
        }



    }
}
