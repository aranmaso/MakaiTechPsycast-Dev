using RimWorld;
using Verse;

namespace MakaiTechPsycast.TrueDestruction
{
    public class HediffCompProperties_FrenziedFlame : HediffCompProperties
    {
        public float severityPerHit;
        public HediffCompProperties_FrenziedFlame()
        {
            compClass = typeof(HediffComp_FrenziedFlame);
        }
    }
}
