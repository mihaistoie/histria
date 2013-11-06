using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Sikia.Models;
using Sikia.Framework.Attributes;
using Sikia.Framework.Model;
using Sikia.Models;

namespace Sikia
{
    class Program
    {
        static void Main(string[] args)
        {
            WindsorContainer container = new WindsorContainer();
            container.Install(FromAssembly.This());



            Customer cust = container.Resolve<Customer>();
            cust.FirstName = "John";
            cust.LastName = "Smith";


            ModelLoader.ReadAttributes(typeof(DummyClass).Namespace);
            System.Console.WriteLine("Press any key to continue");
            System.Console.ReadKey();

        }
    }
}
