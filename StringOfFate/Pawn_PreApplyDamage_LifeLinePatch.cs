using HarmonyLib;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using System.Linq;

namespace MakaiTechPsycast.StringOfFate
{
    [HarmonyPatch(typeof(Pawn))]
    [HarmonyPatch("PreApplyDamage")]
    public class Pawn_PreApplyDamage_LifeLinePatch
    {
        private static void Postfix(ref DamageInfo dinfo, ref bool absorbed, ref Pawn __instance)
        {
            if (__instance == null || !__instance.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiPsy_SF_Lifeline) || absorbed)
            {
                return;
            }
            float originalDamage = dinfo.Amount;
            dinfo.SetAmount(0);
            foreach(Hediff_Injury item in __instance.health.hediffSet.hediffs.OfType<Hediff_Injury>().Distinct())
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
