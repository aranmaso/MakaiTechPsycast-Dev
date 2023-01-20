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
    public static class Bullet_Impact_Spread
    {
        private static void Postfix(Thing hitThing, Bullet __instance)
        {
            if(__instance == null || __instance.Launcher == null || !(__instance.Launcher is Pawn pawn))
            {
                return;
            }
            //if (pawn.equipment.Primary.def.IsMeleeWeapon) return;

            if(pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiTechPsy_TD_SkyBlessing))
            {
                HediffComp_SkyBlessing comp = MakaiUtility.GetFirstHediffOfDef(pawn, MakaiTechPsy_DefOf.MakaiTechPsy_TD_SkyBlessing).TryGetComp<HediffComp_SkyBlessing>();
                if(comp.isToggledOn && comp.useCountLeft > 0 && !comp.isBursted)
                {
                    List<IntVec3> possibleTargetCell = new List<IntVec3>();
                    int count = Mathf.RoundToInt(5 * pawn.GetStatValue(StatDefOf.PsychicSensitivity));
                    for (int i = 0; i < count; i++)
                    {
                        possibleTargetCell.Add(MakaiUtility.RandomCellAroundCellBase(__instance.intendedTarget.Cell, -5, 5));
                    }
                    for (int i = 0; i < possibleTargetCell.Count; i++)
                    {
                        Projectile shrapnelThing = (Projectile)GenSpawn.Spawn(__instance.def, __instance.Position, __instance.Launcher.Map);
                        Thing possibleThing = possibleTargetCell[i].GetFirstPawn(__instance.Launcher.Map) ?? possibleTargetCell[i].GetFirstBuilding(__instance.Launcher.Map) as Thing;
                        if (possibleThing != null)
                        {
                            shrapnelThing.Launch(__instance.Launcher, possibleThing, possibleThing, ProjectileHitFlags.IntendedTarget);
                        }
                        else
                        {
                            shrapnelThing.Launch(__instance.Launcher, possibleTargetCell[i], possibleTargetCell[i], ProjectileHitFlags.NonTargetPawns);
                        }
                    }
                    __instance.Launch(pawn, __instance.Position.ToVector3(), __instance.intendedTarget, __instance.intendedTarget, ProjectileHitFlags.IntendedTarget);
                    comp.isBursted = true;
                    comp.useCountLeft--;
                }                
            }            
        }
    }
}
