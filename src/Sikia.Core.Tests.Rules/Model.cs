using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sikia.Core.Tests.Rules.Customers
{

    [Display("Gender", Description = "Gender of Customer")]
    public enum CustomerGender
    {
        [Display("Masculin")]
        Male,
        [Display("Féminin")]
        Female
    };


    [Db("FirstName, LastName")]
    [Display("Customer", Description = "Class Customer")]
    public class Customer : InterceptedObject
    {
        public int RCount = 0;
        [Display("First Name", Description = "First Name of Customer")]
        public virtual string FirstName { get; set; }

        [Display("Last Name", Description = "Last Name of Customer")]
        public virtual string LastName { get; set; }

        [Display("Full Name", Description = "Full Name of Customer")]
        public virtual string FullName { get; set; }

        public virtual string AfterFirstNameChanged { get; set; }

        [Rule(Rule.Propagation, Property = "FirstName")]
        [Rule(Rule.Propagation, Property = "LastName")]
        [Display("Calculate Full Name", Description = "Calculate Full Name")]
        protected virtual void CalculatePersistentFullName()
        {
            RCount++;
            FullName = FirstName + " " + (String.IsNullOrEmpty(LastName) ? "" : LastName).ToUpper();
        }
    }

    [RulesFor(typeof(Customer))]
    public class PlugInCustomer : RulePluginObject
    {
        [Rule(Rule.Propagation, Property = "FirstName")]
        public static void Test(Customer target)
        {
            target.AfterFirstNameChanged = "AfterFirstNameChanged";
        }

    }

    [Display("Russian Customer", Description = "Russian Class Customer")]
    public class RussianCustomer : Customer
    {
        [Display("Middle Name", Description = "Middle Name of Customer")]
        public virtual string MiddleName { get; set; }
        [Rule(Rule.Propagation, Property = "MiddleName")]
        protected override void CalculatePersistentFullName()
        {
            FullName = (String.IsNullOrEmpty(LastName) ? "" : LastName).ToUpper() + " " + MiddleName + " " + FirstName;
        }
    }


}
