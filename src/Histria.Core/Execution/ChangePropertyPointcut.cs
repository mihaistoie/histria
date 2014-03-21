using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Histria.AOP;

namespace Histria.Core.Execution
{
    public class ChangePropertyPointcut: IPointcut
    {
        public bool Matches(AspectInvocationContext context)
        {
            return this.propertyBySetMethod.ContainsKey(context.Method);
        }

        public PropertyInfo GetPropertyBySetMethod(MethodInfo setMethod)
        {
            PropertyInfo result;
            if(!this.propertyBySetMethod.TryGetValue(setMethod, out result))
            {
                result = null;
            }
            return result;
        }

        public void AddType(Type type)
        {
            if(!types.Contains(type))
            {
                this.LoadProperties(type);
                types.Add(type);
            }
        }
        private Dictionary<MethodInfo, PropertyInfo> propertyBySetMethod = new Dictionary<MethodInfo, PropertyInfo>();

        private List<Type> types = new List<Type>();

        private void LoadProperties(Type type)
        {
            const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public;
            foreach (var pi in type.GetProperties(bindingFlags))
            {
                MethodInfo setMethod = pi.GetSetMethod();
                if (setMethod == null || !setMethod.IsVirtual)
                {
                    continue;
                }
                propertyBySetMethod[setMethod] = pi;
            }
        }
    }
}
