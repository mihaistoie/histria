using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Sikia.Core
{
    public class InstancesByClass : Dictionary<Type, InstanceList>
    {
        public void AddInstance(ITranObject instance)
        {
            Type type = instance.ClassType();
            InstanceList instances = null;
            TryGetValue(type, out instances);
            if (instances == null)
            {
                instances = new InstanceList();
                Add(type, instances);
            }
            instances.Add(instance);
        }

    }
}
