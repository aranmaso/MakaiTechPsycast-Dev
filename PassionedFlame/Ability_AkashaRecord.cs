using RimWorld;
using RimWorld.Planet;
using Verse;
using VFECore;
using VanillaPsycastsExpanded;
using UnityEngine;

namespace MakaiTechPsycast.PassionedFlame
{
    public class Ability_AkashaRecord : VFECore.Abilities.Ability
    {
        private AbilityExtension_Roll1D20 modExtension => def.GetModExtension<AbilityExtension_Roll1D20>();
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            RollInfo rollinfo = new RollInfo();
            rollinfo = MakaiUtility.Roll1D20PassionCount(pawn, rollinfo);
            if (rollinfo.roll >= modExtension.successThreshold && rollinfo.roll < modExtension.greatSuccessThreshold)
            {
                int count = 0;
                foreach (var item in pawn.skills.skills)
                {
                    if(!item.TotallyDisabled)
                    {
                        if(item.passion != Passion.None)
                        {
                            count++;
                        }
                    }
                }
                Hediff hediff = MakaiUtility.CreateCustomHediffWithDuration(pawn,modExtension.hediffDefWhenSuccess,modExtension.hours,modExtension.ticks);
                hediff.Severity = Mathf.Max(1, count);
                pawn.health.AddHediff(hediff);

                Messages.Message("Makai_PassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                Messages.Message("Makai_PassArollcheckAkashaRecord".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
            }
            if (rollinfo.roll >= modExtension.greatSuccessThreshold)
            {
                int count = 0;
                foreach (var item in pawn.skills.skills)
                {
                    if (!item.TotallyDisabled)
                    {
                        if (item.passion != Passion.None)
                        {
                            count++;
                        }
                    }
                }
                count *= 2;
                Hediff hediff = MakaiUtility.CreateCustomHediffWithDuration(pawn, modExtension.hediffDefWhenSuccess, modExtension.hours, modExtension.ticks);
                hediff.Severity = Mathf.Max(1, count);
                pawn.health.AddHediff(hediff);
                Messages.Message("Makai_GreatPassArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
                Messages.Message("Makai_GreatPassArollcheckAkashaRecord".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.PositiveEvent);
            }
            if (rollinfo.roll < modExtension.successThreshold)
            {
                int count = 0;
                foreach (var item in pawn.skills.skills)
                {
                    if (!item.TotallyDisabled)
                    {
                        if (item.passion != Passion.None)
                        {
                            count++;
                        }
                    }
                }
                count /= 2;
                Hediff hediff = MakaiUtility.CreateCustomHediffWithDuration(pawn, modExtension.hediffDefWhenSuccess, modExtension.hours, modExtension.ticks);
                hediff.Severity = Mathf.Max(1, count);
                pawn.health.AddHediff(hediff);
                Messages.Message("Makai_FailArollcheck".Translate(pawn.LabelShort, rollinfo.baseRoll, rollinfo.cumulativeBonusRoll, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
                Messages.Message("Makai_FailArollcheckAkashaRecord".Translate(pawn.LabelShort, pawn.Named("USER")), pawn, MessageTypeDefOf.NegativeEvent);
            }
        }
    }
}
