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

        public void Execute(object target, RoleOperation operation, object[] arguments)
        {
            foreach (RuleItem ri in this)
            {
                if (ri.Operation == operation)
                {
                    try
                    {
                        int methodArgumentsCount = ri.Method.GetParameters().Length;

                        object[] allArgs = new object[methodArgumentsCount];
                        int idx;
                        object instance;

                        if (ri.Method.IsStatic)
                        {
                            allArgs[0] = target;
                            idx = 1;
                            instance = null;
                        }
                        else
                        {
                            idx = 0;
                            instance = target;
                        }

                        if (arguments != null && arguments.Length > 0)
                        {
                            for (int i = 0; i < arguments.Length && idx < allArgs.Length; i++)
                            {
                                allArgs[idx + i] = arguments[i];
                            }
                        }
                        ri.Method.Invoke(instance, allArgs);
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
