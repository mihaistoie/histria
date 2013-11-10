namespace Sikia.Framework.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false)]
    class DbColumnAttribute : System.Attribute
    {
        public string Name = "";
        public DbColumnAttribute(string iName) 
        {
            Name = iName;
        }
        public DbColumnAttribute()
        {
        }
    }
}
