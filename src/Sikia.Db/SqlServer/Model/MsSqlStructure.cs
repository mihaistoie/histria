using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sikia.Db.Model;
using System.Data.Common;
using System.Data;

namespace Sikia.Db.SqlServer.Model
{
    public class MsSqlStructure : DbStructure
    {
        private static string master = "master";
        private static string dbo = "dbo";
        private static string MasterUrl(string url)
        {
            DbUri uri = new DbUri(url);
            uri.DatabaseName = MsSqlStructure.master;
            uri.Query.Clear();
            uri.Query["schema"] = MsSqlStructure.dbo;
            return uri.Url;
        }
        private void LoadTables(MsSqlTranslator translator,  DbUri uri, DbCmd cmd)
        {
            cmd.Clear();
            cmd.Sql = "SELECT TABLE_NAME FROM  INFORMATION_SCHEMA.TABLES WHERE TABLE_CATALOG =@db and TABLE_SCHEMA = @schema and Table_Type='BASE TABLE' ORDER BY TABLE_NAME";
            cmd.Parameters.AddWithValue("@db", DbType.Varchar, uri.DatabaseName);
            cmd.Parameters.AddWithValue("@schema", DbType.Varchar, uri.Query["schema"]);
            using (DbDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    var tableName = rdr.GetString(0);
                    tables.Add(tableName, new MsSqlTable{ TableName = tableName });
                }
            }
        }

        private void LoadColumns(MsSqlTranslator translator, DbUri uri, DbCmd cmd)
        {
            cmd.Clear();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(" SELECT");
            sql.AppendLine(" TABLE_NAME, COLUMN_NAME, DATA_TYPE, IS_NULLABLE, COLUMN_DEFAULT, CHARACTER_MAXIMUM_LENGTH");
            sql.AppendLine(" FROM");
            sql.AppendLine(" INFORMATION_SCHEMA.COLUMNS");
            sql.AppendLine(" WHERE");
            sql.AppendLine(" TABLE_CATALOG =@db");
            sql.AppendLine(" AND TABLE_SCHEMA = @schema");
            sql.AppendLine(" ORDER BY TABLE_NAME, COLUMN_NAME, ORDINAL_POSITION");
            cmd.Sql = sql.ToString();
            cmd.Parameters.AddWithValue("@db", DbType.Varchar, uri.DatabaseName);
            cmd.Parameters.AddWithValue("@schema", DbType.Varchar, uri.Query["schema"]);
            DbTable table = null;
            using (DbDataReader rdr = cmd.ExecuteReader())
            {
                int idxTN = rdr.GetOrdinal("TABLE_NAME");
                int idxCN = rdr.GetOrdinal("COLUMN_NAME");
                int idxIN = rdr.GetOrdinal("IS_NULLABLE");
                int idxLEN = rdr.GetOrdinal("CHARACTER_MAXIMUM_LENGTH");
                while (rdr.Read())
                {
                    var tableName = rdr.GetString(idxTN);
                    if (table == null || table.TableName != tableName)
                    {
                        tables.TryGetValue(tableName, out table);
                    }
                    if (table != null)
                    {
                        MsSqlColumn col = new MsSqlColumn()
                        {
                            ColumnName = rdr.GetString(idxCN),
                            TableName = table.TableName,
                            Nullable = (rdr.GetString(idxIN) == "YES")

                        };
                        table.Columns.Add(col.ColumnName, col);
                    }
                }
            }
        }

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
                }
            }
        }

    }
}
