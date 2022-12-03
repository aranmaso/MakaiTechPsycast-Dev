using HarmonyLib;
using MakaiTechPsycast;
using RimWorld;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace MakaiTechPsycast.TrueDestruction
{
    [HarmonyPatch(typeof(Bullet))]
    [HarmonyPatch("Impact")]
    public class Bullet_Impact_TD_AffectPawn
    {
		private static void Postfix(ref Thing hitThing, ref Bullet __instance)
		{
			Thing turret = __instance.Launcher;
			CompTDturret compTDProps = turret.TryGetComp<CompTDturret>();
			if (turret == null || compTDProps == null)
			{
				return;
			}
			float linkedCount = 1f;
			foreach (Thing item in GenRadial.RadialDistinctThingsAround(turret.Position, turret.Map, 5f, useCenter: true))
            {
                if (turret.TryGetComp<CompAffectedByFacilities>().LinkedFacilitiesListForReading.Contains(item))
                {
                    linkedCount++;
                }
				else
                {
					continue;
                }
            }
			Log.Message("ForeachComplete");
			turret.Map.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrikeGreen(turret.Map, __instance.Position));
			GenExplosion.DoExplosion(__instance.Position, turret.Map, linkedCount, DamageDefOf.Bomb, turret, __instance.DamageAmount, __instance.ArmorPenetration);
			/*float rand = Rand.Value;
			if (pawn2 != null && rand <= 0.25f)
            {
				pawn2.health.AddHediff(compTDProps.Props.hediffDef);
            }*/
		}
	}
}
