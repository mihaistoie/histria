using Histria.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Histria.Model
{
    ///<summary>
    /// Used to register an attribute 
    ///</summary>  
    public sealed class TemplateManager
    {
        #region Private Members
        private readonly Dictionary<string, TypeAttribute> _templates;
        #endregion

        #region Singleton thread-safe pattern
        private static volatile TemplateManager _instance = null;
        private static ReaderWriterLockSlim _syncRoot = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        private TemplateManager()
        {
            _templates = new Dictionary<string, TypeAttribute>();
        }

        public static TemplateManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _syncRoot.EnterWriteLock();
                    try
                    {
                        {
                            if (_instance == null)
                            {
                                TemplateManager tm = new TemplateManager();
                                _instance = tm;
                            }
                        }
                    }
                    finally
                    {
                        _syncRoot.ExitWriteLock();

                    }
                }
                return _instance;
            }
        }
        #endregion

        
        /// <summary>
        /// Register a template
        /// </summary>
        /// <param name="templateName">Template Name</param>
        /// <param name="template">Template value</param>
        public static void Register(string templateName, TypeAttribute template)  
        {
            TemplateManager tm = Instance;
            _syncRoot.EnterWriteLock();
            try
            {
                tm._templates[templateName] = template;
            }
            finally
            {
                _syncRoot.ExitWriteLock();

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateName"></param>
        /// <returns></returns>
        public static TypeAttribute Template(string templateName)
        {
            TypeAttribute res = null;
            TemplateManager tm = Instance;
            _syncRoot.EnterReadLock();
            try
            {
                tm._templates.TryGetValue(templateName, out res);
            }
            finally
            {
                _syncRoot.ExitReadLock();

            }
            return res;

        }

    }
}
