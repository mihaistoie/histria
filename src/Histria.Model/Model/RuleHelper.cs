using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Model
{
    static class RuleHelper
    {
        ///<summary>
        /// Add a rule to a dictionary
        ///</summary>   
        internal static void AddRule(Dictionary<Rule, RuleList> ruleList, RuleItem ri)
        {

            RuleList rl = null;
            if (!ruleList.TryGetValue(ri.Kind, out rl))
            {
                rl = new RuleList();
                ruleList.Add(ri.Kind, rl);
            }
            rl.Add(ri);

        }
        ///<summary>
        /// Execute rules by type
        ///</summary>   
        internal static void ExecuteRules(Dictionary<Rule, RuleList> rules, Rule kind, object target, RoleOperation operation)
        {
            RuleList rl = null;
            if (rules.TryGetValue(kind, out rl))
            {
                rl.Execute(target, operation);
            }

        }
    }
}
