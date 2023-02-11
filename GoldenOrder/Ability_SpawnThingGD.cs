﻿using RimWorld.Planet;
using UnityEngine;
using VanillaPsycastsExpanded;
using RimWorld;
using System.Collections.Generic;
using Verse;
using VFECore.Abilities;

namespace MakaiTechPsycast.GoldenOrder
{
	public class Ability_SpawnThingGD : VFECore.Abilities.Ability
	{
		public override bool CanAutoCast => false;

		AbilityExtension_Roll1D20 modExtension => def.GetModExtension<AbilityExtension_Roll1D20>();

		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
        {
			if(!pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiTechPsy_GD_PathOfNaraka))
            {
				Messages.Message("Not Enough mental strength",MessageTypeDefOf.NeutralEvent,false);
				return false;
            }
			if(MakaiUtility.GetFirstHediffOfDef(pawn,MakaiTechPsy_DefOf.MakaiTechPsy_GD_PathOfNaraka).TryGetComp<HediffComp_PathOfNaraka>().currentStack < modExtension.costs)
            {
				Messages.Message("Not Enough mental strength", MessageTypeDefOf.NeutralEvent, false);
				return false;
            }
            return true;
        }
        public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			MakaiUtility.GetFirstHediffOfDef(pawn, MakaiTechPsy_DefOf.MakaiTechPsy_GD_PathOfNaraka).TryGetComp<HediffComp_PathOfNaraka>().currentStack -= modExtension.costs;
			RollInfo rollinfo = new RollInfo();
			rollinfo = MakaiUtility.Roll1D20(pawn, modExtension.skillBonus, rollinfo);
			if ((rollinfo.roll >= modExtension.successThreshold && rollinfo.roll < modExtension.greatSuccessThreshold))
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
					if (modExtension.hediffDefWhenSuccess != null)
					{
						float dur = modExtension.hours * 2500f;
						Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenSuccess, pawn);
						hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.FloorToInt(dur);
						pawn.health.AddHediff(hediff);
					}
					Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
					Messages.Message("Makai_PassArollcheckSpawnThingGD".Translate(pawn.LabelShort, thing.Label, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
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
							compSpawnedBuilding.finalTick = compSpawnedBuilding.lastDamageTick + (durationForPawn*2);
						}
					}
					if (modExtension.hediffDefWhenGreatSuccess != null)
					{
						float dur = modExtension.hours * 2500f;
						Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenGreatSuccess, pawn);
						hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.FloorToInt(dur);
						pawn.health.AddHediff(hediff);
					}
					Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
					Messages.Message("Makai_GreatPassArollcheckSpawnThingGD".Translate(pawn.LabelShort, thing.Label, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
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
					if (modExtension.hediffDefWhenFail != null)
					{
						float dur = modExtension.hours * 2500f;
						Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenFail, pawn);
						hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.FloorToInt(dur);
						pawn.health.AddHediff(hediff);
					}
					Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
					Messages.Message("Makai_FailArollcheckSpawnThingGD".Translate(pawn.LabelShort, thing.Label, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
				}
			}
		}

		public override bool CanHitTarget(LocalTargetInfo target)
		{
			return base.CanHitTarget(target) && target.Cell.GetFirstBuilding(pawn.Map) == null;
		}
	}
}
