using RimWorld;
using RimWorld.Planet;
using Verse;
using System.Collections.Generic;
using VFECore;
using VanillaPsycastsExpanded;
using UnityEngine;

namespace MakaiTechPsycast.DistortedReality
{
    public class Ability_RandomHediffToANewOne : VFECore.Abilities.Ability
    {
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            AbilityExtension_Roll1D20 modExtension = def.GetModExtension<AbilityExtension_Roll1D20>();
            int threshold1 = Rand.Range(1, 21);
            int threshold2 = Rand.Range(1, 21);
            RollInfo rollinfo = new RollInfo();
            rollinfo = MakaiUtility.Roll1D20(pawn, DefDatabase<SkillDef>.GetRandom(), rollinfo);
            if (rollinfo.roll >= Mathf.Min(threshold1, threshold2) && rollinfo.roll < Mathf.Max(threshold1, threshold2))
            {
                if (targets[0].Thing is Pawn targetPawn)
                {
                    Find.WindowStack.Add(new Dialog_ChooseHediffToRandom(targetPawn));
                    Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                    Messages.Message("Makai_PassArollcheckRandomHediff".Translate(pawn.LabelShort,targetPawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                }
            }
            if (rollinfo.roll >= Mathf.Max(threshold1, threshold2))
            {
                if (targets[0].Thing is Pawn targetPawn)
                {
                    Find.WindowStack.Add(new Dialog_ChooseHediffToRandom(targetPawn));
                    Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                    Messages.Message("Makai_GreatPassArollcheckRandomHediff".Translate(pawn.LabelShort, targetPawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                } 
            }
            if (rollinfo.roll < Mathf.Min(threshold1, threshold2))
            {
                if (targets[0].Thing is Pawn targetPawn)
                {
                    Find.WindowStack.Add(new Dialog_ChooseHediffToRandom(targetPawn));
                    Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
                    Messages.Message("Makai_FailArollcheckRandomHediff".Translate(pawn.LabelShort, targetPawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
                }   
            }
        }
    }
}
