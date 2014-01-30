﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sikia.Json;
using Sikia.Db;
using Sikia.Db.Model;
using Sikia.Db.SqlServer;
using System.Text;
using Sikia.Sys;


namespace Sikia.Db.Tests
{
    [TestClass]
    public class DatabaseTests
    {
        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            ModulePlugIn.Load("Sikia.Db.MsSql");

        }

        [TestMethod]
        public void MsSqlUrlParser()   
        {
            string dburl = "mssql://(local)\\SQLEXPRESS/master?schema=dbo";
            DbConnectionInfo ci = DbDrivers.Instance.Connection(dburl);
            Assert.AreEqual("(local)\\SQLEXPRESS", ci.ServerAddress, "Parse Server part");
            Assert.AreEqual("master", ci.DatabaseName, "Parse Database part");
            MsSqlConnectionInfo ms = (MsSqlConnectionInfo)ci;
            Assert.AreEqual("dbo", ms.Schema, "Parse schema part");
        }

        [TestMethod]
        public void MsSqlConnection()
        {
            string dburl = "mssql://(local)\\SQLEXPRESS/master?schema=dbo";
            using (DbSession ss = DbDrivers.Instance.Session(dburl))
            {
                DbCmd cmd = ss.Command("select 1");
                int res = (int)cmd.ExecuteScalar();
                Assert.AreEqual(1, res, "ExecuteScalar");
            }

        }
        [TestMethod]
        public void MsSqlDatabases()
        {
            string dburl = "mssql://(local)\\SQLEXPRESS/xyzgrls?schema=dbo";
            DbSchema ss = DbDrivers.Instance.Schema(DbServices.Url2Protocol(dburl));
            if (ss.DatabaseExists(dburl))
            {
                ss.DropDatabase(dburl);
            }
            ss.CreateDatabase(dburl);
            DropDB(dburl, ss);

        }
        [TestMethod]
        public void MsSqlTables()
        {
            string dburl = "mssql://(local)\\SQLEXPRESS/msergrls?schema=dbo";
            DbSchema ss = CreateDB(dburl);
            using (DbSession session = DbDrivers.Instance.Session(dburl))
            {
                using (DbCmd cmd = session.Command(""))
                {
                    cmd.Sql = "create table a1 (a integer not null)";
                    cmd.Execute();
                    cmd.Sql = "create table B1 (a integer not null)";
                    cmd.Execute();
                }
            }
            //Test load structure
            ss.Load(dburl);
            Assert.AreEqual(true, ss.ContainsTable("a1"), "Table exists");
            Assert.AreEqual(true, ss.ContainsTable("B1"), "Table exists");
            var structure =  ss.CreateSQL();
            DropDB(dburl, ss);
            //Test create script
            CreateDB(dburl);
            ss.ExecuteSchemaScript(dburl, structure);
            ss.Load(dburl);
            Assert.AreEqual(true, ss.ContainsTable("a1"), "Table exists");
            Assert.AreEqual(true, ss.ContainsTable("B1"), "Table exists");
            DropDB(dburl, ss);

        }
        void checkColumn(DbTable table, string name, DbType dt, string dbtype, int size, int prec, int scale, bool nullable)
        {
            Assert.AreEqual(true, table.ContainsColumn(name), "Column exists");
            DbColumn column = table.ColumnByName(name);

            if (column != null)
            {
                Assert.AreEqual(dt, column.Type, "column type");
                Assert.AreEqual(dbtype, column.DbType, "column sql type");
                Assert.AreEqual(size, column.Size, "column length");
                Assert.AreEqual(nullable, column.Nullable, "column nullable");
                if ((dbtype != "int") && (dbtype != "bigint") && (dbtype != "smallint") && (dbtype != "tinyint") && (dbtype != "money"))
                {
                    Assert.AreEqual(prec, column.Precision, "column precision");
                    Assert.AreEqual(scale, column.Scale, "column scale");
                }

            } else
                Assert.AreEqual(true, false, "Column not found");
        }

