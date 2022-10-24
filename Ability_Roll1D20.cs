using RimWorld.Planet;
using UnityEngine;
using System.Collections.Generic;
using VanillaPsycastsExpanded;
using VFECore.Abilities;
using RimWorld;
using Verse;

namespace MakaiTechPsycast
{
    public class Ability_Roll1D20 : VFECore.Abilities.Ability
    {
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
			AbilityExtension_Roll1D20 modExtension = def.GetModExtension<AbilityExtension_Roll1D20>();
			SkillDef ski = DefDatabase<SkillDef>.AllDefs.RandomElement();
			SkillRecord bonus = pawn.skills.GetSkill(ski);
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
			SkillRecord skillRequirement = pawn.skills.GetSkill(modExtension.skillBonus);
			int skillRec = 0;
			if (modExtension == null)
			{
				return;
			}
			if(modExtension.multiTarget == false)
            {
				if (targets[0].Thing is Pawn pawn2)
				{
					if (roll >= modExtension.successThreshold && roll < modExtension.greatSuccessThreshold)
					{
						if (modExtension.hediffDefWhenSuccess != null)
						{
							float dur = modExtension.hours * 2500f + modExtension.ticks;
							int durRandom = Rand.RangeInclusive(500, 1000);
							if (modExtension.multiplier != null)
							{
								dur *= pawn2.GetStatValue(modExtension.multiplier);
							}
							Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenSuccess, pawn2);
							hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.FloorToInt(dur) + durRandom;
							pawn2.health.AddHediff(hediff);
							Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
						}
						if(modExtension.memoryDefWhenSuccess != null)
                        {
							pawn2.needs.mood.thoughts.memories.TryGainMemory(modExtension.memoryDefWhenSuccess);
                        }
						if(modExtension.thingToSpawnWhenSuccess != null)
                        {
							foreach (GlobalTargetInfo globalTargetInfo in targets)
                            {
								Thing thing = GenSpawn.Spawn(modExtension.thingToSpawnWhenSuccess, globalTargetInfo.Cell, pawn.Map);
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
							}
						}
					}

					if (roll >= modExtension.greatSuccessThreshold)
					{
						if (modExtension.hediffDefWhenGreatSuccess != null)
						{
							float dur = modExtension.hours * 2500f + modExtension.ticks;
							int durRandom = Rand.RangeInclusive(500, 1000);
							if (modExtension.multiplier != null)
							{
								dur *= pawn2.GetStatValue(modExtension.multiplier);
							}
							Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenGreatSuccess, pawn2);
							hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.FloorToInt(dur) + durRandom;
							pawn2.health.AddHediff(hediff);
							Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
						}
						if (modExtension.memoryDefWhenGreatSuccess != null)
						{
							pawn2.needs.mood.thoughts.memories.TryGainMemory(modExtension.memoryDefWhenGreatSuccess);
						}
						if (modExtension.thingToSpawnWhenGreatSuccess != null)
						{
							foreach (GlobalTargetInfo globalTargetInfo in targets)
							{
								Thing thing = GenSpawn.Spawn(modExtension.thingToSpawnWhenGreatSuccess, globalTargetInfo.Cell, pawn.Map);
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
							}
						}

					}

					if (roll < modExtension.successThreshold)
					{
						if (modExtension.hediffDefWhenFail != null)
						{
							float dur = modExtension.hours * 2500f + modExtension.ticks;
							int durRandom = Rand.RangeInclusive(500, 1000);
							if (modExtension.multiplier != null)
							{
								dur *= pawn2.GetStatValue(modExtension.multiplier);
							}
							Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenFail, pawn2);
							hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.FloorToInt(dur) + durRandom;
							pawn2.health.AddHediff(hediff);
							Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
						}
						if (modExtension.memoryDefWhenFail != null)
						{
							pawn2.needs.mood.thoughts.memories.TryGainMemory(modExtension.memoryDefWhenFail);
						}
						if (modExtension.thingToSpawnWhenFail != null)
						{
							foreach (GlobalTargetInfo globalTargetInfo in targets)
							{
								Thing thing = GenSpawn.Spawn(modExtension.thingToSpawnWhenFail, globalTargetInfo.Cell, pawn.Map);
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
							}
						}
					}
				}
			}
			if(modExtension.multiTarget == true)
            {
				if (targets[0].Thing is Pawn pawn2 && targets[1].Thing is Pawn pawn3 && modExtension.multiTarget != true)
				{
					if (roll >= modExtension.successThreshold && roll < modExtension.greatSuccessThreshold)
					{
						if (modExtension.hediffDefWhenSuccess != null)
						{

						}
					}

					if (roll >= modExtension.greatSuccessThreshold)
					{

					}

					if (roll <= modExtension.failThreshold)
					{

					}
				}
			}
		}
    }
}
