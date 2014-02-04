using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia
{
    public interface IInterceptedObject
    {
        #region Intercepors
        bool AOPBeforeSetProperty(string propertyName, ref object value);
        void AOPAfterSetProperty(string propertyName, object value);
        void AOPAfterCreate();
        #endregion
    }
}
