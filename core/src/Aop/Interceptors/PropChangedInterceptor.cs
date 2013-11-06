using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;

namespace Sikia
{
    /// <summary>
    /// This interceptor is automatically applied to any Type in the "Models" namespace 
    /// </summary>
    public class NotifyPropertyChangedInterceptor : IInterceptor
    {
        #region IInterceptor Implementation
        public void Intercept(IInvocation invocation)
        {
            // let the original call 
            bool isSet = (invocation.Method.Name.StartsWith("set_"));
            string propertyName = "";
            string className = "" ;
            PropertyInfo pi = null;
            if (isSet) 
            {
               
               propertyName = invocation.Method.Name.Substring(4);
               className = invocation.TargetType.Name;
               //System.Console.WriteLine("Set on  {0}.{0}", className, propertyName);

               pi = invocation.TargetType.GetProperty(propertyName);
               if (invocation.Arguments[0] == pi.GetValue(invocation.InvocationTarget, null)) return; 
            }
            invocation.Proceed();

            if (isSet)
            {   
            }
			
        }
        #endregion
    }
}
