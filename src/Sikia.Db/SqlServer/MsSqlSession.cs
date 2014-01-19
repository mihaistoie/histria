using Sikia.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Sikia.Db.SqlServer
{
    public class MsSqlSession : DbSession
    {
        private MsSqlConnectionInfo connection = null;
        public SqlConnection Connection
        {
            get
            {
                return (SqlConnection)this.db;
            }
        }

        protected override void setUrl(string value)
        {
            url = value;
            connection = (MsSqlConnectionInfo)DbDrivers.Instance.Connection(url);
            this.db = new SqlConnection(connection.ConnectionString());
        }

        public override DbCmd Command(DbTran transaction)
        {
            return new MsSqlCmd(this, transaction);
        }

        public override DbCmd Command(string sql)
        {
            MsSqlCmd cmd = new MsSqlCmd(this, null);
            cmd.Sql = sql;
            return cmd;
        }

        public override DbCmd Command(string sql, DbTran transaction)
        {
            MsSqlCmd cmd = new MsSqlCmd(this, transaction);
            cmd.Sql = sql;
            return cmd;
        }

        public override void Open()
        {
            Connection.Open();
            int version = ServerMajorVersion();
        }

        #region SqlServer

        public int ServerMajorVersion()
        {
            using (DbCmd cmd = Command("SELECT SERVERPROPERTY('ProductVersion')"))
            {
                string version = (string)cmd.ExecuteScalar();
                version = version.Substring(0, version.IndexOf("."));
                return Convert.ToInt32(version);
            }
        }

        public int EngineEdition()
        {
            using (DbCmd cmd = Command("SELECT SERVERPROPERTY('EngineEdition')"))
            {
                string engine = (string)cmd.ExecuteScalar();
                return Convert.ToInt32(engine);
            }
        }

        public bool DatabaseExists(string dbname)
        {
            using (DbCmd cmd = Command("SELECT count(*) FROM master.dbo.sysdatabases where name = @name"))
            {
                cmd.Parameters.AddWithValue("@name", DbType.Varchar, dbname);
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }

        }

        public void DropDatabase(string dbname)
        {
            using (DbCmd cmd = Command(string.Format("DROP DATABASE {0}", dbname)))
            {
                cmd.Execute();
            }
        }

        public void CreateDatabase(string dbname)
        {
            using (DbCmd cmd = Command(string.Format("CREATE DATABASE {0}", dbname)))
            {
                cmd.Execute();
            }
            int version = ServerMajorVersion();
            int engine = EngineEdition();
            if ((version > SqlServer.Vers2005) && (engine != (int)SqlServer.Engine.Azure))
            {
                // default for Azure
                using (DbCmd cmd = Command("ALTER DATABASE %s SET READ_COMMITTED_SNAPSHOT ON"))
                {
                    cmd.Execute();
                }
            }
        }

        //
        #endregion
    }
}
