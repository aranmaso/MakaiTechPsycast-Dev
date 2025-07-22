using RimWorld.Planet;
using UnityEngine;
using System.Collections.Generic;
using VanillaPsycastsExpanded;
using VEF.Abilities;
using RimWorld;
using Verse;

namespace MakaiTechPsycast.CorruptedProphet
{
    public class Ability_MindBreak : VEF.Abilities.Ability
    {
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
			AbilityExtension_Roll1D20 modExtension = def.GetModExtension<AbilityExtension_Roll1D20>();
            RollInfo rollInfo = new RollInfo();
            rollInfo = pawn.R1D20(modExtension.skillBonus, modExtension.skillBonus2 ?? null);
            int baseRoll = rollInfo.baseRoll;
            int roll = rollInfo.roll;
            int cumulativeBonusRoll = rollInfo.cumulativeBonusRoll;
            if (pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel) && pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel).Severity >= modExtension.costs)
            {
				if(targets[0].Thing is Pawn pawn2)
                {
					if(pawn2.Faction != Faction.OfPlayer)
                    {
						if (roll >= modExtension.successThreshold && roll < modExtension.greatSuccessThreshold)
						{
							float num = modExtension.hours * 2500f + (float)modExtension.ticks;
							float statValue = pawn.GetStatValue(modExtension.multiplier ?? StatDefOf.PsychicSensitivity);
							num *= statValue;
							Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenSuccess, pawn);
							hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.FloorToInt(num);
							pawn2.health.AddHediff(hediff, pawn2.health.hediffSet.GetBrain());
							Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
							Messages.Message("Makai_PassArollcheckMindBreak".Translate(pawn.LabelShort, pawn2.LabelShort, num / 2500f, pawn.Named("USER"), pawn2.Named("USER2")), pawn, MessageTypeDefOf.PositiveEvent);
						}
						if (roll >= modExtension.greatSuccessThreshold)
						{
							float num = modExtension.hours * 2500f + (float)modExtension.ticks;
							float statValue = pawn.GetStatValue(modExtension.multiplier ?? StatDefOf.PsychicSensitivity);
							num *= statValue;
							Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenGreatSuccess, pawn);
							hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.FloorToInt(num *= 2);
							pawn2.health.AddHediff(hediff, pawn2.health.hediffSet.GetBrain());
							Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
							Messages.Message("Makai_GreatPassArollcheckMindBreak".Translate(pawn.LabelShort, pawn2.LabelShort, num / 2500f, pawn.Named("USER"), pawn2.Named("USER2")), pawn, MessageTypeDefOf.PositiveEvent);
						}
						if (roll < modExtension.successThreshold)
						{
							float num = modExtension.hours * 2500f + (float)modExtension.ticks;
							float statValue = pawn.GetStatValue(modExtension.multiplier ?? StatDefOf.PsychicSensitivity);
							num *= statValue;
							Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenFail, pawn);
							hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.FloorToInt(num /= 2);
							pawn2.health.AddHediff(hediff, pawn2.health.hediffSet.GetBrain());
							Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
							Messages.Message("Makai_FailArollcheckMindBreak".Translate(pawn.LabelShort, pawn2.LabelShort, num / 2500f, pawn.Named("USER"), pawn2.Named("USER2")), pawn, MessageTypeDefOf.NegativeEvent);
						}
					}
					else if (pawn2.Faction == pawn.Faction)
					{
						Messages.Message("Makai_FailMindBreak".Translate(pawn.LabelShort, pawn2.LabelShort, pawn.Named("USER"), pawn2.Named("USER2")), pawn, MessageTypeDefOf.NegativeEvent);
					}
				}
			}
			else if (!pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel) || pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel).Severity < modExtension.costs)
			{
				Messages.Message("Makai_CP_NotEnoughTaint".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
			}
		}
    }
}
