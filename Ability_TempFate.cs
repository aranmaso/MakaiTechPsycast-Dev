using RimWorld.Planet;
using UnityEngine;
using System.Collections.Generic;
using VanillaPsycastsExpanded;
using VFECore.Abilities;
using RimWorld;
using Verse;
using System;
using System.Linq;

namespace MakaiTechPsycast.StringOfFate
{
	public class Ability_TempFate : VFECore.Abilities.Ability
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
			if (targets[0].Thing is Pawn pawn2)
			{
				if (roll >= modExtension.successThreshold && roll < modExtension.greatSuccessThreshold)
				{
					if (pawn2.HitPoints < pawn2.MaxHitPoints * 0.1)
					{
						List<Hediff> list = pawn2.health.hediffSet.hediffs.Where(MakaiUtility.FindBadHediff).ToList();
						foreach (Hediff item in list)
						{
							pawn2.health.RemoveHediff(item);
						}
						Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
					}
					Messages.Message("Target hp not low enough", MessageTypeDefOf.NegativeEvent);
				}
				if (roll >= modExtension.greatSuccessThreshold)
				{
					if (pawn2.HitPoints < pawn2.MaxHitPoints * 0.1)
					{
						List<Hediff> list = pawn2.health.hediffSet.hediffs.Where(MakaiUtility.FindBadHediff).ToList();
						foreach (Hediff item in list)
						{
							pawn2.health.RemoveHediff(item);
						}
						Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
					}
					Messages.Message("Target hp not low enough", MessageTypeDefOf.NegativeEvent);
				}
				if (roll < modExtension.successThreshold)
				{
					List<Hediff> list = pawn2.health.hediffSet.hediffs.Where(MakaiUtility.FindBadHediff).ToList();
					for (int i = 0;i < list.Count /2;i++)
                    {
						pawn2.health.RemoveHediff(list.RandomElement());
					}
					Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
				}
			}
		}
	}		
}
