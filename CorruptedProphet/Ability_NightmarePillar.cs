using RimWorld.Planet;
using UnityEngine;
using System.Collections.Generic;
using VanillaPsycastsExpanded;
using VFECore.Abilities;
using RimWorld;
using Verse;

namespace MakaiTechPsycast.CorruptedProphet
{
	public class Ability_NightmarePillar : VFECore.Abilities.Ability
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
			if(pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel).Severity >= modExtension.costs)
            {
				if (roll >= modExtension.successThreshold && roll < modExtension.greatSuccessThreshold)
				{
					foreach (GlobalTargetInfo globalTargetInfo in targets)
					{
						MakaiPsy_CP_PillarStrike orbitalStrike = (MakaiPsy_CP_PillarStrike)GenSpawn.Spawn(MakaiTechPsy_DefOf.MakaiPsy_CP_Pillar, globalTargetInfo.Cell, pawn.Map);
						orbitalStrike.duration = GetDurationForPawn();
						orbitalStrike.instigator = pawn;
						orbitalStrike.StartStrike();
						pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel).Severity -= modExtension.costs;
						Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
						Messages.Message("Makai_PassArollcheckPillar".Translate(pawn.LabelShort, modExtension.costs, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
					}
				}
				if (roll >= modExtension.greatSuccessThreshold)
				{
					foreach (GlobalTargetInfo globalTargetInfo in targets)
					{
						MakaiPsy_CP_PillarStrike orbitalStrike = (MakaiPsy_CP_PillarStrike)GenSpawn.Spawn(MakaiTechPsy_DefOf.MakaiPsy_CP_Pillar, globalTargetInfo.Cell, pawn.Map);
						orbitalStrike.duration = GetDurationForPawn() * 2;
						orbitalStrike.instigator = pawn;
						orbitalStrike.StartStrike();
						pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel).Severity -= modExtension.costs;
						Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
						Messages.Message("Makai_GreatPassArollcheckPillar".Translate(pawn.LabelShort, modExtension.costs, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
					}
				}
				if (roll < modExtension.successThreshold)
				{
					foreach (GlobalTargetInfo globalTargetInfo in targets)
					{
						MakaiPsy_CP_PillarStrike orbitalStrike = (MakaiPsy_CP_PillarStrike)GenSpawn.Spawn(MakaiTechPsy_DefOf.MakaiPsy_CP_Pillar, globalTargetInfo.Cell, pawn.Map);
						orbitalStrike.duration = GetDurationForPawn();
						orbitalStrike.instigator = pawn;
						orbitalStrike.StartStrike();
						pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiPsy_CP_TaintLevel).Severity -= modExtension.costs * 2;
						Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
						Messages.Message("Makai_FailArollcheckPillar".Translate(pawn.LabelShort, modExtension.costs * 2, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
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
