using RimWorld;
using RimWorld.Planet;
using Verse;
using VFECore;
using VanillaPsycastsExpanded;
using UnityEngine;

namespace MakaiTechPsycast.GoldenOrder
{
    public class Ability_ChangeTrait: VFECore.Abilities.Ability
    {
        AbilityExtension_Roll1D20 modExtension => def.GetModExtension<AbilityExtension_Roll1D20>();
        public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
        {
            if (!pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiTechPsy_GD_PathOfNaraka))
            {
                Messages.Message("Not Enough mental strength", MessageTypeDefOf.NeutralEvent, false);
                return false;
            }
            if (MakaiUtility.GetFirstHediffOfDef(pawn, MakaiTechPsy_DefOf.MakaiTechPsy_GD_PathOfNaraka).TryGetComp<HediffComp_PathOfNaraka>().currentStack < modExtension.costs)
            {
                Messages.Message("Not Enough mental strength", MessageTypeDefOf.NeutralEvent, false);
                return false;
            }
            return true;
        }
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);            
            RollInfo rollinfo = new RollInfo();
            rollinfo = MakaiUtility.Roll1D20(pawn, modExtension.skillBonus, rollinfo);
            if (rollinfo.roll >= modExtension.successThreshold && rollinfo.roll < modExtension.greatSuccessThreshold)
            {
                if (targets[0].Thing is Pawn targetPawn)
                {
                    MakaiUtility.GetFirstHediffOfDef(pawn, MakaiTechPsy_DefOf.MakaiTechPsy_GD_PathOfNaraka).TryGetComp<HediffComp_PathOfNaraka>().currentStack -= modExtension.costs;
                    Find.WindowStack.Add(new Dialog_ChooseTraitToRemove(targetPawn, false));
                    Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                    Messages.Message("Makai_PassArollcheckChangeTrait".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                }
            }
            if (rollinfo.roll >= modExtension.greatSuccessThreshold)
            {
                if (targets[0].Thing is Pawn targetPawn)
                {
                    MakaiUtility.GetFirstHediffOfDef(pawn, MakaiTechPsy_DefOf.MakaiTechPsy_GD_PathOfNaraka).TryGetComp<HediffComp_PathOfNaraka>().currentStack -= modExtension.costs;
                    Find.WindowStack.Add(new Dialog_ChooseTraitToRemove(targetPawn, true));
                    Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                    Messages.Message("Makai_GreatPassArollcheckChangeTrait".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                }
            }
            if (rollinfo.roll < modExtension.successThreshold)
            {
                if (targets[0].Thing is Pawn targetPawn)
                {
                    MakaiUtility.GetFirstHediffOfDef(pawn, MakaiTechPsy_DefOf.MakaiTechPsy_GD_PathOfNaraka).TryGetComp<HediffComp_PathOfNaraka>().currentStack -= modExtension.costs;
                    Find.WindowStack.Add(new Dialog_ChooseTraitToRemove(targetPawn, false));
                    Hediff hediff = MakaiUtility.CreateCustomHediffWithDuration(targetPawn,modExtension.hediffDefWhenFail,modExtension.hours,modExtension.ticks);
                    targetPawn.health.AddHediff(hediff);
                    Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
                    Messages.Message("Makai_FailArollcheckChangeTrait".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
                }
            }
        }
    }
}
