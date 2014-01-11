using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sikia.Core.Model
{
    public class RuleList : List<RuleItem>
    {
        public void Execute(object target)
        {
            foreach (RuleItem ri in this)
            {
                try
                {
                    if (ri.Method.IsStatic)
                    {
                        ri.Method.Invoke(null, new object[] { target });
                    }
                    else
                    {
                        ri.Method.Invoke(target, null);
                    }


                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException; // ex now stores the original exception
                }                
            }
        }
    }
}
