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
            ModelManager mm = ModelProxy.Model();
            ClassInfoItem ci = mm.Class<HumanBody>();
            PropinfoItem pp = ci.PropertyByName("Nose");
            bool hasProp = pp != null;
            Assert.AreEqual(hasProp, true, "Property found");
            if (hasProp)
            {
                Assert.AreEqual(pp.IsRole, true, "Is role");
                if (pp.IsRole)
                {
                    Assert.AreEqual(pp.Role.IsList, true, "Is Role list");
                }
            }
            ci = mm.Class<Nose>();
            pp = ci.PropertyByName("Body");
            hasProp = pp != null;
            Assert.AreEqual(hasProp, true, "Property found");
            if (hasProp)
            {
                Assert.AreEqual(pp.IsRole, true, "Is role");
                if (pp.IsRole)
                {
                    Assert.AreEqual(pp.Role.IsRef, true, "Is Role Ref");
                    Assert.AreEqual(pp.Role.IsChild, true, "Is Child");
                }
            }

        }
    }
}
