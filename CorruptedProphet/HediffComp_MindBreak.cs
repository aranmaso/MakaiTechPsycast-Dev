using System.Linq;
using RimWorld;
using Verse;
using Verse.AI.Group;

namespace MakaiTechPsycast.CorruptedProphet
{
    public class HediffComp_MindBreak : HediffComp
    {
		private Lord oldLord;
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			base.CompPostPostAdd(dinfo);
			oldLord = base.Pawn.GetLord();
			oldLord?.RemovePawn(base.Pawn);
			base.Pawn.SetFaction(Faction.OfPlayer);
		}

		public override void CompPostPostRemoved()
		{
			base.CompPostPostRemoved();
			base.Pawn.Map.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrike(base.Pawn.Map, base.Pawn.Position));
			GenExplosion.DoExplosion(base.Pawn.Position, base.Pawn.Map, 2f, DamageDefOf.EMP, null, 10);
			base.Pawn.Kill(new DamageInfo(DamageDefOf.ExecutionCut,13));
		}
	}
}
