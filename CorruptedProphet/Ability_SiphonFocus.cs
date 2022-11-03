using RimWorld.Planet;
using HarmonyLib;
using System.Collections.Generic;
using VanillaPsycastsExpanded;
using VFECore.Abilities;
using RimWorld;
using Verse;

namespace MakaiTechPsycast.CorruptedProphet
{
    public class Ability_SiphonFocus : VFECore.Abilities.Ability
    {
		private static readonly AccessTools.FieldRef<Pawn_PsychicEntropyTracker, float> currentEntropy = AccessTools.FieldRefAccess<Pawn_PsychicEntropyTracker, float>("currentEntropy");
		public override void Cast(params GlobalTargetInfo[] targets)
        {
			base.Cast(targets);
			AbilityExtension_Roll1D20 modExtension = def.GetModExtension<AbilityExtension_Roll1D20>();
			if (pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel) && targets[0].Thing is Pawn pawn2)
            {
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
				float Taint = 0f;
				if (roll >= modExtension.successThreshold && roll < modExtension.greatSuccessThreshold && pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel).Severity >= modExtension.costs)
				{
					if (pawn2.psychicEntropy.CurrentPsyfocus > 0)
					{
						float targetFocus = pawn2.psychicEntropy.CurrentPsyfocus;
						pawn.psychicEntropy.OffsetPsyfocusDirectly(targetFocus);
						pawn2.psychicEntropy.OffsetPsyfocusDirectly(-targetFocus);
						currentEntropy(pawn2.psychicEntropy) += targetFocus;
						Taint += 2f;
						pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel).Severity -= Taint;
						Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
						Messages.Message("Makai_PassArollcheckSiphon".Translate(pawn.LabelShort, Taint, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
					}
				}
				if (roll >= modExtension.greatSuccessThreshold && pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel).Severity >= 1f)
                {
					if (pawn2.psychicEntropy.CurrentPsyfocus > 0)
					{
						float targetFocus = pawn2.psychicEntropy.CurrentPsyfocus;
						pawn.psychicEntropy.OffsetPsyfocusDirectly(targetFocus*2);
						pawn2.psychicEntropy.OffsetPsyfocusDirectly(-targetFocus);
						currentEntropy(pawn2.psychicEntropy) += targetFocus*2;
						Taint += 1f;
						pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel).Severity -= Taint;
						Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
						Messages.Message("Makai_GreatPassArollcheckSiphon".Translate(pawn.LabelShort, Taint, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
					}
				}
				if (roll < modExtension.successThreshold && pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel).Severity >= 3f)
                {
					if (pawn2.psychicEntropy.CurrentPsyfocus > 0)
					{
						float targetFocus = pawn2.psychicEntropy.CurrentPsyfocus;
						pawn.psychicEntropy.OffsetPsyfocusDirectly(targetFocus * 0.5f);
						pawn2.psychicEntropy.OffsetPsyfocusDirectly(-targetFocus * 0.5f);
						currentEntropy(pawn2.psychicEntropy) += targetFocus * 0.5f;
						Taint += 3f;
						pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel).Severity -= Taint;
						Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
						Messages.Message("Makai_FailArollcheckSiphon".Translate(pawn.LabelShort, Taint, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
					}
				}
				if (!pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel) || pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel).Severity < modExtension.costs)
				{
					Messages.Message("Makai_CP_NotEnoughTaint".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
				}
			}
        }
    }
}
