using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sikia.Framework.DataModel
{
    public class KeyItem
    {
        public string Key { get; set;}
        public PropertyInfo Property { get; set; }


        public KeyItem(string key, PropertyInfo pi)
        {
            Key = key;
            Property = pi;
        }
    }
}
