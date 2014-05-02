using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Histria.Core
{
    using Histria.Model;
    
    public abstract class ViewObject : InterceptedObject, IViewModel 
    {
    }

    public class ViewObject<T> : ViewObject, IViewModel<T> where T : InterceptedObject
    {
        public T Model { get; set; }

        internal void Init(T model)
        {
            this.Model = model;
            this.InitAttributes();
            this.InitAssociations();
            this.Model.Views.Add(this);
        }

        private void InitAssociations()
        {
            //TODO
        }

        private void InitAttributes()
        {
            ViewInfoItem viewInfo = this.ClassInfo as ViewInfoItem;
            var propertiesToCopy = from PropInfoItem p in viewInfo.Properties where p.ModelPropInfo != null && !p.IsRole select p;
            foreach (PropInfoItem dest in propertiesToCopy)
            {
                PropInfoItem source = dest.ModelPropInfo;
                object value = source.PropInfo.GetValue(this.Model, null);
                dest.PropInfo.SetValue(this, value, null);
            }
        }
    }

}
