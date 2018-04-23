using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetTemplate.Database.Core
{
    /// <summary>
    /// Generic callback to map each row of data in a result set to an object.
    /// </summary>
    /// <remarks>Implementations of this interface perform the actual work
    /// of mapping rows, but don't need worry about managing
    /// ADO.NET resources, such as closing the reader.
    /// <p>
    /// Typically used for <see cref="AdoTemplate"/>'s query methods (with <see cref="RowMapperResultSetExtractor{T}"/>).
    /// </p>
    /// </remarks>
    /// <author>Sasha</author>
    public interface IRowMapper<T>
    {
        /// <summary>
        /// Implementations must implement this method to map each row of data in the
        /// result set (<see cref="IDataReader"/>).  This method should not call <see cref="IDataReader.NextResult"/> 
        /// or <see cref="IDataReader.Read"/> on the <see cref="IDataReader"/>; it should only extract the values of the current row.
        /// </summary>
        /// <param name="reader">The DataReader to map (pre-initialized to the current row)</param>
        /// <param name="rowNum">The number of the current row..</param>
        /// <returns>The specific typed result object for the current row.</returns>
        T MapRow(IDataReader reader, int rowNum);
    }
}
