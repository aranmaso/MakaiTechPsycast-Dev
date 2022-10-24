using RimWorld.Planet;
using UnityEngine;
using System.Collections.Generic;
using VanillaPsycastsExpanded;
using VFECore.Abilities;
using RimWorld;
using Verse;

namespace MakaiTechPsycast.BondIntertwined
{
    public class Ability_BondAffectFactionRelation : VFECore.Abilities.Ability
    {
        public override bool CanAutoCast => false;

        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            AbilityExtension_Roll1D20 modExtension = def.GetModExtension<AbilityExtension_Roll1D20>();
			if (modExtension == null)
			{
				return;
			}
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
			SkillRecord skillRequirement = pawn.skills.GetSkill(SkillDefOf.Social);
			if (roll >= modExtension.successThreshold && roll < modExtension.greatSuccessThreshold)
			{
				float dur = modExtension.hours * 2500f + modExtension.ticks;
				float dur2 = modExtension.hours * 2500f + modExtension.ticks;
				if (modExtension.multiplier != null)
				{
					dur *= pawn.GetStatValue(modExtension.multiplier);
				}
				if (skillRequirement.Level >= modExtension.skillRequirementSuccessThreshold && skillRequirement.Level < modExtension.skillRequirementGreatSuccessThreshold)
                {
					foreach (Pawn pawn2 in pawn.Map.mapPawns.AllPawnsSpawned)
					{
						if (modExtension.multiplier != null)
						{
							dur2 *= pawn2.GetStatValue(modExtension.multiplier);
						}
						if (pawn2.Faction != null && pawn2.Faction != Faction.OfPlayer && pawn2.Faction.HasGoodwill)
						{
							Faction.OfPlayer.TryAffectGoodwillWith(pawn2.Faction, 5);
							Hediff hediff2 = HediffMaker.MakeHediff(modExtension.hediffDefWhenSuccess, pawn2);
							hediff2.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.FloorToInt(dur);
							pawn2.health.AddHediff(hediff2);
							Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
							Messages.Message("Makai_PassArollcheckMassRelation".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
						}
					}
					Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenSuccess, pawn, pawn.health.hediffSet.GetBrain());
					hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.FloorToInt(dur);
					pawn.health.AddHediff(hediff);
				}
				else
				{
					Messages.Message("Makai_SkillFailArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, skillRequirement.levelInt, modExtension.skillRequirementSuccessThreshold, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
				}
			}
			if (roll >=  modExtension.greatSuccessThreshold)
			{
				float dur = modExtension.hours * 2500f + modExtension.ticks;
				float dur2 = modExtension.hours * 2500f + modExtension.ticks;
				if (modExtension.multiplier != null)
				{
					dur *= pawn.GetStatValue(modExtension.multiplier);
				}
				if ((skillRequirement.Level >= modExtension.skillRequirementGreatSuccessThreshold) && (skillRequirement.Level > modExtension.skillRequirementSuccessThreshold))
				{
					foreach (Pawn pawn2 in pawn.Map.mapPawns.AllPawnsSpawned)
					{
						if (modExtension.multiplier != null)
						{
							dur2 *= pawn2.GetStatValue(modExtension.multiplier);
						}
						if (pawn2.Faction != null && pawn2.Faction != Faction.OfPlayer && pawn2.Faction.HasGoodwill)
						{
							int relation = Rand.Range(5, 10);
							int durRandom = Rand.RangeInclusive(100, 500);
							Faction.OfPlayer.TryAffectGoodwillWith(pawn2.Faction, relation);
							Hediff hediff2 = HediffMaker.MakeHediff(modExtension.hediffDefWhenGreatSuccess, pawn2);
							hediff2.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.FloorToInt(dur) + durRandom;
							pawn2.health.AddHediff(hediff2);
							Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenGreatSuccess, pawn, pawn.health.hediffSet.GetBrain());
							hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.FloorToInt(dur);
							pawn.health.AddHediff(hediff);
							Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
							Messages.Message("Makai_GreatPassArollcheckMassRelation".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
						}
					}
				}
				else
				{
					if(skillRequirement.Level >= modExtension.skillRequirementSuccessThreshold)
                    {
						foreach (Pawn pawn2 in pawn.Map.mapPawns.AllPawnsSpawned)
						{
							if (modExtension.multiplier != null)
							{
								dur2 *= pawn2.GetStatValue(modExtension.multiplier);
							}
							if (pawn2.Faction != null && pawn2.Faction != Faction.OfPlayer && pawn2.Faction.HasGoodwill)
							{
								Faction.OfPlayer.TryAffectGoodwillWith(pawn2.Faction, 5);
								Hediff hediff2 = HediffMaker.MakeHediff(modExtension.hediffDefWhenSuccess, pawn2);
								hediff2.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.FloorToInt(dur);
								pawn2.health.AddHediff(hediff2);
								Messages.Message("Makai_SkillFailDownGradeArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, skillRequirement.levelInt, modExtension.skillRequirementGreatSuccessThreshold, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
							}
						}
					}
					else
                    {
						Messages.Message("Makai_SkillFailMinimum".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, skillRequirement.levelInt, modExtension.skillRequirementSuccessThreshold, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
					}
				}
			}
			if (roll < modExtension.successThreshold && skillRequirement.Level >= modExtension.skillRequirementFailThreshold)
            {
				float dur = modExtension.hours * 2500f + modExtension.ticks;
				float dur2 = modExtension.hours * 2500f + modExtension.ticks;
				if (modExtension.multiplier != null)
				{
					dur *= pawn.GetStatValue(modExtension.multiplier);
				}
				foreach (Pawn pawn2 in pawn.Map.mapPawns.AllPawnsSpawned)
				{
					if (modExtension.multiplier != null)
					{
						dur2 *= pawn2.GetStatValue(modExtension.multiplier);
					}
					if (pawn2.Faction != null && pawn2.Faction != Faction.OfPlayer && pawn2.Faction.HasGoodwill)
					{
						int relation = Rand.Range(-5, -10);
						int durRandom = Rand.RangeInclusive(100,500);
						Faction.OfPlayer.TryAffectGoodwillWith(pawn2.Faction, relation);
						Hediff hediff2 = HediffMaker.MakeHediff(modExtension.hediffDefWhenFail, pawn2);
						hediff2.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.FloorToInt(dur) + durRandom;
						pawn2.health.AddHediff(hediff2);
						Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
						Messages.Message("Makai_FailArollcheckMassRelation".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
					}
				}
				Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenFail, pawn, pawn.health.hediffSet.GetBrain());
				hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.FloorToInt(dur);
				pawn.health.AddHediff(hediff);
			}
			if (roll < modExtension.successThreshold && skillRequirement.Level < modExtension.skillRequirementFailThreshold)
            {
				float dur = modExtension.hours * 2500f + modExtension.ticks;
				float dur2 = modExtension.hours * 2500f + modExtension.ticks;
				if (modExtension.multiplier != null)
				{
					dur *= pawn.GetStatValue(modExtension.multiplier);
				}
				foreach (Pawn pawn2 in pawn.Map.mapPawns.AllPawnsSpawned)
				{
					if (modExtension.multiplier != null)
					{
						dur2 *= pawn2.GetStatValue(modExtension.multiplier);
					}
					if (pawn2.Faction != null && pawn2.Faction != Faction.OfPlayer && pawn2.Faction.HasGoodwill)
					{
						int relation = Rand.Range(-10, -15);
						int durRandom = Rand.RangeInclusive(500, 1000);
						Faction.OfPlayer.TryAffectGoodwillWith(pawn2.Faction, relation);
						Hediff hediff2 = HediffMaker.MakeHediff(modExtension.hediffDefWhenFail, pawn2);
						hediff2.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.FloorToInt(dur) + durRandom;
						pawn2.health.AddHediff(hediff2);
						Messages.Message("Makai_GreatFailArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, skillRequirement.levelInt, modExtension.skillRequirementFailThreshold, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
						Messages.Message("Makai_GreatFailArollcheckMassRelation".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
					}
				}
				Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenFail, pawn, pawn.health.hediffSet.GetBrain());
				hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.FloorToInt(dur);
				pawn.health.AddHediff(hediff);
			}
		}
	}
}
