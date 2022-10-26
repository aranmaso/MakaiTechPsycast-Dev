using VanillaPsycastsExpanded;
using Verse;
using RimWorld;
using VFECore.Abilities;

namespace MakaiTechPsycast.BondIntertwined
{
	public class Ability_BondLink : VFECore.Abilities.Ability
    {
		public override Hediff ApplyHediff(Pawn targetPawn, HediffDef hediffDef, BodyPartRecord bodyPart, int duration, float severity)
		{
			Hediff_BondLink Hediff_BondLink = base.ApplyHediff(targetPawn, hediffDef, bodyPart, duration, severity) as Hediff_BondLink;
			Hediff_BondLink.LinkAllPawnsAround();
			return Hediff_BondLink;
		}
	}
}
