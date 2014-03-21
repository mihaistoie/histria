using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Histria.AOP;
using Histria.Model;

namespace Histria.Core.Execution
{
    public class ChangePropertyStateAspect : IAdvice, IPointcut
    {
        public ChangePropertyStateAspect()
        {
            this.pointcut = new ChangePropertyPointcut();
            this.pointcut.AddType(typeof(PropertyState));
        }

        private ChangePropertyPointcut pointcut;

        public bool Matches(AspectInvocationContext context)
        {
            bool result = context.Target is PropertyState
                && pointcut.Matches(context);
            return result;
        }

        private class ChangePropertyStateAspectInvocationContext : AspectInvocationContext
        {
            public PropertyInfo Property { get; set; }
            public object OldValue { get; set; }
            public IInterceptedObject InterceptedObject { get; set; }
            public new PropertyState Target
            {
                get { return base.Target as PropertyState; }
            }
        }

        private ChangePropertyStateAspectInvocationContext GetInvocationContext(AspectInvocationContext invocationContext)
        {
            ChangePropertyStateAspectInvocationContext context = (ChangePropertyStateAspectInvocationContext)invocationContext;

            if (context.Property == null)
            {
                context.Property = pointcut.GetPropertyBySetMethod(context.Method);
            }

            return context;
        }

        public void DoBefore(AspectInvocationContext invocationContext, IList<Exception> errors)
        {
            ChangePropertyStateAspectInvocationContext context = this.GetInvocationContext(invocationContext);
            PropertyInfo pi = context.Property;
            context.OldValue = pi.GetGetMethod().Invoke(context.Target, null);
        }

        public void DoAfter(AspectInvocationContext invocationContext, IList<Exception> errors)
        {
            ChangePropertyStateAspectInvocationContext context = this.GetInvocationContext(invocationContext);
            PropertyInfo pi = context.Property;
            var newValue = pi.GetGetMethod().Invoke(context.Target, null);

            bool changed = pi.PropertyType.IsValueType ? !newValue.Equals(context.OldValue) : newValue != context.OldValue;

            if (changed)
            {
                context.Target.AOPAfterChange(pi.Name, context.OldValue, newValue);
            }
        }

        public AspectInvocationContext CreateContext()
        {
            return new ChangePropertyStateAspectInvocationContext();
        }
    }
}
