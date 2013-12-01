using System;
using Sikia.Framework.Model;
using Sikia.Framework.Types;
namespace Sikia.Aop
{
    public class InterceptedObject
    {
        #region Warnings & Errors
        #endregion

        #region Errors
        #endregion

        #region Model
        private ClassInfoItem ci = null;
        public ClassInfoItem ClassInfo
        {
            get
            {
                if (ci == null)
                {
                    Type tt = this.GetType();
                    ci = Model.Instance.Classes[tt.BaseType];  
                }
                return ci;
            }
            set
            {
                ci = value;
            }
        }
        #endregion

        #region Transaction
        protected void CheckInTransaction()
        {
        }
        #endregion

        #region Intercepors
        public bool AOPBeforeSetProperty(string propertyName, object target, object value)
        {
            PropinfoItem pi = ClassInfo.PropertyByName(propertyName);
            object oldValue = pi.PropInfo.GetValue(this, null);
            if (oldValue == value) return false;
            CheckInTransaction();
            return true;

        }
        public void AOPAfterSetProperty(string propertyName, object target, object value)
        {
            PropinfoItem pi = ClassInfo.PropertyByName(propertyName);
            // Validate
            // Propagate
            pi.ExecuteRules(RuleType.Propagation, target);

        }
        #endregion
    }

}
