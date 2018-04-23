using System;


namespace AdoNetTemplate.Database.Core
{
    /// <summary>
    /// Provides a functionality to execute operations in a Transactional way for two <see cref="ITransactionParticipant"/>'s.  
    /// </summary>
    /// <remarks>
    /// The participants could be 2 different DB context, one DB and one MQ or 2 different MQ managers.
    /// This Transaction Manager on system fail doesn't preserve the ACID property's. If ACID is a must for your system then consider using 
    /// Distributed Transactions <see cref="System.Transactions.TransactionScope"/> for 2-phase commit at a cost of performance penalty.
    /// a.podgoreanu
    /// </remarks>

    public interface ITransactionManager<TOuter, TInner>
        where TInner : ITransactionParticipant
        where TOuter : ITransactionParticipant
    {

        /// <summary>
        /// The System.TimeSpan after which the transaction scope times out and aborts all transactions
        /// </summary>
        TimeSpan ScopeTimeout { get; }

        /// <summary>
        /// Commits the Inner transaction, as a consequence, must commit or rollback the Inner transaction participant
        /// </summary>
        void CommitInner();

        /// <summary>
        /// Rollback the Outer transaction, as a consequence, must commit or rollback the Inner transaction participant.
        /// </summary>
        void RollbackOuter();

        /// <summary>
        /// Rollback the Inner transaction, as a consequence, must commit or rollback the Outer transaction participant.
        /// </summary>
        void RollbackInner();

        /// <summary>
        /// Commits the Outer transaction, as a consequence, must commit or rollback the Inner transaction participant
        /// </summary>
        void CommitOuter();


        /// <summary>
        /// Commits the transactions, completing the transaction scope.
        /// </summary>
        void Complete();

        /// <summary>
        /// Client that connect and partecipate as an outer transaction.
        /// </summary>
        /// <remarks>
        /// This transaction will be the last to be committed.
        /// If the <see cref="Inner"/> transaction fails to commit neither this will be committed.
        /// If the system fails and <see cref="Outer"/> didn't yet committed, the data will be duplicated.
        /// It's necessary to have a procedure that recognize duplicated data of the <see cref="Inner"/> client.
        /// </remarks>
        TOuter Outer { get; }

        /// <summary>
        /// Client that connect and partecipate in a transaction.
        /// </summary>
        /// <remarks>
        /// This transaction will be the first to be committed.
        /// If this transaction fails to commit neither <see cref="Outer"/> will be committed.
        /// If the system fails and <see cref="Inner"/> didn't yet committed, the data will return at its precedent state.
        /// </remarks>
        TInner Inner { get; }
    }
}
