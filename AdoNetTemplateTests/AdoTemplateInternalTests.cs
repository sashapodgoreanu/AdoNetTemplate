using Microsoft.VisualStudio.TestTools.UnitTesting;
using AdoNetTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdoNetTemplate.Database.Generic;
using System.Data.Common;
using AdoNetTemplate.Tests.DataBase;
using static AdoNetTemplate.AdoTemplate;
using System.Data;
using AdoNetTemplate.Database.Support;
using System.Configuration;
using AdoNetTemplate.Tests.HelperClass;
using AdoNetTemplate.Database.Core;

namespace AdoNetTemplate.Tests
{
    /// <summary>
    /// Internal methods and class unit test.
    /// </summary>
    [TestClass]
    
    public class AdoTemplateInternalTests
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
        [TestCategory("AdoTemplate internal class")]
        public void ExecuteTest_QueryCallback()
        {
            var factory = DbProviderFactories.GetFactory(providerName);
            var connection = factory.CreateConnection();
            var mockDbProvider = new MockDbProvider(connection, connectionString);

            using (var adoTemplate = new AdoTemplate(mockDbProvider))
            {
                var mapper = new DummyTableRowMapper();
                var rmrse = new RowMapperResultSetExtractor<DummyTable>(mapper);
                var t = new QueryCallback<IList<DummyTable>>(CommandType.Text, "select * from DUMMY_TABLE", null, rmrse);
                var result = adoTemplate.Execute(t);

                Assert.IsNotNull(result);
                Assert.AreNotEqual(result.Count(), 0);
            }
        }


        [TestMethod()]
        [TestCategory("AdoTemplate internal class")]
       
        public void ExecuteTest_QueryCallbackDBDataReader()
        {
            var factory = DbProviderFactories.GetFactory(providerName);
            var connection = factory.CreateConnection();
            var mockDbProvider = new MockDbProvider(connection, connectionString);

            using (var adoTemplate = new AdoTemplate(mockDbProvider))
            {
                Action<IDataReader> mapper = reader =>
                {
                    Assert.IsNotNull(reader);
                };

                var rmrse = new IDataReaderAccessorImplementor(mapper);

                var t = new QueryCallbackDBDataReader(CommandType.Text, "select * from DUMMY_TABLE", null, rmrse);
                var result = adoTemplate.Execute(t);

                Assert.IsNotNull(result);
                Assert.IsTrue(result);
            }
        }

        [TestMethod()]
        [TestCategory("AdoTemplate internal class")]
        public void ExecuteTest_NonQueryCallBack()
        {
            var factory = DbProviderFactories.GetFactory(providerName);
            var connection = factory.CreateConnection();
            var mockDbProvider = new MockDbProvider(connection, connectionString);

            using (var adoTemplate = new AdoTemplate(mockDbProvider))
            {
                using (var opw = new MockDbParametersWrapper<string>(connection))
                {
                    opw.Bind(command => {

                        opw["COL_ID"] = command.CreateParameter();
                        opw["COL_ID"].Value = "1";
                        opw["COL_ID"].DbType = DbType.String;
                        opw["COL_ID"].Direction = ParameterDirection.Input;

                        opw["COL_INT"] = command.CreateParameter();
                        opw["COL_INT"].Value = 1;
                        opw["COL_INT"].DbType = DbType.Int32;
                        opw["COL_INT"].Direction = ParameterDirection.Input;

                        opw["COL_LONG"] = command.CreateParameter();
                        opw["COL_LONG"].Value = 1;
                        opw["COL_LONG"].DbType = DbType.Int64;
                        opw["COL_LONG"].Direction = ParameterDirection.Input;

                        opw["COL_DOUBLE"] = command.CreateParameter();
                        opw["COL_DOUBLE"].Value = 1.1;
                        opw["COL_DOUBLE"].DbType = DbType.Double;
                        opw["COL_DOUBLE"].Direction = ParameterDirection.Input;

                        opw["COL_DECIMAL"] = command.CreateParameter();
                        opw["COL_DECIMAL"].Value = 1;
                        opw["COL_DECIMAL"].DbType = DbType.Decimal;
                        opw["COL_DECIMAL"].Direction = ParameterDirection.Input;

                        opw["COL_STRING"] = command.CreateParameter();
                        opw["COL_STRING"].Value = "1";
                        opw["COL_STRING"].DbType = DbType.String;
                        opw["COL_STRING"].Direction = ParameterDirection.Input;

                        opw["COL_DATE"] = command.CreateParameter();
                        opw["COL_DATE"].Value = DateTime.Now;
                        opw["COL_DATE"].DbType = DbType.DateTime;
                        opw["COL_DATE"].Direction = ParameterDirection.Input;

                        opw["COL_CLOB"] = command.CreateParameter();
                        opw["COL_CLOB"].Value = "1";
                        opw["COL_CLOB"].DbType = DbType.String;
                        opw["COL_CLOB"].Direction = ParameterDirection.Input;
                    });


                    NonQueryCallBack t = new NonQueryCallBack(CommandType.Text,
                        SQLBuilder.BuildInsert("DUMMY_TABLE", new[]
                        {
                        "COL_ID"            ,
                        "COL_INT"           ,
                        "COL_LONG"          ,
                        "COL_DOUBLE"        ,
                        "COL_DECIMAL"       ,
                        "COL_STRING"        ,
                        "COL_DATE"          ,
                        "COL_CLOB"          ,
                        }), opw);
                    int result = adoTemplate.Execute(t);
                    if (result != 1)
                    {
                        Assert.Fail("expected 1 insert.");
                    }
                }
            }
        }

