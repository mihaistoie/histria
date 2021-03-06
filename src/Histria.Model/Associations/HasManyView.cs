﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Model
{
    internal class HasManyView<T> : HasMany<T> where T : IInterceptedObject
    {
        private Association model;
        protected Association Model
        {
            get { return this.model; }
            set
            {
                if (value == null)
                {
                    if (this.model != null)
                    {
                        this.model.AssociationViews.Remove(this);
                    }
                }
                this.model = value;
                if (this.model != null)
                {
                    if (!this.model.AssociationViews.Contains(this))
                    {
                        this.model.AssociationViews.Add(this);
                    }
                }
            }
        }

        protected override bool BeforeAddValue(T item)
        {
            IInterceptedObject child = item as IInterceptedObject;
            return Instance.AOPBeforeChangeChild(PropInfo.Name, child, RoleOperation.Add);
        }
        protected override void AfterAddValue(T item)
        {
            IInterceptedObject child = item as IInterceptedObject;
            (Instance as IObjectLifetime).Notify(ObjectLifetimeEvent.AssociationsChanged, PropInfo.Name, this);
            Instance.AOPAfterChangeChild(PropInfo.Name, child, RoleOperation.Add);
        } 

        public override void Remove(T item)
        {
            IInterceptedObject child = item as IInterceptedObject;

            if (!Instance.AOPBeforeChangeChild(PropInfo.Name, child, RoleOperation.Remove))
            {
                return;
            }
            item.AOPDelete(false);
            (this as IRoleParent).RemoveChild(item);
            (Instance as IObjectLifetime).Notify(ObjectLifetimeEvent.AssociationsChanged, PropInfo.Name, this);
            Instance.AOPAfterChangeChild(PropInfo.Name, child, RoleOperation.Remove);
        }
    }
}