        [TestMethod]
        public void MsSqlColumns()
        {
            string dburl = "mssql://(local)\\SQLEXPRESS/slergrls?schema=dbo";
            DbSchema ss = CreateDB(dburl);
            using (DbSession session = DbDrivers.Instance.Session(dburl))
            {
                using (DbCmd cmd = session.Command(""))
                {
                    var sql = new StringBuilder();
                    sql.AppendLine("CREATE TABLE SUPPORTEDTYPES(");
                    sql.AppendLine("a int default 0,");
                    sql.AppendLine("b bigint default 0,");
                    sql.AppendLine("c tinyint default 0,");
                    sql.AppendLine("d smallint default 0,");
                    sql.AppendLine("e decimal (11,4),");
                    sql.AppendLine("f decimal (13,2),");
                    sql.AppendLine("g money default 0,");
                    sql.AppendLine("h varchar(21) not null,");
                    sql.AppendLine("k nvarchar(21),");
                    sql.AppendLine("l uniqueidentifier,");
                    sql.AppendLine("m datetime default CURRENT_TIMESTAMP,");
                    sql.AppendLine("n date,");
                    sql.AppendLine("p time,");
                    sql.AppendLine("r varchar(max),");
                    sql.AppendLine("s bit default 0,");
                    sql.AppendLine("t varbinary(max),");
                    sql.AppendLine("u varchar(max)");
                    sql.AppendLine(")");

                    cmd.Sql = sql.ToString();
                    cmd.Execute();
                }
            }
            ss.Load(dburl);
            Assert.AreEqual(true, ss.ContainsTable("SUPPORTEDTYPES"), "Table exists");
            DbTable table = ss.TableByName("SUPPORTEDTYPES");
            if (table != null)
            {
                checkColumn(table, "a", DbType.Int, "int", 0, 0, 0, true);
                checkColumn(table, "b", DbType.BigInt, "bigint", 0, 0, 0, true);
                checkColumn(table, "c", DbType.Enum, "tinyint", 0, 0, 0, true);
                checkColumn(table, "d", DbType.SmallInt, "smallint", 0, 0, 0, true);
                checkColumn(table, "e", DbType.Number, "decimal", 0, 11, 4, true);
                checkColumn(table, "f", DbType.Number, "decimal", 0, 13, 2, true);
                checkColumn(table, "g", DbType.Currency, "money", 0, 0, 0, true);
                checkColumn(table, "h", DbType.String, "varchar", 21, 0, 0, false);
                checkColumn(table, "k", DbType.String, "nvarchar", 21, 0, 0, true);
                checkColumn(table, "l", DbType.uuid, "uniqueidentifier", 0, 0, 0, true);
                checkColumn(table, "m", DbType.DateTime, "datetime", 0, 0, 0, true);
                checkColumn(table, "n", DbType.Date, "date", 0, 0, 0, true);
                checkColumn(table, "p", DbType.Time, "time", 0, 0, 0, true);
                checkColumn(table, "r", DbType.Memo, "varchar", -1, 0, 0, true);
                checkColumn(table, "s", DbType.Bool, "bit", 0, 0, 0, true);
                checkColumn(table, "t", DbType.Binary, "varbinary", -1, 0, 0, true);
                checkColumn(table, "u", DbType.Memo, "varchar", -1, 0, 0, true);
            }
            ss.CheckModel();
            var structure = ss.CreateSQL();
            DropDB(dburl, ss);
            //Test create script
            CreateDB(dburl);
            ss.ExecuteSchemaScript(dburl, structure);
            ss.Load(dburl);
            Assert.AreEqual(true, ss.ContainsTable("SUPPORTEDTYPES"), "Table exists");
            table = ss.TableByName("SUPPORTEDTYPES");
            if (table != null)
            {
                checkColumn(table, "a", DbType.Int, "int", 0, 0, 0, true);
                checkColumn(table, "b", DbType.BigInt, "bigint", 0, 0, 0, true);
                checkColumn(table, "c", DbType.Enum, "tinyint", 0, 0, 0, true);
                checkColumn(table, "d", DbType.SmallInt, "smallint", 0, 0, 0, true);
                checkColumn(table, "e", DbType.Number, "decimal", 0, 11, 4, true);
                checkColumn(table, "f", DbType.Number, "decimal", 0, 13, 2, true);
                checkColumn(table, "g", DbType.Currency, "money", 0, 0, 0, true);
                checkColumn(table, "h", DbType.String, "varchar", 21, 0, 0, false);
                checkColumn(table, "k", DbType.String, "nvarchar", 21, 0, 0, true);
                checkColumn(table, "l", DbType.uuid, "uniqueidentifier", 0, 0, 0, true);
                checkColumn(table, "m", DbType.DateTime, "datetime", 0, 0, 0, true);
                checkColumn(table, "n", DbType.Date, "date", 0, 0, 0, true);
                checkColumn(table, "p", DbType.Time, "time", 0, 0, 0, true);
                checkColumn(table, "r", DbType.Memo, "varchar", -1, 0, 0, true);
                checkColumn(table, "s", DbType.Bool, "bit", 0, 0, 0, true);
                checkColumn(table, "t", DbType.Binary, "varbinary", -1, 0, 0, true);
                checkColumn(table, "u", DbType.Memo, "varchar", -1, 0, 0, true);
            } 
            DropDB(dburl, ss);

        }

