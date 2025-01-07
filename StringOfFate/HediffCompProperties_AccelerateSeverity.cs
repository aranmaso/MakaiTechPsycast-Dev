using System.Collections.Generic;
using Verse;

namespace MakaiTechPsycast.StringOfFate
{
    public class HediffCompProperties_AccelerateSeverity : HediffCompProperties
    {
        public int interval = 250;

        public int tickIncrease = 1000;

        public FloatRange severityToAccelerate;

        public HediffCompProperties_AccelerateSeverity()
        {
            compClass = typeof(HediffComp_AccelerateSeverity);
        }
    }

}
