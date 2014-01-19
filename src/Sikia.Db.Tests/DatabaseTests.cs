using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sikia.Json;
using Sikia.Db;
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
            JsonObject databases = (JsonObject)JsonValue.Parse("{\"mssql://(local)\\\\SQLEXPRESS/master?schema=dbo\":{\"trustedConnection\":true}}");
            DbConnectionManger.Instance.Load(databases);
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
            JsonObject databases = (JsonObject)JsonValue.Parse("{\"mssql://(local)\\\\SQLEXPRESS/master?schema=dbo\":{\"trustedConnection\":true}}");
            DbConnectionManger.Instance.Load(databases);
            using (DbSession ss = DbDrivers.Instance.Session(dburl))
            {
                ss.Open();
                DbCmd cmd = ss.Command("use master");
                cmd.Execute();
            }
            
        }
    }
}
