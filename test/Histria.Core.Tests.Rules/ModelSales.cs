using Histria.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Core.Tests.Rules.Customers
{
    public class SalesOrder : InterceptedObject
    {
        public virtual string Customer { get; set; }
        public virtual int OrderNumber { get; set; }
        public virtual DateTime OrderDate { get; set; }
        [Association(Relation.Composition, Inv="Order")]
        public virtual HasMany<OrderLine> Lines { get; set; }
    }

    public class OrderLine : InterceptedObject
    {
        public virtual string Product { get; set; }
        public virtual Decimal DecQty { get; set; }
        public virtual Decimal UnitPrice { get; set; }
        public virtual Decimal Price { get; set; }
        [Association(Relation.Composition, Inv = "Lines")]
        public virtual BelongsTo<SalesOrder> Order { get; set; }
    }
}
