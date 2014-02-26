using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Model
{
    public class BelongsTo<T> : Association, IRoleRef, IRoleChild where T : IInterceptedObject
    {
        private IInterceptedObject _value;
        private Guid refuuid;
        private void setParent(IInterceptedObject value) 
        {
             _value = value;
        }
        public BelongsTo()
        {
        }

        void IRoleChild.SetParent(IInterceptedObject value)
        {
             setParent(value);
        }
        void IRoleRef.SetValue(IInterceptedObject value)
        {
        }

        public IInterceptedObject Value { get { return _value; } internal set { setParent(value); } }

    }
}
