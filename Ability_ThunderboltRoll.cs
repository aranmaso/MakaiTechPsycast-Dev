using RimWorld;
using RimWorld.Planet;
using Verse;
using VanillaPsycastsExpanded;
using VFECore.Abilities;

namespace MakaiTechPsycast
{
	public class Ability_ThunderboltRoll : VFECore.Abilities.Ability
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
			if(roll >= modExtension.successThreshold && roll <= modExtension.greatSuccessThreshold)
			{
				Log.Message("NormalSuccess");
				Messages.Message("Makai_PassArollcheckThunder".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
				for (int i = 0; i < targets.Length; i++)
				{
					GlobalTargetInfo globalTargetInfo = targets[i];
					foreach (Thing item in globalTargetInfo.Cell.GetThingList(globalTargetInfo.Map).ListFullCopy())
					{
						item.TakeDamage(new DamageInfo(DamageDefOf.Flame, 25f, -1f, pawn.DrawPos.AngleToFlat(item.DrawPos), pawn));
					}
					GenExplosion.DoExplosion(globalTargetInfo.Cell, globalTargetInfo.Map, GetRadiusForPawn(), DamageDefOf.EMP, pawn);
					pawn.Map.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrikeGreen(pawn.Map, globalTargetInfo.Cell));
					
				}
			}
			if (roll >= modExtension.greatSuccessThreshold)
			{
				Log.Message("GreatSuccess");
				Messages.Message("Makai_GreatPassArollcheckThunder".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
				for (int i = 0; i < targets.Length; i++)
				{
					GlobalTargetInfo globalTargetInfo = targets[i];
					foreach (Thing item in globalTargetInfo.Cell.GetThingList(globalTargetInfo.Map).ListFullCopy())
					{
						item.TakeDamage(new DamageInfo(DamageDefOf.Flame, 25f, -1f, pawn.DrawPos.AngleToFlat(item.DrawPos), pawn));
					}
					for(int j = 0; j < modExtension.repeatEffect; j++)
                    {
						GenExplosion.DoExplosion(globalTargetInfo.Cell, globalTargetInfo.Map, GetRadiusForPawn(), DamageDefOf.EMP, pawn);
						pawn.Map.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrikeGreen(pawn.Map, globalTargetInfo.Cell.RandomAdjacentCell8Way()));
					}
				}
			}
			if (roll <=modExtension.failThreshold)
            {
				Log.Message("fail");
				Messages.Message("Makai_FailArollcheckThunder".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
				for (int i = 0; i < targets.Length; i++)
				{
					GlobalTargetInfo globalTargetInfo = targets[i];
					foreach (Thing item in globalTargetInfo.Cell.GetThingList(globalTargetInfo.Map).ListFullCopy())
					{
						item.TakeDamage(new DamageInfo(DamageDefOf.Flame, 25f, -1f, pawn.DrawPos.AngleToFlat(item.DrawPos), pawn));
					}
					GenExplosion.DoExplosion(pawn.Position, globalTargetInfo.Map, GetRadiusForPawn(), DamageDefOf.EMP, pawn);
					pawn.Map.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrikeGreen(pawn.Map, pawn.RandomAdjacentCell8Way()));
				}
			}
			
		}
	}
}
