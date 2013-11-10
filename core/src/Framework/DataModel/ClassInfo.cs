using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Sikia.Framework.Attributes;
using Sikia.Framework.Utils;

namespace Sikia.Framework.DataModel
{
    public enum ClassType { ctModel, ctViewModel };
    public class ClassInfoItem
    {
        public Type ClassTypeInfo { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ClassInfoItem(Type cType) {
            ClassTypeInfo = cType;
            Name = StrUtils.Namespace2Name(ClassTypeInfo.ToString());
            Title = Name;

            DisplayAttribute da = ClassTypeInfo.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;
            Title = Name;
            if (da != null)
            {
                Title = da.Title;
                Description = da.Description;
            }
            if (Description == "") Description = Title;

            PropertyInfo[] props = ClassTypeInfo.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            
        }

    }
}

