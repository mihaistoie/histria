﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace Histria.Model
{
    using Histria.Sys;

    public class EnumInfoItem : Dictionary<int, string>
    {
        public Type EnumType { get; set; }
        public bool StoredAsString;
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string EnumAsString(object enumValue)
        {
            return Enum.GetName(this.EnumType, enumValue);
        }

        public EnumInfoItem(Type enumType)
        {
            DisplayAttribute da = enumType.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;
            this.EnumType = enumType;
            this.Name = EnumType.Name;
            this.Title = this.Name;
            if (da != null)
            {
                this.Title = da.Title;
                this.Description = da.Description;
            }
            if (string.IsNullOrEmpty(Description)) this.Description = this.Title;

            DbAttribute dba = enumType.GetCustomAttributes(typeof(DbAttribute), false).FirstOrDefault() as DbAttribute;
            this.StoredAsString = (dba != null && dba.EnumStoredAsString);

            foreach (var enumValue in Enum.GetValues(enumType))
            {
                string svalue = Enum.GetName(enumType, enumValue);
                MemberInfo mi = enumType.GetMember(svalue).FirstOrDefault();
                DisplayAttribute eda = mi.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;
                if (eda != null)
                {
                    svalue = eda.Title;
                }
                this.Add((int)enumValue, svalue);

            }


        }
    }
}
