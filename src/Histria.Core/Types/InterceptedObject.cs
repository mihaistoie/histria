using System;

namespace Histria.Core
{
    using Histria.Core.Execution;
    using Histria.Model;
    using System.Reflection;

    public class InterceptedObject : IClassModel, IInterceptedObject, IObjectLifetime
    {
        #region Warnings & Errors
        #endregion

        #region Errors
        #endregion

        #region State & Notifications
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
            return (state & ObjectState.Iddle) == ObjectState.Iddle;
        }

        private bool canNotifyChanges()
        {
            return (state & ObjectState.Iddle) == ObjectState.Iddle;
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
        bool IInterceptedObject.CanExecuteRules { get { return canExecuteRules(); } }
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

        public Container Container { get; set; }

        #region Initialization

        ///<summary>
        /// IInterceptedObject.AOPAfterCreate
        ///</summary>
        void IInterceptedObject.AOPCreate()
        {
            state = ObjectState.Creating;
            try
            {
                AOPInitializeAssociations();
            }
            finally
            {
                state = ObjectState.Iddle;
            }
            ((IObjectLifetime)this).Notify(ObjectLifetimeEvent.Created);
            if (this.canExecuteRules())
                ClassInfo.ExecuteRules(Rule.AfterCreate, this);
        }

        ///<summary>
        /// IInterceptedObject.AOPAfterLoad
        ///</summary>
        void IInterceptedObject.AOPLoad<T>(Action<T> loadAction)
        {
            state = ObjectState.Loading;
            try
            {
                AOPInitializeAssociations();
                loadAction(this as T);
            }
            finally
            {
                state = ObjectState.Iddle;
            }
            ((IObjectLifetime)this).Notify(ObjectLifetimeEvent.Loaded);
            if (this.canExecuteRules())
                ClassInfo.ExecuteRules(Rule.AfterLoad, this);
        }

        private void AOPInitializeAssociations()
        {
            for (int index = 0; index < ClassInfo.Roles.Count; index++)
            {
                PropInfoItem pp = ClassInfo.Roles[index];
                Association roleInstance = Association.AssociationFactory(pp, pp.PropInfo.PropertyType);
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
            pi.ExecuteCheckValueRules(this, ref value);
            if (oldValue == value) return false;
            return true;
        }

        ///<summary>
        /// IInterceptedObject.AOPAfterSetProperty
        ///</summary>
        void IInterceptedObject.AOPAfterSetProperty(string propertyName, object newValue, object oldValue)
        {
            (this as IObjectLifetime).Notify(ObjectLifetimeEvent.Changed, propertyName, oldValue, newValue);
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
        void IInterceptedObject.AOPDeleted(bool notifyParent)
        {
            if (canExecuteRules())
            {
                ClassInfo.ExecuteRules(Rule.BeforeDelete, this);
            }
            //Delete children  
            Association.RemoveChildren(this as IInterceptedObject);

            ((IObjectLifetime)this).Notify(ObjectLifetimeEvent.Deleted);
            state = ObjectState.Deleting;
        }
        #endregion

        #region Memory

        public void CleanObject()
        {
            state = ObjectState.Disposing;
        }

        #endregion

        void IObjectLifetime.Notify(ObjectLifetimeEvent lifetimeEvent, params object[] arguments)
        {
            //Nothing to do
            //used by AOP interception
        }
    }

}
