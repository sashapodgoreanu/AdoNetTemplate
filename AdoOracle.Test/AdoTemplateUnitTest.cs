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
using AdoOracle.Test.HelperClass;
using System.Collections.Generic;
using AdoNetTemplate.Database.Support;

namespace AdoOracle.Test
{
    [TestClass]
    public class AdoTemplateUnitTest
    {

        public const string ConnectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.175.41.33)(PORT=1521))(CONNECT_DATA=(FAILOVER_MODE=(TYPE=select)(METHOD=basic))(SERVER=dedicated)(SERVICE_NAME=ORA12DB)));User ID=monitor;Password=monitor;";
        public OracleFactory OracleFactory;
        private TestContext testContextInstance;

        public TestContext TestContext
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
        public static void AssemblyInit(TestContext context)
        {
            OracleFactory OracleFactory = new OracleFactory();
            using (var adoTemplate = OracleFactory.CreateAdoTemplate(ConnectionString))
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
            OracleFactory OracleFactory = new OracleFactory();
            using (var adoTemplate = OracleFactory.CreateAdoTemplate(ConnectionString))
            {
                try
                {
                    adoTemplate.ExecuteNonQuery(dropDummyTable);
                }
                catch (Exception x)
                { }
            }
        }

        [TestInitialize]
        public void Initialize()
        {
            OracleFactory = new OracleFactory();
        }

        [TestCleanup]
        public void Cleanup()
        {
        }
        
        [TestMethod()]
        [TestCategory("AdoTemplate")]
        public void ExecuteTest_NonQueryCallBack()
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
                    opw.SetDateTime("COL_DATE"   , DateTime.Now );
                    opw.SetClob("COL_CLOB"   ,     "BIG STRING");

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
        [TestCategory("AdoTemplate")]
        [DataSource("Oracle.ManagedDataAccess.Client", ConnectionString,"DUMMY_TABLE", DataAccessMethod.Sequential)]
        public void ExecuteTest_QueryCallback()
        {
            using (var adoTemplate = OracleFactory.CreateAdoTemplate(ConnectionString))
            {
                var mapper = new DummyTableRowMapper();
                var rmrse = new RowMapperResultSetExtractor<DummyTable>(mapper);
                var t = new QueryCallback<IList<DummyTable>>(CommandType.Text,"select * from DUMMY_TABLE", null, rmrse);
                var result = adoTemplate.Execute(t);

                if (result.Count() != TestContext.DataRow.Table.Rows.Count)
                {
                    Assert.Fail("Incorect result.");
                }
            }
        }


        [TestMethod()]
        [TestCategory("AdoTemplate")]
        [DataSource("Oracle.ManagedDataAccess.Client", ConnectionString, "DUMMY_TABLE", DataAccessMethod.Sequential)]
        public void ExecuteTest_QueryCallbackDBDataReader()
        {
            using (var adoTemplate = OracleFactory.CreateAdoTemplate(ConnectionString))
            {
                /*var mapper = new DummyTableRowMapper();
                var rmrse = new RowMapperResultSetExtractor<DummyTable>(mapper);
                var t = new QueryCallbackDBDataReader(CommandType.Text, "select * from DUMMY_TABLE", null, rmrse);
                var result = adoTemplate.Execute(t);

                if (result.Count() != TestContext.DataRow.Table.Rows.Count)
                {
                    Assert.Fail("Incorect result.");
                }*/
            }
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
    }
}
