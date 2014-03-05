using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Histria.AOP
{
    public class Aspect
    {
        public IAdvice Advice { get; set; }
        public IList<IPointcut> Pointcuts { get; set; }

        internal bool MustExecute(AspectInvocationContext context)
        {
            if (this.Advice != null)
            {
                if (this.Pointcuts == null)
                {
                    return true;
                }
                for (int i = 0; i < this.Pointcuts.Count; i++)
                {
                    if (this.Pointcuts[i].Matches(context))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        internal void Execute(AspectInvocationContext externContext, IList<Exception> errors, Action<AspectInvocationContext, IList<Exception>> proceed)
        {
            AspectInvocationContext context = this.Advice.CreateContext();
            context.ExecuteAfterIfError = externContext.ExecuteAfterIfError;
            context.Target = externContext.Target;
            context.Method = externContext.Method;
            context.Arguments = externContext.Arguments;
            context.ReturnValue = externContext.ReturnValue;
            context.Action = externContext.Action;

            ExecuteBefore(context, errors);
            bool executeAfter = context.ExecuteAfterIfError;
            try
            {
                proceed(context, errors);
                if (errors.Count == 0)
                {
                    executeAfter = true;
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex);
            }
            finally
            {
                if (executeAfter)
                {
                    ExecuteAfter(context, errors);
                }
            }
        }

        private void ExecuteBefore(AspectInvocationContext context, IList<Exception> errors)
        {
            try
            {
                Advice.DoBefore(context, errors);
            }
            catch (Exception ex)
            {
                errors.Add(ex);
            }
        }

        private void ExecuteAfter(AspectInvocationContext context, IList<Exception> errors)
        {
            try
            {
                Advice.DoAfter(context, errors);
            }
            catch (Exception ex)
            {
                errors.Add(ex);
            }
        }
    }
}
