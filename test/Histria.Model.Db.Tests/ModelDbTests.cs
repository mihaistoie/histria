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
            DbTable ct = schema.Tables[typeof(CountryClass).Name];
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
            Assert.AreEqual(true, false, "Not Implemented");
            //max length , required
        }
        [TestMethod]
        public void IntegerColumns()
        {
            //int, bigint ... short
            Assert.AreEqual(true, false, "Not Implemented");
        }
        [TestMethod]
        public void EnumColumns()
        {
            // enum as smallint
            // enum as string
            Assert.AreEqual(true, false, "Not Implemented");
        }

        [TestMethod]
        public void NumbersColumns()
        {
        }

        [TestMethod]
        public void DateTimeColumns()
        {
            Assert.AreEqual(true, false, "Not Implemented");
        }

        [TestMethod]
        public void NullablesColumns()
        {
        }

        [TestMethod]
        public void ConstraintsColumns()
        {
            //required ...
            Assert.AreEqual(true, false, "Not Implemented");
        }

        [TestMethod]
        public void CompositeTypesColumns()
        {
            //binary
            //memo
            Assert.AreEqual(true, false, "Not Implemented");
        }


        [TestMethod]
        public void ForeingKeys()
        {

            //composition by uuid

            DbTable ff = schema.Tables[typeof(SteeringWheel).Name];
            Assert.AreEqual(1, ff.ForeignKeys.Count, "One FK");
            Assert.AreEqual(1, ff.ForeignKeys[0].Columns.Count, "FK One field");
            Assert.AreEqual("uidCar", ff.ForeignKeys[0].Columns[0].ColumnName, true, "FK Column name");
            Assert.AreEqual("uuid", ff.ForeignKeys[0].Columns[0].UniqueColumnName, true, "FK Column name");

            //composition by uuid 
            ff = schema.Tables[typeof(Engine).Name];
            Assert.AreEqual(1, ff.ForeignKeys.Count, "One FK");
            Assert.AreEqual(1, ff.ForeignKeys[0].Columns.Count, "FK One field");
            Assert.AreEqual("car", ff.ForeignKeys[0].Columns[0].ColumnName, true, "FK Column name");
            Assert.AreEqual("uuid", ff.ForeignKeys[0].Columns[0].UniqueColumnName, true, "FK Column name");

            //association by uid
            ff = schema.Tables[typeof(AccountingEntry).Name];
            Assert.AreEqual(1, ff.ForeignKeys.Count, "One FK");
            Assert.AreEqual(1, ff.ForeignKeys[0].Columns.Count, "FK One field");
            Assert.AreEqual("uidAccount", ff.ForeignKeys[0].Columns[0].ColumnName, true, "FK Column name");
            Assert.AreEqual("uuid", ff.ForeignKeys[0].Columns[0].UniqueColumnName, true, "FK Column name");

            ff = schema.Tables[typeof(Car).Name];
            Assert.AreEqual(0, ff.ForeignKeys.Count, "No FKs");
           
            //association by code

            ff = schema.Tables[typeof(Address).Name];
            Assert.AreEqual(2, ff.ForeignKeys.Count, "One FK");
            DbFk dfkCity = ff.ForeignKeys[0];
            DbFk dfkCountry = ff.ForeignKeys[1];
            if (string.Compare(dfkCity.UniqueTableName, typeof(Country).Name, true) == 0)
            {
                dfkCity = ff.ForeignKeys[1];
                dfkCountry = ff.ForeignKeys[0];

            }
            Assert.AreEqual(1, dfkCountry.Columns.Count, "FK One field");
            Assert.AreEqual("CountryCode", dfkCity.Columns[0].ColumnName, true, "FK Column name");
            Assert.AreEqual("CountryCode", dfkCity.Columns[0].UniqueColumnName, true, "FK Column name");

            Assert.AreEqual(3, dfkCity.Columns.Count, "FK 3 field");
            Assert.AreEqual("CountryCode", dfkCity.Columns[0].ColumnName, true, "FK Column name");
            Assert.AreEqual("CountryCode", dfkCity.Columns[0].UniqueColumnName, true, "FK Column name");

            Assert.AreEqual("CityCode", dfkCity.Columns[1].ColumnName, true, "FK Column name");
            Assert.AreEqual("CityCode", dfkCity.Columns[1].UniqueColumnName, true, "FK Column name");
            Assert.AreEqual("ZipCode", dfkCity.Columns[2].ColumnName, true, "FK Column name");
            Assert.AreEqual("ZipCode", dfkCity.Columns[2].UniqueColumnName, true, "FK Column name");

            //composition  by code 
            ff = schema.Tables[typeof(Finger).Name];
            Assert.AreEqual(1, ff.ForeignKeys.Count, "One FK");
            Assert.AreEqual(1, ff.ForeignKeys[0].Columns.Count, "FK One field");
            Assert.AreEqual("HandId", ff.ForeignKeys[0].Columns[0].ColumnName, true, "FK Column name");
            Assert.AreEqual("Id", ff.ForeignKeys[0].Columns[0].UniqueColumnName, true, "FK Column name");
            
            
        }

        [TestMethod]
        public void Indexes()
        {
            Assert.AreEqual(true, false, "Not Implemented");
        }

    }
}



