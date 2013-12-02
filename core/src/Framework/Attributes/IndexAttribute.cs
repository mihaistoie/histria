namespace Sikia.Framework
{    
    /// <summary>
    /// Allow to define one or more indexes for a Class 
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true)]
    public class IndexAttribute : System.Attribute
    {
        public string Columns = "";
        public string Name = "";
        public bool Unique = false;
        public IndexAttribute(string iColumns) 
        {
            Columns = iColumns;
        }
    }
}
