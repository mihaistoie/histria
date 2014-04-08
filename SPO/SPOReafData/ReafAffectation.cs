namespace SPOReaf
{
    using Histria;
    using Histria.Core;
    using Histria.Model;

    public partial class ReafAffectation : InterceptedObject
    {
        [Association(Relation.Composition, Inv = "Affectations")]
        public virtual BelongsTo<ReafEngagement> Engagement { get; set; }

        [Association(Relation.Composition, Inv = "Affectation")]
        public virtual HasMany<ReafAffectationProg> ProgAffectations { get; set; }

        [Display("Poste", Description = "Information poste")]
        [Association(Relation.Association, ForeignKey = "IdPoste")]
        public virtual HasOne<ReafPosteInfo> PosteInfo { get; set; }

        [Display("Code poste")]
        public virtual string IdPoste { get; set; }

        [Display("Taux TVA")]
        [DtNumber(Template = "TauxTVA")]
        public virtual decimal TauxTVA { get; set; }

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

        [Display("Libellé")]
        public virtual string Libelle { get; set; }

        [Display("Entreprise")]
        public virtual string Entreprise { get; set; }

        //TODO idmontant, idcontrat, idfact,tranche, avenant, lot, phase
    }
}