using RimWorld;
using RimWorld.Planet;
using Verse;
using VFECore;
using VanillaPsycastsExpanded;
using UnityEngine;

namespace MakaiTechPsycast.StringOfFate
{
    public class Ability_AccelerateFate : VFECore.Abilities.Ability
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
                    if(targetPawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiPsy_SF_Reverse))
                    {
                        targetPawn.health.RemoveHediff(MakaiUtility.GetFirstHediffOfDef(targetPawn,MakaiTechPsy_DefOf.MakaiPsy_SF_Reverse));
                    }
                    if (targetPawn.health.hediffSet.HasHediff(modExtension.hediffDefWhenSuccess))
                    {
                        targetPawn.health.hediffSet.GetFirstHediffOfDef(modExtension.hediffDefWhenSuccess).TryGetComp<HediffComp_Disappears>().ticksToDisappear += Mathf.FloorToInt(modExtension.hours * 2500);
                    }
                    else
                    {
                        float num = modExtension.hours * 2500f + (float)modExtension.ticks;
                        num *= pawn.GetStatValue(modExtension.multiplier ?? StatDefOf.PsychicSensitivity);
                        Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenSuccess, pawn);
                        if (hediff.TryGetComp<HediffComp_Disappears>() != null)
                        {
                            hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.FloorToInt(num);
                        }
                        targetPawn.health.AddHediff(hediff);
                    }
                    Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                    Messages.Message("Makai_PassArollcheckAccelFate".Translate(pawn.LabelShort, targetPawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                }
            }
            if (rollinfo.roll >= modExtension.greatSuccessThreshold)
            {
                if (targets[0].Thing is Pawn targetPawn)
                {
                    if (targetPawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiPsy_SF_Reverse))
                    {
                        targetPawn.health.RemoveHediff(MakaiUtility.GetFirstHediffOfDef(targetPawn, MakaiTechPsy_DefOf.MakaiPsy_SF_Reverse));
                    }
                    if (targetPawn.health.hediffSet.HasHediff(modExtension.hediffDefWhenSuccess))
                    {
                        targetPawn.health.hediffSet.GetFirstHediffOfDef(modExtension.hediffDefWhenSuccess).TryGetComp<HediffComp_Disappears>().ticksToDisappear += Mathf.FloorToInt(modExtension.hours * 2500);
                    }
                    else
                    {
                        float num = modExtension.hours * 2500f + (float)modExtension.ticks;
                        num *= pawn.GetStatValue(modExtension.multiplier ?? StatDefOf.PsychicSensitivity);
                        Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenSuccess, pawn);
                        if (hediff.TryGetComp<HediffComp_Disappears>() != null)
                        {
                            hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.FloorToInt(num * 2);
                        }
                        targetPawn.health.AddHediff(hediff);
                    }
                    Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                    Messages.Message("Makai_GreatPassArollcheckAccelFate".Translate(pawn.LabelShort, targetPawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                }

            }
            if (rollinfo.roll < modExtension.successThreshold)
            {
                if (targets[0].Thing is Pawn targetPawn)
                {
                    if (targetPawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiPsy_SF_Reverse))
                    {
                        targetPawn.health.RemoveHediff(MakaiUtility.GetFirstHediffOfDef(targetPawn, MakaiTechPsy_DefOf.MakaiPsy_SF_Reverse));
                    }
                    if (targetPawn.health.hediffSet.HasHediff(modExtension.hediffDefWhenSuccess))
                    {
                        targetPawn.health.hediffSet.GetFirstHediffOfDef(modExtension.hediffDefWhenSuccess).TryGetComp<HediffComp_Disappears>().ticksToDisappear += Mathf.FloorToInt(modExtension.hours * 2500);
                    }
                    else
                    {
                        float num = modExtension.hours * 2500f + (float)modExtension.ticks;
                        num *= pawn.GetStatValue(modExtension.multiplier ?? StatDefOf.PsychicSensitivity);
                        Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenSuccess, pawn);
                        if (hediff.TryGetComp<HediffComp_Disappears>() != null)
                        {
                            hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.FloorToInt(num / 2);
                        }
                        targetPawn.health.AddHediff(hediff);
                    }
                    Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
                    Messages.Message("Makai_FailArollcheckAccelFate".Translate(pawn.LabelShort, targetPawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
                }
            }
        }
    }
}
