namespace SPOReaf
{
    using Histria.Model;
    using SPOReaf.Tools;

    public partial class ReafAffectationProg
    {
        [RulePropagation("HT")]
        public void HTChanged()
        {
            this.SetTVAandTTC();
            
            if (!this.IsComingFrom("Affectation.HT"))
            {
                this.Affectation.Value.ConsolidateHT();
            }
        }

        public void SetTVAandTTC()
        {
            var tauxTVA = this.Affectation.Value.TauxTVA;

            this.TVA = MathUtil.Round2(tauxTVA * this.HT / 100);
            this.TTC = this.HT + this.TVA;
        }
    }
}