using Histria.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Core.Tests.Rules.Customers
{
    public partial class BlogItem : InterceptedObject
    {
        public virtual Memo Text  { get; set; }
    }

    public partial class BlogItem : InterceptedObject
    {
        public int CCount;
        [Rule(Rule.Propagation, Property = "Text.Value")]
        protected virtual void Calculate()
        {
            CCount = string.IsNullOrEmpty(Text.Value) ? 0 : Text.Value.Length; 
        }
    }

}
