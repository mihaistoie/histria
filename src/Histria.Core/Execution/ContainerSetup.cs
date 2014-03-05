using Histria.AOP;
using Histria.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Core.Execution
{
    public class ContainerSetup
    {
        public ModelManager ModelManager { get; set; }

        public Advisor Advisor {get; set;}
    }
}
