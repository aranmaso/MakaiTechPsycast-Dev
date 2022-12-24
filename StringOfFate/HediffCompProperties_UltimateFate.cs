using RimWorld;
using Verse;

namespace MakaiTechPsycast.StringOfFate
{
    public class HediffCompProperties_UltimateFate :  HediffCompProperties
    {
        public GraphicData graphicData;

        public string texturePath;

        public HediffCompProperties_UltimateFate()
        {
            compClass = typeof(HediffComp_UltimateFate);
        }
    }
}
