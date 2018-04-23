using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetTemplate.Database.Core
{
    /// <summary>
    /// Interface to be implemented by objects that can provide SQL strings
    /// </summary>
    /// <remarks>Typically implemented by ICommandCallbacks that want 
    /// to expose the CommandText they use to create their ADO.NET commands,
    /// to allow for better contextual information in case of exceptions.
    /// </remarks>
    /// <author>Sasha</author>
    public interface ICommandTextProvider
    {
        string CommandText
        {
            get;
        }
    }
}
