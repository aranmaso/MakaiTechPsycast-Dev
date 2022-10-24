using RimWorld.Planet;
using UnityEngine;
using System.Collections.Generic;
using VanillaPsycastsExpanded;
using VFECore.Abilities;
using RimWorld;
using Verse;

namespace MakaiTechPsycast.CorruptedProphet
{
    public class Ability_GainTaint : VFECore.Abilities.Ability
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
				float focus = pawn.psychicEntropy.CurrentPsyfocus;
				float severity = focus * 5;
				if (!pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel))
                {
					pawn.health.AddHediff(hediff);
				}
				pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel).Severity += (severity);
				pawn.psychicEntropy.OffsetPsyfocusDirectly(-1f);
				pawn.psychicEntropy.OffsetPsyfocusDirectly(0.1f);
				Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
				Messages.Message("Makai_PassArollcheckTaint".Translate(pawn.LabelShort, severity, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
			}

			if (roll >= modExtension.greatSuccessThreshold)
			{
				float num = modExtension.hours * 2500f + (float)modExtension.ticks;
				float statValue = pawn.GetStatValue(modExtension.multiplier ?? StatDefOf.PsychicSensitivity);
				num *= statValue;
				Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenGreatSuccess, pawn);
				hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.FloorToInt(num);
				float focus = pawn.psychicEntropy.CurrentPsyfocus;
				float severity = focus * 10;
				if (!pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel))
				{
					pawn.health.AddHediff(hediff);
				}
				pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel).Severity += (severity);
				pawn.psychicEntropy.OffsetPsyfocusDirectly(-1f);
				pawn.psychicEntropy.OffsetPsyfocusDirectly(0.1f);
				Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
				Messages.Message("Makai_GreatPassArollcheckTaint".Translate(pawn.LabelShort, severity, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
			}
			if (roll < modExtension.successThreshold)
			{
				float num = modExtension.hours * 2500f + (float)modExtension.ticks;
				float statValue = pawn.GetStatValue(modExtension.multiplier ?? StatDefOf.PsychicSensitivity);
				num *= statValue;
				Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenFail, pawn);
				hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.FloorToInt(num);
				float focus = pawn.psychicEntropy.CurrentPsyfocus;
				float severity = focus * 2;
				if (!pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel))
				{
					pawn.health.AddHediff(hediff);
				}
				pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel).Severity += (severity);
				pawn.psychicEntropy.OffsetPsyfocusDirectly(-1f);
				pawn.psychicEntropy.OffsetPsyfocusDirectly(0.1f);
				Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
				Messages.Message("Makai_FailArollcheckTaint".Translate(pawn.LabelShort, severity, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
			}
		}
	}
}

