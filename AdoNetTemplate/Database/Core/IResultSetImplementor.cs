using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetTemplate.Database.Core
{
    /// <summary>
    /// Helper class that expose a IDataReader to an object of type <typeparamref name="T"/>.
    /// </summary>
    /// <author>Sasha</author>
    /// <typeparam name="T">Type of the mapped object</typeparam>
    public class IResultSetImplementor<T> : IResultSetExtractor<IList<T>>
    {
        private readonly Func<IDataReader, IList<T>> handle;

        public IResultSetImplementor(Func<IDataReader, IList<T>> handle)
        {
            this.handle = handle ?? throw new ArgumentNullException("Null handle.");
        }

        [DebuggerStepThrough]
        public IList<T> ExtractData(IDataReader reader)
        {
            return handle(reader);
        }
    }
}
