using AdoNetTemplate.Database.Generic;
using IBM.Data.DB2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace AdoDB2.Database
{
    /// <summary>
    /// This class manage a DB2 connection and transaction.
    /// </summary>
    public class DB2DbProvider : GenericDbProvider<DB2Connection, DB2Transaction>
    {
        public DB2DbProvider(string connectionString) : base(connectionString)
        {
        }
    }
}
