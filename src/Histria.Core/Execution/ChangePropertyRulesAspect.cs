using Histria.AOP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Histria.Core.Execution
{
    public class ChangePropertyRulesAspect : IAdvice, IPointcut
    {
        private ChangePropertyPointcut pointcut = new ChangePropertyPointcut();

        public void DoBefore(AspectInvocationContext invocationContext, IList<Exception> errors)
        {
            ChangePropertyRulesAspectContext context = GetContext(invocationContext);
            object value = context.Arguments[0];
            object oldValue = null;
            if (!context.InterceptedObject.AOPBeforeSetProperty(context.PropertyName, ref value, ref oldValue))
            {
                context.DoNotExecuteAction();
            }
            context.OldValue = oldValue;
            context.Arguments[0] = value;
        }

        public void DoAfter(AspectInvocationContext invocationContext, IList<Exception> errors)
        {
            if (invocationContext.ExecuteAction && errors.Count == 0)
            {
                ChangePropertyRulesAspectContext context = GetContext(invocationContext);
                context.InterceptedObject.AOPAfterSetProperty(context.PropertyName, context.Arguments[0], context.OldValue);
            }
        }

        public AspectInvocationContext CreateContext()
        {
            return new ChangePropertyRulesAspectContext();
        }

        public bool Matches(AspectInvocationContext context)
        {
            if (context.Target is IInterceptedObject)
            {
                var realType = ((IInterceptedObject)context.Target).ClassInfo.CurrentType;

                pointcut.AddType(realType);
                return pointcut.Matches(context);
            }
            return false;
        }

        private string GetPropName(MethodInfo method)
        {
            return pointcut.GetPropertyBySetMethod(method).Name;
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
            public string PropertyName { get; set; }
        }

        private ChangePropertyRulesAspectContext GetContext(AspectInvocationContext invocationContext)
        {
            var context = (ChangePropertyRulesAspectContext)invocationContext;
            if (String.IsNullOrEmpty(context.PropertyName))
            {
                context.PropertyName = this.pointcut.GetPropertyBySetMethod(context.Method).Name;
            }
            return context;
        }
    }
}
