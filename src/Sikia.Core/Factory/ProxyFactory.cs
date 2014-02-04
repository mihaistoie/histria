using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Sikia.Core
{
    using Sikia.Model;
    public class ProxyFactory
    {

        #region Singleton thread-safe pattern
        private static volatile ProxyFactory instance = null;
        private IProxyFactory factory = null;
        private static object syncRoot = new Object();
        private ProxyFactory()
        {
        }
        public static ProxyFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            ProxyFactory cf = new ProxyFactory();
                            instance = cf;
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        public IProxyFactory Factory
        {
            get { return factory; }
            set { factory = value; }
        }

        public static T Create<T>()
        {
            if (instance.Factory != null)
            {
                return instance.Factory.Resolve<T>();
            }
            return default(T);
           
        }
    }
}
