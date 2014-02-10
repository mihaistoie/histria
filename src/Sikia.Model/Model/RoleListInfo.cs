using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Model
{

    public class RoleListInfo : RoleInfoItem
    {
        public Relation Type { get; set; }
        public RoleRefInfo InvRole;  
    }
}
