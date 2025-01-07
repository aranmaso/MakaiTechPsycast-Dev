using System.Collections.Generic;
using RimWorld;
using Verse;

namespace MakaiTechPsycast.DestinedDeath
{
    public class HediffComp_LichSoul : HediffComp
    {
		public bool resurrectedRecently = false;

		public HediffCompProperties_LichSoul Props => (HediffCompProperties_LichSoul)props;

		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			if (base.Pawn.IsHashIntervalTick(120))
			{
				resurrectedRecently = false;
			}
		}

		public override void Notify_PawnKilled()
		{
			base.Notify_PawnKilled();
		}

		public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
        {
			Map map = parent.pawn.Corpse.Map;
			if (map != null)
			{
				MoteMaker.ThrowText(parent.pawn.Corpse.Position.ToVector3(), map, "Revived");
				ResurrectionUtility.TryResurrect(parent.pawn.Corpse.InnerPawn);
				AmplifierSeverity();
			}
		}

		public void AmplifierSeverity()
		{
			if (resurrectedRecently)
			{
				return;
			}
			parent.pawn.drafter.Drafted = true;
			parent.pawn.Map.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrikeGreen(parent.pawn.Map,parent.pawn.Position));			
			foreach (Apparel item2 in base.Pawn.apparel.WornApparel)
			{
				if (item2.HitPoints != item2.MaxHitPoints)
				{
					item2.HitPoints = item2.MaxHitPoints;
				}
			}
			resurrectedRecently = true;
		}
	}
}
