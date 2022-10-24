using RimWorld;
using UnityEngine;
using Verse;

namespace MakaiTechPsycast
{
    public class PawnFlyer_Pulled : PawnFlyer
	{
		protected Vector3 effectivePos;

		private int positionLastComputedTick;

		public override Vector3 DrawPos
		{
			get
			{
				RecomputePosition();
				return effectivePos;
			}
		}

		protected override void RespawnPawn()
		{
			base.RespawnPawn();
		}

		public override void ExposeData()
		{
			base.ExposeData();
		}

		public override void Tick()
		{
			base.Tick();
			if (base.FlyingPawn != null)
			{
				RecomputePosition();
			}
		}

		protected bool CheckRecompute()
		{
			if (positionLastComputedTick == ticksFlying)
			{
				return false;
			}
			positionLastComputedTick = ticksFlying;
			return true;
		}

		protected virtual void RecomputePosition()
		{
			if (!CheckRecompute())
			{
				effectivePos = Vector3.Lerp(startVec, base.DestinationPos, (float)ticksFlying / (float)ticksFlightTime);
			}
		}

		public override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.FlyingPawn.DrawAt(drawLoc, flip);
		}
	}
}
