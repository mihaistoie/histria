namespace Histria.Model
{
    [System.AttributeUsage(System.AttributeTargets.Property | System.AttributeTargets.Method, AllowMultiple = false)]
    public class DefaultAttribute : System.Attribute
    {

        public bool Required = false;
        public bool Hidden = false;
        public bool Disabled = false;
        public object Value = null;
        public DefaultAttribute()
        {
        }
    }
}
