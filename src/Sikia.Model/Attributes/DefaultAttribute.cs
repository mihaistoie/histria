namespace Sikia.Model
{
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false)]
    public class DefaultAttribute : System.Attribute
    {

        public bool Required = false;
        public bool Hidden = false;
        public bool Disabled = false;
        public string Value = string.Empty;
        public DefaultAttribute()
        {
        }
    }
}
