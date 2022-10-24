﻿using UnityEngine;
using Verse;
using Verse.Sound;

namespace MakaiTechPsycast.CorruptedProphet
{
	[StaticConstructorOnStartup]
	public class MakaiPsy_CP_CompOrbitalBeam : ThingComp
	{
		private int startTick;

		private int totalDuration;

		private int fadeOutDuration;

		private float angle;

		private Sustainer sustainer;

		private const float AlphaAnimationSpeed = 0.3f;

		private const float AlphaAnimationStrength = 0.025f;

		private const float BeamEndHeightRatio = 0.5f;

		private static readonly Material BeamMat = MaterialPool.MatFrom("Projectiles/CorruptedProphet/Hand", ShaderDatabase.Cutout, MapMaterialRenderQueues.OrbitalBeam);
		private static readonly Material BeamMat2 = MaterialPool.MatFrom("Projectiles/CorruptedProphet/Hand2", ShaderDatabase.Cutout, MapMaterialRenderQueues.OrbitalBeam);

		private static readonly Material BeamEndMat = MaterialPool.MatFrom("Things/Mote/mote_invisible", ShaderDatabase.MoteGlow, MapMaterialRenderQueues.OrbitalBeam);

		private static readonly MaterialPropertyBlock MatPropertyBlock = new MaterialPropertyBlock();

		private float shaking;

		private Material beam1 = BeamMat;

		public MakaiPsyCompProperties_OrbitalBeam Props => (MakaiPsyCompProperties_OrbitalBeam)props;

		private int TicksPassed => Find.TickManager.TicksGame - startTick;

		private int TicksLeft => totalDuration - TicksPassed;

		private float BeamEndHeight => Props.width * 0.5f;

		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look(ref startTick, "startTick", 0);
			Scribe_Values.Look(ref totalDuration, "totalDuration", 0);
			Scribe_Values.Look(ref fadeOutDuration, "fadeOutDuration", 0);
			Scribe_Values.Look(ref angle, "angle", 0f);
		}

		public void StartAnimation(int totalDuration, int fadeOutDuration, float angle)
		{
			startTick = Find.TickManager.TicksGame;
			this.totalDuration = totalDuration;
			this.fadeOutDuration = fadeOutDuration;
			this.angle = angle;
			CheckSpawnSustainer();
		}

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			CheckSpawnSustainer();
		}

		public override void CompTick()
		{
			base.CompTick();
			if (sustainer != null)
			{
				sustainer.Maintain();
				shaking = Rand.Value;
				if (TicksLeft < fadeOutDuration)
				{
					sustainer.End();
					sustainer = null;
					shaking = 0f;
				}
			}
		}
		public override void PostDraw()
		{
			base.PostDraw();
			if (TicksLeft > 0)
			{
				Vector3 drawPos = parent.DrawPos;
				float num = ((float)parent.Map.Size.z - drawPos.z);
				Vector3 vector = Vector3Utility.FromAngleFlat(angle - 90f);
				Vector3 vector2 = drawPos + vector * num * 0.4f;
				vector2.y = AltitudeLayer.MetaOverlays.AltitudeFor();
				float num2 = Mathf.Min((float)TicksPassed / 50f, 1f);
				Vector3 vector3 = vector * ((1f - num2) * num);
				float num3 = 0.975f + Mathf.Sin((float)TicksPassed * 0.3f) * 0.025f;
				if (TicksLeft < fadeOutDuration)
				{
					num3 *= (float)TicksLeft / (float)fadeOutDuration;
				}
				Color color = Props.color;
				color.a *= num3;
				MatPropertyBlock.SetColor(ShaderPropertyIDs.Color, color);
				Vector3 pos1 = drawPos;
				Vector3 mainpos1 = pos1;
				mainpos1.z += Props.offsetZ;
				pos1.z += Props.offsetZ *5f;
				int frame = TicksPassed;
				pos1.z -= frame / 1;
				if (pos1.z == mainpos1.z)
                {
					Props.hitSound.PlayOneShot(new TargetInfo(parent.Position,parent.Map));
				}
				if (pos1.z <= (mainpos1.z))
                {
					pos1.z = mainpos1.z;
                }
				pos1.y = AltitudeLayer.MetaOverlays.AltitudeFor();
				Matrix4x4 matrix = default(Matrix4x4);
				matrix.SetTRS(pos1, Quaternion.Euler(0f, 0f, 0f), new Vector3((Props.width * 1.25f) + shaking * 2.5f, 1f, Props.width*3f));
				Graphics.DrawMesh(MeshPool.plane10, matrix, beam1, 0, null, 0, MatPropertyBlock);
				Vector3 pos = drawPos + vector3;
				pos.y = AltitudeLayer.MetaOverlays.AltitudeFor();
				pos.z -= 2f;
				Matrix4x4 matrix2 = default(Matrix4x4);
				matrix2.SetTRS(pos, Quaternion.Euler(0f, 0f, 0f), new Vector3(Props.width, 1f, BeamEndHeight));
				Graphics.DrawMesh(MeshPool.plane10, matrix2, BeamEndMat, 0, null, 0, MatPropertyBlock);
			}
		}

		private void CheckSpawnSustainer()
		{
			if (TicksLeft >= fadeOutDuration && Props.sound != null)
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					sustainer = Props.sound.TrySpawnSustainer(SoundInfo.InMap(parent, MaintenanceType.PerTick));
				});
			}
		}
	}
}
