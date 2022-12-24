using RimWorld;
using Verse;
using System.Linq;
using HarmonyLib;
using UnityEngine;
using MakaiTechPsycast.StringOfFate;

namespace MakaiTechPsycast
{
    [HarmonyPatch(typeof(Pawn))]
    [HarmonyPatch("DrawAt")]
    public static class Pawn_DrawAtMainPatch
    {
        private static void Postfix(Pawn __instance, Vector3 drawLoc)
        {
            foreach (HediffComp_UltimateFate renderable in __instance.health.hediffSet.hediffs.OfType<HediffWithComps>().SelectMany((HediffWithComps x) => x.comps).OfType<HediffComp_UltimateFate>())
            {
                renderable.DrawAt(drawLoc);
            }
            foreach (HediffComp_DrawAt renderable in __instance.health.hediffSet.hediffs.OfType<HediffWithComps>().SelectMany((HediffWithComps x) => x.comps).OfType<HediffComp_DrawAt>())
            {
                renderable.DrawAt(drawLoc);
            }
        }
    }
}
