namespace Histria.Model
{
    /// <summary>
    /// Allow to define the table name for a class
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
    public class DbAttribute : System.Attribute
    {
        public string TableName {get; set;}
        public DbAttribute()
        {
        }

        public DbAttribute(string iTableName)
        {
            TableName = iTableName;
        }

    }
}
