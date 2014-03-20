using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Histria.Sys.Tests
{
    [TestClass]
    public class SysUnitTests
    {
        [TestMethod]
        public void PropertyChangeStack()
        {
            PCStack stack = new PCStack();
            string g1 = Guid.NewGuid().ToString("N");
            string g2 = Guid.NewGuid().ToString("N");
            string g3 = Guid.NewGuid().ToString("N");
            stack.Push(string.Format("{0}.delails.{1}.UnitPrice", g1, g2));
            stack.Push(string.Format("{0}.delails.{1}.Price", g1, g2));
            stack.Push(string.Format("{0}.delails.{1}.Address.{2}.Country", g1, g2, g3));
            stack.Push(string.Format("{0}.TotalPrice", g1));

            string search = string.Format("{0}.delails.{1}.UnitPrice", g1, g2);  
            Assert.AreEqual(true, stack.Has(search, null), "Found");
            search = string.Format("{0}.delails.{1}.XXX", g1, g2);
            Assert.AreEqual(false, stack.Has(search, null), "Not Found");
            search = string.Format("{0}.delails.{1}.Address", g1, g2);
            Assert.AreEqual(false, stack.Has(search, null), "Not Found");
            
            search = string.Format("{0}.delails.{1}", g1, g2);
            Assert.AreEqual(true, stack.Has(search, "Address.*"), "Found");
            search = string.Format("{0}.delails.{1}", g1, g2);
            Assert.AreEqual(false, stack.Has(search, "Address"), "Not Found");
            search = string.Format("{0}", g1);
            Assert.AreEqual(true, stack.Has(search, "delails.Address.Country"), "Found");
            Assert.AreEqual(false, stack.Has(search, "delails.Address.City"), "NotFound");
            Assert.AreEqual(false, stack.Has(search, "delails.Address"), "NotFound");
            Assert.AreEqual(true, stack.Has(search, "delails.Address.*"), "Found");
        }
    }
}
