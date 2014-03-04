﻿using Sikia.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sikia.Model
{
    public class ViewInfoItem: ClassInfoItem
    {
        ///<summary>
        /// Is View ?
        ///</summary>   	
        internal override bool IsView { get { return true; } }


        protected override void InitializeView(ModelManager model)
        {
            ModelClass = model.ClassByType(CurrentType.GetGenericArguments()[0]);
            if (ModelClass == null)
            {
                throw new ModelException(String.Format(L.T("Invalid Model type \"{0}\" for the view \"{1}\"."), CurrentType.GetGenericArguments()[0], Name), Name);
            }

        }
        internal ClassInfoItem ModelClass { get; set; }
        

        public ViewInfoItem(Type cType)
            : base(cType, false)
        {
        }
    }
}
