using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sikia.Json;
using Sikia.Model;
using Sikia.Sys;

namespace Sikia.Core.Tests.Associations
{
    [TestClass]
    public class ModelRelationships
    {
        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            string scfg = @"{""nameSpaces"": [""Associations""]}";
            JsonObject cfg = (JsonObject)JsonValue.Parse(scfg);
            ModelManager m = ModelManager.LoadModelFromConfig(cfg);
            ModulePlugIn.Load("Sikia.Proxy.Castle");
            ModulePlugIn.Initialize(m);
        }

        [TestMethod]
        public void CompositionOneToOneByCode()
        {
            HumanBody body = ProxyFactory.Create<HumanBody>();
            body.Id = "body";
            Nose nose = ProxyFactory.Create<Nose>();
            nose.Id = "nose";
            body.Nose.Value = nose;
            Assert.AreEqual(nose.BodyId, body.Id, "test update FKs");
            Assert.AreEqual(nose.Body.Value, body, "test inv role");
        }

        ///<summary>
        /// Test associations by code 
        /// The model is defined in AssociationsByCode.cs
        ///</summary> 
        [TestMethod]
        public void AssociationsByCodes()
        {
            
            // create two countries
            Country c1 = ProxyFactory.Create<Country>();
            c1.Code = "France";
            Country c2 = ProxyFactory.Create<Country>();
            c2.Code = "USA";

            // create two cities
            City paris = ProxyFactory.Create<City>();
            paris.Country.Value = c1;
            paris.CityCode = "Paris";
            paris.ZipCode = "75000";
            Assert.AreEqual(paris.CountryCode, c1.Code, "Code propagation");

            City boston = ProxyFactory.Create<City>();
            boston.CityCode = "Boston";
            boston.ZipCode = "925254";
            boston.Country.Value = c2;
            Assert.AreEqual(boston.CountryCode, c2.Code, "Code propagation");

            // create an address
            Address infrance = ProxyFactory.Create<Address>();
            infrance.Country.Value = c1;
            infrance.City.Value = paris;
            Assert.AreEqual(infrance.CountryCode, c1.Code, "Code propagation");
            Assert.AreEqual(infrance.CityCode, paris.CityCode, "Code propagation");
            Assert.AreEqual(infrance.ZipCode, paris.ZipCode, "Code propagation");
            infrance.City.Value = null;
            Assert.AreEqual(string.IsNullOrEmpty(infrance.CityCode), true, "Code propagation");
            Assert.AreEqual(string.IsNullOrEmpty(infrance.ZipCode), true, "Code propagation");


            // set city befor country 
            Address a1 = ProxyFactory.Create<Address>();
            bool doFail = false;
            try
            {
                a1.City.Value = paris;
                doFail = true;
            }
            catch
            {
                Assert.AreEqual(true, true, "Set country before setting city");
            }
            if (doFail)
                Assert.AreEqual(false, true, "Set country before setting city");
            
            // After country chnged  --> city is empty
            Address a2 = ProxyFactory.Create<Address>();
            a2.Country.Value = c1;
            a2.City.Value = paris;
            Assert.AreEqual(a2.CountryCode, c1.Code, "Code propagation");
            Assert.AreEqual(a2.CityCode, paris.CityCode, "Code propagation");
            Assert.AreEqual(a2.ZipCode, paris.ZipCode, "Code propagation");

            a2.Country.Value = c2;
            Assert.AreEqual(a2.City.Value, null, "Code propagation");
            Assert.AreEqual(a2.CountryCode, c2.Code, "Code propagation");
            Assert.AreEqual(string.IsNullOrEmpty(a2.CityCode), true, "Code propagation");
            Assert.AreEqual(string.IsNullOrEmpty(a2.ZipCode), true, "Code propagation");

        }

    }
}
