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

        [Display("Net Amount (excluding VAT)")]
        public virtual Decimal NetAmount { get; set; }
        [Display("VAT")]
        public virtual Decimal VAT { get; set; }
        [Display("Gross Amount (including VAT)")]
        public virtual Decimal GrossAmount { get; set; }

        [Association(Relation.Composition, Inv = "Order")]
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
        [RulePropagation("DecQty")]
        [RulePropagation("UnitPrice")]
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

    [RulesFor(typeof(SalesOrder))]
    public class PropagationRulesForOrder : IPluginModel
    {
        private static decimal VatTax = 18.33M;
        public static void VATForOrder(SalesOrder target)
        {
            target.VAT = Math.Round(target.NetAmount * VatTax / 100, 2);
            target.GrossAmount = target.VAT + target.NetAmount;
        }

        //After NetAmount Changed     
        [RulePropagation("NetAmount")]
        public static void NetAmountChanged(SalesOrder target)
        {
            VATForOrder(target);
        }

        //After VAT Changed
        [RulePropagation("VAT")]
        public static void VATChanged(SalesOrder target)
        {
            if (target.IsComingFrom("NetAmount"))
                return;
            target.NetAmount = Math.Round(target.VAT * 100 / VatTax, 2);
        }

        //After GrossAmount Changed
        [RulePropagation("GrossAmount")]
        public static void GrossAmountChanged(SalesOrder target)
        {
            if (target.IsComingFrom("NetAmount"))
                return;
            const decimal VatTax = 18.33M; // only for demo 
            target.NetAmount = Math.Round(target.GrossAmount * 100 / (100 + VatTax), 2);
        }

    }
}

