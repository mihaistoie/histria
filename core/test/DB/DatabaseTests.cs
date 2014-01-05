using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sikia.Settings;
using Sikia.Json;
using Sikia.Application;
using Sikia.DataBase;
using Sikia.DataBase.SqlServer;


namespace UnitTestModel.DB
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
            ApplicationConfig config = new ApplicationConfig();
            string dburl = "mssql://(local)\\SQLEXPRESS/master?schema=dbo";
            config.Databases = JsonValue.Parse("{\"mssql://(local)\\\\SQLEXPRESS/master?schema=dbo\":{\"trustedConnection\":true, \"user\":\"sa\"}}");
            DbConnectionManger conn = DbConnectionManger.LoadConfig(config);
            DbConnectionInfo ci = conn.ConnectionInfo(dburl);
            Assert.AreEqual(ci.ServerAddress, "(local)\\SQLEXPRESS", "Parse Server part");
            Assert.AreEqual(ci.DatabaseName, "master", "Parse Database part");
            Assert.AreEqual(ci.UserName, "sa", "Parse user name");
            MsSqlConnectionInfo ms = (MsSqlConnectionInfo)ci;
            Assert.AreEqual(ms.Schema, "dbo", "Parse schema part");
        }

        [TestMethod]
        public void MsConnection()
        {
            ApplicationConfig config = new ApplicationConfig();
            string dburl = "mssql://(local)\\SQLEXPRESS/master?schema=dbo";
            config.Databases = JsonValue.Parse("{\"mssql://(local)\\\\SQLEXPRESS/master?schema=dbo\":{\"trustedConnection\":true}}");
            DbConnectionManger conn = DbConnectionManger.LoadConfig(config);
            using (DbSession ss = DbServices.Connection(dburl, conn))
            {
                ss.Open();

            }
            
        }
    }
}
