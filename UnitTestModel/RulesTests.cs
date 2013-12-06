using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sikia.Framework;
using Sikia.Settings;
using System;
using UnitTestModel.Models;

namespace UnitTestModel
{
    [TestClass]
    public class RulesTests
    {
        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            ApplicationConfig config = new ApplicationConfig();
            Type[] Types = { typeof(Customer), typeof(RussianCustomer) };
            config.Types = Types;
            Sikia.Application.GlbApplicaton.Start(config);
        }

        [TestMethod]
        public void SimplePropagationRule()
        {
            Customer cust = ModelFactory.Create<Customer>();
            cust.FirstName = "John";
            cust.LastName = "Smith";
            Assert.AreEqual("John SMITH", cust.FullName, "Propagation rule not called");
        }
        [TestMethod]
        public void InheritedPropagationRule()
        {
            RussianCustomer rcust = ModelFactory.Create<RussianCustomer>();

            rcust.FirstName = "Fiodor";
            rcust.LastName = "Dostoievski";
            rcust.MiddleName = "A."; 
            Assert.AreEqual("DOSTOIEVSKI A. Fiodor", rcust.FullName, "Propagation rule not called");
        }
    }
}
