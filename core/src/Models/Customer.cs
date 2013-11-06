using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Sikia.Framework;
using Sikia.Framework.Attributes;


namespace Sikia.Models
{

    [Display("Gender", Description = "Gender of Customer")]
    [EnumCaptions("Masculin","Féminin")]
    public enum Gender 
    {
        [Display("Masculin")]
        Male, 
        Female 
    };

    [Display("Customer", Description = "Class Customer")]
    public class Customer : BaseObject
    {
        [Display("First Name", Description = "First Name of Customer")]
        public virtual string FirstName { get; set; }
        [Display("Last Name", Description = "Last Name of Customer")]
        public virtual string LastName { get; set; }
        [Display("Full Name", Description = "Full Name of Customer")]
        public virtual string FullName { get { return FirstName + " " + LastName.ToUpper(); } }
    }
    [Display("Address", Description = "Class Address")]
    public class Address : BaseObject
    {
        [Display("Street", Description = "Street Address")]
        public virtual string Street { get; set; }
    }

}
