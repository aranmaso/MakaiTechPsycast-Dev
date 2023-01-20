using RimWorld;
using RimWorld.Planet;
using Verse;
using VFECore;
using VanillaPsycastsExpanded;
using UnityEngine;

namespace MakaiTechPsycast.TrueDestruction
{
    public class Ability_WillOfTheFallen : VFECore.Abilities.Ability
    {
        public AbilityExtension_Roll1D20 modExtension => def.GetModExtension<AbilityExtension_Roll1D20>();
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            RollInfo rollinfo = new RollInfo();
            rollinfo = MakaiUtility.Roll1D20(pawn, modExtension.skillBonus, rollinfo, modExtension.skillBonus2);
            if (rollinfo.roll >= modExtension.successThreshold && rollinfo.roll < modExtension.greatSuccessThreshold)
            {
                foreach (GlobalTargetInfo globalTargetInfo in targets)
                {
                    if (globalTargetInfo.Thing is Pawn targetPawn)
                    {
                        if(targetPawn.health.hediffSet.HasHediff(modExtension.hediffDefWhenSuccess))
                        {
                            MakaiUtility.GetFirstHediffOfDef(targetPawn, modExtension.hediffDefWhenSuccess).TryGetComp<HediffComp_WillOfTheFallen>().buffQuality = 1;
                        }
                        else
                        {
                            Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenSuccess, targetPawn);
                            hediff.TryGetComp<HediffComp_WillOfTheFallen>().buffQuality = 1;
                            targetPawn.health.AddHediff(hediff);
                        }                        
                        Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                        Messages.Message("Makai_PassArollcheckWillOfTheFallen".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
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
                            MakaiUtility.GetFirstHediffOfDef(targetPawn, modExtension.hediffDefWhenSuccess).TryGetComp<HediffComp_WillOfTheFallen>().buffQuality = 2;
                        }
                        else
                        {
                            Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenSuccess, targetPawn);
                            hediff.TryGetComp<HediffComp_WillOfTheFallen>().buffQuality = 1;
                            targetPawn.health.AddHediff(hediff);
                        }
                        Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                        Messages.Message("Makai_GreatPassArollcheckWillOfTheFallen".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
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
                            MakaiUtility.GetFirstHediffOfDef(targetPawn, modExtension.hediffDefWhenSuccess).TryGetComp<HediffComp_WillOfTheFallen>().buffQuality = 0;
                        }
                        else
                        {
                            Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenSuccess, targetPawn);
                            hediff.TryGetComp<HediffComp_WillOfTheFallen>().buffQuality = 1;
                            targetPawn.health.AddHediff(hediff);
                        }
                        Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
                        Messages.Message("Makai_FailArollcheckWillOfTheFallen".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
                    }
                }
            }
        }
    }
}
