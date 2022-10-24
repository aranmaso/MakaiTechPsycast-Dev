using RimWorld.Planet;
using UnityEngine;
using VanillaPsycastsExpanded;
using RimWorld;
using System.Collections.Generic;
using Verse;
using VFECore.Abilities;
using MakaiTechPsycast.BondIntertwined;

namespace MakaiTechPsycast
{
	public class Ability_SpawnThingWithRoll : VFECore.Abilities.Ability
    {
		public override bool CanAutoCast => false;

		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			AbilityExtension_Roll1D20 modExtension = def.GetModExtension<AbilityExtension_Roll1D20>();
			if (modExtension == null)
			{
				return;
			}
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
			if ((roll >= modExtension.successThreshold && roll < modExtension.greatSuccessThreshold) && modExtension.thingToSpawnWhenSuccess != null)
            {
				foreach (GlobalTargetInfo globalTargetInfo in targets)
				{
					Thing thing = GenSpawn.Spawn(modExtension.thingToSpawnWhenSuccess, globalTargetInfo.Cell, pawn.Map);
					Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_Ring_ExpandY.Spawn(globalTargetInfo.Cell,pawn.Map, 0.5f);
					effect.Cleanup();
					thing.SetFactionDirect(pawn.Faction);
					CompSpawnedBuilding compSpawnedBuilding = thing.TryGetComp<CompSpawnedBuilding>();
					if (compSpawnedBuilding != null)
					{
						compSpawnedBuilding.lastDamageTick = Find.TickManager.TicksGame;
						compSpawnedBuilding.damagePerTick = Mathf.RoundToInt(GetPowerForPawn());
						int durationForPawn = GetDurationForPawn();
						if (durationForPawn > 0)
						{	
							compSpawnedBuilding.finalTick = compSpawnedBuilding.lastDamageTick + durationForPawn;
						}
					}
					if (modExtension.hediffDefWhenSuccess != null)
					{
						float dur = modExtension.hours * 2500f;
						Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenSuccess, pawn);
						hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.FloorToInt(dur);
						pawn.health.AddHediff(hediff);
					}
					Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
					Messages.Message("Makai_PassArollcheckSpawnThing".Translate(pawn.LabelShort, thing.Label , pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
				}
			}
			if (roll >= modExtension.greatSuccessThreshold && modExtension.thingToSpawnWhenGreatSuccess != null)
            {
				foreach (GlobalTargetInfo globalTargetInfo in targets)
				{
					Thing thing = GenSpawn.Spawn(modExtension.thingToSpawnWhenGreatSuccess, globalTargetInfo.Cell, pawn.Map);
					Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_Ring_ExpandY.Spawn(globalTargetInfo.Cell, pawn.Map,0.5f);
					effect.Cleanup();
					thing.SetFactionDirect(pawn.Faction);
					CompSpawnedBuilding compSpawnedBuilding = thing.TryGetComp<CompSpawnedBuilding>();
					if (compSpawnedBuilding != null)
					{
						compSpawnedBuilding.lastDamageTick = Find.TickManager.TicksGame;
						compSpawnedBuilding.damagePerTick = Mathf.RoundToInt(GetPowerForPawn());
						int durationForPawn = GetDurationForPawn();
						if (durationForPawn > 0)
						{
							compSpawnedBuilding.finalTick = compSpawnedBuilding.lastDamageTick + durationForPawn;
						}
					}
					if (modExtension.hediffDefWhenGreatSuccess != null)
                    {
						float dur = modExtension.hours * 2500f;
						Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenGreatSuccess, pawn);
						hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.FloorToInt(dur);
						pawn.health.AddHediff(hediff);
					}
					Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
					Messages.Message("Makai_GreatPassArollcheckSpawnThing".Translate(pawn.LabelShort, thing.Label, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
				}
			}
			if (roll < modExtension.successThreshold && modExtension.thingToSpawnWhenFail != null)
            {
				foreach (GlobalTargetInfo globalTargetInfo in targets)
				{
					Thing thing = GenSpawn.Spawn(modExtension.thingToSpawnWhenFail, globalTargetInfo.Cell, pawn.Map);
					Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_Ring_ExpandY.Spawn(globalTargetInfo.Cell, pawn.Map,0.5f);
					effect.Cleanup();
					thing.SetFactionDirect(pawn.Faction);
					CompSpawnedBuilding compSpawnedBuilding = thing.TryGetComp<CompSpawnedBuilding>();
					if (compSpawnedBuilding != null)
					{
						compSpawnedBuilding.lastDamageTick = Find.TickManager.TicksGame;
						compSpawnedBuilding.damagePerTick = Mathf.RoundToInt(GetPowerForPawn());
						int durationForPawn = GetDurationForPawn();
						if (durationForPawn > 0)
						{
							compSpawnedBuilding.finalTick = compSpawnedBuilding.lastDamageTick + durationForPawn;
						}
					}
					if (modExtension.hediffDefWhenFail != null)
					{
						float dur = modExtension.hours * 2500f;
						Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenFail, pawn);
						hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.FloorToInt(dur);
						pawn.health.AddHediff(hediff);
					}
					Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
					Messages.Message("Makai_FailArollcheckSpawnThing".Translate(pawn.LabelShort, thing.Label,modExtension.thingToSpawnWhenSuccess.label , pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
				}
			}
		}

		public override bool CanHitTarget(LocalTargetInfo target)
		{
			return base.CanHitTarget(target) && target.Cell.GetFirstBuilding(pawn.Map) == null;
		}
	}
}
