using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sikia.Framework;

namespace UnitTestModel.Models
{
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

        [Rule("Propagation", Property = "FirstName")]
        [Rule("Propagation", Property = "LastName")]
        [Display("Calculate Full Name", Description = "Calculate Full Name")]
        protected virtual void CalculatePersistentFullName()
        {
            RCount++;
            FullName = FirstName + " " + (String.IsNullOrEmpty(LastName) ? "" : LastName).ToUpper();
        }
    }
}




