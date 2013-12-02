using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sikia.Framework;

namespace TestRules.Models
{
    [Db("FirstName, LastName")]
    [Display("Customer", Description = "Class Customer")]
    public class Customer : InterceptedObject
    {
        [Display("First Name", Description = "First Name of Customer")]
        public virtual string FirstName { get; set; }

        [Display("Last Name", Description = "Last Name of Customer")]
        public virtual string LastName { get; set; }

        [Display("Full Name", Description = "Full Name of Customer")]
        public virtual string FullName { get; set; }

        [Rule("Propagation", Property = "FirstName")]
        [Rule("Propagation", Property = "LastName")]
        [Display("Calculate Full Name", Description = "Calculate Full Name")]
        protected virtual void CalculatePersistentFullName()
        {
            FullName = FirstName + " " + (String.IsNullOrEmpty(LastName) ? "" : LastName).ToUpper();
        }
    }
}




