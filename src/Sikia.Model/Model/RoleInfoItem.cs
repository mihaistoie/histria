using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Model
{
    public class RoleInfoItem
    {
        public int Max { get; set; }
        public int Min { get; set; }
        public Relation Type { get; set; }
        public RoleInfoItem InvRole = null;
        public bool IsChild = false;
        public string RoleInvName;
    }
}
