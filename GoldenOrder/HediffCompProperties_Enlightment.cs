using RimWorld;
using Verse;

namespace MakaiTechPsycast.GoldenOrder
{
    public class HediffCompProperties_Enlightment : HediffCompProperties
    {

        public float costPerTrigger;

        public HediffCompProperties_Enlightment()
        {
            compClass = typeof(HediffComp_Enlightment);
        }
    }
}
