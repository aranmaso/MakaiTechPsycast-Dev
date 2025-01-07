using RimWorld;
using Verse;
using System.Collections.Generic;

namespace MakaiTechPsycast.PassionedFlame
{
    public class HediffCompProperties_PassionCrucible : HediffCompProperties
    {
        public List<HediffDef> possibleHediffs;

        public string uiIcon;

        public HediffCompProperties_PassionCrucible()
        {
            compClass = typeof(HediffComp_PassionCrucible);
        }
    }
}
