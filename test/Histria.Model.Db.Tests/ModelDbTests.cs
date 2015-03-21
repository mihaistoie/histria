using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Histria.Json;
using Histria.Sys;
using Histria.Db;
using Histria.Db.Model;
using Histria.DbModel.Tests;

namespace Histria.Model.Db.Tests
{
    [TestClass]
    public class ModelDbTests
    {

        private static ModelManager model;
        private static DbSchema schema;
        //static string sqlServerInstance = "(local)\\SQLEXPRESS";

        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            JsonObject cfg = (JsonObject)JsonValue.Parse(@"{""nameSpaces"": [""DbModel""]}");
            model = ModelManager.LoadModel(cfg);
            ModulePlugIn.Load("Histria.Db.MsSql");
            ModulePlugIn.Initialize(null);
            schema = model.Schema(DbProtocol.mssql);
        }

        [TestMethod]
        public void DbCollectionNames()
        {
            //simple table
            Assert.AreEqual(true, schema.ContainsTable(typeof(Fruit).Name), "Table found");
            //derived classes with single table
            Assert.AreEqual(true, schema.ContainsTable(typeof(Shape).Name), "Table found");
            Assert.AreEqual(model.Classes[typeof(Shape)].DbName, model.Classes[typeof(Rectangle)].DbName, "Same table name");
            Assert.AreEqual(model.Classes[typeof(Shape)].DbName, model.Classes[typeof(Circle)].DbName, "Same table name");
            Assert.AreEqual(true, schema.ContainsTable(typeof(Shape).Name), "Table not found");
            //derived classes with more tables
            Assert.AreEqual(false, schema.ContainsTable(typeof(Animal).Name), "Table not found");
            Assert.AreEqual(true, schema.ContainsTable(typeof(Mammal).Name), "Table not found");
            Assert.AreNotEqual(model.Classes[typeof(Mammal)].DbName, model.Classes[typeof(Bird)].DbName, "Different table name");
            
        }
         
        [TestMethod]
        public void PrimaryKeys()
        {
            //uid
            DbTable ff = schema.Tables[typeof(Fruit).Name];
            Assert.AreEqual(1, ff.PK.Count, "PK one field");
            Assert.AreEqual("uuid", ff.PK[0], true, "PK uuid");

            //code
            DbTable ct = schema.Tables[typeof(Country).Name];
            Assert.AreEqual(1, ct.PK.Count, "PK one field");
            Assert.AreEqual("codeISO", ct.PK[0], true, "PK codeISO");
         
            //multi key
            ct = schema.Tables[typeof(Color).Name];
            Assert.AreEqual(3, ct.PK.Count, "PK 3 field");
            Assert.AreEqual("Red", ct.PK[0], true, "PK Red");
            Assert.AreEqual("Green", ct.PK[1], true, "PK Red");
            Assert.AreEqual("Blue", ct.PK[2], true, "PK Red");
        }
        [TestMethod]
        public void StringColumns()
        {
            //
            //max length , required
        }
        [TestMethod]
        public void IntegerColumns()
        {
            //int, bigint ... short
        }
        [TestMethod]
        public void EnumColumns()
        {
            // enum as smallint
            // enum as string
        }

        [TestMethod]
        public void NumbersColumns()
        {
        }

        [TestMethod]
        public void DateTimeColumns()
        {
        }

        [TestMethod]
        public void NullablesColumns()
        {
        }

        [TestMethod]
        public void ConstraintsColumns()
        {
            //required ...
        }

        [TestMethod]
        public void CompositeTypesColumns()
        {
            //binary
            //memo
        }


        [TestMethod]
        public void ForeingKeys()
        {
            //multiple
        }


        [TestMethod]
        public void Indexes()
        {
        }

    }
}



