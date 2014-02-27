using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Collections.Generic;

namespace Sikia.AOP.Tests
{
    [TestClass]
    public class ComposedAdvisorTest
    {
        private class TestAdvice : IAdvice
        {
            public IList<string> Log { get; set; }
            public string Id { get; set; }
            public bool BeforeExecuted { get; set; }
            public bool AfterExecuted { get; set; }
            public bool ExecuteAfterIfExcept { get; set; }

            public bool ThrowBefore { get; set; }
            public bool ThrowAfter { get; set; }

            public void DoBefore(AspectInvocationContext invocationContext, IList<Exception> errors)
            {
                invocationContext.ExecuteAfterIfError = this.ExecuteAfterIfExcept;
                Log.Add(Id + " Before");
                this.BeforeExecuted = true;
                if (this.ThrowBefore)
                {
                    throw new ApplicationException(Id + " Before");
                }
            }

            public void DoAfter(AspectInvocationContext invocationContext, IList<Exception> errors)
            {
                Log.Add(Id + " After");
                this.AfterExecuted = true;
                if (this.ThrowAfter)
                {
                    throw new ApplicationException(Id + " After");
                }
            }

            public bool AllwaysExecuteAfter
            {
                get { return this.ExecuteAfterIfExcept; }
            }
           
            public AspectInvocationContext CreateContext()
            {
                return new AspectInvocationContext()
                {
                    ExecuteAfterIfError = this.ExecuteAfterIfExcept
                };
                
            }
        }

        private List<string> log;
        public void AOPMethod()
        {
            log.Add("M");
        }

        public void ThrowMethod()
        {
            log.Add("E");
            throw new ApplicationException("E");
        }

        [TestMethod]
        public void ComposedExecution1Test()
        {
            this.log = new List<string>();

            var innerAdvice = new TestAdvice()
            {
                Id = "inner",
                ThrowAfter = false,
                ThrowBefore = false,
                ExecuteAfterIfExcept = false,
                Log = log
            };
            var outerAdvice = new TestAdvice()
            {
                Id = "outer",
                ThrowAfter = false,
                ThrowBefore = false,
                ExecuteAfterIfExcept = false,
                Log = log
            };
            AdvisorBase inner = new AdvisorBase() { Aspect = new Aspect() { Advice = innerAdvice } };
            ComposedAdvisor outer = new ComposedAdvisor()
            {
                Aspect = new Aspect() { Advice = outerAdvice },
                InnerAdvisor = inner
            };
            AspectInvocationContext context = new AspectInvocationContext()
            {
                Action = c => c.ReturnValue = c.Method.Invoke(c.Target, c.Arguments),
                Target = this,
                Method = this.GetType().GetMethod("AOPMethod"),
                Arguments = null
            };

            outer.Execute(context);

            string[] expectedLog = new string[] { "outer Before", "inner Before", "M", "inner After", "outer After" };
            Assert.AreEqual(String.Join(";", expectedLog), String.Join(";", this.log), "calls are not in order");

        }

        [TestMethod]
        public void ComposedExecution2Test()
        {
            this.log = new List<string>();

            var innerAdvice = new TestAdvice()
            {
                Id = "inner",
                ThrowAfter = false,
                ThrowBefore = true,
                ExecuteAfterIfExcept = false,
                Log = log
            };
            var outerAdvice = new TestAdvice()
            {
                Id = "outer",
                ThrowAfter = false,
                ThrowBefore = false,
                ExecuteAfterIfExcept = false,
                Log = log
            };
            AdvisorBase inner = new AdvisorBase() { Aspect = new Aspect() { Advice = innerAdvice } };
            ComposedAdvisor outer = new ComposedAdvisor()
            {
                Aspect = new Aspect(){Advice = outerAdvice},
                InnerAdvisor = inner
            };
            bool exceptionThrown;
            try
            {
                AspectInvocationContext context = new AspectInvocationContext()
                {
                    Action = c => c.ReturnValue = c.Method.Invoke(c.Target, c.Arguments),
                    Target = this,
                    Method = this.GetType().GetMethod("AOPMethod"),
                    Arguments = null
                };

                outer.Execute(context);
                exceptionThrown = false;
            }
            catch (ApplicationException ex)
            {
                Assert.AreEqual(ex.Message, "inner Before");
                exceptionThrown = true;
            }
            Assert.IsTrue(exceptionThrown, "Exception not thrown");
            string[] expectedLog = new string[] { "outer Before", "inner Before" };
            Assert.AreEqual(String.Join(";", expectedLog), String.Join(";", this.log), "calls are not in order");
        }
        [TestMethod]
        public void ComposedExecution3Test()
        {
            this.log = new List<string>();

            var innerAdvice = new TestAdvice()
            {
                Id = "inner",
                ThrowAfter = false,
                ThrowBefore = false,
                ExecuteAfterIfExcept = false,
                Log = log
            };
            var outerAdvice = new TestAdvice()
            {
                Id = "outer",
                ThrowAfter = false,
                ThrowBefore = false,
                ExecuteAfterIfExcept = true,
                Log = log
            };
            AdvisorBase inner = new AdvisorBase() { Aspect = new Aspect() { Advice = innerAdvice } };
            ComposedAdvisor outer = new ComposedAdvisor()
            {
                Aspect = new Aspect(){Advice = outerAdvice},
                InnerAdvisor = inner
            };
            bool exceptionThrown;
            try
            {
                AspectInvocationContext context = new AspectInvocationContext()
                {
                    Action = c => c.ReturnValue = c.Method.Invoke(c.Target, c.Arguments),
                    Target = this,
                    Method = this.GetType().GetMethod("ThrowMethod"),
                    Arguments = null
                };

                outer.Execute(context);
                exceptionThrown = false;
            }
            catch (ApplicationException ex)
            {
                Assert.AreEqual(Exceptions.GetRealException(ex).Message, "E");
                exceptionThrown = true;
            }
            Assert.IsTrue(exceptionThrown, "Exception not thrown");
            string[] expectedLog = new string[] { "outer Before", "inner Before", "E", "outer After" };
            Assert.AreEqual(String.Join(";", expectedLog), String.Join(";", this.log), "calls are not in order");
        }
    }
}
