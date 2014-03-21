using Histria.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Histria.AOP;

namespace Histria.Core.Execution
{
    public static class TestContainerSetup
    {
        public static ContainerSetup GetSimpleContainerSetup(ModelManager model)
        {
            return new ContainerSetup()
            {
                ModelManager = model,
                Advisor = CreateTestAdvisor()
            };
        }

        private static AOP.Advisor CreateTestAdvisor()
        {
            var aspectTypes = new Type[]
            {
                typeof(ChangePropertyRulesAspect),
                typeof(ChangePropertyStateAspect),
            };
            List<Aspect> aspects = new List<Aspect>();

            foreach(Type t in aspectTypes)
            {
                var o = Activator.CreateInstance(t);
                var aspect = new Aspect()
                {
                    Advice = (IAdvice) o,
                    Pointcuts = new IPointcut[]{(IPointcut)o}
                };
                aspects.Add(aspect);
            }

            Advisor advisor = new Advisor(aspects);
            return advisor;
        }
    }
}
