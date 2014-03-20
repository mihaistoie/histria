using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Sys.Tests
{
    class PCStack : BaseStack
    {
        public void Push(string value)
        {
            base.DoPush(value);
        }
        public string Pop()
        {
            return base.DoPop();
        }

        public bool Has(string fixedPart, string variable)
        {
            return base.Contains(fixedPart, variable);
        }
  
    }
}
