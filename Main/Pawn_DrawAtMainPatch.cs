using RimWorld;
using Verse;
using HarmonyLib;
using UnityEngine;
using System.Linq;
using MakaiTechPsycast.StringOfFate;
using System.Collections.Generic;
using MakaiTechPsycast.Main;

namespace MakaiTechPsycast
{
    [HarmonyPatch(typeof(Pawn))]
    [HarmonyPatch("DrawAt")]
    public static class Pawn_DrawAtMainPatch
    {
        private static void Postfix(Pawn __instance, Vector3 drawLoc)
        {            
            if (__instance == null || __instance.Map == null) return;
            if(Current.Game.GetComponent<GameComponent_DrawAt>().animatedPawns.Contains(__instance))
            {
                for (int i = __instance.health.hediffSet.hediffs.Count - 1; i >= 0; i--)
                {
                    __instance.health.hediffSet.hediffs[i].TryGetComp<HediffComp_DrawAt>()?.DrawAt(drawLoc);
                }
            }            
        }
    }
}
