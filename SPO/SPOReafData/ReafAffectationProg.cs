namespace SPOReaf
{
    using Histria;
    using Histria.Core;
    using Histria.Model;

    [Display("Affectation par programme")]
    public partial class ReafAffectationProg : InterceptedObject
    {
        [Display("Programme", Description = "Information programme")]
        [Association(Relation.Association, ForeignKey = "IdProg")]
        public virtual HasOne<ReafProgInfo> ProgInfo { get; set; }

        [Display("Affectation")]
        [Association(Relation.Composition, Inv = "ProgAffectations")]
        public virtual BelongsTo<ReafAffectation> Affectation { get; set; }

        [Display("Code programme")]
        public virtual string IdProg { get; set; }

        #region Montants

        [Display("HT")]
        [DtNumber(Template = "Montant2dec")]
        public virtual decimal HT { get; set; }

        [Display("TVA")]
        [DtNumber(Template = "Montant2dec")]
        public virtual decimal TVA { get; set; }

        [Display("TTC")]
        [DtNumber(Template = "Montant2dec")]
        public virtual decimal TTC { get; set; }

        #endregion Montants

        #region Montants avant réaffectation

        [Display("HT Avant", Description = "HT avant réaffectation")]
        [DtNumber(Template = "Montant2dec")]
        public virtual decimal OldHT { get; set; }

        //[Display("TVA Avant", Description = "TVA avant réaffectation")]
        //[DtNumber(Template = "Montant2dec")]
        //public virtual decimal OldTVA { get; set; }

        //[Display("TTC Avant", Description = "TTC avant réaffectation")]
        //[DtNumber(Template = "Montant2dec")]
        //public virtual decimal OldTTC { get; set; }

        #endregion Montants avant réaffectation
    }
}