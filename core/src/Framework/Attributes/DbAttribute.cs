﻿namespace Sikia.Framework.Attributes
{
    /// <summary>
    /// Allow to define the table name and  primary key for a class
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
    class DbAttribute : System.Attribute
    {
        public string Keys = "";
        public string TableName = "";
        public DbAttribute(string iKeys) 
        {
            Keys = iKeys;
        }


    }
}
