﻿using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace MakaiTechPsycast
{
	public class Recipe_RestoreSoul : Recipe_Surgery
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
				if ((!recipe.targetsBodyPart || hediffs[i].Part != null) && hediffs[i].def == recipe.removesHediff && hediffs[i].Visible)
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
				if (allHediffs[i].def == recipe.removesHediff && allHediffs[i].Visible)
				{
					yield return allHediffs[i].Part;
				}
			}
		}

		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			bool flag = false;			
			foreach (Thing ingredient in ingredients)
			{
				if (ingredient is Soul soul)
				{
					if (soul.ownerName == pawn.Name.ToStringFull)
					{
						flag = true;
					}
				}
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
				if (ingredients[0] is Soul soul)
				{
					MakaiUtility.applySoulData(soul,pawn);
					Messages.Message("Soul transferred to a new body", MessageTypeDefOf.NegativeEvent);
				}
				Hediff hediff = pawn.health.hediffSet.hediffs.Find((Hediff x) => x.def == recipe.removesHediff && x.Part == part && x.Visible);
				if (hediff != null)
				{
					pawn.health.RemoveHediff(hediff);
				}
				/*Thing thing = ThingMaker.MakeThing(MakaiTechPsy_DefOf.MakaiTechPsy_DD_Soul);
				if (thing is Soul soul2)
				{
					soul2.ownerName = ownerName;
				}
				GenPlace.TryPlaceThing(thing, billDoer.Position, billDoer.Map, ThingPlaceMode.Near);*/
			}
			else if(flag)
            {
				Hediff hediff = pawn.health.hediffSet.hediffs.Find((Hediff x) => x.def == recipe.removesHediff && x.Part == part && x.Visible);
				if (hediff != null)
				{
					pawn.health.RemoveHediff(hediff);
				}
			}
		}
	}
}
