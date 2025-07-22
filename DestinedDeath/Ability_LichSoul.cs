using RimWorld;
using RimWorld.Planet;
using Verse;
using VEF;
using VanillaPsycastsExpanded;
using UnityEngine;
using System.Linq;

namespace MakaiTechPsycast.DestinedDeath
{
	public class Ability_LichSoul : VEF.Abilities.Ability
	{
		AbilityExtension_Roll1D20 modExtension => def.GetModExtension<AbilityExtension_Roll1D20>();
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
        {
            if (!pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul))
            {
                Messages.Message("Not Enough soul strength", MessageTypeDefOf.NeutralEvent, false);
                return false;
            }
            if (MakaiUtility.GetFirstHediffOfDef(pawn, MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul).TryGetComp<HediffComp_SoulCollection>().SoulCount < 50)
            {
                Messages.Message("Not Enough soul strength", MessageTypeDefOf.NeutralEvent, false);
                return false;
            }
            return true;
        }
        public override void Cast(params GlobalTargetInfo[] targets)
		{
			if(!pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul)
				|| MakaiUtility.GetFirstHediffOfDef(pawn, MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul).TryGetComp<HediffComp_SoulCollection>().SoulCount < 50)
            {
				Messages.Message("Not Enough soul strength", MessageTypeDefOf.NeutralEvent, false);
			}
			else
            {
				base.Cast(targets);
				RollInfo rollinfo = new RollInfo();
				rollinfo = MakaiUtility.Roll1D20(pawn, modExtension.skillBonus, rollinfo);
				if (!pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiTechPsy_DD_LichSoul))
				{
					MakaiUtility.GetFirstHediffOfDef(pawn, MakaiTechPsy_DefOf.MakaiTechPsy_DD_CollectSoul).TryGetComp<HediffComp_SoulCollection>().SoulCount -= 50;
					if (rollinfo.roll >= modExtension.successThreshold && rollinfo.roll < modExtension.greatSuccessThreshold)
					{
						Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenSuccess, pawn);
						pawn.health.AddHediff(hediff);
						Thing thing = ThingMaker.MakeThing(MakaiTechPsy_DefOf.MakaiTechPsy_DD_Soul);
						if (thing is Soul soul2)
						{
							soul2 = MakaiUtility.GetPawnCopy(soul2, pawn);
						}
						GenPlace.TryPlaceThing(thing, pawn.RandomAdjacentCell8Way(), pawn.Map, ThingPlaceMode.Near);
						Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
						Messages.Message("Makai_PassArollcheckLichSoul".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
					}
					if (rollinfo.roll >= modExtension.greatSuccessThreshold)
					{

						Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenGreatSuccess, pawn);
						pawn.health.AddHediff(hediff);
						Thing thing = ThingMaker.MakeThing(MakaiTechPsy_DefOf.MakaiTechPsy_DD_Soul);
						if (thing is Soul soul2)
						{
							soul2 = MakaiUtility.GetPawnCopy(soul2, pawn);
						}
						GenPlace.TryPlaceThing(thing, pawn.RandomAdjacentCell8Way(), pawn.Map, ThingPlaceMode.Near);
						System.Collections.Generic.List<Hediff> list1 = pawn.health.hediffSet.hediffs.Where(MakaiUtility.FindBadHediff).ToList();
						foreach (Hediff item in list1)
						{
							pawn.health.RemoveHediff(item);
						}
						Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
						Messages.Message("Makai_GreatPassArollcheckLichSoul".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
					}
					if (rollinfo.roll < modExtension.successThreshold)
					{

						Hediff hediff = HediffMaker.MakeHediff(modExtension.hediffDefWhenFail, pawn);
						Hediff hediff2 = HediffMaker.MakeHediff(MakaiTechPsy_DefOf.MakaiPsy_PK_Brain_Mulfunction, pawn);
						pawn.health.AddHediff(hediff);
						pawn.health.AddHediff(hediff2);
						Thing thing = ThingMaker.MakeThing(MakaiTechPsy_DefOf.MakaiTechPsy_DD_Soul);
						if (thing is Soul soul2)
						{
							soul2 = MakaiUtility.GetPawnCopy(soul2, pawn);
						}
						GenPlace.TryPlaceThing(thing, pawn.RandomAdjacentCell8Way(), pawn.Map, ThingPlaceMode.Near);
						Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
						Messages.Message("Makai_FailArollcheckLichSoul".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
					}
				}
				else if (pawn.health.hediffSet.HasHediff(MakaiTechPsy_DefOf.MakaiTechPsy_DD_LichSoul))
				{
					Messages.Message("LichSoulAlreadyRemove".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
				}
			}			
		}
	}
}
