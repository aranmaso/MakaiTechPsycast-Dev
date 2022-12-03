using RimWorld;
using Verse;

namespace MakaiTechPsycast.DistortedReality
{
    public class HediffCompProperties_BouncingBullet : HediffCompProperties
    {
        public int bounceCount;

        public float chance = 0.25f;

        public HediffCompProperties_BouncingBullet()
        {
            compClass = typeof(HediffComp_BouncingBullet);
        }
    }
}
