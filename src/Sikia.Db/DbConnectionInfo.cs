using Sikia.Json;
using System;
using System.Data.Common;

namespace Sikia.Db
{
    public class DbConnectionInfo
    {

        public bool TrustedConnection { get; set; }
        public string ServerAddress { get; set; }
        public string InstanceAddress { get; set; }
        public string DatabaseName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Url { get; set; }

        public virtual DbProtocol Protocol()
        {
            return DbProtocol.nodb;
        }
        public virtual string ConnectionString()
        {
            return "";
        }
        public virtual void Load(string url)
        {
        }

    }
}
