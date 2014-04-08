namespace SPOReaf
{
    using Histria;
    using Histria.Core;
    using Histria.Model;

    public partial class ReafEngagement : InterceptedObject
    {
        [Association(Relation.Composition, Inv = "Engagement")]
        public virtual HasMany<ReafAffectation> Affectations { get; set; }
    }
}