using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sikia.Aop.Castle;
using Sikia.Framework.Model;


namespace Sikia.Aop
{
    public static class ModelFactory
    {
        public static T Create<T>()
        {
            var  instance = CastleFactory.Instance.CreateInstance<T>();
            if (instance is InterceptedObject)
            {
                InterceptedObject io = instance as InterceptedObject;
                io.ClassInfo = Model.Instance.ModelClasses[typeof(T)];
            }
            return instance;

        }
    }
}
