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
        internal void Push(IInterceptedObject io, string propName)
        {
            DoPush(string.Format("{0}.{1}", io.ObjectPath(), propName));
        }

        internal void Pop()
        {
            DoPop();
        }

        internal void Clear()
        {
            DoClear();
        }
        ///<summary>
        /// Which property changed called me?
        ///</summary>
        internal bool IsComingFrom(IInterceptedObject io, string property)
        {
            string variable;
            bool canFind = true;
            string path = AssociationHelper.ExpandSearchPath(io, property, out variable, out canFind);
            if (!canFind)
            {
                return false;
            }
            return Contains(path, variable);
        }

    
    }
}
