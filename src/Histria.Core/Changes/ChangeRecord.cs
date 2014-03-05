using Histria.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Core.Changes
{
    public class ChangeRecord
    {
        public uint Id { get; set;}
        public Guid TargetId { get; set; }
        public ObjectLifetime Lifetime { get; set; }
        public object[] Arguments { get; set; }
    }
}
