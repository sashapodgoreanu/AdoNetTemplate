using AdoNetTemplate.Database.Core;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace AdoNetTemplate.Database.Generic
{
    /// <summary>
    /// This class manage the connections and transactions.
    /// </summary>
    public abstract class GenericDbProvider<DbCon, DbTrx> : IDbProvider
        where DbCon : DbConnection
        where DbTrx : DbTransaction

    {
        private string _connectionString;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private DbCon connection;
        private DbTrx transaction;
        private bool disposed;

        /// <summary>
        /// The method to be invoked before calling commit on <see cref="GenericDbProvider.transaction"/>.
        /// 
        /// </summary>
        protected EventHandler CommitObserver;

        protected abstract DbCon CreateConnection();

        public GenericDbProvider(string connectionString)
        {
            ConnectionString = connectionString ?? throw new ArgumentNullException("Connection string is null.");
        }

        public GenericDbProvider(DbCon connection)
        {
            this.connection = connection ?? throw new ArgumentNullException("connection is null.");
        }

        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }

            set
            {
                _connectionString = value;
            }
        }


        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        public virtual void OpenConnection()
        {
            try
            {
                if (connection != null && !string.IsNullOrWhiteSpace(connection.DataSource))
                {
                    OpenConnectionInternal();
                }
                else
                {
                    logger.Trace($"Creating new connection");
                    //we don't have a connection.
                    connection = CreateConnection();
                    connection.ConnectionString = ConnectionString;
                    OpenConnectionInternal();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Cannot create connection");
                throw new DataException("Cannot create connection", ex);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public void CloseConnection()
        {
            connection?.Close();
        }


        private void OpenConnectionInternal()
        {
            if (connection == null)
                throw new DataException("OpenConnection", new InvalidOperationException("Connection is null"));

            switch (connection?.State)
            {
                case ConnectionState.Broken:
                    logger.Warn($"Connection is broken. I try to close and open.");
                    connection.Close();
                    connection.Open();
                    logger.Trace("Connection opened.");
                    break;
                case ConnectionState.Closed:
                    logger.Trace($"Connection is closed. I try to open.");
                    connection.Open();
                    logger.Trace("Connection opened.");
                    break;
                default:
                    logger.Trace($"Connection is {connection.State.ToString()}");
                    //DO NOTHING IF:
                    //Connecting
                    //Executing
                    //Fetching
                    break;

            }
        }

        public virtual void BeginTransaction()
        {
            BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
        }

        public virtual void BeginTransaction(System.Data.IsolationLevel il)
        {
            if (connection == null)
                throw new TransactionException("BeginTransaction", new InvalidOperationException("Connection is null"));

            try
            {
                transaction = Connection.BeginTransaction(il) as DbTrx;
                logger.Trace($"BeginTransaction {0}, isolationLevel={1}", transaction.GetHashCode(), transaction.IsolationLevel.ToString());
            }
            catch (System.Exception ex)
            {
                logger.Error(ex, "BeginTransaction");
                throw new TransactionException("BeginTransaction", ex);
            }
        }

        /// <summary>
        /// Rise an event before commit
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnCommit(EventArgs e)
        {
            CommitObserver?.Invoke(this, e);
        }

        public void CommitTransaction()
        {
            OnCommit(EventArgs.Empty);
            transaction.Commit();
            transaction.Dispose();
            transaction = null;
        }

        public void RollbackTransaction()
        {
            transaction.Rollback();
            transaction.Dispose();
            transaction = null;
        }

        /// <summary>
        /// 
        /// </summary>
        public DbTrx Transaction
        {
            get
            {
                return transaction;
            }

            set
            {
                transaction = value;
            }
        }



        /// <summary>
        /// TODO
        /// </summary>
        DbTransaction IDbProvider.Transaction
        {
            get
            {
                return transaction;
            }
        }

        /// <summary>
        /// Returns an opened connection.
        /// </summary>
        public DbCon Connection
        {
            get
            {
                return connection; 
            }
        }

        DbConnection IDbProvider.Connection
        {
            get
            {
                return connection;
            }
        }

        #region Dispose Implementation


        ~GenericDbProvider()
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
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources. 
                    try
                    {
                        logger.Trace($"Disposing transaction {transaction}");
                        transaction?.Rollback();
                        transaction?.Dispose();
                        transaction = null;

                        logger.Trace($"Disposing connection {connection}");
                        connection?.Close();
                        connection?.Dispose();
                        connection = null;
                    }
                    catch (Exception ex)
                    {
                        //ignore exceptions
                        logger.Warn(ex.Message);
                    }

                }

                // Call here the appropriate methods to clean up
                // unmanaged resources here.
                // If disposing is false,
                // only the following code is executed.

                //example:
                //CloseHandle(handle);
                //handle = IntPtr.Zero;

                // Note disposing has been done.
                disposed = true;

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
    }
}
