using Histria.Json;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Histria.Db.SqlServer
{
    public class MsSqlConnectionInfo : DbConnectionInfo
    {
        public static string  MasterDatabase = "master";
        public string Schema = "dbo";
        public override DbProtocol Protocol() { return DbProtocol.mssql; }

 
        public override void Load(string url)
        {
            DbUri uri = new DbUri(url);
            TrustedConnection = true;
            ServerAddress = uri.ServerAddress;
            DatabaseName = uri.DatabaseName;
            if (uri.Query.ContainsKey("schema"))
            {
                Schema = uri.Query["schema"];
            }
            JsonObject settings = DbConnectionManger.Instance.ConnectionSettings(url);
            if (settings != null)
            {
                if (settings.ContainsKey("trustedConnection"))
                {
                    TrustedConnection = settings["trustedConnection"];
                }
                if (settings.ContainsKey("user"))
                {
                    TrustedConnection = false;
                    UserName = settings["user"];
                    if (settings.ContainsKey("password"))
                    {
                        Password = settings["password"];
                    }
                }
            }
        }

        public override string ConnectionString()
        {
            if (TrustedConnection)
            {
                return string.Format("Server = {0}; Database = {1}; Trusted_Connection = True", ServerAddress, DatabaseName);
            }
            else
            {
                return string.Format("Server = {0}; Database = {1}; User ID={2}; Password={3}; Trusted_Connection = False", ServerAddress, DatabaseName, UserName, Password);

            }
        }
        public string MasterConnectionString()
        {

            if (TrustedConnection)
            {
                return string.Format("Server = {0}; Database = {1}; Trusted_Connection = True", ServerAddress, MasterDatabase);
            }
            else
            {
                return string.Format("Server = {0}; Database = {1}; User ID={2}; Password={3}; Trusted_Connection = False", ServerAddress, MasterDatabase, UserName, Password);

            }
        }

    }
}
