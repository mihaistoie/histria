using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Histria.Core
{

    ///<summary>
    /// List of objects
    ///</summary>
    public class InstanceList : KeyedCollection<Guid, ITranObject>
    {
        protected override Guid GetKeyForItem(ITranObject item)
        {
            return item.Uuid;
        }
    }
}
