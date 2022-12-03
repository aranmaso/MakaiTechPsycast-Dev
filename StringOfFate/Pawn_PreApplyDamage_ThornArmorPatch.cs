using HarmonyLib;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace MakaiTechPsycast.StringOfFate
{
	[HarmonyPatch(typeof(Pawn))]
	[HarmonyPatch("PreApplyDamage")]
	public class Pawn_PreApplyDamage_ThornArmorPatch
	{
		private static void Postfix(ref DamageInfo dinfo, ref bool absorbed, ref Pawn __instance)
		{
			if (__instance == null || !__instance.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiPsy_SF_Counter))
			{
				return;
            }
            MirroredFateInfo minfo = __instance.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiPsy_SF_Counter).TryGetComp<HediffComp_MirroredFate>().mirrorInfo;
			HediffComp_MirroredFate mirrorFate = __instance.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiPsy_SF_Counter).TryGetComp<HediffComp_MirroredFate>();

			if (dinfo.Instigator is Pawn pawn && !__instance.Dead && !pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiPsy_SF_Counter) && minfo.reflectMelee && !dinfo.Def.isRanged)
			{
				if(mirrorFate.reflectCount > 0)
                {
					if (!minfo.userTakeDamage)
					{
						dinfo.SetAmount(0);
					}
					if (minfo.reflectOnlyEnemies && pawn.Faction.HostileTo(__instance.Faction) && !minfo.reflectOnlyFriendly)
					{
						BodyPartRecord hitPartAttacker = pawn.RaceProps.body.AllParts.FirstOrFallback((BodyPartRecord p) => p.def == BodyPartDefOf.Hand);
						pawn.TakeDamage(new DamageInfo(DamageDefOf.Cut, dinfo.Amount * minfo.reflectPercent, dinfo.ArmorPenetrationInt, dinfo.Angle, __instance, hitPartAttacker));
					}
					else if (minfo.reflectOnlyFriendly && !pawn.Faction.HostileTo(__instance.Faction) && !minfo.reflectOnlyEnemies)
					{
						BodyPartRecord hitPartAttacker = pawn.RaceProps.body.AllParts.FirstOrFallback((BodyPartRecord p) => p.def == BodyPartDefOf.Hand);
						pawn.TakeDamage(new DamageInfo(DamageDefOf.Cut, dinfo.Amount * minfo.reflectPercent, dinfo.ArmorPenetrationInt, dinfo.Angle, __instance, hitPartAttacker));
					}
					mirrorFate.reflectCount--;
					if (mirrorFate.reflectCount == 0)
					{
						__instance.health.RemoveHediff(__instance.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiPsy_SF_Counter));
					}
				}
				
			}
			if(dinfo.Instigator is Pawn pawn_1 && pawn_1.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiPsy_SF_Counter))
            {
				Messages.Message("Mirrored Fate can't reflect each other",MessageTypeDefOf.SilentInput,false);
            }
			if (dinfo.Instigator is Pawn pawn2 && !__instance.Dead && !pawn2.health .hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiPsy_SF_Counter) && minfo.reflectRanged && dinfo.Def.isRanged)
			{
				if (mirrorFate.reflectCount > 0)
                {
					if (minfo.reflectOnlyEnemies && pawn2.Faction.HostileTo(__instance.Faction) && !minfo.reflectOnlyFriendly)
					{
						ThingDef projectile = dinfo.Weapon.Verbs[0].defaultProjectile;
						if (!minfo.userTakeDamage)
						{
							dinfo.SetAmount(0);
						}
						IntVec3 location = MakaiUtility.RandomCellAround(pawn2, 4);
						Projectile projectile2 = (Projectile)GenSpawn.Spawn(projectile, location, __instance.Map);
						projectile2.Launch(__instance, pawn2, pawn2, ProjectileHitFlags.IntendedTarget);
						Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_WarpBullet.Spawn(location, __instance.Map, 1);
						effect.Cleanup();
					}
					else if (minfo.reflectOnlyFriendly && !pawn2.Faction.HostileTo(__instance.Faction) && !minfo.reflectOnlyEnemies)
					{
						ThingDef projectile = dinfo.Weapon.Verbs[0].defaultProjectile;
						if (!minfo.userTakeDamage)
						{
							dinfo.SetAmount(0);
						}
						IntVec3 location = MakaiUtility.RandomCellAround(pawn2, 4);
						Projectile projectile2 = (Projectile)GenSpawn.Spawn(projectile, location, __instance.Map);
						projectile2.Launch(__instance, pawn2, pawn2, ProjectileHitFlags.IntendedTarget);
						Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_WarpBullet.Spawn(location, __instance.Map, 1);
						effect.Cleanup();
					}
					mirrorFate.reflectCount--;
					if (mirrorFate.reflectCount == 0)
					{
						__instance.health.RemoveHediff(__instance.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiPsy_SF_Counter));
					}
				}				
			}
			if (dinfo.Instigator is Pawn pawn_2 && pawn_2.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiPsy_SF_Counter))
			{
				Messages.Message("Mirrored Fate can't reflect each other", MessageTypeDefOf.SilentInput, false);
			}
			if (dinfo.Instigator is Building building && !__instance.Dead && __instance.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiPsy_SF_Counter))
			{
				if(mirrorFate.reflectCount > 0)
                {
					if (!minfo.userTakeDamage)
					{
						dinfo.SetAmount(0);
					}
					if (minfo.reflectOnlyEnemies && building.Faction.HostileTo(__instance.Faction) && !minfo.reflectOnlyFriendly)
					{
						ThingDef projectile = dinfo.Weapon.Verbs[0].defaultProjectile;
						IntVec3 location = MakaiUtility.RandomCellAround(__instance, 4);
						Projectile projectile2 = (Projectile)GenSpawn.Spawn(projectile, location, __instance.Map);
						projectile2.Launch(__instance, building, building, ProjectileHitFlags.IntendedTarget);
						Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_WarpBullet.Spawn(location, __instance.Map, 1);
						effect.Cleanup();
					}
					else if (minfo.reflectOnlyFriendly && !building.Faction.HostileTo(__instance.Faction) && !minfo.reflectOnlyEnemies)
					{
						ThingDef projectile = dinfo.Weapon.Verbs[0].defaultProjectile;
						IntVec3 location = MakaiUtility.RandomCellAround(__instance, 4);
						Projectile projectile2 = (Projectile)GenSpawn.Spawn(projectile, location, __instance.Map);
						projectile2.Launch(__instance, building, building, ProjectileHitFlags.IntendedTarget);
						Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_WarpBullet.Spawn(location, __instance.Map, 1);
						effect.Cleanup();
					}
					mirrorFate.reflectCount--;
					if(mirrorFate.reflectCount == 0)
                    {
						__instance.health.RemoveHediff(__instance.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiPsy_SF_Counter));
                    }
				}				
			}

		}
	}
}
