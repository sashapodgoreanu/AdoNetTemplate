using AdoNetTemplate.Database.Core;
using AdoNetTemplate.Database.Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetTemplate.Database.Generic
{
    class GenericParametersWrapper
    {
    }

    /// <summary>
    /// Wraps a collection of <see cref="DbParameter"/>.
    /// <para>
    /// If <typeparamref name="TKey"/> is <see cref="int"/> the bind order is given by the key order, from 0 to n.
    /// Else If <typeparamref name="TKey"/> is <see cref="string"/> the bind order is given by the add order, first added is first binded ... last added is last binded.
    /// </para>
    /// </summary>
    /// <remarks>
    /// Used by <see cref="AdoTemplate"/> to bind parameters to a <see cref="DbCommand"/>.
    /// </remarks>
    /// <typeparam name="TKey">Is <see cref="int"/> or <see cref="string"/></typeparam>
    public abstract class GenericParametersWrapper<TKey, TDbParameter> : IDbParameterSetter, IDisposable
        where TDbParameter : DbParameter
    {
        protected KeyedList<TKey, TDbParameter> _parameters;
        protected bool disposed;

        public int ArrayBindCount { get; set; }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="arrayCount">is grater then 0 if is binding to a parameter an array of values</param>
        public GenericParametersWrapper(int arrayCount = 0)
        {
            if (typeof(TKey) != typeof(string) && typeof(TKey) != typeof(int))
                throw new ArgumentException("Invalid Type Specified, may be 'int' or 'string'");

            _parameters = new KeyedList<TKey, TDbParameter>();
            ArrayBindCount = arrayCount;
        }


        public TDbParameter this[TKey i]
        {
            get { return _parameters[i]; }
            set
            {
                _parameters[i] = value;
                _parameters[i].ParameterName = $"par_{i}";
            }
        }

        public bool ContainsKey(TKey key)
        {
            return _parameters.ContainsKey(key);
        }


        public void SetUpParameters(IDataParameterCollection parameters)
        {
            if (disposed)
                throw new InvalidOperationException("ParametersWrapper was disposed");
            //if is int then bind by int order
            if (typeof(TKey) == typeof(int))
            {
                for (int index = 0; index < _parameters.Count; index++)
                {
                    parameters.Add(_parameters[index].Value);
                }
            }
            else //bind by the add order.
            {
                foreach (KeyValuePair<TKey, TDbParameter> param in _parameters)
                {
                    parameters.Add(param.Value);
                }
            }
        }

        public abstract void SetUpCommand(IDbCommand command);

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (typeof(TKey) == typeof(int))
            {
                for (int index = 0; index < _parameters.Count; index++)
                {
                    sb.AppendLine($"[{_parameters[index].Value.ParameterName} = {_parameters[index].Value.Value}]");
                }
            }
            else //bind by the add order.
                foreach (KeyValuePair<TKey, TDbParameter> param in _parameters)
                {
                    sb.AppendLine($"[{param.Value.ParameterName} = {param.Value.Value}]");
                }
            return sb.ToString();
        }

        #region Dispose Implementation

        /// <summary>
        /// Destructor
        /// </summary>
        ~GenericParametersWrapper()
        {
            Dispose(false);
        }


        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the
        // runtime from inside the finalizer and you should not reference
        // other objects. Only unmanaged resources can be disposed.
        protected abstract void Dispose(bool disposing);

        // Implement IDisposable.
        // Do not make this method virtual.
        // A derived class should not be able to override this method.
        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
