using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Sikia.Framework
{
    using Sikia.Aop.Castle;
    using Sikia.Framework.Model;
    public static class ModelFactory
    {
        public static T Create<T>()
        {
            var  instance = CastleFactory.Instance.CreateInstance<T>();
            if (instance is InterceptedObject)
            {
                InterceptedObject io = instance as InterceptedObject;
                io.ClassInfo = ModelManager.Instance.Classes[typeof(T)];
                io.AOPAfterCreate();
            }
            return instance;

        }
        public static void Install() {
            var  instance = CastleFactory.Instance;
        }
    }
}
