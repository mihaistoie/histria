using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Sikia.Framework.Attributes;
using Sikia.Framework.Utils;

namespace Sikia.Framework.DataModel
{
    public class PropinfoItem
    {
        public PropertyInfo PropInfo;
        public string Name { get; set; }
        public string DbName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public PropinfoItem(PropertyInfo cPi)
        {
            PropInfo = cPi;
            Name = PropInfo.Name;
            DbName = PropInfo.Name;
            Title = Name;
            DisplayAttribute da = PropInfo.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;
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

