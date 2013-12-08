using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sikia.Framework;



namespace UnitTestModel.Models
{
    [RulesFor(typeof(Customer))]
    public class PlugInCustomer : RulePluginObject
    {
        [Rule("Propagation", Property = "FirstName", TargetType=typeof(Customer))]
        public static void Test(object target)
        {
            (target as Customer).AfterFirstNameChanged = "AfterFirstNameChanged";
        }
    }
}