using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Histria.Model
{
    public class KeysCollection : KeyedCollection<PropertyInfo, KeyItem>
    {
        protected override PropertyInfo GetKeyForItem(KeyItem item)
        {
            return item.Property;
        }
    }
}
