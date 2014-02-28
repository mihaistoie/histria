using System;

namespace Sikia.Core
{
    using Sikia.Model;
    using System.Reflection;

    public class InterceptedObject : IModelClass, IInterceptedObject
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
            if ((state & ObjectState.Creating) == ObjectState.Disposing)
                return false;
            if ((state & ObjectState.Loading) == ObjectState.Disposing)
                return false;
            if ((state & ObjectState.Disposing) == ObjectState.Disposing)
                return false;
            if ((state & ObjectState.Frozen) == ObjectState.Frozen)
                return false;
            return true;
        }

        private bool canNotifyChanges()
        {
            if ((state & ObjectState.Creating) == ObjectState.Disposing)
                return false;
            if ((state & ObjectState.Loading) == ObjectState.Disposing)
                return false;
            if ((state & ObjectState.Disposing) == ObjectState.Disposing)
                return false;
            if ((state & ObjectState.Frozen) == ObjectState.Frozen)
                return false;

            return true;
        }
        #endregion

        #region Properties
        private Guid uuid = default(Guid);
        public Guid Uuid
        {
            get { return uuid; }
            set { uuid = value; }
        }
        public void AllocId()
        {
            if (uuid == default(Guid))
                uuid = Guid.NewGuid();
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
                    ModelManager model = ModelProxy.Model();
                    ci = model.Classes[tt.BaseType];
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

        ///<summary>
        /// IInterceptedObject.AOPAfterCreate
        ///</summary>
        public void AOPAfterCreate()
        {
            state = ObjectState.Creating;
            AOPInitializeAssociations();
            ClassInfo.ExecuteRules(Rule.AfterCreate, this);
            state = ObjectState.Iddle;
        }

        private void AOPInitializeAssociations()
        {
            MethodInfo method = typeof(ProxyFactory).GetMethod("Create");

            for (int index = 0; index < ClassInfo.Roles.Count; index++)
            {
                PropInfoItem pp = ClassInfo.Roles[index];
                Type tt = pp.PropInfo.PropertyType;
                MethodInfo genericMethod = method.MakeGenericMethod(tt);
                Association roleInstance = (Association)genericMethod.Invoke(null, null);
                roleInstance.PropInfo = pp;
                roleInstance.Instance = this;
                pp.PropInfo.SetValue(this, roleInstance, null);
            }
        }
        #endregion

        #region Interceptors
        ///<summary>
        /// IInterceptedObject.AOPBeforeSetProperty
        ///</summary>

        public bool AOPBeforeSetProperty(string propertyName, ref object value, ref object oldValue)
        {
            if (!canExecuteRules()) return true;
            PropInfoItem pi = ClassInfo.PropertyByName(propertyName);
            oldValue = pi.PropInfo.GetValue(this, null);
            pi.SchemaValidation(ref value);
            if (oldValue == value) return false;
            CheckInTransaction();
            return true;

        }
        ///<summary>
        /// IInterceptedObject.AOPAfterSetProperty
        ///</summary>
        public void AOPAfterSetProperty(string propertyName, object newValue, object oldValue)
        {
            if (!canExecuteRules()) return;
            PropInfoItem pi = ClassInfo.PropertyByName(propertyName);
            // Validate
            pi.ExecuteRules(Rule.Validation, this);
            // Propagate
            pi.ExecuteRules(Rule.Propagation, this);

        }

        ///<summary>
        /// Before modifying a role (add/remove/update)
        ///</summary>
        public bool AOPBeforeChangeChild(string propertyName, IInterceptedObject child, RoleOperation operation)
        {
            return true;
        }
        ///<summary>
        /// After modifying a role (add/remove/update)
        ///</summary>
        public void AOPAfterChangeChild(string propertyName, IInterceptedObject child, RoleOperation operation)
        {
        }

        ///<summary>
        /// IInterceptedObject.AOPDeleted
        ///</summary>
        public void AOPDeleted()
        {
            state = ObjectState.Deleting;
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
