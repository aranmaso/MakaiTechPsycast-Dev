using HarmonyLib;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace MakaiTechPsycast
{
	[HarmonyPatch(typeof(Pawn))]
	[HarmonyPatch("PreApplyDamage")]
	public class Pawn_PreApplyDamage_ThornArmorPatch
	{
		private static void Postfix(ref DamageInfo dinfo, ref bool absorbed, ref Pawn __instance)
		{
			ThingDef weapon = dinfo.Weapon;
			if (weapon == null || __instance == null || !__instance.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiPsy_SF_Counter))
			{
				return;
			}
			if (dinfo.Instigator is Pawn pawn && !__instance.Dead && __instance.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiPsy_SF_Counter) && !dinfo.Def.isRanged)
			{
				float rand = Rand.Value;
				if (rand <= 0.2f)
				{
					BodyPartRecord hitPartAttacker = pawn.RaceProps.body.AllParts.FirstOrFallback((BodyPartRecord p) => p.def == BodyPartDefOf.Hand);
					pawn.TakeDamage(new DamageInfo(DamageDefOf.Cut, dinfo.Amount, dinfo.ArmorPenetrationInt, dinfo.Angle, __instance, hitPartAttacker));
				}
				else
				{
					BodyPartRecord hitPartAttacker = pawn.RaceProps.body.AllParts.FirstOrFallback((BodyPartRecord p) => p.def == BodyPartDefOf.Head);
					pawn.TakeDamage(new DamageInfo(DamageDefOf.Cut, dinfo.Amount * 0.2f, dinfo.ArmorPenetrationInt, dinfo.Angle, __instance, hitPartAttacker));
				}
			}
			if (dinfo.Instigator is Pawn pawn2 && !__instance.Dead && __instance.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiPsy_SF_Counter) && dinfo.Def.isRanged)
			{
				float rand = Rand.Value;
				if (rand <= 0.5f)
				{
					dinfo.SetAmount(0);
					ThingDef projectile = dinfo.Weapon.Verbs[0].defaultProjectile;
					IntVec3 location = MakaiUtility.RandomCellAround(pawn2, 4);
					Projectile projectile2 = (Projectile)GenSpawn.Spawn(projectile, location, __instance.Map);
					projectile2.Launch(__instance, pawn2, pawn2, ProjectileHitFlags.IntendedTarget);
					Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_WarpBullet.Spawn(location, __instance.Map, 1);
					effect.Cleanup();
				}
			}
			if (dinfo.Instigator is Building building && !__instance.Dead && __instance.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiPsy_SF_Counter))
			{
				float rand = Rand.Value;
				if (rand <= 0.5f)
                {
					dinfo.SetAmount(0);
					ThingDef projectile = dinfo.Weapon.Verbs[0].defaultProjectile;
					IntVec3 location = MakaiUtility.RandomCellAround(__instance, 4);
					Projectile projectile2 = (Projectile)GenSpawn.Spawn(projectile, location, __instance.Map);
					projectile2.Launch(__instance, building, building, ProjectileHitFlags.IntendedTarget);
					Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_WarpBullet.Spawn(location, __instance.Map, 1);
					effect.Cleanup();
				}
			}

		}
	}
}
