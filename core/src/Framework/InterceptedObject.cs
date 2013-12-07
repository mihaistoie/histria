using System;

namespace Sikia.Framework
{
    using Sikia.Framework.Model;

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
            state = ObjectState.Creating;
            ClassInfo.ExecuteRules(RuleType.AfterCreate, this);
            state = ObjectState.Iddle;
        }

        #endregion

        #region Intercepors
        public bool AOPBeforeSetProperty(string propertyName, ref object value)
        {
            PropinfoItem pi = ClassInfo.PropertyByName(propertyName);
            object oldValue = pi.PropInfo.GetValue(this, null);
            pi.ExecuteRules(RuleType.Propagation, this);
            if (oldValue == value) return false;
            CheckInTransaction();
            return true;

        }
        public void AOPAfterSetProperty(string propertyName, object value)
        {
            PropinfoItem pi = ClassInfo.PropertyByName(propertyName);
            // Validate
            pi.SchemaValidation(ref value);
            pi.ExecuteRules(RuleType.Validation, this);
            // Propagate
            pi.ExecuteRules(RuleType.Propagation, this);

        }
        #endregion
    }

}
