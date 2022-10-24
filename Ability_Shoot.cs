using RimWorld.Planet;
using UnityEngine;
using System.Collections.Generic;
using VanillaPsycastsExpanded;
using VFECore.Abilities;
using RimWorld;
using Verse;

namespace MakaiTechPsycast
{
	public class Ability_Shoot : VFECore.Abilities.Ability
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
				foreach (GlobalTargetInfo globalTargetInfo in targets)
				{
					Projectile projectile = (Projectile)GenSpawn.Spawn(modExtension.projectileWhenSuccess, pawn.Position, pawn.Map);
					if (globalTargetInfo.HasThing)
					{
						projectile.Launch(pawn, pawn.DrawPos, globalTargetInfo.Thing, globalTargetInfo.Thing, ProjectileHitFlags.IntendedTarget);
					}
					else
					{
						projectile.Launch(pawn, pawn.DrawPos, globalTargetInfo.Cell, globalTargetInfo.Cell, ProjectileHitFlags.IntendedTarget);
					}
					Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
					Messages.Message("Makai_PassArollcheckShoot".Translate(pawn.LabelShort, globalTargetInfo.Label, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
				}
			}
			if (roll >= modExtension.greatSuccessThreshold)
			{
				foreach (GlobalTargetInfo globalTargetInfo in targets)
				{
					Projectile projectile = (Projectile)GenSpawn.Spawn(modExtension.projectileWhenGreatSuccess, pawn.Position, pawn.Map);
					if (globalTargetInfo.HasThing)
					{
						projectile.Launch(pawn, pawn.DrawPos, globalTargetInfo.Thing, globalTargetInfo.Thing, ProjectileHitFlags.IntendedTarget);
						for (int i = 0;i < 3;i++)
                        {
							projectile.Launch(pawn, pawn.DrawPos, globalTargetInfo.Cell.RandomAdjacentCell8Way(), globalTargetInfo.Thing, ProjectileHitFlags.IntendedTarget);
						}
					}
					else
					{
						projectile.Launch(pawn, pawn.DrawPos, globalTargetInfo.Cell, globalTargetInfo.Cell, ProjectileHitFlags.IntendedTarget);
						for (int i = 0; i < 3; i++)
						{
							projectile.Launch(pawn, pawn.DrawPos, globalTargetInfo.Cell.RandomAdjacentCell8Way(), globalTargetInfo.Thing, ProjectileHitFlags.IntendedTarget);
						}
					}
					Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
					Messages.Message("Makai_GreatPassArollcheckShoot".Translate(pawn.LabelShort, globalTargetInfo.Label, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
				}
			}
			if (roll < modExtension.successThreshold)
			{
				foreach (GlobalTargetInfo globalTargetInfo in targets)
				{
					Projectile projectile = (Projectile)GenSpawn.Spawn(modExtension.projectileWhenFail, pawn.Position, pawn.Map);
					if (globalTargetInfo.HasThing)
					{
						projectile.Launch(pawn, pawn.DrawPos, globalTargetInfo.Thing, globalTargetInfo.Thing, ProjectileHitFlags.IntendedTarget);
					}
					else
					{
						projectile.Launch(pawn, pawn.DrawPos, globalTargetInfo.Cell, globalTargetInfo.Cell, ProjectileHitFlags.IntendedTarget);
					}
					Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, baseRoll, cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
					Messages.Message("Makai_FailArollcheckShoot".Translate(pawn.LabelShort, globalTargetInfo.Label, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
				}
			}
		}
	}
}
