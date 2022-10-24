using RimWorld;
using RimWorld.Planet;
using Verse;
using VanillaPsycastsExpanded;
using VFECore.Abilities;

namespace MakaiTechPsycast
{
	public class Ability_PlaceBlueprint : VFECore.Abilities.Ability
	{
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
			if (roll >= modExtension.successThreshold && roll <= modExtension.greatSuccessThreshold)
			{
				for (int i = 0; i < targets.Length; i++)
				{
					GlobalTargetInfo globalTargetInfo = targets[i];
					foreach (Thing item in globalTargetInfo.Cell.GetThingList(globalTargetInfo.Map).ListFullCopy())
					{
						GenConstruct.PlaceBlueprintForBuild(ThingDefOf.Wall, targets[0].Cell, pawn.Map, Rot4.Random, pawn.Faction, ThingDefOf.Steel);
					}
				}
				Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
			}
		}
	}

}
