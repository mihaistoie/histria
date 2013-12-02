namespace Sikia.Framework
{
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false)]
    public class DbColumnAttribute : System.Attribute
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
