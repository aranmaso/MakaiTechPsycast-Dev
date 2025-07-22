using RimWorld.Planet;
using UnityEngine;
using VanillaPsycastsExpanded;
using RimWorld;
using System.Collections.Generic;
using Verse;
using VEF.Abilities;

namespace MakaiTechPsycast.PassionedFlame
{
	public class Ability_SpawnThing : VEF.Abilities.Ability
	{
		public override bool CanAutoCast => false;

		AbilityExtension_Roll1D20 modExtension => def.GetModExtension<AbilityExtension_Roll1D20>();

		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);			
			RollInfo rollinfo = new RollInfo();
			rollinfo = MakaiUtility.Roll1D20PassionCount(pawn, rollinfo);
			if (rollinfo.roll >= modExtension.successThreshold && rollinfo.roll < modExtension.greatSuccessThreshold)
			{
				foreach (GlobalTargetInfo globalTargetInfo in targets)
				{
					Thing thing = GenSpawn.Spawn(modExtension.thingToSpawnWhenSuccess, globalTargetInfo.Cell, pawn.Map);
					Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_Ring_ExpandY.Spawn(globalTargetInfo.Cell, pawn.Map, 0.5f);
					effect.Cleanup();
					thing.SetFactionDirect(pawn.Faction);
					CompSpawnedBuilding compSpawnedBuilding = thing.TryGetComp<CompSpawnedBuilding>();
					if (compSpawnedBuilding != null)
					{
						compSpawnedBuilding.lastDamageTick = Find.TickManager.TicksGame;
						compSpawnedBuilding.damagePerTick = Mathf.RoundToInt(GetPowerForPawn());
						int durationForPawn = Mathf.RoundToInt(modExtension.hours) * 2500;
						if (durationForPawn > 0)
						{
							compSpawnedBuilding.finalTick = compSpawnedBuilding.lastDamageTick + durationForPawn;
						}
					}
					CompPassionCrucible compCrucible = thing.TryGetComp<CompPassionCrucible>();
					if(compCrucible != null)
                    {
						foreach(Pawn item in MakaiUtility.GetNearbyPawnFoeOnly(globalTargetInfo.Cell,pawn.Faction,pawn.Map,GetRadiusForPawn()))
                        {
							foreach(SkillRecord sR in item.skills.skills)
                            {
								if(sR.passion > Passion.None)
                                {
									compCrucible.passionCount++;
									sR.passion = Passion.None;
                                }
                            }
                        }
                    }
					Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
					Messages.Message("Makai_PassArollcheckSpawnThing".Translate(pawn.LabelShort, thing.Label, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
				}
			}
			if (rollinfo.roll >= modExtension.greatSuccessThreshold)
			{
				foreach (GlobalTargetInfo globalTargetInfo in targets)
				{
					Thing thing = GenSpawn.Spawn(modExtension.thingToSpawnWhenGreatSuccess ?? modExtension.thingToSpawnWhenSuccess, globalTargetInfo.Cell, pawn.Map);
					Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_Ring_ExpandY.Spawn(globalTargetInfo.Cell, pawn.Map, 0.5f);
					effect.Cleanup();
					thing.SetFactionDirect(pawn.Faction);
					CompSpawnedBuilding compSpawnedBuilding = thing.TryGetComp<CompSpawnedBuilding>();
					if (compSpawnedBuilding != null)
					{
						compSpawnedBuilding.lastDamageTick = Find.TickManager.TicksGame;
						compSpawnedBuilding.damagePerTick = Mathf.RoundToInt(GetPowerForPawn());
						int durationForPawn = Mathf.RoundToInt(modExtension.hours) * 2500;
						if (durationForPawn > 0)
						{
							compSpawnedBuilding.finalTick = compSpawnedBuilding.lastDamageTick + durationForPawn;
						}
					}
					Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
					Messages.Message("Makai_GreatPassArollcheckSpawnThing".Translate(pawn.LabelShort, thing.Label, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
				}
			}
			if (rollinfo.roll < modExtension.successThreshold)
			{
				foreach (GlobalTargetInfo globalTargetInfo in targets)
				{
					Thing thing = GenSpawn.Spawn(modExtension.thingToSpawnWhenFail ?? modExtension.thingToSpawnWhenSuccess, globalTargetInfo.Cell, pawn.Map);
					Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_Ring_ExpandY.Spawn(globalTargetInfo.Cell, pawn.Map, 0.5f);
					effect.Cleanup();
					thing.SetFactionDirect(pawn.Faction);
					CompSpawnedBuilding compSpawnedBuilding = thing.TryGetComp<CompSpawnedBuilding>();
					if (compSpawnedBuilding != null)
					{
						compSpawnedBuilding.lastDamageTick = Find.TickManager.TicksGame;
						compSpawnedBuilding.damagePerTick = Mathf.RoundToInt(GetPowerForPawn());
						int durationForPawn = Mathf.RoundToInt(modExtension.hours) * 2500;
						if (durationForPawn > 0)
						{
							compSpawnedBuilding.finalTick = compSpawnedBuilding.lastDamageTick + durationForPawn;
						}
					}
					Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
					Messages.Message("Makai_FailArollcheckSpawnThing".Translate(pawn.LabelShort, thing.Label, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
				}
			}
		}

		public override bool CanHitTarget(LocalTargetInfo target)
		{
			return base.CanHitTarget(target) && target.Cell.GetFirstBuilding(pawn.Map) == null;
		}
	}
}
