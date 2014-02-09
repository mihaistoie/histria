using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Core
{
    ///<summary>
    /// List of instances by class
    ///</summary>
    public class Instances
    {
        private InstancesByClass list = new InstancesByClass();
        public void Add(TranObject instance)
        {
            list.AddInstance(instance);
        }
    }
}
