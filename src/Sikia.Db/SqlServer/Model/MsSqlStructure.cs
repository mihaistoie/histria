using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sikia.Db.Model;

namespace Sikia.Db.SqlServer.Model
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
        private void LoadTables(MsSqlTranslator translator, DbCmd cmd) {
        }

        public override void CreateDatabase(string url)
        {
            DbUri uri = new DbUri(url);
            using (MsSqlSession session = (MsSqlSession)DbDrivers.Instance.Session(MasterUrl(url)))
            {
                if (session != null)
                {
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
                    session.DropDatabase(uri.DatabaseName);
                }
            }
        }
        public override void Load(string url)
        {
            tables.Clear();
            MsSqlTranslator translator = (MsSqlTranslator)DbDrivers.Instance.Translator(DbProtocol.mssql);
            using (MsSqlSession session = (MsSqlSession)DbDrivers.Instance.Session(url))
            {
                using (DbCmd cmd = session.Command(""))
                {
                    LoadTables(translator, cmd);
                } 
            }
        }

    }
}
