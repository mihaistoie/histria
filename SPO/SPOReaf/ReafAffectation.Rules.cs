namespace SPOReaf
{
    using Histria.Model;
    using SPOReaf.Tools;

    public partial class ReafAffectation
    {
        [RulePropagation("HT")]
        public void VentilerHTRule()
        {
            if(this.IsComingFrom("ProgAffectations.HT"))
            {
                return;
            }
            this.VentilerHT();
        }

        [RulePropagation("HT")]
        [RulePropagation("TauxTVA")]
        public void SetTVAandTTC()
        {
            this.TVA = MathUtil.Round2(this.HT * this.TauxTVA / 100m);
            this.TTC = this.HT + this.TVA;
        }

        [RulePropagation("TauxTVA")]
        public void SetTVAInProgAff()
        {
            foreach (ReafAffectationProg progAff in this.ProgAffectations)
            {
                progAff.SetTVAandTTC();
            }
        }

        internal void ConsolidateHT()
        {
            decimal total = 0;
            foreach (ReafAffectationProg progAff in this.ProgAffectations)
            {
                total += progAff.HT;
            }
            this.HT = total;
        }

        internal void VentilerHT()
        {
            int cnt = this.ProgAffectations.Count;
            if (cnt == 0)
            {
                return;
            }
            //TODO
            decimal ventile = 0m;
            decimal part = MathUtil.Round2(this.HT / cnt);

            for (int i = 0; i < this.ProgAffectations.Count - 1; i++)
            {
                ReafAffectationProg progAff = this.ProgAffectations[i];
                progAff.HT = part;
                ventile += part;
            }
            this.ProgAffectations[cnt - 1].HT = this.HT - ventile;
        }
    }
}