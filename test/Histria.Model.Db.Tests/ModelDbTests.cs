using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Histria.Json;
using Histria.Sys;

namespace Histria.Model.Db.Tests
{
    [TestClass]
    public class ModelDbTests
    {

        private static ModelManager model;
        static string sqlServerInstance = "(local)\\SQLEXPRESS";
  
        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            JsonObject cfg = (JsonObject)JsonValue.Parse(@"{""nameSpaces"": [""Customers""]}");
            model = ModelManager.LoadModel(cfg);
            ModulePlugIn.Load("Histria.Db.MsSql");
            ModulePlugIn.Initialize(null);
        }

        [TestMethod]
        public void ColumnsStrings()
        {
        }
    }
}



