using System.Collections.Generic;
using RimWorld;
using Verse;
using VFECore;

namespace MakaiTechPsycast.CorruptedProphet
{
	public class CompProperties_CorruptedTower : CompProperties
	{
		public List<HediffDef> hediff;

		public float durationInHour;

		public float severityAmount = 0f;

		public float radius;

		public int targetCount = 6;

		public List<StatDef> stats;

		public int tickRate = 500;

		public string uiIcon;

		public float hediffDuration;

		public CompProperties_CorruptedTower()
		{
			compClass = typeof(CompCorruptedTower);
		}

	}
}
