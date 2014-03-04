using Sikia.AOP;
using Sikia.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Core.Execution
{
    public class ContainerSetup
    {
        public ModelManager ModelManager { get; set; }

        public Advisor Advisor {get; set;}
    }
}
