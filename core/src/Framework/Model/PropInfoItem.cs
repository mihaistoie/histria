using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Sikia.Framework.Attributes;
using Sikia.Framework.Utils;

namespace Sikia.Framework.Model
{
    public class PropinfoItem
    {
        private string title;
        private string description;
        private MethodInfo titleGet = null;
        private MethodInfo descriptionGet = null;

        public PropertyInfo PropInfo;
        public string Name { get; set; }
        public string DbName { get; set; }
        public string Title { get { return titleGet == null ? title : (string)titleGet.Invoke(this, null); } }
        public string Description { get { return descriptionGet == null ? description : (string)descriptionGet.Invoke(this, null); } }
        public PropinfoItem(PropertyInfo cPi)
        {
            PropInfo = cPi;
            Console.WriteLine(" --> " + PropInfo.Name);
            Name = PropInfo.Name;
            DbName = PropInfo.Name;
            title = Name;
            DisplayAttribute da = PropInfo.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;
            title = Name;
            if (da != null)
            {
                title = da.Title;
                description = da.Description;
            }
            if (description == "") description = title;

        }
        public void AfterLoad(ClassInfoItem ci) 
        {
            Console.WriteLine(" <--> " + Name);
        }

    }
}

