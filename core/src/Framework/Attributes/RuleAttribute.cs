using Sikia.Framework.Model;
using System;

namespace Sikia.Framework
{
    /// <summary>
    /// Rule attribute for a method
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.All, AllowMultiple = true)]
    public class RuleAttribute : System.Attribute  
    {
        public string Property = "";
        public Type TargetType = null;
        public Rule Rule = Rule.Unknown;
        public RuleAttribute(Rule ruleType)
        {
            Rule = ruleType;
        }
        public bool CheckProperty()
        {
            if (String.IsNullOrEmpty(Property) && ((Rule & (Rule.Validation | Rule.Propagation)) != 0)) {
                return false;
            }
            return true;
        }
   
    }
}
