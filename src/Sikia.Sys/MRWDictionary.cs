using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Sikia
{
    public class MRWDictionary<T> 
    {
        private ReaderWriterLockSlim rwlock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        private Dictionary<string, T> dic = new Dictionary<string, T>();
        protected T Value(string key, Func<string, T> constructor)
        {
            bool tryAdd = false;
            T value = default(T);
           
            rwlock.EnterReadLock();
            try
            {
                if (dic.TryGetValue(key, out value))
                {
                    return value;
                }
                tryAdd = true;
            }
            finally
            {
                rwlock.ExitReadLock();
            }
            if (tryAdd)
            {
                rwlock.EnterWriteLock();
                try
                {
                    if (dic.TryGetValue(key, out value))
                    {
                        return value;
                    }
                    value = constructor(key);
                    dic.Add(key, value);
                    return value;
                }
                finally
                {
                    rwlock.ExitWriteLock();
                }
            }
            return value;
        }
    }
}
