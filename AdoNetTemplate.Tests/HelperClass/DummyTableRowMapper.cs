using AdoNetTemplate.Database.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetTemplate.Tests.HelperClass
{
    internal class DummyTableRowMapper : IRowMapper<DummyTable>
    {
        public DummyTable MapRow(IDataReader reader, int rowNum)
        {
            var retVal = new DummyTable();
            return retVal;
        }
    }
}
