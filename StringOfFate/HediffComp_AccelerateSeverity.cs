using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace MakaiTechPsycast.StringOfFate
{
    public class HediffComp_AccelerateSeverity : HediffComp
    {

        public HediffCompProperties_AccelerateSeverity Props => (HediffCompProperties_AccelerateSeverity)props;

        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            if(Pawn.IsHashIntervalTick(Props.interval))
            {
                AccelerateSeverity();
            }
        }

        public void AccelerateSeverity()
        {
            IEnumerable<Hediff> hediffs = Pawn.health.hediffSet.hediffs;
            foreach(Hediff item in hediffs)
            {
                if(item is Hediff_Level)
                {
                    continue;
                }
                if(item.def == MakaiTechPsy_DefOf.MakaiTechPsy_DD_LichSoul
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
                if (item.def.maxSeverity <= 1f && (item.def != HediffDefOf.BloodLoss) && (item.def != HediffDefOf.Hypothermia) && (item.def != HediffDefOf.Heatstroke) && (item.def != HediffDefOf.PsychicAmplifier) && (item.def != HediffDefOf.ToxicBuildup) && (item.def != HediffDefOf.Malnutrition) && (item.def != HediffDefOf.Anesthetic) && (item.def != parent.def))
                {
                    if (item.def.maxSeverity <= 1f && item.TryGetComp<HediffComp_SeverityPerDay>().SeverityChangePerDay() < 0 && !item.def.isBad)
                    {
                        item.Severity -= Props.severityToAccelerate;
                    }
                    HediffComp_Disappears hediffComp_Disappears = item.TryGetComp<HediffComp_Disappears>();
                    if (hediffComp_Disappears != null)
                    {
                        hediffComp_Disappears.ticksToDisappear -= Mathf.Min(Props.tickIncrease, 2500);
                    }                    
                }
                else if ((item.def == HediffDefOf.BloodLoss || item.def == HediffDefOf.Heatstroke || item.def == HediffDefOf.Hypothermia || item.def == HediffDefOf.ToxicBuildup || item.def == HediffDefOf.Malnutrition || item.def == HediffDefOf.Anesthetic) && item.def != parent.def)
                {
                    item.Severity += Props.severityToAccelerate;
                }
                else if (item.def.maxSeverity > 1f && item.def != parent.def && item.def.isBad == false && !(item is Hediff_Injury))
                {
                    item.Severity -= Props.severityToAccelerate;
                    HediffComp_Disappears hediffComp_Disappears = item.TryGetComp<HediffComp_Disappears>();
                    if (hediffComp_Disappears != null)
                    {
                        hediffComp_Disappears.ticksToDisappear -= Mathf.Min(Props.tickIncrease, 2500);
                    }
                }
                else if(item is Hediff_Injury && item.IsTended())
                {
                    item.Severity -= Props.severityToAccelerate;
                }
            }
            if (Pawn.health.hediffSet.GetInjuriesTendable().EnumerableCount() > 0)
            {
                Hediff_Injury inju = MakaiUtility.FindInjury(Pawn);
                Pawn.health.RemoveHediff(inju);
                parent.TryGetComp<HediffComp_Disappears>().ticksToDisappear -= 250;
            }
            if (Pawn.health.hediffSet.GetMissingPartsCommonAncestors().Count > 0)
            {
                MakaiUtility.RestorePart(MakaiUtility.FindSmallestMissingBodyPart(Pawn), Pawn);
                parent.TryGetComp<HediffComp_Disappears>().ticksToDisappear -= 1000;
            }
        }
    }
}
