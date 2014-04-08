using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPOReaf.Tools
{
    internal static class MathUtil
    {
        public static decimal Round2(decimal value)
        {
            return Math.Round(value, 2, MidpointRounding.AwayFromZero);
        }
    }
}
