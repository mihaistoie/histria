using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sikia.Json;
using Sikia.Model;
using Sikia.Sys;
using Sikia.Core.Execution;

namespace Sikia.Core.Tests.Associations
{
    [TestClass]
    public class ModelRelationships
    {
        private static ModelManager model;
        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            string scfg = @"{""nameSpaces"": [""Associations""]}";
            JsonObject cfg = (JsonObject)JsonValue.Parse(scfg);
            model = ModelManager.LoadModelFromConfig(cfg);
            ModulePlugIn.Load("Sikia.Proxy.Castle");
            ModulePlugIn.Initialize(model);
        }

        [TestMethod]
        public void CompositionOneToOneByCode()
        {
            Container container = new Container(TestContainerSetup.GetSimpleContainerSetup(model));


            HumanBody body = container.Create<HumanBody>();
            body.Id = "body";
            Nose nose = container.Create<Nose>();
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
            Container container = new Container(TestContainerSetup.GetSimpleContainerSetup(model));
           
            // create two countries
            Country c1 = container.Create<Country>();
            c1.Code = "France";
            Country c2 = container.Create<Country>();
            c2.Code = "USA";

            // create two cities
            City paris = container.Create<City>();
            paris.Country.Value = c1;
            paris.CityCode = "Paris";
            paris.ZipCode = "75000";
            Assert.AreEqual(paris.CountryCode, c1.Code, "Code propagation");

            City boston = container.Create<City>();
            boston.CityCode = "Boston";
            boston.ZipCode = "925254";
            boston.Country.Value = c2;
            Assert.AreEqual(boston.CountryCode, c2.Code, "Code propagation");

            // create an address
            Address infrance = container.Create<Address>();
            infrance.Country.Value = c1;
            infrance.City.Value = paris;
            Assert.AreEqual(infrance.CountryCode, c1.Code, "Code propagation");
            Assert.AreEqual(infrance.CityCode, paris.CityCode, "Code propagation");
            Assert.AreEqual(infrance.ZipCode, paris.ZipCode, "Code propagation");
            infrance.City.Value = null;
            Assert.AreEqual(string.IsNullOrEmpty(infrance.CityCode), true, "Code propagation");
            Assert.AreEqual(string.IsNullOrEmpty(infrance.ZipCode), true, "Code propagation");


            // set city before country 
            Address a1 = container.Create<Address>();
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
            
            // After country changed  --> city is empty
            Address a2 = container.Create<Address>();
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

        ///<summary>
        /// Test  Compositions by uuids 
        /// The model is defined in CompositionsByUids.cs
        ///</summary> 
        [TestMethod]
        public void CompositionsByUuis()
        {
            Container container = new Container(TestContainerSetup.GetSimpleContainerSetup(model));

            // create two countries
            Car car = container.Create<Car>();
            car.Name = "Renault";
            SteeringWheel wheel = container.Create<SteeringWheel>();
            wheel.SerialNumber = "123456789";
            car.SteeringWheel.Value = wheel;
            // TODO
            Assert.AreEqual(wheel.Car.Value, car, "test inv role");
            // TODO
            // Assert.AreEqual(wheel.Car.RefUid, car.uid, "test inv role"); ????
            //
            //

        }

    }
}
