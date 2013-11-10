using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Sikia.Framework.Attributes;
using Sikia.Framework.Utils;

namespace Sikia.Framework.DataModel
{
    public class Propinfo
    {
        private PropertyInfo pi;
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Propinfo(PropertyInfo cPi)
        {
            pi = cPi;
            Name = pi.Name;
            Title = Name;
            DisplayAttribute da = pi.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;
            Title = Name;
            if (da != null)
            {
                Title = da.Title;
                Description = da.Description;
            }
            if (Description == "") Description = Title;

        }

    }
}

