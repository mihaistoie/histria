using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sikia.AOP
{
    public class AspectInvocationContext: IAspectInvocationContext
    {
        public object Target{get; set;}

        public MethodInfo Method{get; set;}

        public object[] Arguments{get; set;}

        public object ReturnValue{get; set;}

        public Action<IAspectInvocationContext> Action { get; set; }
    }
}
