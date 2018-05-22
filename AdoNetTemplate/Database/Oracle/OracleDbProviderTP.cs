using AdoNetTemplate.Database.Generic;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoOracle.DataBase
{
    public class OracleDbProviderTP : GenericDataAccessProviderTP<OracleConnection, OracleTransaction>
    {
        public OracleDbProviderTP(string connectionString) : base(connectionString)
        {
        }
    }
}
