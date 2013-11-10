﻿namespace Sikia.Framework.Attributes
{    
    /// <summary>
    /// Allow to define one or more indexes for a Class 
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true)]
    class IndexAttribute : System.Attribute
    {
        public string Columns = "";
        public bool Unique = false;
        public IndexAttribute(string iColumns) 
        {
            Columns = iColumns;
        }
    }
}
