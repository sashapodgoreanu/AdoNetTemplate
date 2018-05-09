using AdoNetTemplate.Database.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetTemplateTests.HelperClass
{
    class MockRowMapper : IRowMapper<MockObject>
    {
        public MockObject MapRow(IDataReader reader, int rowNum)
        {
            var FInt = reader["FINT"];
            var FString = reader["FString"];

            return new MockObject()
            {
                FInt = FInt == DBNull.Value ? null : (int?) FInt,
                FString = FString == DBNull.Value ? null : (string) FString
            };
        }
    }
}
