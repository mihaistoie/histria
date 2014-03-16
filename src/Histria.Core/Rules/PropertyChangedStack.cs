using Histria.Model;
using Histria.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Core
{
    public class PropertyChangedStack : BaseStack
    {
        public void Push(IInterceptedObject io, string propName)
        {
            DoPush(string.Format("{0}.{1}", io.ObjectPath(), propName));
        }

        public void Pop()
        {
            DoPop();
        }

        public void Clear()
        {
            DoClear();
        }
    }
}
