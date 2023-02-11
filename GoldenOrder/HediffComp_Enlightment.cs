using Verse;
using RimWorld;

namespace MakaiTechPsycast.GoldenOrder
{
    public class HediffComp_Enlightment : HediffComp
    {
		public HediffCompProperties_Enlightment Props => (HediffCompProperties_Enlightment)props;

		public float costPerTrigger => Props.costPerTrigger;
		public bool isNarakaExist => Pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiTechPsy_GD_PathOfNaraka);

		public HediffComp_PathOfNaraka comps => Pawn?.health?.hediffSet?.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_GD_PathOfNaraka)?.TryGetComp<HediffComp_PathOfNaraka>();

		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			if(!isNarakaExist)
            {
				Pawn.health.RemoveHediff(parent);
            }
			if (Pawn.InMentalState && comps.currentStack >= costPerTrigger)
			{
				Pawn.MentalState.RecoverFromState();
				comps.currentStack -= costPerTrigger;
			}
		}
	}
}
