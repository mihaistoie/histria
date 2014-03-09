using System;

namespace Histria.Core
{
    using Histria.Core.Execution;
    using Histria.Model;
    using Histria.Sys;
    using System.Collections.Generic;
    using System.Reflection;

    public class InterceptedObject : IClassModel, IInterceptedObject, IObjectLifetime
    {
        #region Warnings & Errors
        #endregion

        #region Errors
        #endregion

        #region State & Notifications
        private ObjectState state = ObjectState.None;
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

        private bool CanExecuteRules(Rule ruleType)
        {
            return (state & ObjectState.Iddle) == ObjectState.Iddle;
        }
        private void AddState(ObjectState value)
        {
            state = state | value;
        }

        private bool HasState(ObjectState value)
        {
            return ((state | value) == value);
        }

        private void RmvState(ObjectState value)
        {
            state = state & ~value;
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
            AddState(ObjectState.InCreating);
            try
            {
                AOPInitializeAssociations();
            }
            finally
            {
                RmvState(ObjectState.InCreating);
                AddState(ObjectState.Created | ObjectState.Iddle);
            }
            ((IObjectLifetime)this).Notify(ObjectLifetime.Created);
            if (this.CanExecuteRules(Rule.AfterCreate))
                ClassInfo.ExecuteRules(Rule.AfterCreate, this);
        }

        ///<summary>
        /// IInterceptedObject.AOPAfterLoad
        ///</summary>
        void IInterceptedObject.AOPLoad<T>(Action<T> loadAction)
        {
            AddState(ObjectState.InLoading);
            try
            {
                AOPInitializeAssociations();
                loadAction(this as T);
            }
            finally
            {
                RmvState(ObjectState.InCreating);
                AddState(ObjectState.Loaded | ObjectState.Iddle);
            }
            ((IObjectLifetime)this).Notify(ObjectLifetime.Loaded);
            if (this.CanExecuteRules(Rule.AfterLoad))
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
            PropInfoItem pi = ClassInfo.PropertyByName(propertyName);
            if (pi.CanGetValueByReflection)
                oldValue = pi.PropInfo.GetValue(this, null);
            pi.SchemaValidation(ref value);
            if (CanExecuteRules(Rule.Correction))
                pi.ExecuteCheckValueRules(this, ref value);
            if (oldValue == value) return false;
            return true;
        }

        ///<summary>
        /// IInterceptedObject.AOPAfterSetProperty
        ///</summary>
        void IInterceptedObject.AOPAfterSetProperty(string propertyName, object newValue, object oldValue)
        {
            (this as IObjectLifetime).Notify(ObjectLifetime.Changed, propertyName, oldValue, newValue);
            PropInfoItem pi = ClassInfo.PropertyByName(propertyName);
            // Validate
            if (CanExecuteRules(Rule.Validation))
                pi.ExecuteRules(Rule.Validation, this, RoleOperation.None);
            // Propagate
            if (CanExecuteRules(Rule.Propagation))
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
            PropInfoItem pi = ClassInfo.PropertyByName(propertyName);
            // Validate
            if (CanExecuteRules(Rule.Validation))
                pi.ExecuteRules(Rule.Validation, this, operation);
            // Propagate
            if (CanExecuteRules(Rule.Propagation))
                pi.ExecuteRules(Rule.Propagation, this, operation);
        }

        ///<summary>
        /// IInterceptedObject.AOPDelete
        ///</summary>
        void IInterceptedObject.AOPDelete(bool notifyParent)
        {
            if (HasState(ObjectState.InDeleting) || HasState(ObjectState.Deleted))
                return; 
            List<InterceptedObject> toDelete = new List<InterceptedObject>() { this };
            Association.EnumChildren(this as IInterceptedObject, true, (x) => { toDelete.Add((InterceptedObject)x); });

            //Execute before
            toDelete.ForEach((x) =>
            {
                x.AddState(ObjectState.InDeleting);
                x.RmvState(ObjectState.Iddle);
            });
            try
            {
                toDelete.ForEach((x) =>
                {
                    if (!x.HasState(ObjectState.Created))
                    {
                        Association.CkeckConstraints(x as IInterceptedObject);
                    }

                    if (x.CanExecuteRules(Rule.BeforeDelete))
                    {
                        x.ClassInfo.ExecuteRules(Rule.BeforeDelete, x);
                    }
                });

            }
            catch
            {
                toDelete.ForEach((x) =>
                {
                    x.RmvState(ObjectState.InDeleting);
                    x.AddState(ObjectState.Iddle);
                });
                throw;
            }
            Association.RemoveChildren(this as IInterceptedObject);

            
            //Rules After delete 
            try
            {
                toDelete.ForEach((x) =>
                {
                    if (x.CanExecuteRules(Rule.AfterDelete))
                    {
                        x.ClassInfo.ExecuteRules(Rule.AfterDelete, x);
                    }
                });
            }
            catch (Exception e)
            {
                Logger.Error(Logger.RULES, e);
            }
            //Set state deleted
            toDelete.ForEach((x) =>
            {
                x.RmvState(ObjectState.InDeleting);
                x.AddState(ObjectState.Deleted);
                ((IObjectLifetime)x).Notify(ObjectLifetime.Deleted);
            });

        }
        #endregion

        #region Memory

        public void CleanObject()
        {
            state = ObjectState.Disposing;
        }

        #endregion

        void IObjectLifetime.Notify(ObjectLifetime objectLifetime, params object[] arguments)
        {
            //Nothing to do
            //used by AOP interception
        }
    }

}
