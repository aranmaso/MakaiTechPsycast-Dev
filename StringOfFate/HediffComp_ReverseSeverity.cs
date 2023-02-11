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
                    if(item is Hediff_Level || item.def == parent.def || item.def.hediffClass.IsSubclassOf(typeof(Hediff_Level)) || item is Hediff_LevelWithoutPart)
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
                    || item.def == MakaiTechPsy_DefOf.Destined_Death
                    || item.def == MakaiTechPsy_DefOf.MakaiPsy_SF_UltimateFate
                    || item.def.defName == "Destruction_AutoAttack"
                    || item.def.defName == "Order_Enlightenment"
                    || item.def.defName == "Corruption_Avatar"
                    || item.def.defName == "Death_Avatar"
                    || item.def.defName == "Flame_Avatar"
                    || item.def.defName == "Frost_Avatar"
                    || item.def.defName == "Knowledge_Avatar"
                    || item.def.defName == "Existence_Resurrection"
                    || item.def.defName == "Penis"
                    || item.def.defName == "Vagina"
                    || item.def.defName == "Breasts"
                    || item.def.defName == "FeaturelessChest"
                    || item.def.defName == "DemonTentaclePenis"
                    || item.def.defName == "DemonPenis"
                    || item.def.defName == "DemonVagina"
                    || item.def.defName == "DemonAnus"
                    || item.def.defName == "PegDick"
                    || item.def.defName == "HydraulicPenis"
                    || item.def.defName == "HydraulicVagina"
                    || item.def.defName == "HydraulicBreasts"
                    || item.def.defName == "BionicPenis"
                    || item.def.defName == "BionicVagina"
                    || item.def.defName == "BionicBreasts"
                    || item.def.defName == "BionicAnus"
                    || item.def.defName == "ArchotechPenis"
                    || item.def.defName == "ArchotechVagina"
                    || item.def.defName == "ArchotechBreasts"
                    || item.def.defName == "ArchotechAnus"
                    || item.def.defName == "Anus")
                    {
                        continue;
                    }
                    if (item.def.maxSeverity <= 1f && (item.def != HediffDefOf.BloodLoss) && (item.def != HediffDefOf.Hypothermia) && (item.def != HediffDefOf.Heatstroke) && (item.def != HediffDefOf.PsychicAmplifier) && (item.def != HediffDefOf.ToxicBuildup) && (item.def != HediffDefOf.Malnutrition) && (item.def != HediffDefOf.Anesthetic) && (item.def != parent.def) && !(item is Hediff_Injury))
                    {
                        if (item.def.maxSeverity <= 1f && item.TryGetComp<HediffComp_SeverityPerDay>() != null && item.TryGetComp<HediffComp_SeverityPerDay>().SeverityChangePerDay() < 0 && !item.def.isBad)
                        {
                            item.Severity += Props.severityToReverse;
                        }
                        HediffComp_Disappears hediffComp_Disappears = item.TryGetComp<HediffComp_Disappears>();
                        if (hediffComp_Disappears != null)
                        {
                            hediffComp_Disappears.ticksToDisappear += Mathf.Min(Props.tickIncrease, 5000);
                        }
                    }
                    else if (item.def.maxSeverity <= 1f && (item.def == HediffDefOf.BloodLoss || item.def == HediffDefOf.Heatstroke || item.def == HediffDefOf.Hypothermia || item.def == HediffDefOf.ToxicBuildup || item.def == HediffDefOf.Malnutrition || (item.def == HediffDefOf.Anesthetic)) && item.def != parent.def && !(item is Hediff_Injury) || item.def.isBad)
                    {
                        item.Severity -= Props.severityToReverse;
                    }
                    else if(item is Hediff_Injury)
                    {
                        item.Severity -= Rand.Range(Props.severityToReverse,10f);
                    }
                    else if (item.def.maxSeverity > 1f && item.def != parent.def && !item.def.isBad)
                    {
                        item.Severity += Props.severityToReverse;
                        HediffComp_Disappears hediffComp_Disappears = item.TryGetComp<HediffComp_Disappears>();
                        if (hediffComp_Disappears != null)
                        {
                            hediffComp_Disappears.ticksToDisappear += Mathf.Min(Props.tickIncrease, 5000);
                        }
                    }
                    else if(item.def.maxSeverity > 1f && item.def != parent.def && item.def.isBad && !(item is Hediff_Injury))
                    {
                        item.Severity -= Props.severityToReverse;
                        HediffComp_Disappears hediffComp_Disappears = item.TryGetComp<HediffComp_Disappears>();
                        if (hediffComp_Disappears != null)
                        {
                            hediffComp_Disappears.ticksToDisappear -= Mathf.Min(Props.tickIncrease, 5000);
                        }
                    }
                    else if (item is Hediff_Injury && item.IsTended())
                    {
                        item.Severity -= Rand.Range(Props.severityToReverse, 10f);
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
