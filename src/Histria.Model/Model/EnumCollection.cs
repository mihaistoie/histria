using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Histria.Model
{
    public class EnumCollection : KeyedCollection<Type, EnumInfoItem>
    {
        protected override Type GetKeyForItem(EnumInfoItem item)
        {
            return item.EnumType;
        }
        
        public bool TryGetEnumInfo(Type type, out EnumInfoItem enumInfo)
        {
            if (this.Dictionary == null)
            {
                enumInfo = null;
                return false;
            }
            return this.Dictionary.TryGetValue(type, out enumInfo);
        }
    }
    
}
