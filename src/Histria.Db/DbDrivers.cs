using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Histria.Db.Model;

namespace Histria.Db
{

    public class DbDrivers
    {
        class DbDriver
        {
            private Type _connection = null;
            private Type _session = null;
            private Type _structure = null;
            public DbTranslator Translator = null;
            private DbDriver()
            {
            }
            public DbConnectionInfo Connection()
            {
                return (DbConnectionInfo)Activator.CreateInstance(_connection);
            }

            public DbSession Session()
            {
                return (DbSession)Activator.CreateInstance(_session);
            }

            public DbSchema Structure()
            {
                return (DbSchema)Activator.CreateInstance(_structure);
            }

            public DbDriver(Type ConnectionType, Type DbSessionType, Type TranslatorType, Type DbStructure)
            {
                Translator = (DbTranslator)Activator.CreateInstance(TranslatorType);
                _connection = ConnectionType;
                _session = DbSessionType;
                _structure = DbStructure;
            }
        }

        ///<summary>
        /// Database Drivers (singleton)
        /// Contains the list of supported databases (Sqlsetver/Oracle/MySql ) 
        ///</summary>
        #region Singleton thread-safe pattern
        private static volatile DbDrivers _instance = null;
        private static object _syncRoot = new Object();
        private Dictionary<DbProtocol, DbDriver> drivers = new Dictionary<DbProtocol, DbDriver>();

        private DbDrivers()
        {
            //Register supported Drivers
            drivers.Add(DbProtocol.nodb, new DbDriver(typeof(DbConnectionInfo), typeof(DbSession), typeof(DbTranslator), typeof(DbSchema)));
        }
   
        public static void RegisterDriver(DbProtocol protocol, Type connectionType, Type sessionType, Type translatorType, Type SchemaType)
        {
            Drivers.drivers.Add(protocol, new DbDriver(connectionType, sessionType, translatorType, SchemaType));
        }

        private static DbDrivers Drivers
        {
            get
            {
                if (_instance == null)
                {

                    lock (_syncRoot)
                    {
                        if (_instance == null)
                        {
                            DbDrivers conn = new DbDrivers();
                            _instance = conn;
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

        #region Driver services
        public static  DbTranslator Translator(DbProtocol protocol)
        {
            DbDriver driver = Drivers.drivers[protocol];
            if (driver != null)
            {
                return driver.Translator;
            }
            return null;
        }

        public static DbConnectionInfo Connection(string url)
        {
            DbProtocol protocol = DbServices.Url2Protocol(url);
            DbDriver driver = Drivers.drivers[protocol];
            if (driver != null)
            {
                DbConnectionInfo ci = driver.Connection();
                ci.Load(url);
                return ci;
            }
            return null;
        }

        public static DbSession Session(string url)
        {
            DbProtocol protocol = DbServices.Url2Protocol(url);
            DbDriver driver = Drivers.drivers[protocol];
            if (driver != null)
            {
                DbSession session = driver.Session();
                session.Url = url;
                session.Open();
                return session;
            }
            return null;

        }

        public static DbSchema Schema(DbProtocol protocol)
        {
            DbDriver driver = Drivers.drivers[protocol];
            if (driver != null)
            {
                return driver.Structure();
            }
            return null;

        }
        #endregion

    }


}
