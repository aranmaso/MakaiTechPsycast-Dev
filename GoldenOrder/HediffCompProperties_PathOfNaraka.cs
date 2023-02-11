using RimWorld;
using Verse;

namespace MakaiTechPsycast.GoldenOrder
{
    public class HediffCompProperties_PathOfNaraka : HediffCompProperties
    {
        public int maxStack;

        //public float costPerTrigger;

        public HediffCompProperties_PathOfNaraka()
        {
            compClass = typeof(HediffComp_PathOfNaraka);
        }
    }
}
