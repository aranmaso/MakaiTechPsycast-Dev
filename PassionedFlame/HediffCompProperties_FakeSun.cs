using RimWorld;
using Verse;

namespace MakaiTechPsycast.PassionedFlame
{
    public class HediffCompProperties_FakeSun : HediffCompProperties
    {
        public ThingDef projectile;

        public HediffDef hediffDef;

        public int durationTick;

        public int tickInterval = 500;

        public HediffCompProperties_FakeSun()
        {
            compClass = typeof(HediffComp_FakeSun);
        }
    }
}
