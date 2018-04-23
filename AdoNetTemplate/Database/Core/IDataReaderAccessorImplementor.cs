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
    /// Helper class that expose a <see cref="IDataReader"/>.
    /// </summary>
    /// <author>Sasha</author>
    public class IDataReaderAccessorImplementor : IDataReaderAccessor
    {
        private readonly Action<IDataReader> handle;


        public IDataReaderAccessorImplementor(Action<IDataReader> handle)
        {
            if (handle == null)
                throw new ArgumentNullException("Null handle.");

            this.handle = handle;
        }

        [DebuggerStepThrough]
        public void AccessDatareader(IDataReader dataReader)
        {
            handle(dataReader);
        }
    }
}