        [TestMethod]
        public void MsSqlPrimarykeys()
        {
            string dburl = "mssql://(local)\\SQLEXPRESS/slfsrgrls?schema=dbo";
            DbSchema ss = CreateDB(dburl);
            using (DbSession session = DbDrivers.Instance.Session(dburl))
            {
                using (DbCmd cmd = session.Command(""))
                {
                    var sql = new StringBuilder();
                    sql.AppendLine("CREATE TABLE PKTEST(");
                    sql.AppendLine("a int not null,");
                    sql.AppendLine("code varchar(10) not null,");
                    sql.AppendLine("title varchar(100) not null,");
                    sql.AppendLine("PRIMARY KEY(code, a)");

                    sql.AppendLine(")");

                    cmd.Sql = sql.ToString();
                    cmd.Execute();
                }
            }
            ss.Load(dburl);
            Assert.AreEqual(true, ss.ContainsTable("PKTEST"), "Table exists");
            DbTable table = ss.TableByName("PKTEST");
            if (table != null)
            {
                Assert.AreEqual(2, table.PK.Count, "Number of fields in PK");
                Assert.AreEqual(1, table.PK.IndexOf("a"), "Fields order in PK");
                Assert.AreEqual(0, table.PK.IndexOf("code"), "Fields order in PK");

            }
            ss.CheckModel();
            var structure = ss.CreateSQL();
            DropDB(dburl, ss);
           
            //Test create script
            CreateDB(dburl);
            ss.ExecuteSchemaScript(dburl, structure);
            ss.Load(dburl);
            Assert.AreEqual(true, ss.ContainsTable("PKTEST"), "Table exists");
            table = ss.TableByName("PKTEST");
            if (table != null)
            {
                Assert.AreEqual(2, table.PK.Count, "Number of fields in PK");
                Assert.AreEqual(1, table.PK.IndexOf("a"), "Fields order in PK");
                Assert.AreEqual(0, table.PK.IndexOf("code"), "Fields order in PK");

            }
            DropDB(dburl, ss);
 
        }

        private DbSchema CreateDB(string dburl)
        {
            DbSchema ss = DbDrivers.Instance.Schema(DbServices.Url2Protocol(dburl));
            if (ss.DatabaseExists(dburl))
            {
                ss.DropDatabase(dburl);
            }
            ss.CreateDatabase(dburl);
            Assert.AreEqual(true, ss.DatabaseExists(dburl), "DatabaseExists");
            return ss;
        }

