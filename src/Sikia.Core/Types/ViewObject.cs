using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Core
{
    using Sikia.Model;
    public class ViewObject : InterceptedObject, IViewModel 
    {
    }

    public class ViewObject<T> : ViewObject, IViewModel<T> where T : IInterceptedObject
    {
        public T Model { get; set; }
    }

}
