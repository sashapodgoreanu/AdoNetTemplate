using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetTemplate.Database.Core
{
    /// <summary>
    /// Implementation of <see cref="IResultSetExtractor{T}"/>
    /// </summary>
    /// <remarks>
    /// Mainly, used internally to map a set of result to a list of objects.
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    public class RowMapperResultSetExtractor<T> : IResultSetExtractor<IList<T>>
    {
        #region Fields

        private IRowMapper<T> _rowMapper;

        protected IRowMapper<T> RowMapper
        {
            get
            {
                return _rowMapper;
            }

            set
            {
                _rowMapper = value;
            }
        }

        #endregion

        #region Constructor (s)
        /// <summary>
        /// Initializes a new instance of the <see cref="RowMapperResultSetExtractor"/> class.
        /// </summary>
        public RowMapperResultSetExtractor(IRowMapper<T> rowMapper)
        {
            RowMapper = rowMapper ?? throw new ArgumentNullException("null rowmapper");

        }

        #endregion

        public virtual IList<T> ExtractData(IDataReader dataReader)
        {
            if (dataReader == null)
                throw new ArgumentNullException("null rowmapper");

            IList<T> results = new List<T>();
            int rowNum = 0;

            // map me!
            while (dataReader.Read())
            {
                results.Add(RowMapper.MapRow(dataReader, rowNum++));
            }

            return results;
        }
    }
}
