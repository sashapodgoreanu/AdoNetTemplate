using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetTemplate.Database.Core
{
    /// <summary>
    /// Provides a mechanism to execute Query against a Data Base.
    /// </summary>
    /// <author>Sasha</author>
    public interface IAdoOperations
    {
        /// <summary>
        /// Execute a ADO.NET operation on a command object using a generic interface based callback.
        /// </summary>
        /// <typeparam name="T">The type of object returned from the callback.</typeparam>
        /// <param name="action">The callback to execute based on <see cref="IDbCommandCallback{T}"/></param>
        /// <returns>An object returned from callback</returns>
        T Execute<T>(IDbCommandCallback<T> action);

        /// <summary>
        /// Execute a ADO.NET operation on a command object using a generic interface based callback.
        /// </summary>
        /// <typeparam name="T">The type of object returned from the callback.</typeparam>
        /// <param name="action">The callback to execute based on <see cref="IDataAdapterCallback{T}"/></param>
        /// <returns>An object returned from callback</returns>
        T Execute<T>(IDataAdapterCallback<T> dataAdapterCallback);

        /// <summary>
        ///  Executes an SQL statement and returns the number of rows affected. Insert, Update, Delete
        /// </summary>
        /// <param name="cmdType">Text or Stored Procedure</param>
        /// <param name="cmdText">The command text (Insert, Update, Delete)</param>
        /// <param name="dbParameterSetter">The parameter setter to bind to the query</param>
        /// <returns></returns>
        int ExecuteNonQuery(CommandType cmdType, string cmdText, IDbParameterSetter dbParameterSetter);

        /// <summary>
        /// Executes the query, and returns the first column of the first row in the resultset
        /// returned by the query. Extra columns or rows are ignored.
        /// </summary>
        /// <param name="cmdType">Text or Stored Procedure</param>
        /// <param name="cmdText">The command text (Select)</param>
        /// <param name="dbParameterSetter">The parameter setter to bind to the query</param>
        /// <returns></returns>
        object ExecuteScalar(CommandType cmdType, string cmdText, IDbParameterSetter dbParameterSetter);

        /// <summary>
        /// Executes the query, and returns the first column of the first row in the resultset
        /// returned by the query. Extra columns or rows are ignored.
        /// <see cref="IAdoOperations.ExecuteScalar{T}(CommandType, string)"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmdType">Text or Stored Procedure</param>
        /// <param name="cmdText">The command text (Select)</param>
        /// <param name="dbParameterSetter">The parameter setter to bind to the query</param>
        /// <returns></returns>
        DataTable QueryForDataTable(CommandType cmdType, string cmdText, IDbParameterSetter dbParameterSetter);


        /// <summary>
        /// Executes a query with the specified command text and <typeparamref name="dbParameterSetter"/>, mapping a result to an object via a <typeparamref name="rowMapper"/> and post process it via a <typeparamref name="objectFormatter"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmdType">Text or Stored Procedure</param>
        /// <param name="cmdText">The command text (Select)</param>
        /// <param name="dbParameterSetter">The parameter setter to bind to the query</param>
        /// <param name="rowMapper">maps a result to an object</param> 
        /// <returns></returns>
        T QueryForObject<T>(CommandType cmdType, string cmdText, IDbParameterSetter dbParameterSetter, IRowMapper<T> rowMapper);

        /// <summary>
        /// Executes a query with the specified command text and <typeparamref name="dbParameterSetter"/>, mapping a result to an object via a <typeparamref name="rowMapper"/> and post process it via a <typeparamref name="objectFormatter"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmdType">Text or Stored Procedure</param>
        /// <param name="cmdText">The command text (Select)</param>
        /// <param name="dbParameterSetter">The parameter setter to bind to the query</param>
        /// <param name="rowMapper">maps a result to an object</param> 
        /// <returns></returns>
        IList<T> QueryForList<T>(CommandType cmdType, string cmdText, IDbParameterSetter dbParameterSetter, IRowMapper<T> rowMapper);

        /// <summary>
        /// Executes a non query with the specified command text <typeparamref name="dbParameterSetter"/>, mapping a set result row to a list of object via a <typeparamref name="resultSetExtractor"/> and post process with a <typeparamref name="resultSetProcessor"/>
        /// </summary>
        /// <typeparam name="T">the return type</typeparam>
        /// <param name="cmdType">Text or Stored Procedure</param>
        /// <param name="cmdText">The command text (Select)</param>
        /// <param name="dbParameterSetter">The parameter setter to bind to the query</param>
        /// <param name="resultSetExtractor">Maps a set of result to list of objects</param>
        /// <param name="resultSetProcessor">Post process a list of objects</param>
        /// <returns></returns>
        T ExecuteQuery<T>(CommandType cmdType, string cmdText, IDbParameterSetter dbParameterSetter, IResultSetExtractor<T> resultSetExtractor);


        /// <summary>
        /// Begins a transaction.
        /// Only one transaction should be active per instance.
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Commits the database transaction associated with this instance.
        /// </summary>
        void Commit();

        /// <summary>
        /// Rolls back a transaction associated with this instance from a pending state.
        /// </summary>
        void RollBack();

        /// <summary>
        /// An instance of a DbProvider implementation.
        /// </summary>
        IDbProvider DbProvider { get; }

    }
}
