using RimWorld;
using RimWorld.Planet;
using Verse;
using VFECore;
using VanillaPsycastsExpanded;
using UnityEngine;
using System.Collections.Generic;

namespace MakaiTechPsycast.StringOfFate
{
    public class Ability_Precognition : VFECore.Abilities.Ability
    {
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            AbilityExtension_Roll1D20 modExtension = def.GetModExtension<AbilityExtension_Roll1D20>();
            RollInfo rollinfo = new RollInfo();
            rollinfo = MakaiUtility.Roll1D20(pawn, modExtension.skillBonus, rollinfo);
            if (rollinfo.roll >= modExtension.successThreshold && rollinfo.roll < modExtension.greatSuccessThreshold)
            {                
                string incident = null;
                if(Find.Storyteller.incidentQueue != null)
                {
                    incident = null;
                    foreach (QueuedIncident item in Find.Storyteller.incidentQueue)
                    {
                        incident += " " + item.FiringIncident.def.label;
                    }
                }
                else
                {
                    incident = "No Foreseenable event queued";
                }
                Letter future = LetterMaker.MakeLetter("Precognition",incident,LetterDefOf.NeutralEvent);
                Find.LetterStack.ReceiveLetter(future);
                Messages.Message(incident, MessageTypeDefOf.NeutralEvent);
                Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                Messages.Message("Makai_PassArollcheckPrecognition".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
            }
            if (rollinfo.roll >= modExtension.greatSuccessThreshold)
            {
                string incident = null;
                if (Find.Storyteller.incidentQueue != null)
                {
                    incident = null;
                    foreach (QueuedIncident item in Find.Storyteller.incidentQueue)
                    {
                        incident += " " + item.FiringIncident.def.label;
                    }
                }
                else
                {
                    incident = "No Foreseenable event queued";
                }
                Letter future = LetterMaker.MakeLetter("Precognition", incident, LetterDefOf.NeutralEvent);
                Find.LetterStack.ReceiveLetter(future);
                Messages.Message(incident, MessageTypeDefOf.NeutralEvent);
                Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                Messages.Message("Makai_GreatPassArollcheckPrecognition".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
            }
            if (rollinfo.roll < modExtension.successThreshold)
            {
                string incident = null;
                if (Find.Storyteller.incidentQueue != null)
                {
                    incident = null;
                    foreach (QueuedIncident item in Find.Storyteller.incidentQueue)
                    {
                        incident += " " + item.FiringIncident.def.label;
                    }
                }
                else
                {
                    incident = "No Foreseenable event queued";
                }
                Letter future = LetterMaker.MakeLetter("Precognition", incident, LetterDefOf.NeutralEvent);
                Find.LetterStack.ReceiveLetter(future);
                Messages.Message(incident, MessageTypeDefOf.NeutralEvent);
                Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
                Messages.Message("Makai_FailArollcheckPrecognition".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
            }
        }
    }
}
