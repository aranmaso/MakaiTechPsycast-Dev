using RimWorld;
using RimWorld.Planet;
using Verse;
using System.Collections.Generic;
using System.Linq;
using VEF;
using VanillaPsycastsExpanded;
using UnityEngine;

namespace MakaiTechPsycast.PassionedFlame
{
    public class Ability_GiveHediffPassionCurved : VEF.Abilities.Ability
    {
        public AbilityExtension_Roll1D20 modExtension => def.GetModExtension<AbilityExtension_Roll1D20>();
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            RollInfo rollinfo = new RollInfo();
            rollinfo = MakaiUtility.Roll1D20PassionCount(pawn, rollinfo);
            List<SkillRecord> skillSet = pawn.skills.skills;
            if (rollinfo.roll >= modExtension.successThreshold && rollinfo.roll < modExtension.greatSuccessThreshold)
            {                
                foreach (GlobalTargetInfo globalTargetInfo in targets)
                {
                    if (globalTargetInfo.Thing is Pawn targetPawn)
                    {                        
                        for(int i = skillSet.Count - 1; i >= 0; i--)
                        {
                            if (skillSet[i].passion >= Passion.Major)
                            {
                                HediffDef hDef = modExtension.hediffsToApply.FirstOrDefault(x => x.defName == "MakaiTechPsy_PF_InspiringFlame_" + skillSet[i].def);
                                //Log.Message("Def =" + hDef.defName);
                                Hediff hediff = MakaiUtility.CreateCustomHediffWithDurationNoMultiplier(targetPawn, hDef, modExtension.hours, modExtension.ticks);
                                float total = modExtension.curve.Evaluate(skillSet[i].Level);
                                hediff.Severity = Mathf.FloorToInt(total);
                                targetPawn.health.AddHediff(hediff);
                                Messages.Message("Makai_PassArollcheckGiveHediffGeneric".Translate(pawn.LabelShort, hediff.LabelCap, targetPawn.LabelShort, pawn.Named("USER"), targetPawn.Named("USER2")), pawn, MessageTypeDefOf.PositiveEvent);
                            }
                        }                        
                        Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);                        
                    }
                }
            }
            if (rollinfo.roll >= modExtension.greatSuccessThreshold)
            {
                foreach (GlobalTargetInfo globalTargetInfo in targets)
                {
                    if (globalTargetInfo.Thing is Pawn targetPawn)
                    {
                        for (int i = skillSet.Count - 1; i >= 0; i--)
                        {
                            if (skillSet[i].passion >= Passion.Minor)
                            {
                                HediffDef hDef = modExtension.hediffsToApply.FirstOrDefault(x => x.defName == "MakaiTechPsy_PF_InspiringFlame_" + skillSet[i].def);
                                //Log.Message("Def =" + hDef.defName);
                                Hediff hediff = MakaiUtility.CreateCustomHediffWithDurationNoMultiplier(targetPawn, hDef, modExtension.hours, modExtension.ticks);
                                float total = modExtension.curve.Evaluate(skillSet[i].Level);
                                hediff.Severity = Mathf.FloorToInt(total);
                                targetPawn.health.AddHediff(hediff);
                                Messages.Message("Makai_GreatPassArollcheckGiveHediffGeneric".Translate(pawn.LabelShort, hediff.LabelCap, targetPawn.LabelShort, pawn.Named("USER"), targetPawn.Named("USER2")), pawn, MessageTypeDefOf.PositiveEvent);
                            }
                        }
                        Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);                        
                    }
                }
            }
            if (rollinfo.roll < modExtension.successThreshold)
            {
                foreach (GlobalTargetInfo globalTargetInfo in targets)
                {
                    if (globalTargetInfo.Thing is Pawn targetPawn)
                    {
                        for (int i = skillSet.Count - 1; i >= 0; i--)
                        {
                            if (skillSet[i].passion == Passion.Minor)
                            {
                                HediffDef hDef = modExtension.hediffsToApply.FirstOrDefault(x => x.defName == "MakaiTechPsy_PF_InspiringFlame_" + skillSet[i].def);
                                //Log.Message("Def =" + hDef.defName);
                                Hediff hediff = MakaiUtility.CreateCustomHediffWithDurationNoMultiplier(targetPawn, hDef, modExtension.hours, modExtension.ticks);
                                float total = modExtension.curve.Evaluate(skillSet[i].Level);
                                hediff.Severity = Mathf.FloorToInt(total);
                                targetPawn.health.AddHediff(hediff);
                                Messages.Message("Makai_FailArollcheckGiveHediffGeneric".Translate(pawn.LabelShort, hediff.LabelCap, targetPawn.LabelShort, pawn.Named("USER"), targetPawn.Named("USER2")), pawn, MessageTypeDefOf.NegativeEvent);
                            }
                        }
                        Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);                        
                    }
                }
            }
        }
    }
}
