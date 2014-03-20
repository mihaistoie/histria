using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Core
{
    ///<summary>
    ///Represents  errors that occurs during execution
    ///</summary>      
    public class ExecutionException : Exception
    {
        public ExecutionException(string msg) : base(msg) { }

        public ExecutionException(string fmt, params object[] args)
            : base(string.Format(fmt, args))
        {
        }
    }


}
