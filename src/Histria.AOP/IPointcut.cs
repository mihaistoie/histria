using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Histria.AOP
{
    public interface IPointcut
    {
        bool Matches(AspectInvocationContext context);
    }
}
