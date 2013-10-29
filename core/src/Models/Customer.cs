using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Sikia.Framework;
namespace Sikia.Models
{
    /// <summary>
    /// Simple Model 
    /// its auto properties made into 
    /// </summary>
    public class Customer: BaseObject
    {
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string FullName { get { return FirstName + " " + LastName.ToUpper(); } }
    }
    
}
