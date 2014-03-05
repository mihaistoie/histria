using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Histria.AOP
{
    public static class Exceptions
    {
        public static Exception GetRealException(Exception ex)
        {
            if (ex is TargetInvocationException)
            {
                if (ex.InnerException != null)
                {
                    return ex.InnerException;
                }
                else
                {
                    return ex;
                }
            }
            return ex;
        }
    }
}
