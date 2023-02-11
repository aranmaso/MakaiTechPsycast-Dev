using RimWorld;
using RimWorld.Planet;
using Verse;
using VFECore;
using VanillaPsycastsExpanded;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace MakaiTechPsycast.DistortedReality
{
    public class Ability_DistortedBlessing : VFECore.Abilities.Ability
    {
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            AbilityExtension_Roll1D20 modExtension = def.GetModExtension<AbilityExtension_Roll1D20>();
            int threshold1 = Rand.Range(1, 21);
            int threshold2 = Rand.Range(1, 21);
            RollInfo rollinfo = new RollInfo();
            List<SkillDef> randomSkill = DefDatabase<SkillDef>.AllDefs.ToList();
            rollinfo = MakaiUtility.Roll1D20(pawn, randomSkill.RandomElement(), rollinfo);
            if (rollinfo.roll >= Mathf.Min(threshold1, threshold2) && rollinfo.roll < Mathf.Max(threshold1, threshold2))
            {
                if (targets[0].Thing is Pawn targetPawn)
                {
                    if (targetPawn.health.hediffSet.HasHediff(modExtension.hediffDefWhenSuccess))
                    {
                        targetPawn.health.hediffSet.GetFirstHediffOfDef(modExtension.hediffDefWhenSuccess).TryGetComp<HediffComp_Disappears>().ticksToDisappear += Rand.Range(1000, 10000);
                    }
                    else if (!targetPawn.health.hediffSet.HasHediff(modExtension.hediffDefWhenSuccess))
                    {
                        MakaiUtility.CreateCustomHediffWithDuration(targetPawn,modExtension.hediffDefWhenSuccess,modExtension.hours,modExtension.ticks);
                        targetPawn.health.AddHediff(hediff);
                    }
                    Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                    Messages.Message("Makai_PassArollcheckDistortedBlessing".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                }

            }
            if (rollinfo.roll >= Mathf.Max(threshold1, threshold2))
            {
                if (targets[0].Thing is Pawn targetPawn)
                {
                    if (targetPawn.health.hediffSet.HasHediff(modExtension.hediffDefWhenSuccess))
                    {
                        targetPawn.health.hediffSet.GetFirstHediffOfDef(modExtension.hediffDefWhenSuccess).TryGetComp<HediffComp_Disappears>().ticksToDisappear += Rand.Range(1000, 10000);
                    }
                    else if (!targetPawn.health.hediffSet.HasHediff(modExtension.hediffDefWhenSuccess))
                    {
                        MakaiUtility.CreateCustomHediffWithDuration(targetPawn, modExtension.hediffDefWhenSuccess, modExtension.hours*2, modExtension.ticks);
                        targetPawn.health.AddHediff(hediff);
                    }
                    Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                    Messages.Message("Makai_GreatPassArollcheckDistortedBlessing".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                }

            }
            if (rollinfo.roll < Mathf.Min(threshold1, threshold2))
            {
                if (targets[0].Thing is Pawn targetPawn)
                {
                    if (targetPawn.health.hediffSet.HasHediff(modExtension.hediffDefWhenSuccess))
                    {
                        targetPawn.health.hediffSet.GetFirstHediffOfDef(modExtension.hediffDefWhenSuccess).TryGetComp<HediffComp_Disappears>().ticksToDisappear += Rand.Range(1000, 10000);
                    }
                    else if (!targetPawn.health.hediffSet.HasHediff(modExtension.hediffDefWhenSuccess))
                    {
                        MakaiUtility.CreateCustomHediffWithDuration(targetPawn, modExtension.hediffDefWhenSuccess, modExtension.hours/2, modExtension.ticks);
                        targetPawn.health.AddHediff(hediff);
                    }
                    Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
                    Messages.Message("Makai_FailArollcheckDistortedBlessing".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
                }

            }
        }
    }
}
