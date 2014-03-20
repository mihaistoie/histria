using System;

namespace Histria.Model
{
    /// <summary>
    /// Rule attribute for a method
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = true)]
    public class RuleAttribute : System.Attribute  
    {
        public string Property;
        public Type TargetType = null;
        public Rule Rule = Rule.Unknown;
        public RoleOperation Operation = RoleOperation.None; 
        public RuleAttribute(Rule ruleType)
        {
            Rule = ruleType;
        }
        public virtual bool CheckProperty()
        {
            if (string.IsNullOrEmpty(Property) && (Rule == Rule.Validation || Rule == Rule.Propagation)) {
                return false;
            }
            return true;
        }
    }
}
