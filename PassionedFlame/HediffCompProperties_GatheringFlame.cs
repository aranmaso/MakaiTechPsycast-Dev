using RimWorld;
using Verse;

namespace MakaiTechPsycast.PassionedFlame
{
    public class HediffCompProperties_GatheringFlame : HediffCompProperties
    {
        public int ticksToDisappear;

        public int activateInterval = 250;

        public HediffDef hediffToApplyOnceRemove;

        public SimpleCurve curve;

        public HediffCompProperties_GatheringFlame()
        {
            compClass = typeof(HediffComp_GatheringFlame);
        }
    }
}
