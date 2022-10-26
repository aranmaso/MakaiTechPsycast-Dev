using RimWorld;
using Verse;

namespace MakaiTechPsycast.TrueDestruction
{
	public class Verb_CallStrike : Verb_LaunchProjectile
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
			IntVec3 location = MakaiUtility.RandomCellAround(Caster, 4);
			Projectile projectile2 = (Projectile)GenSpawn.Spawn(projectile, location, Caster.Map);
			projectile2.Launch(Caster, currentTarget, currentTarget, ProjectileHitFlags.IntendedTarget, preventFriendlyFire, base.EquipmentSource);
			Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_WarpBullet.Spawn(location, Caster.Map, 1);
			effect.Cleanup();

			MakaiTD_PowerBeam orbitalStrike = (MakaiTD_PowerBeam)GenSpawn.Spawn(MakaiTechPsy_DefOf.MakaiPsy_TD_Beam, currentTarget.Thing.Position, Caster.Map);
			orbitalStrike.duration = 60;
			orbitalStrike.instigator = Caster;
			orbitalStrike.StartStrike();
			Effecter effect2 = MakaiTechPsy_DefOf.MakaiPsy_TD_Blast.Spawn(currentTarget.Thing.Position, Caster.Map, 0.5f);
			effect2.Cleanup();

			return true;
		}
	}
}
