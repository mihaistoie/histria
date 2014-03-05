using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Core
{
    using Histria.Model;
    public class ViewObject : InterceptedObject, IViewModel 
    {
    }

    public class ViewObject<T> : ViewObject, IViewModel<T> where T : IInterceptedObject
    {
        public T Model { get; set; }
    }

}
