using RimWorld;
using RimWorld.Planet;
using Verse;
using VFECore;
using System.Collections.Generic;
using VanillaPsycastsExpanded;
using UnityEngine;

namespace MakaiTechPsycast.DestinedDeath
{
    public class Ability_LifeLink : VFECore.Abilities.Ability
    {
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            AbilityExtension_Roll1D20 modExtension = def.GetModExtension<AbilityExtension_Roll1D20>();
            RollInfo rollinfo = new RollInfo();
            rollinfo = MakaiUtility.Roll1D20(pawn, modExtension.skillBonus, rollinfo);
            List<Thing> AllPawn = new List<Thing>();
            if (rollinfo.roll >= modExtension.successThreshold && rollinfo.roll < modExtension.greatSuccessThreshold)
            {
                foreach (GlobalTargetInfo globalTargetInfo in targets)
                {
                    AllPawn.Add(globalTargetInfo.Thing);
                    if (globalTargetInfo.Thing is Pawn linkedPawn)
                    {
                        if(linkedPawn.health.hediffSet.HasHediff(modExtension.hediffDefWhenSuccess))
                        {
                            Hediff oldLink = linkedPawn.health.hediffSet.GetFirstHediffOfDef(modExtension.hediffDefWhenSuccess);
                            linkedPawn.health.RemoveHediff(oldLink);
                        }
                        Hediff hediff = MakaiUtility.ApplyCustomHediffWithDuration(linkedPawn, modExtension.hediffDefWhenSuccess, modExtension.hours, modExtension.ticks, modExtension.multiplier);
                        hediff.TryGetComp<HediffComp_LifeLink>().linkedPawn = AllPawn;
                        linkedPawn.health.AddHediff(hediff);
                    }
                }
                Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                Messages.Message("Makai_PassArollcheckLifeLink".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
            }
            if (rollinfo.roll >= modExtension.greatSuccessThreshold)
            {
                foreach (GlobalTargetInfo globalTargetInfo in targets)
                {
                    AllPawn.Add(globalTargetInfo.Thing);
                    if (globalTargetInfo.Thing is Pawn linkedPawn)
                    {
                        if (linkedPawn.health.hediffSet.HasHediff(modExtension.hediffDefWhenSuccess))
                        {
                            Hediff oldLink = linkedPawn.health.hediffSet.GetFirstHediffOfDef(modExtension.hediffDefWhenSuccess);
                            linkedPawn.health.RemoveHediff(oldLink);
                        }
                        Hediff hediff = MakaiUtility.ApplyCustomHediffWithDuration(linkedPawn, modExtension.hediffDefWhenSuccess, modExtension.hours, modExtension.ticks, modExtension.multiplier);
                        hediff.TryGetComp<HediffComp_LifeLink>().linkedPawn = AllPawn;
                        linkedPawn.health.AddHediff(hediff);
                    }
                }
                Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                Messages.Message("Makai_GreatPassArollcheckLifeLink".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
            }
            if (rollinfo.roll < modExtension.successThreshold)
            {
                foreach (GlobalTargetInfo globalTargetInfo in targets)
                {
                    AllPawn.Add(globalTargetInfo.Thing);
                    if (globalTargetInfo.Thing is Pawn linkedPawn)
                    {
                        if (linkedPawn.health.hediffSet.HasHediff(modExtension.hediffDefWhenSuccess))
                        {
                            Hediff oldLink = linkedPawn.health.hediffSet.GetFirstHediffOfDef(modExtension.hediffDefWhenSuccess);
                            linkedPawn.health.RemoveHediff(oldLink);
                        }
                        Hediff hediff = MakaiUtility.ApplyCustomHediffWithDuration(linkedPawn, modExtension.hediffDefWhenSuccess, modExtension.hours, modExtension.ticks, modExtension.multiplier);
                        hediff.TryGetComp<HediffComp_LifeLink>().linkedPawn = AllPawn;
                        linkedPawn.health.AddHediff(hediff);
                    }
                }
                Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
                Messages.Message("Makai_FailArollcheckLifeLink".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
            }
        }
    }
}
