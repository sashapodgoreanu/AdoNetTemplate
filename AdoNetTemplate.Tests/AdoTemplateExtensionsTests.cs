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
using AdoNetTemplate.Tests.HelperClass;

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

                var result2 = adoTemplate.ExecuteScalar<int>("select 1 from DUMMY_TABLE");
                Assert.IsTrue(result2 == 1);

                var result12 = adoTemplate.ExecuteScalar<int>(CommandType.Text, "select 1 from DUMMY_TABLE");
                Assert.IsTrue(result12 == 1);

                var result13 = adoTemplate.ExecuteScalar(CommandType.Text, "select 1 from DUMMY_TABLE");
                Assert.IsTrue(Convert.ToInt32(result13) == 1);

                using (var opw = new MockDbParametersWrapper<string>(connection))
                {
                    opw.Bind(command => {

                        opw["param"] = command.CreateParameter();
                        opw["param"].Value = 1;
                        opw["param"].DbType = DbType.Int32;
                        opw["param"].Direction = ParameterDirection.Input;
                    });

                    var result3 = adoTemplate.ExecuteScalar<int>("select 1 from DUMMY_TABLE where 1 = :opw", opw);
                    Assert.IsTrue(result3 == 1);

                    opw.Bind(command => {

                        opw["param"] = command.CreateParameter();
                        opw["param"].Value = 1;
                        opw["param"].DbType = DbType.Int32;
                        opw["param"].Direction = ParameterDirection.Input;
                    });

                    var result32 = adoTemplate.ExecuteScalar<int>(CommandType.Text, "select 1 from DUMMY_TABLE where 1 = :opw", opw);
                    Assert.IsTrue(result32 == 1);

                    opw.Bind(command => {

                        opw["param"] = command.CreateParameter();
                        opw["param"].Value = 1;
                        opw["param"].DbType = DbType.Int32;
                        opw["param"].Direction = ParameterDirection.Input;
                    });

                    var result33 = adoTemplate.ExecuteScalar(CommandType.Text, "select 1 from DUMMY_TABLE where 1 = :opw", opw);
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
                int retVal = adoTemplate.ExecuteNonQuery("insert into DUMMY_TABLE(COL_ID) values ('1')");
                Assert.AreEqual(retVal, 1);
            }
        }

        [TestMethod()]
        public void ExecuteNonQueryTest_WithParameters()
        {
            var factory = DbProviderFactories.GetFactory(providerName);
            var connection = factory.CreateConnection();
            var mockDbProvider = new MockDbProvider(connection, connectionString);

            using (var adoTemplate = new AdoTemplate(mockDbProvider))
            {
                using (var opw = new MockDbParametersWrapper<string>(connection))
                {
                    opw.Bind(command => {

                        opw["param"] = command.CreateParameter();
                        opw["param"].Value = 1;
                        opw["param"].DbType = DbType.String;
                        opw["param"].Direction = ParameterDirection.Input;
                    });

                    int retVal = adoTemplate.ExecuteNonQuery("insert into DUMMY_TABLE(COL_ID) values (:param1)", opw);
                    Assert.AreEqual(retVal, 1);
                }
            }
        }  

        [TestMethod()]
        public void QueryForListTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void QueryForListTest1()
        {
            var factory = DbProviderFactories.GetFactory(providerName);
            var connection = factory.CreateConnection();
            var mockDbProvider = new MockDbProvider(connection, connectionString);

            using (var adoTemplate = new AdoTemplate(mockDbProvider))
            {
                
                var retVal = adoTemplate.QueryForList("select * from DUMMY_TABLE", new DummyTableRowMapper());

                Assert.IsNotNull(retVal.FirstOrDefault());

            }
        }

        [TestMethod()]
        public void QueryForListTest2()
        {
            var factory = DbProviderFactories.GetFactory(providerName);
            var connection = factory.CreateConnection();
            var mockDbProvider = new MockDbProvider(connection, connectionString);

            using (var adoTemplate = new AdoTemplate(mockDbProvider))
            {
                using (var opw = new MockDbParametersWrapper<string>(connection))
                {
                    opw.Bind(command => {

                        opw["param"] = command.CreateParameter();
                        opw["param"].Value = 1;
                        opw["param"].DbType = DbType.String;
                        opw["param"].Direction = ParameterDirection.Input;
                    });

                    var retVal = adoTemplate.QueryForList(
                        "select * from DUMMY_TABLE where 1 = :param",
                        opw,
                        new DummyTableRowMapper());

                    Assert.IsNotNull(retVal.FirstOrDefault());
                }
            }
        }

        [TestMethod()]
        public void QueryForListTest3()
        {
            var factory = DbProviderFactories.GetFactory(providerName);
            var connection = factory.CreateConnection();
            var mockDbProvider = new MockDbProvider(connection, connectionString);

            using (var adoTemplate = new AdoTemplate(mockDbProvider))
            {
                using (var opw = new MockDbParametersWrapper<string>(connection))
                {
                    opw.Bind(command => {

                        opw["param"] = command.CreateParameter();
                        opw["param"].Value = 1;
                        opw["param"].DbType = DbType.String;
                        opw["param"].Direction = ParameterDirection.Input;
                    });

                    var retVal = adoTemplate.QueryForList(
                        "select * from DUMMY_TABLE where 1 = :param",
                        opw,
                        (r, i) => 
                        {
                            var mapper = new DummyTableRowMapper();
                            return mapper.MapRow(r,i);
                        });

                    Assert.IsNotNull(retVal.FirstOrDefault());
                }
            }
        }

        [TestMethod()]
        public void QueryForListTest4()
        {
            var factory = DbProviderFactories.GetFactory(providerName);
            var connection = factory.CreateConnection();
            var mockDbProvider = new MockDbProvider(connection, connectionString);

            using (var adoTemplate = new AdoTemplate(mockDbProvider))
            {
                using (var opw = new MockDbParametersWrapper<string>(connection))
                {
                    opw.Bind(command => {

                        opw["param"] = command.CreateParameter();
                        opw["param"].Value = 1;
                        opw["param"].DbType = DbType.String;
                        opw["param"].Direction = ParameterDirection.Input;
                    });

                    var retVal = adoTemplate.QueryForList(
                        "select * from DUMMY_TABLE where 1 = :param",
                        opw,
                        (r, i) =>
                        {
                            var mapper = new DummyTableRowMapper();
                            return mapper.MapRow(r, i);
                        });

                    Assert.IsNotNull(retVal.FirstOrDefault());
                }
            }
        }

        [TestMethod()]
        public void QueryForListTest5()
        {
            var factory = DbProviderFactories.GetFactory(providerName);
            var connection = factory.CreateConnection();
            var mockDbProvider = new MockDbProvider(connection, connectionString);

            using (var adoTemplate = new AdoTemplate(mockDbProvider))
            {
                using (var opw = new MockDbParametersWrapper<string>(connection))
                {
                    opw.Bind(command => {

                        opw["param"] = command.CreateParameter();
                        opw["param"].Value = 1;
                        opw["param"].DbType = DbType.String;
                        opw["param"].Direction = ParameterDirection.Input;
                    });

                    var retVal = adoTemplate.QueryForList(
                        "select * from DUMMY_TABLE where 1 = :param",
                        opw,
                        (r) =>
                        {
                            var mapper = new DummyTableRowMapper();
                            var list = new List<DummyTable>();
                            var i = 0;
                            while (r.Read())
                            {
                                list.Add(mapper.MapRow(r, i++));
                            }

                            return list;
                        });

                    Assert.IsNotNull(retVal.FirstOrDefault());
                }
            }
        }

        [TestMethod()]
        public void QueryForListTest6()
        {
            var factory = DbProviderFactories.GetFactory(providerName);
            var connection = factory.CreateConnection();
            var mockDbProvider = new MockDbProvider(connection, connectionString);

            using (var adoTemplate = new AdoTemplate(mockDbProvider))
            {
                var retVal = adoTemplate.QueryForList(
                    "select * from DUMMY_TABLE",
                    (r) =>
                    {
                        var mapper = new DummyTableRowMapper();
                        var list = new List<DummyTable>();
                        var i = 0;
                        while (r.Read())
                        {
                            list.Add(mapper.MapRow(r, i++));
                        }

                        return list;
                    });
                    Assert.IsNotNull(retVal.FirstOrDefault());
            }
        }

        [TestMethod()]
        public void QueryForListTest7()
        {
            var factory = DbProviderFactories.GetFactory(providerName);
            var connection = factory.CreateConnection();
            var mockDbProvider = new MockDbProvider(connection, connectionString);

            using (var adoTemplate = new AdoTemplate(mockDbProvider))
            {
                var retVal = adoTemplate.QueryForList(
                    "select * from DUMMY_TABLE where 1 = :param",
                    (r, i) =>
                    {
                        var mapper = new DummyTableRowMapper();
                        return mapper.MapRow(r, i);
                    });

                Assert.IsNotNull(retVal.FirstOrDefault());
            }
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