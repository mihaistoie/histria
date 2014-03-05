using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Histria.Model
{
    public class RulesCollection : KeyedCollection<MethodInfo, RuleItem>
    {
        protected override MethodInfo GetKeyForItem(RuleItem item)
        {
            return item.Method;
        }
    }
}
