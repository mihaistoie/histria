using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Histria.Json;
using Histria.Model;
using Histria.Sys;
using Histria.Core.Execution;

namespace Histria.Core.PropertiesState.Tests
{
    [TestClass]
    public class PropertiesStateTests
    {
        private static ModelManager model;
        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            string scfg = @"{""nameSpaces"": [""PropertiesState""]}";
            JsonObject cfg = (JsonObject)JsonValue.Parse(scfg);
            model = ModelManager.LoadModelFromConfig(cfg);
            ModulePlugIn.Load("Histria.Proxy.Castle");
            ModulePlugIn.Initialize(model);
        }

        [TestMethod]
        public void StateRuleExecution()
        {
            Container container = new Container(TestContainerSetup.GetSimpleContainerSetup(model));
            HBody body = container.Create<HBody>();
            body.Name = "kirilov";
            Assert.AreEqual(true, body.Properties["Name"].IsMandatory, "Initialization");
            Assert.AreEqual(false, body.Properties["Name"].IsDisabled, "Initialization");
            Hand left = container.Create<Hand>();
            body.Hands.Add(left);
            Assert.AreEqual(true, body.Properties["Name"].IsDisabled, "State rule");
            left.Body.Value = null;
            Assert.AreEqual(false, body.Properties["Name"].IsDisabled, "State rule");

            Assert.AreEqual(3, body.RuleHits, "State rule was called 3 times");
            
        }
    }
}
