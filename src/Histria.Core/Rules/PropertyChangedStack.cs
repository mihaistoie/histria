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
            DoPush(Association.ExpandPath(io, propName));
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
