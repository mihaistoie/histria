using Sikia.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Core.Execution
{
    public static class TestContainerSetup
    {
        public static ContainerSetup GetSimpleContainerSetup(ModelManager model)
        {
            return new ContainerSetup()
            {
                ModelManager = model,
                Advisor = ChangePropertyRulesAspect.CreateAdvisor()
            };
        }
    }
}
