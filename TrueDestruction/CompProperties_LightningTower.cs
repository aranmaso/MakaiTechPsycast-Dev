using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using VFECore;

namespace MakaiTechPsycast.TrueDestruction
{
	public class CompProperties_LightningTower : CompProperties
	{
		public float radius;

		public List<StatDef> stats;

		public int tickRate = 500;

		public int targetCount = 6;

		public HediffDef conduct;

		public DamageDef damageDef;

		public ThingDef projectile;

		public string uiIcon;

		public bool canToggleLaserBeam;

		public bool toggleFireMode;
		public CompProperties_LightningTower()
		{
			compClass = typeof(CompLightningTower);
		}
	}
}