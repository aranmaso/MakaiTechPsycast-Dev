using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using VFECore;

namespace MakaiTechPsycast.TrueDestruction
{
	public class CompProperties_TeleportTower : CompProperties
	{
		public float radius;

		public List<StatDef> stats;

		public int tickRate = 500;

		public HediffDef conduct;

		public DamageDef damageDef;

		public ThingDef projectile;

		public string uiIcon;

		public bool canToggleLaserBeam;
		public CompProperties_TeleportTower()
		{
			compClass = typeof(CompTeleportTower);
		}
	}
}