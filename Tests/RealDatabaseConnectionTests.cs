using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using SqlDataReaderMapper;

namespace Tests
{
    [TestClass]
    public class RealDatabaseConnectionTests
    {
        private string _connectionString;
        [TestInitialize]
        public void Setup()
        {
            _connectionString = ConfigurationManager.AppSettings["ConnectionString"];
        }
        [TestMethod]
        public void ReadDataFromStoredProcedure_Success()
        {
            DatabaseConnector connector = new DatabaseConnector(_connectionString);
            List<SqlParameter> sqlParameter = new List<SqlParameter>() {new SqlParameter() {ParameterName = "@Id", Value = 0}};
            IEnumerable<TestTable> testTables = connector.ReadDataFromStoredProcedure<TestTable>("sp_TestTableStoredProc", sqlParameter);
            Assert.IsNotNull(testTables);
            Assert.AreNotEqual(testTables.Count(), 0);
        }
        [TestMethod]
        public void ReadDataFromTable_Success()
        {
            DatabaseConnector connector = new DatabaseConnector(_connectionString);
            IEnumerable<TestTable> testTables = connector.ReadDataFromSqlCommand<TestTable>("Select * from TestTable");
            Assert.IsNotNull(testTables);
            Assert.AreNotEqual(testTables.Count(), 0);
        }
        [TestMethod]
        public void ReadCountFromTable_Success()
        {
            DatabaseConnector connector = new DatabaseConnector(_connectionString);
            int testTablesCount = connector.ReadDataFromSqlCommand<int>("Select count(*) from TestTable").FirstOrDefault();
            Assert.AreNotEqual(testTablesCount, 0);
        }
        [TestMethod]
        public void ReadEnumColumnFromTable_Success()
        {
            DatabaseConnector connector = new DatabaseConnector(_connectionString);
            TestEnum value = connector.ReadDataFromSqlCommand<TestEnum>("Select EnumColumn from TestTable").FirstOrDefault();
            Assert.AreNotEqual(value, 0);
        }
        [TestMethod]
        public void ReadNullableEnumColumnFromTable_Success()
        {
            DatabaseConnector connector = new DatabaseConnector(_connectionString);
            TestEnum? value = connector.ReadDataFromSqlCommand<TestEnum?>("Select NullableEnumColumn from TestTable").FirstOrDefault();
            Assert.AreEqual(value, null);
        }
        [TestMethod]
        public void ReadNullValueFromTable_Success()
        {
            DatabaseConnector connector = new DatabaseConnector(_connectionString);
            int? value = connector.ReadDataFromSqlCommand<int?>("Select NULL from TestTable").FirstOrDefault();
            Assert.AreEqual(value, null);
        }
    }
}
