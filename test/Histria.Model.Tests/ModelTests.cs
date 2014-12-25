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
        public void InvalidStateRuleDefinition()
        {
            JsonObject cfg = (JsonObject)JsonValue.Parse("{\"types\": [\"" + typeof(MStateRuleOnValidation).FullName + "\"]}");
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
        [ExpectedException(typeof(ModelException))]
        public void DuplicatedStateRule()
        {
            JsonObject cfg = (JsonObject)JsonValue.Parse("{\"types\": [\"" + typeof(MSR1).FullName + "\", \"" + typeof(MSR2).FullName + "\"]}");
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

        [TestMethod]
        public void LoadCorrectionsRules()
        {
            JsonObject cfg = (JsonObject)JsonValue.Parse("{\"types\": [\"" + typeof(HumanBody).FullName + "\", \"" + typeof(HumanBodyRules).FullName + "\"]}");
            ModelManager m = ModelManager.LoadModel(cfg);
            ClassInfoItem ci = m.Class<HumanBody>();
            PropInfoItem pi = ci.PropertyByName("Name");
            Assert.AreNotEqual(null, pi, "Property  found");
        }

        [TestMethod]
        public void DerivedClasses()
        {
            JsonObject cfg = (JsonObject)JsonValue.Parse("{\"types\": [\"" + typeof(CParent).FullName
                + "\", \"" + typeof(CC1).FullName
                + "\", \"" + typeof(CC2).FullName
                + "\", \"" + typeof(CC3).FullName
                + "\"]}");
            ModelManager m = ModelManager.LoadModel(cfg);
            ClassInfoItem cip = m.Class<CParent>();
            ClassInfoItem cic1 = m.Class<CC1>();
            ClassInfoItem cic2 = m.Class<CC2>();
            ClassInfoItem cic3 = m.Class<CC3>();
            Assert.AreNotEqual(null, cip, "Class found");
            Assert.AreNotEqual(null, cic1, "Class found");
            Assert.AreNotEqual(null, cic2, "Class found");
            Assert.AreNotEqual(null, cic3, "Class found");

            PropInfoItem pi = cip.PropertyByName("ParentMember");
            Assert.AreNotEqual(null, pi, "Property found");

            pi = cic1.PropertyByName("ParentMember");
            Assert.AreNotEqual(null, pi, "Property found");

            pi = cic2.PropertyByName("ParentMember");
            Assert.AreNotEqual(null, pi, "Property found");

            pi = cic3.PropertyByName("ParentMember");
            Assert.AreNotEqual(null, pi, "Property found");

            pi = cic2.PropertyByName("CC3Member");
            Assert.AreEqual(null, pi, "Property found");


        }
        [TestMethod]
        public void DerivedClassesPersistence()
        {
            JsonObject cfg = (JsonObject)JsonValue.Parse("{\"types\": [\"" + typeof(CP1).FullName
                + "\", \"" + typeof(CC4).FullName
                + "\"]}");
            ModelManager m = ModelManager.LoadModel(cfg);
            ClassInfoItem pp = m.Class<CP1>();
            ClassInfoItem cc = m.Class<CC4>();

            Assert.AreNotEqual(null, pp, "Class found");
            Assert.AreNotEqual(null, cc, "Class found");
            Assert.AreEqual(pp.DbName, cc.DbName, "Same table name");
            Assert.AreEqual("XXX", cc.DbName, "Table name");

            PropInfoItem pi = pp.PropertyByName("ParentMember");
            Assert.AreNotEqual(null, pi, "Property found");
            Assert.AreEqual("M", pi.PersistentName, "Column name");

            pi = cc.PropertyByName("ParentMember");
            Assert.AreNotEqual(null, pi, "Property found");
            Assert.AreEqual("M", pi.PersistentName, "Column name");
        }

        [TestMethod]
        [ExpectedException(typeof(ModelException))]
        public void DerivedClassesInvalidDbAttribute()
        {
            JsonObject cfg = (JsonObject)JsonValue.Parse("{\"types\": [\"" + typeof(CP2).FullName + "\", \"" + typeof(CC5).FullName + "\"]}");
            ModelManager m = ModelManager.LoadModel(cfg);

        }

        [TestMethod]
        [ExpectedException(typeof(ModelException))]
        public void DerivedClassesInvalidPkDefinition()
        {
            JsonObject cfg = (JsonObject)JsonValue.Parse("{\"types\": [\"" + typeof(CP3).FullName + "\", \"" + typeof(CC6).FullName + "\"]}");
            ModelManager m = ModelManager.LoadModel(cfg);

        }
        [TestMethod]
        public void DerivedClassesSamePk()
        {
            JsonObject cfg = (JsonObject)JsonValue.Parse("{\"types\": [\"" + typeof(CP4).FullName + "\", \"" + typeof(CC7).FullName + "\"]}");
            ModelManager m = ModelManager.LoadModel(cfg);

            ClassInfoItem pp = m.Class<CP4>();
            ClassInfoItem cc = m.Class<CC7>();

            Assert.AreNotEqual(null, pp, "Class found");
            Assert.AreNotEqual(null, cc, "Class found");
            Assert.AreEqual("Id", pp.Key.Items[0].Property.Name, "Pk name");
            Assert.AreEqual("Id", cc.Key.Items[0].Property.Name, "Pk name");

        }
        [TestMethod]
        public void DerivedClassesIndexes()
        {
            JsonObject cfg = (JsonObject)JsonValue.Parse("{\"types\": [\"" + typeof(CP1).FullName
                + "\", \"" + typeof(CC4).FullName
                + "\"]}");
            ModelManager m = ModelManager.LoadModel(cfg);
            ClassInfoItem pp = m.Class<CP1>();
            ClassInfoItem cc = m.Class<CC4>();

            Assert.AreNotEqual(null, pp, "Class found");
            Assert.AreNotEqual(null, cc, "Class found");
            Assert.AreEqual(1, pp.Indexes.Count, "Parent has one index");
            Assert.AreEqual(2, cc.Indexes.Count, "Child has two indexes");

        }

        [TestMethod]
        public void EnumLoad()
        {
            JsonObject cfg = (JsonObject)JsonValue.Parse("{\"types\": [\"" + typeof(TypeYesNo).FullName + "\"]}");
            ModelManager m = ModelManager.LoadModel(cfg);
            EnumInfoItem ei;
            m.Enums.TryGetEnumInfo(typeof(TypeYesNo), out ei);
            Assert.AreNotEqual(null, ei, "Enum found");
        }
        
        [TestMethod]
        [ExpectedException(typeof(ModelException))]
        public void EnumMissing()
        {
            JsonObject cfg = (JsonObject)JsonValue.Parse("{\"types\": [\"" + typeof(EnuUsed).FullName + "\"]}");
            ModelManager m = ModelManager.LoadModel(cfg);
            
        }

        [TestMethod]
        public void EnumLoad2()
        {
            JsonObject cfg = (JsonObject)JsonValue.Parse("{\"types\": [\"" + typeof(EnuUsed).FullName
                + "\", \"" + typeof(TypeYesNo).FullName
                + "\"]}");
            ModelManager m = ModelManager.LoadModel(cfg);
            EnumInfoItem ei;
            m.Enums.TryGetEnumInfo(typeof(TypeYesNo), out ei);
            Assert.AreNotEqual(null, ei, "Enum found");
            ClassInfoItem cc = m.Class<EnuUsed>();
            Assert.AreNotEqual(null, cc, "Class found");
            PropInfoItem pi = cc.PropertyByName("IsRed");
            Assert.AreNotEqual(null, pi, "Property found");
        }

        
    }
}
