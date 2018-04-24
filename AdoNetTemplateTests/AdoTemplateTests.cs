using Microsoft.VisualStudio.TestTools.UnitTesting;
using AdoNetTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdoNetTemplate.Database.Generic;
using System.Data.Common;
using AdoNetTemplateTests.DataBase;

namespace AdoNetTemplate.Tests
{
    [TestClass()]
    public class AdoTemplateTests
    {
        public const string ConnectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.175.41.33)(PORT=1521))(CONNECT_DATA=(FAILOVER_MODE=(TYPE=select)(METHOD=basic))(SERVER=dedicated)(SERVICE_NAME=ORA12DB)));User ID=monitor;Password=monitor;";

        private static TestContext testContextInstance;

        public static TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

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



        [AssemblyInitialize()]
        [DataSource("Oracle.ManagedDataAccess.Client", ConnectionString)]
        public static void AssemblyInit(TestContext context)
        {
            var gDbProvider = new MockDbProvider(TestContext.DataConnection);

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


        //[AssemblyCleanup]
        public static void CleanupA()
        {
            /*OracleFactory OracleFactory = new OracleFactory();
            using (var adoTemplate = OracleFactory.CreateAdoTemplate(ConnectionString))
            {
                try
                {
                    adoTemplate.ExecuteNonQuery(dropDummyTable);
                }
                catch (Exception x)
                { }
            }*/
        }

        [TestInitialize]
        public void Initialize()
        {
            //OracleFactory = new OracleFactory();
        }

        [TestCleanup]
        public void Cleanup()
        {
        }

        [TestMethod()]
        public void AdoTemplateTest()
        {

        }

        [TestMethod()]
        public void TestConnectionTest()
        {

        }

        [TestMethod()]
        public void DisposeTest()
        {

        }

        [TestMethod()]
        public void ExecuteTest()
        {

        }

        [TestMethod()]
        public void ExecuteTest1()
        {

        }

        [TestMethod()]
        public void ExecuteNonQueryTest()
        {

        }

        [TestMethod()]
        public void ExecuteScalarTest()
        {

        }

        [TestMethod()]
        public void QueryForDataTableTest()
        {

        }

        [TestMethod()]
        public void QueryForObjectTest()
        {

        }

        [TestMethod()]
        public void QueryForObjectTest1()
        {

        }

        [TestMethod()]
        public void QueryForListTest()
        {

        }

        [TestMethod()]
        public void QueryForListTest1()
        {

        }

        [TestMethod()]
        public void ExecuteQueryTest()
        {

        }

        [TestMethod()]
        public void ExecuteQueryTest1()
        {

        }

        [TestMethod()]
        public void BeginTransactionTest()
        {

        }

        [TestMethod()]
        public void CommitTest()
        {

        }

        [TestMethod()]
        public void RollBackTest()
        {

        }
    }
}