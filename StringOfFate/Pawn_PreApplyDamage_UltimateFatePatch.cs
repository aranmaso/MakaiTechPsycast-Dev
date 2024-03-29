﻿using HarmonyLib;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;
using System.Linq;

namespace MakaiTechPsycast.StringOfFate
{
    [HarmonyPatch(typeof(Pawn),nameof(Pawn.PreApplyDamage))]
    public class Pawn_PreApplyDamage_UltimateFatePatch
    {
        private static void Postfix(ref DamageInfo dinfo, ref bool absorbed, ref Pawn __instance)
        {
            if (__instance == null || !__instance.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiPsy_SF_UltimateFate))
            {
                return;
            }
            HediffComp_UltimateFate hediff = MakaiUtility.GetFirstHediffOfDef(__instance, MakaiTechPsy_DefOf.MakaiPsy_SF_UltimateFate).TryGetComp<HediffComp_UltimateFate>();
            dinfo.SetAmount(hediff.maxThresholdPerHit);
            if (hediff.totalDamage < hediff.threshold)
            {
                return;
            }            
            else
            {
                hediff.totalDamage = 0;
                absorbed = true;
                if (hediff.fatedCount > 0)
                {
                    List<Hediff> list = __instance.health.hediffSet.hediffs.Where(MakaiUtility.FindBadHediff).ToList();
                    foreach (Hediff item in list)
                    {
                        __instance.health.RemoveHediff(item);
                    }
                    MoteMaker.ThrowText(__instance.Position.ToVector3(),__instance.Map,"Fated Save");
                    hediff.fatedCount--;
                    MakaiUtility.ThrowFleck(MakaiTechPsy_DefOf.MakaiPsyMote_ReflectProjectile,__instance.DrawPos,__instance.Map,5f);
                }                                
            }
        }
    }
}
