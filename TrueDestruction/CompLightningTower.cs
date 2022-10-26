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
			foreach (Thing item in GenRadial.RadialDistinctThingsAround(parent.Position, parent.Map, Props.radius, useCenter: true))
			{
				if (!(item is Pawn pawn))
				{
					continue;
				}
				/*parent.TryGetQuality(out var qc);
				if (qc == QualityCategory.Normal)
                {
					
                }*/
				float ValidTarget = Rand.Value;
				if (ValidTarget <= 1f)
                {
					if (pawn.HostileTo(parent.Faction) && !pawn.Downed && attackDowned == false && pawn != null)
					{
						float damRand = Rand.Value;
						if (damRand <= 0.5f && laserBeamToggle == true)
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
							GenExplosion.DoExplosion(pawn.Position, pawn.Map, 2f, DamageDefOf.EMP, null);
						}
						pawn.Map.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrikeGreen(pawn.Map, pawn.Position));

						if (pawn.HostileTo(parent.Faction) && pawn.health.hediffSet.HasHediff(Props.conduct ?? VPE_DefOf.VPE_UnLucky))
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
					if ((pawn.HostileTo(parent.Faction)) && (attackDowned == true) && pawn != null)
					{
						float damRand = Rand.Value;
						if (damRand <= 0.5f && laserBeamToggle == true)
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
							GenExplosion.DoExplosion(pawn.Position, pawn.Map, 2f, DamageDefOf.EMP, null);
						}
						pawn.Map.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrikeGreen(pawn.Map, pawn.Position));

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
					pawnCount++;
					if (pawnCount >= (Props.targetCount+1))
					{
						break;
					}
				}
			}
			pawnCount = 0;
			nextTest += Props.tickRate;
		}
	}
}
