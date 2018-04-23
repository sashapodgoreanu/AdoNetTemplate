using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
namespace AdoNetTemplate.Database.Core
{
    /// <summary>
    /// Provides a functionality to bind values to a <see cref="IDataParameterCollection"/>
    /// </summary>
    /// <author>Sasha</author>
    /// <typeparam name="T">Object Type that is to bind to a <see cref="IDataParameterCollection"/></typeparam>
    public interface IDbParameterSetter
    {
        /// <summary>
        /// Set up parameters with values.
        /// </summary>
        /// <param name="parametters"></param>
        /// <returns></returns>
        void SetUpParameters(IDataParameterCollection parametters);


        /// <summary>
        /// Set up the command.
        /// </summary>
        /// <remarks>
        /// You can set up here any proprieties that is specific to implementation of the <see cref="IDbCommand"/>.
        /// </remarks>
        /// <param name="command"></param>
        void SetUpCommand(IDbCommand command);

    }

}


