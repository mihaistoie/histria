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
            ModulePlugIn.Initialize();
        }

        [TestMethod]
        public void AssociationsLoad()
        {
            HumanBody body = ProxyFactory.Create<HumanBody>();
            Nose nose = ProxyFactory.Create<Nose>();
            ModelManager mm = ModelProxy.Model();
            ClassInfoItem ci = mm.Class<HumanBody>();
            IInterceptedObject ii = nose as InterceptedObject;
            Assert.AreEqual(ii, nose, "Is role");


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
            body.Nose.Value = nose;
            Assert.AreEqual(body.Nose.Value, nose, "test role");
            Assert.AreEqual(nose.Body.Value, body, "test role");
            

        }
    }
}
