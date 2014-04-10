using System;
using Histria.Core.Execution;
using Histria.Json;
using Histria.Model;
using Histria.Sys;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPOReaf;

namespace SPO.Tests
{
    [TestClass]
    public class SpoReafTest
    {
        private static ModelManager model;
        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            string scfg = @"{""nameSpaces"": [""SPOReaf""]}";
            JsonObject cfg = (JsonObject)JsonValue.Parse(scfg);
            model = ModelManager.LoadModel(cfg);
            ModulePlugIn.Load("Histria.Proxy.Castle");
            ModulePlugIn.Initialize(model);
        }

        private ReafEngagement Load()
        {
            Container container = new Container(TestContainerSetup.GetSimpleContainerSetup(model));
            ReafProgInfo[] programmes = new ReafProgInfo[3];
            ReafPosteInfo[] postes = new ReafPosteInfo[3];
            ReafEngagement engagement = null;

            container.Load(() =>
            {
                for (int i = 0; i < programmes.Length; i++)
                {
                    ReafProgInfo p = container.Create<ReafProgInfo>();
                    p.IdProg = "P" + i.ToString();
                    p.Libelle = "Programme P" + i.ToString();
                    programmes[i] = p;
                }

                for (int i = 0; i < postes.Length; i++)
                {
                    ReafPosteInfo p = container.Create<ReafPosteInfo>();
                    p.IdPoste = "S" + i.ToString();
                    p.Libelle = "Poste S" + i.ToString();
                    postes[i] = p;
                }


                engagement = container.Create<ReafEngagement>();

                ReafAffectation affectation;
                affectation = container.Create<ReafAffectation>();
                engagement.Affectations.Add(affectation);
                affectation.TauxTVA = 20m;
                affectation.HT = 200m;
                affectation.TVA = 40m;
                affectation.TTC = 240m;
                affectation.PosteInfo.Value = postes[0];

                ReafAffectationProg affProg;

                for (int i = 0; i < 2; i++)
                {
                    affProg = container.Create<ReafAffectationProg>();
                    affectation.ProgAffectations.Add(affProg);
                    affProg.ProgInfo.Value = programmes[i];
                    affProg.HT = 100m;
                    affProg.TVA = 20m;
                    affProg.TTC = 120m;
                    affProg.OldHT = 100m;
                }
            });

            return engagement;
        }

        [TestMethod]
        public void SpoReafEngagementChangeHTinProgTest()
        {
            var engagement = Load();
            var apToChange = engagement.Affectations[0].ProgAffectations[0];
            apToChange.HT = 50m;
            Assert.AreEqual<decimal>(50m, apToChange.HT);
            Assert.AreEqual<decimal>(10m, apToChange.TVA);
            Assert.AreEqual<decimal>(60m, apToChange.TTC);

            Assert.AreEqual<decimal>(150m, apToChange.Affectation.Value.HT);
            Assert.AreEqual<decimal>(30m, apToChange.Affectation.Value.TVA);
            Assert.AreEqual<decimal>(180m, apToChange.Affectation.Value.TTC);
        }
        [TestMethod]
        public void SpoReafEngagementChangeHTinEngagementTest()
        {
            var engagement = Load();
            var aToChange = engagement.Affectations[0];
            aToChange.HT = 250m;
            Assert.AreEqual<decimal>(250m, aToChange.HT);
            Assert.AreEqual<decimal>(50m, aToChange.TVA);
            Assert.AreEqual<decimal>(300m, aToChange.TTC);
            
            var ht1 = Math.Round(aToChange.HT / aToChange.ProgAffectations.Count, 2, MidpointRounding.AwayFromZero);
            var tva1 = Math.Round(ht1*aToChange.TauxTVA/100, 2, MidpointRounding.AwayFromZero);
            var ttc1 = ht1+tva1;
            var ht2 = aToChange.HT - (aToChange.ProgAffectations.Count - 1) * ht1;
            var tva2 = aToChange.TVA - (aToChange.ProgAffectations.Count - 1) * tva1;
            var ttc2 = aToChange.TTC - (aToChange.ProgAffectations.Count - 1) * ttc1;
            for (int i = 0; i < aToChange.ProgAffectations.Count; i++)
            {
                var ap = aToChange.ProgAffectations[i];
                bool last = i == aToChange.ProgAffectations.Count - 1;
                Assert.AreEqual<decimal>(last ? ht1 : ht2, ap.HT);
                Assert.AreEqual<decimal>(last ? tva1 : tva2, ap.TVA);
                Assert.AreEqual<decimal>(last ? ttc1 : ttc2, ap.TTC);
            }

        }
    }
}
