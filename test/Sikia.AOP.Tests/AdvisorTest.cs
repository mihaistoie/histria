using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Reflection;

namespace Sikia.AOP.Tests
{
    [TestClass]
    public class AdvisorTest
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

            public bool AddErrorBefore { get; set; }
            public bool AddErrorAfter { get; set; }

            public void DoBefore(IAspectInvocationContext invocationContext, IList<Exception> errors)
            {
                Log.Add(Id + " Before");
                this.BeforeExecuted = true;
                if (this.ThrowBefore)
                {
                    throw new ApplicationException(Id + " Before");
                }
            }

            public void DoAfter(IAspectInvocationContext invocationContext, IList<Exception> errors)
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

            public IAspectInvocationContext CreateContext()
            {
                return new AspectInvocationContext();
            }
        }

        private class TrueFalsePointcut : IPointcut
        {
            public bool Match { get; set; }
            public bool Matches(IAspectInvocationContext context)
            {
                return this.Match;
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
        public void AdvisorNoException()
        {
            this.log = new List<string>();
            List<IAdvice> advices = new List<IAdvice>()
            {
                new TestAdvice()
                {
                    Id = "1",
                    ThrowAfter = false,
                    ThrowBefore = false,
                    ExecuteAfterIfExcept = false,
                    Log = log
                },
                new TestAdvice()
                {
                    Id = "2",
                    ThrowAfter = false,
                    ThrowBefore = false,
                    ExecuteAfterIfExcept = false,
                    Log = log
                },
                new TestAdvice()
                {
                    Id = "3",
                    ThrowAfter = false,
                    ThrowBefore = false,
                    ExecuteAfterIfExcept = false,
                    Log = log
                },
            };

            IPointcut pointcut = new TrueFalsePointcut(){Match = true};
            IList<IPointcut> pointcuts = new IPointcut[] { pointcut };
            Advisor a = new Advisor(new Aspect[] 
                {
                    new Aspect(){Pointcuts = pointcuts, Advice = advices[0]},
                    new Aspect(){Pointcuts = pointcuts, Advice = advices[1]},
                    new Aspect(){Pointcuts = pointcuts, Advice = advices[2]},
                });

            IAspectInvocationContext context = new AspectInvocationContext()
            {
                Action = c => c.ReturnValue = c.Method.Invoke(c.Target, c.Arguments),
                Target = this,
                Method = this.GetType().GetMethod("AOPMethod"),
                Arguments = null
            };

            a.Execute(context);

            string[] expectedLog = new string[] { "1 Before", "2 Before", "3 Before", "M", "3 After", "2 After", "1 After" };
            Assert.AreEqual(String.Join(";", expectedLog), String.Join(";", this.log), "calls are not in order");
        }

        [TestMethod]
        public void AdvisorBeforeException()
        {
            this.log = new List<string>();
            List<IAdvice> advices = new List<IAdvice>()
            {
                new TestAdvice()
                {
                    Id = "1",
                    ThrowAfter = false,
                    ThrowBefore = false,
                    ExecuteAfterIfExcept = false,
                    Log = log
                },
                new TestAdvice()
                {
                    Id = "2",
                    ThrowAfter = false,
                    ThrowBefore = true,
                    ExecuteAfterIfExcept = false,
                    Log = log
                },
                new TestAdvice()
                {
                    Id = "3",
                    ThrowAfter = false,
                    ThrowBefore = false,
                    ExecuteAfterIfExcept = false,
                    Log = log
                },
            };
            IPointcut pointcut = new TrueFalsePointcut() { Match = true };
            IList<IPointcut> pointcuts = new IPointcut[] { pointcut };
            Advisor a = new Advisor(new Aspect[] 
                {
                    new Aspect(){Pointcuts = pointcuts, Advice = advices[0]},
                    new Aspect(){Pointcuts = pointcuts, Advice = advices[1]},
                    new Aspect(){Pointcuts = pointcuts, Advice = advices[2]},
                });

            bool exceptionThrown;
            try
            {
                IAspectInvocationContext context = new AspectInvocationContext()
                {
                    Action = c => c.ReturnValue = c.Method.Invoke(c.Target, c.Arguments),
                    Target = this,
                    Method = this.GetType().GetMethod("AOPMethod"),
                    Arguments = null
                };

                a.Execute(context);
                exceptionThrown = false;
            }
            catch (ApplicationException ex)
            {
                Assert.AreEqual(Exceptions.GetRealException(ex).Message, "2 Before");
                exceptionThrown = true;
            }
            Assert.IsTrue(exceptionThrown, "Exception not thrown");

            string[] expectedLog = new string[] { "1 Before", "2 Before", "3 Before" };
            Assert.AreEqual(String.Join(";", expectedLog), String.Join(";", this.log), "calls are not in order");
        }
        [TestMethod]
        public void AdvisorBeforeException2()
        {
            this.log = new List<string>();
            List<IAdvice> advices = new List<IAdvice>()
            {
                new TestAdvice()
                {
                    Id = "1",
                    ThrowAfter = false,
                    ThrowBefore = false,
                    ExecuteAfterIfExcept = false,
                    Log = log
                },
                new TestAdvice()
                {
                    Id = "2",
                    ThrowAfter = false,
                    ThrowBefore = true,
                    ExecuteAfterIfExcept = false,
                    Log = log
                },
                new TestAdvice()
                {
                    Id = "3",
                    ThrowAfter = false,
                    ThrowBefore = false,
                    ExecuteAfterIfExcept = true,
                    Log = log
                },
            };

            IPointcut pointcut = new TrueFalsePointcut() { Match = true };
            IList<IPointcut> pointcuts = new IPointcut[] { pointcut };
            Advisor a = new Advisor(new Aspect[] 
                {
                    new Aspect(){Pointcuts = pointcuts, Advice = advices[0]},
                    new Aspect(){Pointcuts = pointcuts, Advice = advices[1]},
                    new Aspect(){Pointcuts = pointcuts, Advice = advices[2]},
                });
            bool exceptionThrown;
            try
            {
                IAspectInvocationContext context = new AspectInvocationContext()
                {
                    Action = c => c.ReturnValue = c.Method.Invoke(c.Target, c.Arguments),
                    Target = this,
                    Method = this.GetType().GetMethod("AOPMethod"),
                    Arguments = null
                };

                a.Execute(context);
                exceptionThrown = false;
            }
            catch (ApplicationException ex)
            {
                Assert.AreEqual(Exceptions.GetRealException(ex).Message, "2 Before");
                exceptionThrown = true;
            }
            Assert.IsTrue(exceptionThrown, "Exception not thrown");

            string[] expectedLog = new string[] { "1 Before", "2 Before", "3 Before", "3 After"};
            Assert.AreEqual(String.Join(";", expectedLog), String.Join(";", this.log), "calls are not in order");
        }
    }
}
