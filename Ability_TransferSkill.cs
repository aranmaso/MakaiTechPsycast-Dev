using RimWorld.Planet;
using VanillaPsycastsExpanded;
using VFECore.Abilities;
using RimWorld;
using Verse;

namespace MakaiTechPsycast.PerfectKnowledge
{
	public class Ability_TransferSkill : VFECore.Abilities.Ability
    {
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			if (targets[0].Thing is Pawn pawn && targets[1].Thing is Pawn pawn2)
			{
				AbilityExtension_Skill modExtension = def.GetModExtension<AbilityExtension_Skill>();
				SkillRecord skillRecordTarget2 = pawn2.skills?.GetSkill(modExtension.skillDef);
				if (skillRecordTarget2 != null);
				{
					float num = modExtension.amount;
					float xpSinceLastLevel = skillRecordTarget2.XpRequiredForLevelUp - modExtension.amount;
					skillRecordTarget2.Learn(0f - num, direct: true);
					if (skillRecordTarget2.xpSinceLastLevel <= -1000f)
					{
						skillRecordTarget2.levelInt--;
						skillRecordTarget2.xpSinceLastLevel = xpSinceLastLevel;
					}
					if (skillRecordTarget2.XpTotalEarned == 0f)
					{
						skillRecordTarget2.levelInt = 0;
						skillRecordTarget2.xpSinceLastLevel = 0f;
					}
					if (skillRecordTarget2.XpTotalEarned != 0f)
                    {
						pawn.skills?.Learn(modExtension.skillDef, num, direct: true);

					}
				}
			}
		}
	}
}
