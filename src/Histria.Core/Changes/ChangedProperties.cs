using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Histria.Core.Changes
{
    public class ChangedProperties : KeyedCollection<string, PropertyChange>
    {
        public string Name { get; set; }

        protected override string GetKeyForItem(PropertyChange item)
        {
            return item.PropertyName;
        }
    }
}
