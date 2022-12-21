using RimWorld;
using RimWorld.Planet;
using Verse;
using VFECore;
using VanillaPsycastsExpanded;
using UnityEngine;

namespace MakaiTechPsycast
{
    public class Ability_GiveHediffTemplate : VFECore.Abilities.Ability
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
                    if (globalTargetInfo.Thing is Pawn targetPawn)
                    {
                        Hediff hediff = MakaiUtility.ApplyCustomHediffWithDuration(targetPawn,modExtension.hediffDefWhenSuccess,modExtension.hours,modExtension.ticks,modExtension.multiplier);
                        targetPawn.health.AddHediff(hediff);
                        Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                        Messages.Message("Makai_PassArollcheckGiveHediffGeneric".Translate(pawn.LabelShort,hediff.LabelCap,targetPawn.LabelShort, pawn.Named("USER"), targetPawn.Named("USER2")), pawn, MessageTypeDefOf.PositiveEvent);
                    }
                }                
            }
            if (rollinfo.roll >= modExtension.greatSuccessThreshold)
            {
                foreach (GlobalTargetInfo globalTargetInfo in targets)
                {
                    if (globalTargetInfo.Thing is Pawn targetPawn)
                    {
                        Hediff hediff = MakaiUtility.ApplyCustomHediffWithDuration(targetPawn, modExtension.hediffDefWhenGreatSuccess ?? modExtension.hediffDefWhenSuccess, modExtension.hours, modExtension.ticks, modExtension.multiplier);
                        targetPawn.health.AddHediff(hediff);
                        Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                        Messages.Message("Makai_GreatPassArollcheckGiveHediffGeneric".Translate(pawn.LabelShort, hediff.LabelCap, targetPawn.LabelShort, pawn.Named("USER"), targetPawn.Named("USER2")), pawn, MessageTypeDefOf.PositiveEvent);
                    }
                }                
            }
            if (rollinfo.roll < modExtension.successThreshold)
            {
                foreach (GlobalTargetInfo globalTargetInfo in targets)
                {
                    if (globalTargetInfo.Thing is Pawn targetPawn)
                    {
                        Hediff hediff = MakaiUtility.ApplyCustomHediffWithDuration(targetPawn, modExtension.hediffDefWhenFail ?? modExtension.hediffDefWhenSuccess, modExtension.hours, modExtension.ticks, modExtension.multiplier);
                        targetPawn.health.AddHediff(hediff);
                        Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
                        Messages.Message("Makai_FailArollcheckGiveHediffGeneric".Translate(pawn.LabelShort, hediff.LabelCap, targetPawn.LabelShort, pawn.Named("USER"), targetPawn.Named("USER2")), pawn, MessageTypeDefOf.NegativeEvent);
                    }
                }                
            }
        }
    }
}
