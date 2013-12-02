using System;

namespace Sikia.Framework
{
    using Sikia.Framework.Model;
    using Sikia.Framework.Types;

    public enum ObjectState
    {
        Iddle = 0,
        Creating = 1,
        Loading = 2,
        Saving = 4,
        Deleting = 8
    }

    public class InterceptedObject
    {
        #region Warnings & Errors
        #endregion

        #region Errors
        #endregion

        #region State
        private ObjectState state = ObjectState.Iddle;
        public ObjectState State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
            }
        }
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
                    ci = ModelManager.Instance.Classes[tt.BaseType];
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

        #region Initialization
        public void AOPAfterCreate()
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
