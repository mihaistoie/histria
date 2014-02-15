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
        public RoleInfoItem InvRole { get; set; }
        public bool IsChild { get; set; }
        public bool IsList { get; set; }
        public string RoleInvName { get; set; }
        public string Foreingkey { get; set; }
        public bool UseUuid { get; set; }
    }
}
