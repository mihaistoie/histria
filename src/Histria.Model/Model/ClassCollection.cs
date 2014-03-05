using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Histria.Model
{
    public class ClassCollection : KeyedCollection<Type, ClassInfoItem>
    {
        protected override Type GetKeyForItem(ClassInfoItem item)
        {
            return item.CurrentType;
        }
        public Type[] Types
        {
            get
            {
                if (this.Dictionary != null)
                {
                    return this.Dictionary.Keys.ToArray<Type>();
                }
                else
                {
                    return this.Select(i => this.GetKeyForItem(i)).ToArray();
                }
            }
        }
    }
}
