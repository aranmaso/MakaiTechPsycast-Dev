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
		private static void Postfix(ref Thing hitThing, Bullet __instance)
		{
			if (__instance == null) return;
			CompTDturret compTDProps = __instance.TryGetComp<CompTDturret>();
			if (compTDProps == null)
			{
				return;
			}
			float linkedCount = 1f;
			foreach (Thing item in GenRadial.RadialDistinctThingsAround(__instance.Position, __instance.Map, 5f, useCenter: true))
            {
                if (__instance.TryGetComp<CompAffectedByFacilities>().LinkedFacilitiesListForReading.Contains(item))
                {
                    linkedCount++;
                }
				else
                {
					continue;
                }
            }
			Log.Message("ForeachComplete");
			__instance.Map.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrikeGreen(__instance.Launcher.Map, __instance.Position));
			GenExplosion.DoExplosion(__instance.Position, __instance.Map, linkedCount, DamageDefOf.Bomb, __instance, __instance.DamageAmount, __instance.ArmorPenetration);
			/*float rand = Rand.Value;
			if (pawn2 != null && rand <= 0.25f)
            {
				pawn2.health.AddHediff(compTDProps.Props.hediffDef);
            }*/
		}
	}
}
