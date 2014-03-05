using Microsoft.VisualStudio.TestTools.UnitTesting;
using Histria.Model;
using Histria.Json;
using System;

using Histria.Model.Tests.ModelToTest;
using Histria.Model.Tests.XXX;
using System.Reflection;


namespace Histria.Model.Tests
{
    [TestClass]
    public class ModelTests
    {

        [TestMethod]
        public void LoadModelByNamespace()
        {
            JsonObject cfg = (JsonObject)JsonValue.Parse("{\"nameSpaces\": [\"XXX\"]}");
            ModelManager m = ModelManager.LoadModel(cfg);
            ClassInfoItem ci = m.Class<ClassInXXX>();
            Assert.AreNotEqual(ci, null, "Class found");
        }

        [TestMethod]
        [ExpectedException(typeof(ModelException))]
        public void InvalidRuleDefinition()
        {
            JsonObject cfg = (JsonObject)JsonValue.Parse("{\"types\": [\"" + typeof(MInvalidRule).FullName + "\"]}");
            ModelManager m = ModelManager.LoadModel(cfg);

        }

        [TestMethod]
        [ExpectedException(typeof(ModelException))]
        public void DuplicatedRule()
        {
            JsonObject cfg = (JsonObject)JsonValue.Parse("{\"types\": [\"" + typeof(MR1).FullName + "\", \"" + typeof(MR2).FullName + "\"]}");
            ModelManager m = ModelManager.LoadModel(cfg);

        }

        [TestMethod]
        public void StaticClassTitle()
        {
            JsonObject cfg = (JsonObject)JsonValue.Parse("{\"types\": [\"" + typeof(MR3).FullName + "\"]}");
            ModelManager m = ModelManager.LoadModel(cfg);
            ClassInfoItem ci = m.Class<MR3>();
            Assert.AreEqual(ci.Title, "M3-T", "Class static title");
            Assert.AreEqual(ci.Description, "M3-D", "Class static description");
        }

        [TestMethod]
        public void DynamicClassTitle()
        {
            JsonObject cfg = (JsonObject)JsonValue.Parse("{\"types\": [\"" + typeof(MR4).FullName + "\"]}");
            ModelManager m = ModelManager.LoadModel(cfg);
            ClassInfoItem ci = m.Class<MR4>();
            Assert.AreEqual(ci.Title, "MR4.xxx", "Class dynamic title");
            Assert.AreEqual(ci.Description, "MR4.yyy", "Class dynamic title");
        }

        [TestMethod]
        public void Views()
        {
            JsonObject cfg = (JsonObject)JsonValue.Parse("{\"nameSpaces\": [\"XXX\"]}");
            ModelManager m = ModelManager.LoadModel(cfg);
            ClassInfoItem ci = m.Class<ClassInXXX>();
            Assert.AreNotEqual(null, ci, "Class found");
            ci = m.Class<Account>();
            Assert.AreNotEqual(null, ci, "Class found");
            
            ClassInfoItem vi = m.Class<AccountView>();
            Assert.AreNotEqual(null, vi, "Class found");
            if (ci != null && vi != null)
            {
                PropInfoItem cpi = ci.PropertyByName("Code");
                PropInfoItem vpi = vi.PropertyByName("Code");
                Assert.AreNotEqual(cpi, null, "Property found");
                Assert.AreNotEqual(vpi, null, "Property  found");
                if (cpi != null && vpi != null)
                {
                    Assert.AreEqual(cpi.Title, vpi.Title, "Property title");
                }
            }

        }

    }
}
