using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Histria.Json;
using Histria.Model;
using Histria.Sys;
using Histria.Core.Execution;
using Histria.Core.Changes;

namespace Histria.Core.Tests.Changes
{
    [TestClass]
    public class ObjectDeltaTests
    {
        private static ModelManager model;
        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            string scfg = @"{""nameSpaces"": [""Changes""]}";
            JsonObject cfg = (JsonObject)JsonValue.Parse(scfg);
            model = ModelManager.LoadModel(cfg);
            ModulePlugIn.Load("Histria.Proxy.Castle");
            ModulePlugIn.Initialize(model);
        }

        [TestMethod]
        public void ObjectDeltaCreationTest()
        {
            Container container = new Container(TestContainerSetup.GetSimpleContainerSetup(model));
            var simple = container.Create<Simple>();
            simple.Name = "aName";
            simple.Value = 3.14m;
            simple.Date = new DateTime(2014, 1, 1);

            ObjectDelta delta = ObjectDelta.InitFromObject(simple);

            Assert.AreEqual(ObjectLifetimeEvent.Created, delta.Lifetime);
            Assert.AreEqual(simple.Uuid, delta.Target);
            Assert.AreEqual(4, delta.Properties.Count);
            Assert.AreEqual(simple.Name, delta.Properties["Name"].Value);
            Assert.AreEqual(simple.Date, delta.Properties["Date"].Value);
            Assert.AreEqual(simple.Value, delta.Properties["Value"].Value);
            Assert.AreEqual(simple.Uuid, delta.Properties["Uuid"].Value);
        }
    }
}
