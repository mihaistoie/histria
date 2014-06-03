using Histria.Db.SqlServer;
using Histria.Db.SqlServer.Model;
using Histria.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Db.MsSql
{
    public class MsSqlModule : ModulePlugIn
    {
        public override void Register(params object[] args)
        {
            DbDrivers.Instance.RegisterDriver(DbProtocol.mssql, typeof(MsSqlConnectionInfo), typeof(MsSqlSession), typeof(MsSqlTranslator), typeof(MsSqlSchema));

        }
    }
}
