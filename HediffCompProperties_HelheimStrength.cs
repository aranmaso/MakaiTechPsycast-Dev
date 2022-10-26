using Verse;
using RimWorld;

namespace MakaiTechPsycast.DestinedDeath
{
    public class HediffCompProperties_HelheimStrength : HediffCompProperties
    {
        public int ShieldStack;

        public bool countOnlyEnemyAttack = true;

        public HediffCompProperties_HelheimStrength()
        {
            compClass = typeof(HediffComp_HelheimStrength);
        }
    }
}
