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
			if (req.HasThing && req.Thing is Pawn pawn && pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul).TryGetComp<HediffComp_SoulCollection>() is HediffComp_SoulCollection soulConsume && soulConsume.SoulCount > 0)
			{
				val += soulConsume.BonustToStat / 100;
			}
		}

		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && req.Thing is Pawn pawn && pawn.health.hediffSet.GetFirstHediffOfDef(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul).TryGetComp<HediffComp_SoulCollection>() is HediffComp_SoulCollection soulConsume && soulConsume.SoulCount > 0)
			{
				return "Soul Consumed + " + soulConsume.BonustToStat;
			}
			return null;
		}
	}
}
