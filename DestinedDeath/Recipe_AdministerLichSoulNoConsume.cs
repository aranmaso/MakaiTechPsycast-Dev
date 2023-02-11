using System.Collections.Generic;
using RimWorld;
using Verse;

namespace MakaiTechPsycast
{
	public class Recipe_AdministerLichSoulNoConsume : Recipe_Surgery
	{
		public override bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
		{
			if (!(thing is Pawn pawn))
			{
				return false;
			}
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].def == MakaiTechPsy_DefOf.MakaiTechPsy_DD_LichSoul && hediffs[i].Visible)
				{
					return true;
				}
			}
			return false;
		}

		/*public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			List<Hediff> allHediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < allHediffs.Count; i++)
			{
				if (allHediffs[i].Part != null && allHediffs[i].def == MakaiTechPsy_DefOf.MakaiTechPsy_DD_LichSoul && allHediffs[i].Visible)
				{
					yield return allHediffs[i].Part;
				}
			}
		}*/

		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			/*bool flag = false;
			string ownerName = null;
			if (!(ingredients[0] is Soul soul))
			{
				return;
			}
			if (soul.ownerName == pawn.Name.ToStringFull)
			{
				flag = true;
				ownerName = soul.ownerName;
			}			
			if (!flag)
			{
				Messages.Message("Incorrect Soul", MessageTypeDefOf.NegativeEvent);
			}*/
			Hediff hediff = pawn.health.hediffSet.hediffs.Find((Hediff x) => x.def == MakaiTechPsy_DefOf.MakaiTechPsy_DD_LichSoul && x.Part == part && x.Visible);
			if (hediff != null)
			{
				ingredients[0].TryGetComp<CompUsable>().UsedBy(pawn);
			}
		}
        public override void ConsumeIngredient(Thing ingredient, RecipeDef recipe, Map map)
        {           
        }
    }
}
