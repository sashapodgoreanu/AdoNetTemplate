using Microsoft.VisualStudio.TestTools.UnitTesting;
using AdoNetTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.Common;
using AdoNetTemplate.Tests.DataBase;
using System.Data;

namespace AdoNetTemplate.Tests
{
    [TestClass()]
    public class AdoTemplateExtensionsTests
    {
        private TestContext testContextInstance;
        private static readonly string providerName = ConfigurationManager.ConnectionStrings["OracleConn"].ProviderName;
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["OracleConn"].ConnectionString;


        public TestContext TestContext
        {
            get { return TestContextInstance; }
            set { TestContextInstance = value; }
        }

        public TestContext TestContextInstance { get => testContextInstance; set => testContextInstance = value; }

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
        public void ExecuteScalarTest()
        {
            var factory = DbProviderFactories.GetFactory(providerName);
            var connection = factory.CreateConnection();
            var mockDbProvider = new MockDbProvider(connection, connectionString);

            using (var adoTemplate = new AdoTemplate(mockDbProvider))
            {
                try
                {
                    var result = adoTemplate.ExecuteScalar<int>(null);
                    Assert.Fail("Should throw.");
                } catch (Exception)
                {
                    
                }

                var result2 = adoTemplate.ExecuteScalar<int>("select 1 from dual");
                Assert.IsTrue(result2 == 1);

                var result12 = adoTemplate.ExecuteScalar<int>(CommandType.Text, "select 1 from dual");
                Assert.IsTrue(result12 == 1);

                var result13 = adoTemplate.ExecuteScalar(CommandType.Text, "select 1 from dual");
                Assert.IsTrue(Convert.ToInt32(result13) == 1);

                using (var opw = new MockDbParametersWrapper<string>(connection))
                {
                    opw.Bind(command => {

                        opw["param"] = command.CreateParameter();
                        opw["param"].Value = 1;
                        opw["param"].DbType = DbType.Int32;
                        opw["param"].Direction = ParameterDirection.Input;
                    });

                    var result3 = adoTemplate.ExecuteScalar<int>("select 1 from dual where 1 = :opw", opw);
                    Assert.IsTrue(result3 == 1);

                    opw.Bind(command => {

                        opw["param"] = command.CreateParameter();
                        opw["param"].Value = 1;
                        opw["param"].DbType = DbType.Int32;
                        opw["param"].Direction = ParameterDirection.Input;
                    });

                    var result32 = adoTemplate.ExecuteScalar<int>(CommandType.Text, "select 1 from dual where 1 = :opw", opw);
                    Assert.IsTrue(result32 == 1);

                    opw.Bind(command => {

                        opw["param"] = command.CreateParameter();
                        opw["param"].Value = 1;
                        opw["param"].DbType = DbType.Int32;
                        opw["param"].Direction = ParameterDirection.Input;
                    });

                    var result33 = adoTemplate.ExecuteScalar(CommandType.Text, "select 1 from dual where 1 = :opw", opw);
                    Assert.IsTrue(Convert.ToInt32(result33) == 1);
                }
            }
        }

        [TestMethod()]
        public void ExecuteNonQueryTest()
        {
            var factory = DbProviderFactories.GetFactory(providerName);
            var connection = factory.CreateConnection();
            var mockDbProvider = new MockDbProvider(connection, connectionString);

            using (var adoTemplate = new AdoTemplate(mockDbProvider))
            {
                //TODO PARTIRE DA QUI.
                adoTemplate.ExecuteNonQuery("insert");
            }
        }

        [TestMethod()]
        public void ExecuteNonQueryTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ExecuteNonQueryTest2()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void QueryForObjectTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void QueryForObjectTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void QueryForObjectTest2()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void QueryForObjectTest3()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void QueryForObjectTest4()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void QueryForObjectTest5()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void QueryForListTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void QueryForListTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void QueryForListTest2()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void QueryForListTest3()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void QueryForListTest4()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void QueryForListTest5()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void QueryForListTest6()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void QueryForListTest7()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void QueryForListTest8()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void QueryForListTest9()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void QueryForListTest10()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void QueryForListTest11()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ExecuteQueryTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ExecuteQueryTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ExecuteQueryTest2()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ExecuteQueryTest3()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void QueryForDataTableTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void QueryForDataTableTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void QueryForDataTableTest2()
        {
            Assert.Fail();
        }
    }
}