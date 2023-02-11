using RimWorld;
using RimWorld.Planet;
using Verse;
using VFECore;
using VanillaPsycastsExpanded;
using UnityEngine;

namespace MakaiTechPsycast.StringOfFate
{
    public class Ability_UltimateFate : VFECore.Abilities.Ability
    {
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            AbilityExtension_Roll1D20 modExtension = def.GetModExtension<AbilityExtension_Roll1D20>();
            RollInfo rollinfo = new RollInfo();
            rollinfo = MakaiUtility.Roll1D20(pawn, modExtension.skillBonus, rollinfo);
            if (rollinfo.roll >= modExtension.successThreshold && rollinfo.roll < modExtension.greatSuccessThreshold)
            {
                if (targets[0].Thing is Pawn targetPawn)
                {                                      
                    int fatedCount = 0;
                    foreach (Pawn item in MakaiUtility.GetNearbyPawnFriendAndFoe(targetPawn.Position,targetPawn.Map,GetRadiusForPawn()))
                    {
                        if (item == pawn || item == targetPawn)
                        {
                            continue;
                        }
                        if (!item.HostileTo(targetPawn) && modExtension.targetOnlyEnemies)
                        {
                            continue;
                        }
                        if (!(item.IsSlave || item.IsPrisoner) && modExtension.targetOnlyPrisonerOrSlave)
                        {
                            continue;
                        }
                        item.ageTracker.AgeBiologicalTicks += Mathf.FloorToInt(10 * 3600000f);
                        fatedCount++;
                    }
                    if(targetPawn.health.hediffSet.HasHediff(modExtension.hediffDefWhenSuccess))
                    {
                        Hediff hediff = MakaiUtility.GetFirstHediffOfDef(targetPawn, modExtension.hediffDefWhenSuccess);
                        hediff.TryGetComp<HediffComp_UltimateFate>().threshold = 50;
                        hediff.TryGetComp<HediffComp_UltimateFate>().maxThresholdPerHit = 5;
                        hediff.TryGetComp<HediffComp_UltimateFate>().fatedCount += fatedCount;
                    }
                    else
                    {
                        Hediff hediff = MakaiUtility.CreateCustomHediffWithDuration(targetPawn, modExtension.hediffDefWhenSuccess, modExtension.hours, modExtension.ticks);
                        hediff.TryGetComp<HediffComp_UltimateFate>().threshold = 50;
                        hediff.TryGetComp<HediffComp_UltimateFate>().maxThresholdPerHit = 5;
                        hediff.TryGetComp<HediffComp_UltimateFate>().fatedCount += fatedCount;
                        targetPawn.health.AddHediff(hediff);
                    }                    
                    Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                    Messages.Message("Makai_PassArollcheckUltimateFate".Translate(pawn.LabelShort,targetPawn.LabelShort, fatedCount, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                }
            }
            if (rollinfo.roll >= modExtension.greatSuccessThreshold)
            {
                if (targets[0].Thing is Pawn targetPawn)
                {
                    int fatedCount = 0;
                    foreach (Pawn item in MakaiUtility.GetNearbyPawnFriendAndFoe(targetPawn.Position, targetPawn.Map, GetRadiusForPawn()))
                    {
                        if (item == pawn || item == targetPawn)
                        {
                            continue;
                        }
                        if (!item.HostileTo(targetPawn) && modExtension.targetOnlyEnemies)
                        {
                            continue;
                        }
                        if (!(item.IsSlave || item.IsPrisoner) && modExtension.targetOnlyPrisonerOrSlave)
                        {
                            continue;
                        }
                        item.ageTracker.AgeBiologicalTicks += Mathf.FloorToInt(10 * 3600000f);
                        fatedCount++;
                    }
                    if (targetPawn.health.hediffSet.HasHediff(modExtension.hediffDefWhenSuccess))
                    {
                        Hediff hediff = MakaiUtility.GetFirstHediffOfDef(targetPawn, modExtension.hediffDefWhenSuccess);
                        hediff.TryGetComp<HediffComp_UltimateFate>().threshold = 25;
                        hediff.TryGetComp<HediffComp_UltimateFate>().maxThresholdPerHit = 2;
                        hediff.TryGetComp<HediffComp_UltimateFate>().fatedCount += fatedCount;
                    }
                    else
                    {
                        Hediff hediff = MakaiUtility.CreateCustomHediffWithDuration(targetPawn, modExtension.hediffDefWhenSuccess, modExtension.hours, modExtension.ticks);
                        hediff.TryGetComp<HediffComp_UltimateFate>().threshold = 25;
                        hediff.TryGetComp<HediffComp_UltimateFate>().maxThresholdPerHit = 2;
                        hediff.TryGetComp<HediffComp_UltimateFate>().fatedCount += fatedCount;
                        targetPawn.health.AddHediff(hediff);
                    }
                    Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                    Messages.Message("Makai_GreatPassArollcheckUltimateFate".Translate(pawn.LabelShort, targetPawn.LabelShort, fatedCount, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                }
            }
            if (rollinfo.roll < modExtension.successThreshold)
            {
                if (targets[0].Thing is Pawn targetPawn)
                {
                    int fatedCount = 0;
                    foreach (Pawn item in MakaiUtility.GetNearbyPawnFriendAndFoe(targetPawn.Position, targetPawn.Map, GetRadiusForPawn()))
                    {
                        if (item == pawn || item == targetPawn)
                        {
                            continue;
                        }
                        if (!item.HostileTo(targetPawn) && modExtension.targetOnlyEnemies)
                        {
                            continue;
                        }
                        if (!(item.IsSlave || item.IsPrisoner) && modExtension.targetOnlyPrisonerOrSlave)
                        {
                            continue;
                        }
                        item.ageTracker.AgeBiologicalTicks += Mathf.FloorToInt(10 * 3600000f);
                        fatedCount++;
                    }
                    if (targetPawn.health.hediffSet.HasHediff(modExtension.hediffDefWhenSuccess))
                    {
                        Hediff hediff = MakaiUtility.GetFirstHediffOfDef(targetPawn, modExtension.hediffDefWhenSuccess);
                        hediff.TryGetComp<HediffComp_UltimateFate>().threshold = 100;
                        hediff.TryGetComp<HediffComp_UltimateFate>().maxThresholdPerHit = 10;
                        hediff.TryGetComp<HediffComp_UltimateFate>().fatedCount += fatedCount;
                    }
                    else
                    {
                        Hediff hediff = MakaiUtility.CreateCustomHediffWithDuration(targetPawn, modExtension.hediffDefWhenSuccess, modExtension.hours, modExtension.ticks);
                        hediff.TryGetComp<HediffComp_UltimateFate>().threshold = 100;
                        hediff.TryGetComp<HediffComp_UltimateFate>().maxThresholdPerHit = 10;
                        hediff.TryGetComp<HediffComp_UltimateFate>().fatedCount += fatedCount;
                        targetPawn.health.AddHediff(hediff);
                    }
                    Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
                    Messages.Message("Makai_FailArollcheckUltimateFate".Translate(pawn.LabelShort, targetPawn.LabelShort, fatedCount, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
                }
            }
        }
    }
}
