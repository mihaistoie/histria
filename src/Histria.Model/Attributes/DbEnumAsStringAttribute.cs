namespace Histria.Model
{
    /// <summary>
    /// Enums are saved as string
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Enum, AllowMultiple = false)]
    public class DbEnumAsStringAttribute : System.Attribute
    {
        public DbEnumAsStringAttribute()
        {
        }

    }
}
