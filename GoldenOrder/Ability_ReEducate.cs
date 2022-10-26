using RimWorld.Planet;
using System.Collections.Generic;
using VanillaPsycastsExpanded;
using VFECore.Abilities;
using RimWorld;
using Verse;
using System.Linq;
using Verse.AI;

namespace MakaiTechPsycast.GoldenOrder
{
    public class Ability_ReEducate : VFECore.Abilities.Ability
    {
		public Trait trait;
		public override void Cast(params GlobalTargetInfo[] targets)
        {
			base.Cast(targets);
			AbilityExtension_Roll1D20 modExtension = def.GetModExtension<AbilityExtension_Roll1D20>();
			SkillRecord bonus = pawn.skills.GetSkill(modExtension.skillBonus);
			System.Random rand = new System.Random();
			int roll = rand.Next(1, 21);
			int rollBonus = bonus.Level / 5;
			int baseRoll = roll;
			int rollBonusLucky = 0;
			int rollBonusUnLucky = 0;
			if (pawn.health.hediffSet.HasHediff(VPE_DefOf.VPE_Lucky))
			{
				rollBonusLucky = 20;
			}
			if (pawn.health.hediffSet.HasHediff(VPE_DefOf.VPE_UnLucky))
			{
				rollBonusUnLucky = -20;
			}
			roll += rollBonus + rollBonusLucky + rollBonusUnLucky;
			int cumulativeBonusRoll = rollBonus + rollBonusLucky + rollBonusUnLucky;
			if(roll >= modExtension.successThreshold && roll < modExtension.greatSuccessThreshold)
            {
				foreach (GlobalTargetInfo globalTargetInfo in targets)
				if (trait == null)
				{
					pawn.jobs.EndCurrentJob(JobCondition.InterruptForced);
					Messages.Message("No trait choosed",MessageTypeDefOf.NeutralEvent);
				}
                if (targets[0].Thing is Pawn pawn2)
                {
					Find.WindowStack.Add(new Dialog_ChooseTraitToRemove(pawn2));
				}
			}

		}
    }
}
