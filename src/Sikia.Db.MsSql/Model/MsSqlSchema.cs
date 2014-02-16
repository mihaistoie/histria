using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sikia.Db.Model;
using System.Data.Common;
using System.Data;
using Sikia.Sys;

namespace Sikia.Db.SqlServer.Model
{
    public class MsSqlSchema : DbSchema
    {
        private static string master = "master";
        private static string dbo = "dbo";
        #region Implementation
        private static string MasterUrl(string url)
        {
            DbUri uri = new DbUri(url);
            uri.DatabaseName = MsSqlSchema.master;
            uri.Query.Clear();
            uri.Query["schema"] = MsSqlSchema.dbo;
            return uri.Url;
        }

        #region DbTypes Mapping
        private static void Load(DbColumn column, string dbtype, int length, int precision, int scale)
        {
            column.DbType = dbtype;
            column.Size = length;
            column.Precision = precision;
            column.Scale = scale;
            switch (dbtype)
            {
                case "int":
                    column.Type = DbType.Int;
                    break;
                case "bit":
                    column.Type = DbType.Bool;
                    break;
                case "bigint":
                    column.Type = DbType.BigInt;
                    break;
                case "smallint":
                    column.Type = DbType.SmallInt;
                    break;
                case "tinyint":
                    column.Type = DbType.Enum;
                    break;
                case "money":
                    column.Type = DbType.Currency;
                    break;
                case "decimal":
                    column.Type = DbType.Number;
                    break;
                case "uniqueidentifier":
                    column.Type = DbType.uuid;
                    break;
                case "varchar":
                    if (length == -1)
                        column.Type = DbType.Memo;
                    else
                        column.Type = DbType.String;
                    break;
                case "nvarchar":
                    if (length == -1)
                        column.Type = DbType.Memo;
                    else
                        column.Type = DbType.String;
                    break;
                case "datetime":
                    column.Type = DbType.DateTime;
                    break;
                case "date":
                    column.Type = DbType.Date;
                    break;
                case "time":
                    column.Type = DbType.Time;
                    break;
                case "varbinary":
                    column.Type = DbType.Binary;
                    break;
                default:
                    Logger.Warning(Logger.DBMAP, StrUtils.TT(string.Format("{0}", dbtype)));
                    column.Type = DbType.Unknown;
                    break;
            }
        }

        #endregion

        #region Tables
        ///<summary>
        /// Load list of tables
        ///</summary>
        private void LoadTables(MsSqlTranslator translator, DbUri uri, DbCmd cmd)
        {
            cmd.Clear();
            cmd.Sql = "SELECT TABLE_NAME FROM  INFORMATION_SCHEMA.TABLES WHERE TABLE_CATALOG =@db and TABLE_SCHEMA = @schema and Table_Type='BASE TABLE' ORDER BY TABLE_NAME";
            cmd.Parameters.AddWithValue("@db", DbType.String, uri.DatabaseName);
            cmd.Parameters.AddWithValue("@schema", DbType.String, uri.Query["schema"]);
            using (DbDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    var tableName = rdr.GetString(0);
                    tables.Add(new MsSqlTable { TableName = tableName });
                }
            }
        }
        #endregion

