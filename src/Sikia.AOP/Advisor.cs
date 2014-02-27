using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sikia.AOP
{
    public class Advisor
    {
        public Advisor(IList<Aspect> aspects)
        {
            this.executor = this.GetExecutor(aspects);
        }

        public void Execute(AspectInvocationContext context)
        {
            this.executor.Execute(context);
        }

        private AdvisorBase executor;

        private AdvisorBase GetExecutor(IList<Aspect> aspects)
        {
            if (aspects == null)
            {
                throw new ArgumentNullException("aspects");
            }

            if (aspects.Count == 1)
            {
                return new AdvisorBase()
                {
                    Aspect = aspects[0],
                };
            }

            return new ComposedAdvisor()
            {
                Aspect = aspects.First(),
                InnerAdvisor = this.GetExecutor(aspects.Skip(1).ToList())
            };
        }
    }
}
