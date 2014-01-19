using Sikia.Db.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Db
{

    public class DbDrivers
    {
        class DbDriver
        {
            private Type connection = null;
            private Type session = null;
            public DbTranslator Translator = null;
            private DbDriver()
            {
            }
            public DbConnectionInfo Connection()
            {
                return (DbConnectionInfo)Activator.CreateInstance(connection);
            }

            public DbSession Session()
            {
                return (DbSession)Activator.CreateInstance(session);
            }

            public DbDriver(Type ConnectionType, Type DbSessionType, Type TranslatorType)
            {
                Translator = (DbTranslator)Activator.CreateInstance(TranslatorType);
                connection = ConnectionType;
                session = DbSessionType;
            }
        }

        ///<summary>
        /// Database Drivers (singleton)
        /// Contains the list of supported databases (Sqlsetver/Oracle/MySql ) 
        ///</summary>
        #region Singleton thread-safe pattern
        private static volatile DbDrivers instance = null;
        private static object syncRoot = new Object();
        private Dictionary<DbProtocol, DbDriver> drivers = new Dictionary<DbProtocol, DbDriver>();

        private DbDrivers()
        {
            //Register supported Drivers
            drivers.Add(DbProtocol.nodb, new DbDriver(typeof(DbConnectionInfo), typeof(DbSession), typeof(DbTranslator)));
            drivers.Add(DbProtocol.mssql, new DbDriver(typeof(MsSqlConnectionInfo), typeof(MsSqlSession), typeof(MsSqlTranslator)));
        }

        public static DbDrivers Instance
        {
            get
            {
                if (instance == null)
                {

                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            DbDrivers conn = new DbDrivers();
                            instance = conn;
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        #region Driver services
        public DbTranslator Translator(DbProtocol protocol)
        {
            DbDriver driver = drivers[protocol];
            if (driver != null)
            {
                return driver.Translator;
            }
            return null;
        }

        public DbConnectionInfo Connection(string url)
        {
            DbProtocol protocol = DbServices.Url2Protocol(url);
            DbDriver driver = drivers[protocol];
            if (driver != null)
            {
                DbConnectionInfo ci = driver.Connection();
                ci.Load(url);
                return ci;
            }
            return null;
        }

        public DbSession Session(string url)
        {
            DbProtocol protocol = DbServices.Url2Protocol(url);
            DbDriver driver = drivers[protocol];
            if (driver != null)
            {
                DbSession session = driver.Session();
                session.Url = url;
                return session;
            }
            return null;

        }

        #endregion

    }


}
