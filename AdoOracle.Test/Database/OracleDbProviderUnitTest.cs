using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using AdoOracle.Database;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Linq;

namespace AdoOracle.Test.Database
{
    [TestClass]
    public class OracleDbProviderUnitTest
    {
        public string ConnectionString;
        public string CreateDummyTestTable;
        public string DropDummyTestTable;


        [TestInitialize]
        public void Initialize()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["Oracle"].ConnectionString;
            CreateDummyTestTable = "CREATE TABLE DUMMYTESTTABLE(ID_NUMBER NUMBER)";
            DropDummyTestTable = "DROP TABLE DUMMYTESTTABLE";
            using (OracleConnection connection = new OracleConnection(ConnectionString))
            {
                connection.Open();

                using (OracleCommand command = connection.CreateCommand())
                {
                    try
                    {
                        command.CommandText = DropDummyTestTable;
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        
                    }
                    finally
                    {
                        command.CommandText = CreateDummyTestTable;
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            using (OracleConnection connection = new OracleConnection(ConnectionString))
            {
                connection.Open();
                using (OracleCommand command = connection.CreateCommand())
                {
                    command.CommandText = DropDummyTestTable;
                    command.ExecuteNonQuery();
                }
            }
        }

        [TestMethod]
        [TestCategory("Core")]
        public void TestOpenConnectio()
        {
            using (OracleDbProvider dbProvider = new OracleDbProvider(ConnectionString))
            {
                dbProvider.OpenConnection();

                if (dbProvider.Connection.State != ConnectionState.Open)
                {
                    Assert.Fail("Connection should be open!");
                }
            }
        }

        [TestMethod]
        [TestCategory("Core")]
        public void TestOpenConnectioTwice()
        {
            using (OracleDbProvider dbProvider = new OracleDbProvider(ConnectionString))
            {
                dbProvider.OpenConnection();
                dbProvider.OpenConnection();

                if (dbProvider.Connection.State != ConnectionState.Open)
                {
                    Assert.Fail("Connection should be open!");
                }
            }
        }

        [TestMethod]
        [TestCategory("Core")]
        public void TestOpenConnectioCloseConnectionReOpenConnection()
        {
            using (OracleDbProvider dbProvider = new OracleDbProvider(ConnectionString))
            {
                dbProvider.OpenConnection();
                dbProvider.CloseConnection();
                dbProvider.OpenConnection();

                if (dbProvider.Connection.State != ConnectionState.Open)
                {
                    Assert.Fail("Connection should be open!");
                }
            }
        }

        [TestMethod]
        [TestCategory("Core")]
        public void TestOpenConnectioDisposeConnectionReOpenConnection()
        {
            using (OracleDbProvider dbProvider = new OracleDbProvider(ConnectionString))
            {
                dbProvider.OpenConnection();
                dbProvider.Connection.Dispose();
                dbProvider.OpenConnection();

                if (dbProvider.Connection.State != ConnectionState.Open)
                {
                    Assert.Fail("Connection should be open!");
                }
            }
        }

        [TestMethod]
        [TestCategory("Core")]
        public void TestCloseConnection()
        {
            using (OracleDbProvider dbProvider = new OracleDbProvider(ConnectionString))
            {
                dbProvider.OpenConnection();
                dbProvider.CloseConnection();

                if (dbProvider.Connection.State != ConnectionState.Closed)
                {
                    Assert.Fail("Connection should be closed!");
                }
            }
        }

        [TestMethod]
        [TestCategory("Core")]
        public void TestCloseConnectionIfConnectionDestroyed()
        {
            using (OracleDbProvider dbProvider = new OracleDbProvider(ConnectionString))
            {
                dbProvider.OpenConnection();
                dbProvider.Connection.Dispose();

                try
                {
                    dbProvider.CloseConnection();
                    Assert.Fail("Should throw");
                }
                catch (Exception)
                {

                }
            }
        }

        [TestMethod]
        [TestCategory("Core")]
        public void TestCommitTransaction()
        {
            using (OracleDbProvider dbProvider = new OracleDbProvider(ConnectionString))
            {
                dbProvider.OpenConnection();
                dbProvider.BeginTransaction();

                var sqlInsert = "INSERT INTO DUMMYTESTTABLE(ID_NUMBER) VALUES(0)";
                var sqlSelect = "SELECT * FROM DUMMYTESTTABLE";


                using (var command = dbProvider.Connection.CreateCommand())
                {
                    command.CommandText = sqlInsert;
                    command.ExecuteNonQuery();
                }

                dbProvider.CommitTransaction();

                using (var command = dbProvider.Connection.CreateCommand())
                {
                    command.CommandText = sqlSelect;
                    using (var reader = command.ExecuteReader())
                    {
                        List<object> results = new List<object>();
                        while (reader.Read())
                        {
                            results.Add(reader.GetValue(0));
                        }

                        if (results.Count() == 0)
                        {
                            Assert.Fail("Expected rows!");
                        }
                    }   
                }
            }
        }

        [TestMethod]
        [TestCategory("Core")]
        public void TestCommitTransactionIfConnectionDisposed()
        {
            using (OracleDbProvider dbProvider = new OracleDbProvider(ConnectionString))
            {
                dbProvider.OpenConnection();
                dbProvider.BeginTransaction();

                var sqlInsert = "INSERT INTO DUMMYTESTTABLE(ID_NUMBER) VALUES(0)";
                var sqlSelect = "SELECT * FROM DUMMYTESTTABLE";
                var sqlTruncate = "TRUNCATE TABLE DUMMYTESTTABLE";

                using (var command = dbProvider.Connection.CreateCommand())
                {
                    command.CommandText = sqlTruncate;
                    command.ExecuteNonQuery();

                    command.CommandText = sqlInsert;
                    command.ExecuteNonQuery();
                }

                dbProvider.Connection.Dispose();

                try
                {
                    dbProvider.CommitTransaction();
                    Assert.Fail("Expected Exception!");
                }
                catch (Exception)
                {

                }

                dbProvider.OpenConnection();

                using (var command = dbProvider.Connection.CreateCommand())
                {
                    command.CommandText = sqlSelect;
                    using (var reader = command.ExecuteReader())
                    {
                        List<object> results = new List<object>();
                        while (reader.Read())
                        {
                            results.Add(reader.GetValue(0));
                        }

                        if (results.Count() != 0)
                        {
                            Assert.Fail("Expected no rows!");
                        }
                    }
                }
            }
        }

        [TestMethod]
        [TestCategory("Core")]
        public void TestRollBackTransaction()
        {
            using (OracleDbProvider dbProvider = new OracleDbProvider(ConnectionString))
            {
                var sqlInsert = "INSERT INTO DUMMYTESTTABLE(ID_NUMBER) VALUES(0)";
                var sqlSelect = "SELECT * FROM DUMMYTESTTABLE";
                var sqlTruncate = "TRUNCATE TABLE DUMMYTESTTABLE";
                dbProvider.OpenConnection();


                using (var command = dbProvider.Connection.CreateCommand())
                {
                    command.CommandText = sqlTruncate;
                    command.ExecuteNonQuery();
                }

                dbProvider.BeginTransaction();

                using (var command = dbProvider.Connection.CreateCommand())
                {
                    command.CommandText = sqlInsert;
                    command.ExecuteNonQuery();
                }



                dbProvider.RollbackTransaction();

                using (var command = dbProvider.Connection.CreateCommand())
                {
                    command.CommandText = sqlSelect;
                    using (var reader = command.ExecuteReader())
                    {
                        List<object> results = new List<object>();
                        while (reader.Read())
                        {
                            results.Add(reader.GetValue(0));
                        }

                        if (results.Any())
                        {
                            Assert.Fail("Expected no rows!");
                        }
                    }
                }
            }
        }

        [TestMethod]
        [TestCategory("Core")]
        public void TestRollBackTransactionIfConnectionDisposed()
        {
            using (OracleDbProvider dbProvider = new OracleDbProvider(ConnectionString))
            {
                var sqlInsert = "INSERT INTO DUMMYTESTTABLE(ID_NUMBER) VALUES(0)";
                var sqlSelect = "SELECT * FROM DUMMYTESTTABLE";
                var sqlTruncate = "TRUNCATE TABLE DUMMYTESTTABLE";
                dbProvider.OpenConnection();


                using (var command = dbProvider.Connection.CreateCommand())
                {
                    command.CommandText = sqlTruncate;
                    command.ExecuteNonQuery();
                }

                dbProvider.BeginTransaction();

                using (var command = dbProvider.Connection.CreateCommand())
                {
                    command.CommandText = sqlInsert;
                    command.ExecuteNonQuery();
                }

                dbProvider.Connection.Dispose();

                try
                {
                    dbProvider.RollbackTransaction();
                    Assert.Fail("Expected Exception!");
                }
                catch (Exception)
                {

                }

                dbProvider.OpenConnection();

                using (var command = dbProvider.Connection.CreateCommand())
                {
                    command.CommandText = sqlSelect;
                    using (var reader = command.ExecuteReader())
                    {
                        List<object> results = new List<object>();
                        while (reader.Read())
                        {
                            results.Add(reader.GetValue(0));
                        }

                        if (results.Any())
                        {
                            Assert.Fail("Expected no rows!");
                        }
                    }
                }
            }
        }
    }
}
