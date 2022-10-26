using RimWorld.Planet;
using UnityEngine;
using System.Collections.Generic;
using VanillaPsycastsExpanded;
using VFECore.Abilities;
using RimWorld;
using Verse;
using System;
using System.Linq;

namespace MakaiTechPsycast.DestinedDeath
{
    public class Ability_PullTowardCenter : VFECore.Abilities.Ability
    {
		public IntVec3 targetCell;
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			AbilityExtension_Roll1D20 modExtension = def.GetModExtension<AbilityExtension_Roll1D20>();
			if (modExtension.targetOnlyEnemies && target.Thing != null && !target.Thing.HostileTo(pawn))
			{
				if (showMessages)
				{
					Messages.Message("VFEA.TargetMustBeHostile".Translate(), target.Thing, MessageTypeDefOf.CautionInput, null);
				}
				return false;
			}
			return base.ValidateTarget(target, showMessages);
		}
		public override void ModifyTargets(ref GlobalTargetInfo[] targets)
		{
			targetCell = targets[0].Cell;
			base.ModifyTargets(ref targets);
		}
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
			if(roll >= modExtension.successThreshold && roll < modExtension.greatSuccessThreshold)
            {
				foreach (GlobalTargetInfo globalTargetInfo in targets)
                {
					if(globalTargetInfo.Thing is Pawn pawn2)
                    {
						float num = modExtension.hours * 2500f + (float)modExtension.ticks;
						if (pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul))
						{
							num += pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul).TryGetComp<HediffComp_SoulCollection>().SoulCount * 100f;
						}
						num *= pawn.GetStatValue(modExtension.multiplier ?? StatDefOf.PsychicSensitivity);
						Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenSuccess, pawn);
						hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.FloorToInt(num);
						pawn2.health.AddHediff(hediff);
						/*IntVec3 position = pawn.Position;
						int x1 = pawn.Position.x;
						int x2 = targetCell.x;
						int y1 = pawn.Position.z;
						int y2 = targetCell.z;
						for (int i = 0; i < pawn.Position.DistanceTo(targetCell); i++)
						{
							GenExplosion.DoExplosion(position, pawn.Map, 1, DamageDefOf.Bomb, null, 1);
							if (x2 > x1 && position.x != x2)
							{
								position.x++;
							}
							else if (x2 < x1 && position.x != x2)
							{
								position.x--;
							}
							if (y2 > y1 && position.z != y2)
							{
								position.z++;
							}
							else if (y2 < y1 && position.z != y2)
							{
								position.z--;
							}
						}*/
						IntVec3 intVec = targetCell;
						PawnFlyer_Pulled pawnFlyer_Pulled = (PawnFlyer_Pulled)PawnFlyer.MakeFlyer(MakaiTechPsy_DefOf.MakaiPsy_PullSlow, pawn2, intVec,null,null);
						GenSpawn.Spawn(pawnFlyer_Pulled, intVec, pawn.Map);
						Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_DD_Suck.Spawn(globalTargetInfo.Cell, globalTargetInfo.Map, 1);
						effect.Cleanup();
						Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
						Messages.Message("Makai_PassArollcheckDDPull".Translate(pawn.LabelShort, globalTargetInfo.Label, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
					}
                }
			}
			if (roll >= modExtension.greatSuccessThreshold)
			{
				foreach (GlobalTargetInfo globalTargetInfo in targets)
				{
					if (globalTargetInfo.Thing is Pawn pawn2)
					{
						float num = modExtension.hours * 2500f + (float)modExtension.ticks;
						if (pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul))
						{
							num += pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul).TryGetComp<HediffComp_SoulCollection>().SoulCount * 100f;
						}
						num *= pawn.GetStatValue(modExtension.multiplier ?? StatDefOf.PsychicSensitivity);
						Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenGreatSuccess, pawn);
						hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.FloorToInt(num);
						pawn2.health.AddHediff(hediff);
						/*IntVec3 position = pawn.Position;
						int x1 = pawn.Position.x;
						int x2 = targetCell.x;
						int y1 = pawn.Position.z;
						int y2 = targetCell.z;
						for (int i = 0; i < pawn.Position.DistanceTo(targetCell); i++)
						{
							GenExplosion.DoExplosion(position, pawn.Map, 1, DamageDefOf.Bomb, null, 1);
							if (x2 > x1 && position.x != x2)
							{
								position.x++;
							}
							else if (x2 < x1 && position.x != x2)
							{
								position.x--;
							}
							if (y2 > y1 && position.z != y2)
							{
								position.z++;
							}
							else if (y2 < y1 && position.z != y2)
							{
								position.z--;
							}
						}*/
						IntVec3 intVec = targetCell;
						PawnFlyer_Pulled pawnFlyer_Pulled = (PawnFlyer_Pulled)PawnFlyer.MakeFlyer(MakaiTechPsy_DefOf.MakaiPsy_PullSlow, pawn2, intVec, null, null);
						GenSpawn.Spawn(pawnFlyer_Pulled, intVec, pawn.Map);
						Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_DD_Suck.Spawn(globalTargetInfo.Cell, globalTargetInfo.Map, 1);
						effect.Cleanup();
						Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
						Messages.Message("Makai_GreatPassArollcheckDDPull".Translate(pawn.LabelShort, globalTargetInfo.Label, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
					}
				}
			}
			if (roll < modExtension.successThreshold)
			{
				foreach (GlobalTargetInfo globalTargetInfo in targets)
				{
					if (globalTargetInfo.Thing is Pawn pawn2)
					{
						float num = modExtension.hours * 2500f + (float)modExtension.ticks;
						if (pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul))
						{
							num += pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul).TryGetComp<HediffComp_SoulCollection>().SoulCount * 100f;
						}
						num *= pawn.GetStatValue(modExtension.multiplier ?? StatDefOf.PsychicSensitivity);
						Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenFail, pawn);
						hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = Mathf.FloorToInt(num);
						pawn2.health.AddHediff(hediff);
						/*IntVec3 position = pawn.Position;
						int x1 = pawn.Position.x;
						int x2 = targetCell.x;
						int y1 = pawn.Position.z;
						int y2 = targetCell.z;
						for (int i = 0; i < pawn.Position.DistanceTo(targetCell); i++)
						{
							GenExplosion.DoExplosion(position, pawn.Map, 1, DamageDefOf.Bomb, null, 1);
							if (x2 > x1 && position.x != x2)
							{
								position.x++;
							}
							else if (x2 < x1 && position.x != x2)
							{
								position.x--;
							}
							if (y2 > y1 && position.z != y2)
							{
								position.z++;
							}
							else if (y2 < y1 && position.z != y2)
							{
								position.z--;
							}
						}*/
						IntVec3 intVec = MakaiUtility.RandomCellAround(pawn2, 5);
						PawnFlyer_Pulled pawnFlyer_Pulled = (PawnFlyer_Pulled)PawnFlyer.MakeFlyer(MakaiTechPsy_DefOf.MakaiPsy_PullSlow, pawn2, intVec, null, null);
						GenSpawn.Spawn(pawnFlyer_Pulled, intVec, pawn.Map);
						Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_DD_Suck.Spawn(globalTargetInfo.Cell, globalTargetInfo.Map, 1);
						effect.Cleanup();
						Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
						Messages.Message("Makai_FailArollcheckDDPull".Translate(pawn.LabelShort, globalTargetInfo.Label, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
					}
				}
			}
		}

	}
}
