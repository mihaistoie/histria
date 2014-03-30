using Microsoft.VisualStudio.TestTools.UnitTesting;
using Histria.Core;
using Histria.Model;
using Histria.Core.Tests.Rules.Customers;
using Histria.Json;
using System;
using Histria.Sys;
using Histria.Core.Execution;

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
            model = ModelManager.LoadModel(cfg);
            ModulePlugIn.Load("Histria.Proxy.Castle");
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
       
        [TestMethod]
        public void CorrectionRule()
        {
            Container container = new Container(TestContainerSetup.GetSimpleContainerSetup(model));
            HumanBody b = container.Create<HumanBody>();
            b.Name = "name";
            Assert.AreEqual("NAME", b.Name, "Correction rule");
        }

        [TestMethod]
        public void PropagationRuleOnInt()
        {
            Container container = new Container(TestContainerSetup.GetSimpleContainerSetup(model));
            Customer cust = container.Create<Customer>();
            cust.Age = 36;
            cust.Age = 37;
            cust.Age = 37;
            Assert.AreEqual(2, cust.ARCount, "Rule hits");
        }
        [TestMethod]
        public void BasicRuleExecutionControl()
        {
            Container container = new Container(TestContainerSetup.GetSimpleContainerSetup(model));
            SalesOrder order = container.Create<SalesOrder>();
            order.NetAmount = 100;
            Assert.AreEqual(100M, order.NetAmount, "NET");
            Assert.AreEqual(18.33M, order.VAT, "VAT");
            Assert.AreEqual(118.33M, order.GrossAmount, "Gross");

            order.NetAmount = 52M;
            Assert.AreEqual(52M, order.NetAmount, "NET");
            Assert.AreEqual(9.53M, order.VAT, "VAT");
            Assert.AreEqual(61.53M, order.GrossAmount, "Gross");
            order.NetAmount = 0M;

            order.GrossAmount = 61.53M;
            Assert.AreEqual(52M, order.NetAmount, "NET");
            Assert.AreEqual(9.53M, order.VAT, "VAT");
            Assert.AreEqual(61.53M, order.GrossAmount, "Gross");

        }
        [TestMethod]
        public void ComplexRuleExecutionControl()
        {
            Container container = new Container(TestContainerSetup.GetSimpleContainerSetup(model));
            Budget budget = container.Create<Budget>();
            BudgetDetail d1 = container.Create<BudgetDetail>();
            BudgetDetail d2 = container.Create<BudgetDetail>();
            BudgetDetail d3 = container.Create<BudgetDetail>();
            budget.Details.Add(d1);
            budget.Details.Add(d2);
            budget.Details.Add(d3);
            d1.Proc = 25M;
            d2.Proc = 35M;
            d3.Proc = 40M;
            budget.Total = 100M;
            Assert.AreEqual(40M, d3.Value, "R");
            d3.Value = 80M;
            Assert.AreEqual(200M, budget.Total, "R");


        }

    }
}
