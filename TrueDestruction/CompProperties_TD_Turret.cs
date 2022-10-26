using RimWorld;
using Verse;

namespace MakaiTechPsycast.TrueDestruction
{
	public class CompProperties_TD_Turret : CompProperties
	{
		public HediffDef hediffDef;

		public float severity;

		public CompProperties_TD_Turret()
		{
			compClass = typeof(CompTDturret);
		}
	}
}
