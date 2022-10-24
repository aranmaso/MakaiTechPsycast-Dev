using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace MakaiTechPsycast.CorruptedProphet
{
	public class MakaiPsyCompProperties_OrbitalBeam : CompProperties
	{
		public float width = 8f;

		public float offsetX = 1f;
		public float offsetZ = 1f;

		public SoundDef hitSound;

		public Color color = Color.white;

		public SoundDef sound;

		public MakaiPsyCompProperties_OrbitalBeam()
		{
			compClass = typeof(MakaiPsy_CP_CompOrbitalBeam);
		}

		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string item in base.ConfigErrors(parentDef))
			{
				yield return item;
			}
			if (parentDef.drawerType != DrawerType.RealtimeOnly && parentDef.drawerType != DrawerType.MapMeshAndRealTime)
			{
				yield return "orbital beam requires realtime drawer";
			}
		}
	}
}
