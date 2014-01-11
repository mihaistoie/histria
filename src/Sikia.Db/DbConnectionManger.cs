using Sikia.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;



namespace Sikia.Db
{
  
    ///<summary>
    /// Database connection manager (singleton)
    ///</summary>
    public class DbConnectionManger
    {

        private Dictionary<string, DbConnectionInfo> databases =  new Dictionary<string, DbConnectionInfo>();
        private ReaderWriterLockSlim rwlook = new ReaderWriterLockSlim();
   
        #region Singleton thread-safe pattern
        private static volatile DbConnectionManger instance = null;
        private static object syncRoot = new Object();
        

        private DbConnectionManger()
        {
        }
        
        private void Load(JsonValue databases)
        {
            if (databases != null && databases is JsonObject)
            {
                JsonObject db = (JsonObject)databases;
                foreach (string url in db.Keys)
                {
                    RegisterDatabase(url, (JsonObject)db[url]);
                }
            }
        }

        public static DbConnectionManger Instance()
        {
            return Instance(null);
        }
       
        public static void CleanUp()
        {
            lock (syncRoot)
            {
                instance = null;
            }

        }
        public static DbConnectionManger LoadConfig(JsonValue databases)
        {
            DbConnectionManger conn = new DbConnectionManger();
            conn.Load(databases);
            return conn;
        }

        public static DbConnectionManger Instance(JsonValue databases)
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        DbConnectionManger conn = new DbConnectionManger();
                        conn.Load(databases);
                        instance = conn;
                    }
                }
            }

            return instance;
        }
        #endregion

      
        #region Public Properties && Methods

        public void RegisterDatabase(string url, JsonObject settings)
        {
            DbConnectionInfo ci = DbServices.ConnectionInfo(url, settings);
            rwlook.EnterWriteLock();
            try
            {
                databases.Add(url, ci);
            }
            finally
            {
                rwlook.ExitWriteLock();
            }   
        }
        
        public void UnRegisterDatabase(string url)
        {
            rwlook.EnterWriteLock();
            try
            {
                if (databases.ContainsKey(url))
                    databases.Remove(url);

            }
            finally
            {
                rwlook.ExitWriteLock();
            }
        }

        public DbConnectionInfo ConnectionInfo(string url)
        {
            rwlook.EnterReadLock();
            try
            {
                if (databases.ContainsKey(url))
                    return databases[url];
                return null;

            }
            finally
            {
                rwlook.ExitReadLock();
            }
        }
        
        #endregion

    }

}
