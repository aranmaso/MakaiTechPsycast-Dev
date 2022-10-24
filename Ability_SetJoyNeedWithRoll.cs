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
	public class Ability_SetJoyNeedWithRoll : VFECore.Abilities.Ability
    {
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			AbilityExtension_Roll1D20 modExtension = def.GetModExtension<AbilityExtension_Roll1D20>();
			AbilityExtension_needDef modExtension2 = def.GetModExtension<AbilityExtension_needDef>();
			if (targets[0].Thing is Pawn pawn2)
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
				if (roll >= modExtension.successThreshold && roll < modExtension.greatSuccessThreshold)
				{
					pawn2.needs.joy.CurLevelPercentage += 50f;
					Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
					Messages.Message("Makai_PassArollcheckJoy".Translate(pawn2.LabelShort, pawn2.Named("USER2")), pawn2, MessageTypeDefOf.PositiveEvent);
				}
				if (roll >= modExtension.greatSuccessThreshold)
				{
					pawn2.needs.joy.CurLevelPercentage += 100f;
					float num = modExtension.hours * 2500f + (float)modExtension.ticks;
					Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenGreatSuccess ?? VPE_DefOf.PsychicComa, pawn2);
					hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.FloorToInt(num);
					pawn2.health.AddHediff(hediff);
					Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, base.pawn.Named("USER")), base.pawn, MessageTypeDefOf.PositiveEvent);
					Messages.Message("Makai_GreatPassArollcheckJoy".Translate(pawn2.LabelShort, pawn.Named("USER2")), pawn, MessageTypeDefOf.PositiveEvent);
				}
				if (roll < modExtension.successThreshold)
				{
					pawn2.needs.joy.CurLevelPercentage -= 100f;
					pawn2.needs.food.CurLevelPercentage -= 100f;
					foreach (GlobalTargetInfo globalTargetInfo in targets)
					{
						GenExplosion.DoExplosion(modExtension2.onCaster ? pawn.Position : globalTargetInfo.Cell, pawn.Map, modExtension2.explosionRadius, modExtension2.explosionDamageDef, pawn, modExtension2.explosionDamageAmount, modExtension2.explosionArmorPenetration, modExtension2.explosionSound, null, null, null, modExtension2.postExplosionSpawnThingDef, modExtension2.postExplosionSpawnChance, modExtension2.postExplosionSpawnThingCount,GasType.BlindSmoke, modExtension2.applyDamageToExplosionCellsNeighbors, modExtension2.preExplosionSpawnThingDef, modExtension2.preExplosionSpawnChance, modExtension2.preExplosionSpawnThingCount, modExtension2.chanceToStartFire, modExtension2.damageFalloff, modExtension2.explosionDirection, modExtension2.casterImmune ? new List<Thing> { pawn } : null);
					}
					Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, base.pawn.Named("USER")), base.pawn, MessageTypeDefOf.NegativeEvent);
					Messages.Message("Makai_FailArollcheckJoy".Translate(pawn2.LabelShort, pawn.Named("USER2")), pawn, MessageTypeDefOf.NegativeEvent);
				}
			}
		}
	}
}
