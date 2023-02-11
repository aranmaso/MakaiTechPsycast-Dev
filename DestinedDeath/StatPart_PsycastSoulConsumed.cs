using RimWorld;
using UnityEngine;
using VanillaPsycastsExpanded;
using Verse;

namespace MakaiTechPsycast.DestinedDeath
{
	public class StatPart_PsycastSoulConsumed : StatPart
	{
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && req.Thing is Pawn pawn)
			{
				Hediff hediff = MakaiUtility.GetFirstHediffOfDef(pawn, MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul);
				HediffComp_SoulCollection soulConsumed = hediff.TryGetComp<HediffComp_SoulCollection>();
				if (hediff != null && soulConsumed.SoulCount > 0)
                {
					val += soulConsumed.BonustToStat / 100;
				}				
			}
		}

		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && req.Thing is Pawn pawn)
			{
				Hediff hediff = MakaiUtility.GetFirstHediffOfDef(pawn, MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul);
				HediffComp_SoulCollection soulConsumed = hediff.TryGetComp<HediffComp_SoulCollection>();
				if (hediff != null && soulConsumed.SoulCount > 0)
				{
					return "Soul Consumed + " + soulConsumed.BonustToStat;
				}				
			}
			return null;
		}
	}
}
