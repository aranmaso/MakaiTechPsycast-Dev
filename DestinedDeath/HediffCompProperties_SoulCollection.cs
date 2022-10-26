using Verse;
using RimWorld;

namespace MakaiTechPsycast.DestinedDeath
{
    public class HediffCompProperties_SoulCollection : HediffCompProperties
    {
        public int interval = 250;

        public string uiIcon;
        public string restoreFocusDesc;

        public HediffCompProperties_SoulCollection()
        {
            compClass = typeof(HediffComp_SoulCollection);
        }
    }
}
