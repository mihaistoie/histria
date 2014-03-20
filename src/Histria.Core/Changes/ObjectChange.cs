using Histria.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Core.Changes
{
    public class ObjectDelta
    {
        public ObjectLifetimeEvent Lifetime { get; set; }
        public Guid Target { get; set; }
        public string Class { get; set; }

        public ChangedProperties Properties 
        {
            get; set; 
        }
    }
}
