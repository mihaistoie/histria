using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.DataBase.SqlServer
{
    public class MSSqlConnectionInfo : ConnectionInfo
    {
        public override DBProvider Provider() { return DBProvider.mssql;}
        public override string ConnectionString() { return ""; }
    }
}
