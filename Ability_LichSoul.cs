using RimWorld.Planet;
using UnityEngine;
using HarmonyLib;
using System.Collections.Generic;
using VanillaPsycastsExpanded;
using VFECore.Abilities;
using RimWorld;
using Verse;
using System.Linq;

namespace MakaiTechPsycast.DestinedDeath
{
	public class Ability_LichSoul : VFECore.Abilities.Ability
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
			if(!pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiTechPsy_DD_LichSoul))
            {
				if (roll >= modExtension.successThreshold && roll < modExtension.greatSuccessThreshold)
				{
					Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenSuccess, pawn);
					pawn.health.AddHediff(hediff);
					Thing thing = ThingMaker.MakeThing(MakaiTechPsy_DefOf.MakaiTechPsy_DD_Soul);
					if (thing is Soul soul2)
					{
						soul2 = MakaiUtility.GetPawnCopy(soul2,pawn);
					}
					GenPlace.TryPlaceThing(thing, pawn.RandomAdjacentCell8Way(), pawn.Map, ThingPlaceMode.Near);
					Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
					Messages.Message("Makai_PassArollcheckLichSoul".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
				}
				if (roll >= modExtension.greatSuccessThreshold)
				{

					Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenGreatSuccess, pawn);
					pawn.health.AddHediff(hediff);
					Thing thing = ThingMaker.MakeThing(MakaiTechPsy_DefOf.MakaiTechPsy_DD_Soul);
					if (thing is Soul soul2)
					{
						soul2 = MakaiUtility.GetPawnCopy(soul2, pawn);
					}
					GenPlace.TryPlaceThing(thing, pawn.RandomAdjacentCell8Way(), pawn.Map, ThingPlaceMode.Near);
					List<Hediff> list1 = pawn.health.hediffSet.hediffs.Where(MakaiUtility.FindBadHediff).ToList();
					foreach (Hediff item in list1)
					{
						pawn.health.RemoveHediff(item);
					}
					Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
					Messages.Message("Makai_GreatPassArollcheckLichSoul".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
				}
				if (roll < modExtension.successThreshold)
				{

					Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenFail, pawn);
					Hediff hediff2 = HediffMaker.MakeHediff(MakaiTechPsy_DefOf.MakaiPsy_PK_Brain_Mulfunction, pawn);
					pawn.health.AddHediff(hediff);
					pawn.health.AddHediff(hediff2);
					Thing thing = ThingMaker.MakeThing(MakaiTechPsy_DefOf.MakaiTechPsy_DD_Soul);
					if (thing is Soul soul2)
					{
						soul2 = MakaiUtility.GetPawnCopy(soul2, pawn);
					}
					GenPlace.TryPlaceThing(thing, pawn.RandomAdjacentCell8Way(), pawn.Map, ThingPlaceMode.Near);
					Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
					Messages.Message("Makai_FailArollcheckLichSoul".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
				}
			}
			else if(pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiTechPsy_DD_LichSoul))
            {
				Messages.Message("LichSoulAlreadyRemove".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
            }
		}
	}
}
