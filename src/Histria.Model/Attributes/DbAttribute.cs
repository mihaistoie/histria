namespace Histria.Model
{
    /// <summary>
    /// Allow to define the table name for a class, column name, custom 
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Property | System.AttributeTargets.Enum, AllowMultiple = false)]
    public class DbAttribute : System.Attribute
    {
        public string Name {get; set;}
        /// <summary>
        /// Only for enums : Stored as an integer or a string 
        /// </summary>
        public bool EnumStoredAsString { get; set; }

        public DbAttribute(string iName)
        {
            this.Name = iName;
        }

        public DbAttribute()
        {
        }

    }
}
