using RimWorld.Planet;
using UnityEngine;
using System.Collections.Generic;
using VanillaPsycastsExpanded;
using VEF.Abilities;
using RimWorld;
using Verse;

namespace MakaiTechPsycast.CorruptedProphet
{
    public class Ability_GainTaint : VEF.Abilities.Ability
    {
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			AbilityExtension_Roll1D20 modExtension = def.GetModExtension<AbilityExtension_Roll1D20>();
			RollInfo rollInfo = new RollInfo();
			rollInfo = pawn.R1D20(modExtension.skillBonus,modExtension.skillBonus2 ?? null);
			int baseRoll = rollInfo.baseRoll;
			int roll = rollInfo.roll;
			int cumulativeBonusRoll = rollInfo.cumulativeBonusRoll;
            if (roll >= modExtension.successThreshold && roll < modExtension.greatSuccessThreshold)
			{
				Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenSuccess, pawn);
				float focus = pawn.psychicEntropy.CurrentPsyfocus;
				float severity = focus * 5;
				if (!pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel))
                {
					pawn.health.AddHediff(hediff);
				}
				pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel).Severity += (severity);
				pawn.psychicEntropy.OffsetPsyfocusDirectly(-1f);
				pawn.psychicEntropy.OffsetPsyfocusDirectly(0.01f);
				Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
				Messages.Message("Makai_PassArollcheckTaint".Translate(pawn.LabelShort, severity, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
			}

			if (roll >= modExtension.greatSuccessThreshold)
			{
				Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenGreatSuccess, pawn);
				float focus = pawn.psychicEntropy.CurrentPsyfocus;
				float severity = focus * 10;
				if (!pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel))
				{
					pawn.health.AddHediff(hediff);
				}
				pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel).Severity += (severity);
				pawn.psychicEntropy.OffsetPsyfocusDirectly(-1f);
				pawn.psychicEntropy.OffsetPsyfocusDirectly(0.01f);
				Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
				Messages.Message("Makai_GreatPassArollcheckTaint".Translate(pawn.LabelShort, severity, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
			}
			if (roll < modExtension.successThreshold)
			{
				Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenFail, pawn);
				float focus = pawn.psychicEntropy.CurrentPsyfocus;
				float severity = focus * 2;
				if (!pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel))
				{
					pawn.health.AddHediff(hediff);
				}
				pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel).Severity += (severity);
				pawn.psychicEntropy.OffsetPsyfocusDirectly(-1f);
				pawn.psychicEntropy.OffsetPsyfocusDirectly(0.01f);
				Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
				Messages.Message("Makai_FailArollcheckTaint".Translate(pawn.LabelShort, severity, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
			}
		}
	}
}

