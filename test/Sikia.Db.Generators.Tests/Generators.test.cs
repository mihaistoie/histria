using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sikia.Db;
using Sikia.Db.Generators;
using Sikia.Db.Model;
using Sikia.Sys;

namespace Sikia.Db.Generators.Tests
{
    [TestClass]
    public class GeneratorsTest
    {
        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            ModulePlugIn.Load("Sikia.Db.MsSql");
            ModulePlugIn.Initialize(null);
        }


        [TestMethod]
        public void FFFFF()
        {
            Assert.AreEqual(true, true, "True");
            string dbURl = @"mssql://(local)\PC36290RC2/Sikia?schema=dbo";
            DbSchema dbs = DbDrivers.Instance.Schema(DbProtocol.mssql);
            dbs.Load(dbURl);
            Generator g = new Generator(dbs, @"D:\dev\Classes", "Sikia.Db");
            g.Generate();
        }

        [TestMethod]
        public void ClassGenerated()
        {
            Assert.AreEqual(true, true, "True");
            //Generator g = new Generator(dbs, @"D:\dev\Classes", "Sikia.Db");
            //g.Generate();

        }
    }
}
