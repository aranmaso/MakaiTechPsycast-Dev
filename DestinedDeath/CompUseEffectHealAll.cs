using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace MakaiTechPsycast
{
    public class CompUseEffectHealAll : CompUseEffect
    {
		public override void DoEffect(Pawn user)
		{
			base.DoEffect(user);
			if(parent is Soul soul)
            {
				if (soul.originalPawn == user)
				{
					List<Hediff> list = user.health.hediffSet.hediffs.Where(MakaiUtility.FindBadHediff).ToList();
					Hediff bloodloss = user.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BloodLoss);
					if(bloodloss != null)
                    {
						user.health.RemoveHediff(bloodloss);
                    }
					foreach (Hediff item in list)
					{
						user.health.RemoveHediff(item);
					}
					Messages.Message("MakaiPsyDDHealAll".Translate(user.LabelShort, user.Named("USER")), user, MessageTypeDefOf.PositiveEvent);
				}
				else if (soul.ownerName != user.Name.ToStringFull)
				{
					Messages.Message("MakaiPsyDDHealAllIncorrect".Translate(user.LabelShort, user.Named("USER")), user, MessageTypeDefOf.NegativeEvent);
				}
			}
		}
	}
}
