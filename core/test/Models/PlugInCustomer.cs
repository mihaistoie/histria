using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sikia.Framework;
using Sikia.Framework.Model;



namespace UnitTestModel.Models
{
    [RulesFor(typeof(Customer))]
    public class PlugInCustomer : RulePluginObject
    {
        [Rule(Rule.Propagation, Property = "FirstName")]
        public static void Test(object target)
        {
            (target as Customer).AfterFirstNameChanged = "AfterFirstNameChanged";
        }
        
    }
}