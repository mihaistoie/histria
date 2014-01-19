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
    /// Contains the list of registered databases 
    ///</summary>
    public class DbConnectionManger
    {

        private Dictionary<string, JsonObject> databases = new Dictionary<string, JsonObject>();
        private ReaderWriterLockSlim rwlook = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

        #region Singleton thread-safe pattern
        private static volatile DbConnectionManger instance = null;
        private static object syncRoot = new Object();


        private DbConnectionManger()
        {
        }

        public void Load(JsonValue databases)
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

        public static DbConnectionManger Instance
        {
            get
            {
                if (instance == null)
                {

                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            DbConnectionManger conn = new DbConnectionManger();
                            instance = conn;
                        }
                    }
                }

                return instance;

            }
        }
        public static void CleanUp()
        {
            lock (syncRoot)
            {
                instance = null;
            }

        }

        #endregion

        #region Public Properties && Methods

        public void RegisterDatabase(string url, JsonObject settings)
        {
            rwlook.EnterWriteLock();
            try
            {
                if (databases.ContainsKey(url))
                    return;
                databases.Add(url, settings);
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

        public JsonObject ConnectionSettings(string url)
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
