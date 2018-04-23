using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetTemplate.Database.Core
{
    /// <summary>
    /// Callback interface to map and process all results sets and rows in
    /// an <see cref="AdoTemplate"/> query method.
    /// </summary>
    /// <remarks>Implementations of this interface perform the work
    /// of extracting results and processing it but don't need worry about managing
    /// ADO.NET resources, such as closing the reader. 
    /// <p>
    /// This interface is mainly used by <see cref="AdoTemplate"/>.
    /// An <see cref="IResultSetExtractor{T}"/> is usually a simpler choice
    /// for result set (DataReader) processing, in particular a 
    /// <see cref="RowMapperResultSetExtractor{T}"/> in combination with a <see cref="IRowMapper{T}"/>.
    /// </p>
    /// </remarks>
    /// <author>Sasha</author>
    /// 
    public interface IResultSetExtractor<T>
    {
        /// <summary>
        /// Implementations must implement this method to process all
        /// result set and rows in the IDataReader.
        /// </summary>
        /// <param name="reader">The IDataReader to extract data from.
        /// Implementations should not close this: it will be closed
        /// by the <see cref="AdoTemplate"/>.</param>
        /// <returns>An arbitrary result object or null if none.  The
        /// extractor will typically be stateful in the latter case.</returns>
        T ExtractData(IDataReader reader);
    }
}
