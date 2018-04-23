using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetTemplate.Database.Core
{
    /// <summary>
    /// Provides a mechanism to create ADO components.
    /// </summary>
    public interface IAdoFactory
    {
        /// <summary>
        /// Creates a new <see cref="AdoTemplate"/> with the specified <paramref name="connectionString"/>
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        AdoTemplate CreateAdoTemplate(string connectionString);

        /// <summary>
        /// Creates a new <see cref="AdoTemplate"/> with the specified <paramref name="dbProvider"/>
        /// </summary>
        /// <param name="dbProvider"></param>
        /// <returns></returns>
        AdoTemplate CreateAdoTemplate(IDbProvider dbProvider);

        /// <summary>
        /// Creates a new <see cref="IDbProvider"/> with the specified <paramref name="connectionString"/>
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        IDbProvider CreateDbProvider(string connectionString);
    }
}
