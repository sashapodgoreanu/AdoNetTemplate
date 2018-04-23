using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetTemplate.Database.Core
{
    /// <summary>
    /// Factory interface to create and operate on a specific ADO.NET connection and transaction.
    /// </summary>
    /// <author>Sasha</author>
    public interface IDbProvider : IDisposable
    {

        /// <summary>
        /// Returns the current Connection if exists.
        /// </summary>
        DbConnection Connection
        {
            get;
        }


        /// <summary>
        /// Returns the current transaction if exists.
        /// </summary>
        DbTransaction Transaction
        {
            get;
        }

        /// <summary>
        /// Close the current connection. The connection will not be closed if has an active transaction
        /// </summary>
        void CloseConnection();

        /// <summary>
        /// Opens a new or created connection.
        /// </summary>
        void OpenConnection();

        /// <summary>
        /// Begin a new transaction.
        /// </summary>
        void BeginTransaction();

        void BeginTransaction(IsolationLevel il);

        /// <summary>
        /// Commit the current transaction.
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Rollback the current transaction.
        /// </summary>
        void RollbackTransaction();
    }
}
