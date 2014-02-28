using System;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sikia.Db.Generators;
using Sikia.Db.Model;
using Sikia.Sys;

namespace Sikia.Db.Test.Generators
{
    [TestClass]
    public class GeneratorsTests
    {
        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            ModulePlugIn.Load("Sikia.Db.MsSql");
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
            
            string d = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            Uri u = new Uri(d);
            // string dbURl = @"mssql://(local)\PC36290RC2/Sikia?schema=dbo";
            DbSchema dbs = DbDrivers.Instance.Schema(DbProtocol.mssql);
            dbs.Load(dburl);
            //Generator g = new Generator(dbs, @"D:\dev\Classes", "Sikia.Db");
            Generator g = new Generator(dbs, u.AbsolutePath, "Sikia.Db");
            g.Generate();

            Assert.IsTrue(File.Exists(u.AbsolutePath+@"\Pays.cs"));
            Assert.IsTrue(File.Exists(u.AbsolutePath+@"\Continent.cs"));
            dbs.DropDatabase(dburl);
        }
    }
}
