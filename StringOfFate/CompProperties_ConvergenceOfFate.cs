using RimWorld;
using Verse;

namespace MakaiTechPsycast.StringOfFate
{
    public class CompProperties_ConvergenceOfFate : CompProperties
    {
		public float radius;

		public int tickRate = 500;

		public int targetCount = 6;

		public HediffDef hediffDef;
		public HediffDef hediffDefSecond;

		public string uiIcon;

		public CompProperties_ConvergenceOfFate()
        {
			compClass = typeof(CompConvergenceOfFate);
        }
	}
}
