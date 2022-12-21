using RimWorld;
using System.Collections.Generic;
using Verse;

namespace MakaiTechPsycast.StringOfFate
{
    public class HediffCompProperties_FateFavor : HediffCompProperties
    {
        public string uiIcon;
        public List<HediffDef> favorList;
        public HediffCompProperties_FateFavor()
        {
            compClass = typeof(HediffComp_FateFavor);
        }
    }
}
