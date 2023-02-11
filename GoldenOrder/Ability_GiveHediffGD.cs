using RimWorld;
using RimWorld.Planet;
using Verse;
using VFECore;
using VanillaPsycastsExpanded;
using UnityEngine;

namespace MakaiTechPsycast.GoldenOrder
{
    public class Ability_GiveHediffGD : VFECore.Abilities.Ability
    {
        public AbilityExtension_Roll1D20 modExtension => def.GetModExtension<AbilityExtension_Roll1D20>();

        public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
        {
            if (!pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiTechPsy_GD_PathOfNaraka))
            {
                Messages.Message("Not Enough mental strength", MessageTypeDefOf.NeutralEvent, false);
                return false;
            }
            if (MakaiUtility.GetFirstHediffOfDef(pawn, MakaiTechPsy_DefOf.MakaiTechPsy_GD_PathOfNaraka).TryGetComp<HediffComp_PathOfNaraka>().currentStack < modExtension.costs)
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
            rollinfo = MakaiUtility.Roll1D20(pawn, modExtension.skillBonus, rollinfo, modExtension.skillBonus2 ?? null);
            if (rollinfo.roll >= modExtension.successThreshold && rollinfo.roll < modExtension.greatSuccessThreshold)
            {
                foreach (GlobalTargetInfo globalTargetInfo in targets)
                {
                    if (globalTargetInfo.Thing is Pawn targetPawn)
                    {
                        if(modExtension.targetOnlyEnemies && targetPawn.Faction == pawn.Faction && !(targetPawn.Faction.HostileTo(pawn.Faction) || targetPawn.HostileTo(pawn) || targetPawn.HostileTo(pawn.Faction)))
                        {
                            continue;                            
                        }
                        Hediff hediff = MakaiUtility.CreateCustomHediffWithDuration(targetPawn, modExtension.hediffDefWhenSuccess, modExtension.hours, modExtension.ticks, modExtension.multiplier);
                        targetPawn.health.AddHediff(hediff);
                        Messages.Message("Makai_PassArollcheckGiveHediffGeneric".Translate(pawn.LabelShort, hediff.LabelCap, targetPawn.LabelShort, pawn.Named("USER"), targetPawn.Named("USER2")), pawn, MessageTypeDefOf.PositiveEvent);
                    }
                }
                Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);                
            }
            if (rollinfo.roll >= modExtension.greatSuccessThreshold)
            {
                foreach (GlobalTargetInfo globalTargetInfo in targets)
                {
                    if (globalTargetInfo.Thing is Pawn targetPawn)
                    {
                        if (modExtension.targetOnlyEnemies && targetPawn.Faction == pawn.Faction && !(targetPawn.Faction.HostileTo(pawn.Faction) || targetPawn.HostileTo(pawn) || targetPawn.HostileTo(pawn.Faction)))
                        {
                            continue;
                        }
                        Hediff hediff = MakaiUtility.CreateCustomHediffWithDuration(targetPawn, modExtension.hediffDefWhenSuccess, modExtension.hours *2, modExtension.ticks *2, modExtension.multiplier);
                        targetPawn.health.AddHediff(hediff);
                        Messages.Message("Makai_GreatPassArollcheckGiveHediffGeneric".Translate(pawn.LabelShort, hediff.LabelCap, targetPawn.LabelShort, pawn.Named("USER"), targetPawn.Named("USER2")), pawn, MessageTypeDefOf.PositiveEvent);
                    }
                }
                Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
            }
            if (rollinfo.roll < modExtension.successThreshold)
            {
                foreach (GlobalTargetInfo globalTargetInfo in targets)
                {
                    if (globalTargetInfo.Thing is Pawn targetPawn)
                    {
                        if (modExtension.targetOnlyEnemies && targetPawn.Faction == pawn.Faction && !(targetPawn.Faction.HostileTo(pawn.Faction) || targetPawn.HostileTo(pawn) || targetPawn.HostileTo(pawn.Faction)))
                        {
                            continue;
                        }
                        Hediff hediff = MakaiUtility.CreateCustomHediffWithDuration(targetPawn, modExtension.hediffDefWhenSuccess, modExtension.hours /2, modExtension.ticks /2, modExtension.multiplier);
                        targetPawn.health.AddHediff(hediff);
                        Messages.Message("Makai_FailArollcheckGiveHediffGeneric".Translate(pawn.LabelShort, hediff.LabelCap, targetPawn.LabelShort, pawn.Named("USER"), targetPawn.Named("USER2")), pawn, MessageTypeDefOf.PositiveEvent);
                    }
                }
                Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
            }
        }
    }
}
