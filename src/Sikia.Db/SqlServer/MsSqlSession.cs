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
        private bool connected = false;
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
            if (!connected)
            {
                Connection.Open();
                connected = true;
            }
        }

        #region SqlServer

        public int ServerMajorVersion()
        {
            string version = Connection.ServerVersion;
            version = version.Substring(0, version.IndexOf("."));
            return Convert.ToInt32(version);

        }

        public int EngineEdition()
        {

             using (DbCmd cmd = Command("SELECT SERVERPROPERTY('EngineEdition')"))
            {
                return (int)cmd.ExecuteScalar();
            }
        }

        public bool DatabaseExists(string dbname)
        {
            MsSqlTranslator translator = (MsSqlTranslator)DbDrivers.Instance.Translator(DbProtocol.mssql);

            using (DbCmd cmd = Command(translator.SQL_DatabaseExists()))
            {
                cmd.Parameters.AddWithValue("@name", DbType.String, dbname);
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }

        }

        public void DropDatabase(string dbname)
        {
            SqlConnection.ClearAllPools();
            MsSqlTranslator translator = (MsSqlTranslator)DbDrivers.Instance.Translator(DbProtocol.mssql);
            using (DbCmd cmd = Command(string.Format(translator.SQL_DropDatabase(), dbname)))
            {
                cmd.Execute();
            }
        }

        public void CreateDatabase(string dbname)
        {
            MsSqlTranslator translator = (MsSqlTranslator)DbDrivers.Instance.Translator(DbProtocol.mssql);
            using (DbCmd cmd = Command(string.Format(translator.SQL_CreateDatabase(), dbname)))
            {
                cmd.Execute();
            }
            int version = ServerMajorVersion();
            int engine = EngineEdition();
            if ((version > SqlServer.Vers2005) && (engine != (int)SqlServer.Engine.Azure))
            {
                // default for Azure
                using (DbCmd cmd = Command(string.Format(translator.SQL_READ_COMMITTED_SNAPSHOT(), dbname)))
                {
                    cmd.Execute();
                }
            }
        }

        //
        #endregion
    }
}
