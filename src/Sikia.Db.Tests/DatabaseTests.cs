using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sikia.Json;
using Sikia.Db;
using Sikia.Db.Model;
using Sikia.Db.SqlServer;
using System.Text;


namespace Sikia.Db.Tests
{
    [TestClass]
    public class DatabaseTests
    {
        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {


        }

        [TestMethod]
        public void MsSqlUrlParser()
        {
            string dburl = "mssql://(local)\\SQLEXPRESS/master?schema=dbo";
            DbConnectionInfo ci = DbDrivers.Instance.Connection(dburl);
            Assert.AreEqual(ci.ServerAddress, "(local)\\SQLEXPRESS", "Parse Server part");
            Assert.AreEqual(ci.DatabaseName, "master", "Parse Database part");
            MsSqlConnectionInfo ms = (MsSqlConnectionInfo)ci;
            Assert.AreEqual(ms.Schema, "dbo", "Parse schema part");
        }

        [TestMethod]
        public void MsSqlConnection()
        {
            string dburl = "mssql://(local)\\SQLEXPRESS/master?schema=dbo";
            using (DbSession ss = DbDrivers.Instance.Session(dburl))
            {
                DbCmd cmd = ss.Command("select 1");
                int res = (int)cmd.ExecuteScalar();
                Assert.AreEqual(res, 1, "ExecuteScalar");
            }

        }
        [TestMethod]
        public void MsSqlDatabases()
        {
            string dburl = "mssql://(local)\\SQLEXPRESS/xyzgrls?schema=dbo";
            DbStructure ss = DbDrivers.Instance.Structure(DbServices.Url2Protocol(dburl));
            if (ss.DatabaseExists(dburl))
            {
                ss.DropDatabase(dburl);
            }
            ss.CreateDatabase(dburl);
            ss.DropDatabase(dburl);
            Assert.AreEqual(ss.DatabaseExists(dburl), false, "DatabaseExists");

        }
        [TestMethod]
        public void MsSqlTables()
        {
            string dburl = "mssql://(local)\\SQLEXPRESS/msergrls?schema=dbo";
            DbStructure ss = DbDrivers.Instance.Structure(DbServices.Url2Protocol(dburl));
            if (ss.DatabaseExists(dburl))
            {
                ss.DropDatabase(dburl);
            }
            ss.CreateDatabase(dburl);
            Assert.AreEqual(ss.DatabaseExists(dburl), true, "DatabaseExists");
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
            ss.Load(dburl);
            Assert.AreEqual(ss.Tables.ContainsKey("a1"), true, "Table exists");
            Assert.AreEqual(ss.Tables.ContainsKey("B1"), true, "Table exists");
            ss.DropDatabase(dburl);
            Assert.AreEqual(ss.DatabaseExists(dburl), false, "DatabaseExists");

        }
        void checkColumn(DbTable table, string name, DbType dt, string dbtype, int size, int prec, int scale, bool nullable)
        {
            Assert.AreEqual(table.Columns.ContainsKey(name), true, "Column exists");
            DbColumn column = null;
            if (table.Columns.TryGetValue(name, out column))
            {
                Assert.AreEqual(column.Type, dt, "column type");
                Assert.AreEqual(column.DbType, dbtype, "column sql type");
                Assert.AreEqual(column.Size, size, "column length");
                Assert.AreEqual(column.Nullable, nullable, "column nullable");
                if ((dbtype != "int") && (dbtype != "bigint") && (dbtype != "smallint") && (dbtype != "tinyint") && (dbtype != "money"))
                {
                    Assert.AreEqual(column.Precision, prec, "column precision");
                    Assert.AreEqual(column.Scale, scale, "column scale");
                }

            }
        }

        [TestMethod]
        public void MsSqlColumns()
        {
            string dburl = "mssql://(local)\\SQLEXPRESS/slergrls?schema=dbo";
            DbStructure ss = DbDrivers.Instance.Structure(DbServices.Url2Protocol(dburl));
            if (ss.DatabaseExists(dburl))
            {
                ss.DropDatabase(dburl);
            }
            ss.CreateDatabase(dburl);
            Assert.AreEqual(ss.DatabaseExists(dburl), true, "DatabaseExists");
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
            Assert.AreEqual(ss.Tables.ContainsKey("SUPPORTEDTYPES"), true, "Table exists");
            DbTable table = null;
            if (ss.Tables.TryGetValue("SUPPORTEDTYPES", out table))
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
            ss.DropDatabase(dburl);
            Assert.AreEqual(ss.DatabaseExists(dburl), false, "DatabaseExists");

        }

