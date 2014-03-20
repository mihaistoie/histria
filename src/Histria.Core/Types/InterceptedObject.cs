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
        private ObjectStatus status = ObjectStatus.None;

        ///<summary>
        /// Object is deleted ?
        ///</summary>
        public bool IsDeleted
        {
            get { return (status & ObjectStatus.Deleted) == ObjectStatus.Deleted; }
        }
        public bool IsNewObject
        {
            get { return (status & ObjectStatus.Created) == ObjectStatus.Created; }
        }

        private IDictionary<string, PropertyState> propsState;
        public IDictionary<string, PropertyState> Properties
        {
            get
            {
                if (propsState == null && ClassInfo != null)
                {
                    propsState = (IDictionary<string, PropertyState>) Activator.CreateInstance(ClassInfo.StateClassType);
                    for (int idx = 0, len = ci.Properties.Count; idx < len; idx++)
                    {
                        PropInfoItem pi = ci.Properties[idx];
                        PropertyState ps = new PropertyState((IInterceptedObject)this, pi);
                        propsState.Add(pi.Name, ps);
                    }
                }
                return propsState;
            }
        }

        private bool CanExecuteRules(Rule ruleType)
        {
            return (status & ObjectStatus.Active) == ObjectStatus.Active;
        }

        private bool InterceptSet()
        {
            if ((status & ObjectStatus.Frozen) == ObjectStatus.Frozen)
            {
                throw new ExecutionException(L.T("You can't change the object in a state rule."));
            }
            return (status & ObjectStatus.Active) == ObjectStatus.Active;
        }


        private void AddState(ObjectStatus value)
        {
            status = status | value;
        }

        private bool HasState(ObjectStatus value)
        {
            return ((status | value) == value);
        }

        private void RmvState(ObjectStatus value)
        {
            status = status & ~value;
        }

        private bool canNotifyChanges()
        {
            return (status & ObjectStatus.Active) == ObjectStatus.Active;
        }
        private void Frozen(Action action)
        {
            AddState(ObjectStatus.Frozen);
            try
            {
                action();
            }
            finally
            {
                RmvState(ObjectStatus.Frozen);
            }

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
                    ModelManager model = this.Container.ModelManager;
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
            AddState(ObjectStatus.InCreating);
            try
            {
                AOPInitializeAssociations();
            }
            finally
            {
                RmvState(ObjectStatus.InCreating);
                AddState(ObjectStatus.Created | ObjectStatus.Active);
            }
            ((IObjectLifetime)this).Notify(ObjectLifetimeEvent.Created);
            if (this.CanExecuteRules(Rule.AfterCreate))
            {
                ClassInfo.ExecuteRules(Rule.AfterCreate, this);
                Frozen(() => { ClassInfo.ExecuteStateRules(Rule.AfterCreate, this); });
            }
        }

        ///<summary>
        /// IInterceptedObject.AOPAfterLoad
        ///</summary>
        void IInterceptedObject.AOPLoad<T>(Action<T> loadAction)
        {
            AddState(ObjectStatus.InLoading);
            try
            {
                AOPInitializeAssociations();
                loadAction(this as T);
            }
            finally
            {
                RmvState(ObjectStatus.InLoading);
                AddState(ObjectStatus.Loaded | ObjectStatus.Active);
            }
            ((IObjectLifetime)this).Notify(ObjectLifetimeEvent.Loaded);
            if (this.CanExecuteRules(Rule.AfterLoad))
            {
                ClassInfo.ExecuteRules(Rule.AfterLoad, this);
                Frozen(() => { ClassInfo.ExecuteStateRules(Rule.AfterLoad, this); });
            }
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
        /// IInterceptedObject.ObjectPath
        ///  ///</summary>
        private string objectPath;
        string IInterceptedObject.ObjectPath()
        {
            if (string.IsNullOrEmpty(objectPath))
            {
                bool canBeCached = false;
                string s = Association.ObjectPath(this, ref canBeCached);
                if (canBeCached)
                    objectPath = s;
                else
                    return s;

            }
            return objectPath;
        }

        ///<summary>
        /// IInterceptedObject.AOPBeforeSetProperty
        ///</summary>
        bool IInterceptedObject.AOPBeforeSetProperty(string propertyName, ref object value, ref object oldValue)
        {
            if (InterceptSet())
            {
                PropInfoItem pi = ClassInfo.PropertyByName(propertyName);
                if (pi.CanGetValueByReflection)
                    oldValue = pi.PropInfo.GetValue(this, null);
                pi.SchemaValidation(ref value);
                if (CanExecuteRules(Rule.Correction))
                    pi.ExecuteCheckValueRules(this, ref value);
                if (oldValue == value) return false;
            }
            return true;
        }

        ///<summary>
        /// IInterceptedObject.AOPAfterSetProperty
        ///</summary>
        void IInterceptedObject.AOPAfterSetProperty(string propertyName, object newValue, object oldValue)
        {
            if (InterceptSet())
            {
                (this as IObjectLifetime).Notify(ObjectLifetimeEvent.Changed, propertyName, oldValue, newValue);
                PropInfoItem pi = ClassInfo.PropertyByName(propertyName);
                // Validate
                if (CanExecuteRules(Rule.Validation))
                {
                    Frozen(() => { pi.ExecuteRules(Rule.Validation, this, RoleOperation.None); });
                }
                // Propagate
                if (CanExecuteRules(Rule.Propagation))
                {
                    this.Container.PropertyChangedStack.Push(this, propertyName);
                    try
                    {
                        Frozen(() => { pi.ExecuteStateRules(Rule.Propagation, this, RoleOperation.None); });
                        pi.ExecuteRules(Rule.Propagation, this, RoleOperation.None);
                    }
                    finally
                    {
                        this.Container.PropertyChangedStack.Pop();
                    }
                }
            }
        }


        ///<summary>
        /// Before modifying a role (add/remove/update)
        ///</summary>
        bool IInterceptedObject.AOPBeforeChangeChild(string propertyName, IInterceptedObject child, RoleOperation operation)
        {
            if (InterceptSet())
            {
                return true;
            }
            return true;
        }
        ///<summary>
        /// After modifying a role (add/remove/update)
        ///</summary>
        void IInterceptedObject.AOPAfterChangeChild(string propertyName, IInterceptedObject child, RoleOperation operation)
        {
            if (InterceptSet())
            {
                PropInfoItem pi = ClassInfo.PropertyByName(propertyName);
                // Validate
                if (CanExecuteRules(Rule.Validation))
                {
                    Frozen(() => { pi.ExecuteRules(Rule.Validation, this, operation); });
                }
                // Propagate
                if (CanExecuteRules(Rule.Propagation))
                {
                    this.Container.PropertyChangedStack.Push(this, propertyName);
                    try
                    {
                        Frozen(() => { pi.ExecuteStateRules(Rule.Propagation, this, operation); });
                        pi.ExecuteRules(Rule.Propagation, this, operation);
                    }
                    finally
                    {
                        this.Container.PropertyChangedStack.Pop();
                    }
                }
            }
        }

        ///<summary>
        /// IInterceptedObject.AOPDelete
        ///</summary>
        void IInterceptedObject.AOPDelete(bool notifyParent)
        {
            if (HasState(ObjectStatus.InDeleting) || HasState(ObjectStatus.Deleted))
                return;
            if (notifyParent)
            {
                if (Association.RemoveMeFromParent(this))
                {
                    return;
                }
            }
            List<InterceptedObject> toDelete = new List<InterceptedObject>() { this };
            Association.EnumChildren(this as IInterceptedObject, true, (x) => { toDelete.Add((InterceptedObject)x); });

            //Execute before
            toDelete.ForEach((x) =>
            {
                x.AddState(ObjectStatus.InDeleting);
                x.RmvState(ObjectStatus.Active);
            });
            try
            {
                toDelete.ForEach((x) =>
                {
                    if (!x.HasState(ObjectStatus.Created))
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
                    x.RmvState(ObjectStatus.InDeleting);
                    x.AddState(ObjectStatus.Active);
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
                x.RmvState(ObjectStatus.InDeleting);
                x.AddState(ObjectStatus.Deleted);
                ((IObjectLifetime)x).Notify(ObjectLifetimeEvent.Deleted);
            });

        }

        ///<summary>
        /// Remove 
        ///</summary>
        public void Delete()
        {
            (this as IInterceptedObject).AOPDelete(true);
        }

        #endregion


        #region Memory

        public void CleanObject()
        {
            status = ObjectStatus.Disposing;
        }

        #endregion

        void IObjectLifetime.Notify(ObjectLifetimeEvent objectLifetime, params object[] arguments)
        {
            //Nothing to do
            //used by AOP interception
        }
    }

}
