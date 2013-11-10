using Sikia.Framework.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sikia.Framework.DataModel
{

    class EnumCollection : KeyedCollection<Type, EnumInfoItem>
    {
        protected override Type GetKeyForItem(EnumInfoItem item)
        {
            return item.EnumType;
        }
    }
    class ClassCollection : KeyedCollection<Type, ClassInfoItem>
    {
        protected override Type GetKeyForItem(ClassInfoItem item)
        {
            return item.ClassTypeInfo;
        }
    }

    public sealed class Model
    {
        private static readonly Lazy<Model> lazy = new Lazy<Model>(() => new Model());

        public static Model Instance { get { return lazy.Value; } }

        private readonly KeyedCollection<Type, EnumInfoItem> enums;
        private readonly KeyedCollection<Type, ClassInfoItem> classes;
        public Model()
        {
            enums = new EnumCollection();
            classes = new ClassCollection();

        }
        public KeyedCollection<Type, EnumInfoItem> Enums { get { return enums; } }
        public KeyedCollection<Type, ClassInfoItem> Classes { get { return classes; } }

    }
}