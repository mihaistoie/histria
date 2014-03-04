using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sikia.Model
{
    public class PropertiesCollection : KeyedCollection<PropertyInfo, PropInfoItem>
    {
        protected override PropertyInfo GetKeyForItem(PropInfoItem item)
        {
            return item.PropInfo;
        }
    }

}
