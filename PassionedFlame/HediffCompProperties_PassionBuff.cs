using Verse;
using RimWorld;

namespace MakaiTechPsycast.PassionedFlame
{
    public class HediffCompProperties_PassionBuff : HediffCompProperties
    {
        public bool countAllPassion = true;

        public bool countOnlyMajor = false;

        public bool countOnlyMinor = false;

        public float maxSeverityToAffect = 12;

        public int updateInterval = 300;

        public HediffCompProperties_PassionBuff()
        {
            compClass = typeof(HediffComp_PassionBuff);
        }
    }
}
