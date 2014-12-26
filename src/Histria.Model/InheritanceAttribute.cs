namespace Histria.Model
{
    /// <summary>
    /// Class Id for persitent inherited classes  
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
    public class InheritanceAttribute : System.Attribute
    {
        /// <summary>
        /// Name of enum propery used to ifentify the class (class ID column name)
        /// </summary>
        public string EnumProperty { get; set; }
        /// <summary>
        /// Value of EnumProperty for this class
        /// </summary>
        public int Value { get; set; }

        public InheritanceAttribute(string name, int value)
        {
            this.EnumProperty = name;
            this.Value = value;
        }

    }
}
