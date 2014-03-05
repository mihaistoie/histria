using System;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Histria.Db.Generators;
using Histria.Db.Model;
using Histria.Sys;

namespace Histria.Db.Test.Generators
{
    [TestClass]
    public class GeneratorsTests
    {
        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            ModulePlugIn.Load("Histria.Db.MsSql");
            ModulePlugIn.Initialize(null);
        }

        [TestMethod]
        public void GeneratorsClasses()
        {
            string dburl = "mssql://(local)\\SQLEXPRESS/generat?schema=dbo";
            DbSchema ss = DbDrivers.Instance.Schema(DbServices.Url2Protocol(dburl));
            if (ss.DatabaseExists(dburl))
            {
                ss.DropDatabase(dburl);
            }
            ss.CreateDatabase(dburl);


            using (DbSession session = DbDrivers.Instance.Session(dburl))
            {
                using (DbCmd cmd = session.Command(""))
                {
                    cmd.Sql = @"CREATE TABLE dbo.Pays
	(
	code varchar(10) NOT NULL,
	caption varchar(50) NULL,
	Indicatif int NULL
	)  ON [PRIMARY]";
                    cmd.Execute();
                    cmd.Sql = @"CREATE TABLE dbo.Continent(
	code varchar(10) NOT NULL,
    Surface decimal(10,2) NOT NULL,
	caption varchar(50) NULL
    ) ON [PRIMARY]";
                    cmd.Execute();
                }
            }

            string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Uri u = new Uri(dir);
            DbSchema dbs = DbDrivers.Instance.Schema(DbProtocol.mssql);
            dbs.Load(dburl);
            Generator g = new Generator(dbs, dir, "Histria.Db");
            g.Generate();

            Assert.IsTrue(File.Exists(dir + @"\Pays.cs"));
            Assert.IsTrue(File.Exists(dir + @"\Continent.cs"));
            dbs.DropDatabase(dburl);
        }
    }
}
