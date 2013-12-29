using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sikia.Framework;
using Sikia.Framework.Model;

namespace UnitTestModel.Models
{
    
    public class MInvalidRule : InterceptedObject
    {
        public virtual string Name { get; set; }
        [Rule(Rule.Propagation, Property = "FirstName")]
        protected virtual void InvalidRuleDefinition()
        {
        }
    }

    public class MR1 : InterceptedObject
    {
        public virtual string Name { get; set; }
        public virtual string RuleResult { get; set; }
        [Rule(Rule.Propagation, Property = "Name")]
        protected virtual void Test()
        {
            RuleResult = "MR1.Test";
        }
    }
    public class MR2 : MR1
    {

        [Rule(Rule.Propagation, Property = "Name")]
        protected override void Test()
        {
            RuleResult = "MR2.Test";
        }
    }
    
    [Display("M3-T", Description="M3-D")]
    public class MR3 : InterceptedObject
    {
    }

    [Display("@xxx", Description = "@yyy")]
    public class MR4 : InterceptedObject
    {
        public static string xxx()
        {
            return "MR4.xxx";
        }
        public static string yyy()
        {
            return "MR4.yyy";
        }
    }

    
}
