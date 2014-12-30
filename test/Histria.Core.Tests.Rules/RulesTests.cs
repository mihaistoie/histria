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
            TemplateManager.Register("Amount", new DtNumberAttribute() { Decimals = 2 });
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
            ClassInfoItem cc = container.ModelManager.Class<Customer>();
            MethodItem mi = cc.MethodByName("MethodInBaseClass1");
            Assert.AreNotEqual(null, mi, "Method found");
            mi = cc.MethodByName("MethodInBaseClass2");
            Assert.AreNotEqual(null, mi, "Method found");
            cust.MethodInBaseClass2();
            Assert.AreEqual(100, cust.MethodTest, "method call");
        }
        [TestMethod]
        public void InheritedRulePropagation()
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
        public void InheritedMethodsPropagation()
        {
            Container container = new Container(TestContainerSetup.GetSimpleContainerSetup(model));

            ClassInfoItem cc = container.ModelManager.Class<RussianCustomer>();
            MethodItem mi = cc.MethodByName("MethodInBaseClass1");
            Assert.AreNotEqual(null, mi, "Method found");
            mi = cc.MethodByName("MethodInBaseClass2");
            Assert.AreNotEqual(null, mi, "Method found");
            mi = cc.MethodByName("MethodInDerivedClass");
            Assert.AreNotEqual(null, mi, "Method found");

            
            RussianCustomer rcust = container.Create<RussianCustomer>();
            rcust.MethodInBaseClass2();
            Assert.AreEqual(200, rcust.MethodTest, "method call");
            
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
            Assert.AreEqual(1, d1.FromTotal, "Rule hits");
            d3.Value = 80M;
            Assert.AreEqual(200M, budget.Total, "R");
            Assert.AreEqual(1, budget.FromDetail, "Rule hits");
            Assert.AreEqual(1, d3.FromTotal, "Rule hits");
            Assert.AreEqual(2, d2.FromTotal, "Rule hits");

            BudgetDetailModel modelBudget = container.Create<BudgetDetailModel>();
            modelBudget.Value = 20M;
            d3.Model.Value = modelBudget;
            Assert.AreEqual(20M, d3.Value, "R");
            Assert.AreEqual(50M, budget.Total, "R");
            Assert.AreEqual(1, d3.FromModel, "Rule hits");
            Assert.AreEqual(0, d2.FromModel, "Rule hits");

        }

        [TestMethod]
        public void InheritedPersistance()
        {
            Container container = new Container(TestContainerSetup.GetSimpleContainerSetup(model));
            Rectangle rectangle = container.Create<Rectangle>();
            Assert.AreEqual(ShapeType.Rectangle, rectangle.Type, "Type initialization");
            rectangle.Type = ShapeType.Shape;
            Assert.AreEqual(ShapeType.Rectangle, rectangle.Type, "Type is readonly");
            Assert.AreEqual(true, rectangle.Properties["Type"].IsHidden, "Type is hidden");
        }
    }
}
