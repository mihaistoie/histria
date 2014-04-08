namespace SPOReaf
{
    using Histria.Core;
    using Histria.Model;

    [PrimaryKey("IdPoste")]
    [Display("Info Poste")]
    public partial class ReafPosteInfo : InterceptedObject
    {
        [Display("Code poste")]
        public virtual string IdPoste { get; set; }

        [Display("Libellé", Description = "Libellé poste budgétaire")]
        public virtual string Libelle { get; set; }

        [Display("Répartition", Description = "Mode de répartition")]
        public virtual string ModeRepart { get; set; }
    }
}