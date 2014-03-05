using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria
{
    public class RuleException : Exception
    {
        public RuleException(string msg) : base(msg) { }

        public RuleException(string fmt, params object[] args)
            : base(string.Format(fmt, args))
        {
        }
    }
}
