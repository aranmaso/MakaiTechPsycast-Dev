using HarmonyLib;
using Verse;
using UnityEngine;
using System;
using VFECore;

namespace MakaiTechPsycast.TrueDestruction
{
	[HarmonyPatch(typeof(Projectile), "Launch", new Type[]
{
	typeof(Thing),
	typeof(Vector3),
	typeof(LocalTargetInfo),
	typeof(LocalTargetInfo),
	typeof(ProjectileHitFlags),
	typeof(bool),
	typeof(Thing),
	typeof(ThingDef)
})]
	public static class Projectile_Launch_Patch
    {
		//private static readonly AccessTools.FieldRef<Projectile, Vector3> destination = AccessTools.FieldRefAccess<Projectile, Vector3>("destination");
		private static void Postfix(Projectile __instance,Thing launcher,Vector3 origin, ref LocalTargetInfo usedTarget, LocalTargetInfo intendedTarget, bool preventFriendlyFire, Thing equipment, ThingDef targetCoverDef)
        {
			if(__instance == null || !(__instance.Launcher is Pawn instigator) || !instigator.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiTechPsy_TD_SkyBlessing))
            {
				return;
            }
			HediffComp_SkyBlessing comp = MakaiUtility.GetFirstHediffOfDef(instigator, MakaiTechPsy_DefOf.MakaiTechPsy_TD_SkyBlessing).TryGetComp<HediffComp_SkyBlessing>();
			if (comp.useCountLeft <= 0 || !comp.isToggledOn || comp.isTriggered)
            {
				return;
            }
			__instance.destination = new Vector3(__instance.destination.x, __instance.destination.y, __instance.destination.z + 7);			
			comp.isTriggered = true;
		}
	}
}
	