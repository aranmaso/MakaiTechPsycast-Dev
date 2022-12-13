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
                IEnumerable<Hediff> hediffs = pawn.health.hediffSet.hediffs;
                foreach (Hediff item in hediffs)
                {
                    if(item is Hediff_Level || item.def == parent.def)
                    {
                        continue;
                    }
                    if (item.def == MakaiTechPsy_DefOf.MakaiTechPsy_DD_LichSoul
                    || item.def == MakaiTechPsy_DefOf.MakaiTechPsy_DD_MissingSoul
                    || item.def == MakaiTechPsy_DefOf.MakaiPsy_SF_Counter
                    || item.def == MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul
                    || item.def == MakaiTechPsy_DefOf.MakaiTechPsy_DR_DistortBulletBounce
                    || item.def == MakaiTechPsy_DefOf.MakaiPsy_SF_Reverse
                    || item.def == MakaiTechPsy_DefOf.MakaiPsy_SF_Accelerate
                    || item.def == MakaiTechPsy_DefOf.Destined_Death)
                    {
                        continue;
                    }
                    if (item.def.maxSeverity <= 1f && (item.def != HediffDefOf.BloodLoss) && (item.def != HediffDefOf.Hypothermia) && (item.def != HediffDefOf.Heatstroke) && (item.def != HediffDefOf.PsychicAmplifier) && (item.def != HediffDefOf.ToxicBuildup) && (item.def != HediffDefOf.Malnutrition) && (item.def != HediffDefOf.Anesthetic) && (item.def != parent.def) && !(item is Hediff_Injury))
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
                    else if ((item.def == HediffDefOf.BloodLoss || item.def == HediffDefOf.Heatstroke || item.def == HediffDefOf.Hypothermia || item.def == HediffDefOf.ToxicBuildup || item.def == HediffDefOf.Malnutrition || (item.def == HediffDefOf.Anesthetic)) && item.def != parent.def && !(item is Hediff_Injury))
                    {
                        item.Severity -= Props.severityToReverse;
                    }
                    else if(item is Hediff_Injury)
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
                    else if(item.def.maxSeverity > 1f && item.def != parent.def && item.def.isBad == true && !(item is Hediff_Injury))
                    {
                        item.Severity += Props.severityToReverse;
                        HediffComp_Disappears hediffComp_Disappears = item.TryGetComp<HediffComp_Disappears>();
                        if (hediffComp_Disappears != null)
                        {
                            hediffComp_Disappears.ticksToDisappear -= Mathf.Min(Props.tickIncrease, 5000);
                        }
                    }
                    else if (item is Hediff_Injury && item.IsTended())
                    {
                        item.Severity -= Props.severityToReverse;
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
                /*Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_Ring_ExpandY.Spawn(pawn.Position, pawn.Map, 0.5f);
                effect.Cleanup();*/
                tickSinceTrigger = 0;
            }
        }
    }
}
