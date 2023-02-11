using RimWorld;
using RimWorld.Planet;
using Verse;
using VFECore;
using VanillaPsycastsExpanded;
using UnityEngine;

namespace MakaiTechPsycast.GoldenOrder
{
    public class Ability_BlackThread : VFECore.Abilities.Ability
    {
        AbilityExtension_Roll1D20 modExtension => def.GetModExtension<AbilityExtension_Roll1D20>();
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
            RollInfo rollinfo = new RollInfo();
            rollinfo = MakaiUtility.Roll1D20(pawn, modExtension.skillBonus, rollinfo);            
            if (rollinfo.roll >= modExtension.successThreshold && rollinfo.roll < modExtension.greatSuccessThreshold)
            {
                Hediff hediff = MakaiUtility.CreateCustomHediffWithDuration(pawn,modExtension.hediffDefWhenSuccess,modExtension.hours,modExtension.ticks);
                pawn.health.AddHediff(hediff);
                Projectile projectile = (Projectile)GenSpawn.Spawn(modExtension.projectileWhenSuccess, targets[0].Cell, pawn.Map);
                projectile.Launch(pawn, targets[0].Cell.ToVector3(), targets[1].Cell, targets[1].Cell,ProjectileHitFlags.IntendedTarget);
                MakaiUtility.GetFirstHediffOfDef(pawn, MakaiTechPsy_DefOf.MakaiTechPsy_GD_PathOfNaraka).TryGetComp<HediffComp_PathOfNaraka>().currentStack -= modExtension.costs;
                Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                Messages.Message("Makai_PassArollcheckBlackThread".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
            }
            if (rollinfo.roll >= modExtension.greatSuccessThreshold)
            {
                Hediff hediff = MakaiUtility.CreateCustomHediffWithDuration(pawn, modExtension.hediffDefWhenSuccess, modExtension.hours, modExtension.ticks);
                pawn.health.AddHediff(hediff);
                Projectile projectile = (Projectile)GenSpawn.Spawn(modExtension.projectileWhenSuccess, targets[0].Cell, pawn.Map);
                projectile.Launch(pawn, targets[0].Cell.ToVector3(), targets[1].Cell, targets[1].Cell, ProjectileHitFlags.IntendedTarget);
                MakaiUtility.GetFirstHediffOfDef(pawn, MakaiTechPsy_DefOf.MakaiTechPsy_GD_PathOfNaraka).TryGetComp<HediffComp_PathOfNaraka>().currentStack -= modExtension.costs;
                Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                Messages.Message("Makai_GreatPassArollcheckBlackThread".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
            }
            if (rollinfo.roll < modExtension.successThreshold)
            {
                Hediff hediff = MakaiUtility.CreateCustomHediffWithDuration(pawn, modExtension.hediffDefWhenSuccess, modExtension.hours, modExtension.ticks);
                pawn.health.AddHediff(hediff);
                Projectile projectile = (Projectile)GenSpawn.Spawn(modExtension.projectileWhenSuccess, targets[0].Cell, pawn.Map);
                projectile.Launch(pawn, targets[0].Cell.ToVector3(), targets[1].Cell, targets[1].Cell, ProjectileHitFlags.IntendedTarget);
                MakaiUtility.GetFirstHediffOfDef(pawn, MakaiTechPsy_DefOf.MakaiTechPsy_GD_PathOfNaraka).TryGetComp<HediffComp_PathOfNaraka>().currentStack -= modExtension.costs;
                Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
                Messages.Message("Makai_FailArollcheckBlackThread".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
            }
        }
    }
}
