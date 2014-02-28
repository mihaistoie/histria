using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Model
{
    public class BelongsTo<T> : Association, IRoleRef, IRoleChild where T : IInterceptedObject
    {
        private Guid refUid = Guid.Empty;
        private IInterceptedObject _value;
        private void setParent(IInterceptedObject value) 
        {
             _value = value;
             refUid = (_value != null) ? _value.Uuid : Guid.Empty;

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
        public Guid RefUid { get { return refUid; } }
        public IInterceptedObject Value { get { return _value; } internal set { setParent(value); } }

    }
}
