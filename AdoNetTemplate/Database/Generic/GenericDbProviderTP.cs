using AdoNetTemplate.Database.Core;
using AdoNetTemplate.Database.Generic;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace AdoNetTemplate.Database.Generic
{
    /// <summary>
    /// Manage a generic 
    /// </summary>
    public class GenericDataAccessProviderTP<DbCon, DbTrx> : GenericDbProvider<DbCon, DbTrx>, ITransactionParticipant
        where DbCon : DbConnection, new()
        where DbTrx : DbTransaction
    {
        /// <summary>
        /// Represents the method to be invoked before the commit of the transaction.
        /// </summary>
        event EventHandler ITransactionParticipant.CommitObserver
        {
            add
            {
                base.CommitObserver += value;
            }

            remove
            {
                base.CommitObserver -= value;
            }
        }

        public GenericDataAccessProviderTP(string connectionString) : base(connectionString)
        {

        }

        public void Abort()
        {
            base.Transaction?.Dispose();
            base.Transaction = null;
        }

        public void Clean()
        {
            base.Transaction?.Dispose();
            base.Transaction = null;
            base.Connection.Close();
        }

        public void Commit()
        {
            try
            {
                base.CommitTransaction();
            }
            catch (System.Exception ex)
            {
                throw new TransactionException("Failed to Commit", ex);
            }
            
        }

        public void Prepare()
        {
            try
            {
                base.OpenConnection();
                base.BeginTransaction();
            }
            catch (System.Exception ex)
            {
                throw new TransactionException("Failed to Prepare", ex);
            }
            
        }

        public void Rollback()
        {
            try
            {
                base.RollbackTransaction();
            }
            catch (System.Exception ex)
            {
                throw new TransactionException("Failed to Rollback", ex);
            }
        }


        public string TraceTransaction()
        {
            return string.Format($"Transaction {Transaction?.GetHashCode()} isolationLevel={Transaction?.IsolationLevel.ToString()}");
        }

    }
}
