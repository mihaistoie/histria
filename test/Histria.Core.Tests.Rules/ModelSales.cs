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

    [RulesFor(typeof(OrderLine))]
    public class RulesForOrderLine : IPluginModel
    {
        [Rule(Rule.Propagation, Property = "DecQty")]
        [Rule(Rule.Propagation, Property = "UnitPrice")]
        [Display("Calculate Sales Line Price")]
        public static void LinePrice(OrderLine target)
        {
            target.Price = target.DecQty * target.UnitPrice;
        }
    }

    [RulesFor(typeof(OrderLine))]
    public class StateRulesForOrderLine : IPluginModel
    {
        [State(Rule.AfterCreate)]
        [State(Rule.Propagation, Property = "Product")]
        public static void EnableControls(OrderLine target)
        {
            bool isDisabled = string.IsNullOrEmpty(target.Product);
            target.Properties["UnitPrice"].IsDisabled = isDisabled;
            target.Properties["DecQty"].IsDisabled = isDisabled;
            target.Properties["Price"].IsDisabled = isDisabled; 
        }
        [State(Rule.AfterCreate)]
        [State(Rule.AfterLoad)]
        public static void EnableProduct(OrderLine target)
        {
        }

    }

}
