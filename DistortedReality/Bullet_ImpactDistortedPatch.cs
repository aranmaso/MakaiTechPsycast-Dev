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
        private static void Postfix(ref Thing hitThing, ref Bullet __instance)
        {
            Thing thing = __instance.Launcher;
            Thing hitThing2 = hitThing;
            if(thing == null || !(thing is Pawn pawn) || !(pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiTechPsy_DR_DistortBulletBounce)))
            {
                return;
            }
            /*if(!(hitThing is Pawn pawnEnemy))
            {

            }*/
            HediffComp_BouncingBullet Hediff = pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_DR_DistortBulletBounce).TryGetComp<HediffComp_BouncingBullet>();
            if (Hediff != null && pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiTechPsy_DR_DistortBulletBounce))
            {
                if (Hediff.bouncingCountLeft > 0)
                {
                    int count = 0;
                    foreach (Thing item in GenRadial.RadialDistinctThingsAround(__instance.Position, pawn.Map, 20f, true))
                    {
                        if (!(item is Pawn pawnEn))
                        {
                            continue;
                        }
                        if(pawnEn == __instance.intendedTarget)
                        {
                            continue;
                        }
                        if(pawnEn.Downed || pawnEn.Dead)
                        {
                            continue;
                        }
                        float rand = Rand.Value;
                        if (rand < Hediff.Props.chance && pawnEn.Faction != thing.Faction && pawnEn.Faction.HostileTo(thing.Faction))
                        {
                            Projectile projectile = (Projectile)GenSpawn.Spawn(__instance.def, __instance.Position, pawn.Map);
                            projectile.Launch(__instance.Launcher, item, item, ProjectileHitFlags.IntendedTarget);
                            Hediff.bouncingCountLeft -= 1;
                            count++;
                        }
                        else if(pawnEn.Faction != thing.Faction && pawnEn.Faction.HostileTo(thing.Faction))
                        {
                            Projectile projectile = (Projectile)GenSpawn.Spawn(__instance.def, __instance.Position, pawn.Map);
                            projectile.Launch(__instance.Launcher, item.Position, item, ProjectileHitFlags.IntendedTarget);
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