        #region Columns
        ///<summary>
        /// Load columns for tables
        ///</summary>
        private void LoadColumns(MsSqlTranslator translator, DbUri uri, DbCmd cmd)
        {
            cmd.Clear();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(" SELECT");
            sql.AppendLine(" TABLE_NAME, COLUMN_NAME, DATA_TYPE, IS_NULLABLE, COLUMN_DEFAULT, CHARACTER_MAXIMUM_LENGTH,");
            sql.AppendLine(" NUMERIC_PRECISION, NUMERIC_SCALE");
            sql.AppendLine(" FROM");
            sql.AppendLine(" INFORMATION_SCHEMA.COLUMNS");
            sql.AppendLine(" WHERE");
            sql.AppendLine(" TABLE_CATALOG =@db");
            sql.AppendLine(" AND TABLE_SCHEMA = @schema");
            sql.AppendLine(" ORDER BY TABLE_NAME, COLUMN_NAME, ORDINAL_POSITION");
            cmd.Sql = sql.ToString();
            cmd.Parameters.AddWithValue("@db", DbType.String, uri.DatabaseName);
            cmd.Parameters.AddWithValue("@schema", DbType.String, uri.Query["schema"]);
            DbTable table = null;
            using (DbDataReader rdr = cmd.ExecuteReader())
            {
                int idxTN = rdr.GetOrdinal("TABLE_NAME");
                int idxCN = rdr.GetOrdinal("COLUMN_NAME");
                int idxIN = rdr.GetOrdinal("IS_NULLABLE");
                int idxLEN = rdr.GetOrdinal("CHARACTER_MAXIMUM_LENGTH");
                int idxDT = rdr.GetOrdinal("DATA_TYPE");
                int idxPrec = rdr.GetOrdinal("NUMERIC_PRECISION");
                int idxScale = rdr.GetOrdinal("NUMERIC_SCALE");
                int scale = 0;
                int precision = 0;
                int size = 0;
                while (rdr.Read())
                {
                    var tableName = rdr.GetString(idxTN);
                    if (table == null || table.TableName != tableName)
                    {
                        table = TableByName(tableName);
                    }
                    if (table != null)
                    {
                        MsSqlColumn col = new MsSqlColumn()
                        {
                            ColumnName = rdr.GetString(idxCN),
                            TableName = table.TableName,
                            Nullable = (rdr.GetString(idxIN) == "YES")

                        };
                        scale = rdr.IsDBNull(idxScale) ? 0 : rdr.GetInt32(idxScale);
                        precision = rdr.IsDBNull(idxPrec) ? 0 : rdr.GetByte(idxPrec);
                        size = rdr.IsDBNull(idxLEN) ? 0 : rdr.GetInt32(idxLEN);

                        MsSqlSchema.Load(col, rdr.GetString(idxDT), size, precision, scale);
                        table.AddColumn(col);
                    }
                }
            }
        }
        #endregion

        #region Primary Keys
        ///<summary>
        /// Load primary keys for tables
        ///</summary>
        private void LoadPKs(MsSqlTranslator translator, DbUri uri, DbCmd cmd)
        {
            cmd.Clear();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(" SELECT");
            sql.AppendLine(" CC.TABLE_NAME, CC.COLUMN_NAME");
            sql.AppendLine(" FROM");
            sql.AppendLine(" INFORMATION_SCHEMA.KEY_COLUMN_USAGE CC");
            sql.AppendLine(" INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS TC ON");
            sql.AppendLine(" CC.CONSTRAINT_NAME = TC.CONSTRAINT_NAME");
            sql.AppendLine(" AND CC.TABLE_CATALOG = TC.TABLE_CATALOG");
            sql.AppendLine(" AND CC.TABLE_SCHEMA = TC.TABLE_SCHEMA");
            sql.AppendLine(" WHERE");
            sql.AppendLine(" CC.TABLE_CATALOG =@db");
            sql.AppendLine(" AND CC.TABLE_SCHEMA = @schema");
            sql.AppendLine(" AND TC.CONSTRAINT_TYPE = 'PRIMARY KEY'");
            sql.AppendLine(" ORDER BY CC.CONSTRAINT_NAME, CC.ORDINAL_POSITION");
            cmd.Sql = sql.ToString();
            cmd.Parameters.AddWithValue("@db", DbType.String, uri.DatabaseName);
            cmd.Parameters.AddWithValue("@schema", DbType.String, uri.Query["schema"]);
            DbTable table = null;
            using (DbDataReader rdr = cmd.ExecuteReader())
            {
                int idxTN = rdr.GetOrdinal("TABLE_NAME");
                int idxCN = rdr.GetOrdinal("COLUMN_NAME");
                while (rdr.Read())
                {
                    var tableName = rdr.GetString(idxTN);
                    if (table == null || table.TableName != tableName)
                    {
                        table = TableByName(tableName);
                    }
                    if (table != null)
                    {
                        table.PK.Add(rdr.GetString(idxCN));
                    }
                }
            }
        }
        #endregion

