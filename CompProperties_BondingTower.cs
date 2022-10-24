using System.Collections.Generic;
using RimWorld;
using Verse;
using VFECore;

namespace MakaiTechPsycast.BondIntertwined
{
	public class CompProperties_BondingTower : CompProperties
	{
		public List<HediffDef> hediff;

		public float durationInHour;

		public float severityAmount = 0f;

		public float radius;

		public List<StatDef> stats;

		public int tickRate = 500;

		public string uiIcon;

		public float hediffDuration;

		public CompProperties_BondingTower()
		{
			compClass = typeof(CompBondingTower);
		}

	}
}
