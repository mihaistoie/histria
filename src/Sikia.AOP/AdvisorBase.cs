using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sikia.AOP
{
    internal class AdvisorBase
    {
        public Aspect Aspect { get; set; }
        public void Execute(AspectInvocationContext context)
        {
            var errors = new List<Exception>();
            this.Execute(context, errors);
            switch (errors.Count)
            {
                case 0:
                    return;
                case 1:
                    throw errors[0];
                default:
                    throw new AggregateException(errors);
            }
        }

        public void Execute(AspectInvocationContext context, IList<Exception> errors)
        {
            bool executeAspect = MustExecuteAspect(context);

            if (executeAspect)
            {
                ExecuteAspect(context, errors);
            }
            else
            {
                Proceed(context, errors);
            }

        }

        private bool MustExecuteAspect(AspectInvocationContext context)
        {
            if (this.Aspect == null)
            {
                return false;
            }
            return this.Aspect.MustExecute(context);
        }

        protected void ExecuteAspect(AspectInvocationContext context, IList<Exception> errors)
        {
            this.Aspect.Execute(context, errors, this.Proceed);
        }

        protected virtual void Proceed(AspectInvocationContext context, IList<Exception> errors)
        {
            if (context.ExecuteAction && errors.Count == 0)
            {
                context.Action(context);
            }
        }
    }
}
