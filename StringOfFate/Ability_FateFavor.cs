using RimWorld;
using RimWorld.Planet;
using Verse;
using VFECore;
using VanillaPsycastsExpanded;
using UnityEngine;

namespace MakaiTechPsycast.StringOfFate
{
    public class Ability_FateFavor : VFECore.Abilities.Ability
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
                        if(!targetPawn.health.hediffSet.HasHediff(modExtension.hediffDefWhenSuccess))
                        {
                            Hediff hediff = MakaiUtility.CreateCustomHediffWithDuration(targetPawn, modExtension.hediffDefWhenSuccess, modExtension.hours, modExtension.ticks, modExtension.multiplier);
                            ((Hediff_LevelWithoutPart)hediff).level = 4;
                            targetPawn.health.AddHediff(hediff);
                        }
                        else
                        {
                            if(targetPawn.health.hediffSet.GetFirstHediffOfDef(modExtension.hediffDefWhenSuccess) is Hediff_LevelWithoutPart hLevel)
                            {
                                hLevel.ChangeLevel(4);
                            }
                        }                        
                        Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                        Messages.Message("Makai_PassArollcheckFateFavor".Translate(pawn.LabelShort, targetPawn.LabelShort, pawn.Named("USER"), targetPawn.Named("USER2")), pawn, MessageTypeDefOf.PositiveEvent);
                    }
                }
            }
            if (rollinfo.roll >= modExtension.greatSuccessThreshold)
            {
                foreach (GlobalTargetInfo globalTargetInfo in targets)
                {
                    if (globalTargetInfo.Thing is Pawn targetPawn)
                    {
                        if(!targetPawn.health.hediffSet.HasHediff(modExtension.hediffDefWhenSuccess))
                        {
                            Hediff hediff = MakaiUtility.CreateCustomHediffWithDuration(targetPawn,modExtension.hediffDefWhenSuccess, modExtension.hours, modExtension.ticks, modExtension.multiplier);
                            ((Hediff_LevelWithoutPart)hediff).level = 7;
                            targetPawn.health.AddHediff(hediff);
                        }
                        else
                        {
                            if (targetPawn.health.hediffSet.GetFirstHediffOfDef(modExtension.hediffDefWhenSuccess) is Hediff_LevelWithoutPart hLevel)
                            {
                                hLevel.ChangeLevel(7);
                            }
                        }
                        Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                        Messages.Message("Makai_GreatPassArollcheckFateFavor".Translate(pawn.LabelShort, targetPawn.LabelShort, pawn.Named("USER"), targetPawn.Named("USER2")), pawn, MessageTypeDefOf.PositiveEvent);
                    }
                }
            }
            if (rollinfo.roll < modExtension.successThreshold)
            {
                foreach (GlobalTargetInfo globalTargetInfo in targets)
                {
                    if (globalTargetInfo.Thing is Pawn targetPawn)
                    {
                        if(!targetPawn.health.hediffSet.HasHediff(modExtension.hediffDefWhenSuccess))
                        {
                            Hediff hediff = MakaiUtility.CreateCustomHediffWithDuration(targetPawn, modExtension.hediffDefWhenSuccess, modExtension.hours, modExtension.ticks, modExtension.multiplier);
                            ((Hediff_LevelWithoutPart)hediff).level = 2;
                            targetPawn.health.AddHediff(hediff);
                        }      
                        else
                        {
                            if (targetPawn.health.hediffSet.GetFirstHediffOfDef(modExtension.hediffDefWhenSuccess) is Hediff_LevelWithoutPart hLevel)
                            {
                                hLevel.ChangeLevel(2);
                            }
                        }
                        Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
                        Messages.Message("Makai_FailArollcheckFateFavor".Translate(pawn.LabelShort, targetPawn.LabelShort, pawn.Named("USER"), targetPawn.Named("USER2")), pawn, MessageTypeDefOf.NegativeEvent);
                    }
                }
            }
        }
    }
}
