using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sikia.Json;
using Sikia.Model;
using Sikia.Sys;

namespace Sikia.Core.Tests.Associations
{
    [TestClass]
    public class TestRelations
    {
        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            JsonObject cfg = (JsonObject)JsonValue.Parse("{\"nameSpaces\": [\"Associations\"]}");
            ModelManager m = ModelManager.LoadModelFromConfig(cfg);
            ModulePlugIn.Load("Sikia.Proxy.Castle");
        }

        [TestMethod]
        public void AssociationsLoad()
        {
            HumanBody cust = ProxyFactory.Create<HumanBody>();
        }
    }
}
