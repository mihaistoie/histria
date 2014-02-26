using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Collections.Generic;

namespace Sikia.AOP.Tests
{
    [TestClass]
    public class AdvisorBaseTest
    {
        private class TestAdvice : IAdvice
        {
            public bool BeforeExecuted { get; set; }
            public bool AfterExecuted { get; set; }
            public bool ExecuteAfterIfExcept { get; set; }

            public void DoBefore(IAspectInvocationContext invocationContext, IList<Exception> errors)
            {
                this.BeforeExecuted = true;
            }

            public void DoAfter(IAspectInvocationContext invocationContext, System.Collections.Generic.IList<Exception> errors)
            {
                this.AfterExecuted = true;
            }

            public bool AllwaysExecuteAfter
            {
                get { return this.ExecuteAfterIfExcept; }
            }

            public IAspectInvocationContext CreateContext()
            {
                return new AspectInvocationContext();
            }
        }

        public void AOPMethod()
        {

        }

        public void ThrowMethod()
        {
            throw new ApplicationException("Method throw");
        }

        [TestMethod]
        public void BasicExecution1Test()
        {
            var advice = new TestAdvice() { BeforeExecuted = false, AfterExecuted = false };
            var aspect = new Aspect() { Advice = advice };
            AdvisorBase advisor = new AdvisorBase()
            {
                Aspect = aspect
            };

            IAspectInvocationContext context = new AspectInvocationContext()
            {
                Action = c => c.ReturnValue = c.Method.Invoke(c.Target, c.Arguments),
                Target = this,
                Method = this.GetType().GetMethod("AOPMethod"),
                Arguments = null
            };

            advisor.Execute(context);

            Assert.AreEqual(true, advice.BeforeExecuted, "Before not executed");
            Assert.AreEqual(true, advice.AfterExecuted, "After not executed");
        }

        [TestMethod]
        public void BasicExecution2Test()
        {
            var advice = new TestAdvice() { BeforeExecuted = false, AfterExecuted = false, ExecuteAfterIfExcept = true };
            AdvisorBase advisor = new AdvisorBase()
            {
                Aspect = new Aspect() { Advice = advice }
            };
            bool exceptionThrown = false;
            try
            {
                IAspectInvocationContext context = new AspectInvocationContext()
                {
                    Action = c => c.ReturnValue = c.Method.Invoke(c.Target, c.Arguments),
                    Target = this,
                    Method = this.GetType().GetMethod("ThrowMethod"),
                    Arguments = null
                };

                advisor.Execute(context);
            }
            catch (ApplicationException ex)
            {
                Assert.AreEqual(Exceptions.GetRealException(ex).Message, "Method throw");
                exceptionThrown = true;
            }
            Assert.IsTrue(exceptionThrown, "Exception not thrown");
            Assert.AreEqual(true, advice.BeforeExecuted, "Before not executed");
            Assert.AreEqual(true, advice.AfterExecuted, "After not executed");
        }

        [TestMethod]
        public void BasicExecution3Test()
        {
            var advice = new TestAdvice() { BeforeExecuted = false, AfterExecuted = false, ExecuteAfterIfExcept = false };
            AdvisorBase advisor = new AdvisorBase()
            {
                Aspect = new Aspect() { Advice = advice }
            };
            bool exceptionThrown = false;
            try
            {
                IAspectInvocationContext context = new AspectInvocationContext()
                {
                    Action = c => c.ReturnValue = c.Method.Invoke(c.Target, c.Arguments),
                    Target = this,
                    Method = this.GetType().GetMethod("ThrowMethod"),
                    Arguments = null
                };

                advisor.Execute(context);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(Exceptions.GetRealException(ex).Message, "Method throw");
                exceptionThrown = true;
            }
            Assert.IsTrue(exceptionThrown, "Exception not thrown");
            Assert.AreEqual(true, advice.BeforeExecuted, "Before not executed");
            Assert.AreEqual(false, advice.AfterExecuted, "After is executed");
        }
    }
}
