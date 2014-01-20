using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Db.SqlServer
{
    public class MsSqlStructure: DbStructure
    {
        private static string master = "master";
        private static string dbo = "dbo";
        private static string MasterUrl(string url) 
        {
            DbUri uri = new DbUri(url);
            uri.DatabaseName = MsSqlStructure.master;
            uri.Query.Clear();
            uri.Query["schema"] = MsSqlStructure.dbo;
            return uri.Url;
        }

        public override void CreateDatabase(string url)
        {
            DbUri uri = new DbUri(url);
            using (MsSqlSession session = (MsSqlSession)DbDrivers.Instance.Session(MasterUrl(url)))
            {
                if (session != null)
                {
                    session.Open();
                    session.CreateDatabase(uri.DatabaseName);
                }
            }
        }

        public override bool DatabaseExists(string url)
        {
            DbUri uri = new DbUri(url);
            using (MsSqlSession session = (MsSqlSession)DbDrivers.Instance.Session(MasterUrl(url)))
            if (session != null)
            {
                session.Open();
                return session.DatabaseExists(uri.DatabaseName);
            }
            return false;
        }

        public override void DropDatabase(string url)
        {
            DbUri uri = new DbUri(url);
            using (MsSqlSession session = (MsSqlSession)DbDrivers.Instance.Session(MasterUrl(url)))
            {
                if (session != null)
                {
                    session.Open();
                    session.DropDatabase(uri.DatabaseName);
                }
            }
        }
    }
}
