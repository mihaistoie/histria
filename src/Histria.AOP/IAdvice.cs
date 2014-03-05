using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Histria.AOP
{
    public interface IAdvice
    {
        void DoBefore(AspectInvocationContext invocationContext, IList<Exception> errors);
        void DoAfter(AspectInvocationContext invocationContext, IList<Exception> errors);
        AspectInvocationContext CreateContext();
    }
}
