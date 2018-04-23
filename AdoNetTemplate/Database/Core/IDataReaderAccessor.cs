using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetTemplate.Database.Core
{
    /// <summary>
    /// Provide a functionality to expose a <see cref="IDataReader"/> after executing a query.
    /// </summary>
    public interface IDataReaderAccessor
    {
        void AccessDatareader(IDataReader dataReader);
    }
}
