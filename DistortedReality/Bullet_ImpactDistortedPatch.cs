using HarmonyLib;
using RimWorld;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace MakaiTechPsycast.DistortedReality
{
    [HarmonyPatch(typeof(Bullet))]
    [HarmonyPatch("Impact")]
    public class Bullet_ImpactDistortedPatch
    {
        private static void Postfix(Thing hitThing, ref Bullet __instance)
        {
            Thing thing = __instance.Launcher;
            if(thing == null || thing is not Pawn pawn || !(pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiTechPsy_DR_DistortBulletBounce)))
            {
                return;
            }
            HediffComp_BouncingBullet Hediff = pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_DR_DistortBulletBounce).TryGetComp<HediffComp_BouncingBullet>();
            if (Hediff != null && pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiTechPsy_DR_DistortBulletBounce))
            {
                if (Hediff.bouncingCountLeft > 0)
                {
                    int count = 0;                    
                    foreach (var pawnEn in MakaiUtility.GetNearbyPawnFriendAndFoe(__instance.PositionHeld, pawn.MapHeld, 20f).InRandomOrder())
                    {
                        if(pawnEn == __instance.intendedTarget)
                        {
                            continue;
                        }
                        if(pawnEn.Downed || pawnEn.Dead)
                        {
                            continue;
                        }
                        if (Rand.Value < Hediff.Props.chance && pawnEn.Faction != thing.Faction && pawnEn.Faction.HostileTo(thing.Faction))
                        {
                            Projectile projectile = (Projectile)GenSpawn.Spawn(__instance.def, __instance.Position, pawn.Map);
                            projectile.Launch(__instance.Launcher, pawnEn, pawnEn, ProjectileHitFlags.IntendedTarget);
                            Hediff.bouncingCountLeft -= 1;
                            count++;
                        }
                        else if(pawnEn.Faction != thing.Faction && pawnEn.Faction.HostileTo(thing.Faction))
                        {
                            Projectile projectile = (Projectile)GenSpawn.Spawn(__instance.def, __instance.Position, pawn.Map);
                            projectile.Launch(__instance.Launcher, pawnEn.Position, pawnEn, ProjectileHitFlags.IntendedTarget);
                            Hediff.bouncingCountLeft -= 1;
                            count++;
                        }
                        if(count >= 1)
                        {
                            break;
                        }
                    }
                    if (Hediff.bouncingCountLeft == 0)
                    {
                        pawn.health.RemoveHediff(pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_DR_DistortBulletBounce));
                    }
                }
            }
        }
    }
}
