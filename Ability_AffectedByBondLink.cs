using System.Linq;
using RimWorld.Planet;
using VanillaPsycastsExpanded;
using Verse;
using VFECore.Abilities;

namespace MakaiTechPsycast.BondIntertwined
{
	public class Ability_AffectedByBondLink : Ability
	{
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			AbilityExtension_ApplyToPawnWithHediff modExtension = def.GetModExtension<AbilityExtension_ApplyToPawnWithHediff>();
			if (!(pawn.health.hediffSet.GetFirstHediffOfDef(modExtension.hediff) is Hediff_BondLink Hediff_BondLink))
			{
				return;
			}
			foreach (Pawn linkedPawn in Hediff_BondLink.linkedPawns)
			{
				if (!targets.Any((GlobalTargetInfo x) => x.Thing == linkedPawn))
				{
					base.Cast(new GlobalTargetInfo[1] { linkedPawn });
				}
			}
		}
	}
}
