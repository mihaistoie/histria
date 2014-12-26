namespace Histria.Model
{
    /// <summary>
    /// Used to indicate that a classname/column name is no persistent
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Property, AllowMultiple = false)]
    public class NoPersistentlAttribute : System.Attribute
    {
  

    }
}

