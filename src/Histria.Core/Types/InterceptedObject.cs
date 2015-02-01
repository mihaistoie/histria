using System;

namespace Histria.Core
{
    using System.Linq;
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

        public bool IsDisposing
        {
            get { return (status & ObjectStatus.Disposing) == ObjectStatus.Disposing; }
        }
        private IDictionary<string, PropertyState> propsState;

        public IDictionary<string, PropertyState> Properties
        {
            get
            {
                if (propsState == null && ClassInfo != null)
                {
                    AOPInitializeStates();
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
            bool result = (status & ObjectStatus.Active) == ObjectStatus.Active;
            return result;
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

        private bool IsActive()
        {
            return (status & ObjectStatus.Active) == ObjectStatus.Active;
        }

        private bool CanNotifyChanges()
        {
            return IsActive();
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


        void IObjectLifetime.Notify(ObjectLifetimeEvent objectLifetime, params object[] arguments)
        {
            if (CanNotifyChanges())
            {
                AOPNotify(objectLifetime, arguments);
            }
        }

        protected virtual void AOPNotify(ObjectLifetimeEvent objectLifetime, object[] arguments)
        {
            //Nothing to do
            //AOP
        }

        #endregion

        #region Properties

        private Guid uuid = Guid.Empty;
        public virtual Guid Uuid
        {
            get
            {
                AllocId();
                return uuid;
            }
            set { uuid = value; }
        }

        private void AllocId()
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
                    ci = model.ViewsAndClasses[tt.BaseType];
                }
                return ci;
            }
            set
            {
                ci = value;
            }
        }
        #endregion

        #region Initialization

        public Container Container { get; internal set; }




        ///<summary>
        /// IInterceptedObject.AOPAfterCreate
        ///</summary>
        void IInterceptedObject.AOPCreate()
        {
            AddState(ObjectStatus.InCreating);
            try
            {
                AOPInitializeAssociations();
                AOPInitializeProperties();
            }
            finally
            {
                RmvState(ObjectStatus.InCreating);
                AddState(ObjectStatus.Created | ObjectStatus.Active);
            }
            ((IObjectLifetime)this).Notify(ObjectLifetimeEvent.Created, this);
            if (this.CanExecuteRules(Rule.AfterCreate))
            {
                ClassInfo.ExecuteRules(Rule.AfterCreate, this);
                Frozen(() => { ClassInfo.ExecuteStateRules(Rule.AfterCreate, this); });
            }
        }

        ///<summary>
        /// IInterceptedObject.AOPAfterLoad
        ///</summary>
        void IInterceptedObject.AOPBeginLoad()
        {
            AddState(ObjectStatus.InLoading);
            AOPInitializeAssociations();
            AOPInitializeProperties();
        }

        ///<summary>
        /// IInterceptedObject.AOPEndLoad
        ///</summary>
        void IInterceptedObject.AOPEndLoad()
        {
            RmvState(ObjectStatus.InLoading);
            AddState(ObjectStatus.Loaded | ObjectStatus.Active);
            ((IObjectLifetime)this).Notify(ObjectLifetimeEvent.Loaded);
            if (this.CanExecuteRules(Rule.AfterLoad))
            {
                ClassInfo.ExecuteRules(Rule.AfterLoad, this);
                Frozen(() => { ClassInfo.ExecuteStateRules(Rule.AfterLoad, this); });
            }
        }

        private void AOPInitializeAssociations()
        {
            foreach (PropInfoItem pp in this.ClassInfo.Roles)
            {
                Association roleInstance = AssociationHelper.AssociationFactory(pp, pp.PropInfo.PropertyType);
                roleInstance.PropInfo = pp;
                roleInstance.Instance = this;
                pp.PropInfo.SetValue(this, roleInstance, null);
            }

            //  create memo / binary instances
        }

        /// <summary>
        /// Initialize properties
        /// </summary>
        private void AOPInitializeProperties()
        {
            foreach (PropInfoItem pi in this.ClassInfo.Properties)
            {
                if (pi.DefaultValue != null)
                {
                    pi.PropInfo.SetValue(this, pi.DefaultValue, null);
                }
            }
        }

        /// <summary>
        /// Initialize states
        /// </summary>
        private void AOPInitializeStates()
        {
            this.propsState = (IDictionary<string, PropertyState>)Activator.CreateInstance(this.ClassInfo.StateClassType);
            foreach (PropInfoItem pi in this.ClassInfo.Properties)
            {
                PropertyState ps = this.Container.Create<PropertyState>();
                ps.Initialize((IObjectLifetime)this, pi);
                this.propsState.Add(pi.Name, ps);
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
                string s = AssociationHelper.ObjectPath(this, ref canBeCached);
                if (canBeCached)
                    objectPath = s;
                else
                    return s;

            }
            return objectPath;
        }

        private bool fromModelToViewValueFlow;

        ///<summary>
        /// IInterceptedObject.AOPBeforeSetProperty
        ///</summary>
        bool IInterceptedObject.AOPBeforeSetProperty(string propertyName, ref object value, ref object oldValue)
        {
            if (InterceptSet())
            {
                PropInfoItem pi = ClassInfo.PropertyByName(propertyName);
                if (pi.IsReadOnly) return false;
                if (!pi.CanGetValueByReflection)
                {
                }
                else
                    oldValue = pi.PropInfo.GetValue(this, null);

                if (pi.ModelPropInfo != null && !fromModelToViewValueFlow)
                {
                    InterceptedObject model = this.GetModel();
                    if (model != null)
                    {
                        model.SetPropertyValue(propertyName, value);
                        return false;
                    }
                }
                pi.SchemaValidation(ref value);
                if (CanExecuteRules(Rule.Correction))
                    pi.ExecuteCheckValueRules(this, ref value);
                bool changed = pi.PropInfo.PropertyType.IsValueType ? !value.Equals(oldValue) : value != oldValue;
                if (!changed) return false;
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
                foreach (ViewObject view in this.Views.Instances)
                {
                    view.fromModelToViewValueFlow = true;
                    try
                    {
                        view.SetPropertyValue(propertyName, newValue);
                    }
                    finally
                    {
                        view.fromModelToViewValueFlow = false;
                    }
                }

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
                object[] arguments = new object[] { child };
                if (CanExecuteRules(Rule.Validation))
                {
                    Frozen(() => { pi.ExecuteRules(Rule.Validation, this, operation, arguments); });
                }
                // Propagate
                if (CanExecuteRules(Rule.Propagation))
                {
                    this.Container.PropertyChangedStack.Push(this, propertyName);
                    try
                    {
                        Frozen(() => { pi.ExecuteStateRules(Rule.Propagation, this, operation, arguments); });
                        pi.ExecuteRules(Rule.Propagation, this, operation, arguments);
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
                if (AssociationHelper.RemoveMeFromParent(this))
                {
                    return;
                }
            }
            List<InterceptedObject> toDelete = new List<InterceptedObject>() { this };
            AssociationHelper.EnumChildren(this as IInterceptedObject, true, (x) => { toDelete.Add((InterceptedObject)x); });

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
                        AssociationHelper.CkeckConstraints(x as IInterceptedObject);
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
            AssociationHelper.RemoveChildren(this as IInterceptedObject);


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

        #region Rules
        public bool IsComingFrom(string search)
        {
            return this.Container.IsComingFrom(this, search);
        }
        #endregion

        #region Memory

        public void CleanObject()
        {
            status = ObjectStatus.Disposing;
            //TODO clean views
        }

        #endregion

        #region Views

        private ObjectRefCollection<ViewObject> views;
        internal ObjectRefCollection<ViewObject> Views
        {
            get
            {
                if (this.views == null)
                {
                    this.views = new ObjectRefCollection<ViewObject>();
                }
                return this.views;
            }
        }

        internal void SetPropertyValue(string propertyName, object value)
        {
            PropInfoItem pi = this.ClassInfo.PropertyByName(propertyName);
            if (pi.IsRole)
            {
                throw new InvalidOperationException(String.Format("Property {0} is a role", propertyName));
            }
            try
            {
                pi.PropInfo.SetValue(this, value, null);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }

        internal InterceptedObject GetModel()
        {
            ViewInfoItem vi = this.ClassInfo as ViewInfoItem;
            if (vi == null)
            {
                return null;
            }
            Type iViewObjectType = typeof(IViewModel<>).MakeGenericType(new Type[] { vi.ModelClass.TargetType });
            if (this.GetType().GetInterfaces().Where(i => i == iViewObjectType).SingleOrDefault() != null)
            {
                return iViewObjectType.GetProperty("Model").GetValue(this, null) as InterceptedObject;
            }
            return null;
        }
        #endregion

    }

}
