using Verse;
using RimWorld;

namespace MakaiTechPsycast.DistortedReality
{
    public class HediffCompProperties_DistortedShield : HediffCompProperties
    {
        public int defenseCount = 1;

        public bool stopOnlyEnemy;

        public HediffCompProperties_DistortedShield()
        {
            compClass = typeof(HediffComp_DistortedShield);
        }
    }
}
