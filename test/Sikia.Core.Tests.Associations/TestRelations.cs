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
        public void OneToOneCompositionByCodes()
        {
            //test body.Nose.Value = nose;
            Container container = new Container(TestContainerSetup.GetSimpleContainerSetup(model));


            HumanBody body = container.Create<HumanBody>();
            body.Id = "body";
            Nose nose = container.Create<Nose>();
            nose.Id = "nose";
            body.Nose.Value = nose;

            Assert.AreEqual(nose.BodyId, body.Id, "test update FKs");
            Assert.AreEqual(body.Nose.Value, nose, "test direct role");
            Assert.AreEqual(nose.Body.Value, body, "test inv role");
            Assert.AreEqual(false, Guid.Empty == nose.Uuid, "has uuid");
            Assert.AreEqual(nose.Body.RefUid, body.Uuid, "uid");
            Assert.AreEqual(body.Nose.RefUid, nose.Uuid, "uid");
            Assert.AreEqual(nose.Id, body.CurrentNoseId, "Rule prppagation");
            Assert.AreEqual(body.Id, nose.CurrentBodyId, "Rule prppagation");


            body.Nose.Value = null;
            //nose is marked as deleted
            Assert.AreEqual(ObjectState.Deleting, nose.State & ObjectState.Deleting, "uid");
            Assert.AreEqual(nose.Body.RefUid, body.Uuid, "uid");
            Assert.AreEqual(nose.BodyId, body.Id, "test update FKs");
            Assert.AreEqual(body.Nose.RefUid, Guid.Empty, "uid");
            Assert.AreEqual(body.Nose.Value, null, "test direct role");
            Assert.AreEqual(nose.Body.Value, null, "test inv role");
            Assert.AreEqual(true, string.IsNullOrEmpty(body.CurrentNoseId), "Rule prppagation");
            //false !!!! nose is marked as deleted 
            Assert.AreEqual(false, string.IsNullOrEmpty(nose.CurrentBodyId), "Rule prppagation");

            //test nose.Body.Value = body;
            HumanBody body1 = container.Create<HumanBody>();
            body1.Id = "body1";
            Nose nose1 = container.Create<Nose>();
            nose1.Id = "nose1";
            nose1.Body.Value = body1;
            Assert.AreEqual(nose1.BodyId, body1.Id, "test update FKs");
            Assert.AreEqual(body1.Nose.Value, nose1, "test direct role");
            Assert.AreEqual(nose1.Body.Value, body1, "test inv role");
            Assert.AreEqual(false, Guid.Empty == nose1.Uuid, "has uuid");
            Assert.AreEqual(nose1.Body.RefUid, body1.Uuid, "uid");
            Assert.AreEqual(body1.Nose.RefUid, nose1.Uuid, "uid");
            Assert.AreEqual(nose1.Id, body1.CurrentNoseId, "Rule prppagation");
            Assert.AreEqual(body1.Id, nose1.CurrentBodyId, "Rule prppagation");


            nose1.Body.Value = null;
            Assert.AreEqual(ObjectState.Deleting, nose1.State & ObjectState.Deleting, "uid");
            Assert.AreEqual(nose1.Body.RefUid, body1.Uuid, "uid");
            Assert.AreEqual(nose1.BodyId, body1.Id, "test update FKs");
            Assert.AreEqual(body1.Nose.RefUid, Guid.Empty, "uid");
            Assert.AreEqual(body1.Nose.Value, null, "test direct role");
            Assert.AreEqual(nose1.Body.Value, null, "test inv role");
            Assert.AreEqual(true, string.IsNullOrEmpty(body1.CurrentNoseId), "Rule prppagation");
            //false !!!! nose is marked as deleted 
            Assert.AreEqual(false, string.IsNullOrEmpty(nose1.CurrentBodyId), "Rule prppagation");

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
            Assert.AreEqual(c1.Uuid, paris.Country.RefUid, "uid");

            City boston = container.Create<City>();
            boston.CityCode = "Boston";
            boston.ZipCode = "925254";
            boston.Country.Value = c2;
            Assert.AreEqual(boston.CountryCode, c2.Code, "Code propagation");

            // create an address
            Address infrance = container.Create<Address>();
            Assert.AreEqual(string.IsNullOrEmpty(infrance.CountryCity), true, "Code propagation");
            infrance.Country.Value = c1;
            Assert.AreEqual(infrance.CountryCity, c1.Code + "-", "Refchange propagation");
            infrance.City.Value = paris;
            Assert.AreEqual(infrance.CountryCity, c1.Code + "-" + paris.CityCode, "Refchange propagation");

            Assert.AreEqual(infrance.CountryCode, c1.Code, "Code propagation");
            Assert.AreEqual(infrance.CityCode, paris.CityCode, "Code propagation");
            Assert.AreEqual(infrance.ZipCode, paris.ZipCode, "Code propagation");
            Assert.AreEqual(paris.Uuid, infrance.City.RefUid, "uid");

            infrance.City.Value = null;
            Assert.AreEqual(true, string.IsNullOrEmpty(infrance.CityCode), "Code propagation");
            Assert.AreEqual(true, string.IsNullOrEmpty(infrance.ZipCode), "Code propagation");
            Assert.AreEqual(Guid.Empty, infrance.City.RefUid, "uid");


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
        /// Test associations by uid's 
        /// The model is defined in AssociationsByUids.cs
        ///</summary> 
        [TestMethod]
        public void AssociationsByUids()
        {
            Container container = new Container(TestContainerSetup.GetSimpleContainerSetup(model));

            // create two countries
            Account a1 = container.Create<Account>();
            a1.Code = "101";
            Account a2 = container.Create<Account>();
            a1.Code = "102";
            AccountingEntry e1 = container.Create<AccountingEntry>();
            e1.Account.Value = a1;
            Assert.AreEqual(a1.Uuid, e1.Account.RefUid, "uid");
            Assert.AreEqual(a1.Code, e1.AccountCode, "Rule prop");
            e1.Account.Value = a2;
            Assert.AreEqual(a2.Uuid, e1.Account.RefUid, "uid");
            Assert.AreEqual(a2.Code, e1.AccountCode, "Rule prop");

            e1.Account.Value = null;
            Assert.AreEqual(Guid.Empty, e1.Account.RefUid, "uid");
            Assert.AreEqual(true, string.IsNullOrEmpty(e1.AccountCode), "Rule prop");

        }


        ///<summary>
        /// Test  Compositions by uuids 
        /// The model is defined in CompositionsByUids.cs
        ///</summary> 
        [TestMethod]
        public void OneToOneCompositionByUids()
        {
            Container container = new Container(TestContainerSetup.GetSimpleContainerSetup(model));

            // create two countries
            Car car = container.Create<Car>();
            car.Name = "Renault";
            SteeringWheel wheel = container.Create<SteeringWheel>();
            wheel.SerialNumber = "123456789";
            car.SteeringWheel.Value = wheel;

            Assert.AreEqual(car, wheel.Car.Value, "test inv role");
            Assert.AreNotEqual(wheel.Car.RefUid, Guid.Empty, "test inv role uid");
            Assert.AreEqual(wheel.Car.RefUid, car.Uuid, "test inv role uid");
            Assert.AreEqual(car.SteeringWheel.RefUid, wheel.Uuid, "test inv role uid");

            // create two countries
            Car ford = container.Create<Car>();
            ford.Name = "Ford";
            SteeringWheel wheelf = container.Create<SteeringWheel>();
            wheelf.SerialNumber = "123456789";
            wheelf.Car.Value = ford;

            Assert.AreEqual(wheelf, ford.SteeringWheel.Value, "test inv role");
            Assert.AreNotEqual(Guid.Empty, wheelf.Car.RefUid, "test inv role uid");
            Assert.AreEqual(ford.Uuid, wheelf.Car.RefUid, "test inv role uid");
            Assert.AreEqual(wheelf.Uuid, ford.SteeringWheel.RefUid, "test inv role uid");


        }


        ///<summary>
        /// Test  Compositions by uuids 
        /// The model is defined in OneToManyCompositions.cs
        ///</summary> 
        [TestMethod]
        public void OneToManyCompositionByCodes()
        {
            Container container = new Container(TestContainerSetup.GetSimpleContainerSetup(model));
            HBody body = container.Create<HBody>();
            body.Id = "kirilov";
            Assert.AreEqual(0, body.HandsCount, "Rule After create");

            Hand leftHand = container.Create<Hand>();
            leftHand.Id = "left";
            Assert.AreEqual(null, leftHand.BodyName, "Initial value");
            body.Hands.Add(leftHand);

            Assert.AreEqual(1, body.Hands.Count, "Kirilov has 2 hands");
            Assert.AreEqual(body.Id, leftHand.BodyId, "test update FKs");
            Assert.AreEqual(body, leftHand.Body.Value, "test direct role");
            Assert.AreEqual(body.Uuid, leftHand.Body.RefUid, "uid");
            Assert.AreEqual(1, body.HandsCount, "Rule Add/rmv");
            Assert.AreEqual(body.Id, leftHand.BodyName, "Rule Body Change");



            Hand rightHand = container.Create<Hand>();
            rightHand.Id = "right";
            Assert.AreEqual(null, rightHand.BodyName, "Initial value");
            rightHand.Body.Value = body;


            Assert.AreEqual(true, body.Hands.Has(rightHand), "Kirilov has right hand ");
            Assert.AreEqual(2, body.Hands.Count, "Kirilov has 2 hands");
            Assert.AreEqual(body.Id, rightHand.BodyId, "test update FKs");
            Assert.AreEqual(body, rightHand.Body.Value, "test direct role");
            Assert.AreEqual(rightHand.Body.RefUid, body.Uuid, "uid");
            Assert.AreEqual(2, body.HandsCount, "Rule Add/rmv");
            Assert.AreEqual(body.Id, rightHand.BodyName, "Rule Body Change");

            //cut the right hand  
            rightHand.Body.Value = null;
            Assert.AreEqual(ObjectState.Deleting, rightHand.State & ObjectState.Deleting, "deleted");
            Assert.AreEqual(false, body.Hands.Has(rightHand), "Kirilov don't have the right hand");
            Assert.AreEqual(body.Hands.Count, 1, "Kirilov has only 1 hands");
            Assert.AreEqual(null, rightHand.Body.Value, "test direct role");
            Assert.AreEqual(body.Uuid, rightHand.Body.RefUid, "uid");
            Assert.AreEqual(1, body.HandsCount, "Rule Add/rmv");
            Assert.AreEqual(body.Id, rightHand.BodyName, "Rule Body Change");


            rightHand = container.Create<Hand>();
            rightHand.Id = "right";
            Assert.AreEqual(null, rightHand.BodyName, "Initial value");
            rightHand.Body.Value = body;
            Assert.AreEqual(2, body.Hands.Count, "Kirilov has 2 hands");
            Assert.AreEqual(true, body.Hands.Has(rightHand), "Kirilov has right hand ");
            Assert.AreEqual(body.Id, rightHand.BodyId, "test update FKs");
            Assert.AreEqual(body, rightHand.Body.Value, "test direct role");
            Assert.AreEqual(rightHand.Body.RefUid, body.Uuid, "uid");
            Assert.AreEqual(2, body.HandsCount, "Rule Add/rmv");
            Assert.AreEqual(body.Id, rightHand.BodyName, "Rule Body Change");

            //cut the right hand  
            body.Hands.Remove(rightHand);
            Assert.AreEqual(ObjectState.Deleting, rightHand.State & ObjectState.Deleting, "deleted");
            Assert.AreEqual(false, body.Hands.Has(rightHand), "Kirilov don't have the right hand");
            Assert.AreEqual(body.Hands.Count, 1, "Kirilov has only 1 hands");
            Assert.AreEqual(null, rightHand.Body.Value, "test direct role");
            Assert.AreEqual(body.Uuid, rightHand.Body.RefUid, "uid");
            Assert.AreEqual(1, body.HandsCount, "Rule Add/rmv");
            Assert.AreEqual(body.Id, rightHand.BodyName, "Rule Body Change");

        }

    }
}
