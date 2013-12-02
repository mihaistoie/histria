using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sikia.Framework;
using Sikia.Settings;
using TestRules.Models;

namespace TestRules
{
    [TestClass]
    public  class RulesTests
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
    }
}