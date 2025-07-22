using RimWorld.Planet;
using VanillaPsycastsExpanded;
using VEF.Abilities;
using Verse;
using RimWorld;

namespace MakaiTechPsycast.PerfectKnowledge
{
	public class Ability_StealAllIntellectual : VEF.Abilities.Ability
    {
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			if (targets[0].Thing is Pawn pawn2)
			{
				float num = 0;
				float num2 = 0;
				foreach (SkillRecord item in pawn2.skills.skills)
                {
					num += item.XpTotalEarned;
					item.levelInt = 0;
					item.xpSinceLastLevel = 0;
                }
				num2 = num / 12;
				foreach(SkillRecord item in pawn.skills.skills)
                {
					item.Learn(num2, true);
                }
				/*if (skillrecord != null);
				{
					pawn.skills.Learn(SkillDefOf.Melee, num/12, direct: true);
					pawn.skills.Learn(SkillDefOf.Shooting, num/12, direct: true);
					pawn.skills.Learn(SkillDefOf.Construction, num/12, direct: true);
					pawn.skills.Learn(SkillDefOf.Mining, num/12, direct: true);
					pawn.skills.Learn(SkillDefOf.Cooking, num/12, direct: true);
					pawn.skills.Learn(SkillDefOf.Plants, num/12, direct: true);
					pawn.skills.Learn(SkillDefOf.Animals, num/12, direct: true);
					pawn.skills.Learn(SkillDefOf.Crafting, num/12, direct: true);
					pawn.skills.Learn(SkillDefOf.Artistic, num/12, direct: true);
					pawn.skills.Learn(SkillDefOf.Medicine, num/12, direct: true);
					pawn.skills.Learn(SkillDefOf.Social, num/12, direct: true);
					pawn.skills.Learn(SkillDefOf.Intellectual, num/12, direct: true);

					skillrecord.levelInt = 0;
					skillrecord.xpSinceLastLevel = 0f;
				}*/
			}
		}
	}
}
