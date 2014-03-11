using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Model
{
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false)]
    public class PersistentAttribute : System.Attribute
    {
        public bool IsPersistent = true;
        public string PersistentName;
        public PersistentAttribute(bool value) 
        {
            IsPersistent = value;
        }
    }
}
