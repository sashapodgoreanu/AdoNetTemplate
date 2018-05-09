using AdoNetTemplate.Database.Generic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetTemplate.Tests.DataBase
{
    public class MockDbParametersWrapper<TKey> : GenericParametersWrapper<TKey, DbParameter>
    {
        private DbConnection _connection;

        public MockDbParametersWrapper(DbConnection connection)
        {
            _connection = connection;
        }

        public override void SetUpCommand(IDbCommand command)
        {
            var dbCommand = command as DbCommand;
        }

        protected override void Dispose(bool disposing)
        {
            
        }

        public void Bind(Action<DbCommand> action)
        {
            var dbCommand = _connection.CreateCommand();
            action(dbCommand);
        }
    }
}
