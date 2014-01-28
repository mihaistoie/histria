using Sikia.Db.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sikia.Db.Model;
using Sikia.Db.SqlServer.Model;

namespace Sikia.Db
{

    public class DbDrivers
    {
        class DbDriver
        {
            private Type connection = null;
            private Type session = null;
            private Type structure = null;
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

            public DbSchema Structure()
            {
                return (DbSchema)Activator.CreateInstance(structure);
            }
            
            public DbDriver(Type ConnectionType, Type DbSessionType, Type TranslatorType, Type DbStructure)
            {
                Translator = (DbTranslator)Activator.CreateInstance(TranslatorType);
                connection = ConnectionType;
                session = DbSessionType;
                structure = DbStructure;
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
            drivers.Add(DbProtocol.nodb, new DbDriver(typeof(DbConnectionInfo), typeof(DbSession), typeof(DbTranslator), typeof(DbSchema)));
            drivers.Add(DbProtocol.mssql, new DbDriver(typeof(MsSqlConnectionInfo), typeof(MsSqlSession), typeof(MsSqlTranslator), typeof(MsSqlSchema)));
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
                session.Open();
                return session;
            }
            return null;

        }

        public DbSchema Schema(DbProtocol protocol)
        {
             DbDriver driver = drivers[protocol];
            if (driver != null)
            {
                return driver.Structure();
            }
            return null;

        }
        #endregion

    }


}
