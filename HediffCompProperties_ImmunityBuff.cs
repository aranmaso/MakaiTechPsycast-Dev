using Verse;
using RimWorld;
using System.Collections.Generic;

namespace MakaiTechPsycast
{
    public class HediffCompProperties_ImmunityBuff : HediffCompProperties
    {
        public List<HediffInfoList> hediffInfoList;

        public List<HediffDef> immunityList;

        public int checkInterval = 60;
        public HediffCompProperties_ImmunityBuff()
        {
            compClass = typeof(HediffComp_ImmunityBuff);
        }
    }
    public class HediffInfoList
    {
        public HediffDef hediffToImmune;

        public HediffDef hediffToReplaceWith;
    }
}
