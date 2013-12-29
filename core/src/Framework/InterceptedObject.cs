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
        Deleting = 8,
        Disposing = 16,
        Frozen = 32
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

        private bool canExecuteRules()
        {
            if ((state & ObjectState.Disposing) == ObjectState.Disposing)
                return false;
            if ((state & ObjectState.Frozen) == ObjectState.Frozen)
                return false;
            return true;
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
            AOPInitializeAssociations();
            ClassInfo.ExecuteRules(Rule.AfterCreate, this);
            state = ObjectState.Iddle;
        }

        #endregion

        #region Intercepors
        public bool AOPBeforeSetProperty(string propertyName, ref object value)
        {
            if (!canExecuteRules()) return true;
            PropinfoItem pi = ClassInfo.PropertyByName(propertyName);
            object oldValue = pi.PropInfo.GetValue(this, null);
            pi.SchemaValidation(ref value);
            if (oldValue == value) return false;
            CheckInTransaction();
            return true;

        }

        public void AOPAfterSetProperty(string propertyName, object value)
        {
            if (!canExecuteRules()) return;
            PropinfoItem pi = ClassInfo.PropertyByName(propertyName);
            // Validate
            pi.ExecuteRules(Rule.Validation, this);
            // Propagate
            pi.ExecuteRules(Rule.Propagation, this);

        }

        public void AOPInitializeAssociations()
        {
        }
        #endregion

        #region Memory

        public void CleanObject()
        {
            state = ObjectState.Disposing;
        }

        #endregion
    }

}
