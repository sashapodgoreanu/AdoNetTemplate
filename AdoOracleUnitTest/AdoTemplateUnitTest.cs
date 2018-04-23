using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using AdoNetTemplate;
using AdoOracle.Database;
using System.Linq;
using Oracle.ManagedDataAccess.Client;
using AdoOracle;
using AdoNetTemplate.Database.Core;
using System.Data;
using System.Data.Common;
using static AdoNetTemplate.AdoTemplate;
using AdoOracle.Support;

namespace AdoOracleUnitTest
{
    [TestClass]
    public class AdoTemplateUnitTest
    {

        public string ConnectionString;
        public OracleFactory OracleFactory;

        private const string createDummyTable =
@"CREATE TABLE DUMMY_TABLE 
(
    COL_ID VARCHAR2(100),
    COL_INT NUMBER(10,0),
    COL_LONG NUMBER(19,0),
    COL_DOUBLE BINARY_DOUBLE,
    COL_DECIMAL NUMBER,
    COL_STRING VARCHAR2(3000),
    COL_CHAR CHAR,
    COL_DATE DATE,
    COL_CLOB CLOB
)";

        private const string dropDummyTable =
@"DROP TABLE DUMMY_TABLE";

        [TestInitialize]
        public void Initialize()
        {
            OracleFactory = new OracleFactory();


            ConnectionString = ConfigurationManager.ConnectionStrings["Oracle"].ConnectionString;
            using (var adoTemplate = OracleFactory.CreateAdoTemplate(ConnectionString))
            {
                try
                {
                    adoTemplate.ExecuteNonQuery(dropDummyTable);
                }
                catch
                { }

                adoTemplate.ExecuteNonQuery(createDummyTable);
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            using (var adoTemplate = OracleFactory.CreateAdoTemplate(ConnectionString))
            {
                try
                {
                    adoTemplate.ExecuteNonQuery(dropDummyTable);
                }
                catch
                { }

            }
        }

        [TestMethod()]
        [TestCategory("AdoTemplate")]
        public void ExecuteTest()
        {
            using (var adoTemplate = OracleFactory.CreateAdoTemplate(ConnectionString))
            {
                using (var opw = new OracleParametersWrapper<string>())
                {

                    opw.SetString("COL_ID"     , "ID"    ); 
                    opw.SetInt("COL_INT"        , 0  );
                    opw.SetLong("COL_LONG"   ,    0  );
                    opw.SetDouble("COL_DOUBLE" ,   1 );
                    opw.SetDecimal("COL_DECIMAL",  1 );
                    opw.SetString("COL_STRING" ,   "empty" );
                    opw.SetBoolean("COL_CHAR", true );
                    opw.SetDateTime("COL_DATE"   , DateTime.Now );
                    opw.SetClob("COL_CLOB"   ,     "BIG STRING");

                    NonQueryCallBack t = new NonQueryCallBack(CommandType.Text,
                        OracleSQLBuilder.BuildInsert("DUMMY_TABLE", new[]
                        {
                        "COL_ID"            ,
                        "COL_INT"           ,
                        "COL_LONG"          ,
                        "COL_DOUBLE"        ,
                        "COL_DECIMAL"       ,
                        "COL_STRING"        ,
                        "COL_CHAR"          ,
                        "COL_DATE"          ,
                        "COL_CLOB"          ,
                        }), opw);
                    int result = adoTemplate.Execute(t);
                }
            }
        }

        [TestMethod()]
        [TestCategory("AdoTemplate")]
        public void ExecuteTest1()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestCategory("AdoNetExtensions")]
        public void ExecuteScalar_For_Object()
        {
            using (var adoTemplate = OracleFactory.CreateAdoTemplate(ConnectionString))
            {
                var obj = adoTemplate.ExecuteScalar("select 1 as \"Number\" from dual");

                //this will fail because we don't know what type we are Unboxing
                //var intObj = (int)(obj);
                //this will work
                var intObj = Convert.ToInt32(obj);
                if (intObj != 1)
                {
                    Assert.Fail($"{nameof(obj)} should be 1");
                }

                //this will fail because we don't know what type we are Unboxing, maybe is string
                //var intObj = (string)(objString);
                //this will work
                obj = adoTemplate.ExecuteScalar("select 'Sono una stringa' as \"Stringa\" from dual");
                var objString = Convert.ToString(obj);
                if (!objString.Equals("Sono una stringa"))
                {
                    Assert.Fail($"{nameof(obj)} should be Sono una stringa");
                }
            }  
        }

