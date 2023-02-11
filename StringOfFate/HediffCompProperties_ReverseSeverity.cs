using Verse;
using System.Collections.Generic;

namespace MakaiTechPsycast.StringOfFate
{
    public class HediffCompProperties_ReverseSeverity : HediffCompProperties
    {
        public int interval = 250;

        public int tickIncrease = 1000;

        public float severityToReverse = 0.1f;

        public HediffCompProperties_ReverseSeverity()
        {
            compClass = typeof(HediffComp_ReverseSeverity);
        }
    }

}
