using RimWorld.Planet;
using VanillaPsycastsExpanded;
using VFECore.Abilities;
using RimWorld;
using Verse;

namespace MakaiTechPsycast.PerfectKnowledge
{
	public class Ability_XpToCaster : VFECore.Abilities.Ability
    {
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			AbilityExtension_Skill modExtension = def.GetModExtension<AbilityExtension_Skill>();
			SkillRecord skill = pawn.skills?.GetSkill(modExtension.skillDef);
			float num = modExtension.amount;
			pawn.skills?.Learn(modExtension.skillDef, num, direct: true);
			if (targets[0].Thing is Pawn pawn2)
            {
				SkillRecord skill2 = pawn2.skills?.GetSkill(modExtension.skillDef);
				float xpSinceLastLevel = skill2.XpRequiredForLevelUp - modExtension.amount;
				skill2.Learn(0f - num, direct: true);
				if (skill2.xpSinceLastLevel <= -1000f)
				{
					skill2.levelInt--;
					skill2.xpSinceLastLevel = xpSinceLastLevel;
				}
				if (skill2.XpTotalEarned == 0f)
				{
					skill2.levelInt = 0;
					skill2.xpSinceLastLevel = 0f;
				}
			}
		}
		public virtual Hediff ApplyHediff2(Pawn targetPawn, HediffDef hediffDef, BodyPartRecord bodyPart, int duration, float severity)
		{
			Hediff hediff = HediffMaker.MakeHediff(hediffDef, targetPawn, bodyPart);
			if (hediff is Hediff_Ability hediff_Ability)
			{
				hediff_Ability.ability = this;
			}
			if (severity > float.Epsilon)
			{
				hediff.Severity = severity;
			}
			if (hediff is HediffWithComps hediffWithComps)
			{
				foreach (HediffComp comp in hediffWithComps.comps)
				{
					if (comp is HediffComp_Ability hediffComp_Ability)
					{
						hediffComp_Ability.ability = this;
					}
					if (comp is HediffComp_Disappears hediffComp_Disappears)
					{
						hediffComp_Disappears.ticksToDisappear = duration;
					}
				}
			}
			targetPawn.health.AddHediff(hediff);
			return targetPawn.health.hediffSet.GetFirstHediffOfDef(hediffDef);
		}
	}
}
