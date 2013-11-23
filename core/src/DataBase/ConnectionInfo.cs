using System;

namespace Sikia.DataBase
{
    public abstract class ConnectionInfo
    {

        public bool TrustedConnection { get; set; }
        public string ServerAddress { get; set; }
        public string InstanceAddress { get; set; }
        public string DatabaseName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public abstract DBProvider Provider();
        public abstract string ConnectionString();
       
    }
}
