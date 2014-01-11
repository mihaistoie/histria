using System;
using System.Data.Common;

namespace Sikia.Db
{
    public abstract class DbConnectionInfo
    {

        public bool TrustedConnection { get; set; }
        public string ServerAddress { get; set; }
        public string InstanceAddress { get; set; }
        public string DatabaseName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public abstract DbProtocol Protocol();
        public abstract string ConnectionString();
  
    }
}
