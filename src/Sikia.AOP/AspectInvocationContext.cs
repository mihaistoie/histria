using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sikia.AOP
{
    public class AspectInvocationContext
    {
        public AspectInvocationContext()
        {
            this.ExecuteAction = true;
            this.ExecuteAfterIfError = false;
        }

        public object Target{get; set;}

        public MethodInfo Method{get; set;}

        public object[] Arguments{get; set;}

        public object ReturnValue{get; set;}

        public bool ExecuteAction { get; internal set; }

        public Action<AspectInvocationContext> Action { get; set; }

        public void DoNotExecuteAction()
        {
            this.ExecuteAction = false;
        }

        public bool ExecuteAfterIfError { get; set; }
    }
}
