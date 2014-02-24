using Sikia.Db.SqlServer;
using Sikia.Db.SqlServer.Model;
using Sikia.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Db.MsSql
{
    public class MsSqlModule : ModulePlugIn
    {
        public override void Register(params object[] args)
        {
            DbDrivers.Instance.RegisterDriver(DbProtocol.mssql, typeof(MsSqlConnectionInfo), typeof(MsSqlSession), typeof(MsSqlTranslator), typeof(MsSqlSchema));
           
        }
    }
}
