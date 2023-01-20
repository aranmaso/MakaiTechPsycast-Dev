using UnityEngine;
using VanillaPsycastsExpanded;
using Verse;
using Verse.AI;

namespace MakaiTechPsycast.TrueDestruction
{
    public class Hediff_ChainLock : Hediff_Overlay
    {
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			IntVec3 facingCell = pawn.Rotation.FacingCell;
			int ticksToDisappear = this.TryGetComp<HediffComp_Disappears>().ticksToDisappear;
			pawn.jobs.StopAll();
			Job job = JobMaker.MakeJob(VPE_DefOf.VPE_StandFreeze);
			job.expiryInterval = ticksToDisappear;
			job.overrideFacing = pawn.Rotation;
			pawn.jobs.StartJob(job);
			pawn.pather.StopDead();
			pawn.stances.SetStance(new Stance_Stand(ticksToDisappear, facingCell, null));
		}
	}
}
