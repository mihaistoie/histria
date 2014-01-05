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
        private void initialize(string url, DbConnectionManger cm)
        {
            this.url = url;
            if (cm == null)
                cm = DbConnectionManger.Instance();
            DbConnectionInfo ci = cm.ConnectionInfo(url);
            this.db = new SqlConnection(ci.ConnectionString());
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
