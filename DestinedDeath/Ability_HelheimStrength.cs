using RimWorld;
using RimWorld.Planet;
using Verse;
using VFECore;
using VanillaPsycastsExpanded;
using UnityEngine;

namespace MakaiTechPsycast.DestinedDeath
{
    public class Ability_HelheimStrength : VFECore.Abilities.Ability
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
                    if (targetPawn.health.hediffSet.HasHediff(modExtension.hediffDefWhenSuccess))
                    {
                        targetPawn.health.hediffSet.GetFirstHediffOfDef(modExtension.hediffDefWhenSuccess).TryGetComp<HediffComp_HelheimStrength>().ShieldCount += 20;
                        targetPawn.health.hediffSet.GetFirstHediffOfDef(modExtension.hediffDefWhenSuccess).TryGetComp<HediffComp_Disappears>().ticksToDisappear += Mathf.FloorToInt(modExtension.hours * 2500f) + Mathf.FloorToInt(pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul).TryGetComp<HediffComp_SoulCollection>().SoulCount * 100f);
                    }
                    else
                    {
                        Hediff hediff = MakaiUtility.CreateCustomHediffWithDuration(targetPawn, modExtension.hediffDefWhenSuccess, modExtension.hours, modExtension.ticks, modExtension.multiplier);
                        if (pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul))
                        {
                            hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear += Mathf.FloorToInt(pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul).TryGetComp<HediffComp_SoulCollection>().SoulCount * 100f);
                        }
                        hediff.TryGetComp<HediffComp_HelheimStrength>().ShieldCount = 20;
                        targetPawn.health.AddHediff(hediff);
                    }
                    Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                    Messages.Message("Makai_PassArollcheckDeathmarch".Translate(pawn.LabelShort, targetPawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                }
                
            }
            if (rollinfo.roll >= modExtension.greatSuccessThreshold)
            {
                if (targets[0].Thing is Pawn targetPawn)
                {
                    if(targetPawn.health.hediffSet.HasHediff(modExtension.hediffDefWhenGreatSuccess))
                    {
                        targetPawn.health.hediffSet.GetFirstHediffOfDef(modExtension.hediffDefWhenGreatSuccess).TryGetComp<HediffComp_HelheimStrength>().ShieldCount += 40;
                        targetPawn.health.hediffSet.GetFirstHediffOfDef(modExtension.hediffDefWhenGreatSuccess).TryGetComp<HediffComp_Disappears>().ticksToDisappear += Mathf.FloorToInt(modExtension.hours * 2500f) + Mathf.FloorToInt(pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul).TryGetComp<HediffComp_SoulCollection>().SoulCount * 100f);
                    }
                    else
                    {
                        Hediff hediff = MakaiUtility.CreateCustomHediffWithDuration(targetPawn, modExtension.hediffDefWhenGreatSuccess, modExtension.hours, modExtension.ticks, modExtension.multiplier);
                        if (pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul))
                        {
                            hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear += Mathf.FloorToInt(pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul).TryGetComp<HediffComp_SoulCollection>().SoulCount * 100f);
                            hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear *= 2;
                        }
                        hediff.TryGetComp<HediffComp_HelheimStrength>().ShieldCount = 40;
                        targetPawn.health.AddHediff(hediff);
                    }
                    Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                    Messages.Message("Makai_GreatPassArollcheckDeathmarch".Translate(pawn.LabelShort, targetPawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                }
                
            }
            if (rollinfo.roll < modExtension.successThreshold)
            {
                if (targets[0].Thing is Pawn targetPawn)
                {
                    if (targetPawn.health.hediffSet.HasHediff(modExtension.hediffDefWhenSuccess))
                    {
                        targetPawn.health.hediffSet.GetFirstHediffOfDef(modExtension.hediffDefWhenSuccess).TryGetComp<HediffComp_HelheimStrength>().ShieldCount += 10;
                        targetPawn.health.hediffSet.GetFirstHediffOfDef(modExtension.hediffDefWhenSuccess).TryGetComp<HediffComp_Disappears>().ticksToDisappear += Mathf.FloorToInt(modExtension.hours * 2500f) + Mathf.FloorToInt(pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul).TryGetComp<HediffComp_SoulCollection>().SoulCount * 100f);
                    }
                    else
                    {
                        Hediff hediff = MakaiUtility.CreateCustomHediffWithDuration(targetPawn, modExtension.hediffDefWhenSuccess, modExtension.hours, modExtension.ticks, modExtension.multiplier);
                        if (pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul))
                        {
                            hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear += Mathf.FloorToInt(pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul).TryGetComp<HediffComp_SoulCollection>().SoulCount * 100f);
                            hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear /= 2;
                        }
                        hediff.TryGetComp<HediffComp_HelheimStrength>().ShieldCount = 10;
                        targetPawn.health.AddHediff(hediff);
                    }
                    Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
                    Messages.Message("Makai_FailArollcheckDeathmarch".Translate(pawn.LabelShort, targetPawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
                }
                
            }
        }
    }
}