        #region Indexes
        ///<summary>
        /// Load indexes
        ///</summary>
        private void LoadIndexes(MsSqlTranslator translator, DbUri uri, DbCmd cmd)
        {
            cmd.Clear();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(" SELECT");
            sql.AppendLine(" t.name as TABLE_NAME,");
            sql.AppendLine(" ind.name as INDEX_NAME,");
            sql.AppendLine(" col.name as COLUMN_NAME,");
            sql.AppendLine(" ic.is_descending_key as DESCENDING,");
            sql.AppendLine(" ind.is_unique as IsUnique");
            sql.AppendLine(" FROM");
            sql.AppendLine(" sys.indexes ind");
            sql.AppendLine(" INNER JOIN  sys.index_columns ic ON  ind.object_id = ic.object_id and ind.index_id = ic.index_id");
            sql.AppendLine(" INNER JOIN  sys.columns col ON ic.object_id = col.object_id and ic.column_id = col.column_id");
            sql.AppendLine(" INNER JOIN  sys.tables t ON ind.object_id = t.object_id");
            sql.AppendLine(" inner join  sys.schemas s on t.schema_id = s.schema_id");
            sql.AppendLine(" WHERE");
            sql.AppendLine(" ind.is_primary_key = 0");
            sql.AppendLine(" AND ic.key_ordinal > 0");
            sql.AppendLine(" AND ind.is_unique_constraint = 0");
            sql.AppendLine(" AND t.is_ms_shipped = 0");
            sql.AppendLine(" AND s.name = @schema");
            sql.AppendLine(" ORDER BY");
            sql.AppendLine(" t.name, ind.name, ic.key_ordinal");
            cmd.Sql = sql.ToString();
            cmd.Parameters.AddWithValue("@schema", DbType.String, uri.Query["schema"]);
            DbTable table = null;
            DbIndex index = null;
            using (DbDataReader rdr = cmd.ExecuteReader())
            {
                int idxTN = rdr.GetOrdinal("TABLE_NAME");
                int idxIN = rdr.GetOrdinal("INDEX_NAME");
                int idxCN = rdr.GetOrdinal("COLUMN_NAME");
                int idxCD = rdr.GetOrdinal("DESCENDING");
                int idxIU = rdr.GetOrdinal("IsUnique");

                while (rdr.Read())
                {
                    var tableName = rdr.GetString(idxTN);
                    if (table == null || table.TableName != tableName)
                    {
                        index = null;
                        table = TableByName(tableName);
                    }
                    if (table != null)
                    {
                        var indexName = rdr.GetString(idxIN);
                        if (index == null || index.IndexName != indexName)
                        {
                            index = new DbIndex() {IndexName = indexName, Unique = rdr.GetBoolean(idxIU) };
                            table.AddIndex(index);
                        }
                        index.AddColumn(rdr.GetString(idxCN), rdr.GetBoolean(idxCD));

                    }
                }
            }
        }
        #endregion

