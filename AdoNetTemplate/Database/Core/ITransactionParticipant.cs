using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetTemplate.Database.Core
{
    /// <summary>
    /// Client that will connect to a durable storage and will manage internally operations like commit, rollback, begin transaction.
    /// a.podgoreanu
    /// </summary>
    public interface ITransactionParticipant
    {
        /// <summary>
        /// Prepare a transaction of this participant.
        /// This is a good place to Begin transaction for a DB. 
        /// </summary>
        /// <exception cref="System.Transactions.TransactionException">Must throw If Prepare failed.</exception>
        void Prepare();

        /// <summary>
        /// Abort the preparing transaction of this participant.
        /// The abort occurs when one participant didn't prepared successfully his transaction. 
        /// </summary>
        /// <exception cref="System.Transactions.TransactionException">Must throw If Commit failed.</exception>
        void Abort();

        /// <summary>
        /// Commit the transaction of this participant.
        /// </summary>
        /// <exception cref="System.Transactions.TransactionException">Must throw If Commit failed.</exception>
        void Commit();

        /// <summary>
        /// Rollback the transaction of this participant.
        /// </summary>
        /// <remarks>
        /// The Rollback occurs if:
        /// 1. An exception was thrown inside the participants transaction.
        /// 2. Commit ended abnormally.
        /// 3. <see cref="ITransactionManager.Complete"/> was not called before disposing the object.
        /// 4. <see cref="ITransactionManager.Complete"/> was not called before <see cref="ITransactionManager.ScopeTimeout"/> has reached the 0.
        /// </remarks>
        /// <exception cref="System.Transactions.TransactionException">Must throw If rollback failed.</exception>
        void Rollback();

        /// <summary>
        /// Clean the transaction.
        /// </summary>
        void Clean();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string TraceTransaction();

        /// <summary>
        /// Represents the method to be invoked before or after a commit of a transaction was called.
        /// This could be a log or other operations.
        /// </summary>
        event EventHandler CommitObserver;
    }
}
 