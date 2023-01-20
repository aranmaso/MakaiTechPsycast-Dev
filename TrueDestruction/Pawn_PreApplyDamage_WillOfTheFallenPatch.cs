using HarmonyLib;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using System.Linq;

namespace MakaiTechPsycast.TrueDestruction
{
    [HarmonyPatch(typeof(Pawn))]
    [HarmonyPatch("PreApplyDamage")]
    public class Pawn_PreApplyDamage_WillOfTheFallenPatch
    {
        private static void Postfix(ref DamageInfo dinfo, ref bool absorbed, Pawn __instance)
        {
            if (__instance == null || !(dinfo.Instigator is Pawn instigator) || absorbed)
            {
                return;
            }
            if (!instigator.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiTechPsy_TD_WillOfTheFallen)) return;

            HediffComp_WillOfTheFallen comps = MakaiUtility.GetFirstHediffOfDef(instigator, MakaiTechPsy_DefOf.MakaiTechPsy_TD_WillOfTheFallen).TryGetComp<HediffComp_WillOfTheFallen>();
            float originalDamage = dinfo.Amount;
            dinfo.SetAmount(originalDamage + comps.count * 0.05f);
        }
    }
}
