using RimWorld;
using RimWorld.Planet;
using Verse;
using VFECore;
using VanillaPsycastsExpanded;
using UnityEngine;

namespace MakaiTechPsycast.StringOfFate
{
    public class Ability_MirroredFate : VFECore.Abilities.Ability
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
                    MirroredFateInfo minfo = new MirroredFateInfo();
                    minfo = MakaiUtility.GetMirroredFateInfo(minfo,10,0.5f,reflectOnlyEnemies:true,reflectOnlyFriendly:false,reflectMelee: true,reflectRanged: false,userTakeDamage: false);
                    if (targetPawn.health.hediffSet.HasHediff(modExtension.hediffDefWhenSuccess))
                    {
                        targetPawn.health.hediffSet.GetFirstHediffOfDef(modExtension.hediffDefWhenSuccess).TryGetComp<HediffComp_Disappears>().ticksToDisappear += Mathf.FloorToInt(modExtension.hours * 2500);
                        targetPawn.health.hediffSet.GetFirstHediffOfDef(modExtension.hediffDefWhenSuccess).TryGetComp<HediffComp_MirroredFate>().reflectCount += 5;
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
                        hediff.TryGetComp<HediffComp_MirroredFate>().mirrorInfo = minfo;
                        hediff.TryGetComp<HediffComp_MirroredFate>().reflectCount = minfo.reflectCountLeft;
                        hediff.TryGetComp<HediffComp_MirroredFate>().info = "normal";
                        targetPawn.health.AddHediff(hediff);
                    }
                    Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                    Messages.Message("Makai_PassArollcheckMirroredFate".Translate(pawn.LabelShort,targetPawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                }                
            }
            if (rollinfo.roll >= modExtension.greatSuccessThreshold)
            {
                if (targets[0].Thing is Pawn targetPawn)
                {
                    MirroredFateInfo minfo = new MirroredFateInfo();
                    minfo = MakaiUtility.GetMirroredFateInfo(minfo, 20,0.5f, reflectOnlyEnemies: true, reflectOnlyFriendly: false, reflectMelee: true, reflectRanged: true, userTakeDamage: false);
                    if (targetPawn.health.hediffSet.HasHediff(modExtension.hediffDefWhenSuccess))
                    {
                        targetPawn.health.hediffSet.GetFirstHediffOfDef(modExtension.hediffDefWhenSuccess).TryGetComp<HediffComp_Disappears>().ticksToDisappear += Mathf.FloorToInt(modExtension.hours * 2500);
                        targetPawn.health.hediffSet.GetFirstHediffOfDef(modExtension.hediffDefWhenSuccess).TryGetComp<HediffComp_MirroredFate>().reflectCount += 10;
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
                        hediff.TryGetComp<HediffComp_MirroredFate>().mirrorInfo = minfo;
                        hediff.TryGetComp<HediffComp_MirroredFate>().reflectCount = minfo.reflectCountLeft;
                        hediff.TryGetComp<HediffComp_MirroredFate>().info = "great";
                        targetPawn.health.AddHediff(hediff);
                    }
                    Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                    Messages.Message("Makai_GreatPassArollcheckMirroredFate".Translate(pawn.LabelShort, targetPawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                }                
            }
            if (rollinfo.roll < modExtension.successThreshold)
            {
                if (targets[0].Thing is Pawn targetPawn)
                {
                    MirroredFateInfo minfo = new MirroredFateInfo();
                    minfo = MakaiUtility.GetMirroredFateInfo(minfo, 5, 0.25f, reflectOnlyEnemies: true, reflectOnlyFriendly: false, reflectMelee: true, reflectRanged: false, userTakeDamage: true);
                    if (targetPawn.health.hediffSet.HasHediff(modExtension.hediffDefWhenSuccess))
                    {
                        targetPawn.health.hediffSet.GetFirstHediffOfDef(modExtension.hediffDefWhenSuccess).TryGetComp<HediffComp_Disappears>().ticksToDisappear += Mathf.FloorToInt(modExtension.hours * 2500);
                        targetPawn.health.hediffSet.GetFirstHediffOfDef(modExtension.hediffDefWhenSuccess).TryGetComp<HediffComp_MirroredFate>().reflectCount += 2;
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
                        hediff.TryGetComp<HediffComp_MirroredFate>().mirrorInfo = minfo;
                        hediff.TryGetComp<HediffComp_MirroredFate>().reflectCount = minfo.reflectCountLeft;
                        hediff.TryGetComp<HediffComp_MirroredFate>().info = "failed";
                        targetPawn.health.AddHediff(hediff);
                    }
                    Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
                    Messages.Message("Makai_FailArollcheckMirroredFate".Translate(pawn.LabelShort, targetPawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
                }                
            }
        }
    }
}
