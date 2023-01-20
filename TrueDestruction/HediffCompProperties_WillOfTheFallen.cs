using RimWorld;
using Verse;

namespace MakaiTechPsycast.TrueDestruction
{
    public class HediffCompProperties_WillOfTheFallen : HediffCompProperties
    {
        public SoundDef soundDefOnTrigger;
        public HediffCompProperties_WillOfTheFallen()
        {
            compClass = typeof(HediffComp_WillOfTheFallen);
        }
    }
}
