using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sikia.Framework;
using Sikia.Framework.Model;

namespace UnitTestModel.Models
{
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
