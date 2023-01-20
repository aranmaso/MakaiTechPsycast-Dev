using Verse;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using HarmonyLib;
using Verse.Sound;

namespace MakaiTechPsycast
{
    public class CompProximityBurst : ThingComp
    {
        private static readonly AccessTools.FieldRef<Projectile, int> ticksToImpact = AccessTools.FieldRefAccess<Projectile, int>("ticksToImpact");
        public CompProperties_ProximityBurst Props => (CompProperties_ProximityBurst)props;

        private IEnumerable<ThingDef> shrapnel;
        private int shrapnelCount;
        private float range;
        private float burstRange;
        private int interval;
        private bool hasBursted = false;
        private int timer;
        private int distanceToProx;
        public override void PostPostMake()
        {
            base.PostPostMake();
            shrapnel = Props.shrapnelBulletDef;
            shrapnelCount = Props.shrapnelCount;
            range = Props.range;
            burstRange = Props.burstRange;
            if(Props.proximityOnNearestFoe)
            {
                interval = Props.checkInterval;
            }            
            if(Props.proximityOnTimer)
            {
                timer = Props.proximityTimer*60;
            }
            if(Props.proximityOnTargetDistance)
            {
                distanceToProx = Props.distanceFromTargetToProx;
            }
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            if(!(parent is Projectile))
            {
                Scribe_Values.Look(ref interval, "interval", Props.checkInterval + Find.TickManager.TicksGame);
            }
        }
        public override void CompTick()
        {
            base.CompTick();
            if (hasBursted)
            {
                return;
            }
            if(Props.proximityOnNearestFoe || Props.proximityOnIntendedTarget)
            {
                if (interval > 0)
                {
                    interval--;
                    return;
                }
                CheckForPawnNearby();
                //MakaiUtility.ThrowFleck(MakaiTechPsy_DefOf.MakaiPsyMote_ReflectProjectile, parent.DrawPos, parent.Map, range);
                interval = Props.checkInterval;
            }
            if(Props.proximityOnTargetDistance)
            {
                CheckForDistance();
            }
            if(Props.proximityOnTimer)
            {
                if(timer > 0)
                {
                    timer--;
                }
                else
                {
                    BurstTimer();
                }
            }
            /*if (interval > 0)
            {
                interval--;
                return;
            }            
            CheckForPawnNearby();
            //MakaiUtility.ThrowFleck(MakaiTechPsy_DefOf.MakaiPsyMote_ReflectProjectile, parent.DrawPos, parent.Map, range);
            interval = Props.checkInterval;*/
        }
        public void CheckForDistance()
        {
            if(parent is Projectile projectile)
            {
                int ToImpact = ticksToImpact(projectile);
                /*if (projectile.Position.DistanceToSquared(projectile.usedTarget.Cell) <= distanceToProx * distanceToProx)
                {
                    BurstNowFromDistance(projectile.usedTarget.Cell);
                }  */
                if (ToImpact <= distanceToProx)
                {
                    BurstNowFromDistance(projectile.usedTarget.Cell);
                    SoundDefOf.PsycastPsychicEffect.PlayOneShot(new TargetInfo(parent.Position, parent.Map));
                }                
            }
        }
        public void CheckForPawnNearby()
        {
            if(parent is Projectile projectile)
            {
                foreach (Pawn item in MakaiUtility.GetNearbyPawnFoeOnly(projectile.Position, projectile.Launcher.Faction, projectile.Launcher.Map, range))
                {
                    if (item.Downed || item.Dead || item.Faction == projectile.Launcher.Faction)
                    {
                        continue;
                    }
                    BurstNow(item);
                    SoundDefOf.PsycastPsychicEffect.PlayOneShot(new TargetInfo(parent.Position, parent.Map));
                    return;
                }
            }
            else
            {
                foreach (Pawn item in MakaiUtility.GetNearbyPawnFoeOnly(parent.Position, parent.Faction, parent.Map, range))
                {
                    if (item.Downed || item.Dead || item.Faction == parent.Faction)
                    {
                        continue;
                    }
                    BurstNow(item);
                    SoundDefOf.PsycastPsychicEffect.PlayOneShot(new TargetInfo(parent.Position, parent.Map));
                    return;
                }
            }
            
        }
        public void BurstTimer()
        {
            if (parent is Projectile projectile)
            {
                SoundDefOf.PsycastPsychicEffect.PlayOneShot(new TargetInfo(parent.Position, parent.Map));
                List<IntVec3> possibleTargetCell = new List<IntVec3>();
                IntVec3 target = projectile.Position;
                for (int i = 0; i < shrapnelCount; i++)
                {
                    possibleTargetCell.Add(MakaiUtility.RandomCellAround(parent, Mathf.FloorToInt(burstRange)));
                }
                for (int i = 0; i < possibleTargetCell.Count; i++)
                {
                    Projectile shrapnelThing = (Projectile)GenSpawn.Spawn(shrapnel.RandomElement(), projectile.Position, projectile.Launcher.Map);
                    Thing possibleThing = possibleTargetCell[i].GetFirstPawn(projectile.Launcher.Map);
                    if (possibleThing != null)
                    {
                        shrapnelThing.Launch(projectile.Launcher, possibleThing, possibleThing, ProjectileHitFlags.IntendedTarget);
                    }
                    else
                    {
                        shrapnelThing.Launch(projectile.Launcher, possibleTargetCell[i], possibleTargetCell[i], ProjectileHitFlags.NonTargetPawns);
                    }
                }
                hasBursted = true;
                projectile.def = shrapnel.RandomElement();
                projectile.Launch(projectile.Launcher, target.RandomAdjacentCell8Way(), target.RandomAdjacentCell8Way(), ProjectileHitFlags.IntendedTarget);
            }
        }
        public void BurstNowFromDistance(IntVec3 targetCell)
        {
            if (parent is Projectile projectile)
            {
                List<IntVec3> possibleTargetCell = new List<IntVec3>();
                IntVec3 targetOriginal = projectile.Position;
                IntVec3 target = targetOriginal;
                target.x -= (targetOriginal.x - targetCell.x);
                target.z -= (targetOriginal.z - targetCell.z);
                for (int i = 0; i < shrapnelCount; i++)
                {
                    possibleTargetCell.Add(MakaiUtility.RandomCellAround(parent, Mathf.FloorToInt(burstRange)));                    
                }
                for (int i = 0; i < possibleTargetCell.Count; i++)
                {
                    IntVec3 offsetCell = possibleTargetCell[i];
                    offsetCell.x -= targetOriginal.x - targetCell.x;
                    offsetCell.z -= targetOriginal.z - targetCell.z;
                    Projectile shrapnelThing = (Projectile)GenSpawn.Spawn(shrapnel.RandomElement(), projectile.Position, projectile.Launcher.Map);
                    Thing possibleThing = possibleTargetCell[i].GetFirstPawn(projectile.Launcher.Map);
                    Thing possibleBuilding = possibleTargetCell[i].GetFirstBuilding(projectile.Launcher.Map);
                    if (possibleThing != null)
                    {
                        shrapnelThing.Launch(projectile, possibleThing, possibleThing, ProjectileHitFlags.IntendedTarget);
                    }
                    else if(possibleBuilding != null)
                    {
                        shrapnelThing.Launch(projectile, possibleBuilding, possibleBuilding, ProjectileHitFlags.IntendedTarget);
                    }
                    else
                    {
                        shrapnelThing.Launch(projectile, offsetCell, offsetCell, ProjectileHitFlags.NonTargetPawns);
                    }
                }
                hasBursted = true;
                projectile.def = shrapnel.RandomElement();
                if(projectile.usedTarget.Pawn != null)
                {
                    projectile.Launch(projectile.Launcher, projectile.usedTarget.Pawn, projectile.usedTarget.Pawn, ProjectileHitFlags.IntendedTarget);
                }
                else
                {
                    projectile.Launch(projectile.Launcher, target, target, ProjectileHitFlags.IntendedTarget);
                }                
            }
        }
        public void BurstNow(Pawn proxTarget)
        {                     
            if(parent is Projectile projectile)
            {
                List<IntVec3> possibleTargetCell = new List<IntVec3>();
                for(int i = 0; i < shrapnelCount; i++)
                {
                    possibleTargetCell.Add(MakaiUtility.RandomCellAround(proxTarget, Mathf.FloorToInt(burstRange)));
                }
                for (int i = 0; i < possibleTargetCell.Count; i++)
                {                    
                    Projectile shrapnelThing = (Projectile)GenSpawn.Spawn(shrapnel.RandomElement(), projectile.Position, projectile.Launcher.Map);
                    Thing possibleThing = possibleTargetCell[i].GetFirstPawn(projectile.Launcher.Map);
                    Thing possibleBuilding = possibleTargetCell[i].GetFirstBuilding(projectile.Launcher.Map);
                    if (possibleThing != null)
                    {
                        shrapnelThing.Launch(projectile, possibleThing, possibleThing, ProjectileHitFlags.IntendedTarget);
                    }
                    else if (possibleBuilding != null)
                    {
                        shrapnelThing.Launch(projectile, possibleBuilding, possibleBuilding, ProjectileHitFlags.IntendedTarget);
                    }
                    else
                    {
                        shrapnelThing.Launch(projectile, possibleTargetCell[i], possibleTargetCell[i], ProjectileHitFlags.NonTargetPawns);
                    }                   
                }
                hasBursted = true;
                projectile.def = shrapnel.RandomElement();
                projectile.Launch(projectile.Launcher,proxTarget,proxTarget,ProjectileHitFlags.IntendedTarget);
            }
            else
            {
                List<IntVec3> possibleTargetCell = new List<IntVec3>();
                for (int i = 0; i < shrapnelCount; i++)
                {
                    possibleTargetCell.Add(MakaiUtility.RandomCellAround(proxTarget, Mathf.FloorToInt(burstRange)));
                }
                for (int i = 0; i < possibleTargetCell.Count; i++)
                {
                    Projectile shrapnelThing = (Projectile)GenSpawn.Spawn(shrapnel.RandomElement(), parent.Position, parent.Map);
                    Thing possibleThing = possibleTargetCell[i].GetFirstPawn(parent.Map);
                    Thing possibleBuilding = possibleTargetCell[i].GetFirstBuilding(parent.Map);
                    if (possibleThing != null)
                    {
                        shrapnelThing.Launch(parent, possibleThing, possibleThing, ProjectileHitFlags.IntendedTarget);
                    }
                    else if (possibleBuilding != null)
                    {
                        shrapnelThing.Launch(parent, possibleBuilding, possibleBuilding, ProjectileHitFlags.IntendedTarget);
                    }
                    else
                    {
                        shrapnelThing.Launch(parent, possibleTargetCell[i], possibleTargetCell[i], ProjectileHitFlags.NonTargetPawns);
                    }
                }
                parent.Destroy();
            }
        }
    }
}
