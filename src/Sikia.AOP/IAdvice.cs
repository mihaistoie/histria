using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sikia.AOP
{
    public interface IAdvice
    {
        void DoBefore(IAspectInvocationContext invocationContext, IList<Exception> errors);
        void DoAfter(IAspectInvocationContext invocationContext, IList<Exception> errors);
        bool AllwaysExecuteAfter { get; }
        IAspectInvocationContext CreateContext();
    }
}
