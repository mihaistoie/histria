using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sikia.AOP
{
    public interface IPointcut
    {
        bool Matches(IAspectInvocationContext context);
    }
}
