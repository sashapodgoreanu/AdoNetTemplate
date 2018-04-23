using AdoNetTemplate.Database.Core;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace AdoNetTemplate.Database.Generic
{
    /// <summary>
    /// Manages 2 nested transactions.
    /// </summary>
    /// <remarks>
    /// The 2 transactions are committed when the manager completes.
    /// It is possible to commit only one transaction but the other will be roll-backed.
    /// It is possible to rollback only one transaction 
    /// but it is need to be specified the commit of the other, if not then a rollback will take place. 
    /// If the manager does not complete, a rollback will take place. 
    /// </remarks>
    public sealed class TransactionManager<TOuter, TInner> : IDisposable, ITransactionManager<TOuter, TInner>
        where TInner : ITransactionParticipant
        where TOuter : ITransactionParticipant
    {
        #region fields
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private TInner inner;

        private TOuter outer;

        /// <summary>
        /// max executing time for this transaction before rollback is called.
        /// </summary>
        private TimeSpan scopeTimeout;

        /// <summary>
        /// executes the callback on timeout.
        /// </summary>
        private Timer scopeTimer;

        /// <summary>
        /// status of disposed
        /// </summary>
        private volatile bool disposed;

        /// <summary>
        /// complete is true when the transaction has completed
        /// A transaction is complete when all the participants commit successfully or rollback their own transactions.
        /// </summary>
        private volatile bool complete;

        /// <summary>
        /// If the elapsed time to complete transaction is grater then the scopeTimeout set by user or default.
        /// </summary>
        private volatile bool timedOut;

        /// <summary>
        /// used to lock the rollback operation.
        /// </summary>
        private object rollbackLock = new object();

        private bool innerComplete;
        private bool outerComplete;


        #endregion fields

        #region Properties
        /// <summary>
        /// Gets the client that connect and partecipate as an Inner transaction.
        /// </summary>
        /// <seealso cref="ITransactionManager.Inner"/>
        public TInner Inner
        {
            get
            {
                return inner;
            }
            private set
            {
                inner = value;
            }
        }

        /// <summary>
        /// Gets the client that connect and partecipate as an Outer transaction.
        /// </summary>
        /// <seealso cref="ITransactionManager.Outer"/>
        public TOuter Outer
        {
            get
            {
                return outer;
            }
            private set
            {
                outer = value;
            }
        }

        /// <summary>
        /// The System.TimeSpan after which the transaction scope times out and aborts all transactions
        /// </summary>
        public TimeSpan ScopeTimeout
        {
            get
            {
                return scopeTimeout;
            }

            private set
            {
                scopeTimeout = value;
            }
        }
        #endregion

        /// <summary>
        /// Creates a new Transaction Manager and calls <paramref name="outer"/> and <paramref name="inner"/> <see cref="ITransactionParticipant.Prepare"/>. 
        /// </summary>
        /// <param name="outer"></param>
        /// <param name="inner"></param>
        /// <exception cref="TransactionException">If fails to initialize any of inner or outer transaction.</exception>
        public TransactionManager(TOuter outer, TInner inner) : this (outer, inner, TimeSpan.Zero) {}

        /// <summary>
        /// Creates a new Transaction Manager and calls <paramref name="outer"/> and <paramref name="inner"/> <see cref="ITransactionParticipant.Prepare"/>. 
        /// <para>
        /// If executing time is grater then <paramref name="scopeTimeout"/> then transactions will be rolled back.
        /// </para>
        /// </summary>
        /// <param name="outer"></param>
        /// <param name="inner"></param>
        /// <param name="scopeTimeout"></param>
        /// <exception cref="TransactionException">If fails to initialize any of inner or outer transaction.</exception>
        public TransactionManager(TOuter outer, TInner inner, TimeSpan scopeTimeout)
        {
            

            if (outer == null || inner == null)
                throw new ArgumentNullException($"{nameof(ITransactionParticipant)} is null");

            Inner = inner;
            Outer = outer;

            ///observe if inner has called commit.
            ///we will know when inner calls commit of its transaction outside of this class.
            Inner.CommitObserver += CommitHandler;

            //observe if outer has called commit.
            ///we will know when outer calls commit of its transaction outside of this class.
            Outer.CommitObserver += CommitHandler;

            complete = false;
            disposed = false;

            Prepare();

#if DEBUG
            //never start the timer.
            ScopeTimeout = new TimeSpan(0, 0, 0, 0, -1);
#else
            ScopeTimeout = scopeTimeout;
#endif

            if (scopeTimeout != TimeSpan.Zero)
            {
                ///start a timer that will call <see cref="TimerCallback"/> when is reached 0.
                scopeTimer = new Timer(
                    TimerCallback,      ///callback method to be called when <see cref="ScopeTimeout"/> expires.
                    this,               ///object to be passed to callback method
                    ScopeTimeout,       
                    TimeSpan.Zero       ///no repeat, run once.
                    );
            }
        }

        /// <summary>
        /// Callback method that is called when <see cref="ScopeTimeout"/> expires.
        /// </summary>
        /// <param name="state"></param>
        private static void TimerCallback(object state)
        {
            var scope = state as TransactionManager<TInner, TOuter>;
            scope.Timeout();
        }

        private void Timeout()
        {
            //Note this transaction timed out. cannot be reused.
            timedOut = true;
            if ((!this.complete) && (null != this.Inner) && (null != this.Outer))
            {
                Log.Warn($"{nameof(TransactionManager<TInner, TOuter>)} scope timed out!");
                Log.Warn(Inner.TraceTransaction());
                Log.Warn(Outer.TraceTransaction());

                Rollback();
            }
        }

        /// <summary>
        /// Prepare outer and inner
        /// </summary>
        /// <exception cref="TransactionException">If Prepare fails.</exception>
        private void Prepare()
        {
            TransactionException toBeThrown = null; 
            var abortOuter = false;
            var abortInner = false;

            //Prepare OUTER
            try
            {
                Outer.Prepare();
            }
            catch (TransactionException txEx)
            {
                abortOuter = true;
                Log.Debug("Prepare transaction failed.");
                Log.Trace(Outer.TraceTransaction());
                toBeThrown = new TransactionException("Prepare Outer transaction failed.", txEx);
            }

            //IF OK THEN PREPARE INNER
            if (!abortOuter)
            {
                try
                {
                    Inner.Prepare();
                }
                catch (TransactionException txEx)
                {
                    abortInner = true;
                    Log.Debug("Prepare transaction failed.");
                    Log.Trace(Inner.TraceTransaction());
                    toBeThrown = new TransactionException("Prepare Inner transaction failed.", txEx);
                }
            }

            //IF NOT OK INNER and OUTER ABBORT
            if (abortInner || abortOuter)
            {
                try
                {
                    Inner.Abort();
                    Outer.Abort();
                }
                catch { /*IGNORE*/ }
                throw toBeThrown;
            }
        }

        /// <summary>
        /// Indicates that all operations within the scope are completed successfully. This operation calls inner and outer <see cref="ITransactionParticipant.Commit"/>
        /// </summary>
        public void Complete()
        {
            if (disposed)
            {
                throw new ObjectDisposedException($"{nameof(TransactionManager<TInner,TOuter>)}");
            }
            if (complete)
            {
                throw new InvalidOperationException($"Transaction allready completed!");
            }
            else if (innerComplete)
            {
                CommitOuter();
            }
            else if (outerComplete)
            {
                CommitInner();
            }
            else
            {
                if (timedOut)
                {
                    throw new InvalidOperationException("Transaction timed out!");
                }

                try
                {
                    complete = true;
                    commit();
                }
                catch (TransactionException txEx)
                {
                    Log.Debug("Complete transaction failed.");
                    Rollback();
                    throw new TransactionException("Complete transaction failed", txEx);
                }
            }
        }

        public void CommitInner()
        {
            if (disposed)
            {
                throw new ObjectDisposedException($"{nameof(TransactionManager<TInner, TOuter>)}");
            }

            if (complete || innerComplete)
            {
                throw new InvalidOperationException("Inner Transaction already completed!");
            }

            if (timedOut)
            {
                throw new InvalidOperationException("Transaction timed out!");
            }

            try
            {
                //innerComplete = true;
                //complete = innerComplete && outerComplete;
                Inner.Commit();
            }
            catch (TransactionException txEx)
            {
                Log.Debug("Inner Commit transaction failed.");
                Rollback();
                throw new TransactionException("Inner Commit transaction failed", txEx);
            }
        }

        public void CommitOuter()
        {
            if (disposed)
            {
                throw new ObjectDisposedException($"{nameof(TransactionManager<TInner, TOuter>)}");
            }

            if (complete || outerComplete)
            {
                throw new InvalidOperationException("Outer Transaction already completed!");
            }

            if (timedOut)
            {
                throw new InvalidOperationException("Transaction timed out!");
            }

            try
            {
                //outerComplete = true;
                //complete = innerComplete && outerComplete;
                Outer.Commit();
            }
            catch (TransactionException txEx)
            {
                Log.Debug("Outer Commit transaction failed.");
                Rollback();
                throw new TransactionException("Outer Commit  transaction failed", txEx);
            }
        }

        /// <summary>
        /// helper method for commit.
        /// </summary>
        private void commit()
        {
            TransactionException toBeThrown = null;
            var innerFailed = false;
            var outerFailed = false;

            //commit inner transaction
            try
            {
                Inner.Commit();
            }
            catch (TransactionException txEx)
            {
                innerFailed = true;
                Log.Debug("Commit transaction failed.");
                Log.Trace(Inner.TraceTransaction());
                toBeThrown = new TransactionException("Commit Inner transaction failed", txEx);
            }

            //if ok commit outer transaction
            if (!innerFailed)
            {
                try
                {
                    Outer.Commit();
                }
                catch (TransactionException txEx)
                {
                    outerFailed = true;
                    Log.Debug("Commit transaction failed.");
                    Log.Trace(Outer.TraceTransaction());
                    toBeThrown = new TransactionException("Commit Outer transaction failed", txEx);
                }
            }

            //something bad happened
            if (innerFailed || outerFailed)
            {
                throw toBeThrown;
            }

            complete = true;

        }

        public void RollbackOuter()
        {
            if (!complete)
            {
                if (!outerComplete)
                {

                    outerComplete = true;
                    InternRollback(Outer);
                }

                complete = outerComplete && innerComplete;
            }
        }

        public void RollbackInner()
        {
            if (!complete)
            {
                if (!innerComplete)
                {
                    innerComplete = true;
                    InternRollback(Inner);
                }

                complete = outerComplete && innerComplete;
            }
        }

        private void InternRollback(ITransactionParticipant participant)
        {
            try
            {
                participant.Rollback();
            }
            catch (ObjectDisposedException ex)
            {
                // Tolerate the fact that the transaction has already been disposed.
                Log.Debug(ex, $"{nameof(participant)} Transaction already disposed.");
            }
            catch (TransactionException txEx)
            {
                // Tolerate transaction exceptions
                Log.Debug(txEx, $"{nameof(participant)} Rollback failed.");
                Log.Trace(Outer.TraceTransaction());
            }
        }


        /// <summary>
        /// Helper method that executes Rollback.
        /// </summary>
        /// <remarks>
        /// Rolls back only not committed transaction and if this transaction scope is not completed.
        /// </remarks>
        private void Rollback()
        {
            //Dangerous zone. 
            lock (rollbackLock)
            {
                //check again!
                //maybe already completed meanwhile.
                if (!complete)
                {
                    if (!innerComplete)
                    {
                        //now we are safe to do rollback
                        try
                        {
                            Inner.Rollback();

                        }
                        catch (ObjectDisposedException ex)
                        {
                            // Tolerate the fact that the transaction has already been disposed.
                            Log.Debug(ex, "Inner Transaction already disposed.");
                        }
                        catch (TransactionException txEx)
                        {
                            // Tolerate transaction exceptions
                            Log.Debug(txEx, "Inner Rollback failed.");
                            Log.Trace(Inner.TraceTransaction());

                        }
                    }

                    if (!outerComplete)
                    {
                        try
                        {
                            Outer.Rollback();

                        }
                        catch (ObjectDisposedException ex)
                        {
                            // Tolerate the fact that the transaction has already been disposed.
                            Log.Debug(ex, "Outer Transaction already disposed.");
                        }
                        catch (TransactionException txEx)
                        {
                            // Tolerate transaction exceptions
                            Log.Debug(txEx, "Outer Rollback failed.");
                            Log.Trace(Outer.TraceTransaction());
                        }
                    }

                    complete = true;
                }
            }
        }

        /// <summary>
        /// Sets the completion state parameters, depending on who called this method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CommitHandler(object sender, EventArgs e)
        {
            if (sender.Equals(inner))
            {
                innerComplete = true;
                complete = innerComplete && outerComplete;
            }
            else if (sender.Equals(outer))
            {
                outerComplete = true;
                complete = innerComplete && outerComplete;
            }
            
        }

        #region Dispose Implementation

        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the
        // runtime from inside the finalizer and you should not reference
        // other objects. Only unmanaged resources can be disposed.
        private void Dispose(bool disposing)
        {
            try
            {
                // Check to see if Dispose has already been called.
                if (!disposed)
                {
                    if (disposing)
                    {
                        // If there is a not complete transaction, abort it.
                        if (!complete)
                        {
                            Rollback();                             
                        }

                        //we don't dispose the transaction participants.
                        //only clean
                        Inner.Clean();
                        Outer.Clean();

                        if (scopeTimer != null)
                            scopeTimer.Dispose();

                        scopeTimer = null;
                    }
                    // Note disposing has been done.
                    disposed = true;

                }
            }
            catch (Exception ex)
            {
                Log.Warn(ex, "Dispose failed.");
                //IGNORE - we are just cleaning.
            }
            finally
            {
                //we completed the work now.
                complete = true;
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
