﻿using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;
using VanillaPsycastsExpanded;
using Verse.AI;
using System;

namespace MakaiTechPsycast
{
    public class CompProjectileExtraEffect : ThingComp
    {
        public int ticksSinceLastShoot;

        public int ticksSinceLastHurt;

        public int ticksSinceLastCatch;

        private int pawnCount = 0;

        private int targetBonus = 0;
        private int damageBonus = 0;

        private CompProperties_ProjectileExtraEffect Props => (CompProperties_ProjectileExtraEffect)props;

        public override void PostPostMake()
        {
            ticksSinceLastShoot = Find.TickManager.TicksGame + Props.interval;
            ticksSinceLastHurt = Find.TickManager.TicksGame + Props.hurtInterval;
            ticksSinceLastCatch = Find.TickManager.TicksGame + Props.catchInterval;
            base.PostPostMake();
        }
        public override void CompTick()
        {
            base.CompTick();
            if(Find.TickManager.TicksGame == ticksSinceLastShoot && Props.projectileDefs.Count > 0 && Props.shootMoreBullet)
            {
                Shoot();
                ticksSinceLastShoot += Props.interval;
            }
            if (Find.TickManager.TicksGame == ticksSinceLastHurt && Props.hurtNearbyPawn)
            {
                Hurt();
                ticksSinceLastHurt += Props.hurtInterval;
            }
            if (Find.TickManager.TicksGame == ticksSinceLastCatch)
            {
                Catch();
                ticksSinceLastCatch += Props.catchInterval;
            }
        }
        protected void Shoot()
        {
            if (parent is Projectile projectile)
            {
                foreach (Thing item in GenRadial.RadialDistinctThingsAround(projectile.Position, projectile.Map, Props.radius, useCenter: true))
                {
                    if (!(item is Pawn pawn) || (item is Building building && !building.HostileTo(Faction.OfPlayer)))
                    {
                        continue;
                    }
                    if (pawn.HostileTo(Faction.OfPlayer))
                    {
                        Projectile projectile2 = (Projectile)GenSpawn.Spawn(Props.projectileDefs.RandomElement(), projectile.Position, projectile.Map);
                        projectile2.Launch(projectile, pawn, pawn, ProjectileHitFlags.IntendedTarget);
                        pawnCount++;
                        if(projectile.Launcher is Pawn shooter)
                        {
                            Hediff hediff = shooter.health.hediffSet.GetFirstHediffOfDef(Props.hediffBonus);
                            damageBonus += Math.Min(Mathf.FloorToInt(hediff.Severity),25);
                        }
                        if (pawnCount > (5 + targetBonus))
                        {
                            break;
                        }
                    }
                }
                pawnCount = 0;
            }
        }
        protected void Hurt()
        {
            if(parent is Projectile projectile)
            {
                foreach(Thing item in GenRadial.RadialDistinctThingsAround(projectile.Position,projectile.Map,Props.hurtRadius, useCenter: true))
                {
                    if(!(item is Pawn pawn))
                    {
                        continue;
                    }
                    if(pawn.HostileTo(projectile.Faction) && Props.hurtEnemyOnly && pawn != projectile.Launcher)
                    {
                        if (projectile.Launcher is Pawn shooter)
                        {
                            targetBonus += Math.Min(Mathf.FloorToInt(shooter.health.hediffSet.GetFirstHediffOfDef(Props.hediffBonus).Severity), 5);
                        }
                        DamageInfo dinfo = new DamageInfo(projectile.def.projectile.damageDef, Props.damageAmount, Props.armorPen);
                        pawn.TakeDamage(new DamageInfo(dinfo));
                    }
                    else if(!Props.hurtEnemyOnly && pawn != projectile.Launcher)
                    {
                        if (projectile.Launcher is Pawn shooter)
                        {
                            targetBonus += Math.Min(Mathf.FloorToInt(shooter.health.hediffSet.GetFirstHediffOfDef(Props.hediffBonus).Severity), 5);
                        }
                        DamageInfo dinfo = new DamageInfo(projectile.def.projectile.damageDef, Props.damageAmount, Props.armorPen);
                        pawn.TakeDamage(new DamageInfo(dinfo));
                    }
                }
                foreach (Thing item in GenRadial.RadialDistinctThingsAround(projectile.Position, projectile.Map, Props.pullRadius, useCenter: true))
                {
                    if (item is Pawn pawn)
                    {
                        if (Props.pullPawn && (pawn.HostileTo(projectile.Faction) && !pawn.Faction.IsPlayer) && !pawn.Downed)
                        {
                            if (Props.makeGoToJob)
                            {
                                Job job2 = JobMaker.MakeJob(JobDefOf.AttackStatic, projectile);
                                Job job3 = JobMaker.MakeJob(JobDefOf.GotoMindControlled, projectile);
                                if (pawn.equipment.Primary != null && pawn.equipment.Primary.def.IsMeleeWeapon && pawn.jobs?.curJob.targetA != projectile)
                                {
                                    pawn.jobs.StopAll();
                                    pawn.TryStartAttack(projectile);
                                }
                                if (pawn.equipment.Primary != null && pawn.equipment.Primary.def.IsRangedWeapon && pawn.jobs?.curJob.targetA != projectile)
                                {
                                    pawn.jobs.StopAll();
                                    pawn.jobs.StartJob(job2, JobCondition.InterruptForced);
                                }
                                else if(pawn.equipment.Primary == null && pawn.jobs?.curJob.targetA != projectile)
                                {
                                    pawn.jobs.StopAll();
                                    pawn.jobs.StartJob(job3, JobCondition.InterruptForced);
                                }
                            }
                            if (Props.makeFlyer)
                            {
                                IntVec3 intVec = projectile.DrawPos.ToIntVec3();
                                PawnFlyer_Pulled pawnFlyer_Pulled = (PawnFlyer_Pulled)PawnFlyer.MakeFlyer(MakaiTechPsy_DefOf.MakaiPsy_PullSlow, pawn, intVec,null,null);
                                pawnFlyer_Pulled.def.pawnFlyer.flightSpeed = Props.pullSpeed;
                                GenSpawn.Spawn(pawnFlyer_Pulled, intVec, projectile.Map);
                            }
                        }
                    }
                    else
                    {
                        continue;
                    }
                    
                }
            }
        }
        protected void Catch()
        {
            if(parent is Projectile projectile)
            {
                if(!Props.catchAndLaunchBackBullet)
                {
                    return;
                }
                foreach (Thing item in GenRadial.RadialDistinctThingsAround(projectile.Position, projectile.Map, Props.catchRadius, useCenter: true))
                {
                    if (item is Projectile projectileEnemy && projectileEnemy.def != projectile.def && projectileEnemy.Launcher.Faction != projectile.Launcher.Faction && projectileEnemy.def != ThingDefOf.Spark && projectileEnemy.def != ThingDefOf.Fire)
                    {
                        if(!Props.shootAtRandom)
                        {
                            IntVec3 location = MakaiUtility.RandomCellAround(projectile, 1);
                            Projectile projectile2 = (Projectile)GenSpawn.Spawn(projectileEnemy.def, projectile.Position.RandomAdjacentCell8Way(), projectile.Map);
                            projectile2.Launch(projectile, projectileEnemy.Launcher, projectileEnemy.Launcher, ProjectileHitFlags.IntendedTarget);
                            Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_WarpBullet.Spawn(projectileEnemy.Position, projectile.Map, 1);
                            effect.Cleanup();
                            projectileEnemy.Destroy();
                        }
                        if(Props.shootAtRandom)
                        {
                            IntVec3 locationSpread = MakaiUtility.RandomCellAround(projectile, 10);
                            Projectile projectile2 = (Projectile)GenSpawn.Spawn(projectileEnemy.def, projectile.Position, projectile.Map);
                            projectile2.Launch(projectile, locationSpread, locationSpread, ProjectileHitFlags.IntendedTarget);
                            projectileEnemy.Destroy();
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
}
