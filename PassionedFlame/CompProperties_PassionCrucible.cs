using Verse;
using RimWorld;
using System.Collections.Generic;

namespace MakaiTechPsycast.PassionedFlame
{
    public class CompProperties_PassionCrucible : CompProperties
    {
        public float radius;

        public int interval;

        public string uiIcon;

        public CompProperties_PassionCrucible()
        {
            compClass = typeof(CompPassionCrucible);
        }
    }
}
