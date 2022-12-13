using System;
using System.Linq;
using UnityEngine;
using RimWorld;
using Verse;
using VFECore;
using VanillaPsycastsExpanded;
using System.Collections.Generic;

namespace MakaiTechPsycast.TrueDestruction
{

	public class CompLightningTower : ThingComp
	{
		public CompProperties_LightningTower Props => (CompProperties_LightningTower)props;

		public bool isToggledOn = false;

		public bool isAttackDowned = false;

		public bool laserBeamToggle;

		public bool attackDowned;

		public bool attackDownedToggle = true;
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (Props.canToggleLaserBeam)
			{
				Command_Toggle command_ToggleLink = new Command_Toggle();
				if (isToggledOn)
				{
					command_ToggleLink.defaultLabel = "PowerBeam Enabled";
					command_ToggleLink.defaultDesc = "PowerBeam Enabled";
				}
				else
				{
					command_ToggleLink.defaultLabel = "PowerBeam Disabled";
					command_ToggleLink.defaultDesc = "PowerBeam Disabled";
				}
				command_ToggleLink.hotKey = KeyBindingDefOf.Command_ItemForbid;
				command_ToggleLink.icon = ContentFinder<Texture2D>.Get(Props.uiIcon);
				command_ToggleLink.isActive = () => isToggledOn;
				command_ToggleLink.toggleAction = delegate
				{
					isToggledOn = !isToggledOn;
					if (isToggledOn)
					{
						laserBeamToggle = true;
					}
					else
					{
						laserBeamToggle = false;
					}
				};
				yield return command_ToggleLink;
			}
			if (attackDownedToggle == true)
			{
				Command_Toggle command_ToggleLink = new Command_Toggle();
				if (isAttackDowned)
				{
					command_ToggleLink.defaultLabel = "Attack downed pawn enabled";
					command_ToggleLink.defaultDesc = "will target downed pawn";
				}
				else
				{
					command_ToggleLink.defaultLabel = "Attack downed pawn disabled";
					command_ToggleLink.defaultDesc = "will not target downed pawn";
				}
				command_ToggleLink.hotKey = KeyBindingDefOf.Command_ItemForbid;
				command_ToggleLink.icon = ContentFinder<Texture2D>.Get(Props.uiIcon);
				command_ToggleLink.isActive = () => isAttackDowned;
				command_ToggleLink.toggleAction = delegate
				{
					isAttackDowned = !isAttackDowned;
					if (isAttackDowned)
					{
						attackDowned = true;
					}
					else
					{
						attackDowned = false;
					}
				};
				yield return command_ToggleLink;
			}
			if (Prefs.DevMode)
			{
				/*Command_Action command_Action = new Command_Action();
				command_Action.defaultLabel = "Debug: reflect now";
				command_Action.action = delegate
				{
					ReflectNow();
				};
				yield return command_Action;*/

				Command_Action command_Action2 = new Command_Action();
				command_Action2.defaultLabel = "Debug: strike now";
				command_Action2.action = delegate
				{
					Strike();
				};
				yield return command_Action2;
			}
		}
		private int nextTest = 0;

		private int pawnCount = 0;

		public override void PostExposeData()
		{
			Scribe_Values.Look(ref nextTest, "nextTest", 0);
			base.PostExposeData();
		}

		public override void PostPostMake()
		{
			nextTest = Find.TickManager.TicksGame + Props.tickRate;
			base.PostPostMake();
		}

		public override void CompTick()
		{
			base.CompTick();
			if (Find.TickManager.TicksGame != nextTest)
			{
				return;
			}
			Strike();
			pawnCount = 0;
			nextTest += Props.tickRate;
		}

