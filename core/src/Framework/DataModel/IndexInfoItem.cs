using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sikia.Framework.DataModel
{
    public class IndexInfoItem
    {
        public string FiledName { get; set; }
        public PropertyInfo Property { get; set; }
        public bool Descendent { get; set; }
        public IndexInfoItem(PropertyInfo pi, string fieldName, bool descendent = false)
        {
            FiledName = fieldName;
            Property = pi;
            Descendent = descendent;
           
        }

    }
}
