using System.Collections.Generic;
using RimWorld;
using Verse;
using VFECore;

namespace MakaiTechPsycast.PerfectKnowledge
{
	public class CompProperties_SkillGiver : CompProperties
	{
		public List<SkillDef> skillDef;

		public float xpGain = 0f;

		public float radius;

		public List<StatDef> stats;

		public int tickRate = 500;

		public CompProperties_SkillGiver()
		{
			compClass = typeof(CompSkillGiver);
		}
	}
}
