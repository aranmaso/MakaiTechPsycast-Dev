using HarmonyLib;
using RimWorld;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using System;
using VFECore;

namespace MakaiTechPsycast.TrueDestruction
{
    [HarmonyPatch(typeof(Bullet))]
    [HarmonyPatch("Impact")]
    public class Bullet_Impact_FrenziedFlameBuff
    {
        private static void Postfix(Thing hitThing, Bullet __instance)
        {
            if (__instance == null || !(__instance.Launcher is Pawn pawn) || !pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiTechPsy_TD_FrenziedFlame))
            {
                return;
            }
            if(__instance.intendedTarget.Thing is Pawn pawnEn)
            {
                if (!pawnEn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiTechPsy_TD_FrenziedMark) && pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiTechPsy_TD_FrenziedSpeedBuff))
                {
                    pawn.health.RemoveHediff(pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_TD_FrenziedSpeedBuff));
                }
                pawnEn.health.AddHediff(MakaiTechPsy_DefOf.MakaiTechPsy_TD_FrenziedMark);
                if (pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiTechPsy_TD_FrenziedSpeedBuff))
                {
                    pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_TD_FrenziedSpeedBuff).Severity += 0.5f;
                    pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_TD_FrenziedSpeedBuff).TryGetComp<HediffComp_Disappears>().ticksToDisappear = 625;
                }
                else
                {
                    Hediff hediff = HediffMaker.MakeHediff(MakaiTechPsy_DefOf.MakaiTechPsy_TD_FrenziedSpeedBuff, pawn);
                    hediff.Severity = 1;
                    pawn.health.AddHediff(hediff);
                }
            }
        }
    }
}