        [TestMethod()]
        [TestCategory("AdoTemplate internal class")]
        public void ExecuteTest_DataAdapterFillCallbackWithParameters()
        {
            var factory = DbProviderFactories.GetFactory(providerName);
            var connection = factory.CreateConnection();
            var mockDbProvider = new MockDbProvider(connection, connectionString);

            using (var adoTemplate = new AdoTemplate(mockDbProvider))
            {
                var dataTable = new DataTable();
                var t = new DataAdapterFillCallbackWithParameters(
                    dataTable,
                    CommandType.Text,
                    "select * from DUMMY_TABLE", 
                    null);

                var result = adoTemplate.Execute(t);

                Assert.IsNotNull(dataTable);
            }

            using (var adoTemplate = new AdoTemplate(mockDbProvider))
            {
                using (var opw = new MockDbParametersWrapper<string>(connection))
                {
                    opw.Bind(cmd =>
                    {
                        opw["param1"] = cmd.CreateParameter();
                        opw["param1"].Direction = ParameterDirection.Input;
                        opw["param1"].Value = 1;
                        opw["param1"].DbType = DbType.Int32;
                    });
                
                    var dataTable = new DataTable();
                    var t = new DataAdapterFillCallbackWithParameters(
                        dataTable,
                        CommandType.Text,
                        "select * from DUMMY_TABLE where 1 = :param",
                        opw);

                    var result = adoTemplate.Execute(t);

                    Assert.IsNotNull(dataTable);
                }
            }
        }

        [TestMethod()]
        [TestCategory("AdoTemplate internal class")]
        public void ExecuteTest_ExecuteScalarCallbackWithParameters()
        {
            var factory = DbProviderFactories.GetFactory(providerName);
            var connection = factory.CreateConnection();
            var mockDbProvider = new MockDbProvider(connection, connectionString);

            using (var adoTemplate = new AdoTemplate(mockDbProvider))
            {
                var t = new ExecuteScalarCallbackWithParameters(
                    CommandType.Text,
                    "select 1 as \"Number\" from dual",
                    null);

                var result = adoTemplate.Execute(t);

                Assert.IsNotNull(result);
                result = Convert.ChangeType(result, typeof(int));
                Assert.IsTrue(1.CompareTo(result) == 0);
            }

            using (var adoTemplate = new AdoTemplate(mockDbProvider))
            {
                using (var opw = new MockDbParametersWrapper<string>(connection))
                {
                    opw.Bind(cmd =>
                    {
                        opw["param1"] = cmd.CreateParameter();
                        opw["param1"].Direction = ParameterDirection.Input;
                        opw["param1"].Value = 1;
                        opw["param1"].DbType = DbType.Int32;
                    });

                    var t = new ExecuteScalarCallbackWithParameters(
                    CommandType.Text,
                    "select 1 as \"Number\" from dual where 1 = :param1",
                    opw);

                    var result = adoTemplate.Execute(t);

                    Assert.IsNotNull(result);
                    result = Convert.ChangeType(result, typeof(int));
                    Assert.IsTrue(1.CompareTo(result) == 0);
                }
            }
        }
    }
}