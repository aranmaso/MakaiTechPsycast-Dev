using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace MakaiTechPsycast
{
	[StaticConstructorOnStartup]
	public class WeatherEvent_LightningStrikeGreen : WeatherEvent_LightningFlash
	{
		private IntVec3 strikeLoc = IntVec3.Invalid;

		private Mesh boltMesh;

		private static readonly Material LightningMat = MaterialPool.MatFrom("Weather/g_lightningBeam", ShaderDatabase.MoteGlow);

		public WeatherEvent_LightningStrikeGreen(Map map)
			: base(map)
		{
		}

		public WeatherEvent_LightningStrikeGreen(Map map, IntVec3 forcedStrikeLoc)
			: base(map)
		{
			strikeLoc = forcedStrikeLoc;
		}

		public override void FireEvent()
		{
			base.FireEvent();
			if (!strikeLoc.IsValid)
			{
				strikeLoc = CellFinderLoose.RandomCellWith((IntVec3 sq) => sq.Standable(map) && !map.roofGrid.Roofed(sq), map);
			}
			boltMesh = LightningBoltMeshPool.RandomBoltMesh;
			if (!strikeLoc.Fogged(map))
			{
				GenExplosion.DoExplosion(strikeLoc, map, 1.9f, DamageDefOf.Flame, null);
				Vector3 loc = strikeLoc.ToVector3Shifted();
				for (int i = 0; i < 4; i++)
				{
					FleckMaker.ThrowSmoke(loc, map, 1.5f);
					FleckMaker.ThrowMicroSparks(loc, map);
					FleckMaker.ThrowLightningGlow(loc, map, 1.5f);
				}
			}
			SoundInfo info = SoundInfo.InMap(new TargetInfo(strikeLoc, map));
			SoundDefOf.Thunder_OnMap.PlayOneShot(info);
		}

		public override void WeatherEventDraw()
		{
			Graphics.DrawMesh(boltMesh, strikeLoc.ToVector3ShiftedWithAltitude(AltitudeLayer.Weather), Quaternion.identity, FadedMaterialPool.FadedVersionOf(LightningMat, base.LightningBrightness), 0);
		}
	}
}
