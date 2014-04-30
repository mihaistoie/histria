using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Histria.Model;

namespace Histria.Core.Tests.Associations
{

    public class Element : InterceptedObject
    {
        public virtual string Name { get; set; }
    }

    public class ElementList : InterceptedObject
    {
        public int Count { get; private set; }
        public string LastItemChanged;
        [Association(Relation.List)]
        public virtual HasMany<Element> Elements { get; set; }

        [RulePropagation("Elements", Operation = RoleOperation.Add)]
        public void OnAdd(Element added)
        {
            this.Count++;
            LastItemChanged = added.Name;
        }

        [RulePropagation("Elements", Operation = RoleOperation.Remove)]
        public void OnRemove(Element removed)
        {
            this.Count--;
            LastItemChanged = removed.Name;
        }
    }
}
