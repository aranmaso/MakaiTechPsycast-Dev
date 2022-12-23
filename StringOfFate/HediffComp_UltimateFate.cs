using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace MakaiTechPsycast.StringOfFate
{
    public class HediffComp_UltimateFate : HediffComp
    {
        public HediffCompProperties_UltimateFate Props => (HediffCompProperties_UltimateFate)props;

        public int fatedCount = 0;
    }
}
