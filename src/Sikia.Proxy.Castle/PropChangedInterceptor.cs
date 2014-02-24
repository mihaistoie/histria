using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;

namespace Sikia.Proxy.Castle
{
    /// <summary>
    /// This interceptor is automatically applied to any Type in the "Models" namespace 
    /// </summary>
    public class NotifyPropertyChangedInterceptor : IInterceptor
    {
        #region IInterceptor Implementation
        public void Intercept(IInvocation invocation)
        {
            if (invocation.InvocationTarget is IInterceptedObject)
            {
                bool isSet = (invocation.Method.Name.StartsWith("set_"));
                string propertyName = string.Empty;
                object oldValue = null;
                IInterceptedObject io = invocation.InvocationTarget as IInterceptedObject;
                if (isSet)
                {
                    propertyName = invocation.Method.Name.Substring(4);
                    object value = invocation.Arguments[0];
                    if (!io.AOPBeforeSetProperty(propertyName, ref value, ref oldValue))
                        return;
                    invocation.Arguments[0] = value;
                }
                // let the original call 
                invocation.Proceed();
                if (isSet)
                {

                    io.AOPAfterSetProperty(propertyName, invocation.Arguments[0], oldValue);
                }
            } 
            else invocation.Proceed(); 
        }
        #endregion
    }
}
