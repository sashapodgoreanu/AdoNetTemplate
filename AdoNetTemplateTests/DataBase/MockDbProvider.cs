using AdoNetTemplate.Database.Core;
using AdoNetTemplate.Database.Generic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetTemplate.Tests.DataBase
{
    public class MockDbProvider : GenericDbProvider<DbConnection, DbTransaction>
    {
        private DbConnection _connection;

        protected override DbConnection CreateConnection()
        {
            return _connection;
        }

        public MockDbProvider(DbConnection connection, string connString) : base(connection)
        {
            _connection = connection;
            ConnectionString = connString;
        }
    }
}
