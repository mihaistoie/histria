using Histria.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Core.Tests.Rules.Customers
{
    public class BlogItem : InterceptedObject
    {
        public virtual Memo Text  { get; set; }
    }
}
