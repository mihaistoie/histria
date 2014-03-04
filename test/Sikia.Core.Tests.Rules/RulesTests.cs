using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sikia.Core;
using Sikia.Model;
using Sikia.Core.Tests.Rules.Customers;
using Sikia.Json;
using System;
using Sikia.Sys;
using Sikia.Core.Execution;

namespace UnitTestModel
{
    [TestClass]
    public class Rules
    {
        private static ModelManager model;

        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            JsonObject cfg = (JsonObject)JsonValue.Parse(@"{""nameSpaces"": [""Customers""]}");
            model = ModelManager.LoadModelFromConfig(cfg);
            ModulePlugIn.Load("Sikia.Proxy.Castle");
            ModulePlugIn.Initialize(model);
        }

        [TestMethod]
        public void SimplePropagationRule()
        {
            Container container = new Container(TestContainerSetup.GetSimpleContainerSetup(model));

            Customer cust = container.Create<Customer>();
            cust.FirstName = "John";
            cust.LastName = "Smith";
            cust.LastName = "Smith";
            Assert.AreEqual(2, cust.RCount, "Rule hits");
            Assert.AreEqual("John SMITH", cust.FullName, "Propagation rule not called");
            Assert.AreEqual("AfterFirstNameChanged", cust.AfterFirstNameChanged, "Plugin  rule");
        }
        [TestMethod]
        public void InheritedPropagationRule()
        {
            Container container = new Container(TestContainerSetup.GetSimpleContainerSetup(model));

            RussianCustomer rcust = container.Create<RussianCustomer>();

            rcust.FirstName = "Fiodor";
            rcust.LastName = "Dostoievski";
            rcust.MiddleName = "A.";
            Assert.AreEqual(0, rcust.RCount, "Rule hits");
            Assert.AreEqual("DOSTOIEVSKI A. Fiodor", rcust.FullName, "Propagation rule not called");
        }
    }
}
