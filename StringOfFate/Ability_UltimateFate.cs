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
                    Hediff hediff = MakaiUtility.CreateCustomHediffWithDuration(targetPawn, modExtension.hediffDefWhenSuccess, modExtension.hours, modExtension.ticks);
                    hediff.TryGetComp<HediffComp_UltimateFate>().threshold = 250;
                    hediff.TryGetComp<HediffComp_UltimateFate>().maxThresholdPerHit = 5;
                    targetPawn.health.AddHediff(hediff);
                    int fatedCount = 0;
                    foreach (Pawn item in MakaiUtility.GetNearbyPawnFriendAndFoe(targetPawn.Position,targetPawn.Map,GetRadiusForPawn()))
                    {
                        if (item == pawn)
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
                        if (pawn.health.hediffSet.HasHediff(modExtension.hediffDefWhenSuccess))
                        {
                            MakaiUtility.GetFirstHediffOfDef(pawn, modExtension.hediffDefWhenSuccess).TryGetComp<HediffComp_UltimateFate>().fatedCount++;
                            fatedCount++;
                        }                      
                    }
                    Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                    Messages.Message("Makai_PassArollcheckUltimateFate".Translate(pawn.LabelShort,targetPawn.LabelShort, fatedCount, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                }
            }
            if (rollinfo.roll >= modExtension.greatSuccessThreshold)
            {
                if (targets[0].Thing is Pawn targetPawn)
                {
                    Hediff hediff = MakaiUtility.CreateCustomHediffWithDuration(targetPawn, modExtension.hediffDefWhenSuccess, modExtension.hours, modExtension.ticks);
                    hediff.TryGetComp<HediffComp_UltimateFate>().threshold = 100;
                    hediff.TryGetComp<HediffComp_UltimateFate>().maxThresholdPerHit = 2;
                    targetPawn.health.AddHediff(hediff);
                    int fatedCount = 0;
                    foreach (Pawn item in MakaiUtility.GetNearbyPawnFriendAndFoe(targetPawn.Position, targetPawn.Map, GetRadiusForPawn()))
                    {
                        if (item == pawn)
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
                        if (pawn.health.hediffSet.HasHediff(modExtension.hediffDefWhenSuccess))
                        {
                            MakaiUtility.GetFirstHediffOfDef(pawn, modExtension.hediffDefWhenSuccess).TryGetComp<HediffComp_UltimateFate>().fatedCount++;
                            fatedCount++;
                        }
                    }
                    Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                    Messages.Message("Makai_GreatPassArollcheckUltimateFate".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                }
            }
            if (rollinfo.roll < modExtension.successThreshold)
            {
                if (targets[0].Thing is Pawn targetPawn)
                {
                    Hediff hediff = MakaiUtility.CreateCustomHediffWithDuration(targetPawn, modExtension.hediffDefWhenSuccess, modExtension.hours, modExtension.ticks);
                    hediff.TryGetComp<HediffComp_UltimateFate>().threshold = 500;
                    hediff.TryGetComp<HediffComp_UltimateFate>().maxThresholdPerHit = 10;
                    targetPawn.health.AddHediff(hediff);
                    int fatedCount = 0;
                    foreach (Pawn item in MakaiUtility.GetNearbyPawnFriendAndFoe(targetPawn.Position, targetPawn.Map, GetRadiusForPawn()))
                    {
                        if (item == pawn)
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
                        if (pawn.health.hediffSet.HasHediff(modExtension.hediffDefWhenSuccess))
                        {
                            MakaiUtility.GetFirstHediffOfDef(pawn, modExtension.hediffDefWhenSuccess).TryGetComp<HediffComp_UltimateFate>().fatedCount++;
                            fatedCount++;
                        }
                    }
                    Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
                    Messages.Message("Makai_FailArollcheckUltimateFate".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
                }
            }
        }
    }
}
