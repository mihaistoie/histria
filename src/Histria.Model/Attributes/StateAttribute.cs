using System;

namespace Histria.Model
{
    /// <summary>
    /// State attribute for a method
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.All, AllowMultiple = true)]
    public class StateAttribute : System.Attribute  
    {
        public string Property;
        public Type TargetType = null;
        public Rule Rule = Rule.Unknown;
        public RoleOperation Operation = RoleOperation.None;
        public StateAttribute(Rule ruleType)
        {
            Rule = ruleType;
        }
        public bool CheckProperty()
        {
            if (string.IsNullOrEmpty(Property) && (Rule == Rule.Validation || Rule == Rule.Propagation)) {
                return false;
            }
            return true;
        }
   
    }
}
