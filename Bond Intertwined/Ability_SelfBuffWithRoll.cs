using RimWorld.Planet;
using VanillaPsycastsExpanded;
using VEF.Abilities;
using Verse;
using RimWorld;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace MakaiTechPsycast.BondIntertwined
{
    public class Ability_SelfBuffWithRoll : VEF.Abilities.Ability
    {
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            AbilityExtension_Roll1D20 modExtension = def.GetModExtension<AbilityExtension_Roll1D20>();
            RollInfo rollinfo = new RollInfo();
            rollinfo = MakaiUtility.Roll1D20(pawn, modExtension.skillBonus, rollinfo);
            if (rollinfo.roll >= modExtension.successThreshold && rollinfo.roll < modExtension.greatSuccessThreshold)
            {
				float num = modExtension.hours * 2500f + (float)modExtension.ticks;
				float statValue = pawn.GetStatValue(modExtension.multiplier ?? StatDefOf.PsychicSensitivity);
				num *= statValue;
				Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenSuccess, pawn);
				hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.FloorToInt(num);
				pawn.health.AddHediff(hediff);
				Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
				Messages.Message("Makai_PassArollcheckTrading".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
			}
		
			if (rollinfo.roll >= modExtension.greatSuccessThreshold)
            {
				float num = modExtension.hours * 2500f + (float)modExtension.ticks;
				float statValue = pawn.GetStatValue(modExtension.multiplier ?? StatDefOf.PsychicSensitivity);
				num *= statValue;
				Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenGreatSuccess, pawn);
				hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.FloorToInt(num);
				pawn.health.AddHediff(hediff);
				Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
				Messages.Message("Makai_GreatPassArollcheckTrading".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
			}
			if (rollinfo.roll < modExtension.successThreshold)
			{
				float num = modExtension.hours * 2500f + (float)modExtension.ticks;
				float statValue = pawn.GetStatValue(modExtension.multiplier ?? StatDefOf.PsychicSensitivity);
				num *= statValue;
				Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenFail, pawn);
				hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.FloorToInt(num);
				pawn.health.AddHediff(hediff);
				Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
				Messages.Message("Makai_FailArollcheckTrading".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
			}
		}
    }
}
