using Sikia.DataBase.SqlServer;
using Sikia.Framework.Utils;
using Sikia.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Sikia.DataBase
{
    public static class DbServices
    {
        #region Provider
        public static string ProtocolToString(DbProtocol val)
        {
            return Enum.GetName(typeof(DbProtocol), val);
        }
        
        public static DbProtocol StringToProtocol(string val)
        {
            DbProtocol[]allValues =  (DbProtocol[])Enum.GetValues(typeof(DbProtocol));
            Type tdp = typeof(DbProtocol);
            for (int i = 0; i < allValues.Length; i++)
            {
                if (val == Enum.GetName(tdp, allValues[i])) 
                    return allValues[i]; 
            }
            return DbProtocol.unknown;
        }

        public static DbConnectionInfo ConnectionInfo(string url, JsonObject settings)
        {
            DbUri uri = new DbUri(url);
            switch (uri.Protocol)
            {
                case DbProtocol.mssql:
                    MsSqlConnectionInfo ci = new MsSqlConnectionInfo(uri, settings);
                    return ci;
                default:
                    throw new ArgumentException(StrUtils.TT("Database protocol : Not implemented."));
            

            }
        }

        public static DbSession Connection(string url, DbConnectionManger cm)
        {
            DbConnectionInfo ci = cm.ConnectionInfo(url);
            switch (ci.Protocol())
            {
                case DbProtocol.mssql:
                    MsSqlSession session = new MsSqlSession(url, cm);
                    return session;
                default:
                    throw new ArgumentException(StrUtils.TT("Database protocol : Not implemented."));


            }
        }
        
        public static DbSession Connection(string url)
        {
            return DbServices.Connection(url, DbConnectionManger.Instance());
        }

        #endregion
    }
}
