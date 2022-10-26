using RimWorld.Planet;
using VanillaPsycastsExpanded;
using VFECore.Abilities;
using Verse;
using RimWorld;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace MakaiTechPsycast.BondIntertwined
{
    public class Ability_SelfBuffWithRoll : VFECore.Abilities.Ability
    {
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            AbilityExtension_Roll1D20 modExtension = def.GetModExtension<AbilityExtension_Roll1D20>();
			SkillRecord bonus = pawn.skills.GetSkill(modExtension.skillBonus);
			System.Random rand = new System.Random();
			int roll = rand.Next(1, 21);
			int rollBonus = bonus.Level / 5;
			int baseRoll = roll;
			int rollBonusLucky = 0;
			int rollBonusUnLucky = 0;
			if (pawn.health.hediffSet.HasHediff(VPE_DefOf.VPE_Lucky))
			{
				rollBonusLucky = 20;
			}
			if (pawn.health.hediffSet.HasHediff(VPE_DefOf.VPE_UnLucky))
			{
				rollBonusUnLucky = -20;
			}
			roll += rollBonus + rollBonusLucky + rollBonusUnLucky;
			int cumulativeBonusRoll = rollBonus + rollBonusLucky + rollBonusUnLucky;
			if (roll >= modExtension.successThreshold && roll < modExtension.greatSuccessThreshold)
            {
				float num = modExtension.hours * 2500f + (float)modExtension.ticks;
				float statValue = pawn.GetStatValue(modExtension.multiplier ?? StatDefOf.PsychicSensitivity);
				num *= statValue;
				Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenSuccess, pawn);
				hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.FloorToInt(num);
				pawn.health.AddHediff(hediff);
				Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
				Messages.Message("Makai_PassArollcheckTrading".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
			}
		
			if (roll >= modExtension.greatSuccessThreshold)
            {
				float num = modExtension.hours * 2500f + (float)modExtension.ticks;
				float statValue = pawn.GetStatValue(modExtension.multiplier ?? StatDefOf.PsychicSensitivity);
				num *= statValue;
				Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenGreatSuccess, pawn);
				hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.FloorToInt(num);
				pawn.health.AddHediff(hediff);
				Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
				Messages.Message("Makai_GreatPassArollcheckTrading".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
			}
			if (roll < modExtension.successThreshold)
			{
				float num = modExtension.hours * 2500f + (float)modExtension.ticks;
				float statValue = pawn.GetStatValue(modExtension.multiplier ?? StatDefOf.PsychicSensitivity);
				num *= statValue;
				Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenFail, pawn);
				hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.FloorToInt(num);
				pawn.health.AddHediff(hediff);
				Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
				Messages.Message("Makai_FailArollcheckTrading".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
			}
		}
    }
}
