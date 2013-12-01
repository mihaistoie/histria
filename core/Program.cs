using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Sikia.Models;
using Sikia.Framework.Attributes;
using Sikia.Aop;

namespace Sikia
{
    class Program
    {
        static void Main(string[] args)
        {
            Sikia.Application.GlbApplicaton.Start();


            Customer cust = ModelFactory.Create<Customer>();
            
            cust.FirstName = "John";
            cust.LastName = "Smith";
            System.Console.WriteLine(cust.FullName);

            RussianCustomer rcust = ModelFactory.Create<RussianCustomer>();

            rcust.FirstName = "Fiodor";
            rcust.LastName = "Dostoïevski";
            rcust.MiddleName = "A."; 
            System.Console.WriteLine(rcust.FullName);
            

            
            //ModelLoader.ReadAttributes(typeof(DummyClass).Namespace);
           
            System.Console.WriteLine("Press any key to continue");
            System.Console.ReadKey();

        }
    }
}
