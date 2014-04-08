namespace SPOReaf
{
    using Histria.Core;
    using Histria.Model;

    [PrimaryKey("IdProg")]
    public partial class ReafProgInfo : InterceptedObject
    {
        public virtual string IdProg { get; set; }

        public virtual string Libelle { get; set; }

        public virtual bool IsNewProg { get; set; }

        public virtual bool IsDeletedProg { get; set; }
    }
}