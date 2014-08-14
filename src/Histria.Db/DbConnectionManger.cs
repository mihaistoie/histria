using Histria.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;



namespace Histria.Db
{

    ///<summary>
    /// Database _connection manager (singleton)
    /// Contains the list of registered databases 
    ///</summary>
    public class DbConnectionManger
    {

        private Dictionary<string, JsonObject> _databases = new Dictionary<string, JsonObject>();
        private ReaderWriterLockSlim _rwlook = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

        #region Singleton thread-safe pattern
        private static volatile DbConnectionManger _instance = null;
        private static object _syncRoot = new Object();


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
                if (_instance == null)
                {

                    lock (_syncRoot)
                    {
                        if (_instance == null)
                        {
                            DbConnectionManger conn = new DbConnectionManger();
                            _instance = conn;
                        }
                    }
                }

                return _instance;

            }
        }
        public static void CleanUp()
        {
            lock (_syncRoot)
            {
                _instance = null;
            }

        }

        #endregion

        #region Public Properties && Methods

        public void RegisterDatabase(string url, JsonObject settings)
        {
            _rwlook.EnterWriteLock();
            try
            {
                if (_databases.ContainsKey(url))
                    return;
                _databases.Add(url, settings);
            }
            finally
            {
                _rwlook.ExitWriteLock();
            }
        }

        public void UnRegisterDatabase(string url)
        {
            _rwlook.EnterWriteLock();
            try
            {
                if (_databases.ContainsKey(url))
                    _databases.Remove(url);

            }
            finally
            {
                _rwlook.ExitWriteLock();
            }
        }

        public JsonObject ConnectionSettings(string url)
        {
            _rwlook.EnterReadLock();
            try
            {
                if (_databases.ContainsKey(url))
                    return _databases[url];
                return null;

            }
            finally
            {
                _rwlook.ExitReadLock();
            }
        }

        #endregion

    }

}
