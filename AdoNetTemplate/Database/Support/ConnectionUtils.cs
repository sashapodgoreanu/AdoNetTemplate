using AdoNetTemplate.Database.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;

namespace AdoNetTemplate.Database.Support
{
    public class ConnectionUtils
    {

        #region Logging

        private static Logger logger = LogManager.GetCurrentClassLogger();

        #endregion

        /// <summary>
        /// Property dispose of the command.  Useful in finally or catch blocks.
        /// </summary>
        /// <param name="command">command to dispose</param>
        public static void DisposeCommand(IDbCommand command)
        {
            if (command != null)
            {
                DoDisposeCommand(command);
            }
        }

        /// <summary>
        /// NOt used
        /// </summary>
        /// <param name="adapter"></param>
        public static void DisposeDataAdapterCommands(IDbDataAdapter adapter)
        {
            if (adapter == null)
                return;
            if (adapter.SelectCommand != null)
            {
                DoDisposeCommand(adapter.SelectCommand);
            }
            if (adapter.InsertCommand != null)
            {
                DoDisposeCommand(adapter.InsertCommand);
            }
            if (adapter.UpdateCommand != null)
            {
                DoDisposeCommand(adapter.UpdateCommand);
            }
            if (adapter.DeleteCommand != null)
            {
                DoDisposeCommand(adapter.DeleteCommand);
            }

        }

        public static void CloseReader(IDataReader reader)
        {
            if (reader != null)
            {
                try
                {
                    reader.Close();
                    reader.Dispose();
                }
                catch (Exception e)
                {
                    logger.Warn(e,"Could not close IDataRader");
                }
            }
        }

        public static void DisposeConnection(IDbConnection conn)
        {
            if (conn == null)
            {
                return;
            }
            logger.Trace("Connection closed.");
            conn.Close();            
            conn = null;
        }

        public static void DisposeTransaction(IDbTransaction transaction)
        {
            if (transaction == null)
            {
                return;
            }
            transaction.Dispose();
            logger.Debug("transaction disposed.");
            transaction = null;
        }

        private static void DoDisposeCommand(IDbCommand command)
        {
            try
            {
                command.Parameters.Clear();
                command.Dispose();
            }
            catch (Exception e)
            {
                logger.Warn(e,"Could not dispose of command");
            }
        }


        /// <summary>
        /// Close the connection of the specified dbProvider.
        /// dbProvider has an active transaction , the connection will not be closed.
        /// </summary>
        /// <param name="dbProvider"></param>
        internal static void ClosseConnection(IDbProvider dbProvider)
        {
            try
            {
                if (dbProvider.Transaction == null)
                {
                    dbProvider.CloseConnection();
                }
                else
                {
                    //ignore
                }
            }
            catch (Exception ex)
            {
                //ignore the fact that i cant close the connection
                logger.Warn(ex.Message);
            }
            
        }

        ///// <summary>
        ///// Gets a specific <see cref="IDbProvider"/> for Oracle DB.
        ///// </summary>
        ///// <returns></returns>
        //public static IDbProvider CreateOracleDBProvider()
        //{
        //    return new OracleDbProvider(connectionString);
        //}

        ///// <summary>
        ///// Gets a specific <see cref="IDbProvider"/> for Oracle DB.
        ///// </summary>
        ///// <returns></returns>
        //public static IDbProvider CreateOracleDBProvider(string connString)
        //{
        //    return new OracleDbProvider(connString);
        //}

        ///// <summary>
        ///// Gets a specific <see cref="AdoTemplate"/> for Oracle DB.
        ///// </summary>
        ///// <returns></returns>
        //public static AdoTemplate CreateOracleAdoTemplate(string connString)
        //{
        //    return new AdoTemplate(CreateOracleDBProvider(connString));
        //}

        /// <summary>
        /// Ensures that only one result exists, and returns it from the specified list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="results"></param>
        /// <returns></returns>
        public static T RequiredUniqueResultSet<T>(IList<T> results)
        {

            int size = (results != null ? results.Count : 0);
            if (size == 0)
            {
                return default(T); //null if is reference type else default value: 0 for int, 0.0 for double ...
            }
            if (results.Count > 1)
            {
                throw new ConstraintException($"Returned more then 1 result: {results.Count}");
            }

            return results.First();
        }

    }
}
