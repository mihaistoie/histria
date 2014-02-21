using System;

namespace Sikia.Core
{
    using Sikia.Model;

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
            for (int index = 0; index <  ClassInfo.Properties.Count; index++) 
            {
                PropinfoItem pp = ClassInfo.Properties[index];
                if (pp.IsRole) 
                {
                  Type  tt = pp.PropInfo.GetType();
                  //Object oo = ProxyFactory.Create<pp.PropInfo.GetType()>();
                }
            }
            
        }
        #endregion

        #region Interceptors
        ///<summary>
        /// IInterceptedObject.AOPBeforeSetProperty
        ///</summary>

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
        ///<summary>
        /// IInterceptedObject.AOPAfterSetProperty
        ///</summary>
        public void AOPAfterSetProperty(string propertyName, object value)
        {
            if (!canExecuteRules()) return;
            PropinfoItem pi = ClassInfo.PropertyByName(propertyName);
            // Validate
            pi.ExecuteRules(Rule.Validation, this);
            // Propagate
            pi.ExecuteRules(Rule.Propagation, this);

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
