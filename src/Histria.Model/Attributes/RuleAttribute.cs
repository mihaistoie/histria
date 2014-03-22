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
        protected RuleAttribute()
        {
        }
        public RuleAttribute(Rule ruleType)
        {
            Rule = ruleType;
        }
        public virtual bool CheckProperty()
        {
            if (string.IsNullOrEmpty(Property) && (Rule == Rule.Validation || Rule == Rule.Propagation))
            {
                return false;
            }
            return true;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = true)]
    public class RulePropagationAttribute : RuleAttribute
    {
        public RulePropagationAttribute(string property)
        {
            Rule = Rule.Propagation;
            Property = property;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = true)]
    public class RuleAfterCreate : RuleAttribute
    {
        public RuleAfterCreate()
        {
            Rule = Rule.AfterCreate;
        }
    }
   
    [System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = true)]
    public class RuleAfterLoad : RuleAttribute
    {
        public RuleAfterLoad()
        {
            Rule = Rule.AfterLoad;
        }
    }
    
    [System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = true)]
    public class RuleAfterDelete : RuleAttribute
    {
        public RuleAfterDelete()
        {
            Rule = Rule.AfterDelete;
        }
    }

}
