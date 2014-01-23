using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sikia.Json;
using Sikia.Db;
using Sikia.Db.Model;
using Sikia.Db.SqlServer;


namespace Sikia.Db.Tests
{
    [TestClass]
    public class DatabaseTests
    {
        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
     

        }
     
        [TestMethod]
        public void MsSqlUrlParser()
        {
            string dburl = "mssql://(local)\\SQLEXPRESS/master?schema=dbo";
            DbConnectionInfo ci = DbDrivers.Instance.Connection(dburl);
            Assert.AreEqual(ci.ServerAddress, "(local)\\SQLEXPRESS", "Parse Server part");
            Assert.AreEqual(ci.DatabaseName, "master", "Parse Database part");
            MsSqlConnectionInfo ms = (MsSqlConnectionInfo)ci;
            Assert.AreEqual(ms.Schema, "dbo", "Parse schema part");
        }

        [TestMethod]
        public void MsConnection()
        {
            string dburl = "mssql://(local)\\SQLEXPRESS/master?schema=dbo";
            using (DbSession ss = DbDrivers.Instance.Session(dburl))
            {
                DbCmd cmd = ss.Command("select 1");
                int res = (int)cmd.ExecuteScalar();
                Assert.AreEqual(res, 1, "ExecuteScalar");
            }
            
        }
        [TestMethod]
        public void CreateAndDropDatabase()
        {
            string dburl = "mssql://(local)\\SQLEXPRESS/xyzgrls?schema=dbo";
            DbStructure ss = DbDrivers.Instance.Structure(DbServices.Url2Protocol(dburl));
            if (ss.DatabaseExists(dburl))
            {
                ss.DropDatabase(dburl);
            }
            ss.CreateDatabase(dburl);
            Assert.AreEqual( ss.DatabaseExists(dburl), true, "DatabaseExists");
            using (DbSession session = DbDrivers.Instance.Session(dburl))
            {
                using (DbCmd cmd = session.Command(""))
                {
                    cmd.Sql = "create table a1 (a integer not null)";
                    cmd.Execute();
                    cmd.Sql = "create table B1 (a integer not null)";
                    cmd.Execute();
                }
            }
            ss.Load(dburl);
            Assert.AreEqual(ss.Tables.ContainsKey("a1"), true, "DatabaseExists");
            Assert.AreEqual(ss.Tables.ContainsKey("B1"), true, "DatabaseExists");
           
            ss.DropDatabase(dburl);
            Assert.AreEqual(ss.DatabaseExists(dburl), false, "DatabaseExists");

        }
    }
}
