using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Core.Changes
{
    public class PropertyChange
    {
        public string PropertyName { get; set; }
        public object OldValue { get; set; }
        public object Value { get; set; }
    }
}
