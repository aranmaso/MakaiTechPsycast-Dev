using HarmonyLib;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using System;

namespace MakaiTechPsycast
{
	/*[HarmonyPatch(typeof(Pawn))]
	[HarmonyPatch("PostApplyDamage")]
	public class Pawn_PostApplyDamage_ThornArmorPatch
	{
		private static void Postfix(ref DamageInfo dinfo, ref float totalDamageDealt, ref Pawn __instance)
		{
			ThingDef weapon = dinfo.Weapon;
			if (weapon == null || __instance == null || !__instance.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiPsy_SF_Reverse))
			{
				return;
			}
			if(dinfo.Instigator is Pawn pawn && !__instance.Dead && __instance.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiPsy_SF_Reverse) && weapon.IsMeleeWeapon)
            {
				float rand = Rand.Value;
				if(rand <= 0.2f)
                {
                    BodyPartRecord hitPartAttacker = pawn.RaceProps.body.AllParts.FirstOrFallback((BodyPartRecord p) => p.def == BodyPartDefOf.Hand);
					pawn.TakeDamage(new DamageInfo(dinfo.Def, totalDamageDealt, dinfo.ArmorPenetrationInt, dinfo.Angle, __instance, hitPartAttacker, dinfo.Weapon));
					pawn.equipment.DropAllEquipment(pawn.RandomAdjacentCell8Way());
                    MakaiUtility.Cure(MakaiUtility.FindInjury(__instance));
				}
				else
                {
                    BodyPartRecord hitPartAttacker = pawn.RaceProps.body.AllParts.FirstOrFallback((BodyPartRecord p) => p.def == BodyPartDefOf.Hand);
					pawn.TakeDamage(new DamageInfo(dinfo.Def, totalDamageDealt*0.2f, dinfo.ArmorPenetrationInt, dinfo.Angle, __instance, hitPartAttacker, dinfo.Weapon));
					pawn.equipment.DropAllEquipment(pawn.RandomAdjacentCell8Way());
					if (__instance.health.hediffSet.GetInjuriesTendable().EnumerableCount() > 0)
					{
                        MakaiUtility.Cure(MakaiUtility.FindInjury(__instance));
					}
				}
			}
			if(dinfo.Instigator is Pawn pawn2 && !__instance.Dead && __instance.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiPsy_SF_Reverse))
            {
				float rand = Rand.Value;
				if(rand <= 0.5f)
                {
					ThingDef projectile = dinfo.Weapon.Verbs[0].defaultProjectile;
					IntVec3 location = MakaiUtility.RandomCellAround(pawn2, 4);
					Projectile projectile2 = (Projectile)GenSpawn.Spawn(projectile, location, __instance.Map);
					projectile2.Launch(__instance, pawn2, pawn2, ProjectileHitFlags.IntendedTarget);
					Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_WarpBullet.Spawn(location, __instance.Map, 1);
					effect.Cleanup();
					if (__instance.health.hediffSet.GetInjuriesTendable().EnumerableCount() > 0)
					{
						MakaiUtility.Cure(MakaiUtility.FindInjury(__instance));
					}
				}
			}
			if(dinfo.Instigator is Building building && !__instance.Dead && __instance.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiPsy_SF_Reverse))
            {
				building.HitPoints -= Mathf.FloorToInt(dinfo.Amount);
            }
			
		}
	}*/
}
