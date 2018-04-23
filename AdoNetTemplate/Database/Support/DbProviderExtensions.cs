using AdoNetTemplate.Database.Core;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetTemplate.Database.Support
{
    /// <summary>
    /// Implements extension methods for <see cref="IDbProvider"/>
    /// </summary>
    public static class DbProviderExtensions
    {
        public static DbCommand CreateCommand(this IDbProvider dbProvider)
        {
            if (dbProvider.Connection == null)
                throw new ArgumentException("connection is null");

            var command = DbProviderFactories.GetFactory(dbProvider.Connection).CreateCommand();
            return command;
        }

        public static DbDataAdapter CreateDataAddapter(this IDbProvider dbProvider)
        {
            if (dbProvider.Connection == null)
                throw new ArgumentException("connection is null");

            var dataAdapter = DbProviderFactories.GetFactory(dbProvider.Connection).CreateDataAdapter();
            return dataAdapter;
        }
    }
}
