using RimWorld;
using Verse;
using UnityEngine;

namespace MakaiTechPsycast
{
    public class Verb_ShootingOffset : Verb_LaunchProjectile
    {
		protected override int ShotsPerBurst => verbProps.burstShotCount;

		public override void WarmupComplete()
		{
			base.WarmupComplete();
			if (currentTarget.Thing is Pawn pawn && !pawn.Downed && CasterIsPawn && CasterPawn.skills != null)
			{
				float num = (pawn.HostileTo(caster) ? 170f : 20f);
				float num2 = verbProps.AdjustedFullCycleTime(this, CasterPawn);
				CasterPawn.skills.Learn(SkillDefOf.Shooting, num * num2);
			}
		}
		protected override bool TryCastShot()
		{
			ThingDef projectile = base.Projectile;

			ShootLine resultingLine;
			bool flag = TryFindShootLineFromTo(caster.Position, currentTarget, out resultingLine);
			IntVec3 location = MakaiUtility.RandomCellAround(CasterPawn, 2);
			if (base.EquipmentSource != null)
			{
				base.EquipmentSource.GetComp<CompChangeableProjectile>()?.Notify_ProjectileLaunched();
				base.EquipmentSource.GetComp<CompReloadable>()?.UsedOnce();
			}

			Projectile projectile2 = (Projectile)GenSpawn.Spawn(projectile, location, CasterPawn.Map);
			if (verbProps.ForcedMissRadius > 0.5f)
			{
				float num = verbProps.ForcedMissRadius;
				float num2 = VerbUtility.CalculateAdjustedForcedMiss(num, currentTarget.Cell - caster.Position);
				if (num2 > 0.5f)
				{
					int max = GenRadial.NumCellsInRadius(num2);
					int num3 = Rand.Range(0, max);
					if (num3 > 0)
					{
						IntVec3 intVec = currentTarget.Cell + GenRadial.RadialPattern[num3];
						ProjectileHitFlags projectileHitFlags = ProjectileHitFlags.NonTargetWorld;
						if (Rand.Chance(0.5f))
						{
							projectileHitFlags = ProjectileHitFlags.All;
						}
						if (!canHitNonTargetPawnsNow)
						{
							projectileHitFlags &= ~ProjectileHitFlags.NonTargetPawns;
						}
						projectile2.Launch(CasterPawn, intVec, currentTarget, projectileHitFlags, preventFriendlyFire, base.EquipmentSource);
						Effecter effect2 = MakaiTechPsy_DefOf.MakaiPsy_WarpBullet.Spawn(location, CasterPawn.Map, 1);
						effect2.Cleanup();
						return true;
					}
				}
			}
			Vector3 drawPos = caster.DrawPos;
			ShotReport shotReport = ShotReport.HitReportFor(caster, this, currentTarget);
			Thing randomCoverToMissInto = shotReport.GetRandomCoverToMissInto();
			ThingDef targetCoverDef = randomCoverToMissInto?.def;
			if (!Rand.Chance(shotReport.AimOnTargetChance_IgnoringPosture))
			{
				resultingLine.ChangeDestToMissWild(shotReport.AimOnTargetChance_StandardTarget);
				ProjectileHitFlags projectileHitFlags2 = ProjectileHitFlags.NonTargetWorld;
				if (Rand.Chance(0.5f) && canHitNonTargetPawnsNow)
				{
					projectileHitFlags2 |= ProjectileHitFlags.NonTargetPawns;
				}
				projectile2.Launch(CasterPawn, resultingLine.Dest, currentTarget, ProjectileHitFlags.IntendedTarget, preventFriendlyFire, base.EquipmentSource);
				Effecter effectMiss = MakaiTechPsy_DefOf.MakaiPsy_WarpBullet.Spawn(location, CasterPawn.Map, 1);
				effectMiss.Cleanup();
				return true;
			}
			if (currentTarget.Thing != null && currentTarget.Thing.def.category == ThingCategory.Pawn && !Rand.Chance(shotReport.PassCoverChance))
			{
				ProjectileHitFlags projectileHitFlags3 = ProjectileHitFlags.NonTargetWorld;
				if (canHitNonTargetPawnsNow)
				{
					projectileHitFlags3 |= ProjectileHitFlags.NonTargetPawns;
				}
				projectile2.Launch(CasterPawn, randomCoverToMissInto, currentTarget, projectileHitFlags3, preventFriendlyFire, base.EquipmentSource);
				Effecter effectMiss = MakaiTechPsy_DefOf.MakaiPsy_WarpBullet.Spawn(location, CasterPawn.Map, 1);
				effectMiss.Cleanup();
				return true;
			}
			projectile2.Launch(CasterPawn, currentTarget, currentTarget, ProjectileHitFlags.IntendedTarget, preventFriendlyFire, base.EquipmentSource);
			Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_WarpBullet.Spawn(location, CasterPawn.Map, 1);
			effect.Cleanup();
			return true;
		}
	}
}
