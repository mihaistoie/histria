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
        public BelongsTo()
        {
        }
        public void SetParent(IInterceptedObject value)
        {
            _value = value;
        }
        public IInterceptedObject Value { get { return _value;  } internal set { SetParent(value); } }
    }
}
