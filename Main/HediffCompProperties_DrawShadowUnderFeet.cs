using RimWorld;
using Verse;

namespace MakaiTechPsycast
{
    public class HediffCompProperties_DrawShadowUnderFeet : HediffCompProperties
    {
        public float shadowSize;
        public HediffCompProperties_DrawShadowUnderFeet()
        {
            compClass = typeof(HediffComp_DrawShadowUnderFeet);
        }
    }
}
