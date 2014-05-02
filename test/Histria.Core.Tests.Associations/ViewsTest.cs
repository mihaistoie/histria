using System;
using Histria.Core.Execution;
using Histria.Json;
using Histria.Model;
using Histria.Sys;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Histria.Core.Tests.Associations
{
    [TestClass]
    public class ViewsTest
    {
        private static ModelManager model;
        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            string scfg = @"{""nameSpaces"": [""Associations""]}";
            JsonObject cfg = (JsonObject)JsonValue.Parse(scfg);
            model = ModelManager.LoadModel(cfg);
            ModulePlugIn.Load("Histria.Proxy.Castle");
            ModulePlugIn.Initialize(model);
        }

        [TestMethod]
        public void SimpleViewPropertiesTest()
        {
            Container container = new Container(TestContainerSetup.GetSimpleContainerSetup(model));
            User u = null;
            UserView uv = null;
            container.Load(() =>
                {
                    u = container.Create<User>();
                    u.FirstName = "Luis";
                    u.LastName = "Martin";
                    u.Age = 30;
                    u.Email = "luis.martin@histria.org";

                    uv = container.CreateView<UserView, User>(u);
                });
            Assert.AreEqual("Luis MARTIN", uv.FullName);


            u.FirstName = "Mike";

            Assert.AreEqual("Mike MARTIN", uv.FullName);

            uv.FirstName = "Alice";
            Assert.AreEqual("Alice", u.FirstName);
            Assert.AreEqual("Alice", uv.FirstName);
            Assert.AreEqual("Alice MARTIN", uv.FullName);

            try
            {
                uv.FirstName = null;
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ApplicationException));
            }
            Assert.AreEqual(u.FirstName, "Alice");
            Assert.AreEqual(uv.FirstName, u.FirstName);

            uv.Age = 25;
            Assert.AreEqual(25, u.Age);
            Assert.AreEqual(25, uv.Age);

            container.DereferenceView(uv);
            Assert.IsTrue(uv.IsDisposing);
        }
    }
}
