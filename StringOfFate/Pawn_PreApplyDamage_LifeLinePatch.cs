using HarmonyLib;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace MakaiTechPsycast.StringOfFate
{
    [HarmonyPatch(typeof(Pawn))]
    [HarmonyPatch("PreApplyDamage")]
    public class Pawn_PreApplyDamage_LifeLinePatch
    {
        private static void Postfix(ref DamageInfo dinfo, ref bool absorbed, ref Pawn __instance)
        {
            if (__instance == null || !__instance.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiPsy_SF_Lifeline))
            {
                return;
            }
            float originalDamage = dinfo.Amount;
            dinfo.SetAmount(0);
            List<Hediff> hediffs = __instance.health.hediffSet.hediffs;
            foreach(Hediff item in hediffs)
            {
                if(item is Hediff_Injury injury)
                {
                    if(originalDamage > 0)
                    {
                        float deduct = originalDamage * 0.1f;
                        injury.Severity -= deduct;
                        originalDamage -= deduct;
                    }
                }
                else
                {
                    continue;
                }
            }
        }
    }
}
