using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Core
{
    ///<summary>
    /// List of instances by class
    ///</summary>
    public class Instances
    {
        private InstancesByClass list = new InstancesByClass();
        public void Add(ITranObject instance)
        {
            list.AddInstance(instance);
        }
    }
}