        [TestMethod]
        [TestCategory("AdoNetExtensions")]
        public void ExecuteScalar_For_Object_CallFunction()
        {
            using (var adoTemplate = OracleFactory.CreateAdoTemplate(ConnectionString))
            {
                adoTemplate.ExecuteNonQuery(
@"
create or replace function getSysdate
return date is

  l_sysdate date;

begin

  select sysdate
    into l_sysdate
    from dual;

  return NULL;

end;
");
                var returnVal = adoTemplate.ExecuteScalar("select getSysdate() from dual");
                
                adoTemplate.ExecuteNonQuery("drop function getSysdate");

            }
        }


        [TestMethod]
        [TestCategory("AdoNetExtensions")]
        public void ExecuteNonQuery_Insert_Test()
        {
            using (var adoTemplate = OracleFactory.CreateAdoTemplate(ConnectionString))
            {
                //Insert integer
                var inserted = adoTemplate.ExecuteNonQuery($"Insert into DUMMY_TABLE(COL_INT) values ({int.MaxValue})");
                //check inserted
                if (inserted != 1)
                {
                    Assert.Fail($"{nameof(inserted)} should be 1");
                }

                //Insert string
                inserted = adoTemplate.ExecuteNonQuery($"Insert into DUMMY_TABLE(COL_STRING) values ('{"Im A string"}')");
                //check inserted
                if (inserted != 1)
                {
                    Assert.Fail($"{nameof(inserted)} should be 1");
                }
            }
        }

        [TestMethod]
        [TestCategory("AdoNetExtensions")]
        public void ExecuteNonQuery_Update_Test()
        {
            using (var adoTemplate = OracleFactory.CreateAdoTemplate(ConnectionString))
            {
                //Insert integer
                var id = Guid.NewGuid().ToString();
                var inserted = adoTemplate.ExecuteNonQuery($"Insert into DUMMY_TABLE(COL_ID, COL_INT) values ('{id}', {int.MaxValue})");
                var updated = adoTemplate.ExecuteNonQuery($"update DUMMY_TABLE set COL_INT = {12} where COL_ID ='{id}'");
                //check updated
                if (updated != 1)
                {
                    Assert.Fail($"{nameof(updated)} should be 1");
                }

                //Insert string
                id = Guid.NewGuid().ToString();
                inserted = adoTemplate.ExecuteNonQuery($"Insert into DUMMY_TABLE(COL_ID, COL_STRING) values ('{id}', '{"I m A string"}')");
                updated = adoTemplate.ExecuteNonQuery($"update DUMMY_TABLE set COL_STRING = '{"sono un altra stringa"}' where COL_ID ='{id}'");
                //check updated
                if (updated != 1)
                {
                    Assert.Fail($"{nameof(updated)} should be 1");
                }
                
            }
        }

        [TestMethod]
        [TestCategory("AdoNetExtensions")]
        public void ExecuteNonQuery_Delete_Test()
        {
            using (var adoTemplate = OracleFactory.CreateAdoTemplate(ConnectionString))
            {
                //Insert integer
                var id = Guid.NewGuid().ToString();
                var inserted = adoTemplate.ExecuteNonQuery($"Insert into DUMMY_TABLE(COL_ID, COL_INT) values ('{id}', {int.MaxValue})");
                var deleted = adoTemplate.ExecuteNonQuery($"delete from DUMMY_TABLE where COL_ID ='{id}'");
                //check deleted
                if (deleted != 1)
                {
                    Assert.Fail($"{nameof(deleted)} should be 1");
                }

                //Insert string
                id = Guid.NewGuid().ToString();
                inserted = adoTemplate.ExecuteNonQuery($"Insert into DUMMY_TABLE(COL_ID, COL_STRING) values ('{id}', '{"I m A string"}')");
                deleted = adoTemplate.ExecuteNonQuery($"delete from DUMMY_TABLE where COL_ID ='{id}'");
                //check deleted
                if (deleted != 1)
                {
                    Assert.Fail($"{nameof(deleted)} should be 1");
                }
            }
        }

        [TestMethod]
        [TestCategory("AdoNetExtensions")]
        public void ExecuteNonQuery_Create_Drop_Test()
        {
            using (var adoTemplate = OracleFactory.CreateAdoTemplate(ConnectionString))
            {
                try
                {
                    var created = adoTemplate.ExecuteNonQuery($"Create table dumy_to_be_dropped ( UNA_COLLONA varchar2(1) )");
                    //check created
                    if (created != 1)
                    {
                        Assert.Fail($"{nameof(created)} should be 1");
                    }
                }
                catch (Exception x)
                {
                    if (x is OracleException)
                    {
                        var e = x as OracleException;
                        if (e.ErrorCode != 955)
                            throw;
                    }
                }
                finally
                {
                    try
                    {
                        adoTemplate.ExecuteNonQuery($"drop table dumy_to_be_dropped");
                    }
                    catch { }
                    
                }
            }
        }

        

        [TestMethod()]
        public void ExecuteNonQueryTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ExecuteScalarTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void QueryForDataTableTest()
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
        public void BeginTransactionTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CommitTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RollBackTest()
        {
            Assert.Fail();
        }
    }
}
