using RimWorld;
using Verse;

namespace MakaiTechPsycast.DestinedDeath
{
	public class StatPart_SoulWorth : StatPart
	{
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && req.Thing is Soul soul)
			{
				val += soul.pawnMarketValue;
			}
		}

		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && req.Thing is Soul soul)
			{
				return "Soul Worth + " + soul.pawnMarketValue;
			}
			return null;
		}
	}
}
