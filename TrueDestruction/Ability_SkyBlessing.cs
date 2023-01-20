using RimWorld;
using RimWorld.Planet;
using Verse;
using VFECore;
using VanillaPsycastsExpanded;
using UnityEngine;

namespace MakaiTechPsycast.TrueDestruction
{
    public class Ability_SkyBlessing : VFECore.Abilities.Ability
    {
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            AbilityExtension_Roll1D20 modExtension = def.GetModExtension<AbilityExtension_Roll1D20>();
            RollInfo rollinfo = new RollInfo();
            rollinfo = MakaiUtility.Roll1D20(pawn, modExtension.skillBonus, rollinfo, modExtension.skillBonus2);
            if (rollinfo.roll >= modExtension.successThreshold && rollinfo.roll < modExtension.greatSuccessThreshold)
            {
                foreach (GlobalTargetInfo globalTargetInfo in targets)
                {
                    if (globalTargetInfo.Thing is Pawn targetPawn)
                    {
                        if (targetPawn.health.hediffSet.HasHediff(modExtension.hediffDefWhenSuccess))
                        {
                           Hediff hediff = MakaiUtility.GetFirstHediffOfDef(targetPawn,modExtension.hediffDefWhenSuccess);
                           hediff.TryGetComp<HediffComp_SkyBlessing>().useCountLeft += 2;
                        }
                        else
                        {
                            Hediff hediff = MakaiUtility.CreateCustomHediffWithDuration(targetPawn, modExtension.hediffDefWhenSuccess, modExtension.hours, modExtension.ticks, modExtension.multiplier);
                            hediff.TryGetComp<HediffComp_SkyBlessing>().useCountLeft = 5;
                            targetPawn.health.AddHediff(hediff);
                        }
                        Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                        Messages.Message("Makai_PassArollcheckSkyBlessing".Translate(pawn.LabelShort, targetPawn.LabelShort, pawn.Named("USER"), targetPawn.Named("USER2")), pawn, MessageTypeDefOf.PositiveEvent);
                    }
                }
            }
            if (rollinfo.roll >= modExtension.greatSuccessThreshold)
            {
                foreach (GlobalTargetInfo globalTargetInfo in targets)
                {
                    if (globalTargetInfo.Thing is Pawn targetPawn)
                    {
                        if (targetPawn.health.hediffSet.HasHediff(modExtension.hediffDefWhenSuccess))
                        {
                            MakaiUtility.GetFirstHediffOfDef(targetPawn, modExtension.hediffDefWhenSuccess).TryGetComp<HediffComp_SkyBlessing>().useCountLeft += 5;
                        }
                        else
                        {
                            Hediff hediff = MakaiUtility.CreateCustomHediffWithDuration(targetPawn, modExtension.hediffDefWhenSuccess, modExtension.hours, modExtension.ticks, modExtension.multiplier);
                            hediff.TryGetComp<HediffComp_SkyBlessing>().useCountLeft = 10;
                            targetPawn.health.AddHediff(hediff);
                        }
                        Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                        Messages.Message("Makai_GreatPassArollcheckSkyBlessing".Translate(pawn.LabelShort, targetPawn.LabelShort, pawn.Named("USER"), targetPawn.Named("USER2")), pawn, MessageTypeDefOf.PositiveEvent);
                    }
                }
            }
            if (rollinfo.roll < modExtension.successThreshold)
            {
                foreach (GlobalTargetInfo globalTargetInfo in targets)
                {
                    if (globalTargetInfo.Thing is Pawn targetPawn)
                    {
                        if (targetPawn.health.hediffSet.HasHediff(modExtension.hediffDefWhenSuccess))
                        {
                            MakaiUtility.GetFirstHediffOfDef(targetPawn, modExtension.hediffDefWhenSuccess).TryGetComp<HediffComp_SkyBlessing>().useCountLeft += 1;
                        }
                        else
                        {
                            Hediff hediff = MakaiUtility.CreateCustomHediffWithDuration(targetPawn, modExtension.hediffDefWhenSuccess, modExtension.hours, modExtension.ticks, modExtension.multiplier);
                            hediff.TryGetComp<HediffComp_SkyBlessing>().useCountLeft = 3;
                            targetPawn.health.AddHediff(hediff);
                        }
                        Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
                        Messages.Message("Makai_FailArollcheckSkyBlessing".Translate(pawn.LabelShort, targetPawn.LabelShort, pawn.Named("USER"), targetPawn.Named("USER2")), pawn, MessageTypeDefOf.NegativeEvent);
                    }
                }
            }
        }
    }
}
