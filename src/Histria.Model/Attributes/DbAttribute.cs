namespace Histria.Model
{
    /// <summary>
    /// Allow to define the table name for a class
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Property, AllowMultiple = false)]
    public class DbAttribute : System.Attribute
    {
        public string Name {get; set;}
        public bool IsPersistent = true;
        public DbAttribute()
        {
        }

        public DbAttribute(string iName)
        {
            Name = iName;
        }

    }
}
