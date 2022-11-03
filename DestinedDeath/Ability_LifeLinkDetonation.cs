using RimWorld;
using RimWorld.Planet;
using Verse;
using VFECore;
using VanillaPsycastsExpanded;
using UnityEngine;

namespace MakaiTechPsycast.DestinedDeath
{
    public class Ability_LifeLinkDetonation : VFECore.Abilities.Ability
    {
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            AbilityExtension_Roll1D20 modExtension = def.GetModExtension<AbilityExtension_Roll1D20>();
            RollInfo rollinfo = new RollInfo();
            rollinfo = MakaiUtility.Roll1D20(pawn, modExtension.skillBonus, rollinfo);
            if (rollinfo.roll >= modExtension.successThreshold && rollinfo.roll < modExtension.greatSuccessThreshold)
            {
                if (targets[0].Thing is Pawn targetPawn && targetPawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiTechPsy_DD_LifeLink))
                {
                    Hediff hediff = targetPawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_DD_LifeLink);
                    float accumulateDamge = hediff.TryGetComp<HediffComp_LifeLink>().totalDamage;
                    foreach(Pawn linkedPawn in hediff.TryGetComp<HediffComp_LifeLink>().linkedPawn)
                    {
                        linkedPawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_DD_LifeLink).TryGetComp<HediffComp_LifeLink>().totalDamage /= 2;
                    }
                    GenExplosion.DoExplosion(targetPawn.Position,pawn.Map,Mathf.Min(5,accumulateDamge/10),modExtension.damageDef,pawn,Mathf.FloorToInt(accumulateDamge),1f,null,null,null,targetPawn,null,0,0,GasType.RotStink);
                    Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                    Messages.Message("Makai_PassArollcheckLinkDetonate".Translate(pawn.LabelShort,targetPawn.LabelShort,accumulateDamge, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                }
                else
                {
                    Messages.Message("Target doesn't have Life Link",MessageTypeDefOf.NegativeEvent);
                }
            }
            if (rollinfo.roll >= modExtension.greatSuccessThreshold)
            {
                if (targets[0].Thing is Pawn targetPawn && targetPawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiTechPsy_DD_LifeLink))
                {
                    Hediff hediff = targetPawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_DD_LifeLink);
                    float accumulateDamge = hediff.TryGetComp<HediffComp_LifeLink>().totalDamage;
                    foreach (Pawn linkedPawn in hediff.TryGetComp<HediffComp_LifeLink>().linkedPawn)
                    {
                        linkedPawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_DD_LifeLink).TryGetComp<HediffComp_LifeLink>().totalDamage /= 2;
                    }
                    GenExplosion.DoExplosion(targetPawn.Position, pawn.Map, Mathf.Min(10, accumulateDamge / 5), modExtension.damageDef, pawn, Mathf.FloorToInt(accumulateDamge*1.25f), 1f, null, null, null, targetPawn, null, 0, 0, GasType.RotStink);
                    Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                    Messages.Message("Makai_GreatPassArollcheckLinkDetonate".Translate(pawn.LabelShort,targetPawn.LabelShort, accumulateDamge, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                }
                else
                {
                    Messages.Message("Target doesn't have Life Link", MessageTypeDefOf.NegativeEvent);
                }
            }
            if (rollinfo.roll < modExtension.successThreshold)
            {
                if (targets[0].Thing is Pawn targetPawn && targetPawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiTechPsy_DD_LifeLink))
                {
                    Hediff hediff = targetPawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_DD_LifeLink);
                    float accumulateDamge = hediff.TryGetComp<HediffComp_LifeLink>().totalDamage;
                    foreach (Pawn linkedPawn in hediff.TryGetComp<HediffComp_LifeLink>().linkedPawn)
                    {
                        linkedPawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_DD_LifeLink).TryGetComp<HediffComp_LifeLink>().totalDamage /= 2;
                    }
                    GenExplosion.DoExplosion(targetPawn.Position, pawn.Map, Mathf.Min(3, accumulateDamge / 10), modExtension.damageDef, pawn, Mathf.FloorToInt(accumulateDamge*0.75f), 1f, null, null, null, targetPawn, null, 0, 0, GasType.RotStink);
                    Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                    Messages.Message("Makai_FailArollcheckLinkDetonate".Translate(pawn.LabelShort,targetPawn.LabelShort,accumulateDamge, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                }
                else
                {
                    Messages.Message("Target doesn't have Life Link", MessageTypeDefOf.NegativeEvent);
                }
            }
        }
    }
}
