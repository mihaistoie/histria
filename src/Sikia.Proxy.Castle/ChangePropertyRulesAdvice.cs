using Sikia.AOP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sikia.Proxy.Castle
{
    internal class ChangePropertyRulesAspect : IAdvice, IPointcut
    {
        public void DoBefore(IAspectInvocationContext invocationContext, IList<Exception> errors)
        {
            ChangePropertyRulesAspectContext context = GetContext(invocationContext);
            object value = context.Arguments[0];
            object oldValue = null;
            if (context.InterceptedObject.AOPBeforeSetProperty(context.PropertyName, ref value, ref oldValue))
                return;
            context.OldValue = oldValue;
            context.Arguments[0] = value;
        }

        public void DoAfter(IAspectInvocationContext invocationContext, IList<Exception> errors)
        {
            ChangePropertyRulesAspectContext context = GetContext(invocationContext);
            context.InterceptedObject.AOPAfterSetProperty(context.PropertyName, context.Arguments[0], context.OldValue);
        }

        public bool AllwaysExecuteAfter
        {
            get { return false; }
        }

        public IAspectInvocationContext CreateContext()
        {
            return new ChangePropertyRulesAspectContext();
        }

        public bool Matches(IAspectInvocationContext context)
        {
            return (context.Target is IInterceptedObject) && IsSetPropMethod(context.Method);
        }

        private static bool IsSetPropMethod(MethodInfo method)
        {
            return method.Name.StartsWith("set_");
        }

        private string GetPropName(MethodInfo method)
        {
            return method.Name.Substring(4);
        }

        private IInterceptedObject GetInterceptedObject(object target)
        {
            return (IInterceptedObject)target;
        }

        private class ChangePropertyRulesAspectContext : AspectInvocationContext
        {
            public object OldValue { get; set; }

            public IInterceptedObject InterceptedObject
            {
                get { return (IInterceptedObject)this.Target; }
            }
            public string PropertyName
            {
                get { return this.Method.Name.Substring(4); }
            }
        }

        private static ChangePropertyRulesAspectContext GetContext(IAspectInvocationContext invocationContext)
        {
            return (ChangePropertyRulesAspectContext)invocationContext;
        }
    }
}
