using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sikia.Framework;
using Sikia.Framework.Model;
using Sikia.Settings;
using System;
using UnitTestModel.Models;


namespace UnitTestModel
{
    [TestClass]
    public class ModelTests
    {
        [TestMethod]
        [ExpectedException(typeof(ModelException))]
        public void InvalidRuleDefinition()
        {
            ApplicationConfig config = new ApplicationConfig();
            Type[] Types = { typeof(MInvalidRule) };
            config.Types = Types;
            ModelManager m = ModelManager.LoadModel(config);

        }

        [TestMethod]
        [ExpectedException(typeof(ModelException))]
        public void DuplicatedRule()
        {
            ApplicationConfig config = new ApplicationConfig();
            Type[] Types = { typeof(MR1), typeof(MR2) };
            config.Types = Types;
            ModelManager m = ModelManager.LoadModel(config);

        }
        
        [TestMethod]
        public void StaticClassTitle()
        {
            ApplicationConfig config = new ApplicationConfig();
            Type[] Types = { typeof(MR3) };
            config.Types = Types;
            ModelManager m = ModelManager.LoadModel(config);
            ClassInfoItem ci = m.ClassByType(typeof(MR3));
            Assert.AreEqual(ci.Title, "M3-T", "Class static title");
            Assert.AreEqual(ci.Description, "M3-D", "Class static description");
        }
       
        [TestMethod]
        [ExpectedException(typeof(ModelException))]
        public void DynamicClassTitle_NonStaticMethod()
        {
            ApplicationConfig config = new ApplicationConfig();
            Type[] Types = { typeof(MR4) };
            config.Types = Types;
            ModelManager m = ModelManager.LoadModel(config);
        }

        [TestMethod]
        [ExpectedException(typeof(ModelException))]
        public void DynamicClassTitle_NotFoundMethod()
        {
            ApplicationConfig config = new ApplicationConfig();
            Type[] Types = { typeof(MR5) };
            config.Types = Types;
            ModelManager m = ModelManager.LoadModel(config);
        }
       
        [TestMethod]
        public void DynamicClassTitle()
        {
            ApplicationConfig config = new ApplicationConfig();
            Type[] Types = { typeof(MR6) };
            config.Types = Types;
            ModelManager m = ModelManager.LoadModel(config);
            ClassInfoItem ci = m.ClassByType(typeof(MR6));
            Assert.AreEqual(ci.Title, "M6", "Dynamic static title");
            Assert.AreEqual(ci.Description, "M6", "Dynamic static description");
        }

        
    }
}