        [TestMethod]
        public void MsSqlPrimarykeys()
        {
            string dburl = "mssql://(local)\\SQLEXPRESS/slfsrgrls?schema=dbo";
            DbStructure ss = DbDrivers.Instance.Structure(DbServices.Url2Protocol(dburl));
            if (ss.DatabaseExists(dburl))
            {
                ss.DropDatabase(dburl);
            }
            ss.CreateDatabase(dburl);
            Assert.AreEqual(ss.DatabaseExists(dburl), true, "DatabaseExists");
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
            Assert.AreEqual(ss.Tables.ContainsKey("PKTEST"), true, "Table exists");
            DbTable table = null;
            if (ss.Tables.TryGetValue("PKTEST", out table))
            {
                Assert.AreEqual(table.PKs.Count, 2, "Number of fields in PK");
                Assert.AreEqual(table.PKs.IndexOf("a"), 1, "Fields order in PK");
                Assert.AreEqual(table.PKs.IndexOf("code"), 0, "Fields order in PK");

            }
            ss.DropDatabase(dburl);
            Assert.AreEqual(ss.DatabaseExists(dburl), false, "DatabaseExists");

        }

        [TestMethod]
        public void MsSqlIndexes()
        {
            string dburl = "mssql://(local)\\SQLEXPRESS/slfsrgrls?schema=dbo";
            DbStructure ss = DbDrivers.Instance.Structure(DbServices.Url2Protocol(dburl));
            if (ss.DatabaseExists(dburl))
            {
                ss.DropDatabase(dburl);
            }
            ss.CreateDatabase(dburl);
            Assert.AreEqual(ss.DatabaseExists(dburl), true, "DatabaseExists");
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
            Assert.AreEqual(ss.Tables.ContainsKey("INDEXTEST1"), true, "Table exists");
            DbTable table = null;
            DbIndex index = null;
            if (ss.Tables.TryGetValue("INDEXTEST1", out table))
            {
                Assert.AreEqual(table.Indexes.Count, 2, "Number of indexes");
                bool hi = table.Indexes.Contains("INDEXTEST1_title_description");
                Assert.AreEqual(hi, true, "Index name");
                if (hi)
                {
                    index = table.Indexes["INDEXTEST1_title_description"];
                    Assert.AreEqual(index.Unique, false, "Index unique");
                    Assert.AreEqual(index.Columns.Count, 2, "Number of fields in index");
                    if (index.Columns.Count > 0)
                    {
                        Assert.AreEqual(index.Columns[0].ColumnName, "title", "Fields in index");
                        Assert.AreEqual(index.Columns[0].Descending, false, "Descending");
                    }
                    if (index.Columns.Count > 1)
                    {
                        Assert.AreEqual(index.Columns[1].ColumnName, "description", "Fields in index");
                        Assert.AreEqual(index.Columns[1].Descending, true, "Descending");
                    }
                }
                hi = table.Indexes.Contains("INDEXTEST1_title_code");
                Assert.AreEqual(hi, true, "Index name");
                if (hi)
                {
                    index = table.Indexes["INDEXTEST1_title_code"];
                    Assert.AreEqual(index.Unique, true, "Index unique");
                    Assert.AreEqual(index.Columns.Count, 2, "Number of fields in index");
                }

            }
            Assert.AreEqual(ss.Tables.ContainsKey("INDEXTEST2"), true, "Table exists");
            table = null;
            if (ss.Tables.TryGetValue("INDEXTEST2", out table))
            {
                Assert.AreEqual(table.Indexes.Count, 1, "Number of indexes");

            }
            ss.DropDatabase(dburl);
            Assert.AreEqual(ss.DatabaseExists(dburl), false, "DatabaseExists");

        }
    }
}
