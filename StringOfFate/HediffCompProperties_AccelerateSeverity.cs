using System.Collections.Generic;
using Verse;

namespace MakaiTechPsycast.StringOfFate
{
    public class HediffCompProperties_AccelerateSeverity : HediffCompProperties
    {
        public int interval = 250;

        public int tickIncrease = 1000;

        public float severityToAccelerate = 0.1f;

        public HediffCompProperties_AccelerateSeverity()
        {
            compClass = typeof(HediffComp_AccelerateSeverity);
        }
    }

}
