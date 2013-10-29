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
            if (isSet) 
            {
               propertyName = invocation.Method.Name.Substring(4);
               className = invocation.TargetType.Name;
              // if (invocation.Arguments[0] == "John") return; 
            }
            invocation.Proceed();

            if (isSet)
            {
                var pi = invocation.TargetType.GetProperty(propertyName);

 /*
                // check for the special attribute
                if (!pi.HasAttribute<INPCAttribute>())
                    return;

                FieldInfo info = invocation.TargetType.GetFields(
                        BindingFlags.Instance | BindingFlags.NonPublic)
                            .Where(f => f.FieldType == typeof(PropertyChangedEventHandler))
                            .FirstOrDefault();

                if (info != null)
                {
                    //get the INPC field, and invoke it we managed to get it ok
                    PropertyChangedEventHandler evHandler = 
                        info.GetValue(invocation.InvocationTarget) as PropertyChangedEventHandler;
                    if (evHandler != null)
                        evHandler.Invoke(invocation.TargetType, 
                            new PropertyChangedEventArgs(propertyName));
                }
				*/
            }
			
        }
        #endregion
    }
}
