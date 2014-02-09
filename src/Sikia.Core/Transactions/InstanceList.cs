using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Sikia.Core
{

    ///<summary>
    /// List of objects
    ///</summary>
    public class InstanceList : KeyedCollection<Guid, TranObject>
    {
        protected override Guid GetKeyForItem(TranObject item)
        {
            return item.Uuid;
        }
    }
}
