using System.Collections.Generic;
using RimWorld;
using Verse;

namespace MakaiTechPsycast
{
	public class Recipe_AdministerLichSoul : Recipe_Surgery
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

		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			List<Hediff> allHediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < allHediffs.Count; i++)
			{
				if (allHediffs[i].Part != null && allHediffs[i].def == MakaiTechPsy_DefOf.MakaiTechPsy_DD_LichSoul && allHediffs[i].Visible)
				{
					yield return allHediffs[i].Part;
				}
			}
		}

		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			bool flag = false;
			string ownerName = null;
			if(!(ingredients[0] is Soul soul))
            {
				return;
            }
			if(soul.ownerName == pawn.Name.ToStringFull)
            {
				flag = true;
				ownerName = soul.ownerName;
            }
			if (billDoer != null)
			{
				/*if (CheckSurgeryFail(billDoer, pawn, ingredients, part, bill) && !flag)
				{
					return;
				}*/
				TaleRecorder.RecordTale(TaleDefOf.DidSurgery, billDoer, pawn);
				if ((PawnUtility.ShouldSendNotificationAbout(pawn) || PawnUtility.ShouldSendNotificationAbout(billDoer)) && flag)
				{
					string text = (recipe.successfullyRemovedHediffMessage.NullOrEmpty() ? ((string)"MessageSuccessfullyRemovedHediff".Translate(billDoer.LabelShort, pawn.LabelShort, recipe.removesHediff.label.Named("HEDIFF"), billDoer.Named("SURGEON"), pawn.Named("PATIENT"))) : ((string)recipe.successfullyRemovedHediffMessage.Formatted(billDoer.LabelShort, pawn.LabelShort)));
					Messages.Message(text, pawn, MessageTypeDefOf.PositiveEvent);
				}
			}
			if (!flag)
			{
				Messages.Message("Incorrect Soul", MessageTypeDefOf.NegativeEvent);
			}
			Hediff hediff = pawn.health.hediffSet.hediffs.Find((Hediff x) => x.def == MakaiTechPsy_DefOf.MakaiTechPsy_DD_LichSoul && x.Part == part && x.Visible);
			if (hediff != null && flag)
			{
				Thing thing = ThingMaker.MakeThing(MakaiTechPsy_DefOf.MakaiTechPsy_DD_Soul);
				if (thing is Soul soul2)
				{
					MakaiUtility.GetPawnCopy(soul2,pawn);
					//soul2.ownerName = soul.ownerName;
				}
				GenPlace.TryPlaceThing(thing, billDoer.Position, billDoer.Map, ThingPlaceMode.Near);
				soul.TryGetComp<CompUsable>().UsedBy(pawn);
				return;
			}
		}
	}
}
