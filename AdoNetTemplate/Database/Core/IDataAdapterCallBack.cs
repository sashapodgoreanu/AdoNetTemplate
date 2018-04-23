using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetTemplate.Database.Core
{
    /// <summary>
    /// Generic callback interface for code that operates on a 
    /// <see cref="IDbDataAdapter"/>. 
    /// </summary>
    /// <remarks>
    /// <p>Allows you to execute any number of operations
    /// on a <see cref="IDbDataAdapter"/>, for example to Fill a DataSet
    /// or other more advanced operations such as the transfering
    /// data between two different DataSets.
    /// </p>
    /// <p>Note that the passed in <see cref="IDbDataAdapter"/>
    /// has been created by the framework and its SelectCommand
    /// will be populated with values for CommandType and Text properties
    /// along with Connection/Transaction properties based on the
    /// calling transaction context.
    /// </p>
    /// </remarks>    
    /// <author>Alexandru Podgoreanu</author>
    public interface IDataAdapterCallback<T>
    {
        /// <summary>
        /// Called by AdoTemplate.Execute with an preconfigured
        /// ADO.NET IDbDataAdapter instance with its SelectCommand
        /// property populated with CommandType and Text values
        /// along with Connection/Transaction properties based on the
        /// calling transaction context.
        /// </summary>
        /// <param name="dataAdapter">An active IDbDataAdapter instance</param>
        /// <returns>The result object</returns>
        T DoInDataAdapter(IDbDataAdapter dataAdapter);
    }
}
