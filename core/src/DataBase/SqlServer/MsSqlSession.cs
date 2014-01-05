using Sikia.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Sikia.DataBase.SqlServer
{
    public class MsSqlSession : DbSession
    {
        private MsSqlConnectionInfo connection = null;
        private void initialize(string url, DbConnectionManger cm)
        {
            this.url = url;
            if (cm == null)
                cm = DbConnectionManger.Instance();
            connection = (MsSqlConnectionInfo)cm.ConnectionInfo(url);
            this.db = new SqlConnection(connection.ConnectionString());
        }

        public MsSqlSession(string url)
        {
            initialize(url, null);

        }

        public MsSqlSession(string url, DbConnectionManger cm)
        {
            initialize(url, cm);

        }

        public override void Open()
        {
            SqlConnection conn = (SqlConnection)this.db;
            conn.Open();
     
        }
    }
}
