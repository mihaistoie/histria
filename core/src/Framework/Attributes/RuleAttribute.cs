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
        public RuleType Rule = RuleType.Unknown;
        public RuleAttribute(string ruleType) 
        {
            Rule = AttributeParser.ParseRuleType(ruleType);
        }
        public bool CheckProperty()
        {
            if (String.IsNullOrEmpty(Property) && ((Rule & (RuleType.Validation | RuleType.Propagation)) != 0)) {
                return false;
            }
            return true;
        }
   
    }
}
