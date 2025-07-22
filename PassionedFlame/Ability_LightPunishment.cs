using RimWorld;
using RimWorld.Planet;
using Verse;
using VEF;
using VanillaPsycastsExpanded;
using UnityEngine;
using Verse.Sound;

namespace MakaiTechPsycast.PassionedFlame
{
    public class Ability_LightPunishment : VEF.Abilities.Ability
    {
        public AbilityExtension_Roll1D20 modExtension => def.GetModExtension<AbilityExtension_Roll1D20>();
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            RollInfo rollinfo = new RollInfo();
            rollinfo = MakaiUtility.Roll1D20PassionCount(pawn, rollinfo);
            if (rollinfo.roll >= modExtension.successThreshold && rollinfo.roll < modExtension.greatSuccessThreshold)
            {
                foreach (GlobalTargetInfo globalTargetInfo in targets)
                {
                    if (globalTargetInfo.Thing is Pawn targetPawn)
                    {
                        Hediff hediff = MakaiUtility.CreateCustomHediffWithDurationNoMultiplier(targetPawn, modExtension.hediffDefWhenSuccess, modExtension.hours, modExtension.ticks);
                        int p = 1;
                        foreach (SkillRecord item in pawn.skills.skills)
                        {
                            if (item.passion != Passion.None) p++;
                        }
                        hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear /= p;
                        hediff.TryGetComp<HediffComp_DoThingOnExpire>().damamgeBonus = 0;
                        targetPawn.health.AddHediff(hediff);
                        
                        Vector3 fleckPosition = targetPawn.Position.ToVector3Shifted();
                        if (modExtension.fleckPosOffset != null)
                        {
                            fleckPosition += modExtension.fleckPosOffset;
                        }
                        Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_PF_PillarOfLightSpawned.Spawn(fleckPosition.ToIntVec3(), pawn.Map, 1f);
                        effect.Cleanup();
                        MakaiTechPsy_DefOf.FlamingEffect.PlayOneShot(new TargetInfo(fleckPosition.ToIntVec3(), targetPawn.Map));
                        MakaiUtility.ThrowFleckStationary(modExtension.fleckDef, fleckPosition, targetPawn.Map, 1f);
                        Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                        Messages.Message("Makai_PassArollcheckGiveHediffGeneric".Translate(pawn.LabelShort, hediff.LabelCap, targetPawn.LabelShort, pawn.Named("USER"), targetPawn.Named("USER2")), pawn, MessageTypeDefOf.PositiveEvent);
                    }
                }
            }
            if (rollinfo.roll >= modExtension.greatSuccessThreshold)
            {
                foreach (GlobalTargetInfo globalTargetInfo in targets)
                {
                    if (globalTargetInfo.Thing is Pawn targetPawn)
                    {
                        Hediff hediff = MakaiUtility.CreateCustomHediffWithDurationNoMultiplier(targetPawn, modExtension.hediffDefWhenGreatSuccess ?? modExtension.hediffDefWhenSuccess, modExtension.hours * 2, modExtension.ticks);
                        int p = 1;
                        foreach (SkillRecord item in pawn.skills.skills)
                        {
                            if (item.passion != Passion.None) p++;
                        }
                        hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear /= p;
                        hediff.TryGetComp<HediffComp_DoThingOnExpire>().damamgeBonus = 25;
                        targetPawn.health.AddHediff(hediff);
                        Vector3 fleckPosition = targetPawn.Position.ToVector3Shifted();
                        if (modExtension.fleckPosOffset != null)
                        {
                            fleckPosition += modExtension.fleckPosOffset;
                        }
                        Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_PF_PillarOfLightSpawned.Spawn(fleckPosition.ToIntVec3(), pawn.Map, 1f);
                        effect.Cleanup();
                        MakaiTechPsy_DefOf.FlamingEffect.PlayOneShot(new TargetInfo(fleckPosition.ToIntVec3(), targetPawn.Map));
                        MakaiUtility.ThrowFleckStationary(modExtension.fleckDef, fleckPosition, targetPawn.Map, 1f);
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
                        Hediff hediff = MakaiUtility.CreateCustomHediffWithDurationNoMultiplier(targetPawn, modExtension.hediffDefWhenFail ?? modExtension.hediffDefWhenSuccess, modExtension.hours / 2, modExtension.ticks);
                        int p = 1;
                        foreach (SkillRecord item in pawn.skills.skills)
                        {
                            if (item.passion != Passion.None) p++;
                        }
                        hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear /= p;
                        hediff.TryGetComp<HediffComp_DoThingOnExpire>().damamgeBonus = -25;
                        targetPawn.health.AddHediff(hediff);
                        Vector3 fleckPosition = targetPawn.Position.ToVector3Shifted();
                        if (modExtension.fleckPosOffset != null)
                        {
                            fleckPosition += modExtension.fleckPosOffset;
                        }
                        Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_PF_PillarOfLightSpawned.Spawn(fleckPosition.ToIntVec3(), pawn.Map, 1f);
                        effect.Cleanup();
                        MakaiTechPsy_DefOf.FlamingEffect.PlayOneShot(new TargetInfo(fleckPosition.ToIntVec3(), targetPawn.Map));
                        MakaiUtility.ThrowFleckStationary(modExtension.fleckDef, fleckPosition, targetPawn.Map, 1f);
                        Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
                        Messages.Message("Makai_FailArollcheckGiveHediffGeneric".Translate(pawn.LabelShort, hediff.LabelCap, targetPawn.LabelShort, pawn.Named("USER"), targetPawn.Named("USER2")), pawn, MessageTypeDefOf.NegativeEvent);
                    }
                }
            }
        }
    }
}
