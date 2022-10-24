using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VanillaPsycastsExpanded;
using Verse;

namespace MakaiTechPsycast.BondIntertwined
{
	public class Hediff_BondLink : Hediff_Overlay
	{
		public List<Pawn> linkedPawns = new List<Pawn>();

		public override string OverlayPath => "Other/ForceField";

		public virtual Color OverlayColor => new Color(0.37f, 0.34f, 0.64f, 0.5f);

		public override float OverlaySize => ability.GetRadiusForPawn();

		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			LinkAllPawnsAround();
		}

		public void LinkAllPawnsAround()
		{
			foreach (Pawn item in from x in GenRadial.RadialDistinctThingsAround(pawn.Position, pawn.Map, ability.GetRadiusForPawn(), useCenter: true).OfType<Pawn>()
								  where x.RaceProps.Humanlike && x != pawn
								  select x)
			{
				if (!linkedPawns.Contains(item))
				{
					linkedPawns.Add(item);
				}
			}
		}

		private void UnlinkAll()
		{
			for (int num = linkedPawns.Count - 1; num >= 0; num--)
			{
				linkedPawns.RemoveAt(num);
			}
		}

		public override void PostRemoved()
		{
			base.PostRemoved();
			UnlinkAll();
		}

		public override void Tick()
		{
			base.Tick();
			for (int num = linkedPawns.Count - 1; num >= 0; num--)
			{
				Pawn pawn = linkedPawns[num];
				if (pawn.Map != base.pawn.Map || pawn.Position.DistanceTo(base.pawn.Position) > ability.GetRadiusForPawn())
				{
					linkedPawns.RemoveAt(num);
				}
			}
			if (!linkedPawns.Any())
			{
				base.pawn.health.RemoveHediff(this);
			}
		}

		public override void Draw()
		{
			Vector3 drawPos = pawn.DrawPos;
			drawPos.y = AltitudeLayer.MoteOverhead.AltitudeFor();
			Color overlayColor = OverlayColor;
			MatPropertyBlock.SetColor(ShaderPropertyIDs.Color, overlayColor);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(drawPos, Quaternion.identity, new Vector3(OverlaySize * 2f * 1.1601562f, 1f, OverlaySize * 2f * 1.1601562f));
			Graphics.DrawMesh(MeshPool.plane10, matrix, base.OverlayMat, 0, null, 0, MatPropertyBlock);
			foreach (Pawn linkedPawn in linkedPawns)
			{
				GenDraw.DrawLineBetween(linkedPawn.DrawPos, pawn.DrawPos, SimpleColor.Yellow);
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look(ref linkedPawns, "linkedPawns", LookMode.Reference);
		}
	}
}
