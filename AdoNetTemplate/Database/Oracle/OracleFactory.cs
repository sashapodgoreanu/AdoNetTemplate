using AdoNetTemplate.Database.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdoNetTemplate;

namespace AdoOracle.Database
{
    public class OracleFactory : IAdoFactory
    {
        /// <summary>
        /// Creates a new <see cref="AdoTemplate"/> with the specified <paramref name="dbProvider"/>. 
        /// </summary>
        /// <param name="dbProvider"></param>
        /// <returns></returns>
        public AdoTemplate CreateAdoTemplate(IDbProvider dbProvider)
        {
            ///<paramref name="dbProvider"/> is managed externally, like creating and destroying a connection.
            var adoTemplate = new AdoTemplate(dbProvider, true);
            return adoTemplate;
        }

        /// <summary>
        /// Creates a new <see cref="AdoTemplate"/> with the specified <paramref name="connectionString"/>. 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public AdoTemplate CreateAdoTemplate(string connectionString)
        {
            ///<paramref name="dbProvider"/> is managed internally, like creating and destroying a connection.
            var dbProvider = CreateDbProvider(connectionString);
            var adoTemplate = new AdoTemplate(dbProvider, true);
            return adoTemplate;
        }

        /// <summary>
        /// Creates a new <see cref="OracleDbProvider"/> with the specified <paramref name="connectionString"/>. 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public IDbProvider CreateDbProvider(string connectionString)
        {
            var dbProvider = new OracleDbProvider(connectionString);
            return dbProvider;
        }
    }
}