        #region Foreign keys
        ///<summary>
        /// Load Foreign keys
        ///</summary>
        private void LoadForeignKeys(MsSqlTranslator translator, DbUri uri, DbCmd cmd)
        {
            cmd.Clear();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(" SELECT");
            sql.AppendLine(" FKC.TABLE_NAME as RTABLE, PKC.TABLE_NAME as UTABLE, FKC.COLUMN_NAME as RCOLUMN,");
            sql.AppendLine(" PKC.COLUMN_NAME as UCOLUMN,");
            sql.AppendLine(" FKC.CONSTRAINT_NAME");
            sql.AppendLine(" from INFORMATION_SCHEMA.KEY_COLUMN_USAGE FKC");
            sql.AppendLine(" inner join INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS R");
            sql.AppendLine(" ON R.CONSTRAINT_CATALOG = FKC.CONSTRAINT_CATALOG");
            sql.AppendLine(" AND R.CONSTRAINT_SCHEMA = FKC.CONSTRAINT_SCHEMA");
            sql.AppendLine(" AND R.CONSTRAINT_NAME = FKC.CONSTRAINT_NAME");
            sql.AppendLine(" inner join  (");
            sql.AppendLine(" SELECT");
            sql.AppendLine(" t.name as TABLE_NAME,");
            sql.AppendLine(" ind.name as CONSTRAINT_NAME,");
            sql.AppendLine(" col.name as COLUMN_NAME,");
            sql.AppendLine(" ic.key_ordinal as ORDINAL_POSITION,");
            sql.AppendLine(" s.name as CONSTRAINT_SCHEMA");
            sql.AppendLine(" FROM");
            sql.AppendLine(" sys.indexes ind");
            sql.AppendLine(" INNER JOIN  sys.index_columns ic ON  ind.object_id = ic.object_id and ind.index_id = ic.index_id");
            sql.AppendLine(" INNER JOIN  sys.columns col ON ic.object_id = col.object_id and ic.column_id = col.column_id");
            sql.AppendLine(" INNER JOIN  sys.tables t ON ind.object_id = t.object_id");
            sql.AppendLine(" inner join  sys.schemas s on t.schema_id = s.schema_id");
            sql.AppendLine(" WHERE");
            sql.AppendLine(" ic.key_ordinal > 0");
            sql.AppendLine(" AND ind.is_unique_constraint = 0");
            sql.AppendLine(" AND t.is_ms_shipped = 0");
            sql.AppendLine(" AND ind.is_unique = 1 ");
            sql.AppendLine(" ) as PKC on  R.UNIQUE_CONSTRAINT_NAME = PKC.CONSTRAINT_NAME");
            sql.AppendLine(" AND R.UNIQUE_CONSTRAINT_SCHEMA = PKC.CONSTRAINT_SCHEMA");
            sql.AppendLine(" AND FKC.ORDINAL_POSITION = PKC.ORDINAL_POSITION");
            sql.AppendLine(" WHERE");
            sql.AppendLine(" FKC.TABLE_CATALOG =@db");
            sql.AppendLine(" AND FKC.TABLE_SCHEMA = @schema");
            sql.AppendLine(" order by FKC.TABLE_NAME, FKC.ORDINAL_POSITION");


            cmd.Sql = sql.ToString();
            cmd.Parameters.AddWithValue("@db", DbType.String, uri.DatabaseName);
            cmd.Parameters.AddWithValue("@schema", DbType.String, uri.Query["schema"]);
            DbTable rtable = null;
            DbTable utable = null;
            DbFk fk = null;
            using (DbDataReader rdr = cmd.ExecuteReader())
            {
                int idxRT = rdr.GetOrdinal("RTABLE");
                int idxUT = rdr.GetOrdinal("UTABLE");
                int idxRC = rdr.GetOrdinal("RCOLUMN");
                int idxUC = rdr.GetOrdinal("UCOLUMN");
                int idxCN = rdr.GetOrdinal("CONSTRAINT_NAME");


                while (rdr.Read())
                {
                    var rtableName = rdr.GetString(idxRT);
                    var utableName = rdr.GetString(idxUT);

                    if (rtable == null || rtable.TableName != rtableName)
                    {
                        fk = null;
                        rtable = TableByName(rtableName);
                    }

                    if (utable == null || utable.TableName != utableName)
                    {
                        fk = null;
                        utable = TableByName(utableName);
                    }
                    if (rtable != null && utable != null)
                    {
                        var fkName = rdr.GetString(idxCN);
                        if (fk == null || fk.FKName != fkName)
                        {
                            fk = new DbFk() { FKName = fkName, TableName = rdr.GetString(idxRT), UniqueTableName = rdr.GetString(idxUT) };
                            rtable.ForeignKeys.Add(fk);
                        }
                        var fki = new DbFkItem() { ColumnName = rdr.GetString(idxRC), UniqueColumnName = rdr.GetString(idxUC) };
                        fk.Columns.Add(fki);

                    }
                }
            }
        }
        #endregion

        #endregion

        #region Database Management : create/drop/exists
        public override void CreateDatabase(string url)
        {
            DbUri uri = new DbUri(url);
            using (MsSqlSession session = (MsSqlSession)DbDrivers.Instance.Session(MasterUrl(url)))
            {
                if (session != null)
                {
                    session.CreateDatabase(uri.DatabaseName);
                }
            }
        }
        public override bool DatabaseExists(string url)
        {
            DbUri uri = new DbUri(url);
            using (MsSqlSession session = (MsSqlSession)DbDrivers.Instance.Session(MasterUrl(url)))
                if (session != null)
                {
                    return session.DatabaseExists(uri.DatabaseName);
                }
            return false;
        }
        public override void DropDatabase(string url)
        {
            DbUri uri = new DbUri(url);
            using (MsSqlSession session = (MsSqlSession)DbDrivers.Instance.Session(MasterUrl(url)))
            {
                if (session != null)
                {
                    session.DropDatabase(uri.DatabaseName);
                }
            }
        }
        #endregion

        #region Factory
        public override DbTable Table()
        {
            return new MsSqlTable();
        }
        public override DbColumn Column()
        {
            return new MsSqlColumn();
        }
        #endregion

        public override void Load(string url)
        {
            tables.Clear();
            DbUri uri = new DbUri(url);
            MsSqlTranslator translator = (MsSqlTranslator)DbDrivers.Instance.Translator(DbProtocol.mssql);

            using (MsSqlSession session = (MsSqlSession)DbDrivers.Instance.Session(url))
            {
                DataTable Tables = session.Connection.GetSchema("Tables");
                using (DbCmd cmd = session.Command(""))
                {
                    LoadTables(translator, uri, cmd);
                    LoadColumns(translator, uri, cmd);
                    LoadPKs(translator, uri, cmd);
                    LoadIndexes(translator, uri, cmd);
                    LoadForeignKeys(translator, uri, cmd);
                }
            }
        }

    }
}



