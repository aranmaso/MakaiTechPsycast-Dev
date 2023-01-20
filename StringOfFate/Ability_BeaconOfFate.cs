using RimWorld;
using RimWorld.Planet;
using Verse;
using VFECore;
using VanillaPsycastsExpanded;
using UnityEngine;
using VFECore.Abilities;

namespace MakaiTechPsycast.StringOfFate
{
    public class Ability_BeaconOfFate : VFECore.Abilities.Ability
    {
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            AbilityExtension_Roll1D20 modExtension = def.GetModExtension<AbilityExtension_Roll1D20>();
            RollInfo rollinfo = new RollInfo();
            rollinfo = MakaiUtility.Roll1D20(pawn, modExtension.skillBonus, rollinfo);
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
                        int hour = Mathf.RoundToInt(modExtension.hours) * 2500;
                        if (hour > 0)
                        {
                            compSpawnedBuilding.finalTick = compSpawnedBuilding.lastDamageTick + hour;
                        }
                    }
                    Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                    Messages.Message("Makai_PassArollcheckReverseBeacon".Translate(pawn.LabelShort, modExtension.hours, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                }                
            }
            if (rollinfo.roll >= modExtension.greatSuccessThreshold)
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
                        int hour = Mathf.RoundToInt(modExtension.hours) * 2500;
                        if (hour > 0)
                        {
                            compSpawnedBuilding.finalTick = compSpawnedBuilding.lastDamageTick + (hour*2);
                        }
                    }
                    Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                    Messages.Message("Makai_GreatPassArollcheckReverseBeacon".Translate(pawn.LabelShort, modExtension.hours*2, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                }               
            }
            if (rollinfo.roll < modExtension.successThreshold)
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
                        int hour = Mathf.RoundToInt(modExtension.hours) * 2500;
                        if (hour > 0)
                        {
                            compSpawnedBuilding.finalTick = compSpawnedBuilding.lastDamageTick + (hour/2);
                        }
                    }
                    Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
                    Messages.Message("Makai_FailArollcheckReverseBeacon".Translate(pawn.LabelShort, modExtension.hours/2, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
                }                
            }
        }
    }
}
