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
			oldLord = Pawn.GetLord();
			oldLord?.RemovePawn(Pawn);
			Pawn.SetFaction(Faction.OfPlayer);
		}

		public override void CompPostPostRemoved()
		{
			base.CompPostPostRemoved();
			Pawn.Map.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrike(Pawn.Map, Pawn.Position));
			GenExplosion.DoExplosion(Pawn.Position, Pawn.Map, 2f, DamageDefOf.EMP, null, 10);
			Pawn.Kill(new DamageInfo(DamageDefOf.ExecutionCut,13));
		}
	}
}
