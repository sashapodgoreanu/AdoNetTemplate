using AdoNetTemplate.Database.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Diagnostics;

namespace AdoNetTemplate.Database.Core
{
    /// <summary>
    /// Helper class that maps data from IDataReader to object of type <typeparamref name="T"/>.
    /// </summary>
    /// <author>Sasha</author>
    /// <typeparam name="T">Type of the mapped object</typeparam>
    public class IRowMapImplementor<T> : IRowMapper<T>
    {
        private readonly Func<IDataReader, int,T> handle;

        public IRowMapImplementor (Func<IDataReader, int, T> handle)
        {
            if (handle == null)
                throw new ArgumentNullException("Null handle.");

            this.handle = handle;
        }

        /// <summary>
        /// <see cref="IRowMapper{T}.MapRow(IDataReader, int)"/>
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public T MapRow(IDataReader reader, int rowNum)
        {
            return handle(reader, rowNum);
        }
    }
}
