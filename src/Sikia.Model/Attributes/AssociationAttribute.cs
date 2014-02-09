namespace Sikia.Model
{
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false)]
    public class AssociationAttribute : System.Attribute
    {
        public string Inv;
        public Relation Type;
        public string Link;
        public int Min = 0;
        public int Max = -1;
        public AssociationAttribute(Relation type)
        {
        }
    }
}