        private void DropDB(string dburl, DbSchema ss)
        {
            ss.DropDatabase(dburl);
            Assert.AreEqual(false, ss.DatabaseExists(dburl), "DatabaseExists");
        }

        [TestMethod]
        public void MsSqlIndexes()
        {
            string dburl = "mssql://(local)\\SQLEXPRESS/slfsrgrls?schema=dbo";
            DbSchema ss = CreateDB(dburl);
            using (DbSession session = DbDrivers.Instance.Session(dburl))
            {
                using (DbCmd cmd = session.Command(""))
                {
                    var sql = new StringBuilder();
                    sql.AppendLine("CREATE TABLE INDEXTEST1(");
                    sql.AppendLine("a int not null,");
                    sql.AppendLine("code varchar(10) not null,");
                    sql.AppendLine("title varchar(100) not null,");
                    sql.AppendLine("description varchar(100) not null,");
                    sql.AppendLine("PRIMARY KEY(code, a)");
                    sql.AppendLine(")");
                    cmd.Sql = sql.ToString();
                    cmd.Execute();

                    cmd.Clear();
                    cmd.Sql = "CREATE UNIQUE INDEX INDEXTEST1_title_code on INDEXTEST1(title, code)";
                    cmd.Execute();


                    cmd.Clear();
                    cmd.Sql = "CREATE INDEX INDEXTEST1_title_description on INDEXTEST1(title, description desc)";
                    cmd.Execute();

                    sql.Clear();
                    sql.AppendLine("CREATE TABLE INDEXTEST2(");
                    sql.AppendLine("a int not null,");
                    sql.AppendLine("code varchar(10) not null,");
                    sql.AppendLine("title varchar(100) not null,");
                    sql.AppendLine("description varchar(100) not null,");
                    sql.AppendLine("PRIMARY KEY(code, a)");
                    sql.AppendLine(")");
                    cmd.Sql = sql.ToString();
                    cmd.Execute();

                    cmd.Clear();
                    cmd.Sql = "CREATE INDEX INDEXTEST2_title_description on INDEXTEST2(title, description)";
                    cmd.Execute();


                }
            }
            ss.Load(dburl);
            Assert.AreEqual(true, ss.ContainsTable("INDEXTEST1"), "Table exists");
            DbTable table = ss.TableByName("INDEXTEST1");
            DbIndex index = null;
            if (table != null)
            {
                Assert.AreEqual(2, table.IndexCount, "Number of indexes");
                bool hi = table.ContainsIndex("INDEXTEST1_title_description");
                Assert.AreEqual(true, hi, "Index name");
                if (hi)
                {
                    index = table.IndexByName("INDEXTEST1_title_description");
                    Assert.AreEqual(false, index.Unique, "Index unique");
                    Assert.AreEqual(2, index.ColumnCount, "Number of fields in index");
                    if (index.ColumnCount > 0)
                    {
                        Assert.AreEqual("title", index.ColumnByIndex(0).ColumnName, true, "Fields in index");
                        Assert.AreEqual(false, index.ColumnByIndex(0).Descending, "Descending");
                    }
                    if (index.ColumnCount > 1)
                    {
                        Assert.AreEqual("description", index.ColumnByIndex(1).ColumnName, true, "Fields in index");
                        Assert.AreEqual(true, index.ColumnByIndex(1).Descending, "Descending");
                    }
                }
                hi = table.ContainsIndex("INDEXTEST1_title_code");
                Assert.AreEqual(hi, true, "Index name");
                if (hi)
                {
                    index = table.IndexByName("INDEXTEST1_title_code");
                    Assert.AreEqual(true, index.Unique, "Index unique");
                    Assert.AreEqual(2, index.ColumnCount, "Number of fields in index");
                }

            }
            Assert.AreEqual(true, ss.ContainsTable("INDEXTEST2"), "Table exists");
            table = ss.TableByName("INDEXTEST2");
            if (table != null)
            {
                Assert.AreEqual(1, table.IndexCount, "Number of indexes");

            }
            ss.CheckModel();
            var structure = ss.CreateSQL();
            DropDB(dburl, ss);

            //Test create script
            CreateDB(dburl);
            ss.ExecuteSchemaScript(dburl, structure);
            ss.Load(dburl);

            Assert.AreEqual(true, ss.ContainsTable("INDEXTEST1"), "Table exists");
            index = null;
            table = ss.TableByName("INDEXTEST1");
            if (table != null)
            {
                Assert.AreEqual(2, table.IndexCount, "Number of indexes");
                bool hi = table.ContainsIndex("INDEXTEST1_title_description");
                Assert.AreEqual(true, hi, "Index name");
                if (hi)
                {
                    index = table.IndexByName("INDEXTEST1_title_description");
                    Assert.AreEqual(false, index.Unique, "Index unique");
                    Assert.AreEqual(2, index.ColumnCount, "Number of fields in index");
                    if (index.ColumnCount > 0)
                    {
                        Assert.AreEqual("title", index.ColumnByIndex(0).ColumnName, true, "Fields in index");
                        Assert.AreEqual(false, index.ColumnByIndex(0).Descending, "Descending");
                    }
                    if (index.ColumnCount > 1)
                    {
                        Assert.AreEqual("description", index.ColumnByIndex(1).ColumnName, true, "Fields in index");
                        Assert.AreEqual(true, index.ColumnByIndex(1).Descending, "Descending");
                    }
                }
                hi = table.ContainsIndex("INDEXTEST1_title_code");
                Assert.AreEqual(hi, true, "Index name");
                if (hi)
                {
                    index = table.IndexByName("INDEXTEST1_title_code");
                    Assert.AreEqual(true, index.Unique, "Index unique");
                    Assert.AreEqual(2, index.ColumnCount, "Number of fields in index");
                }

            }
            Assert.AreEqual(true, ss.ContainsTable("INDEXTEST2"), "Table exists");
            table = ss.TableByName("INDEXTEST2");
            if (table != null)
            {
                Assert.AreEqual(1, table.IndexCount, "Number of indexes");

            }
            DropDB(dburl, ss);
        }
        [TestMethod]
        public void MsSqlForeignkeys()
        {
            string dburl = "mssql://(local)\\SQLEXPRESS/zqmsrgrls?schema=dbo";
            DbSchema ss = CreateDB(dburl);
            using (DbSession session = DbDrivers.Instance.Session(dburl))
            {
                using (DbCmd cmd = session.Command(""))
                {
                    var sql = new StringBuilder();
                    sql.AppendLine(" create table exams");
                    sql.AppendLine(" (");
                    sql.AppendLine(" exam_id uniqueidentifier not null,");
                    sql.AppendLine(" exam_key int not null,");
                    sql.AppendLine(" exam_name varchar(50),");
                    sql.AppendLine("PRIMARY KEY(exam_key, exam_id)");
                    sql.AppendLine(" )");
                    cmd.Clear();
                    cmd.Sql = sql.ToString();
                    cmd.Execute();

                    sql.Clear();
                    sql.AppendLine(" create table question_bank");
                    sql.AppendLine(" (");
                    sql.AppendLine(" question_id uniqueidentifier primary key,");
                    sql.AppendLine(" question_exam_id uniqueidentifier,");
                    sql.AppendLine(" question_exam_key int,");
                    sql.AppendLine(" question_text varchar(1024) not null,");
                    sql.AppendLine(" question_point_value decimal");
                    sql.AppendLine(" )");
                    cmd.Clear();
                    cmd.Sql = sql.ToString();
                    cmd.Execute();

                    sql.Clear();
                    sql.AppendLine(" create table anwser_bank");
                    sql.AppendLine(" (");
                    sql.AppendLine("anwser_id           uniqueidentifier primary key,");
                    sql.AppendLine("anwser_question_id  uniqueidentifier,");
                    sql.AppendLine("anwser_text         varchar(1024),");
                    sql.AppendLine("anwser_is_correct   bit");
                    sql.AppendLine(" );");
                    cmd.Clear();
                    cmd.Sql = sql.ToString();
                    cmd.Execute();

                    cmd.Clear();
                    cmd.Sql = "Alter table question_bank Add foreign key (question_exam_key, question_exam_id) references exams(exam_key, exam_id)";
                    cmd.Execute();

                    cmd.Clear();
                    cmd.Sql = "Alter table anwser_bank Add foreign key (anwser_question_id) references question_bank(question_id)";
                    cmd.Execute();

                }
            }
            ss.Load(dburl);

            DbTable table = ss.TableByName("question_bank");
            if (table != null)
            {
                Assert.AreEqual(1, table.ForeignKeys.Count, "FK count");
                if (table.ForeignKeys.Count > 0)
                {
                    var fk = table.ForeignKeys[0];
                    Assert.AreEqual(2, fk.Columns.Count, "Number of fields in fkFK");
                    if (fk.Columns.Count > 1)
                    {
                        var fkc = fk.Columns[0];
                        Assert.AreEqual("question_exam_key", fkc.ColumnName, true, "FK1");
                        Assert.AreEqual("exam_key", fkc.UniqueColumnName, true, "FK1");
                        fkc = fk.Columns[1];
                        Assert.AreEqual("question_exam_id", fkc.ColumnName, true, "FK2");
                        Assert.AreEqual("exam_id", fkc.UniqueColumnName, true, "FK2");

                    }
                }

            }
            ss.CheckModel();
            var structure = ss.CreateSQL();
            DropDB(dburl, ss);

            //Test create script
            CreateDB(dburl);
            ss.ExecuteSchemaScript(dburl, structure);
            ss.Load(dburl);
            table = ss.TableByName("question_bank");
            if (table != null)
            {
                Assert.AreEqual(1, table.ForeignKeys.Count, "FK count");
                if (table.ForeignKeys.Count > 0)
                {
                    var fk = table.ForeignKeys[0];
                    Assert.AreEqual(2, fk.Columns.Count, "Number of fields in fkFK");
                    if (fk.Columns.Count > 1)
                    {
                        var fkc = fk.Columns[0];
                        Assert.AreEqual("question_exam_key", fkc.ColumnName, true, "FK1");
                        Assert.AreEqual("exam_key", fkc.UniqueColumnName, true, "FK1");
                        fkc = fk.Columns[1];
                        Assert.AreEqual("question_exam_id", fkc.ColumnName, true, "FK2");
                        Assert.AreEqual("exam_id", fkc.UniqueColumnName, true, "FK2");
                    }
                }
                bool hi = table.ContainsIndex("question_bank_question_exam_key_question_exam_id");
                Assert.AreEqual(true, hi, "Index name");
                if (hi)
                {
                    var index = table.IndexByName("question_bank_question_exam_key_question_exam_id");
                    Assert.AreEqual(false, index.Unique, "Index unique");
                    Assert.AreEqual(2, index.ColumnCount, "Number of fields in index");
                    if (index.ColumnCount > 0)
                    {
                        Assert.AreEqual("question_exam_key", index.ColumnByIndex(0).ColumnName, true, "Fields in index");
                        Assert.AreEqual(false, index.ColumnByIndex(0).Descending, "Descending");
                    }
                    if (index.ColumnCount > 1)
                    {
                        Assert.AreEqual("question_exam_id", index.ColumnByIndex(1).ColumnName, true, "Fields in index");
                        Assert.AreEqual(false, index.ColumnByIndex(1).Descending, "Descending");
                    }
                }
  
            }
            DropDB(dburl, ss);
        }
        [TestMethod]
        public void LoadComplexDB()
        {
            string dburl = "mssql://(local)\\SQLEXPRESS/SFR?schema=dbo";
            DbSchema ss = DbDrivers.Instance.Schema(DbServices.Url2Protocol(dburl));
            if (ss.DatabaseExists(dburl))
            {
                ss.Load(dburl);
            }
        }
    }
}
