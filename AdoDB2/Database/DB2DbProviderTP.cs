using AdoNetTemplate.Database.Generic;
using IBM.Data.DB2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoDB2.Database
{
    public class DB2DbProviderTP : GenericDataAccessProviderTP<DB2Connection, DB2Transaction>
    {
        public DB2DbProviderTP(string connectionString) : base(connectionString)
        {
        }
    }
}
