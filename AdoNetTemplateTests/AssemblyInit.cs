using AdoNetTemplate;
using AdoNetTemplate.Tests.DataBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetTemplateTests
{
    /// <summary>
    /// Class used to initialize database.
    /// </summary>
    [TestClass]
    public class AssemblyInit
    {
        private static readonly string providerName = ConfigurationManager.ConnectionStrings["OracleConn"].ProviderName;
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["OracleConn"].ConnectionString;

        private const string createDummyTable =
@"CREATE TABLE DUMMY_TABLE 
(
    COL_ID VARCHAR2(100),
    COL_INT NUMBER(10,0),
    COL_LONG NUMBER(19,0),
    COL_DOUBLE BINARY_DOUBLE,
    COL_DECIMAL NUMBER,
    COL_STRING VARCHAR2(3000),
    COL_CHAR CHAR(1),
    COL_DATE DATE,
    COL_CLOB CLOB
)";

        private const string dropDummyTable =
@"DROP TABLE DUMMY_TABLE";

        [AssemblyInitialize]
        public static void Initialize(TestContext context)
        {
            var factory = DbProviderFactories.GetFactory(providerName);
            var connection = factory.CreateConnection();
            var gDbProvider = new MockDbProvider(connection, connectionString);

            using (var adoTemplate = new AdoTemplate(gDbProvider))
            {
                try
                {
                    adoTemplate.ExecuteNonQuery(createDummyTable);
                    adoTemplate.ExecuteNonQuery(
@"INSERT INTO DUMMY_TABLE (COL_ID, COL_INT, COL_LONG, COL_DOUBLE, COL_DECIMAL, COL_STRING, COL_CHAR, COL_DATE, COL_CLOB) 
VALUES ('1', '2', '3', '4', '5', '6', '7', TO_DATE('2018-04-23 16:35:31', 'YYYY-MM-DD HH24:MI:SS'), '8')"
);
                    adoTemplate.ExecuteNonQuery(
@"INSERT INTO DUMMY_TABLE (COL_ID, COL_INT, COL_LONG, COL_DOUBLE, COL_DECIMAL, COL_STRING, COL_CHAR, COL_DATE, COL_CLOB) 
VALUES ('2', '2', '3', '4', '5', '6', '7', TO_DATE('2018-04-23 16:35:31', 'YYYY-MM-DD HH24:MI:SS'), '8')"
);
                    adoTemplate.ExecuteNonQuery(
@"INSERT INTO DUMMY_TABLE (COL_ID, COL_INT, COL_LONG, COL_DOUBLE, COL_DECIMAL, COL_STRING, COL_CHAR, COL_DATE, COL_CLOB) 
VALUES ('3', '2', '3', '4', '5', '6', '7', TO_DATE('2018-04-23 16:35:31', 'YYYY-MM-DD HH24:MI:SS'), '8')"
);
                }
                catch (Exception ex)
                { }
            }
        }

#if !DEBUG
        [AssemblyCleanup]
#endif
        public static void CleanupA()
        {
            var factory = DbProviderFactories.GetFactory(providerName);
            var connection = factory.CreateConnection();
            var gDbProvider = new MockDbProvider(connection, connectionString);

            using (var adoTemplate = new AdoTemplate(gDbProvider))
            {
                try
                {
                    adoTemplate.ExecuteNonQuery(dropDummyTable);
                }
                catch (Exception x)
                { }
            }
        }

        [TestMethod]
        public void AAAA()
        {
            //Do nothing.
            //Used to help Inizialize Assembly
        }
    }
}
