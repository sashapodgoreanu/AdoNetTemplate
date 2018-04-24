using AdoNetTemplate.Database.Core;
using AdoNetTemplate.Database.Support;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Transactions;
using NLog;
using AdoNetTemplate.Database.Generic;

namespace AdoOracle.Database
{
    /// <summary>
    /// This class manage Oracle connections and transactions.
    /// </summary>
    public class OracleDbProvider : GenericDbProvider<OracleConnection, OracleTransaction>
    {
       
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public OracleDbProvider(string connectionString) : base(connectionString)
        {
        }

        protected override OracleConnection CreateConnection()
        {
            return new OracleConnection();
        }

        /// <summary>
        /// Open the connection.
        /// </summary>
        /// <returns></returns>
        public override void OpenConnection()
        {
            base.OpenConnection();
            var info = Connection.GetSessionInfo();
            // '.' decimal separator
            // ',' thousand separator
            info.NumericCharacters = ".,";

            try
            {
                Connection.SetSessionInfo(info);
            }
            catch (Exception)
            {
                OracleConnection.ClearPool(Connection);
                Connection.SetSessionInfo(info);
            }
        }               
    }

}
