using AdoNetTemplate.Database.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using AdoNetTemplate.Database.Support;
using Oracle.ManagedDataAccess.Client;
using AdoNetTemplate.Database.Generic;

namespace AdoOracle.Database
{
    /// <summary>
    /// Wraps a collection of <see cref="OracleParameter"/>.
    /// <para>
    /// If <typeparamref name="TKey"/> is <see cref="int"/> the bind order is given by the key order, from 0 to n.
    /// Else If <typeparamref name="TKey"/> is <see cref="string"/> the bind order is given by the add order, first added is first binded ... last added is last binded.
    /// </para>
    /// </summary>
    /// <remarks>
    /// Used by <see cref="AdoTemplate"/> to bind parameters to a <see cref="OracleCommand"/>.
    /// </remarks>
    /// <typeparam name="TKey">Is <see cref="int"/> or <see cref="string"/></typeparam>
    public class OracleParametersWrapper<TKey> : GenericParametersWrapper<TKey, OracleParameter>
    {
        /// <summary>
        /// Default Constructor.
        /// <para>
        /// Allways dispose <see cref="OracleParametersWrapper{TKey}"/> either calling <see cref="OracleParametersWrapper{TKey}.Dispose()"/> or with 'using statement block'.
        /// </para>
        /// </summary>
        /// <param name="arrayCount">is grater then 0 if is binding to a parameter an array of values</param>
        public OracleParametersWrapper(int arrayCount = 0) : base(arrayCount)
        {

        }

        public override void SetUpCommand(IDbCommand command)
        {
            if (command is OracleCommand)
            {
                if (ArrayBindCount > 0)
                    (command as OracleCommand).ArrayBindCount = ArrayBindCount;
            }
            else
            {
                //errore di programmazione!
                throw new SystemException($"Expected {nameof(OracleCommand)} but found {command.GetType()}");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    Task.Factory.StartNew(() =>
                    {
                        foreach (var param in _parameters)
                        {
                            param.Value.Dispose();
                        }
                    }).ContinueWith((task) =>
                    {
                        _parameters.Clear();
                    });
                }

                // Call here the appropriate methods to clean up
                // unmanaged resources here.
                // If disposing is false,
                // only the following code is executed.

                //example:
                //CloseHandle(handle);
                //handle = IntPtr.Zero;

                // Note disposing has been done.
                disposed = true;
            }
        }
    }
}
