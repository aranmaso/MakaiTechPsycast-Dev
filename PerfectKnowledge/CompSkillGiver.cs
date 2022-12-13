using System;
using RimWorld;
using Verse;
using VFECore;

namespace MakaiTechPsycast.PerfectKnowledge
{
	public class CompSkillGiver : ThingComp
	{
		private int nextTest = 0;

		public CompProperties_SkillGiver Props => (CompProperties_SkillGiver)props;

		public override void PostExposeData()
		{
			Scribe_Values.Look(ref nextTest, "nextTest", 0);
			base.PostExposeData();
		}

		public override void PostPostMake()
		{
			nextTest = Find.TickManager.TicksGame + Props.tickRate;
			base.PostPostMake();
		}

		public override void CompTick()
		{
			base.CompTick();
			if (Find.TickManager.TicksGame != nextTest)
			{
				return;
			}
			foreach (Pawn pawn in MakaiUtility.GetNearbyPawnFriendAndFoe(parent.Position, parent.Map, Props.radius))
			{
				foreach (SkillDef skillDef in this.Props.skillDef)
                {
					float num = Props.xpGain;
					SkillRecord skill = pawn.skills?.GetSkill(skillDef);
					if (skill != null && skill.levelInt <= 10 && pawn.HomeFaction.IsPlayer)
					{
						if (!Props.stats.NullOrEmpty())
						{
							foreach (StatDef stat in Props.stats)
							{
								num *= pawn.GetStatValue(stat);
							}
						}
						pawn.skills?.Learn(skillDef, num, direct: true);
						/* skill.passion = Passion.Major; */
					}
				}
				
				
				/*if (pawn.health.hediffSet.HasHediff(Props.skillDef) && num > 0f)
				{
					pawn.health.hediffSet.GetFirstHediffOfDef(Props.skillDef).Severity += num;
				}
				else if (num > 0f)
				{
					Hediff hediff = HediffMaker.MakeHediff(Props.skillDef, pawn);
					hediff.Severity = num;
					pawn.health.AddHediff(hediff);
				} */
			}
			nextTest += Props.tickRate;
		}
	}
}