		public void ReflectNow()
        {
			int reflectCount = 0;
			foreach (Thing item in GenRadialCached.RadialDistinctThingsAround(parent.Position, parent.Map, 25f, useCenter: true))
			{
				if (item is Projectile projectileEnemy && projectileEnemy.def != parent.def && projectileEnemy.Launcher != parent && projectileEnemy.Launcher.Faction != parent.Faction && projectileEnemy.def != ThingDefOf.Spark && projectileEnemy.def != ThingDefOf.Fire)
				{
					IntVec3 location = MakaiUtility.RandomCellAround(parent, 1);
					//Projectile projectile2 = (Projectile)GenSpawn.Spawn(projectileEnemy.def, projectile.Position.RandomAdjacentCell8Way(), projectile.Map);
					//projectile2.Launch(projectile, projectileEnemy.Launcher, projectileEnemy.Launcher, ProjectileHitFlags.IntendedTarget);
					MakaiUtility.ThrowFleck(MakaiTechPsy_DefOf.MakaiPsyMote_ReflectProjectile, projectileEnemy.Position.ToVector3(), parent.Map, 1f);
					//Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_WarpBullet.Spawn(projectileEnemy.Position, projectile.Map, 1);
					//effect.Cleanup();
					projectileEnemy.Launch(parent, projectileEnemy.Launcher, projectileEnemy.Launcher, ProjectileHitFlags.IntendedTarget);
					reflectCount++;
				}
				if (reflectCount >= 6)
				{
					break;
				}
			}
		}
		private void Strike()
        {
			foreach (Pawn pawn in MakaiUtility.GetNearbyPawnFoeOnly(parent.Position, parent.Faction, parent.Map, Props.radius))
			{
				/*parent.TryGetQuality(out var qc);
				if (qc == QualityCategory.Normal)
                {
					
                }*/
				if (!pawn.Downed && !attackDowned && pawn != null)
				{
					if (laserBeamToggle)
					{
						MakaiTD_PowerBeam orbitalStrike = (MakaiTD_PowerBeam)GenSpawn.Spawn(Props.projectile, pawn.Position, pawn.Map);
						orbitalStrike.duration = 60;
						orbitalStrike.instigator = pawn;
						orbitalStrike.StartStrike();
						Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_TD_Blast.Spawn(pawn.Position, pawn.Map, 0.5f);
						effect.Cleanup();
					}
					else
					{
						GenExplosion.DoExplosion(pawn.Position, pawn.Map, 2f, DamageDefOf.EMP, null, 1, 2);
					}
					parent.Map.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrikeGreen(pawn.Map, pawn.Position));

					if (pawn.health.hediffSet.HasHediff(Props.conduct ?? VPE_DefOf.VPE_UnLucky))
						for (int i = 0; i < 2; i++)
						{
							MakaiTD_PowerBeam orbitalStrike = (MakaiTD_PowerBeam)GenSpawn.Spawn(Props.projectile, pawn.Position, pawn.Map);
							orbitalStrike.duration = 60;
							orbitalStrike.instigator = pawn;
							orbitalStrike.StartStrike();
							Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_TD_Blast.Spawn(pawn.Position, pawn.Map, 0.5f);
							effect.Cleanup();
						}
				}
				if (attackDowned && pawn != null)
				{
					if (laserBeamToggle)
					{
						MakaiTD_PowerBeam orbitalStrike = (MakaiTD_PowerBeam)GenSpawn.Spawn(Props.projectile, pawn.Position, pawn.Map);
						orbitalStrike.duration = 60;
						orbitalStrike.instigator = pawn;
						orbitalStrike.StartStrike();
						Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_TD_Blast.Spawn(pawn.Position, pawn.Map, 0.5f);
						effect.Cleanup();
					}
					else
					{
						GenExplosion.DoExplosion(pawn.Position, pawn.Map, 2f, DamageDefOf.EMP, null, 1, 2);
					}
					parent.Map.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrikeGreen(pawn.Map, pawn.Position));

					if (pawn.HostileTo(Faction.OfPlayer) && pawn.health.hediffSet.HasHediff(Props.conduct ?? VPE_DefOf.VPE_UnLucky))
						for (int i = 0; i < 2; i++)
						{
							MakaiTD_PowerBeam orbitalStrike = (MakaiTD_PowerBeam)GenSpawn.Spawn(Props.projectile, pawn.Position, pawn.Map);
							orbitalStrike.duration = 60;
							orbitalStrike.instigator = pawn;
							orbitalStrike.StartStrike();
							Effecter effect = MakaiTechPsy_DefOf.MakaiPsy_TD_Blast.Spawn(pawn.Position, pawn.Map, 0.5f);
							effect.Cleanup();
						}
				}
				/*IntVec3 intVect1 = parent.Position;
                for (int i = 0; i < parent.Position.DistanceTo(pawn.Position); i++)
                {
                    if(intVect1.x > pawn.Position.x)
                    {
                        intVect1.x--;
                    }
                    else
                    {
                        intVect1.x++;
                    }
                    if (intVect1.z > pawn.Position.z)
                    {
                        intVect1.z--;
                    }
                    else
                    {
                        intVect1.z++;
                    }
                    GenExplosion.DoExplosion(intVect1,parent.Map,0f,DamageDefOf.Bomb,parent,1,1);
                }*/
				pawnCount++;
				if (pawnCount >= (Props.targetCount))
				{
					break;
				}
			}
		}		
	}
}
