using RimWorld.Planet;
using VanillaPsycastsExpanded;
using VFECore.Abilities;
using RimWorld;
using Verse;

namespace MakaiTechPsycast.PerfectKnowledge
{
	public class Ability_SwapSkill : VFECore.Abilities.Ability
	{
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			if (targets[0].Thing is Pawn pawn && targets[1].Thing is Pawn pawn2)
			{
				AbilityExtension_Skill modExtension = def.GetModExtension<AbilityExtension_Skill>();
				SkillRecord Target1 = pawn.skills?.GetSkill(modExtension.skillDef);
				SkillRecord Target2 = pawn2.skills?.GetSkill(modExtension.skillDef);
				int num1 = Target1.levelInt;
				int num2 = Target2.levelInt;
				if (Target1 != null && Target2 != null)
                {
					Target1.levelInt = num2;
					Target1.xpSinceLastLevel = 1f;
					Target2.levelInt = num1;
					Target2.xpSinceLastLevel = 1f;
				}
			}
		}
	}
}
