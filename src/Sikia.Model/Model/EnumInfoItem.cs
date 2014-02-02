using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace Sikia.Model
{
    using Sikia.Sys;

    public class EnumInfoItem: Dictionary<int, string>
    {
        public Type EnumType { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public EnumInfoItem(Type enumType)
        {
            DisplayAttribute da = enumType.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;
            EnumType = enumType;
            Name = EnumType.Name;
            Title = Name;
            if (da != null)
            {
                Title = da.Title;
                Description = da.Description;
            }
            if (Description == "") Description = Title;
           
            foreach (var enumValue in Enum.GetValues(enumType)) 
            { 
                string svalue = Enum.GetName(enumType, enumValue);
                MemberInfo mi = enumType.GetMember(svalue).FirstOrDefault();
                DisplayAttribute eda = mi.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;
                if (eda != null)
                {
                    svalue = eda.Title;
                }
                Add((int)enumValue, svalue);
                
            }
           
            
        }
    }
}
