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
        [DtNumber(Decimals = 2)]
        public virtual Decimal Total { get; set; }
        [Association(Relation.Composition, Inv = "Budget")]
        public virtual HasMany<BudgetDetail> Details { get; set; }
    }
    
    public class BudgetDetail : InterceptedObject
    {
        public int FromTotal;
        public int FromModel;
        [DtNumber(Decimals = 2)]
        public virtual Decimal Value { get; set; }
        [DtNumber(Decimals = 2)]
        public virtual Decimal Proc { get; set; }
        [Association(Relation.Composition, Inv = "Details")]
        public virtual BelongsTo<Budget> Budget { get; set; }
        [Association(Relation.Association)]
        public virtual HasOne<BudgetDetailModel> Model { get; set; }
    }

    public class BudgetDetailModel : InterceptedObject
    {
        [DtNumber(Decimals = 2)]
        public virtual Decimal Value { get; set; }
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
                    target.Budget.Value.Total = target.Value * 100 / target.Proc;
                }
            }
            else
            {
                target.FromTotal++;
            }
            if (target.IsComingFrom("Model"))
            {
                target.FromModel++;
            }
        }

        //After Value Changed     
        [RulePropagation("Model")]
        public static void ModelChanged(BudgetDetail target)
        {
            if (target.Model.Value != null)
            {
                target.Value = target.Model.Value.Value;
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
                c.Value = c.Proc * target.Total / 100;
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
