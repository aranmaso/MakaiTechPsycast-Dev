using System;
using RimWorld;
using Verse;
using VFECore;

namespace MakaiTechPsycast.PerfectKnowledge
{
	public class CompSkillGiver : ThingComp
	{

		public CompProperties_SkillGiver Props => (CompProperties_SkillGiver)props;

		public override void PostExposeData()
		{
			base.PostExposeData();
		}

		public override void CompTick()
        {
            base.CompTick();
            if (parent.IsHashIntervalTick(Props.tickRate))
            {
                doTrigger();
            }
        }

        public void doTrigger()
        {
            foreach (Pawn pawn in MakaiUtility.GetNearbyPawnFriendAndFoe(parent.Position, parent.Map, Props.radius))
            {
                foreach (SkillDef skillDef in Props.skillDef)
                {
                    float num = Props.xpGain;
                    SkillRecord skill = pawn.skills?.GetSkill(skillDef);
                    if (skill != null && skill.Level <= Props.maxLevel)
                    {
                        if (!Props.stats.NullOrEmpty())
                        {
                            foreach (StatDef stat in Props.stats)
                            {
                                num *= pawn.GetStatValue(stat);
                            }
                        }
                        skill.Learn(num, true);
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
        }
    }
}
