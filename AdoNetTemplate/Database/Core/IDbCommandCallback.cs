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
    /// IDbCommand. 
    /// </summary>
    /// <typeparam name="T">The return type from executing the
    /// callback</typeparam>
    /// <remarks>
    /// <p>Allows you to execute any number of operations
    /// on a single IDbCommand
    /// </p>
    /// <p>Used internally by AdoTemplate</p>
    /// </remarks>
    /// <author>Sasha</author>
    public interface IDbCommandCallback<T>
    {
        /// <summary>
        /// Called by AdoTemplate.Execute with an active ADO.NET IDbCommand.
        /// The calling code does not need to care about closing the 
        /// command or the connection, or
        /// about handling transactions:  this will all be handled by AdoTemplate
        /// </summary>
        /// <param name="command">An active IDbCommand instance</param>
        /// <returns>The result object</returns>
        T DoInCommand(IDbCommand command);
    }
}
