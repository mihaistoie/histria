using Histria.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Core.Tests.Rules.Customers
{
    public class Budget : InterceptedObject
    {
        public int FromDetail; 
        public virtual Decimal Total { get; set; }
        [Association(Relation.Composition, Inv = "Budget")]
        public virtual HasMany<BudgetDetail> Details { get; set; }
    }
    
    public class BudgetDetail : InterceptedObject
    {
        public int FromTotal; 
        public virtual Decimal Value { get; set; }
        public virtual Decimal Proc { get; set; }
        [Association(Relation.Composition, Inv = "Details")]
        public virtual BelongsTo<Budget> Budget { get; set; }
    }

    [RulesFor(typeof(BudgetDetail))]
    public class PropagationRulesForBudgetDetail : IPluginModel
    {
        //After Value Changed     
        [RulePropagation("Value")]
        public static void ValueChanged(BudgetDetail target)
        {
            if (!target.IsComingFrom("Budget.Total"))
            {
                if ((target.Budget.Value != null) && (target.Proc > 0))
                {
                    target.Budget.Value.Total = Math.Round(target.Value * 100 / target.Proc, 2);
                }
            }
            else
            {
                target.FromTotal++;
            }
        }
    }
    [RulesFor(typeof(Budget))]
    public class PropagationRulesForBudget : IPluginModel
    {
        //After Total Changed     
        [RulePropagation("Total")]
        public static void TotalChanged(Budget target)
        {
            if (target.IsComingFrom("Details.Value"))
            {
                target.FromDetail++;
            }
            BudgetDetail MaxBudget = null;
            Decimal tmp = 0;
            Decimal tt = 0;
            for (int i = 0, len = target.Details.Count; i < len; i++)
            {
                BudgetDetail c = target.Details[i];
                c.Value = Math.Round(c.Proc * target.Total / 100, 2);
                if (c.Value > tmp) {
                    tmp = c.Value;
                    MaxBudget = c;
                }
                tt += c.Value;
            }
            if (MaxBudget != null && (tt != target.Total))
            {
                MaxBudget.Value = MaxBudget.Value + target.Total - tt; 
            }
          
        }
    }
}
