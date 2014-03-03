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
            if ((state & ObjectState.Creating) == ObjectState.Creating)
                return false;
            if ((state & ObjectState.Loading) == ObjectState.Loading)
                return false;
            if ((state & ObjectState.Disposing) == ObjectState.Disposing)
                return false;
            if ((state & ObjectState.Frozen) == ObjectState.Frozen)
                return false;
            if ((state & ObjectState.Deleting) == ObjectState.Deleting)
                return false;
            
            return true;
        }

        private bool canNotifyChanges()
        {
            if ((state & ObjectState.Creating) == ObjectState.Creating)
                return false;
            if ((state & ObjectState.Loading) == ObjectState.Loading)
                return false;
            if ((state & ObjectState.Disposing) == ObjectState.Disposing)
                return false;
            if ((state & ObjectState.Frozen) == ObjectState.Frozen)
                return false;
            if ((state & ObjectState.Deleting) == ObjectState.Deleting)
                return false;

            return true;
        }
        #endregion

        #region Properties
        private Guid uuid = Guid.Empty;
        public Guid Uuid
        {
            get
            {
                AllocId();
                return uuid;
            }
            set { uuid = value; }
        }
        public void AllocId()
        {
            if (uuid == Guid.Empty)
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
        void IInterceptedObject.AOPAfterCreate()
        {
            state = ObjectState.Creating;
            AOPInitializeAssociations();
            ClassInfo.ExecuteRules(Rule.AfterCreate, this);
            state = ObjectState.Iddle;
        }

        private void AOPInitializeAssociations()
        {
            
            bool useFactory = false;
            MethodInfo method = useFactory ? typeof(ProxyFactory).GetMethod("Create") : null;

            for (int index = 0; index < ClassInfo.Roles.Count; index++)
            {
                PropInfoItem pp = ClassInfo.Roles[index];
                Association roleInstance = null;
                if (useFactory)
                {
                    Type tt = Association.AssociationType(pp, pp.PropInfo.PropertyType);
                    MethodInfo genericMethod = method.MakeGenericMethod(tt);
                    roleInstance = (Association)genericMethod.Invoke(null, null);
                }
                else
                {
                    roleInstance = Association.AssociationFactory(pp, pp.PropInfo.PropertyType);
                }
                
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

        bool IInterceptedObject.AOPBeforeSetProperty(string propertyName, ref object value, ref object oldValue)
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
        void IInterceptedObject.AOPAfterSetProperty(string propertyName, object newValue, object oldValue)
        {
            if (!canExecuteRules()) return;
            PropInfoItem pi = ClassInfo.PropertyByName(propertyName);
            // Validate
            pi.ExecuteRules(Rule.Validation, this, RoleOperation.None);
            // Propagate
            pi.ExecuteRules(Rule.Propagation, this, RoleOperation.None);
        }
   
        ///<summary>
        /// Before modifying a role (add/remove/update)
        ///</summary>
        bool IInterceptedObject.AOPBeforeChangeChild(string propertyName, IInterceptedObject child, RoleOperation operation)
        {
            return true;
        }
        ///<summary>
        /// After modifying a role (add/remove/update)
        ///</summary>
        void IInterceptedObject.AOPAfterChangeChild(string propertyName, IInterceptedObject child, RoleOperation operation)
        {
            if (!canExecuteRules()) return;
            PropInfoItem pi = ClassInfo.PropertyByName(propertyName);
            // Validate
            pi.ExecuteRules(Rule.Validation, this, operation);
            // Propagate
            pi.ExecuteRules(Rule.Propagation, this, operation);
        }

        ///<summary>
        /// IInterceptedObject.AOPDeleted
        ///</summary>
        void IInterceptedObject.AOPDeleted()
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
