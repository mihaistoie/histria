using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Histria.Model;

namespace Histria.Core.Tests.Rules.Customers
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
        public int ARCount = 0;
        [Display("First Name", Description = "First Name of Customer")]
        public virtual string FirstName { get; set; }

        [Display("Last Name", Description = "Last Name of Customer")]
        public virtual string LastName { get; set; }

        [Display("Full Name", Description = "Full Name of Customer")]
        public virtual string FullName { get; set; }


        [Display("Age", Description = "Age")]
        public virtual int Age { get; set; }

         
        public virtual string AfterFirstNameChanged { get; set; }



        [RulePropagation("FirstName")]
        [RulePropagation("LastName")]
        [Display("Calculate Full Name", Description = "Calculate Full Name")]
        protected virtual void CalculatePersistentFullName()
        {
            RCount++;
            FullName = FirstName + " " + (String.IsNullOrEmpty(LastName) ? "" : LastName).ToUpper();
        }

        [RulePropagation("Age")]
        protected virtual void CountAgeChanges()
        {
            ARCount++;
        }
  
    }

    [RulesFor(typeof(Customer))]
    public class PlugInCustomer : IPluginModel
    {
        [RulePropagation("FirstName")]
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
        [RulePropagation("MiddleName")]
        protected override void CalculatePersistentFullName()
        {
            FullName = (String.IsNullOrEmpty(LastName) ? "" : LastName).ToUpper() + " " + MiddleName + " " + FirstName;
        }
    }


}
