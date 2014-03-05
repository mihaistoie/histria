using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Histria.Db;
using Histria.Db.Generators;
using Histria.Db.Model;
using Histria.Sys;

namespace Histria.Db.Generators.Tests
{
    [TestClass]
    public class GeneratorsTest
    {
        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            ModulePlugIn.Load("Histria.Db.MsSql");
            ModulePlugIn.Initialize(null);
        }


        [TestMethod]
        public void FFFFF()
        {
            Assert.AreEqual(true, true, "True");
            string dbURl = @"mssql://(local)\PC36290RC2/Histria?schema=dbo";
            DbSchema dbs = DbDrivers.Instance.Schema(DbProtocol.mssql);
            dbs.Load(dbURl);
            Generator g = new Generator(dbs, @"D:\dev\Classes", "Histria.Db");
            g.Generate();
        }

        [TestMethod]
        public void ClassGenerated()
        {
            Assert.AreEqual(true, true, "True");
            //Generator g = new Generator(dbs, @"D:\dev\Classes", "Histria.Db");
            //g.Generate();

        }
    }
}
