using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sikia.AOP
{
    public interface IAspectInvocationContext
    {
        object Target { get; set; }
        MethodInfo Method { get; set; }
        object[] Arguments { get; set; }
        object ReturnValue{get; set;}

        Action<IAspectInvocationContext> Action { get; set; }
    }
}
