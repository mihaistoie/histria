using System;
using Sikia.Framework.Model;
namespace Sikia.Aop
{
    public class InterceptedObject
    {
        #region Warnings & Errors
        #endregion

        #region Errors
        #endregion

        #region Model
        public ClassInfoItem ClassInfo;
        #endregion

        #region Transaction
        protected void CheckInTransaction()
        {
        }
        #endregion

        #region Intercepors
        public bool AOPBeforeSetProperty(string propertyName, object value)
        {

            PropinfoItem pi = ClassInfo.PropertyByName(propertyName);
            object oldValue = pi.PropInfo.GetValue(this, null);
            if (oldValue == value) return false;
            CheckInTransaction();
            return true;

        }
        public void AOPAfterSetProperty(string propertyName, object value)
        {
            //1 Validate  
        }
        #endregion
    }

}
