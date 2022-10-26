using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace MakaiTechPsycast.StringOfFate
{
    public class HediffComp_ReverseSeverity : HediffComp
    {
        public int tickSinceTrigger;
        public HediffCompProperties_ReverseSeverity Props => (HediffCompProperties_ReverseSeverity)props;

        public override void CompExposeData()
        {
            Scribe_Values.Look(ref tickSinceTrigger, "tickSinceTrigger", 0);
        }
        public override void CompPostTick(ref float severityAdjustment)
        {
            tickSinceTrigger++;
            Pawn pawn = parent.pawn;
            if (tickSinceTrigger > Props.interval)
            {
                List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
                foreach (Hediff item in hediffs)
                {
                    if (item.def.maxSeverity <= 1f && (item.def != HediffDefOf.BloodLoss) && (item.def != HediffDefOf.Hypothermia) && (item.def != HediffDefOf.Heatstroke) && (item.def != HediffDefOf.PsychicAmplifier) && (item.def != HediffDefOf.ToxicBuildup) && (item.def != HediffDefOf.Malnutrition) && (item.def != parent.def))
                    {
                        if (item.def.maxSeverity <= 1f && item.TryGetComp<HediffComp_SeverityPerDay>().SeverityChangePerDay() < 0 && !item.def.isBad)
                        {
                            item.Severity += Props.severityToReverse;
                        }
                        HediffComp_Disappears hediffComp_Disappears = item.TryGetComp<HediffComp_Disappears>();
                        if (hediffComp_Disappears != null)
                        {
                            hediffComp_Disappears.ticksToDisappear += Mathf.Min(Props.tickIncrease, 5000);
                        }
                    }
                    else if ((item.def == HediffDefOf.BloodLoss || item.def == HediffDefOf.Heatstroke || item.def == HediffDefOf.Hypothermia || item.def == HediffDefOf.ToxicBuildup || item.def == HediffDefOf.Malnutrition) && item.def != parent.def)
                    {
                        item.Severity -= Props.severityToReverse;
                    }
                    else if (item.def.maxSeverity > 1f && item.def != parent.def && item.def.isBad == false)
                    {
                        item.Severity += Props.severityToReverse;
                        HediffComp_Disappears hediffComp_Disappears = item.TryGetComp<HediffComp_Disappears>();
                        if (hediffComp_Disappears != null)
                        {
                            hediffComp_Disappears.ticksToDisappear += Mathf.Min(Props.tickIncrease, 5000);
                        }
                    }
                    else if(item.def.maxSeverity > 1f && item.def != parent.def && item.def.isBad == true)
                    {
                        item.Severity += Props.severityToReverse;
                        HediffComp_Disappears hediffComp_Disappears = item.TryGetComp<HediffComp_Disappears>();
                        if (hediffComp_Disappears != null)
                        {
                            hediffComp_Disappears.ticksToDisappear -= Mathf.Min(Props.tickIncrease, 5000);
                        }
                    }
                }
                if (pawn.health.hediffSet.GetInjuriesTendable().EnumerableCount() > 0)
                {
                    Hediff_Injury inju = MakaiUtility.FindInjury(pawn);
                    pawn.health.RemoveHediff(inju);
                    parent.TryGetComp<HediffComp_Disappears>().ticksToDisappear -= 250;
                }
                if(pawn.health.hediffSet.GetMissingPartsCommonAncestors().Count > 0)
                {
                    MakaiUtility.RestorePart(MakaiUtility.FindSmallestMissingBodyPart(pawn),pawn);
                    parent.TryGetComp<HediffComp_Disappears>().ticksToDisappear -= 1000;
                }
                Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_Ring_ExpandY.Spawn(pawn.Position, pawn.Map, 0.5f);
                effect.Cleanup();
                tickSinceTrigger = 0;
            }
        }
    }
}
