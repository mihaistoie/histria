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
        public void TableFound()
        {
            //simple table
            Assert.AreEqual(true, schema.ContainsTable(typeof(Fruit).Name), "Table found");
            //derived classes with single table
            //derived classes with more tables
        }
        [TestMethod]
        public void Primarykeys()
        {
            //Assert.AreEqual(true, schema.ContainsTable(typeof(Fruit).Name), "Table found");
            //uid
            //code
            //multi key
        }
        [TestMethod]
        public void StringColumns()
        {
            //max length , requred
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



