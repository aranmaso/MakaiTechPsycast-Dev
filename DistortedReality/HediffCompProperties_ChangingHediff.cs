using Verse;
using RimWorld;
using System.Collections.Generic;

namespace MakaiTechPsycast.DistortedReality
{
    public class HediffCompProperties_ChangingHediff : HediffCompProperties
    {
        public List<HediffDef> hediffList;

        public int interval = 6000;

        public HediffCompProperties_ChangingHediff()
        {
            compClass = typeof(HediffComp_ChangingHediff);
        }
    }
}
