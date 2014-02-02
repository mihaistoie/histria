using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Sikia.Core
{
    using Sikia.Core.Castle;
    using Sikia.Model;
    public static class ModelFactory
    {
        public static T Create<T>()
        {
            var  instance = CastleFactory.Instance.CreateInstance<T>();
            if (instance is InterceptedObject)
            {
                InterceptedObject io = instance as InterceptedObject;
                ModelManager model = ModelProxy.Model();
                io.ClassInfo = model.Classes[typeof(T)];
                io.AOPAfterCreate();
            }
            return instance;

        }
        public static void Install() {
            var  instance = CastleFactory.Instance;
        }
    }
}
