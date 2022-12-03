using RimWorld;
using RimWorld.Planet;
using Verse;
using System.Collections.Generic;
using System.Linq;
using VFECore;
using VanillaPsycastsExpanded;
using UnityEngine;

namespace MakaiTechPsycast.DistortedReality
{
    public class Ability_DistortedReality : VFECore.Abilities.Ability
    {
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            AbilityExtension_Roll1D20 modExtension = def.GetModExtension<AbilityExtension_Roll1D20>();
            int threshold1 = Rand.Range(1, 21);
            int threshold2 = Rand.Range(1, 21);
            RollInfo rollinfo = new RollInfo();
            List<SkillDef> randomSkill = DefDatabase<SkillDef>.AllDefs.ToList();
            rollinfo = MakaiUtility.Roll1D20(pawn, modExtension.skillBonus, rollinfo);
            if(pawn.health.hediffSet.HasHediff(modExtension.hediffDefWhenFail))
            {
                rollinfo.roll += pawn.health.hediffSet.GetFirstHediffOfDef(modExtension.hediffDefWhenFail).TryGetComp<HediffComp_DistortedUltimateCount>().FailCount;
            }
            if (rollinfo.roll >= modExtension.successThreshold && rollinfo.roll < modExtension.greatSuccessThreshold)
            {
                if (pawn.health.hediffSet.HasHediff(modExtension.hediffDefWhenFail))
                {
                    if(pawn.health.hediffSet.GetFirstHediffOfDef(modExtension.hediffDefWhenFail).TryGetComp<HediffComp_DistortedUltimateCount>().FailCount > 0)
                    {
                        pawn.health.hediffSet.GetFirstHediffOfDef(modExtension.hediffDefWhenFail).TryGetComp<HediffComp_DistortedUltimateCount>().FailCount -= 1;
                    }
                }
                Find.WindowStack.Add(new Dialog_ChooseIncidentCategory(pawn.Map, 5, 1));
                Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll + pawn.health.hediffSet.GetFirstHediffOfDef(modExtension.hediffDefWhenFail).TryGetComp<HediffComp_DistortedUltimateCount>().FailCount, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                Messages.Message("Makai_PassArollcheckDistortUltimate".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
            }
            if (rollinfo.roll >= modExtension.greatSuccessThreshold)
            {
                Find.WindowStack.Add(new Dialog_ChooseIncidentCategory(pawn.Map, 8, 2));
                if(pawn.health.hediffSet.HasHediff(modExtension.hediffDefWhenFail))
                {
                    pawn.health.RemoveHediff(pawn.health.hediffSet.GetFirstHediffOfDef(modExtension.hediffDefWhenFail));
                }
                Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll + pawn.health.hediffSet.GetFirstHediffOfDef(modExtension.hediffDefWhenFail).TryGetComp<HediffComp_DistortedUltimateCount>().FailCount, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                Messages.Message("Makai_GreatPassArollcheckDistortUltimate".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
            }
            if (rollinfo.roll < modExtension.successThreshold)
            {
                if (pawn.health.hediffSet.HasHediff(modExtension.hediffDefWhenFail))
                {
                    pawn.health.hediffSet.GetFirstHediffOfDef(modExtension.hediffDefWhenFail).TryGetComp<HediffComp_DistortedUltimateCount>().FailCount += 1;
                }
                else
                {
                    Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenFail, pawn);
                    hediff.TryGetComp<HediffComp_DistortedUltimateCount>().FailCount = 1;
                    pawn.health.AddHediff(hediff);
                }
                Find.WindowStack.Add(new Dialog_ChooseIncidentCategory(pawn.Map, 8, 3));
                Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll + pawn.health.hediffSet.GetFirstHediffOfDef(modExtension.hediffDefWhenFail).TryGetComp<HediffComp_DistortedUltimateCount>().FailCount, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
                Messages.Message("Makai_FailArollcheckDistortUltimate".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
            }
        }
    }
}
