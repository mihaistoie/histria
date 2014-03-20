using System;

namespace Histria.Model
{
    /// <summary>
    /// State attribute for a method
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = true)]
    public class StateAttribute : RuleAttribute
    {
        public StateAttribute(Rule ruleType)
            : base(ruleType)
        {
            Rule = ruleType;
        }

        public override bool CheckProperty()
        {
            if (
                (Rule == Rule.Validation) ||
                (Rule == Rule.Correction)
               )
            {
                return false;
            }
            return true;
        }

    }
}
