using AdoNetTemplate.Database.Core;
using AdoNetTemplate.Database.Support;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;


[assembly: InternalsVisibleTo("AdoOracleUnitTest")]
[assembly: InternalsVisibleTo("AdoNetTemplate.Tests")]
namespace AdoNetTemplate
{
    /// <summary>
    /// Template methods that implements the functionality to execute Query against a Data Base.
    /// 
    /// Sasha
    /// </summary>

    public sealed class AdoTemplate : IAdoOperations, IDisposable
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private bool disposeProviderExternally;

        /// <summary>
        /// Gets the connection provider of this class
        /// </summary>
        public IDbProvider DbProvider { get; private set; }

        /// <summary>
        /// Creates new AdoTemplate with an external DBProvider.
        /// </summary>
        /// <param name="dbProvider">a provided Data Base Provider</param>
        /// <param name="disposeProviderExternally">if true, this class will not dispose the <paramref name="dbProvider"/></param>
        public AdoTemplate(IDbProvider dbProvider, bool disposeProviderExternally = true)
        {
            DbProvider = dbProvider;
            this.disposeProviderExternally = disposeProviderExternally;
        }

        public void TestConnection()
        {
            DbProvider.OpenConnection();
        }

        #region Dispose Implementation

        /// <summary>
        /// Destructor
        /// </summary>
        ~AdoTemplate()
        {
            Dispose(false);
        }


        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the
        // runtime from inside the finalizer and you should not reference
        // other objects. Only unmanaged resources can be disposed.
        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!disposeProviderExternally)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    DbProvider.Dispose();
                }

                // Call here the appropriate methods to clean up
                // unmanaged resources here.
                // If disposing is false,
                // only the following code is executed.

                //example:
                //CloseHandle(handle);
                //handle = IntPtr.Zero;

                // Note disposing has been done.
                var disposed = true;
                disposeProviderExternally = disposed;
            }
        }

        // Implement IDisposable.
        // Do not make this method virtual.
        // A derived class should not be able to override this method.
        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Execute

        ///Execute are the main methods of <see cref="AdoTemplate"/> class.
        ///All other methods will finally pass through one of 
        ///<see cref="AdoTemplate.Execute{T}(IDataAdapterCallback{T})"/> 
        ///or
        ///<see cref="AdoTemplate.Execute{T}(IDbCommandCallback{T})"/> methods.
        /// Direct use of this method is not recommended.
        /// <summary>
        ///Execute a ADO.NET operation on a command object using a generic interface based callback.
        /// </summary>
        /// <typeparam name="T">return type</typeparam>
        /// <param name="action">the action to be executed</param>
        /// <returns></returns>
        public T Execute<T>(IDbCommandCallback<T> action)
        {
            IDbCommand command = null;
            try
            {
                DbProvider.OpenConnection();
                command = DbProvider.CreateCommand();
                command.Connection = DbProvider.Connection;
                command.Transaction = DbProvider.Transaction;

                T result = default(T);
                result = action.DoInCommand(command);

                return result;
            }
            catch (DataException)
            {
                throw;
            }
            catch (DbException e)
            {
                throw new DataException($"Failed to execute a command cllback: {e.Message}", e);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                ConnectionUtils.DisposeCommand(command);
                ConnectionUtils.ClosseConnection(DbProvider);
            }
        }

        /// <summary>
        /// Execute a ADO.NET operation on a command object using a generic interface based callback.
        /// Direct use of this method is not recommended.
        /// </summary>
        /// <typeparam name="T">return type</typeparam>
        /// <param name="action">the action to be executed</param>
        public T Execute<T>(IDataAdapterCallback<T> action)
        {
            IDbDataAdapter dataAdapter = null;
            try
            {
                DbProvider.OpenConnection();
                dataAdapter = DbProvider.CreateDataAddapter();
                dataAdapter.SelectCommand = DbProvider.CreateCommand();
                dataAdapter.SelectCommand.Connection = DbProvider.Connection;
                dataAdapter.SelectCommand.Transaction = DbProvider.Transaction;

                T result = default(T);
                result = action.DoInDataAdapter(dataAdapter);
                return result;

            }
            catch (DataException)
            {
                throw;
            }
            catch (DbException e)
            {
                throw new DataException($"Failed to execute a command cllback: {e.Message}", e);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                ConnectionUtils.DisposeDataAdapterCommands(dataAdapter);
                ConnectionUtils.ClosseConnection(DbProvider);
            }
        }
        #endregion

        /// <summary>
        ///  Executes an SQL statement and returns the number of rows affected. Insert, Update, Delete.
        /// </summary>
        /// <param name="cmdType">Text or Stored Procedure</param>
        /// <param name="cmdText">The command text (Insert, Update, Delete)</param>
        /// <param name="dbParameterSetter">The parameter setter to bind to the query</param>
        /// <returns></returns>
        public int ExecuteNonQuery(CommandType cmdType, string cmdText, IDbParameterSetter dbParameterSetter)
        {
            if (string.IsNullOrEmpty(cmdText))
            {
                throw new ArgumentNullException($"{nameof(cmdText)}: CommandText must be not null");
            }
            NonQueryCallBack t = new NonQueryCallBack(cmdType, cmdText, dbParameterSetter);

            return Execute(t);
        }

        /// <summary>
        /// Executes the query, and returns the first column of the first row in the resultset
        /// returned by the query. Extra columns or rows are ignored.
        /// <see cref="IAdoOperations.ExecuteScalar(CommandType, string)"/>
        /// </summary>
        /// <param name="cmdType">Text or Stored Procedure</param>
        /// <param name="cmdText">The command text (Select)</param>
        /// <param name="dbParameterSetter">The parameter setter to bind to the query</param>
        /// <returns></returns>
        public object ExecuteScalar(CommandType cmdType, string cmdText, IDbParameterSetter dbParameterSetter)// where T : IConvertible
        {
            if (string.IsNullOrEmpty(cmdText))
            {
                throw new ArgumentNullException($"{nameof(cmdText)}: CommandText must be not null");
            }
            return Execute(new ExecuteScalarCallbackWithParameters(cmdType, cmdText, dbParameterSetter));
        }


        #region Query for DataTable

        /// <summary>
        /// Executes a command text, binding the results to a <see cref="DataTable"/>.
        /// </summary>
        /// <param name="cmdType">Text or Stored Procedure</param>
        /// <param name="cmdText">The command text (Select)</param>
        /// <param name="dbParameterSetter">The parameter setter to bind to the query</param>
        /// <returns></returns>
        public DataTable QueryForDataTable(CommandType cmdType, string cmdText, IDbParameterSetter dbParameterSetter)
        {
            DataTable dataTable = CreateDataTable();
            DataTableFillWithParams(dataTable, cmdType, cmdText, dbParameterSetter);
            return dataTable;
        }

        private DataTable CreateDataTable()
        {
            DataTable dataTable = new DataTable();
            dataTable.Locale = CultureInfo.InvariantCulture;
            return dataTable;
        }

        private int DataTableFill(DataTable dataTable, CommandType commandType, string sql)
        {

            return (Execute(new DataAdapterFillCallbackWithParameters(dataTable, commandType, sql, null)));
        }

        private int DataTableFillWithParams(DataTable dataTable, CommandType commandType, string sql,
                                           IDbParameterSetter dbParameterSetter)
        {

            return (Execute(new DataAdapterFillCallbackWithParameters(dataTable, commandType, sql, dbParameterSetter)));
        }

        #endregion

        /// <summary>
        /// Executes a query with the specified command text and <paramref name="dbParameterSetter"/>, mapping a result to an object via a <paramref name="rowMapper"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmdType">Text or Stored Procedure</param>
        /// <param name="cmdText">The command text (Select)</param>
        /// <param name="dbParameterSetter">The parameter setter to bind to the query</param>
        /// <param name="rowMapper">maps a result to an object</param>
        /// <returns></returns>
        public IList<T> QueryForList<T>(CommandType cmdType, string cmdText, IDbParameterSetter dbParameterSetter, IRowMapper<T> rowMapper)
        {
            return ExecuteQuery(cmdType, cmdText, dbParameterSetter, new RowMapperResultSetExtractor<T>(rowMapper));
        }

        /// <summary>
        /// Executes a query with the specified command text and <paramref name="dbParameterSetter"/>, mapping a set result row to a list of object via a <paramref name="resultSetExtractor"/>
        /// </summary>
        /// <typeparam name="T">the return type</typeparam>
        /// <param name="cmdType">Text or Stored Procedure</param>
        /// <param name="cmdText">The command text (Select)</param>
        /// <param name="dbParameterSetter">The parameter setter to bind to the query</param>
        /// <param name="resultSetExtractor">Maps a set of result to list of objects</param>
        /// <returns></returns>
        public IList<T> QueryForList<T>(CommandType cmdType, string cmdText, IDbParameterSetter dbParameterSetter, IResultSetExtractor<IList<T>> resultSetExtractor)
        {
            return ExecuteQuery(cmdType, cmdText, dbParameterSetter, resultSetExtractor);
        }

        /// <summary>
        /// Executes a query with the specified command text <paramref name="dbParameterSetter"/>, mapping a set result row to a list of object via a <paramref name="resultSetExtractor"/>
        /// </summary>
        /// <typeparam name="T">the return type</typeparam>
        /// <param name="cmdType">Text or Stored Procedure</param>
        /// <param name="cmdText">The command text (Select)</param>
        /// <param name="dbParameterSetter">The parameter setter to bind to the query</param>
        /// <param name="resultSetExtractor">Maps a set of result to list of objects</param>
        /// <returns></returns>
        public T ExecuteQuery<T>(CommandType cmdType, string cmdText, IDbParameterSetter dbParameterSetter, IResultSetExtractor<T> resultSetExtractor)
        {
            if (string.IsNullOrEmpty(cmdText))
            {
                throw new ArgumentNullException($"{nameof(cmdText)}: CommandText must be not null");
            }

            if (resultSetExtractor == null)
            {
                throw new ArgumentNullException($"{nameof(resultSetExtractor)}: {nameof(IResultSetExtractor<T>)} must not be null");
            }

            return Execute(new QueryCallback<T>(cmdType, cmdText, dbParameterSetter, resultSetExtractor));
        }

        /// <summary>
        /// Executes a query with the specified command text, <paramref name="dbParameterSetter"/>, exposing a <see cref="IDataReader"/> via a <paramref name="dataReaderAccessor"/>
        /// </summary>
        /// <param name="cmdType">Text or Stored Procedure</param>
        /// <param name="cmdText">The command text (Select)</param>
        /// <param name="dbParameterSetter">The parameter setter to bind to the query</param>
        /// <param name="dataReaderAccessor">The object that expose the <see cref="IDataReader"/></param>
        public void ExecuteQuery(CommandType cmdType, string cmdText, IDbParameterSetter dbParameterSetter, IDataReaderAccessor dataReaderAccessor)
        {
            if (string.IsNullOrEmpty(cmdText))
            {
                throw new ArgumentNullException($"{nameof(cmdText)}: CommandText must be not null");
            }

            if (dataReaderAccessor == null)
            {
                throw new ArgumentNullException($"{nameof(dataReaderAccessor)}: {nameof(IDataReaderAccessor)} must be not null");
            }

            Execute(new QueryCallbackDBDataReader(cmdType, cmdText, dbParameterSetter, dataReaderAccessor));
        }

        #region Transaction operations
        /// <summary>
        /// Wrapper method to begin a new transaction
        /// </summary>
        public void BeginTransaction()
        {
            try
            {
                DbProvider.BeginTransaction();
            }
            catch (DbException e)
            {
                throw new TransactionException($"Cannot begin a transaction: {e.Message} ", e);
            }
            catch (DataException e)
            {
                throw new TransactionException($"Cannot begin a transaction: {e.Message}", e);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Wrapper method to commit a current transaction if exists
        /// </summary>
        public void Commit()
        {
            try
            {
                DbProvider.CommitTransaction();
            }
            catch (DataException)
            {
                throw;
            }
            catch (DbException e)
            {
                throw new DataException($"Failed to execute a command cllback: {e.Message}", e);
            }
            catch (TransactionException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Wrapper method to rollback a current transaction if exists
        /// </summary>
        public void RollBack()
        {
            try
            {
                DbProvider.RollbackTransaction();
            }
            catch (Exception ex)
            {
                logger.Warn(ex.Message);
            }
        }

        #endregion

        #region internal use class

        /// <summary>
        /// Callback class used to execute a query against a database.
        /// </summary>
        /// <typeparam name="T">type of the returned result.</typeparam>
        internal class QueryCallback<T> : IDbCommandCallback<T>, ICommandTextProvider
        {
            private static Logger logger = LogManager.GetCurrentClassLogger();

            protected IResultSetExtractor<T> _rse;

            protected CommandType _commandType;

            protected string _commandText;
            protected IDbParameterSetter dbParameterSetter;

            public QueryCallback(CommandType cmdType,
                     string cmdText,
                     IDbParameterSetter dbParameterSetter,
                     IResultSetExtractor<T> rse
                     ) : this( cmdType, cmdText, rse)
            {

                this.dbParameterSetter = dbParameterSetter;
            }

            public QueryCallback(CommandType cmdType,
                                 string cmdText,
                                 IResultSetExtractor<T> rse)
            {
                _commandType = cmdType;
                _commandText = cmdText;
                this._rse = rse;
            }

            public string CommandText
            {
                get { return _commandText; }
            }

            public virtual T DoInCommand(IDbCommand command)
            {
                try
                {
                    command.CommandType = _commandType;
                    command.CommandText = CommandText;

                    if (dbParameterSetter != null)
                    {
                        dbParameterSetter.SetUpParameters(command.Parameters);
                        dbParameterSetter.SetUpCommand(command);
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        T returnValue = default(T);

                        returnValue = _rse.ExtractData(reader);

                        reader.Close();
                        return returnValue;
                    }
                }
                catch (DbException ex)
                {
#if DEBUG
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine($"{nameof(DbException)} was thrown.");
                    sb.AppendLine("Exception Details:");
                    sb.AppendLine(ex.Message);
                    sb.AppendLine($"{nameof(CommandText)}: {CommandText}");
                    sb.AppendLine($"{nameof(command)}: {(command.PrintForLogger())}");
                    logger.Debug(sb.ToString());
#endif
                    throw;
                }
            }
        }

        /// <summary>
        /// Helper Callback class used to expose directly a <see cref="IDataReader"/> to it's caller. 
        /// </summary>
        internal class QueryCallbackDBDataReader : IDbCommandCallback<bool>, ICommandTextProvider
        {
            private static Logger logger = LogManager.GetCurrentClassLogger();

            private IDataReaderAccessor dataReaderAccessor;

            private CommandType _commandType;

            private string _commandText;
            private IDbParameterSetter dbParameterSetter;

            public QueryCallbackDBDataReader(CommandType cmdType,
                     string cmdText,
                     IDbParameterSetter dbParameterSetter,
                     IDataReaderAccessor dataReaderAccessor
                     ) : this(cmdType, cmdText, dataReaderAccessor)
            {
                this.dbParameterSetter = dbParameterSetter;
            }

            public QueryCallbackDBDataReader(CommandType cmdType,
                                 string cmdText,
                                 IDataReaderAccessor dataReaderAccessor)
            {
                _commandType = cmdType;
                _commandText = cmdText;
                this.dataReaderAccessor = dataReaderAccessor;
            }

            public string CommandText
            {
                get { return _commandText; }
            }

            public bool DoInCommand(IDbCommand command)
            {
                try
                {
                    command.CommandType = _commandType;
                    command.CommandText = CommandText;

                    if (dbParameterSetter != null)
                    {
                        dbParameterSetter.SetUpParameters(command.Parameters);
                        dbParameterSetter.SetUpCommand(command);
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        dataReaderAccessor.AccessDatareader(reader);
                        reader.Close();
                    }

                    return true;
                }
                catch (DbException ex)
                {
#if DEBUG
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine($"{nameof(DbException)} was thrown.");
                    sb.AppendLine("Exception Details:");
                    sb.AppendLine(ex.Message);
                    sb.AppendLine($"{nameof(CommandText)}: {CommandText}");
                    sb.AppendLine($"{nameof(command)}: {(command.PrintForLogger())}");
                    logger.Debug(sb.ToString());
#endif
                    throw;
                }
            }
        }

        /// <summary>
        /// Callback class used to execute a non query. It's main method returns number of rows affected.
        /// </summary>
        internal class NonQueryCallBack : IDbCommandCallback<int>, ICommandTextProvider
        {
            private static Logger logger = LogManager.GetCurrentClassLogger();

            private CommandType _commandType;
            private string _commandText;

            private IDbParameterSetter dbParameterSetter;

            public NonQueryCallBack(
                                 CommandType cmdType,
                                 string cmdText,
                                 IDbParameterSetter dbParameterSetter)
            {
                _commandType = cmdType;
                _commandText = cmdText;
                this.dbParameterSetter = dbParameterSetter;
            }

            public string CommandText
            {
                get { return _commandText; }
            }

            public int DoInCommand(IDbCommand command)
            {
                try
                {
                    command.CommandType = _commandType;
                    command.CommandText = CommandText;

                    if (dbParameterSetter != null)
                    {
                        dbParameterSetter.SetUpParameters(command.Parameters);
                        dbParameterSetter.SetUpCommand(command);
                    }
                    int returnValue = command.ExecuteNonQuery();

                    return returnValue;
                }
                catch (DbException ex)
                {
#if DEBUG
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine($"{nameof(DbException)} was thrown.");
                    sb.AppendLine("Exception Details:");
                    sb.AppendLine(ex.Message);
                    sb.AppendLine($"{nameof(CommandText)}: {CommandText}");
                    sb.AppendLine($"{nameof(command)}: {(command.PrintForLogger())}");
                    logger.Debug(sb.ToString());
#endif
                    throw;
                }
            }
        }

        /// <summary>
        /// Callback class used to execute a query and fill a <see cref="DataTable"/>.
        /// </summary>
        internal class DataAdapterFillCallbackWithParameters : IDataAdapterCallback<int>
        {
            private DataTable _dataTable;
            private string _commandText;
            private CommandType _commandType;

            private static Logger logger = LogManager.GetCurrentClassLogger();


            private IDbParameterSetter dbParameterSetter;

            public DataAdapterFillCallbackWithParameters( DataTable dataTable,
                                            CommandType cmdType,
                                            string cmdText,
                                            IDbParameterSetter dbParameterSetter)
            {
                _dataTable = dataTable;
                _commandType = cmdType;
                _commandText = cmdText;
                this.dbParameterSetter = dbParameterSetter;
            }

            /// <summary>
            /// Return number of rows affected.
            /// </summary>
            /// <param name="dataAdapter"></param>
            /// <returns></returns>
            public int DoInDataAdapter(IDbDataAdapter dataAdapter)
            {
                int retVal = 0;
                dataAdapter.SelectCommand.CommandType = _commandType;
                dataAdapter.SelectCommand.CommandText = _commandText;

                if (dbParameterSetter != null)
                {
                    dbParameterSetter.SetUpParameters(dataAdapter.SelectCommand.Parameters);
                    dbParameterSetter.SetUpCommand(dataAdapter.SelectCommand);
                }

                if (dataAdapter is DbDataAdapter)
                {
                    retVal = ((DbDataAdapter)dataAdapter).Fill(_dataTable);
                }
                else
                {
                    throw new DataException("Provider does not support filling DataTable directly");
                }
                
                return retVal;
            }
        }

        /// <summary>
        /// Callback class that executes a Scalar query.
        /// </summary>
        internal class ExecuteScalarCallbackWithParameters : IDbCommandCallback<object>, ICommandTextProvider
        {
            private CommandType _commandType = CommandType.Text;
            private string commandText;
            private IDbParameterSetter dbParameterSetter;

            public ExecuteScalarCallbackWithParameters(CommandType cmdType, string cmdText, IDbParameterSetter dbParameters)
            {
                _commandType = cmdType;
                commandText = cmdText;
                dbParameterSetter = dbParameters;
            }

            public string CommandText
            {
                get { return commandText; }
            }

            public object DoInCommand(IDbCommand command)
            {


                try
                {
                    command.CommandType = _commandType;
                    command.CommandText = CommandText;

                    if (dbParameterSetter != null)
                    {
                        dbParameterSetter.SetUpParameters(command.Parameters);
                        dbParameterSetter.SetUpCommand(command);
                    }

                    return command.ExecuteScalar();
                }
                catch (DbException)
                {
#if DEBUG
                    var sb = new StringBuilder();
                    sb.AppendLine($"{nameof(DbException)} was thrown.");
                    sb.AppendLine("Exception Details:");
                    sb.AppendLine($"{nameof(CommandText)}: {CommandText}");
                    sb.AppendLine($"{nameof(command)}: {(command.PrintForLogger())}");
                    logger.Debug(sb.ToString());
#endif
                    throw;
                }
            }
        }

        #endregion
    }
}
