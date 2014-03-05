using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Histria.Model
{
    public class RuleList : List<RuleItem>
    {

        public void ExecuteCheckValueRules(object target, ref object value)
        {
            RoleOperation operation = RoleOperation.None;
            foreach (RuleItem ri in this)
            {
                if (ri.Operation == operation)
                {
                    try
                    {
                        object[] arguments = null; 
                        if (ri.Method.IsStatic)
                        {
                            arguments = new object[] { target, value };
                            ri.Method.Invoke(null, arguments);
                            value = arguments[1];
                        }
                        else
                        {
                            arguments = new object[] { value };
                            ri.Method.Invoke(target, arguments);
                            value = arguments[0];
                        }


                    }
                    catch (TargetInvocationException ex)
                    {
                        throw ex.InnerException; // ex now stores the original exception
                    }
                }
            }
        }

        public void Execute(object target, RoleOperation operation)
        {
            foreach (RuleItem ri in this)
            {
                if (ri.Operation == operation)
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
}
